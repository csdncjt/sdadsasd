using System;
using System.IO;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_PromotersBecomeBookComponentSystem : AwakeSystem<UIMine_PromotersBecomeBookComponent>
    {
        public override void Awake(UIMine_PromotersBecomeBookComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名: </summary>
    public class UIMine_PromotersBecomeBookComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Image imageCode;
        private GameObject UINavigationBar;

        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_PromotersBecomeBook, ClickRightSavePng);
            if (obj != null && obj is string)
            {
                var url = obj as string;
                var size = imageCode.GetComponent<RectTransform>().sizeDelta;
                var tSprite = QRCodeHelper.GetQRCodeSprite(url, (int)size.x, (int)size.y);
                imageCode.sprite = tSprite;
            }
        }

        private async void ClickRightSavePng()
        {
            UINavigationBar.SetActive(false);
            await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(100);

            ETModel.Game.Hotfix.OnGetScreenShot = OnGetScreenShot;
            NativeManager.Instance.CaptureScreenshot();

            await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(1000);
            UINavigationBar.SetActive(true);
        }

        private void OnGetScreenShot(Texture2D texture)
        {
            ETModel.Game.Hotfix.OnGetScreenShot = null;
            //转为字节数组
            byte[] bytes = texture.EncodeToJPG();
            if (Application.platform == RuntimePlatform.Android)
            {
                if (NativeManager.OnFuncCall<bool>("SaveImage", bytes, "allin 海报"))
                {
                    UIComponent.Instance.ToastLanguage("Become104");//("图片已保存到相册");
                }
                else
                {
                    UIComponent.Instance.ToastLanguage("Become105");//("图片保存失败");
                }
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                NativeManager.SaveImageToNative(bytes, bytes.Length);
            }
        }

        public override void OnHide()
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            imageCode = rc.Get<GameObject>("ImageCode").GetComponent<Image>();
            UINavigationBar = rc.Get<GameObject>("UINavigationBar");
        }
    }
}
