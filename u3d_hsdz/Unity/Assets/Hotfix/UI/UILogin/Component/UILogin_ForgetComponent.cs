using UnityEngine;
using UnityEngine.UI;
using ETModel;
using System.Collections.Generic;
using System;

namespace ETHotfix
{
    [ObjectSystem]
    public class UILogin_ForgetComponentSystem : AwakeSystem<UILogin_ForgetComponent>
    {
        public override void Awake(UILogin_ForgetComponent self)
        {
            self.Awake();
        }
    }

    public class UILogin_ForgetComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private InputField inputfieldAccount;
        private InputField inputfieldCode;
        private InputField inputfieldPassword;
        private Button buttonCommit;
        private Dictionary<string, Sprite> spriteCommit;
        private Image commitImage;
        private Text commitTxt;
        private Text textCode;
        private Text textLocal;
        private Text textPhoneFirst;
        private Image ImageSecret;

        public void Awake()
        {
            rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            inputfieldAccount = rc.Get<GameObject>("InputField_Account").GetComponent<InputField>();
            inputfieldCode = rc.Get<GameObject>("InputField_Code").GetComponent<InputField>();
            inputfieldPassword = rc.Get<GameObject>("InputField_Password").GetComponent<InputField>();
            buttonCommit = rc.Get<GameObject>("Button_Commit").GetComponent<Button>();
            commitImage = rc.Get<GameObject>("Button_Commit").GetComponent<Image>();
            commitTxt = rc.Get<GameObject>("CommitTxt").GetComponent<Text>();
            textCode = rc.Get<GameObject>("Text_Code").GetComponent<Text>();
            textLocal = rc.Get<GameObject>("Text_Local").GetComponent<Text>();
            textPhoneFirst = rc.Get<GameObject>("Text_PhoneFirst").GetComponent<Text>();
            UIEventListener.Get(rc.Get<GameObject>("Image_ForgetLocal")).onClick = OnClickLocal;
            ImageSecret = rc.Get<GameObject>("Button_LookPW").GetComponent<Image>();

            spriteCommit = new Dictionary<string, Sprite>() {
                {"eye_open", rc.Get<Sprite>("eye_open") },
                {"eye_close", rc.Get<Sprite>("eye_close") },
                {"gray", rc.Get<Sprite>("gray") },
                {"red", rc.Get<Sprite>("red") }
            };

            UIEventListener.Get(rc.Get<GameObject>("Button_Code")).onClick = onClickCode;
            UIEventListener.Get(buttonCommit.gameObject).onClick = onClickCommit;
            UIEventListener.Get(rc.Get<GameObject>("Button_LookPW")).onClick = go =>
            {
                inputfieldPassword.contentType = (inputfieldPassword.contentType == InputField.ContentType.Password ?
                      InputField.ContentType.Standard : InputField.ContentType.Password);
                inputfieldPassword.ForceLabelUpdate();
                ImageSecret.sprite = inputfieldPassword.contentType == InputField.ContentType.Password ? spriteCommit["eye_close"] : spriteCommit["eye_open"];
            };

            inputfieldAccount.onValueChanged.AddListener(PWInputing);
            inputfieldCode.onValueChanged.AddListener(PWInputing);
            inputfieldPassword.onValueChanged.AddListener(PWInputing);
            mIsCanClickCode = true;

     
        }

        private void OnClickLocal(GameObject go)
        {
            string label = textLocal.text;
            UIComponent.Instance.ShowNoAnimation(UIType.UILogin_Local, new UILogin_LocalComponent.LocalCountryData()
            {
                mStrShow = label,
                successDelegate = ChangeText
            });
        }

        private void ChangeText(string pStrKey)
        {
            string nums;
            switch (LanguageManager.mInstance.mCurLanguage)
            {
                case 1:
                    if (UILoginModel.mInstance.mDicCountryNum_EN.TryGetValue(pStrKey, out nums))
                    {
                        textLocal.text = pStrKey;
                        textPhoneFirst.text = UILoginModel.mInstance.mDicCountryNum_EN[pStrKey];
                    }
                    break;
                case 2:
                    if (UILoginModel.mInstance.mDicCountryNum_TW.TryGetValue(pStrKey, out nums))
                    {
                        textLocal.text = pStrKey;
                        textPhoneFirst.text = UILoginModel.mInstance.mDicCountryNum_TW[pStrKey];
                    }
                    break;
                default:
                    if (UILoginModel.mInstance.mDicCountryNum.TryGetValue(pStrKey, out nums))
                    {
                        textLocal.text = pStrKey;
                        textPhoneFirst.text = UILoginModel.mInstance.mDicCountryNum[pStrKey];
                    }
                    break;
            }
        }

        private void PWInputing(string arg0)
        {
            var mPhone = inputfieldAccount.text.Trim();
            var mPwd = inputfieldPassword.text.Trim();
            var mCode = inputfieldCode.text.Trim();
            if (MethodHelper.IsStrNotNull(new string[] { mPhone, mPwd, mCode }) && MethodHelper.IsPhoneNumber(mPhone) && mPwd.Length >= 6)
            {
                commitImage.sprite = spriteCommit["red"];
                commitTxt.color = MethodHelper.RGB255(249, 243, 189);
                commitImage.SetNativeSize();
            }
            else
            {
                commitImage.sprite = spriteCommit["gray"];
                commitTxt.color = MethodHelper.RGB255(175, 175, 175);
                commitImage.SetNativeSize();
            }
        }


        private void onClickCommit(GameObject go)
        {
            var tPhone = inputfieldAccount.text.Trim();
            var tPwd = inputfieldPassword.text.Trim();
            var tCode = inputfieldCode.text.Trim();
            if (string.IsNullOrEmpty(tPhone))
            {
                UIComponent.Instance.ToastLanguage("UILogin_1001");//("请输入手机号");
                return;
            }

            if (tPwd.Length < 6)
            {
                UIComponent.Instance.ToastLanguage("UILogin_1002");//("密码不得少于6个字符");
                return;
            }
            if (string.IsNullOrEmpty(tCode))
            {
                UIComponent.Instance.ToastLanguage("UILogin_1008");//("请输入验证码");
                return;
            }

            tPhone = textPhoneFirst.text.Replace("+", "") + "-" + tPhone;
            UILoginModel.mInstance.APISendModifyPW(tPhone, tCode, tPwd, tDto =>
            {
                if (tDto.status == 0)
                {
                    UIComponent.Instance.Hide(UIType.UILogin_Forget);
                    UIComponent.Instance.ToastLanguage("UILogin_1009");//("更改密码成功");
                }
                else
                {
                    UIComponent.Instance.Toast(tDto.msg);
                }
            });
        }
        bool mIsCanClickCode = true;
        private void onClickCode(GameObject go)
        {
            string tPhone = inputfieldAccount.text.Trim();
            if (string.IsNullOrEmpty(tPhone))
            {
                UIComponent.Instance.ToastLanguage("UILogin_1001");//("请输入手机号");
                return;
            }

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

            var tNumFirst = textPhoneFirst.text.Replace("+", "");
            UILoginModel.mInstance.APIPHoneExist(tNumFirst + "-" + tPhone, pIsExist =>
             {
                 if (pIsExist == false)
                 {
                     UIComponent.Instance.ToastLanguage("UILogin_1010");//("此号码未注册");
                     return;
                 }
                 mIsCanClickCode = false;
                 UILoginModel.mInstance.APISendCode(tNumFirst, tPhone, 2, tDto =>
                    {    
                        if (tDto.status == 0)
                        {
                            UIComponent.Instance.ToastLanguage("UILogin_1007");//("验证码已发送");
                            UILoginModel.mInstance.ShowTimes(textCode);       
                        }
                        else
                        {
                            UIComponent.Instance.Toast(tDto.msg);                  
                        }
                        mIsCanClickCode = true;
                    });
             });
        }



        private void onClickClose()
        {
            Game.Scene.GetComponent<UIComponent>().HideNoAnimation(UIType.UILogin_Forget);
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UILogin_Forget, null, onClickClose);

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
            rc = null;
            inputfieldAccount = null;
            inputfieldCode = null;
            inputfieldPassword = null;
            buttonCommit = null;
            spriteCommit = null;
            commitImage = null;
            commitTxt = null;
            textCode = null;
            textLocal = null;
            textPhoneFirst = null;
            ImageSecret = null;
            base.Dispose();
        }
    }
}
