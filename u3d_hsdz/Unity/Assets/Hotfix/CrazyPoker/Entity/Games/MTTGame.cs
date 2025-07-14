using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using DragonBones;

namespace ETHotfix
{
    [ObjectSystem]
    public class MTTGameAwakeSystem : AwakeSystem<MTTGame, Component>
    {
        public override void Awake(MTTGame self, Component component)
        {
            self.Awake(component);
        }
    }

    [ObjectSystem]
    public class MTTGameUpdateSystem : UpdateSystem<MTTGame>
    {
        public override void Update(MTTGame self)
        {
            self.Update();
        }
    }

    public partial class MTTGame : TexasGame
    {
        protected Button buttonRebuy;
        protected Transform transCountDownView;
        protected Image imageRedistributionTips;
        protected UnityArmatureComponent armatureRewardCircleZH;
        protected UnityArmatureComponent armatureRewardCircleEN;

        private int matchCost; // 参赛的费用
        private bool gameStarted; // 比赛是否已经开始
        private bool hadRequestEnterRoom; //是否已请求进入房间接口
        private bool huntMode; //是否猎人模式

        //盲注数据
        private MTT_GameType blindType; // 盲注表0为A表(普通)，1为B表(快速) 2为C表(普通25) 3为D表(快速25)

        public MTT_GameType BlindType
        {
            get
            {
                return this.blindType;
            }
        }
        private int upBlindTime; // 当前升盲时间
        public int upBlindLeftTime; //升盲剩余时间，秒
        private float upBlindLeftTimeDeltaTime;
        private int blindLevel; // 盲注级别
        private float blindScale;  //盲注倍数
        private int displayedSmallBlind;//桌面显示的盲注(区分smallBlind)
        // 升盲倒计时
        private bool upBldCounting;

        // 重购数据
        private string rebuyScore;
        private int leftTime;
        private int rebuyCount;
        private string rebuyCost;
        private int rebuyLevel;
        private bool inRewardCircle;//是否已进入奖励圈（+1）

        public override void Update()
        {
            base.Update();

            if (upBldCounting)
            {
                if (Time.time - upBlindLeftTimeDeltaTime > 1f)
                {
                    upBlindLeftTimeDeltaTime = Time.time;
                    upBlindLeftTime -= 1;
                    if (upBlindLeftTime <= 0)
                    {
                        upBldCounting = false;
                    }
                    UpdateRoomDes();
                }
            }
        }

        public override void Dispose()
        {
            blindLevel = 0;
            upBlindLeftTime = 0;
            upBldCounting = false;
            blindScale = 0;
            displayedSmallBlind = 0;
            huntMode = false;
            base.Dispose();
        }

        protected override void InitUI(Component comp)
        {
            base.InitUI(comp);
            buttonRebuy = rc.Get<GameObject>("Button_Rebuy").GetComponent<Button>();
            imageRedistributionTips = rc.Get<GameObject>("Image_RedistributionTips").GetComponent<Image>();
            armatureRewardCircleZH = rc.Get<GameObject>("Armature_RewardCircle_zh").GetComponent<UnityArmatureComponent>();
            armatureRewardCircleEN = rc.Get<GameObject>("Armature_RewardCircle_en").GetComponent<UnityArmatureComponent>();
            UIEventListener.Get(buttonRebuy.gameObject).onClick = onClickRebuy;
        }

        public override void UpdateRoom(object obj)
        {
            REQ_GAME_MTT_ENTER_ROOM rec = obj as REQ_GAME_MTT_ENTER_ROOM;
            if (null == rec)
                return;

            Log.Msg(rec.baseMsg);
            huntMode = rec.huntMode == 1;
            GameCache.Instance.mtt_deskId = rec.deskId;
            //倒计时
            if (rec.resideMinTime <= -1)
            {
                // 游戏已开始
                gameStarted = true;
                // 隐藏倒计时界面
                UIComponent.Instance.HideNoAnimation(UIType.UIMTTTime);
            }

            if (rec.rebuyLeftTime != 0)
            {
                //重购申请倒计时，官方赛暂无
            }

            blindType = (MTT_GameType)rec.blindType;
            upBlindTime = rec.raiseBlindTime;
            matchCost = rec.matchCost;

            blindScale = rec.blindScale / 10.0f;
            smallBlind = rec.baseMsg.smallBlind;
            blindLevel = MTTGameUtil.LeveForBlind(rec.baseMsg.smallBlind, blindType, blindScale);
            displayedSmallBlind = smallBlind;

            upBlindLeftTime = rec.mttUpBlindLeftTime;
            upBldCounting = rec.baseMsg.gamestatus == 1;

            //重购
            rebuyScore = StringHelper.ShowGold(rec.rebuyScore);
            rebuyCount = rec.rebuyResideCount;
            rebuyCost = rec.rebuyCost;
            rebuyLevel = rec.rebuyLevel;
            rec.inRewardCircle = rec.inRewardCircle;

            base.UpdateRoomCommon(rec.baseMsg, obj);
            HideWaitForStartTips();
        }

        public void ObtainMTTCountDown()
        {
            // MTT先请求比赛倒计时，开赛才请求进房协议
            hadRequestEnterRoom = false;
            HttpRequestComponent.Instance.Send(WEB2_room_mtt_countdown.API,
            WEB2_room_mtt_countdown.Request(new WEB2_room_mtt_countdown.RequestData()
            {
                rpath = (int)RoomPath.MTT,
                rmid = GameCache.Instance.room_id
            }), json =>
            {
                var tDto = WEB_mtt_countdown.Response(json);
                if (tDto.status == 0)
                {
                    int seconds = tDto.data / 1000;
                    if (seconds > 0)
                    {
                        UI mMatchLoading = UIComponent.Instance.Get(UIType.UIMatch_Loading);
                        if (null != mMatchLoading && null != mMatchLoading.GameObject && mMatchLoading.GameObject.activeInHierarchy)
                        {
                            UIComponent.Instance.HideNoAnimation(UIType.UIMatch_Loading);
                        }

                        if (listSeat == null || listSeat.Count == 0)
                        {
                            InitMttFakeSeat();
                        }

                        //比赛未开始，显示倒计时界面，倒计时30s时调进房协议
                        UIComponent.Instance.ShowNoAnimation(UIType.UIMTTTime, new UIMTTTimeComponent.MTTTimeData()
                        {
                            nickname = GameCache.Instance.roomName,
                            second = seconds
                        });
                        UpdateRoomDes();
                    }
                    else
                    {
                        //比赛已开始，调进房协议
                        texasCompoment.RequestEnterRoom();
                        hadRequestEnterRoom = true;
                    }
                }
                else
                {
                    UIComponent.Instance.Toast(tDto.status);
                    UI mMatchLoading = UIComponent.Instance.Get(UIType.UIMatch_Loading);
                    if (null != mMatchLoading && null != mMatchLoading.GameObject && mMatchLoading.GameObject.activeInHierarchy)
                    {
                        UIComponent.Instance.HideNoAnimation(UIType.UIMatch_Loading);
                    }
                    doExitRoom();
                }
            });
        }

        private void InitMttFakeSeat()
        {
            InitSeatByCount(GameCache.Instance.mtt_type);
            Seat mSeat = null;
            for (int i = 0, n = listSeat.Count; i < n; i++)
            {
                mSeat = listSeat[i];
                mSeat.seatID = (sbyte)i;

                mSeat.FsmLogicComponent.SM.ChangeState(SeatIdle<Entity>.Instance);

                if (i == 0)
                {
                    Player mPlayer = ComponentFactory.CreateWithId<Player>(GameCache.Instance.nUserId);
                    mPlayer.seatID = 0;
                    mPlayer.headPic = GameCache.Instance.headPic;
                    mPlayer.nick = GameCache.Instance.nick;
                    mPlayer.userID = GameCache.Instance.nUserId;
                    mPlayer.chips = 0;
                    mPlayer.canPlayStatus = 1;
                    mPlayer.status = 18;
                    mPlayer.ante = 0;
                    mPlayer.anteNumber = 0;
                    mPlayer.SetCards(GetEnptyHandCards());
                    mSeat.Player = mPlayer;

                    if (null != mainPlayer)
                    {
                        mainPlayer.Dispose();
                        mainPlayer = null;
                    }
                    mainPlayer = mSeat.Player;

                    mSeat.UpdateFSMbyStatus();
                } else
                {
                    mSeat.FsmLogicComponent.SM.ChangeState(SeatEmpty<Entity>.Instance);
                }
            }
        }

        public void countDownTo30Second()
        {
            if (!hadRequestEnterRoom)
            {
                texasCompoment.RequestEnterRoom();
                hadRequestEnterRoom = true;
            }
        }

        protected void ShowArmatureRewardCircle()
        {
            UnityArmatureComponent armatureRewardCircle;
            if (LanguageManager.mInstance.mCurLanguage == 0 || LanguageManager.mInstance.mCurLanguage == 2)
            {
                armatureRewardCircle = this.armatureRewardCircleZH;
            }
            else
            {
                armatureRewardCircle = this.armatureRewardCircleEN;
            }
            if (null != armatureRewardCircle.dragonAnimation)
            {
                armatureRewardCircle.dragonAnimation.Reset();
                armatureRewardCircle.dragonAnimation.Play("newAnimation", 1);
                armatureRewardCircle.AddEventListener(DragonBones.EventObject.COMPLETE, (key, go) =>
                {
                    armatureRewardCircle.gameObject.SetActive(false);
                });
            }
        }

        // 重购
        private void onClickRebuy(GameObject go)
        {
            if (isGPSRestrictions == 1)
            {
                //开启GPS，先获取定位
                waittingGPSCallback = true;
                NativeManager.GetGPSLocation();
                if (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.OSXEditor)
                {
                    return;
                }
            }
            waittingGPSCallback = false;
            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_MTT_GAME_REBUY_REQUEST()
            {
                operate = 1,
                longitude = GameCache.Instance.longitude,
                latitude = GameCache.Instance.latitude,
                uuid = SystemInfoUtil.getDeviceUniqueIdentifier(),
                roomPath = GameCache.Instance.room_path,
                roomID = GameCache.Instance.room_id,
            });
        }

        public override void GPSCallback_Sitdown()
        {
            //MTT的GPS回调用做重购
            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_MTT_GAME_REBUY_REQUEST()
            {
                operate = 1,
                longitude = GameCache.Instance.longitude,
                latitude = GameCache.Instance.latitude,
                uuid = SystemInfoUtil.getDeviceUniqueIdentifier(),
                roomPath = GameCache.Instance.room_path,
                roomID = GameCache.Instance.room_id,
            });
        }

        // 取消重购
        private void onClickRebuyCancel()
        {
            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_MTT_GAME_REBUY_CANCEL()
            {
                roomID = GameCache.Instance.room_id,
                roomPath = GameCache.Instance.room_path,
            });
        }

        // 退出房间
        protected override void onClickExit(GameObject go)
        {
            isExit = false;
            hideMenu();
            if (hadRequestEnterRoom)
            {
                CallbackExit();
            } else
            {
                doExitRoom();
            }
        }

        public override void doExitRoom()
        {
            base.doExitRoom();

            UI uI = UIComponent.Instance.Get(UIType.UIMatch);
            if (null != uI)
            {
                UIMatchComponent matchComponent = uI.UiBaseComponent as UIMatchComponent;
                matchComponent.ObtainMTTListData();
            }
        }

        // 操作延时
        protected override void onClickDelay(GameObject go)
        {
            Game.Scene.GetComponent<CPGameSessionComponent>().HotfixSession.Send(new REQ_MTT_ADD_TIME()
            {
                roomID = GameCache.Instance.room_id,
                roomPath = GameCache.Instance.room_path,
                time = 20,
                diamonds = 0,
                seatId = GameCache.Instance.CurGame.MainPlayer.seatID,
                coins = AddTimeCost()
            });
        }
        protected override int AddTimeCost()
        {
            if (delayCount == 0)
                return 0;

            return Convert.ToInt32(Math.Pow(2, delayCount));
        }

        public override void CheckPlayerInfo(int userId)
        {
            var web = new WEB2_user_public_info.RequestData()
            {
                randomId = "",
                userId = userId
            };
            HttpRequestComponent.Instance.Send(WEB2_user_public_info.API, WEB2_user_public_info.Request(web), json =>
            {
                var tDto = WEB2_user_public_info.Response(json);
                if (tDto.status == 0)
                {
                    UIComponent.Instance.ShowNoAnimation(UIType.UITexasInfo, new UITexasInfoComponent.InfoData()
                    {
                        canKickOut = 0,
                        mUserInfo = tDto.data,
                        mUserId = userId
                    });
                }
                else
                {
                    UIComponent.Instance.Toast(tDto.status);
                }
            });
        }

        // 实时战况
        protected override void onClickReport(GameObject go)
        {
            if (!hadRequestEnterRoom)
                return;
            UIComponent.Instance.ShowNoAnimation(UIType.UITexasReportMTT);
        }

        protected override void onClickCurSituation(GameObject go)
        {
            if (!hadRequestEnterRoom)
                return;
            base.onClickCurSituation(go);
        }

        public override bool ShouldAllowReconectRoom()
        {
            // 比赛已经开始才允许重连
            return hadRequestEnterRoom;
        }

        protected override void UpdateMenu()
        {
            //更新USDT
            textTotalBean.text = StringHelper.GetShortString(GameCache.Instance.gold);
            float menuHeight = 1365f;

            buttonOwner.gameObject.SetActive(false);
            menuHeight -= 129;

            buttonStandup.gameObject.SetActive(false);
            menuHeight -= 129;

            buttonAddChips.gameObject.SetActive(false);
            menuHeight -= 129;

            if (UserSitdown()) //已坐下
            {
                buttonTrust.gameObject.SetActive(true);
                buttonTrust.interactable = !buttonCancelTrust.gameObject.activeInHierarchy;
            }
            else //未坐下
            {
                buttonTrust.gameObject.SetActive(false);
                menuHeight -= 129;
            }

            //提前离桌
            buttonPreLeave.gameObject.SetActive(false);
            menuHeight -= 129;

            //重购
            bool canRebuy = false;
            if (rebuyCount > 0 && mainPlayer != null && mainPlayer.seatID < 0 && blindLevel < rebuyLevel && !inRewardCircle)
            {
                canRebuy = true;
            }
            if (canRebuy)
            {
                buttonRebuy.gameObject.SetActive(true);
                buttonRebuy.interactable = true;
                menuHeight += 129;
            }
            else
            {
                buttonRebuy.gameObject.SetActive(false);
            }

            //线路
            buttonNetline.transform.Find("Text").GetComponent<Text>().text = GlobalData.Instance.NameForServerID(GlobalData.Instance.CurrentUsingServerID());

            RectTransform mRectTransform = transSubMenu as RectTransform;
            if (null != mRectTransform)
                mRectTransform.sizeDelta = new Vector2(mRectTransform.sizeDelta.x, menuHeight);
        }

        protected override void UpdateRoomDes()
        {
            StringBuilder mStringBuilder = new StringBuilder();
            mStringBuilder.AppendLine(GameCache.Instance.roomName);
            mStringBuilder.AppendLine(GetRoomTypeDes());

            if (blindLevel >= MTTGameUtil.numOfLevel(blindType))
            {
                blindLevel = MTTGameUtil.numOfLevel(blindType) - 1;
            }
            int curBld = MTTGameUtil.BlindAtLevel(blindLevel, blindType, blindScale);
            int curAnte = MTTGameUtil.AnteAtLevel(blindLevel, blindType, blindScale);
            mStringBuilder.AppendLine($"{LanguageManager.Get("UITexasReport_Text_MatchCurrBlindTip")}:{StringHelper.GetShortString(curBld)}/{StringHelper.GetShortString(curBld * 2)}({StringHelper.GetShortString(curAnte)})");

            int nextBlindLevel = blindLevel + 1;//从1开始，转一下
            if (nextBlindLevel >= MTTGameUtil.numOfLevel(blindType))
            {
                nextBlindLevel = MTTGameUtil.numOfLevel(blindType) - 1;
            }
            int nextBld = MTTGameUtil.BlindAtLevel(nextBlindLevel, blindType, blindScale);
            int nextAnte = MTTGameUtil.AnteAtLevel(nextBlindLevel, blindType, blindScale);
            mStringBuilder.AppendLine($"{LanguageManager.Get("UITexasReport_Text_MatchNextBlindTip")}:{StringHelper.GetShortString(nextBld)}/{StringHelper.GetShortString(nextBld * 2)}({StringHelper.GetShortString(nextAnte)})");

            mStringBuilder.AppendLine(LanguageManager.Get("MTT_RoomInfo_UpBlindTimeLeft") + TimeHelper.ShowRemainingSemicolonPure(upBlindLeftTime));

            if (isGPSRestrictions == 1 && isIpRestrictions == 1)
            {
                // mStringBuilder.AppendLine("GPS  IP限制");
                mStringBuilder.AppendLine($"GPS  IP{CPErrorCode.LanguageDescription(20008)}");
            }
            else if (isGPSRestrictions == 1 && isIpRestrictions == 0)
            {
                // mStringBuilder.AppendLine("GPS限制");
                mStringBuilder.AppendLine($"GPS{CPErrorCode.LanguageDescription(20008)}");
            }
            else if (isGPSRestrictions == 0 && isIpRestrictions == 1)
            {
                // mStringBuilder.AppendLine("IP限制");
                mStringBuilder.AppendLine($"IP{CPErrorCode.LanguageDescription(20008)}");
            }

            textRoomInfo.text = mStringBuilder.ToString();
        }

        protected override string GetRoomTypeDes()
        {
            var typeDes = LanguageManager.Get("MTT_RoomInfo_Type");
            if (huntMode)
            {
                typeDes = $"{typeDes} {LanguageManager.Get("MTT_RoomInfo_Type_Hunter")}";
            }
            return typeDes;
        }

        protected override void SetUpLogo()
        {
            imageLogo.sprite = this.rc.Get<Sprite>("icon_mtt");
            imageLogo.SetNativeSize();
        }

        // 托管相关
        protected override void SendTrustAction(sbyte option, sbyte initiative)
        {
            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_MTT_AUTO_OP()
            {
                roomID = GameCache.Instance.room_id,
                roomPath = GameCache.Instance.room_path,
                option = option,
                initiative = initiative,
            });
        }

        protected override List<sbyte> GetHandCardsAtEnterRoom(object obj, int index)
        {
            REQ_GAME_MTT_ENTER_ROOM rec = obj as REQ_GAME_MTT_ENTER_ROOM;
            sbyte mFirstCard = rec.baseMsg.firstCardarray[index];
            sbyte mSecondCard = rec.baseMsg.SecondArray[index];
            return new List<sbyte>() { mFirstCard, mSecondCard };
        }

        protected override List<sbyte> GetHandCardsAtRecvStartInfo(object obj, int index)
        {
            REQ_GAME_MTT_RECV_START_INFOR rec = obj as REQ_GAME_MTT_RECV_START_INFOR;
            sbyte mFirstCard = rec.baseMsg.firstCardArray[index];
            sbyte mSecondCard = rec.baseMsg.SecondCardArray[index];
            return new List<sbyte>() { mFirstCard, mSecondCard};
        }
    }

}
