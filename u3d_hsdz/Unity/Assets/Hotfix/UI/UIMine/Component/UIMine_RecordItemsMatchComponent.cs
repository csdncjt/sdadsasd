using System;
using System.Collections.Generic;
using ETModel;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;
using DtoElement = ETHotfix.WEB2_record_mtt_detail.AllUsersRecords;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_RecordItemsMatchComponentAwakeSystem : AwakeSystem<UIMine_RecordItemsMatchComponent>
    {
        public override void Awake(UIMine_RecordItemsMatchComponent self)
        {
            self.Awake();
        }
    }

    public class UIMine_RecordItemsMatchComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_RecordItemsMatch);
            if (obj != null && obj is List<DtoElement>)
            {
                mDtoEles = obj as List<DtoElement>;
                DelayShow();
            }
        }

        async void DelayShow()
        {
            await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(250);
            mLoopListView.InitListView(mDtoEles.Count + 1, OnGetItemByIndex);
        }

        public override void OnHide()
        {
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
                return;

            mDtoEles = null;

            base.Dispose();
        }

        #region InitUI
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mLoopListView = rc.Get<GameObject>("ListScrollView").GetComponent<LoopListView2>();
        }
        #endregion


        #region SupperScrollView 
        private LoopListView2 mLoopListView;
        private List<DtoElement> mDtoEles = new List<DtoElement>();
        LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
        {
            if (index < 0 || mDtoEles == null || index >= mDtoEles.Count)
            {
                return null;
            }
            LoopListViewItem2 item = null;
            var tDto = mDtoEles[index];
            if (tDto == null)
            {
                return null;
            }
            item = listView.NewListViewItem("ItemInfo");
            var go = item.gameObject;
            SetItemInfo(go, tDto);
            return item;
        }

        private void SetItemInfo(GameObject obj, DtoElement tData)
        {
            obj.transform.Find("Item_RankTxt").GetComponent<Text>().text = tData.rank.ToString();
            obj.transform.Find("Item_NameTxt").GetComponent<Text>().text = tData.nickName;
            obj.transform.Find("Item_ScoreTxt").GetComponent<Text>().text = StringHelper.ShowGold((int)tData.bonus);
            WebImageHelper.SetHeadImage(obj.transform.Find("Mask/MemberIcon").GetComponent<RawImage>(), tData.head);
        }
        #endregion

    }
}

