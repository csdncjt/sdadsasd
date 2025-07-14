using System;
using System.Collections;
using System.Collections.Generic;
using ETModel;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_RecordListComponentSystem : AwakeSystem<UIMine_RecordListComponent>
    {
        public override void Awake(UIMine_RecordListComponent self)
        {
            self.Awake();
        }
    }

    public class UIMine_RecordListComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private LoopListView2 mLoopListView;
        private LoadingTipStatus mLoadingTipStatus = LoadingTipStatus.None;
        private float mLoadingTipItemHeight = 100;
        private List<WEB2_record_my.ListElement> mNormalData;
        private List<WEB2_record_mtt_list.ListElement> mMTTData;
        private long mNextTime = -1;//当前时间
        /// <summary>头部 seg 从1开始   </summary>
        int mCurSegTop = 1;
        private GameObject NonShowed;

        int recordType;//0大厅1俱乐部

        private const string chipSpriteName = "icon_list_zhanji_mangzhujibie";
        private const string playerSpriteName = "icon_list_zhanji_cansaairenshu";
        private const string timeSpriteName = "icon_list_zhanji_paijushichang";

        public void Awake()
        {
            mItemStartV3 = Vector3.zero;
            mCurSegTop = 1;
            InitUI();
            //InitSuperView();
        }

        public override void OnShow(object obj)
        {

            recordType = (int)obj;

            SetUpNav(UIType.UIMine_RecordList);
            UISegmentControlComponent.SetUp(rc.transform, new UISegmentControlComponent.SegmentData()
            {
                Titles = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIRecordList"),
                OnClick = OnSegmentClick,
                N_S_Fonts = new[] { 48, 48 },
                N_S_Color = new Color32[] { new Color32(179, 142, 86, 255), new Color32(255, 255, 255, 255) },
                IsEffer = true
            });

            InitSuperView();
        }

        /// <summary>
        /// pSearchType搜索牌局类型:1-德州,2-Omaha,3-AOF,4-必下场
        /// </summary>
        void API2RocordListMy(long pNextTime, int pSearchType, int clubId, Action<WEB2_record_my.ResponseData> pAct)
        {
            var req = new WEB2_record_my.RequestData
            {
                nextTime = pNextTime,
                searchType = pSearchType,
                clubId = clubId

            };
            HttpRequestComponent.Instance.Send(WEB2_record_my.API,
                                               WEB2_record_my.Request(req), resData =>
                                               {
                                                   var tDto = WEB2_record_my.Response(resData);
                                                   if (pAct != null)
                                                       pAct(tDto);
                                               });
        }

        /// <summary>
        /// pSearchType搜索牌局类型:1-德州,2-Omaha,3-AOF,4-必下场
        /// </summary>
        void API2ClubRocordListMy(long pNextTime, int pSearchType, int clubId, Action<WEB2_record_my.ResponseData> pAct)
        {
            var req = new WEB2_record_my.RequestData
            {
                nextTime = pNextTime,
                searchType = pSearchType,
                clubId = clubId
            };
            HttpRequestComponent.Instance.Send(WEB2_record_my.API,
                                               WEB2_record_my.Request(req), resData =>
                                               {
                                                   var tDto = WEB2_record_my.Response(resData);
                                                   if (pAct != null)
                                                       pAct(tDto);
                                               });
        }

        /// <summary>
        /// MTT战绩列表
        /// </summary>
        void API2RocordMTTListMy(long pNextTime, Action<WEB2_record_mtt_list.ResponseData> pAct)
        {
            var req = new WEB2_record_mtt_list.RequestData
            {
                nextTime = pNextTime,
            };
            HttpRequestComponent.Instance.Send(WEB2_record_mtt_list.API,
                                               WEB2_record_mtt_list.Request(req), resData =>
                                               {
                                                   var tDto = WEB2_record_mtt_list.Response(resData);
                                                   if (pAct != null)
                                                       pAct(tDto);
                                               });
        }



        /// <summary>
        /// 点击头部 选项
        /// </summary>
        private void OnSegmentClick(GameObject go, int index)
        {
            index = index + 1;
            if (index == mCurSegTop)
            {
                return;
            }
            mCurSegTop = index;
            DragRequestData(TimeHelper.ClientNow(), EnumLoadType.Refresh);
        }

        public override void OnHide()
        {

        }

        public override void Dispose()
        {
            if (this.IsDisposed)
                return;

            base.Dispose();
        }

        #region InitUI
        protected virtual void InitUI()
        {
            mLoadingTipStatus = LoadingTipStatus.None;
            rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            NonShowed = rc.Get<GameObject>("NonShowed");
            NonShowed.SetActive(false);
        }
        #endregion

        #region SupperScrollView
        private void InitSuperView()
        {
            mLoopListView = rc.Get<GameObject>("BattleRecord_scrollview").GetComponent<LoopListView2>();
            mLoopListView.mOnEndDragAction = OnEndDrag;
            mLoopListView.mOnDownMoreDragAction = OnDownMoreDragAction;
            mLoopListView.mOnUpRefreshDragAction = OnUpRefreshDragAction;

            DragRequestData(TimeHelper.ClientNow(), EnumLoadType.Init);
        }
        /// <summary>          pDir->1下拉追加   0加载更多         </summary>
        void DragRequestData(long pNextTime, EnumLoadType loadType)
        {
            if (this.mCurSegTop >= 0 && this.mCurSegTop < 5)
            {
                if (recordType == 0)
                {
                    API2RocordListMy(pNextTime, mCurSegTop,0, pDto =>
                    {
                        if (pDto.status == 0)
                        {
                            mNextTime = pDto.data.nextTime;
                            if (loadType == EnumLoadType.LoadMore)
                            {//追加
                                mNormalData.RemoveAt(mNormalData.Count - 1);
                                mNormalData.AddRange(pDto.data.list);
                                NonShowed.SetActive(mNormalData.Count == 0);
                                mNormalData.Add(new WEB2_record_my.ListElement());
                                OnDataSourceLoadMoreFinished(mNormalData.Count > 0);
                            }
                            else if (loadType == EnumLoadType.Refresh)
                            {//刷新
                                mNormalData = pDto.data.list ?? new List<WEB2_record_my.ListElement>();
                                NonShowed.SetActive(mNormalData.Count == 0);
                                if (mNormalData != null && mNormalData.Count > 0)
                                    mNormalData.Add(new WEB2_record_my.ListElement());
                                else
                                {
                                    mNormalData = new List<WEB2_record_my.ListElement>() { new WEB2_record_my.ListElement() };
                                }
                                OnDataSourceLoadMoreFinished(true);
                            }
                            else
                            {
                                mNormalData = pDto.data.list ?? new List<WEB2_record_my.ListElement>();
                                NonShowed.SetActive(mNormalData.Count == 0);
                                mNormalData.Add(new WEB2_record_my.ListElement());
                                mLoopListView.InitListView(mNormalData.Count + 1, OnGetItemByIndex);
                            }
                        }
                        else
                        {
                            UIComponent.Instance.Toast(pDto.status);
                            OnDataSourceLoadMoreFinished(false);
                        }
                    });
                }
                else
                {
                    API2ClubRocordListMy(pNextTime, mCurSegTop, recordType, pDto =>
                    {
                        if (pDto.status == 0)
                        {
                            mNextTime = pDto.data.nextTime;
                            if (loadType == EnumLoadType.LoadMore)
                            {//追加
                                mNormalData.RemoveAt(mNormalData.Count - 1);
                                mNormalData.AddRange(pDto.data.list);
                                NonShowed.SetActive(mNormalData.Count == 0);
                                mNormalData.Add(new WEB2_record_my.ListElement());
                                OnDataSourceLoadMoreFinished(mNormalData.Count > 0);
                            }
                            else if (loadType == EnumLoadType.Refresh)
                            {//刷新
                                mNormalData = pDto.data.list ?? new List<WEB2_record_my.ListElement>();
                                NonShowed.SetActive(mNormalData.Count == 0);
                                if (mNormalData != null && mNormalData.Count > 0)
                                    mNormalData.Add(new WEB2_record_my.ListElement());
                                else
                                {
                                    mNormalData = new List<WEB2_record_my.ListElement>() { new WEB2_record_my.ListElement() };
                                }
                                OnDataSourceLoadMoreFinished(true);
                            }
                            else
                            {
                                mNormalData = pDto.data.list ?? new List<WEB2_record_my.ListElement>();
                                NonShowed.SetActive(mNormalData.Count == 0);
                                mNormalData.Add(new WEB2_record_my.ListElement());
                                mLoopListView.InitListView(mNormalData.Count + 1, OnGetItemByIndex);
                            }
                        }
                        else
                        {
                            UIComponent.Instance.Toast(pDto.status);
                            OnDataSourceLoadMoreFinished(false);
                        }
                    });
                }
               
            }
            else
            {
                // MTT
                API2RocordMTTListMy(TimeHelper.ClientNow(), pDto =>
                {
                    if (pDto.status == 0)
                    {
                        mNextTime = pDto.data.nextTime;
                        if (loadType == EnumLoadType.LoadMore)
                        {//追加
                            mMTTData.RemoveAt(mMTTData.Count - 1);
                            mMTTData.AddRange(pDto.data.list);
                            NonShowed.SetActive(mMTTData.Count == 0);
                            mMTTData.Add(new WEB2_record_mtt_list.ListElement());
                            OnDataSourceLoadMoreFinished(mMTTData.Count > 0);
                        }
                        else if (loadType == EnumLoadType.Refresh)
                        {//刷新
                            mMTTData = pDto.data.list ?? new List<WEB2_record_mtt_list.ListElement>();
                            NonShowed.SetActive(mMTTData.Count == 0);
                            if (mMTTData != null && mMTTData.Count > 0)
                                mMTTData.Add(new WEB2_record_mtt_list.ListElement());
                            else
                            {
                                mMTTData = new List<WEB2_record_mtt_list.ListElement>() { new WEB2_record_mtt_list.ListElement() };
                            }
                            OnDataSourceLoadMoreFinished(true);
                        }
                        else
                        {
                            mMTTData = pDto.data.list ?? new List<WEB2_record_mtt_list.ListElement>();
                            NonShowed.SetActive(mMTTData.Count == 0);
                            mMTTData.Add(new WEB2_record_mtt_list.ListElement());
                            mLoopListView.InitListView(mMTTData.Count + 1, OnGetItemByIndex);
                        }
                    }
                    else
                    {
                        UIComponent.Instance.Toast(pDto.status);
                        OnDataSourceLoadMoreFinished(false);
                    }
                });
            }
            
        }


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
            var item = mLoopListView.GetShownItemByItemIndex(mNormalData.Count);
            if (item == null)
            {
                return;
            }
            var item1 = mLoopListView.GetShownItemByItemIndex(mNormalData.Count - 1);
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
        /// <summary>-1上面的菊花 1下面的菊花</summary>
        int mDirDrag = -1;
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

        LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
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

            if (mCurSegTop < 5)
            {
                if (index == mNormalData.Count)
                {
                    item = listView.NewListViewItem("ItemPrefab0");
                    UpdateLoadingTip(item);
                    return item;
                }
                item = listView.NewListViewItem("ItemInfo");
                if (item.IsInitHandlerCalled == false)
                {
                    item.IsInitHandlerCalled = true;
                }
                var itemData = mNormalData[index - 1];
                if (itemData == null)
                {
                    return null;
                }
                SetItemDataInfo(item.gameObject, itemData);

                UIEventListener.Get(item.gameObject).onClick = (p) =>
                {
                    if (CanClick() == false)
                        return;
                    lastClickTime = GetNowTime();

                    UIMineModel.mInstance.API2RecordDetail(mNormalData[index - 1].roomId, pAct =>
                    {
                        UIComponent.Instance.Show(UIType.UIMine_RecordDetailForNormal, new UIMine_RecordDetailForNormalComponent.RecordDetailForNormalData()
                        {
                            roomID = mNormalData[index - 1].roomId,
                            pDto = pAct
                        });
                    });
                };
            }
            else
            {
                if (index == mMTTData.Count)
                {
                    item = listView.NewListViewItem("ItemPrefab0");
                    UpdateLoadingTip(item);
                    return item;
                }
                item = listView.NewListViewItem("ItemInfoMTT");
                if (item.IsInitHandlerCalled == false)
                {
                    item.IsInitHandlerCalled = true;
                }
                var itemData = mMTTData[index - 1];
                if (itemData == null)
                {
                    return null;
                }
                SetMTTItemDataInfo(item.gameObject, itemData);

                UIEventListener.Get(item.gameObject).onClick = (p) =>
                {
                    if (CanClick() == false)
                        return;
                    lastClickTime = GetNowTime();

                    UIMineModel.mInstance.API2RecordMttDetail(mMTTData[index - 1].gameId, pAct =>
                    {
                        UIComponent.Instance.Show(UIType.UIMine_RecordDetailForMatch, new UIMine_RecordDetailForMatchComponent.RecordDetailForMatchData()
                        {
                            gameID = mMTTData[index - 1].gameId,
                            pDto = pAct
                        });
                    });
                };
            }

            
            return item;
        }


        #region 同时点击事件   1.1秒后才能点击
        double lastClickTime = 0;//最后一次点击
        double GetNowTime()//当前时间
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return ts.TotalMilliseconds;
        }
        /// <summary>         是否能点击  true=能点击         </summary>
        bool CanClick()
        {
            if (GetNowTime() - lastClickTime > 1100)
            {
                return true;
            }
            return false;
        }
        #endregion

        private void SetItemDataInfo(GameObject pItemGO, WEB2_record_my.ListElement itemData)
        {
            pItemGO.transform.Find("Day").GetComponent<Text>().text = TimeHelper.TimerDateStr(itemData.startTime);//日期
            pItemGO.transform.Find("SmallGo/InsuranceOpImage").gameObject.SetActive(itemData.insuranceOp);//小标签 保险
            pItemGO.transform.Find("SmallGo/jackPotOpImage").gameObject.SetActive(itemData.jackPotOp);//小标签 JP
            var tItemTitle = pItemGO.transform.Find("TimeSpecific").GetComponent<Text>();
            tItemTitle.text = TimeHelper.TimerDateMinStr(itemData.startTime) + "  " + itemData.roomName;
            tItemTitle.GetComponent<RectTransform>().sizeDelta = new Vector2(tItemTitle.preferredWidth, tItemTitle.preferredHeight);
            var TextJibie = pItemGO.transform.Find("JibieImage/TextJibie").GetComponent<Text>();

            if (itemData.roomPath == 21 || itemData.roomPath == 23)
            {
                TextJibie.text = StringHelper.ShowGold(int.Parse(itemData.blind));
            }
            else
            {
                var blinds = itemData.blind.Split('/');
                string str1 = StringHelper.ShowGold(int.Parse(blinds[0]));
                string str2 = StringHelper.ShowGold(int.Parse(blinds[1]));
                string str3 = StringHelper.ShowGold(itemData.qianzhu);
                TextJibie.text = StringHelper.ShowBlinds(str1, str2, str3);
            }

            pItemGO.transform.Find("AllTimeImage/TextAllTime").GetComponent<Text>().text = UIMineModel.mInstance.ShowGameCountTime(itemData.endTime - itemData.startTime);
            pItemGO.transform.Find("CountScore").GetComponent<Text>().text = StringHelper.GetShortSignedString(itemData.personRecord);
            pItemGO.transform.Find("CountScore").GetComponent<Text>().color = itemData.personRecord > 0 ? new Color32(217, 41, 41, 255) : new Color32(41, 217, 53, 255);
            pItemGO.transform.Find("Image_Type/Text_Type").GetComponent<Text>().text = UIMineModel.mInstance.GetRoomPathRecordListStr(itemData.roomPath);
            pItemGO.transform.Find("SmallGo").GetComponent<RectTransform>().localPosition = new Vector3(222 + tItemTitle.preferredWidth + 5, -18, 0);
            if (itemData.insuranceOp == false && itemData.jackPotOp == true)
            {
                pItemGO.transform.Find("SmallGo/jackPotOpImage").localPosition = pItemGO.transform.Find("SmallGo/InsuranceOpImage").localPosition;
            }
            else if (itemData.insuranceOp == true && itemData.jackPotOp == true)
            {
                var tLocal = pItemGO.transform.Find("SmallGo/InsuranceOpImage").localPosition;
                pItemGO.transform.Find("SmallGo/jackPotOpImage").localPosition = new Vector3(tLocal.x + 88, tLocal.y, tLocal.z);
            }

            if (mItemStartV3 == Vector3.zero)
                mItemStartV3 = pItemGO.transform.Find("JibieImage").localPosition;
            pItemGO.transform.Find("AllTimeImage").localPosition = new Vector3(mItemStartV3.x + TextJibie.preferredWidth + 70, mItemStartV3.y, mItemStartV3.z);
        }

        private void SetMTTItemDataInfo(GameObject pItemGO, WEB2_record_mtt_list.ListElement itemData)
        {
            pItemGO.transform.Find("Day").GetComponent<Text>().text = TimeHelper.TimerDateStr(itemData.startTime);//日期
            pItemGO.transform.Find("SmallGo/InsuranceOpImage").gameObject.SetActive(false);//小标签 保险
            pItemGO.transform.Find("SmallGo/jackPotOpImage").gameObject.SetActive(false);//小标签 JP
            var tItemTitle = pItemGO.transform.Find("TimeSpecific").GetComponent<Text>();
            tItemTitle.text = TimeHelper.TimerDateMinStr(itemData.startTime) + "  " + itemData.mttName;
            tItemTitle.GetComponent<RectTransform>().sizeDelta = new Vector2(tItemTitle.preferredWidth, tItemTitle.preferredHeight);
            var TextJibie = pItemGO.transform.Find("JibieImage/TextJibie").GetComponent<Text>();
            TextJibie.text = itemData.pariticipants.ToString();
            pItemGO.transform.Find("AllTimeImage/TextAllTime").GetComponent<Text>().text = StringHelper.ShowGold(itemData.registationFee);
            pItemGO.transform.Find("CountScore").GetComponent<Text>().text = itemData.rank.ToString();
            pItemGO.transform.Find("CountScore").GetComponent<Text>().color = new Color32(41, 217, 53, 255);
            pItemGO.transform.Find("Image_Type/Text_Type").GetComponent<Text>().text = "MTT";
            pItemGO.transform.Find("SmallGo").GetComponent<RectTransform>().localPosition = new Vector3(222 + tItemTitle.preferredWidth + 5, -18, 0);

            if (mItemStartV3 == Vector3.zero)
                mItemStartV3 = pItemGO.transform.Find("JibieImage").localPosition;
            pItemGO.transform.Find("AllTimeImage").localPosition = new Vector3(mItemStartV3.x + TextJibie.preferredWidth + 70, mItemStartV3.y, mItemStartV3.z);
        }

        Vector3 mItemStartV3 = Vector3.zero;

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
                DragRequestData(TimeHelper.ClientNow(), EnumLoadType.Refresh);
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
                LoopListViewItem2 item = mLoopListView.GetShownItemByItemIndex(mNormalData.Count);
                if (item == null)
                {
                    return;
                }
                mLoopListView.OnItemSizeChanged(item.ItemIndex);
                if (mLoadingTipStatus != LoadingTipStatus.WaitRelease)// || mNormalData.start_time == null || mNormalData.start_time.Count <= 0)
                {
                    return;
                }
                mLoadingTipStatus = LoadingTipStatus.WaitLoad;
                UpdateLoadingTip(item);
                DragRequestData(mNextTime, EnumLoadType.LoadMore);
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
            if (mCurSegTop < 5)
            {
                mLoopListView.SetListItemCount(mNormalData.Count + 1, false);
            }
            else
            {
                mLoopListView.SetListItemCount(mMTTData.Count + 1, false);
            }
            
            mLoopListView.RefreshAllShownItem();
        }
        #endregion


    }


}

