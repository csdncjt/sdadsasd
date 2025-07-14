using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;
using DtoElement = ETHotfix.WEB2_room_mtt_list.RoomListElement;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMatch_MttListComponentSystem : AwakeSystem<UIMatch_MttListComponent>
    {
        public override void Awake(UIMatch_MttListComponent self)
        {
            self.Awake();
        }
    }

    public enum EnumLoadType
    {
        Init = 1,
        Refresh,
        LoadMore,
    }

    /// <summary> 页面名: </summary>
    public class UIMatch_MttListComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private int mCurMttType = 1; //比赛类型 1 = 积分赛 2 = 实物赛
        private int mCurPage = 1;
        private GameObject NonShowed;

        bool hadInit = false;

        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            SetUpNav("官方锦标赛", UIType.UIMatch_MttList);
            InitSuperView();
        }

        void SetItemInfo(GameObject go, DtoElement pDto, int index)
        {
            RawImage imageMatchIcon = go.transform.Find("Image_MatchIcon").GetComponent<RawImage>();
            Text textStatus = go.transform.Find("Text_Status").GetComponent<Text>();
            Image imageStatus = go.transform.Find("Image_Status").GetComponent<Image>();
            ReferenceCollector statusImgrc = imageStatus.gameObject.GetComponent<ReferenceCollector>();
            Button btnSignUp = go.transform.Find("BtnSignUp").GetComponent<Button>();
            Text textBtn = btnSignUp.gameObject.transform.Find("Text").GetComponent<Text>();
            Text textName = go.transform.Find("Text_MatchName").GetComponent<Text>();
            Text textTime = go.transform.Find("Text_MatchTime").GetComponent<Text>();
            Text textProgress = go.transform.Find("Text_MatchProgress").GetComponent<Text>();
            Text textFee = go.transform.Find("Text_MatchFee").GetComponent<Text>();

            if (pDto.logoUrl.Length > 0)
                WebImageHelper.SetUrlImage(imageMatchIcon, pDto.logoUrl);

            textName.text = pDto.matchName;
            textTime.text = LanguageManager.Get("MTT-Start Time") + TimeHelper.GetDateTimer(pDto.startTime).ToString("yyyy/MM/dd HH:mm");
            textProgress.text = $"{pDto.currEntryNum}/{pDto.upperLimit}";
            textFee.text = StringHelper.ShowGold(pDto.entryFee);

            if (pDto.gameStatus == 0 || pDto.gameStatus == 1 || pDto.gameStatus == 4)
            {
                textStatus.text = LanguageManager.Get("MTT-Applying");
                imageStatus.sprite = statusImgrc.Get<Sprite>("images_dating_mtt_baomingzhong");
            }
            else
            {
                textStatus.text = LanguageManager.Get("MTT-Processing");
                imageStatus.sprite = statusImgrc.Get<Sprite>("images_dating_mtt_jinxinzhong"); 
            }

            if (pDto.canPlayerRebuy)
            {
                //重购
                btnSignUp.interactable = true;
                textBtn.text = LanguageManager.Get("MTT_Rebuy");
            }
            else
            {
                //0:可报名;1:等待开赛;2:延期报名;3:进行中;4:立即进入;5:报名截止;6:等待审核 7:重购条件不足

                btnSignUp.interactable = false;
                if (pDto.gameStatus == 0)
                {
                    //报名
                    textBtn.text = LanguageManager.Get("MTT-Apply");
                    btnSignUp.interactable = true;
                }
                else if (pDto.gameStatus == 1)
                {
                    //等待开赛
                    textBtn.text = LanguageManager.Get("mtt_btn_waiting_start");
                }
                else if (pDto.gameStatus == 2)
                {
                    //延时报名
                    textBtn.text = LanguageManager.Get("mtt_btn_delay");
                    btnSignUp.interactable = true;
                }
                else if (pDto.gameStatus == 3)
                {
                    //进行中
                    textBtn.text = LanguageManager.Get("mtt_btn_ongoing");
                    btnSignUp.interactable = true;
                }
                else if (pDto.gameStatus == 4)
                {
                    //立即进入
                    textBtn.text = LanguageManager.Get("mtt_btn_enter");
                    btnSignUp.interactable = true;
                }
                else if (pDto.gameStatus == 5)
                {
                    //报名截止
                    textBtn.text = LanguageManager.Get("mtt_btn_sign_up_deadline");
                }
                else if (pDto.gameStatus == 6)
                {
                    //等待审核
                    textBtn.text = LanguageManager.Get("mtt_btn_waiting_approval");
                }
                else if (pDto.gameStatus == 7)
                {
                    //已淘汰，且不能重购
                    textBtn.text = LanguageManager.Get("mtt_btn_rebuy");
                }
            }

            UIEventListener.Get(btnSignUp.gameObject, index).onIntClick = onClickActionBtn;
        }

        private void onClickActionBtn(GameObject go, int index)
        {
            if (go.GetComponent<Button>().interactable == false)
                return;
            var tDto = mDtoEles[index];
            if (tDto != null)
            {
                UIMatchMTTModel.mInstance.onMTTAction(tDto, UIType.UIMatch_MttList, finish =>
                {
                    ObtainListData(EnumLoadType.Refresh);
                });
            }
        }

        public override void OnHide()
        {
        }

        public override void Dispose()
        {
            hadInit = false;
            base.Dispose();
        }
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            NonShowed = rc.Get<GameObject>("NonShowed");
            NonShowed.SetActive(false);
        }

        private void ObtainListData(EnumLoadType loadType)
        {
            if (IsDisposed) return;
            if (loadType == EnumLoadType.Init || loadType == EnumLoadType.Refresh)
            {
                mCurPage = 1;
            }else
            {
                mCurPage++;
            }

            UIMatchModel.mInstance.APIMTTRoomList(mCurMttType, mCurPage, 20, pDto =>
            {
                if (pDto.status == 0)
                {
                    GameCache.Instance.client_ip = pDto.data.clientIp;
                    if (mCurPage == 1)
                    {
                        mDtoEles = pDto.data.roomList ?? new List<DtoElement>();
                        NonShowed.SetActive(mDtoEles.Count == 0);
                        mDtoEles.Add(new DtoElement());//因为上下都要菊花  给其追加一条空数据
                        if (loadType == EnumLoadType.Init)
                            mLoopListView.InitListView(mDtoEles.Count + 1, OnGetItemByIndex);
                        if (loadType == EnumLoadType.Refresh)
                            OnDataSourceLoadMoreFinished(true);
                    } else
                    {
                        mDtoEles.RemoveAt(mDtoEles.Count - 1);
                        mDtoEles.AddRange(pDto.data.roomList);
                        NonShowed.SetActive(mDtoEles.Count == 0);
                        mDtoEles.Add(new DtoElement());////因为上下都要菊花  给其追加一条空数据           
                        OnDataSourceLoadMoreFinished(pDto.data.roomList.Count > 0);
                    }
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
            mLoopListView = rc.Get<GameObject>("ScrollView").GetComponent<LoopListView2>();
            mLoopListView.mOnEndDragAction = OnEndDrag;
            mLoopListView.mOnDownMoreDragAction = OnDownMoreDragAction;
            mLoopListView.mOnUpRefreshDragAction = OnUpRefreshDragAction;
            mDtoEles = new List<DtoElement>();

            ObtainListData(hadInit ? EnumLoadType.Refresh : EnumLoadType.Init);
            hadInit = true;
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
                UIComponent.Instance.ShowNoAnimation(UIType.UIMatch_MttDetail, tDto.matchId);
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
