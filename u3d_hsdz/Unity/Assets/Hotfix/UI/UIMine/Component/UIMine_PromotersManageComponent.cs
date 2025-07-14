using SuperScrollView;
using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using MyPromoterElement = ETHotfix.WEB2_promotion_query_my_promoters.PromotersListElement;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_PromotersManageComponentSystem : AwakeSystem<UIMine_PromotersManageComponent>
    {
        public override void Awake(UIMine_PromotersManageComponent self)
        {
            self.Awake();
        }
    }

    public class UIMine_PromotersManageComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private InputField inputfieldquery;
        private Text textPeople;
        private Transform transScrollView;
        private int mCurPage = 0;
        private GameObject NonShowed;
        private Text textrightPlayer;
        private Text textleftPromoter;
        Dictionary<string, Sprite> mDicSprite;

        public void Awake()
        {
            InitUI();
            InitSuperView();
            QueryMyPromoter(0, null);//第几
            textPeople.text = "";
            textleftPromoter.text = "";
            textrightPlayer.text = "";
            mDicSprite = new Dictionary<string, Sprite> {
                { "promoter",rc.Get<Sprite>("promoter")},
                { "player",rc.Get<Sprite>("player")},
            };
        }

        private void QueryMyPromoter(int pPage, Action pAct, string pSearch = "")
        {
            UIMineModel.mInstance.APIQueryMyPromoterSizePage(pSearch, 15, pPage, pDto =>
               {
                   if (pDto.data != null)
                   {
                       mCurPage = 1;
                       mMyPromoterEles = pDto.data.promotersList ?? new List<MyPromoterElement>();
                       NonShowed.SetActive(mMyPromoterEles.Count == 0);
                       mMyPromoterEles.Add(new MyPromoterElement());
                       if (pAct != null)
                       {
                           pAct();
                       }
                       else
                       {
                           mLoopListView.InitListView(mMyPromoterEles.Count + 1, OnGetItemByIndex);
                       }
                       textPeople.text = pDto.data.myGroupNumber.ToString();

                       textleftPromoter.text = pDto.data.todayPromotorNumber.ToString();
                       textrightPlayer.text = pDto.data.todayPlayerNumber.ToString();
                   }
               });
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_PromotersManage);
        }
        public override void OnHide()
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        #region UI
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            inputfieldquery = rc.Get<GameObject>("InputField_query").GetComponent<InputField>();
            textPeople = rc.Get<GameObject>("Text_People").GetComponent<Text>();
            transScrollView = rc.Get<GameObject>("ScrollView").transform;
            inputfieldquery.onEndEdit.AddListener(QueryChange);
            NonShowed = rc.Get<GameObject>("NonShowed");
            textrightPlayer = rc.Get<GameObject>("Text_rightPlayer").GetComponent<Text>();
            textleftPromoter = rc.Get<GameObject>("Text_leftPromoter").GetComponent<Text>();
            NonShowed.SetActive(false);
        }

        private void QueryChange(string arg0)
        {
            QueryMyPromoter(0, delegate
            {
                OnDataSourceLoadMoreFinished(true);
                mCurPage = 1;
                mLoopListView.SetListItemCount(mMyPromoterEles.Count + 1, true);
                mLoopListView.RefreshAllShownItem();

                if (mMyPromoterEles.Count <= 1)
                {
                    UIComponent.Instance.ToastFormat1("ProManage101", inputfieldquery.text);
                }
            }, inputfieldquery.text);
        }

        /// <summary>       设置Items的值         </summary>
        void SetItemInfo(GameObject go, MyPromoterElement pDto)
        {
            go.transform.Find("Text_Ranking").GetComponent<Text>().text = pDto.rank.ToString();
            go.transform.Find("Text_ID").GetComponent<Text>().text = pDto.userId.ToString();
            go.transform.Find("Image_PromotionType").GetComponent<Image>().sprite = pDto.promotionType == 0 ? mDicSprite["player"] : mDicSprite["promoter"]; 

            go.transform.Find("Text_TeamNum").GetComponent<Text>().text = pDto.groupMembers.ToString();
            go.transform.Find("Text_Toweek").GetComponent<Text>().text = StringHelper.ShowGold(pDto.weekAchievement);
            var textName = go.transform.Find("Text_Name").GetComponent<Text>();
            UIMatchModel.mInstance.SetLargeTextContent(textName, pDto.nickname);
        }

        #endregion


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
            mMyPromoterEles = new List<MyPromoterElement>();
        }

        private List<MyPromoterElement> mMyPromoterEles = new List<MyPromoterElement>();//推广员列表     


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
            if (index == mMyPromoterEles.Count)
            {
                item = listView.NewListViewItem("ItemPrefab0");
                UpdateLoadingTip(item);
                return item;
            }

            var tDto = mMyPromoterEles[index - 1];
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
                mCurPage = 0;
                QueryMyPromoter(mCurPage, delegate
                  {
                      OnDataSourceLoadMoreFinished(true);
                      mCurPage = 1;
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
                LoopListViewItem2 item = mLoopListView.GetShownItemByItemIndex(mMyPromoterEles.Count);
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
                var search = inputfieldquery.text.Trim();
                UIMineModel.mInstance.APIQueryMyPromoterSizePage(search, 15, mCurPage, pAct =>
                 {
                     mMyPromoterEles.RemoveAt(mMyPromoterEles.Count - 1);
                     mMyPromoterEles.AddRange(pAct.data.promotersList);
                     NonShowed.SetActive(mMyPromoterEles.Count == 0);
                     mMyPromoterEles.Add(new MyPromoterElement());
                     mCurPage += 1;
                     OnDataSourceLoadMoreFinished(pAct.data.promotersList.Count > 0);
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
                mLoopListView.SetListItemCount(mMyPromoterEles.Count + 1, false);
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
            LoopListViewItem2 item = mLoopListView.GetShownItemByItemIndex(mMyPromoterEles.Count);
            if (item == null)
            {
                return;
            }
            LoopListViewItem2 item1 = mLoopListView.GetShownItemByItemIndex(mMyPromoterEles.Count - 1);
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
