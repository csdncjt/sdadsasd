using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ETModel;

//用于处理系统消息
namespace ETHotfix
{

    [Event(EventIdType.AccountFreeze)]
    public class SystemMesEvent : AEvent
    {
        public override void Run()
        {
            SystemMesComponent smes = UIComponent.Instance.Get(UIType.UILobby_Menu).GetComponent<SystemMesComponent>();
            if (smes != null && smes.isForceLoginOut)
            {
                Log.Debug("recv event :: AccountFreeze");
                smes.ForceLoginOut();
            }
        }
    }

    [ObjectSystem]
    public class SystemMesComponentAwakeSystem : AwakeSystem<SystemMesComponent>
    {
        public override void Awake(SystemMesComponent self)
        {
            self.Awake();
        }
    }

    //[ObjectSystem]
    //public class SystemMesComponentUpdateSystem : UpdateSystem<SystemMesComponent>
    //{
    //    public override void Update(SystemMesComponent self)
    //    {
    //        //测试
    //        if (Input.GetKeyDown(KeyCode.Alpha1))
    //        {
    //            self.OnServerMes("cmd@%tactivity_message@%1@%1000@%10001");
    //        }
    //    }
    //}

    public class SystemMesComponent : Component
    {
        static public SystemMesComponent Instance;
        public Action<string> newMessageDelegate;//我的页面注册消息
        public Action<string , string , string> texasGameMessageDelegate;//牌局内消息
        public bool isForceLoginOut;
        public void Awake() {

            Instance = this;
            isForceLoginOut = false;
            //系统事件 统一接收
            ETModel.Game.Hotfix.OnServerMes += OnServerMes;
            ETModel.Game.Hotfix.OnGroupMes += OnGroupMes;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            ETModel.Game.Hotfix.OnServerMes -= OnServerMes;
            ETModel.Game.Hotfix.OnGroupMes -= OnGroupMes;
            base.Dispose();
        }

        /// <summary>
        /// 系统事件  cmd@%activity_message@%arg2@%arg3@%arg4...
        /// </summary>
        /// <param name="str"></param>
        public void OnServerMes(string str)
        {
            string[] args = str.Split(new string[] { "@%" }, StringSplitOptions.None);
            string cmd = args[1].Trim();
            switch (cmd) {

                case "new_message":
                {
                    if (newMessageDelegate != null) newMessageDelegate(args.Length >= 3? args[2].Trim() : null);
                }
                    break;
                case "activity_message":
                {
                    string version = args[2];
                    if (version.Equals("1"))
                    {
                        string bean = args[3];
                        UIComponent.Instance.Show(UIType.UILobby_Activity, bean);
                    }
                    else if (version.Equals("2"))
                    {
                        UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                        {
                                type = UIDialogComponent.DialogData.DialogType.Commit,
                                // title = $"温馨提示",
                                title = CPErrorCode.LanguageDescription(10007),
                                content = CPErrorCode.LanguageDescription(20070),
                                // contentCommit = "知道了",
                                contentCommit = CPErrorCode.LanguageDescription(10024),
                                contentCancel = "",
                                actionCommit = null,
                                actionCancel = null
                        });
                        // UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(20070));
                    }
                }
                    break;
                case "user_forzen_message":
                {
                    isForceLoginOut = true;
                    if (GameCache.Instance.CurGame != null)
                    {
                        //牌局内  强制玩家退出牌局
                        UIComponent.Instance.Get(UIType.UITexas).GetComponent<UITexasComponent>().isOffline = true;
                        GameCache.Instance.CurGame.CallbackExit(7); //action = 7 服务器约定是冻结退出
                    }
                    else
                    {
                        ForceLoginOut();
                    }
                }
                    break;
                case "chgNet":
                {
                    //线路切换  
                    Log.Debug($"参数 {args[2]} = {args[3]}");
                    string time = args[2];
                    string jsonInfo = args[3];
                    if (TimeHelper.GetTimestamp() <= Convert.ToInt64(time))
                    {
                        GlobalData.Instance.OnGetNetLineSwithIMMessage(jsonInfo);
                    }
                }
                    break;
                case "tactivity_message":
                {
                    string arg2 = args.Length >= 3? args[2].Trim() : null; // 固定1，暂时没用
                    string arg3 = args.Length >= 4? args[3].Trim() : null; // USDT数
                    string arg4 = args.Length >= 5? args[4].Trim() : null; // userId
                    if (texasGameMessageDelegate != null) texasGameMessageDelegate(arg2, arg3, arg4);
                }
                    break;
                case "ilit_message":
                {
                    string key = $"{GameCache.Instance.nUserId}86FA75CC4332B3B94DB81C4B96FE8F64";
                    key = key.Substring(0, 16);
                    string imei = GameUtil.AESDecrypt(args[2], key);
                    string[] imeis = imei.Split(new string[] { "," }, StringSplitOptions.None);
                    string curImei = SystemInfoUtil.getDeviceUniqueIdentifier();
                    bool matched = false;
                    for (int i = 0, n = imeis.Length; i < n; i++)
                    {
                        if (curImei.Equals(imeis[i]))
                        {
                            matched = true;
                            break;
                        }
                    }

                    if (!matched)
                    {
                        isForceLoginOut = true;
                        if (GameCache.Instance.CurGame != null)
                        {
                            //牌局内  强制玩家退出牌局
                            UIComponent.Instance.Get(UIType.UITexas).GetComponent<UITexasComponent>().isOffline = true;
                            GameCache.Instance.CurGame.CallbackExit(7); //action = 7 服务器约定是冻结退出
                        }
                        else
                        {
                            ForceLoginOut();
                        }
                    }
                }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// IM群组信息事件
        /// </summary>
        /// <param name="value"></param>
        public void OnGroupMes(string str)
        {
            IMMes mes = JsonHelper.FromJson<IMMes>(str);
            string[] args = mes.mesContent.Split(new string[] { "@%" }, StringSplitOptions.None);
            if (args.Length < 2)
                return;
            string cmd = args[1].Trim();
            switch (cmd)
            {
                case "tactivity_message":
                {
                    string arg2 = args.Length >= 3? args[2].Trim() : null; // 固定1，暂时没用
                    string arg3 = args.Length >= 4? args[3].Trim() : null; // USDT数
                    string arg4 = args.Length >= 5? args[4].Trim() : null; // userId
                    //if(arg3 != null) arg3 = StringHelper.ShowGold(int.Parse(arg3));
                    if (texasGameMessageDelegate != null) texasGameMessageDelegate(arg2, arg3, arg4);
                }
                    break;
                case "activity_switch":
                {
                    int acitvityType = Convert.ToInt32(args[2]); // 类型 1跑马灯
                    int openState = Convert.ToInt32(args[3]); // 开关状态 0关 1开
                    int roomId = Convert.ToInt32(args[4]); // 房间Id
                    switch (acitvityType)
                    {
                        // 跑马灯
                        case 1:
                        {
                            if (null != GameCache.Instance.CurGame && GameCache.Instance.room_id.Equals(roomId))
                            {
                                GameCache.Instance.CurGame.OpenMarquee(openState == 1);
                            }
                        }
                            break;
                    }
                }
                    break;
                default:
                    break;
            }
        }

        public void ForceLoginOut() {

            //账户被冻结  直接登出账户
            GameCache.Instance.strPwd = "";
            GameCache.Instance.nUserId = 0;
            GameCache.Instance.strToken = "";
            GameCache.Instance.headPic = "";
            GameCache.Instance.nick = "";
            UIComponent.Instance.RemoveAll(new List<string>() { UIType.UIMarquee });
            UIComponent.Instance.ShowNoAnimation(UIType.UILogin, "NoAutoLogin");
            NetworkUtil.RemoveAllSessionComponent();
            UIComponent.Instance.ShowNoAnimation(UIType.UIDialog,
            new UIDialogComponent.DialogData()
            {
                type = UIDialogComponent.DialogData.DialogType.Commit,
                title = LanguageManager.mInstance.GetLanguageForKey("club_trans_0"), //"系统消息",
                content = LanguageManager.mInstance.GetLanguageForKey("SystemMes_inadffAl"), //"您的账户已冻结，请点击下角联系客服",
                contentCommit = LanguageManager.mInstance.GetLanguageForKey("adaptation10012"), //"确定",
            });
        }

        public void ForceLoginOutByImei()
        {
            //账户被冻结  直接登出账户
            GameCache.Instance.strPwd = "";
            GameCache.Instance.nUserId = 0;
            GameCache.Instance.strToken = "";
            GameCache.Instance.headPic = "";
            GameCache.Instance.nick = "";
            UIComponent.Instance.RemoveAll(new List<string>() { UIType.UIMarquee });
            UIComponent.Instance.ShowNoAnimation(UIType.UILogin, "NoAutoLogin");
            NetworkUtil.RemoveAllSessionComponent();
            UIComponent.Instance.ShowNoAnimation(UIType.UIDialog,
                                                 new UIDialogComponent.DialogData()
                                                 {
                                                         type = UIDialogComponent.DialogData.DialogType.Commit,
                                                         title = LanguageManager.mInstance.GetLanguageForKey("club_trans_0"), //"系统消息",
                                                         content = CPErrorCode.LanguageDescription(20079), 
                                                         contentCommit = CPErrorCode.LanguageDescription(10012)
                                                 });
        }
    }
}
