using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;
using BannerEle = ETHotfix.WEB_common_banner.DataElement;
using System;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIDiscoverComponentAwakeSystem : AwakeSystem<UIDiscoverComponent>
    {
        public override void Awake(UIDiscoverComponent self)
        {
            self.Awake();
        }
    }

    public class UIDiscoverComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private string GameUrl;


        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            RequestBanner();
        }


        void RequestBanner()
        {
          
        }

        public override void OnHide()
        {

        }

        public override void Dispose()
        {
            //for (int i = 0; i < mRawImgs.Count; ++i)
            //{
            //    mRawImgs[i].texture = null;
            //}
            //mRawImgs.Clear();
            rc = null;
            base.Dispose();
            Resources.UnloadUnusedAssets();
        }

        #region InitUI
        protected virtual void InitUI()
        {
            int lang = LanguageManager.mInstance.mCurLanguage;
            if (lang == 2)
                lang = 0;

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            for (int i = 1; i < 3; i++)
            {
                var item = rc.Get<GameObject>("Item_"+i.ToString()).GetComponent<Button>();
                if (i == 1)
                {
                    item.onClick.AddListener(delegate () { this.OnBtnClickGame1(); });
                }

                if (i == 2)
                {
                    item.onClick.AddListener(delegate () { this.OnBtnClickGame2(); });
                }


                item.transform.GetChild(1).gameObject.SetActive(true);
                item.transform.GetChild(2).gameObject.SetActive(true);
                if (lang == 0)
                    item.transform.GetChild(2).gameObject.SetActive(false);
            }
           

            GameObject baseView = rc.gameObject;
            NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
            float realTop = safeArea.top * 1242 / safeArea.width;
            baseView.GetComponent<VerticalLayoutGroup>().padding.top = (int)realTop;
            float realBottom = safeArea.bottom * 1242 / safeArea.width;
            baseView.GetComponent<VerticalLayoutGroup>().padding.bottom = (int)realBottom;
        }

        void OnBtnClickGame1()
        {
            SendToEnterGame(1001);
        }

        void OnBtnClickGame2()
        {
            SendToEnterGame(1002);
        }

        void SendToEnterGame(int gameid)
        {
            //请求token
            HttpRequestComponent.Instance.Send(
                WEB_game_banner.API,
                WEB_game_banner.Request(new WEB_game_banner.RequestData()), str =>
                {
                    var tDto = WEB_game_banner.Response(str);
                    enterGame(tDto.data.userUid, gameid, tDto.data.gameIp);

                });
        }

        void enterGame(string token, int id, string ip)
        {
            GameUrl = "http://"+ ip + ":8080?account=" + GameCache.Instance.strPhoneFirst + "-" + GameCache.Instance.strPhone;
            string url = string.Format("{0}&KindId={1}&lang={2}&ip={3}&token={4}", GameUrl, id, LanguageManager.mInstance.mCurLanguage, ip, token);
            UIComponent.Instance.ShowNoAnimation(UIType.UIGame_Web, new UIGame_WebComponent.LoadingWebData()
            {
                mWebUrl = url
            });
        }

        #endregion

    }
}
