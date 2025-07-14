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
    public class UIClubComponentSystem : AwakeSystem<UIClubComponent>
    {
        public override void Awake(UIClubComponent self)
        {
            self.Awake();
        }
    }

    public class UIClubComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        WEB2_club_list.ResponseData resClubList;//缓存最近一次的信息
        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            GameObject baseView = rc.gameObject;
            NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
            float realTop = safeArea.top * 1242 / safeArea.width;
            baseView.GetComponent<VerticalLayoutGroup>().padding.top = (int)realTop;
            float realBottom = safeArea.bottom * 1242 / safeArea.width;
            baseView.GetComponent<VerticalLayoutGroup>().padding.bottom = (int)realBottom;
        }

        public override void OnShow(object obj)
        {

            //默认隐藏按钮
            rc.Get<GameObject>(UIClubAssitent.uiclub_creatjoin).SetActive(false);
            rc.Get<GameObject>(UIClubAssitent.uiclub_btn_kaiju).SetActive(false);
            rc.Get<GameObject>(UIClubAssitent.uiclub_btn_jihua).SetActive(false);
            rc.Get<GameObject>(UIClubAssitent.uiclub_clubinfo).SetActive(false);

            //获取俱乐部列表
            GetClubListInfo();
            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.uiclub_btn_kaiju)).onClick = async(go) => {

                await UIComponent.Instance.ShowAsyncNoAnimation(UIType.UIClub_RoomCreat, 0, () => {});
            };
            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.uiclub_btn_jihua)).onClick = (go) => {

                UIComponent.Instance.ShowNoAnimation(UIType.UIClub_PlanRoomList);
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.uiclub_btn_creat)).onClick = async (go) => {

                if (resClubList.data.canCreateClub == 1)
                {
                    await UIComponent.Instance.ShowAsyncNoAnimation(UIType.UIClub_Creat, null, () => { });
                }
                else
                {
                    UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_0"));//您创建的俱乐部正在审核中，不可创建其他俱乐部。
                }
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.uiclub_btn_join)).onClick = (go) => {

                if (resClubList.data.canCreateClub == 1)
                {

                    Game.EventSystem.Run(UIClubEventIdType.CLUB_HOT);
                }
                else
                {
                    UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_1"));//您创建的俱乐部正在审核中，不可加入其他俱乐部。
                }
            };

            UITrManager.Instance.AppendAtRoot(rc.gameObject);
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
            UITrManager.Instance.Remove(rc.gameObject);
            rc = null;
            resClubList = null;
        }

        void UpdateClubInfo(GameObject obj, int index)
        {

            string randomId = resClubList.data.list[index].clubId.ToString();
            string headerId = resClubList.data.list[index].logo;

            Head h = HeadCache.GetHead(eHeadType.CLUB, randomId);
            if (h.headId == string.Empty || h.headId != headerId)
            {
                //重新加载
                //Debug.Log($"重新加载 {headerId}");
                WebImageHelper.GetHeadImage(headerId, (t2d) => {

                    obj.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = t2d;
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

            var name = resClubList.data.list[index].clubName;
            obj.transform.GetChild(2).gameObject.GetComponent<Text>().text = name;
            obj.transform.GetChild(3).gameObject.GetComponent<Text>().text = $"ID:{resClubList.data.list[index].randomId}";
            obj.transform.GetChild(4).gameObject.GetComponent<Text>().text = $"{resClubList.data.list[index].currPe}/{resClubList.data.list[index].total}";
            obj.transform.GetChild(5).gameObject.GetComponent<Text>().text = $"{DBAreaComponent.Instance.QueryCity(resClubList.data.list[index].areaId)}"; 
            int iClubId = resClubList.data.list[index].clubId;
            UIEventListener.Get(obj).onClick = (go) =>
            {
                Log.Debug($"点击俱乐部{iClubId}");
                Game.EventSystem.Run(UIClubEventIdType.CLUB_INFO, iClubId);
            };
        }

        /// <summary>
        /// 获取俱乐部列表 
        /// </summary>
        public void GetClubListInfo() {

            WEB2_club_list.RequestData clublist_req = new WEB2_club_list.RequestData();
            HttpRequestComponent.Instance.Send(
                                WEB2_club_list.API,
                                WEB2_club_list.Request(clublist_req),
                                this.ClubListCall);
        }

        /// <summary>
        /// 俱乐部列表信息回调
        /// </summary>
        /// <param name="resData"></param>
        void ClubListCall(string resData) {

            if (IsDisposed)
            {
                return;//disposed后防止异步执行
            }

            // Debug.Log(resData);
            WEB2_club_list.ResponseData clublist_res = WEB2_club_list.Response(resData);
            if (clublist_res.status != 0) {

                Log.Debug($"error WEB_club_list status = {clublist_res.status} meg = {clublist_res.msg}");
                UIComponent.Instance.Toast(clublist_res.status);
                return;
            }
            resClubList = clublist_res;
            //resClubList.data.list = null;
            //resClubList.data.canCreateClub = 1;
            if (clublist_res.data.list == null || clublist_res.data.list.Count == 0) {

                //显示创建、加入按钮
                rc.Get<GameObject>(UIClubAssitent.uiclub_creatjoin).SetActive(true);
                rc.Get<GameObject>(UIClubAssitent.uiclub_btn_kaiju).SetActive(false);
                rc.Get<GameObject>(UIClubAssitent.uiclub_btn_jihua).SetActive(false);
                rc.Get<GameObject>(UIClubAssitent.uiclub_clubinfo).SetActive(false);
            }
            else
            {
                //隐藏创建、加入按钮
                //显示列表
                rc.Get<GameObject>(UIClubAssitent.uiclub_clubinfo).SetActive(true);
                rc.Get<GameObject>(UIClubAssitent.uiclub_creatjoin).SetActive(false);

                UpdateClubInfo(rc.Get<GameObject>(UIClubAssitent.uiclub_clubinfo), 0);

                //权限控制开局按钮
                if (clublist_res.data.canCreate != 0)
                {
                    rc.Get<GameObject>(UIClubAssitent.uiclub_btn_kaiju).SetActive(true);
                    rc.Get<GameObject>(UIClubAssitent.uiclub_btn_jihua).SetActive(true);
                }
                else
                {
                    rc.Get<GameObject>(UIClubAssitent.uiclub_btn_kaiju).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.uiclub_btn_jihua).SetActive(false);
                }
            }
        }
    }
}
