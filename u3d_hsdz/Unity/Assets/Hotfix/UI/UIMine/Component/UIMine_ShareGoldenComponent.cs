using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_ShareGoldenComponentSystem : AwakeSystem<UIMine_ShareGoldenComponent>
    {
        public override void Awake(UIMine_ShareGoldenComponent self)
        {
            self.Awake();
        }
    }

    public class UIMine_ShareGoldenComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        //private Transform SubMenu;
        //private Button Button_close;
        private Transform webParent;
        private UniWebView webView;
        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_ShareGolden);
            

            NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
            float realTop = safeArea.top * 1242 / safeArea.width;
            float realbottom = safeArea.top * 1242 / safeArea.width;

            RectTransform rectTransform = rc.gameObject.GetComponent<RectTransform>();
            webView.Load("http://res.chatpoker2.com/res/staticPage/usualProblem/index.html");
            float navTop = (146 + realTop) * Screen.height / rectTransform.rect.height + (int)(safeArea.top);
            float navbottom = (262+realbottom)* Screen.height / rectTransform.rect.height + (int)(safeArea.bottom);

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                navTop = (int)(safeArea.width * 146 / 1242 + safeArea.top);
                navbottom = (int)(safeArea.width * 262 / 1242 + safeArea.bottom);
            }
            webView.insets = new UniWebViewEdgeInsets((int)navTop, 0, (int)navbottom, 0);
            //webView.Frame = new Rect(new Vector2(0, navTop), new Vector2(Screen.width, Screen.height - navTop - navbottom));
            //webView.UpdateFrame();

            webView.OnLoadComplete += OnLoadComplete;
            webView.OnReceivedMessage += OnReceivedMessage;
            webView.OnReceivedKeyCode += OnReceivedKeyCode;
            webView.OnWebViewShouldClose += OnWebViewShouldClose;
        }

        public override void OnHide()
        {
        }

        void OnLoadComplete(UniWebView webView, bool success, string errorMessage)
        {
            if (success)
            {
                webView.Show();
            }
        }

        private bool OnWebViewShouldClose(UniWebView webView)
        {
            return true;
        }

        private void OnReceivedKeyCode(UniWebView webView, int keyCode)
        {
            if (keyCode == 4)
            {
                Dispose();
            }
        }

        private void OnReceivedMessage(UniWebView webView, UniWebViewMessage message)
        {
            if (message.path == "close")
            {
                while (webView.CanGoBack())
                {
                    webView.GoBack();
                }
                Dispose();
            }
        }

        public override void Dispose()
        {
            UIMineModel.mInstance.SetNullProblemUrl();
            base.Dispose();
        }

        //private void RightAction()
        //{
        //    //打开菜单
        //    SubMenu.gameObject.SetActive(true);
        //    Button_close.gameObject.SetActive(true);
        //}

        //private void CloseMenu(GameObject go)
        //{
        //    //关闭菜单
        //    SubMenu.gameObject.SetActive(false);
        //    Button_close.gameObject.SetActive(false);
        //}

        #region UI
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            webParent = rc.Get<GameObject>("Panel").transform;
            webView = rc.Get<GameObject>("UniWebView").GetComponent<UniWebView>();

            //SubMenu = rc.Get<GameObject>("SubMenu").transform;
            //SubMenu.gameObject.SetActive(false);

            //Button_close = rc.Get<GameObject>("Button_close").GetComponent<Button>();

            //UIEventListener.Get(Button_close.gameObject).onClick = CloseMenu;

            var name = string.Empty;
            for (int i = 1; i < 5; i++)
            {
                name = "ItemInfo_" + i.ToString();
                UIEventListener.Get(this.rc.Get<GameObject>(name), i).onIntClick = ClickItemInfo;
            }

            //webView.SetLoadWithOverviewMode(true);
            webView.SetUseWideViewPort(true);
            //webView.SetZoomEnabled(true);
        }
      
        private void ClickItemInfo(GameObject go, int index)
        {
            //CloseMenu(go);

            if (index == 1)//成为推广员
            {
                WillOpenPage(UIType.UIMine_PromotersBecome, delegate
                {
                    OpenBecomeDialog();
                });

                UIComponent.Instance.Remove(UIType.UIMine_ShareGolden);
            }
            else if (index == 2)//业绩查询
            {
                WillOpenPage(UIType.UIMine_AchievementQuery, delegate
                {
                    UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10234));
                });

                UIComponent.Instance.Remove(UIType.UIMine_ShareGolden);
            }
            else if (index == 3)//推广员管理
            {
                WillOpenPage(UIType.UIMine_PromotersManage, delegate
                {
                    UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10234));
                });

                UIComponent.Instance.Remove(UIType.UIMine_ShareGolden);
            }
            else if (index == 4)
            {//常见问题
                UIMineModel.mInstance.GetShareProblemUrl(pUrl =>
                {
                    UIComponent.Instance.ShowNoAnimation(UIType.UILogin_LoadingWeb, new UILogin_LoadingWebComponent.LoadingWebData
                    {
                        mWebUrl = pUrl,
                        mTitleTxt = LanguageManager.mInstance.GetLanguageForKey("ShareItem4")
                    });
                });

                UIComponent.Instance.Remove(UIType.UIMine_ShareGolden);
            }
        }

        void WillOpenPage(string pUIPage, Action pAct)
        {
            if (UIMineModel.mInstance.IsPromotion)//已是推广员 就不请求了[是否推广员接口]
                UIComponent.Instance.ShowNoAnimation(pUIPage);
            else
            {
                UIMineModel.mInstance.APIIsPromotion(pDto =>
                {
                    if (pDto.status == 0)
                    {
                        if (pDto.data)
                            UIComponent.Instance.ShowNoAnimation(pUIPage);
                        else
                        {
                            pAct();
                        }
                    }
                    else
                    {
                        UIComponent.Instance.Toast(pDto.status);
                    }
                });
            }

        }


        void OpenBecomeDialog()
        {
            UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
            {
                type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                title = CPErrorCode.LanguageDescription(10007),
                content = CPErrorCode.LanguageDescription(20055),
                contentCommit = CPErrorCode.LanguageDescription(10012),
                contentCancel = CPErrorCode.LanguageDescription(10013),
                actionCommit = () =>
                {
                    UIComponent.Instance.ShowNoAnimation(UIType.UIMine_PromotersBecome);
                },
                actionCancel = null
            });
        }



        #endregion




    }
}
