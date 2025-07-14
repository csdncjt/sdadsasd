using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ETModel;
using Microsoft.IO;

namespace ETHotfix
{
	[ObjectSystem]
	public class SessionAwakeSystem : AwakeSystem<Session, ETModel.Session>
	{
		public override void Awake(Session self, ETModel.Session session)
		{
			self.session = session;
			SessionCallbackComponent sessionComponent = self.session.AddComponent<SessionCallbackComponent>();
			sessionComponent.MessageCallback = (s, flag, opcode, memoryStream) => { self.Run(s, flag, opcode, memoryStream); };
			sessionComponent.DisposeCallback = s => { self.Dispose(); };
		}
	}

	/// <summary>
	/// 用来收发热更层的消息
	/// </summary>
	public class Session: Entity
	{
		public ETModel.Session session;

		private static int RpcId { get; set; }
		private readonly Dictionary<int, Action<ICPResponse>> requestCallback = new Dictionary<int, Action<ICPResponse>>();	// key 为opcode

		public const int HEAD_SIZE = 90;
		private readonly List<byte[]> packetheadbyteses = new List<byte[]>()
		{
				new byte[1]{68}, // "D"
				new byte[1]{90}, // "Z"
				new byte[1]{80}, // "P"
				new byte[1]{75}, // "K"
				new byte[1], // protrol version
				new byte[4], // userID
				new byte[1], // 语言 1 zh 2 en
				new byte[1], // 系统 1 android 2 ios
				new byte[1], // Build版本号
				new byte[2], // 渠道号
				new byte[2], // 产品ID
				new byte[2], // 协议号
				new byte[2], // 包大小(包头+包体)
				new byte[66],// token
				new byte[4], // 预留 用作rpcId
		};

		private readonly MemoryStream memoryStream = new RecyclableMemoryStreamManager().GetStream("hotfixmessage", ushort.MaxValue);

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			
			base.Dispose();

			foreach (Action<ICPResponse> action in this.requestCallback.Values.ToArray())
			{
				action.Invoke(new CPResponseMessage { Error = this.session.Error });
			}

            if (session.Error != 0)
            {
                UIComponent.Instance.HideNoAnimation(UIType.UIPrompt);
                Game.EventSystem.Run(EventIdType.LoginError, session.Error);
            }
            Log.Debug($"{session.RemoteAddress} Session.Error:{session.Error}");

			this.requestCallback.Clear();

			this.session.Dispose();
		}

		public void SetProtrolVersionInHead(byte version)
		{
			packetheadbyteses[4].WriteTo(0, version);
		}

		public void SetUserIdInHead(int userId)
		{
			packetheadbyteses[5].WriteTo(0, System.Net.IPAddress.HostToNetworkOrder(userId));
		}

		public void SetLanguageInHead(byte language)
		{
			packetheadbyteses[6].WriteTo(0, language);
		}

		public void SetPlatformInHead(byte platfform)
		{
			packetheadbyteses[7].WriteTo(0, platfform);
		}

		public void SetBuildVersionInHead(byte version)
		{
			packetheadbyteses[8].WriteTo(0, version);
		}

		public void SetChannelInHead(short channel)
		{
			packetheadbyteses[9].WriteTo(0, System.Net.IPAddress.HostToNetworkOrder(channel));
		}

		public void SetProductIdInHead(short productId)
		{
			packetheadbyteses[10].WriteTo(0, System.Net.IPAddress.HostToNetworkOrder(productId));
		}

		public void SetCmdInHead(ushort cmd)
		{
			packetheadbyteses[11].WriteTo(0, System.Net.IPAddress.HostToNetworkOrder((short)cmd));
		}

		public void SetPacketSizeInHead(ushort size)
		{
			packetheadbyteses[12].WriteTo(0, System.Net.IPAddress.HostToNetworkOrder((short)(HEAD_SIZE + size)));
		}

		public void SetTokenInHead(string token)
		{
			var bytes = token.ToBigEndianUtf16();
			int offset = 0;
			foreach (byte b in bytes)
			{
				packetheadbyteses[13].WriteTo(offset, b);
				offset++;
			}
		}

		public void SetRpcIdInHead(int rpcId)
		{
			packetheadbyteses[14].WriteTo(0, System.Net.IPAddress.HostToNetworkOrder(rpcId));
		}

		public void Run(ETModel.Session s, byte flag, ushort opcode, MemoryStream memoryStream)
		{
			// Log.Debug($"fix run opcode: {opcode}");
			OpcodeTypeComponent opcodeTypeComponent = Game.Scene.GetComponent<OpcodeTypeComponent>();
			object instance = opcodeTypeComponent.GetInstance(opcode);

			int rpcId = System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt32(memoryStream.GetBuffer(), 2));
			object message = CrazyPokerMessagePacker.DeserializeFrom(instance, memoryStream);

			if (ETHotfix.OpcodeHelper.IsNeedDebugLogMessage(opcode))
			{
				if (null != message)
					Log.Msg(message);
			}

			if (ETHotfix.OpcodeHelper.IsNeedShowJuhua(opcode))
			{
				Game.Scene.GetComponent<UIComponent>().Hide(UIType.UIPrompt, null, 0);
			}

			if (rpcId > 0)
			{
				ICPResponse response = message as ICPResponse;
				if (response == null)
				{
					throw new Exception($"flag is response, but hotfix message is not! {opcode}");
				}

				Action<ICPResponse> action;
				if (!this.requestCallback.TryGetValue(rpcId, out action))
				{
					return;
				}
				this.requestCallback.Remove(rpcId);
				action(response);
				return;
			}

			Game.Scene.GetComponent<CPMessageDispatherComponent>().Handle(opcode, message as ICPResponse);
		}

		public void Send(ICPMessage message)
		{
			Send(0x00, message);
		}

		public void Send(byte flag, ICPMessage message)
		{
			ushort opcode = Game.Scene.GetComponent<OpcodeTypeComponent>().GetOpcode(message.GetType());
			this.Send(flag, opcode, message);
		}

		public void Send(byte flag, ushort opcode, ICPMessage message)
		{
			if (ETHotfix.OpcodeHelper.IsNeedDebugLogMessage(opcode))
			{
                Log.Debug($"{session.RemoteAddress.Address}:{session.RemoteAddress.Port}");
				Log.Msg(message);
			}

			if (ETHotfix.OpcodeHelper.IsNeedShowJuhua(opcode))
			{
                UIComponent.Instance.Prompt();
            }

			MemoryStream stream = this.memoryStream;
			stream.Seek(HEAD_SIZE, SeekOrigin.Begin);
			stream.SetLength(HEAD_SIZE);
			ushort bodySize = CrazyPokerMessagePacker.SerializeTo(message, stream);
			stream.Seek(0, SeekOrigin.Begin);

			SetCmdInHead(opcode);
			SetPacketSizeInHead(bodySize);
			SetRpcIdInHead(RpcId);

			int index = 0;
			foreach (var bytes in this.packetheadbyteses)
			{
				Array.Copy(bytes, 0, stream.GetBuffer(), index, bytes.Length);
				index += bytes.Length;
			}

			// StringBuilder mStringBuilder = new StringBuilder();
			// var tmp = stream.GetBuffer();
			// for (int i = 0; i < HEAD_SIZE + bodySize; i++)
			// {
			// 	mStringBuilder.Append(tmp[i].ToHex());
			// 	mStringBuilder.Append(' ');
			// }
			// Log.Debug($"opcode: {opcode}, size: {HEAD_SIZE + bodySize}, bytes: {mStringBuilder}");

            // Log.Debug(stream.ToArray().ToHex());

            try
            {
                session.Send(stream);
            }
            catch (Exception e)
            {
                UIComponent.Instance.Toast(e.Message);
                UIComponent.Instance.ClosePrompt();
                throw;
            }
			
		}

		public Task<ICPResponse> Call(ICPRequest request)
		{
			int rpcId = ++RpcId;
			var tcs = new TaskCompletionSource<ICPResponse>();

			this.requestCallback[rpcId] = (response) =>
			{
				try
				{
					if (ErrorCode.IsRpcNeedThrowException(response.Error))
					{
						// throw new RpcException(response.Error, response.Message);
						throw new RpcException(response.Error, "");
					}

					tcs.SetResult(response);
				}
				catch (Exception e)
				{
					tcs.SetException(new Exception($"Rpc Error: {request.GetType().FullName}", e));
				}
			};

			request.RpcId = rpcId;
			
			this.Send(0x00, request);
			return tcs.Task;
		}

		public Task<ICPResponse> Call(ICPRequest request, CancellationToken cancellationToken)
		{
			int rpcId = ++RpcId;
			var tcs = new TaskCompletionSource<ICPResponse>();

			this.requestCallback[rpcId] = (response) =>
			{
				try
				{
					if (ErrorCode.IsRpcNeedThrowException(response.Error))
					{
						// throw new RpcException(response.Error, response.Message);
						throw new RpcException(response.Error, "");
					}

					tcs.SetResult(response);
				}
				catch (Exception e)
				{
					tcs.SetException(new Exception($"Rpc Error: {request.GetType().FullName}", e));
				}
			};

			cancellationToken.Register(() => { this.requestCallback.Remove(rpcId); });

			request.RpcId = rpcId;

			this.Send(0x00, request);
			return tcs.Task;
		}
	}
}
