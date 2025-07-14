using System;
using System.IO;

namespace ETModel
{
	public enum ParserState
	{
		PacketSize,
		PacketBody
	}
	
	public static class Packet
	{
		public const int PacketSizeLength2 = 2;
		public const int PacketSizeLength4 = 4;
		public const int FlagIndex = 0;
		public const int OpcodeIndex = 1;
		public const int MessageIndex = 3;
		public const int CPPacketSizeLength = 20;
	}

	public class PacketParser
	{
		private readonly CircularBuffer buffer;
		private int packetSize;
		private ushort packetOpcode;
		private int packetRpcId;
		private ParserState state;
		public MemoryStream memoryStream;
		private bool isOK;
		private readonly int packetSizeLength;

		public PacketParser(int packetSizeLength, CircularBuffer buffer, MemoryStream memoryStream)
		{
			this.packetSizeLength = packetSizeLength;
			this.buffer = buffer;
			this.memoryStream = memoryStream;
		}

		public bool Parse()
		{
			if (this.isOK)
			{
				return true;
			}

			bool mCache = false;

			bool finish = false;
			while (!finish)
			{
				switch (this.state)
				{
					case ParserState.PacketSize:
						if (this.buffer.Length < 20)
						{
							finish = true;
						}
						else
						{
							this.buffer.Read(this.memoryStream.GetBuffer(), 0, Packet.CPPacketSizeLength);
							
							this.packetOpcode = (ushort)System.Net.IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(this.memoryStream.GetBuffer(), 4));
							this.packetRpcId = System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt32(this.memoryStream.GetBuffer(), 9));

							switch (this.packetSizeLength)
							{
								case Packet.PacketSizeLength4:
									this.packetSize = System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt32(this.memoryStream.GetBuffer(), 7));
									if (this.packetSize > ushort.MaxValue * 16 || this.packetSize < Packet.CPPacketSizeLength)
									{
										throw new Exception($"recv packet size error: {this.packetSize}");
									}
									break;
								case Packet.PacketSizeLength2:
									this.packetSize = System.Net.IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(this.memoryStream.GetBuffer(), 7));

									if (this.packetSize > ushort.MaxValue || this.packetSize < Packet.CPPacketSizeLength)
									{
										throw new Exception($"recv packet size error: {this.packetSize}");
									}
									break;
								default:
									throw new Exception("packet size byte count must be 2 or 4!");
							}
						
							this.state = ParserState.PacketBody;
						}
						break;
					case ParserState.PacketBody:
						if (this.buffer.Length < this.packetSize - Packet.CPPacketSizeLength)
						{
							finish = true;
						}
						else
						{
							this.memoryStream.Seek(0, SeekOrigin.Begin);
							this.memoryStream.SetLength(this.packetSize - Packet.CPPacketSizeLength + 6);
							byte[] bytes = this.memoryStream.GetBuffer();
							ByteHelper.WriteTo(bytes, 0, System.Net.IPAddress.HostToNetworkOrder((short)this.packetOpcode));
							ByteHelper.WriteTo(bytes, 2, System.Net.IPAddress.HostToNetworkOrder(this.packetRpcId));
							this.buffer.Read(bytes, 6, this.packetSize - Packet.CPPacketSizeLength);
							this.isOK = true;
							this.state = ParserState.PacketSize;
							finish = true;
						}
						break;
				}
			}
			return this.isOK;
		}

		public MemoryStream GetPacket()
		{
			this.isOK = false;
			return this.memoryStream;
		}
	}
}