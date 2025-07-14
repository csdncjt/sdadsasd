using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
//unuse
namespace Assets.Hotfix.UI.UIClub.Component
{

}

namespace ETHotfix
{
    [ObjectSystem]
    public class WebViewComponentAwakeSystem : AwakeSystem<WebViewComponent>
    {
        public override void Awake(WebViewComponent self)
        {
            self.Awake();
        }
    }

    public class WebViewComponent : Component
    {
        //public static WebViewComponent Instance;
        public Text result;
        UniWebView webView;

        int sceneHeight;
        int sceneWidth;

        public void Awake() {

            //Instance = this;

            float rate = 1242f / Screen.width;
            sceneWidth = 1242;
            sceneHeight = Mathf.RoundToInt(rate * Screen.height);

            

        }

        public override void Dispose()
        {
            //Instance = null;
            if(webView != null)GameObject.Destroy(webView.gameObject);
            webView = null;
            base.Dispose();
        }

        public void LoadWeb(string url , Rect rect, Transform parent)
        {
            webView = CreateWebView(parent);
            webView.OnLoadComplete += OnLoadComplete;
            webView.toolBarShow = false;
            webView.url = url;
            webView.Load();
        }

        void OnLoadComplete(UniWebView webView, bool success, string errorMessage)
        {
            if (success)
            {
                webView.Show();
            }
            else
            {
                Debug.Log("Something wrong in webview loading: " + errorMessage);
            }
        }

    
        
        UniWebView CreateWebView(Transform parent)
        {
            var webViewGameObject = new GameObject("UniWebView");
            webViewGameObject.transform.SetParent(parent);
            webViewGameObject.transform.SetSiblingIndex(1);
            var webView = webViewGameObject.AddComponent<UniWebView>();
            return webView;
        }
    }
}
