using System;
using UnityEngine;

namespace ETModel
{
	public static class ConfigHelper
    {
		public static string GetText(string key)
		{
			try
			{
				GameObject config = (GameObject)Game.Scene.GetComponent<ResourcesComponent>().GetAsset("config.unity3d", "Config");
				string configStr = config.Get<TextAsset>(key).text;
				return configStr;
			}
			catch (Exception e)
			{
				throw new Exception($"load config file fail, key: {key}", e);
			}
		}

        // 0开发服 1测试服 2正式服 3提审服
        public static readonly string[] GLOBALPROTOS = new[] { "GlobalProto_Offline", "GlobalProto_Test", "GlobalProto", "GlobalProto_AppStore" };
        public static string GetGlobal()
		{
			try
			{
				GameObject config = (GameObject)ResourcesHelper.Load("KV");
                string globalProtoName = string.Empty;
                int serverType = PlayerPrefsMgr.mInstance.GetInt("serverType", GlobalData.DEFAULT_SERVER_TYPE);
                globalProtoName = serverType < GLOBALPROTOS.Length? GLOBALPROTOS[serverType] : GLOBALPROTOS[2];
				return config.Get<TextAsset>(globalProtoName).text;
			}
			catch (Exception e)
			{
				throw new Exception($"load global config file fail", e);
			}
		}

		public static T ToObject<T>(string str)
		{
			return JsonHelper.FromJson<T>(str);
		}
	}
}