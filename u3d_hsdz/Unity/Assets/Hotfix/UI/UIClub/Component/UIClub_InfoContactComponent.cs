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
    public class UIClub_InfoContactComponentAwakeSystem : AwakeSystem<UIClub_InfoContactComponent>
    {
        public override void Awake(UIClub_InfoContactComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_InfoContactComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        Sprite[] spriteList;
        int iClubId;
        int iContant0;
        int iContant1;
        string strContant0;
        string strContant1;
        public void Awake()
        {

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            spriteList = new Sprite[4] {

                rc.Get<Sprite>("list_icon_wechat"),
                rc.Get<Sprite>("list_icon_te"),
                rc.Get<Sprite>("list_icon_skype"),
                rc.Get<Sprite>("list_icon_sugram")
            };
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIClub_InfoContact);

            object[] arr = (object[])obj;
            iClubId = (int)arr[0];
            iContant0 = Math.Max(0 , (int)arr[1] - 1);
            iContant1 = Math.Max(0, (int)arr[3] - 1);
            strContant0 = (string)arr[2];
            strContant1 = (string)arr[4];

            InputField inputField0 = rc.Get<GameObject>(UIClubAssitent.contact_inputfield_0).GetComponent<InputField>();
            ChooseType(inputField0 , iContant0);
            inputField0.text = strContant0;

            InputField inputField1 = rc.Get<GameObject>(UIClubAssitent.contact_inputfield_1).GetComponent<InputField>();
            ChooseType(inputField1, iContant1);
            inputField1.text = strContant1;

            //type1
            Dropdown dropDown0 = rc.Get<GameObject>(UIClubAssitent.contact_dropdown_0).GetComponent<Dropdown>();
            dropDown0.value = iContant0;
            dropDown0.onValueChanged.AddListener((v) =>
            {
                //Log.Debug($"dropDown0 choose index v = {v}");
                ChooseType(inputField0 , v);
            });

            UIEventListener.Get(dropDown0.gameObject).onExit = (go) =>
            {
                if(dropDown0.transform.childCount <= 2)
                {
                    return;
                }
                ScrollRect scrollrect = dropDown0.transform.GetChild(2).gameObject.GetComponent<ScrollRect>();
                for (int i = 1; i < 5; ++i) {

                    scrollrect.content.transform.GetChild(i).GetChild(3).GetComponent<Image>().sprite = spriteList[i - 1];
                }
            };

            //type2
            Dropdown dropDown1 = rc.Get<GameObject>(UIClubAssitent.contact_dropdown_1).GetComponent<Dropdown>();
            dropDown1.value = iContant1;
            dropDown1.onValueChanged.AddListener((v) =>
            {
                //Log.Debug($"dropDown1 choose index v = {v}");
                ChooseType(inputField1, v);
            });

            UIEventListener.Get(dropDown1.gameObject).onExit = (go) =>
            {
                if (dropDown1.transform.childCount <= 2)
                {
                    return;
                }
                ScrollRect scrollrect = dropDown1.transform.GetChild(2).gameObject.GetComponent<ScrollRect>();
                for (int i = 1; i < 5; ++i)
                {

                    scrollrect.content.transform.GetChild(i).GetChild(3).GetComponent<Image>().sprite = spriteList[i - 1];
                }
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.contact_btn_save)).onClick = (go) =>
            {

                if (strContant0.Equals(inputField0.text) && iContant0 == dropDown0.value &&
                strContant1.Equals(inputField1.text) && iContant1 == dropDown1.value) {

                    UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_info_6"));//没有修改内容
                    return;
                }

                WEB2_club_modify_contact.RequestData contact_req = new WEB2_club_modify_contact.RequestData()
                {
                    userId = GameCache.Instance.nUserId,
                    clubId = iClubId,
                    firstType = inputField0.text.Length == 0 ? 0 : dropDown0.value + 1,
                    firstContact = inputField0.text,
                    secondType = inputField1.text.Length == 0 ? 0 : dropDown1.value + 1,
                    secondContact = inputField1.text

                };
                HttpRequestComponent.Instance.Send(
                                    WEB2_club_modify_contact.API,
                                    WEB2_club_modify_contact.Request(contact_req),
                                    this.ContactResCall);

            };

            UITrManager.Instance.Append(UIComponent.Instance.Get(UIType.UIClub_Info).GameObject, rc.gameObject);
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
            rc.Get<GameObject>(UIClubAssitent.contact_inputfield_0).GetComponent<InputField>().onValueChanged.RemoveAllListeners();
            rc.Get<GameObject>(UIClubAssitent.contact_inputfield_1).GetComponent<InputField>().onValueChanged.RemoveAllListeners();
            rc.Get<GameObject>(UIClubAssitent.contact_dropdown_0).GetComponent<Dropdown>().onValueChanged.RemoveAllListeners();
            rc.Get<GameObject>(UIClubAssitent.contact_dropdown_1).GetComponent<Dropdown>().onValueChanged.RemoveAllListeners();

            UITrManager.Instance.Remove(rc.gameObject);
            rc = null;
            spriteList = null;
        }

        void ContactResCall(string resData)
        {
            WEB2_club_modify_contact.ResponseData contact_res = WEB2_club_modify_contact.Response(resData);
            if (contact_res.status == 0)
            {
                UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_info_8"));//修改成功
                Game.EventSystem.Run(UIClubEventIdType.CLUB_INFO, iClubId);

                //缓存
                strContant0 = rc.Get<GameObject>(UIClubAssitent.contact_inputfield_0).GetComponent<InputField>().text;
                iContant0 = rc.Get<GameObject>(UIClubAssitent.contact_dropdown_0).GetComponent<Dropdown>().value;
                strContant1 = rc.Get<GameObject>(UIClubAssitent.contact_inputfield_1).GetComponent<InputField>().text;
                iContant1 = rc.Get<GameObject>(UIClubAssitent.contact_dropdown_1).GetComponent<Dropdown>().value;
            }
            else
            {
                UIComponent.Instance.Toast(contact_res.status);
            }
        }

        void ChooseType(InputField inputfield , int type) {

            switch (type)
            {
                case 1:
                    inputfield.transform.Find("Image").GetComponent<Image>().sprite = spriteList[1];
                    inputfield.transform.Find("Placeholder").GetComponent<Text>().text = LanguageManager.mInstance.GetLanguageForKey("club_info_2");// "请输入联系人的Telegram";
                    break;
                case 2:
                    inputfield.transform.Find("Image").GetComponent<Image>().sprite = spriteList[2];
                    inputfield.transform.Find("Placeholder").GetComponent<Text>().text = LanguageManager.mInstance.GetLanguageForKey("club_info_3");//"请输入联系人的Skype";
                    break;
                case 3:
                    inputfield.transform.Find("Image").GetComponent<Image>().sprite = spriteList[3];
                    inputfield.transform.Find("Placeholder").GetComponent<Text>().text = LanguageManager.mInstance.GetLanguageForKey("club_info_4");//"请输入联系人的Sugram";
                    break;
                default:
                    inputfield.transform.Find("Image").GetComponent<Image>().sprite = spriteList[0];
                    inputfield.transform.Find("Placeholder").GetComponent<Text>().text = LanguageManager.mInstance.GetLanguageForKey("club_info_5");//"请输入联系人的微信";
                    break;
            }
            inputfield.text = string.Empty;
        }
    }
}



