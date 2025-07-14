using System.Net;
using DG.Tweening;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
	[ObjectSystem]
	public class UiTexasComponentSystem : AwakeSystem<UITexasComponent>
	{
		public override void Awake(UITexasComponent self)
		{
			self.Awake();
		}
	}

	public class UITexasComponent : UIBaseComponent
    {
        /// <summary>
        /// 是否被顶号
        /// </summary>
        public bool isOffline = false;

        private string fromUI = string.Empty;

		public void Awake()
        {
            // SoundComponent.Instance.PlayBGM(SoundComponent.BGM_GAME);

            UIComponent.Instance.ShowNoAnimation(UIType.UIHongBaoYu);

            // 没什么用，纯属用来生成Binding

            DOTween.Sequence().AppendCallback(() => { });

            TexasGame game = null;
            RoomPath mEnumRoomPath = (RoomPath)GameCache.Instance.room_path;
            int mRoomPath = GameCache.Instance.room_path;
            switch (mEnumRoomPath)
			{
				case RoomPath.Normal:
                case RoomPath.DP:
                    // 普通
                    game = ComponentFactory.CreateWithId<TexasGame, Component>(mRoomPath, this);
					break;
                case RoomPath.NormalThanOff:
                    // 普通必下场
                    game = ComponentFactory.CreateWithId<TexasThanOffGame, Component>(mRoomPath, this);
                    break;
                case RoomPath.NormalAof:
                case RoomPath.DPAof:
                    // 普通AOF
                    game = ComponentFactory.CreateWithId<TexasAofGame, Component>(mRoomPath, this);
                    break;
				case RoomPath.MTT:
                    // MTT
                    game = ComponentFactory.CreateWithId<MTTGame, Component>(mRoomPath, this);
                    break;
				case RoomPath.SNG:
					// SNG
					break;
				case RoomPath.Omaha:
					// 奥马哈
					game = ComponentFactory.CreateWithId<OmahaGame, Component>(mRoomPath, this);
					break;
                case RoomPath.OmahaThanOff:
                    // 奥马哈必下场   
                    game = ComponentFactory.CreateWithId<OmahaThanOffGame, Component>(mRoomPath, this);
                    break;
                case RoomPath.OmahaAof:
                    // 奥马哈AOF
                    game = ComponentFactory.CreateWithId<OmahaAofGame, Component>(mRoomPath, this);
                    break;
				case RoomPath.PAP:
					// 大菠萝
					break;
			}

			GameCache.Instance.CurGame = game;

            registerHandler();

            //侦听IM消息 （牌局内消息）
            SystemMesComponent.Instance.texasGameMessageDelegate = OnTexasMes;
        }

		public override void OnShow(object obj)
        {
            fromUI = null != obj? obj.ToString() : string.Empty;

            GameObject defaultObj = GameObject.Find("Global/UI/LobbyCanvas/UIDefault(Clone)");
            if (null != defaultObj)
            {
                GameObject.Destroy(defaultObj);
            }

            if (!string.IsNullOrEmpty(fromUI))
            {
                UIComponent.Instance.HideNoAnimation(fromUI);
                UIComponent.Instance.HideNoAnimation(UIType.UILobby_Menu);
            }

            RoomPath mEnumRoomPath = (RoomPath)GameCache.Instance.room_path;
            if (mEnumRoomPath == RoomPath.MTT)
            {
                // MTT先请求比赛倒计时，开赛才请求进房协议
                (GameCache.Instance.CurGame as MTTGame).ObtainMTTCountDown();
            }
            else
            {
                RequestEnterRoom();
            }
        }

        public void RequestEnterRoom()
        {
            string[] mArrAddress = NetHelper.GetAddressIPs(GlobalData.Instance.LoginHost, GlobalData.Instance.UseDNS);
            var mNetOuterComponent = Game.Scene.ModelScene.GetComponent<NetOuterComponent>();

            ETModel.Session mSession = mNetOuterComponent.Create(new IPEndPoint(IPAddress.Parse(mArrAddress[0]), GameCache.Instance.roomPort));

            CPGameSessionComponent mCpGameSessionComponent = Game.Scene.GetComponent<CPGameSessionComponent>() ?? Game.Scene.AddComponent<CPGameSessionComponent>();
            mCpGameSessionComponent.HotfixSession = ComponentFactory.Create<Session, ETModel.Session>(mSession);
            mCpGameSessionComponent.HotfixSession.SetProtrolVersionInHead(1);
            mCpGameSessionComponent.HotfixSession.SetUserIdInHead(GameCache.Instance.nUserId);
            mCpGameSessionComponent.HotfixSession.SetLanguageInHead(1);
            byte mPlatform = 2;
            if (Application.platform == RuntimePlatform.Android)
                mPlatform = 1;
            mCpGameSessionComponent.HotfixSession.SetPlatformInHead(mPlatform);
            mCpGameSessionComponent.HotfixSession.SetBuildVersionInHead(254);
            mCpGameSessionComponent.HotfixSession.SetChannelInHead(14);
            mCpGameSessionComponent.HotfixSession.SetProductIdInHead(1002);
            mCpGameSessionComponent.HotfixSession.SetTokenInHead(GameCache.Instance.strToken);
            RoomPath mEnumRoomPath = (RoomPath)GameCache.Instance.room_path;
            ICPRequest mCpRequest = null;
            switch (mEnumRoomPath)
            {
                case RoomPath.Normal:
                case RoomPath.DP:
                case RoomPath.DPAof:
                // 普通
                case RoomPath.NormalThanOff:
                // 普通必下场
                case RoomPath.NormalAof:
                    // 普通AOF
                    mCpRequest = new REQ_GAME_ENTER_ROOM()
                    {
                        type = 1,
                        connect = 1,
                        unknow = 1,
                        roomPath = GameCache.Instance.room_path,
                        roomID = GameCache.Instance.room_id,
                        deskId = 0
                    };
                    break;
                case RoomPath.MTT:
                    // MTT
                    mCpRequest = new REQ_GAME_MTT_ENTER_ROOM()
                    {
                        type = 1,
                        connect = 1,
                        unknow = 1,
                        roomPath = GameCache.Instance.room_path,
                        roomID = GameCache.Instance.room_id,
                        deskId = GameCache.Instance.mtt_deskId
                    };
                    break;
                case RoomPath.SNG:
                    // SNG
                    break;
                case RoomPath.Omaha:
                // 奥马哈
                case RoomPath.OmahaThanOff:
                // 奥马哈必下场
                case RoomPath.OmahaAof:
                    // 奥马哈AOF
                    mCpRequest = new REQ_GAME_OMAHA_ENTER_ROOM()
                    {
                        type = 1,
                        connect = 1,
                        unknow = 1,
                        roomPath = GameCache.Instance.room_path,
                        roomID = GameCache.Instance.room_id,
                        deskId = 0
                    };
                    break;
                case RoomPath.PAP:
                    // 大菠萝
                    break;
            }


            if (null != mCpRequest)
                mCpGameSessionComponent.HotfixSession.Send(mCpRequest);
        }

		public override void OnHide()
		{
            SystemMesComponent.Instance.texasGameMessageDelegate = null;//移除消息侦听
        }

		public override void Dispose()
		{
			if (IsDisposed)
			{
				return;
			}

            UIComponent.Instance.Remove(UIType.UIHongBaoYu);

            // SoundComponent.Instance.PlayBGM(SoundComponent.BGM_LOBBY);

            removeHandler();

            if (null != GameCache.Instance.CurGame)
				GameCache.Instance.CurGame.Dispose();
			GameCache.Instance.CurGame = null;

            //设置IM对话组
            IMSdkComponent.Instance.ClearConversation();
            SystemMesComponent.Instance.texasGameMessageDelegate = null;//移除消息侦听

            if (!string.IsNullOrEmpty(fromUI) && !isOffline)
            {
                UIComponent.Instance.ShowNoAnimation(UIType.UILobby_Menu);
                UIComponent.Instance.ShowNoAnimation(fromUI, GameCache.Instance.room_id);
            }

            isOffline = false;

            Game.EventSystem.Run(EventIdType.UIMatch_RefreshList);

            base.Dispose();
		}

        private void registerHandler()
        {
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_ENTER_ROOM, HANDLER_REQ_GAME_ENTER_ROOM);
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_OMAHA_ENTER_ROOM, HANDLER_REQ_GAME_OMAHA_ENTER_ROOM);
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_MTT_ENTER_ROOM, HANDLER_REQ_GAME_MTT_ENTER_ROOM);
        }

        private void removeHandler()
        {
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_ENTER_ROOM, HANDLER_REQ_GAME_ENTER_ROOM);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_OMAHA_ENTER_ROOM, HANDLER_REQ_GAME_OMAHA_ENTER_ROOM);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_MTT_ENTER_ROOM, HANDLER_REQ_GAME_MTT_ENTER_ROOM);
        }

        private void HANDLER_REQ_GAME_ENTER_ROOM(ICPResponse response)
        {
            REQ_GAME_ENTER_ROOM rec = response as REQ_GAME_ENTER_ROOM;
            if (null == rec)
                return;

            CommonEnterRoom(rec.status, rec);
        }

        private void HANDLER_REQ_GAME_OMAHA_ENTER_ROOM(ICPResponse response)
        {
            REQ_GAME_OMAHA_ENTER_ROOM rec = response as REQ_GAME_OMAHA_ENTER_ROOM;
            if (null == rec)
                return;

            CommonEnterRoom(rec.baseMsg.status, rec);
        }

        private void HANDLER_REQ_GAME_MTT_ENTER_ROOM(ICPResponse response)
        {
            REQ_GAME_MTT_ENTER_ROOM rec = response as REQ_GAME_MTT_ENTER_ROOM;
            if (null == rec)
                return;

            CommonEnterRoom(rec.baseMsg.status, rec);
        }

        private void CommonEnterRoom(sbyte status, object obj)
        {
            UI mMatchLoading = null;
            if (status != 0)
            {
                mMatchLoading = UIComponent.Instance.Get(UIType.UIMatch_Loading);
                if (null != mMatchLoading && null != mMatchLoading.GameObject && mMatchLoading.GameObject.activeInHierarchy)
                {
                    UIComponent.Instance.HideNoAnimation(UIType.UIMatch_Loading);
                }
                GameCache.Instance.CurGame.doExitRoom();
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_GAME_ENTER_ROOM, status));
                return;
            }

            CPGameSessionComponent mCpGameSessionComponent = Game.Scene.GetComponent<CPGameSessionComponent>();
            CPGameHeartbeatComponent mCpGameHeartbeatComponent = mCpGameSessionComponent.HotfixSession.GetComponent<CPGameHeartbeatComponent>();
            if (null != mCpGameHeartbeatComponent)
                mCpGameSessionComponent.HotfixSession.RemoveComponent<CPGameHeartbeatComponent>();
            mCpGameHeartbeatComponent = mCpGameSessionComponent.HotfixSession.AddComponent<CPGameHeartbeatComponent>();
            mCpGameHeartbeatComponent.StartSending();

            mMatchLoading = UIComponent.Instance.Get(UIType.UIMatch_Loading);
            if (null != mMatchLoading && null != mMatchLoading.GameObject && mMatchLoading.GameObject.activeInHierarchy)
            {
                UIComponent.Instance.HideNoAnimation(UIType.UIMatch_Loading);
            }

            if (null != GameCache.Instance.CurGame)
                GameCache.Instance.CurGame.UpdateRoom(obj);

            //设置IM对话组
            IMSdkComponent.Instance.SetConversation(GameCache.Instance.room_id.ToString());
        }

        /// <summary>
        /// IM 牌局内消息通知
        /// </summary>
        /// <param name="arg2">固定1，暂时没用</param>
        /// <param name="arg3">USDT数</param>
        /// <param name="arg4">userId</param>
        private void OnTexasMes(string arg2, string arg3, string arg4)
        {
            int userId = -1;
            if (int.TryParse(arg4, out userId))
            {
                if (arg2.Equals("1"))
                    Game.EventSystem.Run(EventIdType.Hongbaoyu_Dispatch, userId, arg3); // 红包雨
                else if(arg2.Equals("2"))
                    Game.EventSystem.Run(EventIdType.TexasMarqueeGame_Dispatch, userId, arg3); // 跑马灯小游戏
            }
        }
    }
}
