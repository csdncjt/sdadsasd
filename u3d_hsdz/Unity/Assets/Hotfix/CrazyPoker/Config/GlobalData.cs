using UnityEngine;
using ETModel;
using System;

namespace ETHotfix
{
    public class GlobalData
    {
        public string GlobalIP = "159.75.233.116";

        /// <summary>
        /// 默认服务器类型 0开发服 1测试服 2正式服 3MTT测试服
        /// </summary>
        // public const int DEFAULT_SERVER_TYPE = 0;
        public const int DEFAULT_SERVER_TYPE = 1;
        //public const int DEFAULT_SERVER_TYPE = 2;
        // public const int DEFAULT_SERVER_TYPE = 3;

        /// <summary>
        /// 腾讯IM 0开发服 1测试服 2正式服
        /// </summary>
        public const int IM_APPID_0 = 1400478370;
        public const int IM_APPID_1 = 1400478370;
        public const int IM_APPID_2 = 1400478370;

        /// <summary>
        /// 腾讯BUGLY 1测试服 2正式服
        /// </summary>
        public const string BUGLY_APPID_ANDROID_1 = "***";
        public const string BUGLY_APPID_ANDROID_2 = "***";
        public const string BUGLY_APPID_IOS_1 = "***";
        public const string BUGLY_APPID_IOS_2 = "***";
        public const string BUGLY_VERSIONCHANNEL = "渠道01";

        /// <summary>
        /// 小能客服
        /// </summary>
        public const string KEFU_XIAONENG_APPID = "***";
        public const string KEFU_XIAONENG_APPKEY = "***";

        /// <summary>
        /// TalkingData 1测试服 2正式服
        /// </summary>
        public const string TALKINGDATA_APPID_1 = "***";
        public const string TALKINGDATA_APPID_2 = "***";
        public const string TALKINGDATA_VERSIONCHANNEL = "dev1.0.0"; // 版本渠道

        private static GlobalData _instance;

        public static GlobalData Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GlobalData();
                    _instance.Refresh();
                    ETModel.Game.Hotfix.OnGetNetLineSwith = _instance.OnGetNetLineSwithInfo;
                }
                return _instance;
            }
        }

        public int serverType
        {
            get
            {
                // return PlayerPrefs.GetInt(SERVER_TYPE, 2);
                return PlayerPrefsMgr.mInstance.GetInt(PlayerPrefsKeys.SERVER_TYPE, DEFAULT_SERVER_TYPE);
            }
            set
            {
                PlayerPrefsMgr.mInstance.SetInt(PlayerPrefsKeys.SERVER_TYPE, value);
                PlayerPrefs.Save();
            }
        }

        //收到IM切换消息
        public void OnGetNetLineSwithIMMessage(string info)
        {
            PlayerPrefsMgr.mInstance.SetString($"{PlayerPrefsKeys.NET_LINE_SWITCH_INFO}{serverType}", info);
            PlayerPrefs.Save();
            OnCheckUpdateSwithInfo(info, true, false);
        }

        //收到切换API响应
        private void OnGetNetLineSwithInfo(string info)
        {
            PlayerPrefsMgr.mInstance.SetString($"{PlayerPrefsKeys.NET_LINE_SWITCH_INFO}{serverType}", info);
            PlayerPrefs.Save();
            OnCheckUpdateSwithInfo(info, false, false);
        }

        //用户主动切换
        public void UserSwitchServer(int serverID)
        {
            PlayerPrefsMgr.mInstance.SetString($"{PlayerPrefsKeys.CURRENT_SERVER_USER_CHOICEID}{serverType}", $"{serverID}");
            PlayerPrefs.Save();
            string info = PlayerPrefsMgr.mInstance.GetString($"{PlayerPrefsKeys.NET_LINE_SWITCH_INFO}{serverType}", string.Empty);
            if (!string.IsNullOrEmpty(info))
            {
                OnCheckUpdateSwithInfo(info, false, true);
            }
        }

        public int CurrentUsingServerID()
        {
            return PlayerPrefsMgr.mInstance.GetInt($"{PlayerPrefsKeys.CURRENT_USING_SERVERID}{serverType}", 0);
        }

        public string NameForServerID(int serverID)
        {
            if (serverID == 1)
            {
                return LanguageManager.Get("UILogin_HKLine");//"香港(主线路)";
            }
            if (serverID == 2)
            {
                return LanguageManager.Get("UILogin_EastNosLine");//"东南亚";
            }
            if (serverID == 3)
            {
                return LanguageManager.Get("UILogin_OtherLine");//"日韩欧美澳";
            }
            return $"Line{serverID}";
        }


        private void OnCheckUpdateSwithInfo(string info, bool isIM, bool isUserChoice)
        {
            NetLineSwitchComponent.NetLineSwitchData lineSwitchData = JsonHelper.FromJson<NetLineSwitchComponent.NetLineSwitchData>(info);

            int selectID;

            string userChoice = PlayerPrefsMgr.mInstance.GetString($"{PlayerPrefsKeys.CURRENT_SERVER_USER_CHOICEID}{serverType}", null);
            if (!string.IsNullOrEmpty(userChoice))
            {
                //优先使用用户选择的线路
                selectID = Convert.ToInt32(userChoice);
            }
            else
            {
                if (isIM)
                {
                    //IM切换的，不要使用接口返回的默认线路，使用上次的线路即可，如无则用线路1
                    selectID = PlayerPrefsMgr.mInstance.GetInt($"{PlayerPrefsKeys.CURRENT_VIP_DNS_SERVERID}{serverType}", 1);
                }
                else
                {
                    selectID = lineSwitchData.selector;
                    PlayerPrefsMgr.mInstance.SetInt($"{PlayerPrefsKeys.CURRENT_VIP_DNS_SERVERID}{serverType}", selectID);
                }
            }

            PlayerPrefsMgr.mInstance.SetInt($"{PlayerPrefsKeys.CURRENT_USING_SERVERID}{serverType}", selectID);
            PlayerPrefs.Save();

            foreach (NetLineSwitchComponent.LineInfo lineInfo in lineSwitchData.list)
            {
                if (lineInfo.id == selectID)
                {
                    bool hadChange = false;
                    if (!string.IsNullOrEmpty(lineInfo.http))
                    {
                        if (lineInfo.http != HTTP)
                        {
                            hadChange = true;
                        }
                        PlayerPrefsMgr.mInstance.SetString($"{PlayerPrefsKeys.CURRENT_SERVER_HTTP_DOMAIN}{serverType}", lineInfo.http);
                        PlayerPrefs.Save();
                    }
                    if (!string.IsNullOrEmpty(lineInfo.sck))
                    {
                        if (lineInfo.sck != LoginHost)
                        {
                            hadChange = true;
                        }
                        PlayerPrefsMgr.mInstance.SetString($"{PlayerPrefsKeys.CURRENT_SERVER_SCK_DOMAIN}{serverType}", lineInfo.sck);
                        PlayerPrefs.Save();
                    }
                    Refresh();
                    if (hadChange && GameCache.Instance.nUserId > 0 && !isUserChoice)
                    {
                        //重连
                        NetworkDetectionComponent mNetworkDetectionComponent = Game.Scene.GetComponent<NetworkDetectionComponent>();
                        if (null != mNetworkDetectionComponent)
                            Game.Scene.GetComponent<NetworkDetectionComponent>().Reconnect();
                        // UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                        // {
                        //     type = UIDialogComponent.DialogData.DialogType.Commit,
                        //     title = $"温馨提示",
                        //     content = $"当前线路不稳定，已为您切换线路重新连接。",
                        //     contentCommit = "好的",
                        //     actionCommit = null
                        // });
                        UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                        {
                                type = UIDialogComponent.DialogData.DialogType.Commit,
                                // title = $"温馨提示",
                                title = CPErrorCode.LanguageDescription(10007),
                            // content = $"当前线路不稳定，已为您切换线路重新连接",
                                content = CPErrorCode.LanguageDescription(20072),
                                // contentCommit = "知道了",
                                contentCommit = CPErrorCode.LanguageDescription(10024),
                                contentCancel = "",
                                actionCommit = null,
                                actionCancel = null
                        });
                    }
                }
            }

        }

        private string HTTP;
        public string WebHost;
        public string LoginHost;
        public int APIPort;
        public int PayPort;
        public int LoginPort;
        public int HeadPort;
        public int PaipuPort;  //牌谱
        public int UploadPort; //头像上传
        public bool UseDNS = false;

        public string WebURL;
        public string PayURL;
        public string HeadUrl;
        public string BannerImageUrl;
        public string UploadURL;
        public string PaipuBaseUrl;
        /// <summary>关于我们</summary>
        public string AboutWeURL;
        /// <summary>用户协议</summary>
        public string UserAgentURL;
        /// <summary>数据分析</summary>       
        public string DataAnalysURL;

        public void Refresh()
        {
            Debug.Log("Refresh");
            string httpKey = $"{PlayerPrefsKeys.CURRENT_SERVER_HTTP_DOMAIN}{serverType}";
            string sckKey = $"{PlayerPrefsKeys.CURRENT_SERVER_SCK_DOMAIN}{serverType}";

            switch (serverType)
            {
                case 0:
                    //开发服
                    HTTP = GlobalIP; // 开发服
                    WebHost = $"http://{HTTP}";
                    LoginHost = GlobalIP; // 开发服
                    APIPort = 0000;
                    PayPort = 0000;
                    LoginPort = 0000;
                    HeadPort = 0000;
                    PaipuPort = 0000;  //牌谱
                    UploadPort = 0000; //头像上传
                    UseDNS = false;
                    AboutWeURL = "****";
                    UserAgentURL = "****";
                    DataAnalysURL = "****";
                    break;
                case 1:
                    //测试服
                    HTTP = PlayerPrefsMgr.mInstance.GetString(httpKey, GlobalIP);
                    WebHost = $"http://{HTTP}";
                    LoginHost = PlayerPrefsMgr.mInstance.GetString(sckKey, GlobalIP);
                    APIPort = 5050;
                    PayPort = 8057;
                    LoginPort = 8058;
                    HeadPort = 8600;
                    PaipuPort = 8600;  //牌谱
                    UploadPort = 9663; //头像上传
                    UseDNS = false;
                    AboutWeURL = "http://121.43.156.142:8061/res/staticPage/aboutUs/index.html";
                    UserAgentURL = "http://121.43.156.142:8061/res/staticPage/userLicense/index.html";
                    DataAnalysURL = "http://121.43.156.142:8061/res/staticPage/dataAnalys/index.html";
                    break;
                case 2:
                    //正式服
                    HTTP = PlayerPrefsMgr.mInstance.GetString(httpKey, GlobalIP);
                    WebHost = $"http://{HTTP}";
                    LoginHost = PlayerPrefsMgr.mInstance.GetString(sckKey, GlobalIP);
                    APIPort = 0000;
                    PayPort = 0000;
                    LoginPort = 0000;
                    HeadPort = 0000;
                    PaipuPort = 0000;  //牌谱
                    UploadPort = 0000; //头像上传
                    UseDNS = false;
                    AboutWeURL = "****";
                    UserAgentURL = "****";
                    DataAnalysURL = "****";
                    break;
                case 3:
                    //测试服（MTT用）
                    HTTP = "139.9.176.178";
                    WebHost = $"http://{HTTP}";
                    LoginHost = "139.9.176.178";
                    APIPort = 5050;
                    PayPort = 8057;
                    LoginPort = 8058;
                    HeadPort = 8600;
                    PaipuPort = 8600;  //牌谱
                    UploadPort = 9663; //头像上传
                    UseDNS = false;
                    AboutWeURL = "https://xx/staticPage/aboutUs/index.html";
                    UserAgentURL = "https://xx/staticPage/userLicense/index.html";
                    DataAnalysURL = "https://xx/staticPage/dataAnalys/index.html";
                    break;
            }

            WebURL = $"{WebHost}:{APIPort}";
            PayURL = $"{WebHost}:{PayPort}";
            // HeadUrl = $"http://{LoginHost}:{HeadPort}/dzfile/img/";
            HeadUrl = $"{WebHost}:{HeadPort}/dzfile/img/";
            // BannerImageUrl = $"http://{LoginHost}:{HeadPort}";
            BannerImageUrl = $"{WebHost}:{HeadPort}";
            UploadURL = $"{WebHost}:{UploadPort}";
            PaipuBaseUrl = $"{WebHost}:{PaipuPort}/?lan=zh&info_id=";
        }

        public static string WebVersion = UnityEngine.Application.version;
        public static int WebChannelId = 3;

        public int WebCliType
        {
            get
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    return 1;
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    return 2;
                }
                return 2;
            }
        }
    }
}
