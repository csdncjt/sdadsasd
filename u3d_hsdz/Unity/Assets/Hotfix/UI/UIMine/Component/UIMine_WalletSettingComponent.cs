using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_WalletSettingComponentSystem : AwakeSystem<UIMine_WalletSettingComponent>
    {
        public override void Awake(UIMine_WalletSettingComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 我的转USDT设置
    /// </summary>
    public class UIMine_WalletSettingComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Text textMobile;
        private InputField inputfieldCode;
        private InputField inputfieldPassword;
        private InputField inputfieldPasswordAgain;
        private Dictionary<string, Sprite> mDicStrSprite;
        private Image spriteCommit;
        private Text textCommit;
        private Text textCode;
        Image ImageLookPWOne;
        Image ImageLookPWTwo;

        public void Awake()
        {
            InitUI();
            mDicStrSprite = new Dictionary<string, Sprite>() {
                { "eye_open",rc.Get<Sprite>("eye_open")},
                { "eye_close",rc.Get<Sprite>("eye_close")},
                { "ImgRedBtn",rc.Get<Sprite>("ImgRedBtn")},
                { "ImgGrayBtn",rc.Get<Sprite>("ImgGrayBtn")}
            };
            mStrNextPage = string.Empty;
            mAdd3_Modify4 = 3;//类型 设置为3   修改为4
        }
        string mStrNextPage;
        int mAdd3_Modify4;
        public override void OnShow(object obj)
        {
            if (obj != null && obj is string)
            {
                var strs = (obj as string).Split(':');
                mStrNextPage = strs[0].Equals("Null") ? string.Empty : strs[0];
                mAdd3_Modify4 = int.Parse(strs[1]);

                if (mAdd3_Modify4 == 4)
                    SetUpNav(LanguageManager.mInstance.GetLanguageForKey("UIMine_WalletSettingModify"), UIType.UIMine_WalletSetting);
                else
                    SetUpNav(UIType.UIMine_WalletSetting);
            }
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
            textMobile = rc.Get<GameObject>("Text_Mobile").GetComponent<Text>();
            inputfieldCode = rc.Get<GameObject>("InputField_Code").GetComponent<InputField>();
            inputfieldPassword = rc.Get<GameObject>("InputField_Password").GetComponent<InputField>();
            inputfieldPasswordAgain = rc.Get<GameObject>("InputField_PasswordAgain").GetComponent<InputField>();

            UIEventListener.Get(rc.Get<GameObject>("Button_Code")).onClick = OnClick_Code;
            UIEventListener.Get(rc.Get<GameObject>("Button_Commit")).onClick = OnClick_Commit;

            inputfieldCode.onValueChanged.AddListener(OnChangeValueInput);
            inputfieldPassword.onValueChanged.AddListener(OnChangeValueInput);
            inputfieldPasswordAgain.onValueChanged.AddListener(OnChangeValueInput);
            textCommit = rc.Get<GameObject>("CommitTxt").GetComponent<Text>();
            textCode = rc.Get<GameObject>("Text_Code").GetComponent<Text>();
            spriteCommit = rc.Get<GameObject>("Button_Commit").GetComponent<Image>();

            ImageLookPWOne = rc.Get<GameObject>("Button_LookPWOne").GetComponent<Image>();
            ImageLookPWTwo = rc.Get<GameObject>("Button_LookPWTwo").GetComponent<Image>();

            textMobile.text = string.Format(LanguageManager.mInstance.GetLanguageForKey("WalletSettingPW1"), UIMineModel.mInstance.GetMobileStarHide());//"您的保密手机（{0}）

            UIEventListener.Get(rc.Get<GameObject>("Button_LookPWTwo")).onClick = ClickButton_LookPWTwo;
            UIEventListener.Get(rc.Get<GameObject>("Button_LookPWOne")).onClick = ClickButton_LookPWOne;
            mIsCanClickCode = true;
        }

        private void ClickButton_LookPWTwo(GameObject go)
        {
            inputfieldPasswordAgain.contentType = inputfieldPasswordAgain.contentType == InputField.ContentType.Password ? InputField.ContentType.Standard : InputField.ContentType.Password;
            inputfieldPasswordAgain.ForceLabelUpdate();
            ImageLookPWTwo.sprite = inputfieldPasswordAgain.contentType == InputField.ContentType.Password ? mDicStrSprite["eye_close"] : mDicStrSprite["eye_open"];
        }

        private void ClickButton_LookPWOne(GameObject go)
        {
            inputfieldPassword.contentType = inputfieldPassword.contentType == InputField.ContentType.Password ? InputField.ContentType.Standard : InputField.ContentType.Password;
            inputfieldPassword.ForceLabelUpdate();
            ImageLookPWOne.sprite = inputfieldPassword.contentType == InputField.ContentType.Password ? mDicStrSprite["eye_close"] : mDicStrSprite["eye_open"];
        }

        private void OnChangeValueInput(string arg0)
        {
            if (MethodHelper.IsStrNotNull(new string[] { inputfieldCode.text, inputfieldPassword.text, inputfieldPasswordAgain.text }))
            {
                spriteCommit.sprite = mDicStrSprite["ImgRedBtn"];
                textCommit.color = new Color32(249, 243, 189, 255);
            }
            else
            {
                spriteCommit.sprite = mDicStrSprite["ImgGrayBtn"];
                textCommit.color = new Color32(175, 175, 175, 255);
            }
        }
        bool mIsCanClickCode = true;
        private void OnClick_Code(GameObject go)
        {
            if (MethodHelper.IsStrNotNull(new string[] { GameCache.Instance.strPhone, GameCache.Instance.strPhoneFirst }))
            {
                if (textCode.text.Equals(LanguageManager.mInstance.GetLanguageForKey("UILogin_GetCode")) == false)
                {
                    UIComponent.Instance.ToastLanguage("UIMine_Setting106");//("请稍等再发");
                    return;
                }
                if (mIsCanClickCode == false)
                {
                    UIComponent.Instance.ToastLanguage("UIMine_Setting106");//("请稍等再发");
                    return;
                }

                mIsCanClickCode = false;
                UILoginModel.mInstance.APISendCode(GameCache.Instance.strPhoneFirst, GameCache.Instance.strPhone, mAdd3_Modify4, tDto =>
                {
                    if (tDto.status == 0)
                    {
                        UIComponent.Instance.ToastLanguage("WalletSettingPW2");//验证码已发送
                        UILoginModel.mInstance.ShowTimes(textCode);
                        mIsCanClickCode = true;
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.msg);
                    }
                });
            }
            else
            {
                UIComponent.Instance.Toast("有误,没存到手机");
            }
        }


        private void OnClick_Commit(GameObject go)
        {
            if (MethodHelper.IsStrNotNull(new string[] { inputfieldCode.text, inputfieldPassword.text, inputfieldPasswordAgain.text }) == false)
            {
                UIComponent.Instance.ToastLanguage("WalletSettingPW3");//请先填全信息
                return;
            }
            if (inputfieldPassword.text.Length < 6 || inputfieldPassword.text.Length > 20)
            {
                UIComponent.Instance.ToastLanguage("WalletSettingPW4");//密码长度在6~20位
                return;
            }

            if (inputfieldPassword.text != inputfieldPasswordAgain.text)
            {
                UIComponent.Instance.ToastLanguage("WalletSettingPW5");//两次密码输入不一致
                return;
            }

            if (UIMineModel.mInstance.IsPasdLetter(inputfieldPassword.text) == false)
            {
                UIComponent.Instance.ToastLanguage("WalletSettingPW6");//密码必须为6-20位数字或字母，组合也可以!@#*
                return;
            }

            var tPwdMd5 = MD5Helper.StringMD5(inputfieldPasswordAgain.text);
            UIMineModel.mInstance.APIPwdSave(inputfieldCode.text, tPwdMd5, delegate
            {
                UIComponent.Instance.ToastLanguage("WalletSettingPW7");//二级密码设置成功，请牢记您的密码
                if (string.IsNullOrEmpty(mStrNextPage) == true)
                {
                    // UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletTransfer);
                }
                else
                    UIComponent.Instance.ShowNoAnimation(mStrNextPage);

                UIComponent.Instance.Remove(UIType.UIMine_WalletSetting);
            });
        }

        #endregion




    }
}
