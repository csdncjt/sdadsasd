using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_SettingPasswordComponentSystem : AwakeSystem<UIMine_SettingPasswordComponent>
    {
        public override void Awake(UIMine_SettingPasswordComponent self)
        {
            self.Awake();
        }
    }

    public class UIMine_SettingPasswordComponent : UIBaseComponent
    {

        private ReferenceCollector rc;
        private InputField inputfieldAccount;
        private InputField inputfieldCode;
        private InputField inputfieldPassword;
        private Button buttonCode;
        private Button buttonCommit;

        /// <summary> [1]红色按钮 </summary>
        private Dictionary<string, Sprite> mDicStrSprite;
        private Image commitImage;
        private Text commitTxt;
        private Text textCode;
        private Text textLocal;
        GameObject LocalGO;
        private Text textPhoneFirst;
        private Image ImageSecret;

        public void Awake()
        {
            rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            inputfieldAccount = rc.Get<GameObject>("InputField_Account").GetComponent<InputField>();
            inputfieldCode = rc.Get<GameObject>("InputField_Code").GetComponent<InputField>();
            inputfieldPassword = rc.Get<GameObject>("InputField_Password").GetComponent<InputField>();
            buttonCode = rc.Get<GameObject>("Button_Code").GetComponent<Button>();
            buttonCommit = rc.Get<GameObject>("Button_Commit").GetComponent<Button>();
            commitImage = rc.Get<GameObject>("Button_Commit").GetComponent<Image>();
            commitTxt = rc.Get<GameObject>("CommitTxt").GetComponent<Text>();
            textCode = rc.Get<GameObject>("Text_Code").GetComponent<Text>();

            textPhoneFirst = rc.Get<GameObject>("Text_PhoneFirst").GetComponent<Text>();
            textLocal = rc.Get<GameObject>("Text_Local").GetComponent<Text>();
            UIEventListener.Get(rc.Get<GameObject>("SelectLocal")).onClick = ClickSelectLocal;
            LocalGO = rc.Get<GameObject>("LocalGO");

            mDicStrSprite = new Dictionary<string, Sprite>() {
                {"ImageRed",     rc.Get<Sprite>("ImageRed") },
                {"ImageGray", rc.Get<Sprite>("ImageGray") },
                 { "eye_open",rc.Get<Sprite>("eye_open")},
                { "eye_close",rc.Get<Sprite>("eye_close")}
            };

            UIEventListener.Get(buttonCode.gameObject).onClick = onClickCode;
            UIEventListener.Get(buttonCommit.gameObject).onClick = onClickCommit;
            ImageSecret = rc.Get<GameObject>("Button_LookPW").GetComponent<Image>();
            UIEventListener.Get(rc.Get<GameObject>("Button_LookPW")).onClick = go =>
            {
                inputfieldPassword.contentType = (inputfieldPassword.contentType == InputField.ContentType.Password ?
                      InputField.ContentType.Standard : InputField.ContentType.Password);
                inputfieldPassword.ForceLabelUpdate();
                ImageSecret.sprite = inputfieldPassword.contentType == InputField.ContentType.Password ? mDicStrSprite["eye_close"] : mDicStrSprite["eye_open"];
            };

            inputfieldAccount.onValueChanged.AddListener(PWInputing);
            inputfieldCode.onValueChanged.AddListener(PWInputing);
            inputfieldPassword.onValueChanged.AddListener(PWInputing);
            mIsCanClickCode = true;
        }

        private void ClickSelectLocal(GameObject go)
        {
            string label = textLocal.text;
            UIComponent.Instance.ShowNoAnimation(UIType.UILogin_Local, new UILogin_LocalComponent.LocalCountryData() { mStrShow = label, successDelegate = ChangeText });
        }

        private void ChangeText(string pStrKey)
        {
            string nums;
            if (LanguageManager.mInstance.mCurLanguage == 0)
            {
                if (UILoginModel.mInstance.mDicCountryNum.TryGetValue(pStrKey, out nums))
                {
                    this.textLocal.text = pStrKey;
                    this.textPhoneFirst.text = UILoginModel.mInstance.mDicCountryNum[pStrKey];
                }
            }
            else if (LanguageManager.mInstance.mCurLanguage == 1)
            {
                if (UILoginModel.mInstance.mDicCountryNum_EN.TryGetValue(pStrKey, out nums))
                {
                    this.textLocal.text = pStrKey;
                    this.textPhoneFirst.text = UILoginModel.mInstance.mDicCountryNum_EN[pStrKey];
                }
            }
            else if (LanguageManager.mInstance.mCurLanguage == 2)
            {
                if (UILoginModel.mInstance.mDicCountryNum_TW.TryGetValue(pStrKey, out nums))
                {
                    this.textLocal.text = pStrKey;
                    this.textPhoneFirst.text = UILoginModel.mInstance.mDicCountryNum_TW[pStrKey];
                }
            }
        }


        private void PWInputing(string arg0)
        {
            var mPhone = inputfieldAccount.text.Trim();
            var mPwd = inputfieldPassword.text.Trim();
            var mCode = inputfieldCode.text.Trim();
            if (MethodHelper.IsStrNotNull(new string[] { mPhone, mPwd, mCode }) && MethodHelper.IsPhoneNumber(mPhone) && mPwd.Length >= 6)
            {
                commitImage.sprite = mDicStrSprite["ImageRed"];
                commitTxt.color = new Color32(249, 243, 189, 255);// Color((float)249/255,(float)243/255,(float)189/255);
            }
            else
            {
                commitImage.sprite = mDicStrSprite["ImageGray"];
                commitTxt.color = new Color32(175, 175, 175, 175);
            }
        }


        private void onClickCommit(GameObject go)
        {
            var tPhone = inputfieldAccount.text.Trim();
            var tPwd = inputfieldPassword.text.Trim();
            var tCode = inputfieldCode.text.Trim();
            if (string.IsNullOrEmpty(tPhone))
            {
                UIComponent.Instance.ToastLanguage("UIMine_Setting102");//Toast("请输入手机号");
                return;
            }
            if (tPwd.Length < 6)
            {
                UIComponent.Instance.ToastLanguage("UIMine_Setting103");//Toast("密码不得少于6个字符");
                return;
            }
            if (string.IsNullOrEmpty(tCode))
            {
                UIComponent.Instance.ToastLanguage("UIMine_Setting104");//Toast("请输入验证码");
                return;
            }

            tPhone = textPhoneFirst.text.Replace("+", "") + "-" + tPhone;
            APISendModifyPW(tPhone, tCode, tPwd, tDto =>
            {
                if (tDto.status == 0)
                {
                    UIComponent.Instance.ToastLanguage("UIMine_Setting105");//Toast("更改密码成功~");
                    UIComponent.Instance.Remove(UIType.UIMine_SettingPassword);
                }
                else
                {
                    UIComponent.Instance.Toast(tDto.msg);
                }
            });

        }

        /// <summary>
        /// 手机号码必须加上地区号并用隔开如:86-12345676789
        /// </summary>
        void APISendModifyPW(string pPhone, string pCode, string pPwd, Action<WEB2_user_mod_pwd.ResponseData> pAct)
        {
            HttpRequestComponent.Instance.Send(WEB2_user_mod_pwd.API,
                                   WEB2_user_mod_pwd.Request(new WEB2_user_mod_pwd.RequestData()
                                   {
                                       phone = pPhone,
                                       code = pCode,
                                       password = MD5Helper.StringMD5(pPwd)
                                   }), s =>
                                   {
                                       var tDto = WEB2_user_mod_pwd.Response(s);
                                       if (pAct != null)
                                           pAct(tDto);
                                   });
        }


        bool mIsCanClickCode = true;
        private void onClickCode(GameObject go)
        {
            string tPhone = inputfieldAccount.text.Trim();
            if (string.IsNullOrEmpty(tPhone))
            {
                UIComponent.Instance.ToastLanguage("UIMine_Setting102");//请输入手机号");
                return;
            }
            if (textCode.text.Equals(LanguageManager.mInstance.GetLanguageForKey("UILogin_GetCode")) == false)
            {
                UIComponent.Instance.ToastLanguage("UIMine_Setting106");//Toast("请稍等再发");
                return;
            }
            if (mIsCanClickCode == false)
            {
                UIComponent.Instance.ToastLanguage("UIMine_Setting106");//Toast("请稍等再发");
                return;
            }

            var tNumFirst = textPhoneFirst.text.Replace("+", "");
            mIsCanClickCode = false;
            UILoginModel.mInstance.APISendCode(tNumFirst, tPhone, 2, tDto =>
            {
                if (tDto.status == 0)
                {
                    UIComponent.Instance.ToastLanguage("UIMine_Setting107");//Toast("验证码已发送");
                    UILoginModel.mInstance.ShowTimes(textCode);            
                }
                else
                {
                    UIComponent.Instance.Toast(tDto.msg);
                }
                mIsCanClickCode = true;
            });
        }

        private void onClickClose(GameObject go)
        {
            Game.Scene.GetComponent<UIComponent>().HideNoAnimation(UIType.UILogin_Forget);
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_SettingPassword);
            inputfieldAccount.text = string.Empty;
            inputfieldPassword.text = string.Empty;
            inputfieldCode.text = string.Empty;

            switch (LanguageManager.mInstance.mCurLanguage)
            {
                case 1:
                    this.textLocal.text = "Australia";
                    break;
                case 2:
                    this.textLocal.text = "澳大利亞";
                    break;
                default:
                    this.textLocal.text = "澳大利亚";
                    break;
            }
            this.textPhoneFirst.text = "+61";
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
            base.Dispose();
        }
    }
}

