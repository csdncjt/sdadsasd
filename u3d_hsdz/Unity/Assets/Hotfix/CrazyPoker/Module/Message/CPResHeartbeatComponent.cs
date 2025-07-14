using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
	[ObjectSystem]
	public class CPResHeartbeatComponentAwakeSystem : AwakeSystem<CPResHeartbeatComponent>
	{
		public override void Awake(CPResHeartbeatComponent self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class CPResHeartbeatComponentUpdateSystem : UpdateSystem<CPResHeartbeatComponent>
	{
		public override void Update(CPResHeartbeatComponent self)
		{
			self.Update();
		}
	}

	public class CPResHeartbeatComponent : Component
	{
		public float sendInterval = 10f;
		private float recordDeltaTime = 0f;

		private bool isSending;

		private CPResSessionComponent cpResSessionComponent;

		public void Awake()
		{
			recordDeltaTime = Time.time;
			isSending = false;
			CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_SYNC_SIGNAL_RESOURCE, HANDLER_REQ_SYNC_SIGNAL_RESOURCE);
		}

		public override void Dispose()
		{
			if (IsDisposed)
			{
				return;
			}

            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_SYNC_SIGNAL_RESOURCE, HANDLER_REQ_SYNC_SIGNAL_RESOURCE);
            isSending = false;

			base.Dispose();
		}

        public void Update()
        {
            if (!isSending)
                return;

            if (!(Time.time - recordDeltaTime > sendInterval)) return;

            recordDeltaTime = Time.time;

            if (null == cpResSessionComponent)
                cpResSessionComponent = Game.Scene.GetComponent<CPResSessionComponent>();

            cpResSessionComponent.HotfixSession.Send(new REQ_SYNC_SIGNAL_RESOURCE());

        }

        private void HANDLER_REQ_SYNC_SIGNAL_RESOURCE(ICPResponse response)
		{
			REQ_SYNC_SIGNAL_RESOURCE rec = response as REQ_SYNC_SIGNAL_RESOURCE;
			if (null == rec)
				return;
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