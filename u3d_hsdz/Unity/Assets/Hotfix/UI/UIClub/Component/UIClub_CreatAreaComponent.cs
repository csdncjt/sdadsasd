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
    public class UIClub_CreatAreaComponentSystem : AwakeSystem<UIClub_CreatAreaComponent>
    {
        public override void Awake(UIClub_CreatAreaComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_CreatAreaComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        public void Awake()
        {

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
        }

        public override void OnShow(object obj)
        {

            SetUpNav(UIType.UIClub_CreatArea);

            UpdateLanuage(rc.Get<GameObject>(UIClubAssitent.join_btn_area0) , 0);
            UpdateLanuage(rc.Get<GameObject>(UIClubAssitent.join_btn_area1), 1);
            UpdateLanuage(rc.Get<GameObject>(UIClubAssitent.join_btn_area2), 2);
            UpdateLanuage(rc.Get<GameObject>(UIClubAssitent.join_btn_area3), 3);
            UpdateLanuage(rc.Get<GameObject>(UIClubAssitent.join_btn_area4), 4);
            UpdateLanuage(rc.Get<GameObject>(UIClubAssitent.join_btn_area5), 5);
            UpdateLanuage(rc.Get<GameObject>(UIClubAssitent.join_btn_area6), 6);

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.join_btn_area0)).onClick = async (go) =>
            {
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_CreatProvince, null, () => { });
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.join_btn_area1)).onClick = async (go) =>
            {
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_CreatCountry, 1, () => { });
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.join_btn_area2)).onClick = async (go) =>
            {
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_CreatCountry, 2, () => { });
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.join_btn_area3)).onClick = async (go) =>
            {
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_CreatCountry, 3, () => { });
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.join_btn_area4)).onClick = async (go) =>
            {
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_CreatCountry, 4, () => { });
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.join_btn_area5)).onClick = async (go) =>
            {
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_CreatCountry, 5, () => { });
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.join_btn_area6)).onClick = async (go) =>
            {
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_CreatCountry, 6, () => { });
            };
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

        void UpdateLanuage(GameObject g , int areaId) {

            Text t = g.transform.GetChild(0).GetComponent<Text>();
            t.text = LanguageManager.mInstance.GetLanguageForKey($"area{areaId}");
        }
    }
}


