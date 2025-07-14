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
    public class UIClub_FundGiveComponentSystem : AwakeSystem<UIClub_FundGiveComponent>
    {
        public override void Awake(UIClub_FundGiveComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_FundGiveComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        long iFund;
        int iClubId;
        CZScrollRect scrollcomponent;
        List<WEB2_club_mlist.DataElement> clubmlist_list;
        List<WEB2_club_mlist.DataElement> tempclubmlist_list;

        //test
        //string[] mlist;
        //List<string> tempmlist;

        public void Awake()
        {

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            scrollcomponent = new CZScrollRect();
            scrollcomponent.prefab = rc.Get<GameObject>(UIClubAssitent.fundgive_info);
            scrollcomponent.scrollRect = rc.Get<GameObject>(UIClubAssitent.fundgive_scrollview).GetComponent<ScrollRect>();
            scrollcomponent.onScrollObj = OnScrollObj;
            scrollcomponent.interval = 230;
            scrollcomponent.limitNum = 12;
            scrollcomponent.Init();


            //temp数据
            //随机产生数据
            //string ns = "为日举个B好1235abciefffchghyoperl看A的积分卡道具哦持续";
            //mlist = new string[200];//[UnityEngine.Random.Range(0, 20)];
            //for (int i = 0; i < mlist.Length; ++i)
            //{

            //    int s = UnityEngine.Random.Range(0, ns.Length - 5);
            //    int l = UnityEngine.Random.Range(1, 4);
            //    mlist[i] = $"{ns.Substring(s, l)}";
            //}
        }

        public override void OnShow(object obj)
        {

            SetUpNav(UIType.UIClub_FundGive);
            object[] arr = obj as object[];
            RefreshView((long)arr[0] , (int)arr[1] , arr[2] as WEB2_club_mlist.ResponseData);

            var inputfield = rc.Get<GameObject>(UIClubAssitent.fundgive_inputfield_query).GetComponent<InputField>();
            inputfield.onValueChanged.RemoveAllListeners();
            inputfield.onValueChanged.AddListener((value) => {

                Search(value);
            });

            UITrManager.Instance.Append(UIComponent.Instance.Get(UIType.UIClub_FundDetail).GameObject, rc.gameObject);

            //test
            //Search(string.Empty);
        }

        public override void OnHide()
        {
            rc.Get<GameObject>(UIClubAssitent.fundgive_inputfield_query).GetComponent<InputField>().onValueChanged.RemoveAllListeners();
            UITrManager.Instance.Remove(rc.gameObject);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            base.Dispose();
            scrollcomponent.Dispose();
            scrollcomponent = null;
            UITrManager.Instance.Remove(rc.gameObject);
            rc = null;
        }

        public void RefreshView(long fund , int clubid, WEB2_club_mlist.ResponseData res)
        {
            iFund = fund;
            iClubId = clubid;

            rc.Get<GameObject>(UIClubAssitent.fundgive_inputfield_query).GetComponent<InputField>().text = string.Empty;

            clubmlist_list = res.data;
            tempclubmlist_list = clubmlist_list;
            //rc.Get<GameObject>(UIClubAssitent.fundgive_text_remain).GetComponent<Text>().text = $"余额{fund}";
            scrollcomponent.Refresh(res.data.Count);
        }

        void Search(string sear) {

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

            //test
            //tempmlist = new List<string>();
            //if (sear == "" || sear == string.Empty) {

            //    for (int i = 0; i < mlist.Length; ++i)
            //    {

            //        tempmlist.Add(mlist[i]);
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < mlist.Length; ++i)
            //    {

            //        if (mlist[i].IndexOf(sear) >= 0)
            //        {

            //            tempmlist.Add(mlist[i]);
            //        }
            //    }
            //}
            //scrollcomponent.Refresh(tempmlist.Count);
        }

        void OnScrollObj(GameObject obj, int index)
        {
            //Debug.Log($"OnScrollObj -- {obj.name} index = {index}");
            string nickName = tempclubmlist_list[index].nickName;
            int type = tempclubmlist_list[index].userType;
            string userid = tempclubmlist_list[index].uid;
            string headerId = tempclubmlist_list[index].userHead;
            Head h = HeadCache.GetHead(eHeadType.USER, userid);
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

            obj.transform.GetChild(1).GetComponent<Text>().text = $"{nickName}";
            GameObject objBtn = obj.transform.GetChild(2).gameObject;
            objBtn.SetActive(true);
            UIEventListener.Get(objBtn).onClick = (go) => {

                // Debug.Log($"发放对象{userid}");
                Game.Scene.GetComponent<UIComponent>().ShowNoAnimation(UIType.UIClub_FundGiveSub, new object[4] { nickName, userid, iClubId, headerId }, () => { });
            };




            //if (type == 1) {

            //    objBtn.SetActive(false);
            //    UIEventListener.Get(objBtn).onClick = (go) => {};
            //}
            //else
            //{
            //    objBtn.SetActive(true);
            //    UIEventListener.Get(objBtn).onClick = async(go) => {

            //        // Debug.Log($"发放对象{userid}");
            //        await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_FundGiveSub, new object[4] { nickName, userid , iClubId, headerId }, () => { });
            //    };
            //}
        }
    }
}


