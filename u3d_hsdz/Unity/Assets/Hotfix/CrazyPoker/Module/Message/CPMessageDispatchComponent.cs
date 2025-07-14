using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
	[ObjectSystem]
	public class CPMessageDispatherComponentAwakeSystem : AwakeSystem<CPMessageDispatherComponent>
	{
		public override void Awake(CPMessageDispatherComponent self)
		{
			self.Awake();
		}
	}

	/// <summary>
	/// 消息分发组件
	/// </summary>
	public class CPMessageDispatherComponent : Component
	{
		public static CPMessageDispatherComponent Instance;

		public delegate void OnActinHandler(ICPResponse response);

		private readonly Dictionary<ushort, List<OnActinHandler>> handlers = new Dictionary<ushort, List<OnActinHandler>>();

		public void Awake()
		{
			Instance = this;
		}

		public void RegisterHandler(ushort opcode, OnActinHandler handler)
		{
			if (!this.handlers.ContainsKey(opcode))
			{
				this.handlers.Add(opcode, new List<OnActinHandler>());
			}
			this.handlers[opcode].Add(handler);
		}

		public void RemoveHandler(ushort opcode, OnActinHandler handler)
		{
			List<OnActinHandler> list = null;
			if (!this.handlers.TryGetValue(opcode, out list))
				return;

			for (int i = 0, n = list.Count; i < n; i++)
			{
				if (handler.Equals(list[i]))
				{
					list.RemoveAt(i);
					if (list.Count == 0)
						this.handlers.Remove(opcode);
					break;
				}
			}
		}

		public void Handle(ushort opcode, ICPResponse response)
		{
			List<OnActinHandler> actions;
			if (!this.handlers.TryGetValue(opcode, out actions))
			{
				// Log.Error($"消息 {opcode} 没有处理");
				return;
			}

			for (int i = 0; i < actions.Count; i++)
			{
				try
				{
					actions[i](response);
				}
				catch (Exception e)
				{
					Log.Error(e);
				}
			}

			// foreach (OnActinHandler ev in actions)
			// {
			// 	try
			// 	{
			// 		ev(response);
			// 	}
			// 	catch (Exception e)
			// 	{
			// 		Log.Error(e);
			// 	}
			// }
		}

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();
		}
	}
}