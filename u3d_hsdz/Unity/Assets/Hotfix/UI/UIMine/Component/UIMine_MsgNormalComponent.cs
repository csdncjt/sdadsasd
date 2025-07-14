using System.Collections.Generic;
using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;
using DtoElement = ETHotfix.WEB2_message_list.ListElement;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_MsgNormalComponentSystem : AwakeSystem<UIMine_MsgNormalComponent>
    {
        public override void Awake(UIMine_MsgNormalComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名: </summary>
    public class UIMine_MsgNormalComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private EnumMSG mCurMsgType = EnumMSG.MSG_Club;
        private EnumMSGFROM mFrom = EnumMSGFROM.MSGFROM_Msg;
        private GameObject NonShowed;

        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            object[] arr = obj as object[];
            mCurMsgType = (EnumMSG)arr[0];
            mFrom = (EnumMSGFROM)arr[1];

            SetUpNav(ShowTopMsg(), UIType.UIMine_MsgNormal);
            InitSuperView();
        }

        string ShowTopMsg()
        {
            return LanguageManager.mInstance.GetLanguageForKeyMoreValue("UIMine_MsgNormal", mCurMsgType.GetHashCode() - 1);
        }

       

        void SetItemInfo(GameObject go, DtoElement pDto)
        {
            string tValue = UIMineModel.mInstance.GetMsg(pDto.type);
            var panel1 = go.transform.Find("Panel1");
            var panel2 = go.transform.Find("Panel2");
            if (string.IsNullOrEmpty(tValue) == false)
            {
                if (pDto.type == 17)//申请带入消息
                {
                    panel1.gameObject.SetActive(false);
                    panel2.gameObject.SetActive(true);

                    var args = pDto.content.Split(',');
                    panel2.Find("Text_Title").GetComponent<Text>().text = string.Format(tValue, args[1]);
                    string c1 = UIMineModel.mInstance.GetMsg(18);
                    panel2.Find("Text_Context").GetComponent<Text>().text = string.Format(c1, args[0], args[2], args[3]);
                    panel2.Find("Text_Time").GetComponent<Text>().text = TimeHelper.GetDateTimer(pDto.time).ToString("HH:mm");

                    UIEventListener.Get(panel2.Find("Button_ok").gameObject).onClick = (tmp) =>
                    {
                        UIMineModel.mInstance.API2MsgOperate(pDto.msgId, 1, pDto1 =>
                        {
                            if (pDto1.status == 0)
                            {
                                FlushMsg();
                            }

                        });
                    };

                    UIEventListener.Get(panel2.Find("Button_no").gameObject).onClick = (tmp) =>
                    {
                        UIMineModel.mInstance.API2MsgOperate(pDto.msgId, 2, pDto1 =>
                        {
                            if (pDto1.status == 0)
                            {
                                FlushMsg();
                            }

                        });
                    };


                }
                else
                {
                    panel1.gameObject.SetActive(true);
                    panel2.gameObject.SetActive(false);

                    var str1 = pDto.content;
                    var str2 = pDto.remark;
                    int bnum = 0;

                    var tContentColor = "<color=\"#E9BF80FF\">" + str1 + "</color>";
                    var tRemarkColor = "<color=\"#E9BF80FF\">" + str2 + "</color>";
                    var tTitleColor = "<color=\"#E9BF80FF\">" + pDto.title + "</color>";
                    var tTxt = panel1.Find("Text_Title").GetComponent<Text>();
                    tTxt.text = string.Format(tValue, tContentColor, tRemarkColor, tTitleColor);
                    panel1.Find("Text_Dot").gameObject.SetActive(tTxt.preferredHeight > 110);
                    panel1.Find("Text_Time").GetComponent<Text>().text = TimeHelper.GetDateTimer(pDto.time).ToString("yyyy/MM/dd HH:mm");
                    UIEventListener.Get(go).onClick = (tmp) =>
                    {
                        if (tTxt.preferredHeight > 110)
                            UIComponent.Instance.ShowNoAnimation(UIType.UIMine_MsgSystemContent, tTxt.text);
                    };
                }
            }
            else
            {
                panel1.Find("Text_Time").GetComponent<Text>().text = TimeHelper.GetDateTimer(pDto.time).ToString("yyyy/MM/dd HH:mm");
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
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            NonShowed = rc.Get<GameObject>("NonShowed");
            NonShowed.SetActive(false);
        }

        #region SupperScrollView 
        int mDirDrag = -1;//-1上面的菊花     1下面的菊花
        private LoopListView2 mLoopListView;
        private LoadingTipStatus mLoadingTipStatus = LoadingTipStatus.None;
        float mLoadingTipItemHeight = 100;

        private void InitSuperView()
        {
            mLoopListView = rc.Get<GameObject>("ScrollView").GetComponent<LoopListView2>();
            mLoopListView.mOnEndDragAction = OnEndDrag;
            mLoopListView.mOnDownMoreDragAction = OnDownMoreDragAction;
            mLoopListView.mOnUpRefreshDragAction = OnUpRefreshDragAction;
            mDtoEles = new List<DtoElement>();

            UIMineModel.mInstance.API2MsgData((int)mCurMsgType, 0, 0, pDto =>
                  {
                      if (pDto.status == 0)
                      {
                          for (int i = 0; i < pDto.data.list.Count; i++)
                          {
                              if (mFrom == EnumMSGFROM.MSGFROM_Game || mFrom == EnumMSGFROM.MSGFROM_Club)
                              {
                                  if (pDto.data.list[i].type != 17)
                                  {
                                      continue;
                                  }
                              }
                              mDtoEles.Add(pDto.data.list[i]);
                          }

                          //mDtoEles = pDto.data.list ?? new List<DtoElement>();
                          NonShowed.SetActive(mDtoEles.Count == 0);

                          mDtoEles.Add(new DtoElement());
                          mLoopListView.InitListView(mDtoEles.Count + 1, OnGetItemByIndex);
                      }
                      else
                      {
                          mDtoEles = new List<DtoElement>() { new DtoElement() };
                          UIComponent.Instance.Toast(pDto.status);
                      }
                  });
        }

        private void FlushMsg()
        {
            UIMineModel.mInstance.API2MsgData((int)mCurMsgType, 0, 0, pDto =>
            {
                if (pDto.status == 0)
                {
                    mDtoEles = new List<DtoElement>();
                    for (int i = 0; i < pDto.data.list.Count; i++)
                    {
                        if (mFrom == EnumMSGFROM.MSGFROM_Game || mFrom == EnumMSGFROM.MSGFROM_Club)
                        {
                            if (pDto.data.list[i].type != 17)
                            {
                                continue;
                            }
                        }
                        mDtoEles.Add(pDto.data.list[i]);
                    }

                    //mDtoEles = pDto.data.list ?? new List<DtoElement>();
                    NonShowed.SetActive(mDtoEles.Count == 0);
                    mDtoEles.Add(new DtoElement());//因为上下都要菊花  给其追加一条空数据
                    OnDataSourceLoadMoreFinished(true);
                }
                else
                {
                    mDtoEles = new List<DtoElement>() { new DtoElement() };
                    OnDataSourceLoadMoreFinished(false);
                }
            });

        }

        private List<DtoElement> mDtoEles = new List<DtoElement>();

        LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)//mCurTop不同时
        {
            if (index < 0)
            {
                return null;
            }
            LoopListViewItem2 item = null;
            if (index == 0)
            {
                mLoadingTipStatus = LoadingTipStatus.None;
                item = listView.NewListViewItem("ItemPrefab0");
                UpdateLoadingTip(item);
                return item;
            }
            if (index == mDtoEles.Count)
            {
                item = listView.NewListViewItem("ItemPrefab0");
                UpdateLoadingTip(item);
                return item;
            }

            var tDto = mDtoEles[index - 1];
            if (tDto == null)
            {
                return null;
            }
            item = listView.NewListViewItem("ItemInfo");
            ListItem2 itemScript = item.GetComponent<ListItem2>();
            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
            }
            var go = item.gameObject;
            SetItemInfo(go, tDto);
            //UIEventListener.Get(go).onClick = (tmp) =>
            //{
            //    //UIComponent.Instance.Show(UIType.UIMine_PromotersBecomeBook);
            //};
            return item;
        }



        void UpdateLoadingTip(LoopListViewItem2 item)
        {
            if (item == null)
            {
                return;
            }
            ListItem0 itemScript0 = item.GetComponent<ListItem0>();
            if (itemScript0 == null)
            {
                return;
            }
            if (mLoadingTipStatus == LoadingTipStatus.None)
            {
                itemScript0.mRoot.SetActive(false);
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
            }
            else if (mLoadingTipStatus == LoadingTipStatus.WaitRelease)
            {
                itemScript0.mRoot.SetActive(true);
                itemScript0.mText.text = LanguageManager.mInstance.GetLanguageForKey("SuperView1");// "放开加载更多 ...";
                itemScript0.mArrow.SetActive(true);
                itemScript0.mWaitingIcon.SetActive(false);
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mLoadingTipItemHeight);
            }
            else if (mLoadingTipStatus == LoadingTipStatus.WaitLoad)
            {
                itemScript0.mRoot.SetActive(true);
                itemScript0.mArrow.SetActive(false);
                itemScript0.mWaitingIcon.SetActive(true);
                itemScript0.mText.text = LanguageManager.mInstance.GetLanguageForKey("SuperView2");// "加载中 ...";
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mLoadingTipItemHeight);
            }
            else if (mLoadingTipStatus == LoadingTipStatus.NoMoreData)
            {
                itemScript0.mRoot.SetActive(true);
                itemScript0.mArrow.SetActive(false);
                itemScript0.mWaitingIcon.SetActive(false);
                itemScript0.mText.text = LanguageManager.mInstance.GetLanguageForKey("SuperView3");//  "已经是最后一条啦";
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mLoadingTipItemHeight);
            }
        }

        void OnEndDrag()
        {
            if (mDirDrag == -1)
            {
                if (mLoopListView.ShownItemCount == 0)
                {
                    return;
                }
                if (mLoadingTipStatus != LoadingTipStatus.None && mLoadingTipStatus != LoadingTipStatus.WaitRelease)
                {
                    return;
                }
                LoopListViewItem2 item = mLoopListView.GetShownItemByItemIndex(0);
                if (item == null)
                {
                    return;
                }
                mLoopListView.OnItemSizeChanged(item.ItemIndex);
                if (mLoadingTipStatus != LoadingTipStatus.WaitRelease)
                {
                    return;
                }
                mLoadingTipStatus = LoadingTipStatus.WaitLoad;
                UpdateLoadingTip(item);
                UIMineModel.mInstance.API2MsgData((int)mCurMsgType, 0, 0, pDto =>
                  {
                      if (pDto.status == 0)
                      {
                          mDtoEles = new List<DtoElement>();
                          for (int i = 0; i < pDto.data.list.Count; i++)
                          {
                              if (mFrom == EnumMSGFROM.MSGFROM_Game || mFrom == EnumMSGFROM.MSGFROM_Club)
                              {
                                  if (pDto.data.list[i].type != 17)
                                  {
                                      continue;
                                  }
                              }
                              mDtoEles.Add(pDto.data.list[i]);
                          }

                          //mDtoEles = pDto.data.list ?? new List<DtoElement>();
                          NonShowed.SetActive(mDtoEles.Count == 0);
                          mDtoEles.Add(new DtoElement());//因为上下都要菊花  给其追加一条空数据
                          OnDataSourceLoadMoreFinished(true);
                      }
                      else
                      {
                          mDtoEles = new List<DtoElement>() { new DtoElement() };
                          OnDataSourceLoadMoreFinished(false);
                      }
                  });
            }
            else if (mDirDrag == 1)
            {
                if (mLoopListView.ShownItemCount == 0)
                {
                    return;
                }
                if (mLoadingTipStatus != LoadingTipStatus.None && mLoadingTipStatus != LoadingTipStatus.WaitRelease)
                {
                    return;
                }
                LoopListViewItem2 item = mLoopListView.GetShownItemByItemIndex(mDtoEles.Count);
                if (item == null)
                {
                    return;
                }
                mLoopListView.OnItemSizeChanged(item.ItemIndex);
                if (mLoadingTipStatus != LoadingTipStatus.WaitRelease)
                {
                    return;
                }
                mLoadingTipStatus = LoadingTipStatus.WaitLoad;
                UpdateLoadingTip(item);
                if (mDtoEles == null || mDtoEles.Count < 2)
                {
                    OnDataSourceLoadMoreFinished(false);
                    return;
                }
                var tTime = mDtoEles[mDtoEles.Count - 2].time;
                UIMineModel.mInstance.API2MsgData((int)mCurMsgType, 1, tTime, pDto =>
                     {
                         if (pDto.status == 0)
                         {
                             mDtoEles.RemoveAt(mDtoEles.Count - 1);

                             for (int i = 0; i < pDto.data.list.Count; i++)
                             {
                                 if (mFrom == EnumMSGFROM.MSGFROM_Game || mFrom == EnumMSGFROM.MSGFROM_Club)
                                 {
                                     if (pDto.data.list[i].type != 17)
                                     {
                                         continue;
                                     }
                                 }
                                 mDtoEles.Add(pDto.data.list[i]);
                             }

                             //mDtoEles.AddRange(pDto.data.list);
                             NonShowed.SetActive(mDtoEles.Count == 0);
                             mDtoEles.Add(new DtoElement());////因为上下都要菊花  给其追加一条空数据           
                             OnDataSourceLoadMoreFinished(pDto.data.list.Count > 0);
                         }
                         else
                         {
                             mDtoEles = new List<DtoElement>() { new DtoElement() };
                             OnDataSourceLoadMoreFinished(false);
                         }
                     });
            }
        }

        /// <summary>  刷新用true,  追加使用count>0   </summary>
        async void OnDataSourceLoadMoreFinished(bool pLength)
        {
            if (mLoopListView.ShownItemCount == 0)
            {
                return;
            }
            //if (mLoadingTipStatus == LoadingTipStatus.WaitLoad)
            {
                await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(500);
                mLoadingTipStatus = pLength ? LoadingTipStatus.None : LoadingTipStatus.NoMoreData;
                mLoopListView.SetListItemCount(mDtoEles.Count + 1, false);
                mLoopListView.RefreshAllShownItem();
            }
        }


        /// <summary>         下面的菊花         </summary>
        private void OnDownMoreDragAction()
        {
            mDirDrag = 1;
            if (mLoopListView.ShownItemCount == 0)
            {
                return;
            }
            if (mLoadingTipStatus != LoadingTipStatus.None && mLoadingTipStatus != LoadingTipStatus.WaitRelease)
            {
                return;
            }
            LoopListViewItem2 item = mLoopListView.GetShownItemByItemIndex(mDtoEles.Count);
            if (item == null)
            {
                return;
            }
            LoopListViewItem2 item1 = mLoopListView.GetShownItemByItemIndex(mDtoEles.Count - 1);
            if (item1 == null)
            {
                return;
            }
            float y = mLoopListView.GetItemCornerPosInViewPort(item1, ItemCornerEnum.LeftBottom).y;
            if (y + mLoopListView.ViewPortSize >= mLoadingTipItemHeight)
            {
                if (mLoadingTipStatus != LoadingTipStatus.None)
                {
                    return;
                }
                mLoadingTipStatus = LoadingTipStatus.WaitRelease;
                UpdateLoadingTip(item);
            }
            else
            {
                if (mLoadingTipStatus != LoadingTipStatus.WaitRelease)
                {
                    return;
                }
                mLoadingTipStatus = LoadingTipStatus.None;
                UpdateLoadingTip(item);
            }
        }

        /// <summary>         上面的菊花         </summary>
        private void OnUpRefreshDragAction()
        {
            mDirDrag = -1;
            if (mLoopListView.ShownItemCount == 0)
            {
                return;
            }
            if (mLoadingTipStatus != LoadingTipStatus.None && mLoadingTipStatus != LoadingTipStatus.WaitRelease)
            {
                return;
            }
            LoopListViewItem2 item = mLoopListView.GetShownItemByItemIndex(0);
            if (item == null)
            {
                return;
            }
            ScrollRect sr = mLoopListView.ScrollRect;
            Vector3 pos = sr.content.anchoredPosition3D;
            if (pos.y < -mLoadingTipItemHeight)
            {
                if (mLoadingTipStatus != LoadingTipStatus.None)
                {
                    return;
                }
                mLoadingTipStatus = LoadingTipStatus.WaitRelease;
                UpdateLoadingTip(item);
                item.CachedRectTransform.anchoredPosition3D = new Vector3(0, mLoadingTipItemHeight, 0);
            }
            else
            {
                if (mLoadingTipStatus != LoadingTipStatus.WaitRelease)
                {
                    return;
                }
                mLoadingTipStatus = LoadingTipStatus.None;
                UpdateLoadingTip(item);
                item.CachedRectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
            }
        }

        #endregion
    }
}
