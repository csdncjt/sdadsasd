using System;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    #region 活动弹窗
    [UIFactory(UIType.UILobby_Activity)]
    public class UILobby_ActivityFactory : IUIFactoryExtend
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
                ui.AddUIBaseComponent<UILobby_ActivityComponent>();
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
                ui.AddUIBaseComponent<UILobby_ActivityComponent>();
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

    #region 系统USDT红包弹窗
    [UIFactory(UIType.UISystemHongbao)]
    public class UISystemHongbaoFactory : IUIFactoryExtend
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
                ui.AddUIBaseComponent<UISystemHongbaoComponent>();
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
                ui.AddUIBaseComponent<UISystemHongbaoComponent>();
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

    #region 收藏网页弹窗
    [UIFactory(UIType.UICollectPageAlert)]
    public class UICollectPageAlertFactory : IUIFactoryExtend
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
                ui.AddUIBaseComponent<UICollectPageAlertComponent>();
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
                ui.AddUIBaseComponent<UICollectPageAlertComponent>();
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
}