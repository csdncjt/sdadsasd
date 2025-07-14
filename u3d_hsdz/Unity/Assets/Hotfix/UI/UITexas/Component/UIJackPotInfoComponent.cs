using System;
using System.Collections;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;

namespace ETHotfix
{
	[ObjectSystem]
	public class UIJackPotInfoComponentSystem : AwakeSystem<UIJackPotInfoComponent>
	{
		public override void Awake(UIJackPotInfoComponent self)
		{
			self.Awake();
		}
	}

	public class UIJackPotInfoComponent : UIBaseComponent
	{
		public sealed class JackPotInfoData
		{
            public int roomPath;
            public int roomId;
		}

		private JackPotInfoData jackPotInfoData;
        private List<WEB2_jackpot_record.RecordListElement> mRecordDataList = new List<WEB2_jackpot_record.RecordListElement>();

        private bool bInitListView;

        private ReferenceCollector rc;
        private Button buttonClose;

        private Transform transIntroduce;
        private UIJackPotLabelComponent jackPotLabel;

        private Transform tranPannalBlind;

        private Transform transJackPot;
        private Text textBlind;
        private Text textths;
        private Text texthj;
        private Text textst;
        private Image circle_ths;
        private Image circle_hj;
        private Image circle_st;

        private Text Text_Tips;

        GameObject blindItem;

        private Transform transRecord;
        private Text textbiggestWinner;
        private Text textbiggestWinnerBlind;
        private Text textbiggestWinnerCardType;
        private Text textbiggestWinnerReward;
        private Text textbiggestWinnerTime;
        private LoopListView2 mLoopListView;

        List<GameObject> itemList;


        public void Awake()
		{
			rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            buttonClose = rc.Get<GameObject>("Button_Close").GetComponent<Button>();

            transIntroduce = rc.Get<GameObject>("view_Introduce").transform;
            jackPotLabel = new UIJackPotLabelComponent(rc.Get<GameObject>("UIJackPotLabel"), 768);


            Text_Tips = rc.Get<GameObject>("Text_Tips").GetComponent<Text>();

            transJackPot = rc.Get<GameObject>("view_JackPot").transform;
            textths = rc.Get<GameObject>("Text_ths").GetComponent<Text>();
            textBlind = rc.Get<GameObject>("Text_blind").GetComponent<Text>();
            texthj = rc.Get<GameObject>("Text_hj").GetComponent<Text>();
            textst = rc.Get<GameObject>("Text_st").GetComponent<Text>();
            circle_ths = rc.Get<GameObject>("circle_ths").GetComponent<Image>();
            circle_hj = rc.Get<GameObject>("circle_hj").GetComponent<Image>();
            circle_st = rc.Get<GameObject>("circle_st").GetComponent<Image>();

            transRecord = rc.Get<GameObject>("view_Record").transform;
            textbiggestWinner = rc.Get<GameObject>("Text_biggestWinner").GetComponent<Text>();
            textbiggestWinnerBlind = rc.Get<GameObject>("Text_biggestWinnerBlind").GetComponent<Text>();
            textbiggestWinnerCardType = rc.Get<GameObject>("Text_biggestWinnerCardType").GetComponent<Text>();
            textbiggestWinnerReward = rc.Get<GameObject>("Text_biggestWinnerReward").GetComponent<Text>();
            textbiggestWinnerTime = rc.Get<GameObject>("Text_biggestWinnerTime").GetComponent<Text>();
            mLoopListView = rc.Get<GameObject>("Scrollview").GetComponent<LoopListView2>();

            tranPannalBlind = rc.Get<GameObject>("Panel_blind").transform;

            blindItem = tranPannalBlind.Find($"blindItem0").gameObject;
            blindItem.SetActive(false);

            itemList = new List<GameObject>();


            UISegmentControlComponent.SetUp(rc.Get<GameObject>("Image_Dialog").transform, new UISegmentControlComponent.SegmentData()
            {
                //Titles = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIJackPotInfoList"),
                Titles = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIJackPotInfoList"),
                OnClick = OnSegmentClick,
                N_S_Fonts = new[] { 40, 40 },
                N_S_Color = new[] {new Color32(168, 168, 168, 255), new Color32(233, 191, 128, 255)}
            });

            UIEventListener.Get(buttonClose.gameObject).onClick = onClickClose;

            registerHandler();
		}

        public void OnSegmentClick(GameObject go, int index)
        {
            switch (index)
            {
                case 0:
                    transIntroduce.gameObject.SetActive(true);
                    transJackPot.gameObject.SetActive(false);
                    transRecord.gameObject.SetActive(false);
                    break;
                case 1:
                    transIntroduce.gameObject.SetActive(false);
                    transJackPot.gameObject.SetActive(true);
                    transRecord.gameObject.SetActive(false);
                    break;
                case 2:
                    transIntroduce.gameObject.SetActive(false);
                    transJackPot.gameObject.SetActive(false);
                    transRecord.gameObject.SetActive(true);
                    break;
            }
        }

        private void onClickClose(GameObject go)
        {
            UIComponent.Instance.HideNoAnimation(UIType.UIJackPotInfo);
		}

		public override void OnShow(object obj)
		{
			if (null != obj)
			{
				jackPotInfoData = obj as JackPotInfoData;

                jackPotLabel.SetNumber(0, false);
                if (null != jackPotInfoData)
				{
                    ObtinJackPotInfo();
                    ObtinJackPotRecord();
				}
			}
		}

        //获取JackPot信息
        void ObtinJackPotInfo()
        {
            WEB2_jackpot_view.RequestData requestData = new WEB2_jackpot_view.RequestData()
            {
                roomPath = jackPotInfoData.roomPath,
                roomId = jackPotInfoData.roomId
            };
            HttpRequestComponent.Instance.Send(
                WEB2_jackpot_view.API,
                WEB2_jackpot_view.Request(requestData),
                OpenJackPotInfo);
        }

        private GameObject GetBlindItem(int index, Transform parent)
        {
            GameObject obj = GameObject.Instantiate(blindItem);
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(parent);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            itemList.Insert(index, obj);
            return obj;
        }

        void OpenJackPotInfo(string resData)
        {
            foreach (var a in itemList)
            {
                GameObject.Destroy(a.gameObject);
            }
            itemList.Clear();

            Log.Debug(resData);
            WEB2_jackpot_view.ResponseData responseData = WEB2_jackpot_view.Response(resData);
            if (responseData.status == 0)
            {
                jackPotLabel.SetNumber((int)responseData.data.jackPotFund, false);

                if (GameCache.Instance.room_path == 21 || GameCache.Instance.room_path == 23)
                {
                    textBlind.text = LanguageManager.Get("UIClub_RoomCreat_gmo7laWj") + $"{StringHelper.ShowGold(int.Parse(responseData.data.blind))}";
                    Text_Tips.text = LanguageManager.Get("UIClub_RoomCreat_gmo7laWj");
                }
                else
                {
                    textBlind.text = LanguageManager.Get("UIJackPotInfo_blindLevel") + $"{StringHelper.ShowBlindName(responseData.data.blind)}";
                    Text_Tips.text = LanguageManager.Get("UIJackPotInfo_blindLevel");

                }
                // texthj.text = $"{responseData.data.royalflushRatio.ToString("F1")}%";
                texthj.text = StringHelper.ShowGold((int)responseData.data.royalflushRatio);
                //circle_hj.fillAmount = (float)responseData.data.royalflushRatio / 100.0f;
                // textths.text = $"{responseData.data.straightflushRatio.ToString("F1")}%";
                textths.text = StringHelper.ShowGold((int)responseData.data.straightflushRatio);
                //circle_ths.fillAmount = (float)responseData.data.straightflushRatio / 100.0f;
                //textst.text = $"{responseData.data.fourofakindRatio.ToString("F1")}%";
                textst.text = StringHelper.ShowGold((int)responseData.data.fourofakindRatio);
                //circle_st.fillAmount = (float)responseData.data.fourofakindRatio / 100.0f;

                for (int i = 0; i < responseData.data.subpoolList.Count; i++)
                {
                    var subpoolInfo = responseData.data.subpoolList[i];
                    var tranBlindItem = GetBlindItem(i, tranPannalBlind).transform;
                   
                   // var tranBlindItem = tranPannalBlind.Find($"blindItem{i}");
                    if (null != tranBlindItem)
                    {
                        GameObject imageCur = tranBlindItem.Find("Image_cur").gameObject;
                        Text textSubBlind = tranBlindItem.Find("Text_blind").gameObject.GetComponent<Text>();
                        if (GameCache.Instance.room_path == 21 || GameCache.Instance.room_path == 23)
                        {
                            textSubBlind.text = StringHelper.ShowGold(int.Parse(subpoolInfo.blindName));
                        }
                        else
                        {
                            textSubBlind.text = StringHelper.ShowBlindName(subpoolInfo.blindName);

                        }
                       
                        Text textSubFund = tranBlindItem.Find("Text_fund").gameObject.GetComponent<Text>();
                        textSubFund.text = $"{StringHelper.ShowGold((int)subpoolInfo.fund)}";
                        if (subpoolInfo.isCurrent == 1)
                        {
                            imageCur.SetActive(true);
                            textSubBlind.color = new Color32(233, 191, 128, 255);
                            textSubFund.color = new Color32(233, 191, 128, 255);
                        }
                        else
                        {
                            imageCur.SetActive(false);
                            textSubBlind.color = new Color32(168, 168, 168, 255);
                            textSubFund.color = new Color32(168, 168, 168, 255);
                        }

                        
                    }
                }
            }
        }

        //获取JackPot奖励记录
        void ObtinJackPotRecord()
        {
            WEB2_jackpot_record.RequestData requestData = new WEB2_jackpot_record.RequestData()
            {
                roomPath = jackPotInfoData.roomPath,
                roomId = jackPotInfoData.roomId
            };
            HttpRequestComponent.Instance.Send(
                WEB2_jackpot_record.API,
                WEB2_jackpot_record.Request(requestData),
                OpenJackPotReward);
        }

        void OpenJackPotReward(string resData)
        {
            Log.Debug(resData);
            WEB2_jackpot_record.ResponseData responseData = WEB2_jackpot_record.Response(resData);
            if (responseData.status == 0)
            {
                mRecordDataList = responseData.data.recordList;
                if (bInitListView)
                {
                    mLoopListView.SetListItemCount(mRecordDataList.Count);
                    mLoopListView.RefreshAllShownItem();
                }
                else
                {
                    mLoopListView.InitListView(mRecordDataList.Count, OnGetItemByIndex);
                    bInitListView = true;
                }

                if (responseData.data.topReward != null)
                {
                    var topJackPot = responseData.data.topReward;
                    textbiggestWinner.text = topJackPot.nickName;
                    textbiggestWinnerBlind.text = StringHelper.ShowBlindName(topJackPot.blindName);
                    textbiggestWinnerCardType.text = CardTypeUtil.GetCardTypeName(topJackPot.pocerType);
                    textbiggestWinnerReward.text = StringHelper.ShowGold(topJackPot.rewardChip) + LanguageManager.Get("UIMine_WalletSC001");
                    textbiggestWinnerTime.text = TimeHelper.GetDateTimer(topJackPot.createTime).ToString("MM-dd");
                }
            }
        }

        public override void OnHide()
		{
			// Log.Debug($"AddClips OnHide");
		}

		public override void Dispose()
		{
            if (IsDisposed)
            {
                return;
            }
            mRecordDataList.Clear();
            removeHandler();
            bInitListView = false;

            base.Dispose();
		}

        private void SetUpItemWithData(GameObject go, WEB2_jackpot_record.RecordListElement element)
        {
            go.transform.Find("Text_Name").GetComponent<Text>().text = element.nickName;
            go.transform.Find("Text_Blind").GetComponent<Text>().text = StringHelper.ShowBlindName(element.blindName);
            go.transform.Find("Text_CardType").GetComponent<Text>().text = CardTypeUtil.GetCardTypeName(element.pocerType);
            go.transform.Find("Text_Reward").GetComponent<Text>().text = $"{StringHelper.ShowGold(element.rewardChip)}"+LanguageManager.Get("UIMine_WalletSC001");
            go.transform.Find("Text_Time").GetComponent<Text>().text = TimeHelper.GetDateTimer(element.createTime).ToString("MM-dd");
        }


        #region SupperScrollView
        LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
        {
            if (index < 0)
            {
                return null;
            }
            WEB2_jackpot_record.RecordListElement itemData = mRecordDataList[index];
            if (itemData == null)
            {
                return null;
            }
            LoopListViewItem2 item = listView.NewListViewItem("JackPotReward_info");
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
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_JACKPOT_CHANGE, HANDLER_REQ_GAME_JACKPOT_CHANGE);
        }

		private void removeHandler()
		{
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_JACKPOT_CHANGE, HANDLER_REQ_GAME_JACKPOT_CHANGE);
        }

        protected void HANDLER_REQ_GAME_JACKPOT_CHANGE(ICPResponse response)
        {
            REQ_GAME_JACKPOT_CHANGE rec = response as REQ_GAME_JACKPOT_CHANGE;
            if (null == rec)
                return;
            if (rec.status == 0)
            {
                if (!string.IsNullOrEmpty(rec.subJackPotNum))
                {
                    jackPotLabel.SetNumber(Convert.ToInt32(rec.subJackPotNum), true);
                }
            }
        }
    }
}
