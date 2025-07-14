using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	[ObjectSystem]
	public class UIPauseComponentAwakeSystem : AwakeSystem<UIPauseComponent>
	{
		public override void Awake(UIPauseComponent self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class UIPauseComponentUpdateSystem: UpdateSystem<UIPauseComponent>
	{
		public override void Update(UIPauseComponent self)
		{
			self.Update();
		}
	}

	public class UIPauseComponent : UIBaseComponent
	{
		public sealed class PauseData
		{
			public int roomPath { get; set; }
			public int roomId { get; set; }  // 房间号
			public int remainingTime { get; set; }  // 暂停剩余时间  秒
			public bool isOwner { get; set; }	// 是否房主
		}

		private PauseData pauseData;

		private ReferenceCollector rc;
		private Button buttonPause;
        private Text textTitle;
        private Text textCountdown;

		private bool bCountdown;
		private float sendInterval = 1f;
		private float recordDeltaTime;

		public void Awake()
		{
			rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			buttonPause = rc.Get<GameObject>("Button_Pause").GetComponent<Button>();
            textTitle = rc.Get<GameObject>("Text_title").GetComponent<Text>();
            textCountdown = rc.Get<GameObject>("Text_Countdown").GetComponent<Text>();

			UIEventListener.Get(buttonPause.gameObject).onClick = onClickPause;

			registerHandler();
		}

		private void onClickPause(GameObject go)
		{
            if (!pauseData.isOwner) return;
            Game.Scene.GetComponent<CPGameSessionComponent>().HotfixSession.Send(new REQ_GAME_PAUSE()
			{
					bPause = 0,
					roomID = pauseData.roomId,
					roomPath = pauseData.roomPath
			});
		}

		public override void OnShow(object obj)
		{
			if (null == obj)
				return;

			pauseData = obj as PauseData;
			if (null == pauseData)
				return;

			bCountdown = true;
			recordDeltaTime = Time.time;

			textCountdown.text = $"{pauseData.remainingTime / 60}:{pauseData.remainingTime % 60}";

            buttonPause.interactable = pauseData.isOwner;
            // textTitle.text = pauseData.isOwner ? "继续游戏" : "暂停游戏";
            textTitle.text = pauseData.isOwner ? CPErrorCode.LanguageDescription(10302) : CPErrorCode.LanguageDescription(10303);

		}

		public override void OnHide()
		{
			bCountdown = false;
		}

		public override void Dispose()
        {
            if (this.IsDisposed)
                return;
			removeHandler();
			base.Dispose();
		}

		private void registerHandler()
		{

		}

		private void removeHandler()
		{

		}

		public void Update()
		{
			if (!bCountdown)
				return;

			// 如果还没有建立Session直接返回、或者没有到达发包时间
			if (!(Time.time - recordDeltaTime > sendInterval)) return;

			// 记录当前时间
			recordDeltaTime = Time.time;

			pauseData.remainingTime -= 1;
			textCountdown.text = $"{pauseData.remainingTime / 60}:{pauseData.remainingTime % 60}";

			if (pauseData.remainingTime < 0)
				bCountdown = false;
		}
	}
}
