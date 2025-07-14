using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using DtoElement = ETHotfix.WEB2_room_mtt_list.RoomListElement;

namespace ETHotfix
{
	//关于 api接口 往这边迁移
	public class UIMatchMTTModel
    {
        private static UIMatchMTTModel instance;
        public static UIMatchMTTModel mInstance { get { if (instance == null) instance = new UIMatchMTTModel(); return instance; } }

        bool isWaitingGPSCallBack = false;

        bool isRequestingRebuy = false;

        private DtoElement curElement;
        string fromCompoment;

        Action<bool> finishAction;

        public UIMatchMTTModel()
        {
            registerHandler();
        }

        public void Dispose()
        {
            removeHandler();
            isWaitingGPSCallBack = false;
        }

        private void registerHandler()
        {
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_MTT_APPLY_JOIN, HANDLER_REQ_MTT_APPLY_JOIN);  // MTT报名
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_MTT_REBUY_REQUEST_OUTSIDE, HANDLER_REQ_MTT_REBUY_REQUEST_OUTSIDE);  // MTT牌局外重购申请
        }

        private void removeHandler()
        {
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_MTT_APPLY_JOIN, HANDLER_REQ_MTT_APPLY_JOIN);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_MTT_REBUY_REQUEST_OUTSIDE, HANDLER_REQ_MTT_REBUY_REQUEST_OUTSIDE);
        }

        //MTT报名整合
        public void onMTTAction(DtoElement tDto, string from, Action<bool> finish)
        {
            finishAction = finish;
            curElement = tDto;
            fromCompoment = from;
            int gameStatus = curElement.gameStatus;

            if (curElement.canPlayerRebuy)
            {
                //重购
                bool isGPSRestrictions = tDto.hasGpsLimit;
                if (isGPSRestrictions)
                {
                    //开启GPS，先获取定位
                    isWaitingGPSCallBack = true;
                    NativeManager.GetGPSLocation();
                    if (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.OSXEditor)
                    {
                        return;
                    }
                }
                isWaitingGPSCallBack = false;
                isRequestingRebuy = true;
                CPResSessionComponent.Instance.HotfixSession.Send(new REQ_MTT_REBUY_REQUEST_OUTSIDE()
                {
                    roomID = tDto.matchId,
                    roomPath = (int)RoomPath.MTT,
                    longitude = "0",
                    latitude = "0",
                    uuid = SystemInfoUtil.getDeviceUniqueIdentifier()
                });
            }
            else
            {
                if (gameStatus == 3 || gameStatus == 4)
                {
                    //进入游戏
                    EnterMTTGame();
                }

                if (gameStatus == 0 || gameStatus == 2)
                {
                    //报名
                    bool isGPSRestrictions = tDto.hasGpsLimit;
                    if (isGPSRestrictions)
                    {
                        //开启GPS，先获取定位
                        isWaitingGPSCallBack = true;
                        NativeManager.GetGPSLocation();
                        if (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.OSXEditor)
                        {
                            return;
                        }
                    }
                    isWaitingGPSCallBack = false;
                    CPResSessionComponent.Instance.HotfixSession.Send(new REQ_MTT_APPLY_JOIN()
                    {
                        operate = 1,
                        longitude = "0",
                        latitude = "0",
                        clientIP = GameCache.Instance.client_ip,
                        roomID = tDto.matchId,
                        roomPath = (int)RoomPath.MTT,
                        clubID = 0,
                        uuid = SystemInfoUtil.getDeviceUniqueIdentifier()
                    });
                }
            }

            
        }

        public void OnGPSCallBack(string status)
        {
            if (isWaitingGPSCallBack)
            {
                isWaitingGPSCallBack = false;
                if (status == "0")
                {
                    if (curElement.canPlayerRebuy)
                    {
                        isRequestingRebuy = true;
                        CPResSessionComponent.Instance.HotfixSession.Send(new REQ_MTT_REBUY_REQUEST_OUTSIDE()
                        {
                            roomID = curElement.matchId,
                            roomPath = (int)RoomPath.MTT,
                            longitude = GameCache.Instance.longitude,
                            latitude = GameCache.Instance.latitude,
                            uuid = SystemInfoUtil.getDeviceUniqueIdentifier()
                        });
                    } else
                    {
                        CPResSessionComponent.Instance.HotfixSession.Send(new REQ_MTT_APPLY_JOIN()
                        {
                            operate = 1,
                            longitude = GameCache.Instance.longitude,
                            latitude = GameCache.Instance.latitude,
                            clientIP = GameCache.Instance.client_ip,
                            roomID = curElement.matchId,
                            roomPath = (int)RoomPath.MTT,
                            clubID = 0,
                            uuid = SystemInfoUtil.getDeviceUniqueIdentifier()
                        });
                    }

                }
            }

        }

        private void EnterMTTGame()
        {
            int pRoomId = curElement.matchId;
            EnterMTTGame(pRoomId, 0);
        }

        public void EnterMTTGame(int roomId, int deskId)
        {
            UIMatchModel.mInstance.APIEnterMTTRoom((int)RoomPath.MTT, roomId, pDto =>
            {
                GameCache.Instance.roomName = pDto.matchName;
                GameCache.Instance.roomIP = pDto.matchIp;
                GameCache.Instance.roomPort = pDto.matchPort;
                GameCache.Instance.room_path = pDto.roomPath;
                GameCache.Instance.rtype = (int)RoomPath.MTT;
                GameCache.Instance.room_id = pDto.matchId;
                GameCache.Instance.client_ip = pDto.clientIp;
                GameCache.Instance.mtt_type = pDto.mttType;

                GameCache.Instance.mtt_deskId = deskId;
                MTTGameUtil.initList(pDto.blindNote, pDto.ante);
                UIComponent.Instance.ShowNoAnimation(UIType.UIMatch_Loading); //Loading页面.在UItexas生成就 remove掉
                UIComponent.Instance.Remove(UIType.UIMatch_MttDetail);
                UIComponent.Instance.ShowNoAnimation(UIType.UITexas, fromCompoment);
            });
        }

        protected void HANDLER_REQ_MTT_APPLY_JOIN(ICPResponse response)
        {
            REQ_MTT_APPLY_JOIN rec = response as REQ_MTT_APPLY_JOIN;
            if (null == rec)
            {
                return;
            }

            //更新一下数据
            if (finishAction != null)
                finishAction(rec.status == 0);

            switch (rec.status)
            {
                case 0:

                    /*
                     result:
                     1: 报名券不足
                     2: 游戏币不足
                     3: 报名被拒绝2次,
                     4: 开赛前1分钟，没开延时报名的比赛，报名已经截止,
                     5: 报名人数达到上限,无法报名
                     6: 当前已经开赛且延时报名开关关闭或不满足延时报名的条件
                     7: 等待开赛
                     8: 立即进入
                     9: 开了控制带入，等待审核
                     10:开了延时报名,开赛前1min，人数小于开赛人数下限，不能报名
                     11:其他异常导致报名失败；
                     12:GPS 限制，没法报名；
                     13:IP 限制，没法报名；
                     14:战队在公会信用不足
                     */
                    switch (rec.result)
                    {
                        case 7:
                        case 8:
                            if (curElement.gameStatus == 2)
                            {
                                //延迟报名成功，直接进入游戏
                                EnterMTTGame();
                            }
                            else
                            {
                                UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                                {
                                    type = UIDialogComponent.DialogData.DialogType.Commit,
                                    title = string.Empty,
                                    content = LanguageManager.Get("MTT_Apply_Success"),
                                    contentCommit = CPErrorCode.LanguageDescription(10012),
                                    actionCommit = () => {
                                    },
                                });
                            }
                            break;
                        default://1-14
                            UIComponent.Instance.Toast(LanguageManager.Get($"MTT_Apply_Result_{rec.result}"));
                            break;
                    }

                    break;
                case 1:
                    UIComponent.Instance.Toast("MTT未开放报名");
                    break;
                case 2:
                    UIComponent.Instance.Toast("MTT不存在");
                    break;
                case 3:
                    UIComponent.Instance.Toast("赛事还未到报名时间，暂时不可报名");
                    break;
                case 4:
                    UIComponent.Instance.Toast("报名人数已经达到上限，请选择其他赛事");
                    break;
                case 5:
                    UIComponent.Instance.Toast("延时报名条件不足");
                    break;
                default:
                    UIComponent.Instance.Toast("报名失败");
                    break;
            }
        }

        protected void HANDLER_REQ_MTT_REBUY_REQUEST_OUTSIDE(ICPResponse response)
        {
            REQ_MTT_REBUY_REQUEST_OUTSIDE rec = response as REQ_MTT_REBUY_REQUEST_OUTSIDE;
            if (null == rec)
            {
                return;
            }

            if (!isRequestingRebuy) return;
            isRequestingRebuy = false;

            //更新一下数据
            if (finishAction != null)
                finishAction(rec.status == 0);

            switch (rec.status)
            {
                case 0:
                    //重购成功，直接进入游戏
                    EnterMTTGame();
                    break;
                default:
                    UIComponent.Instance.Toast(LanguageManager.Get($"MTT_Rebuy_Outside_Result_{rec.status}"));
                    break;
            }
        }

    }
}
