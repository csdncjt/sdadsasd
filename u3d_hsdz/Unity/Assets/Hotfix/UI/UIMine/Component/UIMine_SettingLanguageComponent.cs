using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_SettingLanguageComponentSystem : AwakeSystem<UIMine_SettingLanguageComponent>
    {
        public override void Awake(UIMine_SettingLanguageComponent self)
        {
            self.Awake();
        }
    }

    public class UIMine_SettingLanguageComponent : UIBaseComponent
    {

        private ReferenceCollector rc;
        private Button buttonnet1;
        private Button buttonnet2;
        private Button buttonnet3;
        public int mLangType;

        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            SetUpNav( UIType.UIMine_SettingLanguage, null, ClickLeftClose);
        }

        private void ClickLeftClose()
        {
            if (LanguageManager.mInstance.mCurLanguage.GetHashCode() == mLangType)
            {
                UIComponent.Instance.Remove(UIType.UIMine_SettingLanguage);
                return;
            }
            LoadType();
            UIComponent.Instance.Remove(UIType.UIMine_SettingLanguage);
        }

        public override void OnHide()
        {

        }

        void LoadType()
        {
            string mTmpLanguage = UILoginModel.mInstance.mLanguages[mLangType]; // string.Empty;      
            LanguageManager.mInstance.mCurLanguage = mLangType;
            var tContent = rc.Get<TextAsset>(mTmpLanguage);
            LanguageManager.mInstance.SettingSetDicLanguage(tContent.text);
            UIMineModel.mInstance.APIUpdatePushInfo(mLangType, delegate
            {
            });
            UIMineModel.mInstance.ClearCardName();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        List<Transform> mBtnTrs;
        #region UI
        protected virtual void InitUI()
        {
            mBtnTrs = new List<Transform>();
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            buttonnet1 = rc.Get<GameObject>("Button_net1").GetComponent<Button>();
            buttonnet2 = rc.Get<GameObject>("Button_net2").GetComponent<Button>();
            buttonnet3 = rc.Get<GameObject>("Button_net3").GetComponent<Button>();

            mBtnTrs.Add(buttonnet1.transform);
            mBtnTrs.Add(buttonnet2.transform);
            mBtnTrs.Add(buttonnet3.transform);

            UIEventListener.Get(buttonnet1.gameObject).onClick = (go) =>
            {
                OnClientChangeLan(go, 0);
            };

            UIEventListener.Get(buttonnet2.gameObject).onClick = (go) =>
            {
                OnClientChangeLan(go, 1);
            };

            UIEventListener.Get(buttonnet3.gameObject).onClick = (go) =>
            {
                OnClientChangeLan(go, 2);
            };


            var tSetting = LanguageManager.mInstance.mCurLanguage;
            for (int i = 0; i < mBtnTrs.Count; i++)
            {
                mBtnTrs[i].Find("Image_on").gameObject.SetActive(false);
            }
            mBtnTrs[tSetting.GetHashCode()].Find("Image_on").gameObject.SetActive(true);
            mLangType = tSetting.GetHashCode();
        }

        private void OnClientChangeLan(GameObject go, int pIndexLanguage)
        {
            //GetLanguageText();
            if (pIndexLanguage == mLangType)
            {
                return;
            }
            //if (pIndexLanguage <2)
            //    UIComponent.Instance.Toast("暂只支持繁体中文");
            //return;
            for (int i = 0; i < mBtnTrs.Count; i++)
            {
                mBtnTrs[i].Find("Image_on").gameObject.SetActive(false);
            }
            mBtnTrs[pIndexLanguage].Find("Image_on").gameObject.SetActive(true);
            mLangType = pIndexLanguage;
        }

        //生成excel脚本
        private void GetLanguageText()
        {
            //"USER_TW"
            var TWDic = GetDicForText("USER_TW");
            var ZHDic = GetDicForText("USER_ZH");
            var ENDic = GetDicForText("USER_EN");
            StringBuilder sBuilder = new StringBuilder();
            foreach (string key in TWDic.Keys)
            {
                sBuilder.Append(key);
                sBuilder.Append("\t");
                string zhValue;
                if (ZHDic.TryGetValue(key, out zhValue))
                {
                    sBuilder.Append(zhValue);
                }
                else
                {
                    sBuilder.Append("null");
                }
                sBuilder.Append("\t");
                sBuilder.Append(TWDic[key]);
                sBuilder.Append("\t");
                string enValue;
                if (ENDic.TryGetValue(key, out enValue))
                {
                    sBuilder.Append(enValue);
                }
                else
                {
                    sBuilder.Append("null");
                }
                sBuilder.Append("\n");
            }
            Log.Debug(sBuilder.ToString());
            TextEditor t = new TextEditor();
            t.text = sBuilder.ToString();
            t.OnFocus();
            t.Copy();

        }

        private Dictionary<string, string> GetDicForText(string textName)
        {
            string pLanguage = rc.Get<TextAsset>(textName).text;
            Dictionary<string, string> mDicKeyLanguage = new Dictionary<string, string>();
            StringBuilder sBuilder = new StringBuilder();
            mDicKeyLanguage.Clear();
            string[] lines = pLanguage.Split(new string[] { "\n" }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                if (line == null || line.Contains("=") == false) continue;

                string[] keyAndValue = line.Split('=');
                if (keyAndValue.Length <= 2)
                {
                    mDicKeyLanguage[keyAndValue[0]] = keyAndValue[1];
                }
                else
                {
                    if (null == sBuilder)
                        sBuilder = new StringBuilder();
                    if (sBuilder.Length > 0)
                        sBuilder.Clear();
                    for (int i = 1; i < keyAndValue.Length; i++)
                    {
                        sBuilder.Append(keyAndValue[i]);
                        if (i < keyAndValue.Length - 1)
                        {
                            sBuilder.Append("=");
                        }
                    }
                    mDicKeyLanguage[keyAndValue[0]] = sBuilder.ToString();
                }
            }
            return mDicKeyLanguage;
        }

        #endregion


    }
}

