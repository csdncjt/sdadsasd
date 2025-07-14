using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ETModel;

namespace ETHotfix
{

    [ObjectSystem]
    public class UILogin_LocalComponentSystem : AwakeSystem<UILogin_LocalComponent>
    {
        public override void Awake(UILogin_LocalComponent self)
        {
            self.Awake();
        }
    }

    public class UILogin_LocalComponent : UIBaseComponent
    {
        public sealed class LocalCountryData
        {
            public string mStrShow;
            public LocalDelegate successDelegate;
        }
        public delegate void LocalDelegate(string pStr);

        private ReferenceCollector rc;

        private Button mButtonClose;
        private LocalCountryData mCountryLocalCountry;
        private GameObject ItemInfo;


        private GameObject Scrollview_Content;


        public void Awake()
        {
            rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mButtonClose = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            UIEventListener.Get(mButtonClose.gameObject).onClick = onClickClose;
            ItemInfo = rc.Get<GameObject>("ItemInfo");
            Scrollview_Content = rc.Get<GameObject>("Scrollview_Content");
            mCloneItems = new List<GameObject>();
        }


        private void onClickClose(GameObject go)
        {
            mCountryLocalCountry.successDelegate(mTargetStr);
            Game.Scene.GetComponent<UIComponent>().HideNoAnimation(UIType.UILogin_Local);
        }

        public override void OnShow()
        {

        }
        List<GameObject> mCloneItems = new List<GameObject>();
        GameObject mTempSelected;
        string mTargetStr = "Australia";

        public override void OnShow(object obj)
        {
            if (obj != null && obj is LocalCountryData)
            {
                mCountryLocalCountry = obj as LocalCountryData;

                mCloneItems = new List<GameObject>();
                GameObject tItem;
                var tCountryKeys = UILoginModel.mInstance.mDicCountryNum.Keys.ToList();
                if (LanguageManager.mInstance.mCurLanguage == 1)
                {
                    tCountryKeys = UILoginModel.mInstance.mDicCountryNum_EN.Keys.ToList();
                }
                else if(LanguageManager.mInstance.mCurLanguage == 2)
                {
                    tCountryKeys = UILoginModel.mInstance.mDicCountryNum_TW.Keys.ToList();
                }
                for (int i = 0; i < tCountryKeys.Count; i++)
                {
                    tItem = GameObject.Instantiate(ItemInfo, Scrollview_Content.transform);
                    tItem.SetActive(true);
                    tItem.transform.Find("Text_Left").GetComponent<Text>().text = tCountryKeys[i];
                    var tTxt = tCountryKeys[i];
                    var tSelected = tItem.transform.Find("Selected").gameObject;
                    if (mCountryLocalCountry.mStrShow == tTxt)
                    {
                        tSelected.SetActive(true);
                        mTargetStr = tTxt;
                        mTempSelected = tSelected;
                    }
                    UIEventListener.Get(tItem).onClick = delegate
                    {
                        if (mTempSelected != null)
                        {
                            if (mTempSelected.GetInstanceID() == tSelected.GetInstanceID())
                            {
                                return;
                            }
                            mTempSelected.SetActive(false);
                        }
                        tSelected.SetActive(true);
                        mTempSelected = tSelected;
                        mTargetStr = tTxt;

                        onClickClose(null);
                    };
                    mCloneItems.Add(tItem);
                }
                var rectTr = Scrollview_Content.GetComponent<RectTransform>().localPosition;
                Scrollview_Content.GetComponent<RectTransform>().sizeDelta = new Vector2(992, tCountryKeys.Count * 160);
                Scrollview_Content.GetComponent<RectTransform>().localPosition = new Vector3(rectTr.x, rectTr.y - 1675, 0);
            }
        }

        public override void OnHide()
        {
            if (mCloneItems != null && mCloneItems.Count > 0)
            {
                for (int i = 0; i < mCloneItems.Count; i++)
                {
                    GameObject.DestroyImmediate(mCloneItems[i]);
                }
                mCloneItems.Clear();
                mTempSelected = null;
            }
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            rc = null;
            mButtonClose = null;
            mCountryLocalCountry = null;
            ItemInfo = null;
            Scrollview_Content = null;
            base.Dispose();
        }
    }
}
