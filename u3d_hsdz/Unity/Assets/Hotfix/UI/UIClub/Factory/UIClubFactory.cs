using System;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [UIFactory(UIType.UIClub)]
    public class UIClubFactory : IUIFactoryExtend
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
                ui.AddUIBaseComponent<UIClubComponent>();
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClubComponent>();
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
            //ui.AddComponent<UIClub_CreatComponent>();
            //ui.AddComponent<UIClub_JionComponent>();
            //ui.AddComponent<UIClub_QueryComponent>();
            //ui.AddComponent<UIClub_RoomCreateComponent>();
            //ui.AddComponent<UIClub_InfoComponent>();
            //ui.AddComponent<UIClub_UpdateDecComponent>();
        }
    }

    //romm creat
    [UIFactory(UIType.UIClub_RoomCreat)]
    public class UIClub_RoomCreatFactory : IUIFactoryExtend
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
                ui.AddUIBaseComponent<UIClub_RoomCreateComponent>();
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_RoomCreateComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui){}
    }


    [UIFactory(UIType.UIClub_Info)]
    public class UIClub_InfoFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_InfoComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_Creat)]
    public class UIClub_CreatFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_CreatComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_CreatArea)]
    public class UIClub_CreatAreaFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_CreatAreaComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_CreatCountry)]
    public class UIClub_CreatCountryFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_CreatCountryComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_CreatProvince)]
    public class UIClub_CreatProvinceFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_CreatProvinceComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_CreatCity)]
    public class UIClub_CreatCityFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_CreatCityComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_Join)]
    public class UIClub_JionFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_JionComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui) { }
    }


    [UIFactory(UIType.UIClub_JoinRecommend)]
    public class UIClub_JoinRecommendFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_JoinRecommendComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui) { }
    }



    [UIFactory(UIType.UIClub_JoinCountry)]
    public class UIClub_JionCountryFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_JoinCountryComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui) { }
    }


    [UIFactory(UIType.UIClub_JoinProvince)]
    public class UIClub_JoinProvinceFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_JoinProvinceComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_JoinCity)]
    public class UIClub_JoinCityFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_JoinCityComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_JoinQuery)]
    public class UIClub_QueryFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_JoinQueryComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui) { }
    }
   

    [UIFactory(UIType.UIClub_InfoIntroduce)]
    public class UIClub_UpdateDecFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_InfoIntroduceComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui) { }
    }


    [UIFactory(UIType.UIClub_InfoName)]
    public class UIClub_InfoNameFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_InfoNameComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_InfoContact)]
    public class UIClub_InfoContactFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_InfoContactComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui) { }
    }




    [UIFactory(UIType.UIClub_Tribe)]
    public class UIClub_TribeFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_TribeComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_TribeCreat)]
    public class UIClub_TribeCreatFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_TribeCreatComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_TribeJoin)]
    public class UIClub_TribeJoinFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_TribeJoinComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_TribeInfo)]
    public class UIClub_TribeInfoFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_TribeInfoComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void AddSubComponent(UI ui) { }
    }


    [UIFactory(UIType.UIClub_FundDetail)]
    public class UIClub_FundDetailFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_FundDetailComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_GameFundDetail)]
    public class UIClub_GameFundDetailFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_GameFundDetailComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_FundRecharge)]
    public class UIClub_FundRechargeFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_FundRechargeComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_GameFundTransfer)]
    public class UIClub_GameFundTransferFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_GameFundTransferComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_FundGive)]
    public class UIClub_FundGiveFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_FundGiveComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
        public void AddSubComponent(UI ui) { }
    }


    [UIFactory(UIType.UIClub_FundGiveSub)]
    public class UIClub_FundGiveSubFactory : IUIFactoryExtend
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
                ui.AddUIBaseComponent<UIClub_FundGiveSubComponent>();
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_FundGiveSubComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_Mlist)]
    public class UIClub_MlistFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_MlistComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_MlistInfo)]
    public class UIClub_MlistInfoFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_MlistInfoComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
        public void AddSubComponent(UI ui) { }
    }



    [UIFactory(UIType.UIClub_MlistRemarks)]
    public class UIClub_MlistRemarksFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_MlistRemarksComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_Trans)]
    public class UIClub_TransFactory : IUIFactoryExtend
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
            return null;
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_TransComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_PlanRoomList)]
    public class UIClub_PlanRoomListFactory : IUIFactoryExtend
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
                ui.AddUIBaseComponent<UIClub_PlanRoomListComponent>();
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_PlanRoomListComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
        public void AddSubComponent(UI ui) { }
    }

    [UIFactory(UIType.UIClub_RoomList)]
    public class UIClub_RoomListFactory : IUIFactoryExtend
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
                ui.AddUIBaseComponent<UIClub_RoomListComponent>();
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
                GameObject club = UnityEngine.Object.Instantiate(bundleGameObject);
                club.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(club);
                ui.AddUIBaseComponent<UIClub_RoomListComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
        public void AddSubComponent(UI ui) { }
    }
}