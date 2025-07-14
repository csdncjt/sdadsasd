using ETModel;
using UnityEngine;
using UnityEngine.UI;
using xcharts;
using DG.Tweening;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIDataComponentAwakeSystem : AwakeSystem<UIDataComponent>
    {
        public override void Awake(UIDataComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 1-全部, 2-30天, 3-7天
    /// </summary>
    public enum AllSevenMonth
    {
        All = 1,
        Month = 2,
        Seven = 3
    }

    public class UIDataComponent : UIBaseComponent
    {
        ReferenceCollector rc;
        private int mRoomPath;
        private Transform transDateSelectView;
        private GameObject btnAll;
        private GameObject btnWeek;
        private GameObject btnMonth;

        private GameObject gameBaseScrollView;
        private GameObject papDataView;
        private GameObject matchDataView;

        private Button buttonVIP;
        private Button buttonLock;

        //普通局，奥马哈
        private Image chartCircleVIP;
        private Image chartCircleWins;
        private Text textVPIP;
        private Text textWins;
        private Text textTotalScore;

        private Text textTotalScore2;
        private Text textTotalTitle2;

        private GridLayoutGroup baseDataGridGroup;
        private Text baseAllHands;
        private Text baseVPIP;
        private Text baseAverageScore;
        private Text baseTotalGame;
        private Text baseWins;
        private Text baseAverageBring;

        private RadarChart radarChart;
        private GridLayoutGroup advanceDataGrid;
        private Text advanceVPIP;
        private Text advancePRF;
        private Text advanceThreeBet;
        private Text advanceCbet;
        private Text advanceAF;
        private Text advanceWTSD;
        private Text advanceAllWins;
        private Text advanceWins;

        //大菠萝
        private Text papWins;
        private Text papFantasy;
        private Text papHandAverage;
        private Text papFantasyAverage;

        //SNG,MTT
        private Text matchPlayTimes;
        private Text matchWinTimes;
        private Text matchFirstTimes;
        private Text matchSecondTimes;
        private Text matchThirdTimes;

        private RectTransform Content_Normal;

        /// <summary>
        /// AOF需要隐藏
        /// </summary>
        private GameObject view_pro_data;
        int[] mRoomPaths = new int[] { 61, 91, 31, 21 };

        //大菠萝，SNG，MTT

        public void Awake()
        {
            mStartContent = Vector2.zero;
            InitUI();
            mRoomPath = mRoomPaths[0];//RoomPath.Normal;
            //ObtainData();
        }

        public void ObtainData()
        {
            var tDto = new WEB2_data_stat_person_profit.RequestData()
            {
                roomPath = mRoomPath,
                timeType = mCurTopTimeType.GetHashCode()
            };
            HttpRequestComponent.Instance.Send(
                WEB2_data_stat_person_profit.API,
                WEB2_data_stat_person_profit.Request(tDto),
                this.OpenDataInfo);
        }

        private Sequence mCircleSequence;

        void OpenDataInfo(string resData)
        {
            WEB2_data_stat_person_profit.ResponseData responseData = WEB2_data_stat_person_profit.Response(resData);
            if (responseData.status == 0)
            {
                var data = responseData.data;
                if (data.roomPath <= 0)
                {
                    Log.Error("roomPath<=0  数据无效  蜘蛛网未初始化");
                    return;
                }
                if (data.roomPath == mRoomPaths[0] || data.roomPath == mRoomPaths[1] || data.roomPath == mRoomPaths[2] || data.roomPath == mRoomPaths[3])
                {
                    var targetVIP = (float)data.VPIP / 100;
                    mCircleSequence.Append(DOTween.To(delegate (float value)
                    {
                        chartCircleVIP.fillAmount = value;
                        textVPIP.text = ((int)(value * 100)).ToString() + "%";
                    }, 0, targetVIP, 0.5f));

                    var targetWins = (float)data.Wins / 100;
                    mCircleSequence.Append(DOTween.To(delegate (float value)
                    {
                        chartCircleWins.fillAmount = value;
                        textWins.text = ((int)(value * 100)).ToString() + "%";
                    }, 0, targetWins, 0.5f));

                    textTotalScore2.text = $"{StringHelper.ShowGold(data.clubRoomPlTotal)}";

                    textTotalScore.text = $"{StringHelper.ShowGold(data.totalEarn)}";
                    baseAllHands.text = $"{data.totalHand}";
                    baseVPIP.text = $"{data.VPIP}%";
                    baseAverageScore.text = StringHelper.GetSignedString(data.aveageEarn);
                    if (data.aveageEarn > 0)
                    {
                        baseAverageScore.color = new Color32(217, 41, 41, 255);
                    }
                    else if (data.aveageEarn < 0)
                    {
                        baseAverageScore.color = new Color32(41, 217, 53, 255);
                    }

                    baseTotalGame.text = $"{data.totalGameCnt}";
                    baseWins.text = $"{data.Wins}%";
                    baseAverageBring.text = StringHelper.ShowGold(data.aveageBringIn);

                    //高阶数据
                    if (data.showAdvance == 1)
                    {
                        //可查看高阶数据
                        buttonLock.gameObject.SetActive(false);
                        string vipName = "";
                        if (GameCache.Instance.vipLevel >= 1)
                        {
                            vipName = LanguageManager.mInstance.GetLanguageForKeyMoreValue("UIData_Script101", GameCache.Instance.vipLevel - 1);
                        }
                        //if (GameCache.Instance.vipLevel == 1)
                        //{
                        //    vipName = "黄金VIP";
                        //}
                        //else if (GameCache.Instance.vipLevel == 2)
                        //{
                        //    vipName = "铂金VIP";
                        //}
                        //else if (GameCache.Instance.vipLevel == 3)
                        //{
                        //    vipName = "钻石VIP";
                        //}
                        string endDate = TimeHelper.GetDateTimer(long.Parse(GameCache.Instance.vipEndDate)).ToString("yyyy-MM-dd");
                        buttonVIP.gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text = vipName + endDate + LanguageManager.mInstance.GetLanguageForKey("UIData_Script104");

                        RadarData vpipData = new RadarData
                        {
                            name = CPErrorCode.LanguageDescription(10311),
                            value = data.VPIP,
                        };
                        RadarData PRFData = new RadarData
                        {
                            name = CPErrorCode.LanguageDescription(10312),
                            value = data.PRF,
                        };
                        RadarData threeBetData = new RadarData
                        {
                            name = CPErrorCode.LanguageDescription(10313),
                            value = data.threeBet,
                        };
                        RadarData CBetData = new RadarData
                        {
                            name = CPErrorCode.LanguageDescription(10314),
                            value = data.CBet,
                        };
                        RadarData AFData = new RadarData
                        {
                            name = "AF(0)",
                            value = data.AF,
                        };
                        RadarData WTSDData = new RadarData
                        {
                            name = CPErrorCode.LanguageDescription(10317),
                            value = data.WTSD,
                        };
                        RadarData AllinWinsData = new RadarData
                        {
                            name = CPErrorCode.LanguageDescription(10318),
                            value = data.Allin_Wins,
                        };
                        RadarData WinsData = new RadarData
                        {
                            name = CPErrorCode.LanguageDescription(10319),
                            value = data.Wins,
                        };
                        RadarData[] datas = { vpipData, PRFData, threeBetData, CBetData, AFData, WTSDData, AllinWinsData, WinsData };
                        radarChart.SetRadarData(datas);

                        advanceVPIP.text = $"{data.VPIP}%";
                        advancePRF.text = $"{data.PRF}%";
                        advanceThreeBet.text = $"{data.threeBet}%";
                        advanceCbet.text = $"{data.CBet}%";
                        advanceAF.text = $"{data.AF}%";
                        advanceWTSD.text = $"{data.WTSD}%";
                        advanceAllWins.text = $"{data.Allin_Wins}%";
                        advanceWins.text = $"{data.Wins}%";
                    }
                    else
                    {
                        buttonLock.gameObject.SetActive(true);

                        string vipTips = @"";
                        var tStrLangs = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIData_Script102");
                        if (mCurTopTimeType == AllSevenMonth.All)
                        {
                            vipTips = tStrLangs[0];
                            //vipTips = "钻石";
                        }
                        else if (mCurTopTimeType == AllSevenMonth.Month)
                        {
                            vipTips = tStrLangs[1];
                            // vipTips = "铂金,钻石";
                        }
                        else if (mCurTopTimeType == AllSevenMonth.Seven)
                        {
                            vipTips = tStrLangs[2];
                            //vipTips = "所有";
                        }

                        buttonVIP.gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text = vipTips + LanguageManager.mInstance.GetLanguageForKey("UIData_Script103"); //$"{ vipTips}VIP可查看";

                        RadarData vpipData = new RadarData
                        {
                            name = CPErrorCode.LanguageDescription(10311),
                            value = 0,
                        };
                        RadarData PRFData = new RadarData
                        {
                            name = CPErrorCode.LanguageDescription(10312),
                            value = 0,
                        };
                        RadarData threeBetData = new RadarData
                        {
                            name = CPErrorCode.LanguageDescription(10313),
                            value = 0,
                        };
                        RadarData CBetData = new RadarData
                        {
                            name = CPErrorCode.LanguageDescription(10314),
                            value = 0,
                        };
                        RadarData AFData = new RadarData
                        {
                            name = "AF",
                            value = 0,
                        };
                        RadarData WTSDData = new RadarData
                        {
                            name = CPErrorCode.LanguageDescription(10317),
                            value = 0,
                        };
                        RadarData AllinWinsData = new RadarData
                        {
                            name = CPErrorCode.LanguageDescription(10318),
                            value = 0,
                        };
                        RadarData WinsData = new RadarData
                        {
                            name = CPErrorCode.LanguageDescription(10319),
                            value = 0,
                        };
                        RadarData[] datas = { vpipData, PRFData, threeBetData, CBetData, AFData, WTSDData, AllinWinsData, WinsData };
                        radarChart.SetRadarData(datas);

                        advanceVPIP.text = "--";
                        advancePRF.text = "--";
                        advanceThreeBet.text = "--";
                        advanceCbet.text = "--";
                        advanceAF.text = "--";
                        advanceWTSD.text = "--";
                        advanceAllWins.text = "--";
                        advanceWins.text = "--";
                    }
                }
                else if (data.roomPath == (int)RoomPath.PAP)//暂无的,不用理会
                {
                    papWins.text = $"{data.papWins}%";
                    papFantasy.text = $"{data.fantasy}%";
                    papHandAverage.text = $"{data.handAverage}";
                    papFantasyAverage.text = $"{data.fantasyAverage}";
                }
                else if (data.roomPath == (int)RoomPath.SNG || data.roomPath == (int)RoomPath.MTT)//暂无的,不用理会
                {
                    matchPlayTimes.text = $"{data.playTimes}";
                    matchWinTimes.text = $"{data.winTimes}";
                    matchFirstTimes.text = $"{data.firstTimes}";
                    matchSecondTimes.text = $"{data.secondTimes}";
                    matchThirdTimes.text = $"{data.thirdTimes}";
                }
            }
        }

        public override void OnShow(object obj)
        {
            //SetUpNav("数据", UIType.UIData);
            float gridWidthCount = rc.GetComponent<RectTransform>().rect.width;
            baseDataGridGroup.cellSize = new Vector2(gridWidthCount / 3, 240);
            advanceDataGrid.cellSize = new Vector2((gridWidthCount - 48 * 2) / 2, 250);

            UISegmentControlComponent.TextUpdate = true;

            ObtainData();
        }

        public override void OnHide()
        {

        }

        public override void Dispose()
        {
            mCircleSequence = null;
            base.Dispose();
        }

        Vector2 mStartContent;
        #region UI

        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            Content_Normal = rc.Get<GameObject>("Content_Normal").GetComponent<RectTransform>();
            view_pro_data = rc.Get<GameObject>("view_pro_data");

            NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
            float realTop = safeArea.top * 1242 / safeArea.width;
            rc.Get<GameObject>("view_title").GetComponent<LayoutElement>().minHeight = 200 + realTop;
            Content_Normal.sizeDelta = new Vector2(Content_Normal.sizeDelta.x, Content_Normal.sizeDelta.y + realTop);
            if (mStartContent == Vector2.zero)
                mStartContent = Content_Normal.sizeDelta;

            UISegmentControlComponent.SetUp(rc.transform, new UISegmentControlComponent.SegmentData()
            {
                //Titles = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIDataList"),
                Titles = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIDataList"),
                OnClick = OnSegmentClick,
                N_S_Fonts = new[] { 48, 48 },
                N_S_Color = new Color32[] { new Color32(179, 142, 86, 255), new Color32(255, 255, 255, 255) },
                IsEffer = true
            });

            baseDataGridGroup = rc.Get<GameObject>("view_base_data_grid").GetComponent<GridLayoutGroup>();
            advanceDataGrid = rc.Get<GameObject>("view_advance_data_grid").GetComponent<GridLayoutGroup>();
            transDateSelectView = rc.Get<GameObject>("view_select_type").transform;
            btnAll = rc.Get<GameObject>("btn_all");
            btnWeek = rc.Get<GameObject>("btn_week");
            btnMonth = rc.Get<GameObject>("btn_month");

            gameBaseScrollView = rc.Get<GameObject>("game_scrollView");
            papDataView = rc.Get<GameObject>("view_pap_data");
            matchDataView = rc.Get<GameObject>("view_match_data");

            buttonVIP = rc.Get<GameObject>("Button_VIP").GetComponent<Button>();
            buttonLock = rc.Get<GameObject>("Button_Lock").GetComponent<Button>();

            //普通局，奥马哈
            chartCircleVIP = rc.Get<GameObject>("chart_circle_VIP").GetComponent<Image>();
            chartCircleWins = rc.Get<GameObject>("chart_circle_Wins").GetComponent<Image>();

            textVPIP = rc.Get<GameObject>("text_VPIP_value").GetComponent<Text>();
            textWins = rc.Get<GameObject>("text_Wins_value").GetComponent<Text>();
            textTotalScore = rc.Get<GameObject>("text_totalScore_value").GetComponent<Text>();

            textTotalScore2 = rc.Get<GameObject>("text_totalScore_value2").GetComponent<Text>();

            baseAllHands = rc.Get<GameObject>("text_base_allHands").GetComponent<Text>();
            baseVPIP = rc.Get<GameObject>("text_base_VPIP").GetComponent<Text>();
            baseAverageScore = rc.Get<GameObject>("text_base_average_score").GetComponent<Text>();
            baseTotalGame = rc.Get<GameObject>("text_base_total_game").GetComponent<Text>();
            baseWins = rc.Get<GameObject>("text_base_Wins").GetComponent<Text>();
            baseAverageBring = rc.Get<GameObject>("text_base_average_bring").GetComponent<Text>();

            radarChart = rc.Get<GameObject>("radar_Chart").GetComponent<RadarChart>();

            advanceVPIP = rc.Get<GameObject>("text_advance_VPIP").GetComponent<Text>();
            advancePRF = rc.Get<GameObject>("text_advance_PRF").GetComponent<Text>();
            advanceThreeBet = rc.Get<GameObject>("text_advance_3Bet").GetComponent<Text>();
            advanceCbet = rc.Get<GameObject>("text_advance_Cbet").GetComponent<Text>();
            advanceAF = rc.Get<GameObject>("text_advance_AF").GetComponent<Text>();
            advanceWTSD = rc.Get<GameObject>("text_advance_WTSD").GetComponent<Text>();
            advanceAllWins = rc.Get<GameObject>("text_advance_All_wins").GetComponent<Text>();
            advanceWins = rc.Get<GameObject>("text_advance_Wins").GetComponent<Text>();

            //大菠萝
            papWins = rc.Get<GameObject>("text_pap_wins").GetComponent<Text>();
            papFantasy = rc.Get<GameObject>("text_fantasy").GetComponent<Text>();
            papHandAverage = rc.Get<GameObject>("text_hand_average").GetComponent<Text>();
            papFantasyAverage = rc.Get<GameObject>("text_fantasy_average").GetComponent<Text>();

            //SNG，MTT
            matchPlayTimes = rc.Get<GameObject>("text_play_times").GetComponent<Text>();
            matchWinTimes = rc.Get<GameObject>("text_win_times").GetComponent<Text>();
            matchFirstTimes = rc.Get<GameObject>("text_first_times").GetComponent<Text>();
            matchSecondTimes = rc.Get<GameObject>("text_second_times").GetComponent<Text>();
            matchThirdTimes = rc.Get<GameObject>("text_third_times").GetComponent<Text>();

            UIEventListener.Get(btnAll.gameObject, 1).onIntClick = OnTopBtnClick;
            UIEventListener.Get(btnWeek.gameObject, 3).onIntClick = OnTopBtnClick;
            UIEventListener.Get(btnMonth.gameObject, 2).onIntClick = OnTopBtnClick;

            UIEventListener.Get(rc.Get<GameObject>("BtnHelp")).onClick = ClickBtnHelp;

            UIEventListener.Get(buttonVIP.gameObject).onClick = ClickBtnVIP;
            UIEventListener.Get(buttonLock.gameObject).onClick = ClickBtnVIP;
        }

        private void ClickBtnHelp(GameObject go)
        {
            //UIComponent.Instance.ShowNoAnimation(UIType.UIData_ShowProblem);
            //var tEle = new string[] { "cn", "en", "ft" };
            //var tUrl = GlobalData.Instance.DataAnalysURL + tEle[LanguageManager.mInstance.mCurLanguage];
            UIComponent.Instance.ShowNoAnimation(UIType.UIData_Terminol);//数据说明
        }

        private void ClickBtnVIP(GameObject go)
        {
            UIComponent.Instance.ShowNoAnimation(UIType.UIMine_VIP);
        }
        #endregion

        #region Action
        /// <summary>
        /// 1-全部, 2-30天, 3-7天
        /// </summary>
        AllSevenMonth mCurTopTimeType = AllSevenMonth.All;
        private void OnTopBtnClick(GameObject pGo, int pIndex)
        {
            if (pIndex == mCurTopTimeType.GetHashCode())
            {
                return;
            }
            mCurTopTimeType = (AllSevenMonth)pIndex;
            ObtainData();
        }

        public void OnSegmentClick(GameObject go, int index)
        {
            mRoomPath = mRoomPaths[index];
            ObtainData();
            // gameBaseScrollView.SetActive(true);
            // papDataView.SetActive(false);
            // matchDataView.SetActive(false);
            // Content_Normal.localPosition = new Vector3(0, 0, 0);
            //
            // if (index <= 1)
            // {
            //     view_pro_data.SetActive(true);
            //     Content_Normal.sizeDelta = mStartContent;
            // }
            // else
            // {
            //     Content_Normal.sizeDelta = new Vector2(mStartContent.x, mStartContent.y - 2000);
            //     view_pro_data.SetActive(false);
            // }

            if (index >= 0 && index <= 3)
            {
                // 普通、奥马哈、AOF
                gameBaseScrollView.SetActive(true);
                papDataView.SetActive(false);
                matchDataView.SetActive(false);
                Content_Normal.localPosition = new Vector3(0, 0, 0);
                view_pro_data.SetActive(true);
                Content_Normal.sizeDelta = mStartContent;
            }
            else if (index == 4)
            {
                // MTT
                gameBaseScrollView.SetActive(false);
                papDataView.SetActive(false);
                matchDataView.SetActive(true);
                Content_Normal.localPosition = new Vector3(0, 0, 0);
                Content_Normal.sizeDelta = new Vector2(mStartContent.x, mStartContent.y - 2000);
                view_pro_data.SetActive(false);
            }

            //if (index == 0)
            //{
            //    mRoomPath =RoomPath.Normal;
            //}
            //if (index == 1)
            //{
            //    mRoomPath = RoomPath.Omaha;
            //}
            //if (index == 2)
            //{
            //    mRoomPath = RoomPath.NormalAof
            //    //   mRoomPath = RoomPath.PAP;//改成AOF 了
            //}
            //if (index == 3)
            //{
            //    mRoomPath = RoomPath.SNG;
            //}
            //if (index == 4)
            //{
            //    mRoomPath = RoomPath.MTT;
            //}
            //ObtainData();

            //if (index <= 2)
            //{
            //    gameBaseScrollView.SetActive(true);
            //    papDataView.SetActive(false);
            //    matchDataView.SetActive(false);
            //    Content_Normal.localPosition = new Vector3(0, 0, 0);

            //    if (index <= 1)
            //    {
            //        view_pro_data.SetActive(true);
            //        Content_Normal.sizeDelta = mStartContent;
            //    }
            //    else
            //    {
            //        Content_Normal.sizeDelta = new Vector2(mStartContent.x, mStartContent.y - 2000);
            //        view_pro_data.SetActive(false);
            //    }
            //}
            //else if (index == 2)
            //{
            //    //view_pro_data.SetActive(false);
            //    //Content_Normal.localPosition = new Vector3(0, 0, 0);
            //    //gameBaseScrollView.SetActive(false);
            //    //papDataView.SetActive(true);
            //    //matchDataView.SetActive(false);
            //}
            //else
            //{
            //    //gameBaseScrollView.SetActive(false);
            //    //papDataView.SetActive(false);
            //    //matchDataView.SetActive(true);
            //}
        }
        #endregion
    }
}
