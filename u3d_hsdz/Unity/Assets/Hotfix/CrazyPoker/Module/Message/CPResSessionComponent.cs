using System.Collections.Generic;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class CPResNetworkComponentAwakeSystem : AwakeSystem<CPResSessionComponent>
    {
        public override void Awake(CPResSessionComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class CPResNetworkComponentUpdateSystem : UpdateSystem<CPResSessionComponent>
    {
        public override void Update(CPResSessionComponent self)
        {
            self.Update();
        }
    }

    public class CPResSessionComponent : Component
    {
        public static CPResSessionComponent Instance;

        public Session HotfixSession;

        public float sendInterval = 8;
        private float recordDeltaTime = 0f;

        public void Awake()
        {
            Instance = this;

            recordDeltaTime = Time.time;

            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_OFFLINE, REQ_OFFLINE_HANDLER);

        }

        public void Update()
        {
            if (!(Time.time - recordDeltaTime > sendInterval)) return;
            recordDeltaTime = Time.time;

            if (HotfixSession?.session != null && HotfixSession.session.Error != 0)
            {
                Game.Scene.GetComponent<NetworkDetectionComponent>().Reconnect();
            }
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            HotfixSession.Dispose();
            HotfixSession = null;

            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_OFFLINE, REQ_OFFLINE_HANDLER);


            base.Dispose();
        }

        private void REQ_OFFLINE_HANDLER(ICPResponse response)
        {
            REQ_OFFLINE rec = response as REQ_OFFLINE;
            if (null == rec)
                return;

            Game.EventSystem.Run(EventIdType.ResOffLineDispose);

            NetworkUtil.RemoveAllSessionComponent();
            NetworkUtil.LogoutClear();
            UIComponent.Instance.RemoveAll(new List<string>() { UIType.UIMarquee });
            UIComponent.Instance.ShowNoAnimation(UIType.UILogin, "NoAutoLogin");
        }

    }
}