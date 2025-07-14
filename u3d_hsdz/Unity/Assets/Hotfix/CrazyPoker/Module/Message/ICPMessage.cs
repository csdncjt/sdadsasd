using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETHotfix
{
	public interface ICPMessage 
	{
		int RpcId { get; set; }
	}

	public interface ICPRequest : ICPMessage
	{
		ushort SerializeTo(MemoryStream stream);
	}

	public interface ICPResponse : ICPMessage
	{
		int Error { get; set; }
		object DeserializeFrom(MemoryStream stream);
	}

	public class CPResponseMessage: ICPResponse
	{
		public int RpcId { get; set; }
		public int Error { get; set; }
		public object DeserializeFrom(MemoryStream stream)
		{
			return this;
		}
	}
}
