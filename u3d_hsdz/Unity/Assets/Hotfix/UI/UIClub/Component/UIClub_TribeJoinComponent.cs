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
    public class UIClub_TribeJoinAwakeSystem : AwakeSystem<UIClub_TribeJoinComponent>
    {
        public override void Awake(UIClub_TribeJoinComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_TribeJoinComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        CZScrollRect scrollcomponent;
        WEB2_tribe_search.ResponseData res;
        int iClubId;
        public void Awake()
        {

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            scrollcomponent = new CZScrollRect();
            scrollcomponent.prefab = rc.Get<GameObject>(UIClubAssitent.tribejoin_info);
            scrollcomponent.scrollRect = rc.Get<GameObject>(UIClubAssitent.tribejoin_scrollview).GetComponent<ScrollRect>();
            scrollcomponent.interval = 230;
            scrollcomponent.onScrollObj = OnScrollObj;
            scrollcomponent.Init();
        }

        public override void OnShow(object obj)
        {

            SetUpNav(UIType.UIClub_TribeJoin);
            iClubId = (int)obj;

            //激活输入框
            rc.Get<GameObject>(UIClubAssitent.tribejoin_inputfield_search).GetComponent<InputField>().ActivateInputField();

            //点击搜索联盟
            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.tribejoin_btn_search)).onClick = (go) =>
            {
                QueryTribe();
            };

            rc.Get<GameObject>(UIClubAssitent.tribejoin_inputfield_search).GetComponent<InputField>().onEndEdit.AddListener((v) => {

                string strid = rc.Get<GameObject>(UIClubAssitent.tribejoin_inputfield_search).GetComponent<InputField>().text;
                if (strid.Length < 1) return;
                QueryTribe();
            });

            rc.Get<GameObject>(UIClubAssitent.tribejoin_inputfield_search).GetComponent<InputField>().onValueChanged.AddListener((v) => {

                UpdateSearchStatus(string.Empty);
            });

            UITrManager.Instance.Append(UIComponent.Instance.Get(UIType.UIClub_Tribe).GameObject, rc.gameObject);
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
        }

        void QueryTribe() {

            var strId = rc.Get<GameObject>(UIClubAssitent.tribejoin_inputfield_search).GetComponent<InputField>().text;
            try
            {
                WEB2_tribe_search.RequestData search_req = new WEB2_tribe_search.RequestData()
                {

                    clubId = iClubId,
                    key = strId,
                };
                HttpRequestComponent.Instance.Send(WEB2_tribe_search.API, WEB2_tribe_search.Request(search_req), (resData) => {

                    WEB2_tribe_search.ResponseData search_res = WEB2_tribe_search.Response(resData);
                    if (search_res.status == 0)
                    {
                        res = search_res;
                        scrollcomponent.Refresh(res.data.Count);
                        UpdateSearchStatus($"{LanguageManager.mInstance.GetLanguageForKey("club_join_2")}({res.data.Count})");//搜索结果
                    }
                    else
                    {
                        scrollcomponent.Refresh(0);
                        UpdateSearchStatus(LanguageManager.mInstance.GetLanguageForKey("club_tribe_9"));//抱歉,该ID没有搜索到联盟
                        UIComponent.Instance.Toast(search_res.status);
                    }
                });
            }
            catch
            {
                Log.Error("请输入正确的ID");
            }
        }


        //void TribeJoinCall(string resData)
        //{
        //    Debug.Log(resData);
        //    WEB2_tribe_aJoin.ResponseData tribejoin_res = WEB2_tribe_aJoin.Response(resData);
        //    if (tribejoin_res.status == 0) {

        //        Log.Debug("加入成功");
        //        UIComponent.Instance.Toast("加入成功,等待审批");
        //    }
        //    else
        //    {
        //        Log.Error($"Error status = {tribejoin_res.status} , msg = {tribejoin_res.msg}");
        //        UIComponent.Instance.Toast(tribejoin_res.msg);
        //    }
        //}

        void OnScrollObj(GameObject obj, int index)
        {
            string tribeName = res.data[index].tribeName;
            int tribeId = res.data[index].tribeId;
            obj.transform.GetChild(1).gameObject.GetComponent<Text>().text = tribeName;
            obj.transform.GetChild(2).gameObject.GetComponent<Text>().text = $"ID:{res.data[index].randomId}";
            UIEventListener.Get(obj.transform.GetChild(3).gameObject, tribeId).onIntClick = (go, id) =>
            {
                Log.Debug($"加入联盟{id}");
                WEB2_tribe_aJoin.RequestData tribejoin_req = new WEB2_tribe_aJoin.RequestData()
                {
                    clubId = iClubId,
                    tribeId = tribeId,
                };
                HttpRequestComponent.Instance.Send(WEB2_tribe_aJoin.API,WEB2_tribe_aJoin.Request(tribejoin_req),(resData)=> {

                    WEB2_tribe_aJoin.ResponseData tribejoin_res = WEB2_tribe_aJoin.Response(resData);
                    if (tribejoin_res.status == 0)
                    {
                        Log.Debug("加入联盟成功");
                        UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_tribe_10"));//加入成功,等待审批
                        UIComponent.Instance.Remove(UIType.UIClub_TribeJoin);
                        Game.EventSystem.Run(UIClubEventIdType.CLUB_TRIBE, iClubId);
                    }
                    else
                    {
                        Log.Error($"Error status = {tribejoin_res.status} , msg = {tribejoin_res.msg}");
                        UIComponent.Instance.Toast(tribejoin_res.status);
                    }
                });
            };
        }

        void UpdateSearchStatus(string str)
        {
            rc.Get<GameObject>(UIClubAssitent.tribejoin_text_search).GetComponent<Text>().text = str;
        }
    }
}


