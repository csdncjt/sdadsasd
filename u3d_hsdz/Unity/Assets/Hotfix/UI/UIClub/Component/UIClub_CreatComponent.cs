using System;
using System.Collections.Generic;
using System.Net;
using BestHTTP;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIClub_CreatComponentSystem : AwakeSystem<UIClub_CreatComponent>
    {
        public override void Awake(UIClub_CreatComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_CreatComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        int areaId = -1;
        string strClubHead = "-1";//默认头像
        public void Awake() {

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIClub_Creat);

            GameObject scrollview = rc.Get<GameObject>(UIClubAssitent.creat_scrollview);
            UIScrollEventListener.Get(scrollview).RegisterButton(rc.Get<GameObject>(UIClubAssitent.creat_inputfield_name));
            UIScrollEventListener.Get(scrollview).RegisterButton(rc.Get<GameObject>(UIClubAssitent.creat_inputfield_phone));
            UIScrollEventListener.Get(scrollview).RegisterButton(rc.Get<GameObject>(UIClubAssitent.creat_inputfield_wechat));
            UIScrollEventListener.Get(scrollview).RegisterButton(rc.Get<GameObject>(UIClubAssitent.creat_inputfield_telegram));
            UIScrollEventListener.Get(scrollview).RegisterButton(rc.Get<GameObject>(UIClubAssitent.creat_inputfield_email));
            UIScrollEventListener.Get(scrollview).RegisterButton(rc.Get<GameObject>(UIClubAssitent.creat_inputfield_mesbox));
            UIScrollEventListener.Get(scrollview).RegisterButton(rc.Get<GameObject>(UIClubAssitent.creat_inputfield_detail));


            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.creat_inputfield_place)).onClick = async(go) =>
            {
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_CreatArea, null , () => { });
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.creat_btn_camera)).onClick = async(go) =>
            {
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

            rc.Get<GameObject>(UIClubAssitent.creat_inputfield_name).GetComponent<InputField>().onValueChanged.AddListener(OnInputChange);
            rc.Get<GameObject>(UIClubAssitent.creat_inputfield_phone).GetComponent<InputField>().onValueChanged.AddListener(OnInputChange);
            rc.Get<GameObject>(UIClubAssitent.creat_inputfield_wechat).GetComponent<InputField>().onValueChanged.AddListener(OnInputChange);
            rc.Get<GameObject>(UIClubAssitent.creat_inputfield_telegram).GetComponent<InputField>().onValueChanged.AddListener(OnInputChange);
            rc.Get<GameObject>(UIClubAssitent.creat_inputfield_email).GetComponent<InputField>().onValueChanged.AddListener(OnInputChange);
            rc.Get<GameObject>(UIClubAssitent.creat_inputfield_mesbox).GetComponent<InputField>().onValueChanged.AddListener(OnInputChange);
            rc.Get<GameObject>(UIClubAssitent.creat_inputfield_detail).GetComponent<InputField>().onValueChanged.AddListener(OnInputChange);

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.creat_btn_creat)).onClick = (go)=> {

                if (IsCreatEnable(true) != 0) {

                    return;
                }
                string clubName = rc.Get<GameObject>(UIClubAssitent.creat_inputfield_name).GetComponent<InputField>().text;
                if (SensitiveWordComponent.Instance.SensitiveWordJudge(LanguageManager.mInstance.GetLanguageForKey("UIClub_Creat_2LvGNmS7"), clubName))
                {
                    return;
                }
                string phone = rc.Get<GameObject>(UIClubAssitent.creat_inputfield_phone).GetComponent<InputField>().text;
                string wechat = rc.Get<GameObject>(UIClubAssitent.creat_inputfield_wechat).GetComponent<InputField>().text;
                string telegram = rc.Get<GameObject>(UIClubAssitent.creat_inputfield_telegram).GetComponent<InputField>().text;
                string email = rc.Get<GameObject>(UIClubAssitent.creat_inputfield_email).GetComponent<InputField>().text;
                string mesbox = rc.Get<GameObject>(UIClubAssitent.creat_inputfield_mesbox).GetComponent<InputField>().text;
                string detail = rc.Get<GameObject>(UIClubAssitent.creat_inputfield_detail).GetComponent<InputField>().text;

                if (SensitiveWordComponent.Instance.SensitiveWordJudge(LanguageManager.mInstance.GetLanguageForKey("UIClub_InfoIntroduce_44vQpjkd"), detail))
                {
                    return;
                }

                //创建请求
                WEB2_club_create.RequestData creat_req = new WEB2_club_create.RequestData()
                {

                    clubName = clubName,
                    clubHead = strClubHead,
                    areaId = areaId.ToString(),
                    phone = phone,
                    wechat = wechat,
                    telegram = telegram,
                    email = email,
                    message = mesbox,
                    desc = detail
                };
                HttpRequestComponent.Instance.Send(
                    WEB2_club_create.API, WEB2_club_create.Request(creat_req),
                    this.ClubCreatCall);
            };

            UITrManager.Instance.Append(UIComponent.Instance.Get(UIType.UIClub).GameObject, rc.gameObject);
        }

        public override void OnHide()
        {
            //创建完毕后整体回收
            UIComponent.Instance.Remove(UIType.UIClub_CreatCity);
            UIComponent.Instance.Remove(UIType.UIClub_CreatCountry);
            UIComponent.Instance.Remove(UIType.UIClub_CreatProvince);
            UIComponent.Instance.Remove(UIType.UIClub_CreatArea);

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

        void HeadImageCall(string headImgage)
        {
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
            if (texture != null)
            {
                //上传
                HttpRequestComponent.Instance.SendPicUpload(WEB_upload_head.API, buffer, (resData) =>
                {
                    Log.Debug(resData);
                    WEB_upload_head.ResponseData responseData = WEB_upload_head.Response(resData);
                    if (responseData.status == 0)
                    {
                        Log.Debug("上传成功");
                        strClubHead = responseData.data.pic_name;
                        rc.Get<GameObject>(UIClubAssitent.creat_img_clubicon).GetComponent<RawImage>().texture = texture;
                    }
                    else
                    {
                        Log.Debug("上传失败");
                        UIComponent.Instance.Toast(responseData.status);
                    }
                });
            }
        }

        public void SetArea(DBArea db) {

            Log.Debug($"set db = {db.id} name = {db.name_zh}");
            areaId = db.id;
            rc.Get<GameObject>(UIClubAssitent.creat_inputfield_place).GetComponent<InputField>().text = LanguageManager.mInstance.GetLanguageForKey($"area{db.id}");
            RefreshView();
        }

        void OnInputChange(string str) {

            RefreshView();
        }

        void RefreshView() {

            if (IsCreatEnable(false) == 0) {
                
                rc.Get<GameObject>(UIClubAssitent.creat_btn_creat).GetComponent<Image>().sprite = rc.Get<Sprite>("club_btn_enable");
            }
            else
            {
                rc.Get<GameObject>(UIClubAssitent.creat_btn_creat).GetComponent<Image>().sprite = rc.Get<Sprite>("club_btn_disable");
            }
        }

        int IsCreatEnable(bool isToast) {

            //是否已经输入了俱乐部名字
            string clubName = rc.Get<GameObject>(UIClubAssitent.creat_inputfield_name).GetComponent<InputField>().text;
            //限制24个字符长度
            int len = StringUtil.GetStringLength(clubName);
            if (len > 24)
            {
                if(isToast) UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_creat_0"));
                return -1;
            }
            else if (len == 0)
            {
                if (isToast) UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_creat_1"));
                return -1;
            }

            //是否已经选择地区
            if (DBAreaComponent.Instance.QueryCity(areaId).Equals("unknow"))
            {
                if (isToast) UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_creat_2"));
                return -1;
            }

            //是否已经填入其中一项信息
            string phone = rc.Get<GameObject>(UIClubAssitent.creat_inputfield_phone).GetComponent<InputField>().text;
            len = StringUtil.GetStringLength(phone);
            if (len > 11)
            {
                if (isToast) UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_creat_3"));
                return -1;
            }

            string wechat = rc.Get<GameObject>(UIClubAssitent.creat_inputfield_wechat).GetComponent<InputField>().text;
            len = StringUtil.GetStringLength(wechat);
            if (len > 24)
            {
                if (isToast) UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_creat_4"));
                return -1;
            }

            string telegram = rc.Get<GameObject>(UIClubAssitent.creat_inputfield_telegram).GetComponent<InputField>().text;
            len = StringUtil.GetStringLength(telegram);
            if (len > 24)
            {
                if (isToast) UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_creat_5"));
                return -1;
            }

            string email = rc.Get<GameObject>(UIClubAssitent.creat_inputfield_email).GetComponent<InputField>().text;
 
            string mesbox = rc.Get<GameObject>(UIClubAssitent.creat_inputfield_mesbox).GetComponent<InputField>().text;
            len = StringUtil.GetStringLength(mesbox);
            if (len > 60)
            {
                if (isToast) UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_creat_6"));
                return -1;
            }

            if (phone.Length == 0 && wechat.Length == 0 && telegram.Length == 0 && email.Length == 0 && mesbox.Length == 0)
            {
                if (isToast) UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_creat_7"));
                return -1;
            }

            return 0;
        }


        void ClubCreatCall(string resData)
        {
            
            // Debug.Log(resData);
            WEB2_club_create.ResponseData club_res = WEB2_club_create.Response(resData);
            if (club_res.status == 0)
            {
                Log.Debug("俱乐部创建成功，请等待审核通过。");
                UIComponent.Instance.ShowNoAnimation(UIType.UIToast, LanguageManager.mInstance.GetLanguageForKey("club_creat_8"));
                UIComponent.Instance.Remove(UIType.UIClub_Creat);
                UIComponent.Instance.Get(UIType.UIClub).GetComponent<UIClubComponent>().GetClubListInfo();
                return;
            }
            else
            {
                UIComponent.Instance.Toast(club_res.status);
            }
        }
    }
}
