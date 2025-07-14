using System;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using DragonBones;

namespace ETHotfix
{
    [ObjectSystem]
    public class TexasGameAwakeSystem : AwakeSystem<TexasGame, Component>
    {
        public override void Awake(TexasGame self, Component component)
        {
            self.Awake(component);
        }
    }

    [ObjectSystem]
    public class TexasGameUpdateSystem : UpdateSystem<TexasGame>
    {
        public override void Update(TexasGame self)
        {
            self.Update();
        }
    }

    public class GameAnimInfo
    {
        /// <summary>
        /// 动画名
        /// </summary>
        public string name;
        /// <summary>
        /// 动画状态 0结束，1播放中
        /// </summary>
        public int state;
    }

    public class PublicCardInfo
    {
        public sbyte cardId;
        public Transform trans;
        public Image imageCard;
        public Image imageSelect;
    }

    public class PotInfo
    {
        public int pot;
        public Transform trans;
        public Text textPot;
        public Image imagePot;
        public Image imagePotFrame;

        public PotInfo(Transform transform)
        {
            trans = transform;
            if (null != trans)
            {
                imagePotFrame = trans.Find("Image_PotFrame").GetComponent<Image>();
                imagePot = trans.Find("Image_Pot").GetComponent<Image>();
                textPot = imagePotFrame.transform.Find("Text_Pot").GetComponent<Text>();
            }
        }
    }

    public partial class TexasGame : Entity
    {
        #region 个性设置

        public float time_02 = 0.16f;
        public float time_05 = 0.4f;



        private List<string> settingAbnames;

        public static string[] DeskTypeAbnames = new string[] { "reddesk.unity3d", "bluedesk.unity3d", "qingdesk.unity3d", "lanhuidesk.unity3d" };
        public static string[] DeskTypeObjnames = new string[] { "RedDesk", "BlueDesk", "QingDesk", "LanhuiDesk" };
        public static string[] DeskTypeSpritenames = new string[] { "texture_bg-02", "texture_bg_ls_01", "texture_bg_04", "texture_bg_03" };

        public static string[] ButtonTypeSpritenames = new string[] { "nav_button_bg", "nav_button_bg1", "nav_button_bg", "nav_button_bg1" };

        public static string[] CardTypeAbnames = new string[] { "ordinarycard.unity3d", "fourcolorcard.unity3d", "portraitcard.unity3d" };
        public static string[] CardTypeObjnames = new string[] { "OrdinaryCard", "FourColorCard", "PortraitCard" };

        #endregion


        public UITexasComponent texasCompoment;

        /// <summary>
        /// 当前游戏状态，0:倒计时中 1:游戏中 -2:等待开局 -1:其他状态
        /// </summary>
        public sbyte gamestatus;
        /// <summary>
        /// 大盲所在位置
        /// </summary>
        public sbyte bigIndex;
        /// <summary>
        /// 小盲所在位置
        /// </summary>
        public sbyte smallIndex;
        /// <summary>
        /// 庄家所在位置
        /// </summary>
        public sbyte bankerIndex;
        /// <summary>
        /// 当前操作玩家所在位置
        /// </summary>
        public sbyte operationID;
        /// <summary>
        /// 已发出公共牌
        /// </summary>
        public List<sbyte> cards;
        /// <summary>
        /// 大盲
        /// </summary>
        public int bigBlind;
        /// <summary>
        /// 小盲
        /// </summary>
        public int smallBlind;
        /// <summary>
        /// 底池数目
        /// </summary>
        public int alreadAnte;
        /// <summary>
        /// 房主的userID
        /// </summary>
        public int roomOwner;
        /// <summary>
        /// 房间的时间总长度（分钟）
        /// </summary>
        public int maxPlayTime;
        /// <summary>
        /// 是否开启控制带入 1 开启 0关闭
        /// </summary>
        public sbyte bControl;
        /// <summary>
        /// 用户带入房间的筹码
        /// </summary>
        public int nowChip;
        /// <summary>
        /// 房间剩余时间（秒）
        /// </summary>
        public int roomLeftTime;
        /// <summary>
        /// 暂停剩余时间，大于0则为暂停状态（秒）
        /// </summary>
        public int pauseLeftTime;
        /// <summary>
        /// 当前最小带入倍数
        /// </summary>
        public int currentMinRate;
        /// <summary>
        /// 当前最大带入倍数
        /// </summary>
        public int currentMaxRate;
        /// <summary>
        /// 当前操作玩家剩余时间（s）
        /// </summary>
        public int leftOperateTime;
        /// <summary>
        /// 玩家操作默认时间
        /// </summary>
        public int opTime;
        /// <summary>
        /// 结束时是否亮手牌 0不亮 1亮第一张 2亮第二张 3亮两张
        /// </summary>
        public int showCardsId;
        /// <summary>
        /// 前注
        /// </summary>
        public int groupBet;
        /// <summary>
        /// 申请带入等待剩余时间，大于0会在右上角显示倒计时
        /// </summary>
        public int leftSecs;
        /// <summary>
        /// 各分池的筹码数
        /// </summary>
        public List<int> pots;
        /// <summary>
        /// 当前最小可加注额，操作按钮上的加注额要用到
        /// </summary>
        public int minAnteNum;
        /// <summary>
        /// 是否可加注，与minAnteNum及剩余筹码联合判断是否显示加注按钮
        /// </summary>
        public int canRaise;
        /// <summary>
        /// 是否开启保险
        /// </summary>
        public int insurance;
        /// <summary>
        /// 1需要弹出选择补盲，0不需要弹出
        /// </summary>
        public int waitBlind;
        /// <summary>
        /// 用户信用额度
        /// </summary>
        public int leftCreditValue;
        /// <summary>
        /// 房间类型，1：普通房 2:战队有限时长局 3:战队无限时长局（现在没这种类型）
        /// </summary>
        public int roomType;
        /// <summary>
        /// 房间模式，0：普通房 1:积分房
        /// </summary>
        public int roomMode;
        /// <summary>
        /// 需要带入的筹码数，只有roomType==2时有效 （未使用）
        /// </summary>
        public int needBring;
        /// <summary>
        /// 是否在其他房间被托管
        /// </summary>
        public int isTrusted;
        /// <summary>
        /// 跟注额，操作按钮上使用
        /// </summary>
        public int callAmount;
        /// <summary>
        /// 当前玩家是不是在其他房间有留座的状态 1 有 0 无
        /// </summary>
        public int isKeptSeat;
        /// <summary>
        /// 花钻看牌玩家的的昵称
        /// </summary>
        public string seeCardPlayerName;
        /// <summary>
        /// 是否开启IP限制，1 开启 0关闭
        /// </summary>
        public int isIpRestrictions;
        /// <summary>
        /// 是否开启GPS限制，1 开启 0关闭
        /// </summary>
        public int isGPSRestrictions;
        /// <summary>
        /// 同步到同盟的id 未同步时为0
        /// </summary>
        public int tribeId;
        /// <summary>
        /// 该玩家是否在本牌局中带入过0 1 用于判断是否是否显示菜单中的提前离桌
        /// </summary>
        public int bringIn;
        /// <summary>
        /// 是否已提前离桌 1 开启 0关闭
        /// </summary>
        public int preLeave;
        /// <summary>
        /// 是否开启了观众语音 1 开启 0关闭
        /// </summary>
        public sbyte bSpectatorsVoice;
        /// <summary>
        /// APP的最新版本，如果当前app版本较小，则在牌桌中间显示升级提示
        /// </summary>
        public string ServerVersion;
        /// <summary>
        /// 建房是房主设置的提前离桌功能是否开启 1 开启 0 不开启
        /// </summary>
        public sbyte canPreLeave;
        /// <summary>
        /// 当前操作轮数，用户操作时会把当前轮数发过去，后台会较验是否是本轮的操作
        /// </summary>
        public int operationRound;
        /// <summary>
        /// 当前操作pot值，快捷下注使用
        /// </summary>
        public int potNumber;
        public int firstBet;  // 第一个差额
        public int lastPlayerBet;  // 上个玩家加注额

        public int round;//0第一回合

        public int callnum = 0;
        public int foldnum = 0;

        /// <summary>
        /// 是否定向同盟  0否  1是,定向同盟房间的延时，消耗钻石会不一样
        /// </summary>
        public sbyte isOrientationTribe;
        /// <summary>
        /// 服务费百分比 如10%则为10
        /// </summary>
        public int serviceFeePer;
        /// <summary>
        /// 是否开启社区分带入 1 开启 0关闭
        /// </summary>
        public sbyte bCreditControl;
        /// <summary>
        /// 0straddle成功， 1straddle失败，2不处理 （应该是以前的手动straddle模式用到，现在都是自动的）
        /// </summary>
        public int isCurStraddle;
        /// <summary>
        /// 自动弃牌
        /// </summary>
        public bool autoFold;
        /// <summary>
        /// 自动跟注
        /// </summary>
        public bool autoCall;
        /// <summary>
        /// 自动ALLIN
        /// </summary>
        public bool autoAllin;
        /// <summary>
        /// 自动看牌
        /// </summary>
        public bool autoCheck;
        /// <summary>
        /// 是否首次坐下
        /// </summary>
        public bool bFirstSit;
        /// <summary>
        /// 是否在牌局开始前站起
        /// </summary>
        public bool bStandBeforeStart;
        /// <summary>
        /// 是否之前输光了
        /// </summary>
        public bool bLoseAll;
        /// <summary>
        /// 游戏是否暂停中
        /// </summary>
        protected bool bPaused;
        /// <summary>
        /// 上一个玩家下注金额
        /// </summary>
        protected int lastCallAmount = 0;
        public bool IsPause
        {
            get
            {
                return bPaused;
            }
        }
        /// <summary>
        /// 缓存玩家自己操作
        /// </summary>
        private sbyte cacheMainPlayerStatus;

        protected Player mainPlayer;
        /// <summary>
        /// 服务器位置下标就是位置
        /// </summary>
        public List<Seat> listSeat;
        /// <summary>
        /// key:客户端seatId
        /// </summary>
        protected Dictionary<int, Seat> dicSeatOnlyClient;
        /// <summary>
        /// 最短上桌时间倒计时，单位S
        /// </summary>
        private int shortestTime;
        /// <summary>
        /// 是否最短上桌时间倒计时中
        /// </summary>
        protected bool bShortestTimeCounting;
        private float recordShortestTimeDeltaTime;

        /// <summary>
        /// 是否退出游戏
        /// </summary>
        protected bool isExit;

        protected bool noLeftOperateTime;

        /// <summary>
        /// 缓存坐下的客户端位置Id，用于查询其他房间托管
        /// </summary>
        private int cacheClientSeatId;

        /// <summary>
        /// 操作延时次数
        /// </summary>
        protected int delayCount;

        public List<Vector3> listDefaultPublicCardsLPos;

        /// <summary>
        /// 上一局庄家
        /// </summary>
        public sbyte lastBankerIndex;
        /// <summary>
        /// 缓存坐下SeatId
        /// </summary>
        protected sbyte cacheSitdownSeatId;
        /// <summary>
        /// 等待GPS
        /// </summary>
        public bool waittingGPSCallback;

        /// <summary>
        /// 马上完成公共牌动画
        /// </summary>
        protected bool stopUpdatePublicCardsAnimation;

        /// <summary>
        /// 等待翻牌结束，播放赢家牌型
        /// </summary>
        protected bool waittingUpdatePublicCardsAnimation;

        /// <summary>
        /// 已经Allin下发玩家手牌
        /// </summary>
        protected bool isAllinGetPlayerCards;

        /// <summary>
        /// 保险模式，三张公共牌后，没有保险可买，马上来了第四张公共牌 0默认 1首次收筹码并位移
        /// </summary>
        protected int fuck4thPCardByInsuranceState = 0;

        /// <summary>
        /// 是否正在播放大牌动画
        /// </summary>
        public bool isPlayingBigWinAnimation = false;

        #region Tweener

        protected Tweener tweenerResetSeatUIInfo;
        protected Sequence sequencePlayDealAnimation;
        protected Sequence sequencePlayRecyclingChipAnimation;
        protected Sequence sequencePlayFirstRecyclingChipAnimation;
        protected Sequence sequencePlayFirstRecyclingChipSubAnimation;
        protected Sequence sequencePlayFirstInsurance;
        protected Sequence sequenceUpdatePublicCards;
        protected Sequence sequencePlayEndPublicCardsAnimation;

        #endregion

        public Sequence GetSequencePlayDealAnimation()
        {
            return sequencePlayDealAnimation;
        }

        /// <summary>
        /// im chat
        /// </summary>
        protected UIChatComponent uiChat = null;
        protected float defenseTime;
        protected bool isCancelRecording;
        protected bool isRecording;

        #region 跑马灯游戏

        protected bool isInMarqueeGame = false;
        protected float[] arrMarqueeGameInterval = new float[] { 0.3f, 0.2f, 0.1f, 0.05f, 0.05f, 0.05f, 0.05f };
        protected int curMoveRound = 0;
        protected int maxMoveRound = 0;
        protected float curMarqueeGameTimer;
        protected int curMoveCount = 0;
        protected int maxMoveCount = 0;
        protected int curMarqueeGameSeatIndex;
        protected List<Seat> listMarqueeGameSeats;
        /// <summary>
        /// 跑马灯游戏参与的最少人数
        /// </summary>
        protected int minPlayerMarqueeGame = 4;

        protected string marqueeGameWinCoin;

        protected TweenerCore<Vector3, Path, PathOptions> marqueePath;
        protected Transform transKing;
        protected Animator animatorKing;
        protected Transform transPudao;
        protected Transform transYanlei;
        protected Transform transDaizi;
        protected Tweener tweenerDaizi;
        protected UnityArmatureComponent armatureDaizi;

        protected Vector3[] pathKing;

        #endregion


        public void Awake(Component cmp)
        {
            listSeat = new List<Seat>();
            dicSeatOnlyClient = new Dictionary<int, Seat>();
            recordShortestTimeDeltaTime = Time.time;
            Init(cmp);
        }

        /// <summary>         得到 当前 在场玩家     不包括自己    </summary>
        public List<Player> GetCurrentPlayers()
        {
            var tCurPlayers = new List<Player>();
            if (listSeat != null && listSeat.Count > 0)
            {
                var count = listSeat.Count;
                for (int i = 0; i < count; i++)
                {
                    if (listSeat[i].Player != null && listSeat[i].Player.Id > 0 && listSeat[i].Player.Id != GameCache.Instance.nUserId)
                    {
                        tCurPlayers.Add(listSeat[i].Player);
                    }
                }
            }
            return tCurPlayers;
        }

        public string GetPlayerPlayerNickName(int pUserId)
        {
            var list = GetCurrentPlayers();
            var count = list.Count;
            for (int i = 0; i <count; i++)
            {
                if (pUserId == list[i].Id)
                {
                    return list[i].nick;
                }
            }
            return "";
        }


        public virtual void Update()
        {
            if (bShortestTimeCounting)
            {
                if (Time.time - recordShortestTimeDeltaTime > 1f)
                {
                    recordShortestTimeDeltaTime = Time.time;
                    shortestTime -= 1;
                    if (shortestTime <= 0)
                    {
                        bShortestTimeCounting = false;
                        // 返回游戏
                        if (null != textCancelTrust)
                            textCancelTrust.text = CPErrorCode.LanguageDescription(10011);
                    }
                    else
                    {
                        // 返回游戏 10:20
                        if (null != textCancelTrust)
                            textCancelTrust.text = $"{CPErrorCode.LanguageDescription(10011)}({shortestTime / 60}:{(shortestTime % 60).ToString().PadLeft(2, '0')})";
                    }
                }
            }

            if (isInMarqueeGame)
            {
                // 跑马灯游戏中
                if (Time.time - this.curMarqueeGameTimer > arrMarqueeGameInterval[curMoveRound])
                {
                    curMarqueeGameTimer = Time.time;
                    this.curMoveCount++;
                    curMarqueeGameSeatIndex++;
                    if (curMoveCount > this.maxMoveCount - 1)
                    {
                        this.isInMarqueeGame = false;
                        // 跑马灯结果
                        this.armatureMarqueeGame.dragonAnimation.Play("the_winning", 2);
                        this.armatureMarqueeGame.AddDBEventListener(EventObject.COMPLETE, (type, eventObject) =>
                                                                    {
                                                                        this.armatureMarqueeGame.gameObject.SetActive(false);

                                                                        UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                                                                        {
                                                                                type = UIDialogComponent.DialogData.DialogType.Commit,
                                                                                title = string.Empty,
                                                                                content = marqueeGameWinCoin,
                                                                                contentCommit = CPErrorCode.LanguageDescription(10012),
                                                                                actionCommit = null,
                                                                        });
                                                                        //CPErrorCode.LanguageDescription(20078, new List<object>() { seatId.ToString(), content });
                                                                        // UIComponent.Instance.Toast(marqueeGameWinCoin);
                                                                    });
                    }
                    else
                    {
                        int mCount = this.listMarqueeGameSeats.Count;
                        if (this.curMarqueeGameSeatIndex > mCount - 1)
                            curMarqueeGameSeatIndex = 0;
                        curMoveRound = this.curMoveCount / mCount;
                        if (curMoveRound >= this.maxMoveRound)
                            curMoveRound = this.maxMoveRound - 1;

                        Seat mSeat = this.listMarqueeGameSeats[curMarqueeGameSeatIndex];
                        if (null != mSeat)
                        {
                            armatureMarqueeGame.transform.localPosition = GameUtil.ChangeToLocalPos(mSeat.Trans, armatureMarqueeGame.transform);
                        }
                    }
                }
            }
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            ETModel.Game.Hotfix.OnServerMes -= OnServerMsg;

            ClearAllData();

            KillAllTweener();

            // 离开牌局断开TCP
            if (null != CPGameSessionComponent.Instance)
            {
                CPGameSessionComponent.Instance.Dispose();
                CPGameSessionComponent.Instance = null;
                Game.Scene.RemoveComponent<CPGameSessionComponent>();
            }

            // 清空公共牌
            if (null != listCards)
                listCards.Clear();

            // 清空座位
            if (null != listSeat)
            {
                for (int i = 0; i < listSeat.Count; i++)
                {
                    if (null != listSeat[i])
                    {
                        listSeat[i].Dispose();
                    }
                }

                listSeat.Clear();
                listSeat = null;
            }

            // 清空座位(客户端标记)
            if (null != dicSeatOnlyClient)
            {
                dicSeatOnlyClient.Clear();
                dicSeatOnlyClient = null;
            }

            // 清空分池
            if (null != listPotInfo)
            {
                listPotInfo.Clear();
                listPotInfo = null;
            }

            // 清空玩家自己
            if (null != mainPlayer)
            {
                mainPlayer.Dispose();
                mainPlayer = null;
            }

            removeHandler();

            uiChat = null;

            // 卸载牌局内已加载过的ab
            if (null != settingAbnames && settingAbnames.Count > 0)
            {
                ResourcesComponent mResourcesComponent = Game.Scene.ModelScene.GetComponent<ResourcesComponent>();
                for (int i = 0, n = settingAbnames.Count; i < n; i++)
                {
                    mResourcesComponent.UnloadBundle(settingAbnames[i]);
                }
                settingAbnames.Clear();
                settingAbnames = null;
            }

            isInMarqueeGame = false;

            if (null != marqueePath)
            {
                if (marqueePath.IsPlaying())
                    marqueePath.Kill();
                marqueePath = null;
            }

            if (null != tweenerDaizi)
            {
                tweenerDaizi.Kill();
                tweenerDaizi = null;
            }
            
            if (null != armatureDaizi)
            {
                if (null != armatureDaizi.dragonAnimation)
                    armatureDaizi.dragonAnimation.Stop();
                armatureDaizi = null;
            }

            base.Dispose();
        }

        protected virtual void Init(Component comp)
        {
            texasCompoment = comp as UITexasComponent;
            InitUI(comp);
            registerHandler();
        }

        public virtual void OnShow(object obj)
        {
            UpdateRoom(obj);
        }

        #region UI

        protected ReferenceCollector rc;
        protected Button buttonAddBean;
        protected Button buttonAddChips;
        protected Button buttonRule;
        protected Button buttonCancelTrust;
        protected Button buttonCurSituation;
        protected Button buttonMarqueeGame;
        protected Button buttonMsg;
        protected Button buttonExit;
        protected Button buttonMenu;
        protected Button buttonNetline;
        protected Image imageMenuNetline;
        protected Button buttonOwner;
        protected Button buttonPreLeave;
        protected Button buttonReport;

        protected Button buttonSeeMorePublic;
        protected Button buttonSetting;
        protected Button buttonStandup;
        protected Button buttonStart;
        protected Button buttoninvite;
        protected Button buttonTrust;
        protected Button buttonVoice;
        protected Button buttonChat;
        protected Button buttonDelay;
        protected Transform bottomPanel;
        protected GameObject invitePanle;
        protected GameObject inviteImgPanle;
        protected Image imageInsurance;
        protected Image imageLogo;
        protected Image imageMask;
        protected Image imageMenuMask;
        protected Image imagePublicCard0;
        protected Image imagePublicCard1;
        protected Image imagePublicCard2;
        protected Image imagePublicCard3;
        protected Image imagePublicCard4;
        protected Image imageSeeMorePublicTips;
        protected Image imageSelectPublicCard0;
        protected Image imageSelectPublicCard1;
        protected Image imageSelectPublicCard2;
        protected Image imageSelectPublicCard3;
        protected Image imageSelectPublicCard4;
        protected Image imageSelectSeatTips;
        protected Image imageWaitForStartTips;
        protected Image imageNeedUpdateIcon;
        protected Image imageNeedUpdate;
        protected Image Image_MsgRedPoint;
        protected Transform transPots;
        protected Transform transPot;
        protected ReferenceCollector rcPokerSprite;
        protected ReferenceCollector rcChipSprite;
        protected Transform transSeat;
        protected Transform transSubMenu;
        protected Text textAlreadAnte;
        protected Text textCancelTrust;
        protected Text textNeedUpdate;
        protected Text textRoomInfo;
        protected Text textSeeMorePublic;
        protected Text textSeeMorePublicGold;
        protected Text textSeeMorePublicTips;
        protected Text textTotalBean;
        protected UIJackPotLabelComponent jackPotLabel;
        protected Button buttonWaitBlind;
        protected UnityArmatureComponent armatureBustCard;
        protected Transform transMarqueeGame;
        protected UnityArmatureComponent armatureMarqueeGame;

        public List<PublicCardInfo> listCards;
        protected List<PotInfo> listPotInfo;


        protected Vector3 buttonMenuDefaultV3 = Vector3.zero;
        protected Vector3 buttonCurSituationDefaultV3 = Vector3.zero;
        protected Vector3 buttonMarqueeGameDefaultV3 = Vector3.zero;
        protected Vector3 transSubMenuDefaultV3 = Vector3.zero;

        protected Vector3 bottomPanelV3 = Vector3.zero;

        //protected Vector3 buttonReportDefaultV3 = Vector3.zero;
        //protected Vector3 buttonDelayDefaultV3 = Vector3.zero;
        //protected Vector3 buttonChatDefaultV3 = Vector3.zero;
        //protected Vector3 buttonVoiceDefaultV3 = Vector3.zero;
        //protected Vector3 buttonSeeMorePublicDefaultV3 = Vector3.zero;

        protected Sprite spriteGoodNetwork;
        protected Sprite spriteNormalNetwork;
        protected Sprite spriteErrorNetwork;

        protected Button buttonTest;

        /// <summary>
        /// 更新状态 0没有更新 1更新安装包 2更新热更包
        /// </summary>
        protected int updateStatus = 0;

        protected virtual void InitUI(Component comp)
        {
            rc = comp.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            buttonAddBean = rc.Get<GameObject>("Button_AddBean").GetComponent<Button>();
            buttonAddChips = rc.Get<GameObject>("Button_AddChips").GetComponent<Button>();
            buttonCancelTrust = rc.Get<GameObject>("Button_CancelTrust").GetComponent<Button>();
            buttonCurSituation = rc.Get<GameObject>("Button_CurSituation").GetComponent<Button>();
            buttonMarqueeGame = rc.Get<GameObject>("Button_MarqueeGame").GetComponent<Button>();
            buttonMsg = rc.Get<GameObject>("Button_Msg").GetComponent<Button>();
            Image_MsgRedPoint = rc.Get<GameObject>("Image_MsgRedPoint").GetComponent<Image>();
            buttonExit = rc.Get<GameObject>("Button_Exit").GetComponent<Button>();
            buttonMenu = rc.Get<GameObject>("Button_Menu").GetComponent<Button>();
            buttonNetline = rc.Get<GameObject>("Button_Netline").GetComponent<Button>();
            imageMenuNetline = rc.Get<GameObject>("Image_MenuNetline").GetComponent<Image>();
            buttonOwner = rc.Get<GameObject>("Button_Owner").GetComponent<Button>();
            buttonRule = rc.Get<GameObject>("Button_Rule").GetComponent<Button>();
            buttonPreLeave = rc.Get<GameObject>("Button_PreLeave").GetComponent<Button>();
            buttonReport = rc.Get<GameObject>("Button_Report").GetComponent<Button>();
            buttonSeeMorePublic = rc.Get<GameObject>("Button_SeeMorePublic").GetComponent<Button>();
            buttonSetting = rc.Get<GameObject>("Button_Setting").GetComponent<Button>();
            buttonStandup = rc.Get<GameObject>("Button_Standup").GetComponent<Button>();
            buttonStart = rc.Get<GameObject>("Button_Start").GetComponent<Button>();
            invitePanle = rc.Get<GameObject>("UIShare");
            inviteImgPanle = rc.Get<GameObject>("UIShareImg");
            buttoninvite = rc.Get<GameObject>("Button_Share").GetComponent<Button>();
            buttonTrust = rc.Get<GameObject>("Button_Trust").GetComponent<Button>();
            buttonVoice = rc.Get<GameObject>("Button_Voice").GetComponent<Button>();
            buttonChat = rc.Get<GameObject>("Button_Chat").GetComponent<Button>();
            buttonDelay = rc.Get<GameObject>("Button_Delay").GetComponent<Button>();
            bottomPanel = rc.Get<GameObject>("Panel_bottom").transform;
            imageInsurance = rc.Get<GameObject>("Image_Insurance").GetComponent<Image>();
            imageLogo = rc.Get<GameObject>("Image_Logo").GetComponent<Image>();
            imageMask = rc.Get<GameObject>("Image_Mask").GetComponent<Image>();
            imageMenuMask = rc.Get<GameObject>("Image_MenuMask").GetComponent<Image>();
            imagePublicCard0 = rc.Get<GameObject>("Image_PublicCard0").GetComponent<Image>();
            imagePublicCard1 = rc.Get<GameObject>("Image_PublicCard1").GetComponent<Image>();
            imagePublicCard2 = rc.Get<GameObject>("Image_PublicCard2").GetComponent<Image>();
            imagePublicCard3 = rc.Get<GameObject>("Image_PublicCard3").GetComponent<Image>();
            imagePublicCard4 = rc.Get<GameObject>("Image_PublicCard4").GetComponent<Image>();
            imageSeeMorePublicTips = rc.Get<GameObject>("Image_SeeMorePublicTips").GetComponent<Image>();
            imageSelectPublicCard0 = rc.Get<GameObject>("Image_SelectPublicCard0").GetComponent<Image>();
            imageSelectPublicCard1 = rc.Get<GameObject>("Image_SelectPublicCard1").GetComponent<Image>();
            imageSelectPublicCard2 = rc.Get<GameObject>("Image_SelectPublicCard2").GetComponent<Image>();
            imageSelectPublicCard3 = rc.Get<GameObject>("Image_SelectPublicCard3").GetComponent<Image>();
            imageSelectPublicCard4 = rc.Get<GameObject>("Image_SelectPublicCard4").GetComponent<Image>();
            imageSelectSeatTips = rc.Get<GameObject>("Image_SelectSeatTips").GetComponent<Image>();
            imageWaitForStartTips = rc.Get<GameObject>("Image_WaitForStartTips").GetComponent<Image>();
            imageNeedUpdateIcon = rc.Get<GameObject>("Image_NeedUpdateIcon").GetComponent<Image>();
            imageNeedUpdate = rc.Get<GameObject>("Image_NeedUpdate").GetComponent<Image>();
            transPots = rc.Get<GameObject>("Pots").transform;
            transPot = rc.Get<GameObject>("Pot").transform;
            rcChipSprite = rc.Get<GameObject>("RC_ChipSprite").GetComponent<ReferenceCollector>();
            transSeat = rc.Get<GameObject>("Seat").transform;
            transSubMenu = rc.Get<GameObject>("SubMenu").transform;
            textAlreadAnte = rc.Get<GameObject>("Text_AlreadAnte").GetComponent<Text>();
            textCancelTrust = rc.Get<GameObject>("Text_CancelTrust").GetComponent<Text>();
            textNeedUpdate = rc.Get<GameObject>("Text_NeedUpdate").GetComponent<Text>();
            textRoomInfo = rc.Get<GameObject>("Text_RoomInfo").GetComponent<Text>();
            textSeeMorePublic = rc.Get<GameObject>("Text_SeeMorePublic").GetComponent<Text>();
            textSeeMorePublicGold = rc.Get<GameObject>("Text_SeeMorePublicGold").GetComponent<Text>();
            textSeeMorePublicTips = rc.Get<GameObject>("Text_SeeMorePublicTips").GetComponent<Text>();
            textTotalBean = rc.Get<GameObject>("Text_TotalBean").GetComponent<Text>();
            jackPotLabel = new UIJackPotLabelComponent(rc.Get<GameObject>("UIJackPotLabel"), 500);
            buttonWaitBlind = rc.Get<GameObject>("Button_WaitBlind").GetComponent<Button>();
            armatureBustCard = rc.Get<GameObject>("Armature_BustCard").GetComponent<UnityArmatureComponent>();

            transMarqueeGame = rc.Get<GameObject>("MarqueeGame").transform;
            armatureMarqueeGame = rc.Get<GameObject>("Armature_MarqueeGame").GetComponent<UnityArmatureComponent>();
            transKing = rc.Get<GameObject>("King").transform;
            animatorKing = transKing.GetComponent<Animator>();
            transPudao = rc.Get<GameObject>("Pudao").transform;
            transYanlei = rc.Get<GameObject>("Yanlei").transform;
            transDaizi = rc.Get<GameObject>("daizi").transform;
            armatureDaizi = transDaizi.GetComponent<UnityArmatureComponent>();

            buttonTest = rc.Get<GameObject>("btn_test").GetComponent<Button>();

            #region 公共牌数据(UI、Id)

            if (null == listCards)
                listCards = new List<PublicCardInfo>();
            if (listCards.Count > 0)
                listCards.Clear();
            listCards.Add(new PublicCardInfo()
            {
                cardId = -1,
                trans = imagePublicCard0.transform,
                imageCard = imagePublicCard0,
                imageSelect = imageSelectPublicCard0
            });
            listCards.Add(new PublicCardInfo()
            {
                cardId = -1,
                trans = imagePublicCard1.transform,
                imageCard = imagePublicCard1,
                imageSelect = imageSelectPublicCard1
            });
            listCards.Add(new PublicCardInfo()
            {
                cardId = -1,
                trans = imagePublicCard2.transform,
                imageCard = imagePublicCard2,
                imageSelect = imageSelectPublicCard2
            });
            listCards.Add(new PublicCardInfo()
            {
                cardId = -1,
                trans = imagePublicCard3.transform,
                imageCard = imagePublicCard3,
                imageSelect = imageSelectPublicCard3
            });
            listCards.Add(new PublicCardInfo()
            {
                cardId = -1,
                trans = imagePublicCard4.transform,
                imageCard = imagePublicCard4,
                imageSelect = imageSelectPublicCard4
            });

            #endregion

            // 公共牌默认位置
            if (null == listDefaultPublicCardsLPos)
                listDefaultPublicCardsLPos = new List<Vector3>();
            if (listDefaultPublicCardsLPos.Count > 0)
                listDefaultPublicCardsLPos.Clear();
            listDefaultPublicCardsLPos.Add(imagePublicCard0.transform.localPosition);
            listDefaultPublicCardsLPos.Add(imagePublicCard1.transform.localPosition);
            listDefaultPublicCardsLPos.Add(imagePublicCard2.transform.localPosition);
            listDefaultPublicCardsLPos.Add(imagePublicCard3.transform.localPosition);
            listDefaultPublicCardsLPos.Add(imagePublicCard4.transform.localPosition);

            // 分池UI
            if (null == listPotInfo)
                listPotInfo = new List<PotInfo>();

            UIEventListener.Get(imageMenuMask.gameObject).onClick = onClickMenuMask;
            UIEventListener.Get(buttonMenu.gameObject).onClick = onClickMenu;
            UIEventListener.Get(buttonVoice.gameObject).onDown = onDownVoice;
            UIEventListener.Get(buttonVoice.gameObject).onUp = onUpVoice;
            UIEventListener.Get(buttonVoice.gameObject).onEnter = onEnterVoice;
            UIEventListener.Get(buttonVoice.gameObject).onExit = onExitVoice;

            UIEventListener.Get(buttonChat.gameObject).onClick = onClickChat;
            UIEventListener.Get(buttonDelay.gameObject).onClick = onClickDelay;
            UIEventListener.Get(buttonCurSituation.gameObject).onClick = onClickCurSituation;
            UIEventListener.Get(buttonMarqueeGame.gameObject).onClick = onClickMarqueeGame;
            UIEventListener.Get(buttonMsg.gameObject).onClick = onClickMsg;
            UIEventListener.Get(buttonReport.gameObject).onClick = onClickReport;
            UIEventListener.Get(buttonOwner.gameObject).onClick = onClickOwner;
            UIEventListener.Get(buttonRule.gameObject).onClick = onClickRule;
            UIEventListener.Get(buttonSeeMorePublic.gameObject).onClick = onClickSeeMorePublic;
            UIEventListener.Get(buttonSetting.gameObject).onClick = onClickSetting;
            UIEventListener.Get(buttonStandup.gameObject).onClick = onClickStandup;
            UIEventListener.Get(buttonTrust.gameObject).onClick = onClickTrust;
            UIEventListener.Get(buttonNetline.gameObject).onClick = onClickNetline;
            UIEventListener.Get(buttonExit.gameObject).onClick = onClickExit;
            UIEventListener.Get(buttonStart.gameObject).onClick = onClickStart;
            UIEventListener.Get(buttonCancelTrust.gameObject).onClick = onClickCancelTrust;
            UIEventListener.Get(buttonAddBean.gameObject).onClick = onclickAddBean;
            UIEventListener.Get(buttonAddChips.gameObject).onClick = onClickAddChip;
            UIEventListener.Get(buttonPreLeave.gameObject).onClick = onClickPreLeave;
            GameObject mObjUIJackPotLabel = rc.Get<GameObject>("UIJackPotLabel");
            if (null != mObjUIJackPotLabel)
                UIEventListener.Get(mObjUIJackPotLabel).onClick = onClickJackPotLabel;
            UIEventListener.Get(buttonWaitBlind.gameObject).onClick = onClickWaitBlind;
            UIEventListener.Get(imageNeedUpdate.gameObject).onClick = onClickNeedUpdate;

            //添加，如过是测试账号，显示测试按钮
            UIEventListener.Get(buttonTest.gameObject).onClick = onClickTest;

            UIEventListener.Get(buttoninvite.gameObject).onClick = (go) =>
            {
                invitePanle.SetActive(true);
            };

            UIEventListener.Get(rc.Get<GameObject>("ui_btn_close")).onClick = (go) =>
            {
                invitePanle.SetActive(false);
            };

            UIEventListener.Get(rc.Get<GameObject>("ui_btn_text")).onClick = (go) =>
            {
                invitePanle.SetActive(false);
                string mang = $"{StringHelper.ShowGold(smallBlind)}/{StringHelper.ShowGold(bigBlind)}";
                if (groupBet > 0)
                {
                    mang += $"/{StringHelper.ShowGold(groupBet)}";
                }
                string time = string.Format(LanguageManager.Get("UITexasReport_Text_MatchZmsysj"), maxPlayTime);
                string pnum = LanguageManager.Get("UIMatchFilter_5MWvUt") + ":"+ listSeat.Count.ToString();
                string url = mang + "," + time + "," + pnum + ",ID:" + GameCache.Instance.room_id.ToString();
                UniClipboard.SetText(url);
                UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10305));
            };

            UIEventListener.Get(rc.Get<GameObject>("ui_btn_img")).onClick = (go) =>
            {
                invitePanle.SetActive(false);
                inviteImgPanle.SetActive(true);
                string mang = $"{StringHelper.ShowGold(smallBlind)}/{StringHelper.ShowGold(bigBlind)}";
                if (groupBet > 0)
                {
                    mang += $"/{StringHelper.ShowGold(groupBet)}";
                }
                string time = maxPlayTime.ToString() + "m";
                string pnum = listSeat.Count.ToString();

                

                rc.Get<GameObject>("UIShareImg_Title").GetComponent<Text>().text =  UIMatchModel.mInstance.GetRoomPathStr(GameCache.Instance.room_path);
                rc.Get<GameObject>("UIShareImg_name").GetComponent<Text>().text = string.Format(LanguageManager.Get("UIMatch_join33"), GameCache.Instance.roomName);
                rc.Get<GameObject>("UIShareImg_id").GetComponent<Text>().text = GameCache.Instance.room_id.ToString();
                rc.Get<GameObject>("UIShareImg_Blind").GetComponent<Text>().text = mang;
                rc.Get<GameObject>("UIShareImg_People").GetComponent<Text>().text = pnum;
                rc.Get<GameObject>("UIShareImg_Time").GetComponent<Text>().text = time;

            };

            UIEventListener.Get(rc.Get<GameObject>("UIShareImg_close")).onClick = (go) =>
            {
                inviteImgPanle.SetActive(false);
            };

            UIEventListener.Get(rc.Get<GameObject>("UIShareImg_img")).onClick = async (go) =>
            {
                await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(100);

                ETModel.Game.Hotfix.OnGetScreenShot = OnGetScreenShot;
                NativeManager.Instance.CaptureScreenshot();

                await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(1000);

                inviteImgPanle.SetActive(false);
            };

            #region 适配刘海屏

            NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
            float realTop = safeArea.top * 1242 / safeArea.width;
            float realBottom = safeArea.bottom * 1242 / safeArea.width;

            if (buttonMenuDefaultV3.Equals(Vector3.zero))
            {
                buttonMenuDefaultV3 = buttonMenu.transform.localPosition;
            }

            if (buttonCurSituationDefaultV3.Equals(Vector3.zero))
            {
                buttonCurSituationDefaultV3 = buttonCurSituation.transform.localPosition;
            }

            if (buttonMarqueeGameDefaultV3.Equals(Vector3.zero))
            {
                buttonMarqueeGameDefaultV3 = buttonMarqueeGame.transform.localPosition;
            }


            if (transSubMenuDefaultV3.Equals(Vector3.zero))
            {
                transSubMenuDefaultV3 = transSubMenu.localPosition;
            }

            if (bottomPanelV3.Equals(Vector3.zero))
            {
                bottomPanelV3 = bottomPanel.localPosition;
            }

            //if (buttonReportDefaultV3.Equals(Vector3.zero))
            //{
            //    buttonReportDefaultV3 = buttonReport.transform.localPosition;
            //}

            //if (buttonDelayDefaultV3.Equals(Vector3.zero))
            //{
            //    buttonDelayDefaultV3 = buttonDelay.transform.localPosition;
            //}

            //if (buttonChatDefaultV3.Equals(Vector3.zero))
            //{
            //    buttonChatDefaultV3 = buttonChat.transform.localPosition;
            //}

            //if (buttonVoiceDefaultV3.Equals(Vector3.zero))
            //{
            //    buttonVoiceDefaultV3 = buttonVoice.transform.localPosition;
            //}

            //if (buttonSeeMorePublicDefaultV3.Equals(Vector3.zero))
            //{
            //    buttonSeeMorePublicDefaultV3 = buttonSeeMorePublic.transform.localPosition;
            //}

            transSubMenu.transform.localPosition = new Vector3(transSubMenuDefaultV3.x, transSubMenuDefaultV3.y - realTop);

            bottomPanel.localPosition = new Vector3(bottomPanelV3.x, bottomPanelV3.y + realBottom);
            //buttonReport.transform.localPosition = new Vector3(buttonReportDefaultV3.x, buttonReportDefaultV3.y + realBottom);
            //buttonDelay.transform.localPosition = new Vector3(buttonDelayDefaultV3.x, buttonDelayDefaultV3.y + realBottom);
            //buttonChat.transform.localPosition = new Vector3(buttonChatDefaultV3.x, buttonChatDefaultV3.y + realBottom);
            //buttonVoice.transform.localPosition = new Vector3(buttonVoiceDefaultV3.x, buttonVoiceDefaultV3.y + realBottom);

            //buttonSeeMorePublic.transform.localPosition = new Vector3(buttonSeeMorePublicDefaultV3.x, buttonSeeMorePublicDefaultV3.y + realBottom);

            if (null != mObjUIJackPotLabel)
                mObjUIJackPotLabel.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);

            //长屏幕，如iPhoneX，要特殊布局一下
            if (Screen.width / (Screen.height * 1f) <= 0.48f)
            {
                float topGap = 160;
                if (realTop > topGap)
                    topGap = realTop;

                if (null != mObjUIJackPotLabel)
                    mObjUIJackPotLabel.GetComponent<RectTransform>().localPosition = new Vector2(0, -topGap);

                float buttonHeight = buttonMenu.gameObject.GetComponent<RectTransform>().sizeDelta.y;

                buttonMenu.transform.localPosition = new Vector3(buttonMenuDefaultV3.x, -buttonHeight * 0.5f - topGap);
                buttonCurSituation.transform.localPosition = new Vector3(buttonCurSituationDefaultV3.x, -buttonHeight * 0.5f - topGap);
                buttonMarqueeGame.transform.localPosition = new Vector3(buttonMarqueeGameDefaultV3.x, -buttonHeight * 0.5f - topGap);
                buttonMsg.transform.localPosition = new Vector3(buttonMarqueeGameDefaultV3.x, -buttonHeight * 0.5f - topGap);
                bottomPanel.localPosition = new Vector3(bottomPanelV3.x, bottomPanelV3.y + 100);
                //buttonDelay.transform.localPosition = new Vector3(buttonDelayDefaultV3.x, buttonReportDefaultV3.y + realBottom);
                //buttonVoice.transform.localPosition = new Vector3(buttonVoiceDefaultV3.x, buttonChatDefaultV3.y + realBottom);
            }

            #endregion

            //添加语音播放
            if (uiChat == null)
                uiChat = comp.GetParent<UI>().AddComponent<UIChatComponent>();

            #region 个性设置

            // 桌布
            int mTmpDeskType = PlayerPrefsMgr.mInstance.GetInt(PlayerPrefsKeys.SettingDeskType, 0);
            SetDeskType(mTmpDeskType);

            // 扑克牌
            int mTmpCardType = PlayerPrefsMgr.mInstance.GetInt(PlayerPrefsKeys.SettingCardType, 0);
            SetCardType(mTmpCardType);

            #endregion

            spriteGoodNetwork = rc.Get<Sprite>("leftbar_route_good_network");
            spriteNormalNetwork = rc.Get<Sprite>("leftbar_route_general_network");
            spriteErrorNetwork = rc.Get<Sprite>("leftbar_route_network_error");
        }

        private void OnGetScreenShot(Texture2D texture)
        {
            ETModel.Game.Hotfix.OnGetScreenShot = null;
            //转为字节数组
            byte[] bytes = texture.EncodeToJPG();
            if (Application.platform == RuntimePlatform.Android)
            {
                if (NativeManager.OnFuncCall<bool>("SaveImage", bytes, "allin 海报"))
                {
                    UIComponent.Instance.ToastLanguage("Become104");//("图片已保存到相册");
                }
                else
                {
                    UIComponent.Instance.ToastLanguage("Become105");//("图片保存失败");
                }
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                NativeManager.SaveImageToNative(bytes, bytes.Length);
            }
        }

        /// <summary>
        /// 初始化位置UI
        /// </summary>
        /// <param name="seatCount"></param>
        protected virtual void InitSeatByCount(int seatCount)
        {
            SeatUIInfo[] mInfos = GameUtil.SeatUIInfos[seatCount];
            for (int i = 0; i < seatCount; i++)
            {
                GameObject mGo = GameObject.Instantiate(transSeat.gameObject, transSeat.parent);
                mGo.name = $"Seat{i}";
                mGo.transform.localPosition = mInfos[i].Pos;
                mGo.transform.localRotation = Quaternion.identity;
                mGo.transform.localScale = Vector3.one;
                Seat mSeat = ComponentFactory.CreateWithId<Seat, Transform>(i, mGo.transform);
                mSeat.InitSeatUIInfo(mInfos[i]);
                listSeat.Add(mSeat);
                dicSeatOnlyClient.Add(mSeat.ClientSeatId, mSeat);
            }
        }

        private void onClickMenuMask(GameObject go)
        {
            hideMenu();
        }

        private async void onclickAddBean(GameObject go)
        {
            hideMenu();
            //商城
            UIMineModel.mInstance.ObtainUserInfo(pDto =>
            {
                if (pDto.wallet.status == 1)
                    UIMineModel.mInstance.GetTransferOpen(show =>
                    {
                        UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletMy, new object[] { pDto, show });
                    });
                else if (pDto.wallet.status == 2)
                {
                    var tLangs = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIWallet_ClubStop_D01");
                    UIMineModel.mInstance.SetDialogShow(tLangs, delegate
                    {
                        UIMineModel.mInstance.ShowSDKCustom();
                    });
                }
            });
            // await UIComponent.Instance.ShowAsyncNoAnimation(UIType.UIMine_WalletMy);
        }

        private void onClickAddChip(GameObject go)
        {
            if (null == buttonAddChips || !buttonAddChips.interactable)
            {
                return;
            }

            hideMenu();
            // 弹代入框
            UIComponent.Instance.Show(UIType.UIAddChips,
                                      new UIAddChipsComponent.AddClipsData()
                                      {
                                          qianzhu = groupBet,
                                          bigBlind = bigBlind,
                                          smallBlind = smallBlind,
                                          currentMinRate = currentMinRate,
                                          currentMaxRate = currentMaxRate,
                                          totalCoin = GameCache.Instance.gold,
                                          tableChips = mainPlayer.chips,
                                          isNeedCost = roomMode == 0 ? 1 : 0
                                      });
        }

        private void onClickJackPotLabel(GameObject go)
        {
            UIComponent.Instance.Show(UIType.UIJackPotInfo,
                                          new UIJackPotInfoComponent.JackPotInfoData()
                                          {
                                              roomPath = GameCache.Instance.room_path,
                                              roomId = GameCache.Instance.room_id
                                          });
        }

        private void onClickPreLeave(GameObject go)
        {
            if (null == buttonPreLeave || !buttonPreLeave.interactable)
            {
                return;
            }

            hideMenu();
            UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
            {
                type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                title = string.Empty,
                // content = $"提前离桌后，您将不能在此房间继续坐下游戏，是否继续？",
                content = CPErrorCode.LanguageDescription(20000),
                // contentCommit = "确定",
                contentCommit = CPErrorCode.LanguageDescription(10012),
                // contentCancel = "取消",
                contentCancel = CPErrorCode.LanguageDescription(10013),
                actionCommit = () =>
                {
                    CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_SEND_SEAT_ACTION()
                    {
                        action = 6,
                        seatID = mainPlayer.seatID,
                        longitude = GameCache.Instance.longitude,
                        latitude = GameCache.Instance.latitude,
                        clientIP = GameCache.Instance.client_ip,
                        roomPath = GameCache.Instance.room_path,
                        roomID = GameCache.Instance.room_id,
                    });
                },
                actionCancel = null
            });
        }

        private void onClickCancelTrust(GameObject go)
        {
            if (null == mainPlayer)
            {
                UIComponent.Instance.Toast($"Good Luck");
                Game.EventSystem.Run(EventIdType.GameErrorReconnect);
                return;
            }

            if (mainPlayer.trust == 0)
            {
                return;
            }

            SendTrustAction(0, 1);
        }

        private void onClickStart(GameObject go)
        {
            if (gamestatus != -2)
            {
                // Log.Debug($"游戏状态错误 gamestatus: {gamestatus}");
                UIComponent.Instance.Toast($"Good Luck");
                Game.EventSystem.Run(EventIdType.GameErrorReconnect);
                return;
            }

            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_START_GAME()
            {
                roomPath = GameCache.Instance.room_path,
                roomId = GameCache.Instance.room_id
            });
        }

        protected virtual void onClickExit(GameObject go)
        {
            isExit = false;
            hideMenu();
            if (buttonCancelTrust.gameObject.activeInHierarchy)
            {
                CallbackExit();
                return;
            }
            if (gamestatus == -2)
            {
                CallbackExit();
            }
            else
            {
                Seat mSeat = GetSeatByUserId(mainPlayer.userID);
                if (null == mSeat)
                {
                    CallbackExit();
                }
                else
                {
                    if (shortestTime > 0 && mainPlayer.status != 15)
                    {
                        // 最短上桌时间
                        string mTime = TimeHelper.ShowRemainingSemicolon(shortestTime);
                        UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                        {
                            type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                            // title = $"上桌时间不满{((float)GameCache.Instance.shortest_time / 60.0f).ToString("F1")}小时",
                            title = CPErrorCode.LanguageDescription(20001, new List<object>() { (GameCache.Instance.shortest_time / 60.0f).ToString("F1") }),
                            // content = $"您还需要继续打牌{mTime}，或者选择留盲代打的托管模式",
                            content = CPErrorCode.LanguageDescription(20002, new List<object>() { mTime }),
                            // contentCommit = "托管模式",
                            contentCommit = CPErrorCode.LanguageDescription(10014),
                            // contentCancel = "继续打牌",
                            contentCancel = CPErrorCode.LanguageDescription(10015),
                            actionCommit = () =>
                            {
                                SendTrustAction(1, 1);
                                isExit = true;
                            },
                            actionCancel = null
                        });
                    }
                    else if (mainPlayer.isPlaying)
                    {

                        UIComponent.Instance.ShowNoAnimation(UIType.UIDialog,
                                                             new UIDialogComponent.DialogData()
                                                             {
                                                                 type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                                                                 title = "",
                                                                 // content = "退出游戏，在这手牌结束后将自动站起",
                                                                 content = CPErrorCode.LanguageDescription(20003),
                                                                 // contentCommit = "确定",
                                                                 contentCommit = CPErrorCode.LanguageDescription(10012),
                                                                 // contentCancel = "取消",
                                                                 contentCancel = CPErrorCode.LanguageDescription(10013),
                                                                 actionCommit = () => { CallbackExit(); },
                                                                 actionCancel = null
                                                             });
                    }
                    else
                    {
                        CallbackExit();
                    }
                }
            }
        }

        public void CallbackExit(sbyte exitAction = 3)
        {
            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_SEND_SEAT_ACTION()
            {
                action = exitAction,
                seatID = mainPlayer.seatID,
                longitude = GameCache.Instance.longitude,
                latitude = GameCache.Instance.latitude,
                clientIP = GameCache.Instance.client_ip,
                roomPath = GameCache.Instance.room_path,
                roomID = GameCache.Instance.room_id,
            });
        }

        /// <summary>
        /// 退出房间
        /// </summary>
        public virtual void doExitRoom()
        {
            if (null != CPGameSessionComponent.Instance)
                CPGameSessionComponent.Instance.HotfixSession.RemoveComponent<CPGameHeartbeatComponent>();
            if (null != Game.Scene.GetComponent<CPGameSessionComponent>())
                Game.Scene.RemoveComponent<CPGameSessionComponent>();
            UIComponent.Instance.Remove(UIType.UITexas);

            UIComponent.Instance.Remove(UIType.UIOwner);//强制关闭 页面
            UIComponent.Instance.Remove(UIType.UIInsurance);
            UIComponent.Instance.Remove(UIType.UITexasHistory);
            UIComponent.Instance.Remove(UIType.UITexasReport);
            UIComponent.Instance.Remove(UIType.UITexasInfo);
            UIComponent.Instance.Remove(UIType.UIUserRemarks);
            UIComponent.Instance.Remove(UIType.UITexasRule);
            UIComponent.Instance.Remove(UIType.UITexasSetting);
            UIComponent.Instance.Remove(UIType.UIJackPotInfo);
            UIComponent.Instance.Remove(UIType.UITexasComplaint);
            UIComponent.Instance.Remove(UIType.UITexasMarqueeGame);
            UIComponent.Instance.Remove(UIType.UITexasReportMTT);
        }

        /// <summary>
        /// 站起
        /// </summary>
        /// <param name="seatID"></param>
        protected void doStandUp(sbyte seatID)
        {
            if (!HasStarted())
            {
                bStandBeforeStart = true;
            }
            Seat mSeat = GetSeatById(mainPlayer.seatID != seatID ? mainPlayer.seatID : seatID);
            if (null != mSeat)
            {
                mainPlayer.ClearGameData();
                mSeat.Player = null;
                mSeat.FsmLogicComponent.SM.ChangeState(SeatStandupAnimation<Entity>.Instance);
            }

            UI mUIAddChips = UIComponent.Instance.Get(UIType.UIAddChips);
            if (null != mUIAddChips && mUIAddChips.GameObject.activeInHierarchy)
            {
                UIComponent.Instance.HideNoAnimation(UIType.UIAddChips);
            }

            HideOperationPanel();
            HideAutoOperationPanel();
            HideWaitBlindBtn();
            HideCancelTrustBtn();
        }


        private void onClickNetline(GameObject go)
        {
            hideMenu();
            UIComponent.Instance.ShowNoAnimation(UIType.UILogin_Line);
        }

        private void onClickSetting(GameObject go)
        {
            hideMenu();
            UIComponent.Instance.ShowNoAnimation(UIType.UITexasSetting);
        }

        private void onClickTest(GameObject go)
        {
            if (GameCache.Instance.playerflag == 0)
                return;

            //查询测试数据
            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_ASK_TEST()
            {
                roomId = GameCache.Instance.room_id,
                roomPath = GameCache.Instance.room_path,
            });
            
          
        }

        private void onClickTrust(GameObject go)
        {
            if (!buttonTrust.interactable)
            {
                return;
            }

            hideMenu();

            if (null == mainPlayer)
            {
                UIComponent.Instance.Toast($"Good Luck");
                Game.EventSystem.Run(EventIdType.GameErrorReconnect);
                return;
            }

            if (mainPlayer.trust == 1)
                return;

            SendTrustAction(1, 1);
        }

        private void onClickStandup(GameObject go)
        {
            hideMenu();

            if (null == mainPlayer)
            {
                UIComponent.Instance.Toast($"Good Luck");
                Game.EventSystem.Run(EventIdType.GameErrorReconnect);
                return;
            }

            if (shortestTime > 0 && mainPlayer.status != 15)
            {
                // 最短上桌时间
                string mTime = TimeUtilHandle.Instance.ParseSecond(shortestTime);
                UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                {
                    type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                    // title = $"上桌时间不满{((float)GameCache.Instance.shortest_time / 60.0f).ToString("F1")}小时",
                    title = CPErrorCode.LanguageDescription(20001, new List<object>() { (GameCache.Instance.shortest_time / 60.0f).ToString("F1") }),
                    // content = $"您还需要继续打牌{mTime}，或者选择留盲代打的托管模式",
                    content = CPErrorCode.LanguageDescription(20002, new List<object>() { mTime }),
                    // contentCommit = "托管模式",
                    contentCommit = CPErrorCode.LanguageDescription(10014),
                    // contentCancel = "继续打牌",
                    contentCancel = CPErrorCode.LanguageDescription(10015),
                    actionCommit = () =>
                    {
                        SendTrustAction(1, 1);
                    },
                    actionCancel = null
                });
            }
            else if (mainPlayer.isPlaying)
            {

                UIComponent.Instance.ShowNoAnimation(UIType.UIDialog,
                                                     new UIDialogComponent.DialogData()
                                                     {
                                                         type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                                                         title = "",
                                                                 // content = "退出游戏，在这手牌结束后将自动站起",
                                                                 content = CPErrorCode.LanguageDescription(20003),
                                                                 // contentCommit = "确定",
                                                                 contentCommit = CPErrorCode.LanguageDescription(10012),
                                                                 // contentCancel = "取消",
                                                                 contentCancel = CPErrorCode.LanguageDescription(10013),
                                                         actionCommit = () => { Standup(); },
                                                         actionCancel = null
                                                     });
            }
            else
            {
                Standup();
            }

        }

        private void onClickRule(GameObject go)
        {
            UIComponent.Instance.ShowNoAnimation(UIType.UITexasRule);
            hideMenu();
        }

        private void onClickSeeMorePublic(GameObject go)
        {
            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_SEE_MORE_PUBLIC_ACTION()
            {
                recvCardsNum = (sbyte)GetCurPublicCardsCount(),
                roomPath = GameCache.Instance.room_path,
                roomID = GameCache.Instance.room_id
            });
        }

        private void onClickOwner(GameObject go)
        {
            Game.Scene.GetComponent<UIComponent>().Show(UIType.UIOwner,
                                                        new UIOwnerComponent.OwnerData()
                                                        {
                                                            roomPath = GameCache.Instance.room_path,
                                                            roomId = GameCache.Instance.room_id,
                                                            currentMinRate = currentMinRate,
                                                            currentMaxRate = currentMaxRate,
                                                            muck_switch = GameCache.Instance.muck_switch,
                                                            spectatorsVoice = bSpectatorsVoice,
                                                            straddle = GameCache.Instance.straddle
                                                        }, null, 0);

            hideMenu();
        }

        private void onDownVoice(GameObject go)
        {
            if (defenseTime != 0 && Time.time - defenseTime < 1)
                return;
            defenseTime = Time.time;

            if (mainPlayer.seatID == -1 && bSpectatorsVoice == 0)
            {
                // UIComponent.Instance.Toast("观众无法发送语音");
                UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(20004));
                return;
            }

            isCancelRecording = false;
            isRecording = uiChat.StartRecord();
            if (isRecording)
            {
                UIComponent.Instance.ShowNoAnimation(UIType.UIVoice);
            }
        }

        private void onUpVoice(GameObject go)
        {
            if (isRecording)
            {
                UIComponent.Instance.HideNoAnimation(UIType.UIVoice);
                if (uiChat.StopRecord(!isCancelRecording) <= 0)
                {
                    if (!isCancelRecording)
                        // UIComponent.Instance.Toast("录音时间太短");
                        UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10017));
                }
                isRecording = false;
            }
        }

        private void onEnterVoice(GameObject go)
        {
            if (!isRecording)
                return;
            isCancelRecording = false;
            UIComponent.Instance.ShowNoAnimation(UIType.UIVoice, new UIVoiceComponent.VoiceData() { isCancel = false, isContinue = true });
        }

        private void onExitVoice(GameObject go)
        {
            if (!isRecording)
                return;
            isCancelRecording = true;
            UIComponent.Instance.ShowNoAnimation(UIType.UIVoice, new UIVoiceComponent.VoiceData() { isCancel = true, isContinue = true });
        }

        private void onClickChat(GameObject go)
        {
            if (null == mainPlayer)
            {
                UIComponent.Instance.Toast($"Good Luck");
                Game.EventSystem.Run(EventIdType.GameErrorReconnect);
                return;
            }

            if (mainPlayer.seatID < 0)
            {
                // UIComponent.Instance.Toast("观众无法发送表情");
                UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10016));
                return;
            }
            UIComponent.Instance.ShowNoAnimation(UIType.UIEmojiPanel);
        }

        public void SendEmoji(string name)
        {
            if (null != uiChat)
                uiChat.SendEmojiMsg(name);
            //return;
            //测试jackPot
            //Seat mSeat = GetSeatById(1);
            //UIAnimojiComponent animojiComponent = UIComponent.Instance.Get(UIType.UIAnimoji).UiBaseComponent as UIAnimojiComponent;
            //animojiComponent.PlayJackpotHitAnimation(mSeat, rc.Get<GameObject>("UIJackPotLabel").transform.localPosition);
            //测试emoji
            //onReceiveEmoji(name, GameCache.Instance.nUserId.ToString());
            //测试Animoji
            //Seat mSendSeat = GetSeatById(0);
            //Seat mReceiveSeat = GetSeatById(6);
            //UIAnimojiComponent animojiComponent = UIComponent.Instance.Get(UIType.UIAnimoji).UiBaseComponent as UIAnimojiComponent;
            //animojiComponent.PlayAnimoji("shayu", mSendSeat, mReceiveSeat);
        }

        public void SendAnimoji(string name, string receiver)
        {
            if (null != uiChat)
                uiChat.SendAnimojiMsg(name, receiver);
            //测试Animoji
            //Seat mSendSeat = GetSeatById(0);
            //Seat mReceiveSeat = GetSeatById(2);
            //Seat mReceiveSeat = GetSeatByUserId(Convert.ToInt32(receiver));
            //UIAnimojiComponent animojiComponent = UIComponent.Instance.Get(UIType.UIAnimoji).UiBaseComponent as UIAnimojiComponent;
            //animojiComponent.PlayAnimoji(name, mSendSeat, mReceiveSeat);
        }          public void onReceiveEmoji(string name, string sender)         {             Seat mSeat = GetSeatByUserId(Convert.ToInt32(sender));             if (null != mSeat && mSeat.seatID > -1)             {                 mSeat.ShowEmoji(name);             }         }          public void onReceiveAnimoji(string name, string sender, string receiver)         {
            Seat mSendSeat = GetSeatByUserId(Convert.ToInt32(sender));
            Seat mReceiveSeat = GetSeatByUserId(Convert.ToInt32(receiver));
            if (null != mSendSeat && mSendSeat.seatID > -1 && null != mReceiveSeat && mReceiveSeat.seatID > -1)
            {
                UIAnimojiComponent animojiComponent = UIComponent.Instance.Get(UIType.UIAnimoji).UiBaseComponent as UIAnimojiComponent;
                animojiComponent.PlayAnimoji(name, mSendSeat, mReceiveSeat);
            }
        }

        public void onHitJackPot(Seat seat)
        {
            UI mUI = UIComponent.Instance.Get(UIType.UIAnimoji);
            if (null != mUI && !isPlayingBigWinAnimation)//播放大牌动画时，已自带JP击中特效，不用播放
            {
                UIAnimojiComponent mUIAnimojiComponent = mUI.UiBaseComponent as UIAnimojiComponent;
                if (null != mUIAnimojiComponent)
                {
                    GameObject mObjUIJackPotLabel = rc.Get<GameObject>("UIJackPotLabel");
                    if (null != mObjUIJackPotLabel)
                        mUIAnimojiComponent.PlayJackpotHitAnimation(seat, mObjUIJackPotLabel.transform.localPosition);
                }
            }
        }

        protected virtual void onClickDelay(GameObject go)
        {
            Game.Scene.GetComponent<CPGameSessionComponent>().HotfixSession.Send(new REQ_ADD_TIME()
            {
                roomID = GameCache.Instance.room_id,
                roomPath = GameCache.Instance.room_path,
                time = 20,
                diamonds = 0,
                seatId = GameCache.Instance.CurGame.MainPlayer.seatID,
                coins = AddTimeCost()
            });
        }

        protected virtual int AddTimeCost()
        {
            return Convert.ToInt32(Math.Pow(2, delayCount + 1));
        }

        private void onClickMenu(GameObject go)
        {
            showMenu();
        }

        private void showMenu()
        {
            UpdateMenu();
            RectTransform mRectTransform = transSubMenu as RectTransform;
            if (null != mRectTransform)
                mRectTransform.DOAnchorPosX(0, 0.25f);
            if (null != imageMenuMask)
                imageMenuMask.gameObject.SetActive(true);
        }

        protected virtual void hideMenu()
        {
            RectTransform mRectTransform = transSubMenu as RectTransform;
            if (null != mRectTransform)
                mRectTransform.DOAnchorPosX(-500, 0.25f);
            if (null != imageMenuMask)
                imageMenuMask.gameObject.SetActive(false);
        }

        protected virtual void UpdateMenu()
        {
            //更新USDT
            textTotalBean.text = StringHelper.GetShortString(GameCache.Instance.gold);
            float menuHeight = 1365f;
            if (roomOwner == GameCache.Instance.nUserId)
            {
                //房主
                buttonOwner.gameObject.SetActive(true);
            }
            else
            {
                //非房主
                buttonOwner.gameObject.SetActive(false);
                menuHeight -= 129;
            }

            if (UserSitdown()) //已坐下
            {
                buttonStandup.gameObject.SetActive(true);

                buttonAddChips.gameObject.SetActive(true);
                if (mainPlayer.chips >= GameCache.Instance.carry_small * (currentMaxRate + 1))
                {
                    //已带入最大值,不可点击
                    buttonAddChips.interactable = false;
                }
                else
                {
                    buttonAddChips.interactable = true;
                }

                buttonTrust.gameObject.SetActive(true);
                buttonTrust.interactable = !buttonCancelTrust.gameObject.activeInHierarchy;

            }
            else //未坐下
            {
                buttonStandup.gameObject.SetActive(false);
                menuHeight -= 129;

                buttonAddChips.gameObject.SetActive(false);
                menuHeight -= 129;

                buttonTrust.gameObject.SetActive(false);
                menuHeight -= 129;
            }

            //提前离桌
            if (canPreLeave == 1)
            {
                buttonPreLeave.gameObject.SetActive(true);
                if ((bringIn == 1) && (preLeave == 0) && (shortestTime <= 0) && (canPreLeave == 1))
                {
                    buttonPreLeave.interactable = true;
                }
                else
                {
                    buttonPreLeave.interactable = false;
                }
            }
            else
            {
                buttonPreLeave.gameObject.SetActive(false);
                menuHeight -= 129;
            }

            //线路
            buttonNetline.transform.Find("Text").GetComponent<Text>().text = GlobalData.Instance.NameForServerID(GlobalData.Instance.CurrentUsingServerID());

            RectTransform mRectTransform = transSubMenu as RectTransform;
            if (null != mRectTransform)
                mRectTransform.sizeDelta = new Vector2(mRectTransform.sizeDelta.x, menuHeight);
        }

        /// <summary>
        /// 更新网络延迟
        /// </summary>
        /// <param name="delay"></param>
        public virtual void UpdateNetworkDelay(float delay)
        {
            if (delay <= 0.140)
            {
                // imageMenuNetline.sprite = rc.Get<Sprite>("leftbar_route_good_network");
                imageMenuNetline.sprite = spriteGoodNetwork;
            }
            else if (delay <= 0.220)
            {
                // imageMenuNetline.sprite = rc.Get<Sprite>("leftbar_route_general_network");
                imageMenuNetline.sprite = spriteNormalNetwork;
            }
            else
            {
                // imageMenuNetline.sprite = rc.Get<Sprite>("leftbar_route_network_error");
                imageMenuNetline.sprite = spriteErrorNetwork;
            }
        }

        protected virtual void onClickReport(GameObject go)
        {
            // Game.Scene.GetComponent<NetworkDetectionComponent>().Reconnect();
            // return;

            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_REAL_TIME()
            {
                roomID = GameCache.Instance.room_id,
                roomPath = GameCache.Instance.room_path,
            });
        }

        protected virtual void onClickCurSituation(GameObject go)
        {
            UIComponent.Instance.ShowNoAnimation(UIType.UITexasHistory, new UITexasHistoryComponent.HistoryInfoData()
            {
                bInsurance = insurance == 1,
                bJackPot = GameCache.Instance.jackPot_on == 1,
                rcPokerSprite = rcPokerSprite
            });
        }

        private void onClickMsg(GameObject go)
        {
            UIComponent.Instance.ShowNoAnimation(UIType.UIMine_MsgNormal, new object[2] { EnumMSG.MSG_Club, EnumMSGFROM.MSGFROM_Game});
            Image_MsgRedPoint.gameObject.SetActive(false);
        }

        private int tmpRunIndex = 0;

        private void onClickMarqueeGame(GameObject go)
        {
            // PlayKingRun(tmpRunIndex);
            // tmpRunIndex++;
            // if (tmpRunIndex >= this.listSeat.Count)
            //     this.tmpRunIndex = 0;
            // return;

            // PlayMarqueeGameBySeatId((sbyte)tmpRunIndex, "500");
            // tmpRunIndex++;
            // if (tmpRunIndex >= this.listSeat.Count)
            // this.tmpRunIndex = 0;
            // return;

            HttpRequestComponent.Instance.Send(WEB2_raffle_query.API,
                                               WEB2_raffle_query.Request(new WEB2_raffle_query.RequestData()), json =>
                                               {
                                                   var tDto = WEB2_raffle_query.Response(json);
                                                   if (tDto.status == 0)
                                                   {
                                                       UIComponent.Instance.ShowNoAnimation(UIType.UITexasMarqueeGame, tDto.data);
                                                   }
                                               });
        }
        #endregion

        #region Logic

        #region Private

        /// <summary>
        /// 播放发牌动画
        /// </summary>
        protected virtual void PlayDealAnimation(TweenCallback tweenCallback)
        {
            sequencePlayDealAnimation = DOTween.Sequence();

            Seat mSeat = null;

            // 庄家标志动画
            mSeat = GetSeatById(bankerIndex);
            if (null != mSeat)
            {
                Tweener mTweener = mSeat.PlayBankerAnimation();
                if (null != mTweener)
                {
                    sequencePlayDealAnimation.Append(mTweener);
                    sequencePlayDealAnimation.AppendInterval(time_02);
                }
            }

            // 前注
            if (groupBet > 0)
            {
                int allGroupBet = 0;
                bool mIsFirstGroupBet = true;
                for (int i = 0, n = listSeat.Count; i < n; i++)
                {
                    mSeat = listSeat[i];
                    if (null == mSeat || null == mSeat.Player || mSeat.Player.canPlayStatus == 0)
                        continue;
                    mSeat.UpdateGroupBet();
                    allGroupBet += groupBet;
                    if (mIsFirstGroupBet)
                    {
                        mIsFirstGroupBet = false;
                        sequencePlayDealAnimation.Append(mSeat.PlayBetAnimation());
                    }
                    else
                    {
                        sequencePlayDealAnimation.Join(mSeat.PlayBetAnimation());
                    }
                }

                mIsFirstGroupBet = true;
                for (int i = 0, n = listSeat.Count; i < n; i++)
                {
                    mSeat = listSeat[i];
                    if (null == mSeat || null == mSeat.Player || mSeat.Player.canPlayStatus == 0)
                        continue;
                    if (mIsFirstGroupBet)
                    {
                        mIsFirstGroupBet = false;

                        sequencePlayDealAnimation.Append(mSeat.PlayRecyclingChipAnimation().OnStart(() => { }));
                    }
                    else
                    {
                        sequencePlayDealAnimation.Join(mSeat.PlayRecyclingChipAnimation());
                    }
                }

                GameObject mObj = null;
                PotInfo mPotInfo = null;
                if (listPotInfo.Count == 0)
                {
                    mObj = GameObject.Instantiate(transPot.gameObject);
                    mObj.transform.SetParent(transPots);
                    mObj.transform.localPosition = Vector3.zero;
                    mObj.transform.localRotation = Quaternion.identity;
                    mObj.transform.localScale = Vector3.one;
                    mObj.name = $"Pot{0}";

                    mPotInfo = new PotInfo(mObj.transform);
                    listPotInfo.Add(mPotInfo);
                }
                else
                {
                    mPotInfo = listPotInfo[0];
                }

                mPotInfo.imagePot.sprite = rcChipSprite.Get<Sprite>(GameUtil.GetChipSpriteName(allGroupBet));
                mPotInfo.textPot.text = StringHelper.GetShortString(allGroupBet);
                float mFrameWidth = mPotInfo.textPot.preferredWidth + mPotInfo.imagePot.rectTransform.sizeDelta.x + 10f;
                mPotInfo.imagePotFrame.rectTransform.sizeDelta = new Vector2(0, mPotInfo.imagePotFrame.rectTransform.sizeDelta.y);
                mPotInfo.imagePot.transform.localPosition = Vector3.zero;

                mPotInfo.trans.localPosition = GameUtil.ChangeToLocalPos(textAlreadAnte.transform.localPosition, textAlreadAnte.rectTransform.parent, mPotInfo.trans.parent);

                sequencePlayDealAnimation.Append(mPotInfo.imagePotFrame.rectTransform.DOSizeDelta(new Vector2(mFrameWidth, mPotInfo.imagePotFrame.rectTransform.sizeDelta.y), 0.5f).OnStart(() =>
                {
                    mPotInfo.trans.gameObject.SetActive(true);
                }));
                sequencePlayDealAnimation.Join(mPotInfo.imagePotFrame.rectTransform.DOLocalMoveX(-mFrameWidth / 2f, time_05));
                sequencePlayDealAnimation.Join(mPotInfo.imagePot.rectTransform.DOLocalMoveX(-mFrameWidth / 2f, time_05));
                sequencePlayDealAnimation.Append(mPotInfo.trans.DOLocalMoveY(0, 0.3f));
            }

            // 从小盲位置开始发牌
            Vector3 mStartPos = rc.transform.TransformPoint(Vector3.zero);
            bool mIsFirst = true;
            int mTmpIndex = 0;
            for (int i = smallIndex, n = listSeat.Count; i < n; i++)
            {
                mSeat = listSeat[i];
                if (null == mSeat || null == mSeat.Player || mSeat.Player.canPlayStatus == 0)
                    continue;

                if (mIsFirst)
                {
                    mIsFirst = false;
                    sequencePlayDealAnimation.Append(i == smallIndex ? listSeat[i].PlayDealAnimation(mStartPos)
                                                           : listSeat[i].PlayDealAnimation(mStartPos).SetDelay(time_02 * mTmpIndex));
                }
                else
                {
                    sequencePlayDealAnimation.Join(i == smallIndex ? listSeat[i].PlayDealAnimation(mStartPos)
                                                           : listSeat[i].PlayDealAnimation(mStartPos).SetDelay(time_02 * mTmpIndex));
                }

                mTmpIndex++;    // 发牌时间间隔
            }

            for (int i = 0, n = smallIndex; i < n; i++)
            {
                if (i >= listSeat.Count)
                    break;

                mSeat = listSeat[i];
                if (null == mSeat || null == mSeat.Player || mSeat.Player.canPlayStatus == 0)
                    continue;

                sequencePlayDealAnimation.Join(listSeat[i].PlayDealAnimation(mStartPos).SetDelay(time_02 * mTmpIndex));

                mTmpIndex++;    // 发牌时间间隔
            }

            if (null != tweenCallback)
                // sequencePlayDealAnimation.AppendCallback(tweenCallback);
                sequencePlayDealAnimation.OnComplete(tweenCallback);

            sequencePlayDealAnimation.Play();
        }

        /// <summary>
        /// 播放收筹码到底池动画
        /// </summary>
        private void PlayRecyclingChipAnimation(TweenCallback tweenCallback)
        {
            Seat mSeat = null;
            sequencePlayRecyclingChipAnimation = null;
            for (int i = 0, n = listSeat.Count; i < n; i++)
            {
                mSeat = listSeat[i];
                if (null == mSeat || null == mSeat.Player || mSeat.Player.canPlayStatus == 0)
                    continue;
                sequencePlayRecyclingChipAnimation = mSeat.PlayRecyclingChipAnimation(0);
            }

            if (null != sequencePlayRecyclingChipAnimation)
            {

                // sequencePlayRecyclingChipAnimation.AppendCallback(() =>
                // {
                //     UpdatePots();
                // });
                // if (null != tweenCallback)
                //     sequencePlayRecyclingChipAnimation.AppendCallback(tweenCallback);

                if (null != tweenCallback)
                {
                    sequencePlayRecyclingChipAnimation.OnComplete(() =>
                    {
                        UpdatePots();
                        tweenCallback();
                    });
                }
                else
                {
                    sequencePlayRecyclingChipAnimation.OnComplete(UpdatePots);
                }

                sequencePlayRecyclingChipAnimation.Play();
            }
            else
            {
                UpdatePots();
                if (null != tweenCallback)
                    tweenCallback();
            }


        }

        /// <summary>
        /// 播放首次收筹码到底池动画
        /// </summary>
        private Sequence PlayFirstRecyclingChipAnimation(TweenCallback tweenCallback)
        {
            fuck4thPCardByInsuranceState = 1;
            Seat mSeat = null;
            sequencePlayFirstRecyclingChipAnimation = null;
            for (int i = 0, n = listSeat.Count; i < n; i++)
            {
                mSeat = listSeat[i];
                if (null == mSeat || null == mSeat.Player || mSeat.Player.canPlayStatus == 0)
                    continue;
                sequencePlayFirstRecyclingChipAnimation = mSeat.PlayRecyclingChipAnimation();
            }

            if (null != sequencePlayFirstRecyclingChipAnimation)
            {
                // sequencePlayFirstRecyclingChipAnimation.AppendCallback(tweenCallback);
                sequencePlayFirstRecyclingChipAnimation.OnComplete(tweenCallback);

                sequencePlayFirstRecyclingChipAnimation.Play();
            }
            else
            {
                if (null != tweenCallback)
                    tweenCallback();
            }

            return sequencePlayFirstRecyclingChipAnimation;
        }

        private void PlayFirstRecyclingChipSubAnimation(TweenCallback tweenCallback)
        {
            fuck4thPCardByInsuranceState = 2;

            GameObject mObj = null;
            PotInfo mPotInfo = null;

            if (listPotInfo.Count == 0)
            {
                mObj = GameObject.Instantiate(transPot.gameObject);
                mObj.transform.SetParent(transPot.parent);
                mObj.transform.localPosition = GameUtil.TexasPots[0];
                mObj.transform.localRotation = Quaternion.identity;
                mObj.transform.localScale = Vector3.one;
                mObj.name = $"Pot{0}";

                mPotInfo = new PotInfo(mObj.transform);
                listPotInfo.Add(mPotInfo);
            }
            else
            {
                mPotInfo = listPotInfo[0];
            }

            mPotInfo.imagePot.sprite = rcChipSprite.Get<Sprite>(GameUtil.GetChipSpriteName(alreadAnte));
            mPotInfo.textPot.text = StringHelper.GetShortString(alreadAnte);
            float mFrameWidth = mPotInfo.textPot.preferredWidth + mPotInfo.imagePot.rectTransform.sizeDelta.x + 10f;

            mPotInfo.imagePotFrame.rectTransform.pivot = new Vector2(0, 0.5f);
            mPotInfo.imagePotFrame.rectTransform.sizeDelta = new Vector2(0, mPotInfo.imagePotFrame.rectTransform.sizeDelta.y);
            mPotInfo.imagePotFrame.transform.localPosition = Vector3.zero;
            mPotInfo.imagePot.transform.localPosition = Vector3.zero;

            mPotInfo.trans.localPosition = GameUtil.ChangeToLocalPos(textAlreadAnte.transform.localPosition, textAlreadAnte.rectTransform.parent, mPotInfo.trans.parent);
            mPotInfo.trans.gameObject.SetActive(true);

            sequencePlayFirstRecyclingChipSubAnimation = DOTween.Sequence();
            sequencePlayFirstRecyclingChipSubAnimation.Append(mPotInfo.imagePotFrame.rectTransform.DOSizeDelta(new Vector2(mFrameWidth, mPotInfo.imagePotFrame.rectTransform.sizeDelta.y), 0.5f));
            sequencePlayFirstRecyclingChipSubAnimation.Join(mPotInfo.imagePotFrame.rectTransform.DOLocalMoveX(-mFrameWidth / 2f, time_05));
            sequencePlayFirstRecyclingChipSubAnimation.Join(mPotInfo.imagePot.rectTransform.DOLocalMoveX(-mFrameWidth / 2f, time_05));
            sequencePlayFirstRecyclingChipSubAnimation.Append(mPotInfo.trans.DOLocalMoveY(0, 0.3f));

            // sequencePlayFirstRecyclingChipSubAnimation.AppendCallback(() =>
            // {
            //     UpdatePots();
            // });
            //
            // if (null != tweenCallback)
            //     sequencePlayFirstRecyclingChipSubAnimation.AppendCallback(tweenCallback);

            if (null != tweenCallback)
            {
                sequencePlayFirstRecyclingChipSubAnimation.OnComplete(() =>
                {
                    UpdatePots();
                    tweenCallback();
                });
            }
            else
            {
                sequencePlayFirstRecyclingChipSubAnimation.OnComplete(UpdatePots);
            }

            sequencePlayFirstRecyclingChipSubAnimation.Play();
        }

        /// <summary>
        /// 播放首次触发保险动画
        /// </summary>
        private void PlayFirstInsurance(TweenCallback tweenCallback)
        {
            sequencePlayFirstInsurance = null;
            if (null != imageInsurance)
            {
                imageInsurance.transform.localPosition = new Vector3(0, -200);
                Transform mTrans = imageInsurance.transform.Find("Text");
                if (null != mTrans)
                {
                    CanvasGroup mCanvasGroup = mTrans.GetComponent<CanvasGroup>();
                    if (null != mCanvasGroup)
                    {
                        mCanvasGroup.alpha = 0;
                        imageInsurance.gameObject.SetActive(true);
                        sequencePlayFirstInsurance = DOTween.Sequence();
                        sequencePlayFirstInsurance.Append(imageInsurance.transform.DOLocalMoveY(0, 0.4f, true));
                        sequencePlayFirstInsurance.Join(mCanvasGroup.DOFade(1, 0.2f));
                        sequencePlayFirstInsurance.AppendInterval(0.7f);
                        sequencePlayFirstInsurance.Append(mCanvasGroup.DOFade(0, 0.2f));
                        // sequencePlayFirstInsurance.AppendCallback(() =>
                        // {
                        //     imageInsurance.gameObject.SetActive(false);
                        // });
                        // if (null != tweenCallback)
                        //     sequencePlayFirstInsurance.AppendCallback(tweenCallback);

                        if (null != tweenCallback)
                        {
                            sequencePlayFirstInsurance.OnComplete(() =>
                            {
                                imageInsurance.gameObject.SetActive(false);
                                tweenCallback();
                            });
                        }
                        else
                        {
                            sequencePlayFirstInsurance.OnComplete(() =>
                            {
                                imageInsurance.gameObject.SetActive(false);
                            });
                        }

                        sequencePlayFirstInsurance.Play();
                    }
                    else
                    {
                        if (null != tweenCallback)
                            tweenCallback();
                    }
                }
                else
                {
                    if (null != tweenCallback)
                        tweenCallback();
                }
            }
            else
            {
                if (null != tweenCallback)
                    tweenCallback();
            }
        }

        protected void ShowBustCardAnimation()
        {
            int iCount = GetCurPublicCardsCount();
            if (iCount >= 4)
            {
                armatureBustCard.gameObject.SetActive(true);
                armatureBustCard.transform.localPosition = new Vector3(153 * (iCount - 3), 220, 0);
                if (null != armatureBustCard.dragonAnimation)
                {
                    armatureBustCard.dragonAnimation.Reset();
                    armatureBustCard.dragonAnimation.Play("newAnimation", 1);
                    armatureBustCard.AddEventListener(DragonBones.EventObject.COMPLETE, (key, go) =>
                    {
                        armatureBustCard.gameObject.SetActive(false);
                    });
                }
            }
        }


        /// <summary>
        /// 播放本轮结束公共牌动画
        /// </summary>
        protected void PlayEndPublicCardsAnimation(REQ_GAME_RECV_WINNER rec)
        {
            // Log.Msg(rec);

            List<sbyte> mCacheWinnerSeatIds = null; // 赢家座位
            List<sbyte> mCacheWinnerCardTypes = null; // 赢家牌型

            for (int i = 0, n = rec.seatIDArray.Count; i < n; i++)
            {
                // 找到赢家
                if (rec.isWinArray[i] == 0)
                {
                    if (null == mCacheWinnerSeatIds)
                        mCacheWinnerSeatIds = new List<sbyte>();
                    mCacheWinnerSeatIds.Add(rec.seatIDArray[i]);
                    if (null == mCacheWinnerCardTypes)
                        mCacheWinnerCardTypes = new List<sbyte>();
                    mCacheWinnerCardTypes.Add(rec.cardTypesArray[i]);
                }
            }

            // rec.cardSort会五个五个一组，对应赢家数量
            List<List<sbyte>> mTmpCardSorts = new List<List<sbyte>>();
            int mGroup = rec.cardSort.Count / 5;
            for (int i = 0, n = mGroup; i < n; i++)
            {
                List<sbyte> mTmpCards = new List<sbyte>();
                for (int j = 0, m = 5; j < m; j++)
                {
                    mTmpCards.Add(rec.cardSort[j + i * 5]);
                }

                mTmpCardSorts.Add(mTmpCards);
            }

            bool mHaveCardSort = true;
            for (int i = 0, n = rec.cardSort.Count; i < n; i++)
            {
                if (rec.cardSort[i] < 0)
                {
                    mHaveCardSort = false;
                    break;
                }
            }

            if (null == mCacheWinnerSeatIds || mCacheWinnerSeatIds.Count == 0 || !mHaveCardSort)
            {
                // 没有赢家
                return;
            }

            // 如果玩家只有一个且为大牌,播放大牌动画
            if (mCacheWinnerSeatIds.Count == 1 && mCacheWinnerCardTypes[0] > 0 && mCacheWinnerCardTypes[0] <= 4)
            {
                UIComponent.Instance.ShowNoAnimation(UIType.UIBigWinAnimation, new UIBigWinAnimationComponent.WinnerData
                {
                    seatID = mCacheWinnerSeatIds[0],
                    mCards = mTmpCardSorts[0]
                });
                return;
            }

            // Log.Msg(mCacheWinnerSeatIds);
            // Log.Msg(mTmpCardSorts);

            int mWinnerIndex = 0;
            for (int i = 0, n = mCacheWinnerSeatIds.Count; i < n; i++)
            {
                if (mainPlayer.isPlaying && mainPlayer.seatID == mCacheWinnerSeatIds[i])
                {
                    mWinnerIndex = i;
                    break;
                }
            }

            for (int i = 0, n = listCards.Count; i < n; i++)
            {
                GameObject mObj = listCards[i].imageSelect.gameObject;
                if (i == 0)
                {
                    sequencePlayEndPublicCardsAnimation.Append(listCards[i].imageCard.DOColor(Color.gray, time_02)
                                                                       .OnStart(() => { mObj.SetActive(false); }));
                }
                else
                {
                    sequencePlayEndPublicCardsAnimation.Join(listCards[i].imageCard.DOColor(Color.gray, time_02)
                                                                     .OnStart(() => { mObj.SetActive(false); }));
                }
            }

            bool mIsFirst = true;
            List<sbyte> mCacheCardIds = new List<sbyte>();
            for (int i = 0, n = listCards.Count; i < n; i++)
            {
                for (int j = 0, m = mTmpCardSorts[mWinnerIndex].Count; j < m; j++)
                {
                    if (mCacheCardIds.Contains(listCards[i].cardId))
                        continue;
                    if (mTmpCardSorts[mWinnerIndex][j] > 4 || mTmpCardSorts[mWinnerIndex][j] < 0)
                        continue;

                    if (listCards[i].cardId == GameCache.Instance.CurGame.cards[mTmpCardSorts[mWinnerIndex][j]])
                    {
                        mCacheCardIds.Add(listCards[i].cardId);
                        if (mIsFirst)
                        {
                            mIsFirst = false;
                            sequencePlayEndPublicCardsAnimation.Append(listCards[i].trans.DOLocalMoveY(40, 0.3f));
                        }
                        else
                        {
                            sequencePlayEndPublicCardsAnimation.Join(listCards[i].trans.DOLocalMoveY(40, 0.3f));
                        }

                        sequencePlayEndPublicCardsAnimation.Join(listCards[i].imageCard.DOColor(Color.white, time_02));
                        break;
                    }
                }
            }

            if (waittingUpdatePublicCardsAnimation)
            {
                // sequencePlayEndPublicCardsAnimation.Play().SetDelay(0.5f);
                sequencePlayEndPublicCardsAnimation.SetDelay(time_05);
            }
            else
            {
                // sequencePlayEndPublicCardsAnimation.Play();
            }
        }

        /// <summary>
        /// 刷新公共牌样式
        /// </summary>
        public void UpdatePublicCardSettingType()
        {
            PublicCardInfo mPublicCardInfo = null;
            if (null != cards)
            {
                for (int i = 0, n = cards.Count; i < n; i++)
                {
                    mPublicCardInfo = listCards[i];
                    mPublicCardInfo.imageCard.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(cards[i]));
                }
            }
        }

        /// <summary>
        /// 刷新公共牌
        /// </summary>
        protected virtual void UpdatePublicCards(int startIndex, TweenCallback tweenCallback)
        {
            if (null == cards)
                return;

            int mCacheCount = GetCurPublicCardsCount();

            if (mCacheCount == 0)
            {
                ClearPublicCardsUI();
                return;
            }

            // 公共牌动画
            waittingUpdatePublicCardsAnimation = true;
            sequenceUpdatePublicCards = DOTween.Sequence();

            // 保险就是多事，特殊处理一下。来了三张公共牌，动画播放中，没有保险可买，马上又来了一张公共牌。
            if (fuck4thPCardByInsuranceState == 1)
            {
                if (startIndex == 3)
                {
                    sequenceUpdatePublicCards.SetDelay(3f);
                }
            }
            else if (fuck4thPCardByInsuranceState == 2)
            {
                if (startIndex == 3)
                {
                    sequenceUpdatePublicCards.SetDelay(1.4f);
                }
            }

            fuck4thPCardByInsuranceState = 0;

            if (startIndex == 0)
            {
                PublicCardInfo mPublicCardInfo = null;
                // mPublicCardInfo = listCards[startIndex];
                mPublicCardInfo = listCards[startIndex + 2];
                mPublicCardInfo.imageCard.color = Color.white;
                // mPublicCardInfo.cardId = cards[startIndex];
                mPublicCardInfo.cardId = cards[startIndex + 2];
                mPublicCardInfo.imageCard.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(-1));
                mPublicCardInfo.trans.localPosition = listDefaultPublicCardsLPos[0];
                mPublicCardInfo.trans.localScale = new Vector3(1, 1);
                mPublicCardInfo.trans.gameObject.SetActive(true);
                sbyte mCacheCardId = mPublicCardInfo.cardId;
                Image mCacheImage = mPublicCardInfo.imageCard;
                sequenceUpdatePublicCards.Append(mPublicCardInfo.trans.DOScaleX(0, 0.1f).OnComplete(() =>
                {
                    mCacheImage.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(mCacheCardId));
                }));
                sequenceUpdatePublicCards.Append(mPublicCardInfo.trans.DOScaleX(1, 0.1f).OnStart(() =>
                {
                    SoundComponent.Instance.PlaySFX(SoundComponent.SFX_DESK_CHAT);
                }).OnComplete(() =>
                {
                    PublicCardInfo mPublicCardInfo0 = listCards[0];
                    mPublicCardInfo0.imageCard.color = Color.white;
                    mPublicCardInfo0.cardId = cards[0];
                    mPublicCardInfo0.imageCard.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(mPublicCardInfo0.cardId));
                    mPublicCardInfo0.trans.localPosition = listDefaultPublicCardsLPos[0];
                    mPublicCardInfo0.trans.localScale = new Vector3(1, 1);
                    mPublicCardInfo0.trans.gameObject.SetActive(true);
                }));
                sequenceUpdatePublicCards.AppendInterval(0.4f);

                int mTmpStartIndex = startIndex + 1;
                for (int i = mTmpStartIndex; i < 3; i++)
                {

                    mPublicCardInfo = listCards[i];
                    mPublicCardInfo.imageCard.color = Color.white;
                    mPublicCardInfo.cardId = cards[i];
                    if (i != 2)
                    {
                        mPublicCardInfo.imageCard.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(mPublicCardInfo.cardId));
                    }
                    mPublicCardInfo.trans.localPosition = listDefaultPublicCardsLPos[0];
                    Transform mCacheTrans = mPublicCardInfo.trans;
                    if (i == mTmpStartIndex)
                    {
                        sequenceUpdatePublicCards.Append(mPublicCardInfo.trans.DOLocalMove(listDefaultPublicCardsLPos[i], 0.4f).OnStart(() =>
                        {
                            mCacheTrans.gameObject.SetActive(true);
                        }));
                    }
                    else if (i > mTmpStartIndex)
                    {
                        sequenceUpdatePublicCards.Join(mPublicCardInfo.trans.DOLocalMove(listDefaultPublicCardsLPos[i], 0.4f).OnStart(() =>
                        {
                            mCacheTrans.gameObject.SetActive(true);
                        }));
                    }
                }

                if (mCacheCount == 5)
                {
                    // 一下下发5张
                    for (int i = 3; i < mCacheCount; i++)
                    {
                        mPublicCardInfo = listCards[i];
                        mPublicCardInfo.imageCard.color = Color.white;
                        mPublicCardInfo.cardId = cards[i];
                        mPublicCardInfo.imageCard.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(-1));
                        mPublicCardInfo.trans.localPosition = listDefaultPublicCardsLPos[i];
                        Transform mCacheTrans = mPublicCardInfo.trans;
                        sbyte mCacheCardId1 = mPublicCardInfo.cardId;
                        Image mCacheImage1 = mPublicCardInfo.imageCard;
                        sequenceUpdatePublicCards.Append(mPublicCardInfo.trans.DOScaleX(0, time_02).OnStart(() =>
                        {
                            mCacheTrans.gameObject.SetActive(true);
                        }).OnComplete(() =>
                        {
                            mCacheImage1.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(mCacheCardId1));
                        }));
                        sequenceUpdatePublicCards.Append(mPublicCardInfo.trans.DOScaleX(1, time_02).OnStart(() =>
                        {
                            SoundComponent.Instance.PlaySFX(SoundComponent.SFX_DESK_CHAT);
                        }));
                    }
                }
            }
            else
            {
                for (int i = startIndex, n = mCacheCount; i < n; i++)
                {
                    PublicCardInfo mPublicCardInfo = listCards[i];
                    mPublicCardInfo.cardId = cards[i];
                    mPublicCardInfo.trans.localPosition = listDefaultPublicCardsLPos[i];
                    mPublicCardInfo.trans.localScale = new Vector3(1, 1);
                    mPublicCardInfo.imageCard.color = Color.white;
                    mPublicCardInfo.imageCard.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(-1));
                    sbyte mCacheCardId = mPublicCardInfo.cardId;
                    Image mCacheImage = mPublicCardInfo.imageCard;
                    Transform mCacheTrans = mPublicCardInfo.trans;
                    sequenceUpdatePublicCards.Append(mPublicCardInfo.trans.DOScaleX(0, time_02).OnStart(() =>
                    {
                        mCacheTrans.gameObject.SetActive(true);
                    }).OnComplete(() =>
                    {
                        mCacheImage.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(mCacheCardId));
                    }));
                    sequenceUpdatePublicCards.Append(mPublicCardInfo.trans.DOScaleX(1, time_02).OnStart(() =>
                    {
                        SoundComponent.Instance.PlaySFX(SoundComponent.SFX_DESK_CHAT);
                    }));
                }

                PublicCardInfo mHidePublicCardInfo = null;
                for (int i = mCacheCount, n = listCards.Count; i < n; i++)
                {
                    mHidePublicCardInfo = listCards[i];
                    mHidePublicCardInfo.cardId = -1;
                    // mHidePublicCardInfo.imageCard.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(mHidePublicCardInfo.cardId));
                    mHidePublicCardInfo.trans.gameObject.SetActive(false);
                }
            }

            // 参与了牌局，才能看到牌型提示
            Seat mClientSeat = GetSeatByClientId(0);
            // if (null != mClientSeat.Player && mClientSeat.Player.userID == MainPlayer.userID && MainPlayer.canPlayStatus == 1)
            // {
            //     sequenceUpdatePublicCards.AppendCallback(() =>
            //     {
            //         List<sbyte> highlightCards;
            //         CardType cardType = GetCardType(out highlightCards);
            //         for (int i = 0, n = listCards.Count; i < n; i++)
            //         {
            //             listCards[i].imageSelect.gameObject.SetActive(false);
            //             for (int j = 0, m = highlightCards.Count; j < m; j++)
            //             {
            //                 if (listCards[i].cardId == highlightCards[j])
            //                 {
            //                     listCards[i].imageSelect.gameObject.SetActive(true);
            //                     break;
            //                 }
            //             }
            //         }
            //
            //         Seat mSeat = GetSeatById(mainPlayer.seatID);
            //         if (null != mSeat)
            //         {
            //             mSeat.UpdateCardType(cardType, highlightCards);
            //         }
            //     });
            // }
            //
            // if (null != tweenCallback)
            //     sequenceUpdatePublicCards.AppendCallback(tweenCallback);
            //
            // sequenceUpdatePublicCards.AppendCallback(() =>
            // {
            //     waittingUpdatePublicCardsAnimation = false;
            // });

            if (null != mClientSeat.Player && mClientSeat.Player.userID == MainPlayer.userID && MainPlayer.canPlayStatus == 1)
            {
                if (null != tweenCallback)
                {
                    sequenceUpdatePublicCards.OnComplete(() =>
                    {
                        List<sbyte> highlightCards;
                        CardType cardType = GetCardType(out highlightCards);
                        for (int i = 0, n = listCards.Count; i < n; i++)
                        {
                            listCards[i].imageSelect.gameObject.SetActive(false);
                            for (int j = 0, m = highlightCards.Count; j < m; j++)
                            {
                                if (listCards[i].cardId == highlightCards[j])
                                {
                                    listCards[i].imageSelect.gameObject.SetActive(true);
                                    break;
                                }
                            }
                        }

                        Seat mSeat = GetSeatById(mainPlayer.seatID);
                        if (null != mSeat)
                        {
                            mSeat.UpdateCardType(cardType, highlightCards);
                        }

                        tweenCallback();
                        waittingUpdatePublicCardsAnimation = false;
                    });
                }
                else
                {
                    sequenceUpdatePublicCards.OnComplete(() =>
                    {
                        List<sbyte> highlightCards;
                        CardType cardType = GetCardType(out highlightCards);
                        for (int i = 0, n = listCards.Count; i < n; i++)
                        {
                            listCards[i].imageSelect.gameObject.SetActive(false);
                            for (int j = 0, m = highlightCards.Count; j < m; j++)
                            {
                                if (listCards[i].cardId == highlightCards[j])
                                {
                                    listCards[i].imageSelect.gameObject.SetActive(true);
                                    break;
                                }
                            }
                        }

                        Seat mSeat = GetSeatById(mainPlayer.seatID);
                        if (null != mSeat)
                        {
                            mSeat.UpdateCardType(cardType, highlightCards);
                        }

                        waittingUpdatePublicCardsAnimation = false;
                    });
                }
            }
            else
            {
                if (null != tweenCallback)
                {
                    sequenceUpdatePublicCards.OnComplete(() =>
                    {
                        tweenCallback();
                        waittingUpdatePublicCardsAnimation = false;
                    });
                }
                else
                {
                    sequenceUpdatePublicCards.OnComplete(() => { waittingUpdatePublicCardsAnimation = false; });

                }
            }

            sequenceUpdatePublicCards.Play();
        }

        protected virtual CardType GetCardType(out List<sbyte> highlightCards)
        {
            List<sbyte> mCards = new List<sbyte>();
            mCards.AddRange(GameCache.Instance.CurGame.cards);
            mCards.AddRange(mainPlayer.cards);
            return CardTypeUtil.GetCardType(mCards, out highlightCards);
        }

        /// <summary>
        /// 刷新公共牌(不带动画）
        /// </summary>
        protected virtual void UpdatePublicCardsNoAnim()
        {
            PublicCardInfo mPublicCardInfo;
            for (int i = 0, n = GetCurPublicCardsCount(); i < n; i++)
            {
                mPublicCardInfo = listCards[i];
                mPublicCardInfo.cardId = cards[i];
                mPublicCardInfo.trans.localPosition = listDefaultPublicCardsLPos[i];
                mPublicCardInfo.trans.gameObject.SetActive(true);
                mPublicCardInfo.imageCard.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(mPublicCardInfo.cardId));
                mPublicCardInfo.imageCard.color = new Color(1, 1, 1, 1);
            }

            for (int i = GetCurPublicCardsCount(), n = listCards.Count; i < n; i++)
            {
                mPublicCardInfo = listCards[i];
                mPublicCardInfo.cardId = -1;
                mPublicCardInfo.trans.localPosition = listDefaultPublicCardsLPos[i];
                mPublicCardInfo.imageCard.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(mPublicCardInfo.cardId));
                mPublicCardInfo.trans.gameObject.SetActive(false);
            }

            // 参与了牌局，才能看到牌型提示
            if (null != mainPlayer && mainPlayer.isPlaying)
            {
                List<sbyte> highlightCards;
                CardType cardType = GetCardType(out highlightCards);
                for (int i = 0, n = listCards.Count; i < n; i++)
                {
                    listCards[i].imageSelect.gameObject.SetActive(false);
                    for (int j = 0, m = highlightCards.Count; j < m; j++)
                    {
                        if (listCards[i].cardId == highlightCards[j])
                        {
                            listCards[i].imageSelect.gameObject.SetActive(true);
                            break;
                        }
                    }
                }

                Seat mSeat = GetSeatById(mainPlayer.seatID);
                if (null != mSeat)
                {
                    mSeat.UpdateCardType(cardType, highlightCards);
                }
            }
            else
            {
                //清空牌型提示
                for (int i = 0, n = listCards.Count; i < n; i++)
                {
                    listCards[i].imageSelect.gameObject.SetActive(false);
                }
                Seat mSeat = GetSeatById(mainPlayer.seatID);
                if (null != mSeat)
                {
                    mSeat.HideCardType();
                }
            }
        }

        /// <summary>
        /// 清空公共牌UI
        /// </summary>
        protected virtual void ClearPublicCardsUI()
        {
            PublicCardInfo mPublicCardInfo = null;
            for (int i = 0, n = listCards.Count; i < n; i++)
            {
                mPublicCardInfo = listCards[i];
                mPublicCardInfo.trans.localPosition = listDefaultPublicCardsLPos[i];
                mPublicCardInfo.cardId = -1;
                mPublicCardInfo.imageCard.color = Color.white;
                mPublicCardInfo.imageSelect.gameObject.SetActive(false);
                mPublicCardInfo.trans.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 添加公共牌Id
        /// </summary>
        /// <param name="list"></param>
        protected virtual void AddPublicCards(List<sbyte> list)
        {
            // 判断一下cards的合法性
            if (null == cards)
                cards = new List<sbyte>();
            if (cards.Count < listCards.Count)
            {
                for (int i = 0, n = listCards.Count - cards.Count; i < n; i++)
                {
                    cards.Add(-1);
                }
            }
            else if (cards.Count > listCards.Count)
            {
                Log.Error($"Add PublicCards Error cards.Count:{cards.Count}");
                return;
            }

            int mStartIndex = 0;
            for (int i = 0, n = cards.Count; i < n; i++)
            {
                if (cards[i] == -1)
                {
                    mStartIndex = i;
                    break;
                }
            }

            if (list.Count > cards.Count - mStartIndex)
            {
                Log.Error($"Add PublicCards Error index:{mStartIndex}, list.Count:{list.Count}");
                return;
            }

            for (int i = 0, n = list.Count; i < n; i++)
            {
                cards[i + mStartIndex] = list[i];
            }
        }

        /// <summary>
        /// 重置公共牌Id
        /// </summary>
        protected virtual void ResetPublicCardsId()
        {
            if (null == cards)
                cards = new List<sbyte>();

            if (cards.Count == listCards.Count)
            {
                for (int i = 0, n = listCards.Count; i < n; i++)
                {
                    cards[i] = -1;
                }
            }
            else
            {
                cards.Clear();
                for (int i = 0, n = listCards.Count; i < n; i++)
                {
                    cards.Add(-1);
                }
            }
        }

        /// <summary>
        /// 重置公共牌Image
        /// </summary>
        protected virtual void ResetPublicCardsImage()
        {
            PublicCardInfo mPublicCardInfo = null;
            for (int i = 0, n = listCards.Count; i < n; i++)
            {
                mPublicCardInfo = listCards[i];
                mPublicCardInfo.imageCard.color = Color.white;
                mPublicCardInfo.cardId = -1;
                mPublicCardInfo.imageCard.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(mPublicCardInfo.cardId));
                mPublicCardInfo.trans.localPosition = listDefaultPublicCardsLPos[i];
                mPublicCardInfo.trans.localScale = new Vector3(1, 1);
                mPublicCardInfo.trans.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 设置公共牌Id
        /// </summary>
        protected void SetPublicCardInfosId()
        {
            PublicCardInfo mPublicCardInfo = null;
            for (int i = 0, n = cards.Count; i < n; i++)
            {
                mPublicCardInfo = listCards[i];
                mPublicCardInfo.cardId = cards[i];
            }
        }

        /// <summary>
        /// 获取当前已发公共牌数量
        /// </summary>
        /// <returns></returns>
        public virtual int GetCurPublicCardsCount()
        {
            if (null == cards)
                ResetPublicCardsId();

            for (int i = 0, n = cards.Count; i < n; i++)
            {
                if (cards[i] == -1)
                    return i;
            }

            // 最多5张
            return 5;
        }

        /// <summary>
        /// 显示查看更多公共牌
        /// </summary>
        protected void ShowSeeMorePublic()
        {
            if (mainPlayer.canPlayStatus == 0)
                return;

            if (GetCurPublicCardsCount() == 5)
                return;

            int mCost = GameUtil.GetSeeMoreCost(smallBlind);
            
            textSeeMorePublicGold.text = $"{StringHelper.ShowGold(mCost)}";

            if (GetCurPublicCardsCount() == 0)
            {
                // textSeeMorePublic.text = $"查看翻牌";
                textSeeMorePublic.text = CPErrorCode.LanguageDescription(10018);
            }
            else if (GetCurPublicCardsCount() == 3)
            {
                // textSeeMorePublic.text = $"查看转牌";
                textSeeMorePublic.text = CPErrorCode.LanguageDescription(10019);
            }
            else
            {
                // textSeeMorePublic.text = $"查看河牌";
                textSeeMorePublic.text = CPErrorCode.LanguageDescription(10020);
            }

            buttonSeeMorePublic.gameObject.SetActive(true);
        }

        protected void HideWaitForStartTips()
        {
            if (imageWaitForStartTips.gameObject.activeInHierarchy)
            {
                imageWaitForStartTips.gameObject.SetActive(false);
            }
        }

        protected void HideSelectSeatTips()
        {
            if (imageSelectSeatTips.gameObject.activeInHierarchy)
            {
                imageSelectSeatTips.gameObject.SetActive(false);
            }
        }

        protected void HideSeeMorePublic()
        {
            buttonSeeMorePublic.gameObject.SetActive(false);
        }

        /// <summary>
        /// 显示花费查看公共牌提示
        /// </summary>
        /// <param name="content"></param>
        private void ShowSeeMorePublicTips(string content)
        {
            textSeeMorePublicTips.text = content;
            imageSeeMorePublicTips.gameObject.SetActive(true);
        }

        protected void HideSeeMorePublicTips()
        {
            imageSeeMorePublicTips.gameObject.SetActive(false);
        }

        /// <summary>
        /// 刷新分池
        /// </summary>
        protected virtual void UpdatePots()
        {
            int mNewStart = 0, mNewEnd = 0;
            int mUpdateStart = 0, mUpdateEnd = 0;
            int mHideStart = 0, mHideEnd = 0;

            if (pots.Count < listPotInfo.Count)
            {
                mUpdateStart = 0;
                mUpdateEnd = pots.Count;
                mHideStart = pots.Count;
                mHideEnd = listPotInfo.Count;
            }
            else if (pots.Count > listPotInfo.Count)
            {
                mUpdateStart = 0;
                mUpdateEnd = listPotInfo.Count;
                mNewStart = listPotInfo.Count;
                mNewEnd = pots.Count;
            }
            else
            {
                mUpdateStart = 0;
                mUpdateEnd = listPotInfo.Count;
            }

            GameObject mObj = null;
            PotInfo mPotInfo = null;

            // 隐藏放最前面
            for (int i = mHideStart; i < mHideEnd; i++)
            {
                mObj = listPotInfo[i].trans.gameObject;
                mObj.SetActive(false);
            }

            for (int i = mNewStart; i < mNewEnd; i++)
            {
                mObj = GameObject.Instantiate(transPot.gameObject);
                mObj.transform.SetParent(transPots);

                mObj.transform.localRotation = Quaternion.identity;
                mObj.transform.localScale = Vector3.one;
                mObj.name = $"Pot{i}";

                mPotInfo = new PotInfo(mObj.transform);
                listPotInfo.Add(mPotInfo);

                mPotInfo.imagePot.sprite = rcChipSprite.Get<Sprite>(GameUtil.GetChipSpriteName(pots[i]));
                mPotInfo.textPot.text = StringHelper.GetShortString(pots[i]);
                float mFrameWidth = mPotInfo.textPot.preferredWidth + mPotInfo.imagePot.rectTransform.sizeDelta.x + 10f;
                mPotInfo.imagePotFrame.rectTransform.sizeDelta = new Vector2(mFrameWidth, mPotInfo.imagePotFrame.rectTransform.sizeDelta.y);
                mPotInfo.imagePotFrame.rectTransform.pivot = new Vector2(0, 0.5f);
                if (i == 0)
                {
                    mPotInfo.imagePotFrame.rectTransform.localPosition = new Vector3(-mPotInfo.imagePotFrame.rectTransform.sizeDelta.x / 2f, mPotInfo.imagePotFrame.rectTransform.localPosition.y);
                    mPotInfo.imagePot.rectTransform.localPosition = new Vector3(-mPotInfo.imagePotFrame.rectTransform.sizeDelta.x / 2f, 0);
                }
                else
                {
                    mPotInfo.imagePotFrame.rectTransform.localPosition = Vector3.zero;
                    mPotInfo.imagePot.rectTransform.localPosition = Vector3.zero;
                }

                if (pots[i] > 0)
                {
                    if (i == 0)
                    {
                        mPotInfo.trans.localPosition = GameUtil.TexasPots[i];
                    }
                    else
                    {
                        mPotInfo.trans.localPosition = GameUtil.TexasPots[0];
                        mPotInfo.trans.DOLocalMove(GameUtil.TexasPots[i], 0.3f);
                    }
                }

                mObj.SetActive(pots[i] > 0);
            }

            for (int i = mUpdateStart; i < mUpdateEnd; i++)
            {
                mPotInfo = listPotInfo[i];
                mObj = listPotInfo[i].trans.gameObject;

                mPotInfo.imagePot.sprite = rcChipSprite.Get<Sprite>(GameUtil.GetChipSpriteName(pots[i]));
                mPotInfo.textPot.text = StringHelper.GetShortString(pots[i]);
                float mFrameWidth = mPotInfo.textPot.preferredWidth + mPotInfo.imagePot.rectTransform.sizeDelta.x + 10f;
                mPotInfo.imagePotFrame.rectTransform.sizeDelta = new Vector2(mFrameWidth, mPotInfo.imagePotFrame.rectTransform.sizeDelta.y);
                if (i == 0)
                {
                    mPotInfo.imagePotFrame.rectTransform.pivot = new Vector2(0, 0.5f);
                    mPotInfo.imagePotFrame.rectTransform.localPosition = new Vector3(-mPotInfo.imagePotFrame.rectTransform.sizeDelta.x / 2f, mPotInfo.imagePotFrame.rectTransform.localPosition.y);
                    mPotInfo.imagePot.rectTransform.localPosition = new Vector3(-mPotInfo.imagePotFrame.rectTransform.sizeDelta.x / 2f, 0);
                }
                else
                {
                    mPotInfo.imagePotFrame.rectTransform.pivot = new Vector2(0, 0.5f);
                    mPotInfo.imagePotFrame.rectTransform.localPosition = Vector3.zero;
                    mPotInfo.imagePot.rectTransform.localPosition = Vector3.zero;
                }

                if (pots[i] > 0 && !mObj.activeInHierarchy)
                {
                    if (i == 0)
                    {
                        mObj.transform.localPosition = GameUtil.TexasPots[i];
                    }
                    else
                    {
                        mObj.transform.localPosition = GameUtil.TexasPots[0];
                        mObj.transform.DOLocalMove(GameUtil.TexasPots[i], 0.3f);
                    }
                }

                mObj.SetActive(pots[i] > 0);
            }
        }

        /// <summary>
        /// 刷新底池
        /// </summary>
        protected void UpdateAlreadAnte()
        {
            // textAlreadAnte.text = $"底池:{alreadAnte}";
            textAlreadAnte.text = $"{CPErrorCode.LanguageDescription(20005)}:{StringHelper.ShowGold(alreadAnte)}";
        }

        /// <summary>
        /// 刷新房间描述
        /// </summary>
        protected virtual void UpdateRoomDes()
        {
            StringBuilder mStringBuilder = new StringBuilder();
            mStringBuilder.AppendLine(GameCache.Instance.roomName);

            if (GameCache.Instance.roomMode == 2)
            {
                mStringBuilder.AppendLine("ID:" + GameCache.Instance.room_id.ToString());
            }

            mStringBuilder.AppendLine(GetRoomTypeDes());

            // mStringBuilder.AppendLine($"盲注{smallBlind}/{bigBlind}");
            if (roomType == 21 || roomType == 23)
            {

            }
            else
            {
                mStringBuilder.AppendLine($"{CPErrorCode.LanguageDescription(20006)}{StringHelper.ShowGold(smallBlind, false)}/{StringHelper.ShowGold(bigBlind, false)}");
            }

            if (groupBet > 0)
            {
                mStringBuilder.AppendLine($"{CPErrorCode.LanguageDescription(20007)}{StringHelper.ShowGold(groupBet)}");
            }

            if (GameCache.Instance.insurance == 1)
            {
                mStringBuilder.AppendLine(CPErrorCode.LanguageDescription(10021));
            }

            if (isGPSRestrictions == 1 && isIpRestrictions == 1)
            {
                mStringBuilder.AppendLine($"GPS  IP{CPErrorCode.LanguageDescription(20008)}");
            }
            else if (isGPSRestrictions == 1 && isIpRestrictions == 0)
            {
                mStringBuilder.AppendLine($"GPS{CPErrorCode.LanguageDescription(20008)}");
            }
            else if (isGPSRestrictions == 0 && isIpRestrictions == 1)
            {
                mStringBuilder.AppendLine($"IP{CPErrorCode.LanguageDescription(20008)}");
            }

            textRoomInfo.text = mStringBuilder.ToString();
        }

        protected virtual string GetRoomTypeDes()
        {
            // return "德州";
            if (roomType == 21 || roomType == 23)
            {
                return CPErrorCode.LanguageDescription(10334);
            }
            return CPErrorCode.LanguageDescription(10022);
        }

        protected virtual void SetUpLogo()
        {
            imageLogo.sprite = this.rc.Get<Sprite>("icon_normal");
            // imageLogo.SetNativeSize();
        }

        // 托管相关
        protected virtual void SendTrustAction(sbyte option, sbyte initiative)
        {
            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_TRUST_ACTION()
            {
                roomID = GameCache.Instance.room_id,
                roomPath = GameCache.Instance.room_path,
                option = option,
                initiative = initiative,
            });
        }

        /// <summary>
        /// 手牌绿眼睛
        /// </summary>
        protected virtual void ResetShowCardsId()
        {
            showCardsId = 0;
        }

        /// <summary>
        /// 最小可玩的筹码，少于等于此数需要带入才能玩
        /// </summary>
        protected virtual int GetMinPlayChips()
        {
            return bigBlind + groupBet;
        }

        /// <summary>
        /// 清空气泡
        /// </summary>
        protected virtual void ClearSeatBubble(bool isRoundFinish)
        {
            Seat mSeat = null;
            for (int i = 0, n = listSeat.Count; i < n; i++)
            {
                mSeat = listSeat[i];
                if (null == mSeat || null == mSeat.Player)
                    continue;

                if (!isRoundFinish && mSeat.Player.status == 6)
                    continue;

                mSeat.HideBubble();
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// 获取扑克牌Sprite
        /// </summary>
        /// <param name="spriteName"></param>
        /// <returns></returns>
        public Sprite GetPokerSpriteBySpriteName(string spriteName)
        {
            return rcPokerSprite.Get<Sprite>(spriteName);
        }

        /// <summary>
        /// 获取筹码Sprite
        /// </summary>
        /// <param name="spriteName"></param>
        /// <returns></returns>
        public Sprite GetChipSpriteBySpriteName(string spriteName)
        {
            return rcChipSprite.Get<Sprite>(spriteName);
        }

        /// <summary>
        /// 坐下 
        /// </summary>
        /// <param name="clientSeatId"></param>
        public virtual void Sitdown(int clientSeatId)
        {
            if (isTrusted == 1)
            {
                cacheClientSeatId = clientSeatId;
                // 查询托管
                //进房后请求托管
                SendTrustAction(4, 0);
                return;
            }

            SitdownFunc(clientSeatId);
        }

        private async void SitdownFunc(int clientSeatId)
        {
            Seat mSeat = GetSeatByClientId(clientSeatId);
            if (null == mSeat)
            {
                Log.Error($"Sitdown 位置不存在 clientSeatId:{clientSeatId}");
                return;
            }

            if (mainPlayer.seatID != -1)
            {
                Log.Error($"Sitdown 你已在其他位置 seatID {mainPlayer.seatID}, clientSeatId {GetSeatById(mainPlayer.seatID).ClientSeatId}");
                return;
            }

            if (null != mSeat.Player)
            {
                if (mSeat.Player.userID == mainPlayer.userID)
                {
                    Log.Error($"Sitdown 你已在该位置 clientSeatId:{clientSeatId}");
                    return;
                }

                Log.Error($"Sitdown 该位置有其他玩家 clientSeatId:{clientSeatId}");
                return;
            }

            cacheSitdownSeatId = mSeat.seatID;

            if (isGPSRestrictions == 1)
            {
                //开启GPS，先获取定位
                waittingGPSCallback = true;
                NativeManager.GetGPSLocation();
                if (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.OSXEditor)
                {
                    return;
                }
            }

            // 是否有最短入局时间
            if (GameCache.Instance.shortest_time > 0 && bFirstSit)
            {
                UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                {
                    type = UIDialogComponent.DialogData.DialogType.Commit,
                    // title = $"最短上桌时间",
                    title = CPErrorCode.LanguageDescription(10023),
                    // content = $"房主设置玩家的最短打牌时间为<color=#ff0000>{(GameCache.Instance.shortest_time / 60f).ToString("F1")}小时</color>,玩家在此期间内不能离桌,否则将实行留盲代打得托管模式.",
                    content = CPErrorCode.LanguageDescription(20009, new List<object>() { (GameCache.Instance.shortest_time / 60f).ToString("F1") }),
                    // contentCommit = "知道了",
                    contentCommit = CPErrorCode.LanguageDescription(10024),
                    actionCommit = () =>
                    {
                        // 请求坐下
                        CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_SEND_SEAT_ACTION()
                        {
                            action = 1,
                            seatID = mSeat.seatID,
                            longitude = GameCache.Instance.longitude,
                            latitude = GameCache.Instance.latitude,
                            clientIP = GameCache.Instance.client_ip,
                            roomPath = GameCache.Instance.room_path,
                            roomID = GameCache.Instance.room_id,
                        });
                    },
                    actionCancel = null
                });

                return;
            }

            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_SEND_SEAT_ACTION()
            {
                action = 1,
                seatID = mSeat.seatID,
                longitude = GameCache.Instance.longitude,
                latitude = GameCache.Instance.latitude,
                clientIP = GameCache.Instance.client_ip,
                roomPath = GameCache.Instance.room_path,
                roomID = GameCache.Instance.room_id,
            });
        }

        public virtual void GPSCallback_Sitdown()
        {
            // 是否有最短入局时间
            if (GameCache.Instance.shortest_time > 0 && bFirstSit)
            {
                UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                {
                    type = UIDialogComponent.DialogData.DialogType.Commit,
                    // title = $"最短上桌时间",
                    title = CPErrorCode.LanguageDescription(10023),
                    // content = $"房主设置玩家的最短打牌时间为<color=#ff0000>{(GameCache.Instance.shortest_time / 60f).ToString("F1")}小时</color>,玩家在此期间内不能离桌,否则将实行留盲代打得托管模式.",
                    content = CPErrorCode.LanguageDescription(20009, new List<object>() { (GameCache.Instance.shortest_time / 60f).ToString("F1") }),
                    // contentCommit = "知道了",
                    contentCommit = CPErrorCode.LanguageDescription(10024),
                    actionCommit = () =>
                    {
                        // 请求坐下
                        CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_SEND_SEAT_ACTION()
                        {
                            action = 1,
                            seatID = cacheSitdownSeatId,
                            longitude = GameCache.Instance.longitude,
                            latitude = GameCache.Instance.latitude,
                            clientIP = GameCache.Instance.client_ip,
                            roomPath = GameCache.Instance.room_path,
                            roomID = GameCache.Instance.room_id,
                        });
                    },
                    actionCancel = null
                });

                return;
            }

            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_SEND_SEAT_ACTION()
            {
                action = 1,
                seatID = cacheSitdownSeatId,
                longitude = GameCache.Instance.longitude,
                latitude = GameCache.Instance.latitude,
                clientIP = GameCache.Instance.client_ip,
                roomPath = GameCache.Instance.room_path,
                roomID = GameCache.Instance.room_id,
            });
        }

        /// <summary>
        /// 站起
        /// </summary>
        /// <param name="clientSeatId"></param>
        public virtual void Standup()
        {
            if (mainPlayer.seatID == -1)
            {
                return;
            }

            Seat mSeat = GetSeatById(mainPlayer.seatID);
            if (null == mSeat)
                return;

            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_SEND_SEAT_ACTION()
            {
                action = 2,
                seatID = mainPlayer.seatID,
                longitude = GameCache.Instance.longitude,
                latitude = GameCache.Instance.latitude,
                clientIP = GameCache.Instance.client_ip,
                roomPath = GameCache.Instance.room_path,
                roomID = GameCache.Instance.room_id
            });
        }

        /// <summary>
        /// 强制站起
        /// </summary>
        public virtual void ForcedStandup(int userId)
        {
            Seat mSeat = GetSeatByUserId(userId);
            if (null == mSeat)
                return;

            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_SEND_SEAT_ACTION()
            {
                action = 4,
                seatID = mSeat.seatID,
                longitude = GameCache.Instance.longitude,
                latitude = GameCache.Instance.latitude,
                clientIP = GameCache.Instance.client_ip,
                roomPath = GameCache.Instance.room_path,
                roomID = GameCache.Instance.room_id
            });
        }

        /// <summary>
        /// 强制踢出
        /// </summary>
        /// <param name="userId"></param>
        public virtual void ForcedOut(int userId)
        {
            Seat mSeat = GetSeatByUserId(userId);
            if (null == mSeat)
                return;

            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_SEND_SEAT_ACTION()
            {
                action = 5,
                seatID = mSeat.seatID,
                longitude = GameCache.Instance.longitude,
                latitude = GameCache.Instance.latitude,
                clientIP = GameCache.Instance.client_ip,
                roomPath = GameCache.Instance.room_path,
                roomID = GameCache.Instance.room_id
            });
        }

        /// <summary>
        /// 带入
        /// </summary>
        /// <param name="anteNumber"></param>
        public virtual void AddChips(int anteNumber)
        {
            if (GameCache.Instance.gold < anteNumber && GameCache.Instance.roomMode == 0)
            {
                UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                {
                    type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                    // title = $"余额不足",
                    title = CPErrorCode.LanguageDescription(10025),
                    // content = $"USDT余额不足，请先充值",
                    content = CPErrorCode.LanguageDescription(20010),
                    // contentCommit = "去充值",
                    contentCommit = CPErrorCode.LanguageDescription(10026),
                    // contentCancel = "取消",
                    contentCancel = CPErrorCode.LanguageDescription(10013),
                    actionCommit = () =>
                    {
                        //商城
                        UIMineModel.mInstance.ObtainUserInfo(pDto =>
                        {
                            if (pDto.wallet.status == 1)
                                UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletMy, pDto);
                            else if (pDto.wallet.status == 2)
                            {
                                var tLangs = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIWallet_ClubStop_D01");
                                UIMineModel.mInstance.SetDialogShow(tLangs, delegate
                                {
                                    UIMineModel.mInstance.ShowSDKCustom();
                                });
                            }
                        });
                        // UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletMy);
                    },
                    actionCancel = null
                });
                return;
            }

            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_ADD_CHIPS()
            {
                seatID = mainPlayer.seatID,
                unknow = 1,
                longitude = GameCache.Instance.longitude,
                latitude = GameCache.Instance.latitude,
                clientIP = GameCache.Instance.client_ip,
                anteNumber = anteNumber,
                roomPath = GameCache.Instance.room_path,
                roomId = GameCache.Instance.room_id,
                anteType = 0,
                clubID = 0,
                managerID = 0,
                uuid = SystemInfoUtil.getDeviceUniqueIdentifier(),
                mac = SystemInfoUtil.getLocalMac(),
                simulator = SystemInfoUtil.isSimulator,
                locationName = GameCache.Instance.locationName
            });
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="go"></param>
        private void onClickNeedUpdate(GameObject go)
        {
            if (updateStatus == 0)
            {
                // 没有更新，状态异常
            }
            else if (updateStatus == 1)
            {
                InstallPacketDownloaderComponent mInstallPacketDownloaderComponent = Game.Scene.ModelScene.GetComponent<InstallPacketDownloaderComponent>();
                // 更新安装包
                Application.OpenURL(mInstallPacketDownloaderComponent.remoteInstallPacketConfig.IOSUrl);
            }
            else if (updateStatus == 2)
            {
                // 更新热更包
                UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                {
                    type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                    title = string.Empty,
                    // content = "将为您关闭APP，重新打开即可更新",
                    content = CPErrorCode.LanguageDescription(20064),
                    // contentCommit = "确定",
                    contentCommit = CPErrorCode.LanguageDescription(10012),
                    // contentCancel = "取消",
                    contentCancel = CPErrorCode.LanguageDescription(10013),
                    actionCommit = () => { Application.Quit(); },
                    actionCancel = null
                });
            }
        }

        /// <summary>
        /// 补盲
        /// </summary>
        public virtual void onClickWaitBlind(GameObject go)
        {
            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_WAIT_BLIND()
            {
                roomID = GameCache.Instance.room_id,
                roomPath = GameCache.Instance.room_path,
                option = 1,
            });
        }

        /// <summary>
        /// 操作
        /// </summary>
        /// <param name="action"></param>
        /// <param name="anteNumber"></param>
        public virtual void OptAction(sbyte action, int anteNumber)
        {
            cacheMainPlayerStatus = action;
            // 下注putchip = 1,跟注call = 2,加注raise = 3,全下allin = 4,让牌check = 5,弃牌fold = 6,超时timeout = 7
            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_SEND_ACTION()
            {
                action = action,
                roomPath = GameCache.Instance.room_path,
                roomID = GameCache.Instance.room_id,
                anteNumber = anteNumber,
                operationRound = operationRound
            });
        }

        /// <summary>
        /// 扑克牌Sprite
        /// </summary>
        public virtual ReferenceCollector RcPokerSprite
        {
            get
            {
                return rcPokerSprite;
            }
        }

        /// <summary>
        /// 回桌
        /// </summary>
        /// <param name="seatId"></param>
        public virtual void BackDesk(int seatId)
        {
            if (seatId != mainPlayer.seatID)
                return;

            // 弹代入框
            UIComponent.Instance.Show(UIType.UIAddChips, new UIAddChipsComponent.AddClipsData()
            {
                qianzhu = groupBet,
                bigBlind = bigBlind,
                smallBlind = smallBlind,
                currentMinRate = currentMinRate,
                currentMaxRate = currentMaxRate,
                totalCoin = GameCache.Instance.gold,
                tableChips = mainPlayer.chips,
                isNeedCost = roomMode == 0 ? 1 : 0
            });
        }

        public virtual void CheckPlayerInfo(int userId)
        {
            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_CHECK_CAN_CICK()
            {
                roomID = GameCache.Instance.room_id,
                roomPath = GameCache.Instance.room_path,
                userID = userId
            });
        }

        /// <summary>
        /// 通过服务器位置获取位置对象
        /// </summary>
        /// <param name="seatId"></param>
        /// <returns></returns>
        public virtual Seat GetSeatById(sbyte seatId)
        {
            Seat mSeat = null;
            if (seatId >= 0 && seatId < listSeat.Count)
                mSeat = listSeat[seatId];
            return mSeat;
        }

        /// <summary>
        /// 通过UserId获取位置对象
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual Seat GetSeatByUserId(int userId)
        {
            if (userId <= 0)
                return null;

            Seat mSeat = null;
            for (int i = 0, n = listSeat.Count; i < n; i++)
            {
                mSeat = listSeat[i];
                if (null != mSeat && null != mSeat.Player && mSeat.Player.userID == userId)
                {
                    return mSeat;
                }
            }

            return null;
        }

        /// <summary>
        /// 通过昵称获取位置对象，尽量不要用
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual Seat GetSeatByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return null;

            Seat mSeat = null;
            for (int i = 0, n = listSeat.Count; i < n; i++)
            {
                mSeat = listSeat[i];
                if (null != mSeat && null != mSeat.Player && mSeat.Player.nick == userName)
                {
                    return mSeat;
                }
            }

            return null;
        }

        /// <summary>
        /// 通过客户端位置获取位置对象
        /// </summary>
        /// <param name="clientSeatId"></param>
        /// <returns></returns>
        public virtual Seat GetSeatByClientId(int clientSeatId)
        {
            Seat mSeat = null;
            dicSeatOnlyClient.TryGetValue(clientSeatId, out mSeat);
            return mSeat;
        }

        public virtual Player MainPlayer
        {
            get
            {
                return mainPlayer;
            }
        }

        /// <summary>
        /// 当前玩法的手牌数量
        /// </summary>
        public virtual int HandCards
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// 当前用户是否有坐下
        /// </summary>
        /// <returns></returns>
        public virtual bool UserSitdown()
        {
            if (null != mainPlayer)
            {
                return mainPlayer.seatID != -1;
            }

            return false;
        }

        /// <summary>
        /// 是否有激进操作
        /// </summary>
        /// <returns></returns>
        public virtual bool RaiseBeforeFlop()
        {
            int mStraddle = bigBlind * 2;
            if (isCurStraddle != 0)
                mStraddle = 0;
            int mAllGroupBet = 0;
            Seat mSeat = null;
            for (int i = 0, n = listSeat.Count; i < n; i++)
            {
                mSeat = listSeat[i];
                if (null == mSeat || null == mSeat.Player)
                    continue;
                if (mSeat.Player.canPlayStatus == 1)
                    mAllGroupBet += groupBet;
            }
            return alreadAnte > (bigBlind + smallBlind + mStraddle + mAllGroupBet);
        }

        public virtual int GroupBetBeforeFlop()
        {
            int mAllGroupBet = 0;
            Seat mSeat = null;
            for (int i = 0, n = listSeat.Count; i < n; i++)
            {
                mSeat = listSeat[i];
                if (null == mSeat || null == mSeat.Player)
                    continue;
                if (mSeat.Player.canPlayStatus == 1)
                    mAllGroupBet += groupBet;
            }
            return mAllGroupBet;
        }


        // 刷新房间数据
        public virtual void UpdateRoom(object obj)
        {
            REQ_GAME_ENTER_ROOM rec = obj as REQ_GAME_ENTER_ROOM;
            if (null == rec)
                return;

            UpdateRoomCommon(rec, obj);
        }

        public virtual void UpdateRoomCommon(REQ_GAME_ENTER_ROOM rec, object obj)
        {
            waittingUpdatePublicCardsAnimation = false;
            isAllinGetPlayerCards = false;
            fuck4thPCardByInsuranceState = 0;

            if (null != listSeat && listSeat.Count > 0)
            {
                // UIComponent.Instance.Toast("重连Game EnterRoom成功");

                // 处理一下红包雨动画
                Game.EventSystem.Run(EventIdType.Hongbaoyu_AllStop);

                // 处理一下跑马灯
                ClearMarqueeGame();

                // 清掉所有动画，避免极端条件下，动画结束的操作覆盖重置后的方法
                KillAllTweener(true);

                ClearAllData();

                HideStartBtn();
                HideCancelTrustBtn();
                HideSeeMorePublic();
                HideSelectSeatTips();
                HideWaitForStartTips();
                HideOperationPanel();
                HideWaitBlindBtn();
                HideAutoOperationPanel();
                UIComponent.Instance.HideNoAnimation(UIType.UIPause);
                if (null != imageInsurance && imageInsurance.gameObject.activeInHierarchy)
                    imageInsurance.gameObject.SetActive(false);
                HideSeeMorePublicTips();
                if (null != armatureBustCard && armatureBustCard.gameObject.activeInHierarchy)
                    armatureBustCard.gameObject.SetActive(false);
                UIComponent.Instance.HideNoAnimation(UIType.UIVoice);
                UIComponent.Instance.HideNoAnimation(UIType.UIAddChips);
                UIComponent.Instance.HideNoAnimation(UIType.UIInsurance);

                ResetPublicCardsId();
                ResetPublicCardsImage();
                // ResetShowCardsId();
            }
            else
            {
                // 初始化位置
                InitSeatByCount(rec.playerIDArray.Count);

                // 跑马灯活动按钮
                HttpRequestComponent.Instance.Send(WEB2_raffle_status_query.API,
                                                   WEB2_raffle_status_query.Request(new WEB2_raffle_status_query.RequestData()), json =>
                                                   {
                                                       var tDto = WEB2_raffle_status_query.Response(json);
                                                       if (tDto.status == 0)
                                                       {
                                                           OpenMarquee(tDto.data == 1);
                                                       }
                                                   });
            }

            waittingGPSCallback = false;
            bShortestTimeCounting = false;
            shortestTime = 0;
            bFirstSit = operationRound == 0;

            ClearAllPlayers();  // 清空玩家数据
            mainPlayer = ComponentFactory.CreateWithId<Player>(GameCache.Instance.nUserId);
            mainPlayer.seatID = rec.mySeatID;
            mainPlayer.sex = GameCache.Instance.sex;
            mainPlayer.headPic = GameCache.Instance.headPic;
            mainPlayer.nick = GameCache.Instance.nick;
            mainPlayer.userID = GameCache.Instance.nUserId;
            mainPlayer.chips = rec.nowChip;
            mainPlayer.SetCards(GetEnptyHandCards());

            gamestatus = rec.gamestatus;
            bigIndex = rec.bigIndex;
            smallIndex = rec.smallIndex;
            bankerIndex = rec.bankerIndex;
            operationID = rec.operationID;

            AddPublicCards(rec.cards);
            bigBlind = rec.bigBlind;
            smallBlind = rec.smallBlind;
            alreadAnte = rec.alreadAnte;
            roomOwner = rec.roomOwner;
            maxPlayTime = rec.maxPlayTime;
            bControl = rec.bControl;
            roomLeftTime = rec.roomLeftTime;
            pauseLeftTime = rec.pauseLeftTime;
            currentMinRate = rec.currentMinRate;
            currentMaxRate = rec.currentMaxRate;
            if (rec.operationID > -1 && rec.operationID < rec.playerIDArray.Count)
            {
                leftOperateTime = rec.leftOperateTime;
                noLeftOperateTime = false;
            }
            else
            {
                noLeftOperateTime = true;
            }

            opTime = rec.opTime;
            showCardsId = rec.showCardsId;
            groupBet = rec.groupBet;
            leftSecs = rec.leftSecs;
            pots = rec.pots;
            minAnteNum = rec.minAnteNum;
            canRaise = rec.canRaise;
            potNumber = rec.potNumber;
            firstBet = rec.firstBet;
            lastPlayerBet = rec.lastPlayerBet;
            round = rec.round;
            callnum = rec.callnum;
            foldnum = rec.foldmum;
            insurance = rec.insurance;
            waitBlind = rec.waitBlind;
            leftCreditValue = rec.leftCreditValue;
            roomType = rec.roomType;
            roomMode = rec.roomMode;
            isTrusted = rec.isTrusted;
            callAmount = rec.callAmount;
            isKeptSeat = rec.isKeptSeat;
            seeCardPlayerName = rec.seeCardPlayerName;
            isIpRestrictions = rec.isIpRestrictions;
            isGPSRestrictions = rec.isGPSRestrictions;
            tribeId = rec.tribeId;
            bringIn = rec.bringIn;
            preLeave = rec.preLeave;
            bSpectatorsVoice = rec.bSpectatorsVoice;
            ServerVersion = rec.ServerVersion;
            canPreLeave = rec.canPreLeave;
            operationRound = rec.operationRound;
            isOrientationTribe = rec.isOrientationTribe;
            serviceFeePer = rec.serviceFeePer;
            bCreditControl = rec.bCreditControl;

            GameCache.Instance.roomMode = roomMode;

            if (waitBlind == 1)
            {
                ShowWaitBlindBtn();
            }
            else
            {
                HideWaitBlindBtn();
            }

            string[] mNicknameArray = rec.nick.Split(new string[] { "@%" }, StringSplitOptions.None);
            string[] mHeadPicArray = rec.headPicString.Split(new string[] { "@%" }, StringSplitOptions.None);
            // 显示可用位置
            Seat mSeat = null;
            for (int i = 0, n = listSeat.Count; i < n; i++)
            {
                mSeat = listSeat[i];
                mSeat.seatID = (sbyte)i;

                mSeat.FsmLogicComponent.SM.ChangeState(SeatIdle<Entity>.Instance);

                int mPlayerId = rec.playerIDArray[i];
                if (mPlayerId == -1)
                {
                    mSeat.FsmLogicComponent.SM.ChangeState(SeatEmpty<Entity>.Instance);
                    continue;
                }

                int mStatus = rec.statusArray[i];
                int mAnte = rec.anteArray[i];
                string mNickname = mNicknameArray[i];
                int mChips = rec.chipsArray[i];
                string mHeadPic = mHeadPicArray[i];
                mSeat.isBig = bigIndex == i;
                mSeat.isSmall = smallIndex == i;
                if (roomType == 21 || roomType == 23)
                {
                    mSeat.isBig = false;
                    mSeat.isSmall = false;
                }
                mSeat.isBank = bankerIndex == i;
                mSeat.isStraddle = false;
                sbyte mSex = rec.sexArray[i];
                int mCanPlayStatus = rec.canPlayStatus != null ? rec.canPlayStatus[i] : (mPlayerId != -1 ? 1 : 0);
                int mSeatStatus = rec.seatStatus != null ? rec.seatStatus[i] : 0;
                int mKeptTime = rec.keptTimeArray != null ? rec.keptTimeArray[i] : -1;
                int mHoldingSeatLeftTime = rec.holdingSeatLeftTimeArray != null ? rec.holdingSeatLeftTimeArray[i] : 0;

                mSeat.keepSeatLeftTime = mKeptTime;

                Player mPlayer = ComponentFactory.CreateWithId<Player>(mPlayerId);
                mPlayer.seatID = (sbyte)i;
                mPlayer.sex = mSex;
                mPlayer.headPic = mHeadPic;
                mPlayer.nick = mNickname;
                mPlayer.userID = mPlayerId;
                mPlayer.chips = mChips;
                mPlayer.canPlayStatus = mCanPlayStatus;
                mPlayer.status = mStatus;
                mPlayer.ante = mAnte;
                mPlayer.anteNumber = mAnte;
                mPlayer.SetCards(GetHandCardsAtEnterRoom(obj, i));
                mSeat.Player = mPlayer;

                if (rec.mySeatID == i)
                {
                    if (null != mainPlayer)
                    {
                        mainPlayer.Dispose();
                        mainPlayer = null;
                    }
                    mainPlayer = mSeat.Player;
                }

                mSeat.UpdateFSMbyStatus();
            }

            buttonStart.gameObject.SetActive(gamestatus == -2 && roomOwner == GameCache.Instance.nUserId);

            buttoninvite.gameObject.SetActive(gamestatus == -2 && GameCache.Instance.roomMode == 2);

            imageWaitForStartTips.gameObject.SetActive(gamestatus == -2 && roomOwner != GameCache.Instance.nUserId);
            UpdateAlreadAnte();
            UpdateRoomDes();
            SetUpLogo();
            UpdatePublicCardsNoAnim();

            if (GameCache.Instance.jackPot_on == 0 || roomMode > 0)
            {
                rc.Get<GameObject>("UIJackPotLabel").SetActive(false);
            }
            else
            {
                rc.Get<GameObject>("UIJackPotLabel").SetActive(true);
                jackPotLabel.SetNumber(GameCache.Instance.jackPot_fund, false);
            }

            buttonMarqueeGame.gameObject.SetActive(roomMode == 0);
            buttonMsg.gameObject.SetActive(roomMode == 1);
            Image_MsgRedPoint.gameObject.SetActive(false);

            mSeat = GetSeatById(mainPlayer.seatID);

            if (null != mSeat)
            {
                ResetSeatUIInfo(mSeat.ClientSeatId);
            }

            // 占座
            if (mainPlayer.status == 18)
            {
                // 弹代入框
                UIComponent.Instance.Show(UIType.UIAddChips,
                                          new UIAddChipsComponent.AddClipsData()
                                          {
                                              qianzhu = groupBet,
                                              bigBlind = bigBlind,
                                              smallBlind = smallBlind,
                                              currentMinRate = currentMinRate,
                                              currentMaxRate = currentMaxRate,
                                              totalCoin = GameCache.Instance.gold,
                                              tableChips = mainPlayer.chips,
                                              isNeedCost = roomMode == 0 ? 1 : 0
                                          });
            }

            // 如果有让牌操作的时候点弃牌会出现弹框，先隐藏
            UI mTmpDialog = UIComponent.Instance.Get(UIType.UIDialog);
            if (null != mTmpDialog && mTmpDialog.GameObject.activeInHierarchy)
            {
                UIComponent.Instance.HideNoAnimation(UIType.UIDialog);
            }

            //当前操作人
            if (rec.operationID != -1)
            {
                mSeat = GetSeatById(rec.operationID);
                if (null != mSeat && null != mSeat.Player)
                {
                    if (mSeat.seatID == mainPlayer.seatID && mSeat.Player.userID == mainPlayer.userID && mainPlayer.canPlayStatus == 1)
                    {
                        //自己操作中
                        HideAutoOperationPanel();   // 隐藏预操作
                        ShowOperationPanel(new UIOperationComponent.OperationData()
                        {
                            firstRound = GetCurPublicCardsCount() == 0,
                            minAnteNum = minAnteNum,
                            canRaise = canRaise,
                            callAmount = callAmount
                        }, rec.userDelayTimes);
                    }
                    else
                    {
                        // 下一个操作不是自己
                        HideOperationPanel();
                        if (mainPlayer.canPlayStatus == 1)
                        {
                            // 自己有参与游戏,但allin弃牌不显示
                            if ((mainPlayer.status > 0 && mainPlayer.status < 6 && mainPlayer.status != 4 || mainPlayer.status == 10) && mainPlayer.trust == 0)
                            {
                                UIComponent.Instance.ShowNoAnimation(UIType.UIAutoOperation, new UIAutoOperationComponent.AutoOperationData()
                                {
                                    callAmount = callAmount
                                });
                            }
                            else
                            {
                                HideAutoOperationPanel();
                            }
                        }
                        else
                        {
                            // 观众
                            HideAutoOperationPanel();
                        }
                    }
                    mSeat.FsmLogicComponent.SM.ChangeState(SeatOperation<Entity>.Instance);
                }
            }
            else
            {
                HideOperationPanel();
                HideAutoOperationPanel();
            }

            // 暂停
            if (pauseLeftTime > 0)
            {
                bPaused = true;
                UIComponent.Instance.ShowNoAnimation(UIType.UIPause, new UIPauseComponent.PauseData()
                {
                    roomPath = GameCache.Instance.room_path,
                    roomId = GameCache.Instance.room_id,
                    remainingTime = pauseLeftTime,
                    isOwner = roomOwner == GameCache.Instance.nUserId
                });

                // 需要暂停其他地方的倒计时
                PauseCountDown();
            }

            // 进房后请求托管
            SendTrustAction(2, 0);

            UpdatePots();

            CheckUpdate();

            CheckMsg();
            ETModel.Game.Hotfix.OnServerMes += OnServerMsg;
        }

        private void OnServerMsg(string obj)
        {
            if (obj.Equals("[cmd@%new_message"))
            {                //新消息
                if (Image_MsgRedPoint != null)
                    Image_MsgRedPoint.gameObject.SetActive(true);
            }
        }

        private void CheckMsg()
        {
            //检查消息的红点
            UIMineModel.mInstance.APIMessagesNumsByType(1, nums =>
            {
                if (Image_MsgRedPoint != null)
                    Image_MsgRedPoint.gameObject.SetActive(nums>0);
            });
        }

        public virtual bool ShouldAllowReconectRoom()
        {
            // 是否允许重连
            return true;
        }

        protected virtual void PauseCountDown()
        {
            // 操作面板倒计时
            UI mUIOperationComponentUI = UIComponent.Instance.Get(UIType.UIOperation);
            if (null != mUIOperationComponentUI)
            {
                UIOperationComponent mUIOperationComponent = mUIOperationComponentUI.UiBaseComponent as UIOperationComponent;
                mUIOperationComponent.PauseCountDown();
            }

            // 保险界面倒计时
            UI mUIInsuranceComponentUI = UIComponent.Instance.Get(UIType.UIInsurance);
            if (null != mUIInsuranceComponentUI)
            {
                UIInsuranceComponent mUIInsuranceComponent = mUIInsuranceComponentUI.UiBaseComponent as UIInsuranceComponent;
                mUIInsuranceComponent.PauseCountDown();
            }

            Seat mSeat = null;
            // 操作人倒计时
            for (int i = 0, n = listSeat.Count; i < n; i++)
            {
                mSeat = listSeat[i];
                if (null == mSeat || null == mSeat.Player)
                    continue;

                mSeat.PauseCountDown();
            }

            // 最短上座倒计时
            bShortestTimeCounting = false;
        }

        protected virtual void ContinueCountDown()
        {
            // 操作面板倒计时
            UI mUIOperationComponentUI = UIComponent.Instance.Get(UIType.UIOperation);
            if (null != mUIOperationComponentUI && mUIOperationComponentUI.GameObject.activeInHierarchy)
            {
                UIOperationComponent mUIOperationComponent = mUIOperationComponentUI.UiBaseComponent as UIOperationComponent;
                mUIOperationComponent.ContinueCountDown();
            }

            // 保险界面倒计时
            UI mUIInsuranceComponentUI = UIComponent.Instance.Get(UIType.UIInsurance);
            if (null != mUIInsuranceComponentUI && mUIInsuranceComponentUI.GameObject.activeInHierarchy)
            {
                UIInsuranceComponent mUIInsuranceComponent = mUIInsuranceComponentUI.UiBaseComponent as UIInsuranceComponent;
                mUIInsuranceComponent.ContinueCountDown();
            }

            Seat mSeat = null;
            // 操作人倒计时
            for (int i = 0, n = listSeat.Count; i < n; i++)
            {
                mSeat = listSeat[i];
                if (null == mSeat || null == mSeat.Player)
                    continue;

                mSeat.ContinueCountDown();
            }

            // 最短上座倒计时
            if (shortestTime > 0)
                bShortestTimeCounting = true;
        }

        /// <summary>
        /// 清空所有玩家信息
        /// </summary>
        protected virtual void ClearAllPlayers()
        {
            if (null != mainPlayer)
            {
                mainPlayer.Dispose();
                mainPlayer = null;
            }

            Seat mSeat = null;
            for (int i = 0, n = listSeat.Count; i < n; i++)
            {
                mSeat = listSeat[i];
                if (null == mSeat)
                    continue;

                if (null == mSeat.Player)
                    continue;

                mSeat.Player.Dispose();
                mSeat.Player = null;
            }

            
        }

        protected virtual List<sbyte> GetEnptyHandCards()
        {
            return new List<sbyte>() { -1, -1 };
        }

        protected virtual List<sbyte> GetHandCardsAtEnterRoom(object obj, int index)
        {
            REQ_GAME_ENTER_ROOM rec = obj as REQ_GAME_ENTER_ROOM;
            sbyte mFirstCard = rec.firstCardarray[index];
            sbyte mSecondCard = rec.SecondArray[index];
            return new List<sbyte>() { mFirstCard, mSecondCard };
        }

        protected virtual List<sbyte> GetHandCardsAtRecvWinner(object obj, int index)
        {
            REQ_GAME_RECV_WINNER rec = obj as REQ_GAME_RECV_WINNER;
            sbyte mFirstCard = rec.firstCardArray[index];
            sbyte mSecondCard = rec.secondCardArray[index];
            return new List<sbyte>() { mFirstCard, mSecondCard };
        }

        protected virtual List<sbyte> GetHandCardsAtRecvStartInfo(object obj, int index)
        {
            REQ_GAME_RECV_START_INFOR rec = obj as REQ_GAME_RECV_START_INFOR;
            sbyte mFirstCard = rec.firstCardArray[index];
            sbyte mSecondCard = rec.SecondCardArray[index];
            return new List<sbyte>() { mFirstCard, mSecondCard };
        }

        /// <summary>
        /// 是否已经开始
        /// </summary>
        /// <returns></returns>
        public virtual bool HasStarted()
        {
            if (buttonStart.gameObject.activeInHierarchy || imageWaitForStartTips.gameObject.activeInHierarchy)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public virtual void ShowOperationPanel(UIOperationComponent.OperationData operationData, int delay = 0)
        {
            buttonDelay.gameObject.SetActive(true);
            delayCount = delay;
            UpdateDelayBtn();
            UIComponent.Instance.ShowNoAnimation(UIType.UIOperation, operationData);
        }

        public virtual void HideOperationPanel()
        {
            buttonDelay.gameObject.SetActive(false);
            if (UIComponent.Instance.Get(UIType.UIOperation).GameObject.activeInHierarchy)
                UIComponent.Instance.HideNoAnimation(UIType.UIOperation);
        }

        public virtual void HideAutoOperationPanel()
        {
            if (UIComponent.Instance.Get(UIType.UIAutoOperation).GameObject.activeInHierarchy)
            {
                UIComponent.Instance.HideNoAnimation(UIType.UIAutoOperation);
            }
        }

        public virtual void ShowWaitBlindBtn()
        {
            //现在是服务端补盲，前端不需要显示
            buttonWaitBlind.gameObject.SetActive(false);

            //if (null == buttonWaitBlind || buttonWaitBlind.gameObject.activeInHierarchy)
            //    return;

            //buttonWaitBlind.gameObject.SetActive(true);
        }

        public virtual void HideWaitBlindBtn()
        {
            if (null == buttonWaitBlind || !buttonWaitBlind.gameObject.activeInHierarchy)
                return;

            buttonWaitBlind.gameObject.SetActive(false);
        }

        public virtual void HideStartBtn()
        {
            if (buttonStart.gameObject.activeInHierarchy)
            {
                buttonStart.gameObject.SetActive(false);
            }
            if (buttoninvite.gameObject.activeInHierarchy)
            {
                buttoninvite.gameObject.SetActive(false);
            }
        }

        public virtual void HideCancelTrustBtn()
        {
            if (buttonCancelTrust.gameObject.activeInHierarchy)
            {
                buttonCancelTrust.gameObject.SetActive(false);
            }
        }

        public virtual void UpdateDelayBtn()
        {
            buttonDelay.gameObject.transform.Find("Text_DelayTip").GetComponent<Text>().text = $"{StringHelper.ShowGold(AddTimeCost())}";
        }

        /// <summary>
        /// 回收筹码位置的世界坐标
        /// </summary>
        /// <returns></returns>
        public virtual Vector3 GetRecyclingChipPosV3(int targetType)
        {
            Vector3 mV3 = Vector3.zero;
            if (targetType == -1 || null == listPotInfo || targetType >= listPotInfo.Count)
                mV3 = rc.transform.TransformPoint(textAlreadAnte.transform.localPosition);
            else
                mV3 = transPot.TransformPoint(GameUtil.TexasPots[targetType]);
            return mV3;
        }

        /// <summary>
        /// 弃牌动画目标点
        /// </summary>
        /// <returns></returns>
        public Vector3 GetMyFoldPosV3()
        {
            return rc.transform.TransformPoint(imageLogo.transform.localPosition);
        }

        public virtual int GetOpTime()
        {
            if (noLeftOperateTime == false && leftOperateTime > 0)
            {
                noLeftOperateTime = true;
                return leftOperateTime;
            }

            return opTime;
        }

        public void StopRecord(bool isSend)
        {
            uiChat.StopRecord(isSend);
        }

        public bool PlayRecordByUserId(string userId, long duration)
        {
            int mUserId = -1;
            if (int.TryParse(userId, out mUserId))
            {
                Seat mSeat = GetSeatByUserId(mUserId);
                if (null != mSeat)
                {
                    mSeat.PlayRecord(duration);
                    return true;
                }
            }
            return false;
        }

        public void StopAllRecord()
        {
            Seat mSeat = null;
            for (int i = 0, n = listSeat.Count; i < n; i++)
            {
                mSeat = listSeat[i];
                mSeat.StopRecord();
            }
        }

        /// <summary>
        /// 设置桌布样式
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool SetDeskType(int index)
        {
            if (index > DeskTypeAbnames.Length - 1)
                return false;

            ResourcesComponent resourcesComponent = Game.Scene.ModelScene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle($"{DeskTypeAbnames[index]}");
            if (null == settingAbnames)
                settingAbnames = new List<string>();
            settingAbnames.Add(DeskTypeAbnames[index]);
            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{DeskTypeAbnames[index]}", $"{DeskTypeObjnames[index]}");
            if (null != bundleGameObject)
            {
                GameObject mObj = GameObject.Instantiate(bundleGameObject);
                ReferenceCollector mRc = mObj.GetComponent<ReferenceCollector>();
                Sprite mSprite = mRc.Get<Sprite>(DeskTypeSpritenames[index]);
                if (null != mSprite)
                {
                    imageMask.sprite = mSprite;
                    GameObject.Destroy(mObj);


                    SetButtonType(index);

                    return true;
                }
                GameObject.Destroy(mObj);

                return false;
            }

            return false;
        }

        /// <summary>
        /// 设置按钮样式
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool SetButtonType(int index)
        {
            if (index > ButtonTypeSpritenames.Length - 1)
                return false;

            Sprite mSprite = this.rc.Get<Sprite>(ButtonTypeSpritenames[index]);
            Image mImage = null;
            Transform mTransform = null;

            if (null != buttonMenu)
                mTransform = buttonMenu.transform;
            if (null != mTransform)
                mImage = mTransform.GetComponent<Image>();
            if (null != mImage && null != mSprite)
                mImage.sprite = mSprite;

            mImage = null;
            mTransform = null;
            if (null != buttonCurSituation)
                mTransform = buttonCurSituation.transform;
            if (null != mTransform)
                mImage = mTransform.GetComponent<Image>();
            if (null != mImage && null != mSprite)
                mImage.sprite = mSprite;

            mImage = null;
            mTransform = null;
            if (null != buttonReport)
                mTransform = buttonReport.transform;
            if (null != mTransform)
                mImage = mTransform.GetComponent<Image>();
            if (null != mImage && null != mSprite)
                mImage.sprite = mSprite;

            mImage = null;
            mTransform = null;
            if (null != buttonDelay)
                mTransform = buttonDelay.transform;
            if (null != mTransform)
                mImage = mTransform.GetComponent<Image>();
            if (null != mImage && null != mSprite)
                mImage.sprite = mSprite;

            mImage = null;
            mTransform = null;
            if (null != buttonVoice)
                mTransform = buttonVoice.transform;
            if (null != mTransform)
                mImage = mTransform.GetComponent<Image>();
            if (null != mImage && null != mSprite)
                mImage.sprite = mSprite;

            mImage = null;
            mTransform = null;
            if (null != buttonChat)
                mTransform = buttonChat.transform;
            if (null != mTransform)
                mImage = mTransform.GetComponent<Image>();
            if (null != mImage && null != mSprite)
                mImage.sprite = mSprite;

            return true;
        }

        /// <summary>
        /// 设置扑克牌样式
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool SetCardType(int index)
        {
            if (index > CardTypeAbnames.Length - 1)
                return false;

            ResourcesComponent resourcesComponent = Game.Scene.ModelScene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle($"{CardTypeAbnames[index]}");
            if (null == settingAbnames)
                settingAbnames = new List<string>();
            settingAbnames.Add(CardTypeAbnames[index]);
            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{CardTypeAbnames[index]}", $"{CardTypeObjnames[index]}");
            if (null != bundleGameObject)
            {
                GameObject mObj = GameObject.Instantiate(bundleGameObject);
                rcPokerSprite = mObj.GetComponent<ReferenceCollector>();

                UpdatePublicCardSettingType();
                if (null != listSeat)
                {
                    for (int i = 0, n = listSeat.Count; i < n; i++)
                    {
                        listSeat[i].UpdateCardSettingType();
                    }
                }
                GameObject.Destroy(mObj);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 播放跑马灯小游戏动画
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="content"></param>
        public void PlayMarqueeGameByUserId(int userId, string content)
        {
            marqueeGameWinCoin = string.Empty;
            int mSeatId = -1;
            Seat mSeat = null;
            for (int i = 0, n = this.listSeat.Count; i < n; i++)
            {
                mSeat = this.listSeat[i];
                if(null == mSeat || null == mSeat.Player)
                    continue;
                if(!userId.Equals(mSeat.Player.userID))
                    continue;
                mSeatId = mSeat.seatID;
                break;
            }
            if (mSeatId != -1)
            {
                // PlayMarqueeGame(mSeatId);
                // marqueeGameWinCoin = CPErrorCode.LanguageDescription(20078, new List<object>(){mSeat.Player.nick, content});

                PlayKingRun(mSeatId);
               // content = StringHelper.ShowGold(int.Parse(content));
                marqueeGameWinCoin = CPErrorCode.LanguageDescription(20078, new List<object>() { mSeat.Player.nick, content });
            }
        }

        protected void PlayMarqueeGameBySeatId(sbyte seatId, string content)
        {
            marqueeGameWinCoin = string.Empty;
            Seat mSeat = GetSeatByClientId(seatId);
            if (null == mSeat)
                return;

            PlayKingRun(seatId);
            // marqueeGameWinCoin = CPErrorCode.LanguageDescription(20078, new List<object>() { mSeat.Player.nick, content });
            marqueeGameWinCoin = CPErrorCode.LanguageDescription(20078, new List<object>() { seatId.ToString(), content });

        }

        /// <summary>
        /// 播放跑马灯小游戏动画
        /// </summary>
        /// <param name="seatId"></param>
        public void PlayMarqueeGame(int seatId)
        {
            if (null == this.listMarqueeGameSeats)
                listMarqueeGameSeats = new List<Seat>();
            if(this.listMarqueeGameSeats.Count > 0)
                listMarqueeGameSeats.Clear();
            isInMarqueeGame = false;
            curMarqueeGameTimer = Time.time;
            curMarqueeGameSeatIndex = 0;
            curMoveRound = 0;
            curMoveCount = 0;
            maxMoveRound = arrMarqueeGameInterval.Length;
            int mOffset = 0;
            Seat mSeat = null;
            for (int i = 0, n = listSeat.Count; i < n; i++)
            {
                mSeat = this.listSeat[i];
                if (null == mSeat)
                    continue;

                if(null == mSeat.Player)
                    continue;
                
                if (mSeat.Player.isPlaying)
                {
                    this.listMarqueeGameSeats.Add(listSeat[i]);
                    if (mSeat.Player.seatID == seatId)
                    {
                        mOffset = this.listMarqueeGameSeats.Count;
                    }
                }

                // this.listMarqueeGameSeats.Add(listSeat[i]);
            }

            // mOffset = UnityEngine.Random.Range(0, 4);

            maxMoveCount = listMarqueeGameSeats.Count * this.maxMoveRound + mOffset;

            if (this.curMarqueeGameSeatIndex >= this.listMarqueeGameSeats.Count)
                return;

            this.isInMarqueeGame = true;

            mSeat = this.listMarqueeGameSeats[this.curMarqueeGameSeatIndex];
            if (null != mSeat)
            {
                this.armatureMarqueeGame.transform.localPosition = GameUtil.ChangeToLocalPos(mSeat.Trans, this.armatureMarqueeGame.transform);
                this.armatureMarqueeGame.gameObject.SetActive(true);
                this.armatureMarqueeGame.dragonAnimation.Play("horse_race_lamp");
            }
        }

        /// <summary>
        /// 停止跑马灯小游戏动画
        /// </summary>
        public void StopMarqueeGame()
        {
            isInMarqueeGame = false;
            if(null != this.listMarqueeGameSeats)
                listMarqueeGameSeats.Clear();
        }

        /// <summary>
        /// 清空跑马灯小游戏数据
        /// </summary>
        public void ClearMarqueeGame()
        {
            isInMarqueeGame = false;
            if (null != listMarqueeGameSeats && listMarqueeGameSeats.Count > 0)
                listMarqueeGameSeats.Clear();

            if (armatureMarqueeGame.dragonAnimation != null)
            {
                if (armatureMarqueeGame.dragonAnimation.isPlaying)
                    armatureMarqueeGame.dragonAnimation.Stop();
            }

            if (this.armatureMarqueeGame.gameObject.activeInHierarchy)
            {
                armatureMarqueeGame.gameObject.SetActive(false);
            }

            if (null != marqueePath)
            {
                if (marqueePath.IsPlaying())
                    marqueePath.Kill();
                marqueePath = null;
            }

            if (null != tweenerDaizi)
            {
                if(tweenerDaizi.IsPlaying())
                    tweenerDaizi.Kill();
                tweenerDaizi = null;
            }

            if (null != armatureDaizi && null != armatureDaizi.dragonAnimation)
            {
                if (armatureDaizi.dragonAnimation != null)
                {
                    if (armatureDaizi.dragonAnimation.isPlaying)
                        armatureDaizi.dragonAnimation.Stop();
                }
            }

            if (null != transDaizi)
            {
                transDaizi.gameObject.SetActive(false);
            }

            if (null != transKing)
            {
                transKing.gameObject.SetActive(false);
            }
        }

        #endregion

        #region Protected

        // 重置位置信息
        protected virtual void ResetSeatUIInfo(int clientSeatId)
        {
            if (clientSeatId == 0)
                return;


            dicSeatOnlyClient.Clear();
            SeatUIInfo[] mInfos = GameUtil.SeatUIInfos[listSeat.Count];
            for (int i = 0, n = mInfos.Length; i < n; i++)
            {
                Seat mSeat = listSeat[i];
                int tmp = mSeat.ClientSeatId - clientSeatId;
                if (tmp < 0)
                    tmp += mInfos.Length;
                mSeat.ClientSeatId = tmp;
                mSeat.Trans.name = $"Seat{tmp}";
                dicSeatOnlyClient.Add(tmp, mSeat);
                tweenerResetSeatUIInfo = mSeat.Trans.DOLocalMove(mInfos[tmp].Pos, 0.3f).OnComplete(() =>
                {
                    mSeat.InitSeatUIInfo(mInfos[tmp]);
                });
            }

            // PlayGameAnimation(GameAnimation.ResetSeatUIInfo);
            tweenerResetSeatUIInfo.onComplete += () =>
            {
                // StopGameAnimation(GameAnimation.ResetSeatUIInfo);
            };
        }

        /// <summary>
        /// 杀死所有DoTweener动画
        /// </summary>
        /// <param name="complete"></param>
        protected virtual void KillAllTweener(bool complete = false)
        {
            if (null != tweenerResetSeatUIInfo && tweenerResetSeatUIInfo.IsPlaying())
            {
                tweenerResetSeatUIInfo.Kill(complete);
            }

            tweenerResetSeatUIInfo = null;

            if (null != sequencePlayDealAnimation && sequencePlayDealAnimation.IsPlaying())
            {
                sequencePlayDealAnimation.Kill(complete);
            }

            sequencePlayDealAnimation = null;

            if (null != sequencePlayRecyclingChipAnimation && sequencePlayRecyclingChipAnimation.IsPlaying())
            {
                sequencePlayRecyclingChipAnimation.Kill(complete);
            }

            sequencePlayRecyclingChipAnimation = null;

            if (null != sequencePlayFirstRecyclingChipAnimation && sequencePlayFirstRecyclingChipAnimation.IsPlaying())
            {
                sequencePlayFirstRecyclingChipAnimation.Kill(complete);
            }

            sequencePlayFirstRecyclingChipAnimation = null;

            if (null != sequencePlayFirstRecyclingChipSubAnimation && sequencePlayFirstRecyclingChipSubAnimation.IsPlaying())
            {
                sequencePlayFirstRecyclingChipSubAnimation.Kill(complete);
            }

            sequencePlayFirstRecyclingChipSubAnimation = null;

            if (null != sequencePlayFirstInsurance && sequencePlayFirstInsurance.IsPlaying())
            {
                sequencePlayFirstInsurance.Kill(complete);
            }

            sequencePlayFirstInsurance = null;

            if (null != sequenceUpdatePublicCards && sequenceUpdatePublicCards.IsPlaying())
            {
                sequenceUpdatePublicCards.Kill(complete);
            }

            sequenceUpdatePublicCards = null;

            if (null != sequencePlayEndPublicCardsAnimation && sequencePlayEndPublicCardsAnimation.IsPlaying())
            {
                sequencePlayEndPublicCardsAnimation.Kill(complete);
            }

            sequencePlayEndPublicCardsAnimation = null;
        }

        #endregion

        #endregion

        /// <summary>
        /// 清空所有数据
        /// </summary>
        protected virtual void ClearAllData()
        {
            gamestatus = -1;
            bigIndex = 0;
            smallIndex = 0;
            bankerIndex = 0;
            operationID = 0;
            if (null != cards)
            {
                cards.Clear();
                cards = null;
            }
            bigBlind = 0;
            smallBlind = 0;
            alreadAnte = 0;
            roomOwner = 0;
            maxPlayTime = 0;
            bControl = 0;
            nowChip = 0;
            roomLeftTime = 0;
            pauseLeftTime = 0;
            currentMinRate = 0;
            currentMaxRate = 0;
            leftOperateTime = 0;
            opTime = 0;
            showCardsId = 0;
            groupBet = 0;
            leftSecs = 0;
            if (null != pots)
            {
                pots.Clear();
                pots = null;
            }

            minAnteNum = 0;
            canRaise = 0;
            insurance = 0;
            waitBlind = 0;
            leftCreditValue = 0;
            roomType = 0;
            needBring = 0;
            isTrusted = 0;
            callAmount = 0;
            isKeptSeat = 0;
            seeCardPlayerName = string.Empty;
            isIpRestrictions = 0;
            isGPSRestrictions = 0;
            tribeId = 0;
            bringIn = 0;
            preLeave = 0;
            bSpectatorsVoice = 0;
            ServerVersion = string.Empty;
            canPreLeave = 0;
            operationRound = 0;
            isOrientationTribe = 0;
            serviceFeePer = 0;
            bCreditControl = 0;
            isCurStraddle = 0;
            autoFold = false;
            autoCall = false;
            autoAllin = false;
            autoCheck = false;
            bFirstSit = false;
            bStandBeforeStart = false;
            bLoseAll = false;
            bPaused = false;
            lastCallAmount = 0;
            cacheMainPlayerStatus = 0;
            if (null != mainPlayer)
            {
                mainPlayer.Dispose();
                mainPlayer = null;
            }

            if (null != listSeat)
            {
                for (int i = 0, n = listSeat.Count; i < n; i++)
                {
                    if (null != listSeat[i] && null != listSeat[i].Player)
                    {
                        listSeat[i].Player.Dispose();
                        listSeat[i].Player = null;
                    }
                }
            }

            shortestTime = 0;
            bShortestTimeCounting = false;
            recordShortestTimeDeltaTime = 0;
            isExit = false;
            noLeftOperateTime = false;
            cacheClientSeatId = 0;
            delayCount = 0;
            lastBankerIndex = 0;
            cacheSitdownSeatId = 0;
            waittingGPSCallback = false;
            stopUpdatePublicCardsAnimation = false;
            waittingUpdatePublicCardsAnimation = false;
            isAllinGetPlayerCards = false;
            fuck4thPCardByInsuranceState = 0;
        }

        /// <summary>
        /// 检查更新
        /// </summary>
        protected async void CheckUpdate()
        {
            updateStatus = 0;
            // 检查App版本
            await InstallPacketHelper.CheckInstallPacket();
            if (InstallPacketHelper.NeedInstallPacket())
            {
                updateStatus = 1;
                // 提示有版本更新
                if (null != imageNeedUpdate)
                {
                    imageNeedUpdate.gameObject.SetActive(true);
                    float mTmpWidth = textNeedUpdate.preferredWidth / 2f;
                    if (null != imageNeedUpdateIcon)
                    {
                        RectTransform mRectTransform = imageNeedUpdateIcon.rectTransform;
                        mRectTransform.localPosition = new Vector3(-mTmpWidth - mRectTransform.sizeDelta.x * 0.8f, mRectTransform.localPosition.y);
                    }
                    if (null != imageWaitForStartTips)
                        imageWaitForStartTips.gameObject.SetActive(false);
                }
            }
            else
            {
                // 检查Res版本
                await BundleHelper.CheckDownloadBundle();
                if (BundleHelper.NeedDownloadBundle())
                {
                    updateStatus = 2;
                    // 提示有版本更新
                    if (null != this.imageNeedUpdate)
                    {
                        imageNeedUpdate.gameObject.SetActive(true);
                        float mTmpWidth = textNeedUpdate.preferredWidth / 2f;
                        if (null != this.imageNeedUpdateIcon)
                        {
                            RectTransform mRectTransform = imageNeedUpdateIcon.rectTransform;
                            mRectTransform.localPosition = new Vector3(-mTmpWidth - mRectTransform.sizeDelta.x * 0.8f, mRectTransform.localPosition.y);
                        }

                        if (null != this.imageWaitForStartTips)
                            imageWaitForStartTips.gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (imageNeedUpdate.gameObject.activeInHierarchy)
                        imageNeedUpdate.gameObject.SetActive(false);
                }
            }
        }

        public void OpenMarquee(bool open)
        {
            buttonMarqueeGame.gameObject.SetActive(open);
            if (roomMode > 0)
            {
                buttonMarqueeGame.gameObject.SetActive(false);
            }
        }

        public virtual void PlayKingRun(int seatId)
        {
            SeatUIInfo[] mSeatUiInfos = GameUtil.SeatUIInfos[9];
            Vector3[] mTmpVec = GetFallDownPosBySeatId(seatId);

            if (null == mTmpVec)
                return;

            int mRound = UnityEngine.Random.Range(2, 4);
            pathKing = new Vector3[mSeatUiInfos.Length * mRound + mTmpVec.Length];
            for (int start = 0, end = mRound; start < end; start++)
            {
                for (int i = 0, n = mSeatUiInfos.Length; i < n; i++)
                {
                    pathKing[mSeatUiInfos.Length * start + i] = mSeatUiInfos[i].Pos;
                }
            }

            for (int i = 0, n = mTmpVec.Length; i < n; i++)
            {
                pathKing[mSeatUiInfos.Length * mRound + i] = mTmpVec[i];
            }

            if (null == pathKing || pathKing.Length == 0)
                return;

            if (null == this.transKing)
                return;

            transKing.localPosition = pathKing[0];
            marqueePath = transKing.DOLocalPath(pathKing, 5, PathType.CatmullRom, PathMode.Full3D, 10, Color.green);
            // path.SetOptions(true, AxisConstraint.X, AxisConstraint.Y | AxisConstraint.Z);
            marqueePath.SetLookAt(0);
            marqueePath.SetEase(Ease.InSine);
            marqueePath.SetLoops(1);
            marqueePath.OnStart(() =>
            {
                if (null != transPudao)
                    transPudao.gameObject.SetActive(false);
                if (null != transYanlei)
                    transYanlei.gameObject.SetActive(false);
                transKing.gameObject.SetActive(true);
                if (null != animatorKing)
                {
                    animatorKing.SetInteger("state", 0);
                }

                SoundComponent.Instance.RepeatSFX(SoundComponent.SFX_KING_RUN, 5);
            });
            marqueePath.OnComplete(() =>
            {
                if (null != animatorKing)
                {
                    animatorKing.SetInteger("state", 1);
                    transYanlei.gameObject.SetActive(true);

                    SoundComponent.Instance.PlaySFX(SoundComponent.SFX_KING_FALLDOWN);
                }
                if (null != transPudao)
                    transPudao.gameObject.SetActive(true);
                if (null != transPudao)
                    transYanlei.gameObject.SetActive(true);
                if (null != this.transDaizi)
                {
                    transDaizi.gameObject.SetActive(true);
                    armatureDaizi.dragonAnimation.Play("yidong");
                    Seat mSeat = GetSeatById((sbyte) seatId);
                    this.transDaizi.localPosition = GameUtil.ChangeToLocalPos(mSeat.Trans, this.transDaizi);
                    tweenerDaizi = this.transDaizi.DOLocalMove(Vector3.zero, 1);
                    tweenerDaizi.OnStart(()=>
                    {
                        SoundComponent.Instance.PlaySFX(SoundComponent.SFX_DAIZI_MOVE);
                    });
                    tweenerDaizi.OnComplete(() =>
                    {
                        if (armatureDaizi.HasDBEventListener(EventObject.COMPLETE))
                        {
                            armatureDaizi.RemoveDBEventListener(EventObject.COMPLETE, ListenerDaiziTiaodongComplete);
                            armatureDaizi.RemoveDBEventListener(EventObject.COMPLETE, ListenerDaiziBaozhaComplete);
                        }
                        armatureDaizi.dragonAnimation.Play("tiaodong", 1);
                        armatureDaizi.AddDBEventListener(EventObject.COMPLETE, ListenerDaiziTiaodongComplete);

                        SoundComponent.Instance.PlaySFX(SoundComponent.SFX_DAIZI_JUMP);
                    });
                }
            });
        }

        private void ListenerDaiziTiaodongComplete(string type, EventObject eventobject)
        {
            transKing.gameObject.SetActive(false);
            armatureDaizi.RemoveDBEventListener(EventObject.COMPLETE, ListenerDaiziTiaodongComplete);
            armatureDaizi.dragonAnimation.Play("baozha", 1);
            armatureDaizi.AddDBEventListener(EventObject.COMPLETE, ListenerDaiziBaozhaComplete);

            SoundComponent.Instance.PlaySFX(SoundComponent.SFX_DAIZI_BOMB);
        }

        private void ListenerDaiziBaozhaComplete(string type, EventObject eventobject)
        {
            armatureDaizi.RemoveDBEventListener(EventObject.COMPLETE, ListenerDaiziBaozhaComplete);
            transDaizi.gameObject.SetActive(false);
            UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
            {
                    type = UIDialogComponent.DialogData.DialogType.Commit,
                    title = string.Empty,
                    content = marqueeGameWinCoin,
                    contentCommit = CPErrorCode.LanguageDescription(10012),
                    actionCommit = null,
            });
        }

        protected virtual Vector3[] GetFallDownPosBySeatId(int seatId)
        {
            Seat mSeat = GetSeatById((sbyte) seatId);
            if (null == mSeat)
                return null;

            int mStart = 0;
            int mEnd = 0;
            
            for (int i = 0, n = GameUtil.SeatPosV3.Length; i < n; i++)
            {
                Vector3 mTmpVec3 = GameUtil.SeatPosV3[i];
                if (mTmpVec3.x.Equals(mSeat.Trans.localPosition.x) &&
                    mTmpVec3.y.Equals(mSeat.Trans.localPosition.y))
                {
                    mEnd = i + 1;
                    break;
                }
            }

            if (mEnd > mStart)
            {
                Vector3[] mTarget = new Vector3[mEnd-mStart];
                for (int i = mStart, n = mEnd - 1; i < n; i++)
                {
                    // Vector3 mTmpVec3 = GameUtil.SeatPosV3[i];
                    Vector3 mTmpVec3 = GameUtil.MarquessFallDownPosV3[i];
                    mTarget[i] = mTmpVec3;
                }

                mTarget[mTarget.Length - 1] = GameUtil.MarquessFallDownPosV3[mEnd - 1];

                return mTarget;
            }

            return null;
        }

       
    }
}
