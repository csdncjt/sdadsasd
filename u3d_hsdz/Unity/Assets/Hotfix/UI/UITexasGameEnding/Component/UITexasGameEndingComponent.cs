using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UiTexasGameEndingComponentAwakeSystem: AwakeSystem<UITexasGameEndingComponent>
    {
        public override void Awake(UITexasGameEndingComponent self)
        {
            self.Awake();
        }
    }

    public class UITexasGameEndingComponent: UIBaseComponent
    {
        public sealed class GameEndingData
        {
            public REQ_GAME_ENDING rec;
            public string title;
        }

        private sealed class PlayerInfo
        {
            public int userID;
            public string name;
            public int bring;
            public int pl;
            public int hand;
            public int vp;
            public string head;
            public bool vpOn;
        }

        private sealed class JacpotInfo
        {
            public int userID;
            public string name;
            public int cardType;
            public int jackPottReward;
        }

        private sealed class PlayerItem
        {
            public Transform trans;
            private ReferenceCollector rc;
            private Text text1;
            private Text text2;
            private GameObject panel3;
            private Text text3;
            private Text text4;

            public PlayerItem(Transform transform)
            {
                trans = transform;
                rc = trans.GetComponent<ReferenceCollector>();
                text1 = rc.Get<GameObject>("Text_1").GetComponent<Text>();
                text2 = rc.Get<GameObject>("Text_2").GetComponent<Text>();
                panel3 = rc.Get<GameObject>("Panel_3");
                text3 = rc.Get<GameObject>("Text_3").GetComponent<Text>();
                text4 = rc.Get<GameObject>("Text_4").GetComponent<Text>();

            }

            public void UpdatePlayerItem(PlayerInfo playerInfo)
            {
                text1.text = playerInfo.name;
                text2.text = StringHelper.ShowGold(playerInfo.bring);
                panel3.SetActive(playerInfo.vpOn);
                text3.text = $"{playerInfo.vp}%";
                text4.text = StringHelper.GetSignedString(playerInfo.pl);
                if (playerInfo.pl > 0)
                {
                    text4.color = new Color32(184, 43, 48, 255);
                }
                else if (playerInfo.pl < 0)
                {
                    text4.color = new Color32(75, 159, 93, 255);
                }
                else
                {
                    text4.color = new Color32(143, 143, 143, 255);
                }

                if (playerInfo.userID == GameCache.Instance.nUserId)
                {
                    text1.color = new Color32(233, 191, 128, 255);
                    text2.color = new Color32(233, 191, 128, 255);
                    text3.color = new Color32(233, 191, 128, 255);
                }
                else
                {
                    text1.color = new Color32(143, 143, 143, 255);
                    text2.color = new Color32(143, 143, 143, 255);
                    text3.color = new Color32(143, 143, 143, 255);
                }
            }

            public void UpdateJPItem(JacpotInfo jacpotInfo)
            {
                text1.text = jacpotInfo.name;
                text2.text = CardTypeUtil.GetCardTypeName(jacpotInfo.cardType);
                panel3.SetActive(false);
                text4.text = $"{StringHelper.ShowGold(jacpotInfo.jackPottReward)}";
            }
        }

        private REQ_GAME_ENDING rec;
        private int profitAmount; //水上数据
        private int lossAmount; //水下数据

        private List<PlayerInfo> players;
        private List<JacpotInfo> jacpots;

        private PlayerInfo myInfo;
        private PlayerInfo mvpInfo;
        private PlayerInfo bigFishInfo;
        private PlayerInfo workerInfo;

        private ReferenceCollector rc;
        private Text textTitle;
        private Button buttonClose;
        private Text textRoomTime;
        private RawImage imagehead;
        private Text textmyReward;
        private Text texttotalHand;
        private RawImage imageheadMVP;
        private Text textMVPName;
        private Text textMVPValue;
        private RawImage imageheadLeft;
        private Text textLeftName;
        private Text textLeftValue;
        private RawImage imageheadRight;
        private Text textRightName;
        private Text textRightValue;
        private Transform transVP;
        private Transform scrollContent;
        private Transform transUserInfo;
        private Transform transJPTitle;
        private Transform transInsurance;
        private Text textInsurance;
        private Text textAllBuyIn;
        private Text textBigestPot;
        private Text textWaterUp;
        private Text textWaterDown;
        private Text textFee;

        private GameObject Panle_Fee;




        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            textTitle = rc.Get<GameObject>("Text_Title").GetComponent<Text>();
            buttonClose = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            textRoomTime = rc.Get<GameObject>("Text_RoomTime").GetComponent<Text>();
            imagehead = rc.Get<GameObject>("Image_head").GetComponent<RawImage>();
            textmyReward = rc.Get<GameObject>("Text_myReward").GetComponent<Text>();
            texttotalHand = rc.Get<GameObject>("Text_totalHand").GetComponent<Text>();
            imageheadMVP = rc.Get<GameObject>("Image_headMVP").GetComponent<RawImage>();
            textMVPName = rc.Get<GameObject>("Text_MVPName").GetComponent<Text>();
            textMVPValue = rc.Get<GameObject>("Text_MVPValue").GetComponent<Text>();
            imageheadLeft = rc.Get<GameObject>("Image_headLeft").GetComponent<RawImage>();
            textLeftName = rc.Get<GameObject>("Text_LeftName").GetComponent<Text>();
            textLeftValue = rc.Get<GameObject>("Text_LeftValue").GetComponent<Text>();
            imageheadRight = rc.Get<GameObject>("Image_headRight").GetComponent<RawImage>();
            textRightName = rc.Get<GameObject>("Text_RightName").GetComponent<Text>();
            textRightValue = rc.Get<GameObject>("Text_RightValue").GetComponent<Text>();
            transVP = rc.Get<GameObject>("Title_VP").transform;
            scrollContent = rc.Get<GameObject>("Content").transform;
            transUserInfo = rc.Get<GameObject>("Cell_UserInfo").transform;
            transJPTitle = rc.Get<GameObject>("Section_JP_Title").transform;
            transInsurance = rc.Get<GameObject>("View_Insurance").transform;
            textInsurance = rc.Get<GameObject>("Text_Insurance").GetComponent<Text>();
            textAllBuyIn = rc.Get<GameObject>("Text_AllBuyIn").GetComponent<Text>();
            textBigestPot = rc.Get<GameObject>("Text_BigestPot").GetComponent<Text>();
            textWaterUp = rc.Get<GameObject>("Text_WaterUp").GetComponent<Text>();
            textWaterDown = rc.Get<GameObject>("Text_WaterDown").GetComponent<Text>();
            textFee = rc.Get<GameObject>("Text_Fee").GetComponent<Text>();
            Panle_Fee = rc.Get<GameObject>("Panle_Fee").gameObject;

            UIEventListener.Get(this.buttonClose.gameObject).onClick = onClickClose;
        }

        private void onClickClose(GameObject go)
        {
            UI mUILobbyMenu = UIComponent.Instance.ShowNoAnimation(UIType.UILobby_Menu);
            mUILobbyMenu.GetComponent<UILobby_MenuComponent>().ChangeView(UILobby_MenuComponent.ePageType.MATCH);
            UIComponent.Instance.Remove(UIType.UITexasGameEnding);
        }

        public override void OnShow(object obj)
        {
            if (null == obj)
                return;

            GameEndingData data = obj as GameEndingData;
            if (null == data)
                return;

            textTitle.text = data.title;

            rec = data.rec;

            UpdatePlayer();
            UpdateJackPot();

            UpdateUI();

        }

        private void UpdatePlayer()
        {
            players = new List<PlayerInfo>();

            profitAmount = 0;
            lossAmount = 0;

            string[] topNicks = rec.nickNameStr.Split(new string[] { "@%" }, StringSplitOptions.None);
            string[] topHead = rec.userPortrait.Split(new string[] { "@%" }, StringSplitOptions.None);

            string[] mNames = rec.userNickname.Split(new string[] { "@%" }, StringSplitOptions.None);

            workerInfo = new PlayerInfo();
            if (topNicks.Length > 0)
            {
                workerInfo.name = topNicks[0];
                workerInfo.head = topHead[0];
            }
            workerInfo.pl = 0;

            mvpInfo = new PlayerInfo();
            if (topNicks.Length > 1)
            {
                mvpInfo.name = topNicks[1];
                mvpInfo.head = topHead[1];
            }
            mvpInfo.pl = 0;

            bigFishInfo = new PlayerInfo();
            if (topNicks.Length > 2)
            {
                bigFishInfo.name = topNicks[2];
                bigFishInfo.head = topHead[2];
            }
            bigFishInfo.pl = 0;

            myInfo = null;

            for (int i = 0; i < rec.userIdArray.Count; i++)
            {
                PlayerInfo playerInfo = new PlayerInfo();
                playerInfo.userID = rec.userIdArray[i];
                playerInfo.name = i < mNames.Length? mNames[i] : "";
                playerInfo.bring = i < rec.bringArray.Count? rec.bringArray[i] : 0;
                playerInfo.pl = i < rec.plArray.Count? rec.plArray[i] : 0;
                playerInfo.hand = i < rec.handArray.Count? rec.handArray[i] : 0;
                playerInfo.vp = i < rec.vpArray.Count? rec.vpArray[i] : 0;
                playerInfo.vpOn = rec.vpOn == 1;

                if (playerInfo.pl > 0)
                {
                    profitAmount += playerInfo.pl;
                }

                if (playerInfo.pl < 0)
                {
                    lossAmount += playerInfo.pl;
                }

                if (playerInfo.name == topNicks[0])
                {
                    playerInfo.head = topHead[0];
                    workerInfo = playerInfo;
                }

                if (playerInfo.name == topNicks[1])
                {
                    playerInfo.head = topHead[1];
                    mvpInfo = playerInfo;
                }

                if (playerInfo.name == topNicks[2])
                {
                    playerInfo.head = topHead[2];
                    bigFishInfo = playerInfo;
                }

                if (playerInfo.userID == GameCache.Instance.nUserId)
                {
                    myInfo = playerInfo;
                }

                players.Add(playerInfo);
            }

        }

        private void UpdateJackPot()
        {
            jacpots = new List<JacpotInfo>();

            string[] jpNames = rec.jackPotUserNickname.Split(new string[] { "@%" }, StringSplitOptions.None);
            for (int i = 0; i < rec.jackPotUserIdArray.Count; i++)
            {
                JacpotInfo jpInfo = new JacpotInfo();
                jpInfo.userID = rec.jackPotUserIdArray[i];
                jpInfo.name = i < jpNames.Length? jpNames[i] : "";
                jpInfo.cardType = i < rec.jackPotCardTypeArray.Count? rec.jackPotCardTypeArray[i] : 0;
                jpInfo.jackPottReward = i < rec.jackPotRewardArray.Count? rec.jackPotRewardArray[i] : 0;
                jacpots.Add(jpInfo);
            }
        }

        private void UpdateUI()
        {
            //顶部信息
            textmyReward.text = StringHelper.GetShortSignedString(myInfo != null? myInfo.pl : 0);
            texttotalHand.text = $"{(myInfo != null? myInfo.hand : 0)}";
            WebImageHelper.SetHeadImage(imagehead, GameCache.Instance.headPic);

            string hourStr = rec.playTime / 3600 >= 10 ? $"{rec.playTime / 3600}" : $"0{rec.playTime / 3600}";
            string minStr = (rec.playTime % 3600) / 60 >= 10 ? $"{(rec.playTime % 3600) / 60}" : $"0{(rec.playTime % 3600) / 60}";
            string secondStr = rec.playTime % 60 >= 10 ? $"{rec.playTime % 60}" : $"0{rec.playTime % 60}";
            textRoomTime.text =CPErrorCode.LanguageDescription(20065) + $"{hourStr}:{minStr}:{secondStr}";

            //称号信息
            textMVPName.text = mvpInfo.name;
            textMVPValue.text = StringHelper.GetSignedString(mvpInfo.pl);
            WebImageHelper.SetHeadImage(imageheadMVP, mvpInfo.head);

            textLeftName.text = bigFishInfo.name;
            textLeftValue.text = StringHelper.GetSignedString(bigFishInfo.pl);
            WebImageHelper.SetHeadImage(imageheadLeft, bigFishInfo.head);

            textRightName.text = workerInfo.name;
            textRightValue.text = StringHelper.GetSignedString(workerInfo.pl);
            WebImageHelper.SetHeadImage(imageheadRight, workerInfo.head);

            float contentHeight = 0;
            //战绩数据
            if (rec.vpOn != 1)
            {
                transVP.gameObject.SetActive(false);
            }

            foreach (PlayerInfo playerInfo in players)
            {
                GameObject playerObject = GetPlayerItem(scrollContent);
                PlayerItem playerItem = new PlayerItem(playerObject.transform);
                playerItem.UpdatePlayerItem(playerInfo);
                contentHeight += 134;
            }

            //JackPot数据
            if (jacpots.Count > 0)
            {
                GetJPTitle(scrollContent);
                contentHeight += 108;
                foreach (JacpotInfo jpInfo in jacpots)
                {
                    GameObject jpObject = GetPlayerItem(scrollContent);
                    PlayerItem playerItem = new PlayerItem(jpObject.transform);
                    playerItem.UpdateJPItem(jpInfo);
                    contentHeight += 134;
                }
            }

            scrollContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, contentHeight);

            //保险数据
            textInsurance.text = StringHelper.GetSignedString(rec.insuranceSum);

            //底部数据
            textAllBuyIn.text = StringHelper.GetShortString(rec.allBring);
            textBigestPot.text = StringHelper.GetShortString(rec.maxPot);
            textWaterUp.text = StringHelper.GetShortSignedString(profitAmount);
            textWaterDown.text = StringHelper.GetShortSignedString(lossAmount);

            Panle_Fee.SetActive(rec.fee > 0);
            if (rec.fee > 0)
            {
                textFee.text = StringHelper.GetShortSignedString(rec.fee);
            }
           
        }

        GameObject GetPlayerItem(Transform parent)
        {
            GameObject obj = GameObject.Instantiate(transUserInfo.gameObject);
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(parent);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            return obj;
        }

        GameObject GetJPTitle(Transform parent)
        {
            GameObject obj = GameObject.Instantiate(transJPTitle.gameObject);
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(parent);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            return obj;
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


    }
}