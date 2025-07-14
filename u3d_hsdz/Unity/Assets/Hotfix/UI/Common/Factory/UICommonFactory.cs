using System;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [UIFactory(UIType.Root)]
    public class UIBaseComponentFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject segment = UnityEngine.Object.Instantiate(bundleGameObject);
                segment.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(segment);

                ui.AddUIBaseComponent<UIBaseComponent>();

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
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject segment = UnityEngine.Object.Instantiate(bundleGameObject);
                segment.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(segment);

                ui.AddUIBaseComponent<UIBaseComponent>();

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
            // Debug.Log($"aaabbbcccddd");
        }
    }
}