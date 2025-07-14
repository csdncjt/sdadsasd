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
    public class UIClub_MlistInfoComponentSystem : AwakeSystem<UIClub_MlistInfoComponent>
    {
        public override void Awake(UIClub_MlistInfoComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_MlistInfoComponent : UIBaseComponent
    {

        string userId;
        ReferenceCollector rc;
        public void Awake()
        {

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIClub_MlistInfo);

            object[] arr = obj as object[];
            userId = arr[0] as string;
            WEB2_user_public_info.ResponseData userinfo_res = arr[1] as WEB2_user_public_info.ResponseData;
            WEB2_data_stat_person.ResponseData person_res = arr[2] as WEB2_data_stat_person.ResponseData;

            Head h = HeadCache.GetHead(eHeadType.USER, userId);
            if (h.headId == string.Empty || h.headId != userinfo_res.data.head)
            {
                string headerId = userinfo_res.data.head;
                WebImageHelper.GetHeadImage(userinfo_res.data.head, (t2d) => {

                    rc.Get<GameObject>(UIClubAssitent.info_img_clubicon).GetComponent<RawImage>().texture = t2d;
                    //缓存头像
                    h.headId = userinfo_res.data.head;
                    h.t2d = t2d;
                });
            }
            else
            {
                //已存在图片
                rc.Get<GameObject>(UIClubAssitent.mlistinfo_img_icon).GetComponent<RawImage>().texture = h.t2d;
            }

            rc.Get<GameObject>(UIClubAssitent.mlistinfo_text_name).GetComponent<Text>().text = userinfo_res.data.nickName;
            rc.Get<GameObject>(UIClubAssitent.mlistinfo_text_id).GetComponent<Text>().text = $"ID:{userId}";
            if(userinfo_res.data.sex == 0)
            {
                rc.Get<GameObject>(UIClubAssitent.mlistinfo_img_sex).GetComponent<Image>().sprite = rc.Get<Sprite>(UIClubAssitent.mlistinfo_img_icon_boy);
            }
            else
            {
                rc.Get<GameObject>(UIClubAssitent.mlistinfo_img_sex).GetComponent<Image>().sprite = rc.Get<Sprite>(UIClubAssitent.mlistinfo_img_icon_girl);
            }

            RefreshMark();
            rc.Get<GameObject>(UIClubAssitent.mlistinfo_text_poolrate).GetComponent<Text>().text = $"{person_res.data.VPIP}%";
            rc.Get<GameObject>(UIClubAssitent.mlistinfo_text_winrate).GetComponent<Text>().text = $"{person_res.data.Wins}%";
            List<object> strs = new List<object>() { person_res.data.totalGameCnt , person_res.data.totalHand};
            rc.Get<GameObject>(UIClubAssitent.mlistinfo_text_skill).GetComponent<Text>().text = CPErrorCode.LanguageDescription(10328 , strs);//$"参与{person_res.data.totalGameCnt}|共{person_res.data.totalHand}手";
            
            rc.Get<GameObject>(UIClubAssitent.mlistinfo_img_criclein).GetComponent<Image>().fillAmount = person_res.data.VPIP / 100f;
            rc.Get<GameObject>(UIClubAssitent.mlistinfo_img_cricleout).GetComponent<Image>().fillAmount = person_res.data.Wins / 100f;

            string nickName = userinfo_res.data.nickName;
            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.mlistinfo_btn_remark)).onClick = async (go) =>
            {
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_MlistRemarks, new object[] { userId  , nickName } , () => { });
            };

            UITrManager.Instance.Append(UIComponent.Instance.Get(UIType.UIClub_Mlist).GameObject, rc.gameObject);
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

        public void RefreshMark() {

            if (!userId.Equals(GameCache.Instance.nUserId.ToString()))
            {

                WEB2_sns_batch_relations.DataElement data = CacheDataManager.mInstance.GetSNSBatchRelation(userId);
                if (data == null || data.remarkName == "" || data.remarkName == string.Empty)
                {
                    rc.Get<GameObject>(UIClubAssitent.mlistinfo_text_remark).GetComponent<Text>().text = LanguageManager.mInstance.GetLanguageForKey("club_mlst_1");//"编辑备注名和打法标记";
                }
                else rc.Get<GameObject>(UIClubAssitent.mlistinfo_text_remark).GetComponent<Text>().text = data.remarkName;
                rc.Get<GameObject>(UIClubAssitent.mlistinfo_text_remark).SetActive(true);
                rc.Get<GameObject>(UIClubAssitent.mlistinfo_btn_remark).SetActive(true);
            }
            else
            {
                rc.Get<GameObject>(UIClubAssitent.mlistinfo_text_remark).SetActive(false);
                rc.Get<GameObject>(UIClubAssitent.mlistinfo_btn_remark).SetActive(false);
            }
        }
    }
}

