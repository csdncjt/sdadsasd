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
    public class UIClub_MlistComponentSystem : AwakeSystem<UIClub_MlistComponent>
    {
        public override void Awake(UIClub_MlistComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_MlistComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        int iClubId;
        CZScrollRect scrollcomponent;
        List<WEB2_club_mlist.DataElement> clubmlist_list;
        List<WEB2_club_mlist.DataElement> tempclubmlist_list;

        public void Awake()
        {

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            scrollcomponent = new CZScrollRect();
            scrollcomponent.prefab = rc.Get<GameObject>(UIClubAssitent.mlist_info);
            scrollcomponent.scrollRect = rc.Get<GameObject>(UIClubAssitent.mlist_scrollview).GetComponent<ScrollRect>();
            scrollcomponent.onScrollObj = OnScrollObj;
            scrollcomponent.interval = 230;
            scrollcomponent.limitNum = 12;
            scrollcomponent.Init();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIClub_Mlist);

            object[] arr = obj as object[];
            RefreshView((int)arr[0], arr[1] as WEB2_club_mlist.ResponseData);

            var inputfield = rc.Get<GameObject>(UIClubAssitent.mlist_inputfield_query).GetComponent<InputField>();
            inputfield.onValueChanged.RemoveAllListeners();
            inputfield.onValueChanged.AddListener((value) => {

                Search(value);
            });

            UITrManager.Instance.Append(UIComponent.Instance.Get(UIType.UIClub_Info).GameObject, rc.gameObject);
        }

        public override void OnHide()
        {
            rc.Get<GameObject>(UIClubAssitent.mlist_inputfield_query).GetComponent<InputField>().onValueChanged.RemoveAllListeners();
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

        public void RefreshView(int clubid, WEB2_club_mlist.ResponseData res)
        {
            iClubId = clubid;
            rc.Get<GameObject>(UIClubAssitent.mlist_inputfield_query).GetComponent<InputField>().text = string.Empty;
            clubmlist_list = res.data;
            tempclubmlist_list = clubmlist_list;
            scrollcomponent.Refresh(res.data.Count);
        }

        void Search(string sear)
        {
            tempclubmlist_list = new List<WEB2_club_mlist.DataElement>();
            if (sear == "" || sear == string.Empty)
            {

                for (int i = 0; i < clubmlist_list.Count; ++i)
                {

                    tempclubmlist_list.Add(clubmlist_list[i]);
                }
            }
            else
            {
                for (int i = 0; i < clubmlist_list.Count; ++i)
                {

                    if (clubmlist_list[i].nickName.IndexOf(sear) >= 0)
                    {

                        tempclubmlist_list.Add(clubmlist_list[i]);
                    }
                }
            }
            scrollcomponent.Refresh(tempclubmlist_list.Count);
        }

        void OnScrollObj(GameObject obj, int index)
        {
            
            string user_id = tempclubmlist_list[index].uid;
            string headerId = tempclubmlist_list[index].userHead;
            //Log.Debug($"index = {index} headerId = {headerId}");
            Head h = HeadCache.GetHead(eHeadType.USER, user_id);
            if (h.headId == string.Empty || h.headId != headerId)
            {
                obj.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = WebImageHelper.GetDefaultHead();//未加载图片时先显示默认图片
                WebImageHelper.GetHeadImage(headerId, (t2d) => {

                    //Log.Debug($"index = {index} 加载成功 headerId = {headerId} objindex = {scrollcomponent.GetObjIndex(obj)}");
                    if (obj != null && scrollcomponent.GetObjIndex(obj) == index) {

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

            //Debug.Log($"OnScrollObj -- {obj.name} index = {index}");
            string nickName = tempclubmlist_list[index].nickName;
            int type = tempclubmlist_list[index].userType;
            string userid = tempclubmlist_list[index].uid;
            string userHead = tempclubmlist_list[index].userHead;

            obj.transform.GetChild(1).GetComponent<Text>().text = $"{nickName}";
            if (type == 1)
            {

                obj.transform.GetChild(2).gameObject.SetActive(true);
            }
            else
            {
                obj.transform.GetChild(2).gameObject.SetActive(false);
            }
            UIEventListener.Get(obj.transform.GetChild(5).gameObject).onClick = (go) => {

                Game.EventSystem.Run(UIClubEventIdType.CLUB_MLISTINFO, userid);
            };
        }
    }
}



