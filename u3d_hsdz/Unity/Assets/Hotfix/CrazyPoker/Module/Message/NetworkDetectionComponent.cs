using System;
using System.Collections.Generic;
using System.Net;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class NetworkDetectionAwakeSystem : AwakeSystem<NetworkDetectionComponent>
    {
        public override void Awake(NetworkDetectionComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class NetworkDetectionUpdateSystem : UpdateSystem<NetworkDetectionComponent>
    {
        public override void Update(NetworkDetectionComponent self)
        {
            self.Update();
        }
    }

    public class NetworkDetectionComponent : Component
    {
        public static NetworkDetectionComponent Instance;

        private string LOG_TAG = "Hotfix_NetworkDetectionComponent";

        public float sendInterval = 1f;
        private float recordDeltaTime = 0f;

        public float beginReconnectTime = 0f;
        public float reconnectTimeOut = 5f;

        private bool netAvailable = true;  // 网络是否可用
        private bool fromBack = false;

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            ETModel.Game.Hotfix.OnApplicationPauseTrue -= OnApplicationPauseTrue;
            ETModel.Game.Hotfix.OnApplicationPauseFalse -= OnApplicationPauseFalse;

            base.Dispose();
        }

        public void Awake()
        {
            Instance = this;
            ETModel.Game.Hotfix.OnApplicationPauseTrue += OnApplicationPauseTrue;
            ETModel.Game.Hotfix.OnApplicationPauseFalse += OnApplicationPauseFalse;

            recordDeltaTime = 0;
        }

        public void Update()
        {
            // 每秒检测一次
            if (!(Time.time - recordDeltaTime > sendInterval))
            {
                return;
            }
            recordDeltaTime = Time.time;

            bool isReconentTimeOut = false;
            if (beginReconnectTime > 0 && Time.time - beginReconnectTime > reconnectTimeOut)
            {
                isReconentTimeOut = true;
            }

            if (UnityEngine.Application.internetReachability != 0)
            {
                if (!netAvailable || fromBack || isReconentTimeOut)
                {
                    UIComponent.Instance.Prompt();
                    if (!netAvailable)
                    {
                        UI mPromptWifi = UIComponent.Instance.Get(UIType.UIPromptWifi);
                        if (null != mPromptWifi)
                        {
                            UIComponent.Instance.Remove(UIType.UIPromptWifi);
                        }
                    }
                    netAvailable = true;
                    fromBack = false;
                    beginReconnectTime = Time.time;

                    NetworkUtil.RemoveAllSessionComponent();

                    // 登录界面不需要重连
                    UI mUILogin = UIComponent.Instance.Get(UIType.UILogin);
                    if (null != mUILogin && mUILogin.GameObject.activeInHierarchy)
                    {
                        UIComponent.Instance.ClosePrompt();
                        beginReconnectTime = 0;
                        return;
                    }

                    // Login服
                    CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_LOGIN, REQ_LOGIN_HANDLER);
                    NetOuterComponent mNetOuterComponent = Game.Scene.ModelScene.GetComponent<NetOuterComponent>();
                    if (!string.IsNullOrEmpty(GameCache.Instance.strToken))
                    {
                        // token重连
                        try
                        {
                            string[] mArrAddress = NetHelper.GetAddressIPs(GlobalData.Instance.LoginHost, GlobalData.Instance.UseDNS);
                            ETModel.Session mSession = mNetOuterComponent.Create(new IPEndPoint(IPAddress.Parse(mArrAddress[0]), GlobalData.Instance.LoginPort));
                            CPLoginSessionComponent mCPLoginSessionComponent = Game.Scene.GetComponent<CPLoginSessionComponent>();
                            if (null != mCPLoginSessionComponent)
                                Game.Scene.RemoveComponent<CPLoginSessionComponent>();
                            mCPLoginSessionComponent = Game.Scene.AddComponent<CPLoginSessionComponent>();
                            mCPLoginSessionComponent.HotfixSession = ComponentFactory.Create<Session, ETModel.Session>(mSession);
                            mCPLoginSessionComponent.HotfixSession.SetProtrolVersionInHead(1);
                            mCPLoginSessionComponent.HotfixSession.SetUserIdInHead(GameCache.Instance.nUserId);
                            mCPLoginSessionComponent.HotfixSession.SetLanguageInHead(1);
                            byte mPlatform = 2;
                            if (Application.platform == RuntimePlatform.Android)
                                mPlatform = 1;
                            mCPLoginSessionComponent.HotfixSession.SetPlatformInHead(mPlatform);
                            mCPLoginSessionComponent.HotfixSession.SetBuildVersionInHead(254);
                            mCPLoginSessionComponent.HotfixSession.SetChannelInHead(14);
                            mCPLoginSessionComponent.HotfixSession.SetProductIdInHead(1002);
                            mCPLoginSessionComponent.HotfixSession.SetTokenInHead(GameCache.Instance.strToken);
                            mCPLoginSessionComponent.HotfixSession.Send(new REQ_LOGIN
                            {
                                userType = 10,
                                userUUID = GameCache.Instance.strToken,
                                macAddress = SystemInfoUtil.getLocalMac(),
                                imsi = SystemInfoUtil.getDeviceUniqueIdentifier(),
                                HeighthAndWidth = $"{Screen.width}*{Screen.height}",
                                model = SystemInfoUtil.isSimulator == 0 ? SystemInfoUtil.getDeviceMode() : "simulator",
                                system_version = SystemInfoUtil.getOperatingSystem(),
                                sessionKey = string.Empty,
                                nickname = string.Empty,
                                countryCode = string.Empty,
                                version = "3.9.7",
                                channel = 3
                            });
                        }
                        catch (Exception e)
                        {
                            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_LOGIN, REQ_LOGIN_HANDLER);
                            NetworkUtil.RemoveAllSessionComponent();

                            // UIComponent.Instance.RemoveAll();
                            // UIComponent.Instance.ShowNoAnimation(UIType.UILogin, "NoAutoLogin");
                            // UIComponent.Instance.Toast("网路异常");
                            // UIComponent.Instance.ClosePrompt();
                        }
                    }
                }
            }
            else
            {
                if (netAvailable)
                {
                    netAvailable = false;
                    NetworkUtil.RemoveAllSessionComponent();

                    // UIComponent.Instance.Toast("请检查网络");
                    UIComponent.Instance.ShowNoAnimation(UIType.UIPromptWifi);
                }
            }
        }

        private void OnApplicationPauseTrue()
        {
            // 暂停
            if (Application.platform == RuntimePlatform.Android)
            {
                NetworkUtil.RemoveAllSessionComponent();
                fromBack = false;
            }
        }

        private void OnApplicationPauseFalse()
        {
            // 恢复
            if (Application.platform == RuntimePlatform.Android)
            {
                fromBack = true;
            }
        }

        public void Reconnect()
        {
            NetworkUtil.RemoveAllSessionComponent();
            fromBack = true;
        }

        private void REQ_LOGIN_HANDLER(ICPResponse response)
        {
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_LOGIN, REQ_LOGIN_HANDLER);
            REQ_LOGIN rec = response as REQ_LOGIN;
            if (null == rec)
                return;

            switch (rec.status)
            {
                case 0:
                    // Log.Debug($"重连Login成功");
                    //UIComponent.Instance.Toast( "重连Login成功");

                    if (rec.user_type == 12 || rec.user_type == 10)
                    {
                        Game.Scene.RemoveComponent<CPLoginSessionComponent>();
                        PlayerPrefsMgr.mInstance.SetString(PlayerPrefsKeys.KEY_TOKEN, rec.token);
                        PlayerPrefs.Save();
                        GameCache.Instance.strToken = rec.token;
                        GameCache.Instance.resIP = rec.IP;
                        GameCache.Instance.resport = rec.port;
                        GameCache.Instance.isfirstLogin = rec.isfirstLogin;

                        // 连接Res服务器
                        try
                        {
                            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_LOGIN_RESOURCES, REQ_LOGIN_RESOURCES_HANDLER);
                            string[] mArrAddress = NetHelper.GetAddressIPs(GlobalData.Instance.LoginHost, GlobalData.Instance.UseDNS);
                            ETModel.Session mSession = Game.Scene.ModelScene.GetComponent<NetOuterComponent>()
                                    .Create(new IPEndPoint(IPAddress.Parse(mArrAddress[0]), GameCache.Instance.resport));
                            CPResSessionComponent mCpResSessionComponent =
                                    Game.Scene.GetComponent<CPResSessionComponent>() ?? Game.Scene.AddComponent<CPResSessionComponent>();
                            mCpResSessionComponent.HotfixSession = ComponentFactory.Create<Session, ETModel.Session>(mSession);
                            mCpResSessionComponent.HotfixSession.SetProtrolVersionInHead(1);
                            mCpResSessionComponent.HotfixSession.SetUserIdInHead(GameCache.Instance.nUserId);
                            mCpResSessionComponent.HotfixSession.SetLanguageInHead(1);
                            byte mPlatform = 2;
                            if (Application.platform == RuntimePlatform.Android)
                                mPlatform = 1;
                            mCpResSessionComponent.HotfixSession.SetPlatformInHead(mPlatform);
                            mCpResSessionComponent.HotfixSession.SetBuildVersionInHead(254);
                            mCpResSessionComponent.HotfixSession.SetChannelInHead(14);
                            mCpResSessionComponent.HotfixSession.SetProductIdInHead(1002);
                            mCpResSessionComponent.HotfixSession.SetTokenInHead(GameCache.Instance.strToken);
                            mCpResSessionComponent.HotfixSession.Send(new REQ_LOGIN_RESOURCES() { key = rec.key });
                        }
                        catch (Exception e)
                        {
                            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_LOGIN_RESOURCES, REQ_LOGIN_RESOURCES_HANDLER);
                            NetworkUtil.RemoveAllSessionComponent();
                            // UIComponent.Instance.RemoveAll();
                            // UIComponent.Instance.ShowNoAnimation(UIType.UILogin, "NoAutoLogin");
                            // UIComponent.Instance.Toast($"登录Res失败[{rec.status}]");
                            // UIComponent.Instance.ClosePrompt();
                        }
                    }
                    break;
                case 1:
                    Log.Debug("用户不存在");
                    optErrorReqLoginHandler();
                    UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10144));
                    break;
                case 2:
                    Log.Debug("密码不对");
                    optErrorReqLoginHandler();
                    UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10145));
                    break;
                case 3:
                    Log.Debug("一般错误");
                    optErrorReqLoginHandler();
                    UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10146));
                    break;
                case 4:
                    Log.Debug("版本太低");
                    optErrorReqLoginHandler();
                    UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10147));
                    break;
                case 5:
                    // token失效，返回登录页
                    Log.Debug("token不对");
                    optErrorReqLoginHandler();
                    GotoLoginPage();
                    UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(6, 5));
                    break;
                case 6:
                    Log.Debug("账户或密码错误");
                    optErrorReqLoginHandler();
                    UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10148));
                    break;
                case 7:
                    Log.Debug("账号被封禁");
                    optErrorReqLoginHandler();
                    GotoLoginPage();
                    UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10149));
                    break;
                case 8:
                    Log.Debug("手机异常，无法登录");//账号被冻结
                    optErrorReqLoginHandler();
                    GotoLoginPage();
                    UIComponent.Instance.Toast(CPErrorCode.ErrorDescription(2004));
                    break;
                case 9:
                    Log.Debug("玩家非指定机器码登录");//玩家非指定机器码登录
                    UI mUITexas = UIComponent.Instance.Get(UIType.UITexas);
                    if (null == mUITexas || !mUITexas.GameObject.activeInHierarchy)
                    {
                        // 在大厅界面
                        UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                        {
                                type = UIDialogComponent.DialogData.DialogType.Commit,
                                content = CPErrorCode.LanguageDescription(20079),
                                contentCommit = CPErrorCode.LanguageDescription(10012),
                                actionCommit = () =>
                                {
                                    NetworkUtil.RemoveAllSessionComponent();
                                    NetworkUtil.LogoutClear();
                                    UIComponent.Instance.RemoveAll(new List<string>() { UIType.UIMarquee });
                                    UIComponent.Instance.ShowNoAnimation(UIType.UILogin, "NoAutoLogin");
                                }
                        });
                    }
                    else
                    {
                        // 在游戏中
                        NetworkUtil.RemoveAllSessionComponent();
                        NetworkUtil.LogoutClear();
                        UIComponent.Instance.RemoveAll(new List<string>() { UIType.UIMarquee });
                        UIComponent.Instance.ShowNoAnimation(UIType.UILogin, "NoAutoLogin");
                        UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(20079));
                    }
                    break;
                default:
                    Log.Debug("未知错误");
                    optErrorReqLoginHandler();
                    UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10052));
                    break;
            }
        }

        private void optErrorReqLoginHandler()
        {
            beginReconnectTime = 0;
            NetworkUtil.RemoveAllSessionComponent();
        }

        private void GotoLoginPage()
        {
            NetworkUtil.LogoutClear();
            UIComponent.Instance.RemoveAll(new List<string>() { UIType.UIMarquee });
            UIComponent.Instance.ShowNoAnimation(UIType.UILogin, "NoAutoLogin");
        }

        private void REQ_LOGIN_RESOURCES_HANDLER(ICPResponse response)
        {
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_LOGIN_RESOURCES, REQ_LOGIN_RESOURCES_HANDLER);
            REQ_LOGIN_RESOURCES rec = response as REQ_LOGIN_RESOURCES;
            if (null == rec)
                return;

            if (rec.status != 0)
            {
                string mErrorMsg = string.Empty;
                if (rec.status == 7)
                {
                    mErrorMsg = CPErrorCode.LanguageDescription(10149);
                    GotoLoginPage();
                }
                else
                { 
                    mErrorMsg = CPErrorCode.LanguageDescription(10150); 
                }
                    
                beginReconnectTime = 0;
                NetworkUtil.RemoveAllSessionComponent();

                UIComponent.Instance.Toast(mErrorMsg);
                return;
            }

            // Log.Debug($"登录Res成功");
            //UIComponent.Instance.Toast( "重连Res成功");

            GameCache.Instance.sex = rec.sex;
            GameCache.Instance.msg_count = rec.msg_count;
            GameCache.Instance.nick = rec.nick;
            GameCache.Instance.headPic = rec.head_picName;
            GameCache.Instance.gold = rec.gold;
            GameCache.Instance.idou = rec.idou;
            GameCache.Instance.vipLevel = rec.vipLevel;
            GameCache.Instance.vipEndDate = rec.vipEndDate;
            GameCache.Instance.playerflag = rec.playerflag;

            // 添加Res服务器心跳组件
            CPResSessionComponent mCPResSessionComponent = Game.Scene.GetComponent<CPResSessionComponent>();
            CPResHeartbeatComponent mCPResHeartbeatComponent = mCPResSessionComponent.HotfixSession.GetComponent<CPResHeartbeatComponent>();
            if (null != mCPResHeartbeatComponent)
                mCPResSessionComponent.HotfixSession.RemoveComponent<CPResHeartbeatComponent>();
            mCPResHeartbeatComponent = mCPResSessionComponent.HotfixSession.AddComponent<CPResHeartbeatComponent>();
            mCPResHeartbeatComponent.StartSending();

            // 是否牌桌中
            UI mUITexas = UIComponent.Instance.Get(UIType.UITexas);
            if (null != mUITexas && mUITexas.GameObject.activeInHierarchy)
            {
                if (!GameCache.Instance.CurGame.ShouldAllowReconectRoom())
                    return;

                try
                {
                    string[] mArrAddress = NetHelper.GetAddressIPs(GlobalData.Instance.LoginHost, GlobalData.Instance.UseDNS);
                    var mNetOuterComponent = Game.Scene.ModelScene.GetComponent<NetOuterComponent>();
                    ETModel.Session mSession = mNetOuterComponent.Create(new IPEndPoint(IPAddress.Parse(mArrAddress[0]), GameCache.Instance.roomPort));
                    CPGameSessionComponent mCPGameSessionComponent = Game.Scene.GetComponent<CPGameSessionComponent>();
                    if (null != mCPGameSessionComponent)
                        Game.Scene.RemoveComponent<CPGameSessionComponent>();
                    mCPGameSessionComponent = Game.Scene.AddComponent<CPGameSessionComponent>();
                    mCPGameSessionComponent.HotfixSession = ComponentFactory.Create<Session, ETModel.Session>(mSession);
                    mCPGameSessionComponent.HotfixSession.SetProtrolVersionInHead(1);
                    mCPGameSessionComponent.HotfixSession.SetUserIdInHead(GameCache.Instance.nUserId);
                    mCPGameSessionComponent.HotfixSession.SetLanguageInHead(1);
                    byte mPlatform = 2;
                    if (Application.platform == RuntimePlatform.Android)
                        mPlatform = 1;
                    mCPGameSessionComponent.HotfixSession.SetPlatformInHead(mPlatform);
                    mCPGameSessionComponent.HotfixSession.SetBuildVersionInHead(254);
                    mCPGameSessionComponent.HotfixSession.SetChannelInHead(14);
                    mCPGameSessionComponent.HotfixSession.SetProductIdInHead(1002);
                    mCPGameSessionComponent.HotfixSession.SetTokenInHead(GameCache.Instance.strToken);
                    RoomPath mEnumRoomPath = (RoomPath)GameCache.Instance.room_path;
                    ICPRequest mCpRequest = null;
                    switch (mEnumRoomPath)
                    {
                        case RoomPath.Normal:
                        // 普通
                        case RoomPath.NormalThanOff:
                        // 普通必下场
                        case RoomPath.NormalAof:
                        case RoomPath.DP:
                        case RoomPath.DPAof:
                            // 普通AOF
                            mCpRequest = new REQ_GAME_ENTER_ROOM()
                            {
                                type = 1,
                                connect = 1,
                                unknow = 1,
                                roomPath = GameCache.Instance.room_path,
                                roomID = GameCache.Instance.room_id,
                                deskId = 0
                            };
                            break;
                        case RoomPath.MTT:
                            // MTT
                            mCpRequest = new REQ_GAME_MTT_ENTER_ROOM()
                            {
                                type = 1,
                                connect = 1,
                                unknow = 1,
                                roomPath = GameCache.Instance.room_path,
                                roomID = GameCache.Instance.room_id,
                                deskId = GameCache.Instance.mtt_deskId
                            };
                            break;
                        case RoomPath.SNG:
                            // SNG
                            break;
                        case RoomPath.Omaha:
                        // 奥马哈
                        case RoomPath.OmahaThanOff:
                        // 奥马哈必下场
                        case RoomPath.OmahaAof:
                            // 奥马哈AOF
                            mCpRequest = new REQ_GAME_OMAHA_ENTER_ROOM()
                            {
                                type = 1,
                                connect = 1,
                                unknow = 1,
                                roomPath = GameCache.Instance.room_path,
                                roomID = GameCache.Instance.room_id,
                                deskId = 0
                            };
                            break;
                        case RoomPath.PAP:
                            // 大菠萝
                            break;
                    }

                    UIComponent.Instance.ClosePrompt();

                    if (null != mCpRequest)
                    {
                        mCPGameSessionComponent.HotfixSession.Send(mCpRequest);
                    }

                    beginReconnectTime = 0;
                }
                catch (Exception e)
                {
                    NetworkUtil.RemoveAllSessionComponent();
                }
            }
            else
            {
                // 重连成功
                beginReconnectTime = 0;
                UIComponent.Instance.ClosePrompt();
            }
        }
    }
}
