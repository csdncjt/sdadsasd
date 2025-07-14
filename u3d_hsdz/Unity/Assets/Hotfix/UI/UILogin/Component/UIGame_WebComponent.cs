using UnityEngine;
using UnityEngine.UI;
using ETModel;
using System.Collections.Generic;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIGame_WebComponentComponentSystem : AwakeSystem<UIGame_WebComponent>
    {
        public override void Awake(UIGame_WebComponent self)
        {
            self.Awake();
        }
    }

    public class UIGame_WebComponent : UIBaseComponent
    {
        public string cmd_closeview = "close";
       

        public sealed class LoadingWebData
        {
            public string mWebUrl { get; set; }
        }


        private ReferenceCollector rc;
        private GameObject webPanel;
        private UniWebView webView;

        public void Awake()
        {
            rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            webPanel = rc.Get<GameObject>("Panel");
            //webView = rc.Get<GameObject>("UniWebView").GetComponent<UniWebView>();
            //webView.OnReceivedMessage += OnReceivedMessage;
            //webView.OnLoadComplete += OnLoadComplete;
            //webView.OnWebViewShouldClose += OnWebViewShouldClose;
            //webView.SetUseWideViewPort(true);
        }
        

        public override void OnShow(object obj)
        {

            if (obj != null && obj is LoadingWebData)
            {
                var tDto = obj as LoadingWebData;
                NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;

                UniWebViewEdgeInsets insets = new UniWebViewEdgeInsets((int)-safeArea.top, 0, (int)-safeArea.bottom, 0);
                if (Application.platform == RuntimePlatform.Android)
                    insets = new UniWebViewEdgeInsets((int)safeArea.bottom, 0, (int)safeArea.top, 0);

                webView = GameObject.Find("Global").GetComponent<ETModel.Init>().showWebView(webPanel, tDto.mWebUrl, insets, callFunc);
                webView.alpha = 0;

            }
        }

        public void callFunc(string arg)
        {
            if (arg == "close")
            {
                UIComponent.Instance.Remove(UIType.UIGame_Web);
            }
            else if (arg == "loadSuccess")
            {
                webView.alpha = 1;
            }
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
            rc = null;
            //_webView.CleanCache();
            //_webView = null;
            base.Dispose();
        }
    }
}
