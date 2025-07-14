using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIReportComponentSystem : AwakeSystem<UITexasReportComponent>
    {
        public override void Awake(UITexasReportComponent self)
        {
            self.Awake();
        }
    }


    public class ReportPlayer
    {
        public int userId;
        public string nickName;
        public int hand;
        public int comeIn;
        public int score;
    }


    public class UITexasReportComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Image imageMenuMask;
        private Text textTime;
        private Text textAllnum;
        private Text textInsnum;
        ScrollRect scrollrect;
        RectTransform rectcontent;
        GameObject titleViewer;
        GameObject tPlayingBar;
        GameObject tLeaveBar;
        GameObject viewerList;
        GameObject imageHead;
        REQ_GAME_REAL_TIME mDto;
        Dictionary<int, List<ReportPlayer>> mDicTypePlayerInfo;
        private GameObject title_Leave;
        private LayoutElement NullHeight;
        private GameObject InsurancePool;

        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            if (obj != null && obj is REQ_GAME_REAL_TIME)
            {
                var rec = obj as REQ_GAME_REAL_TIME;
                if (rec.status == 1)
                {
                    return;
                }
                mDto = rec;
                imageMenuMask.gameObject.SetActive(true);
                UpdateViewList();
            }
            else
            {
                Log.Error("数据类型错误");
            }
        }
        public override void OnHide()
        {
            isLoad = false;
        }

        public override void Dispose()
        {
            isLoad = false;
            if (IsDisposed)
            {
                return;
            }
            base.Dispose();
        }
        #region UI     
        void UpdateViewList()
        {
            GameObject tItem;
            int totalLen = 0;
            if (string.IsNullOrEmpty(mDto.nickNameStr) == false)//玩家模块
            {
                var tAllNum = 0;
                var tPlayerNames = mDto.nickNameStr.Split(new string[] { "@%" }, StringSplitOptions.None);
                mDicTypePlayerInfo = new Dictionary<int, List<ReportPlayer>>();

                List<ReportPlayer> tInfo_0 = new List<ReportPlayer>();
                List<ReportPlayer> tInfo_1 = new List<ReportPlayer>();
                List<ReportPlayer> tInfo_2 = new List<ReportPlayer>();
                ReportPlayer tSignPlayer;
                for (int i = 0; i < tPlayerNames.Length; i++)
                {
                    tSignPlayer = new ReportPlayer();
                    tSignPlayer.userId = mDto.userIdArray[i];
                    tSignPlayer.nickName = tPlayerNames[i];
                    tSignPlayer.hand = mDto.handsArray[i];
                    tSignPlayer.comeIn = mDto.bringArray[i];
                    tSignPlayer.score = mDto.plArray[i];

                    if (mDto.playingPersonArray[i] == 0)// 用户状态 0在玩 1不在 2提前离桌
                    {
                        tInfo_0.Add(tSignPlayer);
                    }
                    else if (mDto.playingPersonArray[i] == 1)
                    {
                        tInfo_1.Add(tSignPlayer);
                    }
                    else if (mDto.playingPersonArray[i] == 2)
                    {
                        tInfo_2.Add(tSignPlayer);
                    }
                    tAllNum = tAllNum + mDto.bringArray[i];
                }

                try
                {
                    tInfo_0.Sort((x, y) => { return -x.score.CompareTo(y.score); });
                    tInfo_1.Sort((x, y) => { return -x.score.CompareTo(y.score); });
                    tInfo_2.Sort((x, y) => { return -x.score.CompareTo(y.score); });
                }
                catch
                {

                }

                mDicTypePlayerInfo[0] = tInfo_0;
                mDicTypePlayerInfo[1] = tInfo_1;
                mDicTypePlayerInfo[2] = tInfo_2;//造数据

                for (int i = 0; i < mDicTypePlayerInfo[0].Count; i++)
                {
                    tItem = GameObject.Instantiate(tPlayingBar, rectcontent.transform);
                    SetInfos(tItem, mDicTypePlayerInfo[0][i], true);
                    tItem.SetActive(true);
                }
                for (int i = 0; i < mDicTypePlayerInfo[1].Count; i++)
                {
                    tItem = GameObject.Instantiate(tLeaveBar, rectcontent.transform);
                    SetInfos(tItem, mDicTypePlayerInfo[1][i], false);
                    tItem.SetActive(true);
                }
                totalLen = totalLen + 100 * (mDicTypePlayerInfo[0].Count + mDicTypePlayerInfo[1].Count);


                if (mDicTypePlayerInfo[2].Count > 0)
                {
                    totalLen = totalLen + 100 * 1;
                    tItem = GameObject.Instantiate(title_Leave, rectcontent.transform);
                    tItem.SetActive(true);
                    totalLen += 80;
                    for (int i = 0; i < mDicTypePlayerInfo[2].Count; i++)
                    {
                        tItem = GameObject.Instantiate(tLeaveBar, rectcontent.transform);
                        SetInfos(tItem, mDicTypePlayerInfo[2][i], false);
                        tItem.SetActive(true);
                    }
                    totalLen = totalLen + mDicTypePlayerInfo[2].Count * 15;
                }
                textAllnum.text = StringHelper.ShowGold(tAllNum);
                textInsnum.text = mDto.insuranceSum.ToString();
            }

            if (string.IsNullOrEmpty(mDto.watcherNickname) == false)//观众模块
            {
                tItem = GameObject.Instantiate(titleViewer, rectcontent.transform);
                tItem.gameObject.SetActive(true);
                totalLen = totalLen + 100 * 1;//观众文字一行
                var tNicks = mDto.watcherNickname.Split(new string[] { "@%" }, StringSplitOptions.None);//名字
                var tHeads = mDto.watcherHeadStr.Split(new string[] { "@%" }, StringSplitOptions.None);//头像  暂空
                tItem.transform.Find("Text_ReportViewer").GetComponent<Text>().text = CPErrorCode.LanguageDescription(20052, new List<object>() { tNicks.Length });// "观众(" + tNicks.Length.ToString() + ")";
                GameObject headView;
                headView = GameObject.Instantiate(viewerList, rectcontent.transform);
                headView.gameObject.SetActive(true);
                totalLen = totalLen + 100 * 1;
                for (int i = 0; i < tNicks.Length; i++)
                {
                    tItem = GameObject.Instantiate(imageHead, headView.transform);
                    tItem.transform.Find("Text").GetComponent<Text>().text = tNicks[i];
                    if (tHeads[i] != "-1")
                    {
                        var tRaw = tItem.transform.Find("Mask/ImageIcon").GetComponent<RawImage>();
                        WebImageHelper.SetHeadImage(tRaw, tHeads[i]);
                    }
                    tItem.SetActive(true);
                    tItem.transform.Find("ImageGray").gameObject.SetActive(mDto.watcherLeaveRoomArray[i] == 1);
                }
                totalLen = totalLen + 220 * tNicks.Length / 5 + 400;
            }
            if (mDto.roomLeftTime > 0)
            {
                mRoomLeaveTime = mDto.roomLeftTime;
                textTime.text = TimeHelper.ShowRemainingSemicolon(mRoomLeaveTime);
                ShowLeaveTimer();
            }
            rectcontent.sizeDelta = new Vector2(0, Math.Max(totalLen, rectcontent.GetComponent<RectTransform>().sizeDelta.y));
        }
        int mRoomLeaveTime;
        bool isLoad = true;
        private async void ShowLeaveTimer()
        {
            TimerComponent mTC = Game.Scene.ModelScene.GetComponent<TimerComponent>();
            while (mRoomLeaveTime >= 0 && isLoad)
            {
                await mTC.WaitAsync(1000);
                mRoomLeaveTime--;
                if (textTime != null)
                    textTime.text = TimeHelper.ShowRemainingSemicolon(mRoomLeaveTime);
            }
            if (textTime != null)
                textTime.text = "00:00";
        }

        string ColorText(bool onLine, string str)
        {
            string tt = "";
            if (onLine)
            {
                tt = "<color=\"#E9BF80FF\">" + str + "</color>";
            }
            else
            {
                tt = "<color=\"#E9BF807D\">" + str + "</color>";
            }
            return tt;
        }

        void SetInfos(GameObject objTemp, ReportPlayer pDto, bool onLine)
        {
            if (pDto.userId == GameCache.Instance.nUserId)
            {

                
                objTemp.transform.Find("Text_Name").GetComponent<Text>().text = ColorText(onLine, pDto.nickName);
                objTemp.transform.Find("Text_Num").GetComponent<Text>().text = ColorText(onLine, pDto.hand.ToString());
                objTemp.transform.Find("Text_All").GetComponent<Text>().text = ColorText(onLine, StringHelper.ShowGold(pDto.comeIn));
                objTemp.transform.Find("Text_Count").GetComponent<Text>().text = ColorText(onLine, StringHelper.GetSignedString(pDto.score));
                objTemp.transform.Find("SelfGo").gameObject.SetActive(true);
            }
            else
            {
                objTemp.transform.Find("Text_Name").GetComponent<Text>().text = pDto.nickName;
                objTemp.transform.Find("Text_Num").GetComponent<Text>().text = pDto.hand.ToString();
                objTemp.transform.Find("Text_All").GetComponent<Text>().text = StringHelper.ShowGold(pDto.comeIn);
                var tCount = objTemp.transform.Find("Text_Count").GetComponent<Text>();
                tCount.text = StringHelper.ShowGold(pDto.score);

                byte a = onLine ? (byte)255 : (byte)125;
                if (pDto.score > 0)
                {
                    tCount.color = new Color32(184, 43, 48, a);
                    tCount.text = "+" + tCount.text;
                }
                else if (pDto.score < 0)
                    tCount.color = new Color32(66, 200, 113, a);
            }
        }


        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            imageMenuMask = rc.Get<GameObject>("Image_MenuMask").GetComponent<Image>();
            textTime = rc.Get<GameObject>("Text_Time").GetComponent<Text>();
            UIEventListener.Get(imageMenuMask.gameObject).onClick = onClickMenuMask;
            scrollrect = rc.Get<GameObject>("ScrollView").GetComponent<ScrollRect>();
            rectcontent = rc.Get<GameObject>("Content").GetComponent<RectTransform>();
            titleViewer = rc.Get<GameObject>("title_viewer");
            tPlayingBar = rc.Get<GameObject>("OnLine");
            tLeaveBar = rc.Get<GameObject>("Left_List");
            viewerList = rc.Get<GameObject>("Viewer_List");
            imageHead = rc.Get<GameObject>("Image_Head");
            textAllnum = rc.Get<GameObject>("Text_Allnum").GetComponent<Text>();
            textInsnum = rc.Get<GameObject>("Text_Insnum").GetComponent<Text>();
            isLoad = true;
            title_Leave = rc.Get<GameObject>("title_Leave");
            NullHeight = rc.Get<GameObject>("NullHeight").GetComponent<LayoutElement>();

            NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
            float realTop = safeArea.top * 1242 / safeArea.width;
            NullHeight.minHeight = NullHeight.minHeight + realTop;

            UIEventListener.Get(rc.Get<GameObject>("BtnShowProblem")).onClick = ClickBtnShowProblem;
            InsurancePool = rc.Get<GameObject>("InsurancePool");
            if (GameCache.Instance.room_path == RoomPath.NormalAof.GetHashCode() || GameCache.Instance.room_path == RoomPath.OmahaAof.GetHashCode() || GameCache.Instance.room_path == RoomPath.DPAof.GetHashCode())
            {
                InsurancePool.SetActive(false);
            }
        }

        private void ClickBtnShowProblem(GameObject go)
        {
            UIComponent.Instance.ShowNoAnimation(UIType.UITexasRule, EnumRuleOpenLoad.ReportLoad);
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UITexasReport);
        }

        private void onClickMenuMask(GameObject go)
        {
            imageMenuMask.gameObject.SetActive(false);
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UITexasReport);
        }
        #endregion


    }
}

