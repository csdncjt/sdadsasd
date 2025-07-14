using System;
using System.Collections.Generic;
using System.IO;
using ETModel;

namespace ETHotfix
{
	public class CPHotfixMessageHelper
	{
		public static int WriteField(byte fieldId, sbyte field, MemoryStream stream)
		{
			stream.WriteByte(fieldId);
			byte bfield = 0;
			if (field >= 0)
			{
				bfield = (byte)field;
			}
			else
			{
				bfield = (byte) (field + 256);
			}
			stream.WriteByte(bfield);
			return 2;
		}

		public static int WriteField(byte fieldId, short field, MemoryStream stream)
		{
			stream.WriteByte(fieldId);
			field = System.Net.IPAddress.HostToNetworkOrder(field);
			byte[] bytes = BitConverter.GetBytes(field);
			stream.Write(BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(bytes.Length)), 0, 2);
			stream.Write(bytes, 0, bytes.Length);
			return 5;
		}

		public static int WriteField(byte fieldId, ushort field, MemoryStream stream)
		{
			stream.WriteByte(fieldId);
			field = (ushort)System.Net.IPAddress.HostToNetworkOrder((short)field);
			byte[] bytes = BitConverter.GetBytes(field);
			stream.Write(BitConverter.GetBytes((ushort)System.Net.IPAddress.HostToNetworkOrder((short)bytes.Length)), 0, 2);
			stream.Write(bytes, 0, bytes.Length);
			return 5;
		}

		public static int WriteField(byte fieldId, int field, MemoryStream stream)
		{
			stream.WriteByte(fieldId);
			field = System.Net.IPAddress.HostToNetworkOrder(field);
			byte[] bytes = BitConverter.GetBytes(field);
			stream.Write(BitConverter.GetBytes((ushort)System.Net.IPAddress.HostToNetworkOrder((short)bytes.Length)), 0, 2);
			stream.Write(bytes, 0, bytes.Length);
			return 7;
		}

		public static int WriteField(byte fieldId, string field, MemoryStream stream)
		{
			int size = 3;
			stream.WriteByte(fieldId);
			byte[] bytes = field.ToBigEndianUtf16();
			stream.Write(BitConverter.GetBytes((ushort)System.Net.IPAddress.HostToNetworkOrder((short)bytes.Length)), 0, 2);
			size += bytes.Length;
			stream.Write(bytes, 0, bytes.Length);
			return size;
		}

		public static int WriteField(byte fieldId, List<sbyte> field, MemoryStream stream)
		{
			int size = 1;
			stream.WriteByte(fieldId);
			stream.Write(BitConverter.GetBytes((ushort)System.Net.IPAddress.HostToNetworkOrder((short)field.Count)), 0, 2);
			size += 2;
			for (int i = 0, n = field.Count; i < n; i++)
			{
				byte bfield = 0;
				if (field[i] >= 0)
				{
					bfield = (byte)field[i];
				}
				else
				{
					bfield = (byte)(field[i] + 256);
				}
				stream.WriteByte(bfield);
				size++;
			}
			return size;
		}

		public static int WriteField(byte fieldId, List<int> field, MemoryStream stream)
		{
			int size = 1;
			stream.WriteByte(fieldId);
			stream.Write(BitConverter.GetBytes((ushort)System.Net.IPAddress.HostToNetworkOrder((short)(field.Count * 4))), 0, 2);
			size += 2;
			for (int i = 0, n = field.Count; i < n; i++)
			{
                stream.Write(BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(field[i])), 0, 4);
				size += 4;
			}
			return size;
		}

		public static sbyte ReadFieldByte(MemoryStream stream, ref int offset)
		{
			byte result = stream.GetBuffer()[offset];
			sbyte sresult = 0;
			if (result < 128)
			{
				sresult = (sbyte) result;
			}
			else
			{
				sresult = (sbyte) (result - 256);
			}
			offset++;
			return sresult;
		}

		public static short ReadFieldShort(MemoryStream stream, ref int offset)
		{
			short length = BitConverter.ToInt16(stream.GetBuffer(), offset);
			length = System.Net.IPAddress.NetworkToHostOrder(length);
			offset += 2;
			short result = System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt16(stream.GetBuffer(), offset));
			offset += length;
			return result;
		}

		public static ushort ReadFieldUshort(MemoryStream stream, ref int offset)
		{
			short length = BitConverter.ToInt16(stream.GetBuffer(), offset);
			length = System.Net.IPAddress.NetworkToHostOrder(length);
			offset += 2;
			ushort result = (ushort)System.Net.IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(stream.GetBuffer(), offset));
			offset += length;
			return result;
		}

		public static int ReadFieldInt32(MemoryStream stream, ref int offset)
		{
			short length = BitConverter.ToInt16(stream.GetBuffer(), offset);
			length = System.Net.IPAddress.NetworkToHostOrder(length);
			offset += 2;
			int result = System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt32(stream.GetBuffer(), offset));
			offset += length;
			return result;
		}

		public static uint ReadFieldUint32(MemoryStream stream, ref int offset)
		{
			short length = BitConverter.ToInt16(stream.GetBuffer(), offset);
			length = System.Net.IPAddress.NetworkToHostOrder(length);
			offset += 2;
			uint result = (uint)System.Net.IPAddress.NetworkToHostOrder((int)BitConverter.ToUInt32(stream.GetBuffer(), offset));
			offset += length;
			return result;
		}

		public static string ReadFieldString(MemoryStream stream, ref int offset)
		{
			short length = BitConverter.ToInt16(stream.GetBuffer(), offset);
			length = System.Net.IPAddress.NetworkToHostOrder(length);
			offset += 2;
			string result = ByteHelper.Utf16BigEndianToStr(stream.GetBuffer(), offset, length);
			if (!string.IsNullOrEmpty(result))
			{
				if ((int)result[0] == 65279)
					result = result.Substring(1);
			}
			offset += length;
			return result;
		}

		public static List<sbyte> ReadFieldBytes(MemoryStream stream, ref int offset)
		{
			List<sbyte> list = new List<sbyte>();
			short length = BitConverter.ToInt16(stream.GetBuffer(), offset);
			length = System.Net.IPAddress.NetworkToHostOrder(length);
			offset += 2;
			for (int i = 0; i < length; i++)
			{
				byte result = stream.GetBuffer()[offset];
				sbyte sresult = 0;
				if (result < 128)
				{
					sresult = (sbyte)result;
				}
				else
				{
					sresult = (sbyte)(result - 256);
				}
				list.Add(sresult);
				offset++;
			}
			return list;
		}

		public static List<int> ReadFieldInt32s(MemoryStream stream, ref int offset)
		{
			List<int> list = new List<int>();
			short length = BitConverter.ToInt16(stream.GetBuffer(), offset);
			length = System.Net.IPAddress.NetworkToHostOrder(length);
			length = (short)(length / 4);
			offset += 2;
			for (int i = 0; i < length; i++)
			{
				list.Add(System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt32(stream.GetBuffer(), offset)));
				offset += 4;
			}
			return list;
		}
	}
}
