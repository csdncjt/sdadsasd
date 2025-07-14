
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
    public class UIClub_TribeInfoComponentAwakeSystem : AwakeSystem<UIClub_TribeInfoComponent>
    {
        public override void Awake(UIClub_TribeInfoComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_TribeInfoComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        CZScrollRect scrollcomponent;
        List<WEB2_tribe_member.DataElement> list;
        int tribeId;
        public void Awake()
        {

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            scrollcomponent = new CZScrollRect();
            scrollcomponent.prefab = rc.Get<GameObject>(UIClubAssitent.tribe_clubinfo);
            scrollcomponent.scrollRect = rc.Get<GameObject>(UIClubAssitent.tribeinfo_scrollview).GetComponent<ScrollRect>();
            scrollcomponent.onScrollObj = OnScrollObj;
            scrollcomponent.interval = 230;
            scrollcomponent.Init();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIClub_TribeInfo);

            object[] arr = obj as object[];

            string tribeName = arr[1] as string;
            WEB2_tribe_detail.ResponseData tribeDetail = arr[2] as WEB2_tribe_detail.ResponseData;//联盟信息
            WEB2_tribe_member.ResponseData tribeMember = arr[3] as WEB2_tribe_member.ResponseData;//联盟俱乐部列表
            tribeId = tribeDetail.data.tribeId;


            rc.Get<GameObject>(UIClubAssitent.tribeInfo_text_tribename).GetComponent<Text>().text = tribeDetail.data.tribeName;
            rc.Get<GameObject>(UIClubAssitent.tribeInfo_text_tribeid).GetComponent<Text>().text = $"ID:{tribeDetail.data.randomId}";
            rc.Get<GameObject>(UIClubAssitent.tribeInfo_text_creatname).GetComponent<Text>().text = $"{LanguageManager.mInstance.GetLanguageForKey("club_tribe_6")}:{tribeDetail.data.creatorName}";//创建者
            rc.Get<GameObject>(UIClubAssitent.tribeInfo_text_ratio).GetComponent<Text>().text = $"{LanguageManager.mInstance.GetLanguageForKey("club_tribe_7")}:{tribeDetail.data.profit}%";//占成比例
            rc.Get<GameObject>(UIClubAssitent.tribeInfo_text_clubnum).GetComponent<Text>().text = $"{LanguageManager.mInstance.GetLanguageForKey("club_tribe_8")}{tribeDetail.data.clubCount}/{tribeDetail.data.clubLimit}";//加入的俱乐部
            //Debug.Log(tribeMember.data.Count);

            list = tribeMember.data;
            scrollcomponent.Refresh(list.Count);

            int tribe_randomId = tribeDetail.data.randomId;
            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.tribeInfo_btn_copy)).onClick = (go) =>
            {
                UniClipboard.SetText(tribe_randomId.ToString());
                UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_info_1"));//复制成功
            };

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
            list = null;
        }

        void OnScrollObj(GameObject obj, int index)
        {

            string userid = list[index].randomId.ToString();
            string headerId = list[index].clubHeader;
            Head h = HeadCache.GetHead(eHeadType.CLUB, userid);
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

            obj.transform.GetChild(1).GetComponent<Text>().text = list[index].clubName;
            obj.transform.GetChild(2).GetComponent<Text>().text = $"ID:{list[index].randomId}";
            obj.transform.GetChild(3).GetComponent<Text>().text = $"{list[index].clubPleNum}";
            obj.transform.GetChild(4).GetComponent<Text>().text = $"{DBAreaComponent.Instance.QueryCity(int.Parse(list[index].clubAreaId))}";
            obj.transform.GetChild(5).gameObject.SetActive(list[index].status == 0 ? false : true);
        }
    }
}


