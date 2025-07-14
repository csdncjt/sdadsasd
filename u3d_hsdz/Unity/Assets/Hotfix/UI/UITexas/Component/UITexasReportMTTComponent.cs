using System;
using System.Collections.Generic;
using ETModel;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UITexasReportMTTComponentAwakeSystem : AwakeSystem<UITexasReportMTTComponent>
    {
        public override void Awake(UITexasReportMTTComponent self)
        { 
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UITexasReportMTTComponentUpdateSystem: UpdateSystem<UITexasReportMTTComponent>
    {
        public override void Update(UITexasReportMTTComponent self)
        {
            self.Update();
        }
    }

    public class UITexasReportMTTComponent : UIBaseComponent
    {
        public sealed class LoopListDeskData
        {
            public int deskNum;
            public int playerCount;
            public int minScore;
            public int maxScore;
        }

        public sealed class LoopListRewardData
        {
            public int rank;
            public string bonus;
            public double percent;
        }

        public sealed class LoopListBlindData
        {
            public int level;
            public int blind;
            public int ante;
            public int updateTime;
        }

        public sealed class LoopListRankingData
        {
            public bool isHunter;
            public int ranking;
            public string nickname;
            public int num;
            public string reward;
            public int score;
            public int userId;
            public bool isLooser;
        }

        private ReferenceCollector rc;
        private Transform transBlind;
        private Button buttonFirstPage;
        private Button buttonLastPage;
        private Button buttonNextPage;
        private Button buttonUpPage;
        private Transform transDesk;
        private Image imageAllBarJL;
        private Image imageAllBarMZ;
        private Image imageAllBarPZ;
        private Image imageAllBarSK;
        private Image imageMenuMask;
        private Transform transInfoBlindListScrollView;
        private Transform transInfoDeskListScrollView;
        private Transform transInfoRewardListScrollView;
        private Transform transListScrollView;
        private Transform transMatch;
        private Transform transNullHeight;
        private RawImage rawimageMyInfoIcon;
        private Transform transReward;
        private Text textAllBarJL;
        private Text textAllBarMZ;
        private Text textAllBarPZ;
        private Text textAllBarSK;
        private Text textMatchAverageScoreboard;
        private Text textMatchCurrBlind;
        private Text textMatchNextBlind;
        private Text textMatchNextBlindTime;
        private Text textMatchParticipants;
        private Text textMatchTotalBonus;
        private Text textMatchZmsysj;
        private Text textMyInfoNickname;
        private Text textMyInfoRanking;
        private Text textMyInfoRebuild;
        private Text textMyInfoReward;
        private Text textMyInfoScore;
        private Text textPage;
        private Text textRewardNextReward;
        private Text textRewardReward;
        private Text textRewardTotalBonus;
        private Text textTitleBarTime;
        private Toggle toggleAllBarJL;
        private Toggle toggleAllBarMZ;
        private Toggle toggleAllBarPZ;
        private Toggle toggleAllBarSK;


        private Sprite[] spritesSK;
        private Sprite[] spritesPZ;
        private Sprite[] spritesJL;
        private Sprite[] spritesMZ;
        private static Color32 togglePressed = new Color32(233, 191, 128, 255);
        private static Color32 toggleNormal = new Color32(168, 168, 168, 255);


        #region 赛况

        private float averageScoreboard;    // 平均USDT
        private int raiseBlindTime; // 升盲剩余时间
        private int playerNum;  // 总人数
        private int allLoser;  // 已经淘汰的用户数
        private int currentBlind;    // 当前盲注
        private int nextBlind;   // 下一盲注
        private int updateTime;  // 升盲时间

        #endregion

        #region 牌桌

        private int dragDirDesk = -1;
        private bool isMoreLoadDesk = false;
        private LoopListView2 loopListView2Desk;
        private LoadingTipStatus loadingTipStatusDesk = LoadingTipStatus.None;
        private float loadingTipItemHeightDesk = 100;
        private List<LoopListDeskData> listLoopDeskData = null;
        private int currentPageDesk = 1;

        #endregion

        #region 奖励

        private int dragDirReward = -1;
        private bool isMoreLoadReward = false;
        private LoopListView2 loopListView2Reward;
        private LoadingTipStatus loadingTipStatusReward = LoadingTipStatus.None;
        private float loadingTipItemHeightReward = 100;
        private int rewardNumber = 0;
        private string nextReward = string.Empty;
        private List<LoopListRewardData> listLoopRewardData = null;
        private int currentPageReward = 1;

        #endregion

        #region 盲注

        private bool isMoreLoadBlind;
        private int currentBlindLevel = 3;
        private static Color32 lessBlindLevel = new Color32(168, 168, 168, 128);
        private static Color32 equalBlindLevel = new Color32(233, 191, 128, 255);
        private static Color32 greaterBlindLevel = new Color32(168, 168, 168, 255);
        private LoopListView2 loopListView2Blind;
        private List<LoopListBlindData> listLoopBlindData = null;

        #endregion

        #region 实时排名

        private int currentPageRec = 0; // 0开始
        private int totalPage = 8;
        private bool isMoreLoadRanking;
        private static Color32 myColor = new Color32(233, 191, 128, 255);
        private static Color32 normalColor = new Color32(168, 168, 168, 255);
        private static Color32 loserColor = new Color32(168, 168, 168, 128);
        private LoopListView2 loopListView2Ranking;
        private List<LoopListRankingData> listLoopRankingData = null;
        private List<List<LoopListRankingData>> loopListRankingDatas = null;

        #endregion

        private int currentRank = 0; // 当前排名
        private int hunterReward = 0;
        private int myScore = 0;
        private int totalRebuyTime = 0;
        private int rebuyTime = 0;

        private int totalReward;    // 总奖池
        private int totalScore; // 总USDT
        private int hunterKingId;   // 猎人王用户id

        protected bool bReportTimeCounting;
        private float reportTimeDeltaTime;
        private int playedTime; // 牌局进行时间

        private float blindScale;

        private void InitAwakeData()
        {
            averageScoreboard = 0; // 平均USDT
            raiseBlindTime = 0; // 升盲剩余时间
            playerNum = 0; // 总人数
            allLoser = 0; // 已经淘汰的用户数
            currentBlind = 0; // 当前盲注
            nextBlind = 0; // 下一盲注
            updateTime = 0; // 升盲时间

            dragDirDesk = -1;
            isMoreLoadDesk = false;
            loadingTipStatusDesk = LoadingTipStatus.None;
            listLoopDeskData = new List<LoopListDeskData>();
            currentPageDesk = 1;

            dragDirReward = -1;
            isMoreLoadReward = false;
            loadingTipStatusReward = LoadingTipStatus.None;
            rewardNumber = 0;
            nextReward = string.Empty;
            listLoopRewardData = null;
            currentPageReward = 1;

            isMoreLoadBlind = false;
            currentBlindLevel = 0;
            listLoopBlindData = null;

            currentPageRec = 0; // 0开始
            totalPage = 2;
            isMoreLoadRanking = false;
            listLoopRankingData = null;
            loopListRankingDatas = new List<List<LoopListRankingData>>() { };

            currentRank = 0; // 当前排名
            hunterReward = 0;
            myScore = 0;
            totalRebuyTime = 0;
            rebuyTime = 0;

            totalReward = 0; // 总奖池
            totalScore = 0; // 总USDT
            hunterKingId = 0; // 猎人王用户id

            bReportTimeCounting = false;
            reportTimeDeltaTime = 0;
            playedTime = 0; // 牌局进行时间

            blindScale = 0;
        }

        public void Awake()
        {
            InitAwakeData();
            InitUI();

            registerHandler();
        }

        public void Update()
        {
            CountDown();

            this.textMatchZmsysj.text = TimeHelper.ShowRemainingSemicolonPure(((MTTGame)GameCache.Instance.CurGame).upBlindLeftTime);
        }

        public override void OnShow(object obj)
        {
            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_MTT_REAL_TIME()
            {
                    roomID = GameCache.Instance.room_id,
                    roomPath = GameCache.Instance.room_path,
                    currentPage = 1
            });
        }

        public override void OnHide()
        {
            
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();

            listLoopDeskData = null;        
            listLoopRewardData = null;     
            listLoopBlindData = null;
            listLoopRankingData = null;
            loopListRankingDatas = null;

            bReportTimeCounting = false;

            removeHandler();
        }

        private void registerHandler()
        {
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_MTT_REAL_TIME, HANDLER_REQ_GAME_MTT_REAL_TIME);  // MTT实时战况
        }

        private void removeHandler()
        {
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_MTT_REAL_TIME, HANDLER_REQ_GAME_MTT_REAL_TIME);
        }

        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            transBlind = rc.Get<GameObject>("Blind").transform;
            buttonFirstPage = rc.Get<GameObject>("Button_FirstPage").GetComponent<Button>();
            buttonLastPage = rc.Get<GameObject>("Button_LastPage").GetComponent<Button>();
            buttonNextPage = rc.Get<GameObject>("Button_NextPage").GetComponent<Button>();
            buttonUpPage = rc.Get<GameObject>("Button_UpPage").GetComponent<Button>();
            transDesk = rc.Get<GameObject>("Desk").transform;
            imageAllBarJL = rc.Get<GameObject>("Image_AllBarJL").GetComponent<Image>();
            imageAllBarMZ = rc.Get<GameObject>("Image_AllBarMZ").GetComponent<Image>();
            imageAllBarPZ = rc.Get<GameObject>("Image_AllBarPZ").GetComponent<Image>();
            imageAllBarSK = rc.Get<GameObject>("Image_AllBarSK").GetComponent<Image>();
            imageMenuMask = rc.Get<GameObject>("Image_MenuMask").GetComponent<Image>();
            transInfoBlindListScrollView = rc.Get<GameObject>("InfoBlindListScrollView").transform;
            transInfoDeskListScrollView = rc.Get<GameObject>("InfoDeskListScrollView").transform;
            transInfoRewardListScrollView = rc.Get<GameObject>("InfoRewardListScrollView").transform;
            transListScrollView = rc.Get<GameObject>("ListScrollView").transform;
            transMatch = rc.Get<GameObject>("Match").transform;
            transNullHeight = rc.Get<GameObject>("NullHeight").transform;
            rawimageMyInfoIcon = rc.Get<GameObject>("RawImage_MyInfoIcon").GetComponent<RawImage>();
            transReward = rc.Get<GameObject>("Reward").transform;
            textAllBarJL = rc.Get<GameObject>("Text_AllBarJL").GetComponent<Text>();
            textAllBarMZ = rc.Get<GameObject>("Text_AllBarMZ").GetComponent<Text>();
            textAllBarPZ = rc.Get<GameObject>("Text_AllBarPZ").GetComponent<Text>();
            textAllBarSK = rc.Get<GameObject>("Text_AllBarSK").GetComponent<Text>();
            textMatchAverageScoreboard = rc.Get<GameObject>("Text_MatchAverageScoreboard").GetComponent<Text>();
            textMatchCurrBlind = rc.Get<GameObject>("Text_MatchCurrBlind").GetComponent<Text>();
            textMatchNextBlind = rc.Get<GameObject>("Text_MatchNextBlind").GetComponent<Text>();
            textMatchNextBlindTime = rc.Get<GameObject>("Text_MatchNextBlindTime").GetComponent<Text>();
            textMatchParticipants = rc.Get<GameObject>("Text_MatchParticipants").GetComponent<Text>();
            textMatchTotalBonus = rc.Get<GameObject>("Text_MatchTotalBonus").GetComponent<Text>();
            textMatchZmsysj = rc.Get<GameObject>("Text_MatchZmsysj").GetComponent<Text>();
            textMyInfoNickname = rc.Get<GameObject>("Text_MyInfoNickname").GetComponent<Text>();
            textMyInfoRanking = rc.Get<GameObject>("Text_MyInfoRanking").GetComponent<Text>();
            textMyInfoRebuild = rc.Get<GameObject>("Text_MyInfoRebuild").GetComponent<Text>();
            textMyInfoReward = rc.Get<GameObject>("Text_MyInfoReward").GetComponent<Text>();
            textMyInfoScore = rc.Get<GameObject>("Text_MyInfoScore").GetComponent<Text>();
            textPage = rc.Get<GameObject>("Text_Page").GetComponent<Text>();
            textRewardNextReward = rc.Get<GameObject>("Text_RewardNextReward").GetComponent<Text>();
            textRewardReward = rc.Get<GameObject>("Text_RewardReward").GetComponent<Text>();
            textRewardTotalBonus = rc.Get<GameObject>("Text_RewardTotalBonus").GetComponent<Text>();
            textTitleBarTime = rc.Get<GameObject>("Text_TitleBarTime").GetComponent<Text>();
            toggleAllBarJL = rc.Get<GameObject>("Toggle_AllBarJL").GetComponent<Toggle>();
            toggleAllBarMZ = rc.Get<GameObject>("Toggle_AllBarMZ").GetComponent<Toggle>();
            toggleAllBarPZ = rc.Get<GameObject>("Toggle_AllBarPZ").GetComponent<Toggle>();
            toggleAllBarSK = rc.Get<GameObject>("Toggle_AllBarSK").GetComponent<Toggle>();


            NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
            float realTop = safeArea.top * 1242 / safeArea.width;
            LayoutElement mLayoutElement = this.transNullHeight.GetComponent<LayoutElement>();
            mLayoutElement.minHeight = mLayoutElement.minHeight + realTop;

            UIEventListener.Get(this.imageMenuMask.gameObject).onClick = onClickImageMenuMask;
            UIEventListener.Get(this.buttonFirstPage.gameObject).onClick = onClickFirstPage;
            UIEventListener.Get(this.buttonUpPage.gameObject).onClick = onClickUpPage;
            UIEventListener.Get(this.buttonNextPage.gameObject).onClick = onClickNextPage;
            UIEventListener.Get(this.buttonLastPage.gameObject).onClick = onClickLastPage;

            this.toggleAllBarSK.onValueChanged.AddListener(onValueChangeAllBarSK);
            this.toggleAllBarPZ.onValueChanged.AddListener(onValueChangeAllBarPZ);
            this.toggleAllBarJL.onValueChanged.AddListener(onValueChangeAllBarJL);
            this.toggleAllBarMZ.onValueChanged.AddListener(onValueChangeAllBarMZ);

            spritesSK = new Sprite[2];
            spritesSK[0] = this.rc.Get<Sprite>("mtt_button_sk_normal");
            spritesSK[1] = this.rc.Get<Sprite>("mtt_button_sk_pressed");
            spritesPZ = new Sprite[2];
            spritesPZ[0] = this.rc.Get<Sprite>("mtt_button_pz_normal");
            spritesPZ[1] = this.rc.Get<Sprite>("mtt_button_pz_pressed");
            spritesJL = new Sprite[2];
            spritesJL[0] = this.rc.Get<Sprite>("mtt_button_jl_normal");
            spritesJL[1] = this.rc.Get<Sprite>("mtt_button_jl_pressed");
            spritesMZ = new Sprite[2];
            spritesMZ[0] = this.rc.Get<Sprite>("mtt_button_mz_normal");
            spritesMZ[1] = this.rc.Get<Sprite>("mtt_button_mz_pressed");

            loopListView2Desk = this.transInfoDeskListScrollView.GetComponent<LoopListView2>();
            loopListView2Reward = this.transInfoRewardListScrollView.GetComponent<LoopListView2>();
            loopListView2Blind = this.transInfoBlindListScrollView.GetComponent<LoopListView2>();
            loopListView2Ranking = this.transListScrollView.GetComponent<LoopListView2>();
        }

        private void onValueChangeAllBarMZ(bool arg0)
        {
            if (arg0)
            {
                this.textAllBarMZ.color = togglePressed;
                this.imageAllBarMZ.sprite = this.spritesMZ[1];
                this.transBlind.gameObject.SetActive(true);
                if (this.isMoreLoadBlind == false)
                {
                    InitBlindLoopListView();
                    this.isMoreLoadBlind = true;
                }
                else
                {
                    
                }
                UpdateBlindInfo();
            }
            else
            {
                this.textAllBarMZ.color = toggleNormal;
                this.imageAllBarMZ.sprite = this.spritesMZ[0];
                this.transBlind.gameObject.SetActive(false);
            }
        }

        private void onValueChangeAllBarJL(bool arg0)
        {
            if (arg0)
            {
                this.textAllBarJL.color = togglePressed;
                this.imageAllBarJL.sprite = this.spritesJL[1];
                this.transReward.gameObject.SetActive(true);
                if (this.isMoreLoadReward == false)
                {
                    InitRewardLoopListView();
                    isMoreLoadReward = true;

                    UIMatchModel.mInstance.APIMTTRewardList(GameCache.Instance.room_id, 1, pDto =>
                    {
                        if (pDto.status == 0)
                        {
                            if (pDto.data.currMttRewardList.Count > 0)
                            {
                                this.listLoopRewardData = new List<LoopListRewardData>();
                                for (int i = 0, n = pDto.data.currMttRewardList.Count; i < n; i++)
                                {
                                    listLoopRewardData.Add(new LoopListRewardData()
                                    {
                                            rank = pDto.data.currMttRewardList[i].rank,
                                            percent = pDto.data.currMttRewardList[i].percent,
                                            bonus = pDto.data.currMttRewardList[i].bonus
                                    });
                                }

                                //刷新
                                this.currentPageReward = 1;
                                listLoopRewardData.Insert(0, new LoopListRewardData());
                                this.loopListView2Reward.SetListItemCount(listLoopRewardData.Count + 1, false);
                                loopListView2Reward.RefreshAllShownItem();

                                nextReward = pDto.data.nextReward;
                                rewardNumber = pDto.data.rewardNumber;

                                UpdateRewardInfo();
                            }
                        }
                        else
                        {
                            UIComponent.Instance.Toast(pDto.status);
                        }
                    });
                }
            }
            else
            {
                this.textAllBarJL.color = toggleNormal;
                this.imageAllBarJL.sprite = this.spritesJL[0];
                this.transReward.gameObject.SetActive(false);
            }
        }

        private void onValueChangeAllBarPZ(bool arg0)
        {
            if (arg0)
            {
                this.textAllBarPZ.color = togglePressed;
                this.imageAllBarPZ.sprite = this.spritesPZ[1];
                this.transDesk.gameObject.SetActive(true);
                if (this.isMoreLoadDesk == false)
                {
                    InitDeskLoopListView();
                    isMoreLoadDesk = true;
                }
                HttpRequestComponent.Instance.Send(WEB2_room_mtt_desk_info.API,
                                                   WEB2_room_mtt_desk_info.Request(new WEB2_room_mtt_desk_info.RequestData()
                                                   {
                                                       page = 1,
                                                       rpath = (int)RoomPath.MTT,
                                                       rmid = GameCache.Instance.room_id
                                                   }), json =>
                                                   {
                                                       var tDto = WEB2_room_mtt_desk_info.Response(json);
                                                       if (tDto.status == 0)
                                                       {
                                                           if (tDto.data.deskList.Count > 0)
                                                           {
                                                               listLoopDeskData = new List<LoopListDeskData>();
                                                               for (int i = 0, n = tDto.data.deskList.Count; i < n; i++)
                                                               {
                                                                   listLoopDeskData.Add(new LoopListDeskData()
                                                                   {
                                                                       deskNum = tDto.data.deskList[i].deskNum,
                                                                       playerCount = tDto.data.deskList[i].playerCount,
                                                                       minScore = tDto.data.deskList[i].minScore,
                                                                       maxScore = tDto.data.deskList[i].maxScore
                                                                   });
                                                               }

                                                               //刷新
                                                               currentPageDesk = 1;
                                                               listLoopDeskData.Insert(0, new LoopListDeskData());
                                                               loopListView2Desk.SetListItemCount(listLoopDeskData.Count + 1, false);
                                                               loopListView2Desk.RefreshAllShownItem();
                                                           }
                                                       }
                                                       else
                                                       {
                                                           UIComponent.Instance.Toast(tDto.status);
                                                       }
                                                   });

                UpdateDeskInfo();
            }
            else
            {
                this.textAllBarPZ.color = toggleNormal;
                this.imageAllBarPZ.sprite = this.spritesPZ[0];
                this.transDesk.gameObject.SetActive(false);
            }
        }

        private void onValueChangeAllBarSK(bool arg0)
        {
            if (arg0)
            {
                UpdateMatchInfo();
                this.textAllBarSK.color = togglePressed;
                this.imageAllBarSK.sprite = this.spritesSK[1];
                this.transMatch.gameObject.SetActive(true);
            }
            else
            {
                this.textAllBarSK.color = toggleNormal;
                this.imageAllBarSK.sprite = this.spritesSK[0];
                this.transMatch.gameObject.SetActive(false);
            }
        }

        private void onClickLastPage(GameObject go)
        {
            if (this.currentPageRec < this.totalPage - 1)
            {
                this.currentPageRec = this.totalPage - 1;
                listLoopRankingData = loopListRankingDatas[this.currentPageRec];
                loopListView2Ranking.SetListItemCount(listLoopRankingData.Count, true);
                loopListView2Ranking.RefreshAllShownItem();
                UpdatePage();
            }
        }

        private void onClickNextPage(GameObject go)
        {
            if (this.currentPageRec < this.totalPage - 1)
            {
                this.currentPageRec++;
                listLoopRankingData = loopListRankingDatas[this.currentPageRec];
                loopListView2Ranking.SetListItemCount(listLoopRankingData.Count, true);
                loopListView2Ranking.RefreshAllShownItem();
                UpdatePage();
            }
        }

        private void onClickUpPage(GameObject go)
        {
            if (this.currentPageRec > 0)
            {
                this.currentPageRec--;
                listLoopRankingData = loopListRankingDatas[this.currentPageRec];
                loopListView2Ranking.SetListItemCount(listLoopRankingData.Count, true);
                loopListView2Ranking.RefreshAllShownItem();
                UpdatePage();
            }
        }

        private void onClickFirstPage(GameObject go)
        {
            if (this.currentPageRec != 0)
            {
                this.currentPageRec = 0;
                listLoopRankingData = loopListRankingDatas[this.currentPageRec];
                loopListView2Ranking.SetListItemCount(listLoopRankingData.Count, true);
                loopListView2Ranking.RefreshAllShownItem();
                UpdatePage();
            }
        }

        private void onClickImageMenuMask(GameObject go)
        {
            UIComponent.Instance.Remove(UIType.UITexasReportMTT);
        }

        #region 赛况

        //实时战况
        protected void HANDLER_REQ_GAME_MTT_REAL_TIME(ICPResponse response)
        {
            REQ_GAME_MTT_REAL_TIME rec = response as REQ_GAME_MTT_REAL_TIME;
            if (null == rec)
            {
                return;
            }

            currentRank = rec.currentRank;
            hunterReward = rec.hunterReward;
            myScore = rec.myScore;
            totalRebuyTime = rec.totalRebuyTime;
            rebuyTime = rec.rebuyTime;

            raiseBlindTime = rec.raiseBlindTime;
            totalReward = rec.totalReward;
            totalScore = rec.totalScore;
            playerNum = rec.playerNum;
            averageScoreboard = totalScore / (float)(rec.playerNum - rec.allLoser);

            blindScale = rec.blindScale / 10;
            int blindLevel = rec.currentBlindLevel - 1;//从1开始，转一下
            if (blindLevel >= MTTGameUtil.numOfLevel(((MTTGame)GameCache.Instance.CurGame).BlindType))
            {
                blindLevel = MTTGameUtil.numOfLevel(((MTTGame)GameCache.Instance.CurGame).BlindType) - 1;
            }
            currentBlind = MTTGameUtil.BlindAtLevel(blindLevel, (MTT_GameType)0, blindScale);
            int nextBlindLevel = rec.currentBlindLevel;//从1开始，转一下
            if (nextBlindLevel >= MTTGameUtil.numOfLevel(((MTTGame)GameCache.Instance.CurGame).BlindType))
            {
                nextBlindLevel = MTTGameUtil.numOfLevel(((MTTGame)GameCache.Instance.CurGame).BlindType) - 1;
            }
            nextBlind = MTTGameUtil.BlindAtLevel(nextBlindLevel, ((MTTGame)GameCache.Instance.CurGame).BlindType, blindScale);
            updateTime = rec.updateTime;

            hunterKingId = rec.hunterKingId;
            currentPageRec = rec.currentPage;
            totalPage = rec.totalPage;
            string[] allNickName = rec.allNickName.Split(new string[] { "@%" }, StringSplitOptions.None);
            string[] allHuntGold = rec.allHuntGold.Split(new string[] { "@%" }, StringSplitOptions.None);
            if (allNickName.Length == allHuntGold.Length && allHuntGold.Length == rec.allRank.Count && rec.allRank.Count == rec.allUserId.Count && rec.allUserId.Count == rec.allCoin.Count && rec.allCoin.Count == rec.allDesk.Count)
            {
                if (currentPageRec >= 0)
                {
                    List<LoopListRankingData> tmpDatas = null;
                    if (loopListRankingDatas.Count == 0)
                    {
                        tmpDatas = new List<LoopListRankingData>();
                        loopListRankingDatas.Add(tmpDatas);
                    }
                    else if (currentPageRec < loopListRankingDatas.Count)
                    {
                        tmpDatas = loopListRankingDatas[currentPageRec];
                        tmpDatas.Clear();
                    }

                    if (null != tmpDatas)
                    {
                        for (int i = 0, n = allNickName.Length; i < n; i++)
                        {
                            LoopListRankingData tmpData = new LoopListRankingData();
                            tmpData.ranking = rec.allRank[i];
                            tmpData.nickname = allNickName[i];
                            tmpData.num = rec.allDesk[i];
                            tmpData.reward = allHuntGold[i];
                            tmpData.score = rec.allCoin[i];
                            tmpData.userId = rec.allUserId[i];
                            tmpData.isHunter = tmpData.userId == hunterKingId;
                            tmpData.isLooser = tmpData.ranking > allNickName.Length - rec.allLoser;
                            tmpDatas.Add(tmpData);
                        }
                    }
                }

                UpdateRankingInfo();
            }

            UpdateMyInfo();

            currentBlindLevel = rec.currentBlindLevel;

            bReportTimeCounting = true;
            playedTime = rec.playedTime;

            UpdateMatchInfo();
        }

        private void UpdateMatchInfo()
        {
            this.textMatchZmsysj.text = TimeHelper.ShowRemainingSemicolonPure(raiseBlindTime);
            this.textMatchTotalBonus.text = string.Format(LanguageManager.mInstance.GetLanguageForKey("UITexasReport_Text_MatchTotalBonus"), StringHelper.GetShortSignedString(this.totalReward));
            this.textMatchAverageScoreboard.text = StringHelper.ShowGold((int)this.averageScoreboard);
            this.textMatchParticipants.text = $"{this.playerNum - this.allLoser}/{this.playerNum}";
            this.textMatchCurrBlind.text = StringHelper.ShowBlinds(this.currentBlind.ToString(), (2 * this.currentBlind).ToString(), "0");
            this.textMatchNextBlind.text = StringHelper.ShowBlinds(this.nextBlind.ToString(), (2 * this.nextBlind).ToString(), "0");
            this.textMatchNextBlindTime.text = string.Format(LanguageManager.mInstance.GetLanguageForKey("UITexasReport_Text_MatchZmsysj"), this.updateTime);
        }

        #endregion

        #region 牌桌

        private void UpdateDeskInfo()
        {

        }

        private void InitDeskLoopListView()
        {
            if (null == listLoopDeskData)
            {
                listLoopDeskData = new List<LoopListDeskData>();
            }
            else
            {
                listLoopDeskData.Clear();
            }
            this.listLoopDeskData.Insert(0, new LoopListDeskData());

            loopListView2Desk.InitListView(listLoopDeskData.Count + 1, OnGetItemByIndexDesk);
            this.loopListView2Desk.mOnEndDragAction = onEndDragDesk;
            this.loopListView2Desk.mOnDownMoreDragAction = onDownMoreDragDesk;
            this.loopListView2Desk.mOnUpRefreshDragAction = onUpRefreshDragDesk;
        }

        private void onUpRefreshDragDesk()
        {
            this.dragDirDesk = -1;
            if (loopListView2Desk.ShownItemCount == 0)
            {
                return;
            }

            if (this.loadingTipStatusDesk != LoadingTipStatus.None && this.loadingTipStatusDesk != LoadingTipStatus.WaitRelease)
            {
                return;
            }

            LoopListViewItem2 item = loopListView2Desk.GetShownItemByItemIndex(0);
            if (item == null)
            {
                return;
            }

            ScrollRect sr = loopListView2Desk.ScrollRect;
            Vector3 pos = sr.content.anchoredPosition3D;
            if (pos.y < -this.loadingTipItemHeightDesk)
            {
                if (this.loadingTipStatusDesk != LoadingTipStatus.None)
                {
                    return;
                }

                this.loadingTipStatusDesk = LoadingTipStatus.WaitRelease;
                this.UpdateLoadingTipDesk(item);
                item.CachedRectTransform.anchoredPosition3D = new Vector3(0, this.loadingTipItemHeightDesk, 0);
            }
            else
            {
                if (this.loadingTipStatusDesk != LoadingTipStatus.WaitRelease)
                {
                    return;
                }

                this.loadingTipStatusDesk = LoadingTipStatus.None;
                this.UpdateLoadingTipDesk(item);
                item.CachedRectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
            }
        }

        private void onDownMoreDragDesk()
        {
            this.dragDirDesk = 1;
            if (loopListView2Desk.ShownItemCount == 0)
            {
                return;
            }

            if (this.loadingTipStatusDesk != LoadingTipStatus.None && this.loadingTipStatusDesk != LoadingTipStatus.WaitRelease)
            {
                return;
            }

            LoopListViewItem2 item = loopListView2Desk.GetShownItemByItemIndex(listLoopDeskData.Count);
            if (item == null)
            {
                return;
            }

            LoopListViewItem2 item1 = loopListView2Desk.GetShownItemByItemIndex(listLoopDeskData.Count - 1);
            if (item1 == null)
            {
                return;
            }

            float y = loopListView2Desk.GetItemCornerPosInViewPort(item1, ItemCornerEnum.LeftBottom).y;
            if (y + loopListView2Desk.ViewPortSize >= this.loadingTipItemHeightDesk)
            {
                if (this.loadingTipStatusDesk != LoadingTipStatus.None)
                {
                    return;
                }

                this.loadingTipStatusDesk = LoadingTipStatus.WaitRelease;
                this.UpdateLoadingTipDesk(item);
            }
            else
            {
                if (this.loadingTipStatusDesk != LoadingTipStatus.WaitRelease)
                {
                    return;
                }

                this.loadingTipStatusDesk = LoadingTipStatus.None;
                this.UpdateLoadingTipDesk(item);
            }
        }

        private void onEndDragDesk()
        {
            if (this.dragDirDesk == -1)
            {
                if (loopListView2Desk.ShownItemCount == 0)
                {
                    return;
                }

                if (this.loadingTipStatusDesk != LoadingTipStatus.None && this.loadingTipStatusDesk != LoadingTipStatus.WaitRelease)
                {
                    return;
                }

                LoopListViewItem2 item = loopListView2Desk.GetShownItemByItemIndex(0);
                if (item == null)
                {
                    return;
                }

                loopListView2Desk.OnItemSizeChanged(item.ItemIndex);
                if (this.loadingTipStatusDesk != LoadingTipStatus.WaitRelease)
                {
                    return;
                }

                this.loadingTipStatusDesk = LoadingTipStatus.WaitLoad;
                this.UpdateLoadingTipDesk(item);
                this.DragRequestDataDesk(item, 0);
            }
            else if (this.dragDirDesk == 1)
            {
                if (loopListView2Desk.ShownItemCount == 0)
                {
                    return;
                }

                if (this.loadingTipStatusDesk != LoadingTipStatus.None && this.loadingTipStatusDesk != LoadingTipStatus.WaitRelease)
                {
                    return;
                }

                LoopListViewItem2 item = loopListView2Desk.GetShownItemByItemIndex(listLoopDeskData.Count);
                if (item == null)
                {
                    return;
                }

                loopListView2Desk.OnItemSizeChanged(item.ItemIndex);
                if (this.loadingTipStatusDesk != LoadingTipStatus.WaitRelease)
                {
                    return;
                }

                this.loadingTipStatusDesk = LoadingTipStatus.WaitLoad;
                this.UpdateLoadingTipDesk(item);
                var tIndex = 0;
                if (listLoopDeskData.Count >= 2)
                    tIndex = listLoopDeskData.Count - 2;
                else
                    tIndex = listLoopDeskData.Count - 1;

                this.DragRequestDataDesk(item, 1);
            }
        }

        private LoopListViewItem2 OnGetItemByIndexDesk(LoopListView2 listView, int index)
        {
            if (index < 0)
            {
                return null;
            }

            LoopListViewItem2 item = null;
            if (index == 0)
            {
                item = listView.NewListViewItem("ItemPrefab0");
                this.UpdateLoadingTipDesk(item);
                return item;
            }

            if (index == listLoopDeskData.Count)
            {
                item = listView.NewListViewItem("ItemPrefab0");
                this.UpdateLoadingTipDesk(item);
                return item;
            }

            int itemIndex = index;
            var itemData = listLoopDeskData[itemIndex];
            if (itemData == null)
            {
                return null;
            }
            item = listView.NewListViewItem("ItemInfo");
            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
            }
            SetItemOtherData(item.gameObject, itemData);
            return item;
        }

        private void SetItemOtherData(GameObject go, LoopListDeskData data)
        {
            if (null == go || null == data)
                return;

            Text mText = go.transform.Find("Text_DeskNum").GetComponent<Text>();
            mText.text = data.deskNum.ToString();
            mText = go.transform.Find("Text_PlayerCount").GetComponent<Text>();
            mText.text = data.playerCount.ToString();
            mText = go.transform.Find("Text_Score").GetComponent<Text>();
            mText.text = StringHelper.ShowGold(data.minScore)+"/"+ StringHelper.ShowGold(data.maxScore);
        }

        /// <summary>
        /// dir=0刷新(上面的菊花)   1下面的菊花
        /// </summary>
        private void DragRequestDataDesk(LoopListViewItem2 item, int dir) //, string time, string dir)
        {
            OnDataSourceLoadMoreFinishedDesk(dir, item);
        }

        private void OnDataSourceLoadMoreFinishedDesk(int dir, LoopListViewItem2 item)
        {
            if (dir == 1)
            {
                HttpRequestComponent.Instance.Send(WEB2_room_mtt_desk_info.API,
                                                   WEB2_room_mtt_desk_info.Request(new WEB2_room_mtt_desk_info.RequestData()
                                                   {
                                                       page = currentPageDesk + 1,
                                                       rpath = (int)RoomPath.MTT,
                                                       rmid = GameCache.Instance.room_id
                                                   }), json =>
                                                   {
                                                       var tDto = WEB2_room_mtt_desk_info.Response(json);
                                                       if (tDto.status == 0)
                                                       {
                                                           if (tDto.data.deskList.Count > 0)
                                                           {
                                                               List<LoopListDeskData> tmp = new List<LoopListDeskData>();
                                                               for (int i = 0, n = tDto.data.deskList.Count; i < n; i++)
                                                               {
                                                                   tmp.Add(new LoopListDeskData()
                                                                   {
                                                                       deskNum = tDto.data.deskList[i].deskNum,
                                                                       playerCount = tDto.data.deskList[i].playerCount,
                                                                       minScore = tDto.data.deskList[i].minScore,
                                                                       maxScore = tDto.data.deskList[i].maxScore
                                                                   });
                                                               }

                                                               //追加
                                                               currentPageDesk++;
                                                               listLoopDeskData.AddRange(tmp);
                                                               OnDataSourceLoadMoreFinishedDesk(tmp.Count > 0);
                                                           }
                                                       }
                                                       else
                                                       {
                                                           UIComponent.Instance.Toast(tDto.status);
                                                           OnDataSourceLoadMoreFinishedDesk(true);
                                                       }
                                                   });
            }
            else if (dir == 0)
            {
                HttpRequestComponent.Instance.Send(WEB2_room_mtt_desk_info.API,
                                                   WEB2_room_mtt_desk_info.Request(new WEB2_room_mtt_desk_info.RequestData()
                                                   {
                                                       page = 1,
                                                       rpath = (int)RoomPath.MTT,
                                                       rmid = GameCache.Instance.room_id
                                                   }), json =>
                                                   {
                                                       var tDto = WEB2_room_mtt_desk_info.Response(json);
                                                       if (tDto.status == 0)
                                                       {
                                                           if (tDto.data.deskList.Count > 0)
                                                           {
                                                               listLoopDeskData = new List<LoopListDeskData>();
                                                               for (int i = 0, n = tDto.data.deskList.Count; i < n; i++)
                                                               {
                                                                   listLoopDeskData.Add(new LoopListDeskData()
                                                                   {
                                                                       deskNum = tDto.data.deskList[i].deskNum,
                                                                       playerCount = tDto.data.deskList[i].playerCount,
                                                                       minScore = tDto.data.deskList[i].minScore,
                                                                       maxScore = tDto.data.deskList[i].maxScore
                                                                   });
                                                               }

                                                               //刷新
                                                               currentPageDesk = 1;
                                                               listLoopDeskData.Insert(0, new LoopListDeskData());
                                                               OnDataSourceLoadMoreFinishedDesk(true);
                                                           }
                                                       }
                                                       else
                                                       {
                                                           UIComponent.Instance.Toast(tDto.status);
                                                           OnDataSourceLoadMoreFinishedDesk(true);
                                                       }
                                                   });
            }
            this.UpdateLoadingTipDesk(item);
            // OnDataSourceLoadMoreFinishedDesk(true);
        }

        private async void OnDataSourceLoadMoreFinishedDesk(bool length)
        {
            if (loopListView2Desk.ShownItemCount == 0)
            {
                return;
            }

            if (this.loadingTipStatusDesk == LoadingTipStatus.WaitLoad)
            {
                await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(200);
                this.loadingTipStatusDesk = length ? LoadingTipStatus.None : LoadingTipStatus.NoMoreData;
                loopListView2Desk.SetListItemCount(listLoopDeskData.Count + 1, false);
                loopListView2Desk.RefreshAllShownItem();
            }
        }

        private void UpdateLoadingTipDesk(LoopListViewItem2 item)
        {
            if (null == item)
            {
                return;
            }

            ListItem0 itemScript0 = item.GetComponent<ListItem0>();
            if (itemScript0 == null)
            {
                return;
            }

            if (this.loadingTipStatusDesk == LoadingTipStatus.None)
            {
                itemScript0.mRoot.SetActive(false);
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
            }
            else if (this.loadingTipStatusDesk == LoadingTipStatus.WaitRelease)
            {
                itemScript0.mRoot.SetActive(true);
                itemScript0.mText.text = LanguageManager.mInstance.GetLanguageForKey("SuperView1");// "放开加载更多 ...";
                itemScript0.mArrow.SetActive(true);
                itemScript0.mWaitingIcon.SetActive(false);
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.loadingTipItemHeightDesk);
            }
            else if (this.loadingTipStatusDesk == LoadingTipStatus.WaitLoad)
            {
                itemScript0.mRoot.SetActive(true);
                itemScript0.mArrow.SetActive(false);
                itemScript0.mWaitingIcon.SetActive(true);
                itemScript0.mText.text = LanguageManager.mInstance.GetLanguageForKey("SuperView2");// "加载中 ...";
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.loadingTipItemHeightDesk);
            }
            else if (this.loadingTipStatusDesk == LoadingTipStatus.NoMoreData)
            {
                itemScript0.mRoot.SetActive(true);
                itemScript0.mArrow.SetActive(false);
                itemScript0.mWaitingIcon.SetActive(false);
                itemScript0.mText.text = LanguageManager.mInstance.GetLanguageForKey("SuperView3");//  "已经是最后一条啦";
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.loadingTipItemHeightDesk);
            }
        }

        #endregion

        #region 奖励

        private void UpdateRewardInfo()
        {
            this.textRewardTotalBonus.text = this.totalReward.ToString();
            this.textRewardReward.text = string.Format(LanguageManager.mInstance.GetLanguageForKey("UITexasReport_Text_RewardReward"), this.rewardNumber);
            this.textRewardNextReward.text = this.nextReward;
        }

        private void InitRewardLoopListView()
        {
            if (null == listLoopRewardData)
            {
                listLoopRewardData = new List<LoopListRewardData>();
            }
            else
            {
                listLoopRewardData.Clear();
            }
            this.listLoopRewardData.Insert(0, new LoopListRewardData());

            loopListView2Reward.InitListView(listLoopRewardData.Count + 1, OnGetItemByIndexReward);
            this.loopListView2Reward.mOnEndDragAction = onEndDragReward;
            this.loopListView2Reward.mOnDownMoreDragAction = onDownMoreDragReward;
            this.loopListView2Reward.mOnUpRefreshDragAction = onUpRefreshDragReward;
        }

        private void onUpRefreshDragReward()
        {
            this.dragDirReward = -1;
            if (loopListView2Reward.ShownItemCount == 0)
            {
                return;
            }

            if (this.loadingTipStatusReward != LoadingTipStatus.None && this.loadingTipStatusReward != LoadingTipStatus.WaitRelease)
            {
                return;
            }

            LoopListViewItem2 item = loopListView2Reward.GetShownItemByItemIndex(0);
            if (item == null)
            {
                return;
            }

            ScrollRect sr = loopListView2Reward.ScrollRect;
            Vector3 pos = sr.content.anchoredPosition3D;
            if (pos.y < -this.loadingTipItemHeightReward)
            {
                if (this.loadingTipStatusReward != LoadingTipStatus.None)
                {
                    return;
                }

                this.loadingTipStatusReward = LoadingTipStatus.WaitRelease;
                this.UpdateLoadingTipReward(item);
                item.CachedRectTransform.anchoredPosition3D = new Vector3(0, this.loadingTipItemHeightReward, 0);
            }
            else
            {
                if (this.loadingTipStatusReward != LoadingTipStatus.WaitRelease)
                {
                    return;
                }

                this.loadingTipStatusReward = LoadingTipStatus.None;
                this.UpdateLoadingTipReward(item);
                item.CachedRectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
            }
        }

        private void onDownMoreDragReward()
        {
            this.dragDirReward = 1;
            if (loopListView2Reward.ShownItemCount == 0)
            {
                return;
            }

            if (this.loadingTipStatusReward != LoadingTipStatus.None && this.loadingTipStatusReward != LoadingTipStatus.WaitRelease)
            {
                return;
            }

            LoopListViewItem2 item = loopListView2Reward.GetShownItemByItemIndex(listLoopRewardData.Count);
            if (item == null)
            {
                return;
            }

            LoopListViewItem2 item1 = loopListView2Reward.GetShownItemByItemIndex(listLoopRewardData.Count - 1);
            if (item1 == null)
            {
                return;
            }

            float y = loopListView2Reward.GetItemCornerPosInViewPort(item1, ItemCornerEnum.LeftBottom).y;
            if (y + loopListView2Reward.ViewPortSize >= this.loadingTipItemHeightReward)
            {
                if (this.loadingTipStatusReward != LoadingTipStatus.None)
                {
                    return;
                }

                this.loadingTipStatusReward = LoadingTipStatus.WaitRelease;
                this.UpdateLoadingTipReward(item);
            }
            else
            {
                if (this.loadingTipStatusReward != LoadingTipStatus.WaitRelease)
                {
                    return;
                }

                this.loadingTipStatusReward = LoadingTipStatus.None;
                this.UpdateLoadingTipReward(item);
            }
        }

        private void onEndDragReward()
        {
            if (this.dragDirReward == -1)
            {
                if (loopListView2Reward.ShownItemCount == 0)
                {
                    return;
                }

                if (this.loadingTipStatusReward != LoadingTipStatus.None && this.loadingTipStatusReward != LoadingTipStatus.WaitRelease)
                {
                    return;
                }

                LoopListViewItem2 item = loopListView2Reward.GetShownItemByItemIndex(0);
                if (item == null)
                {
                    return;
                }

                loopListView2Reward.OnItemSizeChanged(item.ItemIndex);
                if (this.loadingTipStatusReward != LoadingTipStatus.WaitRelease)
                {
                    return;
                }

                this.loadingTipStatusReward = LoadingTipStatus.WaitLoad;
                this.UpdateLoadingTipReward(item);
                this.DragRequestDataReward(item, 0);
            }
            else if (this.dragDirReward == 1)
            {
                if (loopListView2Reward.ShownItemCount == 0)
                {
                    return;
                }

                if (this.loadingTipStatusReward != LoadingTipStatus.None && this.loadingTipStatusReward != LoadingTipStatus.WaitRelease)
                {
                    return;
                }

                LoopListViewItem2 item = loopListView2Reward.GetShownItemByItemIndex(listLoopRewardData.Count);
                if (item == null)
                {
                    return;
                }

                loopListView2Reward.OnItemSizeChanged(item.ItemIndex);
                if (this.loadingTipStatusReward != LoadingTipStatus.WaitRelease)
                {
                    return;
                }

                this.loadingTipStatusReward = LoadingTipStatus.WaitLoad;
                this.UpdateLoadingTipReward(item);
                var tIndex = 0;
                if (listLoopRewardData.Count >= 2)
                    tIndex = listLoopRewardData.Count - 2;
                else
                    tIndex = listLoopRewardData.Count - 1;

                this.DragRequestDataReward(item, 1);
            }
        }

        private LoopListViewItem2 OnGetItemByIndexReward(LoopListView2 listView, int index)
        {
            if (index < 0)
            {
                return null;
            }

            LoopListViewItem2 item = null;
            if (index == 0)
            {
                item = listView.NewListViewItem("ItemPrefab0");
                this.UpdateLoadingTipReward(item);
                return item;
            }

            if (index == listLoopRewardData.Count)
            {
                item = listView.NewListViewItem("ItemPrefab0");
                this.UpdateLoadingTipReward(item);
                return item;
            }

            int itemIndex = index;
            var itemData = listLoopRewardData[itemIndex];
            if (itemData == null)
            {
                return null;
            }
            item = listView.NewListViewItem("ItemInfo");
            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
            }
            SetItemOtherData(item.gameObject, itemData);
            return item;
        }

        private void SetItemOtherData(GameObject go, LoopListRewardData data)
        {
            if (null == go || null == data)
                return;

            Text mText = go.transform.Find("Text_RewardRanking").GetComponent<Text>();
            mText.text = data.rank.ToString();
            mText = go.transform.Find("Text_RewardScore").GetComponent<Text>();
            mText.text = data.bonus;
        }

        /// <summary>
        /// dir=0刷新(上面的菊花)   1下面的菊花
        /// </summary>
        private void DragRequestDataReward(LoopListViewItem2 item, int dir) //, string time, string dir)
        {
            OnDataSourceLoadMoreFinishedReward(dir, item);
        }

        private void OnDataSourceLoadMoreFinishedReward(int dir, LoopListViewItem2 item)
        {
            if (dir == 1)
            {
                UIMatchModel.mInstance.APIMTTRewardList(GameCache.Instance.room_id, currentPageReward+1, pDto =>
                {
                    if (pDto.status == 0)
                    {
                        if (pDto.data.currMttRewardList.Count > 0)
                        {
                            List<LoopListRewardData> tmp = new List<LoopListRewardData>();
                            for (int i = 0, n = pDto.data.currMttRewardList.Count; i < n; i++)
                            {
                                tmp.Add(new LoopListRewardData()
                                {
                                        rank = pDto.data.currMttRewardList[i].rank,
                                        percent = pDto.data.currMttRewardList[i].percent,
                                        bonus = pDto.data.currMttRewardList[i].bonus
                                });
                            }

                            //追加
                            this.currentPageReward++;
                            listLoopRewardData.AddRange(tmp);
                            OnDataSourceLoadMoreFinishedReward(tmp.Count > 0);

                            nextReward = pDto.data.nextReward;
                            rewardNumber = pDto.data.rewardNumber;

                            UpdateRewardInfo();
                        }
                    }
                    else
                    {
                        UIComponent.Instance.Toast(pDto.status);
                        OnDataSourceLoadMoreFinishedReward(true);
                    }
                });
            }
            else if (dir == 0)
            {
                UIMatchModel.mInstance.APIMTTRewardList(GameCache.Instance.room_id, 1, pDto =>
                {
                    if (pDto.status == 0)
                    {
                        if (pDto.data.currMttRewardList.Count > 0)
                        {
                            this.listLoopRewardData = new List<LoopListRewardData>();
                            for (int i = 0, n = pDto.data.currMttRewardList.Count; i < n; i++)
                            {
                                listLoopRewardData.Add(new LoopListRewardData()
                                {
                                        rank = pDto.data.currMttRewardList[i].rank,
                                        percent = pDto.data.currMttRewardList[i].percent,
                                        bonus = pDto.data.currMttRewardList[i].bonus
                                });
                            }

                            //刷新
                            this.currentPageReward = 1;
                            listLoopRewardData.Insert(0, new LoopListRewardData());
                            OnDataSourceLoadMoreFinishedReward(true);

                            nextReward = pDto.data.nextReward;
                            rewardNumber = pDto.data.rewardNumber;

                            UpdateRewardInfo();
                        }
                    }
                    else
                    {
                        UIComponent.Instance.Toast(pDto.status);
                        OnDataSourceLoadMoreFinishedReward(true);
                    }
                });
                
            }
            this.UpdateLoadingTipReward(item);
            // OnDataSourceLoadMoreFinishedReward(true);
        }

        private async void OnDataSourceLoadMoreFinishedReward(bool length)
        {
            if (loopListView2Reward.ShownItemCount == 0)
            {
                return;
            }

            if (this.loadingTipStatusReward == LoadingTipStatus.WaitLoad)
            {
                await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(200);
                this.loadingTipStatusReward = length ? LoadingTipStatus.None : LoadingTipStatus.NoMoreData;
                loopListView2Reward.SetListItemCount(listLoopRewardData.Count + 1, false);
                loopListView2Reward.RefreshAllShownItem();
            }
        }

        private void UpdateLoadingTipReward(LoopListViewItem2 item)
        {
            if (null == item)
            {
                return;
            }

            ListItem0 itemScript0 = item.GetComponent<ListItem0>();
            if (itemScript0 == null)
            {
                return;
            }

            if (this.loadingTipStatusReward == LoadingTipStatus.None)
            {
                itemScript0.mRoot.SetActive(false);
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
            }
            else if (this.loadingTipStatusReward == LoadingTipStatus.WaitRelease)
            {
                itemScript0.mRoot.SetActive(true);
                itemScript0.mText.text = LanguageManager.mInstance.GetLanguageForKey("SuperView1");// "放开加载更多 ...";
                itemScript0.mArrow.SetActive(true);
                itemScript0.mWaitingIcon.SetActive(false);
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.loadingTipItemHeightReward);
            }
            else if (this.loadingTipStatusReward == LoadingTipStatus.WaitLoad)
            {
                itemScript0.mRoot.SetActive(true);
                itemScript0.mArrow.SetActive(false);
                itemScript0.mWaitingIcon.SetActive(true);
                itemScript0.mText.text = LanguageManager.mInstance.GetLanguageForKey("SuperView2");// "加载中 ...";
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.loadingTipItemHeightReward);
            }
            else if (this.loadingTipStatusReward == LoadingTipStatus.NoMoreData)
            {
                itemScript0.mRoot.SetActive(true);
                itemScript0.mArrow.SetActive(false);
                itemScript0.mWaitingIcon.SetActive(false);
                itemScript0.mText.text = LanguageManager.mInstance.GetLanguageForKey("SuperView3");//  "已经是最后一条啦";
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.loadingTipItemHeightReward);
            }
        }

        #endregion

        #region 盲注

        private void UpdateBlindInfo()
        {

        }

        private void InitBlindLoopListView()
        {
            if (null == listLoopBlindData)
                listLoopBlindData = new List<LoopListBlindData>();
            if(listLoopBlindData.Count > 0)
                listLoopBlindData.Clear();

            int levelCount = MTTGameUtil.numOfLevel(((MTTGame)GameCache.Instance.CurGame).BlindType);
            for (int i = 0; i < levelCount; i++)
            {
                int sb = MTTGameUtil.BlindAtLevel(i, ((MTTGame)GameCache.Instance.CurGame).BlindType, blindScale);
                int ante = MTTGameUtil.AnteAtLevel(i, ((MTTGame)GameCache.Instance.CurGame).BlindType, blindScale);
                LoopListBlindData mData = new LoopListBlindData()
                {
                        ante = ante,
                        blind = sb,
                        level = i+1,
                        updateTime = updateTime 
                };
                listLoopBlindData.Add(mData);
            }

            loopListView2Blind.InitListView(listLoopBlindData.Count, OnGetItemByIndexBlind);
        }

        private LoopListViewItem2 OnGetItemByIndexBlind(LoopListView2 listView, int index)
        {
            if (index < 0)
            {
                return null;
            }

            LoopListViewItem2 item = null;
            var itemData = listLoopBlindData[index];
            if (itemData == null)
            {
                return null;
            }
            item = listView.NewListViewItem("ItemInfo");
            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
            }
            SetItemOtherData(item.gameObject, itemData);
            return item;
        }

        private void SetItemOtherData(GameObject go, LoopListBlindData data)
        {
            if (null == go || null == data)
                return;

            Color32 tmp = equalBlindLevel;
            if (data.level < this.currentBlindLevel)
            {
                tmp = lessBlindLevel;
            } else if (data.level > this.currentBlindLevel)
            {
                tmp = greaterBlindLevel;
            }
            Transform mArrow = go.transform.Find("Image_Arrow");
            mArrow.gameObject.SetActive(this.currentBlindLevel == data.level);
            Text mText = go.transform.Find("Text_Level").GetComponent<Text>();
            mText.text = data.level.ToString();
            mText.color = tmp;
            mText = go.transform.Find("Text_Blind").GetComponent<Text>();
            mText.text = StringHelper.GetShortString(data.blind) + "/" + StringHelper.GetShortString(data.blind * 2);
            mText.color = tmp;
            mText = go.transform.Find("Text_Ante").GetComponent<Text>();
            mText.text = StringHelper.GetShortString(data.ante);
            mText.color = tmp;
            mText = go.transform.Find("Text_Time").GetComponent<Text>();
            mText.text = string.Format(LanguageManager.Get("UITexasReport_Text_MatchNextBlindTime"), data.updateTime); 
            mText.color = tmp;
        }

        #endregion

        #region 实时排名

        private void UpdateRankingInfo()
        {
            if (!isMoreLoadRanking)
            {
                InitRankingLoopListView();
                isMoreLoadRanking = true;
                UpdatePage();
            }
            else
            {

            }
        }

        private void UpdatePage()
        {
            this.textPage.text = $"{this.currentPageRec+1}/{this.totalPage}";
        }

        private void InitRankingLoopListView()
        {
            listLoopRankingData = loopListRankingDatas[this.currentPageRec];
            loopListView2Ranking.InitListView(listLoopRankingData.Count, OnGetItemByIndexRanking);
        }

        private LoopListViewItem2 OnGetItemByIndexRanking(LoopListView2 listView, int index)
        {
            if (index < 0)
            {
                return null;
            }

            LoopListViewItem2 item = null;
            var itemData = listLoopRankingData[index];
            if (itemData == null)
            {
                return null;
            }
            item = listView.NewListViewItem("ItemInfo");
            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
            }
            SetItemOtherData(item.gameObject, itemData);
            return item;
        }

        private void SetItemOtherData(GameObject go, LoopListRankingData data)
        {
            if (null == go || null == data)
                return;

            Color32 tmp = normalColor;
            tmp = !data.isLooser? normalColor : loserColor;

            if (data.ranking == currentRank)
            {
                tmp = myColor;
            }

            Transform mArrow = go.transform.Find("Image_My");
            mArrow.gameObject.SetActive(this.currentRank == data.ranking);
            mArrow = go.transform.Find("Image_HunterIicon");
            mArrow.gameObject.SetActive(data.isHunter);
            Text mText = go.transform.Find("Text_Ranking").GetComponent<Text>();
            mText.text = data.ranking.ToString();
            mText.color = tmp;
            mText = go.transform.Find("Text_Nickname").GetComponent<Text>();
            mText.text = data.nickname;
            mText.color = tmp;
            mText = go.transform.Find("Text_Num").GetComponent<Text>();
            mText.text = data.num.ToString();
            mText.color = tmp;
            mText = go.transform.Find("Text_Reward").GetComponent<Text>();
            mText.text = data.reward;
            mText.color = tmp;
            mText = go.transform.Find("Text_Score").GetComponent<Text>();
            mText.text = StringHelper.ShowGold(data.score);
            mText.color = tmp;
        }

        #endregion

        private void UpdateMyInfo()
        {
            this.textMyInfoRanking.text = this.currentRank.ToString();
            this.textMyInfoNickname.text = GameCache.Instance.nick;
            this.textMyInfoReward.text = StringHelper.ShowGold(this.hunterReward);
            this.textMyInfoScore.text = StringHelper.ShowGold(this.myScore);
            this.textMyInfoRebuild.text = $"{this.rebuyTime}/{this.totalRebuyTime}";
            WebImageHelper.SetHeadImage(rawimageMyInfoIcon, GameCache.Instance.headPic);
        }

        private void CountDown()
        {
            if (bReportTimeCounting)
            {
                if (Time.time - reportTimeDeltaTime > 1f)
                {
                    reportTimeDeltaTime = Time.time;
                    this.playedTime++;
                    if (this.playedTime <= 0)
                    {
                        bReportTimeCounting = false;
                        textTitleBarTime.text = "00:00:00";
                    }
                    else
                    {
                        int mMin = this.playedTime / 60;
                        textTitleBarTime.text = $"{(mMin / 60).ToString().PadLeft(2, '0')}:{(mMin % 60).ToString().PadLeft(2, '0')}:{(this.playedTime % 60).ToString().PadLeft(2, '0')}";
                    }
                }
            }
        }
    }
}

