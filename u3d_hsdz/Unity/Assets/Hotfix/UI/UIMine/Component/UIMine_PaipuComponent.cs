using System.Collections;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_PaipuComponentSystem : AwakeSystem<UIMine_PaipuComponent>
    {
        public override void Awake(UIMine_PaipuComponent self)
        {
            self.Awake();
        }
    }

    enum LoadingTipStatus
    {
        None,
        WaitRelease,
        WaitLoad,
        Loaded,
        NoMoreData,
    }


    public class UIMine_PaipuComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private LoopListView2 mLoopListView;
        private Text textTotal;
        private GameObject popMenu;
        private Button btnSortPl;
        private Button btnSortTime;

        private LoadingTipStatus mLoadingTipStatus = LoadingTipStatus.None;
        float mLoadingTipItemHeight = 100;
        private string roomID;
        private GameObject NonShowed;
        private List<WEB2_brand_spe_info.HandsElement> mPaipuDataList = new List<WEB2_brand_spe_info.HandsElement>();
        private int currentPage = 1;
        private int sortType = 0; //0时间升序 1积分升序
        private bool isInit = false;

        private Text title_room;
        private Text title;

        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            Log.Debug($"onShow");
            SetUpNav(LanguageManager.Get("UIMine_Paipu_title"), UIType.UIMine_Paipu, ClickMenuAction, null);

            roomID = obj as string;

            mPaipuDataList.Clear();
            sortType = 0;
            currentPage = 1;
            isInit = true;
            ObtinPaipuData();
        }

        //获取牌谱列表
        void ObtinPaipuData()
        {
            if (!string.IsNullOrEmpty(roomID))
            {
                //修改标题

                title_room.gameObject.SetActive(true);
                title.gameObject.SetActive(false);

                WEB2_brand_spe_hand_list.RequestData requestData = new WEB2_brand_spe_hand_list.RequestData()
                {
                    pageNo = currentPage,
                    roomId = roomID,
                    sortType = sortType
                };
                HttpRequestComponent.Instance.Send(
                    WEB2_brand_spe_hand_list.API,
                    WEB2_brand_spe_hand_list.Request(requestData),
                    this.OpenPaipuInfo);
            }
            else
            {
                title_room.gameObject.SetActive(false);
                title.gameObject.SetActive(true);

                WEB2_brand_spe_info.RequestData requestData = new WEB2_brand_spe_info.RequestData()
                {
                    pageNo = currentPage,
                    sortType = sortType
                };
                HttpRequestComponent.Instance.Send(
                    WEB2_brand_spe_info.API,
                    WEB2_brand_spe_info.Request(requestData),
                    this.OpenPaipuInfo);
            }

        }

        async void OpenPaipuInfo(string resData)
        {
           // Log.Debug(resData);
            WEB2_brand_spe_info.ResponseData responseData = WEB2_brand_spe_info.Response(resData);
            if (responseData.status == 0)
            {
                textTotal.text = string.Format(LanguageManager.Get("UIMine_Paipu_count"), responseData.data.count);

                this.mPaipuDataList.AddRange(responseData.data.hands);
                // Debug.Log($"curret{currentPage}");
                if (currentPage == 1 && isInit)
                {
                    if (responseData.data.count == mPaipuDataList.Count)
                    {
                        mLoadingTipStatus = LoadingTipStatus.NoMoreData;
                    }
                    else
                    {
                        mLoadingTipStatus = LoadingTipStatus.None;
                    }
                    isInit = false;
                    mLoopListView = null;
                    mLoopListView = rc.Get<GameObject>("scrollview").GetComponent<LoopListView2>();
                    mLoopListView.InitListView(mPaipuDataList.Count + 1, OnGetItemByIndex);
                    mLoopListView.mOnBeginDragAction = OnBeginDrag;
                    mLoopListView.mOnDragingAction = OnDraging;
                    mLoopListView.mOnEndDragAction = OnEndDrag;
                    NonShowed.SetActive(mPaipuDataList.Count <= 0);
                }
                else
                {
                    await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(300);
                    OnDataSourceLoadMoreFinished(responseData.data.count == mPaipuDataList.Count);
                }

                currentPage++;
            }
        }


        private void ClickMenuAction()
        {
            popMenu.SetActive(!popMenu.activeInHierarchy);
        }



        public override void OnHide()
        {
            // Debug.Log($"OnHide");
            mPaipuDataList.Clear();
            OnDataSourceLoadMoreFinished(false);
            currentPage = 1;
        }

        public override void Dispose()
        {
            // Debug.Log($"Dispose");

            base.Dispose();
        }

        #region InitUI
        protected virtual void InitUI()
        {
            this.rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            textTotal = rc.Get<GameObject>("text_total_hand").GetComponent<Text>();
            popMenu = rc.Get<GameObject>("Pop_Menu");
            btnSortPl = rc.Get<GameObject>("Button_pl").GetComponent<Button>();
            btnSortTime = rc.Get<GameObject>("Button_time").GetComponent<Button>();

            title_room = rc.Get<GameObject>("Text_title2").GetComponent<Text>();
            title = rc.Get<GameObject>("Text_title").GetComponent<Text>();
            title_room.gameObject.SetActive(false);
            title.gameObject.SetActive(true);

            UIEventListener.Get(btnSortPl.gameObject).onClick = OnClickSortPl;
            UIEventListener.Get(btnSortTime.gameObject).onClick = OnClickSortTime;

            NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
            float realTop = safeArea.top * 1242 / safeArea.width;
            popMenu.GetComponent<RectTransform>().localPosition = new Vector3(0, -120 - realTop, 0);
            NonShowed = rc.Get<GameObject>("NonShowed");
        }

        private void SetUpItemWithData(GameObject go, WEB2_brand_spe_info.HandsElement element)
        {
            go.transform.Find("text_Title").GetComponent<Text>().text = string.Format(LanguageManager.Get("UIMine_Paipu_handNum"), element.index);

            if (element.roomType == 2)
            {
                go.transform.Find("text_Blind").GetComponent<Text>().text = StringHelper.ShowGold(element.qianzhu);
            }
            else {
                string str1 = StringHelper.ShowGold(element.smallBlind);
                string str2 = StringHelper.ShowGold(element.bigBlind);
                string str3 = StringHelper.ShowGold(element.qianzhu);
                go.transform.Find("text_Blind").GetComponent<Text>().text = StringHelper.ShowBlinds(str1, str2, str3);
            }
           
            go.transform.Find("text_Time").GetComponent<Text>().text = TimeHelper.GetDateTimer(element.timeStamp).ToString("MM/dd HH:mm");
            Text coinText = go.transform.Find("text_coin").GetComponent<Text>();
            coinText.text = StringHelper.GetSignedString(element.win);
            coinText.color = element.win >= 0 ? new Color32(200, 37, 37, 255) : new Color32(49, 155, 44, 255);
            Text typeImg = go.transform.Find("Image_Type/Text_Type").GetComponent<Text>();
            if (element.roomType == 0)
            {
                typeImg.text = CPErrorCode.LanguageDescription(10022);
            }
            else if (element.roomType == 1)
            {
                typeImg.text = CPErrorCode.LanguageDescription(10009);
            }
            else if (element.roomType == 2)
            {
                typeImg.text = CPErrorCode.LanguageDescription(10334);
            }
        }

        #endregion

        #region Action
        async void OnItemClicked(int index)
        {
            WEB2_brand_spe_info.HandsElement itemData = mPaipuDataList[index];
            await Game.Scene.GetComponent<UIComponent>().ShowAsync(UIType.UIMine_PaipuDetail, new UIMine_PaipuDetailComponent.PaipuDetailData()
            {
                paipuData = itemData,
                roomId = roomID
            }, () => { });
            // Debug.Log($"{index}");
        }

        private void OnClickSortPl(GameObject go)
        {
            isInit = false;
            ClickMenuAction();
            sortType = 1;
            mPaipuDataList.Clear();
            currentPage = 1;
            ObtinPaipuData();
        }

        private void OnClickSortTime(GameObject go)
        {
            isInit = false;
            ClickMenuAction();
            sortType = 0;
            mPaipuDataList.Clear();
            currentPage = 1;
            ObtinPaipuData();
        }

        #endregion

        #region SupperScrollView
        LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
        {
            if (index < 0)
            {
                return null;
            }
            LoopListViewItem2 item = null;
            if (index == mPaipuDataList.Count)
            {
                item = listView.NewListViewItem("ItemPrefab0");
                UpdateLoadingTip(item);
                return item;
            }
            WEB2_brand_spe_info.HandsElement itemData = mPaipuDataList[index];
            if (itemData == null)
            {
                return null;
            }
            item = listView.NewListViewItem("Paipu_info");
            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
            }
            if (index == mPaipuDataList.Count - 1)
            {
                item.Padding = 0;
            }
            GameObject paipuObject = item.gameObject;
            //todo
            SetUpItemWithData(paipuObject, itemData);
            ClickEventListener listener = ClickEventListener.Get(paipuObject);
            listener.SetClickEventHandler(delegate (GameObject obj) { OnItemClicked(index); });

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
                itemScript0.mText.text = LanguageManager.Get("adaptation10089");
                itemScript0.mArrow.SetActive(true);
                itemScript0.mWaitingIcon.SetActive(false);
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mLoadingTipItemHeight);
            }
            else if (mLoadingTipStatus == LoadingTipStatus.WaitLoad)
            {
                itemScript0.mRoot.SetActive(true);
                itemScript0.mArrow.SetActive(false);
                itemScript0.mWaitingIcon.SetActive(true);
                itemScript0.mText.text = LanguageManager.Get("DataLoading");
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mLoadingTipItemHeight);
            }
            else if (mLoadingTipStatus == LoadingTipStatus.NoMoreData)
            {
                itemScript0.mRoot.SetActive(true);
                itemScript0.mText.text = LanguageManager.Get("NoMoreData");
                itemScript0.mArrow.SetActive(false);
                itemScript0.mWaitingIcon.SetActive(false);
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mLoadingTipItemHeight);
            }
        }

        void OnBeginDrag()
        {

        }
        void OnDraging()
        {
            if (mLoopListView.ShownItemCount == 0)
            {
                return;
            }
            if (mLoadingTipStatus != LoadingTipStatus.None && mLoadingTipStatus != LoadingTipStatus.WaitRelease)
            {
                return;
            }
            LoopListViewItem2 item = mLoopListView.GetShownItemByItemIndex(mPaipuDataList.Count);
            if (item == null)
            {
                return;
            }
            LoopListViewItem2 item1 = mLoopListView.GetShownItemByItemIndex(mPaipuDataList.Count - 1);
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
        void OnEndDrag()
        {
            if (mLoopListView.ShownItemCount == 0)
            {
                return;
            }
            if (mLoadingTipStatus != LoadingTipStatus.None && mLoadingTipStatus != LoadingTipStatus.WaitRelease)
            {
                return;
            }
            LoopListViewItem2 item = mLoopListView.GetShownItemByItemIndex(mPaipuDataList.Count);
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
            ObtinPaipuData();
        }

        void OnDataSourceLoadMoreFinished(bool noMoreData)
        {
            mLoopListView.SetListItemCount(mPaipuDataList.Count + 1, false);
            mLoopListView.RefreshAllShownItem();

            if (noMoreData)
            {
                mLoadingTipStatus = LoadingTipStatus.NoMoreData;
            }
            else
            {
                mLoadingTipStatus = LoadingTipStatus.None;
            }

        }
        #endregion

    }




}

