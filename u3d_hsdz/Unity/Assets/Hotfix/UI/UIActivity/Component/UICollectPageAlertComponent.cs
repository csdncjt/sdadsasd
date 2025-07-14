using System;
using System.Collections;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{

    [ObjectSystem]
    public class UICollectPageAlertComponentAwakeSystem : AwakeSystem<UICollectPageAlertComponent>
    {
        public override void Awake(UICollectPageAlertComponent self)
        {
            self.Awake();
        }
    }

    public class UICollectPageAlertComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Button buttonGo;
        private Button buttonClose;

        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            buttonGo = rc.Get<GameObject>("Button_Go").GetComponent<Button>();
            buttonClose = rc.Get<GameObject>("Button_Close").GetComponent<Button>();

            UIEventListener.Get(buttonGo.gameObject).onClick = OnClickGo;
            UIEventListener.Get(buttonClose.gameObject).onClick = OnClickClose;
        }

        private void OnClickGo(GameObject go)
        {
            UIComponent.Instance.Remove(UIType.UICollectPageAlert);

            InstallPacketDownloaderComponent mInstallPacketDownloaderComponent = Game.Scene.ModelScene.GetComponent<InstallPacketDownloaderComponent>();
            UIComponent.Instance.ShowNoAnimation(UIType.UILogin_LoadingWeb, new UILogin_LoadingWebComponent.LoadingWebData()
            {
                mWebUrl = mInstallPacketDownloaderComponent.remoteInstallPacketConfig.IOSUrl,
                mTitleTxt = LanguageManager.Get("CollectPage_title")
            });
        }

        private void OnClickClose(GameObject go)
        {
            UIComponent.Instance.Remove(UIType.UICollectPageAlert);
        }

        public override void OnShow(object obj)
        {
        }

        public override void OnHide()
        {
            rc.gameObject.SetActive(false);
        }

    }
}

