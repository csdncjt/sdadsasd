using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using AchElement = ETHotfix.WEB2_promotion_query_achievements.MonthAchievementsElement;
using BeansElement = ETHotfix.WEB2_promotion_query_achievements.MonthBeansElement;
using SuperScrollView;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_AchievementQueryComponentSystem : AwakeSystem<UIMine_AchievementQueryComponent>
    {
        public override void Awake(UIMine_AchievementQueryComponent self)
        {
            self.Awake();
        }
    }

    public class UIMine_AchievementQueryComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Transform transScrollView;
        private Text textTodayLabel;
        private Text textMonthLabel;
        /// <summary>         头部切换 index         </summary>
        private int mCurTopIndex = -1;
        private int mCurPage = 0;
        private List<AchElement> mAchEles;
        private List<BeansElement> mBeansEles;
        private GameObject NonShowed;
        private Text textRightTitle;
        private Text text30AchValue;
        private Text text1AchValue;
        private Text Text1BeanValue;
        private Text text30BeanValue;
        private GameObject AchValue;
        private GameObject BeanValue;


        public void Awake()
        {
            mCurPage = 0;
            InitUI();

            text1AchValue.text = "";
            Text1BeanValue.text = "";
            text30AchValue.text = "";
            text30BeanValue.text = "";

            InitSuperView();
            ShowAchView();
        }

        void ShowAchView(Action pInitAct = null)
        {
            UIMineModel.mInstance.APIQueryAchievements(pAct =>
            {
                mAchEles = pAct.monthAchievements ?? new List<AchElement>();
                NonShowed.SetActive(mAchEles.Count == 0);

                mAchEles.Add(new AchElement());
                if (pInitAct == null)
                    mLoopListView.InitListView(mAchEles.Count + 1, OnGetItemByIndex);
                else
                    pInitAct();

                mBeansEles = null;
                text1AchValue.text = pAct == null ? "0" :  StringHelper.ShowGold(pAct.todayAchievement);
                text30AchValue.text = pAct == null ? "0" : StringHelper.ShowGold(pAct.totalMonthAchievement);
                mCurPage = 1;
            });
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_AchievementQuery);
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

            Text1BeanValue = rc.Get<GameObject>("Text_1BeanValue").GetComponent<Text>();
            text30BeanValue = rc.Get<GameObject>("Text_30BeanValue").GetComponent<Text>();
            transScrollView = rc.Get<GameObject>("ScrollView").transform;
            textTodayLabel = rc.Get<GameObject>("Text_TodayLabel").GetComponent<Text>();
            textMonthLabel = rc.Get<GameObject>("Text_MonthLabel").GetComponent<Text>();
            text30AchValue = rc.Get<GameObject>("Text_30AchValue").GetComponent<Text>();
            text1AchValue = rc.Get<GameObject>("Text_1AchValue").GetComponent<Text>();

            mCurTopIndex = 0;
            textTodayLabel.text = LanguageManager.mInstance.GetLanguageForKeyMoreValue("SelectAch", 0);// "今日业绩";
            textMonthLabel.text = LanguageManager.mInstance.GetLanguageForKeyMoreValue("SelectWeek", 0);// "本月业绩";        


            AchValue = rc.Get<GameObject>("AchValue");
            BeanValue = rc.Get<GameObject>("BeanValue");
            BeanValue.SetActive(false);

            NonShowed = rc.Get<GameObject>("NonShowed");
            NonShowed.SetActive(false);
            
            textRightTitle = rc.Get<GameObject>("Text_RightTitle").GetComponent<Text>();
            textRightTitle.text = LanguageManager.mInstance.GetLanguageForKey("ToweekAch"); //"本月业绩";

            UISegmentControlComponent.SetUp(rc.transform, new UISegmentControlComponent.SegmentData()
            {
                Titles = LanguageManager.mInstance.GetLanguageForKeyMoreValues("ProManage103"),//new string[] { "我的业绩", "返USDT" },
                OnClick = OnSegmentClick,
                N_S_Fonts = new[] { 48, 48 },
                N_S_Color = new Color32[] { new Color32(179, 142, 86, 255), new Color32(255, 255, 255, 255) },
                IsEffer = true
            });
        }

        private void OnSegmentClick(GameObject go, int index)
        {
            if (mCurTopIndex == index)
            {
                return;
            }
            mCurTopIndex = index;
            //切换看数量 
            if (index == 0)
            {//我的业绩
                textTodayLabel.text = LanguageManager.mInstance.GetLanguageForKeyMoreValue("SelectAch", 0);// "今日业绩";
                textMonthLabel.text = LanguageManager.mInstance.GetLanguageForKeyMoreValue("SelectWeek", 0);// "本月业绩";
                textRightTitle.text = LanguageManager.mInstance.GetLanguageForKey("ToweekAch"); //"本月业绩";
                BeanValue.SetActive(false);
                AchValue.SetActive(true);
                ShowAchView(delegate
                {
                    mLoopListView.SetListItemCount(mAchEles.Count + 1, true);
                    mLoopListView.RefreshAllShownItem();
                });
            }
            else if (index == 1)
            {//返USDT
                textTodayLabel.text = LanguageManager.mInstance.GetLanguageForKeyMoreValue("SelectAch", 1);// "";
                textMonthLabel.text = LanguageManager.mInstance.GetLanguageForKeyMoreValue("SelectWeek", 1);// "";
                textRightTitle.text = LanguageManager.mInstance.GetLanguageForKey("ProManage104");// "本月返还";
                BeanValue.SetActive(true);
                AchValue.SetActive(false);

                UIMineModel.mInstance.APIQueryAchievements(pAct =>
                {
                    mBeansEles = pAct.monthBeans;
                    NonShowed.SetActive(mBeansEles.Count == 0);
                    mBeansEles.Add(new BeansElement());
                    mLoopListView.SetListItemCount(mBeansEles.Count + 1, true);
                    mLoopListView.RefreshAllShownItem();

                    mAchEles = null;

                    text30BeanValue.text = StringHelper.ShowGold(pAct.totalMonthBeans, false);
                    Text1BeanValue.text = StringHelper.ShowGold(pAct.yesterdayBeans, false);

                    mCurPage = 1;
                });
            }
        }


        void SetFindLabelText(GameObject go, string path, string strValue)
        {
            go.transform.Find(path).GetComponent<Text>().text = strValue;
        }


        public void SetGetLabelText(string path, string content)
        {
            rc.Get<GameObject>(path).GetComponent<Text>().text = content;
        }

        private void SetItemInfoAch(GameObject go, AchElement pDto)
        {
            go.transform.Find("DayTime").GetComponent<Text>().text = TimeHelper.GetDateTimer(pDto.date).ToString("yyyy/MM/dd");
            go.transform.Find("DayValue").GetComponent<Text>().text = StringHelper.ShowGold(pDto.dailyData);
        }
        private void SetItemInfoBeans(GameObject go, BeansElement pDto)
        {
            go.transform.Find("DayTime").GetComponent<Text>().text = TimeHelper.GetDateTimer(pDto.date).ToString("yyyy/MM/dd");
            go.transform.Find("DayValue").GetComponent<Text>().text = StringHelper.ShowGold(pDto.dailyData, false);
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
        }

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
            if (index == (mCurTopIndex == 0 ? mAchEles.Count : mBeansEles.Count))
            {
                item = listView.NewListViewItem("ItemPrefab0");
                UpdateLoadingTip(item);
                return item;
            }

            if (mCurTopIndex == 0)
            {
                var tDto = mAchEles[index - 1];
                if (tDto == null)
                    return null;
            }
            else
            {
                var tDto = mBeansEles[index - 1];
                if (tDto == null)
                    return null;
            }

            item = listView.NewListViewItem("ItemInfo");
            ListItem2 itemScript = item.GetComponent<ListItem2>();
            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
            }
            var go = item.gameObject;
            if (mCurTopIndex == 0)
            {
                var tDto = mAchEles[index - 1];
                SetItemInfoAch(go, tDto);
            }
            else
            {
                var tDto = mBeansEles[index - 1];
                SetItemInfoBeans(go, tDto);
            }
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
                UIMineModel.mInstance.APIQueryAchievements(pAct =>
                 {
                     if (mCurTopIndex == 0)
                     {
                         mAchEles = pAct.monthAchievements ?? new List<AchElement>();
                         NonShowed.SetActive(mAchEles.Count == 0);
                         mAchEles.Add(new AchElement());
                         mBeansEles = null;
                     }
                     else
                     {
                         mBeansEles = pAct.monthBeans;
                         NonShowed.SetActive(mBeansEles.Count == 0);
                         mBeansEles.Add(new BeansElement());
                         mAchEles = null;
                     }
                     mCurPage = 1;
                     OnDataSourceLoadMoreFinished(true);
                 }, 15, mCurPage);
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
                LoopListViewItem2 item = mLoopListView.GetShownItemByItemIndex(mCurTopIndex == 0 ? mAchEles.Count : mBeansEles.Count);
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
                UIMineModel.mInstance.APIQueryAchievements(pAct =>
                {
                    if (mCurTopIndex == 0)
                    {
                        mAchEles.RemoveAt(mAchEles.Count - 1);
                        mAchEles.AddRange(pAct.monthAchievements);
                        NonShowed.SetActive(mAchEles.Count == 0);
                        mAchEles.Add(new AchElement());
                    }
                    else
                    {
                        mBeansEles.RemoveAt(mBeansEles.Count - 1);
                        mBeansEles.AddRange(pAct.monthBeans);
                        NonShowed.SetActive(mBeansEles.Count == 0);
                        mBeansEles.Add(new BeansElement());
                    }
                    OnDataSourceLoadMoreFinished(pAct.monthAchievements.Count > 0);
                    mCurPage += 1;
                }, 15, mCurPage);
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
                if (mCurTopIndex == 0)
                    mLoopListView.SetListItemCount(mAchEles.Count + 1, false);
                else
                    mLoopListView.SetListItemCount(mBeansEles.Count + 1, false);
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
            LoopListViewItem2 item = mLoopListView.GetShownItemByItemIndex(mCurTopIndex == 0 ? mAchEles.Count : mBeansEles.Count);
            if (item == null)
            {
                return;
            }
            LoopListViewItem2 item1 = mLoopListView.GetShownItemByItemIndex(mCurTopIndex == 0 ? mAchEles.Count - 1 : mBeansEles.Count - 1);
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
