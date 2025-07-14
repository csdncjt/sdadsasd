using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	[ObjectSystem]
	public class UiAddClipsComponentSystem : AwakeSystem<UIAddChipsComponent>
	{
		public override void Awake(UIAddChipsComponent self)
		{
			self.Awake();
		}
	}

	public class UIAddChipsComponent : UIBaseComponent
	{
		public sealed class AddClipsData
		{
			public int bigBlind { get; set; } // 大盲
			public int smallBlind { get; set; } // 小盲
            public int qianzhu { get; set; }//前注
			public int currentMinRate { get; set; } // 当前最小带入倍数
			public int currentMaxRate { get; set; } // 当前最大带入倍数
			public int totalCoin { get; set; } // 总USDT
			public int tableChips { get; set; } // 玩家剩余USDT
            public int isNeedCost { get; set; }//是否需要消耗
		}

		private AddClipsData addClipsData;

		private ReferenceCollector rc;
		private Slider sliderCoin;
		private Button buttonCommit;
		private Button buttonClose;
		private Text textBlind;
		private Text textCoin;
		private Text textTotalCoin;
		private Text textNeedCoin;
        private Text Text_BlindTip;
        private Text Text_CoinTip;
        private Text Text_QianzhuTip;
        private Text Text_qianzhu;
        private int carryGold;


		public void Awake()
		{
			rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			sliderCoin = rc.Get<GameObject>("Slider_Coin").GetComponent<Slider>();
			buttonCommit = rc.Get<GameObject>("Button_Commit").GetComponent<Button>();
			buttonClose = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
			textBlind = rc.Get<GameObject>("Text_Blind").GetComponent<Text>();
			textCoin = rc.Get<GameObject>("Text_Coin").GetComponent<Text>();
			textTotalCoin = rc.Get<GameObject>("Text_TotalCoin").GetComponent<Text>();
            textNeedCoin = rc.Get<GameObject>("Text_NeedCoin").GetComponent<Text>();

            Text_BlindTip = rc.Get<GameObject>("Text_BlindTip").GetComponent<Text>();
            Text_CoinTip = rc.Get<GameObject>("Text_CoinTip").GetComponent<Text>();
            Text_QianzhuTip = rc.Get<GameObject>("Text_QianzhuTip").GetComponent<Text>();
            Text_qianzhu = rc.Get<GameObject>("Text_qianzhu").GetComponent<Text>();

            UIEventListener.Get(this.buttonCommit.gameObject).onClick = onClickCommit;
			UIEventListener.Get(this.buttonClose.gameObject).onClick = onClickClose;

			sliderCoin.onValueChanged.AddListener(onValueChangedSliderCoin);

			registerHandler();
		}

		private void onValueChangedSliderCoin(float arg0)
		{
            carryGold = Convert.ToInt32(arg0 * GameCache.Instance.carry_small);

            if (addClipsData.isNeedCost == 0)
                this.textNeedCoin.text = "0";
            else
                this.textNeedCoin.text = StringHelper.GetShortString(carryGold);

            this.textCoin.text = StringHelper.GetShortString(carryGold);
            if(arg0 * GameCache.Instance.carry_small * 100 > GameCache.Instance.gold)
            {
                this.textNeedCoin.color = new Color32(184, 43, 48, 255);
            }
            else
            {
                this.textNeedCoin.color = new Color32(233, 191, 128, 255);
            }
        }

		private void onClickCommit(GameObject go)
		{
			GameCache.Instance.CurGame.AddChips(carryGold);
		}

		private void onClickClose(GameObject go)
        {
            UIComponent.Instance.HideNoAnimation(UIType.UIAddChips);
            // 占座才要调站起
            if (GameCache.Instance.CurGame.MainPlayer.status == 18)
            {
                GameCache.Instance.CurGame.Standup();
            }
		}

		public override void OnShow(object obj)
        {
            
			if (null != obj)
			{
				addClipsData = obj as AddClipsData;
				if (null != addClipsData)
				{
                    string str1 = StringHelper.ShowGold(addClipsData.smallBlind, false);
                    string str2 = StringHelper.ShowGold(addClipsData.bigBlind, false);

                    string str3 = StringHelper.ShowGold(GameCache.Instance.carry_small, false);

                    carryGold = GameCache.Instance.carry_small;

                    textBlind.text = $"{str1}/{str2}";
                    if (GameCache.Instance.room_path == 21 || GameCache.Instance.room_path == 23)//短牌
                    {
                        Text_BlindTip.gameObject.SetActive(false);
                        Text_QianzhuTip.gameObject.SetActive(true);
                        Text_qianzhu.text = StringHelper.ShowGold(addClipsData.qianzhu, false);
                    }
                    else
                    {
                        Text_BlindTip.gameObject.SetActive(true);
                        Text_QianzhuTip.gameObject.SetActive(false);
                    }
					textCoin.text = $"{str3}";
					textTotalCoin.text = StringHelper.ShowGold(addClipsData.totalCoin, true);

                    if (addClipsData.isNeedCost == 1)
                        textNeedCoin.text = StringHelper.ShowGold(Convert.ToInt32(addClipsData.currentMinRate * GameCache.Instance.carry_small), false);
                    else
                        textNeedCoin.text = "0";

                    int currentMaxBring = (addClipsData.currentMaxRate + 1) * GameCache.Instance.carry_small - addClipsData.tableChips;
                    int maxRate = currentMaxBring / GameCache.Instance.carry_small;
                    if (maxRate > addClipsData.currentMaxRate)
                    {
                        maxRate = addClipsData.currentMaxRate;
                    }
                    if (maxRate < addClipsData.currentMinRate)
                    {
                        maxRate = addClipsData.currentMinRate;
                    }

                    sliderCoin.maxValue = maxRate;
                    sliderCoin.minValue = addClipsData.currentMinRate;
					sliderCoin.value = addClipsData.currentMinRate;
                    onValueChangedSliderCoin(addClipsData.currentMinRate);
                    sliderCoin.wholeNumbers = true;

				}

                if (GameCache.Instance.roomMode > 0)
                {
                    Text_CoinTip.text = LanguageManager.Get("UITexas_AddChips2");
                }
                else
                {
                    Text_CoinTip.text = LanguageManager.Get("UITexas_AddChips");
                }

                // 显示带入时，获取一下玩家USDT
                UIMineModel.mInstance.ObtainUserInfo(data =>
                {
                    GameCache.Instance.gold = data.chip;
                    addClipsData.totalCoin = data.chip;
                    textTotalCoin.text = StringHelper.ShowGold(addClipsData.totalCoin, true);
                });
            }
		}

		public override void OnHide()
		{
			// Log.Debug($"AddClips OnHide");
		}

		public override void Dispose()
        {
            if (IsDisposed)
                return;
			removeHandler();
			base.Dispose();
		}

		private void registerHandler()
		{

		}

		private void removeHandler()
		{

		}
	}
}
