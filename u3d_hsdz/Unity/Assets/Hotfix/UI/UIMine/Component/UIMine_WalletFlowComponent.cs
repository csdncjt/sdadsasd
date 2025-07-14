using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;
using RechargeLog_0 = ETHotfix.WEB2_wallet_recharge_log.ListElement;//充值记录
using FlowLog_1 = ETHotfix.WEB2_wallet_flow_list.ListElement;//转USDT记录
using WithdrawLog_2 = ETHotfix.WEB2_wallet_withdraw_log.ListElement;//提取记录
using FeedbackLog_3 = ETHotfix.WEB2_wallet_other_income_list.OtherIncomeBo;//返还记录
using EIdTabel = ETHotfix.WEB2_wallet_other_income_list.idTable;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_WalletFlowComponentSystem : AwakeSystem<UIMine_WalletFlowComponent>
    {
        public override void Awake(UIMine_WalletFlowComponent self)
        {
            self.Awake();
        }
    }

    public class FlowItemDto
    {
        public string order;
        public int num;
        public string status;
        public long recordTime;
    }

    /// <summary>
    /// 往来记录
    /// </summary>
    public class UIMine_WalletFlowComponent : UIBaseComponent
    {
        //因为四个接口,坑
        List<FlowItemDto> mItemFDtos;
        /// <summary>         前三个接口传 mNextId         </summary>
        long mNextId = -1;
        List<EIdTabel> mIdTables;
        private GameObject NonShowed;

        /// <summary>
        /// 充值记录 转为通用 数据  0
        /// </summary>
        List<FlowItemDto> SetRechargeItemData_0(List<RechargeLog_0> pFlowEles)
        {
            var tItemData = new List<FlowItemDto>();
            FlowItemDto tNewEle = null;
            for (int i = 0; i < pFlowEles.Count; i++)
            {
                tNewEle = new FlowItemDto();
                tNewEle.order = pFlowEles[i].orderNo;
                tNewEle.num = pFlowEles[i].status == 3 ? 0 : pFlowEles[i].chipNum;//失败 则为0
                tNewEle.status = LanguageManager.mInstance.GetLanguageForKeyMoreValue("Wallet_Flow1", pFlowEles[i].status - 1);//  pFlowEles[i].status ==1 ? "充值中" : pFlowEles[i].status == 2 ? "充值成功" : "充值失败"; // 充值状态(1-充值中,2-充值成功,3-充值失败)
                tNewEle.recordTime = pFlowEles[i].createTime;
                tItemData.Add(tNewEle);
            }
            return tItemData;
        }


        /// <summary>
        /// 转USDT记录 转为通用 数据  1
        /// </summary>
        List<FlowItemDto> SetFlowItemData_1(List<FlowLog_1> pFlowEles)
        {
            var tItemData = new List<FlowItemDto>();
            FlowItemDto tNewEle = null;
            for (int i = 0; i < pFlowEles.Count; i++)
            {
                tNewEle = new FlowItemDto();
                tNewEle.order = pFlowEles[i].flowChipNo;
                tNewEle.num = pFlowEles[i].chip;
                tNewEle.status = LanguageManager.mInstance.GetLanguageForKey("Wallet_Flow2");// "转USDT成功";
                tNewEle.recordTime = pFlowEles[i].createdTime;
                tItemData.Add(tNewEle);
            }
            return tItemData;
        }

        /// <summary>
        /// 提取记录 转为通用 数据  2
        /// </summary>
        List<FlowItemDto> SetWithDrawItemData_2(List<WithdrawLog_2> pFlowEles)
        {
            var tItemData = new List<FlowItemDto>();
            FlowItemDto tNewEle = null;
            for (int i = 0; i < pFlowEles.Count; i++)
            {
                tNewEle = new FlowItemDto();
                tNewEle.order = pFlowEles[i].withdrawNo;
                tNewEle.num = pFlowEles[i].chip * -1;
                tNewEle.status = LanguageManager.mInstance.GetLanguageForKeyMoreValue("Wallet_Flow3", pFlowEles[i].status); //pFlowEles[i].status == 0 ? "等待中" : pFlowEles[i].status == 1 ? "处理中" : pFlowEles[i].status == 2 ? "成功" : "失败";
                tNewEle.recordTime = pFlowEles[i].createdTime;
                tItemData.Add(tNewEle);
            }
            return tItemData;
        }


        /// <summary>
        /// 返还转入记录 转为通用 数据  3
        /// </summary>
        List<FlowItemDto> SetFeedbackItemData_3(List<FeedbackLog_3> pFlowEles)
        {
            var tItemData = new List<FlowItemDto>();
            FlowItemDto tNewEle = null;
            mIdTables = new List<EIdTabel>();
            EIdTabel tIdTable = null;
            for (int i = 0; i < pFlowEles.Count; i++)
            {
                tNewEle = new FlowItemDto();
                tNewEle.order = LanguageManager.mInstance.GetLanguageForKeyMoreValue("otherLog_001", pFlowEles[i].type - 1);//1-分享赚USDT, 2-系统赠送, 3-JP击中
                tNewEle.num = pFlowEles[i].chip;
                tNewEle.status = LanguageManager.mInstance.GetLanguageForKeyMoreValue("otherLog_002", pFlowEles[i].status);//pFlowEles[i].status == 0 ? "失败" : "成功";
                tNewEle.recordTime = pFlowEles[i].grantTime;
                tItemData.Add(tNewEle);

                tIdTable = new EIdTabel();
                tIdTable.id = pFlowEles[i].id;
                tIdTable.table = pFlowEles[i].table;
                mIdTables.Add(tIdTable);
            }
            return tItemData;
        }

        /// <summary>         头部切换 index     从1开始    </summary>
        private int mCurTopIndex;
        private ReferenceCollector rc;
        private GridLayoutGroup TopGrid4;
        private Text textOrder;
        private Text textNum;
        private Text textStatus;
        private Text textTime;
        public void Awake()
        {
            gridWidth1 = 0;
            mItemFDtos = new List<FlowItemDto>();
            InitUI();
            UISegmentControlComponent.SetUp(rc.transform, new UISegmentControlComponent.SegmentData()
            {
                Titles = LanguageManager.mInstance.GetLanguageForKeyMoreValues(UIMineModel.mInstance.mTransferOpen ? "Wallet_Flow54" : "Wallet_Flow53"), //new string[] { "充值记录",  "提取记录", "返还转入" },//index=1 将去掉
                OnClick = this.OnSegmentClick,
                N_S_Fonts = new[] { 48, 48 },
                N_S_Color = new Color32[] { new Color32(179, 142, 86, 255), new Color32(255, 255, 255, 255) },
                IsEffer = true
            });
            mCurTopIndex = 0;
            InitSuperView();
        }

        /// <summary>        /// 切换头部选项  从0开始        /// </summary>
        private void OnSegmentClick(GameObject go, int index)
        {
            if (mCurTopIndex == index)
            {
                return;
            }
            if (index == 0)//充值记录
            {
                UIMineModel.mInstance.APIRechargeLog_0(TimeHelper.ClientNow(), pDto =>
                    {
                        mItemFDtos = SetRechargeItemData_0(pDto.list);
                        NonShowed.SetActive(mItemFDtos.Count == 0);
                        mItemFDtos.Add(new FlowItemDto());//因为上下都要菊花  给其追加一条空数据
                        mLoopListView.SetListItemCount(mItemFDtos.Count + 1, true);
                        mLoopListView.RefreshAllShownItem();
                        mNextId = pDto.nextTime;
                    });
            }
            else if (UIMineModel.mInstance.mTransferOpen == true && index == 1)
            {
                UIMineModel.mInstance.APIFlowListLog_1(-1, pDto =>
                    {
                        mItemFDtos = SetFlowItemData_1(pDto.list);
                        NonShowed.SetActive(mItemFDtos.Count == 0);
                        mItemFDtos.Add(new FlowItemDto());//因为上下都要菊花  给其追加一条空数据
                        mLoopListView.SetListItemCount(mItemFDtos.Count + 1, true);
                        mLoopListView.RefreshAllShownItem();
                        mNextId = pDto.nextId;
                    });
            }
            else if ((UIMineModel.mInstance.mTransferOpen == false && index == 1) || (UIMineModel.mInstance.mTransferOpen == true && index == 2))//提取记录
            {
                UIMineModel.mInstance.APIWithdrawLog_2(-1, pDto =>
                {
                    mItemFDtos = SetWithDrawItemData_2(pDto.list);
                    NonShowed.SetActive(mItemFDtos.Count == 0);
                    mItemFDtos.Add(new FlowItemDto());//因为上下都要菊花  给其追加一条空数据
                    mLoopListView.SetListItemCount(mItemFDtos.Count + 1, true);
                    mLoopListView.RefreshAllShownItem();
                    mNextId = pDto.nextId;
                });
            }
            else if ((UIMineModel.mInstance.mTransferOpen == false && index == 2) || (UIMineModel.mInstance.mTransferOpen == true && index == 3))//返还转入记录
            {
                UIMineModel.mInstance.APIOtherRecordLog_3(null, pDto =>
                {
                    mItemFDtos = SetFeedbackItemData_3(pDto.data);
                    NonShowed.SetActive(mItemFDtos.Count == 0);
                    mItemFDtos.Add(new FlowItemDto());//因为上下都要菊花  给其追加一条空数据
                    mLoopListView.SetListItemCount(mItemFDtos.Count + 1, true);
                    mLoopListView.RefreshAllShownItem();
                });
            }
            mCurTopIndex = index;
            SetTopTitle();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_WalletFlow, null, ClickLeftAction);

            float gridWidthCount = rc.GetComponent<RectTransform>().rect.width;
            TopGrid4.cellSize = new Vector2(gridWidthCount * 0.25f, 125);
        }

        private void ClickLeftAction()
        {
            var tUI = UIComponent.Instance.Get(UIType.UIMine_WalletMy);
            if (tUI != null)
            {
                var tWalletMy = tUI.UiBaseComponent as UIMine_WalletMyComponent;
                tWalletMy.ShowRefresh();
            }
            UIComponent.Instance.Remove(UIType.UIMine_WalletFlow);
        }

        public override void OnHide()
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        float gridWidth1 = 0;
        private void SetItemInfo(GameObject go, FlowItemDto pDto)
        {
            var gridItem = go.transform.Find("GridItem4").GetComponent<GridLayoutGroup>();
            if (gridWidth1 == 0)
                gridWidth1 = rc.GetComponent<RectTransform>().rect.width * 0.25f;
            gridItem.cellSize = new Vector2(gridWidth1, 118);
            go.transform.Find("GridItem4/TextNum").GetComponent<Text>().text = StringHelper.ShowSignedGold(pDto.num, false);//.ToString(); //
            go.transform.Find("GridItem4/TexStatus").GetComponent<Text>().text = pDto.status;// ? "成功" : "失败"; //
            go.transform.Find("GridItem4/TextTime").GetComponent<Text>().text = TimeHelper.GetDateTimer(pDto.recordTime).ToString("yy/MM/dd HH:mm"); //
            if (pDto.order.Length > 7)
            {
                go.transform.Find("GridItem4/TextOrder").GetComponent<Text>().text = pDto.order.Substring(0, 7) + "...";
            }
            else
            {
                go.transform.Find("GridItem4/TextOrder").GetComponent<Text>().text = pDto.order;
            }
        }

        #region UI
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            textOrder = rc.Get<GameObject>("Text_Order").GetComponent<Text>();
            textNum = rc.Get<GameObject>("Text_Num").GetComponent<Text>();
            textStatus = rc.Get<GameObject>("Text_Status").GetComponent<Text>();
            textTime = rc.Get<GameObject>("Text_Time").GetComponent<Text>();
            NonShowed = rc.Get<GameObject>("NonShowed");
            NonShowed.SetActive(false);
            TopGrid4 = rc.Get<GameObject>("TopGrid4").GetComponent<GridLayoutGroup>();
        }

        private void SetTopTitle()
        {
            textStatus.text = LanguageManager.mInstance.GetLanguageForKey("Wallet_Flow6");//=状态
            if (mCurTopIndex == 0)
            {
                var t = LanguageManager.mInstance.GetLanguageForKeyMoreValues("Wallet_Flow7");
                textOrder.text = t[0];// "充值订单";
                textNum.text = t[1];//"充值数量";
                textTime.text = t[2];//"充值时间";            
            }
            else if ((UIMineModel.mInstance.mTransferOpen == true && mCurTopIndex == 1))
            {
                var t = LanguageManager.mInstance.GetLanguageForKeyMoreValues("Wallet_Flow8");
                textOrder.text = t[0];// "转USDT订单";
                textNum.text = t[1];//""转USDT数量";
                textTime.text = t[2];//""转USDT时间";
            }
            else if ((UIMineModel.mInstance.mTransferOpen == false && mCurTopIndex == 1) || (UIMineModel.mInstance.mTransferOpen == true && mCurTopIndex == 2))
            {
                var t = LanguageManager.mInstance.GetLanguageForKeyMoreValues("Wallet_Flow9");
                textOrder.text = t[0];// "提取订单";
                textNum.text = t[1];//""提取数量";
                textTime.text = t[2];//""提取时间";
            }
            else if ((UIMineModel.mInstance.mTransferOpen == false && mCurTopIndex == 2) || (UIMineModel.mInstance.mTransferOpen == true && mCurTopIndex == 3))
            {
                var t = LanguageManager.mInstance.GetLanguageForKeyMoreValues("Wallet_Flow10");
                textOrder.text = t[0];// "返还订单";
                textNum.text = t[1];//""返还数量";
                textTime.text = t[2];//""返还时间";
            }
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

            //todo充值记录   用转USDT记录代替  待接口
            UIMineModel.mInstance.APIRechargeLog_0(TimeHelper.ClientNow(), pDto =>
            {
                mItemFDtos = SetRechargeItemData_0(pDto.list);
                NonShowed.SetActive(mItemFDtos.Count == 0);
                mItemFDtos.Add(new FlowItemDto());//因为上下都要菊花  给其追加一条空数据
                mLoopListView.InitListView(mItemFDtos.Count + 1, OnGetItemByIndex);
                mNextId = pDto.nextTime;
            });
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
            if (index == mItemFDtos.Count)
            {
                item = listView.NewListViewItem("ItemPrefab0");
                UpdateLoadingTip(item);
                return item;
            }

            var tDto = mItemFDtos[index - 1];
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
            UIEventListener.Get(go).onClick = tGO =>
            {
                if (mCurTopIndex != 3)
                    UIComponent.Instance.ShowNoAnimation(UIType.UIOrderDialog, tDto.order);
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

                if (mCurTopIndex == 0)
                {
                    UIMineModel.mInstance.APIRechargeLog_0(TimeHelper.ClientNow(), pDto =>
                    {
                        mItemFDtos = SetRechargeItemData_0(pDto.list);
                        NonShowed.SetActive(mItemFDtos.Count == 0);
                        mItemFDtos.Add(new FlowItemDto());//因为上下都要菊花  给其追加一条空数据
                        OnDataSourceLoadMoreFinished(true);
                        mNextId = pDto.nextTime;
                    });
                }
                else if (UIMineModel.mInstance.mTransferOpen == true && mCurTopIndex == 1)
                {
                    UIMineModel.mInstance.APIFlowListLog_1(-1, pDto =>
                    {
                        mItemFDtos = SetFlowItemData_1(pDto.list);
                        NonShowed.SetActive(mItemFDtos.Count == 0);
                        mItemFDtos.Add(new FlowItemDto());//因为上下都要菊花  给其追加一条空数据
                        OnDataSourceLoadMoreFinished(true);
                        mNextId = pDto.nextId;
                    });
                }
                else if ((UIMineModel.mInstance.mTransferOpen == false && mCurTopIndex == 1) || (UIMineModel.mInstance.mTransferOpen == true && mCurTopIndex == 2))
                {
                    UIMineModel.mInstance.APIWithdrawLog_2(-1, pDto =>
                    {
                        mItemFDtos = SetWithDrawItemData_2(pDto.list);
                        NonShowed.SetActive(mItemFDtos.Count == 0);
                        mItemFDtos.Add(new FlowItemDto());//因为上下都要菊花  给其追加一条空数据
                        OnDataSourceLoadMoreFinished(true);
                        mNextId = pDto.nextId;
                    });
                }
                else if ((UIMineModel.mInstance.mTransferOpen == false && mCurTopIndex == 2) || (UIMineModel.mInstance.mTransferOpen == true && mCurTopIndex == 3))
                {
                    UIMineModel.mInstance.APIOtherRecordLog_3(null, pDto =>
                    {
                        mItemFDtos = SetFeedbackItemData_3(pDto.data);
                        NonShowed.SetActive(mItemFDtos.Count == 0);
                        mItemFDtos.Add(new FlowItemDto());//因为上下都要菊花  给其追加一条空数据
                        OnDataSourceLoadMoreFinished(true);
                    });
                }
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
                LoopListViewItem2 item = mLoopListView.GetShownItemByItemIndex(mItemFDtos.Count);
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

                if (mCurTopIndex == 0)
                {
                    UIMineModel.mInstance.APIRechargeLog_0(mNextId, pDto =>
                    {
                        if (pDto.list != null && pDto.list.Count > 0)
                        {
                            mNextId = pDto.nextTime;
                            mItemFDtos.RemoveAt(mItemFDtos.Count - 1);
                            mItemFDtos.AddRange(SetRechargeItemData_0(pDto.list));
                            NonShowed.SetActive(mItemFDtos.Count == 0);
                            mItemFDtos.Add(new FlowItemDto());//因为上下都要菊花  给其追加一条空数据
                        }
                        OnDataSourceLoadMoreFinished(true);
                    });
                }
                else if (UIMineModel.mInstance.mTransferOpen == true && mCurTopIndex == 1)
                {
                    UIMineModel.mInstance.APIFlowListLog_1((int)mNextId, pDto =>
                    {
                        if (pDto.list != null && pDto.list.Count > 0)
                        {
                            mNextId = pDto.nextId;
                            mItemFDtos.RemoveAt(mItemFDtos.Count - 1);
                            mItemFDtos.AddRange(SetFlowItemData_1(pDto.list));
                            NonShowed.SetActive(mItemFDtos.Count == 0);
                            mItemFDtos.Add(new FlowItemDto());//因为上下都要菊花  给其追加一条空数据
                        }
                        OnDataSourceLoadMoreFinished(true);
                    });
                }
                else if ((UIMineModel.mInstance.mTransferOpen == false && mCurTopIndex == 1) || (UIMineModel.mInstance.mTransferOpen == true && mCurTopIndex == 2))
                {
                    UIMineModel.mInstance.APIWithdrawLog_2((int)mNextId, pDto =>
                    {
                        if (pDto.list != null && pDto.list.Count > 0)
                        {
                            mNextId = pDto.nextId;
                            mItemFDtos.RemoveAt(mItemFDtos.Count - 1);
                            mItemFDtos.AddRange(SetWithDrawItemData_2(pDto.list));
                            NonShowed.SetActive(mItemFDtos.Count == 0);
                            mItemFDtos.Add(new FlowItemDto());//因为上下都要菊花  给其追加一条空数据
                        }
                        OnDataSourceLoadMoreFinished(true);
                    });
                }
                else if ((UIMineModel.mInstance.mTransferOpen == false && mCurTopIndex == 2) || (UIMineModel.mInstance.mTransferOpen == true && mCurTopIndex == 3))
                {
                    UIMineModel.mInstance.APIOtherRecordLog_3(mIdTables, pDto =>
                    {
                        if (pDto.data != null && pDto.data.Count > 0)
                        {
                            mItemFDtos.RemoveAt(mItemFDtos.Count - 1);
                            mItemFDtos.AddRange(SetFeedbackItemData_3(pDto.data));
                            NonShowed.SetActive(mItemFDtos.Count == 0);
                            mItemFDtos.Add(new FlowItemDto());//因为上下都要菊花  给其追加一条空数据
                        }
                        OnDataSourceLoadMoreFinished(true);
                    });
                }
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
                mLoopListView.SetListItemCount(mItemFDtos.Count + 1, false);
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
            LoopListViewItem2 item = mLoopListView.GetShownItemByItemIndex(mItemFDtos.Count);
            if (item == null)
            {
                return;
            }
            LoopListViewItem2 item1 = mLoopListView.GetShownItemByItemIndex(mItemFDtos.Count - 1);
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
