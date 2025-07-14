using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	[ObjectSystem]
	public class UIInsuranceComponentAwakeSystem : AwakeSystem<UIInsuranceComponent>
	{
		public override void Awake(UIInsuranceComponent self)
		{
			self.Awake();
		}
	}

    [ObjectSystem]
    public class UIInsuranceComponentUpdateSystem : UpdateSystem<UIInsuranceComponent>
    {
        public override void Update(UIInsuranceComponent self)
        {
            self.Update();
        }
    }

    public class UIInsuranceComponent : UIBaseComponent
	{
		public sealed class InsuranceData
		{
			public List<sbyte> cards;
			public List<WrapTriggedInsuranceData> triggedData;
            public int timeLeft;
            public int existEqualOuts;
            public int delayTimes;
        }

		private sealed class PlayerItem
		{
			public Transform trans;
			private ReferenceCollector rc;
			private Image imageCard0;
			private Image imageCard1;
            private Image imageCard2;
            private Image imageCard3;
            private Text textNickname;
			private Text textOuts;

            private List<Image> imageCards;

			public PlayerItem(Transform transform)
			{
				trans = transform;
				rc = trans.GetComponent<ReferenceCollector>();
				imageCard0 = rc.Get<GameObject>("Image_Card0").GetComponent<Image>();
				imageCard1 = rc.Get<GameObject>("Image_Card1").GetComponent<Image>();
                imageCard2 = rc.Get<GameObject>("Image_Card2").GetComponent<Image>();
                imageCard3 = rc.Get<GameObject>("Image_Card3").GetComponent<Image>();
                textNickname = rc.Get<GameObject>("Text_Nickname").GetComponent<Text>();
				textOuts = rc.Get<GameObject>("Text_Outs").GetComponent<Text>();

                imageCards = new List<Image>();
                imageCards.Add(imageCard0);
                imageCards.Add(imageCard1);
                imageCards.Add(imageCard2);
                imageCards.Add(imageCard3);
            }

			public void UpdateItem(List<sbyte> cardIds, string nickName, int outs)
            {
                // imageCard0.sprite = GameCache.Instance.CurGame.RcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(cardIds[0]));
                // imageCard1.sprite = GameCache.Instance.CurGame.RcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(cardIds[1]));
                // if ((RoomPath)GameCache.Instance.room_path == RoomPath.Omaha)
                // {
                //     imageCard2.sprite = GameCache.Instance.CurGame.RcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(cardIds[2]));
                //     imageCard3.sprite = GameCache.Instance.CurGame.RcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(cardIds[3]));
                //     imageCard2.gameObject.SetActive(true);
                //     imageCard3.gameObject.SetActive(true);
                //     imageCard1.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-16, 0);
                // }
                // else
                // {
                //     imageCard2.gameObject.SetActive(false);
                //     imageCard3.gameObject.SetActive(false);
                //     imageCard1.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(48, 0);
                // }


                int mUpdateStart = 0;
                int mUpdateEnd = 0;
                int mHideStart = 0;
                int mHideEnd = 0;
                int mTmpHandCards = GameCache.Instance.CurGame.HandCards;
                int mTmpCount = imageCards.Count;
                if (mTmpHandCards > mTmpCount)
                {
                    mUpdateEnd = mTmpCount;
                }
                else if(mTmpHandCards < mTmpCount)
                {
                    mUpdateEnd = mTmpHandCards;
                    mHideStart = mTmpHandCards;
                    mHideEnd = mTmpCount;
                }
                else
                {
                    mUpdateEnd = mTmpHandCards;
                }

                for (int i = mUpdateStart; i < mUpdateEnd; i++)
                {
                    imageCards[i].sprite = GameCache.Instance.CurGame.RcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(cardIds[i]));
                    imageCards[i].gameObject.SetActive(true);
                }

                for (int i = mHideStart; i < mHideEnd; i++)
                {
                    imageCards[i].gameObject.SetActive(false);
                }

                imageCard1.gameObject.GetComponent<RectTransform>().anchoredPosition = mTmpHandCards >= mTmpCount? new Vector2(-16, 0) : new Vector2(48, 0);



                textNickname.text = nickName;
				// textOuts.text = outs >= 0? $"outs={outs}" : "购买保险中";
				textOuts.text = outs >= 0? $"outs={outs}" : CPErrorCode.LanguageDescription(10298);
			}
		}

		private sealed class InsuranceCardItem
		{
			public Transform trans;
			private ReferenceCollector rc;
			private Image imageOnSelect;
			private Image imageInsuranceCard;

            public sbyte CardId { get; set; }

			public GameObject GoInsuranceCard
			{
				get
				{
					return imageInsuranceCard.gameObject;
				}
			}

			public bool IsSelect
			{
				get
				{
					return imageOnSelect.gameObject.activeInHierarchy;
				}
			}

			public InsuranceCardItem(Transform transform)
			{
				trans = transform;
				rc = trans.GetComponent<ReferenceCollector>();
				imageOnSelect = rc.Get<GameObject>("Image_OnSelect").GetComponent<Image>();
				imageInsuranceCard = rc.Get<GameObject>("Image_InsuranceCard").GetComponent<Image>();
			}

			public void UpdateItem(sbyte cardId, bool onSelect)
            {
                CardId = cardId;

                imageInsuranceCard.sprite = GameCache.Instance.CurGame.RcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(cardId));
				imageOnSelect.gameObject.SetActive(onSelect);
			}

			public void OnSelect(bool isSelect)
			{
				imageOnSelect.gameObject.SetActive(isSelect);
			}

		}

		public sealed class WrapTriggedInsuranceData
		{
            public sbyte subPot;
            public List<int> outs;
            public float odd;
			public int leastAmount;
			public int mostAmount;
			public int secureAmount;
			public sbyte potAllowOutSelection;
			public int potTotalCost;
            public int myChipsInPool;

            public int pot;

            public string userNames;
            public List<int> outsPerUser;
            public List<sbyte> playerCards;

        }

        private sealed class WrapPlayerData
        {
            public string name;
            public int outsPerUser;
            public List<sbyte> playerCards;
        }

        private InsuranceData data;

		private ReferenceCollector rc;
		private Button buttonBuy;
		private Button buttonDelay;
		private Button buttonCancel;
		private Button buttonMin;
		private Button buttonAll;
		private Transform transPlayers;
		private Transform transPlayer;
        private Transform transPlayerMine;
        private Image imagePublicCard0;
		private Image imagePublicCard1;
		private Image imagePublicCard2;
		private Image imagePublicCard3;
		private Image imagePublicCard4;
		private Transform transInsuranceCards;
		private Transform transInsuranceCard;
		private Text textPot;
		private Text textMainPut;
		private Text textInsuranceValue;
		private Text textPayValue;
		private Text textOuts;
		private Text textOdds;
        private Text textTips;
		private Slider sliderInsuranceValue;

		private List<Image> listCards;
		private List<PlayerItem> listPlayerItems;
		private List<InsuranceCardItem> listInsuranceCardItems;

        private WrapTriggedInsuranceData myWrapTriggedInsuranceData;

        private int selectOuts;
        public int countDownTime { get; set; }  // 剩余时间  秒
        public int addTimeCount { get; set; }  // 加时次数
        private bool isCountdown;
        private float sendInterval = 1f;
        private float recordDeltaTime;

        public void Awake()
		{
			rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			buttonBuy = rc.Get<GameObject>("Button_Buy").GetComponent<Button>();
			buttonDelay = rc.Get<GameObject>("Button_Delay").GetComponent<Button>();
			buttonCancel = rc.Get<GameObject>("Button_Cancel").GetComponent<Button>();
			buttonMin = rc.Get<GameObject>("Button_Min").GetComponent<Button>();
			buttonAll = rc.Get<GameObject>("Button_All").GetComponent<Button>();
			transPlayers = rc.Get<GameObject>("Players").transform;
			transPlayer = rc.Get<GameObject>("Player").transform;
            transPlayerMine = rc.Get<GameObject>("Player_Mine").transform;
            imagePublicCard0 = rc.Get<GameObject>("Image_PublicCard0").GetComponent<Image>();
			imagePublicCard1 = rc.Get<GameObject>("Image_PublicCard1").GetComponent<Image>();
			imagePublicCard2 = rc.Get<GameObject>("Image_PublicCard2").GetComponent<Image>();
			imagePublicCard3 = rc.Get<GameObject>("Image_PublicCard3").GetComponent<Image>();
			imagePublicCard4 = rc.Get<GameObject>("Image_PublicCard4").GetComponent<Image>();
			transInsuranceCards = rc.Get<GameObject>("InsuranceCards").transform;
			transInsuranceCard = rc.Get<GameObject>("InsuranceCard").transform;
			textPot = rc.Get<GameObject>("Text_Pot").GetComponent<Text>();
			textMainPut = rc.Get<GameObject>("Text_MainPut").GetComponent<Text>();
			textInsuranceValue = rc.Get<GameObject>("Text_InsuranceValue").GetComponent<Text>();
			textPayValue = rc.Get<GameObject>("Text_PayValue").GetComponent<Text>();
			textOuts = rc.Get<GameObject>("Text_Outs").GetComponent<Text>();
			textOdds = rc.Get<GameObject>("Text_Odds").GetComponent<Text>();
            textTips = rc.Get<GameObject>("Text_tips").GetComponent<Text>();
            sliderInsuranceValue = rc.Get<GameObject>("Slider_InsuranceValue").GetComponent<Slider>();

			UIEventListener.Get(buttonBuy.gameObject).onClick = onClickBuy;
			UIEventListener.Get(buttonDelay.gameObject).onClick = onClickDelay;
			UIEventListener.Get(buttonCancel.gameObject).onClick = onClickCancel;
			UIEventListener.Get(buttonMin.gameObject).onClick = onClickMin;
			UIEventListener.Get(buttonAll.gameObject).onClick = onClickAll;

			sliderInsuranceValue.onValueChanged.AddListener(onValueChangedInsuranceValue);

			if (null == listCards)
				listCards = new List<Image>();
			if (listCards.Count > 0)
				listCards.Clear();
			listCards.Add(imagePublicCard0);
			listCards.Add(imagePublicCard1);
			listCards.Add(imagePublicCard2);
			listCards.Add(imagePublicCard3);
			listCards.Add(imagePublicCard4);

			if (null == listPlayerItems)
				listPlayerItems = new List<PlayerItem>();
			if(listPlayerItems.Count > 0)
				listPlayerItems.Clear();

			if (null == listInsuranceCardItems)
				listInsuranceCardItems = new List<InsuranceCardItem>();
			if(listInsuranceCardItems.Count > 0)
				listInsuranceCardItems.Clear();

            registerHandler();
        }

		private void onValueChangedInsuranceValue(float arg0)
		{
            float mTmpOdd = SelectedOdd();

            if (GameCache.Instance.roomMode > 0)
            {
                textPayValue.text = ((int)Math.Floor(mTmpOdd * arg0)).ToString();
                textInsuranceValue.text = ((int)arg0).ToString();
            }
            else
            {
                textPayValue.text = StringHelper.ShowGold((int)Math.Floor(mTmpOdd * arg0));
                textInsuranceValue.text = StringHelper.ShowGold((int)arg0);
            }
           
            UpdateOuts();

            if (arg0 == CurrentSecureAmount())
            {
                HighlightMinBtn();
            }
            else if (arg0 == CurrentMostAmount())
            {
                HighlightAllBtn();
            }
            else
            {
                UnHighlighTwoBtn();
            }
        }

		private void onClickAll(GameObject go)
		{
            sliderInsuranceValue.value = CurrentMostAmount();
            if (GameCache.Instance.roomMode > 0)
            {
                sliderInsuranceValue.value = CurrentMostAmount() / 100;
            }
            HighlightAllBtn();
        }

        private void onClickMin(GameObject go)
        {
            sliderInsuranceValue.value = CurrentSecureAmount();
            if (GameCache.Instance.roomMode > 0)
            {
                sliderInsuranceValue.value = CurrentMostAmount() / 100;
            }
            HighlightMinBtn();
        }

        private void HighlightMinBtn()
        {
            buttonAll.gameObject.transform.Find("Image").GetComponent<Image>().sprite = rc.Get<Sprite>("hand_button_quandichi_normal");
            buttonAll.gameObject.transform.Find("Text").GetComponent<Text>().color = new Color32(168, 168, 168, 255);
            buttonMin.gameObject.transform.Find("Image").GetComponent<Image>().sprite = rc.Get<Sprite>("hand_button_baoben_pressed");
            buttonMin.gameObject.transform.Find("Text").GetComponent<Text>().color = new Color32(233, 191, 128, 255);
        }

        private void HighlightAllBtn()
        {
            buttonAll.gameObject.transform.Find("Image").GetComponent<Image>().sprite = rc.Get<Sprite>("hand_button_quandich_pressed");
            buttonAll.gameObject.transform.Find("Text").GetComponent<Text>().color = new Color32(233, 191, 128, 255);
            buttonMin.gameObject.transform.Find("Image").GetComponent<Image>().sprite = rc.Get<Sprite>("hand_button_baoben_normal");
            buttonMin.gameObject.transform.Find("Text").GetComponent<Text>().color = new Color32(168, 168, 168, 255);
        }

        private void UnHighlighTwoBtn()
        {
            buttonAll.gameObject.transform.Find("Image").GetComponent<Image>().sprite = rc.Get<Sprite>("hand_button_quandichi_normal");
            buttonAll.gameObject.transform.Find("Text").GetComponent<Text>().color = new Color32(168, 168, 168, 255);
            buttonMin.gameObject.transform.Find("Image").GetComponent<Image>().sprite = rc.Get<Sprite>("hand_button_baoben_normal");
            buttonMin.gameObject.transform.Find("Text").GetComponent<Text>().color = new Color32(168, 168, 168, 255);
        }

        private void onClickCancel(GameObject go)
		{
            if (sliderInsuranceValue.minValue > 0)
            {
                // ETHotfix.UIComponent.Instance.Toast($"由于你已买转牌保险，为保障你的利益，河牌保险强制购买。已自动为你背保{sliderInsuranceValue.minValue}（本次无法选牌）");
                ETHotfix.UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(20053, new List<object>(){ sliderInsuranceValue.minValue }));
                return;
            }
            Game.Scene.GetComponent<CPGameSessionComponent>().HotfixSession.Send(new REQ_BUY_INSURANCE()
			{
					roomID = GameCache.Instance.room_id,
					roomPath = GameCache.Instance.room_path,
					seatId = GameCache.Instance.CurGame.MainPlayer.seatID,
					buy = 0,
					pots = new List<sbyte>(),
					potInsureAmount = new List<int>(),
					insuredCards = new List<int>()
			});

            UIComponent.Instance.HideNoAnimation(UIType.UIInsurance);
        }

		private void onClickDelay(GameObject go)
		{
            int fee = Convert.ToInt32(10 * Math.Pow(2, addTimeCount + 1));
            Game.Scene.GetComponent<CPGameSessionComponent>().HotfixSession.Send(new REQ_INSURANCE_ADD_TIME()
            {
                    roomID = GameCache.Instance.room_id,
                    roomPath =  GameCache.Instance.room_path,
                    time = 20,
                    costDiamonds = 0,
                    seatId = GameCache.Instance.CurGame.MainPlayer.seatID,
                    coins = fee
            });


		}

        private void UpdateDelayButton()
        {
            int fee = Convert.ToInt32(10 * Math.Pow(2, addTimeCount + 1));
            buttonDelay.gameObject.transform.Find("Text_delay_bean").GetComponent<Text>().text = $"{StringHelper.ShowGold(fee)}";

        }

        private void onClickBuy(GameObject go)
        {
            List<int> mInsuredCards = new List<int>();

            InsuranceCardItem mInsuranceCardItem = null;
            for (int i = 0, n = listInsuranceCardItems.Count; i < n; i++)
            {
                mInsuranceCardItem = listInsuranceCardItems[i];
                if (null == mInsuranceCardItem)
                    continue;

                if (!mInsuranceCardItem.IsSelect)
                    continue;

                mInsuredCards.Add(mInsuranceCardItem.CardId);
            }

            if (mInsuredCards.Count == 0)             {                 // UIComponent.Instance.Toast($"请选择要投保的牌");                 UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10299));                 return;             }


            if (sliderInsuranceValue.value <= 0)             {                 // UIComponent.Instance.Toast($"投保额要大于0");                 UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10324));                 return;             }

            int butnum = Convert.ToInt32(sliderInsuranceValue.value);
            if (GameCache.Instance.roomMode > 0)
            {
                butnum = butnum * 100;
            }

            Game.Scene.GetComponent<CPGameSessionComponent>().HotfixSession.Send(new REQ_BUY_INSURANCE()
            {
                    roomID = GameCache.Instance.room_id,
                    roomPath = GameCache.Instance.room_path,
                    seatId = GameCache.Instance.CurGame.MainPlayer.seatID,
                    buy = 1,
                    pots = new List<sbyte>() { myWrapTriggedInsuranceData.subPot },
                    potInsureAmount = new List<int>() { butnum },
                    insuredCards = mInsuredCards
            });

            switch (data.existEqualOuts)
            {
                case 0:
                    //转牌
                    break;
                case 1:
                    //河牌存在平分
                    ETHotfix.UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10325));
                    break;
                case 2:
                    //河牌不存在平分

                    break;
                default:
                    break;
            }

            UIComponent.Instance.HideNoAnimation(UIType.UIInsurance);
        }

		public override void OnShow(object obj)
		{
			if (null == obj)
				return;

			data = obj as InsuranceData;
			if (null == data)
				return;

            myWrapTriggedInsuranceData = data.triggedData[0];

            UpdatePublicCards();
			UpdatePlayers();
            InsuranceCards();

           

            UpdateInsuranceSlider();
            sliderInsuranceValue.value = myWrapTriggedInsuranceData.secureAmount;
            if (GameCache.Instance.roomMode > 0)
            {
                sliderInsuranceValue.value = myWrapTriggedInsuranceData.secureAmount / 100;
            }

            float mTmpOdd = SelectedOdd();
            textOdds.text = $"1:{mTmpOdd}";
            textPot.text = StringHelper.ShowGold(myWrapTriggedInsuranceData.pot);
            textMainPut.text = StringHelper.ShowGold(myWrapTriggedInsuranceData.potTotalCost);

            if (GameCache.Instance.roomMode > 0)
            {
                textInsuranceValue.text =((int)sliderInsuranceValue.value).ToString();
                textPayValue.text = ((int)Math.Floor(mTmpOdd * sliderInsuranceValue.value)).ToString();
            }
            else
            {
                textInsuranceValue.text = StringHelper.ShowGold(myWrapTriggedInsuranceData.secureAmount);
                textPayValue.text = StringHelper.ShowGold((int)Math.Floor(mTmpOdd * myWrapTriggedInsuranceData.secureAmount));
            }


          

            recordDeltaTime = Time.time;
            isCountdown = true;
            addTimeCount = data.delayTimes;
            countDownTime = data.timeLeft;
            if (countDownTime < 0)
                countDownTime = 0;
            buttonBuy.gameObject.transform.Find("Text").GetComponent<Text>().text = $"{CPErrorCode.LanguageDescription(10326)}{countDownTime}s";
            UpdateDelayButton();
        }


        public void Update()
        {
            if (!isCountdown)
                return;

            // 如果还没有建立Session直接返回、或者没有到达发包时间
            if (!(Time.time - recordDeltaTime > sendInterval)) return;

            // 记录当前时间
            recordDeltaTime = Time.time;

            countDownTime -= 1;
            if (countDownTime < 0)
                countDownTime = 0;
            // buttonBuy.gameObject.transform.Find("Text").GetComponent<Text>().text = $"购买{countDownTime}s";
            buttonBuy.gameObject.transform.Find("Text").GetComponent<Text>().text = $"{CPErrorCode.LanguageDescription(10326)}{countDownTime}s";

            if (countDownTime <= 0)
            {
                isCountdown = false;
                //自动点不买
                onClickCancel(null);
            }
        }



        public override void OnHide()
		{
            // Log.Debug($"AddClips OnHide");
            isCountdown = false;
        }

		public override void Dispose()
		{
            if (IsDisposed)
            {
                return;
            }
			if (null != listCards)
			{
				listCards.Clear();
				listCards = null;
			}
			if (null != listPlayerItems)
			{
				listPlayerItems.Clear();
				listPlayerItems = null;
			}
			if (null != listInsuranceCardItems)
			{
				listInsuranceCardItems.Clear();
				listInsuranceCardItems = null;
			}

            myWrapTriggedInsuranceData = null;
            data = null;

            removeHandler();

			base.Dispose();
		}

        #region DataHelper
        private int CurrentMostAmount()
        {
            if (selectOuts == myWrapTriggedInsuranceData.outs.Count)
            {
                return myWrapTriggedInsuranceData.mostAmount;
            }
            double amount1 = Math.Ceiling(myWrapTriggedInsuranceData.mostAmount * UnSelectedOdd() / (1 + UnSelectedOdd()));
            double amount2 = Math.Floor(myWrapTriggedInsuranceData.pot / SelectedOdd()); // 不可以超过分池的池底
            return Convert.ToInt32(Math.Max(1, Math.Min(amount1, amount2)));
        }

        private int CurrentSecureAmount()
        {
            if (selectOuts == myWrapTriggedInsuranceData.outs.Count)
            {
                return myWrapTriggedInsuranceData.secureAmount;
            }
            else
            {
                return Convert.ToInt16(Math.Ceiling(myWrapTriggedInsuranceData.potTotalCost / SelectedOdd()));
            }
        }

        private float UnSelectedOdd()
        {
            //todo 区分奥马哈
            RoomPath mRoomPath = (RoomPath) GameCache.Instance.room_path;
            switch (mRoomPath)
            {
                case RoomPath.Normal:
                    return GameUtil.GetNormalOuts(myWrapTriggedInsuranceData.outs.Count - selectOuts);
                    break;
                case RoomPath.Omaha:
                    return GameUtil.GetOmahaOuts(myWrapTriggedInsuranceData.outs.Count - selectOuts);
                    break;
                case RoomPath.NormalAof:
                    return GameUtil.GetNormalOuts(myWrapTriggedInsuranceData.outs.Count - selectOuts);
                    break;
                case RoomPath.OmahaAof:
                    return GameUtil.GetOmahaOuts(myWrapTriggedInsuranceData.outs.Count - selectOuts);
                    break;
                case RoomPath.DP:
                case RoomPath.DPAof:
                    return GameUtil.GetShortOuts(myWrapTriggedInsuranceData.outs.Count - selectOuts);
                    break;

            }

            return GameUtil.GetNormalOuts(myWrapTriggedInsuranceData.outs.Count - selectOuts);
        }

        private float SelectedOdd()
        {
            RoomPath mRoomPath = (RoomPath)GameCache.Instance.room_path;
            switch (mRoomPath)
            {
                case RoomPath.Normal:
                    return GameUtil.GetNormalOuts(selectOuts);
                    break;
                case RoomPath.Omaha:
                    return GameUtil.GetOmahaOuts(selectOuts);
                    break;
                case RoomPath.NormalAof:
                    return GameUtil.GetNormalOuts(selectOuts);
                    break;
                case RoomPath.OmahaAof:
                    return GameUtil.GetOmahaOuts(selectOuts);
                    break;
                case RoomPath.DP:
                case RoomPath.DPAof:
                    return GameUtil.GetShortOuts(selectOuts);
                    break;
            }

            //todo 区分奥马哈
            return GameUtil.GetNormalOuts(selectOuts);
        }
        #endregion


        /// <summary>
        /// 刷新公共牌
        /// </summary>
        private void UpdatePublicCards()
		{
			if (null == data.cards)
				return;

			for (int i = 0, n = data.cards.Count; i < n; i++)
			{
				listCards[i].sprite = GameCache.Instance.CurGame.RcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(data.cards[i]));
                listCards[i].gameObject.SetActive(data.cards[i] >= 0);
			}

			for (int i = data.cards.Count, n = listCards.Count; i < n; i++)
			{
				listCards[i].gameObject.SetActive(false);
			}
		}

		private void UpdatePlayers()
        {
            if (data.triggedData.Count == 0)
                return;

            List<WrapPlayerData> mList = new List<WrapPlayerData>();
            WrapPlayerData mWrapTriggedPlayerData = null;
            WrapTriggedInsuranceData mWrapTriggedInsuranceData = myWrapTriggedInsuranceData;
            string[] mNames = mWrapTriggedInsuranceData.userNames.Split(new string[] { "@%" }, StringSplitOptions.None);
            for (int i = 0, n = mNames.Length; i < n; i++)
            {
                mWrapTriggedPlayerData = new WrapPlayerData();
                mWrapTriggedPlayerData.name = mNames[i];
                mWrapTriggedPlayerData.outsPerUser = mWrapTriggedInsuranceData.outsPerUser[i];
                if (null == mWrapTriggedPlayerData.playerCards)
                    mWrapTriggedPlayerData.playerCards = new List<sbyte>();

                int mHandCards = GameCache.Instance.CurGame.HandCards;
                for (int j = 0; j < mHandCards; j++)
                {
                    mWrapTriggedPlayerData.playerCards.Add(mWrapTriggedInsuranceData.playerCards[i * mHandCards + j]);
                }

                mList.Add(mWrapTriggedPlayerData);
            }

            mWrapTriggedPlayerData = new WrapPlayerData();
            mWrapTriggedPlayerData.name = GameCache.Instance.CurGame.MainPlayer.nick;
            mWrapTriggedPlayerData.outsPerUser = -1;
            mWrapTriggedPlayerData.playerCards = GameCache.Instance.CurGame.MainPlayer.cards;
            mList.Insert(0, mWrapTriggedPlayerData);

            if (null == listPlayerItems)
				listPlayerItems = new List<PlayerItem>();

			PlayerItem mPlayerItem = null;
			GameObject mGo = null;
			int mNewStart = 0;
			int mNewEnd = 0;
			int mUpdateStart = 0;
			int mUpdateEnd = 0;
			int mHideStart = 0;
			int mHideEnd = 0;

			if (mList.Count > listPlayerItems.Count)
			{
				mUpdateStart = 0;
				mUpdateEnd = listPlayerItems.Count;
				mNewStart = listPlayerItems.Count == 0 ? 0 : mUpdateEnd;
				mNewEnd = mList.Count;
			}
			else if(mList.Count < listPlayerItems.Count)
			{
				mUpdateStart = 0;
				mUpdateEnd = mList.Count;
				mHideStart = mUpdateEnd;
				mHideEnd = listPlayerItems.Count;
			}
			else
			{
				mUpdateStart = 0;
				mUpdateEnd = mList.Count;
			}

			for (int i = mUpdateStart; i < mUpdateEnd; i++)
			{
				mPlayerItem = listPlayerItems[i];
				mWrapTriggedPlayerData = mList[i];
                mPlayerItem.UpdateItem(mWrapTriggedPlayerData.playerCards, mWrapTriggedPlayerData.name, mWrapTriggedPlayerData.outsPerUser);
                    
				mPlayerItem.trans.gameObject.SetActive(true);
			}

			for (int i = mNewStart; i < mNewEnd; i++)
			{
                if (i == 0)
                {
                    mGo = transPlayerMine.gameObject;
                }
                else
                {
                    mGo = GameObject.Instantiate(transPlayer.gameObject, transPlayers);
                    mGo.transform.localPosition = Vector3.zero;
                    mGo.transform.localRotation = Quaternion.identity;
                    mGo.transform.localScale = Vector3.one;
                }
				mPlayerItem = new PlayerItem(mGo.transform);
				mWrapTriggedPlayerData = mList[i];
                mPlayerItem.UpdateItem(mWrapTriggedPlayerData.playerCards, mWrapTriggedPlayerData.name, mWrapTriggedPlayerData.outsPerUser);
                    
				mPlayerItem.trans.gameObject.SetActive(true);
				listPlayerItems.Add(mPlayerItem);
			}

			for (int i = mHideStart; i < mHideEnd; i++)
			{
				mPlayerItem = listPlayerItems[i];
				mPlayerItem.trans.gameObject.SetActive(false);
			}
		}

		private void InsuranceCards()
		{
			if (null == listInsuranceCardItems)
				listInsuranceCardItems = new List<InsuranceCardItem>();

            WrapTriggedInsuranceData mWrapTriggedInsuranceData = myWrapTriggedInsuranceData;

            InsuranceCardItem mInsuranceCardItem = null;
			GameObject mGo = null;
			int mNewStart = 0;
			int mNewEnd = 0;
			int mUpdateStart = 0;
			int mUpdateEnd = 0;
			int mHideStart = 0;
			int mHideEnd = 0;

			if (mWrapTriggedInsuranceData.outs.Count > listInsuranceCardItems.Count)
			{
				mUpdateStart = 0;
				mUpdateEnd = listInsuranceCardItems.Count;
				mNewStart = listInsuranceCardItems.Count == 0 ? 0 : mUpdateEnd;
				mNewEnd = mWrapTriggedInsuranceData.outs.Count;
			}
			else if (mWrapTriggedInsuranceData.outs.Count < listInsuranceCardItems.Count)
			{
				mUpdateStart = 0;
				mUpdateEnd = mWrapTriggedInsuranceData.outs.Count;
				mHideStart = mUpdateEnd;
				mHideEnd = listInsuranceCardItems.Count;
			}
			else
			{
				mUpdateStart = 0;
				mUpdateEnd = mWrapTriggedInsuranceData.outs.Count;
			}

			for (int i = mUpdateStart; i < mUpdateEnd; i++)
			{
                
				mInsuranceCardItem = listInsuranceCardItems[i];
				mInsuranceCardItem.UpdateItem((sbyte)mWrapTriggedInsuranceData.outs[i], true);
				mInsuranceCardItem.trans.gameObject.SetActive(true);
			}

			for (int i = mNewStart; i < mNewEnd; i++)
			{
				mGo = GameObject.Instantiate(transInsuranceCard.gameObject, transInsuranceCards);
				mGo.name = i.ToString();
				mGo.transform.localPosition = Vector3.zero;
				mGo.transform.localRotation = Quaternion.identity;
				mGo.transform.localScale = Vector3.one;
				mInsuranceCardItem = new InsuranceCardItem(mGo.transform);
				mInsuranceCardItem.UpdateItem((sbyte)mWrapTriggedInsuranceData.outs[i], true);
				mInsuranceCardItem.trans.gameObject.SetActive(true);
				UIEventListener.Get(mInsuranceCardItem.GoInsuranceCard).onClick = onClickInSuranceCard;
				listInsuranceCardItems.Add(mInsuranceCardItem);
			}

			for (int i = mHideStart; i < mHideEnd; i++)
			{
				mInsuranceCardItem = listInsuranceCardItems[i];
				mInsuranceCardItem.trans.gameObject.SetActive(false);
			}

            selectOuts = myWrapTriggedInsuranceData.outs.Count;
			UpdateOuts();
		}

		private void onClickInSuranceCard(GameObject go)
		{
            return;

            if (myWrapTriggedInsuranceData.potAllowOutSelection == 1)
            {
                int mIndex = Convert.ToInt32(go.transform.parent.name);
                InsuranceCardItem mInsuranceCardItem = listInsuranceCardItems[mIndex];
                if (mInsuranceCardItem.IsSelect && selectOuts <= 1)//至少选择一个outs
                {
                    return;
                }
                mInsuranceCardItem.OnSelect(!mInsuranceCardItem.IsSelect);
                if (mInsuranceCardItem.IsSelect)
                    selectOuts++;
                else
                    selectOuts--;
                UpdateOuts();

                UpdateInsuranceSlider();
            }

        }

		private void UpdateOuts()
        {
			textOuts.text = $"{selectOuts}/{myWrapTriggedInsuranceData.outs.Count}";
            textOdds.text = $"1:{SelectedOdd()}";

            if (GameCache.Instance.roomMode > 0)
            {
                textPayValue.text = ((int)Math.Floor(SelectedOdd() * sliderInsuranceValue.value)).ToString();
            }
            else
            {
                textPayValue.text = StringHelper.ShowGold((int)Math.Floor(SelectedOdd() * sliderInsuranceValue.value));
            }

           

            if (myWrapTriggedInsuranceData.potAllowOutSelection == 1)
            {
                if (selectOuts != myWrapTriggedInsuranceData.outs.Count)
                {
                    int autoInsured = Convert.ToInt32(Math.Ceiling(Convert.ToInt32(sliderInsuranceValue.value) / UnSelectedOdd()));
                    // textTips.text = $"强制背保：未选中OUTS {myWrapTriggedInsuranceData.outs.Count - selectOuts}张，赔率{UnSelectedOdd()}，自动投保额{autoInsured}";
                    textTips.text = CPErrorCode.LanguageDescription(20034, new List<object>(){(myWrapTriggedInsuranceData.outs.Count - selectOuts), UnSelectedOdd(), autoInsured });
                }
                else
                {
                    textTips.text = "";
                }
            }
            else
            {
                // textTips.text = "这一轮你必须购买所有OUTS";
                textTips.text = CPErrorCode.LanguageDescription(20035);
            }
        }

        private void UpdateInsuranceSlider()
        {
            if (GameCache.Instance.roomMode > 0)
            {
                sliderInsuranceValue.minValue = myWrapTriggedInsuranceData.leastAmount / 100;
                sliderInsuranceValue.maxValue = CurrentMostAmount() / 100;
                if (sliderInsuranceValue.value > CurrentMostAmount())
                {
                    sliderInsuranceValue.value = CurrentMostAmount() / 100;
                }
            }
            else
            {
                sliderInsuranceValue.minValue = myWrapTriggedInsuranceData.leastAmount;
                sliderInsuranceValue.maxValue = CurrentMostAmount();
                if (sliderInsuranceValue.value > CurrentMostAmount())
                {
                    sliderInsuranceValue.value = CurrentMostAmount();
                }
            }
        }

        private void HANDLER_REQ_INSURANCE_ADD_TIME(ICPResponse response)
        {
            REQ_INSURANCE_ADD_TIME rec = response as REQ_INSURANCE_ADD_TIME;
            if (null == rec)
                return;

            if (rec.playerId == GameCache.Instance.CurGame.MainPlayer.userID)
            {
                if (rec.status == 0)
                {
                    GameCache.Instance.gold -= rec.coastCoins;
                    GameCache.Instance.idou -= rec.coastJewel;
                    addTimeCount += 1;
                    countDownTime += rec.addTime;
                    isCountdown = true;
                    UpdateDelayButton();
                }
                else if (rec.status == 2)
                {
                    // Game.Scene.GetComponent<UIComponent>().Show(UIType.UIToast, $"钻石不足", null, 0);
                    UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10102));
                }
                else
                {
                    // Game.Scene.GetComponent<UIComponent>().Show(UIType.UIToast, $"error", null, 0);
                    UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10052));
                }
            }
        }

        private void registerHandler()
        {
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_INSURANCE_ADD_TIME, HANDLER_REQ_INSURANCE_ADD_TIME);
        }

        private void removeHandler()
        {
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_INSURANCE_ADD_TIME, HANDLER_REQ_INSURANCE_ADD_TIME);
        }

        public void PauseCountDown()
        {
           isCountdown = false;
        }

        public void ContinueCountDown()
        {
            isCountdown = true;
        }
    }
}
