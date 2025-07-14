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
    public class UIClub_TribeComponentAwakeSystem : AwakeSystem<UIClub_TribeComponent>
    {
        public override void Awake(UIClub_TribeComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_TribeComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        string sourceId;
        CZScrollRect scrollcomponent;
        WEB2_tribe_mine.ResponseData reslist_cache;
        public void Awake()
        {

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            scrollcomponent = new CZScrollRect();
            scrollcomponent.prefab = rc.Get<GameObject>(UIClubAssitent.tribe_tribeinfo);
            scrollcomponent.scrollRect = rc.Get<GameObject>(UIClubAssitent.tribe_scrollview).GetComponent<ScrollRect>();
            scrollcomponent.onScrollObj = OnScrollObj;
            scrollcomponent.interval = 300;
            scrollcomponent.Init();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIClub_Tribe);

            object[] arr = obj as object[];
            int iClubId = (int)arr[0];
            WEB2_tribe_mine.ResponseData alliview_res = arr[1] as WEB2_tribe_mine.ResponseData;
            UpdateViewInfo(alliview_res);

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.tribe_btn_tribejoin)).onClick = async(go) =>
            {

                if (reslist_cache.data.canCreateTribe == 0) {

                    UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_tribe_0"));//您已创建or加入联盟，请勿重复点击
                    return;
                }

                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_TribeJoin, iClubId, () => {});
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.tribe_btn_tribecreat)).onClick = async(go) =>
            {

                if (reslist_cache.data.canCreateTribe == 0)
                {
                    UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_tribe_0"));//您已创建or加入联盟，请勿重复点击
                    return;
                }
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_TribeCreat, iClubId, () => {});
            };

            UITrManager.Instance.Append(UIComponent.Instance.Get(UIType.UIClub_Info).GameObject, rc.gameObject);

        }

        public override void OnHide()
        {
            UITrManager.Instance.Remove(rc.gameObject);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            base.Dispose();
            UITrManager.Instance.Remove(rc.gameObject);
            rc = null;
            scrollcomponent.Dispose();
            scrollcomponent = null;
            reslist_cache = null;
        }

        void OnScrollObj(GameObject obj, int index)
        {

            string tribeName = reslist_cache.data.tribeName;
            int tribeId = reslist_cache.data.tribeId;
            obj.transform.GetChild(2).gameObject.GetComponent<Text>().text = tribeName;
            obj.transform.GetChild(3).gameObject.GetComponent<Text>().text = $"ID:{reslist_cache.data.randomId}";
            UIEventListener.Get(obj, tribeId).onIntClick = (go, id) =>
            {
               Log.Debug($"点击联盟{id}");
               Game.EventSystem.Run(UIClubEventIdType.CLUB_TRIBE_INFO, id, tribeName);
            };
        }

        public void UpdateViewInfo(WEB2_tribe_mine.ResponseData res) {

            //一个俱乐部只存在一个联盟
            reslist_cache = res;
            //res.data.canCreateTribe = 1;
            if (res.data.canCreateTribe == 0 && res.data.randomId != 0)
            {
                scrollcomponent.Refresh(1);
                rc.Get<GameObject>(UIClubAssitent.tribe_unlist).SetActive(false);
                rc.Get<GameObject>(UIClubAssitent.tribe_scrollview).transform.parent.gameObject.SetActive(true);
            }
            else
            {
                scrollcomponent.Refresh(0);
                rc.Get<GameObject>(UIClubAssitent.tribe_unlist).SetActive(true);
                rc.Get<GameObject>(UIClubAssitent.tribe_scrollview).transform.parent.gameObject.SetActive(false);
            }
        }
    }
}



