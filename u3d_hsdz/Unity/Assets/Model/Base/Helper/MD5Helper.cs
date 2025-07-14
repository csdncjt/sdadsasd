using System.IO;
using System.Security.Cryptography;

namespace ETModel
{
	public static class MD5Helper
	{
		public static string FileMD5(string filePath)
		{
			byte[] retVal;
            using (FileStream file = new FileStream(filePath, FileMode.Open))
			{
				MD5 md5 = new MD5CryptoServiceProvider();
				retVal = md5.ComputeHash(file);
			}
			return retVal.ToHex("x2");
		}

		public static string StringMD5(string str)
		{
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] retVal = md5.ComputeHash(str.ToUtf8());
			return retVal.ToHex("x2");
		}
	}
}
