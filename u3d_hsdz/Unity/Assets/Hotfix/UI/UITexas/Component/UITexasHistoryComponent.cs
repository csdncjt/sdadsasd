using System;
using System.Collections;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;
using JPush;

namespace ETHotfix
{

    [ObjectSystem]
    public class UITexasHistoryComponentSystem : AwakeSystem<UITexasHistoryComponent>
    {
        public override void Awake(UITexasHistoryComponent self)
        {
            self.Awake();
        }
    }

    public class SingleCardScore
    {
        //因为关联属性加入roomid和version
        public int roomId { get; set; }
        //角色
        public int role;
        public int brandType { get; set; }
        //用户名
        public String userName { get; set; }
        //牌谱
        public List<int> card { get; set; }
        //最后比牌高亮的标志位
        public List<int> liangPai { get; set; }
        //盈利值
        public int profit;
        //盈利值
        public int insurer;
        //类型是否显示0显示/1为不显示
        public int cardeType { get; set; }
        //第几局
        public int version { get; set; }
    }

    public class CardScore
    {
        //房间id
        public int roomId { get; set; }
        //所有用户的牌
        public List<SingleCardScore> card { get; set; }
        //用户人数
        public int userNumber { get; set; }
        //底池
        public int poolChip { get; set; }

        public List<int> publicCard { get; set; }
    }

    public class SingleProcess
    {
        //角色    
        public int role { get; set; }
        //用户名    
        public String userName { get; set; }
        //动作：1:下注  2:跟注  3:加注  4:全下 5:让牌  6:弃牌 7抓头；8再加注，9再再加注，10再再加注    
        public int action { get; set; }
        //下注    
        public int wager { get; set; }
        //筹码    
        public int chip { get; set; }

    }
    public class GameProcess {
        //房间id    
        public int roomId { get; set; }
        //手数code1：preflop，2flop，3turn，4river
        public int shoushuCode { get; set; }
        //发的公共牌    
        public List<int> publicCard { get; set; }
        //底池    
        public int chip { get; set; }
        //在位用户人数   
        public int users { get; set; }
        //详细记录的id    
        public List<int> singleProcess { get; set; }
        //详细的记录    
        public List<SingleProcess> singleProcessList { get; set; }
        //第几手   
        public int version { get; set; }
    }

    public class UITexasHistoryComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Text textinsuranceValue;
        private Text textJackPotValue;
        private Button buttonCollect;
        private Button buttonShare;
        private Image imageMenuMask;
        private Text textnum;
        private Button buttonFirstPage;
        private Button buttonPrePage;
        private Button buttonNextPage;
        private Button buttonLastPage;
        private Image imageInsurance;
        private Image imageJackPot;
        
        //private Text textShareTip;
        //private Text textCollectTip;

        private LoopListView2 mLoopListView;

        private bool isPlayerInGame;
        private bool bInitListView;
        private List<PlayerInfo> playerInfos;
        private int currentPage;
        REQ_GAME_PREV_GAME rec;
        bool isCollect;

        ReferenceCollector rcPokerSprite;


        //新
        private Text room_blind;
        private Text room_people;
        private Text river_gold;
        private Text river_people;
        private Text flop_gold;
        private Text flop_people;
        private Text turn_people;
        private Text turn_gold;
        private Text preflop_gold;
        private Text paipu_gold;
        private Text paipu_people;
        private Text show_gold;
        private Text show_people;

        private RectTransform public_card;
        private RectTransform preflop_card;
        private RectTransform flop_card;
        private RectTransform turn_card;
        private RectTransform river_card;
        private RectTransform show_card;

        private RectTransform paipuinfo_player;
        private RectTransform paipu_player2;
        private RectTransform paipuinfo_folds;
        private RectTransform paipu_player;

        private RectTransform paipu_panel;
        private RectTransform Content;
        private RectTransform scrollview_Content;

        private RectTransform preflop;
        private RectTransform flop;
        private RectTransform turn;
        private RectTransform river;
        private RectTransform showdown;
        private RectTransform InfoList;

        CardScore paipu;
        private int handCardCount;



        public sealed class HistoryInfoData
        {
            public bool bInsurance;
            public bool bJackPot;
            public ReferenceCollector rcPokerSprite;
        }

        private HistoryInfoData historyInfoData;

        private class PlayerInfo
        {
            public int playerId;
            public String userName;
            public String headPic;
            public sbyte firstCard;
            public sbyte secondCard;
            public List<sbyte> maxCardIndex;
            public sbyte maxCardType;
            public int winAnte;
            public int insuranceGain;
            public List<sbyte> operationTypes;
            public List<int> operationAntes;
            public sbyte playerType;
            public sbyte thirdCard;
            public sbyte fourthCard;
            public bool isMine;
        }


        public void Awake()
        {
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle("smallcard.unity3d");
            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset("smallcard.unity3d", "SmallCard");
            if (null != bundleGameObject)
            {
                GameObject mObj = GameObject.Instantiate(bundleGameObject);
                rcPokerSprite = mObj.GetComponent<ReferenceCollector>();
            }

            InitUI();
            registerHandler();

            playerInfos = new List<PlayerInfo>();
        }

        public override void OnShow(object obj)
        {
            if (null == obj)
                return;

            historyInfoData = obj as HistoryInfoData;
            if(rcPokerSprite == null)
                rcPokerSprite = historyInfoData.rcPokerSprite;

            imageMenuMask.gameObject.SetActive(true);
            currentPage = -1;
            bInitListView = false;

            buttonFirstPage.interactable = false;
            buttonLastPage.interactable = false;
            buttonPrePage.interactable = false;
            buttonNextPage.interactable = false;

            //imageInsurance.gameObject.SetActive(historyInfoData.bInsurance);
            //imageJackPot.gameObject.SetActive(historyInfoData.bJackPot);

            RefreshData();
        }

        public override void OnHide()
        {

        }

        public override void Dispose()
        {
            if (this.IsDisposed)
                return;
            removeHandler();
            base.Dispose();
        }

        #region UI
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            buttonCollect = rc.Get<GameObject>("Button_Collect").GetComponent<Button>();
            buttonShare = rc.Get<GameObject>("Button_Share").GetComponent<Button>();
            imageMenuMask = rc.Get<GameObject>("Image_MenuMask").GetComponent<Image>();
            textnum = rc.Get<GameObject>("Text_num").GetComponent<Text>();
            textinsuranceValue = rc.Get<GameObject>("Text_insuranceValue").GetComponent<Text>();
            textJackPotValue = rc.Get<GameObject>("Text_JackPotValue").GetComponent<Text>();
            mLoopListView = rc.Get<GameObject>("Scrollview").GetComponent<LoopListView2>();
            buttonFirstPage = rc.Get<GameObject>("Button_FirstPage").GetComponent<Button>();
            buttonPrePage = rc.Get<GameObject>("Button_PrePage").GetComponent<Button>();
            buttonNextPage = rc.Get<GameObject>("Button_NextPage").GetComponent<Button>();
            buttonLastPage = rc.Get<GameObject>("Button_LastPage").GetComponent<Button>();
            imageInsurance = rc.Get<GameObject>("Image_Insurance").GetComponent<Image>();
            imageJackPot = rc.Get<GameObject>("Image_JackPot").GetComponent<Image>();
            //textShareTip = rc.Get<GameObject>("Text_ShareTip").GetComponent<Text>();
            //textCollectTip = rc.Get<GameObject>("Text_CollectTip").GetComponent<Text>();

            room_blind = rc.Get<GameObject>("room_blind").GetComponent<Text>();
            room_people = rc.Get<GameObject>("room_people").GetComponent<Text>();
            river_gold = rc.Get<GameObject>("river_gold").GetComponent<Text>();
            river_people = rc.Get<GameObject>("river_people").GetComponent<Text>();
            flop_gold = rc.Get<GameObject>("flop_gold").GetComponent<Text>();
            flop_people = rc.Get<GameObject>("flop_people").GetComponent<Text>();
            turn_people = rc.Get<GameObject>("turn_people").GetComponent<Text>();
            turn_gold = rc.Get<GameObject>("turn_gold").GetComponent<Text>();
            preflop_gold = rc.Get<GameObject>("preflop_gold").GetComponent<Text>();
            paipu_gold = rc.Get<GameObject>("paipu_gold").GetComponent<Text>();
            show_gold = rc.Get<GameObject>("show_gold").GetComponent<Text>();
            show_people = rc.Get<GameObject>("show_people").GetComponent<Text>();
            paipu_people = rc.Get<GameObject>("paipu_people").GetComponent<Text>();

            public_card = rc.Get<GameObject>("public_card").GetComponent<RectTransform>();
            preflop_card = rc.Get<GameObject>("preflop_card").GetComponent<RectTransform>();
            flop_card = rc.Get<GameObject>("flop_card").GetComponent<RectTransform>();
            turn_card = rc.Get<GameObject>("turn_card").GetComponent<RectTransform>();
            river_card = rc.Get<GameObject>("river_card").GetComponent<RectTransform>();
            show_card = rc.Get<GameObject>("show_card").GetComponent<RectTransform>();
            paipuinfo_player = rc.Get<GameObject>("paipuinfo_player").GetComponent<RectTransform>();
            paipu_player2 = rc.Get<GameObject>("paipu_player2").GetComponent<RectTransform>();
            paipuinfo_folds = rc.Get<GameObject>("paipuinfo_folds").GetComponent<RectTransform>();
            paipu_player = rc.Get<GameObject>("paipu_player").GetComponent<RectTransform>();

            paipu_panel = rc.Get<GameObject>("paipu_panel").GetComponent<RectTransform>();
            Content = rc.Get<GameObject>("Content").GetComponent<RectTransform>();
            scrollview_Content = rc.Get<GameObject>("scrollview_Content").GetComponent<RectTransform>();

            preflop = rc.Get<GameObject>("preflop").GetComponent<RectTransform>();
            flop = rc.Get<GameObject>("flop").GetComponent<RectTransform>();
            turn = rc.Get<GameObject>("turn").GetComponent<RectTransform>();
            river = rc.Get<GameObject>("river").GetComponent<RectTransform>();
            showdown = rc.Get<GameObject>("showdown").GetComponent<RectTransform>();
            InfoList = rc.Get<GameObject>("InfoList").GetComponent<RectTransform>();




            UIEventListener.Get(imageMenuMask.gameObject).onClick = onClickMenuMask;
            UIEventListener.Get(buttonFirstPage.gameObject).onClick = onClickFirstPage;
            UIEventListener.Get(buttonPrePage.gameObject).onClick = onClickPrePage;
            UIEventListener.Get(buttonNextPage.gameObject).onClick = onClickNextPage;
            UIEventListener.Get(buttonLastPage.gameObject).onClick = onClickLastPage;

            UIEventListener.Get(buttonShare.gameObject).onClick = onClickShare;
            UIEventListener.Get(buttonCollect.gameObject).onClick = onClickCollect;

            NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
            float realTop = safeArea.top * 1242 / safeArea.width;
            float realBottom = safeArea.bottom * 1242 / safeArea.width;
            VerticalLayoutGroup layoutGroup = rc.Get<GameObject>("Content").GetComponent<VerticalLayoutGroup>();
            layoutGroup.padding = new RectOffset(0, 0, (int)realTop, (int)realBottom);

            //mono.StartCoroutine(UpdateLayout(scrollview_Content));
        }

        private void onClickMenuMask(GameObject go)
        {
            imageMenuMask.gameObject.SetActive(false);
            // UIComponent.Instance.RemoveAnimated(UIType.UITexasHistory);
            UIComponent.Instance.Remove(UIType.UITexasHistory);
        }

        private void onClickFirstPage(GameObject go)
        {
            if (go.GetComponent<Button>().interactable == false)
                return;
            currentPage = 1;
            RefreshData();
        }

        private void onClickPrePage(GameObject go)
        {
            if (go.GetComponent<Button>().interactable == false)
                return;
            currentPage--;
            RefreshData();
        }

        private void onClickNextPage(GameObject go)
        {
            if (go.GetComponent<Button>().interactable == false)
                return;
            currentPage++;
            RefreshData();
        }

        private void onClickLastPage(GameObject go)
        {
            if (go.GetComponent<Button>().interactable == false)
                return;
            currentPage = -1;
            RefreshData();
        }

        private void onClickShare(GameObject go)
        {
            if (go.GetComponent<Button>().interactable == false)
                return;

            string url = $"{GlobalData.Instance.PaipuBaseUrl}{rec.playbackLink}&c=1";
            UniClipboard.SetText(url);
            // Game.Scene.GetComponent<UIComponent>().Show(UIType.UIToast, "复制链接成功", null, 0);
            UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10305));
        }

        private void onClickCollect(GameObject go)
        {
            if (go.GetComponent<Button>().interactable == false)
                return;
            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_COLLECT_CARD()
            {
                collect = (sbyte)(isCollect ? 0 : 1),
                roomID = GameCache.Instance.room_id,
                roomPath = GameCache.Instance.room_path,
                handId = currentPage
            });
        }

        private void RefreshData()
        {
            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_PREV_GAME()
            {
                roomID = GameCache.Instance.room_id,
                roomPath = GameCache.Instance.room_path,
                handId = currentPage
            });
        }

        #endregion

        private void SetUpItemWithData(GameObject go, PlayerInfo element)
        {
            go.transform.Find("Text_name").GetComponent<Text>().text = element.userName;
            RawImage headImg = go.transform.Find("UIHead").Find("Mask_head").Find("img_head").GetComponent<RawImage>();
            WebImageHelper.SetHeadImage(headImg, element.headPic);
            go.transform.Find("Image_current").gameObject.SetActive(element.isMine);
            var imgInsurance = go.transform.Find("Image_Insurance").gameObject;
            imgInsurance.SetActive(historyInfoData.bInsurance);
            imgInsurance.transform.Find("Text").GetComponent<Text>().text = StringHelper.GetShortSignedString(element.insuranceGain);

            go.transform.Find("Text_wins").GetComponent<Text>().text = StringHelper.GetShortSignedString(element.winAnte);
            if (element.winAnte >= 0)
            {
                go.transform.Find("Text_wins").GetComponent<Text>().color = new Color32(184, 43, 48, 255);
            }
            else
            {
                go.transform.Find("Text_wins").GetComponent<Text>().color = new Color32(75, 159, 93, 255);
            }
            go.transform.Find("Text_CardType").GetComponent<Text>().text = GetShowCardType(element.maxCardType);

            //玩家类型
            Image imgPlayerType = go.transform.Find("Image_plyerType").GetComponent<Image>();
            Sprite typeSprite = null;
            switch (element.playerType)
            {
                case 0:
                    // 普通人
                    break;
                case 1:
                    // 庄家
                    typeSprite = rc.Get<Sprite>("hand_icon_d");
                    break;
                case 2:
                    // 小盲
                    typeSprite = rc.Get<Sprite>("hand_icon_s");
                    break;
                case 3:
                    // 既是小盲又是庄家
                    typeSprite = rc.Get<Sprite>("hand_icon_s");
                    break;
                case 4:
                    // 大盲
                    typeSprite = rc.Get<Sprite>("hand_icon_b");
                    break;
                default:
                    // 未知
                    break;
            }
            if (typeSprite != null)
            {
                imgPlayerType.gameObject.SetActive(true);
                imgPlayerType.sprite = typeSprite;
            }
            else
            {
                imgPlayerType.gameObject.SetActive(false);
            }

            //牌
            Image handCard1 = go.transform.Find("Image_handCard1").GetComponent<Image>();
            Image handCard2 = go.transform.Find("Image_handCard2").GetComponent<Image>();
            Image handCard3 = go.transform.Find("Image_handCard3").GetComponent<Image>();
            Image handCard4 = go.transform.Find("Image_handCard4").GetComponent<Image>();
            Image publicCard1 = go.transform.Find("Image_publicCard1").GetComponent<Image>();
            Image publicCard2 = go.transform.Find("Image_publicCard2").GetComponent<Image>();
            Image publicCard3 = go.transform.Find("Image_publicCard3").GetComponent<Image>();
            Image publicCard4 = go.transform.Find("Image_publicCard4").GetComponent<Image>();
            Image publicCard5 = go.transform.Find("Image_publicCard5").GetComponent<Image>();
            //rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(mPublicCardInfo.cardId));

            // int handCardNum = GameCache.Instance.room_path == (int)RoomPath.Omaha ? 4 : 2;
            int handCardNum = GameCache.Instance.CurGame.HandCards;

            handCard1.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(element.firstCard));
            handCard2.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(element.secondCard));

            if (handCardNum == 4)
            {
                //Omaha
                handCard3.gameObject.SetActive(true);
                handCard4.gameObject.SetActive(true);
                handCard3.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(element.thirdCard));
                handCard4.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(element.fourthCard));
                handCard1.gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-237, 0, 0);
                handCard2.gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-210, 0, 0);
            }
            else
            {
                handCard3.gameObject.SetActive(false);
                handCard4.gameObject.SetActive(false);
                handCard1.gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-219, 0, 0);
                handCard2.gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-173, 0, 0);
            }

            publicCard1.gameObject.SetActive(false);
            publicCard2.gameObject.SetActive(false);
            publicCard3.gameObject.SetActive(false);
            publicCard4.gameObject.SetActive(false);
            publicCard5.gameObject.SetActive(false);

            int usedPublicCard = 0;
            for (int i = 0; i < 5; i++)
            {
                Image publicCard = go.transform.Find($"Image_publicCard{i + 1}").GetComponent<Image>();
                if (rec.publicCard[i] == -1)
                {
                    //没发完的公共牌不显示
                    publicCard.gameObject.SetActive(false);
                }
                else
                {
                    usedPublicCard++;
                    publicCard.gameObject.SetActive(true);
                    publicCard.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum((sbyte)rec.publicCard[i]));
                }
            }

            //有最大牌型就高亮区分，否则不做区别
            int cardNumber = 5 + handCardNum;
            if (usedPublicCard == 5 && element.maxCardIndex[0] > -1)
            {
                int[] cardUsed = new int[cardNumber];
                for (int i = 0; i < cardNumber; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (i == element.maxCardIndex[j])
                        {
                            cardUsed[i] = 1;
                        }
                    }
                }
                for (int i = 0; i < cardNumber; i++)
                {
                    Image cardImage = GetCardImageAtIndex(i, go);
                    if (cardUsed[i] == 0)
                    {
                        cardImage.color = new Color32(127, 127, 127, 255);
                    }
                    else
                    {
                        cardImage.color = new Color32(255, 255, 255, 255);
                    }
                }
            }
            else
            {
                for (int i = 0; i < cardNumber; i++)
                {
                    Image cardImage = GetCardImageAtIndex(i, go);
                    cardImage.color = new Color32(255, 255, 255, 255);
                }
            }

            //操作
            for (int i = 0; i < 4; ++i)
            {
                sbyte opType = element.operationTypes[i];
                int opAnte = element.operationAntes[i];
                // 说明：
                // -1表示前一轮已经弃牌或者ALL IN，其余情况最高1位表示本轮是否弃牌（1表示弃牌，0表示进行到下一轮），
                // 剩余7位（0表示直接弃牌，1表示过牌，2表示跟注，3表示加注，4表示ALL IN）
                string strOpType = null;
                string strOpAnte = StringHelper.GetShortString(opAnte);
                bool foldTail = false;
                switch (opType)
                {
                    case -1:
                        // -1 -- 占位符，不显示了
                        break;
                    case -128:
                        // -128 -- 本轮直接弃牌，要显示
                        // strOpType = "弃牌";
                        strOpType = CPErrorCode.LanguageDescription(10047);
                        break;
                    case -128 + 1:
                        {
                            // -127 -- 本轮弃牌，上一手是过牌
                            // strOpType = "让牌";
                            strOpType = CPErrorCode.LanguageDescription(10315);
                            foldTail = false;
                        }
                        break;
                    case -128 + 2:
                        {
                            // -126 -- 本轮弃牌，上一手是跟注
                            // strOpType = "跟注";
                            strOpType = CPErrorCode.LanguageDescription(10044);
                            foldTail = true;
                            break;
                        }
                    case -128 + 3:
                        {
                            // -125 -- 本轮弃牌，上一手是跟注
                            // strOpType = "加注";
                            strOpType = CPErrorCode.LanguageDescription(10045);
                            foldTail = true;
                            break;
                        }
                    case -128 + 4:
                        {
                            // -124 -- 本轮弃牌，上一手是ALL IN 原则上不可能？
                            strOpType = "ALL IN";
                            foldTail = true;
                            break;
                        }
                    case 1:
                        // 1 -- 看牌，要显示
                        // strOpType = "让牌";
                        strOpType = CPErrorCode.LanguageDescription(10315);
                        break;
                    case 2:
                        // 2 -- 跟注，要显示
                        // strOpType = "跟注";
                        strOpType = CPErrorCode.LanguageDescription(10044);
                        break;
                    case 3:
                        // 3 -- 加注，要显示
                        // strOpType = "加注";
                        strOpType = CPErrorCode.LanguageDescription(10045);
                        break;
                    case 4:
                        // 4 -- ALL IN，要显示
                        strOpType = "ALL IN";
                        break;
                    default:
                        // 未知
                        break;
                }

                Text opActionText = go.transform.Find($"Text_Action{i + 1}").GetComponent<Text>();
                Text opNumText = go.transform.Find($"Text_Chips{i + 1}").GetComponent<Text>();
                if (strOpType != null)
                {
                    //要显示
                    opActionText.gameObject.SetActive(true);
                    bool withAnte = (opType != -128 && opType != 1 && opType != -127);  // 让牌或者弃牌时筹码不显示
                    if (foldTail)
                    {
                        strOpType = $"{strOpType} {CPErrorCode.LanguageDescription(10047)}";
                    }
                    if (withAnte)
                    {
                        opNumText.gameObject.SetActive(true);
                        opNumText.text = strOpAnte;
                    }
                    else
                    {
                        opNumText.gameObject.SetActive(false);
                    }
                    opActionText.text = strOpType;
                }
                else
                {
                    opActionText.gameObject.SetActive(false);
                    opNumText.gameObject.SetActive(false);
                }
            }
        }

        private string GetShowCardType(int cardType)
        {
            if (cardType == -2)
            {
                // return "盖牌";
                return CPErrorCode.LanguageDescription(10048);
            }
            return CardTypeUtil.GetCardTypeName(cardType);
        }

        private Image GetCardImageAtIndex(int index, GameObject go)
        {
            // int handCardNum = GameCache.Instance.room_path == (int)RoomPath.Omaha ? 4 : 2;
            int handCardNum = GameCache.Instance.CurGame.HandCards;
            if (index < handCardNum)
            {
                return go.transform.Find($"Image_handCard{index+1}").GetComponent<Image>();
            }
            else
            {
                return go.transform.Find($"Image_publicCard{index - handCardNum + 1}").GetComponent<Image>();
            }
        }


        #region SupperScrollView
        LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
        {
            if (index < 0)
            {
                return null;
            }
            PlayerInfo itemData = playerInfos[index];
            if (itemData == null)
            {
                return null;
            }
            LoopListViewItem2 item = listView.NewListViewItem("player_info");
            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
            }
            GameObject storeObject = item.gameObject;
            SetUpItemWithData(storeObject, itemData);

            return item;
        }
        #endregion

        private void registerHandler()
        {
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_PREV_GAME, HANDLER_REQ_GAME_PREV_GAME);
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_COLLECT_CARD, HANDLER_REQ_GAME_COLLECT_CARD);
        }

        private void removeHandler()
        {
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_PREV_GAME, HANDLER_REQ_GAME_PREV_GAME);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_COLLECT_CARD, HANDLER_REQ_GAME_COLLECT_CARD);
        }

        private string getBrandType(int type)
        {
            string[] names = { "fold", "royal flush", "straight flush", "four of a kind", "full house", "flush", "straight", "three of a kind", "two Pairs", "one Pairs", "high card" };
            if (type < 1 || type > 11)
                return "0";
            return names[type];
        }

        private string getMarkType(int type)
        {
            string[] names = { "SB", "BB", "UTG", "UTG+1", "UTG+2", "MP", "HJ", "CO", "BTN" };
            if (type < 1 || type > 9)
                return "0";
            return names[type-1];
           
        }

        private string getActionStr(int type)
        {
            string[] names = {"A", "B", "C", "X", "F", "S", "SB", "BB", "R", "3B"};
            if (type < 1)
                return "0";

            if (type > 10)
            {
                return (type - 10 + 3).ToString() + "B";
            }
                
            return names[type-1];
        }
        private Sprite getActionColor(int type)
        {
            Sprite typeSprite = null;
            if (type == 7 || type == 8 || type == 3 || type == 4)
            {
                typeSprite = rc.Get<Sprite>("icon_green");
            }
            else if (type == 5)
            {
                typeSprite = rc.Get<Sprite>("icon_grey");
            }
            else
            {
                typeSprite = rc.Get<Sprite>("icon_red");
            }

            return typeSprite;
        }

        private void initPaipuItem(GameObject item, SingleCardScore data)
        {
            var txt_mark = item.transform.Find("Image/mark").gameObject.GetComponent<Text>();
            txt_mark.text = getMarkType(data.role);
            var name = item.transform.Find("name").gameObject.GetComponent<Text>();
            name.text = data.userName;
            var operate = item.transform.Find("operate").gameObject.GetComponent<Text>();
            operate.text = getBrandType(data.brandType);
            var score = item.transform.Find("score").gameObject.GetComponent<Text>();
            score.text = StringHelper.ShowSignedGold(data.profit, false);

            var score_bx = item.transform.Find("score_bx").gameObject.GetComponent<Text>();
            score_bx.gameObject.SetActive(data.insurer != 0);
            if (data.insurer != 0)
            {
                score_bx.text = StringHelper.ShowSignedGold(data.insurer, false);
            }

            List<Image> cards = new List<Image>();
            List<int> cardValues = new List<int>();

            for (int i = 0; i < handCardCount; i++)
            {
                Image card = item.transform.Find("card/hand_card/card" + (i + 1).ToString()).GetComponent<Image>();
                card.gameObject.SetActive(true);
            }

            //手牌
            for (int i = 0; i < data.card.Count; i++)
            {
                Image card = item.transform.Find("card/hand_card/card"+(i+1).ToString()).GetComponent<Image>();
                card.gameObject.SetActive(true);
                card.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum((sbyte)data.card[i]));
                card.color = Color.grey;
                cards.Add(card);
                cardValues.Add(data.card[i]);
            }

            //公共牌
            for (int i = 0; i < paipu.publicCard.Count; i++)
            {
                Image card = item.transform.Find("card/public_card/card" + (i + 1).ToString()).GetComponent<Image>();
                card.gameObject.SetActive(true);
                card.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum((sbyte)paipu.publicCard[i]));
                card.color = Color.grey;
                cards.Add(card);
                cardValues.Add(paipu.publicCard[i]);
            }

            //显示高亮 data.liangPai
            foreach (var pcard in data.liangPai)
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    if (cardValues[i] == pcard)
                    {
                        cards[i].color = Color.white;
                        break;
                    }
                }
            }
           
        }

        private void initPaipuInfoItem(GameObject item, SingleProcess data, bool isP)
        {
            var txt_mark = item.transform.Find("Image/mark").gameObject.GetComponent<Text>();
            txt_mark.text = getMarkType(data.role);

            var name = item.transform.Find("name").gameObject.GetComponent<Text>();
            name.text = data.userName;

            var operate_bg = item.transform.Find("operate_bg").gameObject.GetComponent<Image>();
            operate_bg.sprite = getActionColor(data.action);

            var operate = item.transform.Find("operate_bg/operate").gameObject.GetComponent<Text>();
            operate.text = getActionStr(data.action);

            var ma = item.transform.Find("ma").gameObject.GetComponent<Text>();
            ma.text = StringHelper.ShowGold(data.wager);

            var score = item.transform.Find("score").gameObject.GetComponent<Text>();
            score.text = StringHelper.ShowGold(data.chip);
            if (isP)
            {
                score.text = "P:" + score.text;
            }

        }

        protected void HANDLER_REQ_GAME_PREV_GAME(ICPResponse response)
        {
            rec = response as REQ_GAME_PREV_GAME;
            if (null == rec)
                return;

            if (rec.status != 0)
            {
                return;
            }

            //LayoutRebuilder.ForceRebuildLayoutImmediate(scrollview_Content);

            paipu = JsonHelper.FromJson<CardScore>(rec.paipu);
            var paipuinfo = JsonHelper.FromJson<List<GameProcess>>(rec.paipuinfo);
            CardScore showdowninfo = null;
            if (rec.showdown != "null")
                showdowninfo = JsonHelper.FromJson<CardScore>(rec.showdown);

            handCardCount = 2;
            if (rec.roomPath == 91 || rec.roomPath == 92 || rec.roomPath == 93)
            {
                handCardCount = 4;
            }

            //textinsuranceValue.text = StringHelper.GetShortSignedString(rec.insuranceSum);
            //textJackPotValue.text = StringHelper.GetShortSignedString(rec.jackPotSum);
            currentPage = rec.currentHandId;
            textnum.text = $"{currentPage}/{rec.totalHandsNumber}";

            buttonFirstPage.interactable = currentPage > 1;
            buttonLastPage.interactable = currentPage < rec.totalHandsNumber;
            buttonPrePage.interactable = currentPage > 1;
            buttonNextPage.interactable = currentPage < rec.totalHandsNumber;

            if (GameCache.Instance.room_path == 21 || GameCache.Instance.room_path == 23)//短牌
            {
                room_blind.text = StringHelper.ShowGold(int.Parse(rec.blind));
            }
            else
            {
                room_blind.text = StringHelper.ShowBlindName(rec.blind);
            }

            room_people.text = rec.playermum.ToString();
            paipu_people.text = paipu.userNumber.ToString();
            paipu_gold.text = StringHelper.ShowGold(paipu.poolChip);

            for (int i = 0; i < 5; i++)
            {
                Image card = public_card.transform.Find("card" + (i + 1).ToString()).GetComponent<Image>();
                if (card != null)
                    card.gameObject.SetActive(false);
            }
            //公共牌
            for (int i = 0; i < paipu.publicCard.Count; i++)
            {
                Image card = public_card.transform.Find("card" + (i + 1).ToString()).GetComponent<Image>();
                if(card != null)
                    card.gameObject.SetActive(true);
                    card.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum((sbyte)paipu.publicCard[i]));
            }

            //牌谱
            List<GameObject> paipu_players = new List<GameObject>();
            GameObject playerItem = null;
            if (handCardCount == 2)
            {
                playerItem = paipu_player.gameObject;
            }
            else if (handCardCount == 4)
            {
                playerItem = paipu_player2.gameObject;
            }

            for (int i = 2;i< paipu_panel.childCount;i++)
            {
                GameObject.Destroy(paipu_panel.GetChild(i).gameObject);
            }
            foreach (var i in paipu.card)
            {
                var item = GameObject.Instantiate(playerItem) as GameObject;
                item.transform.parent = paipu_panel;
                item.SetActive(true);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = new Vector3(1, 1, 1);
                initPaipuItem(item, i);
                paipu_players.Add(item);
            }

            //牌谱详情
            preflop.gameObject.SetActive(false);
            flop.gameObject.SetActive(false);
            turn.gameObject.SetActive(false);
            river.gameObject.SetActive(false);
            foreach (var p in paipuinfo)
            {
                RectTransform parent = null;
                Text txt_gold = null;
                Text txt_num = null;
                RectTransform cardRect = null;

                if (p.shoushuCode == 1)
                {
                    parent = preflop;
                    txt_gold = preflop_gold;
                    cardRect = preflop_card;
                }

                if (p.shoushuCode == 2)
                {
                    parent = flop;
                    txt_gold = flop_gold;
                    txt_num = flop_people;
                    cardRect = flop_card;
                }

                if (p.shoushuCode == 3)
                {
                    parent = turn;
                    txt_gold = turn_gold;
                    txt_num = turn_people;
                    cardRect = turn_card;
                }

                if (p.shoushuCode == 4)
                {
                    parent = river;
                    txt_gold = river_gold;
                    txt_num = river_people;
                    cardRect = river_card;
                }
                if (parent == null)
                    break;

                parent.gameObject.SetActive(true);
                txt_gold.text = StringHelper.ShowGold(p.chip);
                if (txt_num != null)
                {
                    txt_num.text = p.users.ToString();
                }

                //公共牌
                for (int i = 0; i < p.publicCard.Count; i++)
                {
                    Image card = cardRect.transform.Find("card" + (i + 1).ToString()).GetComponent<Image>();
                    if (card != null)
                    {
                        card.gameObject.SetActive(true);
                        card.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum((sbyte)p.publicCard[i]));
                    }
                }

                //连续弃牌不显示
                Dictionary<int, int> giveupdict = new Dictionary<int, int>();

                int bgiveup = 0;
                int startpos = -1;

                for (int i = 0; i < p.singleProcessList.Count; i++)
                {
                    if (p.singleProcessList[i].action == 5)
                    {
                        if (startpos == -1)
                        {
                            startpos = i;
                            bgiveup = 1;
                        }
                        else
                        {
                            bgiveup++;
                        }
                    }
                    else
                    {
                        if (startpos > -1)
                        {
                            giveupdict.Add(startpos, bgiveup);
                            bgiveup = 0;
                            startpos = -1;
                        }
                    }
                }

                for (int i = 1; i < parent.childCount; i++)
                {
                    GameObject.Destroy(parent.GetChild(i).gameObject);
                }

                bool isp = (p.shoushuCode > 1);

                for (int i = 0; i < p.singleProcessList.Count; i++)
                {
                    int gnum = 0;
                    giveupdict.TryGetValue(i, out gnum);
                    if (gnum <= 1)
                    {
                        GameObject player = GameObject.Instantiate(paipuinfo_player.gameObject) as GameObject;
                       
                        player.transform.parent = parent;
                        player.SetActive(true);
                        player.transform.localPosition = Vector3.zero;
                        player.transform.localScale = new Vector3(1, 1, 1);
                        initPaipuInfoItem(player, p.singleProcessList[i], isp);
                    }
                    else
                    {
                        var folds = GameObject.Instantiate(paipuinfo_folds.gameObject) as GameObject;
                        folds.transform.parent = parent;
                        folds.SetActive(true);
                        folds.transform.localPosition = Vector3.zero;
                        folds.transform.localScale = new Vector3(1, 1, 1);
                        var txt_folds = folds.transform.Find("ma").gameObject.GetComponent<Text>();
                        txt_folds.text = gnum+"folds";

                        i = i + gnum - 1;
                    }
                }
               
            }
            //showdown
            showdown.gameObject.SetActive(false);
            InfoList.gameObject.SetActive(false);
            if (showdowninfo != null)
            {
                showdown.gameObject.SetActive(true);
                show_gold.text = StringHelper.ShowGold(showdowninfo.poolChip);
                show_people.text = showdowninfo.userNumber.ToString();

                for (int i = 0; i < 5; i++)
                {
                    Image card = show_card.transform.Find("card" + (i + 1).ToString()).GetComponent<Image>();
                    if (card != null)
                        card.gameObject.SetActive(false);
                }

                if (paipu.publicCard != null)
                {
                    //公共牌
                    for (int i = 0; i < paipu.publicCard.Count; i++)
                    {
                        Image card = show_card.transform.Find("card" + (i + 1).ToString()).GetComponent<Image>();
                        if (card != null)
                        {
                            card.gameObject.SetActive(true);
                            card.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum((sbyte)paipu.publicCard[i]));
                        }
                    }
                }

                for (int i = 1; i < showdown.childCount; i++)
                {
                    GameObject.Destroy(showdown.GetChild(i).gameObject);
                }

                foreach (var i in showdowninfo.card)
                {
                    var item = GameObject.Instantiate(playerItem) as GameObject;
                    item.transform.parent = showdown;
                    item.SetActive(true);
                    item.transform.localPosition = Vector3.zero;
                    item.transform.localScale = new Vector3(1, 1, 1);
                    initPaipuItem(item, i);
                }

                //InfoList.gameObject.SetActive(rec.insuranceSum != 0);
                //if (rec.insuranceSum != 0)
                //{
                //    textinsuranceValue.text = StringHelper.GetShortSignedString(rec.insuranceSum);
                //}
            }

            //var scrollview_Content_Content = scrollview_Content.Find("Content").GetComponent<RectTransform>();
            //LayoutRebuilder.ForceRebuildLayoutImmediate(scrollview_Content_Content);

            //var height = scrollview_Content_Content.rect.height;
            //scrollview_Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

            

           //playerInfos.Clear();

            //string[] userNames = rec.userNames.Split(new string[] { "@%" }, StringSplitOptions.None);
            //string[] headPics = rec.headPics.Split(new string[] { "@%" }, StringSplitOptions.None);
            isPlayerInGame = false;
            for (int i = 0; i < rec.playerIds.Count; i++)
            {
                if (rec.playerIds[i] == GameCache.Instance.nUserId)
                {
                    isPlayerInGame = true;
                }
            }
            //for (int i = 0; i < rec.playerIds.Count; i++)
            //{
            //    PlayerInfo player = new PlayerInfo();
            //    player.playerId = rec.playerIds[i];
            //    player.playerType = rec.playerTypes[i];
            //    //player.userName = userNames[i];
            //   // player.headPic = headPics[i];
            //    player.firstCard = rec.firstCard[i];
            //    player.secondCard = rec.secondCard[i];
            //    // if (GameCache.Instance.room_path == (int)RoomPath.Omaha)
            //    if (GameCache.Instance.CurGame.HandCards == 4)
            //    {
            //        player.thirdCard = rec.thirdCard[i];
            //        player.fourthCard = rec.fourthCard[i];
            //    }
            //    player.maxCardIndex = rec.maxCardIndex.GetRange(i*5, 5);
            //    player.maxCardType = rec.maxCardType[i];
            //    player.winAnte = rec.winAntes[i];
            //    if (player.playerId == GameCache.Instance.nUserId)
            //    {
            //        player.isMine = true;
            //        isPlayerInGame = true;
            //    }
            //    else
            //    {
            //        player.isMine = false;
            //    }
            //    if (rec.insuranceGains != null)
            //    {
            //        player.insuranceGain = rec.insuranceGains[i];
            //    }
            //    player.operationTypes = rec.operationTypes.GetRange(i * 4, 4);
            //    player.operationAntes = rec.operationAntes.GetRange(i * 4, 4);

            //    playerInfos.Add(player);
            //}
            //if (bInitListView)
            //{
            //    mLoopListView.SetListItemCount(playerInfos.Count);
            //    mLoopListView.RefreshAllShownItem();
            //}
            //else
            //{
            //    mLoopListView.InitListView(playerInfos.Count, OnGetItemByIndex);
            //    bInitListView = true;
            //}
            buttonShare.interactable = isPlayerInGame;
            buttonCollect.interactable = isPlayerInGame;
            //if (!isPlayerInGame)
            //{
                //textShareTip.color = new Color32(233, 191, 128, 130);
                //textCollectTip.color = new Color32(233, 191, 128, 130);
            //}
            //else
            //{
                //textShareTip.color = new Color32(233, 191, 128, 255);
                //textCollectTip.color = new Color32(233, 191, 128, 255);
            //}
            isCollect = rec.isCollected == 1;
            if (rec.isCollected == 1)
            {
                buttonCollect.gameObject.GetComponent<Image>().sprite = rc.Get<Sprite>("review_button_collection_selected");
            }
            else
            {
                buttonCollect.gameObject.GetComponent<Image>().sprite = rc.Get<Sprite>("review_button_collection_normal");
            }

            ViewUpdate();
        }

        private async void ViewUpdate()
        {
            await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(10);
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollview_Content);

        }

        protected void HANDLER_REQ_GAME_COLLECT_CARD(ICPResponse response) {
            REQ_GAME_COLLECT_CARD collectRec = response as REQ_GAME_COLLECT_CARD;
            if (null == collectRec)
                return;

            if (collectRec.status == 0)
            {
                if (collectRec.handIdRec != currentPage)
                    return;
                isCollect = collectRec.collectRec == 1;
                if (collectRec.collectRec == 1)
                {
                    buttonCollect.gameObject.GetComponent<Image>().sprite = rc.Get<Sprite>("review_button_collection_selected");
                }
                else
                {
                    buttonCollect.gameObject.GetComponent<Image>().sprite = rc.Get<Sprite>("review_button_collection_normal");
                }
            }
            else if (collectRec.status == 1)
            {
                // UIComponent.Instance.Toast("收藏牌谱已达上限");
                UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10306));
            }

        }

        IEnumerator UpdateLayout(RectTransform rect)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            yield return new WaitForEndOfFrame();
            Vector3 vecScale = rect.localScale;
            float width = rect.rect.width;
            float height = rect.rect.height;
            while (rect.rect.width == 0)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
                yield return new WaitForEndOfFrame();
            }
        }

    }
}

