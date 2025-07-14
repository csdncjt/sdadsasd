using System.Collections;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_PaipuDetailComponentSystem : AwakeSystem<UIMine_PaipuDetailComponent>
    {
        public override void Awake(UIMine_PaipuDetailComponent self)
        {
            self.Awake();
        }
    }

    public class UIMine_PaipuDetailComponent : UIBaseComponent
    {
        public sealed class PaipuDetailData
        {
            public WEB2_brand_spe_info.HandsElement paipuData;
            public string roomId;
        }

        private PaipuDetailData paipuDetailData;

        private ReferenceCollector rc;
        private GameObject view_menu_bg;
        private GameObject view_menu_content;
        private Button Button_collect;
        private Button Button_copyLink;
        private UniWebView webView;

        private WEB2_brand_spe_info.HandsElement paipuData;
        private bool isCollect;

        private string urlStr;

        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            Log.Debug($"onShow");
            SetUpNav(CPErrorCode.LanguageDescription(10210), UIType.UIMine_PaipuDetail, ClickMenuAction, null);

            if (null != obj)
            {
                paipuDetailData = obj as PaipuDetailData;
                paipuData = paipuDetailData.paipuData;
                isCollect = paipuData.collect;
                UpdateCollectView();

                string roomId = string.IsNullOrEmpty(paipuDetailData.roomId) ? "0" : paipuDetailData.roomId;
                string url = $"{GlobalData.Instance.PaipuBaseUrl}{roomId}_{GameCache.Instance.nUserId}_{paipuData.id}&c=1";
                urlStr = url;
                // Debug.Log($"{url}");
                Log.Debug($"{url}");

                NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
                float realTop = safeArea.top * 1242 / safeArea.width;

                RectTransform rectTransform = rc.gameObject.GetComponent<RectTransform>();

                webView.zoomEnable = true;
                webView.SetUseWideViewPort(true);
                webView.Load(url);
                int navTop = (int)((146 + realTop) * Screen.height / rectTransform.rect.height) + (int)(safeArea.top);
                int bottom = (int)(440 * Screen.height / rectTransform.rect.height);

                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    navTop = (int)(safeArea.width * 146 / 1242 + safeArea.top);
                    bottom = (int)(safeArea.width * 440 / 1242 + safeArea.bottom);
                }
                   

                webView.insets = new UniWebViewEdgeInsets(navTop, 0, 0, 0);

            }
        }

        public override void OnHide()
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }

        #region InitUI
        protected virtual void InitUI()
        {
            this.rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            view_menu_bg = rc.Get<GameObject>("view_menu_bg");
            view_menu_content = rc.Get<GameObject>("view_menu_content");
            Button_collect = rc.Get<GameObject>("Button_collect").GetComponent<Button>();
            Button_copyLink = rc.Get<GameObject>("Button_copyLink").GetComponent<Button>();
            webView = rc.Get<GameObject>("UniWebView").GetComponent<UniWebView>();

            //webView.SetLoadWithOverviewMode(true);
            //webView.SetUseWideViewPort(true);
            //webView.SetZoomEnabled(true);

            UIEventListener.Get(view_menu_bg).onClick = (go) =>
            {
                view_menu_bg.SetActive(false);

                NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
                float realTop = safeArea.top * 1242 / safeArea.width;
                RectTransform rectTransform = rc.gameObject.GetComponent<RectTransform>();
                int navTop =(int) ((146 + realTop) * Screen.height / rectTransform.rect.height) + (int)(safeArea.top);
                int bottom = (int)(440 * Screen.height / rectTransform.rect.height);
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    navTop = (int)(safeArea.width * 146 / 1242 + safeArea.top);
                    bottom = (int)(safeArea.width * 440 / 1242 + safeArea.bottom);
                }
                webView.insets = new UniWebViewEdgeInsets(navTop, 0, 0, 0);
            };
            Button_collect.onClick.Add(() =>
            {
                SetPaipuCollect(!isCollect);
            });
            Button_copyLink.onClick.Add(() =>
            {
                UniClipboard.SetText(urlStr);
                Toast(CPErrorCode.LanguageDescription(10106));
            });

        }
       

        #endregion

        private async void Toast(string msg)
        {
            GameObject goToast = rc.Get<GameObject>("Toast");
            goToast.SetActive(true);
            Text textToast = goToast.transform.Find("Text_Toast").gameObject.GetComponent<Text>();
            textToast.text = msg;
            TimerComponent iOSTimerComponent = Game.Scene.ModelScene.GetComponent<TimerComponent>();
            await iOSTimerComponent.WaitAsync(3000);
            goToast.SetActive(false);

        }

        #region Action
        private void ClickMenuAction()
        {
            view_menu_bg.SetActive(true);

            NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
            float realTop = safeArea.top * 1242 / safeArea.width;
            RectTransform rectTransform = rc.gameObject.GetComponent<RectTransform>();
            int navTop = (int)((146 + realTop) * Screen.height / rectTransform.rect.height) + (int)(safeArea.top);
            int bottom = (int)(440 * Screen.height / rectTransform.rect.height);
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                navTop = (int)(safeArea.width * 146 / 1242 + safeArea.top);
                bottom = (int)(safeArea.width * 440 / 1242 + safeArea.bottom);
            }
            webView.insets = new UniWebViewEdgeInsets(navTop, 0, bottom, 0);
        }

        void SetPaipuCollect(bool collect)
        {
            WEB2_brand_spe_collect.RequestData requestData = new WEB2_brand_spe_collect.RequestData()
            {
                handId = paipuData.id,
                collect = collect,
            };
            HttpRequestComponent.Instance.Send(
                WEB2_brand_spe_collect.API,
                WEB2_brand_spe_collect.Request(requestData),
                this.OpenPaipuInfo);
        }

        void OpenPaipuInfo(string resData)
        {
            Log.Debug(resData);
            WEB2_brand_spe_collect.ResponseData responseData = WEB2_brand_spe_collect.Response(resData);
            if (responseData.status == 0)
            {
                isCollect = !isCollect;
                UpdateCollectView();
            }
            else
            {
                string errorDes = CPErrorCode.ErrorDescription(responseData.status);
                Toast(errorDes);
            }
        }

        void UpdateCollectView()
        {
            Button_collect.gameObject.transform.Find("Text").GetComponent<Text>().text = isCollect ? CPErrorCode.LanguageDescription(10211) : CPErrorCode.LanguageDescription(10212);
            Button_collect.gameObject.transform.Find("Image").GetComponent<Image>().sprite = isCollect ? rc.Get<Sprite>("button_pop_up_cancle_shoucang") : rc.Get<Sprite>("button_pop_up_shoucang");
        }

        #endregion


    }




}

