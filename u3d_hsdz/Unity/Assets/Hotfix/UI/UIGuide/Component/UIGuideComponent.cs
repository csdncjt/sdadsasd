using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	[ObjectSystem]
	public class UiGuideComponentSystem : AwakeSystem<UIGuideComponent>
	{
		public override void Awake(UIGuideComponent self)
		{
			self.Awake();
		}
	}

	public class UIGuideComponent : UIBaseComponent
	{
		private ReferenceCollector rc;
		private Button buttonClose;

		public void Awake()
		{
			rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			buttonClose = rc.Get<GameObject>("Button_Close").GetComponent<Button>();

			UIEventListener.Get(this.buttonClose.gameObject).onClick = onClickClose;

			registerHandler();
		}

		private void onClickClose(GameObject go)
		{
			Game.Scene.GetComponent<UIComponent>().Hide(UIType.UIGuide, null, 0);
		}

		public override void OnShow(object obj)
		{
			if (null != obj)
			{
				
			}
		}

		public override void OnHide()
		{
			// Log.Debug($"Guide OnHide");
		}

		public override void Dispose()
		{
			removeHandler();
			base.Dispose();
		}

		private void registerHandler()
		{

		}

		private void removeHandler()
		{

		}
	}
}
