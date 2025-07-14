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
    public class UIClub_RoomCreateComponentSystem : AwakeSystem<UIClub_RoomCreateComponent>
    {
        public override void Awake(UIClub_RoomCreateComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_RoomCreateComponent : UIBaseComponent
    {
        ReferenceCollector rc;
        public int pageIndex;
        public int subpageIndex;//子页页码

        static Color color0 = new Color(244 / 255f, 197 / 255f, 106 / 255f, 1);
        static Color color1 = new Color(213 / 255f, 43 / 255f, 44 / 255f, 1);

        int createType = 0;//0俱乐部开房1俱乐部开积分房2大厅开积分房

        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            this.GetParent<UI>().AddComponent<UIClub_RoomCreateNormalComponent>();//德州普通局
            this.GetParent<UI>().AddComponent<UIClub_RoomCreateOmahaComponent>();//奥马哈
            this.GetParent<UI>().AddComponent<UIClub_RoomCreateAofdzComponent>();//aof德州
            this.GetParent<UI>().AddComponent<UIClub_RoomCreateAofomaComponent>();//aof奥马哈
            this.GetParent<UI>().AddComponent<UIClub_RoomCreateOndzComponent>();//必下场德州
            this.GetParent<UI>().AddComponent<UIClub_RoomCreateOnomaComponent>();//必下场奥马哈
            this.GetParent<UI>().AddComponent<UIClub_RoomCreateDpComponent>();//短牌
            this.GetParent<UI>().AddComponent<UIClub_RoomCreateAofDpComponent>();//aof短牌
            pageIndex = 0;
        }
 
        public void Refresh() {

            //Log.Debug($"pageIndex = {pageIndex}");
            switch (pageIndex) {

                case 0:
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type0).GetComponent<Toggle>().isOn = true;
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type1).GetComponent<Toggle>().isOn = false;
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type2).GetComponent<Toggle>().isOn = false;
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type3).GetComponent<Toggle>().isOn = false;
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type6).GetComponent<Toggle>().isOn = false;
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_subbtn).SetActive(false);

                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page0).SetActive(true);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page1).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page2).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page3).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page4).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page5).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page6).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page7).SetActive(false);

                    this.GetParent<UI>().GetComponent<UIClub_RoomCreateNormalComponent>().OnShow(createType);
                    break;
                case 1:
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type0).GetComponent<Toggle>().isOn = false;
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type1).GetComponent<Toggle>().isOn = true;
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type2).GetComponent<Toggle>().isOn = false;
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type3).GetComponent<Toggle>().isOn = false;
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type6).GetComponent<Toggle>().isOn = false;
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_subbtn).SetActive(false);

                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page0).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page1).SetActive(true);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page2).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page3).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page4).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page5).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page6).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page7).SetActive(false);

                    this.GetParent<UI>().GetComponent<UIClub_RoomCreateOmahaComponent>().OnShow(createType);
                    break;
                case 2:

                    //必下场
                    if (subpageIndex == 0) {

                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type0).GetComponent<Toggle>().isOn = false;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type1).GetComponent<Toggle>().isOn = false;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type2).GetComponent<Toggle>().isOn = true;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type3).GetComponent<Toggle>().isOn = false;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type6).GetComponent<Toggle>().isOn = false;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_subbtn).SetActive(true);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type4).GetComponent<Toggle>().isOn = true;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type5).GetComponent<Toggle>().isOn = false;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page0).SetActive(false);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page1).SetActive(false);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page2).SetActive(true);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page3).SetActive(false);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page4).SetActive(false);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page5).SetActive(false);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page6).SetActive(false);

                        this.GetParent<UI>().GetComponent<UIClub_RoomCreateOndzComponent>().OnShow(createType);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type4).transform.GetChild(1).GetComponent<Text>().color = color1;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type5).transform.GetChild(1).GetComponent<Text>().color = color0;
                    }
                    else
                    {
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type0).GetComponent<Toggle>().isOn = false;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type1).GetComponent<Toggle>().isOn = false;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type2).GetComponent<Toggle>().isOn = true;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type3).GetComponent<Toggle>().isOn = false;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type6).GetComponent<Toggle>().isOn = false;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_subbtn).SetActive(true);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type4).GetComponent<Toggle>().isOn = false;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type5).GetComponent<Toggle>().isOn = true;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page0).SetActive(false);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page1).SetActive(false);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page2).SetActive(false);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page3).SetActive(true);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page4).SetActive(false);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page5).SetActive(false);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page6).SetActive(false);
                        this.GetParent<UI>().GetComponent<UIClub_RoomCreateOnomaComponent>().OnShow(createType);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type4).transform.GetChild(1).GetComponent<Text>().color = color0;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type5).transform.GetChild(1).GetComponent<Text>().color = color1;
                    }
                    break;

                case 3:
                    {
                        //aof
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type0).GetComponent<Toggle>().isOn = false;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type1).GetComponent<Toggle>().isOn = false;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type2).GetComponent<Toggle>().isOn = false;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type3).GetComponent<Toggle>().isOn = true;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type6).GetComponent<Toggle>().isOn = false;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_subbtn).SetActive(true);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type4).GetComponent<Toggle>().isOn = false;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type5).GetComponent<Toggle>().isOn = false;
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type7).GetComponent<Toggle>().isOn = false;

                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page0).SetActive(false);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page1).SetActive(false);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page2).SetActive(false);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page3).SetActive(false);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page4).SetActive(false);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page5).SetActive(false);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page6).SetActive(false);
                        rc.Get<GameObject>(UIClubAssitent.roomcreat_page7).SetActive(false);

                        if (subpageIndex == 0)
                        {
                            rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type4).GetComponent<Toggle>().isOn = true;
                            rc.Get<GameObject>(UIClubAssitent.roomcreat_page4).SetActive(true);

                            this.GetParent<UI>().GetComponent<UIClub_RoomCreateAofdzComponent>().OnShow(createType);
                            rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type4).transform.GetChild(1).GetComponent<Text>().color = color1;
                            rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type5).transform.GetChild(1).GetComponent<Text>().color = color0;
                            rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type7).transform.GetChild(1).GetComponent<Text>().color = color0;
                        }
                        else if (subpageIndex == 1)
                        {
                            rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type5).GetComponent<Toggle>().isOn = true;
                            rc.Get<GameObject>(UIClubAssitent.roomcreat_page5).SetActive(true);

                            this.GetParent<UI>().GetComponent<UIClub_RoomCreateAofomaComponent>().OnShow(createType);
                            rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type4).transform.GetChild(1).GetComponent<Text>().color = color0;
                            rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type5).transform.GetChild(1).GetComponent<Text>().color = color1;
                            rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type7).transform.GetChild(1).GetComponent<Text>().color = color0;
                        }
                        else
                        {
                            rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type7).GetComponent<Toggle>().isOn = true;
                            rc.Get<GameObject>(UIClubAssitent.roomcreat_page7).SetActive(true);
                            this.GetParent<UI>().GetComponent<UIClub_RoomCreateAofDpComponent>().OnShow(createType);
                            rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type4).transform.GetChild(1).GetComponent<Text>().color = color0;
                            rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type5).transform.GetChild(1).GetComponent<Text>().color = color0;
                            rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type7).transform.GetChild(1).GetComponent<Text>().color = color1;
                        }
                    }
                    break;
                case 4:
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type0).GetComponent<Toggle>().isOn = false;
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type1).GetComponent<Toggle>().isOn = false;
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type2).GetComponent<Toggle>().isOn = false;
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type3).GetComponent<Toggle>().isOn = false;
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type6).GetComponent<Toggle>().isOn = true;
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_subbtn).SetActive(false);

                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page0).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page1).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page2).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page3).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page4).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page5).SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page6).SetActive(true);
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_page7).SetActive(false);

                    this.GetParent<UI>().GetComponent<UIClub_RoomCreateDpComponent>().OnShow(createType);
                    break;
                default:
                    Log.Error($"error pageIndex = {pageIndex} ,subpageIndex =  {subpageIndex}");
                    break;
            }
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIClub_RoomCreat);

            createType = (int)obj;

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type0)).onClick = (go) => {

                pageIndex = 0;
                Refresh();
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type1)).onClick = (go) => {

                pageIndex = 1;
                Refresh();
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type2)).onClick = (go) => {

                pageIndex = 2;
                subpageIndex = 0;
                Refresh();
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type3)).onClick = (go) => {

                pageIndex = 3;
                subpageIndex = 0;
                Refresh();
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type6)).onClick = (go) => {

                pageIndex = 4;
                Refresh();
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type4)).onClick = (go) => {

                subpageIndex = 0;
                Refresh();
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type5)).onClick = (go) => {

                subpageIndex = 1;
                Refresh();
            };


            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_type7)).onClick = (go) => {

                subpageIndex = 2;
                Refresh();
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.roomcreat_btn_start)).onClick = (go) => {

                switch (pageIndex)
                {

                    case 0:
                        this.GetParent<UI>().GetComponent<UIClub_RoomCreateNormalComponent>().OnStart(createType);
                        break;
                    case 1:
                        this.GetParent<UI>().GetComponent<UIClub_RoomCreateOmahaComponent>().OnStart(createType);
                        break;
                    case 2:

                        if (subpageIndex == 0) {

                            this.GetParent<UI>().GetComponent<UIClub_RoomCreateOndzComponent>().OnStart(createType);
                        }
                        else
                        {
                            this.GetParent<UI>().GetComponent<UIClub_RoomCreateOnomaComponent>().OnStart(createType);
                        }
                        break;
                    case 3:
                        if (subpageIndex == 0)
                        {
                            this.GetParent<UI>().GetComponent<UIClub_RoomCreateAofdzComponent>().OnStart(createType);
                        }
                        else if(subpageIndex == 1)
                        {
                            this.GetParent<UI>().GetComponent<UIClub_RoomCreateAofomaComponent>().OnStart(createType);
                        }
                        else if (subpageIndex == 2)
                        {
                            this.GetParent<UI>().GetComponent<UIClub_RoomCreateAofDpComponent>().OnStart(createType);
                        }
                        break;
                    case 4:
                        this.GetParent<UI>().GetComponent<UIClub_RoomCreateDpComponent>().OnStart(createType);
                        break;
                    default:
                        
                        break;
                }
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.roomcreat_insurance_btn_close)).onClick = (go) => {

                rc.Get<GameObject>(UIClubAssitent.roomcreat_insurance).SetActive(false);
            };


            Refresh();

            if (createType < 2)
            {
                UITrManager.Instance.Append(UIComponent.Instance.Get(UIType.UIClub).GameObject, rc.gameObject);
            }
        }

        public void ShowTip(int type) {

            if (type == 0)
            {
                ShowXzTip();
            }
            else if (type == 1)
            {
                rc.Get<GameObject>(UIClubAssitent.roomcreat_insurance).SetActive(true);
                rc.Get<GameObject>(UIClubAssitent.TexasInsurance).SetActive(true);
                rc.Get<GameObject>(UIClubAssitent.OmahaInsurance).SetActive(false);
                rc.Get<GameObject>(UIClubAssitent.ShortInsurance).SetActive(false);

            } else if (type == 2) {

                rc.Get<GameObject>(UIClubAssitent.roomcreat_insurance).SetActive(true);
                rc.Get<GameObject>(UIClubAssitent.TexasInsurance).SetActive(false);
                rc.Get<GameObject>(UIClubAssitent.OmahaInsurance).SetActive(true);
                rc.Get<GameObject>(UIClubAssitent.ShortInsurance).SetActive(false);
            }
            else if (type == 3)
            {
                rc.Get<GameObject>(UIClubAssitent.roomcreat_insurance).SetActive(true);
                rc.Get<GameObject>(UIClubAssitent.ShortInsurance).SetActive(true);
                rc.Get<GameObject>(UIClubAssitent.TexasInsurance).SetActive(false);
                rc.Get<GameObject>(UIClubAssitent.OmahaInsurance).SetActive(false);
            }
        }

        int count = 0;
        async void ShowXzTip() {

            //只是显示血战提示信息
            count++;
            rc.Get<GameObject>(UIClubAssitent.roomcreat_xuezhantip).SetActive(true);
            ETModel.TimerComponent timer = ETModel.Game.Scene.GetComponent<TimerComponent>();
            await timer.WaitAsync(3000);
            count--;
            if (count <= 0)
            {
                count = 0;
                rc.Get<GameObject>(UIClubAssitent.roomcreat_xuezhantip).SetActive(false);
            }
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
        }
    }


    public class RcConfig
    {
        public float element_people_value;
        public float element_autopeople_value;
        public float element_sizeblind_value;
        public float element_dragin_value;
        public float element_playtime_value;
        public float element_shortplaytime_value;
        public float element_thinktime_value;
        public float element_note_value;
        public float element_multiple0_value;
        public float element_multiple1_value;
        public float element_fee_value;

        public bool element_toggle0_value;
        public bool element_toggle1_value;
        public bool element_toggle2_value;
        public bool element_toggle3_value;
        public bool element_toggle4_value;
        public bool element_toggle5_value;
        public bool element_toggle6_value;
        public bool element_toggle7_value;
        public bool element_toggle8_value;
        public bool element_toggle9_value;
        public bool element_toggle10_value;
        public bool element_toggle_xuezhan_value;//奥马哈独有  血战模式

        public bool element_toggle_autop_value;//滿人自動開局
        public float element_planEnptyPeople_value;//空位自动开桌
    }
}
