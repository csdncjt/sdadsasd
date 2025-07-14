using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SuperScrollView;
using HallDto = ETHotfix.WEB2_room_list.DataElement;//别名,大厅列表Dto
using MyPaiDto = ETHotfix.WEB2_room_mylist.DataElement;

using BlindDto = ETHotfix.WEB_room_blindData.GameBlindElement;

using UnityEngine.SceneManagement;

using DragonBones;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMatchComponentAwakeSystem : AwakeSystem<UIMatchComponent>
    {
        public override void Awake(UIMatchComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>     起过别名的  HallDto = ETHotfix.WEB2_room_list.DataElement;     </summary>
    public class UIMatchComponent : UIBaseComponent
    {
        private AsyncOperation asyncCowboyOpt;

        private ReferenceCollector rc;
        private GameObject transRoomList;
        private GameObject transMatchList;

        private Transform ListRooms;

        private GameObject uicreatjoin;
        private GameObject uijoin;

        private GameObject transMyMatch;
        private GameObject transTopLineMatch;
        private Dictionary<string, Sprite> mDicStrSprite;
        private GameObject Image_MsgRedPoint;
        private Image ImageBtnClass;
        private Button buttonMoreMtt;
        private Dictionary<int, List<int>> mDicPageFilter2 = new Dictionary<int, List<int>>();
        private Dictionary<int, List<int>> mDicPageFilter = new Dictionary<int, List<int>>();
        /// <summary>是否第2次+  进入</summary>
        bool mIsMoreLoad = false;
        List<Toggle> mClassToggles;

        bool hadCheckActivity = false;
        bool isWaitingGPSCallBack = false;

        private bool isEnterRoom = false;

        private int listSize = 0;

        Text text_gold_bean;

        public void Awake()
        {
            mIsMoreLoad = false;
            mDicPageFilter = new Dictionary<int, List<int>>();
            mDicPageFilter2 = new Dictionary<int, List<int>>();
            mItemStartV3 = Vector3.zero;
            InitUI();
            LoadSpriteDics();
            UIMatchModel.mInstance.APIGetBatchRelationSNS();
            UIMineModel.mInstance.AddListenerMsg();
            UIMineModel.mInstance.CancelShowUIMine += RefreshRedPoint;

            hadCheckActivity = false;

            registerHandler();

            //asyncCowboyOpt = SceneManager.LoadSceneAsync("Scenes/Game");
            //asyncCowboyOpt.allowSceneActivation = false;
        }

        private void CheckActivity()
        {
            //是否上报活动用户信息
            if (GameCache.Instance.isActivity == 1 && !hadCheckActivity)
            {
                //先获取GPS权限
                NativeManager.GetGPSLocation();
                isWaitingGPSCallBack = true;
            }
            hadCheckActivity = true;
        }

        public void OnGPSCallBack(string status)
        {
            if (isWaitingGPSCallBack)
            {
                isWaitingGPSCallBack = false;
                int gps = status == "0" ? 1 : 0;
                UIMatchModel.mInstance.APIActivityKdouApply(gps);

                if (GameCache.Instance.isActivity == 1)
                    UIMatchModel.mInstance.APIActivityRechargeApply(gps);
            }

        }

        #region  sprite load出来到dic

        void LoadSpriteDics()
        {
            mDicStrSprite = new Dictionary<string, Sprite>();
            AddDicSprite("icon_list_playing");
            AddDicSprite("icon_list_waiting");
            AddDicSprite("color_jp");
            AddDicSprite("color_baoxian");
            AddDicSprite("color_laizi");
            AddDicSprite("color_tongshi");
            AddDicSprite("color_xuezhan");
            AddDicSprite("color_aomaha");
            AddDicSprite("FilterSelected");
            AddDicSprite("FilterSelectNon");
            AddDicSprite("game_icon_61_0");
            AddDicSprite("game_icon_61_1");
            AddDicSprite("game_icon_61_2");
            AddDicSprite("game_icon_91_0");
            AddDicSprite("game_icon_91_1");
            AddDicSprite("game_icon_91_2");
            AddDicSprite("img_default_head");
        }

        void AddDicSprite(string str)
        {
            mDicStrSprite[str] = rc.Get<Sprite>(str);
        }
        #endregion

        #region 筛选按钮
        /// <summary>         非大厅  是否已选         </summary>
        void SetFilterImage_Other(List<int> pFilter)
        {
            bool tSelected = false;
            for (int i = 1; i < pFilter.Count; i++)
            {
                if (pFilter[i] != -1)
                    tSelected = true;
            }
            ImageBtnClass.sprite = (tSelected ? mDicStrSprite["FilterSelected"] : mDicStrSprite["FilterSelectNon"]);
        }

        /// <summary>
        /// 大厅  是否已选
        /// </summary>
        void SetFilterImage_Hall(List<int> pFilter)
        {
            bool tSelected = false;
            for (int i = 0; i < pFilter.Count; i++)
            {
                if (pFilter[i] != -1)
                    tSelected = true;
            }
            ImageBtnClass.sprite = (tSelected ? mDicStrSprite["FilterSelected"] : mDicStrSprite["FilterSelectNon"]);
        }
        #endregion

        protected virtual void InitUI()
        {
            rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            GameObject baseView = rc.Get<GameObject>("view_main");
            NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
            float realTop = safeArea.top * 1242 / safeArea.width;
            baseView.GetComponent<RectTransform>().offsetMax = new Vector2(0, -realTop);
            float realBottom = safeArea.bottom * 1242 / safeArea.width;
            baseView.GetComponent<RectTransform>().offsetMin = new Vector2(0, realBottom);

            Image_MsgRedPoint = rc.Get<GameObject>("Image_MsgRedPoint");

            transMatchList = rc.Get<GameObject>("ListScrollView2");

            transMyMatch = rc.Get<GameObject>("MyMatch");
            transMyMatch.gameObject.SetActive(false);
            UIEventListener.Get(transMyMatch.gameObject).onClick = ClickMyMatchItem;
            transTopLineMatch = rc.Get<GameObject>("TopLineMatch");
            transTopLineMatch.gameObject.SetActive(false);

            transRoomList = rc.Get<GameObject>("ListScrollView");

            ListRooms = rc.Get<GameObject>("ListRooms").transform;

            uicreatjoin = rc.Get<GameObject>("uicreatjoin");
            uijoin = rc.Get<GameObject>("uijoin");
            uicreatjoin.SetActive(false);
            uijoin.SetActive(false);

            InitTargetClass();
            InitBottomEle();


            text_gold_bean = rc.Get<GameObject>("text_gold_bean").GetComponent<Text>();
            UIEventListener.Get(rc.Get<GameObject>("btn_bean_store")).onClick = (go) =>
            {//商城
                if (CanClick() == false)
                    return;
                lastClickTime = GetNowTime();
                UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletAddBeansList);
            };

            UIEventListener.Get(rc.Get<GameObject>("BtnAddRoom")).onClick = (go) =>
            {
                uicreatjoin.SetActive(true);
            };

            UIEventListener.Get(rc.Get<GameObject>("ui_btn_close")).onClick = (go) =>
            {
                uicreatjoin.SetActive(false);
            };

            UIEventListener.Get(rc.Get<GameObject>("ui_btn_creat")).onClick = (go) =>
            {
                uicreatjoin.SetActive(false);
                UIComponent.Instance.ShowNoAnimation(UIType.UIClub_RoomCreat, 2, () => { });
            };

            UIEventListener.Get(rc.Get<GameObject>("ui_btn_join")).onClick = async (go) =>
            {
                uicreatjoin.SetActive(false);
                uijoin.SetActive(true);
                rc.Get<GameObject>("uijoin_InputField").GetComponent<InputField>().text = "";
            };

            UIEventListener.Get(rc.Get<GameObject>("uijoin_close")).onClick = (go) =>
            {
                uijoin.SetActive(false);
            };

            UIEventListener.Get(rc.Get<GameObject>("uijoin_join")).onClick = (go) =>
            {
                string strid = rc.Get<GameObject>("uijoin_InputField").GetComponent<InputField>().text;
                if (strid.Length < 6)
                {
                    return;
                }
                uijoin.SetActive(false);
                if (isEnterRoom)
                    return;
                isEnterRoom = true;
                EnterRoomAPI(int.Parse(strid));
            };

            UIEventListener.Get(rc.Get<GameObject>("BtnMsg")).onClick = ClickBtnMsg;
            UIEventListener.Get(rc.Get<GameObject>("BtnCowboy")).onClick = ClickBtnCowboy;
            ImageBtnClass = rc.Get<GameObject>("BtnClass").GetComponent<Image>();

            buttonMoreMtt = rc.Get<GameObject>("Button_MoreMTT").GetComponent<Button>();
            UIEventListener.Get(buttonMoreMtt.gameObject).onClick = ClickBtnMoreMTT;

            UIEventListener.Get(rc.Get<GameObject>("Text_LeftTop")).onClick = go =>
            {
                //Log.Debug("不显示了");
                //UIComponent.Instance.Show(UIType.UILobby_Activity, "16754");
            };
        }

        void EnterRoomAPI(int pRoomId)
        {
            UIMatchModel.mInstance.APIEnterRoom(-1, pRoomId, pDto =>
            {
                GameCache.Instance.roomName = pDto.name;
                GameCache.Instance.roomIP = pDto.gmip;
                GameCache.Instance.roomPort = int.Parse(pDto.gmprt);
                GameCache.Instance.room_path = pDto.rpath;
                GameCache.Instance.rtype = pDto.rtype;
                GameCache.Instance.room_id = pDto.rmid;
                GameCache.Instance.client_ip = pDto.cliip;
                GameCache.Instance.carry_small = pDto.incp;
                GameCache.Instance.straddle = pDto.sdlon;
                GameCache.Instance.insurance = pDto.isron;
                GameCache.Instance.muck_switch = pDto.muckon;
                GameCache.Instance.jackPot_fund = pDto.jpfnd;
                GameCache.Instance.jackPot_on = pDto.jpon;
                GameCache.Instance.jackPot_id = pDto.jpid;
                GameCache.Instance.shortest_time = pDto.gmxt;

                GameCache.Instance.roomMode = 2;

                AsyncEnterRoom();
            }, () =>
            {
                isEnterRoom = false;
            });

        }

        #region 同时点击事件   1.1秒后才能点击
        double lastClickTime = 0;//最后一次点击
        double GetNowTime()//当前时间
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return ts.TotalMilliseconds;
        }
        /// <summary>         是否能点击  true=能点击         </summary>
        bool CanClick()
        {
            if (GetNowTime() - lastClickTime > 1100)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 左上角 滚轮       

        ParticleSystem douziPS;
        void PlayerDouziPs()
        {
            douziPS.Play();
        }

        #endregion

        #region 德州牛仔
        private void ClickBtnCowboy(GameObject go)
        {
            //GameObject.Find("Global").GetComponent<ETModel.Init>().changeScreen(ScreenOrientation.Landscape);
            //UIComponent.Instance.ShowNoAnimation(UIType.UIMatch_Loading); //Loading页面.在UItexas生成就 remove掉                
            //await UIComponent.Instance.ShowAsyncNoAnimation(UIType.UICowBoy, UIType.UIMatch);
            //isEnterRoom = false;
        }
        #endregion

        #region 消息
        private void ClickBtnMsg(GameObject go)
        {
            UIMineModel.mInstance.APIGetMessageNumList(pDtos =>
            {
                UIComponent.Instance.ShowNoAnimation(UIType.UIMine_MsgSummary, new UIMine_MsgSummaryComponent.MsgSummaryData()
                {
                    mDicMsgLast = pDtos,
                    mOpenResource = 0
                });
            });
           
                

        }
        #endregion

        #region MTT
        private ETHotfix.WEB2_room_mtt_list.RoomListElement mttElement;
        private void ClickBtnMoreMTT(GameObject go)
        {
            UIComponent.Instance.ShowNoAnimation(UIType.UIMatch_MttList, 5);
        }

        private void ClickMyMatchItem(GameObject go)
        {
            UIComponent.Instance.ShowNoAnimation(UIType.UIMatch_MttDetail, mttElement.matchId);
        }

        public void ObtainMTTListData()
        {
            UIMatchModel.mInstance.APIMTTRoomList(1, 1, 1, pDto =>
            {
                if (pDto.status == 0)
                {
                    if (pDto.data.roomList.Count > 0)
                    {
                        transMyMatch.gameObject.SetActive(true);
                        transTopLineMatch.gameObject.SetActive(true);
                        mttElement = pDto.data.roomList[0];
                        SetMTTItemInfo();
                    }
                    else
                    {
                        transMyMatch.gameObject.SetActive(false);
                        transTopLineMatch.gameObject.SetActive(false);
                        mttElement = null;
                    }
                }
                else
                {
                    transMyMatch.gameObject.SetActive(false);
                    transTopLineMatch.gameObject.SetActive(false);
                    mttElement = null;
                }
            });
        }

        void SetMTTItemInfo()
        {
            GameObject go = transMyMatch.gameObject;
            var pDto = mttElement;
            RawImage imageMatchIcon = go.transform.Find("Image_MatchIcon").GetComponent<RawImage>();
            Text textStatus = go.transform.Find("Text_Status").GetComponent<Text>();
            Image imageStatus = go.transform.Find("Image_Status").GetComponent<Image>();
            ReferenceCollector statusImgrc = imageStatus.gameObject.GetComponent<ReferenceCollector>();
            Button btnSignUp = go.transform.Find("BtnSignUp").GetComponent<Button>();
            Text textBtn = btnSignUp.gameObject.transform.Find("Text").GetComponent<Text>();
            Text textName = go.transform.Find("Text_MatchName").GetComponent<Text>();
            Text textTime = go.transform.Find("Text_MatchTime").GetComponent<Text>();
            Text textProgress = go.transform.Find("Text_MatchProgress").GetComponent<Text>();
            Text textFee = go.transform.Find("Text_MatchFee").GetComponent<Text>();

            if(pDto.logoUrl.Length > 0)
                WebImageHelper.SetUrlImage(imageMatchIcon, pDto.logoUrl);
            textName.text = pDto.matchName;
            textTime.text = LanguageManager.Get("MTT-Start Time") + TimeHelper.GetDateTimer(pDto.startTime).ToString("yyyy/MM/dd HH:mm");
            textProgress.text = $"{pDto.currEntryNum}/{pDto.upperLimit}";
            textFee.text = StringHelper.ShowGold(pDto.entryFee);

            if (pDto.gameStatus == 0 || pDto.gameStatus == 1 || pDto.gameStatus == 4)
            {
                textStatus.text = LanguageManager.Get("MTT-Applying");
                imageStatus.sprite = statusImgrc.Get<Sprite>("images_dating_mtt_baomingzhong");
            }
            else
            {
                textStatus.text = LanguageManager.Get("MTT-Processing");
                imageStatus.sprite = statusImgrc.Get<Sprite>("images_dating_mtt_jinxinzhong");
            }

            if (pDto.canPlayerRebuy)
            {
                //重购
                btnSignUp.interactable = true;
                textBtn.text = LanguageManager.Get("MTT_Rebuy");
            }
            else
            {
                //0:可报名;1:等待开赛;2:延期报名;3:进行中;4:立即进入;5:报名截止;6:等待审核 7:重购条件不足

                btnSignUp.interactable = false;
                if (pDto.gameStatus == 0)
                {
                    //报名
                    textBtn.text = LanguageManager.Get("MTT-Apply");
                    btnSignUp.interactable = true;
                }
                else if (pDto.gameStatus == 1)
                {
                    //等待开赛
                    textBtn.text = LanguageManager.Get("mtt_btn_waiting_start");
                }
                else if (pDto.gameStatus == 2)
                {
                    //延时报名
                    textBtn.text = LanguageManager.Get("mtt_btn_delay");
                    btnSignUp.interactable = true;
                }
                else if (pDto.gameStatus == 3)
                {
                    //进行中
                    textBtn.text = LanguageManager.Get("mtt_btn_ongoing");
                    btnSignUp.interactable = true;
                }
                else if (pDto.gameStatus == 4)
                {
                    //立即进入
                    textBtn.text = LanguageManager.Get("mtt_btn_enter");
                    btnSignUp.interactable = true;
                }
                else if (pDto.gameStatus == 5)
                {
                    //报名截止
                    textBtn.text = LanguageManager.Get("mtt_btn_sign_up_deadline");
                }
                else if (pDto.gameStatus == 6)
                {
                    //等待审核
                    textBtn.text = LanguageManager.Get("mtt_btn_waiting_approval");
                }
                else if (pDto.gameStatus == 7)
                {
                    //已淘汰，且不能重购
                    textBtn.text = LanguageManager.Get("mtt_btn_rebuy");
                }
            }

            UIEventListener.Get(btnSignUp.gameObject).onClick = onClickMTTActionBtn;
        }

        private void onClickMTTActionBtn(GameObject go)
        {
            if (go.GetComponent<Button>().interactable == false)
                return;
            var tDto = mttElement;
            if (tDto != null)
            {
				UIMatchMTTModel.mInstance.onMTTAction(tDto, UIType.UIMatch, finish =>
				{
					ObtainMTTListData();
				});
			}
		}

        private void registerHandler()
        {
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_MTT_REMIND, HANDLER_REQ_GAME_MTT_REMIND);  // MTT比赛开始消息  (收到该消息，会在全局顶部弹出比赛即将开始的通知:"锦标赛xxx即将开始，是否进入比赛？")
        }

        private void removeHandler()
        {
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_MTT_REMIND, HANDLER_REQ_GAME_MTT_REMIND);
        }

        protected void HANDLER_REQ_GAME_MTT_REMIND(ICPResponse response)
        {
            REQ_GAME_MTT_REMIND rec = response as REQ_GAME_MTT_REMIND;
            if (null == rec)
                return;

            // 是否当前MTT牌桌中
            bool inGame = false;
            bool inCurGame = false;
            UI mUITexas = UIComponent.Instance.Get(UIType.UITexas);
            if (null != mUITexas && mUITexas.GameObject.activeInHierarchy)
            {
                inGame = true;
                if (GameCache.Instance.room_id == rec.mttId)
                {
                    inCurGame = true;
                }
            }
            
            if (!inCurGame)
            {
                string content = string.Format(LanguageManager.Get("MTT_Start_Remind"), rec.mttName);//$"锦标赛{rec.mttName}即将开始，是否进入比赛？";
                UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                {
                    type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                    title = string.Empty,
                    content = content,
                    contentCommit = CPErrorCode.LanguageDescription(10012),
                    contentCancel = CPErrorCode.LanguageDescription(10013),
                    actionCommit = () => {
                        if (inGame)
                        {
                            GameCache.Instance.CurGame.doExitRoom();
                        }
                        EnterMTTGame(rec.mttId);
                    },
                    actionCancel = null,
                });
            }
        }

        private void EnterMTTGame(int gameID)
        {
            UIMatchModel.mInstance.APIEnterMTTRoom((int)RoomPath.MTT, gameID, pDto =>
            {
                GameCache.Instance.roomName = pDto.matchName;
                GameCache.Instance.roomIP = pDto.matchIp;
                GameCache.Instance.roomPort = pDto.matchPort;
                GameCache.Instance.room_path = pDto.roomPath;
                GameCache.Instance.rtype = (int)RoomPath.MTT;
                GameCache.Instance.room_id = pDto.matchId;
                GameCache.Instance.client_ip = pDto.clientIp;
                GameCache.Instance.mtt_type = pDto.mttType;

                GameCache.Instance.mtt_deskId = 0;
                MTTGameUtil.initList(pDto.blindNote, pDto.ante);
                UIComponent.Instance.ShowNoAnimation(UIType.UIMatch_Loading); //Loading页面.在UItexas生成就 remove掉
                RemoveLogicCanvas();
                UIComponent.Instance.ShowNoAnimation(UIType.UITexas, null);
            });
        }

        private void RemoveLogicCanvas()
        {
            GameObject logicCanvas = GameObject.Find("Global/UI/LogicCanvas");
            for (int i = 0; i < logicCanvas.transform.childCount; i++)
            {
                GameObject subObj = logicCanvas.transform.GetChild(i).gameObject;
                string name = subObj.name;
                name = name.Replace("(Clone)", "");
                UIComponent.Instance.Remove(name);
            }
        }


        #endregion

        /// <summary>         筛选完后的回调         </summary>
        private void OnGetFilterValue(List<int> pFilter, List<int> pFilter2, int gameType)
        {
            var tTab = -1;
            //if (isHall == true)
            //{
            //    tTab = 1;
            //    SetFilterImage_Hall(pFilter);
            //}
            //else if (isHall == false)
            {// 牌局类型,不需此条件，传值：-1   61=德州   91=奥马哈  51=大菠萝  41=必下场   31=AOF   81=SNG  71=MTT
                if (pFilter[0] == 61)
                    tTab = 3;
                else if (pFilter[0] == 91)
                    tTab = 4;
                else if (pFilter[0] == 31)
                    tTab = 5;
                else if (pFilter[0] == 41)
                    tTab = 6;

                SetFilterImage_Other(pFilter);
            }

            mDicPageFilter[tTab] = pFilter;
            mDicPageFilter2[pFilter[0]] = pFilter2;
            UIMatchModel.mInstance.APIHallRoomListFilter(0, 0, pAct =>
            {
                if (pAct.status == 0)
                {
                    listSize = int.Parse(pAct.msg);
                    mHallDtos = pAct.data ?? new List<HallDto>();
                    SortHallDtos();
                    mHallDtos.Add(new HallDto());
                    mLoopListView.SetListItemCount(mHallDtos.Count + 1, false);
                    mLoopListView.RefreshAllShownItem();
                    OnDataSourceLoadMoreFinished(true);
                }
                else
                {
                    listSize = 0;
                    mHallDtos = new List<HallDto>() { new HallDto() };
                    UIComponent.Instance.Toast(pAct.status);
                    OnDataSourceLoadMoreFinished(false);
                }
            }, pFilter, pFilter2, null);
        }


        public override void OnHide()
        {
            ETModel.Game.Hotfix.OnServerMes -= OnServerMsg;

            var tUI = UIComponent.Instance.Get(UIType.UIMatch_Banner);
            if (tUI != null)
            {
                var tBannerTop = tUI.UiBaseComponent as UIMatch_BannerComponent;
                tBannerTop.SetBannerHide();
            }
        }

        public override void OnShow(object obj)
        {
            var tUI = UIComponent.Instance.Get(UIType.UIMatch_Banner);
            if (tUI != null)
            {
                var tWalletMy = tUI.UiBaseComponent as UIMatch_BannerComponent;
                tWalletMy.RequestBanner();
            }

            GetGameBlindList();

            if (mIsMoreLoad == false)
                InitRoomListData();

            //else
            //OnShowClickBottom(mCurSelectTab);

            //OnShowClickBottom(mCurSelectTab);
            text_gold_bean.text = StringHelper.ShowGold(GameCache.Instance.gold, true);
            mIsMoreLoad = true;
            isEnterRoom = false;

            RefreshRedPoint(0);
            ETModel.Game.Hotfix.OnServerMes += OnServerMsg;

            CheckActivity();

            //ObtainMTTListData();
        }

        public void GetGameBlindList()
        {
            mBindDtoDict.Clear();

            UIMatchModel.mInstance.APIGetBlinds(pDto =>
            {
                if (pDto.status == 0)
                {
                    mLoadingTipStatus = LoadingTipStatus.None;
                    mBindDtos = pDto.data ?? new List<BlindDto>();
                    //mBindDtos.Add(new BlindDto());

                    if (mBindDtos.Count > 0)
                    {
                        foreach (BlindDto info in mBindDtos)
                        {
                            if (!mBindDtoDict.ContainsKey(info.type))
                            {
                                mBindDtoDict[info.type] = new Dictionary<int, List<BlindDto>>();
                                List<BlindDto> infos = new List<BlindDto>();
                                infos.Add(info);
                                mBindDtoDict[info.type].Add(info.pattern, infos);
                            }
                            else
                            {
                                if (mBindDtoDict[info.type].ContainsKey(info.pattern))
                                {
                                    mBindDtoDict[info.type][info.pattern].Add(info);
                                }
                                else
                                {
                                    List<BlindDto> infos = new List<BlindDto>();
                                    infos.Add(info);
                                    mBindDtoDict[info.type].Add(info.pattern, infos);
                                }
                            }

                        }

                    }

                    UIMatchModel.mInstance.mBindDtoDict = mBindDtoDict;

                    OnShowClickBottom(mCurSelectTab);
                }
                else
                {
                    UIComponent.Instance.Toast(pDto.status);
                }
            });
        }

        public void NewLoadPanel()
        {
            //OnShow(null);//从大厅回来 OnShow自动会刷的啦
        }

        private void RefreshRedPoint(int pLoad)
        {
            if (pLoad == 1)
                return;

            UIMineModel.mInstance.APIMessagesNums(nums =>
            {
                if (Image_MsgRedPoint != null)
                    Image_MsgRedPoint.SetActive(nums);
            });
        }

        private void OnServerMsg(string obj)
        {
            if (obj.Equals("[cmd@%new_message"))
            {                //新消息
                if (Image_MsgRedPoint != null)
                    Image_MsgRedPoint.SetActive(true);
            }
        }



        private void InitTargetClass()
        {
            mClassToggles = new List<Toggle>();
            string tName;
            GameObject GO;
            for (int i = 2; i < 9; i++)
            {
                tName = "Toggle_" + i.ToString();
                GO = rc.Get<GameObject>(tName);
                mClassToggles.Add(GO.GetComponent<Toggle>());
                UIEventListener.Get(GO, i).onIntClick = OnTabTargetClassType;
            }
            UIEventListener.Get(rc.Get<GameObject>("BtnClass")).onClick = OnClick_RightFilter;


            for (int i = 0; i < 4; i++)
            {
                tName = "Room_" + i.ToString();
                GO = rc.Get<GameObject>(tName);
                UIEventListener.Get(GO, i-1).onIntClick = OnRoomTabTargetClassType;
            }

        }

        /// <summary>         点击  tab右边的 筛选页         </summary>
        private void OnClick_RightFilter(GameObject go)
        {
            List<int> tFilter = new List<int>() { -1, -1, -1, -1, -1, -1 };
            List<int> tFilter2 = new List<int>() { };
            var tRoomTypeShow = true;
            if (mDicPageFilter.ContainsKey(mCurSelectTab))
            {
                tFilter = mDicPageFilter[mCurSelectTab]; // new List<int>() { -1, -1, -1, -1, -1, -1 };
            }

            

            if (mCurSelectTab == 2)
            {
                Log.Debug("我的牌局 没筛选");
                return;
            }
            else if (mCurSelectTab == 3)//牌局类型,不需此条件，传值：-1 <br/>61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
            {
                tFilter[0] = 61;
                tRoomTypeShow = false;
            }
            else if (mCurSelectTab == 4)
            {
                tFilter[0] = 91;
                tRoomTypeShow = false;
            }
            else if (mCurSelectTab == 5)
            {
                tFilter[0] = 31;
                tRoomTypeShow = false;
            }
            else if (mCurSelectTab == 6)
            {
                tFilter[0] = 41;
                tRoomTypeShow = false;
            }
            else if (mCurSelectTab == 8)
            {
                tFilter[0] = 21;
                tRoomTypeShow = false;
            }

            if (mDicPageFilter2.ContainsKey(tFilter[0]))
            {
                tFilter2 = mDicPageFilter2[tFilter[0]];
            }

            UIComponent.Instance.ShowNoAnimation(UIType.UIMatch_Filter,
                                                 new UIMatch_FilterComponent.MatchFilterData()
                                                 {
                                                     successData = OnGetFilterValue,
                                                     mFilterOld = tFilter,
                                                     gameType = tFilter[0],
                                                     mDict = mBindDtoDict
                                                 });
        }

        /// <summary>
        /// 1大厅 2我的牌局 3德州 4奥马哈
        /// </summary>
        private int mCurSelectTab = 3;

        //0小1中2大
        private int mCurRoomSelectTab = 0;

        void OnShowClickBottom(int index)
        {
            mCurSelectTab = index;
            if (index == 2)
            {
                transRoomList.gameObject.SetActive(true);
                transMatchList.gameObject.SetActive(false);
                ListRooms.gameObject.SetActive(false);

                UIMatchModel.mInstance.APIMyRoomList(pDto =>
                {
                    if (pDto.status == 0)
                    {
                        mLoadingTipStatus = LoadingTipStatus.None;
                        mPaiDtos = pDto.data ?? new List<MyPaiDto>();
                        mPaiDtos.Add(new MyPaiDto());
                        mLoopListView.SetListItemCount(mPaiDtos.Count + 1, false);
                        mLoopListView.RefreshAllShownItem();
                    }
                    else
                    {
                        UIComponent.Instance.Toast(pDto.status);
                    }
                });
            }
            else if (index == 7)//"锦标赛"
            {
                transRoomList.gameObject.SetActive(false);
                transMatchList.gameObject.SetActive(true);

                ObtainMTTListData();
            }
            else
            {//非锦标赛
                transRoomList.gameObject.SetActive(true);
                transMatchList.gameObject.SetActive(false);
                ListRooms.gameObject.SetActive(true);

                List<int> tFilter = new List<int>() { -1, -1, -1, -1, -1, -1 };
                if (mDicPageFilter.ContainsKey(mCurSelectTab))
                {
                    tFilter = mDicPageFilter[mCurSelectTab];
                }

                if (mCurSelectTab == 3)//61 = 德州   91 = 奥马哈     51 = 大菠萝      41 = 必下场  31 = AOF   81 = SNG     71 = MTT
                    tFilter[0] = 61;
                else if (mCurSelectTab == 4)
                    tFilter[0] = 91;
                else if (mCurSelectTab == 5)
                    tFilter[0] = 31;
                else if (mCurSelectTab == 6)
                    tFilter[0] = 41;
                else if (mCurSelectTab == 8)
                    tFilter[0] = 21;

                List<int> tFilter2 = new List<int>() { };
                if (mDicPageFilter2.ContainsKey(tFilter[0]))
                {
                    tFilter2 = mDicPageFilter2[tFilter[0]];
                }

                if (index == 1)//"大厅"
                {
                    SetFilterImage_Hall(tFilter);
                }
                else//非大厅 可筛选的
                {
                    SetFilterImage_Other(tFilter);
                }

                if (tFilter2.Count == 0)//没有筛选
                {
                    //选小
                    var GO = rc.Get<GameObject>("Room_0");
                    OnRoomTabTargetClassType(GO, -1);
                    return;
                }
                LoadRefreshAPI(tFilter, tFilter2);
            }
        }



        /// <summary>
        /// 61 = 德州   91 = 奥马哈     51 = 大菠萝      41 = 必下场  31 = AOF   81 = SNG     71 = MTT
        /// </summary> 
        void LoadRefreshAPI(List<int> tFilter, List<int> tFilter2)
        {
            UIMatchModel.mInstance.APIHallRoomListFilter(0, 0, pDto =>
            {
                if (pDto.status == 0)
                {
                    listSize = int.Parse(pDto.msg);
                    mHallDtos = pDto.data ?? new List<HallDto>();
                    SortHallDtos();
                    mHallDtos.Add(new HallDto());
                    mLoopListView.SetListItemCount(mHallDtos.Count + 1, true);
                    mLoopListView.RefreshAllShownItem();
                }
                else
                {
                    listSize = 0;
                    UIComponent.Instance.Toast(pDto.status);
                }
            }, tFilter, tFilter2, null);
        }
        //大中小筛选
        private void OnRoomTabTargetClassType(GameObject go, int index)
        {

            mCurRoomSelectTab = index;

            if (go.GetComponent<Toggle>().isOn == false)
            {
                go.GetComponent<Toggle>().isOn = true;
            }

            List<int> tFilter = new List<int>() { -1, -1, -1, -1, -1, -1 };
            if (mCurSelectTab == 3)//61 = 德州   91 = 奥马哈     51 = 大菠萝      41 = 必下场  31 = AOF   81 = SNG     71 = MTT
                tFilter[0] = 61;
            else if (mCurSelectTab == 4)
                tFilter[0] = 91;
            else if (mCurSelectTab == 5)
                tFilter[0] = 31;
            else if (mCurSelectTab == 6)
                tFilter[0] = 41;
            else if (mCurSelectTab == 8)
                tFilter[0] = 21;

            List<int> tFilter2 = new List<int>();
            if (index == -1)
            {
                for (int k = 0; k < 3; k++)
                {
                    var plist = onGetBlindByGameAndType(tFilter[0], k);
                    tFilter2.AddRange(plist);
                }
            }
            else
            {
                tFilter2 = onGetBlindByGameAndType(tFilter[0], index);
            }

            LoadRefreshAPI(tFilter, tFilter2);

        }

        private List<int> onGetBlindByGameAndType(int gameId, int roomType)
        {
            List<int> list = new List<int>();

            Dictionary<int, List<ETHotfix.WEB_room_blindData.GameBlindElement>> mTypeDict;
            mBindDtoDict.TryGetValue(gameId, out mTypeDict);
            if (mTypeDict == null)
            {
                return list;
            }
           
            List<ETHotfix.WEB_room_blindData.GameBlindElement> mlist;
            mTypeDict.TryGetValue(roomType, out mlist);
            if (mlist == null)
            {
                return list;
            }

            foreach (var data in mlist)
            {
                string valueStr = data.blindnote;
                int minValue = 0;
                if (data.type == 21)
                {
                    minValue = int.Parse(valueStr);
                }
                else
                {
                    minValue = int.Parse(valueStr.Split('/')[0]);
                }
                list.Add(minValue);
            }
            return list;

        }

        /// <summary>     点击    头部Tab切换         </summary>
        private void OnTabTargetClassType(GameObject go, int index)
        {
            if (mCurSelectTab == index)
            {
                return;
            }

            mCurSelectTab = index;
            if (index == 2)//"我的牌局"
            {
                transRoomList.gameObject.SetActive(true);
                transMatchList.gameObject.SetActive(false);
                ListRooms.gameObject.SetActive(false);

                ImageBtnClass.sprite = mDicStrSprite["FilterSelectNon"];
                UIMatchModel.mInstance.APIMyRoomList(pDto =>
                {
                    if (pDto.status == 0)
                    {
                        mPaiDtos = pDto.data ?? new List<MyPaiDto>();
                        mPaiDtos.Add(new MyPaiDto());
                        mLoopListView.SetListItemCount(mPaiDtos.Count + 1, false);
                        mLoopListView.RefreshAllShownItem();
                    }
                    else
                    {
                        UIComponent.Instance.Toast(pDto.status);
                    }
                });
            }
            else if (index == 7)//"锦标赛"
            {
                transRoomList.gameObject.SetActive(false);
                transMatchList.gameObject.SetActive(true);
                ObtainMTTListData();
            }
            //else if (index == 8)//"短牌"
            //{
            //    transRoomList.gameObject.SetActive(true);
            //    transMatchList.gameObject.SetActive(false);
            //    ListRooms.gameObject.SetActive(true);
            //    List<int> tFilter = new List<int>() { -1, -1, -1, -1, -1, -1 };
            //    tFilter[0] = 21;
            //    List<int> tFilter2 = new List<int>() { };
            //    LoadRefreshAPI(tFilter, tFilter2);
            //}
            else
            {//非锦标赛
                transRoomList.gameObject.SetActive(true);
                transMatchList.gameObject.SetActive(false);
                ListRooms.gameObject.SetActive(true);
                List<int> tFilter = new List<int>() { -1, -1, -1, -1, -1, -1 };
                if (mDicPageFilter.ContainsKey(mCurSelectTab))
                {
                    tFilter = mDicPageFilter[mCurSelectTab];
                }

                if (mCurSelectTab == 3)//61 = 德州   91 = 奥马哈     51 = 大菠萝      41 = 必下场  31 = AOF   81 = SNG     71 = MTT
                    tFilter[0] = 61;
                else if (mCurSelectTab == 4)
                    tFilter[0] = 91;
                else if (mCurSelectTab == 5)
                    tFilter[0] = 31;
                else if (mCurSelectTab == 6)
                    tFilter[0] = 41;
                else if (mCurSelectTab == 8)
                    tFilter[0] = 21;

                List<int> tFilter2 = new List<int>() { };
                if (mDicPageFilter2.ContainsKey(tFilter[0]))
                {
                    tFilter2 = mDicPageFilter2[tFilter[0]];
                }

                if (index == 1)//"大厅"
                {
                    SetFilterImage_Hall(tFilter);
                }
                else//非大厅 可筛选的
                {
                    SetFilterImage_Other(tFilter);
                }

                if (tFilter2.Count == 0)//没有筛选
                {
                    //选小
                    var GO = rc.Get<GameObject>("Room_0");
                    OnRoomTabTargetClassType(GO, -1);
                    return;
                }

                LoadRefreshAPI(tFilter, tFilter2);
            }
        }


        public override void Dispose()
        {
            UIMineModel.mInstance.CancelShowUIMine -= RefreshRedPoint;
            ETModel.Game.Hotfix.OnServerMes -= OnServerMsg;
            if (IsDisposed)
            {
                return;
            }

            removeHandler();

            base.Dispose();
        }

        /// <summary>初次加载数据</summary>
        void InitRoomListData()
        {
            mLoopListView.InitListView(1, OnGetItemByIndex);

            //UIMatchModel.mInstance.APIHallRoomListFilter(0, 0, pDto =>
            //{
            //    mHallDtos = pDto.data ?? new List<HallDto>();
            //    mHallDtos.Add(new HallDto()); //追加一个空数据
            //    mLoopListView.InitListView(mHallDtos.Count + 1, OnGetItemByIndex);
            //}, new List<int>() { -1, -1, -1, -1, -1, -1, -1 }, new List<int>() { }, null);

            transMatchList.gameObject.SetActive(false);
        }

        #region 列表的Item显示

        HallDto SetPaiDto_2_HallDto(MyPaiDto pDto)
        {
            HallDto tHallDto = new HallDto();
            tHallDto.name = pDto.name;
            tHallDto.state = pDto.state;
            tHallDto.qianzhu = pDto.qianzhu;
            tHallDto.slmz = pDto.slmz;
            tHallDto.htron = pDto.htron;
            tHallDto.stdn = pDto.stdn;
            tHallDto.rmsec = pDto.rmsec;
            tHallDto.omamod = pDto.omamod;
            tHallDto.dlmod = pDto.dlmod;
            tHallDto.rmid = pDto.rmid;
            tHallDto.logo = pDto.logo;
            tHallDto.jkon = pDto.jkon;
            tHallDto.isron = pDto.isron;
            tHallDto.jpon = pDto.jpon;
            tHallDto.rtype = pDto.rtype;
            tHallDto.lcrdt = pDto.lcrdt;
            tHallDto.bpmod = pDto.bpmod;
            tHallDto.rpu = pDto.rpu;
            tHallDto.rpath = pDto.rpath;
            return tHallDto;
        }

        Vector3 mItemStartV3 = Vector3.zero;

        private void SetItemOtherData(GameObject go, HallDto pDto)
        {
            if (go == null || pDto == null) return;
            go.transform.Find("Image_People/Text_People").GetComponent<Text>().text = pDto.stdn.ToString() + "/" + pDto.rpu.ToString(); //人数

            string str1 = StringHelper.ShowGold(pDto.slmz, false);
            string str2 = StringHelper.ShowGold(pDto.slmz * 2, false);
            string str3 = StringHelper.ShowGold(pDto.qianzhu);
            string zhuatou = "";
            if (pDto.zhuatou == 1)
            {
                zhuatou = StringHelper.ShowGold(pDto.slmz * 4);
            }
            if (pDto.rpath == RoomPath.DP.GetHashCode() || pDto.rpath == RoomPath.DPAof.GetHashCode())
            {
                go.transform.Find("Image_Blind/Text_Blind").GetComponent<Text>().text = StringHelper.ShowGold(pDto.qianzhu);

            }
            else
            {
                go.transform.Find("Image_Blind/Text_Blind").GetComponent<Text>().text = StringHelper.ShowBlinds(str1, str2, str3, zhuatou);
            }

            go.transform.Find("Image_Time/Text_Time").GetComponent<Text>().text =
                    pDto.rmsec == 0 ? LanguageManager.mInstance.GetLanguageForKeyMoreValue("UIMatch_itemState", 0) : TimeHelper.ShowRemainingTimer(pDto.rmsec * 1000, false) + LanguageManager.mInstance.GetLanguageForKey("UIMatch_itemJiu");
            // WebImageHelper.SetHeadImage(go.transform.Find("Item_Icon").GetComponent<RawImage>(), pDto.logo);
            var tStatu = go.transform.Find("Text_Status").GetComponent<Text>();
            tStatu.text = GetStatuTxt(pDto.state); //0等待中 1进行中 2MTT延迟报名中 3MTT报名截止
            tStatu.color = GetStatuColor(pDto.state);
            go.transform.Find("Image_Status").GetComponent<Image>().sprite = GetStatuSprite(pDto.state);

            //算位置
            var tBlindWidth = go.transform.Find("Image_Blind/Text_Blind").GetComponent<Text>().preferredWidth + 60;
            var tPeopleWidth = tBlindWidth + go.transform.Find("Image_People/Text_People").GetComponent<Text>().preferredWidth + 60;
            if (mItemStartV3 == Vector3.zero)
                mItemStartV3 = go.transform.Find("Image_People").localPosition;
            go.transform.Find("Image_People").localPosition = new Vector3(mItemStartV3.x + tBlindWidth, mItemStartV3.y, mItemStartV3.z);
            go.transform.Find("Image_Time").localPosition = new Vector3(mItemStartV3.x + tPeopleWidth, mItemStartV3.y, mItemStartV3.z);

            go.transform.Find("Image_Type2/Text_Type2").GetComponent<Text>().text = UIMatchModel.mInstance.GetRoomPathStr(pDto.rtype);

            //头像
            go.transform.Find("Item_Icon").GetComponent<Image>().sprite = GetIconSprite(pDto.rtype);

            //smallImages
            for (int i = 1; i < 7; i++)
            {
                go.transform.Find("SmallGo/SmallImage" + i.ToString()).gameObject.SetActive(false);
            }

            var tLangs = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIMatch_Script102");//保险^德州^血战^血进^同时^顺序^赖子^血战
            var count1 = SetSmall(go, "SmallGo/SmallImage1", pDto.isron == 1 ? tLangs[0] : "", mDicStrSprite["color_baoxian"], pDto.isron == 1);//"保险" : "", mDicStrSprite["color_baoxian"], pDto.isron == 1);
            var count2 = SetSmall(go, "SmallGo/SmallImage2", pDto.jpon == 1 ? "JP" : "", mDicStrSprite["color_jp"], pDto.jpon == 1);
            var count3 = 0;
            if (pDto.rtype == RoomPath.Normal.GetHashCode()) //德州局
            {
            }
            else if (pDto.rtype == RoomPath.Normal.GetHashCode()) //必下场
            {
            }
            else if (pDto.rtype == RoomPath.PAP.GetHashCode()) //大菠萝
            {
                SetSmall(go, "SmallGo/SmallImage3", pDto.bpmod == 1 ? tLangs[1] : pDto.bpmod == 2 ? tLangs[2] : tLangs[3], mDicStrSprite["color_xuezhan"]);// "德州" : pDto.bpmod == 2 ? "血战" : "血进",
                SetSmall(go, "SmallGo/SmallImage4", pDto.dlmod == 0 ? tLangs[4] : tLangs[5], mDicStrSprite["color_tongshi"]);//"同时" : "顺序",
                SetSmall(go, "SmallGo/SmallImage5", pDto.jkon == 1 ? tLangs[6] : "", mDicStrSprite["color_laizi"], pDto.jkon == 1);// "赖子" 
            }
            else if (pDto.rtype == RoomPath.Omaha.GetHashCode()) //奥马哈
            {
                count3 = SetSmall(go, "SmallGo/SmallImage3", pDto.omamod == 1 ? tLangs[7] : "", mDicStrSprite["color_xuezhan"], pDto.omamod == 1);// "血战"
            }

            //比下场和AOF显示奥马哈标签
            if (pDto.rpath == RoomPath.OmahaThanOff.GetHashCode() || pDto.rpath == RoomPath.OmahaAof.GetHashCode())
            {
                var str = UIMatchModel.mInstance.GetRoomPathStr(RoomPath.Omaha.GetHashCode());
                SetSmall(go, "SmallGo/SmallImage6", str, mDicStrSprite["color_aomaha"], true);
            }

            var tItemTitle = go.transform.Find("Text_Title").GetComponent<Text>();
            if (count1 + count2 + count3 < 3)
            {
                tItemTitle.text = pDto.name;
            }
            else
            {
                UIMatchModel.mInstance.SetLargeTextContent(tItemTitle, pDto.name, 560);
            }
            tItemTitle.GetComponent<RectTransform>().sizeDelta = new Vector2(tItemTitle.preferredWidth, tItemTitle.preferredHeight);
            go.transform.Find("SmallGo").GetComponent<RectTransform>().localPosition = new Vector3(278 + tItemTitle.preferredWidth, -65, 0);
        }

        int SetSmall(GameObject go, string pathImg, string txt, Sprite sprite, bool show = true)
        {
            int i = 0;
            go.transform.Find(pathImg).gameObject.SetActive(show);
            if (show)
            {
                go.transform.Find(pathImg + "/Text_Small").GetComponent<Text>().text = txt;
                go.transform.Find(pathImg).GetComponent<Image>().sprite = sprite;
                i = 1;
            }
            return i;
        }


        private Sprite GetStatuSprite(int pStatus)
        {
            if (pStatus == 4)
                return mDicStrSprite["icon_list_waiting"];
            else if (pStatus == 5)
                return mDicStrSprite["icon_list_playing"];
            else if (pStatus == 3)
                return mDicStrSprite["icon_list_waiting"];
            return mDicStrSprite["icon_list_waiting"];
        }

        private Sprite GetIconSprite(int roomPath)
        {
            //int mTmpCurLanguage = LanguageManager.mInstance.mCurLanguage;
            //if (roomPath == 61 || roomPath == 62 || roomPath == 63)
            //{
            //    string name = string.Format("game_icon_61_{0}", mTmpCurLanguage);
            //    return mDicStrSprite[name];
            //}
            //else if (roomPath == 91 || roomPath == 92 || roomPath == 93)
            //{
            //    string name = string.Format("game_icon_91_{0}", mTmpCurLanguage);
            //    return mDicStrSprite[name];
            //}
            return mDicStrSprite["img_default_head"];
        }

        string GetStatuTxt(int pStatus)
        {
            var t = LanguageManager.mInstance.GetLanguageForKeyMoreValue("UIMatch_itemState", (pStatus - 3));
            return t;
            //if (pStatus == 4)
            //    return "进行中";
            //else if (pStatus == 5)
            //    return "游戏中";
            //else if (pStatus == 3)
            //    return "等待中";
            //return "";
        }

        Color GetStatuColor(int pStatus)
        {
            if (pStatus == 4)
                return new Color32(61, 179, 102, 255);
            else if (pStatus == 5)
                return new Color32(61, 179, 102, 255);
            else if (pStatus == 3)
                return new Color32(233, 191, 128, 255); //黄

            return new Color32(233, 191, 128, 255);
        }

        #endregion

        #region SupperScrollView 底部的

        private void InitBottomEle()
        {
            mLoopListView = rc.Get<GameObject>("ListScrollView").GetComponent<LoopListView2>();
            mLoopListView.mOnEndDragAction = OnEndDrag;
            mLoopListView.mOnDownMoreDragAction = OnDownMoreDragAction;
            mLoopListView.mOnUpRefreshDragAction = OnUpRefreshDragAction;
        }

        /// <summary> 其他房间   -->大厅 </summary>
        private List<HallDto> mHallDtos;

        private List<MyPaiDto> mPaiDtos;

        public List<BlindDto> mBindDtos;

        public Dictionary<int, Dictionary<int, List<BlindDto>>> mBindDtoDict = new Dictionary<int, Dictionary<int, List<BlindDto>>>();

        /// <summary>        /// -1上面的菊花     1下面的菊花        /// </summary>
        int mDirDrag = -1;

        private LoopListView2 mLoopListView;
        private LoadingTipStatus mLoadingTipStatus = LoadingTipStatus.None;
        float mLoadingTipItemHeight = 100;

        //牌局排序
        private void SortHallDtos()
        {
            mHallDtos.Sort((x, y) => { return x.rpath.CompareTo(y.rpath); });
        }

        /// <summary>
        /// pDir=0刷新(上面的菊花)   1下面的菊花
        /// </summary>
        private void DragRequestData(LoopListViewItem2 item, int pDir, long pLastTime) //, string time, string dir)
        {
            if (mCurSelectTab == 2) //无
            {
            }
            else
            {
                SetDargData(mCurSelectTab, pDir, pLastTime, item);
            }
        }

        void SetDargData(int pTab, int pDir, long pLastTime, LoopListViewItem2 item)
        {
            List<int> tFilter = new List<int>() { -1, -1, -1, -1, -1, -1 };
            if (mDicPageFilter.ContainsKey(mCurSelectTab))
            {
                tFilter = mDicPageFilter[mCurSelectTab];
            }

            if (mCurSelectTab == 3)//牌局类型,不需此条件，传值：-1 <br/>61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
                tFilter[0] = 61;
            else if (mCurSelectTab == 4)
                tFilter[0] = 91;
            else if (mCurSelectTab == 5)
                tFilter[0] = 31;
            else if (mCurSelectTab == 6)
                tFilter[0] = 41;
            else if (mCurSelectTab == 8)
                tFilter[0] = 21;


            List<int> tFilter2 = new List<int>() { };
            if (mDicPageFilter2.ContainsKey(tFilter[0]))
            {
                tFilter2 = mDicPageFilter2[tFilter[0]];
            }

            if (tFilter2.Count == 0)
            {
                var name = "Room_" + (mCurRoomSelectTab + 1).ToString();
                var GO = rc.Get<GameObject>(name);
                if (GO.GetComponent<Toggle>().isOn == false)
                {
                    GO.GetComponent<Toggle>().isOn = true;
                }

                if (mCurRoomSelectTab == -1)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        var plist = onGetBlindByGameAndType(tFilter[0], k);
                        tFilter2.AddRange(plist);
                    }
                }
                else
                {
                    tFilter2 = onGetBlindByGameAndType(tFilter[0], mCurRoomSelectTab);
                }
                
            }

            UIMatchModel.mInstance.APIHallRoomListFilter(pDir, listSize, pDto =>
            {
                if (pDto.status == 0)
                {
                    listSize = int.Parse(pDto.msg);

                    if (pDto.data == null || pDto.data.Count <= 0)
                    {
                        if (pDir == 0)
                        {
                            mHallDtos = new List<HallDto>() { new HallDto() };
                            OnDataSourceLoadMoreFinished(false);
                            return;
                        }
                        else if (pDir == 1)
                        {
                            OnDataSourceLoadMoreFinished(false);
                            return;
                        }
                    }

                    if (pDir == 1)
                    {
                        //追加
                        mHallDtos.RemoveAt(mHallDtos.Count - 1);
                        mHallDtos.AddRange(pDto.data);
                        SortHallDtos();
                        mHallDtos.Add(new HallDto());
                        OnDataSourceLoadMoreFinished(pDto.data.Count > 0);
                    }
                    else if (pDir == 0)
                    {
                        //刷新
                        mHallDtos = pDto.data ?? new List<HallDto>();
                        SortHallDtos();
                        mHallDtos.Add(new HallDto());
                        OnDataSourceLoadMoreFinished(true);
                    }

                    if (mCurSelectTab != 2)
                        UpdateLoadingTip(item);
                }
                else
                {
                    listSize = 0;
                    UIComponent.Instance.Toast(pDto.status);
                    OnDataSourceLoadMoreFinished(true);
                    return;
                }
            }, tFilter, tFilter2, delegate { OnDataSourceLoadMoreFinished(true); });

        }


        /// <summary>         上面的菊花         </summary>
        private void OnUpRefreshDragAction()
        {
            mDirDrag = -1;
            if (mLoopListView.ShownItemCount == 0)
            {
                return;
            }

            if (mLoadingTipStatus != LoadingTipStatus.None && mLoadingTipStatus != LoadingTipStatus.WaitRelease)
            {
                return;
            }

            LoopListViewItem2 item = mLoopListView.GetShownItemByItemIndex(0);
            if (item == null)
            {
                return;
            }

            ScrollRect sr = mLoopListView.ScrollRect;
            Vector3 pos = sr.content.anchoredPosition3D;
            if (pos.y < -mLoadingTipItemHeight)
            {
                if (mLoadingTipStatus != LoadingTipStatus.None)
                {
                    return;
                }

                mLoadingTipStatus = LoadingTipStatus.WaitRelease;
                if (mCurSelectTab != 2)
                    UpdateLoadingTip(item);
                item.CachedRectTransform.anchoredPosition3D = new Vector3(0, mLoadingTipItemHeight, 0);
            }
            else
            {
                if (mLoadingTipStatus != LoadingTipStatus.WaitRelease)
                {
                    return;
                }

                mLoadingTipStatus = LoadingTipStatus.None;
                if (mCurSelectTab != 2)
                    UpdateLoadingTip(item);
                item.CachedRectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
            }
        }

        /// <summary>         下面的菊花         </summary>
        private void OnDownMoreDragAction()
        {
            if (mCurSelectTab == 2) return;
            mDirDrag = 1;
            if (mLoopListView.ShownItemCount == 0)
            {
                return;
            }

            if (mLoadingTipStatus != LoadingTipStatus.None && mLoadingTipStatus != LoadingTipStatus.WaitRelease)
            {
                return;
            }

            LoopListViewItem2 item = mLoopListView.GetShownItemByItemIndex(mHallDtos.Count);
            if (item == null)
            {
                return;
            }

            LoopListViewItem2 item1 = mLoopListView.GetShownItemByItemIndex(mHallDtos.Count - 1);
            if (item1 == null)
            {
                return;
            }

            float y = mLoopListView.GetItemCornerPosInViewPort(item1, ItemCornerEnum.LeftBottom).y;
            if (y + mLoopListView.ViewPortSize >= mLoadingTipItemHeight)
            {
                if (mLoadingTipStatus != LoadingTipStatus.None)
                {
                    return;
                }

                mLoadingTipStatus = LoadingTipStatus.WaitRelease;
                if (mCurSelectTab != 2)
                    UpdateLoadingTip(item);
            }
            else
            {
                if (mLoadingTipStatus != LoadingTipStatus.WaitRelease)
                {
                    return;
                }

                mLoadingTipStatus = LoadingTipStatus.None;
                if (mCurSelectTab != 2)
                    UpdateLoadingTip(item);
            }
        }


        LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
        {
            if (index < 0)
            {
                return null;
            }

            LoopListViewItem2 item = null;
            if (mCurSelectTab == 2)
            {
                if (index == 0)
                {
                    mLoadingTipStatus = LoadingTipStatus.None;
                    item = listView.NewListViewItem("ItemPrefab0");
                    UpdateLoadingTip(item);
                    return item;
                }

                if (mPaiDtos == null) return null;
                if (index == mPaiDtos.Count)
                {
                    mLoadingTipStatus = LoadingTipStatus.None;
                    item = listView.NewListViewItem("ItemPrefab0");
                    UpdateLoadingTip(item);
                    return item;
                }

                if (index - 1 > mPaiDtos.Count) return null;
                var tDto = mPaiDtos[index - 1];
                if (tDto == null) return null;

                item = listView.NewListViewItem("ItemInfo");
                if (item.IsInitHandlerCalled == false)
                {
                    item.IsInitHandlerCalled = true;
                }

                var go = item.gameObject;
                SetItemOtherData(go, SetPaiDto_2_HallDto(tDto));
                UIEventListener.Get(go).onClick = (p) =>
                {
                    if (isEnterRoom)
                        return;
                    isEnterRoom = true;
                    EnterRoomAPI(tDto.rpath, tDto.rmid);
                };
                return item;
            }
            else
            {
                if (index == 0)
                {
                    mLoadingTipStatus = LoadingTipStatus.None;
                    item = listView.NewListViewItem("ItemPrefab0");
                    UpdateLoadingTip(item);
                    return item;
                }

                if (mHallDtos == null) return null;
                if (index == mHallDtos.Count)
                {
                    item = listView.NewListViewItem("ItemPrefab0");
                    UpdateLoadingTip(item);
                    return item;
                }

                if (index - 1 > mHallDtos.Count) return null;
                var tDto = mHallDtos[index - 1];
                if (tDto == null) return null;

                item = listView.NewListViewItem("ItemInfo");
                if (item.IsInitHandlerCalled == false)
                {
                    item.IsInitHandlerCalled = true;
                }

                var go = item.gameObject;
                SetItemOtherData(go, tDto);
                UIEventListener.Get(go).onClick = (p) =>
                {
                    if (isEnterRoom)
                        return;
                    isEnterRoom = true;
                    EnterRoomAPI(tDto.rpath, tDto.rmid);
                };
                return item;
            }
            return null;
        }

        void UpdateLoadingTip(LoopListViewItem2 item)
        {
            if (item == null) //|| mCurSelectTab == 2)
            {
                return;
            }

            ListItem0 itemScript0 = item.GetComponent<ListItem0>();
            if (itemScript0 == null)
            {
                return;
            }

            if (mLoadingTipStatus == LoadingTipStatus.None)
            {
                itemScript0.mRoot.SetActive(false);
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
            }
            else if (mLoadingTipStatus == LoadingTipStatus.WaitRelease)
            {
                itemScript0.mRoot.SetActive(true);
                itemScript0.mText.text = LanguageManager.mInstance.GetLanguageForKey("SuperView1");// "放开加载更多 ...";
                itemScript0.mArrow.SetActive(true);
                itemScript0.mWaitingIcon.SetActive(false);
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mLoadingTipItemHeight);
            }
            else if (mLoadingTipStatus == LoadingTipStatus.WaitLoad)
            {
                itemScript0.mRoot.SetActive(true);
                itemScript0.mArrow.SetActive(false);
                itemScript0.mWaitingIcon.SetActive(true);
                itemScript0.mText.text = LanguageManager.mInstance.GetLanguageForKey("SuperView2");// "加载中 ...";
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mLoadingTipItemHeight);
            }
            else if (mLoadingTipStatus == LoadingTipStatus.NoMoreData)
            {
                itemScript0.mRoot.SetActive(true);
                itemScript0.mArrow.SetActive(false);
                itemScript0.mWaitingIcon.SetActive(false);
                itemScript0.mText.text = LanguageManager.mInstance.GetLanguageForKey("SuperView3");//  "已经是最后一条啦";
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mLoadingTipItemHeight);
            }
        }

        void OnEndDrag()
        {
            if (mCurSelectTab == 2) return;
            if (mDirDrag == -1)
            {
                if (mLoopListView.ShownItemCount == 0)
                {
                    return;
                }

                if (mLoadingTipStatus != LoadingTipStatus.None && mLoadingTipStatus != LoadingTipStatus.WaitRelease)
                {
                    return;
                }

                LoopListViewItem2 item = mLoopListView.GetShownItemByItemIndex(0);
                if (item == null)
                {
                    return;
                }

                mLoopListView.OnItemSizeChanged(item.ItemIndex);
                if (mLoadingTipStatus != LoadingTipStatus.WaitRelease)
                {
                    return;
                }

                mLoadingTipStatus = LoadingTipStatus.WaitLoad;
                UpdateLoadingTip(item);
                DragRequestData(item, 0, 0);
            }
            else if (mDirDrag == 1)
            {
                if (mLoopListView.ShownItemCount == 0)
                {
                    return;
                }

                if (mLoadingTipStatus != LoadingTipStatus.None && mLoadingTipStatus != LoadingTipStatus.WaitRelease)
                {
                    return;
                }

                LoopListViewItem2 item = mLoopListView.GetShownItemByItemIndex(mHallDtos.Count);
                if (item == null)
                {
                    return;
                }

                mLoopListView.OnItemSizeChanged(item.ItemIndex);
                if (mLoadingTipStatus != LoadingTipStatus.WaitRelease)
                {
                    return;
                }

                mLoadingTipStatus = LoadingTipStatus.WaitLoad;
                UpdateLoadingTip(item);
                var tIndex = 0;
                if (mHallDtos.Count >= 2)
                    tIndex = mHallDtos.Count - 2;
                else
                    tIndex = mHallDtos.Count - 1;

                DragRequestData(item, 1, mHallDtos[tIndex].lcrdt);
            }
        }

        /// <summary>  刷新用true,  追加使用count>0   </summary>
        async void OnDataSourceLoadMoreFinished(bool pLength)
        {
            if (mLoopListView.ShownItemCount == 0)
            {
                return;
            }

            if (mLoadingTipStatus == LoadingTipStatus.WaitLoad)
            {
                await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(200);
                mLoadingTipStatus = pLength ? LoadingTipStatus.None : LoadingTipStatus.NoMoreData;
                mLoopListView.SetListItemCount(mHallDtos.Count + 1, false);
                mLoopListView.RefreshAllShownItem();
            }
        }

        #endregion

        #region 加入房间

        /// <summary> 加入牌局 </summary>
        void EnterRoomAPI(int pRoomPath, int pRoomId)
        {
            if (pRoomPath == RoomPath.Normal.GetHashCode() || pRoomPath == RoomPath.NormalThanOff.GetHashCode() || pRoomPath == RoomPath.NormalAof.GetHashCode())
            {
                UIMatchModel.mInstance.APIEnterNormalRoom(pRoomPath, pRoomId, pDto =>
                {
                    GameCache.Instance.roomName = pDto.name;
                    GameCache.Instance.roomIP = pDto.gmip;
                    GameCache.Instance.roomPort = int.Parse(pDto.gmprt);
                    GameCache.Instance.room_path = pDto.rpath;
                    GameCache.Instance.rtype = pDto.rtype;
                    GameCache.Instance.room_id = pDto.rmid;
                    GameCache.Instance.client_ip = pDto.cliip;
                    GameCache.Instance.carry_small = pDto.incp;
                    GameCache.Instance.straddle = pDto.sdlon;
                    GameCache.Instance.insurance = pDto.isron;
                    GameCache.Instance.muck_switch = pDto.muckon;
                    GameCache.Instance.jackPot_fund = pDto.jpfnd;
                    GameCache.Instance.jackPot_on = pDto.jpon;
                    GameCache.Instance.jackPot_id = pDto.jpid;
                    GameCache.Instance.shortest_time = pDto.gmxt;
                    AsyncEnterRoom();
                }, () =>
                {
                    isEnterRoom = false;
                });
            }
            else if (pRoomPath == RoomPath.DP.GetHashCode() || pRoomPath == RoomPath.DPAof.GetHashCode())
            {
                    UIMatchModel.mInstance.APIEnterShortRoom(pRoomPath, pRoomId, pDto =>
                    {
                        GameCache.Instance.roomName = pDto.name;
                        GameCache.Instance.roomIP = pDto.gmip;
                        GameCache.Instance.roomPort = int.Parse(pDto.gmprt);
                        GameCache.Instance.room_path = pDto.rpath;
                        GameCache.Instance.rtype = pDto.rtype;
                        GameCache.Instance.room_id = pDto.rmid;
                        GameCache.Instance.client_ip = pDto.cliip;
                        GameCache.Instance.carry_small = pDto.incp;
                        GameCache.Instance.straddle = pDto.sdlon;
                        GameCache.Instance.insurance = pDto.isron;
                        GameCache.Instance.muck_switch = pDto.muckon;
                        GameCache.Instance.jackPot_fund = pDto.jpfnd;
                        GameCache.Instance.jackPot_on = pDto.jpon;
                        GameCache.Instance.jackPot_id = pDto.jpid;
                        GameCache.Instance.shortest_time = pDto.gmxt;
                        AsyncEnterRoom();
                    }, () =>
                    {
                        isEnterRoom = false;
                    });
            }
            else if (pRoomPath == RoomPath.Omaha.GetHashCode() || pRoomPath == RoomPath.OmahaThanOff.GetHashCode() || pRoomPath == RoomPath.OmahaAof.GetHashCode())
            {
                UIMatchModel.mInstance.APIEnterOmahaRoom(pRoomPath, pRoomId, pDto =>
                {
                    GameCache.Instance.roomName = pDto.name;
                    GameCache.Instance.roomIP = pDto.gmip;
                    GameCache.Instance.roomPort = int.Parse(pDto.gmprt);
                    GameCache.Instance.room_path = pDto.rpath;
                    GameCache.Instance.rtype = pDto.rtype;
                    GameCache.Instance.room_id = pDto.rmid;
                    GameCache.Instance.client_ip = pDto.cliip;
                    GameCache.Instance.carry_small = pDto.incp;
                    GameCache.Instance.straddle = pDto.sdlon;
                    GameCache.Instance.insurance = pDto.isron;
                    GameCache.Instance.muck_switch = pDto.muckon;
                    GameCache.Instance.jackPot_fund = pDto.jpfnd;
                    GameCache.Instance.jackPot_on = pDto.jpon;
                    GameCache.Instance.jackPot_id = pDto.jpid;
                    GameCache.Instance.shortest_time = pDto.gmxt;
                    AsyncEnterRoom();
                }, () =>
                {
                    isEnterRoom = false;
                });
            }
        }

        private async void AsyncEnterRoom()
        {
            UIComponent.Instance.ShowNoAnimation(UIType.UIMatch_Loading); //Loading页面.在UItexas生成就 remove掉                
            await UIComponent.Instance.ShowAsyncNoAnimation(UIType.UITexas, UIType.UIMatch);
            isEnterRoom = false;
        }

        #endregion

    }
}
