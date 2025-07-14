using System;
using System.Collections.Generic;
using System.Net;
using BestHTTP;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using xcharts;
using DG.Tweening;
using UnityEngine.EventSystems;
using SuperScrollView;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_VIPComponentSystem : AwakeSystem<UIMine_VIPComponent>
    {
        public override void Awake(UIMine_VIPComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UIMine_VIPComponentUpdateSystem : UpdateSystem<UIMine_VIPComponent>
    {
        public override void Update(UIMine_VIPComponent self)
        {
            self.Update();
        }
    }

    public enum VIPType
    {
        VIP_None,
        VIP_A,
        VIP_B,
        VIP_C,
    }

    public class UIMine_VIPComponent : UIBaseComponent
    {
        private List<GameObject> chartItems;
        private List<WEB2_user_vip_config.DataElement> configList = new List<WEB2_user_vip_config.DataElement>();
        private WEB2_user_vip_my.Data myVIPInfo;
        VIPType selectedVIPType = VIPType.VIP_None;
        VIPType myVIPType;

        bool shouldRefreshTop;

        ReferenceCollector rc;

        GameObject scroll_view_top_cards;
        private LoopListView2 mLoopListView;

        Button btn_vip_buy_month;
        Button btn_vip_buy_year;
        Text Text_price_month;
        Text Text_price_year;

        GameObject img_vip_right_lock_1;
        GameObject img_vip_right_lock_3;
        GameObject img_vip_right_lock_4;

        GameObject chart_vip_right_grip;
        GameObject chart_item;

        public void Awake()
        {
            this.chartItems = new List<GameObject>();
            myVIPType = (VIPType)GameCache.Instance.vipLevel;

            InitUI();

            ObtinVIPConfigInfo();

        }

        //获取VIP特权信息
        void ObtinVIPConfigInfo()
        {
            WEB2_user_vip_config.RequestData requestData = new WEB2_user_vip_config.RequestData()
            {
            };
            HttpRequestComponent.Instance.Send(
                WEB2_user_vip_config.API,
                WEB2_user_vip_config.Request(requestData),
                this.OpenVIPConfigInfo);
        }

        void OpenVIPConfigInfo(string resData)
        {
            ObtainMyVIPInfo();

            Log.Debug(resData);
            WEB2_user_vip_config.ResponseData responseData = WEB2_user_vip_config.Response(resData);
            if (responseData.status == 0)
            {
                configList = responseData.data;
            }
        }

        //查询自己的剩余权利
        void ObtainMyVIPInfo()
        {
            WEB2_user_vip_my.RequestData requestData = new WEB2_user_vip_my.RequestData()
            {
            };
            HttpRequestComponent.Instance.Send(
                WEB2_user_vip_my.API,
                WEB2_user_vip_my.Request(requestData),
                this.OpenMyVIPInfo);
        }


        void OpenMyVIPInfo(string resData)
        {

            Log.Debug(resData);
            WEB2_user_vip_my.ResponseData responseData = WEB2_user_vip_my.Response(resData);
            if (responseData.status == 0)
            {
                myVIPInfo = responseData.data;
                myVIPType = (VIPType)myVIPInfo.vipLevel;
                GameCache.Instance.vipLevel = (sbyte)responseData.data.vipLevel;
                GameCache.Instance.vipEndDate = $"{responseData.data.vipEndDate}";
            }
            else
            {
                myVIPInfo = null;
            }
            UpdateUI();
            if (shouldRefreshTop)
            {
                mLoopListView.RefreshAllShownItem();
                shouldRefreshTop = false;
            }
        }



        public void Update()
        {
            //ScrollRect scrollRect = scroll_view_top_cards.GetComponent<ScrollRect>();
            //Debug.Log($"{scrollRect.content.sizeDelta.x},{scrollRect.content.sizeDelta.y},{scrollRect.content.localPosition.x}");

            mLoopListView.UpdateAllShownItemSnapData();
            int count = mLoopListView.ShownItemCount;
            SetCurrentVIP(mLoopListView.CurSnapNearestItemIndex);
            for (int i = 0; i < count; ++i)
            {
                LoopListViewItem2 item = mLoopListView.GetShownItemByIndex(i);
                if (item.gameObject.name == "view_vip_card(Clone)")
                {
                    float scale = 1 - Mathf.Abs(item.DistanceWithViewPortSnapCenter) / 6000f;
                    scale = Mathf.Clamp(scale, 0.8f, 1);
                    Transform rootObj = item.gameObject.transform.Find("root_obj");
                    rootObj.GetComponent<CanvasGroup>().alpha = scale;
                    rootObj.transform.localScale = new Vector3(scale, scale, 1);
                }

            }
        }


        public override void OnShow(object obj)
        {
            Log.Debug($"onshow");

            SetUpNav(LanguageManager.Get("UIMine_VIP_title"), UIType.UIMine_VIP);

            mLoopListView.InitListView(5, OnGetItemByIndex);

            int snapIndex = (int)myVIPType;
            if (snapIndex == (int)VIPType.VIP_None)
            {
                snapIndex = (int)VIPType.VIP_A;
            }
            ClickVIPCard(snapIndex);
        }

        LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
        {
            if (index < 0 || index >= 5)
            {
                return null;
            }

            LoopListViewItem2 item = null;
            if (index == 0 || index == 4)
            {
                item = listView.NewListViewItem("view_vip_card_enpty");
                if (item.IsInitHandlerCalled == false)
                {
                    item.IsInitHandlerCalled = true;
                }
                return item;
            }

            item = listView.NewListViewItem("view_vip_card");
            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
            }
            GameObject vipCardObject = item.gameObject;
            SetUpItemWithData(vipCardObject, index);
            ClickEventListener listener = ClickEventListener.Get(vipCardObject);
            listener.SetClickEventHandler(delegate (GameObject obj) { ClickVIPCard(index); });

            return item;
        }

        private void SetUpItemWithData(GameObject go, int index)
        {
            if (myVIPType == (VIPType)index)
            {
                go.transform.Find("root_obj").Find("Image").gameObject.SetActive(true);
                string endDate = TimeHelper.GetDateTimer(long.Parse(GameCache.Instance.vipEndDate)).ToString("yyyy-MM-dd");
                go.transform.Find("root_obj").Find("Text").gameObject.GetComponent<Text>().text = $"{LanguageManager.Get("UIMine_VIP_expire")}{endDate}";
            }
            else
            {
                go.transform.Find("root_obj").Find("Image").gameObject.SetActive(false);
                go.transform.Find("root_obj").Find("Text").gameObject.GetComponent<Text>().text = LanguageManager.Get("UIMine_VIP_openVIP");
            }

            Image vipImage = go.transform.Find("root_obj").gameObject.GetComponent<Image>();
            if (index == (int)VIPType.VIP_A)
            {
                vipImage.sprite = rc.Get<Sprite>("vip_card_gold");
            }
            else if(index == (int)VIPType.VIP_B)
            {
                vipImage.sprite = rc.Get<Sprite>("vip_card_platinum");
            }
            else if (index == (int)VIPType.VIP_C)
            {
                vipImage.sprite = rc.Get<Sprite>("vip_card_diamond");
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

            scroll_view_top_cards = rc.Get<GameObject>("scroll_view_top_cards");
            //view_vip_card_a = rc.Get<GameObject>("view_vip_card_a");
            //view_vip_card_b = rc.Get<GameObject>("view_vip_card_b");
            //view_vip_card_c = rc.Get<GameObject>("view_vip_card_c");

            btn_vip_buy_month = rc.Get<GameObject>("btn_vip_buy_month").GetComponent<Button>();
            btn_vip_buy_year = rc.Get<GameObject>("btn_vip_buy_year").GetComponent<Button>();
            Text_price_month = rc.Get<GameObject>("Text_price_month").GetComponent<Text>();
            Text_price_year = rc.Get<GameObject>("Text_price_year").GetComponent<Text>();

            img_vip_right_lock_1 = rc.Get<GameObject>("img_vip_right_lock_1");
            img_vip_right_lock_3 = rc.Get<GameObject>("img_vip_right_lock_3");
            img_vip_right_lock_4 = rc.Get<GameObject>("img_vip_right_lock_4");

            chart_vip_right_grip = rc.Get<GameObject>("chart_vip_right_grip");
            chart_item = rc.Get<GameObject>("chart_item");

            //UIEventListener.Get(view_vip_card_a, (int)VIPType.VIP_A).onIntClick = ClickVIPCard;
            //UIEventListener.Get(view_vip_card_b, (int)VIPType.VIP_B).onIntClick = ClickVIPCard;
            //UIEventListener.Get(view_vip_card_c, (int)VIPType.VIP_C).onIntClick = ClickVIPCard;

            UIEventListener.Get(btn_vip_buy_month.gameObject, 0).onIntClick = ClickBuyVIP;
            UIEventListener.Get(btn_vip_buy_year.gameObject, 1).onIntClick = ClickBuyVIP;

            mLoopListView = rc.Get<GameObject>("scroll_view_top_cards").GetComponent<LoopListView2>();
        }


        private int GetNearestItemIndex()
        {
            return 0;
        }

        private void SetSnapTargetItemIndex(int index)
        {

        }

        //选择不同VIP后更新界面
        private void UpdateUI()
        {
            switch (selectedVIPType)
            {
                case VIPType.VIP_A:
                    Text_price_month.text = $"30{LanguageManager.Get("UIMine_VIP_month_price")}";
                    Text_price_year.text = $"320{LanguageManager.Get("UIMine_VIP_year_price")}";
                    img_vip_right_lock_1.SetActive(true);
                    img_vip_right_lock_3.SetActive(true);
                    img_vip_right_lock_4.SetActive(true);
                    break;
                case VIPType.VIP_B:
                    Text_price_month.text = $"50{LanguageManager.Get("UIMine_VIP_month_price")}";
                    Text_price_year.text = $"510{LanguageManager.Get("UIMine_VIP_year_price")}";
                    img_vip_right_lock_1.SetActive(false);
                    img_vip_right_lock_3.SetActive(true);
                    img_vip_right_lock_4.SetActive(true);
                    break;
                case VIPType.VIP_C:
                    Text_price_month.text = $"99{LanguageManager.Get("UIMine_VIP_month_price")}";
                    Text_price_year.text = $"950{LanguageManager.Get("UIMine_VIP_year_price")}";
                    img_vip_right_lock_1.SetActive(false);
                    img_vip_right_lock_3.SetActive(false);
                    img_vip_right_lock_4.SetActive(false);
                    break;
            }
            if (selectedVIPType < myVIPType)
            {
                btn_vip_buy_month.enabled = false;
                btn_vip_buy_year.enabled = false;
            }
            else
            {
                btn_vip_buy_month.enabled = true;
                btn_vip_buy_year.enabled = true;
            }

            if (selectedVIPType == myVIPType)
            {
                btn_vip_buy_month.gameObject.transform.Find("Text_cardTitle").gameObject.GetComponent<Text>().text = LanguageManager.Get("UIMine_VIP_cardTitle_month_add");
                btn_vip_buy_year.gameObject.transform.Find("Text_cardTitle").gameObject.GetComponent<Text>().text = LanguageManager.Get("UIMine_VIP_cardTitle_year_add");
            }
            else
            {
                btn_vip_buy_month.gameObject.transform.Find("Text_cardTitle").gameObject.GetComponent<Text>().text = LanguageManager.Get("UIMine_VIP_cardTitle_month");
                btn_vip_buy_year.gameObject.transform.Find("Text_cardTitle").gameObject.GetComponent<Text>().text = LanguageManager.Get("UIMine_VIP_cardTitle_year");
            }

            SetUpChartView();
        }


        //设置底部表格
        private void SetUpChartView()
        {
            if (myVIPType == selectedVIPType)
            {
                //用户当前的VIP，显示剩余特权
                GridLayoutGroup grid = chart_vip_right_grip.GetComponent<GridLayoutGroup>();
                float viewWidth = rc.GetComponent<RectTransform>().rect.width - 80;
                grid.cellSize = new Vector2(viewWidth / 4, 96);
                grid.constraintCount = 4;

                if (configList.Count == 4)
                {
                    if (null == myVIPInfo)
                        return;
                    foreach (GameObject chartItem in chartItems)
                    {
                        UnityEngine.Object.Destroy(chartItem);
                    }
                    AddChartItem(new string[] { LanguageManager.Get("UIMine_VIP_right"), LanguageManager.Get("UIMine_VIP_upperLimit"), LanguageManager.Get("UIMine_VIP_used"), LanguageManager.Get("UIMine_VIP_left") });

                    WEB2_user_vip_config.DataElement configData = configList[(int)myVIPType];

                    AddChartItem(new string[] { LanguageManager.Get("UIMine_VIP_paipuLimit") });
                    AddChartItem(new string[] { configData.cardSpectrum });
                    AddChartItem(new string[] { $"{myVIPInfo.collectSpectrum}" });
                    AddChartItem(new string[] { $"{int.Parse(configData.cardSpectrum) - myVIPInfo.collectSpectrum}" });

                }
            }
            else
            {
                //非用户VIP等级的，显示特权表格
                GridLayoutGroup grid = chart_vip_right_grip.GetComponent<GridLayoutGroup>();
                float viewWidth = rc.GetComponent<RectTransform>().rect.width - 80;
                grid.cellSize = new Vector2(viewWidth / 5, 96);
                grid.constraintCount = 5;

                if (configList.Count == 4)
                {
                    foreach (GameObject chartItem in chartItems)
                    {
                        UnityEngine.Object.Destroy(chartItem);
                    }
                    AddChartItem(new string[] { LanguageManager.Get("UIMine_VIP_right"), LanguageManager.Get("UIMine_VIP_normal"), LanguageManager.Get("UIMine_VIP_gold"), LanguageManager.Get("UIMine_VIP_platinum"), LanguageManager.Get("UIMine_VIP_diamond") });
                    string[] titles = { LanguageManager.Get("UIMine_VIP_paipuLimit"), LanguageManager.Get("UIMine_VIP_dataTimeTitle") };
                    for (int i = 0; i < 2; i++)
                    {
                        AddChartItem(new string[] { titles[i] });
                        for (int n = 0; n < 4; n++)
                        {
                            WEB2_user_vip_config.DataElement data = configList[n];
                            string chartValue = "";
                            if (i == 0)
                            {
                                chartValue = data.cardSpectrum;
                            }
                            else if (i == 1)
                            {
                                if (data.viewDays == -1)
                                {
                                    chartValue = LanguageManager.Get("UIMine_VIP_dataAll");
                                }
                                else
                                {
                                    chartValue = $"{data.viewDays}";
                                }
                            }

                            AddChartItem(new string[] { chartValue });
                        }
                    }
                }
            }


        }

        private void AddChartItem(string[] itemStrs)
        {
            foreach (string itemStr in itemStrs)
            {
                GameObject chartItem = GetChartItem(chart_vip_right_grip.transform);
                chartItem.transform.Find("Text").GetComponent<Text>().text =itemStr;
                this.chartItems.Add(chartItem);
            }
        }

        GameObject GetChartItem(Transform parent)
        {
            GameObject obj = GameObject.Instantiate(chart_item);
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(parent);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            return obj;
        }
        #endregion

        #region Action
        //切换VIP类型
        private void ClickVIPCard(int index)
        {
            mLoopListView.SetSnapTargetItemIndex(index);
            SetCurrentVIP(index);
        }

        private void SetCurrentVIP(int index)
        {
            if (index < 1 || index > 3)
            {
                return;
            }
            if ((int)selectedVIPType == index)
            {
                return;
            }
            selectedVIPType = (VIPType)index;

            this.rc.Get<GameObject>("view_dot").transform.Find("dot_0").GetComponent<Image>().color = index == 1 ? new Color32(233, 191, 128, 255) : new Color32(39, 39, 43, 255);
            this.rc.Get<GameObject>("view_dot").transform.Find("dot_1").GetComponent<Image>().color = index == 2 ? new Color32(233, 191, 128, 255) : new Color32(39, 39, 43, 255);
            this.rc.Get<GameObject>("view_dot").transform.Find("dot_2").GetComponent<Image>().color = index == 3 ? new Color32(233, 191, 128, 255) : new Color32(39, 39, 43, 255);

            UpdateUI();
        }

        //购买VIP
        private void ClickBuyVIP(GameObject go, int index)
        {
            Button button = go.GetComponent<Button>();
            if (button.enabled)
            {
                UIComponent.Instance.Prompt();
                UIComponent.Instance.ShowNoAnimation(UIType.UIMine_VIPPurchase, new UIMine_VIPPurchaseComponent.PurchaseData()
                {
                    remain_fee = myVIPInfo != null ? myVIPInfo.remainFee : 0,
                    remain_days = myVIPInfo != null ? myVIPInfo.remainDays : 0,
                    myVIPType = myVIPType,
                    selectedVIPType = selectedVIPType,
                    buyTimeType = (BuyTimeType)index,
                    successDelegate = PurchaseSuccess,
                });
                UIComponent.Instance.ClosePrompt();
            }

        }
        #endregion

        #region Delegate
        void PurchaseSuccess()
        {
            shouldRefreshTop = true;
            myVIPType = (VIPType)GameCache.Instance.vipLevel;
            ObtinVIPConfigInfo();

            UI uI = UIComponent.Instance.Get(UIType.UIMine);
            if (null != uI)
            {
                UIMineComponent mineComponent = uI.UiBaseComponent as UIMineComponent;
                mineComponent.ObtainUserInfo();
            }

            UI dataUI = UIComponent.Instance.Get(UIType.UIData);
            if (null != dataUI)
            {
                UIDataComponent dataComponent = dataUI.UiBaseComponent as UIDataComponent;
                dataComponent.ObtainData();
            }
        }
        #endregion
    }
}
