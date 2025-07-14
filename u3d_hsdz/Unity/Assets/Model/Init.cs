using System;
using System.IO;
using System.Threading;
using UnityEngine;

using System.Collections;

namespace ETModel
{
    public class Init : MonoBehaviour
    {
        private async void Start()
        {
            try
            {
#if UNITY_IOS || UNITY_ANDROID
                Application.runInBackground = true;
                // Application.targetFrameRate = 60;
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
#endif

#if LOG_ZSX
                var t = GameObject.Find("LoginCanvas");
                var ts = t.GetComponentsInChildren<RectTransform>();
                for (int i = 0; i < ts.Length; i++)
                {
                    ts[i].gameObject.SetActive(false);
                }
                t.SetActive(true);
#endif

                SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

                DontDestroyOnLoad(gameObject);

                Game.EventSystem.Add(DLLType.Model, typeof(Init).Assembly);

                Game.Scene.AddComponent<GlobalConfigComponent>();
                Game.Scene.AddComponent<NetOuterComponent>();
                Game.Scene.AddComponent<ResourcesComponent>();
                Game.Scene.AddComponent<LocalResourcesComponent>();

                Game.Scene.AddComponent<NetHttpComponent>();

                Game.Scene.AddComponent<UIComponent>();
                Game.Scene.AddComponent<NetLineSwitchComponent>();

                UIComponent.Instance.ShowNoAnimation(UIType.UIDefault);

#if UNITY_IOS
                TimerComponent iOSTimerComponent = Game.Scene.GetComponent<TimerComponent>();
                while (true)
                {
                    await iOSTimerComponent.WaitAsync(200);
                    if (UnityEngine.Application.internetReachability != NetworkReachability.NotReachable)
                        break;
                }
#endif

                // 安装包更新
                await InstallPacketHelper.CheckInstallPacket();
                if (InstallPacketHelper.NeedInstallPacket())
                {
                    InstallPacketDownloaderComponent mInstallPacketDownloaderComponent = Game.Scene.GetComponent<InstallPacketDownloaderComponent>();

                    if (null != mInstallPacketDownloaderComponent)
                    {
                        bool mWaitting = true;

                        string[] mArr = mInstallPacketDownloaderComponent.remoteInstallPacketConfig.Msg.Split(new string[] { "@%" }, StringSplitOptions.None);
                        int mTmpLanguage = PlayerPrefsMgr.mInstance.GetInt(PlayerPrefsKeys.KEY_LANGUAGE, 2);
                        if (mTmpLanguage < 0)
                            mTmpLanguage = 0;
                        if (mTmpLanguage >= LanguageManager.mInstance.ConsumeMobileData.Count)
                            mTmpLanguage = 2;

                        UIComponent.Instance.ShowNoAnimation(UIType.UIDialog,
                                                             new UIDialogComponent.DialogData()
                                                             {
                                                                 type = UIDialogComponent.DialogData.DialogType.Commit,
                                                                 title = mArr[mTmpLanguage],
                                                                 content = mArr[mTmpLanguage + LanguageManager.mInstance.ConsumeMobileData.Count],
                                                                 contentCommit = mArr[mTmpLanguage + 2 * LanguageManager.mInstance.ConsumeMobileData.Count],
                                                                 contentCancel = string.Empty,
                                                                 actionCommit = () => { mWaitting = false; },
                                                                 actionCancel = null
                                                             });

                        TimerComponent mUpdateTimerComponent = Game.Scene.GetComponent<TimerComponent>();
                        while (mWaitting)
                        {
                            await mUpdateTimerComponent.WaitAsync(200);
                        }
                        UIComponent.Instance.Remove(UIType.UIDialog);

                    }

#if UNITY_ANDROID
                    switch (UnityEngine.Application.internetReachability)
                    {
                        case NetworkReachability.ReachableViaCarrierDataNetwork:
                            // "当前为运行商网络(2g、3g、4g)"
                            string mTmpMsg = string.Empty;
                            int mTmpLanguage = PlayerPrefsMgr.mInstance.GetInt(PlayerPrefsKeys.KEY_LANGUAGE, 2);
                            if (mTmpLanguage < 0)
                                mTmpLanguage = 0;
                            if (mTmpLanguage >= LanguageManager.mInstance.ConsumeMobileData.Count)
                                mTmpLanguage = 2;
                            mTmpMsg = string.Format(LanguageManager.mInstance.ConsumeMobileData[mTmpLanguage], mInstallPacketDownloaderComponent.remoteInstallPacketConfig.ApkSize / (1024 * 1024));
                            Game.Scene.GetComponent<UIComponent>().Show(UIType.UICarrierDataNetwork, new UICarrierDataNetworkComponent.UICarrierDataNetworkData()
                            {
                                Msg = mTmpMsg,
                                Callback = async () =>
                                {
                                    await InstallPacketHelper.DownloadInstallPacket();
                                    // Log.Debug($"下载完啦!");
                                    // todo 调起安装
                                    NativeManager.OpenApk(InstallPacketHelper.InstallPacketPath());
                                }
                            }, null, 0);
                            break;
                        case NetworkReachability.ReachableViaLocalAreaNetwork:
                            // wifi网络
                            await InstallPacketHelper.DownloadInstallPacket();
                            // todo 调起安装
                            NativeManager.OpenApk(InstallPacketHelper.InstallPacketPath());
                            return;
                    }

#elif UNITY_IOS
                    UnityEngine.Application.OpenURL(mInstallPacketDownloaderComponent.remoteInstallPacketConfig.IOSUrl);
                    Application.Quit();
#endif
                    return;
                }

                // 下载ab包
                await BundleHelper.CheckDownloadBundle();
                BundleHelper.IsDownloadBundleFinish = false;
                if (BundleHelper.NeedDownloadBundle())
                {
#if UNITY_IOS
                    bool AppStorePack = false;
#if APPStore
                    AppStorePack = true;
#endif
                    InstallPacketDownloaderComponent mInstallPacketDownloaderComponent = Game.Scene.GetComponent<InstallPacketDownloaderComponent>();
                    if (!mInstallPacketDownloaderComponent.remoteInstallPacketConfig.CheckRes && AppStorePack)
                    {
                        await BundleHelper.DownloadBundleFinish();
                    }
                    else
#endif
                    {
                        BundleDownloaderComponent mBundleDownloaderComponent = Game.Scene.GetComponent<BundleDownloaderComponent>();
                        switch (UnityEngine.Application.internetReachability)
                        {
                            case NetworkReachability.ReachableViaCarrierDataNetwork:
                                // "当前为运行商网络(2g、3g、4g)"
                                string mTmpMsg = string.Empty;
                                int mTmpLanguage = PlayerPrefsMgr.mInstance.GetInt(PlayerPrefsKeys.KEY_LANGUAGE, 2);
                                if (mTmpLanguage < 0)
                                    mTmpLanguage = 0;
                                if (mTmpLanguage >= LanguageManager.mInstance.ConsumeMobileData.Count)
                                    mTmpLanguage = 2;
                                mTmpMsg = string.Format(LanguageManager.mInstance.ConsumeMobileData[mTmpLanguage], mBundleDownloaderComponent.TotalSize / (1024 * 1024));
                                Game.Scene.GetComponent<UIComponent>().ShowNoAnimation(UIType.UICarrierDataNetwork, new UICarrierDataNetworkComponent.UICarrierDataNetworkData()
                                {
                                    Msg = mTmpMsg,
                                    Callback = async () =>
                                    {
                                        await BundleHelper.DownloadBundle();
                                        BundleHelper.IsDownloadBundleFinish = true;
                                    },
                                });
                                TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();
                                while (true)
                                {
                                    await timerComponent.WaitAsync(1000);
                                    if (BundleHelper.IsDownloadBundleFinish)
                                        break;
                                }
                                Game.Scene.GetComponent<UIComponent>().Remove(UIType.UICarrierDataNetwork);
                                break;
                            case NetworkReachability.ReachableViaLocalAreaNetwork:
                                // wifi网络
                                await BundleHelper.DownloadBundle();
                                break;
                        }
                    }
                }
                else
                {
                    await BundleHelper.DownloadBundleFinish();
                }

                Game.Hotfix.LoadHotfixAssembly();

                // 加载配置
                Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("config.unity3d");
                Game.Scene.AddComponent<ConfigComponent>();
                Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("config.unity3d");
                Game.Scene.AddComponent<OpcodeTypeComponent>();
                Game.Scene.AddComponent<MessageDispatherComponent>();

                Game.Hotfix.GotoHotfix();

                if (Define.IsAsync && BundleHelper.Loading)
                {
                    Game.EventSystem.Run(EventIdType.LoadingFinish);
                    BundleHelper.Loading = false;
                }

                // Game.EventSystem.Run(EventIdType.TestHotfixSubscribMonoEvent, "TestHotfixSubscribMonoEvent");
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            //StartCoroutine(StartGPS());
        }

        private void Update()
        {
            OneThreadSynchronizationContext.Instance.Update();
            Game.Hotfix.Update?.Invoke();
            Game.EventSystem.Update();
        }

        private void LateUpdate()
        {
            Game.Hotfix.LateUpdate?.Invoke();
            Game.EventSystem.LateUpdate();
        }

        private void OnApplicationQuit()
        {
            Game.Hotfix.OnApplicationQuit?.Invoke();
            Game.Close();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                //this.LogInfo("游戏进入了后台=》  游戏暂停 一切停止");  // Home到桌面或者打进电话等不适前台的时候触Game.Hotfix.OnApplicationPauseTrue?.Invoke();          
                Game.Hotfix.OnApplicationPauseTrue?.Invoke();
            }
            else
            {
                //this.LogInfo("游戏回到了前台  继续监听");  //回到游戏前台的时候触发 最后执行是Game.Hotfix.OnApplicationPauseFalse?.Invoke();
                Game.Hotfix.OnApplicationPauseFalse?.Invoke();
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                Game.Hotfix.OnApplicationFocusTrue?.Invoke();
            }
            else
            {
                Game.Hotfix.OnApplicationFocusFalse?.Invoke();
            }
        }

        public void changeScreen(ScreenOrientation s)
        {
            Screen.orientation = s;
        }


        void StopGPS()
        {
            Input.location.Stop();
        }

        public string GetNowGps()
        { 
            Input.location.Start(10.0f, 10.0f);
            return Input.location.lastData.latitude + "|" + Input.location.lastData.longitude;
        }

        private UniWebView _webView;
        public delegate void callback(string arg);
        public callback _callBack;
        public UniWebView showWebView(GameObject parent, string url, UniWebViewEdgeInsets insets, callback call)
        {
            _webView = CreateWebView(parent);
            _webView.OnLoadComplete += OnLoadComplete;
            _webView.OnReceivedMessage += OnReceivedMessage;
            _webView.OnWebViewShouldClose += OnWebViewShouldClose;
            _webView.OnReceivedKeyCode += OnReceivedKeyCode;
            _webView.SetUseWideViewPort(true);
            _webView.zoomEnable = true;
            _webView.insets = insets;
            _webView.Load(url);
            _callBack = call;
            return _webView;
        }

        private void OnReceivedKeyCode(UniWebView webView, int keyCode)
        {
            if (keyCode == 4)
            {
                webView.CleanCache();
                Destroy(webView);
                webView = null;
                _callBack("close");
            }
        }

        private bool OnWebViewShouldClose(UniWebView webView)
        {
            return true;
        }

        void OnLoadComplete(UniWebView webView, bool success, string errorMessage)
        {
            if (success)
            {
                webView.Show();
            }
        }

        private void OnReceivedMessage(UniWebView webView, UniWebViewMessage message)
        {
            if (message.path == "close")
            {
                webView.CleanCache();
                Destroy(webView);
                webView = null;
            }

            _callBack(message.path);
        }

        UniWebView CreateWebView(GameObject parent)
        {
            var webViewGameObject = GameObject.Find("WebView");
            if (webViewGameObject == null)
            {
                webViewGameObject = new GameObject("WebView");
            }
            webViewGameObject.transform.parent = parent.transform;
            var webView = webViewGameObject.AddComponent<UniWebView>();
            return webView;
        }

    }
}