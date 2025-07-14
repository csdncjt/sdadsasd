using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;
using DtoElement = ETHotfix.WEB2_message_list.ListElement;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_MsgSystemComponentSystem : AwakeSystem<UIMine_MsgSystemComponent>
    {
        public override void Awake(UIMine_MsgSystemComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名: </summary>
    public class UIMine_MsgSystemComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private GameObject NonShowed;

        public void Awake()
        {
            InitUI();
            InitSuperView();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_MsgSystem);
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

        private void SetItemInfo(GameObject go, DtoElement pDto)
        {
            string tValue = UIMineModel.mInstance.GetMsg(pDto.type);
            if (string.IsNullOrEmpty(tValue) == false)
            {
                var str1 = pDto.content;
                var str2 = pDto.remark;
                int bnum = 0;

                //if (StringHelper.isNumberic(str2, out bnum))
                //{
                //    str2 = StringHelper.ShowGold(bnum);
                //}
                //else
                //{
                //    if (StringHelper.isNumberic(str1, out bnum))
                //    {
                //        str1 = StringHelper.ShowGold(bnum);
                //    }
                //}

                var tStr = string.Format(tValue, str1, str2);
                var tTxt = go.transform.Find("Root/Text_Title").GetComponent<Text>();
                UIMatchModel.mInstance.SetLargeTextContent(tTxt, tStr, 0, 1.9f);

                UIEventListener.Get(go).onClick = (tmp) =>
                {
                    if (tTxt.text.Contains("..."))
                        UIComponent.Instance.ShowNoAnimation(UIType.UIMine_MsgSystemContent, tStr);
                };
            }
            go.transform.Find("Root/Text_Content").GetComponent<Text>().text = TimeHelper.GetDateTimer(pDto.time).ToString("yyyy/MM/dd HH:mm");
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

            UIMineModel.mInstance.API2MsgData(3, 0, 0, pDto =>
               {
                   if (pDto.status == 0)
                   {
                       mDtoEles = pDto.data.list ?? new List<DtoElement>();
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
                UIMineModel.mInstance.API2MsgData(3, 0, 0, pDto =>
                 {
                     if (pDto.status == 0)
                     {
                         mDtoEles = pDto.data.list ?? new List<DtoElement>();
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
                mLoadingTipStatus = LoadingTipStatus.WaitLoad;
                UpdateLoadingTip(item);
                if (mDtoEles == null || mDtoEles.Count < 2)
                {
                    OnDataSourceLoadMoreFinished(false);
                    return;
                }
                if (mDtoEles == null || mDtoEles.Count < 2) return;
                var tTime = mDtoEles[mDtoEles.Count - 2].time;
                UIMineModel.mInstance.API2MsgData(3, 1, tTime, pDto =>
                 {
                     if (pDto.status == 0)
                     {
                         mDtoEles.RemoveAt(mDtoEles.Count - 1);
                         mDtoEles.AddRange(pDto.data.list);
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
            if (mLoadingTipStatus == LoadingTipStatus.WaitLoad)
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


