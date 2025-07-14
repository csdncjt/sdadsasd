using System;
using UnityEngine;
/// <summary>
/// 对PlayerPrefs的管理
/// </summary>
public class PlayerPrefsMgr
{
    private static PlayerPrefsMgr _instance;
    public static PlayerPrefsMgr mInstance
    {
        get
        {
            if (_instance == null)
                _instance = new PlayerPrefsMgr();
            return _instance;
        }
    }

    #region string的get与set
    public void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public string GetString(string key,string def="")
    {
        if (PlayerPrefs.HasKey(key))
        {
            var value = PlayerPrefs.GetString(key,def);
            return value;
        }
        return def;
    }
    #endregion

    #region int的get与set
    public void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public int GetInt(string key,int def=0)
    {
        if (PlayerPrefs.HasKey(key))
        {
            var value = PlayerPrefs.GetInt(key,def);
            return value;
        }
        return def;
    }
    #endregion


}

/// <summary>
/// 对Key 所有定义
/// </summary>
public class PlayerPrefsKeys
{
    public const string Key1 = "Key1";

    public const string KEY_USERID = "USER_ID";
    public const string KEY_TOKEN = "USER_TOKEN";
    public const string KEY_PHONE = "USER_PHONE";
    public const string KEY_LANGUAGE = "KEY_LANGUAGE";//中文ZH,英文EN
    public const string KEY_PHONE_FIRST = "KEY_PHONE_FIRST"; //用户登录手机前缀(例"86")
    public const string KEY_INFODEFAULT = "KEY_INFODEFAULT";//牌局内 个人信息 默认页  德州=1,奥马哈=2
    public const string KEY_COUNTRYCOD = "KEY_COUNTRYCOD";//国家码

    public const string SettingCardType = "SettingCardType";
    public const string SettingDeskType = "SettingDeskType";
    public const string SettingVoice = "SettingVoice";

    public const string SERVER_TYPE = "serverType";
    public const string CURRENT_SERVER_USER_CHOICEID = "CurrentServerUserChoiceID_";
    public const string CURRENT_USING_SERVERID = "CurrentUsingServerID_";
    public const string CURRENT_SERVER_HTTP_DOMAIN = "CurrentServerHttpDomain_";
    public const string CURRENT_SERVER_SCK_DOMAIN = "CurrentServerSckDomain_";
    public const string CURRENT_SERVER_RES_DOMAIN = "CurrentServerResDomain_";
    public const string NET_LINE_SWITCH_INFO = "NetLineSwithInfo_";
    public const string CURRENT_VIP_DNS_SERVERID = "CurrentVipDNSServerID_";

    public const string KEY_PWD = "USER_PWD";

    public const string OnceShowLanguage = "OnceShowLanguage";
}


