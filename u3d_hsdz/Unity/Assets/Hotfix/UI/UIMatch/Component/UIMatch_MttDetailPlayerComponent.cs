using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;
using DtoRoomInfo = ETHotfix.WEB2_room_mtt_view.Data;
using DtoElement = ETHotfix.REQ_MTT_PLAYER_LIST.PlayerListElement;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMatch_MttDetailPlayerComponentSystem : AwakeSystem<UIMatch_MttDetailPlayerComponent>
    {
        public override void Awake(UIMatch_MttDetailPlayerComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名: </summary>
    public class UIMatch_MttDetailPlayerComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private InputField mlist_inputfield_query;

        DtoRoomInfo roomInfo;
        private int mCurPage = 1;
        EnumLoadType mCurLoadType = EnumLoadType.Init;

        public void Awake()
        {
            registerHandler();
            InitUI();
        }

        private void registerHandler()
        {
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_MTT_PLAYER_LIST, HANDLER_REQ_MTT_PLAYER_LIST);  // MTT报名
        }

        private void removeHandler()
        {
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_MTT_PLAYER_LIST, HANDLER_REQ_MTT_PLAYER_LIST);
        }

        public override void OnShow(object obj)
        {
            roomInfo = obj as DtoRoomInfo;
            InitSuperView();

            mlist_inputfield_query.onValueChanged.RemoveAllListeners();
            mlist_inputfield_query.onValueChanged.AddListener((value) => {
                ObtainListData(EnumLoadType.Refresh);
            });
        }

        void SetItemInfo(GameObject go, DtoElement pDto, int index)
        {
            Text textNum = go.transform.Find("content/Text_Num").GetComponent<Text>();
            Text textName = go.transform.Find("content/Text_Name").GetComponent<Text>();
            Text textRebuy = go.transform.Find("content/Text_Rebuy").GetComponent<Text>();
            Text textHunter = go.transform.Find("content/Text_Hunter").GetComponent<Text>();
            Text textScore = go.transform.Find("content/Text_Score").GetComponent<Text>();

            textNum.text = $"{pDto.rank}";
            textName.text = pDto.nickName;
            textRebuy.text = $"{pDto.rebuyTimes}";
            textHunter.text = $"{pDto.hunterMoney}";
            textScore.text = $"{ StringHelper.ShowGold(int.Parse(pDto.scoreBoard))}";
        }

        public override void OnHide()
        {
            mlist_inputfield_query.onValueChanged.RemoveAllListeners();
        }

        public override void Dispose()
        {
            removeHandler();
            base.Dispose();
        }
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mlist_inputfield_query = rc.Get<GameObject>("mlist_inputfield_query").GetComponent<InputField>();
        }

        private void ObtainListData(EnumLoadType loadType)
        {
            mCurLoadType = loadType;
            if (loadType == EnumLoadType.Init || loadType == EnumLoadType.Refresh)
            {
                mCurPage = 1;
            }
            else
            {
                mCurPage++;
            }
            CPResSessionComponent.Instance.HotfixSession.Send(new REQ_MTT_PLAYER_LIST()
            {
                roomID = roomInfo.matchId,
                page = mCurPage,
                searchWork = mlist_inputfield_query.text,
                clubID = 0,
            });

        }

        protected void HANDLER_REQ_MTT_PLAYER_LIST(ICPResponse response)
        {
            REQ_MTT_PLAYER_LIST rec = response as REQ_MTT_PLAYER_LIST;
            if (null == rec)
            {
                return;
            }

            if (rec.status == 0)
            {
                if (mCurPage == 1)
                {
                    mDtoEles = rec.response.playerList ?? new List<DtoElement>();
                    mDtoEles.Add(new DtoElement());//因为上下都要菊花  给其追加一条空数据
                    if (mCurLoadType == EnumLoadType.Init)
                        mLoopListView.InitListView(mDtoEles.Count + 1, OnGetItemByIndex);
                    if (mCurLoadType == EnumLoadType.Refresh)
                        OnDataSourceLoadMoreFinished(true);
                }
                else
                {
                    mDtoEles.RemoveAt(mDtoEles.Count - 1);
                    mDtoEles.AddRange(rec.response.playerList);
                    mDtoEles.Add(new DtoElement());////因为上下都要菊花  给其追加一条空数据           
                    OnDataSourceLoadMoreFinished(rec.response.playerList.Count > 0);
                }
            }
            else
            {
                mDtoEles = new List<DtoElement>() { new DtoElement() };
                OnDataSourceLoadMoreFinished(false);
            }
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
                mLoopListView = rc.Get<GameObject>("Player_scrollview").GetComponent<LoopListView2>();
                mLoopListView.mOnEndDragAction = OnEndDrag;
                mLoopListView.mOnDownMoreDragAction = OnDownMoreDragAction;
                mLoopListView.mOnUpRefreshDragAction = OnUpRefreshDragAction;
                mDtoEles = new List<DtoElement>();

                ObtainListData(EnumLoadType.Init);
            } else
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
                if (roomInfo.isStart && int.Parse(tDto.scoreBoard) > 0)
                    UIMatchMTTModel.mInstance.EnterMTTGame(roomInfo.matchId, tDto.roomNum);
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
            await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(500);
            mLoadingTipStatus = pLength ? LoadingTipStatus.None : LoadingTipStatus.NoMoreData;
            mLoopListView.SetListItemCount(mDtoEles.Count + 1, false);
            mLoopListView.RefreshAllShownItem();
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
