using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
	[UIFactory(UIType.UIPrompt)]
	public class UIPromptFactory : IUIFactoryExtend
	{
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

				ui.AddUIBaseComponent<UIPromptComponent>();

				this.AddSubComponent(ui);

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

				ui.AddUIBaseComponent<UIPromptComponent>();

				this.AddSubComponent(ui);

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
	}

    #region 
    [UIFactory(UIType.UIPromptWifi)]
    public class UIPromptWifiFactory : IUIFactoryExtend
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
                ui.AddUIBaseComponent<UIPromptWifiComponent>();
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
                ui.AddUIBaseComponent<UIPromptWifiComponent>();
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