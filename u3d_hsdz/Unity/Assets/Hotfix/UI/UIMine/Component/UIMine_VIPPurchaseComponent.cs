using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_VIPPurchaseComponentSystem : AwakeSystem<UIMine_VIPPurchaseComponent>
    {
        public override void Awake(UIMine_VIPPurchaseComponent self)
        {
            self.Awake();
        }
    }

    public enum BuyTimeType
    {
        Month,
        Year,
    }

    public class UIMine_VIPPurchaseComponent : UIBaseComponent
    {

        public sealed class PurchaseData
        {
            public int remain_fee;
            public int remain_days;
            public VIPType myVIPType;
            public VIPType selectedVIPType;
            public BuyTimeType buyTimeType;
            public PurchaseSuccessDelegate successDelegate;
        }

        private PurchaseData purchaseData;
        public delegate void PurchaseSuccessDelegate();

        private int[,] priceArr = new int[2, 3]
        {
            {3000, 5000, 9900},
            {32000, 51000, 95000},
        };
        private int selectTime;

        ReferenceCollector rc;
        GameObject vip_card;
        Text text_top_tips;
        GameObject view_buy_cards_grip;
        GameObject btn_buy_card;
        Button btn_confirm;

        Transform view_purchase_success;
        Text text_success_VIP;
        Button btn_check_right;
        Button Button_close;
        Text text_success_endTime;
        Image Image_vip_type;

        public void Awake()
        {
            InitUI();

        }

        public override void OnShow(object obj)
        {
            Log.Debug($"onshow");

            SetUpNav(LanguageManager.Get("UIMine_VIPPurchase_title"), UIType.UIMine_VIPPurchase);

            if (null != obj)
            {
                purchaseData = obj as PurchaseData;

            }

            //vip卡片
            if (purchaseData.myVIPType == purchaseData.selectedVIPType)
            {
                string endDate = TimeHelper.GetDateTimer(long.Parse(GameCache.Instance.vipEndDate)).ToString("yyyy-MM-dd");
                vip_card.transform.Find("Text").GetComponent<Text>().text = $"{LanguageManager.Get("UIMine_VIP_expire")}{endDate}";
            }
            else
            {
                vip_card.transform.Find("Text").GetComponent<Text>().text = LanguageManager.Get("UIMine_VIP_openVIP");
            }

            Image vipImage = vip_card.GetComponent<Image>();
            if (purchaseData.selectedVIPType == VIPType.VIP_A)
            {
                vipImage.sprite = rc.Get<Sprite>("vip_card_gold");
            }
            else if (purchaseData.selectedVIPType == VIPType.VIP_B)
            {
                vipImage.sprite = rc.Get<Sprite>("vip_card_platinum");
            }
            else if (purchaseData.selectedVIPType == VIPType.VIP_C)
            {
                vipImage.sprite = rc.Get<Sprite>("vip_card_diamond");
            }

            //购买
            GridLayoutGroup grid = view_buy_cards_grip.GetComponent<GridLayoutGroup>();
            float viewWidth = rc.GetComponent<RectTransform>().rect.width;
            grid.cellSize = new Vector2((viewWidth - 48*2 - 32) / 2, 318);
            for (int i = 0; i < 4; i++)
            {
                GameObject buyCard = GetVIPBuyCard(view_buy_cards_grip.transform);
                string title;
                int price;
                GetCardTitleAndPrice(i, out title, out price);
                buyCard.transform.Find("Text_time").GetComponent<Text>().text = title;
                buyCard.transform.Find("Text_price").GetComponent<Text>().text = $"{price}";
                UIEventListener.Get(buyCard, i).onIntClick = SelectCardAction;
                if ((sbyte)purchaseData.selectedVIPType == GameCache.Instance.vipLevel)
                {
                    buyCard.GetComponent<Button>().enabled = true;
                }
                else
                {
                    if (purchaseData.remain_fee <= price)
                    {
                        buyCard.GetComponent<Button>().enabled = true;
                        buyCard.transform.Find("Text_time").GetComponent<Text>().color = new Color32(233, 191, 128, 255);
                        buyCard.transform.Find("Text_price").GetComponent<Text>().color = new Color32(233, 191, 128, 255);
                        buyCard.transform.Find("Image_bean").GetComponent<Image>().sprite = rc.Get<Sprite>("vip_ziuanshi_huangse");
                    }
                    else
                    {
                        buyCard.GetComponent<Button>().enabled = false;
                        buyCard.transform.Find("Text_time").GetComponent<Text>().color = new Color32(151, 151, 151, 255);
                        buyCard.transform.Find("Text_price").GetComponent<Text>().color = new Color32(151, 151, 151, 255);
                        buyCard.transform.Find("Image_bean").GetComponent<Image>().sprite = rc.Get<Sprite>("vip_ziuanshi_huise");
                    }
                }
            }
            SelectCardAction(null, 0);

            //成功
            if (purchaseData.selectedVIPType == VIPType.VIP_A)
            {
                text_success_VIP.text = LanguageManager.Get("UIMine_VIP_gold") + LanguageManager.Get("UIMine_VIPPurchas_vipName");
                Image_vip_type.sprite = rc.Get<Sprite>("vip_success_gold");
            } 
            else if (purchaseData.selectedVIPType == VIPType.VIP_B)
            {
                text_success_VIP.text = LanguageManager.Get("UIMine_VIP_platinum") + LanguageManager.Get("UIMine_VIPPurchas_vipName");
                Image_vip_type.sprite = rc.Get<Sprite>("vip_success_platinum");
            }
            else if (purchaseData.selectedVIPType == VIPType.VIP_C)
            {
                text_success_VIP.text = LanguageManager.Get("UIMine_VIP_diamond") + LanguageManager.Get("UIMine_VIPPurchas_vipName");
                Image_vip_type.sprite = rc.Get<Sprite>("vip_success_diamond");
            }

        }

        private void GetCardTitleAndPrice(int index, out string title, out int price)
        {
            int[] monthNum = { 1, 3, 6, 9};
            int[] yearNum = { 1, 2, 3, 5};

            int perPrice = priceArr[(int)purchaseData.buyTimeType, (int)purchaseData.selectedVIPType - 1];

            switch (purchaseData.buyTimeType)
            {
                case BuyTimeType.Month:
                    title = $"{monthNum[index]}{LanguageManager.Get("UIMine_VIPPurchase_month")}";
                    price = perPrice * monthNum[index];
                    break;
                case BuyTimeType.Year:
                    title = $"{yearNum[index]}{LanguageManager.Get("UIMine_VIP_year")}";
                    price = perPrice * yearNum[index];
                    break;
                default:
                    title = "";
                    price = 0;
                    break;
            }

        }

        public override void OnHide()
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }

        #region UI
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            vip_card = rc.Get<GameObject>("vip_card");
            text_top_tips = rc.Get<GameObject>("text_top_tips").GetComponent<Text>();
            view_buy_cards_grip = rc.Get<GameObject>("view_buy_cards_grip");
            btn_buy_card = rc.Get<GameObject>("btn_buy_card");
            btn_confirm = rc.Get<GameObject>("btn_confirm").GetComponent<Button>();

            view_purchase_success = rc.Get<GameObject>("view_purchase_success").transform;
            text_success_VIP = rc.Get<GameObject>("text_success_VIP").GetComponent<Text>();
            btn_check_right = rc.Get<GameObject>("btn_check_right").GetComponent<Button>();
            Button_close = rc.Get<GameObject>("Button_close").GetComponent<Button>();
            text_success_endTime = rc.Get<GameObject>("text_success_endTime").GetComponent<Text>();
            Image_vip_type = rc.Get<GameObject>("Image_vip_type").GetComponent<Image>();

            UIEventListener.Get(btn_confirm.gameObject).onClick = ClickConfirmBtn;
            UIEventListener.Get(btn_check_right.gameObject).onClick = ClickCheckRightBtn;
            UIEventListener.Get(Button_close.gameObject).onClick = ClickCheckRightBtn;
        }

        GameObject GetVIPBuyCard(Transform parent)
        {
            GameObject obj = GameObject.Instantiate(btn_buy_card);
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(parent);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            return obj;
        }

        #endregion

        #region Action
        private void SelectCardAction(GameObject go, int index)
        {
            for (int i = 0; i < view_buy_cards_grip.transform.childCount; i++)
            {
                view_buy_cards_grip.transform.GetChild(i).Find("Image_selected").gameObject.SetActive(i == index+1);
            }

            int[] monthNum = { 1, 3, 6, 9 };
            int[] yearNum = { 1, 2, 3, 5 };

            switch (purchaseData.buyTimeType)
            {
                case BuyTimeType.Month:
                    selectTime = monthNum[index];
                    break;
                case BuyTimeType.Year:
                    selectTime = yearNum[index];
                    break;
            }
        }

        //点击确认购买
        private void ClickConfirmBtn(GameObject go)
        {
            WEB2_user_vip_setting.RequestData requestData = new WEB2_user_vip_setting.RequestData()
            {
                cardNum = selectTime,
                cardType = (int)purchaseData.buyTimeType + 1,
                vipLevel = (int)purchaseData.selectedVIPType,
            };
            HttpRequestComponent.Instance.Send(
                WEB2_user_vip_setting.API,
                WEB2_user_vip_setting.Request(requestData),
                this.OpenVIPPurchase);
        }

        private void OpenVIPPurchase(string resData)
        {
            Log.Debug(resData);
            WEB2_user_vip_setting.ResponseData responseData = WEB2_user_vip_setting.Response(resData);
            if (responseData.status == 0)
            {
                GameCache.Instance.vipLevel = (sbyte)purchaseData.selectedVIPType;
                GameCache.Instance.vipEndDate = $"{responseData.data.vipEndDate}";

                string endDate = TimeHelper.GetDateTimer(long.Parse(GameCache.Instance.vipEndDate)).ToString("yyyy-MM-dd");
                text_success_endTime.text = $"{LanguageManager.Get("UIMine_VIP_expire")}{endDate}";

                purchaseData.successDelegate();
                view_purchase_success.gameObject.SetActive(true);
            }
            else
            {
                string errorDes = CPErrorCode.ErrorDescription(responseData.status);
                UIComponent.Instance.Toast(errorDes);
            }
        }

        private void ClickCheckRightBtn(GameObject go)
        {
            Game.Scene.GetComponent<UIComponent>().RemoveAnimated(UIType.UIMine_VIPPurchase);
        }
        #endregion
    }
}
