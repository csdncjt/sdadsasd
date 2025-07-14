using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using DtoRoomInfo = ETHotfix.WEB2_room_mtt_view.Data;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMatch_MttDetailStateComponentSystem : AwakeSystem<UIMatch_MttDetailStateComponent>
    {
        public override void Awake(UIMatch_MttDetailStateComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名: </summary>
    public class UIMatch_MttDetailStateComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Text textName;
        private Image imageStatus;
        private Text textStatus;
        private Text textFee;
        private Text textUpBlindTime;
        private Text textRebuyLevel;
        private Text textDelayJoin;
        private Text textHunterScale;
        private Text textJoinCount;
        private Text textDeskPlayerCount;

        private Transform panelMoreInfo;
        private Text textUpBlindLeftTime;
        private Text textAverageScore;
        private Text textPlayerCount;
        private Text textCurBlind;
        private Text textNextLevelTime;
        private Text textGamePlayTime;
        private Text textNextBlind;

        private Transform panelGameStartTime;
        private Text textCurJoinCount;
        private Text textBeginTime;


        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            textName = rc.Get<GameObject>("Text_Name").GetComponent<Text>();
            imageStatus = rc.Get<GameObject>("Image_Status").GetComponent<Image>();
            textStatus = rc.Get<GameObject>("Text_Status").GetComponent<Text>();
            textFee = rc.Get<GameObject>("Text_Fee").GetComponent<Text>();
            textUpBlindTime = rc.Get<GameObject>("Text_UpBlindTime").GetComponent<Text>();
            textRebuyLevel = rc.Get<GameObject>("Text_RebuyLevel").GetComponent<Text>();
            textDelayJoin = rc.Get<GameObject>("Text_DelayJoin").GetComponent<Text>();
            textHunterScale = rc.Get<GameObject>("Text_HunterScale").GetComponent<Text>();
            textJoinCount = rc.Get<GameObject>("Text_JoinCount").GetComponent<Text>();
            textDeskPlayerCount = rc.Get<GameObject>("Text_DeskPlayerCount").GetComponent<Text>();

            panelMoreInfo = rc.Get<GameObject>("Panel_MoreInfo").GetComponent<Transform>();
            textUpBlindLeftTime = rc.Get<GameObject>("Text_UpBlindLeftTime").GetComponent<Text>();
            textAverageScore = rc.Get<GameObject>("Text_AverageScore").GetComponent<Text>();
            textPlayerCount = rc.Get<GameObject>("Text_PlayerCount").GetComponent<Text>();
            textCurBlind = rc.Get<GameObject>("Text_CurBlind").GetComponent<Text>();
            textNextLevelTime = rc.Get<GameObject>("Text_NextLevelTime").GetComponent<Text>();
            textGamePlayTime = rc.Get<GameObject>("Text_GamePlayTime").GetComponent<Text>();
            textNextBlind = rc.Get<GameObject>("Text_NextBlind").GetComponent<Text>();

            panelGameStartTime = rc.Get<GameObject>("Panel_GameStartTime").GetComponent<Transform>();
            textCurJoinCount = rc.Get<GameObject>("Text_CurJoinCount").GetComponent<Text>();
            textBeginTime = rc.Get<GameObject>("Text_BeginTime").GetComponent<Text>();

        }

        public void UpdateInfo(DtoRoomInfo roomInfo)
        {
           

            if (roomInfo.gameStatus == 0 || roomInfo.gameStatus == 1)
            {
                textStatus.text = LanguageManager.Get("MTT-Applying");
                imageStatus.sprite = rc.Get<Sprite>("images_dating_mtt_baomingzhong01");
            }
            else
            {
                textStatus.text = LanguageManager.Get("MTT-Processing");
                imageStatus.sprite = rc.Get<Sprite>("images_dating_mtt_jinxinzhong01");
            }
            textName.text = roomInfo.name;

            textFee.text = LanguageManager.Get("MTT_State_ApplyFee") + $"{StringHelper.ShowGold(roomInfo.registerFee)}+{StringHelper.ShowGold(roomInfo.serviceFee)}";
            textUpBlindTime.text = string.Format(LanguageManager.Get("MTT_State_UpBlindTime"), roomInfo.updateCycle);
            if (roomInfo.canRebuy && roomInfo.rebuyTimes > 0)
            {
                textRebuyLevel.text = LanguageManager.Get("MTT_State_RebuyLevel") + string.Format(LanguageManager.Get("MTT_State_RebuyDetail"), roomInfo.maxRebuyLevel, roomInfo.rebuyTimes);
            }
            else
            {
                textRebuyLevel.text = LanguageManager.Get("MTT_State_RebuyLevel") + LanguageManager.Get("MTT_State_CannotRebuy");
            }
            if (roomInfo.maxDelayLevel > 1)
            {
                textDelayJoin.text = LanguageManager.Get("MTT_State_DelayApply") + string.Format(LanguageManager.Get("MTT_State_DelayDetail"), roomInfo.maxDelayLevel);
            }
            else
            {
                textDelayJoin.text = LanguageManager.Get("MTT_State_DelayApply") + LanguageManager.Get("MTT_State_CannotDelay");
            }
            textHunterScale.gameObject.SetActive(false);
            textJoinCount.text = LanguageManager.Get("MTT_State_ApplyCount") + $"{roomInfo.participants}/{roomInfo.upperLimit}";
            textDeskPlayerCount.text = string.Format(LanguageManager.Get("MTT_State_DeskPlayerCount"), roomInfo.mttType);
            
            if (roomInfo.isStart)
            {
                panelGameStartTime.gameObject.SetActive(false);
                panelMoreInfo.gameObject.SetActive(true);
            }
            else
            {
                panelGameStartTime.gameObject.SetActive(true);
                panelMoreInfo.gameObject.SetActive(false);
            }

            textUpBlindLeftTime.text = string.Format(LanguageManager.Get("UITexasReport_Text_MatchZmsysj"), Mathf.Ceil(roomInfo.leftBlindTime / 60.0f));
            textAverageScore.text = StringHelper.GetShortString((int)roomInfo.averageScore);
            textPlayerCount.text = $"{roomInfo.leftPlayer}/{roomInfo.participants}";
            int blindLevel = roomInfo.currBlindLevel - 1;//从1开始，转一下
            if (blindLevel >= MTTGameUtil.numOfLevel((MTT_GameType)roomInfo.blindType))
            {
                blindLevel = MTTGameUtil.numOfLevel((MTT_GameType)roomInfo.blindType) - 1;
            }
            int curBld = MTTGameUtil.BlindAtLevel(blindLevel, (MTT_GameType)roomInfo.blindType, (float)roomInfo.blindScale);
            int curAnte = MTTGameUtil.AnteAtLevel(blindLevel, (MTT_GameType)roomInfo.blindType, (float)roomInfo.blindScale);
            textCurBlind.text = $"{StringHelper.GetShortString(curBld)}/{StringHelper.GetShortString(curBld * 2)}({StringHelper.GetShortString(curAnte)})";

            int nextBlindLevel = roomInfo.currBlindLevel;//从1开始，转一下
            if (nextBlindLevel >= MTTGameUtil.numOfLevel((MTT_GameType)roomInfo.blindType))
            {
                nextBlindLevel = MTTGameUtil.numOfLevel((MTT_GameType)roomInfo.blindType) - 1;
            }
            int nextBld = MTTGameUtil.BlindAtLevel(nextBlindLevel, (MTT_GameType)roomInfo.blindType, (float)roomInfo.blindScale);
            int nextAnte = MTTGameUtil.AnteAtLevel(nextBlindLevel, (MTT_GameType)roomInfo.blindType, (float)roomInfo.blindScale);
            textNextBlind.text = $"{StringHelper.GetShortString(nextBld)}/{StringHelper.GetShortString(nextBld * 2)}({StringHelper.GetShortString(nextAnte)})";

            if (roomInfo.runningTime > 0)
            {
                textGamePlayTime.text = TimeHelper.ShowRemainingTimer(roomInfo.runningTime * 1000, false);
            }
            else
            {
                textGamePlayTime.text = LanguageManager.Get("MTT_State_NotStart");
            }

            textNextLevelTime.text = string.Format(LanguageManager.Get("UITexasReport_Text_MatchZmsysj"), roomInfo.updateCycle);

            textCurJoinCount.text = string.Format(LanguageManager.Get("MTT_State_ApplyNumBottom"), roomInfo.participants);
            textBeginTime.text = TimeHelper.TimerDateStr((long)roomInfo.startTime * 1000) + "  " + TimeHelper.TimerDateMinStr((long)roomInfo.startTime * 1000);
        }

        public override void OnShow(object obj)
        {

        }

        public override void OnHide()
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }
      
    }
}
