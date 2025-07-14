using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using RecordDetailDto = ETHotfix.WEB2_record_mtt_detail.ResponseData;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_RecordDetailForMatchComponentAwakeSystem : AwakeSystem<UIMine_RecordDetailForMatchComponent>
    {
        public override void Awake(UIMine_RecordDetailForMatchComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// MTT战绩详情
    /// </summary>
    public class UIMine_RecordDetailForMatchComponent : UIBaseComponent
    {
        public sealed class RecordDetailForMatchData
        {
            public int gameID;
            public RecordDetailDto pDto;
        }

        private int mGameId;
        private ReferenceCollector rc;

        int mLocalPosY = 1581;
        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            mLocalPosY = 1701;
            SetUpNav(UIType.UIMine_RecordDetailForMatch);
            if (obj != null && obj is RecordDetailForMatchData)
            {
                var data = obj as RecordDetailForMatchData;
                var pDto = data.pDto;
                if (pDto.data == null) return;

                SetFindLabelText("TitleNameTxt", pDto.data.mttName);
                // SetFindLabelText("CountTxt", TimeHelper.ShowRemainingTimer(pDto.data.playTime * 1000, false));
                // SetFindLabelText("Text_Type", UIMineModel.mInstance.GetRoomPathRecordListStr(pDto.data.roomPath));
                SetFindLabelText("Text_Type", "MTT");
                // SetFindLabelText("TimeCountTxt", LanguageManager.mInstance.GetLanguageForKey("RecordDetail103") + UIMineModel.mInstance.ShowGameCountTime(pDto.data.playTime * 1000));// "牌局时长
                SetFindLabelText("PlayTimeCountTxt", LanguageManager.mInstance.GetLanguageForKey("RecordDetail103") + UIMineModel.mInstance.ShowGameCountTime(pDto.data.playTime * 1000));// "牌局时长
                SetFindLabelText("TotalHandCountTxt", LanguageManager.mInstance.GetLanguageForKey("UIMine_RecordDetailForNormal_CqIHdrgI") +":" + pDto.data.totalHand);
                SetFindLabelText("PariticipantsTxt", LanguageManager.mInstance.GetLanguageForKey("UIMine_RecordDetailForMatchPariticipants") + ":" + pDto.data.pariticipants);
                SetFindLabelText("CreatorTxt", LanguageManager.mInstance.GetLanguageForKey("adaptation10219").Replace("##" ,pDto.data.creator));
                SetFindLabelText("TotalBonusTxt", LanguageManager.mInstance.GetLanguageForKey("UIMine_RecordDetailForMatchTotalBonus") + ":" + StringHelper.ShowGold(pDto.data.totalBonus));
                SetFindLabelText("MyBonusTxt", LanguageManager.mInstance.GetLanguageForKey("UIMine_RecordDetailForMatchMyBonus") + ":" + StringHelper.ShowGold((int)pDto.data.myBonus));
                this.mGameId = data.gameID;

                var dic = new Dictionary<int, WEB2_record_mtt_detail.HonorsElement>() {
                        { 1,new WEB2_record_mtt_detail.HonorsElement()},
                        { 2,new WEB2_record_mtt_detail.HonorsElement()},
                        { 3,new WEB2_record_mtt_detail.HonorsElement()}
                    };
                for (int i = 0; i < pDto.data.honors.Count; i++)
                {
                    dic[pDto.data.honors[i].rank] = pDto.data.honors[i];
                }
                pDto.data.honors = dic.Values.ToList();


                for (int i = 0; i < pDto.data.honors.Count; i++)
                {
                    if (pDto.data.honors[i] != null && !string.IsNullOrEmpty(pDto.data.honors[i].nickName))
                    {
                        rc.Get<GameObject>($"Head{i + 1}").SetActive(true);
                        SetFindLabelText(("NameTxt_H" + (i + 1).ToString()), pDto.data.honors[i].nickName);
                        SetFindLabelText(("ScoreTxt_H" + (i + 1).ToString()), GetHonorTopScore(pDto.data.honors[i], pDto.data.allUsersRecords));
                        WebImageHelper.SetHeadImage(rc.Get<GameObject>("HeadImg_H" + (i + 1).ToString()).GetComponent<RawImage>(), pDto.data.honors[i].head);
                    }
                    else
                    {
                        rc.Get<GameObject>($"Head{i + 1}").SetActive(false);
                    }
                }
                mClubDatas = pDto.data.clubGameRecord;
                mUserRecords = pDto.data.allUsersRecords;
                SetClubTransData(pDto.data.clubGameRecord);
                SetMemberTransData(pDto.data.allUsersRecords);
            }
        }

        List<WEB2_record_mtt_detail.ClubGameRecordElement> mClubDatas;
        List<WEB2_record_mtt_detail.AllUsersRecords> mUserRecords;

        private void ClickClubItem(GameObject go, int i)
        {
            if (mClubDatas == null || mClubDatas.Count <= 0 || mUserRecords == null || mUserRecords.Count <= 0) return;
            var tClubUserIds = mClubDatas[i].userGameRecords;
            if (tClubUserIds == null || tClubUserIds.Count == 0) return;
            var tGetsUser = new List<WEB2_record_mtt_detail.AllUsersRecords>();
            for (int j = 0; j < tClubUserIds.Count; j++)
            {
                for (int x = 0; x < mUserRecords.Count; x++)
                {
                    if (tClubUserIds[j].userId == mUserRecords[x].userId && tGetsUser.Contains(mUserRecords[x]) == false)
                    {
                        tGetsUser.Add(mUserRecords[x]);
                    }
                }
            }
            UIComponent.Instance.ShowNoAnimation(UIType.UIMine_RecordItemsMatch, tGetsUser);
        }

        string GetHonorTopScore(WEB2_record_mtt_detail.HonorsElement pUser, List<WEB2_record_mtt_detail.AllUsersRecords> pUserRecords)
        {
            for (int i = 0; i < pUserRecords.Count; i++)
            {
                if (pUser.userId == pUserRecords[i].userId)
                    return StringHelper.ShowGold((int)pUserRecords[i].bonus);
            }
            return "";
        }

        private void SetClubTransData(List<WEB2_record_mtt_detail.ClubGameRecordElement> pClubDatas)
        {
            var father = rc.Get<GameObject>("Club").GetComponent<RectTransform>();
            if (pClubDatas == null || pClubDatas.Count == 0)
            {
                father.gameObject.SetActive(false);
                Log.Error("没有俱乐部");
                return;
            }
            var tClubItem = rc.Get<GameObject>("ClubItem");
            mLocalPosY = mLocalPosY + 96;
            GameObject obj;
            for (int i = 0; i < pClubDatas.Count; i++)
            {
                obj = GameObject.Instantiate(tClubItem, father);
                obj.SetActive(true);
                obj.transform.Find("ClubNameTxt").GetComponent<Text>().text = pClubDatas[i].clubName;
                obj.transform.Find("ClubScoreTxt").GetComponent<Text>().text = StringHelper.ShowGold((int)pClubDatas[i].totalPl);
                obj.GetComponent<RectTransform>().localPosition = new Vector3(0, -96 - i * 135, 0);
                UIEventListener.Get(obj, i).onIntClick = ClickClubItem;
                mLocalPosY += 135;
            }
            rc.Get<GameObject>("Club").GetComponent<LayoutElement>().minHeight = 96 + 135 * pClubDatas.Count;
            tClubItem.SetActive(false);
        }

        private void SetMemberTransData(List<WEB2_record_mtt_detail.AllUsersRecords> tData)
        {
            var tClubItem = rc.Get<GameObject>("MemberItem");
            if (tData == null || tData.Count == 0)
            {
                tClubItem.gameObject.SetActive(false);
                Log.Debug("没有成员");
                return;
            }
            var father = rc.Get<GameObject>("Member").GetComponent<RectTransform>();
            father.localPosition = new Vector3(0, -mLocalPosY, 0);
            mLocalPosY = mLocalPosY + 99;
            GameObject obj;
            for (int i = 0; i < tData.Count; i++)
            {
                obj = GameObject.Instantiate(tClubItem, father);
                obj.SetActive(true);

                var bg1 = obj.transform.GetComponent<Image>();
                if (tData[i].userId == GameCache.Instance.nUserId)
                {
                    bg1.color = new Color(0f, 0f, 0f, 1f);
                }
                else
                {
                    bg1.color = new Color(0.1f, 0.1f, 0.1f, 0.5f);
                }

                obj.transform.Find("MemberNumTxt").GetComponent<Text>().text = (i + 1).ToString();
                var nameTxt = obj.transform.Find("MemberNameTxt").GetComponent<Text>();
                UIMatchModel.mInstance.SetLargeTextContent(nameTxt, tData[i].nickName);

                obj.transform.Find("MemberScoreTxt").GetComponent<Text>().text = StringHelper.GetSignedString((int)(tData[i].bonus));
                WebImageHelper.SetHeadImage(obj.transform.Find("Mask/MemberIcon").GetComponent<RawImage>(), tData[i].head);
                obj.GetComponent<RectTransform>().localPosition = new Vector3(0, -99 - i * 125, 0);
                mLocalPosY += 125;
            }
            tClubItem.SetActive(false);
            rc.Get<GameObject>("Content").GetComponent<RectTransform>().sizeDelta = new Vector2(0, mLocalPosY);
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
            rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
        }

        #endregion

        public void SetFindLabelText(string path, string content)
        {
            rc.Get<GameObject>(path).GetComponent<Text>().text = content;
        }
    }
}