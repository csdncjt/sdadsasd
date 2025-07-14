using System;
using System.Collections.Generic;
using DG.Tweening;
using ETModel;

namespace ETHotfix
{
    public partial class TexasGame : Entity
    {
        protected virtual void registerHandler()
        {
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_SEND_SEAT_ACTION, HANDLER_REQ_GAME_SEND_SEAT_ACTION);    // 当前玩家站起或坐下或离开
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_SEND_ACTION, HANDLER_REQ_GAME_SEND_ACTION);  // 自己牌桌操作
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_RECV_ACTION, HANDLER_REQ_GAME_RECV_ACTION);  // 收到牌桌操作
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_RECV_LEAVE, HANDLER_REQ_GAME_RECV_LEAVE);  // 别人离开座位
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_RECV_SEAT_DOWN, HANDLER_REQ_GAME_RECV_SEAT_DOWN);  // 别人坐下
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_RECV_CARDS, HANDLER_REQ_GAME_RECV_CARDS);  // 收到公共牌
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_RECV_WINNER, HANDLER_REQ_GAME_RECV_WINNER);  // 收到胜利者
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_RECV_READYTIME, HANDLER_REQ_GAME_RECV_READYTIME);  // 收到准备时间
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_RECV_START_INFOR, HANDLER_REQ_GAME_RECV_START_INFOR);  // 新一手开始（普通局、SNG）
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_ADD_CHIPS, HANDLER_REQ_GAME_ADD_CHIPS);  // 带入
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_START_GAME, HANDLER_REQ_GAME_START_GAME);  // 房主点击开始游戏
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_ENDING, HANDLER_REQ_GAME_ENDING);  // 牌局结束
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_TAKE_CONTROL, HANDLER_REQ_GAME_TAKE_CONTROL);  // 房主设置控制带入开关
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_CONTROL_RANGE, HANDLER_REQ_GAME_CONTROL_RANGE);  // 房主设置带入倍数
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_PLAYER_CARDS, HANDLER_REQ_GAME_PLAYER_CARDS);  // Allin下发玩家手牌
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_SHOWDOWN, HANDLER_REQ_SHOWDOWN);  // 设置结束时亮的手牌
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_JACKPOT_SHOW_CARDS, HANDLER_REQ_GAME_JACKPOT_SHOW_CARDS);  // 设置结束时亮的手牌
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_RECV_PRE_START_GAME, HANDLER_REQ_GAME_RECV_PRE_START_GAME);  // 每手游戏开始前处理
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_TRUST_ACTION, HANDLER_REQ_GAME_TRUST_ACTION);  // 托管
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_FINISHGAME, HANDLER_REQ_FINISHGAME);  // 解散房间
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_CHECK_CAN_CICK, HANDLER_REQ_GAME_CHECK_CAN_CICK);  // 查询对某人是否有踢出权限
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_JACKPOT_CHANGE, HANDLER_REQ_GAME_JACKPOT_CHANGE);  // JackPot变化通知
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_START_ROOM, HANDLER_REQ_START_ROOM);  // 等待页开始、退出或解散
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_REAL_TIME, HANDLER_REQ_GAME_REAL_TIME);  // 实时战况
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_ADD_TIME, HANDLER_REQ_ADD_TIME);  // 操作加时
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_PAUSE, HANDLER_REQ_GAME_PAUSE);  // 暂停游戏
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_COLLECT_CARD, HANDLER_REQ_GAME_COLLECT_CARD);  // 收藏牌谱
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_SHOW_SIDE_POTS, HANDLER_REQ_SHOW_SIDE_POTS);  // 显示分池筹码
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_WAIT_BLIND, HANDLER_REQ_WAIT_BLIND);  // 过庄补盲
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_INSURANCE_TRIGGED, HANDLER_REQ_INSURANCE_TRIGGED);  // 保险触发
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_BUY_INSURANCE, HANDLER_REQ_BUY_INSURANCE);  // 购买保险
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_CLAIM_INSURANCE, HANDLER_REQ_CLAIM_INSURANCE);  // 保险赔付消息
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_INSURANCE_ADD_TIME, HANDLER_REQ_INSURANCE_ADD_TIME);  // 保险加时
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_INSURANCE_NOT_TRIGGED, HANDLER_REQ_INSURANCE_NOT_TRIGGED);  // 保险条件不足  (客户端端未解析内容，收到该协议则提示：“OUTS>16或OUTS=0，不能购买保险”)
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_SEE_MORE_PUBLIC_ACTION, HANDLER_REQ_SEE_MORE_PUBLIC_ACTION);  // 查看未发公共牌  (查看成功，会在【24】收到公共牌协议下发查看的公共牌)
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_CURRENT_ROUND_FINISH, HANDLER_REQ_CURRENT_ROUND_FINISH);  // 当前手结束  (收到该协议，清空当前手的界面和数据)
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_KEEP_SEAT, HANDLER_REQ_GAME_KEEP_SEAT);  // 留座离桌
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_WAIT_DAIRU, HANDLER_REQ_GAME_WAIT_DAIRU);  // 审核带入
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_ADD_ROOM_TIME, HANDLER_REQ_ADD_ROOM_TIME);  // 房间加时  (房主操作了加时，房间内所有玩家都能收到通知)
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_RECV_PRE_START_GAME_STRADDLE, HANDLER_REQ_GAME_RECV_PRE_START_GAME_STRADDLE);  // 开启关闭强制Straddle  (房主操作了开启关闭Straddle，房间内所有玩家都能收到通知)
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_RECV_PRE_START_GAME_MUCK, HANDLER_REQ_GAME_RECV_PRE_START_GAME_MUCK);  // 开启关闭Muck  (房主操作了开启关闭Muck，房间内所有玩家都能收到通知)
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_RECV_SPECTATORS_VOICE, HANDLER_REQ_GAME_RECV_SPECTATORS_VOICE);  // 开启关闭观众语言  (房主操作了开启关闭Muck，房间内所有玩家都能收到通知)
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_JACKPOT_HIT_BROADCAST, HANDLER_REQ_GAME_JACKPOT_HIT_BROADCAST);    // JackPot击中广播
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_ASK_TEST, HANDLER_REQ_GAME_ASK_TEST);  // 测试


        }

        protected virtual void removeHandler()
        {
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_SEND_SEAT_ACTION, HANDLER_REQ_GAME_SEND_SEAT_ACTION);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_SEND_ACTION, HANDLER_REQ_GAME_SEND_ACTION);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_RECV_ACTION, HANDLER_REQ_GAME_RECV_ACTION);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_RECV_LEAVE, HANDLER_REQ_GAME_RECV_LEAVE);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_RECV_SEAT_DOWN, HANDLER_REQ_GAME_RECV_SEAT_DOWN);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_RECV_CARDS, HANDLER_REQ_GAME_RECV_CARDS);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_RECV_WINNER, HANDLER_REQ_GAME_RECV_WINNER);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_RECV_READYTIME, HANDLER_REQ_GAME_RECV_READYTIME);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_RECV_START_INFOR, HANDLER_REQ_GAME_RECV_START_INFOR);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_ADD_CHIPS, HANDLER_REQ_GAME_ADD_CHIPS);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_START_GAME, HANDLER_REQ_GAME_START_GAME);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_ENDING, HANDLER_REQ_GAME_ENDING);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_TAKE_CONTROL, HANDLER_REQ_GAME_TAKE_CONTROL);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_CONTROL_RANGE, HANDLER_REQ_GAME_CONTROL_RANGE);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_PLAYER_CARDS, HANDLER_REQ_GAME_PLAYER_CARDS);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_SHOWDOWN, HANDLER_REQ_SHOWDOWN);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_JACKPOT_SHOW_CARDS, HANDLER_REQ_GAME_JACKPOT_SHOW_CARDS);  // 设置结束时亮的手牌
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_RECV_PRE_START_GAME, HANDLER_REQ_GAME_RECV_PRE_START_GAME);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_TRUST_ACTION, HANDLER_REQ_GAME_TRUST_ACTION);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_FINISHGAME, HANDLER_REQ_FINISHGAME);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_CHECK_CAN_CICK, HANDLER_REQ_GAME_CHECK_CAN_CICK);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_JACKPOT_CHANGE, HANDLER_REQ_GAME_JACKPOT_CHANGE);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_START_ROOM, HANDLER_REQ_START_ROOM);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_REAL_TIME, HANDLER_REQ_GAME_REAL_TIME);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_ADD_TIME, HANDLER_REQ_ADD_TIME);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_PAUSE, HANDLER_REQ_GAME_PAUSE);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_COLLECT_CARD, HANDLER_REQ_GAME_COLLECT_CARD);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_SHOW_SIDE_POTS, HANDLER_REQ_SHOW_SIDE_POTS);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_WAIT_BLIND, HANDLER_REQ_WAIT_BLIND);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_INSURANCE_TRIGGED, HANDLER_REQ_INSURANCE_TRIGGED);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_BUY_INSURANCE, HANDLER_REQ_BUY_INSURANCE);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_CLAIM_INSURANCE, HANDLER_REQ_CLAIM_INSURANCE);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_INSURANCE_ADD_TIME, HANDLER_REQ_INSURANCE_ADD_TIME);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_INSURANCE_NOT_TRIGGED, HANDLER_REQ_INSURANCE_NOT_TRIGGED);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_SEE_MORE_PUBLIC_ACTION, HANDLER_REQ_SEE_MORE_PUBLIC_ACTION);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_CURRENT_ROUND_FINISH, HANDLER_REQ_CURRENT_ROUND_FINISH);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_KEEP_SEAT, HANDLER_REQ_GAME_KEEP_SEAT);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_WAIT_DAIRU, HANDLER_REQ_GAME_WAIT_DAIRU);  // 审核带入
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_ADD_ROOM_TIME, HANDLER_REQ_ADD_ROOM_TIME);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_RECV_PRE_START_GAME_STRADDLE, HANDLER_REQ_GAME_RECV_PRE_START_GAME_STRADDLE);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_RECV_PRE_START_GAME_MUCK, HANDLER_REQ_GAME_RECV_PRE_START_GAME_MUCK);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_RECV_SPECTATORS_VOICE, HANDLER_REQ_GAME_RECV_SPECTATORS_VOICE);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_JACKPOT_HIT_BROADCAST, HANDLER_REQ_GAME_JACKPOT_HIT_BROADCAST);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_ASK_TEST, HANDLER_REQ_GAME_ASK_TEST);  // 测试

        }

        protected void HANDLER_REQ_GAME_ASK_TEST(ICPResponse response)
        {
            REQ_GAME_ASK_TEST rec = response as REQ_GAME_ASK_TEST;
            if (null == rec)
                return;

            if (rec.status != 0)
            {
                return;
            }
            UIComponent.Instance.ShowNoAnimation(UIType.UITexasTest, rec);
        }

        protected void HANDLER_REQ_GAME_RECV_SPECTATORS_VOICE(ICPResponse response)
        {
            REQ_GAME_RECV_SPECTATORS_VOICE rec = response as REQ_GAME_RECV_SPECTATORS_VOICE;
            if (null == rec)
                return;

            if (rec.status != 0)
            {
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_GAME_RECV_SPECTATORS_VOICE, rec.status));
                return;
            }

            bSpectatorsVoice = rec.spectatorsVoice;
            if (rec.spectatorsVoice == 1)
            {
                // UIComponent.Instance.Toast("房主已开启观众语音");
                UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10027));
            }
            else
            {
                // UIComponent.Instance.Toast("房主已关闭观众语音");
                UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10028));
            }
        }

        protected void HANDLER_REQ_GAME_RECV_PRE_START_GAME_MUCK(ICPResponse response)
        {
            REQ_GAME_RECV_PRE_START_GAME_MUCK rec = response as REQ_GAME_RECV_PRE_START_GAME_MUCK;
            if (null == rec)
                return;

            if (rec.status != 0)
            {
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_GAME_RECV_PRE_START_GAME_MUCK, rec.status));
                return;
            }

            GameCache.Instance.muck_switch = rec.bMuckRec;
            if (rec.bMuckRec == 1)
            {
                // UIComponent.Instance.Toast("房主已开启Muck");
                UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(20011));
            }
            else
            {
                // UIComponent.Instance.Toast("房主已关闭Muck");
                UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(20012));
            }
        }

        protected void HANDLER_REQ_GAME_RECV_PRE_START_GAME_STRADDLE(ICPResponse response)
        {
            REQ_GAME_RECV_PRE_START_GAME_STRADDLE rec = response as REQ_GAME_RECV_PRE_START_GAME_STRADDLE;
            if (null == rec)
                return;

            if (rec.status != 0)
            {
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_GAME_RECV_PRE_START_GAME_STRADDLE, rec.status));
                return;
            }

            GameCache.Instance.straddle = rec.straddle;
            if (rec.straddle == 1)
            {
                // UIComponent.Instance.Toast("房主已开启强制Straddle");
                UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(20013));
            }
            else
            {
                // UIComponent.Instance.Toast("房主已关闭强制Straddle");
                UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(20014));
            }
        }

        protected void HANDLER_REQ_ADD_ROOM_TIME(ICPResponse response)
        {
            REQ_ADD_ROOM_TIME rec = response as REQ_ADD_ROOM_TIME;
            if (null == rec)
                return;

            if (rec.status != 0)
            {
                CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_ADD_ROOM_TIME, rec.status);
                return;
            }


            roomLeftTime = rec.minutesRec * 60;
            // UIComponent.Instance.Toast($"房主进行延时{rec.minutesRec}分钟");
            UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(20015, new List<object>() { rec.minutesRec }));
        }

        protected void HANDLER_REQ_GAME_WAIT_DAIRU(ICPResponse response)
        {
            REQ_GAME_WAIT_DAIRU rec = response as REQ_GAME_WAIT_DAIRU;
            if (null == rec)
                return;
            if (rec.status != 0)
                return;

            Seat mSeat = GetSeatById((sbyte)rec.seatId);
            if (null == mSeat)
                return;

            mSeat.Player.timeLeft_dairu = rec.leftTime;
            mSeat.FsmLogicComponent.SM.ChangeState(SeatWaitDairu<Entity>.Instance);
        }

        protected void HANDLER_REQ_GAME_KEEP_SEAT(ICPResponse response)
        {
            REQ_GAME_KEEP_SEAT rec = response as REQ_GAME_KEEP_SEAT;
            if (null == rec)
                return;

            if (rec.status != 0)
                return;

            switch (rec.optionRec)
            {
                case 1:
                    break;
                case 2:
                    if (GameCache.Instance.CurGame.mainPlayer.userID == rec.userId)
                    {
                        UIComponent.Instance.Show(UIType.UIToast, $"您在 {rec.roomName} 牌局中处于留座状态，无法加入本局游戏", null, 0);
                    }
                    break;
                case 3:
                    Seat mSeat = GetSeatById((sbyte)rec.seatId);
                    if (null == mSeat)
                        break;

                    mSeat.keepSeatLeftTime = rec.leftTime;
                    mSeat.Player.status = 15;

                    if (GameCache.Instance.CurGame.mainPlayer.userID == rec.userId)
                    {
                        // 弹代入框
                        UIComponent.Instance.Show(UIType.UIAddChips,
                                                  new UIAddChipsComponent.AddClipsData()
                                                  {
                                                      qianzhu = groupBet,
                                                      bigBlind = bigBlind,
                                                      smallBlind = smallBlind,
                                                      currentMinRate = currentMinRate,
                                                      currentMaxRate = currentMaxRate,
                                                      totalCoin = GameCache.Instance.gold,
                                                      tableChips = mainPlayer.chips,
                                                      isNeedCost = roomMode == 0 ? 1:0,
                                                  }, null, 0);

                        if (GameCache.Instance.shortest_time > 0)
                        {
                            if (bShortestTimeCounting)
                            {
                                bLoseAll = true; //输光了，最短上桌倒计时取消，保留此时倒计时剩余时间
                            }

                            bShortestTimeCounting = false;
                            buttonCancelTrust.gameObject.SetActive(false);
                        }
                    }

                    mSeat.FsmLogicComponent.SM.ChangeState(SeatKeep<Entity>.Instance);
                    break;
            }
        }

        protected virtual void HANDLER_REQ_CURRENT_ROUND_FINISH(ICPResponse response)
        {
            REQ_CURRENT_ROUND_FINISH rec = response as REQ_CURRENT_ROUND_FINISH;
            if (null == rec)
                return;

            gamestatus = -1;

            ResetShowCardsId();

            HideSeeMorePublic();
            HideSeeMorePublicTips();

            HideWaitBlindBtn();

            //防止大牌动画未消失
            if(isPlayingBigWinAnimation)
                UIComponent.Instance.HideNoAnimation(UIType.UIBigWinAnimation);

            // 刷新底池
            alreadAnte = 0;
            UpdateAlreadAnte();

            // 刷新分池
            if(pots!=null)
                pots.Clear();

            UpdatePots();

            // 刷新公共牌
            ResetPublicCardsId();
            ResetPublicCardsImage();
            // UpdatePublicCards(0, null);

            Seat mSeat = null;
            for (int i = 0, n = listSeat.Count; i < n; i++)
            {
                mSeat = listSeat[i];
                if (null == mSeat || null == mSeat.Player)
                    continue;

                if (mSeat.Player.canPlayStatus == 0)
                    continue;

                mSeat.Player.canPlayStatus = 0;
                if (mSeat.Player.status != 15 && mSeat.Player.status != 18)
                {
                    mSeat.Player.status = 0;
                }
                mSeat.FsmLogicComponent.SM.ChangeState(SeatRoundEnd<Entity>.Instance);
            }
        }

        protected void HANDLER_REQ_SEE_MORE_PUBLIC_ACTION(ICPResponse response)
        {
            REQ_SEE_MORE_PUBLIC_ACTION rec = response as REQ_SEE_MORE_PUBLIC_ACTION;
            if (null == rec)
                return;

            if (rec.status != 0)
            {
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_SEE_MORE_PUBLIC_ACTION, rec.status));
                return;
            }

            GameCache.Instance.gold = rec.chips;
        }

        protected void HANDLER_REQ_INSURANCE_NOT_TRIGGED(ICPResponse response)
        {
            UIComponent.Instance.HideNoAnimation(UIType.UIInsurance);

            // Game.Scene.GetComponent<UIComponent>().Show(UIType.UIToast, $"OUTS>16 或 OUTS=0，不能购买保险", null, 0);
            UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(20017));
        }

        protected void HANDLER_REQ_INSURANCE_ADD_TIME(ICPResponse response)
        {
            REQ_INSURANCE_ADD_TIME rec = response as REQ_INSURANCE_ADD_TIME;
            if (null == rec)
                return;

            if (rec.status != 0)
            {
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_INSURANCE_ADD_TIME, rec.status));
                return;
            }

            Seat mSeat = GetSeatByUserId(rec.playerId);
            if (null == mSeat || null == mSeat.Player)
                return;

            mSeat.AddCountDown(rec.addTime);
        }

        protected void HANDLER_REQ_CLAIM_INSURANCE(ICPResponse response)
        {
            REQ_CLAIM_INSURANCE rec = response as REQ_CLAIM_INSURANCE;
            if (null == rec)
                return;

            bool bust = false;
            for (int i = 0; i < rec.userIds.Count; i++)
            {
                if (rec.claimAmounts[i] > 0)
                {
                    Seat mSeat = GetSeatById(Convert.ToSByte(rec.seatIds[i]));
                    mSeat.Player.claimInsuredAmount = rec.claimAmounts[i];
                    mSeat.UpdateBubbleInsuranceClaim();
                    bust = true;
                }
            }
            if (bust)
            {
                //爆牌动画
                ShowBustCardAnimation();
            }
        }

        protected void HANDLER_REQ_BUY_INSURANCE(ICPResponse response)
        {
            REQ_BUY_INSURANCE rec = response as REQ_BUY_INSURANCE;
            if (null == rec)
                return;

            Seat mSeat = GetSeatById(rec.seatIdRec);
            mSeat.Player.totalInsuredAmount = rec.totalInsuredAmount;
            mSeat.Player.autoInsuredAmount = rec.autoInsuredAmount;
            mSeat.HideBubbleInsuranceCountDown();
            mSeat.UpdateBubbleInsurance();

            if (rec.userId == GameCache.Instance.nUserId)
            {
                switch (rec.status)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        // UIComponent.Instance.ShowNoAnimation(UIType.UIToast, $"系统自动背保险，购买{rec.autoInsuredAmount}游戏币");
                        UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(20018, new List<object>() { StringHelper.ShowGold(rec.autoInsuredAmount)}));
                        break;
                    case 3:
                        // UIComponent.Instance.ShowNoAnimation(UIType.UIToast, $"您本次投保额最大为自身投入底池积分的0.45，已自动购买最高值{rec.totalInsuredAmount + rec.autoInsuredAmount}");
                        UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(20019, new List<object>() { StringHelper.ShowGold(rec.totalInsuredAmount + rec.autoInsuredAmount) }));

                        break;
                    case 4:
                        // UIComponent.Instance.ShowNoAnimation(UIType.UIToast, $"二次投保额最高为1/2底池，已自动购买最高值%d{rec.totalInsuredAmount + rec.autoInsuredAmount}");
                        UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(20020, new List<object>() { StringHelper.ShowGold(rec.totalInsuredAmount + rec.autoInsuredAmount) }));
                        break;
                    case 5:
                        UIComponent.Instance.HideNoAnimation(UIType.UIInsurance);
                        break;
                }
            }

        }

        protected virtual void HANDLER_REQ_INSURANCE_TRIGGED(ICPResponse response)
        {
            REQ_INSURANCE_TRIGGED rec = response as REQ_INSURANCE_TRIGGED;
            if (null == rec)
                return;

            bool mCanInsurance = false;

            Seat mSeat = null;
            // 拆分数据
            try
            {
                for (int i = 0, n = rec.players.Count; i < n; i++)
                {
                    mSeat = GetSeatByUserId(rec.players[i]);
                    if (null == mSeat || null == mSeat.Player)
                    {
                        continue;
                    }

                    mSeat.Player.playerStatus_insurance = rec.playerStatus[i];
                    mSeat.Player.totalInsuredAmount = rec.playerInsuredAmount[i];
                    mSeat.Player.timeLeft_insurance = rec.timeLeft[i];

                    if (mSeat.Player.userID == mainPlayer.userID && mSeat.Player.playerStatus_insurance == 0)
                    {
                        mCanInsurance = true;
                    }

                    if (mSeat.Player.playerStatus_insurance == 0)
                        mSeat.FsmLogicComponent.SM.ChangeState(SeatInsurance<Entity>.Instance);
                }
            }
            catch (Exception e)
            {
                Game.Scene.GetComponent<NetworkDetectionComponent>().Reconnect();
            }

            TweenCallback mTweenCallback = () =>
            {
                if (!mCanInsurance) // 如果可购买保险用户中没有自己，不用往下执行
                    return;

                int mOutsIndex = 0;
                List<List<int>> mOuts = new List<List<int>>();
                try
                {
                    for (int i = 0, n = rec.outs.Count; i < n; i++)
                    {
                        if (rec.outs[i] == -1)
                        {
                            mOutsIndex++;
                            continue;
                        }

                        if (mOutsIndex == mOuts.Count)
                        {
                            List<int> mSubOuts = new List<int>();
                            mOuts.Add(mSubOuts);
                        }
                        mOuts[mOutsIndex].Add(rec.outs[i]);
                    }
                }
                catch (Exception e)
                {
                    Game.Scene.GetComponent<NetworkDetectionComponent>().Reconnect();
                }

                string[] mNamesByPot = rec.userNames.Split(new string[] { "@%@%" }, StringSplitOptions.None);

                List<List<int>> mOutsPreUser = new List<List<int>>();
                List<List<sbyte>> mPlayerCards = new List<List<sbyte>>();
                string[] mNames = null;
                int mOutsPreUserIndex = 0;
                try
                {
                    for (int i = 0, n = mNamesByPot.Length; i < n; i++)
                    {
                        List<int> mSubOutsPreUser = new List<int>();
                        List<sbyte> mSubPlayerCards = new List<sbyte>();
                        mNames = mNamesByPot[i].Split(new string[] { "@%" }, StringSplitOptions.None);
                        for (int j = 0, m = mNames.Length; j < m; j++)
                        {
                            mSubOutsPreUser.Add(rec.outsPerUser[mOutsPreUserIndex]);
                            int mTmpHandCards = GameCache.Instance.CurGame.HandCards;
                            for (int k = 0; k < mTmpHandCards; k++)
                            {
                                mSubPlayerCards.Add((sbyte)rec.playerCards[mOutsPreUserIndex * mTmpHandCards + k]);
                            }

                            mOutsPreUserIndex++;
                        }

                        mOutsPreUser.Add(mSubOutsPreUser);
                        mPlayerCards.Add(mSubPlayerCards);
                    }
                }
                catch (Exception e)
                {
                    Game.Scene.GetComponent<NetworkDetectionComponent>().Reconnect();
                }
                
                List<UIInsuranceComponent.WrapTriggedInsuranceData> mListWrapTriggedInsuranceDatas = null;
                UIInsuranceComponent.WrapTriggedInsuranceData mWrapTriggedInsuranceData = null;
                try
                {
                    for (int i = 0, n = rec.subPots.Count; i < n; i++)
                    {
                        mWrapTriggedInsuranceData = new UIInsuranceComponent.WrapTriggedInsuranceData();
                        mWrapTriggedInsuranceData.subPot = rec.subPots[i];
                        if (i < mOuts.Count)
                            mWrapTriggedInsuranceData.outs = mOuts[i];
                        mWrapTriggedInsuranceData.odd = rec.odds[i];
                        mWrapTriggedInsuranceData.leastAmount = rec.leastAmount[i];
                        mWrapTriggedInsuranceData.mostAmount = rec.mostAmount[i];
                        mWrapTriggedInsuranceData.secureAmount = rec.secureAmount[i];
                        mWrapTriggedInsuranceData.potAllowOutSelection = rec.potAllowOutSelections[i];
                        mWrapTriggedInsuranceData.potTotalCost = rec.potTotalCosts[i];
                        mWrapTriggedInsuranceData.myChipsInPool = rec.myChipsInPool[i];
                        if (i < mNamesByPot.Length)
                            mWrapTriggedInsuranceData.userNames = mNamesByPot[i];
                        if (i < mOutsPreUser.Count)
                            mWrapTriggedInsuranceData.outsPerUser = mOutsPreUser[i];
                        if (i < mPlayerCards.Count)
                            mWrapTriggedInsuranceData.playerCards = mPlayerCards[i];
                        mWrapTriggedInsuranceData.pot = pots[mWrapTriggedInsuranceData.subPot];

                        if (null == mListWrapTriggedInsuranceDatas)
                            mListWrapTriggedInsuranceDatas = new List<UIInsuranceComponent.WrapTriggedInsuranceData>();
                        mListWrapTriggedInsuranceDatas.Add(mWrapTriggedInsuranceData);
                    }
                }
                catch (Exception e)
                {
                    Game.Scene.GetComponent<NetworkDetectionComponent>().Reconnect();
                }

                UIComponent.Instance.ShowNoAnimation(UIType.UIInsurance, new UIInsuranceComponent.InsuranceData()
                {
                    cards = cards,
                    triggedData = mListWrapTriggedInsuranceDatas,
                    timeLeft = mainPlayer.timeLeft_insurance,
                    existEqualOuts = rec.existEqualOuts,
                    delayTimes = rec.insuranceDelayTimes
                });
            };

            if (rec.count <= 1)
            {
                // 当前触发保险次数，如果<=1，则先显示“保险模式”动画，再弹出保险框
                PlayFirstInsurance(mTweenCallback);
                // await WaitGameAnimation(GameAnimation.PlayFirstInsurance);
            }
            else
            {
                mTweenCallback();
            }
        }

        protected void HANDLER_REQ_WAIT_BLIND(ICPResponse response)
        {
            REQ_WAIT_BLIND rec = response as REQ_WAIT_BLIND;
            if (null == rec)
                return;

            if (rec.status != 0)
            {
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_WAIT_BLIND, rec.status));
                return;
            }

            HideWaitBlindBtn();
            Seat mSeat = GetSeatById(mainPlayer.seatID);
            if (rec.optionRec == 0)
            {
                // 过庄
                // UIComponent.Instance.Toast("已选择过庄模式");
                UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10029));
                mSeat.ShowGuozhuangBuuble();

            }
            else if (rec.optionRec == 1)
            {
                // 补盲
                // UIComponent.Instance.Toast("当前局若不是大盲位，你将在扣至大盲数额的游戏币后，加入牌局");
                UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(20021));
                if (null != mSeat)
                {
                    mSeat.FsmLogicComponent.SM.ChangeState(SeatWaitStart<Entity>.Instance);
                }
            }
        }

        protected void HANDLER_REQ_SHOW_SIDE_POTS(ICPResponse response)
        {
            REQ_SHOW_SIDE_POTS rec = response as REQ_SHOW_SIDE_POTS;
            if (null == rec)
                return;
            pots = rec.pots;

            // 播放首次收筹码到底池动画是不需要显示Pots
            // if (GetCurPublicCardsCount() > 0)
            // UpdatePots();
        }

        protected void HANDLER_REQ_GAME_COLLECT_CARD(ICPResponse response)
        {
            REQ_GAME_COLLECT_CARD rec = response as REQ_GAME_COLLECT_CARD;
            if (null == rec)
                return;

            if (rec.status != 0)
            {
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_GAME_COLLECT_CARD, rec.status));
                return;
            }
        }

        protected void HANDLER_REQ_GAME_PAUSE(ICPResponse response)
        {
            REQ_GAME_PAUSE rec = response as REQ_GAME_PAUSE;
            if (null == rec)
                return;

            switch (rec.isPause)
            {
                case 0:
                    {
                        // 继续
                        bPaused = false;
                        ContinueCountDown();
                        UIComponent.Instance.HideNoAnimation(UIType.UIPause);
                    }
                    break;
                case 1:
                    {
                        // 暂停
                        bPaused = true;
                        PauseCountDown();
                        UIComponent.Instance.ShowNoAnimation(UIType.UIPause, new UIPauseComponent.PauseData()
                        {
                            roomPath = GameCache.Instance.room_path,
                            roomId = GameCache.Instance.room_id,
                            remainingTime = rec.remainingTime,
                            isOwner = roomOwner == GameCache.Instance.nUserId
                        });
                    }
                    break;
                case 2:
                    // 操作失败
                    break;
            }
        }

        protected void HANDLER_REQ_ADD_TIME(ICPResponse response)
        {
            REQ_ADD_TIME rec = response as REQ_ADD_TIME;
            if (null == rec)
                return;

            if (rec.status != 0)
            {
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_ADD_TIME, rec.status));
                return;
            }

            if (rec.playerId == GameCache.Instance.CurGame.MainPlayer.userID)
            {
                //自己延时
                GameCache.Instance.gold -= rec.coastCoins;
                GameCache.Instance.idou -= rec.coastJewel;
                delayCount++;
                UpdateDelayBtn();
            }
            else
            {
                //他人延时
                Seat mSeat = GetSeatByUserId(rec.playerId);
                if (null == mSeat)
                    return;
                mSeat.AddOperationTime(rec.addTime);
            }

        }

        protected void HANDLER_REQ_GAME_REAL_TIME(ICPResponse response)
        {
            REQ_GAME_REAL_TIME rec = response as REQ_GAME_REAL_TIME;
            if (null == rec)
                return;

            if (rec.status != 0)
            {
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_GAME_REAL_TIME, rec.status));
                return;
            }

            UIComponent.Instance.ShowNoAnimation(UIType.UITexasReport, rec);

        }

        protected void HANDLER_REQ_START_ROOM(ICPResponse response)
        {
            REQ_START_ROOM rec = response as REQ_START_ROOM;
            if (null == rec)
                return;

            if (rec.status != 0)
            {
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_START_ROOM, rec.status));
                return;
            }
        }

        protected void HANDLER_REQ_GAME_JACKPOT_CHANGE(ICPResponse response)
        {
            REQ_GAME_JACKPOT_CHANGE rec = response as REQ_GAME_JACKPOT_CHANGE;
            if (null == rec)
                return;
            if (rec.status == 0)
            {
                jackPotLabel.SetNumber(Convert.ToInt32(rec.jackPotNum), true);
            }
        }

        protected void HANDLER_REQ_GAME_CHECK_CAN_CICK(ICPResponse response)
        {
            REQ_GAME_CHECK_CAN_CICK rec = response as REQ_GAME_CHECK_CAN_CICK;
            if (null == rec)
                return;

            var web = new WEB2_user_public_info.RequestData()
            {
                randomId = "",
                userId = rec.userIDRec
            };
            HttpRequestComponent.Instance.Send(WEB2_user_public_info.API, WEB2_user_public_info.Request(web), json =>
            {
                var tDto = WEB2_user_public_info.Response(json);
                if (tDto.status == 0)
                {
                    UIComponent.Instance.ShowNoAnimation(UIType.UITexasInfo, new UITexasInfoComponent.InfoData()
                    {
                        canKickOut = rec.canKickOut,
                        mUserInfo = tDto.data,
                        mUserId = rec.userIDRec
                    });
                }
                else
                {
                    UIComponent.Instance.Toast(tDto.status);
                }
            });
        }

        protected void HANDLER_REQ_FINISHGAME(ICPResponse response)
        {
            REQ_FINISHGAME rec = response as REQ_FINISHGAME;
            if (null == rec)
                return;

            // 失败
            if (rec.status == 1)
            {
                CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_FINISHGAME, rec.status);
                return;
            }


            // UIComponent.Instance.Toast("房主已经解散房间");
            UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10030));
        }

        protected void HANDLER_REQ_GAME_TRUST_ACTION(ICPResponse response)
        {
            REQ_GAME_TRUST_ACTION rec = response as REQ_GAME_TRUST_ACTION;
            if (null == rec)
            {
                return;
            }

            if (rec.status != 0)
            {
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_GAME_TRUST_ACTION, rec.status));
                return;
            }

            Seat mSeat = null;
            switch (rec.optionRec)
            {
                case 0:
                    // 取消托管
                    mSeat = GetSeatById(mainPlayer.seatID);
                    mSeat.Player.trust = 0;
                    mSeat.UpdateTrust();
                    buttonCancelTrust.gameObject.SetActive(false);
                    shortestTime = rec.trustTime;
                    break;
                case 1:
                    // 托管
                    mSeat = GetSeatById(mainPlayer.seatID);
                    mSeat.Player.trust = 1;
                    mSeat.UpdateTrust();
                    shortestTime = rec.trustTime;
                    if (shortestTime > 0)
                    {
                        bShortestTimeCounting = !bPaused;
                        // textCancelTrust.text = $"返回游戏({shortestTime / 60}:{(shortestTime % 60).ToString().PadLeft(2, '0')})";
                        textCancelTrust.text = $"{CPErrorCode.LanguageDescription(10011)}({shortestTime / 60}:{(shortestTime % 60).ToString().PadLeft(2, '0')})";
                    }
                    else
                    {
                        // textCancelTrust.text = "返回游戏";
                        textCancelTrust.text = CPErrorCode.LanguageDescription(10011);
                    }

                    buttonCancelTrust.gameObject.SetActive(true);
                    hideMenu();
                    HideOperationPanel();
                    HideAutoOperationPanel();
                    HideWaitBlindBtn();

                    if (isExit)
                    {
                        isExit = false;
                        CallbackExit();
                    }

                    break;
                case 2:
                    {
                        // 进入房间时请求所有玩家的托管状态
                        bFirstSit = rec.firstSit == 1;
                        shortestTime = rec.trustTime;
                        if (shortestTime > 0 && !bFirstSit)
                        {
                            bShortestTimeCounting = !bPaused;
                        }
                        for (int i = 0, n = rec.trustStatus.Count; i < n; i++)
                        {
                            mSeat = GetSeatById((sbyte)i);
                            if (null == mSeat || null == mSeat.Player)
                                continue;

                            mSeat.Player.trust = rec.trustStatus[i];
                            mSeat.UpdateTrust();

                            if (mSeat.seatID == mainPlayer.seatID)
                            {
                                buttonCancelTrust.gameObject.SetActive(mSeat.Player.trust == 1);
                                if (mSeat.Player.trust == 1)
                                {
                                    if (shortestTime > 0 && !bFirstSit)
                                    {
                                        // textCancelTrust.text = $"返回游戏({shortestTime / 60}:{(shortestTime % 60).ToString().PadLeft(2, '0')})";
                                        textCancelTrust.text = $"{CPErrorCode.LanguageDescription(10011)}({shortestTime / 60}:{(shortestTime % 60).ToString().PadLeft(2, '0')})";
                                    }
                                    else
                                    {
                                        // textCancelTrust.text = "返回游戏";
                                        textCancelTrust.text = CPErrorCode.LanguageDescription(10011);
                                    }
                                    HideOperationPanel();
                                    HideAutoOperationPanel();
                                    HideWaitBlindBtn();
                                }
                            }
                        }
                    }
                    break;
                case 3:
                    // 托管时间到
                    mSeat = GetSeatById(mainPlayer.seatID);
                    mSeat.Player.trust = 0;
                    mSeat.UpdateTrust();
                    buttonCancelTrust.gameObject.SetActive(false);
                    break;
                case 4:
                    // 其它房间状态查询
                    if (rec.isTrusted == 1)
                    {
                        // UIComponent.Instance.ShowNoAnimation(UIType.UIToast, $"在 {rec.roomName} 房间被托管");
                        UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(20022, new List<object>() { rec.roomName }));
                        break;
                    }
                    SitdownFunc(cacheClientSeatId);
                    break;
                case 5:
                    {
                        // 服务器主动下发其他玩家的托管状态
                        for (int i = 0, n = rec.trustStatus.Count; i < n; i++)
                        {
                            mSeat = GetSeatById((sbyte)i);
                            if (null == mSeat || null == mSeat.Player)
                                continue;

                            mSeat.Player.trust = rec.trustStatus[i];
                            mSeat.UpdateTrust();

                            if (mSeat.seatID == mainPlayer.seatID)
                            {
                                buttonCancelTrust.gameObject.SetActive(mSeat.Player.trust == 1);
                                if (mSeat.Player.trust == 1)
                                {
                                    HideOperationPanel();
                                    HideAutoOperationPanel();
                                    HideWaitBlindBtn();
                                }
                            }
                        }
                    }
                    break;
            }
        }

        protected void HANDLER_REQ_GAME_RECV_PRE_START_GAME(ICPResponse response)
        {
            REQ_GAME_RECV_PRE_START_GAME rec = response as REQ_GAME_RECV_PRE_START_GAME;

        }

        protected void HANDLER_REQ_SHOWDOWN(ICPResponse response)
        {
            REQ_SHOWDOWN rec = response as REQ_SHOWDOWN;
            if (null == rec)
                return;

            if (rec.status != 0)
            {
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_SHOWDOWN, rec.status));
                return;
            }

            showCardsId = rec.pokerstates;

            Seat mSeat = GetSeatById(MainPlayer.seatID);
            if (null == mSeat)
                return;

            mSeat.UpdateShowCardsId();
        }

        protected void HANDLER_REQ_GAME_JACKPOT_SHOW_CARDS(ICPResponse response)
        {
            REQ_GAME_JACKPOT_SHOW_CARDS rec = response as REQ_GAME_JACKPOT_SHOW_CARDS;
            if (null == rec)
                return;

            Seat mSeat = GetSeatById(rec.seatId);
            if (null != mSeat && null != mSeat.Player)
            {
                mSeat.Player.SetCards(new List<sbyte>() { rec.firstCard, rec.secondCard });
                mSeat.UpdateCards();
            }
        }

        protected void HANDLER_REQ_GAME_PLAYER_CARDS(ICPResponse response)
        {
            REQ_GAME_PLAYER_CARDS rec = response as REQ_GAME_PLAYER_CARDS;
            if (null == rec)
                return;

            isAllinGetPlayerCards = true;
            // 保险模式，allin后要收筹码，不用等收到公共牌再收。
            if (insurance == 1 && GetCurPublicCardsCount() > 0)
            {
                PlayRecyclingChipAnimation(null);
            }


            Seat mSeat = null;
            for (int i = 0, n = rec.playerSeat.Count; i < n; i++)
            {
                if (rec.playerSeat[i] == -1)
                    continue;

                mSeat = GetSeatById(rec.playerSeat[i]);
                if (null == mSeat)
                    return;

                mSeat.Player.SetCards(new List<sbyte>() { rec.firstCards[i], rec.secondCards[i] });
                mSeat.UpdateCards();
            }
        }

        protected void HANDLER_REQ_GAME_CONTROL_RANGE(ICPResponse response)
        {
            REQ_GAME_CONTROL_RANGE rec = response as REQ_GAME_CONTROL_RANGE;
            if (null == rec)
                return;

            if (rec.status != 0)
            {
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_GAME_CONTROL_RANGE, rec.status));
                return;
            }

            currentMinRate = rec.minTime;
            currentMaxRate = rec.maxTime;
            // UIComponent.Instance.Toast($"房主设置带入倍数为{rec.minTime}-{rec.maxTime}倍");
            UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(20023, new List<object>() { rec.minTime, rec.maxTime }));
        }

        protected void HANDLER_REQ_GAME_TAKE_CONTROL(ICPResponse response)
        {
            REQ_GAME_TAKE_CONTROL rec = response as REQ_GAME_TAKE_CONTROL;
            if (null == rec)
                return;

            if (rec.status != 0)
            {
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_GAME_TAKE_CONTROL, rec.status));
                return;
            }
        }

        protected virtual void HANDLER_REQ_GAME_ENDING(ICPResponse response)
        {
            REQ_GAME_ENDING rec = response as REQ_GAME_ENDING;
            if (null == rec)
                return;
            if (rec.status != 0)
                return;

            UIComponent.Instance.Show(UIType.UITexasGameEnding, new UITexasGameEndingComponent.GameEndingData
            {
                rec = rec,
                title = GetRoomTypeDes()
            });

            doExitRoom();
        }

        protected void HANDLER_REQ_GAME_START_GAME(ICPResponse response)
        {
            REQ_GAME_START_GAME rec = response as REQ_GAME_START_GAME;
            if (null == rec)
                return;

            if (rec.status != 0)
            {
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_GAME_START_GAME, rec.status));
                return;
            }

            buttonStart.gameObject.SetActive(false);
            buttoninvite.gameObject.SetActive(false);
            imageWaitForStartTips.gameObject.SetActive(false);

            gamestatus = 1;

            if (GameCache.Instance.shortest_time > 0 && UserSitdown())
            {
                //启动最短上桌倒计时
                shortestTime = GameCache.Instance.shortest_time * 60;
                bShortestTimeCounting = true;
            }
        }

        protected void HANDLER_REQ_GAME_ADD_CHIPS(ICPResponse response)
        {
            REQ_GAME_ADD_CHIPS rec = response as REQ_GAME_ADD_CHIPS;
            if (null == rec)
                return;

            if (rec.status != 0)
            {
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_GAME_ADD_CHIPS, rec.status));
                return;
            }

            Seat mSeat = GetSeatById(rec.seatIDRec);
            if (null == mSeat) return;
            mSeat.Player.chips = rec.Chips;
            mSeat.Player.leavelChips = rec.allChips;
            mSeat.Player.leftSecs = rec.leftSecs;
            mSeat.Player.leftCreditValue = rec.leftCreditValue;
            if (mSeat.Player.status == 18)
            {
                mSeat.Player.status = 8;
            }

            if (mainPlayer.seatID == rec.seatIDRec)
            {
                UIComponent.Instance.HideNoAnimation(UIType.UIAddChips);
                bringIn = 1;
                GameCache.Instance.gold = rec.allChips;
            }

            // if (mainPlayer.seatID != rec.seatIDRec)
            // {
            mSeat.FsmLogicComponent.SM.ChangeState(SeatAddChips<Entity>.Instance);
            // }

            mSeat.FsmLogicComponent.SM.ChangeState(SeatWaitStart<Entity>.Instance);
        }

        protected void HANDLER_REQ_GAME_RECV_START_INFOR(ICPResponse response)
        {
            REQ_GAME_RECV_START_INFOR rec = response as REQ_GAME_RECV_START_INFOR;
            if (null == rec)
                return;

            handleRecvStartInfoCommon(rec, response);
        }

        protected virtual void handleRecvStartInfoCommon(REQ_GAME_RECV_START_INFOR rec, object obj)
        {
            gamestatus = 1;

            fuck4thPCardByInsuranceState = 0;

            isAllinGetPlayerCards = false;

            lastBankerIndex = bankerIndex;
            bankerIndex = rec.bankerID;
            bigIndex = rec.bigSeatID;
            smallIndex = rec.smallSeatID;
            operationID = rec.operationID;
            smallBlind = rec.smallChip;
            bigBlind = rec.bigChip;
            minAnteNum = rec.minAnteNum;
            canRaise = rec.canRaise;
            potNumber = rec.potNumber;
            firstBet = rec.firstBet;
            lastPlayerBet = rec.lastPlayerBet;
            round = rec.round;
            callnum = rec.callnum;
            foldnum = rec.foldmum;
            isCurStraddle = rec.isCurStraddle;
            callAmount = rec.callAmount;
            lastCallAmount = callAmount;
            operationRound = rec.operationRound;

            ResetShowCardsId();

            ResetPublicCardsId();
            ClearPublicCardsUI();
            HideWaitBlindBtn();

            bool isStraddleGame = false;
            for (int i = 0, n = listSeat.Count; i < n; i++)
            {
                if (rec.straddlePop[i] > 0)
                {
                    isStraddleGame = true;
                    break;
                }
            }

            Seat mSeat = null;
            for (int i = 0, n = listSeat.Count; i < n; i++)
            {
                mSeat = listSeat[i];
                if (null == mSeat || null == mSeat.Player)
                {
                    continue;
                }

                mSeat.isBank = mSeat.seatID == rec.bankerID;
                mSeat.isBig = mSeat.seatID == rec.bigSeatID;
                mSeat.isSmall = mSeat.seatID == rec.smallSeatID;
                if (roomType == 21 || roomType == 23)
                {
                    mSeat.isBig = false;
                    mSeat.isSmall = false;
                }
                mSeat.isStraddle = rec.straddlePop[mSeat.seatID] > 0;
                mSeat.Player.SetCards(GetHandCardsAtRecvStartInfo(obj, mSeat.seatID));
                mSeat.Player.chips = rec.chipsArray[mSeat.seatID];
                mSeat.Player.canPlayStatus = rec.canPlayStatus[mSeat.seatID];
                mSeat.Player.extraBlind = rec.extraBlind[mSeat.seatID];
                mSeat.Player.isFold = false;
                mSeat.FoldHeadGray(mSeat.Player.isFold);
                if (mSeat.Player.status != 15 && mSeat.Player.status != 18)
                {
                    mSeat.Player.status = 0;
                }

                mSeat.Player.anteNumber = 0;

                if (null != rec.initialBets)
                {
                    mSeat.Player.initialBets = rec.initialBets[mSeat.seatID];
                }

                if (mSeat.Player.initialBets > 0)
                {
                    mSeat.Player.anteNumber += mSeat.Player.initialBets;
                    mSeat.Player.status = 1;
                    alreadAnte += mSeat.Player.initialBets;
                }

                if (mSeat.Player.canPlayStatus == 1)
                {
                    mSeat.FsmLogicComponent.SM.ChangeState(SeatStart<Entity>.Instance);
                }

                if (mSeat.isBig)
                {
                    mSeat.Player.status = 1;
                    mSeat.Player.anteNumber += bigBlind;
                    alreadAnte += bigBlind;
                }
                else if (mSeat.isSmall)
                {
                    mSeat.Player.status = 1;
                    if (mSeat.Player.extraBlind == 1)
                    {
                        int temp = bigBlind;
                        if (isStraddleGame)
                            temp = bigBlind * 2;

                        mSeat.Player.status = 1;
                        mSeat.Player.anteNumber += temp; //补盲玩家需要下大盲,有抓头局补抓头
                        alreadAnte += temp;

                    }
                    else
                    {
                        mSeat.Player.anteNumber += smallBlind;
                        alreadAnte += smallBlind;
                    }
                }
                else
                {
                    if (mSeat.Player.extraBlind == 1)
                    {
                        int temp = bigBlind;
                        if (isStraddleGame)
                            temp = bigBlind * 2;

                        mSeat.Player.status = 1;
                        mSeat.Player.anteNumber += temp; //补盲玩家需要下大盲,有抓头局补抓头
                        alreadAnte += temp;
                    }

                    if (mSeat.isStraddle)
                    {
                        mSeat.Player.anteNumber += rec.straddlePop[mSeat.seatID];
                        alreadAnte += rec.straddlePop[mSeat.seatID];

                        mSeat.Player.status = 10;
                        mSeat.FsmLogicComponent.SM.ChangeState(SeatStraddle<Entity>.Instance);
                    }
                }

                // 增加前注
                if (mSeat.Player.canPlayStatus == 1)
                {
                    alreadAnte += groupBet;
                }

                // 补盲按钮，过庄气泡
                if (mSeat.Player.canPlayStatus == 0 && mSeat.Player.status != 15 && mSeat.Player.status != 18)
                {
                    mSeat.ShowGuozhuangBuuble();
                    if (mSeat.seatID == mainPlayer.seatID)
                    {
                        ShowWaitBlindBtn();
                        if (rec.seatStatus[mSeat.seatID] == 1)
                        {
                            // UIComponent.Instance.Toast("公平起见，轮到你是大盲位时才能加入牌局");
                            //UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(20024));
                        }
                    }
                }
                else
                {
                    if (mSeat.seatID == mainPlayer.seatID)
                    {
                        HideWaitBlindBtn();
                    }
                }
            }

            if (smallIndex >= 0)
            {
                SoundComponent.Instance.PlaySFX(SoundComponent.SFX_DESK_BET_FIRST);
            }

            if (bigIndex >= 0)
            {
                SoundComponent.Instance.PlaySFX(SoundComponent.SFX_DESK_BET_SECOND);
            }

            // 发牌动画
            PlayDealAnimation(() =>
            {
                UpdateAlreadAnte();
                Seat mSeat0 = null;
                for (int i = 0, n = listSeat.Count; i < n; i++)
                {
                    mSeat0 = listSeat[i];
                    if (null == mSeat0 || null == mSeat0.Player || mSeat0.Player.canPlayStatus == 0)
                    {
                        continue;
                    }

                    mSeat0.FsmLogicComponent.SM.ChangeState(SeatStartToPlaying<Entity>.Instance);

                    if (operationID == mSeat0.seatID)
                    {
                        mSeat0.FsmLogicComponent.SM.ChangeState(SeatOperation<Entity>.Instance);
                    }
                    else
                    {
                        mSeat0.FsmLogicComponent.SM.ChangeState(SeatWaitOther<Entity>.Instance);
                    }
                }

                mSeat0 = GetSeatById(operationID);
                Seat mMySeat = GetSeatById(mainPlayer.seatID);

                if (null != mMySeat && mMySeat.seatID == mSeat0.seatID && mMySeat.Player.userID == mSeat0.Player.userID)
                {
                    // 到自己操作
                    HideAutoOperationPanel();
                    if (mMySeat.Player.canPlayStatus == 1 && mMySeat.Player.trust == 0)
                    {
                        ShowOperationPanel(new UIOperationComponent.OperationData()
                        {
                            firstRound = true,
                            minAnteNum = minAnteNum,
                            canRaise = canRaise,
                            callAmount = callAmount
                        });
                    }
                }
                else
                {
                    // 其他人操作
                    HideOperationPanel();
                    if (mMySeat.Player.canPlayStatus == 1)
                    {
                        // 自己参与游戏
                        // 非弃牌 && 非ALLIN && 非托管
                        if (mMySeat.Player.status != 6 && mMySeat.Player.status != 4 && mMySeat.Player.trust == 0)
                        {
                            UIComponent.Instance.ShowNoAnimation(UIType.UIAutoOperation, new UIAutoOperationComponent.AutoOperationData()
                            {
                                callAmount = callAmount
                            });
                        }
                        else
                        {
                            HideAutoOperationPanel();
                        }
                    }
                    else
                    {
                        // 观众
                        HideAutoOperationPanel();
                    }
                }
            });
        }

        protected virtual void HANDLER_REQ_GAME_RECV_READYTIME(ICPResponse response)
        {
            REQ_GAME_RECV_READYTIME rec = response as REQ_GAME_RECV_READYTIME;
            if (null == rec)
                return;
        }

        protected void HANDLER_REQ_GAME_RECV_WINNER(ICPResponse response)
        {
            REQ_GAME_RECV_WINNER rec = response as REQ_GAME_RECV_WINNER;
            if (null == rec)
                return;

            handleWinnerInfoCommon(rec, response);
        }

        protected void handleWinnerInfoCommon(REQ_GAME_RECV_WINNER rec, object obj)
        {
            autoFold = false;
            autoCall = false;
            autoAllin = false;
            autoCheck = false;

            gamestatus = -1;

            if (UIComponent.Instance.Get(UIType.UIAutoOperation).GameObject.activeInHierarchy)
                UIComponent.Instance.HideNoAnimation(UIType.UIAutoOperation);
            if (UIComponent.Instance.Get(UIType.UIOperation).GameObject.activeInHierarchy)
                UIComponent.Instance.HideNoAnimation(UIType.UIOperation);

            GameCache.Instance.CurGame.mainPlayer.chips = rec.selfChips;

            if (mainPlayer.canPlayStatus == 1)
                ShowSeeMorePublic();

            ClearSeatBubble(true);
            SetPublicCardInfosId();

            Seat mSeat = null;

            // 1.发完5张公共牌
            // 2.有发生比牌
            bool mOtherAllFold = true;
            for (int i = 0, n = listSeat.Count; i < n; i++)
            {
                mSeat = listSeat[i];
                if (null != mSeat && null != mSeat.Player && mSeat.Player.canPlayStatus == 1 && mSeat.Player.isFold == false)
                {
                    mOtherAllFold = false;
                }

                //先更新手牌，方便后面做大牌动画
                if(null == mSeat || null == mSeat.Player || rec.allplayerID[i] != mSeat.Player.userID)
                    continue;
                if (!mSeat.IsMySeat)
                {
                    //自己的牌不用更新
                    mSeat.Player.SetCards(GetHandCardsAtRecvWinner(obj, i));
                }

                bool mShow = false;
                for (int j = 0, k = mSeat.Player.cards.Count; j < k; j++)
                {
                    if (mSeat.Player.cards[j] != -1)
                    {
                        mShow = true;
                        break;
                    }
                }
                if (!mShow)
                {
                    if (!mSeat.Player.isFold)
                        mSeat.UpdateCards();
                }
                else
                {
                    mSeat.UpdateCards();
                }

            }

            sequencePlayEndPublicCardsAnimation = DOTween.Sequence();

            sbyte mSeatId = -1;
            for (int i = 0, n = rec.seatIDArray.Count; i < n; i++)
            {
                mSeatId = rec.seatIDArray[i];

                mSeat = GetSeatById(mSeatId);
                if (null == mSeat || null == mSeat.Player || rec.playerArray[i] != mSeat.Player.userID)
                    continue;

                mSeat.Player.recyclingChip = rec.chipsArray[i]; // 收筹码数量
                mSeat.Player.cardType = rec.cardTypesArray[i];
                mSeat.Player.isWin = rec.isWinArray[i];
                mSeat.Player.winChips = rec.winChipsArray[i];
                mSeat.Player.jackPotUp = rec.jackPotUpArray != null ? rec.jackPotUpArray[i] : 0;
                mSeat.Player.hitJackPotChips = rec.hitJackPotChipsArray != null ? rec.hitJackPotChipsArray[i] : 0;

            }

            int mCount = GetCurPublicCardsCount();
            bool mCanPlayEndPublicCardsAnimation = mCount == 5 && !mOtherAllFold;
            if (mCanPlayEndPublicCardsAnimation)
            {
                PlayEndPublicCardsAnimation(rec);
            }

            Sequence mSequence = null;
            if (mCanPlayEndPublicCardsAnimation)
                mSequence = sequencePlayEndPublicCardsAnimation;
            bool mIsFirst = true;
            for (int i = 0, n = rec.allplayerID.Count; i < n; i++)
            {
                mSeat = GetSeatById((sbyte)i);

                if (null == mSeat || null == mSeat.Player || rec.allplayerID[i] != mSeat.Player.userID)
                    continue;

                mSeat.Player.chips = rec.allPlayerchip[i];
                mSeat.Player.muckStatus = rec.muckStatus != null ? rec.muckStatus[i] : (sbyte)0;
                mSeat.Player.anteNumber = 0;
                mSeat.Player.isMaxcard = rec.isMaxcardArray[i];
                if (mSeat.Player.muckStatus == 1)
                {
                    //盖牌
                    mSeat.Player.status = 11;
                    mSeat.UpdateBubble();
                }

                mSeat.UpdateCoin();
                // mSeat.UpdateCards();

                if (mCanPlayEndPublicCardsAnimation)
                {
                    if (mSeat.CanPlayRecyclingWinChipAnimation)
                    {
                        if (mIsFirst)
                        {
                            mIsFirst = false;
                            mSequence.Append(mSeat.PlayRecyclingChipAnimation(0));
                        }
                        else
                        {
                            mSequence.Join(mSeat.PlayRecyclingChipAnimation(0));
                        }
                    }
                }
                else
                {
                    if (mSeat.CanPlayRecyclingWinChipAnimation)
                        mSequence = mSeat.PlayRecyclingChipAnimation(0);
                }
            }

            if (null == mSequence)
                mSequence = DOTween.Sequence();

            // 配合收筹码后刷新分池UI
            // if (null != mSequence && GetCurPublicCardsCount() > 0)
            // {
            //     mSequence.AppendCallback(UpdatePots);
            // }

            mIsFirst = true;
            for (int i = 0, n = rec.seatIDArray.Count; i < n; i++)
            {
                mSeatId = rec.seatIDArray[i];

                mSeat = GetSeatById(mSeatId);
                if (null == mSeat || null == mSeat.Player || rec.playerArray[i] != mSeat.Player.userID)
                    continue;

                //mSeat.Player.recyclingChip = rec.chipsArray[i]; // 收筹码数量
                //mSeat.Player.cardType = rec.cardTypesArray[i];
                //mSeat.Player.isWin = rec.isWinArray[i];
                //mSeat.Player.winChips = rec.winChipsArray[i];
                //mSeat.Player.jackPotUp = rec.jackPotUpArray != null ? rec.jackPotUpArray[i] : 0;
                //mSeat.Player.hitJackPotChips = rec.hitJackPotChipsArray != null ? rec.hitJackPotChipsArray[i] : 0;

                // mSeat.UpdateWinCoin();
                mSeat.StopAllinArmature();
                mSeat.PlayWinArmature();
                mSeat.UpdateRecyclingWinChip();

                if (mCanPlayEndPublicCardsAnimation)
                {
                    if (mIsFirst)
                    {
                        mIsFirst = false;
                        mSequence.Append(mSeat.PlayRecyclingWinChipAnimation(rc.transform.TransformPoint(textAlreadAnte.transform.localPosition)));
                    }
                    else
                    {
                        mSequence.Join(mSeat.PlayRecyclingWinChipAnimation(rc.transform.TransformPoint(textAlreadAnte.transform.localPosition)));
                    }
                }
                else
                {
                    if (mIsFirst)
                    {
                        mIsFirst = false;
                        mSequence.Append(mSeat.PlayRecyclingWinChipAnimation(rc.transform.TransformPoint(textAlreadAnte.transform.localPosition)));
                    }
                    else
                    {
                        mSequence.Join(mSeat.PlayRecyclingWinChipAnimation(rc.transform.TransformPoint(textAlreadAnte.transform.localPosition)));
                    }
                    // mSeat.PlayRecyclingWinChipAnimation(rc.transform.TransformPoint(textAlreadAnte.transform.localPosition));
                }
            }

            // 配合收筹码后刷新分池UI
            if (null != mSequence && GetCurPublicCardsCount() > 0)
            {
                mSequence.OnComplete(UpdatePots);
            }
        }

        protected virtual void HANDLER_REQ_GAME_RECV_CARDS(ICPResponse response)
        {
            REQ_GAME_RECV_CARDS rec = response as REQ_GAME_RECV_CARDS;
            if (null == rec)
                return;

            UIComponent.Instance.HideNoAnimation(UIType.UIInsurance);

            minAnteNum = rec.minAnteNum;
            canRaise = rec.canRaise;
            potNumber = rec.potNumber;
            seeCardPlayerName = rec.seeMoreCardsPlayer;
            operationRound = rec.operationRound;
            callAmount = 0;
            lastCallAmount = 0;

            firstBet = 0;
            lastPlayerBet = -1;
            round = rec.round;
            callnum = rec.callnum;
            foldnum = rec.foldmum;
            autoCall = false;
            autoAllin = false;
            autoCheck = false;
            autoFold = false;

            int iCount = GetCurPublicCardsCount();  // 要在更新公共牌前拿数量
            AddPublicCards(rec.systemIDArray);

            // 花费查看未发公共牌
            if (!string.IsNullOrEmpty(rec.seeMoreCardsPlayer))
            {
                UpdatePublicCardsNoAnim();
                if (iCount < 3)
                {
                    // ShowSeeMorePublicTips($"{rec.seeMoreCardsPlayer}查看翻牌圈的牌");
                    ShowSeeMorePublicTips($"{rec.seeMoreCardsPlayer}{CPErrorCode.LanguageDescription(20025)}");
                }
                else if (iCount == 3)
                {
                    // ShowSeeMorePublicTips($"{rec.seeMoreCardsPlayer}查看转牌圈的牌");
                    ShowSeeMorePublicTips($"{rec.seeMoreCardsPlayer}{CPErrorCode.LanguageDescription(20026)}");
                }
                else if (iCount == 4)
                {
                    // ShowSeeMorePublicTips($"{rec.seeMoreCardsPlayer}查看河牌圈的牌");
                    ShowSeeMorePublicTips($"{rec.seeMoreCardsPlayer}{CPErrorCode.LanguageDescription(20027)}");
                }

                if (GetCurPublicCardsCount() == 5)
                {
                    HideSeeMorePublic();
                }
                return;
            }

            ClearSeatBubble(false);

            Seat mCacheSeat = null;
            for (int i = 0, n = listSeat.Count; i < n; i++)
            {
                mCacheSeat = listSeat[i];
                if (null == mCacheSeat || null == mCacheSeat.Player || mCacheSeat.Player.canPlayStatus == 0)
                    continue;

                mCacheSeat.Player.anteNumber = 0;

                if (mCacheSeat.seatID == rec.seatID)
                    continue;
                mCacheSeat.FsmLogicComponent.SM.ChangeState(SeatWaitOther<Entity>.Instance);
            }

            sbyte opSeatID = rec.seatID;
            TweenCallback mTweenCallback = () =>
            {
                Seat mSeat = GetSeatById(opSeatID);
                if (null == mSeat)
                    return;

                if (mSeat.seatID == mainPlayer.seatID && mSeat.Player.userID == mainPlayer.userID && mSeat.Player.canPlayStatus == 1)
                {
                    // 到自己操作
                    HideAutoOperationPanel();

                    // 非托管
                    if (mainPlayer.trust == 0)
                    {
                        ShowOperationPanel(new UIOperationComponent.OperationData()
                        {
                            firstRound = false,
                            minAnteNum = minAnteNum,
                            canRaise = canRaise,
                            callAmount = callAmount
                        });
                    }
                }
                else
                {
                    // 下一个操作不是自己
                    HideOperationPanel();

                    // 自己有参与游戏
                    if (mainPlayer.canPlayStatus == 1)
                    {
                        // 非弃牌、非ALL IN、非空闲等待下一局、非托管
                        if (mainPlayer.status != 6 && mainPlayer.status != 4 && mainPlayer.status != 8 && mainPlayer.trust == 0)
                        {
                            // 预操作UI
                            UIComponent.Instance.ShowNoAnimation(UIType.UIAutoOperation, new UIAutoOperationComponent.AutoOperationData()
                            {
                                callAmount = callAmount
                            });
                        }
                        else
                        {
                            // 无预操作UI
                            HideAutoOperationPanel();
                        }
                    }
                    else
                    {
                        // 观众
                        HideAutoOperationPanel();
                    }
                }
                mSeat.FsmLogicComponent.SM.ChangeState(SeatOperation<Entity>.Instance);
            };

            stopUpdatePublicCardsAnimation = false;
            if (iCount == 0)
            {
                PlayFirstRecyclingChipAnimation(() =>
                {
                    PlayFirstRecyclingChipSubAnimation(() =>
                    {
                        UpdatePublicCards(iCount, mTweenCallback);
                        if (stopUpdatePublicCardsAnimation && null != sequenceUpdatePublicCards && sequenceUpdatePublicCards.IsPlaying())
                        {
                            sequenceUpdatePublicCards.Complete(true);
                        }

                        stopUpdatePublicCardsAnimation = false;
                    });
                    if (stopUpdatePublicCardsAnimation && null != sequencePlayFirstRecyclingChipSubAnimation &&
                        sequencePlayFirstRecyclingChipSubAnimation.IsPlaying())
                    {
                        sequencePlayFirstRecyclingChipSubAnimation.Complete(true);
                    }
                });
            }
            else
            {
                if (isAllinGetPlayerCards && insurance == 1)
                {
                    // 保险就是多事，特殊处理一下。来了三张公共牌，动画播放中，没有保险可买，马上又来了一张公共牌。
                    UpdatePublicCards(iCount, mTweenCallback);
                    if (stopUpdatePublicCardsAnimation && null != sequenceUpdatePublicCards && sequenceUpdatePublicCards.IsPlaying())
                    {
                        sequenceUpdatePublicCards.Complete(true);
                    }

                    stopUpdatePublicCardsAnimation = false;
                }
                else
                {
                    PlayRecyclingChipAnimation(() =>
                    {
                        UpdatePublicCards(iCount, mTweenCallback);
                        if (stopUpdatePublicCardsAnimation && null != sequenceUpdatePublicCards && sequenceUpdatePublicCards.IsPlaying())
                        {
                            sequenceUpdatePublicCards.Complete(true);
                        }

                        stopUpdatePublicCardsAnimation = false;
                    });
                }
            }
        }

        protected virtual async void HANDLER_REQ_GAME_RECV_SEAT_DOWN(ICPResponse response)
        {
            REQ_GAME_RECV_SEAT_DOWN rec = response as REQ_GAME_RECV_SEAT_DOWN;
            if (null == rec)
                return;

            Seat mSeat = GetSeatById(rec.seatID);
            if (null == mSeat)
                return;

            if (null != mSeat.Player)
            {
                mSeat.Player.Dispose();
                mSeat.Player = null;
            }

            Player mPlayer = ComponentFactory.CreateWithId<Player>(rec.userID);
            mPlayer.seatID = mSeat.seatID;
            mPlayer.sex = rec.sex;
            mPlayer.headPic = rec.headPic;
            mPlayer.nick = rec.nick;
            mPlayer.userID = rec.userID;
            mPlayer.chips = rec.chips;
            mPlayer.canPlayStatus = 0;
            mPlayer.status = mPlayer.chips <= GetMinPlayChips() ? 18 : 0;
            mPlayer.ante = 0;
            mPlayer.trust = 0;
            mPlayer.cards = GetEnptyHandCards();
            mSeat.Player = mPlayer;
            mSeat.isBank = false;

            // mSeat.FsmLogicComponent.SM.ChangeState(SeatSit<Entity>.Instance);
            mSeat.FsmLogicComponent.SM.ChangeState(SeatSitAnimation<Entity>.Instance);
            await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(300);
            mSeat.FsmLogicComponent.SM.ChangeState(SeatWaitStart<Entity>.Instance);
        }

        protected void HANDLER_REQ_GAME_RECV_LEAVE(ICPResponse response)
        {
            REQ_GAME_RECV_LEAVE rec = response as REQ_GAME_RECV_LEAVE;
            if (null == rec)
                return;

            Seat mSeat = GetSeatById(rec.seatID);
            if (null == mSeat)
                return;

            if (mSeat.Player.userID == mainPlayer.userID)
            {
                HideOperationPanel();
                HideAutoOperationPanel();
            }

            //离开座位时取消其语音内容
            if (uiChat.IsPlayingUserVoice(mSeat.Player.userID.ToString()))
            {
                uiChat.StopVoice();
                uiChat.PlayVoiceNext();
            }

            if (null != mSeat.Player)
            {
                mSeat.Player.Dispose();
                mSeat.Player = null;
            }

            mSeat.FsmLogicComponent.SM.ChangeState(SeatStandupAnimation<Entity>.Instance);
            // mSeat.FsmLogicComponent.SM.ChangeState(SeatStandup<Entity>.Instance);
            // mSeat.FsmLogicComponent.SM.ChangeState(SeatEmpty<Entity>.Instance);
        }

        protected void HANDLER_REQ_GAME_RECV_ACTION(ICPResponse response)
        {
            REQ_GAME_RECV_ACTION rec = response as REQ_GAME_RECV_ACTION;
            if (null == rec)
                return;

            Seat mSeat = GetSeatById(rec.seatID);
            if (null == mSeat)
            {
                Game.EventSystem.Run(EventIdType.GameErrorReconnect);
                return;
            }

            if (mSeat.Player == null)
            {
                return;
            }

            stopUpdatePublicCardsAnimation = false;
            if (null != sequencePlayFirstRecyclingChipAnimation && sequencePlayFirstRecyclingChipAnimation.IsPlaying())
            {
                stopUpdatePublicCardsAnimation = true;
                sequencePlayFirstRecyclingChipAnimation.Complete(true);

            }
            if (null != sequencePlayRecyclingChipAnimation && sequencePlayRecyclingChipAnimation.IsPlaying())
            {
                stopUpdatePublicCardsAnimation = true;
                sequencePlayRecyclingChipAnimation.Complete(true);

            }
            if (null != sequenceUpdatePublicCards && sequenceUpdatePublicCards.IsPlaying())
            {
                sequenceUpdatePublicCards.Complete(true);
            }


            operationID = rec.operationID;
            alreadAnte += rec.anteNumber;
            minAnteNum = rec.minAnteNum;
            canRaise = rec.canRaise;
            callAmount = rec.callAmount;
            operationRound = rec.operationRound;
            potNumber = rec.potNumber;

            firstBet = rec.firstBet;
            lastPlayerBet = rec.lastPlayerBet;
            round = rec.round;
            callnum = rec.callnum;
            foldnum = rec.foldmum;
            UpdateAlreadAnte();

            mSeat.Player.status = rec.action;
            mSeat.Player.chips -= rec.anteNumber;
            mSeat.Player.anteNumber += rec.anteNumber;

            // 下注putchip = 1,跟注call = 2,加注raise = 3,全下allin = 4,让牌check = 5,弃牌fold = 6,超时timeout = 7
            switch (rec.action)
            {
                case 1:
                    //mSeat.FsmLogicComponent.SM.ChangeState(SeatCall<Entity>.Instance);
                    break;
                case 2:
                    mSeat.FsmLogicComponent.SM.ChangeState(SeatCall<Entity>.Instance);
                    break;
                case 3:
                    mSeat.FsmLogicComponent.SM.ChangeState(SeatRaise<Entity>.Instance);
                    break;
                case 4:
                    mSeat.FsmLogicComponent.SM.ChangeState(SeatAllin<Entity>.Instance);
                    break;
                case 5:
                    // 其他玩家托管状态，发一牌就check
                    if (null != sequencePlayDealAnimation && sequencePlayDealAnimation.IsPlaying())
                    {
                        sequencePlayDealAnimation.Complete(true);
                    }
                    mSeat.FsmLogicComponent.SM.ChangeState(SeatCheck<Entity>.Instance);
                    break;
                case 6:
                    // 其他玩家托管状态，发一牌就弃牌
                    mSeat.Player.isFold = true;
                    if (null != sequencePlayDealAnimation && sequencePlayDealAnimation.IsPlaying())
                    {
                        sequencePlayDealAnimation.Complete(true);
                    }
                    mSeat.FsmLogicComponent.SM.ChangeState(SeatFold<Entity>.Instance);
                    //if (mSeat.Player.userID == mainPlayer.userID)
                    //{
                    //    //弃牌，取消牌型高亮
                    //    for (int i = 0, n = listCards.Count; i < n; i++)
                    //    {
                    //        listCards[i].imageSelect.gameObject.SetActive(false);
                    //    }
                    //}
                    break;
                case 7:
                    mSeat.FsmLogicComponent.SM.ChangeState(SeatTimeout<Entity>.Instance);
                    break;
            }
            mSeat.FsmLogicComponent.SM.ChangeState(SeatWaitOther<Entity>.Instance);

            if (callAmount > lastCallAmount)
            {
                autoCall = false;
                autoCheck = false;
            }
            lastCallAmount = callAmount;

            if (operationID != -1)
            {
                mSeat = GetSeatById(operationID);
                if (null == mSeat || null == mSeat.Player)
                {
                    Game.EventSystem.Run(EventIdType.GameErrorReconnect);
                    return;
                }

                if (mSeat.seatID == mainPlayer.seatID && mSeat.Player.userID == mainPlayer.userID && mainPlayer.canPlayStatus == 1)
                {
                    // 到自己操作
                    // 自动
                    HideAutoOperationPanel();
                    if (autoFold || autoCheck || autoCall || autoAllin)
                    {
                        HideOperationPanel();
                        if (autoFold)
                        {
                            if (callAmount == 0)
                            {
                                // 看牌
                                GameCache.Instance.CurGame.OptAction(5, 0);
                            }
                            else
                            {
                                // 弃牌
                                GameCache.Instance.CurGame.OptAction(6, 0);
                            }
                        }
                        if (autoCheck)
                            GameCache.Instance.CurGame.OptAction(5, 0);
                        if (autoCall)
                        {
                            if (rec.callAmount >= GameCache.Instance.CurGame.MainPlayer.chips)
                            {
                                GameCache.Instance.CurGame.OptAction(4, GameCache.Instance.CurGame.MainPlayer.chips);
                            }
                            else
                            {
                                GameCache.Instance.CurGame.OptAction(2, rec.callAmount);
                            }
                        }
                        if (autoAllin)
                        {
                            GameCache.Instance.CurGame.OptAction(4, GameCache.Instance.CurGame.MainPlayer.chips);
                        }
                    }
                    else
                    {
                        ShowOperationPanel(new UIOperationComponent.OperationData()
                        {
                            firstRound = false,
                            minAnteNum = minAnteNum,
                            canRaise = canRaise,
                            callAmount = callAmount
                        });
                    }
                }
                else
                {
                    // 下一个操作不是自己
                    HideOperationPanel();

                    if (mainPlayer.canPlayStatus == 1)
                    {
                        // 自己有参与游戏
                        if ((mainPlayer.status >= 0 && mainPlayer.status < 6 && mainPlayer.status != 4 || mainPlayer.status == 10) && mainPlayer.trust == 0)
                        {
                            UIComponent.Instance.ShowNoAnimation(UIType.UIAutoOperation, new UIAutoOperationComponent.AutoOperationData()
                            {
                                callAmount = callAmount
                            });
                        }
                        else
                        {
                            HideAutoOperationPanel();
                        }
                    }
                    else
                    {
                        // 观众
                        HideAutoOperationPanel();
                    }
                }

                mSeat.FsmLogicComponent.SM.ChangeState(SeatOperation<Entity>.Instance);
            }
            else
            {
                HideOperationPanel();
                UIComponent.Instance.HideNoAnimation(UIType.UIAutoOperation);
            }
        }

        protected void HANDLER_REQ_GAME_SEND_ACTION(ICPResponse response)
        {
            REQ_GAME_SEND_ACTION rec = response as REQ_GAME_SEND_ACTION;
            if (null == rec)
                return;

            autoFold = false;
            autoCall = false;
            autoAllin = false;
            autoCheck = false;

            if (rec.status != 0)
            {
                // UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_GAME_SEND_ACTION, rec.status));
                return;
            }

            HideOperationPanel();

            alreadAnte += rec.anteNumber;
            UpdateAlreadAnte();

            Seat mSeat = GetSeatById(mainPlayer.seatID);
            if (null == mSeat)
                return;

            mSeat.Player.status = cacheMainPlayerStatus;

            // 下注putchip = 1,跟注call = 2,加注raise = 3,全下allin = 4,让牌check = 5,弃牌fold = 6,超时timeout = 7
            switch (mSeat.Player.status)
            {
                case 1:
                    mSeat.FsmLogicComponent.SM.ChangeState(SeatPutChip<Entity>.Instance);
                    break;
                case 2:
                    mSeat.FsmLogicComponent.SM.ChangeState(SeatCall<Entity>.Instance);
                    break;
                case 3:
                    mSeat.FsmLogicComponent.SM.ChangeState(SeatRaise<Entity>.Instance);
                    break;
                case 4:
                    mSeat.FsmLogicComponent.SM.ChangeState(SeatAllin<Entity>.Instance);
                    break;
                case 5:
                    mSeat.FsmLogicComponent.SM.ChangeState(SeatCheck<Entity>.Instance);
                    break;
                case 6:
                    mSeat.Player.isFold = true;
                    mSeat.FsmLogicComponent.SM.ChangeState(SeatFold<Entity>.Instance);
                    break;
                case 7:
                    mSeat.FsmLogicComponent.SM.ChangeState(SeatTimeout<Entity>.Instance);
                    break;
            }

            mSeat.FsmLogicComponent.SM.ChangeState(SeatWaitOther<Entity>.Instance);
        }

        protected void HANDLER_REQ_GAME_SEND_SEAT_ACTION(ICPResponse response)
        {
            REQ_GAME_SEND_SEAT_ACTION rec = response as REQ_GAME_SEND_SEAT_ACTION;
            if (rec.status != 0)
            {
                // todo 统一错误码
                // string mErrMsg = $"未知错误{rec.status}:{rec.actionRec}";
                string mErrMsg = CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_GAME_SEND_SEAT_ACTION, rec.status);
                switch (rec.status)
                {
                    case 1:
                        // mErrMsg = "座位已被别的玩家坐下";
                        break;
                    case 2:
                        // mErrMsg = "有相同IP玩家";
                        break;
                    case 3:
                        // mErrMsg = "游戏即将结束";
                        break;
                    case 4:
                        // mErrMsg = "信用额度不足";
                        break;
                    case 5:
                        // mErrMsg = "正在游戏中，无法站起";
                        break;
                    case 6:
                        // mErrMsg = $"未知错误[6]";
                        break;
                    case 7:
                        // mErrMsg = "有GPS位置相近玩家";
                        break;
                    case 8:
                        // mErrMsg = $"未知错误[8]";
                        break;
                    case 9:
                        if (rec.actionRec == 4)
                            // mErrMsg = "强制站起玩家失败";
                            mErrMsg = CPErrorCode.LanguageDescription(10034);
                        else if (rec.actionRec == 5)
                            // mErrMsg = "强制踢出玩家失败";
                            mErrMsg = CPErrorCode.LanguageDescription(10035);
                        break;
                    case 10:
                        if (rec.actionRec == 4)
                            // mErrMsg = "强制站起玩家成功";
                            mErrMsg = CPErrorCode.LanguageDescription(10036);
                        else if (rec.actionRec == 5)
                            // mErrMsg = "强制踢出玩家成功";
                            mErrMsg = CPErrorCode.LanguageDescription(10037);
                        break;
                    case 11:
                        if (UserSitdown())
                        {
                            // mErrMsg = "你将在这手牌结束后提前离桌";
                            mErrMsg = CPErrorCode.LanguageDescription(10038);
                        }
                        else
                        {
                            // mErrMsg = "提前离桌成功";
                            mErrMsg = CPErrorCode.LanguageDescription(10039);
                        }
                        preLeave = 1;
                        UpdateMenu();
                        break;
                    case 12:
                        // mErrMsg = "坐下失败，已提前离桌";
                        break;
                    case 13:
                        // mErrMsg = "你已被冻结";
                        mainPlayer.chips = rec.tableChip;
                        mainPlayer.leavelChips = rec.leavelChips;
                        GameCache.Instance.gold = rec.leavelChips;
                        doExitRoom();
                        Game.EventSystem.Run(EventIdType.AccountFreeze);
                        break;
                }

                UIComponent.Instance.Toast(mErrMsg);
                return;
            }

            mainPlayer.chips = rec.tableChip;
            mainPlayer.leavelChips = rec.leavelChips;
            GameCache.Instance.gold = rec.leavelChips;

            //  1 坐下 2 站起  3 离开房间 4被强制站起 5被强制踢出 7被管理员踢出战队，强制站起
            Seat mSeat = null;
            switch (rec.actionRec)
            {
                case 1:
                    if (HasStarted())
                    {
                        if (GameCache.Instance.shortest_time > 0 && (bFirstSit || bStandBeforeStart || bLoseAll))
                        {
                            bShortestTimeCounting = true; //启动最短上桌时间倒计时
                            if (bLoseAll)
                            {
                                bLoseAll = false;
                            }
                            else
                            {
                                shortestTime = GameCache.Instance.shortest_time * 60;
                            }
                        }
                    }
                    bFirstSit = false;
                    bStandBeforeStart = false;

                    if (!mainPlayer.isPlaying)
                    {
                        mainPlayer.status = mainPlayer.chips <= GetMinPlayChips() ? 18 : 0;
                        mainPlayer.canPlayStatus = 0;
                        mainPlayer.trust = 0;
                        mainPlayer.ante = 0;
                        mainPlayer.anteNumber = 0;
                        mainPlayer.cards = GetEnptyHandCards();
                    }


                    GameCache.Instance.gold = rec.leavelChips;
                    mSeat = GetSeatById(rec.seatId);
                    if (null == mSeat)
                        break;

                    mainPlayer.seatID = rec.seatId;
                    mSeat.Player = mainPlayer;
                    mSeat.isBank = false;

                    HideWaitBlindBtn();
                    // 带入回调之后，服务器会再发送一次坐下成功，获取是否补盲
                    if ((mSeat.Player.chips > GetMinPlayChips() || mainPlayer.isPlaying) && !mSeat.FsmLogicComponent.SM.IsInState(SeatEmpty<Entity>.Instance))
                    {
                        if (rec.waitBlind == 1)
                        {
                            // 需要补盲
                            ShowWaitBlindBtn();
                            mSeat.FsmLogicComponent.SM.ChangeState(SeatWaitBlind<Entity>.Instance);
                        }
                        else
                        {
                            mSeat.FsmLogicComponent.SM.ChangeState(SeatWaitStart<Entity>.Instance);
                        }
                        break;
                    }

                    // mSeat.FsmLogicComponent.SM.ChangeState(SeatSit<Entity>.Instance);
                    mSeat.FsmLogicComponent.SM.ChangeState(SeatSitAnimation<Entity>.Instance);

                    // todo 这里要搞十分十分十分酷炫的动画，把自己位移到最下方，0号位
                    if (mSeat.ClientSeatId > 0)
                    {
                        ResetSeatUIInfo(mSeat.ClientSeatId);
                    }

                    // await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(500);

                    if (mainPlayer.chips <= GetMinPlayChips())
                    {
                        // 弹代入框
                        UIComponent.Instance.Show(UIType.UIAddChips, new UIAddChipsComponent.AddClipsData()
                        {
                            qianzhu = groupBet,
                            bigBlind = bigBlind,
                            smallBlind = smallBlind,
                            currentMinRate = currentMinRate,
                            currentMaxRate = currentMaxRate,
                            totalCoin = GameCache.Instance.gold,
                            tableChips = mainPlayer.chips,
                            isNeedCost = roomMode == 0 ? 1 : 0
                        });
                    }
                    else
                    {
                        if (rec.waitBlind == 1)
                        {
                            // 需要补盲
                            ShowWaitBlindBtn();
                            mSeat.FsmLogicComponent.SM.ChangeState(SeatWaitBlind<Entity>.Instance);
                        }
                        else
                        {
                            mSeat.FsmLogicComponent.SM.ChangeState(SeatWaitStart<Entity>.Instance);
                        }
                    }
                    break;
                case 2:
                    doStandUp(rec.seatID);
                    break;
                case 3:
                    doExitRoom();
                    break;
                case 4:
                    UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                    {
                        type = UIDialogComponent.DialogData.DialogType.Commit,
                        // title = $"温馨提示",
                        title = CPErrorCode.LanguageDescription(10007),
                        // content = $"您被房主强制站起",
                        content = CPErrorCode.LanguageDescription(10041),
                        // contentCommit = "知道了",
                        contentCommit = CPErrorCode.LanguageDescription(10024),
                        contentCancel = "",
                        actionCommit = null,
                        actionCancel = null
                    });
                    doStandUp(rec.seatID);

                    break;
                case 5:
                    UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                    {
                        type = UIDialogComponent.DialogData.DialogType.Commit,
                        // title = $"温馨提示",
                        title = CPErrorCode.LanguageDescription(10007),
                        // content = $"您被房主强制踢出",
                        content = CPErrorCode.LanguageDescription(10042),
                        // contentCommit = "知道了",
                        contentCommit = CPErrorCode.LanguageDescription(10024),
                        contentCancel = "",
                        actionCommit = null,
                        actionCancel = null
                    });
                    doExitRoom();
                    break;
                case 7:
                    UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                    {
                        type = UIDialogComponent.DialogData.DialogType.Commit,
                        // title = $"温馨提示",
                        title = CPErrorCode.LanguageDescription(10007),
                        // content = $"您被管理员踢出战队，强制站起",
                        content = CPErrorCode.LanguageDescription(20028),
                        // contentCommit = "知道了",
                        contentCommit = CPErrorCode.LanguageDescription(10024),
                        contentCancel = "",
                        actionCommit = null,
                        actionCancel = null
                    });
                    doStandUp(rec.seatID);
                    break;
                case 9:
                    UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                    {
                        type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                        //title = $"温馨提示",
                        title = CPErrorCode.LanguageDescription(10025),
                        //content = $"你的余额不足带入，请先充值",
                        content = CPErrorCode.LanguageDescription(20010),
                        //contentCommit = "去充值",
                        contentCommit = CPErrorCode.LanguageDescription(10026),
                        // contentCancel = "取消",
                        contentCancel = CPErrorCode.LanguageDescription(10013),
                        actionCommit = () =>
                        {
                            UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletMy);
                        },
                        actionCancel = null
                    });
                    doStandUp(rec.seatID);
                    break;
            }
        }

        private void HANDLER_REQ_GAME_JACKPOT_HIT_BROADCAST(ICPResponse response)
        {
            REQ_GAME_JACKPOT_HIT_BROADCAST rec = response as REQ_GAME_JACKPOT_HIT_BROADCAST;
            if (null == rec)
                return;

            if (rec.status != 0)
                return;

            if (GameCache.Instance.room_id == rec.roomid)
            {
                Seat seat = GetSeatById(rec.seatIndex);
                if (null != seat)
                {
                    onHitJackPot(seat);
                }
            }



        }
    }
}
