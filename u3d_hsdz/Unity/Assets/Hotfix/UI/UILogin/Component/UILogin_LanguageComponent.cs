using System;
using System.Collections.Generic;
using System.Net;
using BestHTTP;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using xcharts;
using DG.Tweening;

namespace ETHotfix
{
    [ObjectSystem]
    public class UILogin_LanguageComponentSystem : AwakeSystem<UILogin_LanguageComponent>
    {
        public override void Awake(UILogin_LanguageComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名:选择语言</summary>
    public class UILogin_LanguageComponent : UIBaseComponent
    {
        public sealed class UILanguageData
        {
            public mDelegateLanguage successDelegate;
        }
        public delegate void mDelegateLanguage();


        public UILanguageData mLanguageData;
        private ReferenceCollector rc;
        private Button buttonClose;
        private Button buttonCommit;
        private List<Toggle> mToggles;

        public void Awake()
        {
            InitUI();
        }
        private void onClickClose(GameObject go)
        {
            Game.Scene.GetComponent<UIComponent>().HideNoAnimation(UIType.UILogin_Language);
        }

        public override void OnShow(object obj)
        {
            if (obj != null && obj is UILanguageData)
            {
                mLanguageData = obj as UILanguageData;
                var tIndex = LanguageManager.mInstance.mCurLanguage.GetHashCode();
                for (int i = 0; i < mToggles.Count; i++)
                {
                    mToggles[i].isOn = false;
                }
                mToggles[tIndex].isOn = true;
                mSelectIndex = tIndex;
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
            rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            if (mToggles == null) mToggles = new List<Toggle>();
            for (int i = 0; i < 3; i++)
            {
                var tToggle = rc.Get<GameObject>("Toggle_" + i.ToString()).GetComponent<Toggle>();
                UIEventListener.Get(tToggle.gameObject, i).onIntClick = SelectToggle;
                mToggles.Add(tToggle);
            }
            buttonClose = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            buttonCommit = rc.Get<GameObject>("Button_Commit").GetComponent<Button>();
            UIEventListener.Get(buttonClose.gameObject).onClick = onClickClose;
            UIEventListener.Get(buttonCommit.gameObject).onClick = onClickCommit;
        }
        private int mSelectIndex = -1;
        private void SelectToggle(GameObject go, int index)
        {
            //if (index < 2)
            //    UIComponent.Instance.Toast("暂只支持繁体中文");
            //return;
            mSelectIndex = index;//Checkmark改回正常时,UILogin.prefab的 toggle记得改回来
        }


        private void onClickCommit(GameObject go)
        {
            //Game.Scene.GetComponent<UIComponent>().HideNoAnimation(UIType.UILogin_Language);
            //return;
            LanguageManager.mInstance.mCurLanguage = mSelectIndex;

            mLanguageData.successDelegate();
            Game.Scene.GetComponent<UIComponent>().HideNoAnimation(UIType.UILogin_Language);
        }
    }
}
