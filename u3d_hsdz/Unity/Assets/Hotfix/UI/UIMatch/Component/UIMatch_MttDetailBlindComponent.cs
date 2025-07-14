using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;
using DtoRoomInfo = ETHotfix.WEB2_room_mtt_view.Data;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMatch_MttDetailBlindComponentSystem : AwakeSystem<UIMatch_MttDetailBlindComponent>
    {
        public override void Awake(UIMatch_MttDetailBlindComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名: </summary>
    public class UIMatch_MttDetailBlindComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Text textBlindNum;

        DtoRoomInfo roomInfo;

        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            roomInfo = obj as DtoRoomInfo;
           

            InitSuperView();
        }

        void SetItemInfo(GameObject go, int index)
        {

            Text textLevel = go.transform.Find("content/Text_Level").GetComponent<Text>();
            Text textBlind = go.transform.Find("content/Text_Blind").GetComponent<Text>();
            Text textAnte = go.transform.Find("content/Text_Ante").GetComponent<Text>();
            Text textTime = go.transform.Find("content/Text_Time").GetComponent<Text>();

            textLevel.text = (index + 1).ToString();
            int sb = MTTGameUtil.BlindAtLevel(index, (MTT_GameType)roomInfo.blindType, (float)roomInfo.blindScale);
            int ante = MTTGameUtil.AnteAtLevel(index, (MTT_GameType)roomInfo.blindType, (float)roomInfo.blindScale);

            textBlind.text = StringHelper.ShowGold(sb, false) + "/" + StringHelper.ShowGold(sb * 2, false);
            textAnte.text = StringHelper.ShowGold(ante, false);
            textTime.text = string.Format(LanguageManager.Get("UITexasReport_Text_MatchNextBlindTime"), roomInfo.updateCycle);
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
            textBlindNum = rc.Get<GameObject>("Text_BlindNum").GetComponent<Text>();

        }

        #region SupperScrollView 
        private LoopListView2 mLoopListView;

        private void InitSuperView()
        {
            if (mLoopListView == null)
            {
                mLoopListView = rc.Get<GameObject>("Blind_scrollview").GetComponent<LoopListView2>();
                mLoopListView.mOnEndDragAction = OnEndDrag;
                int levelCount = MTTGameUtil.numOfLevel((MTT_GameType)roomInfo.blindType);
                mLoopListView.InitListView(levelCount, OnGetItemByIndex);

                textBlindNum.text = string.Format(LanguageManager.Get("MTT_Blind_Num"), levelCount);
            }

        }

        LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)//mCurTop不同时
        {
            if (index < 0)
            {
                return null;
            }
            LoopListViewItem2 item = null;

            item = listView.NewListViewItem("ItemInfo");
            ListItem2 itemScript = item.GetComponent<ListItem2>();
            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
            }
            var go = item.gameObject;
            SetItemInfo(go, index);
            return item;
        }

        void OnEndDrag()
        {
        }

        #endregion
    }
}
