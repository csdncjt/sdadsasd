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
    public class UIClub_JoinRecommendComponentSystem : AwakeSystem<UIClub_JoinRecommendComponent>
    {
        public override void Awake(UIClub_JoinRecommendComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_JoinRecommendComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        CZScrollRect scrollcomponent;
        WEB2_club_hotClubs.ResponseData res;
        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            scrollcomponent = new CZScrollRect();
            scrollcomponent.prefab = rc.Get<GameObject>(UIClubAssitent.joinrecommend_clubinfo);
            scrollcomponent.scrollRect = rc.Get<GameObject>(UIClubAssitent.joinrecommend_scrollview).GetComponent<ScrollRect>();
            scrollcomponent.onScrollObj = OnScrollObj;
            scrollcomponent.interval = 230;
            scrollcomponent.limitNum = 12;
            scrollcomponent.Init();
        }

        public override void OnShow(object obj)
        {

            SetUpNav(UIType.UIClub_JoinRecommend);
            res = obj as WEB2_club_hotClubs.ResponseData;
            if (res == null)
            {
                scrollcomponent.Refresh(0);
            }
            else scrollcomponent.Refresh(res.data.list.Count);

            UITrManager.Instance.Append(UIComponent.Instance.Get(UIType.UIClub_Join).GameObject, rc.gameObject);
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
        }

        void OnScrollObj(GameObject obj, int index)
        {

            int randomId = res.data.list[index].randomId;
            string headerId = res.data.list[index].clubHeader;
            Head h = HeadCache.GetHead(eHeadType.CLUB, randomId.ToString());
            if (h.headId == string.Empty || h.headId != headerId)
            {
                WebImageHelper.GetHeadImage(headerId, (t2d) => {

                    if (obj != null && scrollcomponent.GetObjIndex(obj) == index)
                    {
                        obj.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = t2d;
                    }
                    //缓存头像
                    h.headId = headerId;
                    h.t2d = t2d;
                });
            }
            else
            {
                //已存在图片
                obj.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = h.t2d;
            }

            int clubId = res.data.list[index].clubId;
            obj.transform.GetChild(1).GetComponent<Text>().text = res.data.list[index].clubName;
            obj.transform.GetChild(2).GetComponent<Text>().text = $"ID:{res.data.list[index].randomId}";
            obj.transform.GetChild(3).GetComponent<Text>().text = $"{res.data.list[index].memberNum}/{res.data.list[index].memberLimit}";

            int areaId;
            int.TryParse(res.data.list[index].clubAreaId, out areaId);
            obj.transform.GetChild(4).GetComponent<Text>().text = $"{DBAreaComponent.Instance.QueryCity(areaId)}";


            /*UIEventListener.Get(obj).onClick = (o) => {

                Game.EventSystem.Run(UIClubEventIdType.CLUB_INFO, res.data.list[index].clubId);
              };*/

            UIEventListener.Get(obj.transform.GetChild(5).gameObject).onClick = (o) => {

                JoinClub(clubId, string.Empty);
            };
        }


        void JoinClub(int clubId, string mess)
        {

            WEB2_club_join.RequestData clubjoin_req = new WEB2_club_join.RequestData()
            {
                clubId = clubId.ToString()
            };
            HttpRequestComponent.Instance.Send(
                                WEB2_club_join.API,
                                WEB2_club_join.Request(clubjoin_req),
                                this.ClubJoinCall);
        }

        void ClubJoinCall(string resData)
        {
            // Debug.Log(resData);
            WEB2_club_join.ResponseData clubjoin_res = WEB2_club_join.Response(resData);
            if (clubjoin_res.status == 0)
            {
                UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_join_3"));//加入俱乐部成功
                UIComponent.Instance.Remove(UIType.UIClub_JoinRecommend);
                UIComponent.Instance.Remove(UIType.UIClub_Join);
                Game.EventSystem.Run(UIClubEventIdType.CLUB_LIST);
            }
            else
            {
                UIComponent.Instance.Toast(clubjoin_res.status);
            }
        }
    }
}

