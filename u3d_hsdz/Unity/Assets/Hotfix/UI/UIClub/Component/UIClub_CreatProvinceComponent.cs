using System;
using System.Collections.Generic;
using System.Net;
using BestHTTP;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIClub_CreatProvinceComponentSystem : AwakeSystem<UIClub_CreatProvinceComponent>
    {
        public override void Awake(UIClub_CreatProvinceComponent self)
        {
            self.Awake();
        }
    }


    public class UIClub_CreatProvinceComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mIsWalletLoad = false;
        }

        private bool mIsWalletLoad = false;

        public override void OnShow(object obj)
        {
            if (obj != null && obj is bool)
            {
                mIsWalletLoad = (bool)obj;
            }
            SetUpNav(UIType.UIClub_CreatProvince);
            List<DBArea> list = DBAreaComponent.Instance.GetProvinceList();
            GameObject container = rc.Get<GameObject>(UIClubAssitent.creatarea_area);
            GameObject prefab = rc.Get<GameObject>(UIClubAssitent.creatarea_prefab);

            AreaGridElment gridElment = new AreaGridElment(container, prefab);
            gridElment.Init(list, ItemChoose);
        }

        async void ItemChoose(GameObject go, DBArea db)
        {
            if (mIsWalletLoad==false)
            {
               // Log.Debug($"ItemChoose db = {db.id} name = {db.name_zh}");
                if (db.isNl == 1)
                {
                    //直辖市
                    Game.Scene.GetComponent<UIComponent>().Get(UIType.UIClub_Creat).GetComponent<UIClub_CreatComponent>().SetArea(db);
                    UIComponent.Instance.Remove(UIType.UIClub_CreatProvince);
                    UIComponent.Instance.Remove(UIType.UIClub_CreatArea);
                }
                else
                {
                    await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_CreatCity, db, () => { });
                }
            }
            else if (mIsWalletLoad == true)//钱包加载出来的
            {
                //await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_CreatCity, db, () => { });
                var tUI = UIComponent.Instance.Get(UIType.UIMine_WalletTransBeansBindingCard);
                if (tUI != null)
                {
                    var tCardBind = tUI.UiBaseComponent as UIMine_WalletTransBeansBindingCardComponent;
                    tCardBind.ShowWalletProvince(db);
                }
                UIComponent.Instance.Remove(UIType.UIClub_CreatProvince);
                //await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_CreatCountry, db , () => { });
            }
        }

        public override void OnHide()
        {

        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            base.Dispose();
            rc = null;
        }
    }
}



