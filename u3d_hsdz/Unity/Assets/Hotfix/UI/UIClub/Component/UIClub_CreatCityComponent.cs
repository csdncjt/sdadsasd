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
    public class UIClub_CreatCityComponentSystem : AwakeSystem<UIClub_CreatCityComponent>
    {
        public override void Awake(UIClub_CreatCityComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_CreatCityComponent : UIBaseComponent
    {
        ReferenceCollector rc;
        bool mIsWalletLoad = false;
        public void Awake()
        {
            mIsWalletLoad = false;
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIClub_CreatCity);
            DBArea db = (DBArea)obj;
            List<DBArea> list = DBAreaComponent.Instance.GetCityList(db.id);
            GameObject container = rc.Get<GameObject>(UIClubAssitent.creatarea_area);
            GameObject prefab = rc.Get<GameObject>(UIClubAssitent.creatarea_prefab);

            AreaGridElment gridElment = new AreaGridElment(container, prefab);
            gridElment.Init(list, ItemChoose);

            mIsWalletLoad = db.walletLoad;
        }

        void ItemChoose(GameObject go, DBArea db)
        {
            //Log.Debug($"ItemChoose db = {db.id} name = {db.name_zh}");
            if (mIsWalletLoad == false)
            {
                Game.Scene.GetComponent<UIComponent>().Get(UIType.UIClub_Creat).GetComponent<UIClub_CreatComponent>().SetArea(db);
                UIComponent.Instance.Remove(UIType.UIClub_CreatCity);
                UIComponent.Instance.Remove(UIType.UIClub_CreatProvince);
                UIComponent.Instance.Remove(UIType.UIClub_CreatArea);
            }
            else
            {
                var tUI = UIComponent.Instance.Get(UIType.UIMine_WalletTransBeansBindingCard);
                if (tUI != null)
                {
                    var tCardBind = tUI.UiBaseComponent as UIMine_WalletTransBeansBindingCardComponent;
                    tCardBind.ShowWalletCity(db);
                }
                UIComponent.Instance.Remove(UIType.UIClub_CreatCity);
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


