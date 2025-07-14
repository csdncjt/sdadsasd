using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	[ObjectSystem]
	public class UiPromptComponentAwakeSystem : AwakeSystem<UIPromptComponent>
	{
		public override void Awake(UIPromptComponent self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class UiPromptComponentUpdateSystem : UpdateSystem<UIPromptComponent>
	{
		public override void Update(UIPromptComponent self)
		{
			self.Update();
		}
	}

	public class UIPromptComponent : UIBaseComponent
	{
		private ReferenceCollector rc;
        private Transform transContent;
        // private Tweener tweener;
		private float interval = 2f;
        private float timeOut = 10f;
        private float recordDeltaTime = 0f;
        private bool waitingShow;
        private bool isShow;

		public void Awake()
		{
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            transContent = rc.Get<GameObject>("Content").transform;

        }

        public void Update()
		{
			if (isShow)
            {
                if (Time.time - recordDeltaTime > timeOut)
                {
                    UIComponent.Instance.HideNoAnimation(UIType.UIPrompt);
                }
                return;
            }
            if (!waitingShow) return;
			if (!(Time.time - recordDeltaTime > interval)) return;

			isShow = true;
            waitingShow = false;
            transContent.gameObject.SetActive(true);
		}

		public override void OnShow(object obj)
		{
			recordDeltaTime = Time.time;
			waitingShow = true;
            isShow = false;
		}

		public override void OnHide()
		{
			waitingShow = false;
            isShow = false;
            transContent.gameObject.SetActive(false);
		}

		public override void Dispose()
		{
            if (IsDisposed)
            {
                return;
            }
            waitingShow = false;
			isShow = false;
            transContent.gameObject.SetActive(false);
			base.Dispose();
		}

	}
}
