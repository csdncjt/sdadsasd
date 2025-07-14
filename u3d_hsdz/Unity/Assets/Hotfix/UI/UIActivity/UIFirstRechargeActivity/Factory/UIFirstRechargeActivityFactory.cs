using System;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [UIFactory(UIType.UIFirstRechargeActivity)]
    public class UIFirstRechargeActivityFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            try
            {
                ResourcesComponent resourcesComponent = Game.Scene.ModelScene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject go = UnityEngine.Object.Instantiate(bundleGameObject);
                go.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(go);

                ui.AddUIBaseComponent<UIFirstRechargeActivityComponent>();

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
                GameObject go = UnityEngine.Object.Instantiate(bundleGameObject);
                go.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(go);

                ui.AddUIBaseComponent<UIFirstRechargeActivityComponent>();

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
}