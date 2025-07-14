using System;
using System.Collections;
using System.IO;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_PromotersBecomeComponentSystem : AwakeSystem<UIMine_PromotersBecomeComponent>
    {
        public override void Awake(UIMine_PromotersBecomeComponent self)
        {
            self.Awake();
        }
    }

    public class UIMine_PromotersBecomeComponent : UIBaseComponent
    {

        private ReferenceCollector rc;
        private Text textDownload;
        private Image imageQRCode;
        private Image imageQRCode2;
        private Transform Panel_menu;
        private Transform Panel_Qr;
        private GameObject UINavigationBar;
        public void Awake()
        {
            InitUI();
            textDownload.text = "";
            UIMineModel.mInstance.APIToBePromoter(pDto =>
            {
                if (pDto.status == 0)
                {
                    var size = imageQRCode.GetComponent<RectTransform>().sizeDelta;
                    var tUrl = pDto.data == null ? "" : pDto.data.url;
                    var tSprite = QRCodeHelper.GetQRCodeSprite(pDto.data.url, (int)size.x, (int)size.y);
                    imageQRCode.sprite = tSprite;


                    var size2 = imageQRCode2.GetComponent<RectTransform>().sizeDelta;
                    var tSprite2 = QRCodeHelper.GetQRCodeSprite(pDto.data.url, (int)size2.x, (int)size2.y);
                    imageQRCode2.sprite = tSprite2;

                    textDownload.text = LanguageManager.mInstance.GetLanguageForKey("Become102") + pDto.data.url;
                }
                else if (pDto.status == 1601 || pDto.status == 2101)
                {
                    OpenAddClubDialog();
                }
                else
                {
                    UIComponent.Instance.Toast(pDto.status);
                }
            });
        }

        ///// <summary>
        ///// 融合图片和二维码,得到新图片 
        ///// </summary>
        ///// <param name="tex_base">底图</param>
        ///// <param name="tex_code">二维码</param>
        //public Texture2D MixImagAndQRCode(Texture2D tex_base, Texture2D tex_code)
        //{
        //    Texture2D newTexture = GameObject.Instantiate(tex_base) as Texture2D; ;
        //    Vector2 uv = new Vector2((tex_base.width - tex_code.width) / tex_base.width, (tex_base.height - tex_code.height) / tex_base.height);
        //    for (int i = 0; i < tex_code.width; i++)
        //    {
        //        for (int j = 0; j < tex_code.height; j++)
        //        {
        //            float w = uv.x * tex_base.width - tex_code.width + i;
        //            float h = uv.y * tex_base.height - tex_code.height + j;
        //            //从底图图片中获取到（w，h）位置的像素
        //            Color baseColor = newTexture.GetPixel((int)w, (int)h);
        //            //从二维码图片中获取到（i，j）位置的像素
        //            Color codeColor = tex_code.GetPixel(i, j);
        //            //融合
        //            newTexture.SetPixel((int)w, (int)h, codeColor);
        //        }
        //    }
        //    newTexture.Apply();
        //    return newTexture;
        //}

        private void RightAction()
        {
            //打开菜单
            Panel_menu.gameObject.SetActive(true);
        }

        private void CloseMenu(GameObject go)
        {
            Panel_menu.gameObject.SetActive(false);
        }

        void OpenAddClubDialog()
        {
            var tStrLanguages = LanguageManager.mInstance.GetLanguageForKeyMoreValues("Become103");
            UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
            {
                type = UIDialogComponent.DialogData.DialogType.CommitCancel,//title = "温馨提示",content = "请先加入俱乐部，才可以成为推广员。",contentCommit = "前往",contentCancel = "退出",
                title = tStrLanguages[0],
                content = tStrLanguages[1],
                contentCommit = tStrLanguages[2],
                contentCancel = tStrLanguages[3],
                actionCommit = () =>
                {
                    Game.EventSystem.Run(UIClubEventIdType.CLUB_HOT);
                    UIComponent.Instance.Remove(UIType.UIMine_PromotersBecome);
                },
                actionCancel = () =>
                {
                    UIComponent.Instance.Remove(UIType.UIMine_PromotersBecome);
                }
            });
        }

        public override void OnShow(object obj)
        {
            SetUpNav(CPErrorCode.LanguageDescription(10214), UIType.UIMine_PromotersBecome, RightAction);
        }

        private void ClickRightBtn()
        {
            //常见问题
            UIMineModel.mInstance.GetShareProblemUrl(pUrl =>
            {
                UIComponent.Instance.ShowNoAnimation(UIType.UILogin_LoadingWeb, new UILogin_LoadingWebComponent.LoadingWebData
                {
                    mWebUrl = pUrl,
                    mTitleTxt = LanguageManager.mInstance.GetLanguageForKey("ShareItem4")
                });
            });
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
            UINavigationBar = rc.Get<GameObject>("UINavigationBar");
            textDownload = rc.Get<GameObject>("Text_Download").GetComponent<Text>();
            imageQRCode = rc.Get<GameObject>("Image_QRCode").GetComponent<Image>();
            imageQRCode2 = rc.Get<GameObject>("ImageCode").GetComponent<Image>();

            Panel_menu = rc.Get<GameObject>("Panel_menu").transform;
            Panel_Qr = rc.Get<GameObject>("Panel_Qr").transform;

            UIEventListener.Get(rc.Get<GameObject>("Button_OpenQr")).onClick = ClickOpenQr;
            UIEventListener.Get(rc.Get<GameObject>("Button_Copy")).onClick = ClickCopy;
            UIEventListener.Get(rc.Get<GameObject>("Button_Save")).onClick = ClickOpenSm;
            UIEventListener.Get(rc.Get<GameObject>("Button_SaveXY")).onClick = ClickButton_SaveXY;
            UIEventListener.Get(rc.Get<GameObject>("CloseBtn")).onClick = ClickCloseQr;
            UIEventListener.Get(rc.Get<GameObject>("Image_Mask")).onClick = ClickCloseQr;

            UIEventListener.Get(Panel_menu.gameObject).onClick = CloseMenu;
        }

        private void ClickButton_SaveXY(GameObject go)
        {
            CloseMenu(go);

            //var tStrLanguage = LanguageManager.mInstance.GetLanguageForKey("Become102").Trim();
            //var tCopy = textDownload.text.Replace(tStrLanguage, "");
            //UIComponent.Instance.ShowNoAnimation(UIType.UIMine_PromotersBecomeBook, tCopy);
            ClickRightSavePng();
        }

        private void ClickButton_Save(GameObject go)
        {
            byte[] bytes = imageQRCode.sprite.texture.EncodeToJPG();
            if (Application.platform == RuntimePlatform.Android)
            {
                if (NativeManager.OnFuncCall<bool>("SaveImage", bytes, "allin成为推广员二维码"))
                {
                    UIComponent.Instance.ToastLanguage("Become104");//Toast("图片已保存到相册");
                }
                else
                {
                    UIComponent.Instance.ToastLanguage("Become105");//Toast("图片保存失败");
                }
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                NativeManager.SaveImageToNative(bytes, bytes.Length);
            }
        }
        private void ClickCopy(GameObject go)
        {
            CloseMenu(go);

            var tStrLanguage = LanguageManager.mInstance.GetLanguageForKey("Become102");
            var tCopy = textDownload.text.Replace(tStrLanguage, "");
            UniClipboard.SetText(tCopy);
            UIComponent.Instance.ToastLanguage("Become106");//Toast("复制成功");
        }

        private void ClickOpenQr(GameObject go)
        {
            //打开二维码
            CloseMenu(go);
            Panel_Qr.gameObject.SetActive(true);
        }

        private void ClickCloseQr(GameObject go)
        {
            //关闭二维码
            Panel_Qr.gameObject.SetActive(false);
        }

        private void ClickOpenSm(GameObject go)
        {
            //打开说明
            CloseMenu(go);
            ClickRightBtn();
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
        #endregion
    }

}
