
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
    public class UIClub_JoinQueryComponentSystem : AwakeSystem<UIClub_JoinQueryComponent>
    {
        public override void Awake(UIClub_JoinQueryComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_JoinQueryComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        CZScrollRect scrollcomponent;
        List<WEB2_club_search.DataElement> dataElements;
        //List<WEB2_club_search.DataElement> dataElements_1;
        int searchType = 0;

        //string strClubId;
        public void Awake()
        {

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            scrollcomponent = new CZScrollRect();
            scrollcomponent.prefab = rc.Get<GameObject>(UIClubAssitent.joinquery_clubinfo);
            scrollcomponent.scrollRect = rc.Get<GameObject>(UIClubAssitent.joinquery_scrollview).GetComponent<ScrollRect>();
            scrollcomponent.interval = 230;
            scrollcomponent.onScrollObj = OnScrollObj;
            scrollcomponent.limitNum = 12;
            scrollcomponent.Init();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIClub_JoinQuery);

            UpdateSearchStatus(string.Empty);
            scrollcomponent.Refresh(0);
            rc.Get<GameObject>(UIClubAssitent.joinquery_inputfield_search).GetComponent<InputField>().text = string.Empty;
            if (obj != null) {

                object[] arr = obj as object[];
                int searchId = (int)arr[1];
                string searchName = arr[0] as string;

                if(searchId > 0)
                {
                    rc.Get<GameObject>(UIClubAssitent.joinquery_inputfield_search).GetComponent<InputField>().text = searchName;
                    Search(searchId.ToString() , 2);
                }
            }
            else
            {
                rc.Get<GameObject>(UIClubAssitent.joinquery_inputfield_search).GetComponent<InputField>().ActivateInputField();
            }

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.joinquery_btn_search)).onClick = (go) =>
            {
                string strid = rc.Get<GameObject>(UIClubAssitent.joinquery_inputfield_search).GetComponent<InputField>().text;
                if (strid.Length < 1)
                {

                    Debug.LogError("请输入ID");
                    return;
                }
                Search(strid , 1);
            };

            rc.Get<GameObject>(UIClubAssitent.joinquery_inputfield_search).GetComponent<InputField>().onEndEdit.AddListener((v)=> {

                string strid = rc.Get<GameObject>(UIClubAssitent.joinquery_inputfield_search).GetComponent<InputField>().text;
                if (strid.Length < 1)
                {

                    Debug.LogError("请输入ID");
                    return;
                }
                Search(strid, 1);
            });

            rc.Get<GameObject>(UIClubAssitent.joinquery_inputfield_search).GetComponent<InputField>().onValueChanged.AddListener((v) => {

                UpdateSearchStatus(string.Empty);
            });

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

            //Debug.Log($"OnScrollObj -- {obj.name} index = {index}");
            string name;
            int onlineCount;
            int totalCount;
            string areaID;
            int clubId;
            int randomId = dataElements[index].randomId;

            name = dataElements[index].clubName;
            onlineCount = dataElements[index].currPe;
            totalCount = dataElements[index].total;
            areaID = dataElements[index].areaId;
            clubId = dataElements[index].clubId;

            string headerId = dataElements[index].logo;
            Head h = HeadCache.GetHead(eHeadType.CLUB, clubId.ToString());
            if (h.headId == string.Empty || h.headId != headerId)
            {
                obj.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = WebImageHelper.GetDefaultHead();
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

            obj.transform.GetChild(1).GetComponent<Text>().text = name;
            obj.transform.GetChild(2).GetComponent<Text>().text = $"ID:{randomId}";
            obj.transform.GetChild(3).GetComponent<Text>().text = $"{onlineCount}/{totalCount}";
            obj.transform.GetChild(4).GetComponent<Text>().text = $"{DBAreaComponent.Instance.QueryCity(areaID)}";

            UIEventListener.Get(obj).onClick = (o) => {

                Game.EventSystem.Run(UIClubEventIdType.CLUB_INFO, clubId);
            };

            UIEventListener.Get(obj.transform.GetChild(5).gameObject).onClick = (o) => {

                //加入俱乐部
                JoinClub(clubId);
            };
        }

        /// <summary>
        /// 搜索俱乐部
        /// </summary>
        /// <param name="strid">俱乐部ID</param>
        void Search(string strid , int t) {

            searchType = t;
            WEB2_club_search.RequestData search_req = new WEB2_club_search.RequestData()
            {
                key = strid,
                type = t
            };
            HttpRequestComponent.Instance.Send(
                                WEB2_club_search.API,
                                WEB2_club_search.Request(search_req),
                                this.SearchCall);
        }

        void SearchCall(string resData)
        {
            WEB2_club_search.ResponseData search_res = WEB2_club_search.Response(resData);
            if (search_res.status != 0) {

                UIComponent.Instance.Toast(search_res.status);
                if(searchType == 1) UpdateSearchStatus(LanguageManager.mInstance.GetLanguageForKey("club_join_0"));//抱歉,该ID没有搜索到俱乐部
                else UpdateSearchStatus(LanguageManager.mInstance.GetLanguageForKey("club_join_1"));//抱歉,该地区没有搜索到俱乐部
                return;
            }
            else
            {
                dataElements = search_res.data;
                scrollcomponent.Refresh(search_res.data.Count);
                UpdateSearchStatus($"{LanguageManager.mInstance.GetLanguageForKey("club_join_2")}({search_res.data.Count})");//搜索结果
            }
        }

        void JoinClub(int clubId)
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
                UIComponent.Instance.Remove(UIType.UIClub_JoinQuery);
                UIComponent.Instance.Remove(UIType.UIClub_Join);
                Game.EventSystem.Run(UIClubEventIdType.CLUB_LIST);
            }
            else
            {
                UIComponent.Instance.Toast(clubjoin_res.status);
            }
        }

        void UpdateSearchStatus(string str) {

            rc.Get<GameObject>(UIClubAssitent.joinquery_text_search).GetComponent<Text>().text = str;
        }



    }
}


