using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;
using UnityEngine;
using UnityEngine.UI;


namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_WalletCardNamesComponentSystem : AwakeSystem<UIMine_WalletCardNamesComponent>
    {
        public override void Awake(UIMine_WalletCardNamesComponent self)
        {
            self.Awake();
        }
    }
    /// <summary> 页面名: </summary>
    public class UIMine_WalletCardNamesComponent : UIBaseComponent
    {
        public sealed class LocalCardNameData
        {
            public string mCardNameKey;
            public CardNameDelegate successDelegate;
        }
        public delegate void CardNameDelegate(string pStr);
        private ReferenceCollector rc;
        private Button mButtonClose;
        private LocalCardNameData mCardNameData;
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
            mTargetKey = string.Empty;
        }

        private void onClickClose(GameObject go)
        {
            mCardNameData.successDelegate(mTargetKey);
            UIComponent.Instance.Remove(UIType.UIMine_WalletCardNames);
        }
        List<GameObject> mCloneItems = new List<GameObject>();
        GameObject mTempSelected;
        string mTargetKey;

        public override void OnShow(object obj)
        {
            if (obj != null && obj is LocalCardNameData)
            {
                //var tDicCardNames = UIMineModel.mInstance.GetCardNameDics();
                UIMineModel.mInstance.APIGetBeans(tDtos =>
                {
                    var tDicCardNames = new Dictionary<string, string>();
                    for (int i = 0; i < tDtos.Count; i++)
                    {
                        tDicCardNames[tDtos[i].bankCode] = tDtos[i].bankName;
                    }

                    mCardNameData = obj as LocalCardNameData;
                    mTargetKey = mCardNameData.mCardNameKey;

                    foreach (var i in mCloneItems)
                    {
                        GameObject.Destroy(i);
                    }
                    mCloneItems.Clear();

                    GameObject tItem;               
                    foreach (var item in tDicCardNames)
                    {
                        tItem = GameObject.Instantiate(ItemInfo, Scrollview_Content.transform);
                        tItem.SetActive(true);
                        tItem.transform.Find("Text_Left").GetComponent<Text>().text = item.Value;

                        var tSelected = tItem.transform.Find("Selected").gameObject;
                        if (mCardNameData.mCardNameKey == item.Key)
                        {
                            tSelected.SetActive(true);
                            mTargetKey = item.Key;
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
                            mTargetKey = item.Key;

                            onClickClose(null);
                        };
                        mCloneItems.Add(tItem);         
                    }
     
                });
            }
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


