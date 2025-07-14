using ETModel;
using UnityEngine;

namespace ETHotfix
{
	[ObjectSystem]
	public class CPGameSessionComponentAwakeSystem : AwakeSystem<CPGameSessionComponent>
	{
		public override void Awake(CPGameSessionComponent self)
		{
			self.Awake();
		}
	}

    [ObjectSystem]
    public class CPGameSessionComponentUpdateSystem : UpdateSystem<CPGameSessionComponent>
    {
        public override void Update(CPGameSessionComponent self)
        {
            self.Update();
        }
    }

    public class CPGameSessionComponent : Component
	{
		public static CPGameSessionComponent Instance;

		public Session HotfixSession;

        public float sendInterval = 2;
        private float recordDeltaTime = 0f;

        public void Awake()
		{
			Instance = this;
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

			base.Dispose();
		}
	}
}