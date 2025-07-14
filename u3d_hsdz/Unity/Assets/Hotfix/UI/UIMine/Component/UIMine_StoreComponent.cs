using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_StoreComponentSystem : AwakeSystem<UIMine_StoreComponent>
    {
        public override void Awake(UIMine_StoreComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UIMine_StoreComponentUpdateSystem : UpdateSystem<UIMine_StoreComponent>
    {
        public override void Update(UIMine_StoreComponent self)
        {
            self.Update();
        }
    }

    public class UIMine_StoreComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private LoopListView2 mLoopListView;
        private RawImage imageBanner;

        public float sendInterval = 5;
        private float recordDeltaTime = 0f;

        int payType = 0; // 0 = 系统决定支付类型  1 = 支付宝 2 = 内购

        string orderID;
        string currentProductID;

        string transactionId;

        private List<WEB_pay_product_list.ListElement> mStoreDataList = new List<WEB_pay_product_list.ListElement>();


        public void Awake()
        {
            InitUI();
            ETModel.Game.Hotfix.OnApplicationPauseFalse += OnApplicationPauseFalse;

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                InstallPacketDownloaderComponent mInstallPacketDownloaderComponent = Game.Scene.ModelScene.GetComponent<InstallPacketDownloaderComponent>();
                if (mInstallPacketDownloaderComponent.remoteInstallPacketConfig.IsAppStore)
                {
                    payType = 2;
                }
            }

        }

        public void Update()
        {
            if (!(Time.time - recordDeltaTime > sendInterval)) return;
            recordDeltaTime = Time.time;

            if (null != orderID && payType != 2)
            {
                CheckOrder();
            }
        }

        public override void OnHide()
        {
            orderID = null;
            ETModel.Game.Hotfix.OnInAppPurchaseCallBack = null;
        }

        public override void Dispose()
        {
            ETModel.Game.Hotfix.OnApplicationPauseFalse -= OnApplicationPauseFalse;
            orderID = null;
            mStoreDataList.Clear();
            base.Dispose();
        }

        private void OnApplicationPauseFalse()
        {
            // 恢复
            if (null != orderID && payType != 2)
            {
                CheckOrder();
            }
        }

        // 较验订单
        void CheckOrder()
        {
            WEB_pay_recharge_check.RequestData requestData = new WEB_pay_recharge_check.RequestData()
            {
                order_id = orderID
            };
            HttpRequestComponent.Instance.Send(
                WEB_pay_recharge_check.API,
                WEB_pay_recharge_check.Request(requestData),
                this.OpenCheckInfo,
                GlobalData.Instance.PayURL);
        }

        void OpenCheckInfo(string resData)
        {
            Log.Debug(resData);
            WEB_pay_recharge_check.ResponseData responseData = WEB_pay_recharge_check.Response(resData);
            if (responseData.status == 0)
            {
                if (responseData.data.pay_status == 1)
                {
                    GameCache.Instance.gold = responseData.data.chip;
                    UI uI = UIComponent.Instance.Get(UIType.UIMine);
                    if (null != uI)
                    {
                        UIMineComponent mineComponent = uI.UiBaseComponent as UIMineComponent;
                        mineComponent.ObtainUserInfo();
                    }

                    UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10235));
                    UIComponent.Instance.RemoveAnimated(UIType.UIMine_Store);
                }
                else
                {
                    UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10236));
                }
            }
            else
            {
                //UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10236));
            }
        }


        public override void OnShow(object obj)
        {
            ETModel.Game.Hotfix.OnInAppPurchaseCallBack = OnInAppPurchaseCallBack;

            Log.Debug($"onShow");
            SetUpNav(CPErrorCode.LanguageDescription(10333), UIType.UIMine_Store);

            mStoreDataList.Clear();
            ObtinStoreData();
        }

        //获取商品列表
        void ObtinStoreData()
        {
            WEB_pay_product_list.RequestData requestData = new WEB_pay_product_list.RequestData()
            {
                ios_pay = payType == 2 ? 1 : 0
            };
            HttpRequestComponent.Instance.Send(
                WEB_pay_product_list.API,
                WEB_pay_product_list.Request(requestData),
                this.OpenStoreInfo,
                GlobalData.Instance.PayURL);
        }

        void OpenStoreInfo(string resData)
        {
            Log.Debug(resData);
            WEB_pay_product_list.ResponseData responseData = WEB_pay_product_list.Response(resData);
            if (responseData.status == 0)
            {
                mStoreDataList = responseData.data.list;
                mLoopListView.InitListView(mStoreDataList.Count, OnGetItemByIndex);
                if (string.IsNullOrEmpty(responseData.data.banner))
                {
                    imageBanner.gameObject.SetActive(false);
                }
                else
                {
                    imageBanner.gameObject.SetActive(true);
                    WebImageHelper.SetHttpBanner(imageBanner, responseData.data.banner);
                }
            }
        }


        #region InitUI
        protected virtual void InitUI()
        {
            this.rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mLoopListView = rc.Get<GameObject>("Scrollview").GetComponent<LoopListView2>();
            imageBanner = rc.Get<GameObject>("ImageBanner").GetComponent<RawImage>();
        }

        private void SetUpItemWithData(GameObject go, WEB_pay_product_list.ListElement element)
        {
            go.transform.Find("text_Title").GetComponent<Text>().text = element.title;
            go.transform.Find("Button").Find("Text").GetComponent<Text>().text = element.price_show;
            if (element.icon.Length != 0)
            {
                go.transform.Find("RawImage_icon").GetComponent<RawImage>().texture = null;
                WebImageHelper.SetHttpBanner(go.transform.Find("RawImage_icon").GetComponent<RawImage>(), element.icon);
            }
        }

        #endregion

        #region Action
        private void OnItemClicked(int index)
        {
            WEB_pay_product_list.ListElement itemData = mStoreDataList[index];
            currentProductID = itemData.pdt_id;

            WEB_pay_recharge_apply.RequestData requestData = new WEB_pay_recharge_apply.RequestData()
            {
                pay_type = payType,
                pdt_id = itemData.pdt_id,
                lang = LanguageManager.mInstance.mCurLanguage
            };
            HttpRequestComponent.Instance.Send(
                WEB_pay_recharge_apply.API,
                WEB_pay_recharge_apply.Request(requestData),
                this.OpenOrderInfo,
                GlobalData.Instance.PayURL);
        }
        #endregion


        void OpenOrderInfo(string resData)
        {
            Log.Debug(resData);
            WEB_pay_recharge_apply.ResponseData responseData = WEB_pay_recharge_apply.Response(resData);
            if (responseData.status == 0)
            {
                orderID = responseData.data.order_id;
                if (responseData.data.pay_type == 2)
                {
                    //内购
                    NativeManager.InAppPurchase(currentProductID, orderID);
                    UIComponent.Instance.Prompt();
                }
                else
                {
                    if (responseData.data.pay_mode == 2)
                    {
                        string url = responseData.data.pay_info;
                        Application.OpenURL(url);
                    }
                }

            }
        }

        #region SupperScrollView
        LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
        {
            if (index < 0)
            {
                return null;
            }
            WEB_pay_product_list.ListElement itemData = mStoreDataList[index];
            if (itemData == null)
            {
                return null;
            }
            LoopListViewItem2 item = listView.NewListViewItem("Store_info");
            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
            }
            GameObject storeObject = item.gameObject;
            //todo
            SetUpItemWithData(storeObject, itemData);
            ClickEventListener listener = ClickEventListener.Get(storeObject);
            listener.SetClickEventHandler(delegate (GameObject obj) { OnItemClicked(index); });

            return item;
        }
        #endregion

        #region InAppPurchase
        private void OnInAppPurchaseCallBack(string purchaseInfo)
        {
            UIComponent.Instance.ClosePrompt();

            string[] mInfns = purchaseInfo.Split(new string[] { "," }, StringSplitOptions.None);
            string status = mInfns[0]; //0 成功 1 默认错误 2 网络错误 3 取消支付 4 等待上一个支付 5 内购不可用 6 无法获取产品信息 7 提交订单失败 8 等待交易中
            string productId = mInfns[1];
            string orderId = mInfns[2];
            this.transactionId = mInfns[3];
            string receiptData = mInfns[4];

            if (status == "0")
            {
                WEB_pay_iospay_verify.RequestData requestData = new WEB_pay_iospay_verify.RequestData()
                {
                    order_id = orderId,
                    pay_status = 1,
                    trade_no = transactionId,
                    receipt = receiptData
                };
                HttpRequestComponent.Instance.Send(
                    WEB_pay_iospay_verify.API,
                    WEB_pay_iospay_verify.Request(requestData),
                    this.OpenIosPayCheckInfo,
                    GlobalData.Instance.PayURL);
            }
            else
            {
                UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10236));
            }

        }

        void OpenIosPayCheckInfo(string resData)
        {
            Log.Debug(resData);
            WEB_pay_iospay_verify.ResponseData responseData = WEB_pay_iospay_verify.Response(resData);
            if (responseData.status == 0)
            {
                if (responseData.data.trade_no == transactionId)
                {
                    GameCache.Instance.gold = responseData.data.chips;
                    GameCache.Instance.idou = responseData.data.idous;

                    UI uI = UIComponent.Instance.Get(UIType.UIMine);
                    if (null != uI)
                    {
                        UIMineComponent mineComponent = uI.UiBaseComponent as UIMineComponent;
                        mineComponent.ObtainUserInfo();
                    }

                    UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10235));
                    UIComponent.Instance.RemoveAnimated(UIType.UIMine_Store);
                }
                else
                {
                    UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10236));
                }
            }
        }
        #endregion

    }




}

