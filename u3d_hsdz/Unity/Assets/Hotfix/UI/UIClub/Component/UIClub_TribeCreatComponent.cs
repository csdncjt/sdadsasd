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
    public class UIClub_TribeCreatAwakeSystem : AwakeSystem<UIClub_TribeCreatComponent>
    {
        public override void Awake(UIClub_TribeCreatComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_TribeCreatComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        int iClubId;
        public void Awake()
        {

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
        }

        public override void OnShow(object obj)
        {

            SetUpNav(UIType.UIClub_TribeCreat);
            iClubId = (int)obj;

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.tribecreat_btn_creat)).onClick = (go) => {

                if (IsCreatEnable(true) != 0)
                {
                    return;
                }


                string tribeName = rc.Get<GameObject>(UIClubAssitent.tribecreat_inputField_name).GetComponent<InputField>().text;
                if (SensitiveWordComponent.Instance.SensitiveWordJudge(LanguageManager.mInstance.GetLanguageForKey("UIClub_TribeCreat_0HvQpjkd"), tribeName))
                {
                    return;
                }

                string phone = rc.Get<GameObject>(UIClubAssitent.tribecreat_inputField_phone).GetComponent<InputField>().text;
                string wechat = rc.Get<GameObject>(UIClubAssitent.tribecreat_inputField_wechat).GetComponent<InputField>().text;
                string telegram = rc.Get<GameObject>(UIClubAssitent.tribecreat_inputField_telegram).GetComponent<InputField>().text;
                string email = rc.Get<GameObject>(UIClubAssitent.tribecreat_inputField_email).GetComponent<InputField>().text;
                string mesbox = rc.Get<GameObject>(UIClubAssitent.tribecreat_inputField_mesbox).GetComponent<InputField>().text;
                try
                {
                    WEB2_tribe_create.RequestData tribenew_req = new WEB2_tribe_create.RequestData()
                    {
                        tribeName = tribeName,
                        clubId = iClubId,
                        phone = phone,
                        wechat = wechat,
                        telegram = telegram,
                        email = email,
                        message = mesbox
                    };
                    HttpRequestComponent.Instance.Send(
                                        WEB2_tribe_create.API,
                                        WEB2_tribe_create.Request(tribenew_req),
                                        this.ClubCreatCall);
                }
                catch(Exception e)
                {
                    Log.Error($"Error clubId = {iClubId} e = {e}");
                }
            };

            rc.Get<GameObject>(UIClubAssitent.tribecreat_inputField_name).GetComponent<InputField>().onValueChanged.AddListener(OnInputChange);
            rc.Get<GameObject>(UIClubAssitent.tribecreat_inputField_phone).GetComponent<InputField>().onValueChanged.AddListener(OnInputChange);
            rc.Get<GameObject>(UIClubAssitent.tribecreat_inputField_wechat).GetComponent<InputField>().onValueChanged.AddListener(OnInputChange);
            rc.Get<GameObject>(UIClubAssitent.tribecreat_inputField_telegram).GetComponent<InputField>().onValueChanged.AddListener(OnInputChange);
            rc.Get<GameObject>(UIClubAssitent.tribecreat_inputField_email).GetComponent<InputField>().onValueChanged.AddListener(OnInputChange);
            rc.Get<GameObject>(UIClubAssitent.tribecreat_inputField_mesbox).GetComponent<InputField>().onValueChanged.AddListener(OnInputChange);

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
        }

        void OnInputChange(string str)
        {

            RefreshView();
        }

        void RefreshView()
        {

            if (IsCreatEnable(false) == 0)
            {

                rc.Get<GameObject>(UIClubAssitent.tribecreat_btn_creat).GetComponent<Image>().sprite = rc.Get<Sprite>("club_btn_enable");
            }
            else
            {
                rc.Get<GameObject>(UIClubAssitent.tribecreat_btn_creat).GetComponent<Image>().sprite = rc.Get<Sprite>("club_btn_disable");
            }
        }

        int IsCreatEnable(bool isToast)
        {

            //是否已经输入了联盟名字
            string tribeName = rc.Get<GameObject>(UIClubAssitent.tribecreat_inputField_name).GetComponent<InputField>().text;
            //限制24个字符长度
            int len = StringUtil.GetStringLength(tribeName);
            if (len > 24)
            {
                if (isToast) UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_tribe_2"));//联盟名字长度超出
                return -1;
            }
            else if (len == 0)
            {
                if (isToast) UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_tribe_3"));//请输入联盟名称
                return -1;
            }

            //是否已经填入其中一项信息
            string phone = rc.Get<GameObject>(UIClubAssitent.tribecreat_inputField_phone).GetComponent<InputField>().text;
            len = StringUtil.GetStringLength(phone);
            if (len > 11)
            {
                if (isToast) UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_creat_3"));//手机信息长度超出
                return -1;
            }

            string wechat = rc.Get<GameObject>(UIClubAssitent.tribecreat_inputField_wechat).GetComponent<InputField>().text;
            len = StringUtil.GetStringLength(wechat);
            if (len > 24)
            {
                if (isToast) UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_creat_4"));//微信信息长度超出
                return -1;
            }

            string telegram = rc.Get<GameObject>(UIClubAssitent.tribecreat_inputField_telegram).GetComponent<InputField>().text;
            len = StringUtil.GetStringLength(telegram);
            if (len > 24)
            {
                if (isToast) UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_creat_5"));//telegram信息长度超出
                return -1;
            }

            string email = rc.Get<GameObject>(UIClubAssitent.tribecreat_inputField_email).GetComponent<InputField>().text;

            string mesbox = rc.Get<GameObject>(UIClubAssitent.tribecreat_inputField_mesbox).GetComponent<InputField>().text;
            len = StringUtil.GetStringLength(mesbox);
            if (len > 60)
            {
                if (isToast) UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_creat_6"));//留言信息长度超出
                return -1;
            }

            if (phone.Length == 0 && wechat.Length == 0 && telegram.Length == 0 && email.Length == 0 && mesbox.Length == 0)
            {
                if (isToast) UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_creat_7"));//选填信息内容，请您至少填写一项信息
                return -1;
            }

            if (phone.Length == 0 && wechat.Length == 0 && telegram.Length == 0 && email.Length == 0 && mesbox.Length == 0)
            {
                return -1;
            }
            return 0;
        }


        void ClubCreatCall(string resData)
        {
            // Debug.Log(resData);
            WEB2_tribe_create.ResponseData tribenew_res = WEB2_tribe_create.Response(resData);
            if (tribenew_res.status == 0)
            {
                
                Log.Debug("联盟创建成功，请等待审核通过。");
                UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_tribe_4"));//联盟创建成功，请等待审核通过。
                UIComponent.Instance.Remove(UIType.UIClub_TribeCreat);
                Game.EventSystem.Run(UIClubEventIdType.CLUB_TRIBE, iClubId);
            }
            else if (tribenew_res.status == 1305)
            {

                UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_tribe_5"));//您创建的联盟正在审核中，不可创建其他联盟
            }
            else
            {
                Log.Debug($"创建失败 status = {tribenew_res.status}");
                UIComponent.Instance.Toast(tribenew_res.status);
            }
        }
    }
}

