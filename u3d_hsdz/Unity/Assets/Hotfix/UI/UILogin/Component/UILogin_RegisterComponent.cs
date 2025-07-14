using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ETModel;
using System.Net;

namespace ETHotfix
{

    [ObjectSystem]
    public class UILogin_RegisterComponentSystem : AwakeSystem<UILogin_RegisterComponent>
    {
        public override void Awake(UILogin_RegisterComponent self)
        {
            self.Awake();
        }
    }

    public class UILogin_RegisterComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private InputField inputfieldAccount;
        private InputField inputfieldCode;
        private InputField inputfieldPassword;
        private Button buttonRegister;
        private Text textCode;
        private Dictionary<string, Sprite> spriteCommit;
        private Image mRegisterImage;
        private Text mRegisterTxt;
        private InputField inputFieldInvitation;
        private Text textLocal;
        private Text textPhoneFirst;
        private Text textUserAgent;
        private Image ImageSecret;
        public void Awake()
        {
            rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            inputfieldAccount = rc.Get<GameObject>("InputField_Account").GetComponent<InputField>();
            inputfieldCode = rc.Get<GameObject>("InputField_Code").GetComponent<InputField>();
            inputfieldPassword = rc.Get<GameObject>("InputField_Password").GetComponent<InputField>();
            buttonRegister = rc.Get<GameObject>("Button_Register").GetComponent<Button>();
            textUserAgent = rc.Get<GameObject>("Text_UserAgent").GetComponent<Text>();
            UIEventListener.Get(rc.Get<GameObject>("Button_Code")).onClick = onClickCode;
            UIEventListener.Get(buttonRegister.gameObject).onClick = onClickRegister;
            UIEventListener.Get(rc.Get<GameObject>("Image_RegisterLocal")).onClick = onClickRegisterLocal;
            UIEventListener.Get(textUserAgent.gameObject).onClick = onClickUserAgent;
            textCode = rc.Get<GameObject>("Text_Code").GetComponent<Text>();
            textLocal = rc.Get<GameObject>("Text_Local").GetComponent<Text>();
            textPhoneFirst = rc.Get<GameObject>("Text_PhoneFirst").GetComponent<Text>();
            ImageSecret = rc.Get<GameObject>("Button_LookPW").GetComponent<Image>();

            mRegisterImage = rc.Get<GameObject>("Button_Register").GetComponent<Image>();
            mRegisterTxt = rc.Get<GameObject>("RegisterTxt").GetComponent<Text>();
            inputFieldInvitation = rc.Get<GameObject>("InputField_Invitation").GetComponent<InputField>();
            spriteCommit = new Dictionary<string, Sprite>() {
                 {"eye_open", rc.Get<Sprite>("eye_open") },
                 {"eye_close", rc.Get<Sprite>("eye_close") },
                 {"gray", rc.Get<Sprite>("gray") },
               {"red", rc.Get<Sprite>("red") }
            };
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

        private void onClickRegisterLocal(GameObject go)
        {
            string label = textLocal.text;
            UIComponent.Instance.ShowNoAnimation(UIType.UILogin_Local, new UILogin_LocalComponent.LocalCountryData() { mStrShow = label, successDelegate = ChangeText });
        }

        private void ChangeText(string pStrKey)
        {
            string nums;
            switch (LanguageManager.mInstance.mCurLanguage)
            {
                case 1:
                    if (UILoginModel.mInstance.mDicCountryNum_EN.TryGetValue(pStrKey, out nums))
                    {
                        this.textLocal.text = pStrKey;
                        this.textPhoneFirst.text = UILoginModel.mInstance.mDicCountryNum_EN[pStrKey];
                    }
                    break;
                case 2:
                    if (UILoginModel.mInstance.mDicCountryNum_TW.TryGetValue(pStrKey, out nums))
                    {
                        this.textLocal.text = pStrKey;
                        this.textPhoneFirst.text = UILoginModel.mInstance.mDicCountryNum_TW[pStrKey];
                    }
                    break;
                default:
                    if (UILoginModel.mInstance.mDicCountryNum.TryGetValue(pStrKey, out nums))
                    {
                        this.textLocal.text = pStrKey;
                        this.textPhoneFirst.text = UILoginModel.mInstance.mDicCountryNum[pStrKey];
                    }
                    break;
            }
        }

        private void onClickUserAgent(GameObject go)
        {
            UIComponent.Instance.ShowNoAnimation(UIType.UILogin_LoadingWeb, new UILogin_LoadingWebComponent.LoadingWebData
            {
                mTitleTxt = LanguageManager.mInstance.GetLanguageForKey("UIMatchModel_102"),
                mWebUrl = GlobalData.Instance.UserAgentURL + UIMineModel.mInstance.GetUrlSuffix()
            });//用户协议
        }

        private void PWInputing(string arg0)
        {
            var mPhone = inputfieldAccount.text.Trim();
            var mPwd = inputfieldPassword.text.Trim();
            var mCode = inputfieldCode.text.Trim();
            if (MethodHelper.IsStrNotNull(new string[] { mPhone, mPwd, mCode }) && MethodHelper.IsPhoneNumber(mPhone) && mPwd.Length >= 6)
            {
                mRegisterImage.sprite = spriteCommit["red"];
                mRegisterTxt.color = MethodHelper.RGB255(249, 243, 189);
                mRegisterImage.SetNativeSize();
            }
            else
            {
                mRegisterImage.sprite = spriteCommit["gray"];
                mRegisterTxt.color = MethodHelper.RGB255(175, 175, 175);
                mRegisterImage.SetNativeSize();
            }
        }

        private void onClickRegister(GameObject go)
        {
            var tPhone = inputfieldAccount.text.Trim();
            var tPwd = inputfieldPassword.text.Trim();
            var tCode = inputfieldCode.text.Trim();
            if (string.IsNullOrEmpty(tPhone))
            {
                UIComponent.Instance.ToastLanguage("UILogin_1001");//("请输入手机号");
                return;
            }

            if (UIMineModel.mInstance.IsPasdLetter(tPwd) == false)
            {
                UIComponent.Instance.ToastLanguage("WalletSettingPW6");//("密码不得少于6个字符");
                return;
            }

            //if (tPwd.Length < 6)
            //{
            //    UIComponent.Instance.ToastLanguage("UILogin_1002");//("密码不得少于6个字符");
            //    return;
            //}
            if (string.IsNullOrEmpty(tCode))
            {
                UIComponent.Instance.ToastLanguage("UILogin_1008");//("请输入验证码");
                return;
            }

            tPhone = textPhoneFirst.text.Replace("+", "") + "-" + tPhone;
            UILoginModel.mInstance.APISendRegister(tPhone, tCode, tPwd, inputFieldInvitation.text, tDto =>
                 {
                     if (tDto.status == 0)//注册成功
                     {
                         var tUI = UIComponent.Instance.Get(UIType.UILogin);
                         if (tUI != null)
                         {
                             var tWalletMy = tUI.UiBaseComponent as UILoginComponent;
                             tWalletMy.SetInputfieldAccount(inputfieldAccount.text, textPhoneFirst.text);
                         }

                         UIComponent.Instance.ToastLanguage("UILogin_1003");//("注册成功");
                         if (tDto.data != null && tDto.data.userId > 0)
                         {
                             LoginReq(inputfieldAccount.text, tPwd, textPhoneFirst.text.Replace("+", ""));
                         }
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
            string tPhone = inputfieldAccount.text;
            if (string.IsNullOrEmpty(tPhone))
            {
                UIComponent.Instance.ToastLanguage("UILogin_1004");//请输入手机号
                return;
            }

            //    if (textCode.text != (LanguageManager.mInstance.GetLanguageForKey("UILogin_GetCode")))
            if (textCode.text.Equals(LanguageManager.mInstance.GetLanguageForKey("UILogin_GetCode")) == false)
            {
                UIComponent.Instance.ToastLanguage("UILogin_1005");//("请稍等再发");
                return;
            }
            if (mIsCanClickCode == false)
            {
                UIComponent.Instance.ToastLanguage("UILogin_1005");//Toast("请稍等再发");
                return;
            }

            var tNumFirst = textPhoneFirst.text.Replace("+", "");
            UILoginModel.mInstance.APIPHoneExist(tNumFirst + "-" + tPhone, pIsExite =>
              {
                  if (pIsExite)
                  {
                      UIComponent.Instance.ToastLanguage("UILogin_1006");//("此号码已注册");
                      return;
                  }
                  mIsCanClickCode = false;

                  UILoginModel.mInstance.APISendCode(tNumFirst, tPhone, 1, tDto =>
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
            Game.Scene.GetComponent<UIComponent>().Hide(UIType.UILogin_Register, null, 0);
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UILogin_Register, null, onClickClose);

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
            inputfieldAccount.text = "";
            inputfieldCode.text = "";
            inputfieldPassword.text = "";
            inputFieldInvitation.text = "";
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
            buttonRegister = null;
            textCode = null;
            spriteCommit = null;
            mRegisterImage = null;
            mRegisterTxt = null;
            inputFieldInvitation = null;
            textLocal = null;
            textPhoneFirst = null;
            textUserAgent = null;
            ImageSecret = null;
            base.Dispose();
        }




        #region 假装登录
        void LoginReq(string mAccount, string mPassword, string phoneFirst)
        {
            REQ_LOGIN res_login = new REQ_LOGIN
            {
                userType = 12,
                userUUID = mAccount,
                macAddress = SystemInfoUtil.getLocalMac(),
                imsi = SystemInfoUtil.getDeviceUniqueIdentifier(),
                HeighthAndWidth = $"{Screen.width}*{Screen.height}",
                model = SystemInfoUtil.getDeviceMode(),
                system_version = SystemInfoUtil.getOperatingSystem(),
                sessionKey = MD5Helper.StringMD5(mPassword),
                nickname = string.Empty,
                countryCode = phoneFirst,
                version = "3.9.7",
                channel = 3
            };
            REQ_LOGIN(res_login);
        }
        private void REQ_LOGIN(REQ_LOGIN req)
        {
            var mNetOuterComponent = Game.Scene.ModelScene.GetComponent<NetOuterComponent>();
            if (null == mNetOuterComponent)
                return;
            try
            {
                string[] mArrAddress = NetHelper.GetAddressIPs(GlobalData.Instance.LoginHost, GlobalData.Instance.UseDNS);
                ETModel.Session mSession = mNetOuterComponent.Create(new IPEndPoint(IPAddress.Parse(mArrAddress[0]), GlobalData.Instance.LoginPort));
                var mCPLoginNetworkComponent =
                        Game.Scene.GetComponent<CPLoginSessionComponent>() ?? Game.Scene.AddComponent<CPLoginSessionComponent>();
                mCPLoginNetworkComponent.HotfixSession = ComponentFactory.Create<Session, ETModel.Session>(mSession);
                mCPLoginNetworkComponent.HotfixSession.SetProtrolVersionInHead(1);
                mCPLoginNetworkComponent.HotfixSession.SetUserIdInHead(GameCache.Instance.nUserId);
                mCPLoginNetworkComponent.HotfixSession.SetLanguageInHead(1);
                byte mPlatform = 2;
                if (Application.platform == RuntimePlatform.Android)
                    mPlatform = 1;
                mCPLoginNetworkComponent.HotfixSession.SetPlatformInHead(mPlatform);
                mCPLoginNetworkComponent.HotfixSession.SetBuildVersionInHead(254);
                mCPLoginNetworkComponent.HotfixSession.SetChannelInHead(14);
                mCPLoginNetworkComponent.HotfixSession.SetProductIdInHead(1002);
                mCPLoginNetworkComponent.HotfixSession.SetTokenInHead(GameCache.Instance.strToken);
                mCPLoginNetworkComponent.HotfixSession.Send(req);
            }
            catch (Exception e)
            {
                UIComponent.Instance.ClosePrompt();
                Log.Error(e);
            }
        }


        #endregion

    }
}

