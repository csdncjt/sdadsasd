using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using RecordDetailDto = ETHotfix.WEB2_record_detail.ResponseData;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_RecordDetailForNormalComponentSystem : AwakeSystem<UIMine_RecordDetailForNormalComponent>
    {
        public override void Awake(UIMine_RecordDetailForNormalComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 战绩详情
    /// </summary>
    public class UIMine_RecordDetailForNormalComponent : UIBaseComponent
    {
        public sealed class RecordDetailForNormalData
        {
            public string roomID;
            public RecordDetailDto pDto;
        }

        private string mRoomId;
        private ReferenceCollector rc;
        private Text textInsurancePool;
        private GameObject imageArrow;

        private bool bSeePaipu;

        int mLocalPosY = 1581;
        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            bSeePaipu = true;
            mLocalPosY = 1701;
            SetUpNav(UIType.UIMine_RecordDetailForNormal);
            if (obj != null && obj is RecordDetailForNormalData)
            {
                var data = obj as RecordDetailForNormalData;
                var pDto = data.pDto;
                if (pDto.data == null) return;

                SetFindLabelText("TitleNameTxt", pDto.data.roomName);
                string blindstr = "";
                if (pDto.data.roomPath == 21 || pDto.data.roomPath == 23)
                {
                    blindstr = StringHelper.ShowGold(int.Parse(pDto.data.blind));
                }
                else
                {
                    var blinds = pDto.data.blind.Split('/');
                    string str1 = StringHelper.ShowGold(int.Parse(blinds[0]));
                    string str2 = StringHelper.ShowGold(int.Parse(blinds[1]));
                    string str3 = StringHelper.ShowGold(pDto.data.qianzhu);
                    blindstr = StringHelper.ShowBlinds(str1, str2, str3);
                }

                SetFindLabelText("JibieTxt", blindstr);
                SetFindLabelText("PeopleTxt", pDto.data.userRecords.Count.ToString());
                SetFindLabelText("CountTxt", TimeHelper.ShowRemainingTimer(pDto.data.playTime * 1000, false));
                SetFindLabelText("LvTxt", LanguageManager.mInstance.GetLanguageForKey("RecordDetail101") + blindstr);// 盲注级别
                SetFindLabelText("LeaveTxt", LanguageManager.mInstance.GetLanguageForKey("RecordDetail102") + TimeHelper.GetDateTimer(pDto.data.endTime).ToString("MM/dd HH:mm"));  //"结束时间:" 
                SetFindLabelText("Text_Type", UIMineModel.mInstance.GetRoomPathRecordListStr(pDto.data.roomPath));
                SetFindLabelText("TimeCountTxt", LanguageManager.mInstance.GetLanguageForKey("RecordDetail103") + UIMineModel.mInstance.ShowGameCountTime(pDto.data.playTime * 1000));// "牌局时长
                SetFindLabelText("AllHandleTxt", pDto.data.totalHand.ToString());
                SetFindLabelText("AllOpenTxt", StringHelper.ShowGold(pDto.data.totalBring));

                var feePanle = rc.Get<GameObject>("Panel5").gameObject;

                feePanle.SetActive(pDto.data.roomChargeTotal > 0);

                SetFindLabelText("FeeTxt", StringHelper.ShowGold(pDto.data.roomChargeTotal));

                mRoomId = data.roomID;

                //判断牌谱时间是否超过24小时
                var nowtime = TimeHelper.Now();
                var ptime = (nowtime - pDto.data.endTime)/3600000;
                if (ptime > 24)
                {
                    bSeePaipu = false;
                }

                if (pDto.data.totalPl > 0)
                {
                    ScoreTxt1.text = "+" + StringHelper.ShowGold(pDto.data.totalPl);
                    ScoreTxt1.color = new Color32(217, 41, 41, 255);//红正
                }
                else
                {
                    ScoreTxt1.text = StringHelper.ShowGold(pDto.data.totalPl);
                    ScoreTxt1.color = new Color32(41, 217, 53, 255);//绿负
                }

                if (pDto.data.totalLose > 0)
                {
                    ScoreTxt2.text = "+" + StringHelper.ShowGold(pDto.data.totalLose);
                    ScoreTxt2.color = new Color32(217, 41, 41, 255);//红正
                }
                else
                {
                    ScoreTxt2.text = StringHelper.ShowGold(pDto.data.totalLose);
                    ScoreTxt2.color = new Color32(41, 217, 53, 255);//绿负
                }

                var dic = new Dictionary<int, WEB2_record_detail.HonorsElement>() {
                        { 1,new WEB2_record_detail.HonorsElement()},
                        { 2,new WEB2_record_detail.HonorsElement()},
                        { 3,new WEB2_record_detail.HonorsElement()}
                    };
                for (int i = 0; i < pDto.data.honors.Count; i++)
                {
                    dic[pDto.data.honors[i].labelType] = pDto.data.honors[i];
                }
                pDto.data.honors = dic.Values.ToList();


                for (int i = 0; i < pDto.data.honors.Count; i++)
                {
                    if (pDto.data.honors[i] != null && pDto.data.honors[i].user != null)
                    {
                        SetFindLabelText(("NameTxt_H" + (i + 1).ToString()), pDto.data.honors[i].user.nickName);
                        SetFindLabelText(("ScoreTxt_H" + (i + 1).ToString()), GetHonorTopScore(pDto.data.honors[i].user, pDto.data.userRecords));
                        WebImageHelper.SetHeadImage(rc.Get<GameObject>("HeadImg_H" + (i + 1).ToString()).GetComponent<RawImage>(), pDto.data.honors[i].user.head);
                    }
                }
                mClubDatas = pDto.data.clubGameRecord;
                mUserRecords = pDto.data.userRecords;
                SetClubTransData(pDto.data.clubGameRecord);
                SetJackPotData(pDto.data.jpRecords);
                SetMemberTransData(pDto.data.userRecords);

                imageArrow.SetActive(false);
                if (pDto.data.insuranceOp && pDto.data.insurances != null && pDto.data.insurances.Count > 0)
                {
                    UIEventListener.Get(rc.Get<GameObject>("InsurancePool")).onClick = go =>
                    {
                        UIComponent.Instance.ShowNoAnimation(UIType.UIMine_InsurancesItems, new UIMine_InsurancesItemsComponent.InsurancesData() { mInsuranceDtos = pDto.data.insurances, timeStr = LeaveTxt.text });
                    };
                    textInsurancePool.text = SetInsurancePool(pDto.data.insurancePool);
                    imageArrow.SetActive(true);
                }

            }
        }


        private string SetInsurancePool(int insurancePool)
        {
            var str = "";
            if (insurancePool > 0)
            {
                str = "+" + StringHelper.ShowGold(insurancePool);
            }
            else
            {
                str = "<color='#29D935FF'>" + StringHelper.ShowGold(insurancePool) + "</color>";
            }
            return str;
        }

        private void SetJackPotData(List<WEB2_record_detail.JpRecordsElement> pjpRes)
        {
            var father = rc.Get<GameObject>("JackPot").GetComponent<RectTransform>();
            if (pjpRes == null || pjpRes.Count == 0)
            {
                father.gameObject.SetActive(false);
                Log.Debug("JP-------********-----------");
                return;
            }
            var tJackPotItem = rc.Get<GameObject>("JackPotItem");
            mLocalPosY = mLocalPosY + 200;//130;
            GameObject obj;
            for (int i = 0; i < pjpRes.Count; i++)
            {
                obj = GameObject.Instantiate(tJackPotItem, father);
                obj.SetActive(true);
                obj.transform.Find("JPNameTxt").GetComponent<Text>().text = pjpRes[i].user == null ? "user=null" : pjpRes[i].user.nickName;//
                obj.transform.Find("JPCardTxt").GetComponent<Text>().text = LanguageManager.mInstance.GetLanguageForKeyMoreValue("RecordDetail104", pjpRes[i].jpPokerType - 1); //pjpRes[i].jpPokerType == 1 ? "皇家同花" : pjpRes[i].jpPokerType == 2 ? "同花顺" : "四条";
                obj.transform.Find("JPScoreTxt").GetComponent<Text>().text = StringHelper.ShowGold(pjpRes[i].jpReward);
                obj.GetComponent<RectTransform>().localPosition = new Vector3(0, -200 - i * 140, 0);
                obj.GetComponent<RectTransform>().localScale = Vector3.one;
                mLocalPosY += 140;
            }
            rc.Get<GameObject>("JackPot").GetComponent<LayoutElement>().minHeight = 200+140 * pjpRes.Count;
            tJackPotItem.SetActive(false);
        }

        List<WEB2_record_detail.ClubGameRecordElement> mClubDatas;
        List<WEB2_record_detail.UserRecordsElement> mUserRecords;

        private void ClickClubItem(GameObject go, int i)
        {
            if (mClubDatas == null || mClubDatas.Count <= 0 || mUserRecords == null || mUserRecords.Count <= 0) return;
            var tClubUserIds = mClubDatas[i].userIds;
            if (tClubUserIds == null || tClubUserIds.Count == 0) return;
            var tGetsUser = new List<WEB2_record_detail.UserRecordsElement>();
            for (int j = 0; j < tClubUserIds.Count; j++)
            {
                for (int x = 0; x < mUserRecords.Count; x++)
                {
                    if (tClubUserIds[j] == mUserRecords[x].userId && tGetsUser.Contains(mUserRecords[x]) == false)
                    {
                        tGetsUser.Add(mUserRecords[x]);
                    }
                }
            }
            UIComponent.Instance.ShowNoAnimation(UIType.UIMine_RecordItemsNormal, tGetsUser);
        }

        string GetHonorTopScore(WEB2_record_detail.User pUser, List<WEB2_record_detail.UserRecordsElement> pUserRecords)
        {
            for (int i = 0; i < pUserRecords.Count; i++)
            {
                if (pUser.userId == pUserRecords[i].userId)
                    return StringHelper.ShowGold(pUserRecords[i].pl);
            }
            return "";
        }




        private void SetClubTransData(List<WEB2_record_detail.ClubGameRecordElement> pClubDatas)
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
                obj.transform.Find("ClubScoreTxt").GetComponent<Text>().text = StringHelper.ShowGold(pClubDatas[i].totalPl);
                obj.GetComponent<RectTransform>().localPosition = new Vector3(0, -96 - i * 135, 0);
                UIEventListener.Get(obj, i).onIntClick = ClickClubItem;
                mLocalPosY += 135;
            }
            rc.Get<GameObject>("Club").GetComponent<LayoutElement>().minHeight = 96 + 135 * pClubDatas.Count;
            tClubItem.SetActive(false);
        }



        private void SetMemberTransData(List<WEB2_record_detail.UserRecordsElement> tData)
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

                obj.transform.Find("MemberHandleTxt").GetComponent<Text>().text = tData[i].hand.ToString();
                obj.transform.Find("MemberComeTxt").GetComponent<Text>().text = StringHelper.ShowGold(tData[i].bring);
                obj.transform.Find("MemberScoreTxt").GetComponent<Text>().text = StringHelper.GetSignedString(tData[i].pl, true);
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
            base.Dispose();
        }
        Text LeaveTxt;
        #region InitUI
        protected virtual void InitUI()
        {
            rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            UIEventListener.Get(rc.Get<GameObject>("TopLookPai")).onClick = (go) =>
            {
                if (bSeePaipu)
                {
                    UIComponent.Instance.ShowNoAnimation(UIType.UIMine_Paipu, $"{mRoomId}");
                }
                else
                {
                    //提示时间过期
                    UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(20176));
                }
                
            };
            LeaveTxt = rc.Get<GameObject>("LeaveTxt").GetComponent<Text>();
            textInsurancePool = rc.Get<GameObject>("Text_InsurancePool").GetComponent<Text>();
            imageArrow = rc.Get<GameObject>("Image_Arrow");
            ScoreTxt1 = rc.Get<GameObject>("ScoreTxt1").GetComponent<Text>();
            ScoreTxt2 = rc.Get<GameObject>("ScoreTxt2").GetComponent<Text>();

        }
        private Text ScoreTxt1;
        private Text ScoreTxt2;
        #endregion
        public void SetFindLabelText(string path, string content)
        {
            rc.Get<GameObject>(path).GetComponent<Text>().text = content;
        }
    }
}