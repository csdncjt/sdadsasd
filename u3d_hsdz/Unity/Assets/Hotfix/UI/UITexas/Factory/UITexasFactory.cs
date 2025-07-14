using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
	[UIFactory(UIType.UITexas)]
	public class UITexasFactory : IUIFactoryExtend
	{
		private List<string> subs = new List<string>();

		public UI Create(Scene scene, string type, GameObject gameObject)
		{
			try
			{
				ResourcesComponent resourcesComponent = Game.Scene.ModelScene.GetComponent<ResourcesComponent>();
				resourcesComponent.LoadBundle($"{type}.unity3d");
				GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
				GameObject login = UnityEngine.Object.Instantiate(bundleGameObject);
				login.layer = LayerMask.NameToLayer(LayerNames.UI);
				UI ui = ComponentFactory.Create<UI, GameObject>(login);

				ui.AddUIBaseComponent<UITexasComponent>();

				AddSubComponent(ui);

				return ui;
			}
			catch (Exception e)
			{
				Log.Error(e);
				return null;
			}
		}

		public void Remove(string type)
		{
			RemoveSubComponents();
			Game.Scene.ModelScene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
		}

		public async Task<UI> CreateAsync(Scene scene, string type, GameObject parent)
		{
			try
			{
				ResourcesComponent resourcesComponent = Game.Scene.ModelScene.GetComponent<ResourcesComponent>();
				await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
				GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
				GameObject login = UnityEngine.Object.Instantiate(bundleGameObject);
				login.layer = LayerMask.NameToLayer(LayerNames.UI);
				UI ui = ComponentFactory.Create<UI, GameObject>(login);

				ui.AddUIBaseComponent<UITexasComponent>();

				AddSubComponent(ui);

				return ui;
			}
			catch (Exception e)
			{
				Log.Error(e);
				return null;
			}
		}

		public void AddSubComponent(UI ui)
		{
			if (subs == null)
				subs = new List<string>();
			else
				subs.Clear();

			UIComponent uiComponent = Game.Scene.GetComponent<UIComponent>();
			GameObject go = ui.GameObject.transform.Find(UIType.UIAddChips).gameObject;
			UI subUi = ComponentFactory.Create<UI, GameObject>(go);
			subUi.AddUIBaseComponent<UIAddChipsComponent>();
			uiComponent.Add(UIType.UIAddChips, subUi);
			subs.Add(UIType.UIAddChips);

			go = ui.GameObject.transform.Find(UIType.UIOperation).gameObject;
			subUi = ComponentFactory.Create<UI, GameObject>(go);
			subUi.AddUIBaseComponent<UIOperationComponent>();
			uiComponent.Add(UIType.UIOperation, subUi);
			subs.Add(UIType.UIOperation);

            go = ui.GameObject.transform.Find(UIType.UIAutoOperation).gameObject;
            subUi = ComponentFactory.Create<UI, GameObject>(go);
            subUi.AddUIBaseComponent<UIAutoOperationComponent>();
            uiComponent.Add(UIType.UIAutoOperation, subUi);
            subs.Add(UIType.UIAutoOperation);

			go = ui.GameObject.transform.Find(UIType.UIPause).gameObject;
			subUi = ComponentFactory.Create<UI, GameObject>(go);
			subUi.AddUIBaseComponent<UIPauseComponent>();
			uiComponent.Add(UIType.UIPause, subUi);
			subs.Add(UIType.UIPause);

            go = ui.GameObject.transform.Find(UIType.UIVoice).gameObject;
            subUi = ComponentFactory.Create<UI, GameObject>(go);
            subUi.AddUIBaseComponent<UIVoiceComponent>();
            uiComponent.Add(UIType.UIVoice, subUi);
            subs.Add(UIType.UIVoice);

            go = ui.GameObject.transform.Find(UIType.UIEmojiPanel).gameObject;
            subUi = ComponentFactory.Create<UI, GameObject>(go);
            subUi.AddUIBaseComponent<UIEmojiPanelComponent>();
            uiComponent.Add(UIType.UIEmojiPanel, subUi);
            subs.Add(UIType.UIEmojiPanel);

            go = ui.GameObject.transform.Find(UIType.UIStatusBar).gameObject;
            subUi = ComponentFactory.Create<UI, GameObject>(go);
            subUi.AddUIBaseComponent<UIStatusBarComponent>();
            uiComponent.Add(UIType.UIStatusBar, subUi);
            subs.Add(UIType.UIStatusBar);

            go = ui.GameObject.transform.Find(UIType.UIAnimoji).gameObject;
            subUi = ComponentFactory.Create<UI, GameObject>(go);
            subUi.AddUIBaseComponent<UIAnimojiComponent>();
            uiComponent.Add(UIType.UIAnimoji, subUi);
            subs.Add(UIType.UIAnimoji);

            go = ui.GameObject.transform.Find(UIType.UIBigWinAnimation).gameObject;
            subUi = ComponentFactory.Create<UI, GameObject>(go);
            subUi.AddUIBaseComponent<UIBigWinAnimationComponent>();
            uiComponent.Add(UIType.UIBigWinAnimation, subUi);
            subs.Add(UIType.UIBigWinAnimation);

            go = ui.GameObject.transform.Find(UIType.UIMTTTime).gameObject;
            subUi = ComponentFactory.Create<UI, GameObject>(go);
            subUi.AddUIBaseComponent<UIMTTTimeComponent>();
            uiComponent.Add(UIType.UIMTTTime, subUi);
            subs.Add(UIType.UIMTTTime);
        }

		public void RemoveSubComponents()
		{
			UIComponent uiComponent = Game.Scene.GetComponent<UIComponent>();
			for (int i = 0, n = subs.Count; i < n; i++)
			{
				uiComponent.RemoveSub(subs[i]);
			}
		}
	}

    [UIFactory(UIType.UITexasHistory)]
    public class UITexasHistoryFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UITexasHistoryComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public async Task<UI> CreateAsync(Scene scene, string type, GameObject parent)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UITexasHistoryComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void Remove(string type)
        {
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }

        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UITexasReport)]
    public class UIReportFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UITexasReportComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public async Task<UI> CreateAsync(Scene scene, string type, GameObject parent)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UITexasReportComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void Remove(string type)
        {
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }

        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UITexasInfo)]
    public class UITexasInfoFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UITexasInfoComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public async Task<UI> CreateAsync(Scene scene, string type, GameObject parent)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UITexasInfoComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void Remove(string type)
        {
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }

        public void AddSubComponent(UI ui) { }
    }


    #region  修改备注名
    [UIFactory(UIType.UIUserRemarks)]
    public class UIUserRemarksFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIUserRemarksComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public async Task<UI> CreateAsync(Scene scene, string type, GameObject parent)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIUserRemarksComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void Remove(string type)
        {
            RemoveSubComponents();
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }
        public void RemoveSubComponents()
        {
            UIComponent uiComponent = Game.Scene.GetComponent<UIComponent>();
            for (int i = 0, n = subs.Count; i < n; i++)
            {
                uiComponent.RemoveSub(subs[i]);
            }
        }

        private List<string> subs = new List<string>();
        public void AddSubComponent(UI ui)
        {
            if (subs == null)
                subs = new List<string>();
            else
                subs.Clear();
        }
    }
    #endregion

    #region 规则
    [UIFactory(UIType.UITexasRule)]
    public class UITexasRuleFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UITexasRuleComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public async Task<UI> CreateAsync(Scene scene, string type, GameObject parent)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UITexasRuleComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void Remove(string type)
        {
            RemoveSubComponents();
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }
        public void RemoveSubComponents()
        {
            UIComponent uiComponent = Game.Scene.GetComponent<UIComponent>();
            for (int i = 0, n = subs.Count; i < n; i++)
            {
                uiComponent.RemoveSub(subs[i]);
            }
        }

        private List<string> subs = new List<string>();
        public void AddSubComponent(UI ui)
        {
            if (subs == null)
                subs = new List<string>();
            else
                subs.Clear();
        }
    }
    #endregion

    #region 房主功能
    [UIFactory(UIType.UIOwner)]
    public class UIOwnerFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIOwnerComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public async Task<UI> CreateAsync(Scene scene, string type, GameObject parent)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIOwnerComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void Remove(string type)
        {
            RemoveSubComponents();
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }
        public void RemoveSubComponents() { }

        public void AddSubComponent(UI ui) { }
    }
    #endregion

    #region 保险
    [UIFactory(UIType.UIInsurance)]
    public class UIInsuranceFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIInsuranceComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public async Task<UI> CreateAsync(Scene scene, string type, GameObject parent)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIInsuranceComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void Remove(string type)
        {
            RemoveSubComponents();
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }
        public void RemoveSubComponents() { }

        public void AddSubComponent(UI ui) { }
    }
    #endregion

    #region JackPot信息
    [UIFactory(UIType.UIJackPotInfo)]
    public class UIJackPotInfoFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIJackPotInfoComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public async Task<UI> CreateAsync(Scene scene, string type, GameObject parent)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIJackPotInfoComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void Remove(string type)
        {
            RemoveSubComponents();
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }
        public void RemoveSubComponents() { }

        public void AddSubComponent(UI ui) { }
    }
    #endregion

    #region 个性设置
    [UIFactory(UIType.UITexasSetting)]
    public class UITexasSettingFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UITexasSettingComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public async Task<UI> CreateAsync(Scene scene, string type, GameObject parent)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UITexasSettingComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void Remove(string type)
        {
            RemoveSubComponents();
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }
        public void RemoveSubComponents() { }

        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIHongBaoYu)]
    public class UIHongBaoYuFactory : IUIFactoryExtend
    {
        private List<string> subs = new List<string>();

        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject dataUI = UnityEngine.Object.Instantiate(bundleGameObject);
                dataUI.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(dataUI);
                ui.AddUIBaseComponent<UIHongBaoYuComponent>();


                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void Remove(string type)
        {
            RemoveSubComponents();
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }

        public async Task<UI> CreateAsync(Scene scene, string type, GameObject parent)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject dataUI = UnityEngine.Object.Instantiate(bundleGameObject);
                dataUI.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(dataUI);
                ui.AddUIBaseComponent<UIHongBaoYuComponent>();

                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui)
        {
        }

        public void RemoveSubComponents()
        {
        }
    }
    #endregion

    #region 投诉反馈
    [UIFactory(UIType.UITexasComplaint)]
    public class UITexasComplaintFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UITexasComplaintComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public async Task<UI> CreateAsync(Scene scene, string type, GameObject parent)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UITexasComplaintComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void Remove(string type)
        {
            RemoveSubComponents();
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }
        public void RemoveSubComponents()
        {
            UIComponent uiComponent = Game.Scene.GetComponent<UIComponent>();
            for (int i = 0, n = subs.Count; i < n; i++)
            {
                uiComponent.RemoveSub(subs[i]);
            }
        }

        private List<string> subs = new List<string>();
        public void AddSubComponent(UI ui)
        {
            if (subs == null)
                subs = new List<string>();
            else
                subs.Clear();
        }
    }
    #endregion

    #region 跑马灯小游戏

    [UIFactory(UIType.UITexasMarqueeGame)]
    public class UITexasMarqueeGameFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UITexasMarqueeGameComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public async Task<UI> CreateAsync(Scene scene, string type, GameObject parent)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UITexasMarqueeGameComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void Remove(string type)
        {
            RemoveSubComponents();
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }
        public void RemoveSubComponents()
        {
            UIComponent uiComponent = Game.Scene.GetComponent<UIComponent>();
            for (int i = 0, n = subs.Count; i < n; i++)
            {
                uiComponent.RemoveSub(subs[i]);
            }
        }

        private List<string> subs = new List<string>();
        public void AddSubComponent(UI ui)
        {
            if (subs == null)
                subs = new List<string>();
            else
                subs.Clear();
        }
    }

    #endregion

    #region MTT

    [UIFactory(UIType.UIMTT_Ranking)]
    public class UIMTTRankingFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject go = UnityEngine.Object.Instantiate(bundleGameObject);
                go.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(go);
                ui.AddUIBaseComponent<UIMTTRankingComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public async Task<UI> CreateAsync(Scene scene, string type, GameObject parent)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject go = UnityEngine.Object.Instantiate(bundleGameObject);
                go.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(go);
                ui.AddUIBaseComponent<UIMTTRankingComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void Remove(string type)
        {
            RemoveSubComponents();
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }
        public void RemoveSubComponents()
        {
            UIComponent uiComponent = Game.Scene.GetComponent<UIComponent>();
            for (int i = 0, n = subs.Count; i < n; i++)
            {
                uiComponent.RemoveSub(subs[i]);
            }
        }

        private List<string> subs = new List<string>();
        public void AddSubComponent(UI ui)
        {
            if (subs == null)
                subs = new List<string>();
            else
                subs.Clear();
        }
    }

    [UIFactory(UIType.UITexasReportMTT)]
    public class UITexasReportMTTFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject go = UnityEngine.Object.Instantiate(bundleGameObject);
                go.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(go);
                ui.AddUIBaseComponent<UITexasReportMTTComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public async Task<UI> CreateAsync(Scene scene, string type, GameObject parent)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject go = UnityEngine.Object.Instantiate(bundleGameObject);
                go.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(go);
                ui.AddUIBaseComponent<UITexasReportMTTComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void Remove(string type)
        {
            RemoveSubComponents();
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }
        public void RemoveSubComponents()
        {
            UIComponent uiComponent = Game.Scene.GetComponent<UIComponent>();
            for (int i = 0, n = subs.Count; i < n; i++)
            {
                uiComponent.RemoveSub(subs[i]);
            }
        }

        private List<string> subs = new List<string>();
        public void AddSubComponent(UI ui)
        {
            if (subs == null)
                subs = new List<string>();
            else
                subs.Clear();
        }
    }

    #endregion

    #region 测试

    [UIFactory(UIType.UITexasTest)]
    public class UITexasTestFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<SelectPanel>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public async Task<UI> CreateAsync(Scene scene, string type, GameObject parent)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<SelectPanel>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void Remove(string type)
        {
            RemoveSubComponents();
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }
        public void RemoveSubComponents()
        {
            UIComponent uiComponent = Game.Scene.GetComponent<UIComponent>();
            for (int i = 0, n = subs.Count; i < n; i++)
            {
                uiComponent.RemoveSub(subs[i]);
            }
        }

        private List<string> subs = new List<string>();
        public void AddSubComponent(UI ui)
        {
            if (subs == null)
                subs = new List<string>();
            else
                subs.Clear();
        }
    }

    #endregion

    #region 牛仔

    [UIFactory(UIType.UICowBoy)]
    public class UICowBoyFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UICowboy>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public async Task<UI> CreateAsync(Scene scene, string type, GameObject parent)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UICowboy>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void Remove(string type)
        {
            RemoveSubComponents();
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }
        public void RemoveSubComponents()
        {
            UIComponent uiComponent = Game.Scene.GetComponent<UIComponent>();
            for (int i = 0, n = subs.Count; i < n; i++)
            {
                uiComponent.RemoveSub(subs[i]);
            }
        }

        private List<string> subs = new List<string>();
        public void AddSubComponent(UI ui)
        {
            if (subs == null)
                subs = new List<string>();
            else
                subs.Clear();
        }
    }

    #endregion
}