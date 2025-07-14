using System;
using System.Collections;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;


namespace ETHotfix
{
    [ObjectSystem]
    public class UIMatch_FilterComponentSystem : AwakeSystem<UIMatch_FilterComponent>
    {
        public override void Awake(UIMatch_FilterComponent self)
        {
            self.Awake();
        }
    }

    public class UIMatch_FilterComponent : UIBaseComponent
    {
        public sealed class MatchFilterData
        {
            public FilterData successData;
            public List<int> mFilterOld;
            public List<int> mFilternBlindOld;//盲注多选列表
            //public bool mHallShow;//是否大厅点击进来的
            public int gameType;
            public Dictionary<int, Dictionary<int, List<ETHotfix.WEB_room_blindData.GameBlindElement>>> mDict;
        }
        public delegate void FilterData(List<int> tt, List<int> tt1, int gameType);
        private ReferenceCollector rc;
        private List<Toggle> mToggle1_Room;//房间类型 aof 奥马哈.....
        private List<Toggle> mToggle2_Blind;//盲注
        private List<Toggle> mToggle3_Number;//人数
        private List<Toggle> mToggle4_Front;//前注
        private List<Toggle> mToggle5_Insurance;//保险
        private List<Toggle> mToggle6_Seat;//座位
        private List<int> mSelecteds;
        private Dictionary<int, List<ETHotfix.WEB_room_blindData.GameBlindElement>> mTypeDict;

        private List<int> mBlindSelecteds;

        private MatchFilterData mFilterData;
        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMatch_Filter, RightClearAction);
            if (obj != null && obj is MatchFilterData)
            {
                mFilterData = obj as MatchFilterData;
                if (mFilterData.mFilterOld != null && mFilterData.mFilterOld.Count > 0)
                {
                    ClickTypeRoomPath(null, mFilterData.mFilterOld[0]);
                    //ClickBlind(null, mFilterData.mFilterOld[1]);
                    ClickPeople(null, mFilterData.mFilterOld[2]);
                    ClickQianzhu(null, mFilterData.mFilterOld[3]);
                    ClickBaoxian(null, mFilterData.mFilterOld[4]);
                    ClickZuowei(null, mFilterData.mFilterOld[5]);
                    mSelecteds = mFilterData.mFilterOld;
                    //RoomPathGO.SetActive(mFilterData.mHallShow);//筛选roomPath显示与否   
                    RoomPathGO.SetActive(false);
                    mFirstRoomPath = mFilterData.mFilterOld[0];
                }

                if (mFilterData.mFilternBlindOld != null && mFilterData.mFilternBlindOld.Count > 0)
                {
                    mBlindSelecteds = mFilterData.mFilternBlindOld;
                }

                FreshBlinds();

            }
        }
        int mFirstRoomPath = -1;
        /// <summary>         左上角 清空         </summary>
        private void RightClearAction()
        {
            ClickTypeRoomPath(null, -1);
            for (int i = 0; i < mToggle1_Room.Count; i++)
            {
                mToggle1_Room[i].isOn = false;
                mToggle1_Room[i].GetComponentInChildren<Text>().color = new Color32(244, 197, 106, 255);
            }

            mBlindSelecteds = new List<int>() { };

            if (RoomPathGO.activeInHierarchy)//显示的,全部清空
            {
                mSelecteds = new List<int>() { -1, -1, -1, -1, -1, -1 };
            }
            else
            {
                mSelecteds = new List<int>() { mFirstRoomPath, -1, -1, -1, -1, -1 };
            }
        }

        public override void OnHide()
        {
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();
        }
        int[] mRoomPaths = new int[] { 61, 91, 31, 41, 21 };//61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
        //小中大列表
        int[] mBlinds1 = new int[] {10, 25, 50, 100, 200};
        int[] mBlinds2 = new int[] {500, 1000, 2500, 5000};
        int[] mBlinds3 = new int[] {10000, 20000, 50000};

        /// <summary>         前注 保险 座位         </summary>
        int[] mFirstIns = new int[] { 1, 0 };
        #region InitUI
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mToggle1_Room = new List<Toggle>();
            mToggle2_Blind = new List<Toggle>();
            mToggle3_Number = new List<Toggle>();
            mToggle4_Front = new List<Toggle>();
            mToggle5_Insurance = new List<Toggle>();
            mToggle6_Seat = new List<Toggle>();
            mSelecteds = new List<int>() { -1, -1, -1, -1, -1, -1 };
            mBlindSelecteds = new List<int>() {};
            var tStr = string.Empty;

            for (int i = 1; i < 5; i++)//类型 
            {
                tStr = "ToggleType_" + i.ToString();
                UIEventListener.Get(rc.Get<GameObject>(tStr), mRoomPaths[i - 1]).onIntClick = ClickTypeRoomPath;
                mToggle1_Room.Add(rc.Get<GameObject>(tStr).GetComponent<Toggle>());
            }

            //FreshBlinds();

            for (int i = 1; i < 4; i++)//盲注 人数
            {
                //tStr = "ToggleBlind_" + i.ToString();
                //UIEventListener.Get(rc.Get<GameObject>(tStr), mBlinds[i - 1]).onIntClick = ClickBlind;
                //mToggle2_Blind.Add(rc.Get<GameObject>(tStr).GetComponent<Toggle>());
                tStr = "TogglePeople_" + i.ToString();
                UIEventListener.Get(rc.Get<GameObject>(tStr), i).onIntClick = ClickPeople;
                mToggle3_Number.Add(rc.Get<GameObject>(tStr).GetComponent<Toggle>());
            }
            for (int i = 1; i < 3; i++)//前注 保险 座位
            {
                tStr = "ToggleQianzhu_" + i.ToString();
                UIEventListener.Get(rc.Get<GameObject>(tStr), mFirstIns[i - 1]).onIntClick = ClickQianzhu;
                mToggle4_Front.Add(rc.Get<GameObject>(tStr).GetComponent<Toggle>());
                tStr = "ToggleBaoxian_" + i.ToString();
                UIEventListener.Get(rc.Get<GameObject>(tStr), mFirstIns[i - 1]).onIntClick = ClickBaoxian;
                mToggle5_Insurance.Add(rc.Get<GameObject>(tStr).GetComponent<Toggle>());
                tStr = "ToggleZuowei_" + i.ToString();
                UIEventListener.Get(rc.Get<GameObject>(tStr), mFirstIns[i - 1]).onIntClick = ClickZuowei;
                mToggle6_Seat.Add(rc.Get<GameObject>(tStr).GetComponent<Toggle>());
            }
            UIEventListener.Get(rc.Get<GameObject>("BtnConfirm")).onClick = ClickBtnConfirm;
            RoomPathGO = rc.Get<GameObject>("RoomPathGO");
        }
        private GameObject RoomPathGO;

        private void FreshBlinds()
        {
            mFilterData.mDict.TryGetValue(mFilterData.gameType, out mTypeDict);

            if (mTypeDict == null)
            {
                return;
            }

            List<ETHotfix.WEB_room_blindData.GameBlindElement> list;

            var tStr = string.Empty;
            //小
            for (int i = 1; i < 9; i++)
            {
                tStr = "ToggleBlind_" + i.ToString();

                mTypeDict.TryGetValue(0, out list);
                if (list == null ||  i > list.Count)
                {
                    rc.Get<GameObject>(tStr).SetActive(false);
                    continue;
                }

                string valueStr = list[i - 1].blindnote;
                int minValue = 0;
                if (mFilterData.gameType == 21)//短牌
                {
                    minValue = int.Parse(valueStr);
                    rc.Get<GameObject>(tStr).GetComponentInChildren<Text>().text = StringHelper.ShowGold(minValue);
                }
                else
                {
                    minValue = int.Parse(valueStr.Split('/')[0]);
                    int maxValue = int.Parse(valueStr.Split('/')[1]);
                    rc.Get<GameObject>(tStr).GetComponentInChildren<Text>().text = StringHelper.ShowGold(minValue, false) + "/" + StringHelper.ShowGold(maxValue, false);
                }
                UIEventListener.Get(rc.Get<GameObject>(tStr), minValue).onIntClick = ClickBlind;
                mToggle2_Blind.Add(rc.Get<GameObject>(tStr).GetComponent<Toggle>());

            }
            //中
            for (int i = 1; i < 9; i++)
            {
                tStr = "ToggleBlind_" + (i + 8).ToString();
                mTypeDict.TryGetValue(1, out list);
                if (list == null || i > list.Count)
                {
                    rc.Get<GameObject>(tStr).SetActive(false);
                    continue;
                }
                string valueStr = list[i - 1].blindnote;
                int minValue = 0;
                if (mFilterData.gameType == 21)//短牌
                {
                    minValue = int.Parse(valueStr);
                    rc.Get<GameObject>(tStr).GetComponentInChildren<Text>().text = StringHelper.ShowGold(minValue);
                }
                else
                {
                    minValue = int.Parse(valueStr.Split('/')[0]);
                    int maxValue = int.Parse(valueStr.Split('/')[1]);
                    rc.Get<GameObject>(tStr).GetComponentInChildren<Text>().text = StringHelper.ShowGold(minValue, false) + "/" + StringHelper.ShowGold(maxValue, false);
                }

                UIEventListener.Get(rc.Get<GameObject>(tStr), minValue).onIntClick = ClickBlind;
                mToggle2_Blind.Add(rc.Get<GameObject>(tStr).GetComponent<Toggle>());
            }
            //大
            for (int i = 1; i < 9; i++)
            {
                tStr = "ToggleBlind_" + (i + 16).ToString();

                mTypeDict.TryGetValue(2, out list);
                if (list == null || i > list.Count)
                {
                    rc.Get<GameObject>(tStr).SetActive(false);
                    continue;
                }
                string valueStr = list[i - 1].blindnote;
                int minValue = 0;
                if (mFilterData.gameType == 21)//短牌
                {
                    minValue = int.Parse(valueStr);
                    rc.Get<GameObject>(tStr).GetComponentInChildren<Text>().text = StringHelper.ShowGold(minValue);
                }
                else
                {
                    minValue = int.Parse(valueStr.Split('/')[0]);
                    int maxValue = int.Parse(valueStr.Split('/')[1]);
                    rc.Get<GameObject>(tStr).GetComponentInChildren<Text>().text = StringHelper.ShowGold(minValue, false) + "/" + StringHelper.ShowGold(maxValue, false);
                }
                UIEventListener.Get(rc.Get<GameObject>(tStr), minValue).onIntClick = ClickBlind;
                mToggle2_Blind.Add(rc.Get<GameObject>(tStr).GetComponent<Toggle>());
            }
        }

        private void ClickBtnConfirm(GameObject go)
        {
            var tCount = 0;
            for (int i = 0; i < mSelecteds.Count; i++)
            {
                tCount += mSelecteds[i];
            }

            tCount += mBlindSelecteds.Count;

            if (tCount <= 0)
            {
                Log.Debug("并无筛选内容");
            }

            mFilterData.successData(mSelecteds, mBlindSelecteds, mFilterData.gameType);
            UIComponent.Instance.Remove(UIType.UIMatch_Filter);
        }

        private void ClickTypeRoomPath(GameObject go, int index)
        {
            if (index == mRoomPaths[2])//AOF
            {
                //SetOtherCanSelect(true, true, true, false, true);
                SetOtherCanSelect(true, true, false, false, false);
            }
            else
            {
                //SetOtherCanSelect(true, true, true, true, true);
                SetOtherCanSelect(true, true, false, false, false);
            }
            for (int i = 1; i < mSelecteds.Count; i++)
            {
                mSelecteds[i] = -1;
            }
            if (mSelecteds[0] != index)
                mSelecteds[0] = index;
            else
                mSelecteds[0] = -1;

            //SetItemToggleLabel(mToggle1_Room, 0, mRoomPaths, index);
        }

        /// <summary>第二个选项开始        是否可以选择 </summary>
        private void SetOtherCanSelect(bool show2, bool show3, bool show4, bool show5, bool show6)
        {
            for (int i = 0; i < mToggle2_Blind.Count; i++)
            {
                mToggle2_Blind[i].isOn = false;
                //mToggle2_Blind[i].GetComponentInChildren<Text>().color = show2 ? new Color32(244, 197, 106, 255) : new Color32(255, 255, 255, 30);//字颜色只有aof 保险才有关系
                mToggle2_Blind[i].enabled = show2;
            }
            for (int i = 0; i < mToggle3_Number.Count; i++)
            {
                mToggle3_Number[i].isOn = false;
                //mToggle3_Number[i].GetComponentInChildren<Text>().color = show3 ? new Color32(244, 197, 106, 255) : new Color32(255, 255, 255, 30);
                mToggle3_Number[i].enabled = show3;
            }
            for (int i = 0; i < mToggle4_Front.Count; i++)
            {
                mToggle4_Front[i].isOn = false;
                //mToggle4_Front[i].GetComponentInChildren<Text>().color = show4 ? new Color32(244, 197, 106, 255) : new Color32(255, 255, 255, 30);
                mToggle4_Front[i].enabled = show4;
            }
            for (int i = 0; i < mToggle5_Insurance.Count; i++)
            {
                mToggle5_Insurance[i].isOn = false;
                mToggle5_Insurance[i].GetComponentInChildren<Text>().color = show5 ? new Color32(244, 197, 106, 255) : new Color32(255, 255, 255, 30);////字颜色只有aof 保险才有关系
                mToggle5_Insurance[i].enabled = show5;
            }
            for (int i = 0; i < mToggle6_Seat.Count; i++)
            {
                mToggle6_Seat[i].isOn = false;
                //mToggle6_Seat[i].GetComponentInChildren<Text>().color = show6 ? new Color32(244, 197, 106, 255) : new Color32(255, 255, 255, 30);
                mToggle6_Seat[i].enabled = show6;
            }
        }

        void SetItemData(Toggle pToggle, int index, int curSelect)
        {
            if (pToggle.enabled == false) return;

            if (mSelecteds[index] == curSelect)
            {
                mSelecteds[index] = -1;
            }
            else
            {
                mSelecteds[index] = curSelect;
            }
        }

        void SetItemToggleLabel(List<Toggle> pToggles, int pSelectIndex, int[] pNums, int pToggleBinding)//toggle绑定的值
        {
            if (pToggles == null || pToggles.Count == 0 || pToggles[0].enabled == false) return;

            for (int i = 0; i < pToggles.Count; i++)
            {
                pToggles[i].GetComponentInChildren<Text>().color = new Color32(244, 197, 106, 255);
            }
            var valueIndex = Array.IndexOf(pNums, pToggleBinding);
            if (mSelecteds[pSelectIndex] != -1)
            {
                pToggles[valueIndex].GetComponentInChildren<Text>().color = new Color32(244, 197, 106, 255);
            }

            int arrayIndex = -1;
            for (int i = 0; i < pNums.Length; i++)
            {
                pToggles[i].isOn = false;
                if (pNums[i] == pToggleBinding)
                    arrayIndex = i;
            }
            if (arrayIndex >= 0 && mSelecteds[pSelectIndex] != -1)
                pToggles[arrayIndex].isOn = true;
        }


        private void ClickZuowei(GameObject go, int index)
        {
            SetItemData(mToggle6_Seat[0], 5, index);
            SetItemToggleLabel(mToggle6_Seat, 5, mFirstIns, index);
        }

        private void ClickBaoxian(GameObject go, int index)
        {
            SetItemData(mToggle5_Insurance[0], 4, index);
            SetItemToggleLabel(mToggle5_Insurance, 4, mFirstIns, index);
        }

        private void ClickQianzhu(GameObject go, int index)
        {
            SetItemData(mToggle4_Front[0], 3, index);
            SetItemToggleLabel(mToggle4_Front, 3, mFirstIns, index);
        }

        private void ClickPeople(GameObject go, int index)
        {
            SetItemData(mToggle3_Number[0], 2, index);
            SetItemToggleLabel(mToggle3_Number, 2, new int[] { 1, 2, 3 }, index);
        }

        private void ClickBlind(GameObject go, int index)
        {
            if (mBlindSelecteds.Contains(index))
            {
                mBlindSelecteds.Remove(index);
            }
            else
            {
                mBlindSelecteds.Add(index);
            }

            //SetItemData(mToggle2_Blind[0], 1, index);
            //SetItemToggleLabel(mToggle2_Blind, 1, mBlinds, index);
        }
        #endregion

    }
}

