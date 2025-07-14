using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using BestHTTP;
using ETModel;
using ETHotfix;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIClub_InfoComponentSystem : AwakeSystem<UIClub_InfoComponent>
    {
        public override void Awake(UIClub_InfoComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_InfoComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        int iClubId;
        int userType;//1 = 创建者<br/>2 = 成员<br/>3 = 非成员
        Texture2D club_headtexture;
        WEB2_club_view.ResponseData cacheData;
        public void Awake()
        {

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
        }

        public override void OnShow(object obj)
        {

            SetUpNav(UIType.UIClub_Info);
            WEB2_club_view.ResponseData res = obj as WEB2_club_view.ResponseData;
            UpdateClubInfo(res);
            CheckMsg();

            //修改简介
            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.info_btn_detail)).onClick = async(go) =>
            {
                //只有带权限时才能点击
                if(userType == 1)
                {
                    await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_InfoIntroduce, new object[] { iClubId, cacheData.data.description }, () => { });
                }
            };

            //修改名字
            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.info_btn_changename)).onClick = async (go) =>
            {
                //只有带权限时才能点击
                if (userType == 1)
                {
                    await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_InfoName, new object[] { iClubId, cacheData.data.clubName }, () => { });
                }
            };

            //点击联盟
            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.info_btn_alli)).onClick = (go) =>
            {
                //只有带权限时才能点击
                if (userType == 1)
                {
                    Game.EventSystem.Run(UIClubEventIdType.CLUB_TRIBE, iClubId);
                }
            };

            //点击房间列表
            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.mode_room)).onClick = (go) =>
            {
                //只有带权限时才能点击
               // if (userType == 1)
                {
                    UIComponent.Instance.ShowNoAnimation(UIType.UIClub_RoomList);
                }
            };

            //点击战绩列表
            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.mode_record)).onClick = (go) =>
            {
                //只有带权限时才能点击
               // if (userType == 1)
                {
                    UIComponent.Instance.ShowNoAnimation(UIType.UIMine_RecordList, res.data.clubId);
                }
            };

            //点击俱乐部消息
            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.mode_msg)).onClick = (go) =>
            {
                //只有带权限时才能点击
               // if (userType == 1)
                {
                    UIComponent.Instance.ShowNoAnimation(UIType.UIMine_MsgNormal, new object[2] { EnumMSG.MSG_Club, EnumMSGFROM.MSGFROM_Club});
                }
            };

            //基金信息
            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.info_btn_fund)).onClick = (go) =>
            {
                //只有带权限时才能点击
                if (userType == 1)
                {
                    Game.EventSystem.Run(UIClubEventIdType.CLUB_FUNDINFO, iClubId);
                }
            };

            //基金信息
            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.info_btn_Gamefund)).onClick = (go) =>
            {
                //只有带权限时才能点击
                if (userType == 1)
                {
                    Game.EventSystem.Run(UIClubEventIdType.CLUB_GAMEFUNDINFO, iClubId);
                }
            };

            //交易模式
            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.info_btn_exchange)).onClick = async(go) =>
            {
                //只有带权限时才能点击
                if (userType == 1) {

                    //UIComponent.Instance.Toast("敬请期待");
                    //上线后才开启
                    await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_Trans, new object[2] {iClubId , cacheData.data.transType}, () => { });
                }
            };

            //成员
            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.info_btn_member)).onClick = (go) =>
            {
                //只有带权限时才能点击
                if (userType == 1)
                {
                    Game.EventSystem.Run(UIClubEventIdType.CLUB_MLIST, UIClubEvent.PassType.OPEN, iClubId);
                }
            };

            //照相
            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.info_btn_camera)).onClick = async (go) =>
            {
                //只有带权限时才能点击
                if (userType != 1)
                {
                    return;
                }
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIActionSheet, new UIActionSheetComponent.ActionSheetData()
                {
                    titles = new string[] { LanguageManager.Get("UIMine_UserInfoSetting_Camera"), LanguageManager.Get("UIMine_UserInfoSetting_album") },
                    actionDelegate = (index) =>
                    {
                        if (index == 0)
                        {
                            NativeManager.TakeHeadImage("1");
                            ETModel.Game.Hotfix.OnGetHeadImagePath = HeadImageCall;
                        }
                        if (index == 1)
                        {
                            NativeManager.TakeHeadImage("2");
                            ETModel.Game.Hotfix.OnGetHeadImagePath = HeadImageCall;
                            //HeadImageCall(""); test
                        }
                    }
                }, () => { });
            };

            //copy
            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.info_btn_contact_copy0)).onClick = (go) =>
            {
                UniClipboard.SetText(cacheData.data.contactList[0].content);
                UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_info_1"));//复制成功
                
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.info_btn_contact_copy1)).onClick = (go) =>
            {
                UniClipboard.SetText(cacheData.data.contactList[1].content);
                UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_info_1"));//复制成功

            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.info_btn_contact)).onClick = async(go) =>
            {
                //只有带权限时才能点击
                if (userType == 1)
                {
                    int t0 = 0;
                    int t1 = 0;
                    string con0 = string.Empty;
                    string con1 = string.Empty;
                    if(cacheData.data.contactList != null)
                    {
                        if (cacheData.data.contactList.Count == 1)
                        {
                            t0 = cacheData.data.contactList[0].type;
                            con0 = cacheData.data.contactList[0].content;
                        }
                        else if (cacheData.data.contactList.Count == 2) {

                            t0 = cacheData.data.contactList[0].type;
                            con0 = cacheData.data.contactList[0].content;
                            t1 = cacheData.data.contactList[1].type;
                            con1 = cacheData.data.contactList[1].content;
                        }
                    }
                    await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_InfoContact, new object[] {iClubId , t0 , con0 , t1 , con1} , () => { });
                }
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.mode_kaifan)).onClick = async (go) => {

                if (userType == 1)
                    await UIComponent.Instance.ShowAsyncNoAnimation(UIType.UIClub_RoomCreat, 1, () => { });
            };


            UITrManager.Instance.Append(UIComponent.Instance.Get(UIType.UIClub).GameObject , rc.gameObject);
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
            club_headtexture = null;
            cacheData = null;
        }

        private void CheckMsg()
        {
            //检查消息的红点
            UIMineModel.mInstance.APIMessagesNumsByType(1, nums =>
            {
                rc.Get<GameObject>(UIClubAssitent.info_text_msg).GetComponent<Text>().text = $"{nums}";
            });
        }

        void HeadImageCall(string headImgage) {

            //清除回调
            ETModel.Game.Hotfix.OnGetHeadImagePath = null;
            if (headImgage == "error")
            {
                Log.Debug("头像获取失败");
                return;
            }
            Log.Debug($"headImgage = {headImgage}");
            //读取
            //string path = $"file://{headImgage}";
            FileStream fs = new FileStream(headImgage, FileMode.Open);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            //byte[] buffer = await LoadLocalImg("");
            Texture2D texture = new Texture2D(320, 320);
            var iSLoad = texture.LoadImage(buffer);
            texture.Apply();
            if (iSLoad && texture != null)
            {
                //上传
                HttpRequestComponent.Instance.SendPicUpload(WEB_upload_head.API, buffer, (resData) =>
                {
                    Log.Debug(resData);
                    WEB_upload_head.ResponseData responseData = WEB_upload_head.Response(resData);
                    if (responseData.status == 0)
                    {
                        Log.Debug("上传成功");
                        string picName = responseData.data.pic_name;
                        club_headtexture = texture;
                        //设置头像
                        WEB2_club_modify.RequestData update_req = new WEB2_club_modify.RequestData()
                        {
                            clubId = iClubId,
                            content = picName,
                            type = 3,
                        };
                        HttpRequestComponent.Instance.Send(
                                            WEB2_club_modify.API,
                                            WEB2_club_modify.Request(update_req),
                                            this.ModifyCall);
                    }
                    else
                    {
                        UIComponent.Instance.Toast(responseData.status);
                    }
                });
            }
            else {

                Log.Debug("Error 图片加载失败");
            }
        }

        void ModifyCall(string resData)
        {

            WEB2_club_modify.ResponseData update_res = WEB2_club_modify.Response(resData);
            if (update_res.status == 0)
            {
                Log.Debug("头像修改成功");
                rc.Get<GameObject>(UIClubAssitent.info_img_clubicon).GetComponent<RawImage>().texture = club_headtexture;
            }
            else
            {
                Log.Error($"Error status = {update_res.status} , msg = {update_res.msg}");
                UIComponent.Instance.Toast(update_res.status);
            }
        }

        public void UpdateClubInfo(WEB2_club_view.ResponseData res) {

            cacheData = res;
            Head h = HeadCache.GetHead(eHeadType.CLUB, res.data.clubId.ToString());
            if (h.headId == string.Empty || h.headId != res.data.clubHeader)
            {
                string headerId = res.data.clubHeader;
                WebImageHelper.GetHeadImage(res.data.clubHeader, (t2d) => {

                    rc.Get<GameObject>(UIClubAssitent.info_img_clubicon).GetComponent<RawImage>().texture = t2d;
                    //缓存头像
                    h.headId = res.data.clubHeader;
                    h.t2d = t2d;
                });
            }
            else
            {
                //已存在图片
                rc.Get<GameObject>(UIClubAssitent.info_img_clubicon).GetComponent<RawImage>().texture = h.t2d;
            }

            //刷新俱乐部信息
            rc.Get<GameObject>(UIClubAssitent.info_text_clubname).GetComponent<Text>().text = res.data.clubName;
            rc.Get<GameObject>(UIClubAssitent.info_text_id).GetComponent<Text>().text = $"ID:{res.data.randomId}";
            rc.Get<GameObject>(UIClubAssitent.info_text_area).GetComponent<Text>().text = $"{DBAreaComponent.Instance.QueryCity(res.data.areaId)}";
            rc.Get<GameObject>(UIClubAssitent.info_text_detail).GetComponent<Text>().text = res.data.description.Length > 0 ? res.data.description : "";
            rc.Get<GameObject>(UIClubAssitent.info_text_menber).GetComponent<Text>().text = $"{res.data.clubMembers}/{res.data.membersLimit}";

            string danwei = "桌";
            if (LanguageManager.mInstance.mCurLanguage == 1)
            {
                danwei = "desk";
            }
            rc.Get<GameObject>(UIClubAssitent.info_text_room).GetComponent<Text>().text = $"{res.data.roomCount}"+ danwei;

            float halfw = rc.Get<GameObject>(UIClubAssitent.info_text_area).GetComponent<Text>().preferredWidth / 2;
            rc.Get<GameObject>(UIClubAssitent.info_text_area).transform.GetChild(0).localPosition = new Vector3(-halfw - 30 , 0 , 0);

            GameObject detailObj = rc.Get<GameObject>(UIClubAssitent.info_text_detail);
            float preHeigth = detailObj.GetComponent<Text>().preferredHeight;
            detailObj.transform.parent.GetComponent<LayoutElement>().minHeight = Math.Max(180 , 130 + preHeigth);


            iClubId = res.data.clubId;

            //设置修改俱乐部名字按钮位置
            int nlen = StringUtil.GetStringLength(res.data.clubName);
            RectTransform rect = rc.Get<GameObject>(UIClubAssitent.info_text_clubname).GetComponent<RectTransform>();
            Vector2 offsetm1 = rect.offsetMin;
            Vector2 offsetm2 = rect.offsetMax;
            int mx = Math.Max(80, nlen * 15 + 50);
            offsetm1.x = 600 - mx;
            offsetm2.x = -600 + mx;
            rect.offsetMin = offsetm1;
            rect.offsetMax = offsetm2;

            userType = res.data.userType;
            //只有创建者才可以看到联盟

            rc.Get<GameObject>(UIClubAssitent.mode_room).SetActive(true);
            rc.Get<GameObject>(UIClubAssitent.mode_record).SetActive(true);
            rc.Get<GameObject>(UIClubAssitent.mode_msg).SetActive(true);
            if (userType == 1)
            {
                rc.Get<GameObject>(UIClubAssitent.mode_kaifan).SetActive(true);
                rc.Get<GameObject>(UIClubAssitent.mode_alli).SetActive(true);
                rc.Get<GameObject>(UIClubAssitent.mode_fund).SetActive(true);
                rc.Get<GameObject>(UIClubAssitent.mode_Gamefund).SetActive(true);
                rc.Get<GameObject>(UIClubAssitent.mode_member).SetActive(true);
                rc.Get<GameObject>(UIClubAssitent.info_btn_changename).SetActive(true);
                rc.Get<GameObject>(UIClubAssitent.info_btn_camera).SetActive(true);
                rc.Get<GameObject>(UIClubAssitent.info_img_detail).SetActive(true);
              
            }
            else
            {
                rc.Get<GameObject>(UIClubAssitent.mode_kaifan).SetActive(false);
                rc.Get<GameObject>(UIClubAssitent.mode_alli).SetActive(false);
                rc.Get<GameObject>(UIClubAssitent.mode_fund).SetActive(false);
                rc.Get<GameObject>(UIClubAssitent.mode_member).SetActive(false);
                rc.Get<GameObject>(UIClubAssitent.mode_Gamefund).SetActive(false);
                rc.Get<GameObject>(UIClubAssitent.mode_exchange).SetActive(false);
                rc.Get<GameObject>(UIClubAssitent.info_btn_changename).SetActive(false);
                rc.Get<GameObject>(UIClubAssitent.info_btn_camera).SetActive(false);
                rc.Get<GameObject>(UIClubAssitent.info_img_detail).SetActive(false);

            }

            //联系方式和微信、tele的显示逻辑
            //userType = 2;
            if (userType == 1) {

                rc.Get<GameObject>(UIClubAssitent.mode_contact).SetActive(true);
                rc.Get<GameObject>(UIClubAssitent.mode_contactType0).SetActive(false);
                rc.Get<GameObject>(UIClubAssitent.mode_contactType1).SetActive(false);
            }
            else if(userType == 2)
            {

                rc.Get<GameObject>(UIClubAssitent.mode_contact).SetActive(false);
                if (res.data.contactList == null || res.data.contactList.Count == 0)
                {
                    rc.Get<GameObject>(UIClubAssitent.mode_contactType0).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.mode_contactType1).SetActive(false);
                    return;
                }

                if (res.data.contactList.Count == 1) {

                    rc.Get<GameObject>(UIClubAssitent.mode_contactType0).SetActive(true);
                    rc.Get<GameObject>(UIClubAssitent.mode_contactType1).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.info_text_contact_name0).GetComponent<Text>().text = GetContactTypeName(res.data.contactList[0].type);
                    rc.Get<GameObject>(UIClubAssitent.info_text_contact_content0).GetComponent<Text>().text = res.data.contactList[0].content;

                }
                else
                {
                    rc.Get<GameObject>(UIClubAssitent.mode_contactType0).SetActive(true);
                    rc.Get<GameObject>(UIClubAssitent.mode_contactType1).SetActive(true);
                    rc.Get<GameObject>(UIClubAssitent.info_text_contact_name0).GetComponent<Text>().text = GetContactTypeName(res.data.contactList[0].type);
                    rc.Get<GameObject>(UIClubAssitent.info_text_contact_content0).GetComponent<Text>().text = res.data.contactList[0].content;
                    rc.Get<GameObject>(UIClubAssitent.info_text_contact_name1).GetComponent<Text>().text = GetContactTypeName(res.data.contactList[1].type);
                    rc.Get<GameObject>(UIClubAssitent.info_text_contact_content1).GetComponent<Text>().text = res.data.contactList[1].content;
                }
                //适配位置
                float width = rc.Get<GameObject>(UIClubAssitent.info_text_contact_name0).GetComponent<Text>().preferredWidth;
                Vector2 offs = rc.Get<GameObject>(UIClubAssitent.info_text_contact_content0).GetComponent<RectTransform>().offsetMin;
                rc.Get<GameObject>(UIClubAssitent.info_text_contact_content0).GetComponent<RectTransform>().offsetMin = new Vector2(width + 120 , offs.y);
                width = rc.Get<GameObject>(UIClubAssitent.info_text_contact_name1).GetComponent<Text>().preferredWidth;
                offs = rc.Get<GameObject>(UIClubAssitent.info_text_contact_content1).GetComponent<RectTransform>().offsetMin;
                rc.Get<GameObject>(UIClubAssitent.info_text_contact_content1).GetComponent<RectTransform>().offsetMin = new Vector2(width + 120, offs.y);
            }
            else
            {
                rc.Get<GameObject>(UIClubAssitent.mode_contact).SetActive(false);
                rc.Get<GameObject>(UIClubAssitent.mode_contactType0).SetActive(false);
                rc.Get<GameObject>(UIClubAssitent.mode_contactType1).SetActive(false);
            }
        }

        string GetContactTypeName(int type) {

            string name = string.Empty;
            switch (type)
            {
                case 1:
                    name = LanguageManager.mInstance.GetLanguageForKey("club_info_0");//微信
                    break;
                case 2:
                    name = "Telegram";
                    break;
                case 3:
                    name = "Skype";
                    break;
                default:
                    name = "Sugram";
                    break;
            }
            return name;
        }
    }
}
