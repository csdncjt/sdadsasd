using System;
using System.Collections.Generic;

namespace ETHotfix
{
    public partial class MTTGame : TexasGame
    {
        protected override void registerHandler()
        {
            base.registerHandler();
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_MTT_Self_Ranking, HANDLER_REQ_GAME_MTT_Self_Ranking);  // 
            // CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_MTT_REAL_TIME, HANDLER_REQ_GAME_MTT_REAL_TIME);  // MTT实时战况
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_MTT_SYSTEM_NOTIFY, HANDLER_REQ_GAME_MTT_SYSTEM_NOTIFY);  // MTT牌局内各种系统消息
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_MTT_GAME_REBUY_REQUEST, HANDLER_REQ_MTT_GAME_REBUY_REQUEST);  // MTT重购申请
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_MTT_FINAL_RANK, HANDLER_REQ_GAME_MTT_FINAL_RANK);  // MTT结束游戏通知名次  (收到该消息，如果没有重购机会，则弹出游戏结束排名页。如果有重购机会，则弹出重购框)
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_MTT_GAME_REBUY_CANCEL, HANDLER_REQ_MTT_GAME_REBUY_CANCEL);  // MTT取消重购  (由175协议触发的重购提示框，点取消时会调这个协议)
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_MTT_START_GAME, HANDLER_REQ_GAME_MTT_START_GAME);  // mtt比赛开始的消息
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_MTT_RECV_START_INFOR, HANDLER_REQ_GAME_MTT_RECV_START_INFOR);  // mtt每局开局前的消息
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_MTT_AUTO_OP, HANDLER_REQ_GAME_MTT_AUTO_OP);  // MTT托管
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_MTT_ADD_TIME, HANDLER_REQ_MTT_ADD_TIME);  // MTT操作加时
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_MTT_GAME_DISMISS, HANDLER_REQ_MTT_GAME_DISMISS);  //MTT解散
        }

        protected override void removeHandler()
        {
            base.removeHandler();
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_MTT_Self_Ranking, HANDLER_REQ_GAME_MTT_Self_Ranking);
            // CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_MTT_REAL_TIME, HANDLER_REQ_GAME_MTT_REAL_TIME);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_MTT_SYSTEM_NOTIFY, HANDLER_REQ_GAME_MTT_SYSTEM_NOTIFY);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_MTT_GAME_REBUY_REQUEST, HANDLER_REQ_MTT_GAME_REBUY_REQUEST);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_MTT_FINAL_RANK, HANDLER_REQ_GAME_MTT_FINAL_RANK);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_MTT_GAME_REBUY_CANCEL, HANDLER_REQ_MTT_GAME_REBUY_CANCEL);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_MTT_START_GAME, HANDLER_REQ_GAME_MTT_START_GAME);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_MTT_RECV_START_INFOR, HANDLER_REQ_GAME_MTT_RECV_START_INFOR);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_MTT_AUTO_OP, HANDLER_REQ_GAME_MTT_AUTO_OP);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_MTT_ADD_TIME, HANDLER_REQ_MTT_ADD_TIME);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_MTT_GAME_DISMISS, HANDLER_REQ_MTT_GAME_DISMISS);
        }

        protected void HANDLER_REQ_MTT_REBUY_REQUEST_OUTSIDE(ICPResponse response)
        {

        }

        protected void HANDLER_REQ_MTT_GAME_REBUY_CANCEL(ICPResponse response)
        {

        }

        protected void HANDLER_REQ_MTT_GAME_DISMISS(ICPResponse response)
        {
            REQ_MTT_GAME_DISMISS rec = response as REQ_MTT_GAME_DISMISS;
            if (null == rec)
            {
                return;
            }

            if (GameCache.Instance.room_id == rec.MTTID)
            {
                UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                {
                    type = UIDialogComponent.DialogData.DialogType.Commit,
                    title = string.Empty,
                    content = rec.msg,
                    contentCommit = CPErrorCode.LanguageDescription(10012),
                    actionCommit = () => {
                        
                    },
                });
                doExitRoom();
            }
        }

        //结束时的名次通知
        protected void HANDLER_REQ_GAME_MTT_FINAL_RANK(ICPResponse response)
        {
            REQ_GAME_MTT_FINAL_RANK rec = response as REQ_GAME_MTT_FINAL_RANK;
            if (null == rec)
            {
                return;
            }

            if (rec.status == 0)
            {
                // 无重购机会，显示结束页
                if (rec.ranking > 0)
                {
                    // 去除重购框 显示结束页
                    UIComponent.Instance.HideNoAnimation(UIType.UIDialog);

                    string content = "";
                    if (rec.giftReward.Length > 0)
                    {
                        content += $"{rec.giftReward}\n";
                    }
                    if (rec.chipsReward.Length > 0 && rec.chipsReward != "0")
                    {
                        content += $"{StringHelper.ShowGold(int.Parse(rec.chipsReward))}游戏币\n";
                    }
                    if (rec.hunterReward.Length > 0 && rec.hunterReward != "0")
                    {
                        content += $"猎人奖励：{StringHelper.ShowGold(int.Parse(rec.hunterReward))}游戏币";
                    }

                    if (content.Length == 0)
                    {
                        content = "调整好状态，再来一局吧！";
                    }

                    int rank = rec.ranking;

                    doExitRoom();

                    UIComponent.Instance.ShowNoAnimation(UIType.UIMTT_Ranking, new UIMTTRankingComponent.MTTRankingData
                    {
                        ranking = rank,
                        content = content,
                    });

                    
                }
            }
            else if (rec.status == 2)
            {
                // 弹出重购框
                ShowRebuyView();
            }
        }

        private void ShowRebuyView()
        {
            string content = string.Format(LanguageManager.Get("MTT_Rebuy_Alert"), rebuyCost, rebuyScore, rebuyCount);
            UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
            {
                type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                title = string.Empty,
                content = content,
                contentCommit = CPErrorCode.LanguageDescription(10012),
                contentCancel = CPErrorCode.LanguageDescription(10013),
                actionCommit = () => {
                    onClickRebuy(null);
                },
                actionCancel = () =>
                {
                    onClickRebuyCancel();
                }
            });
        }

        //重购申请结果
        protected void HANDLER_REQ_MTT_GAME_REBUY_REQUEST(ICPResponse response)
        {
            REQ_MTT_GAME_REBUY_REQUEST rec = response as REQ_MTT_GAME_REBUY_REQUEST;
            if (null == rec)
            {
                return;
            }

            switch (rec.status)
            {
                case 0:
                    // 重购成功
                    rebuyCount = rec.rebuyTime;
                    break;
                case 1:
                    // 服务器异常
                    UIComponent.Instance.Toast(LanguageManager.Get("MTT-Rebuy rebuy server error"));
                    break;
                case 2:
                    // 前一个重购申请中
                    break;
                case 3:
                    // 去买游戏币
                case 4:
                    // 去买钻石
                    UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                    {
                        type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                        // title = $"余额不足",
                        title = CPErrorCode.LanguageDescription(10025),
                        // content = $"USDT余额不足，请先充值",
                        content = CPErrorCode.LanguageDescription(20010),
                        // contentCommit = "去充值",
                        contentCommit = CPErrorCode.LanguageDescription(10026),
                        // contentCancel = "取消",
                        contentCancel = CPErrorCode.LanguageDescription(10013),
                        actionCommit = () =>
                        {
                            //商城
                            UIMineModel.mInstance.ObtainUserInfo(pDto =>
                            {
                                if (pDto.wallet.status == 1)
                                    UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletMy, pDto);
                                else if (pDto.wallet.status == 2)
                                {
                                    var tLangs = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIWallet_ClubStop_D01");
                                    UIMineModel.mInstance.SetDialogShow(tLangs, delegate
                                    {
                                        UIMineModel.mInstance.ShowSDKCustom();
                                    });
                                }
                            });
                            // UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletMy);
                        },
                        actionCancel = null
                    });
                    break;
                case 5:
                    // 正在审核
                    break;
                case 6:
                    // 报名券不足
                    break;
                case 7:
                    // 重购关闭
                    UIComponent.Instance.Toast(LanguageManager.Get("MTT-Rebuy rebuy no more allow"));
                    break;
                case 8:
                    // 被拒绝
                    UIComponent.Instance.Toast(LanguageManager.Get("MTT-Rebuy rebuy rejected"));
                    break;
                case 9:
                    // 战队在公会信用不足
                    break;
                case 10:
                    // 拆分桌中，无法重购
                    UIComponent.Instance.Toast(LanguageManager.Get("MTT-Rebuy rebuy is distributing"));
                    break;
            }
        }

        // MTT各种牌局内消息
        protected void HANDLER_REQ_GAME_MTT_SYSTEM_NOTIFY(ICPResponse response)
        {
            REQ_GAME_MTT_SYSTEM_NOTIFY rec = response as REQ_GAME_MTT_SYSTEM_NOTIFY;
            if (null == rec)
            {
                return;
            }

            if (rec.status != 0)
                return;

            string content = rec.content;

            if (rec.type == 7)
            {
                //进入奖励圈
                //显示进圈动画
                ShowArmatureRewardCircle();
                return;
            }

            if (rec.type == 9)
            {
                //观众被拆分桌
                if (content.Length == 0)
                    content = LanguageManager.Get("MTT-Game has redistribute for audience");
            }

            if (rec.type == 10)
            {
                //进入奖励圈+1
                inRewardCircle = true;
                UpdateMenu();
                return;
            }

            if (rec.type == 2)
            {
                //升盲
                blindLevel = int.Parse(content) - 1;
                if (blindLevel < MTTGameUtil.numOfLevel(blindType))
                {
                    int nextBld = MTTGameUtil.BlindAtLevel(blindLevel, blindType, blindScale);
                    content = LanguageManager.Get("Up blind notice") + $"{StringHelper.ShowGold(nextBld)}/{StringHelper.ShowGold(nextBld*2)}";

                    //更新前注
                    int ante = MTTGameUtil.AnteAtLevel(blindLevel, blindType, blindScale);
                    groupBet = ante;

                    //开始新一轮计时
                    displayedSmallBlind = nextBld;
                    upBlindLeftTime = upBlindTime * 60;
                    upBldCounting = true;
                }
                else
                {
                    upBlindLeftTime = 0;
                    upBldCounting = false;
                }
            }

            if (rec.type == 1)
            {
                //拆分桌
                //显示拆分桌提示界面
                imageRedistributionTips.gameObject.SetActive(true);
                return;
            }

            UIComponent.Instance.Toast(content);
        }

        //实时战况
        // protected void HANDLER_REQ_GAME_MTT_REAL_TIME(ICPResponse response)
        // {
        //     REQ_GAME_MTT_REAL_TIME rec = response as REQ_GAME_MTT_REAL_TIME;
        //     if (null == rec)
        //     {
        //         return;
        //     }
        // }



        protected void HANDLER_REQ_GAME_MTT_Self_Ranking(ICPResponse response)
        {

        }

        protected void HANDLER_REQ_GAME_MTT_START_GAME(ICPResponse response)
        {
            REQ_GAME_MTT_START_GAME rec = response as REQ_GAME_MTT_START_GAME;
            if (null == rec)
                return;
            HANDLER_REQ_GAME_START_GAME(rec.baseMsg);
        }

        //游戏开始了
        protected override void HANDLER_REQ_GAME_RECV_READYTIME(ICPResponse response)
        {
            base.HANDLER_REQ_GAME_RECV_READYTIME(response);
            upBldCounting = true;
        }

        protected void HANDLER_REQ_GAME_MTT_RECV_START_INFOR(ICPResponse response)
        {
            REQ_GAME_MTT_RECV_START_INFOR rec = response as REQ_GAME_MTT_RECV_START_INFOR;
            if (null == rec)
                return;

            handleRecvStartInfoCommon(rec.baseMsg, response);

            gameStarted = true;

            UIComponent.Instance.HideNoAnimation(UIType.UIMTTTime);
            imageRedistributionTips.gameObject.SetActive(false);

            if (smallBlind != rec.baseMsg.smallChip)
            {
                //盲注不同，已经升盲了
                smallBlind = rec.baseMsg.smallChip;
                displayedSmallBlind = smallBlind;
            }
            blindLevel = MTTGameUtil.LeveForBlind(smallBlind, blindType, blindScale);
        }

        protected void HANDLER_REQ_MTT_ADD_TIME(ICPResponse response)
        {
            REQ_MTT_ADD_TIME rec = response as REQ_MTT_ADD_TIME;
            if (null == rec)
                return;

            if (rec.baseMsg.status != 0)
            {
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_MTT_ADD_TIME, rec.baseMsg.status));
                return;
            }

            Log.Msg(rec.baseMsg);
            HANDLER_REQ_ADD_TIME(rec.baseMsg);
        }

        protected void HANDLER_REQ_GAME_MTT_AUTO_OP(ICPResponse response)
        {
            REQ_GAME_MTT_AUTO_OP rec = response as REQ_GAME_MTT_AUTO_OP;
            if (null == rec)
            {
                return;
            }

            if (rec.status != 0)
            {
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_GAME_MTT_AUTO_OP, rec.status));
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
                    break;
                case 1:
                    // 托管
                    mSeat = GetSeatById(mainPlayer.seatID);
                    mSeat.Player.trust = 1;
                    mSeat.UpdateTrust();
                    textCancelTrust.text = CPErrorCode.LanguageDescription(10011);

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
                        if (rec.trustStatus == null)
                            return;
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
                                    textCancelTrust.text = CPErrorCode.LanguageDescription(10011);
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

        protected override void HANDLER_REQ_GAME_ENDING(ICPResponse response)
        {
            REQ_GAME_ENDING rec = response as REQ_GAME_ENDING;
            if (null == rec)
                return;
            if (rec.status == 2)
            {
                // 拆分桌观众退出
                UIComponent.Instance.Toast(LanguageManager.Get("MTT-Game audience exit"));
            }

            doExitRoom();
        }
    }
}
