using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_SettingDownloadComponentSystem : AwakeSystem<UIMine_SettingDownloadComponent>
    {
        public override void Awake(UIMine_SettingDownloadComponent self)
        {
            self.Awake();
        }
    }

    public class UIMine_SettingDownloadComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Text texturl;
        private Button buttoncopy;
        private Image imageQRCode;


        public void Awake()
        {
            InitUI();
            
        }

        public override void OnShow(object obj)
        {
            SetUpNav( UIType.UIMine_SettingDownload);
            UIMineModel.mInstance.APIPromotionIsOpen(pDto =>
            {
                if (pDto.status == 0)
                {
                    if (pDto.data != null && pDto.data.Count > 0)
                    {
                        for (int i = 0; i < pDto.data.Count; i++)
                        {
                            if (pDto.data[i].functionCode == 4)
                            {
                                texturl.text = pDto.data[i].value;
                                var size = imageQRCode.GetComponent<RectTransform>().sizeDelta;
                                var tSprite = QRCodeHelper.GetQRCodeSprite(pDto.data[i].value, (int)size.x, (int)size.y);
                                imageQRCode.sprite = tSprite;
                            }
                        }
                    }
                }
                else
                {
                    UIComponent.Instance.Toast(pDto.status);
                }
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
            texturl = rc.Get<GameObject>("Text_url").GetComponent<Text>();
            buttoncopy = rc.Get<GameObject>("Button_copy").GetComponent<Button>();
            UIEventListener.Get(buttoncopy.gameObject).onClick = ClickCopyBtn;
            imageQRCode = rc.Get<GameObject>("Image_QRCode").GetComponent<Image>();


            UIEventListener.Get(rc.Get<GameObject>("Button_Save")).onClick = ClickButton_Save;
        }


        private void ClickButton_Save(GameObject go)
        {
            byte[] bytes = imageQRCode.sprite.texture.EncodeToJPG();
            if (Application.platform == RuntimePlatform.Android)
            {
                if (NativeManager.OnFuncCall<bool>("SaveImage", bytes, "allin 二维码"))
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

        private void ClickCopyBtn(GameObject go)
        {
            UniClipboard.SetText(texturl.text);
            UIComponent.Instance.ToastLanguage("Become106");//Toast("复制成功");
        }



        #endregion


    }
}

