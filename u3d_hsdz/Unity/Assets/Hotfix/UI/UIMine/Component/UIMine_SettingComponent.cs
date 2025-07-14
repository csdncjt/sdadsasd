using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_SettingComponentSystem : AwakeSystem<UIMine_SettingComponent>
    {
        public override void Awake(UIMine_SettingComponent self)
        {
            self.Awake();
        }
    }
    public class UIMine_SettingComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Text textRightLine;
        private Text textRightLanguage;

        private Image imageVoice;
        Dictionary<string, Sprite> mDicStrSprite;
        private bool isVoice = true;


        public void Awake()
        {
            InitUI();
            mDicStrSprite = new Dictionary<string, Sprite>() {
                { "btn_open",rc.Get<Sprite>("btn_open")},
                { "btn_close",rc.Get<Sprite>("btn_close")},
            };
            LanguageManager.mInstance.EActTextChange += ActionLangeuageChange;
        }

        private void ActionLangeuageChange()
        {
            textRightLanguage.text = LanguageManager.mInstance.GetLanguageForKeyMoreValue("UserLanguage",
         LanguageManager.mInstance.mCurLanguage.GetHashCode());
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_Setting);

            isVoice = SoundComponent.Instance.audioManager.SoundVolume > 0f;
            imageVoice.sprite = isVoice ? mDicStrSprite["btn_open"] : mDicStrSprite["btn_close"];
        }

        public override void OnHide()
        {

        }

        public override void Dispose()
        {
            LanguageManager.mInstance.EActTextChange -= ActionLangeuageChange;
            if (IsDisposed)
            {
                return;
            }
            base.Dispose();
        }

        #region UI

        private void ClickItemInfo(GameObject go, int index)
        {
            if (index == 0)
            {
                UIComponent.Instance.Show(UIType.UILogin_Line, null);
            }
            else if (index == 1)
            {
                //UIComponent.Instance.Toast("正在开发中...");
                UIComponent.Instance.ShowNoAnimation(UIType.UIMine_SettingLanguage);
            }
            else if (index == 2)
            {
                isVoice = !isVoice;
                imageVoice.sprite = isVoice ? mDicStrSprite["btn_open"] : mDicStrSprite["btn_close"];
                SoundComponent.Instance.SFXVolume(isVoice ? 1f : 0f);
            }
            else if (index == 3)
            {
                UIComponent.Instance.ShowNoAnimation(UIType.UIMine_SettingPassword);
            }
            else if (index == 4)
            {
                UIComponent.Instance.ShowNoAnimation(UIType.UIMine_SettingReport);
            }
            else if (index == 5)
            {//关于我们
                UIComponent.Instance.ShowNoAnimation(UIType.UILogin_LoadingWeb, new UILogin_LoadingWebComponent.LoadingWebData()
                {
                    mWebUrl = GlobalData.Instance.AboutWeURL + UIMineModel.mInstance.GetUrlSuffix(),
                    mTitleTxt = LanguageManager.mInstance.GetLanguageForKey("tc_YQAGnw3p")
                });
            }
            else if (index == 6)
            {
                UIComponent.Instance.ShowNoAnimation(UIType.UIMine_SettingDownload);
            }
            else if (index == 7)
            {//用户协议                
                UIComponent.Instance.ShowNoAnimation(UIType.UILogin_LoadingWeb, new UILogin_LoadingWebComponent.LoadingWebData
                {
                    mTitleTxt = LanguageManager.mInstance.GetLanguageForKey("UIMine_Setting101"),
                    mWebUrl = GlobalData.Instance.UserAgentURL + UIMineModel.mInstance.GetUrlSuffix()
                });
            }
            else if (index == 8)
            {
                UIComponent.Instance.ShowNoAnimation(UIType.UIMine_SettingVersion);
            }
        }

        public void UpdateNetLine(string name)
        {
            textRightLine.text = name;
        }

        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            imageVoice = rc.Get<GameObject>("Image_voice").GetComponent<Image>();
            textRightLine = rc.Get<GameObject>("Text_RightLine").GetComponent<Text>();
            textRightLanguage = rc.Get<GameObject>("Text_RightLanguage").GetComponent<Text>();

            string tStr = string.Empty;
            for (int i = 0; i < 9; i++)
            {
                tStr = "ItemInfo" + i.ToString();
                UIEventListener.Get(rc.Get<GameObject>(tStr), i).onIntClick = ClickItemInfo;
            }
            UIEventListener.Get(rc.Get<GameObject>("Button_logout")).onClick = (go) =>
            {
                NetworkUtil.RemoveAllSessionComponent();
                GameCache.Instance.strPwd = "";
                GameCache.Instance.nUserId = 0;
                GameCache.Instance.strToken = "";
                GameCache.Instance.headPic = "";
                GameCache.Instance.nick = "";
                PlayerPrefsMgr.mInstance.SetString(PlayerPrefsKeys.KEY_PWD, string.Empty);
                PlayerPrefsMgr.mInstance.SetInt(PlayerPrefsKeys.KEY_USERID, 0);
                PlayerPrefsMgr.mInstance.SetString(PlayerPrefsKeys.KEY_TOKEN, string.Empty);
                GameCache.Instance.isFirstShowActivity = true;
                UIComponent.Instance.RemoveAll(new List<string>() { UIType.UIMarquee });
                UIComponent.Instance.ShowNoAnimation(UIType.UILogin, "NoAutoLogin");
                UIMineModel.mInstance.Dispose();
                UIMatchModel.mInstance.Dispose();
            };

            textRightLanguage.text = LanguageManager.mInstance.GetLanguageForKeyMoreValue("UserLanguage",
                LanguageManager.mInstance.mCurLanguage.GetHashCode());
            textRightLine.text = GlobalData.Instance.NameForServerID(GlobalData.Instance.CurrentUsingServerID());
        }
        #endregion


    }
}


