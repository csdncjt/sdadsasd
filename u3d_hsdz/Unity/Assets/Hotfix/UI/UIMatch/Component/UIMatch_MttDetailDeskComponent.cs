using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;
using DtoRoomInfo = ETHotfix.WEB2_room_mtt_view.Data;
using DtoElement = ETHotfix.WEB2_room_mtt_desk_info.DeskListElement;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMatch_MttDetailDeskComponentSystem : AwakeSystem<UIMatch_MttDetailDeskComponent>
    {
        public override void Awake(UIMatch_MttDetailDeskComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名: </summary>
    public class UIMatch_MttDetailDeskComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Text textDeskNum;

        DtoRoomInfo roomInfo;
        private int mCurPage = 1;

        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            roomInfo = obj as DtoRoomInfo;
            InitSuperView();
        }

        void SetItemInfo(GameObject go, DtoElement pDto, int index)
        {
            Text textDeskID = go.transform.Find("content/Text_DeskID").GetComponent<Text>();
            Text textPlayerCount = go.transform.Find("content/Text_PlayerCount").GetComponent<Text>();
            Text textScore = go.transform.Find("content/Text_Score").GetComponent<Text>();

            textDeskID.text = pDto.deskNum.ToString();
            textPlayerCount.text = pDto.playerCount.ToString();
            textScore.text = StringHelper.ShowGold(pDto.minScore)+"/" + StringHelper.ShowGold(pDto.maxScore);
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
            textDeskNum = rc.Get<GameObject>("Text_DeskNum").GetComponent<Text>();

        }

        private void ObtainListData(EnumLoadType loadType)
        {
            if (loadType == EnumLoadType.Init || loadType == EnumLoadType.Refresh)
            {
                mCurPage = 1;
            }
            else
            {
                mCurPage++;
            }

            UIMatchModel.mInstance.APIMTTDeskList(roomInfo.matchId, mCurPage, pDto =>
            {
                if (pDto.status == 0)
                {
                    GameCache.Instance.client_ip = pDto.data.clientIp;
                    if (mCurPage == 1)
                    {
                        mDtoEles = pDto.data.deskList ?? new List<DtoElement>();
                        mDtoEles.Add(new DtoElement());//因为上下都要菊花  给其追加一条空数据
                        if (loadType == EnumLoadType.Init)
                            mLoopListView.InitListView(mDtoEles.Count + 1, OnGetItemByIndex);
                        if (loadType == EnumLoadType.Refresh)
                            OnDataSourceLoadMoreFinished(true);
                    }
                    else
                    {
                        mDtoEles.RemoveAt(mDtoEles.Count - 1);
                        mDtoEles.AddRange(pDto.data.deskList);
                        mDtoEles.Add(new DtoElement());////因为上下都要菊花  给其追加一条空数据           
                        OnDataSourceLoadMoreFinished(pDto.data.deskList.Count > 0);
                    }

                    textDeskNum.text = string.Format(LanguageManager.Get("MTT_Desk_Num"), pDto.data.totalDeskNum);
                }
                else
                {
                    mDtoEles = new List<DtoElement>() { new DtoElement() };
                    OnDataSourceLoadMoreFinished(false);
                }
            });
        }

        #region SupperScrollView 
        int mDirDrag = -1;//-1上面的菊花     1下面的菊花
        private LoopListView2 mLoopListView;
        private LoadingTipStatus mLoadingTipStatus = LoadingTipStatus.None;
        float mLoadingTipItemHeight = 100;

        private void InitSuperView()
        {
            if (mLoopListView == null)
            {
                mLoopListView = rc.Get<GameObject>("Desk_scrollview").GetComponent<LoopListView2>();
                mLoopListView.mOnEndDragAction = OnEndDrag;
                mLoopListView.mOnDownMoreDragAction = OnDownMoreDragAction;
                mLoopListView.mOnUpRefreshDragAction = OnUpRefreshDragAction;
                mDtoEles = new List<DtoElement>();

                ObtainListData(EnumLoadType.Init);
            }
            else
            {
                ObtainListData(EnumLoadType.Refresh);
            }

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
            SetItemInfo(go, tDto, index - 1);
            UIEventListener.Get(go).onClick = (tmp) =>
            {
                UIMatchMTTModel.mInstance.EnterMTTGame(roomInfo.matchId, tDto.deskNum);
            };
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
                ObtainListData(EnumLoadType.Refresh);
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
                ObtainListData(EnumLoadType.LoadMore);
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
