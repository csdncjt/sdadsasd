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
    public class UIClub_MlistRemarksComponentSystem : AwakeSystem<UIClub_MlistRemarksComponent>
    {
        public override void Awake(UIClub_MlistRemarksComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_MlistRemarksComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        int colorIndex = 0;
        public void Awake()
        {

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIClub_MlistRemarks);

            
            object[] arr = obj as object[];
            string userId = arr[0] as string;
            string nickName = arr[1] as string;
            //string strClubId = 
            //WEB_friend_e_detail.ResponseData res = arr[1] as WEB_friend_e_detail.ResponseData;

            Log.Debug(userId);
            WEB2_sns_batch_relations.DataElement data = CacheDataManager.mInstance.GetSNSBatchRelation(userId);
            string oRemarkNme;
            string oRemark;
            int oColor;
            if (data == null) {


                data = new WEB2_sns_batch_relations.DataElement()
                {
                    randomNum = userId
                };
                oRemarkNme = string.Empty;
                oRemark = string.Empty;
                oColor = 0;
            }
            else
            {
                oRemarkNme = data.remarkName;
                oRemark = data.remark;
                oColor = data.remarkColor;
            }

            rc.Get<GameObject>(UIClubAssitent.mlistremarks_text_name).GetComponent<Text>().text = nickName;
            rc.Get<GameObject>(UIClubAssitent.mlistremarks_inputfield_remark).GetComponent<InputField>().text = oRemarkNme;
            rc.Get<GameObject>(UIClubAssitent.mlistremarks_inputfield_sign).GetComponent<InputField>().text = oRemark;
            ChooseColorSign(oColor);
            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.mlistremarks_toggle_color0)).onClick = (go) =>
            {
                if (colorIndex == 1)
                {
                    rc.Get<GameObject>(UIClubAssitent.mlistremarks_toggle_color0).GetComponent<Toggle>().isOn = false;
                    ChooseColorSign(0);
                }
                else ChooseColorSign(1);
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.mlistremarks_toggle_color1)).onClick = (go) =>
            {
                if (colorIndex == 2)
                {
                    rc.Get<GameObject>(UIClubAssitent.mlistremarks_toggle_color1).GetComponent<Toggle>().isOn = false;
                    ChooseColorSign(0);
                }
                else ChooseColorSign(2);
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.mlistremarks_toggle_color2)).onClick = (go) =>
            {
                if (colorIndex == 3)
                {
                    rc.Get<GameObject>(UIClubAssitent.mlistremarks_toggle_color2).GetComponent<Toggle>().isOn = false;
                    ChooseColorSign(0);
                }
                else ChooseColorSign(3);
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.mlistremarks_btn_subimt)).onClick = (go) =>
            {

                string remarkName = rc.Get<GameObject>(UIClubAssitent.mlistremarks_inputfield_remark).GetComponent<InputField>().text;
                string remarks = rc.Get<GameObject>(UIClubAssitent.mlistremarks_inputfield_sign).GetComponent<InputField>().text;

                if (StringUtil.GetStringLength(remarkName) > 24) {

                    UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_mlst_2"));//备注昵称长度超出
                    return;
                }

                if (StringUtil.GetStringLength(remarks) > 24)
                {
                    UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_mlst_3"));//打法标记长度超出
                    return;
                }

                WEB2_sns_mod_remark.RequestData remarks_req = new WEB2_sns_mod_remark.RequestData()
                {
                    otherRandomId = userId,
                    remarkColor = colorIndex,
                    remarkName = remarkName,
                    remark = remarks
                };
                HttpRequestComponent.Instance.Send(
                        WEB2_sns_mod_remark.API,
                        WEB2_sns_mod_remark.Request(remarks_req),
                        (resData) =>
                        {
                            WEB2_sns_mod_remark.ResponseData remarks_res = WEB2_sns_mod_remark.Response(resData);
                            if (remarks_res.status == 0)
                            {
                                Log.Debug("修改成功");
                                //保存本地
                                data.remarkColor = colorIndex;
                                data.remark = remarks;
                                data.remarkName = remarkName;
                                CacheDataManager.mInstance.AddModifySNSBatchDto(data);
                                Game.EventSystem.Run(UIClubEventIdType.CLUB_MLISTINFO_MARKER);
                                UIComponent.Instance.Remove(UIType.UIClub_MlistRemarks);
                            }
                            else
                            {
                                Log.Error($"Error status = {remarks_res.status} msg = {remarks_res.msg}");
                                UIComponent.Instance.Toast(remarks_res.status);
                            }
                        });
            };

            UITrManager.Instance.Append(UIComponent.Instance.Get(UIType.UIClub_MlistInfo).GameObject, rc.gameObject);
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

        void ChooseColorSign(int cordex) {

            colorIndex = cordex;
            if (cordex == 1)
            {
                rc.Get<GameObject>(UIClubAssitent.mlistremarks_toggle_color0).GetComponent<Toggle>().isOn = true;
                rc.Get<GameObject>(UIClubAssitent.mlistremarks_toggle_color1).GetComponent<Toggle>().isOn = false;
                rc.Get<GameObject>(UIClubAssitent.mlistremarks_toggle_color2).GetComponent<Toggle>().isOn = false;
            } else if (cordex == 2)
            {
                rc.Get<GameObject>(UIClubAssitent.mlistremarks_toggle_color0).GetComponent<Toggle>().isOn = false;
                rc.Get<GameObject>(UIClubAssitent.mlistremarks_toggle_color1).GetComponent<Toggle>().isOn = true;
                rc.Get<GameObject>(UIClubAssitent.mlistremarks_toggle_color2).GetComponent<Toggle>().isOn = false;
            }
            else if (cordex == 3)
            {
                rc.Get<GameObject>(UIClubAssitent.mlistremarks_toggle_color0).GetComponent<Toggle>().isOn = false;
                rc.Get<GameObject>(UIClubAssitent.mlistremarks_toggle_color1).GetComponent<Toggle>().isOn = false;
                rc.Get<GameObject>(UIClubAssitent.mlistremarks_toggle_color2).GetComponent<Toggle>().isOn = true;
            }
            else
            {
                rc.Get<GameObject>(UIClubAssitent.mlistremarks_toggle_color0).GetComponent<Toggle>().isOn = false;
                rc.Get<GameObject>(UIClubAssitent.mlistremarks_toggle_color1).GetComponent<Toggle>().isOn = false;
                rc.Get<GameObject>(UIClubAssitent.mlistremarks_toggle_color2).GetComponent<Toggle>().isOn = false;
            }
        }
    }
}

