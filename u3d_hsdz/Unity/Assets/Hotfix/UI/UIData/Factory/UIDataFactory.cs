using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [UIFactory(UIType.UIData)]
    public class UIDataFactory : IUIFactoryExtend
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
                ui.AddUIBaseComponent<UIDataComponent>();

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
                ui.AddUIBaseComponent<UIDataComponent>();

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

    [UIFactory(UIType.UIData_Terminol)]
    public class UIData_TerminolFactory : IUIFactoryExtend
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
                ui.AddUIBaseComponent<UIData_TerminolComponent>();

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
                ui.AddUIBaseComponent<UIData_TerminolComponent>();

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
    //#region 
    //[UIFactory(UIType.UIData_ShowProblem)]
    //public class UIData_ShowProblemFactory : IUIFactoryExtend
    //{
    //    public UI Create(Scene scene, string type, GameObject gameObject)
    //    {
    //        try
    //        {
    //            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
    //            resourcesComponent.LoadBundle($"{type}.unity3d");
    //            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
    //            GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
    //            club.layer = LayerMask.NameToLayer(LayerNames.UI);
    //            UI ui = ComponentFactory.Create<UI, GameObject>(club);
    //            ui.AddUIBaseComponent<UIData_ShowProblemComponent>();
    //            AddSubComponent(ui);
    //            return ui;
    //        }
    //        catch (Exception e)
    //        {
    //            Log.Error(e);
    //            return null;
    //        }
    //    }

    //    public async Task<UI> CreateAsync(Scene scene, string type, GameObject parent)
    //    {
    //        try
    //        {
    //            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
    //            await resourcesComponent.LoadBundleAsync($"{type}.unity3d");
    //            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
    //            GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
    //            club.layer = LayerMask.NameToLayer(LayerNames.UI);
    //            UI ui = ComponentFactory.Create<UI, GameObject>(club);
    //            ui.AddUIBaseComponent<UIData_ShowProblemComponent>();
    //            AddSubComponent(ui);
    //            return ui;
    //        }
    //        catch (Exception e)
    //        {
    //            Log.Error(e);
    //            return null;
    //        }
    //    }

    //    public void Remove(string type)
    //    {
    //        RemoveSubComponents();
    //        ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
    //    }
    //    public void RemoveSubComponents()
    //    {
    //        UIComponent uiComponent = Game.Scene.GetComponent<UIComponent>();
    //        for (int i = 0, n = subs.Count; i < n; i++)
    //        {
    //            uiComponent.RemoveSub(subs[i]);
    //        }
    //    }

    //    private List<string> subs = new List<string>();
    //    public void AddSubComponent(UI ui)
    //    {
    //        if (subs == null)
    //            subs = new List<string>();
    //        else
    //            subs.Clear();
    //    }
    //}
    //#endregion
}