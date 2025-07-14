using System;
using System.Collections.Generic;
using System.Text;

public class LanguageManager
{
    private static StringBuilder sBuilder;
    private static LanguageManager instance;
    public static LanguageManager mInstance { get { if (instance == null) instance = new LanguageManager(); return instance; } }

    public Dictionary<int, string> ConsumeMobileData = new Dictionary<int, string>()
    {
            {0,"当前使用移动数据,需要消耗{0}M流量,是否继续下载?" },
            {1,"The current use of mobile data, need to consume {0} M, whether to download?" },
            {2,"當前使用移動數據,需要消耗{0}M流量,是否繼續下載?" },
    };

    /// <summary>
    /// 当前的语言 0简中 1英文 2繁中
    /// </summary>
    public int mCurLanguage = 1;
    private Dictionary<string, string> mDicKeyLanguage = new Dictionary<string, string>();
    public event Action EActTextChange;

    /// <summary>
    /// 初始化 从component传内容
    /// </summary>
    public void StartSetDicLanguage(string pLanguage)
    {
        mDicKeyLanguage.Clear();
        string[] lines = pLanguage.Split(new string[] { "\n" }, StringSplitOptions.None);
        foreach (string line in lines)
        {
            if (line == null || line.Contains("=") == false) continue;

            string[] keyAndValue = line.Split('=');
            if (keyAndValue.Length <= 2)
            {
                mDicKeyLanguage[keyAndValue[0]] = keyAndValue[1];
            }
            else
            {
                if (null == sBuilder)
                    sBuilder = new StringBuilder();
                if (sBuilder.Length > 0)
                    sBuilder.Clear();
                for (int i = 1; i < keyAndValue.Length; i++)
                {
                    sBuilder.Append(keyAndValue[i]);
                    if (i < keyAndValue.Length - 1)
                    {
                        sBuilder.Append("=");
                    }
                }
                mDicKeyLanguage[keyAndValue[0]] = sBuilder.ToString();
            }
        }
    }

    /// <summary>     手动改语言.将发事件给Text 们     </summary>
    public void SettingSetDicLanguage(string pLanguage)
    {
        PlayerPrefsMgr.mInstance.SetInt(PlayerPrefsKeys.KEY_LANGUAGE, mCurLanguage.GetHashCode());
        StartSetDicLanguage(pLanguage);
        EActTextChange();
    }

    /// <summary>     根据key 去取值     </summary>
    public string GetLanguageForKey(string key)
    {
        string value;
        if (mDicKeyLanguage.TryGetValue(key, out value))
        {
//#if LOG_ZSX
//            return value.Replace("\\n", "\n") + "T";
//#endif
            return value.Replace("\\n", "\n");
        }
        return "";
    }

    public static string Get(string key)
    {
        return LanguageManager.mInstance.GetLanguageForKey(key);
    }

    /// <summary>     根据key 去取值   value又有多个值 用^分隔开  index从0开始 </summary>
    public string GetLanguageForKeyMoreValue(string key, int index)
    {
        string value;
        if (mDicKeyLanguage.TryGetValue(key, out value))
        {
            var tOthers = value.Split('^');
//#if LOG_ZSX
//            return tOthers[index] + "T";
//#endif
            return tOthers[index];
        }
        return "";
    }

    public string[] GetLanguageForKeyMoreValues(string key)
    {
        string value;
        if (mDicKeyLanguage.TryGetValue(key, out value))
        {
            var tOthers = value.Split('^');
//#if LOG_ZSX
//            for (int i = 0; i < tOthers.Length; i++)
//            {
//                tOthers[i] = tOthers[i] + "T";
//            }
//#endif
            return tOthers;
        }
        return new string[] { };
    }

}