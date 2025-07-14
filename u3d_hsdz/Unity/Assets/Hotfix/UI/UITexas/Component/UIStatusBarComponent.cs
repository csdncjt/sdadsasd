using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	[ObjectSystem]
	public class UIStatusBarComponentAwakeSystem : AwakeSystem<UIStatusBarComponent>
	{
		public override void Awake(UIStatusBarComponent self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class UIStatusBarComponentUpdateSystem : UpdateSystem<UIStatusBarComponent>
	{
		public override void Update(UIStatusBarComponent self)
		{
			self.Update();
		}
	}

	public class UIStatusBarComponent : UIBaseComponent
	{
        private ReferenceCollector rc;
        private Text textTime;
        private Image imageNetwork;
        private Image imageCellular;
        private Image imageWifi;
        private Image imageBatteryProgress;
        private Image imageBatteryCharging;

        private bool bCountdown;
		private float sendInterval = 1f;
		private float recordDeltaTime;

		public void Awake()
		{
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            textTime = rc.Get<GameObject>("Text_Time").GetComponent<Text>();
            imageNetwork = rc.Get<GameObject>("Image_Network").GetComponent<Image>();
            imageCellular = rc.Get<GameObject>("Image_Cellular").GetComponent<Image>();
            imageWifi = rc.Get<GameObject>("Image_Wifi").GetComponent<Image>();
            imageBatteryProgress = rc.Get<GameObject>("Image_BatteryProgress").GetComponent<Image>();
            imageBatteryCharging = rc.Get<GameObject>("Image_BatteryCharging").GetComponent<Image>();

            if (Screen.width / (Screen.height * 1f) <= 0.48f)
            {
                //长屏幕，如iPhoneX，要往下一点
                rc.gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(30, -90);
                rc.gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(-30, -30);
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer)
                NativeManager.UpdateShowStatusBar(false);

            ETModel.Game.Hotfix.OnApplicationPauseFalse += HideNativeStatusBar;

            bCountdown = true;
            recordDeltaTime = Time.time;
            RefreshUI();
        }

        private void HideNativeStatusBar()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                NativeManager.UpdateShowStatusBar(false);
        }

		public override void OnShow(object obj)
		{
		}

		public override void OnHide()
		{
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                NativeManager.UpdateShowStatusBar(true);
            bCountdown = false;
		}

		public override void Dispose()
		{
            ETModel.Game.Hotfix.OnApplicationPauseFalse -= HideNativeStatusBar;
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                NativeManager.UpdateShowStatusBar(true);
            bCountdown = false;
            base.Dispose();
		}

        public void UpdateNetworkDelay(float delay)
        {
            if (delay <= 0.140)
            {
                imageNetwork.sprite = rc.Get<Sprite>("leftbar_route_good_network");
            }
            else if (delay <= 0.220)
            {
                imageNetwork.sprite = rc.Get<Sprite>("leftbar_route_general_network");
            }
            else
            {
                imageNetwork.sprite = rc.Get<Sprite>("leftbar_route_network_error");
            }
        }

		public void Update()
		{
			if (!bCountdown)
				return;

			// 如果还没有建立Session直接返回、或者没有到达发包时间
			if (!(Time.time - recordDeltaTime > sendInterval)) return;

			// 记录当前时间
			recordDeltaTime = Time.time;

            RefreshUI();

            HideNativeStatusBar();
        }

        private void RefreshUI()
        {
            textTime.text = DateTime.Now.ToString("HH:mm");

            switch (UnityEngine.Application.internetReachability)
            {
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    imageCellular.gameObject.SetActive(false);
                    imageWifi.gameObject.SetActive(true);
                    break;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    imageCellular.gameObject.SetActive(true);
                    imageWifi.gameObject.SetActive(false);
                    break;
                case NetworkReachability.NotReachable:
                    imageCellular.gameObject.SetActive(false);
                    imageWifi.gameObject.SetActive(false);
                    imageNetwork.sprite = rc.Get<Sprite>("leftbar_route_network_error");
                    break;
            }

            imageBatteryProgress.GetComponent<RectTransform>().sizeDelta = new Vector2(54 * SystemInfo.batteryLevel, 21);

            if (SystemInfo.batteryStatus == BatteryStatus.Charging)
            {
                imageBatteryCharging.gameObject.SetActive(true);
                imageBatteryProgress.color = new Color32(120, 225, 121, 255);
            }
            else
            {
                imageBatteryCharging.gameObject.SetActive(false);
                imageBatteryProgress.color = new Color(1, 1, 1, 1);
            }
        }
    }
}
