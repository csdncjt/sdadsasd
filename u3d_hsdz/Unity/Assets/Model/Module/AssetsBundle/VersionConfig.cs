using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
	public class FileVersionInfo
	{
		public string File;
		public string MD5;
		public long Size;
	}

	public class VersionConfig : Object
	{
		public long Version;
		
		public long TotalSize;

        /// <summary>
        /// 0不加密 1加密
        /// </summary>
        public int ABEncrypt;

         [BsonIgnore]
		public Dictionary<string, FileVersionInfo> FileInfoDict = new Dictionary<string, FileVersionInfo>();

		public override void EndInit()
		{
			base.EndInit();

			foreach (FileVersionInfo fileVersionInfo in this.FileInfoDict.Values)
			{
				this.TotalSize += fileVersionInfo.Size;
			}
		}
	}
}