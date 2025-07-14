using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
	[ObjectSystem]
	public class CPGameHeartbeatComponentAwakeSystem : AwakeSystem<CPGameHeartbeatComponent>
	{
		public override void Awake(CPGameHeartbeatComponent self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class CPGameHeartbeatComponentUpdateSystem: UpdateSystem<CPGameHeartbeatComponent>
	{
		public override void Update(CPGameHeartbeatComponent self)
		{
			self.Update();
		}
	}

	public class CPGameHeartbeatComponent : Component
	{
		public float sendInterval = 5f;
		private float recordDeltaTime = 0f;

		private bool isSending;

		private CPGameSessionComponent cpGameSessionComponent;

		public void Awake()
		{
			recordDeltaTime = Time.time;
			isSending = false;
			CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_SYNC_SIGNAL_GAME, HANDLER_REQ_SYNC_SIGNAL_GAME);
		}

		public override void Dispose()
		{
			if (IsDisposed)
			{
				return;
			}

			isSending = false;

			base.Dispose();
		}

        public void Update()
        {
            if (!isSending)
                return;

            if (!(Time.time - recordDeltaTime > sendInterval)) return;

            recordDeltaTime = Time.time;

            if (null == cpGameSessionComponent)
                cpGameSessionComponent = Game.Scene.GetComponent<CPGameSessionComponent>();

            if (null != cpGameSessionComponent.HotfixSession)
                cpGameSessionComponent.HotfixSession.Send(new REQ_SYNC_SIGNAL_GAME());

        }

        private void HANDLER_REQ_SYNC_SIGNAL_GAME(ICPResponse response)
		{
            float delay = Time.time - recordDeltaTime;
            UI uI = UIComponent.Instance.Get(UIType.UIStatusBar);
            if (null != uI)
            {
                UIStatusBarComponent barComponent = uI.UiBaseComponent as UIStatusBarComponent;
                barComponent.UpdateNetworkDelay(delay);
            }
            GameCache.Instance.CurGame.UpdateNetworkDelay(delay);

            REQ_SYNC_SIGNAL_GAME rec = response as REQ_SYNC_SIGNAL_GAME;
            if (null == rec)
                return;
            if (rec.status == 1)
            {
                //后台判断为掉线，需要重连
                Game.Scene.GetComponent<NetworkDetectionComponent>().Reconnect();
            }
        }

		public void StartSending()
		{
			isSending = true;
		}

		public void StopSending()
		{
			isSending = false;
		}
	}
}