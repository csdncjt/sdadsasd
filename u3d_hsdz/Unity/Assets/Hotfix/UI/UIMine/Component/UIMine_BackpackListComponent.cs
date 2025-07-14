using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using GoodsEle = ETHotfix.WEB2_backpack_info.GameBagGoodsElement;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_BackpackListComponentSystem : AwakeSystem<UIMine_BackpackListComponent>
    {
        public override void Awake(UIMine_BackpackListComponent self)
        {
            self.Awake();
        }
    }

    public class UIMine_BackpackListComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private int mBackId = 0;
        Dictionary<int, GameObject> mDicIdTexts = new Dictionary<int, GameObject>();
        private List<GoodsEle> mBackpackData;
        RectTransform Backpack_scrollview_Content;
        GameObject Backpack_info;
        private GameObject NonShowed;
        public void Awake()
        {
            mDicIdTexts.Clear();
            InitUI();
        }

        #region InitUI
        protected virtual void InitUI()
        {
            rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            Backpack_scrollview_Content = rc.Get<GameObject>("Backpack_scrollview_Content").GetComponent<RectTransform>();
            Backpack_info = rc.Get<GameObject>("Backpack_info");
            NonShowed = rc.Get<GameObject>("NonShowed");
            NonShowed.SetActive(false);
        }
        #endregion



        public override void OnShow(object obj)
        {
            SetUpNav(LanguageManager.Get("UIMine_Backpack"), UIType.UIMine_BackpackList, ClickMenuAction, null);

            if (obj is List<GoodsEle>)
            {
                mBackpackData = obj as List<GoodsEle>;
                if (mBackpackData == null) return;

                if (mBackpackData.Count <= 0)
                {
                    NonShowed.SetActive(true);
                }
                else
                {
                    RefreshView(mBackpackData);
                }
            }
        }

        private void RefreshView(List<GoodsEle> pDtos)
        {
            GameObject clone;
            int creatCount = 0;
            for (int i = 0; i < pDtos.Count; ++i)
            {
                if (i < pDtos.Count)
                {
                    if (i + 1 > Backpack_scrollview_Content.transform.childCount)
                    {
                        clone = GameObject.Instantiate(Backpack_info, Backpack_scrollview_Content.transform);
                    }
                    else
                    {
                        clone = Backpack_scrollview_Content.transform.GetChild(creatCount).gameObject;
                    }
                    clone.SetActive(true);
                    var tData = pDtos[i];
                    clone.transform.Find("Item_Title").GetComponent<Text>().text = tData.propName;
                    clone.transform.Find("Item_Time").GetComponent<Text>().text = tData.propDesc;
                    mDicIdTexts[tData.id] = clone;
                    string btnStr = "";
                    if (tData.propType == 1)//1 门票使用
                    {
                        if (tData.propStatus == 0)
                        {
                            btnStr = LanguageManager.Get("adaptation10200");
                            UIEventListener.Get(clone.transform.Find("Item_Using").gameObject).onClick = btn =>
                            {
                                Log.Debug("去使用ing");
                                UIComponent.Instance.Toast("使用的,暂不支持,找兑换吧");
                            };
                        }
                        else if (tData.propStatus == 1)
                            btnStr = LanguageManager.Get("adaptation10201");//使用中
                        else if (tData.propStatus == 2)
                            btnStr = LanguageManager.Get("adaptation10202");//已使用
                    }
                    else
                    {
                        if (tData.propStatus == 0)
                        {
                            btnStr = LanguageManager.Get("adaptation10203");//兑换
                            UIEventListener.Get(clone.transform.Find("Item_Using").gameObject).onClick = btn =>
                          {
                              Log.Debug("去兑换ing  1获取地址  2请求兑换的接口  3回调换文字");
                              mBackId = tData.id;
                              // APIConsignee(tData.id);
                              API2GetConsigneeInfo(pDto =>
                              {
                                  if (pDto.status == 0)
                                  {
                                      UIComponent.Instance.Show(UIType.UIMine_BackpackAddress,
                                            new UIMine_BackpackAddressComponent.BackpackAddressData()
                                            {
                                                backpackId = tData.id,
                                                addressData = pDto.data,
                                                successDelegate = TextUpdate
                                            });
                                  }
                              });
                          };
                        }
                        else if (tData.propStatus == 1)
                            btnStr = LanguageManager.Get("adaptation10204");//兑换中
                        else if (tData.propStatus == 2)
                            btnStr = LanguageManager.Get("adaptation10205");//已兑换
                    }
                    clone.transform.Find("Item_Using/Item_UsingTxt").GetComponent<Text>().text = btnStr;
                    creatCount++;
                }
            }
            Backpack_scrollview_Content.sizeDelta = new Vector2(0, pDtos.Count * (265 + 5));
            Backpack_scrollview_Content.localPosition = new Vector3(-621, 0, 0);// Vector3.zero;
        }
        private void ClickMenuAction()
        {
            API2GetConsigneeInfo(pDto =>
            {
                if (pDto.status == 0)
                {
					if (NonShowed == null)//回调前 页面被关闭了
                        return;
					
                    UIComponent.Instance.ShowNoAnimation(UIType.UIMine_BackpackAddress,
                   new UIMine_BackpackAddressComponent.BackpackAddressData()
                   {
                       backpackId = 0,
                       addressData = pDto.data,
                       successDelegate = TextUpdate
                   }, () => { });
                }
            });
        }

        /// <summary>         获取用户地址         </summary>
        void API2GetConsigneeInfo(Action<WEB2_backpack_consignee_info.ResponseData> pAct)
        {
            var tReq = new WEB2_backpack_consignee_info.RequestData() { };
            HttpRequestComponent.Instance.Send(WEB2_backpack_consignee_info.API,
            WEB2_backpack_consignee_info.Request(tReq), (resData) =>
            {
                var tDto = WEB2_backpack_consignee_info.Response(resData);
                if (pAct != null)
                {
                    pAct(tDto);
                }
            });
        }


        void TextUpdate()
        {
            mDicIdTexts[mBackId].transform.Find("Item_Using/Item_UsingTxt").GetComponent<Text>().text = LanguageManager.Get("adaptation10204");//兑换中
            UIEventListener.Get(mDicIdTexts[mBackId].transform.Find("Item_Using").gameObject).onClick = go =>
            {
                Log.Debug("123");
            };
        }

        public override void OnHide()
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }


    }
}

