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
    public class UIClub_CreatCountryComponentSystem : AwakeSystem<UIClub_CreatCountryComponent>
    {
        public override void Awake(UIClub_CreatCountryComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_CreatCountryComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIClub_CreatCountry);
            int areaId = (int)obj;
            List<DBArea> list = DBAreaComponent.Instance.GetCountryList(areaId);
            GameObject container = rc.Get<GameObject>(UIClubAssitent.creatarea_area);
            GameObject prefab = rc.Get<GameObject>(UIClubAssitent.creatarea_prefab);

            AreaGridElment gridElment = new AreaGridElment(container, prefab);
            gridElment.Init(list, ItemChoose);
        }

        void ItemChoose(GameObject go , DBArea db) {

            Log.Debug($"ItemChoose db = {db.id} name = {db.name_zh}");
            Game.Scene.GetComponent<UIComponent>().Get(UIType.UIClub_Creat).GetComponent<UIClub_CreatComponent>().SetArea(db);
            UIComponent.Instance.Remove(UIType.UIClub_CreatCountry);
            UIComponent.Instance.Remove(UIType.UIClub_CreatArea);
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


