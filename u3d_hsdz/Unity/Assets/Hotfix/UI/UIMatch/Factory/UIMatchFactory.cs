
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [UIFactory(UIType.UIMatch)]
    public class UIMatchFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject lobby = UnityEngine.Object.Instantiate(bundleGameObject);
                lobby.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(lobby);

                ui.AddUIBaseComponent<UIMatchComponent>();
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
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
            RemoveSubComponents();
        }
        public void RemoveSubComponents()
        {
            UIComponent uiComponent = Game.Scene.GetComponent<UIComponent>();
            for (int i = 0, n = subs.Count; i < n; i++)
            {
                uiComponent.RemoveSub(subs[i]);
            }
        }

        public async Task<UI> CreateAsync(Scene scene, string type, GameObject parent)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject instanObj = UnityEngine.Object.Instantiate(bundleGameObject);
                instanObj.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(instanObj);

                ui.AddUIBaseComponent<UIMatchComponent>();
                AddSubComponent(ui);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
        private List<string> subs = new List<string>();
        public void AddSubComponent(UI ui)
        {
            if (subs == null)
                subs = new List<string>();
            else subs.Clear();

            UIComponent uiComponent = Game.Scene.GetComponent<UIComponent>();
            GameObject go = ui.GameObject.transform.Find("view_main/" + UIType.UIMatch_Banner).gameObject;
            UI subUi = ComponentFactory.Create<UI, GameObject>(go);
            subUi.AddUIBaseComponent<UIMatch_BannerComponent>();
            uiComponent.Add(UIType.UIMatch_Banner, subUi);
            subs.Add(UIType.UIMatch_Banner);//
        }
    }


    [UIFactory(UIType.UIMatch_Filter)]
    public class UIMatch_FilterFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject lobby = UnityEngine.Object.Instantiate(bundleGameObject);
                lobby.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(lobby);

                ui.AddUIBaseComponent<UIMatch_FilterComponent>();
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
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }

        public async Task<UI> CreateAsync(Scene scene, string type, GameObject parent)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject instanObj = UnityEngine.Object.Instantiate(bundleGameObject);
                instanObj.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(instanObj);

                ui.AddUIBaseComponent<UIMatch_FilterComponent>();
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
            // ui.AddComponent<UILobby_FilterComponent>();
        }
    }


    #region 
    [UIFactory(UIType.UIMatch_Loading)]
    public class UIMatch_LoadingFactory : IUIFactoryExtend
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
                ui.AddUIBaseComponent<UIMatch_LoadingComponent>();
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
                ui.AddUIBaseComponent<UIMatch_LoadingComponent>();
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

    #region 
    [UIFactory(UIType.UIMatch_MttList)]
    public class UIMatch_MttListFactory : IUIFactoryExtend
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
                ui.AddUIBaseComponent<UIMatch_MttListComponent>();
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
                ui.AddUIBaseComponent<UIMatch_MttListComponent>();
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

    #region 
    [UIFactory(UIType.UIMatch_MttDetail)]
    public class UIMatch_MttDetailFactory : IUIFactoryExtend
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
                ui.AddUIBaseComponent<UIMatch_MttDetailComponent>();
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
                ui.AddUIBaseComponent<UIMatch_MttDetailComponent>();
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

            UIComponent uiComponent = Game.Scene.GetComponent<UIComponent>();
            GameObject go = ui.GameObject.transform.Find("Contents/" + UIType.UIMatch_MttDetailState).gameObject;
            UI subUi = ComponentFactory.Create<UI, GameObject>(go);
            subUi.AddUIBaseComponent<UIMatch_MttDetailStateComponent>();
            uiComponent.Add(UIType.UIMatch_MttDetailState, subUi);
            subs.Add(UIType.UIMatch_MttDetailState);

            go = ui.GameObject.transform.Find("Contents/" + UIType.UIMatch_MttDetailPlayer).gameObject;
            subUi = ComponentFactory.Create<UI, GameObject>(go);
            subUi.AddUIBaseComponent<UIMatch_MttDetailPlayerComponent>();
            uiComponent.Add(UIType.UIMatch_MttDetailPlayer, subUi);
            subs.Add(UIType.UIMatch_MttDetailPlayer);

            go = ui.GameObject.transform.Find("Contents/" + UIType.UIMatch_MttDetailReward).gameObject;
            subUi = ComponentFactory.Create<UI, GameObject>(go);
            subUi.AddUIBaseComponent<UIMatch_MttDetailRewardComponent>();
            uiComponent.Add(UIType.UIMatch_MttDetailReward, subUi);
            subs.Add(UIType.UIMatch_MttDetailReward);

            go = ui.GameObject.transform.Find("Contents/" + UIType.UIMatch_MttDetailDesk).gameObject;
            subUi = ComponentFactory.Create<UI, GameObject>(go);
            subUi.AddUIBaseComponent<UIMatch_MttDetailDeskComponent>();
            uiComponent.Add(UIType.UIMatch_MttDetailDesk, subUi);
            subs.Add(UIType.UIMatch_MttDetailDesk);

            go = ui.GameObject.transform.Find("Contents/" + UIType.UIMatch_MttDetailBlind).gameObject;
            subUi = ComponentFactory.Create<UI, GameObject>(go);
            subUi.AddUIBaseComponent<UIMatch_MttDetailBlindComponent>();
            uiComponent.Add(UIType.UIMatch_MttDetailBlind, subUi);
            subs.Add(UIType.UIMatch_MttDetailBlind);
        }
    }
    #endregion
}