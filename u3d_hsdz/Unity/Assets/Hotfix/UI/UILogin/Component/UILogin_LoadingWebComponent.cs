using UnityEngine;
using UnityEngine.UI;
using ETModel;
using System.Collections.Generic;

namespace ETHotfix
{
    [ObjectSystem]
    public class UILogin_LoadingWebComponentSystem : AwakeSystem<UILogin_LoadingWebComponent>
    {
        public override void Awake(UILogin_LoadingWebComponent self)
        {
            self.Awake();
        }
    }

    public class UILogin_LoadingWebComponent : UIBaseComponent
    {
        public sealed class LoadingWebData
        {
            public string mWebUrl { get; set; }
            public string mTitleTxt { get; set; }
        }


        private ReferenceCollector rc;
        private UniWebView webView;

        public void Awake()
        {
            rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            webView = rc.Get<GameObject>("UniWebView").GetComponent<UniWebView>();
        }

        public override void OnShow(object obj)
        {
            NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
            int realTop = (int)(safeArea.top * 1242 / safeArea.width);
            RectTransform rectTransform = rc.gameObject.GetComponent<RectTransform>();
            int navTop = (int)((146 + realTop) * Screen.height / rectTransform.rect.height) + (int)(safeArea.top);
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                navTop = (int)(safeArea.width * 146 / 1242 + safeArea.top);

            webView.insets = new UniWebViewEdgeInsets(navTop, 0,0,0);

            webView.toolBarShow = false; 

            if (obj != null && obj is LoadingWebData)
            {
                var tDto = obj as LoadingWebData;
                SetUpNav(tDto.mTitleTxt, UIType.UILogin_LoadingWeb);
                webView.Load(tDto.mWebUrl);
                Log.Debug("url=" + tDto.mWebUrl);
            }
            else
            {//空时
                webView.Load(GlobalData.Instance.UserAgentURL+UIMineModel.mInstance.GetUrlSuffix());
                SetUpNav(LanguageManager.mInstance.GetLanguageForKey("UIMatchModel_102"), UIType.UILogin_LoadingWeb);
            }

        }

        //获取VIP特权信息
        void FinishTask()
        {
            WEB2_user_task_finish_task_app.RequestData requestData = new WEB2_user_task_finish_task_app.RequestData()
            {
                action = 3
            };
            HttpRequestComponent.Instance.Send(
                WEB2_user_task_finish_task_app.API,
                WEB2_user_task_finish_task_app.Request(requestData),
                this.OpenFinishTask);
        }

        void OpenFinishTask(string resData)
        {
            Log.Debug(resData);
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
            webView.CleanCache();
            webView = null;
            base.Dispose();
        }
    }
}
