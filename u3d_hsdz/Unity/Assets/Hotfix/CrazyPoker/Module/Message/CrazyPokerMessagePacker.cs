using System;
using System.IO;

namespace ETHotfix
{
	public class CrazyPokerMessagePacker
	{
		public static byte[] SerializeTo(object obj)
		{
			throw new NotImplementedException();
		}

		public static ushort SerializeTo(object obj, MemoryStream stream)
		{
			ICPRequest request = obj as ICPRequest;
			if (request != null)
			{
				return request.SerializeTo(stream);
			}

			return 0;
		}

		public static object DeserializeFrom(Type type, byte[] bytes, int index, int count)
		{
			throw new NotImplementedException();
		}

		public static object DeserializeFrom(object instance, byte[] bytes, int index, int count)
		{
			throw new NotImplementedException();
		}

		public static object DeserializeFrom(Type type, MemoryStream stream)
		{
			throw new NotImplementedException();
		}

		public static object DeserializeFrom(object instance, MemoryStream stream)
		{
			ICPResponse response = instance as ICPResponse;
			if (response != null)
			{
				return response.DeserializeFrom(stream);
			}

			return null;
		}
	}
}
