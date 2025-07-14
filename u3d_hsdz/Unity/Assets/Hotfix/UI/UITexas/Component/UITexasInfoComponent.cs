using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using xcharts;
using DtoResp = ETHotfix.WEB2_data_stat_person.ResponseData;
using DG.Tweening;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIInfoComponentSystem : AwakeSystem<UITexasInfoComponent>
    {
        public override void Awake(UITexasInfoComponent self)
        {
            self.Awake();
        }
    }

    public class UITexasInfoComponent : UIBaseComponent
    {
        public class InfoData
        {
            /// <summary>
            /// 含 randomId=num 
            /// </summary>
            public WEB2_user_public_info.Data mUserInfo;
            /// <summary>
            /// 0:没有权限，1：有踢出站起权限  
            /// </summary>
            public sbyte canKickOut;
            /// <summary>
            /// 面板的userId
            /// </summary>
            public int mUserId;
        }


        private ReferenceCollector rc;
        private Button buttonClose;
        private RawImage rawimageHead;
        private Image imageBoy;
        private Image imageGirl;
        private Text textNick;
        private Text textUserId;
        private Button buttonSign;
        private Button buttonStandup;
        private Button buttonFourceout;
        private Button buttonComplaint;
        private RadarChart radarChart;
        private InfoData mInfoData;
        private Text textPs;
        private Transform transLockGo;
        private Text textLockTitle;
        private Text textLockSubTitle;
        private Image imageLockBean;
        private RectTransform EmojiContent;

        private Text textjoinPai;
        private Text textenterPoolWin;
        private Text texttotalHand;
        private Text textenterPoolRate;

        private Text textPAP1;
        private Text textPAP2;
        private Text textPAP3;
        private Text textPAP4;
        private Transform transPAP;
        private Transform transNormalOmaha;

        private Text textSNGMTT1;
        private Text textSNGMTT2;
        private Text textSNGMTT3;
        private Text textSNGMTT4;
        private Text textSNGMTT5;
        private Transform transSNGMTT;
        private GameObject DescGO;
        private Toggle toggleDefault;
        private Text textProblemDesc;

        int[] mRoomPaths = new int[] { 61, 91, 31, 71 };
        private int mCurRoomPath;

        private Transform radarChartScale_Tr;
        private RadarChart radarChartScale_Data;

        private List<PlayerComplaint> mCurAwakePlayers;


        /// <summary>
        /// 能否有权限破隐 1401-无权破隐 1402-已破隐（只有已破隐), 1403-可破隐(未破隐)
        /// </summary>
        private int mCurBreakStatus;

        public void Awake()
        {
            InitUI();
            SetCacheData();
        }

        private void SetCacheData()
        {
            var tPlayers = GameCache.Instance.CurGame.GetCurrentPlayers();
            mCurAwakePlayers = new List<PlayerComplaint>();
            PlayerComplaint tPC = null;
            for (int i = 0; i < tPlayers.Count; i++)
            {
                tPC = new PlayerComplaint();
                tPC.headPic = tPlayers[i].headPic;
                tPC.nick = tPlayers[i].nick;
                tPC.userID = tPlayers[i].userID;
                mCurAwakePlayers.Add(tPC);
            }
        }

        public override void OnShow(object obj)
        {
            if (null != obj && obj is InfoData)
            {
                mInfoData = obj as InfoData;
                var tDto = CacheDataManager.mInstance.GetSNSBatchRelation(mInfoData.mUserInfo.randomNum);
                if (tDto != null && string.IsNullOrEmpty(tDto.remarkName) == false)
                {
                    textNick.text = tDto.remarkName;
                    // textPs.text = "昵称:" + mInfoData.mUserInfo.nickName + "\n" + tDto.remark;
                    textPs.text = $"{CPErrorCode.LanguageDescription(20041)}{mInfoData.mUserInfo.nickName}\n{tDto.remark}";
                }
                else
                {
                    textNick.text = mInfoData.mUserInfo.nickName;
                    // textPs.text = "备注和打法标记";
                    textPs.text = CPErrorCode.LanguageDescription(10307);
                }
                // textUserId.text = "ID:" + mInfoData.mUserInfo.randomNum;
                textUserId.text = $"ID:{mInfoData.mUserInfo.randomNum}";
                buttonStandup.gameObject.SetActive(mInfoData.canKickOut != 0);
                buttonFourceout.gameObject.SetActive(mInfoData.canKickOut != 0);

                if (GameCache.Instance.CurInfoRoomPath == 0)//设置为默认页 的加载
                    LoadHttpInterface();
                else
                {
                    ClickTabToggle_Non(GameCache.Instance.CurInfoRoomPath);
                }

                WebImageHelper.SetHeadImage(rawimageHead, mInfoData.mUserInfo.head);
                imageBoy.gameObject.SetActive(mInfoData.mUserInfo.sex == 0);// 用户性别:0-男,1-女
                imageGirl.gameObject.SetActive(mInfoData.mUserInfo.sex == 1);// 用户性别:0-男,1-女

                if (GameCache.Instance.nUserId == mInfoData.mUserId)
                {
                    buttonSign.gameObject.SetActive(false);
                    textPs.gameObject.SetActive(false);
                    Image_Menu.localPosition = new Vector3(0, 135, 0);
                    Image_Menu.sizeDelta = new Vector3(1008, 1350, 0);
                    Head_list.gameObject.SetActive(false);
                    textNick.transform.localPosition = new Vector3(-32, 31, 0);
                    textUserId.transform.localPosition = new Vector3(2, -46, 0);
                }
            }
        }

        void ShowFourData(DtoResp pDto)
        {
            textjoinPai.text = pDto.data.totalGameCnt.ToString();
            textenterPoolWin.text = pDto.data.Wins.ToString() + "%";
            texttotalHand.text = pDto.data.totalHand.ToString();
            textenterPoolRate.text = pDto.data.VPIP.ToString() + "%";
        }


        void LoadHttpInterface()
        {
            APILookPersonData(mInfoData.mUserInfo.randomNum, mCurRoomPath, pDto =>
            {
                if (pDto.status == 0)
                {
                    mCurBreakStatus = pDto.data.breakStatus;
                    ShowFourData(pDto);
                    if (pDto.data.imageType == 1)
                    {
                        textProblemDesc.text = LanguageManager.Get("UITexasInfo_all");
                    }
                    else if (pDto.data.imageType == 2)
                    {
                        textProblemDesc.text = CPErrorCode.LanguageDescription(20042); // textProblemDesc.text = "仅显示近30天数据";
                    }
                    else
                    {
                        textProblemDesc.text = CPErrorCode.LanguageDescription(20043);// textProblemDesc.text = "仅显示近7天数据";
                    }
                    if (mCurBreakStatus == 1402
                    || pDto.data.roomPath == RoomPath.PAP.GetHashCode() || pDto.data.roomPath == RoomPath.SNG.GetHashCode() || pDto.data.roomPath == RoomPath.MTT.GetHashCode())
                    {
                        //已破隐
                        if (pDto.data.roomPath == mRoomPaths[0] || pDto.data.roomPath == mRoomPaths[1] || pDto.data.roomPath == mRoomPaths[2])
                        {
                            SetNormalOmahaView(pDto);
                        }
                        else if (pDto.data.roomPath == RoomPath.PAP.GetHashCode())//暂无用
                        {
                            SetPAPView(pDto);
                        }
                        else if (pDto.data.roomPath == RoomPath.SNG.GetHashCode() || pDto.data.roomPath == RoomPath.MTT.GetHashCode())
                        {
                            SetSNGMTTView(pDto);
                        }
                        transLockGo.gameObject.SetActive(false);

                    }
                    else
                    {
                        transNormalOmaha.gameObject.SetActive(true);

                        transPAP.gameObject.SetActive(false);
                        transSNGMTT.gameObject.SetActive(false);
                        if (mCurBreakStatus == 1403)
                        {
                            //可破隐，显示破隐USDT
                            imageLockBean.gameObject.SetActive(true);
                            textLockTitle.text = $"{pDto.data.breakPrice}";
                        }
                        else
                        {
                            //不可破隐
                            imageLockBean.gameObject.SetActive(false);
                            if (GameCache.Instance.vipLevel == 0)
                            {
                                textLockTitle.text = CPErrorCode.LanguageDescription(20044);                   // textLockTitle.text = "开通VIP";
                            }
                            else
                            {
                                if (mInfoData.mUserInfo.vip == 3)
                                {
                                    textLockTitle.text = CPErrorCode.LanguageDescription(10308);// textLockTitle.text = "最强隐身";
                                }
                                else
                                {
                                    textLockTitle.text = CPErrorCode.LanguageDescription(20045);    // textLockTitle.text = "升级VIP";
                                }
                            }
                        }
                        if (mInfoData.mUserInfo.vip == 0)
                        {                            // textLockSubTitle.text = "所有VIP可查看";
                            textLockSubTitle.text = CPErrorCode.LanguageDescription(20046);
                        }
                        else if (mInfoData.mUserInfo.vip == 1)
                        {                            // textLockSubTitle.text = "铂金钻石VIP可查看";
                            textLockSubTitle.text = CPErrorCode.LanguageDescription(20047);
                        }
                        else if (mInfoData.mUserInfo.vip == 2)
                        {                            // textLockSubTitle.text = "钻石VIP可查看";
                            textLockSubTitle.text = CPErrorCode.LanguageDescription(20048);
                        }
                        else if (mInfoData.mUserInfo.vip == 3)
                        {                            // textLockSubTitle.text = "所有人不可查看";
                            textLockSubTitle.text = CPErrorCode.LanguageDescription(10309);
                        }
                    }
                }
                else
                {
                    UIComponent.Instance.Toast(pDto.status);
                }
            });
        }

        private void SetSNGMTTView(DtoResp pDto)
        {
            transNormalOmaha.gameObject.SetActive(false);
            transPAP.gameObject.gameObject.SetActive(false);
            transSNGMTT.gameObject.SetActive(true);

            textSNGMTT1.text = pDto.data.playTimes.ToString();
            textSNGMTT2.text = pDto.data.winTimes.ToString();
            textSNGMTT3.text = pDto.data.firstTimes.ToString();
            textSNGMTT4.text = pDto.data.secondTimes.ToString();
            textSNGMTT5.text = pDto.data.thirdTimes.ToString();
        }

        private void SetPAPView(DtoResp pDto)
        {
            transNormalOmaha.gameObject.SetActive(false);
            transPAP.gameObject.gameObject.SetActive(true);
            transSNGMTT.gameObject.SetActive(false);

            textPAP1.text = pDto.data.papWins.ToString() + "%";
            textPAP2.text = pDto.data.fantasy.ToString() + "%";
            textPAP3.text = pDto.data.handAverage.ToString();
            textPAP4.text = pDto.data.fantasyAverage.ToString();
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
        }

        #region UI
        List<Text> mToggleTabTxts;
        List<Toggle> mToggleTabs;
        private RectTransform Image_Menu;
        private GameObject Head_list;

        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mCurRoomPath = mRoomPaths[0];

            radarChartScale_Tr = rc.Get<GameObject>("Radar_ChartScale").transform;
            radarChartScale_Data = radarChartScale_Tr.GetComponent<RadarChart>();
            radarChartScale_Tr.localScale = Vector3.zero;

            buttonClose = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            mToggleTabTxts = new List<Text>();
            mToggleTabs = new List<Toggle>();
            // var tPaths = new int[] { 61, 91, 31 , 71};//德州,奥马哈,AOF,MTT
            var tPaths = mRoomPaths;//德州,奥马哈,AOF,MTT
            string tStr = string.Empty;
            for (int i = 1; i < 5; i++)
            {
                tStr = "Toggle_Tab" + i.ToString();
                UIEventListener.Get(rc.Get<GameObject>(tStr), tPaths[i - 1]).onIntClick = ClickSelectTabToggle;
                mToggleTabTxts.Add(rc.Get<GameObject>(tStr).transform.Find("Text").GetComponent<Text>());
                mToggleTabs.Add(rc.Get<GameObject>(tStr).GetComponent<Toggle>());
            }

            textProblemDesc = rc.Get<GameObject>("Text_ProblemDesc").GetComponent<Text>();
            Head_list = rc.Get<GameObject>("Head_list");
            Image_Menu = rc.Get<GameObject>("Image_Menu").GetComponent<RectTransform>();
            rawimageHead = rc.Get<GameObject>("RawImage_Head").GetComponent<RawImage>();
            imageBoy = rc.Get<GameObject>("Image_Boy").GetComponent<Image>();
            imageGirl = rc.Get<GameObject>("Image_Girl").GetComponent<Image>();
            textNick = rc.Get<GameObject>("Text_Nick").GetComponent<Text>();
            textUserId = rc.Get<GameObject>("Text_UserId").GetComponent<Text>();
            buttonSign = rc.Get<GameObject>("Button_Sign").GetComponent<Button>();
            buttonStandup = rc.Get<GameObject>("Button_Standup").GetComponent<Button>();
            buttonFourceout = rc.Get<GameObject>("Button_Fourceout").GetComponent<Button>();
            textPs = rc.Get<GameObject>("Text_Ps").GetComponent<Text>();
            radarChart = rc.Get<GameObject>("Radar_Chart").GetComponent<RadarChart>();
            transLockGo = rc.Get<GameObject>("LockGo").transform;
            textLockTitle = rc.Get<GameObject>("Text_LockTitle").GetComponent<Text>();
            textLockSubTitle = rc.Get<GameObject>("Text_LockSubTitle").GetComponent<Text>();
            imageLockBean = rc.Get<GameObject>("Image_LockBean").GetComponent<Image>();
            EmojiContent = rc.Get<GameObject>("EmojiContent").GetComponent<RectTransform>();

            textjoinPai = rc.Get<GameObject>("Text_joinPai").GetComponent<Text>();
            textenterPoolWin = rc.Get<GameObject>("Text_enterPoolWin").GetComponent<Text>();
            texttotalHand = rc.Get<GameObject>("Text_totalHand").GetComponent<Text>();
            textenterPoolRate = rc.Get<GameObject>("Text_enterPoolRate").GetComponent<Text>();

            textPAP1 = rc.Get<GameObject>("Text_PAP1").GetComponent<Text>();
            textPAP2 = rc.Get<GameObject>("Text_PAP2").GetComponent<Text>();
            textPAP3 = rc.Get<GameObject>("Text_PAP3").GetComponent<Text>();
            textPAP4 = rc.Get<GameObject>("Text_PAP4").GetComponent<Text>();
            transPAP = rc.Get<GameObject>("PAP").transform;
            transNormalOmaha = rc.Get<GameObject>("NormalOmaha").transform;

            textSNGMTT1 = rc.Get<GameObject>("Text_SNGMTT1").GetComponent<Text>();
            textSNGMTT2 = rc.Get<GameObject>("Text_SNGMTT2").GetComponent<Text>();
            textSNGMTT3 = rc.Get<GameObject>("Text_SNGMTT3").GetComponent<Text>();
            textSNGMTT4 = rc.Get<GameObject>("Text_SNGMTT4").GetComponent<Text>();
            textSNGMTT5 = rc.Get<GameObject>("Text_SNGMTT5").GetComponent<Text>();
            transSNGMTT = rc.Get<GameObject>("SNGMTT").transform;
            DescGO = rc.Get<GameObject>("DescGO");

            toggleDefault = rc.Get<GameObject>("ToggleDefault").GetComponent<Toggle>();
            UIEventListener.Get(toggleDefault.gameObject).onClick = ClickToggleDefault;

            UIEventListener.Get(rc.Get<GameObject>("ImageLockGO")).onClick = ClickBreakLock;
            UIEventListener.Get(buttonClose.gameObject).onClick = onClickClose;
            UIEventListener.Get(buttonStandup.gameObject).onClick = onClickStandup;
            UIEventListener.Get(buttonFourceout.gameObject).onClick = onClickFourceout;
            UIEventListener.Get(buttonSign.gameObject).onClick = ClickSign;
            UIEventListener.Get(rc.Get<GameObject>("BtnProblem")).onClick = ClickBtnProblem;
            UIEventListener.Get(rc.Get<GameObject>("Button_Complaint")).onClick = ClickButton_Complaint;

            foreach (Transform tranAnimoji in EmojiContent.transform)
            {
                string animojiName = tranAnimoji.gameObject.name;
                int cost = GameUtil.AnimojiCost(animojiName);
                tranAnimoji.gameObject.GetComponentInChildren<Text>().text = StringHelper.ShowGold(cost);

                UIEventListener.Get(tranAnimoji.gameObject).onClick = onClickAnimoji;
            }
        }

        private void ClickButton_Complaint(GameObject go)
        {
            if (GameCache.Instance.CurGame.MainPlayer.seatID < 0)
            {
                UIComponent.Instance.ToastLanguage("UITexasCom001");
                return;
            }
            UIComponent.Instance.ShowNoAnimation(UIType.UITexasComplaint,
                new UITexasComplaintComponent.ComplaintData() { mLoadPanelPlayers = mCurAwakePlayers, mLoadPanelPlayerId = mInfoData.mUserId });
            UIComponent.Instance.Remove(UIType.UITexasInfo);
        }

        private void onClickAnimoji(GameObject go)
        {
            if (GameCache.Instance.CurGame.MainPlayer.seatID < 0)
            {
                // UIComponent.Instance.Toast("观众无法发送魔法表情");
                UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(20049));
                return;
            }
            string animojiName = go.name;

            int cost = GameUtil.AnimojiCost(animojiName);
            if (cost > GameCache.Instance.gold)
            {
                UIComponent.Instance.Toast(LanguageManager.Get("adaptation20010"));
                return;
            }

            GameCache.Instance.CurGame.SendAnimoji(animojiName, mInfoData.mUserId.ToString());

            UIComponent.Instance.Remove(UIType.UITexasInfo);
        }

        private void ClickToggleDefault(GameObject go)
        {
            var tIsSelected = toggleDefault.isOn;
            GameCache.Instance.CurInfoRoomPath = tIsSelected ? mCurRoomPath : 0;
        }

        private void ClickBtnProblem(GameObject go)
        {
            DescGO.SetActive(!DescGO.activeSelf);
        }


        /// <summary>         锁上,点击         </summary>
        private void ClickBreakLock(GameObject go)
        {
            if (mCurBreakStatus == 1403)
            {//可破隐
                APIUserBreak(pDto =>
                {
                    if (pDto.status == 0)
                    {
                        // UIComponent.Instance.Toast("破隐成功");
                        UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10310));
                        LoadHttpInterface();
                    }
                    else
                    {
                        UIComponent.Instance.Toast(pDto.status);
                    }
                });
            }
            else if (mCurBreakStatus == 1401)
            {
                //不可破隐
                if (GameCache.Instance.vipLevel == 0)
                {
                    UIComponent.Instance.ShowNoAnimation(UIType.UIMine_VIP);
                }
                else
                {
                    if (mInfoData.mUserInfo.vip == 3)
                    {
                        //最强隐身"
                    }
                    else
                    {
                        UIComponent.Instance.ShowNoAnimation(UIType.UIMine_VIP);
                    }
                }
            }
        }


        /// <summary>
        /// 选项  德州/奥马哈/AOF
        /// </summary>
        private void ClickSelectTabToggle(GameObject go, int index)
        {
            if (index == mCurRoomPath)
            {
                return;
            }
            ClickTabToggle_Non(index);
        }
        private void ClickTabToggle_Non(int index)
        {
            // var tPaths = new int[] { 61, 91, 31, 71 };
            var tPaths = mRoomPaths;
            var tShow = 0;
            for (int i = 0; i < mToggleTabTxts.Count; i++)
            {
                mToggleTabTxts[i].color = new Color32(233, 191, 128, 255);
                if (index == tPaths[i])
                    tShow = i;
            }
            mToggleTabTxts[tShow].color = new Color32(25, 21, 21, 255);
            mCurRoomPath = index;
            LoadHttpInterface();

            radarChartScale_Tr.localScale = Vector3.zero;
            //默认页
            toggleDefault.isOn = (index == GameCache.Instance.CurInfoRoomPath);
            mToggleTabs[tShow].isOn = true;
        }
        /// <summary>        /// 加载蜘蛛网    德州,奥马哈    /// </summary>
        void SetNormalOmahaView(DtoResp pDto)
        {
            radarChartScale_Tr.localScale = Vector3.zero;
            transNormalOmaha.gameObject.SetActive(true);
            transPAP.gameObject.gameObject.SetActive(false);
            transSNGMTT.gameObject.SetActive(false);

            ShowFourData(pDto);

            //textjoinPai.text = pDto.data.totalGameCnt.ToString();
            //textenterPoolWin.text = pDto.data.Wins.ToString() + "%";
            //texttotalHand.text = pDto.data.totalHand.ToString();
            //textenterPoolRate.text = pDto.data.VPIP.ToString() + "%";

            var Data1 = new RadarData { name = CPErrorCode.LanguageDescription(10311), value = pDto.data.VPIP };
            var Data2 = new RadarData { name = CPErrorCode.LanguageDescription(10312), value = pDto.data.PRF };
            var Data3 = new RadarData { name = CPErrorCode.LanguageDescription(10313), value = pDto.data.threeBet };
            var Data4 = new RadarData { name = CPErrorCode.LanguageDescription(10314), value = pDto.data.CBet };
            var Data5 = new RadarData { name = "AF", value = pDto.data.AF };
            var Data6 = new RadarData { name = CPErrorCode.LanguageDescription(10317), value = pDto.data.WTSD };
            var Data7 = new RadarData { name = CPErrorCode.LanguageDescription(10318), value = pDto.data.Allin_Wins };
            var Data8 = new RadarData { name = CPErrorCode.LanguageDescription(10319), value = pDto.data.Wins, };

            RadarData[] datas = { Data1, Data2, Data3, Data4, Data5, Data6, Data7, Data8 };
            radarChart.SetRadarData(datas);


            radarChartScale_Data.SetRadarData(datas, false);
            radarChartScale_Tr.DOScale(Vector3.one * 0.9f, 0.5f);
        }

        private void ClickSign(GameObject go)
        {
            if (mInfoData == null)
            {
                // UIComponent.Instance.Toast("错误");
                UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10322));
                return;
            }
            if (GameCache.Instance.nUserId == mInfoData.mUserId)
            {
                // UIComponent.Instance.Toast("无法给自己另起标记");
                UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10320));
                return;
            }

            UIComponent.Instance.ShowNoAnimation(UIType.UIUserRemarks, new UIUserRemarksComponent.RemarkData()
            {
                RandomId = mInfoData.mUserInfo.randomNum,
                userId = mInfoData.mUserId,
                successDelegateAPI = SuccessDelegateAPI
            });
        }

        private void SuccessDelegateAPI()
        {
            var tDto = CacheDataManager.mInstance.GetSNSBatchRelation(mInfoData.mUserInfo.randomNum);
            if (tDto != null)
            {
                textNick.text = tDto.remarkName;
                // textPs.text = "昵称:" + mInfoData.mUserInfo.nickName + "\n" + tDto.remark;
                textPs.text = $"{CPErrorCode.LanguageDescription(20041)}{mInfoData.mUserInfo.nickName}\n{tDto.remark}"; ;

                Seat mSeat = GameCache.Instance.CurGame.GetSeatByUserId(tDto.userId);
                if (mSeat != null)
                    mSeat.UpdateNickname();
            }
        }

        private void onClickFourceout(GameObject go)
        {
            UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
            {
                type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                // title = $"温馨提示",
                title = CPErrorCode.LanguageDescription(10007),
                // content = $"是否强制踢出该玩家？踢出后无法进入本房间。",
                content = CPErrorCode.LanguageDescription(20050),
                // contentCommit = "确定",
                contentCommit = CPErrorCode.LanguageDescription(10012),
                // contentCancel = "取消",
                contentCancel = CPErrorCode.LanguageDescription(10013),
                actionCommit = () =>
                {
                    GameCache.Instance.CurGame.ForcedOut(mInfoData.mUserId);
                    UIComponent.Instance.Remove(UIType.UITexasInfo);
                },
                actionCancel = null
            });
        }

        private void onClickStandup(GameObject go)
        {
            UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
            {
                type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                // title = $"温馨提示",
                title = CPErrorCode.LanguageDescription(10007),
                // content = $"是否强制站起该玩家?",
                content = CPErrorCode.LanguageDescription(20051),
                // contentCommit = "确定",
                contentCommit = CPErrorCode.LanguageDescription(10012),
                // contentCancel = "取消",
                contentCancel = CPErrorCode.LanguageDescription(10013),
                actionCommit = () =>
                {
                    GameCache.Instance.CurGame.ForcedStandup(mInfoData.mUserId);
                    UIComponent.Instance.Remove(UIType.UITexasInfo);
                },
                actionCancel = null
            });
        }

        private void onClickClose(GameObject go)
        {
            PlayerPrefsMgr.mInstance.SetInt(PlayerPrefsKeys.KEY_INFODEFAULT, GameCache.Instance.CurInfoRoomPath);
            UIComponent.Instance.Remove(UIType.UITexasInfo);
        }

        /// <summary>        /// 查询他人统计数据        /// </summary>
        void APILookPersonData(string pRandomId, int pRoomPath, Action<DtoResp> pDto)
        {
            var web = new WEB2_data_stat_person.RequestData()//牌局:61-普通局, 71-MTT, 81-SNG 91-奥马哈 51-大菠萝
            {
                breakRandomId = pRandomId,
                breakUserId = 0,
                roomPath = pRoomPath
            };
            HttpRequestComponent.Instance.Send(WEB2_data_stat_person.API, WEB2_data_stat_person.Request(web), (resData) =>
            {
                var tDto = WEB2_data_stat_person.Response(resData);
                if (pDto != null)
                    pDto(tDto);
            });
        }

        /// <summary>
        /// 破隐动作
        /// </summary>
        void APIUserBreak(Action<WEB2_user_vip_break.ResponseData> pDto)
        {
            var web = new WEB2_user_vip_break.RequestData()//牌局:61-普通局, 71-MTT, 81-SNG 91-奥马哈 51-大菠萝
            {
                breakUserRId = mInfoData.mUserInfo.randomNum
            };
            HttpRequestComponent.Instance.Send(WEB2_user_vip_break.API, WEB2_user_vip_break.Request(web), (resData) =>
            {
                var tDto = WEB2_user_vip_break.Response(resData);
                if (pDto != null)
                    pDto(tDto);
            });
        }
        #endregion


    }

    public class PlayerComplaint
    {
        public string headPic;// 头像id
        public string nick;// 昵称
        public int userID;
    }
}


