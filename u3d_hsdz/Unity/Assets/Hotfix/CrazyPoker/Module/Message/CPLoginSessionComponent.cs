using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
	[ObjectSystem]
	public class CPLoginNetworkComponentAwakeSystem : AwakeSystem<CPLoginSessionComponent>
	{
		public override void Awake(CPLoginSessionComponent self)
		{
			self.Awake();
		}
	}

	public class CPLoginSessionComponent : Component
    {
        public const string LOG = "CPLoginSessionComponent";

        public static CPLoginSessionComponent Instance;

		public Session HotfixSession;

		public void Awake()
		{
			Instance = this;
		}

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			this.HotfixSession.Dispose();
			this.HotfixSession = null;

			base.Dispose();
		}
	}
}