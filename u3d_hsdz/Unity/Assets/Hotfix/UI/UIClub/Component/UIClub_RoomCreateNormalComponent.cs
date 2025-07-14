using System;
using System.Collections.Generic;
using System.Net;
using BestHTTP;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

//普通局界面
namespace ETHotfix
{

    [ObjectSystem]
    public class UIClub_RoomCreatNormalComponentSystem : AwakeSystem<UIClub_RoomCreateNormalComponent>
    {
        public override void Awake(UIClub_RoomCreateNormalComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_RoomCreateNormalComponent : UIBaseComponent
    {
        public ReferenceCollector rc;
        /// <summary>
        /// 小盲注
        /// </summary>
        int[] sizeblindArr = new int[] { 1, 2, 5, 10, 20, 25 , 50, 100, 200 , 300 , 500 ,1000 };
        /// <summary>
        /// 带入USDT
        /// </summary>
        int[] draginArr = new int[] { 50, 100 };
        /// <summary>
        /// 牌局时长(秒)
        /// </summary>
        int[] maxPlayTimeType = new int[] { 30, 60, 90, 120, 150, 180, 240, 360 };
        float[] qianzhuType;
        /// <summary>
        /// 思考时间(秒)
        /// </summary>
        int[] thinkTime = new int[] { 10, 15 };
        /// <summary>
        /// 最短上桌时间
        /// </summary>
        float[] shortTimeArr;

        ScrollRect scrollrect;

        bool isInit;
        ScrollBarElement element_people;
        ScrollBarElement element_autopeople;
        ScrollBarElement element_sizeblind;
        ScrollBarElement element_dragin;
        ScrollBarElement element_playtime;
        ScrollBarElement element_shortplaytime;
        ScrollBarElement element_thinktime;
        ScrollBarElement element_note;
        DoubleScrollBarElement element_multiple;
        ScrollBarElement element_fee;
        ToggleElement element_toggle0;
        ToggleElement element_toggle1;
        ToggleElement element_toggle2;
        ToggleElement element_toggle3;
        ToggleElement element_toggle4;
        ToggleElement element_toggle5;
        ToggleElement element_toggle6;
        ToggleElement element_toggle7;
        ToggleElement element_toggle8;
        ToggleElement element_toggle9;

        ToggleElement element_toggle_autop;

        ScrollBarElement element_planEnptyPeople;
        Toggle toggle_planSetting;
        int createType;//0大厅1俱乐部
        public void Awake()
        {
            //默认当前界面内容
            isInit = false;
            rc = this.GetParent<UI>().GameObject.transform.Find("roomcreat_page0").GetComponent<ReferenceCollector>();
            scrollrect = rc.gameObject.GetComponent<ScrollRect>();
        }

        public override void OnShow(object obj)
        {
            createType = (int)obj;
            int ptype = createType == 0 ? 61 : 62;
            var sizeblindList = ETHotfix.UIMatchModel.mInstance.GetRoomBlindsByGameAndRoomType(ptype, -1);
            if (sizeblindList.Count > 0)
            {
                sizeblindArr = sizeblindList.ToArray();
            }

            rc.Get<GameObject>(UIClubAssitent.mode_fee).gameObject.SetActive(createType == 1);

            rc.Get<GameObject>("mode_planSetting").gameObject.SetActive(createType == 0);

            scrollrect.verticalScrollbar.value = 1;
            if (!isInit)
            {
                GameObject assitent = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_scrollbar_peoplenum);
                element_people = new ScrollBarElement(
                    assitent.GetComponent<Scrollbar>(),
                    assitent.transform.GetChild(1).GetChild(0).GetComponent<Image>(),
                    assitent.transform.GetChild(0)
                );

                assitent = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_scrollbar_autopeoplenum);
                element_autopeople = new ScrollBarElement(
                    assitent.GetComponent<Scrollbar>(),
                    assitent.transform.GetChild(1).GetChild(0).GetComponent<Image>(),
                    assitent.transform.GetChild(0)
                );

                assitent = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_scrollbar_sizeblind);
                element_sizeblind = new ScrollBarElement(
                    assitent.GetComponent<Scrollbar>(),
                    assitent.transform.GetChild(1).GetChild(0).GetComponent<Image>(),
                    assitent.transform.GetChild(0)
                );

                assitent = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_scrollbar_dragin);
                element_dragin = new ScrollBarElement(
                    assitent.GetComponent<Scrollbar>(),
                    assitent.transform.GetChild(1).GetChild(0).GetComponent<Image>(),
                    assitent.transform.GetChild(0)
                );

                assitent = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_scrollbar_playtime);
                element_playtime = new ScrollBarElement(
                    assitent.GetComponent<Scrollbar>(),
                    assitent.transform.GetChild(1).GetChild(0).GetComponent<Image>(),
                    assitent.transform.GetChild(0)
                );

                assitent = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_scrollbar_shortPlaytime);
                element_shortplaytime = new ScrollBarElement(
                    assitent.GetComponent<Scrollbar>(),
                    assitent.transform.GetChild(1).GetChild(0).GetComponent<Image>(),
                    assitent.transform.GetChild(0)
                );

                assitent = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_scrollbar_thinktime);
                element_thinktime = new ScrollBarElement(
                    assitent.GetComponent<Scrollbar>(),
                    assitent.transform.GetChild(1).GetChild(0).GetComponent<Image>(),
                    assitent.transform.GetChild(0)
                );

                assitent = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_scrollbar_fee);
                element_fee = new ScrollBarElement(
                    assitent.GetComponent<Scrollbar>(),
                    assitent.transform.GetChild(1).GetChild(0).GetComponent<Image>(),
                    assitent.transform.GetChild(0)
                );

                assitent = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_scrollbar_note);
                element_note = new ScrollBarElement(
                    assitent.GetComponent<Scrollbar>(),
                    assitent.transform.GetChild(1).GetChild(0).GetComponent<Image>(),
                    assitent.transform.GetChild(0)
                );

                element_multiple = new DoubleScrollBarElement(
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_0_scrollbar_multiple0).GetComponent<Scrollbar>(),
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_0_scrollbar_multiple1).GetComponent<Scrollbar>(),
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_0_multiple_img_progress).GetComponent<Image>(),
                    rc.Get<GameObject>(UIClubAssitent.roomcreat_0_multiple_container).transform
                );

                assitent = rc.Get<GameObject>("roomcreat_0_scrollbar_planEnptyPeople");
                element_planEnptyPeople = new ScrollBarElement(
                    assitent.GetComponent<Scrollbar>(),
                    assitent.transform.GetChild(1).GetChild(0).GetComponent<Image>(),
                    assitent.transform.GetChild(0)
                );

                assitent = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_0);
                element_toggle0 = new ToggleElement(assitent.GetComponent<Toggle>(), assitent.transform.GetChild(1).gameObject.GetComponent<Text>());
                assitent = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_1);
                element_toggle1 = new ToggleElement(assitent.GetComponent<Toggle>(), assitent.transform.GetChild(1).gameObject.GetComponent<Text>());
                assitent = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_2);
                element_toggle2 = new ToggleElement(assitent.GetComponent<Toggle>(), assitent.transform.GetChild(1).gameObject.GetComponent<Text>());
                assitent = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_3);
                element_toggle3 = new ToggleElement(assitent.GetComponent<Toggle>(), assitent.transform.GetChild(1).gameObject.GetComponent<Text>());
                assitent = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_4);
                element_toggle4 = new ToggleElement(assitent.GetComponent<Toggle>(), assitent.transform.GetChild(1).gameObject.GetComponent<Text>());
                assitent = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_5);
                element_toggle5 = new ToggleElement(assitent.GetComponent<Toggle>(), assitent.transform.GetChild(1).gameObject.GetComponent<Text>());
                assitent = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_6);
                element_toggle6 = new ToggleElement(assitent.GetComponent<Toggle>(), assitent.transform.GetChild(1).gameObject.GetComponent<Text>());
                assitent = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_7);
                element_toggle7 = new ToggleElement(assitent.GetComponent<Toggle>(), assitent.transform.GetChild(1).gameObject.GetComponent<Text>());
                assitent = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_8);
                element_toggle8 = new ToggleElement(assitent.GetComponent<Toggle>(), assitent.transform.GetChild(1).gameObject.GetComponent<Text>());
                assitent = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_9);
                element_toggle9 = new ToggleElement(assitent.GetComponent<Toggle>(), assitent.transform.GetChild(1).gameObject.GetComponent<Text>());
                assitent.SetActive(createType == 1);
                assitent = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_autopeoplenum);
                element_toggle_autop = new ToggleElement(assitent.GetComponent<Toggle>(), assitent.transform.GetChild(1).gameObject.GetComponent<Text>());

                isInit = true;
            }

            element_people.AddEvent();
            element_autopeople.AddEvent();
            element_sizeblind.AddEvent();
            element_dragin.AddEvent();
            element_playtime.AddEvent();
            element_shortplaytime.AddEvent();
            element_thinktime.AddEvent();
            element_note.AddEvent();
            element_multiple.AddEvent();
            element_planEnptyPeople.AddEvent();
            element_fee.AddEvent();

            element_toggle0.AddEvent();
            element_toggle1.AddEvent();
            element_toggle2.AddEvent();
            element_toggle3.AddEvent();
            element_toggle4.AddEvent();
            element_toggle5.AddEvent();
            element_toggle6.AddEvent();
            element_toggle7.AddEvent();
            element_toggle8.AddEvent();

            element_toggle_autop.AddEvent();


            string str1 = StringHelper.ShowGold(this.sizeblindArr[0], false);
            string str2 = StringHelper.ShowGold(this.sizeblindArr[this.sizeblindArr.Length -1], false);
            element_sizeblind.SetHeadText(str1.ToString(), str2.ToString());

            element_sizeblind.bar.onValueChanged.AddListener((v) => {

                this.UpdateBlindAndDragIn(0 , true);
            });

            element_dragin.bar.onValueChanged.AddListener((v) => {

                this.UpdateBlindAndDragIn(1 , true);
            });

            //牌局时间事件
            element_playtime.bar.onValueChanged.AddListener((v) => {

                UpdatePlayTimeInfo(true);
            });

            element_people.bar.onValueChanged.AddListener((v) => {

                UpdateAutoPeopleInfo();
            });

            element_autopeople.bar.onValueChanged.AddListener((v) => {

                UpdateAutoPeopleInfo();
            });

            element_toggle_autop.toggle.onValueChanged.AddListener((v) =>
            {
                UpdateAutoPeopleInfo();
            });

            element_planEnptyPeople.bar.onValueChanged.AddListener((v) => {

                UpdatePlanEnptyPeopleInfo();
            });

            toggle_planSetting = rc.Get<GameObject>("Toggle_Plan").GetComponent<Toggle>();
            toggle_planSetting.onValueChanged.AddListener((on) =>
            {
                LayoutElement layout = rc.Get<GameObject>("mode_planSetting").GetComponent<LayoutElement>();
                layout.minHeight = on ? 900 : 140;
                SetPreferredHeight();
                if (on)
                {
                    element_toggle_autop.toggle.isOn = true;
                    UpdateAutoPeopleInfo();
                }
            });

            InputField inputField_autoDismiss = rc.Get<GameObject>("InputField_AutoDismiss").GetComponent<InputField>();
            Text textAutoDismiss = rc.Get<GameObject>("Text_AutoDismiss").GetComponent<Text>();
            textAutoDismiss.text = string.Format(LanguageManager.mInstance.GetLanguageForKey("UIClub_RoomCreat_autoDismissTips"), inputField_autoDismiss.text);
            inputField_autoDismiss.onValueChanged.RemoveAllListeners();
            inputField_autoDismiss.onValueChanged.AddListener((value) =>
            {
                if (value.Length > 0)
                {
                    if (int.Parse(value) > 10000)
                    {
                        value = "10000";
                    }
                    if (int.Parse(value) < 0)
                    {
                        value = "0";
                    }
                    inputField_autoDismiss.text = $"{int.Parse(value)}";
                }
                textAutoDismiss.text = string.Format(LanguageManager.mInstance.GetLanguageForKey("UIClub_RoomCreat_autoDismissTips"), inputField_autoDismiss.text);
            });


            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.roomcreat_0_btn_qus)).onClick = (go) =>
            {
                Game.EventSystem.Run(UIClubEventIdType.CLUB_ROOMCREAT_TIP, 1);
            };

            //显示默认配置
            element_sizeblind.bar.numberOfSteps = sizeblindArr.Length;

            SetDefaultConfig();
            UpdateView();
        }

        public override void OnHide()
        {
            element_people.RemoveEvent();
            element_autopeople.RemoveEvent();
            element_sizeblind.RemoveEvent();
            element_dragin.RemoveEvent();
            element_playtime.RemoveEvent();
            element_shortplaytime.RemoveEvent();
            element_thinktime.RemoveEvent();
            element_note.RemoveEvent();
            element_multiple.RemoveEvent();
            element_planEnptyPeople.RemoveEvent();
            toggle_planSetting.onValueChanged.RemoveAllListeners();
            element_fee.RemoveEvent();
            element_toggle0.RemoveEvent();
            element_toggle1.RemoveEvent();
            element_toggle2.RemoveEvent();
            element_toggle3.RemoveEvent();
            element_toggle4.RemoveEvent();
            element_toggle5.RemoveEvent();
            element_toggle6.RemoveEvent();
            element_toggle7.RemoveEvent();
            element_toggle8.RemoveEvent();

            element_toggle_autop.RemoveEvent();
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            base.Dispose();
            rc = null;
        }

        #region 接口内容
        public bool isClickCreat;
        public void OnStart(int ctype)
        {
            createType = ctype;
            if (isClickCreat) return;
            var input_name = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_inputfield_name).GetComponent<InputField>().text;
            //限制24个字符长度
            int len = StringUtil.GetStringLength(input_name);
            if (len > 24)
            {
                UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("adaptation10111"));//名字长度超出
                return;
            }
            else if (len == 0)
            {
                UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("adaptation10112"));//请输入牌局名称
                return;
            }

            if (SensitiveWordComponent.Instance.SensitiveWordJudge(LanguageManager.mInstance.GetLanguageForKey("UIClub_RoomCreat_0HvQkjkd"), input_name)) {
                return;
            }

            //获取牌局时间
            int playTime = maxPlayTimeType[element_playtime.step];
            //小盲
            int sizeblind = this.sizeblindArr[element_sizeblind.step];
            //前注
            int qianzhu = (int)(qianzhuType[element_note.step] * sizeblind * 2);
            //最短上桌时间
            int shortTime = Mathf.RoundToInt(shortTimeArr[element_shortplaytime.step] * 60);

            //自动开桌设置
            int startTimeM = int.Parse(rc.Get<GameObject>("InputField_StartM").GetComponent<InputField>().text);
            int startTimeH = int.Parse(rc.Get<GameObject>("InputField_StartH").GetComponent<InputField>().text);
            int endTimeM = int.Parse(rc.Get<GameObject>("InputField_EndM").GetComponent<InputField>().text);
            int endTimeH = int.Parse(rc.Get<GameObject>("InputField_EndH").GetComponent<InputField>().text);
            if (startTimeH < 0 || startTimeH > 23 || endTimeH < 0 || endTimeH > 23 ||
                startTimeM < 0 || startTimeM > 59 || endTimeM < 0 || endTimeM > 59)
            {
                UIComponent.Instance.Toast(LanguageManager.Get("error2601"));
                return;
            }

            string stringAutoDismiss = rc.Get<GameObject>("InputField_AutoDismiss").GetComponent<InputField>().text;
            if (stringAutoDismiss.Length == 0 || int.Parse(stringAutoDismiss) <= 0)
            {
                UIComponent.Instance.Toast(LanguageManager.Get("UIClub_RoomCreat_autoDismissAlert"));
                return;
            }

            //新接口
            WEB2_room_dz_create.RequestData rdz = new WEB2_room_dz_create.RequestData()
            {
                clubRoomType = createType,
                incp = sizeblind * 2 * draginArr[element_dragin.step],
                gmxt = shortTime,
                gmnt = playTime,
                slmz = sizeblind,
                qzhu = qianzhu,
                inmnr = element_multiple.stepStart + 1,//倍数从1开始
                inmxr = element_multiple.stepEnd + 1,//倍数从1开始
                name = input_name,
                jpon = 1, //默认开启 rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_0).GetComponent<Toggle>().isOn ? 1 : 0,
                opts = thinkTime[element_thinktime.step],
                rpu = element_people.step + 2,//人数从2开始
                sdlon = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_1).GetComponent<Toggle>().isOn ? 1 : 0,
                ipon = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_2).GetComponent<Toggle>().isOn ? 1 : 0,
                ahdlon = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_3).GetComponent<Toggle>().isOn ? 1 : 0,
                vpon = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_4).GetComponent<Toggle>().isOn ? 1 : 0,
                isron = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_5).GetComponent<Toggle>().isOn ? 1 : 0,
                muckon = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_6).GetComponent<Toggle>().isOn ? 1 : 0,
                gpson = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_7).GetComponent<Toggle>().isOn ? 1 : 0,
                delay = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_8).GetComponent<Toggle>().isOn ? 1 : 0,
                requestToBringIn = rc.Get<GameObject>(UIClubAssitent.roomcreat_0_toggle_9).GetComponent<Toggle>().isOn ? 1 : 0,
                rpath = 61,
                autopu = element_toggle_autop.toggle.isOn ? (element_autopeople.step + 2) : 0,
                autoPlanOn = toggle_planSetting.isOn ? 1 : 0,
                emptySeatConfig = element_planEnptyPeople.step + 2,
                startTime = startTimeH.ToString().PadLeft(2, '0') + ":" + startTimeM.ToString().PadLeft(2, '0'),
                endTime = endTimeH.ToString().PadLeft(2, '0') + ":" + endTimeM.ToString().PadLeft(2, '0'),
                autoDismiss = rc.Get<GameObject>("Toggle_AutoDismiss").GetComponent<Toggle>().isOn ? 1 : 0,
                roomsLimit = int.Parse(stringAutoDismiss),
                fee = createType == 0 ? 0 : element_fee.step
            };
            //Log.Debug(WEB2_room_dz_create.Request(rdz));

            isClickCreat = true;
            HttpRequestComponent.Instance.Send(WEB2_room_dz_create.API, WEB2_room_dz_create.Request(rdz), this.RoomCreatCall , null ,this.RoomCreatError);
        }

        void RoomCreatCall(string resData)
        {
            isClickCreat = false;
            // Log.Debug($"-- RoomCreatCall -- {resData}");
            //新接口
            WEB2_room_dz_create.ResponseData res_dzroomData = WEB2_room_dz_create.Response(resData);
            if (res_dzroomData.status == 0)
            {
                //保存配置
                SaveConfig();

                if (toggle_planSetting.isOn)
                {
                    UIComponent.Instance.Remove(UIType.UIClub_RoomCreat);
                    UIComponent.Instance.ShowNoAnimation(UIType.UIClub_PlanRoomList);
                    return;
                }
                GameCache.Instance.roomName = res_dzroomData.data.name;
                GameCache.Instance.roomIP = res_dzroomData.data.gmip;
                GameCache.Instance.roomPort = int.Parse(res_dzroomData.data.gmprt);
                GameCache.Instance.room_path = res_dzroomData.data.rpath;
                GameCache.Instance.room_id = res_dzroomData.data.rmid;
                GameCache.Instance.client_ip = res_dzroomData.data.cliip;
                GameCache.Instance.carry_small = res_dzroomData.data.incp;
                GameCache.Instance.straddle = res_dzroomData.data.sdlon;
                GameCache.Instance.insurance = res_dzroomData.data.isron;
                GameCache.Instance.muck_switch = res_dzroomData.data.muckon;
                GameCache.Instance.jackPot_fund = res_dzroomData.data.jpfnd;
                GameCache.Instance.jackPot_on = res_dzroomData.data.jpon;
                GameCache.Instance.jackPot_id = res_dzroomData.data.jpid;
                GameCache.Instance.shortest_time = res_dzroomData.data.gmxt;
                GameCache.Instance.rtype = res_dzroomData.data.rtype;
                AsyncEnterRoom();
            }
            else
            {
                Log.Debug($"创建失败 status =  {res_dzroomData.status}");
                UIComponent.Instance.Toast(res_dzroomData.status);
            }

            
        }

        void RoomCreatError() {

            isClickCreat = false;
        }

        #endregion

        #region 刷新带入积分选项
        /// <summary>
        /// 更新大小盲和带入
        /// </summary>
        void UpdateBlindAndDragIn(int type , bool isEvent)
        {
            int sizeblind = this.sizeblindArr[element_sizeblind.step];

            string str1 = StringHelper.ShowGold(sizeblind, false);
            string str2 = StringHelper.ShowGold(sizeblind * 2, false);
            string str3 = StringHelper.ShowGold(draginArr[element_dragin.step] * sizeblind * 2, false);

            rc.Get<GameObject>(UIClubAssitent.mode_sizeblind_text_size).GetComponent<Text>().text = str1 +"/"+ str2;
            rc.Get<GameObject>(UIClubAssitent.mode_sizeblind_text_dragin).GetComponent<Text>().text = str3;



            if (0 == type) {
                //刷新前注信息
                if (createType == 1)
                {
                    element_note.bar.numberOfSteps = 8;
                    qianzhuType = new float[8] { 0, 0.5f, 1, 2, 5, 10, 20, 30 };
                    element_note.SetText(0, "0BB", 0f);
                    element_note.SetText(1, "0.5BB", 0.142f);
                    element_note.SetText(2, "1BB", 0.285f);
                    element_note.SetText(3, "2BB", 0.428f);
                    element_note.SetText(4, "5BB", 0.571f);
                    element_note.SetText(5, "10BB", 0.714f);
                    element_note.SetText(6, "20BB", 0.857f);
                    element_note.SetText(7, "30BB", 1f);
                }
                else
                {
                    if (sizeblind == 1)
                    {

                        element_note.bar.numberOfSteps = 3;
                        qianzhuType = new float[3] { 0, 0.5f, 1 };
                        element_note.SetText(0, "0BB", 0f);
                        element_note.SetText(1, "0.5BB", 0.5f);
                        element_note.SetText(2, "1BB", 1f);
                        element_note.SetText(3, "", 1f);
                        element_note.SetText(4, "", 1f);

                    }
                    else if (sizeblind == 2)
                    {

                        element_note.bar.numberOfSteps = 4;
                        qianzhuType = new float[4] { 0, 0.25f, 0.5f, 1 };
                        element_note.SetText(0, "0BB", 0f);
                        element_note.SetText(1, "0.25BB", 0.33f);
                        element_note.SetText(2, "0.5BB", 0.66f);
                        element_note.SetText(3, "1BB", 1f);
                        element_note.SetText(4, "", 1f);


                    }
                    else if (sizeblind >= 10)
                    {

                        element_note.bar.numberOfSteps = 5;
                        qianzhuType = new float[5] { 0, 0.1f, 0.25f, 0.5f, 1 };
                        element_note.SetText(0, "0BB", 0f);
                        element_note.SetText(1, "0.1BB", 0.25f);
                        element_note.SetText(2, "0.25BB", 0.5f);
                        element_note.SetText(3, "0.5BB", 0.75f);
                        element_note.SetText(4, "1BB", 1f);
                    }
                    else
                    {
                        element_note.bar.numberOfSteps = 4;
                        qianzhuType = new float[4] { 0, 0.1f, 0.5f, 1 };
                        element_note.SetText(0, "0BB", 0f);
                        element_note.SetText(1, "0.1BB", 0.33f);
                        element_note.SetText(2, "0.5BB", 0.66f);
                        element_note.SetText(3, "1BB", 1f);
                        element_note.SetText(4, "", 1f);
                    }
                }
                
               
                //设置后重置前注
                if(isEvent) element_note.bar.value = 0;
            }
        }
        #endregion

        #region 刷新最短上桌时间选择项
        void UpdateShortTime(int type) {

            switch (type) {

                case 0:
                    element_shortplaytime.bar.numberOfSteps = 1;
                    shortTimeArr = new float[1] {0};
                    element_shortplaytime.SetText(0, "", 0f);
                    element_shortplaytime.SetText(1, "", 0f);
                    element_shortplaytime.SetText(2, "", 0f);
                    element_shortplaytime.SetText(3, "", 0f);
                    element_shortplaytime.SetText(4, "", 0f);
                    element_shortplaytime.SetText(5, "", 0f);
                    element_shortplaytime.SetText(6, "", 0f);
                    break;
                case 1:
                    element_shortplaytime.bar.numberOfSteps = 3;
                    shortTimeArr = new float[3] { 0 , 0.5f , 1};
                    element_shortplaytime.SetText(0, "0h", 0f);
                    element_shortplaytime.SetText(1, "0.5h", 0.5f);
                    element_shortplaytime.SetText(2, "1h", 1f);
                    element_shortplaytime.SetText(3, "", 0f);
                    element_shortplaytime.SetText(4, "", 0f);
                    element_shortplaytime.SetText(5, "", 0f);
                    element_shortplaytime.SetText(6, "", 0f);
                    break;
                case 2:
                    element_shortplaytime.bar.numberOfSteps = 4;
                    shortTimeArr = new float[4] { 0 , 0.5f , 1 , 1.5f};
                    element_shortplaytime.SetText(0, "0h", 0f);
                    element_shortplaytime.SetText(1, "0.5h", 0.33f);
                    element_shortplaytime.SetText(2, "1h", 0.66f);
                    element_shortplaytime.SetText(3, "1.5h", 1f);
                    element_shortplaytime.SetText(4, "", 0f);
                    element_shortplaytime.SetText(5, "", 0f);
                    element_shortplaytime.SetText(6, "", 0f);
                    break;
                case 3:
                    element_shortplaytime.bar.numberOfSteps = 5;
                    shortTimeArr = new float[5] { 0, 0.5f, 1, 1.5f , 2 };
                    element_shortplaytime.SetText(0, "0h", 0f);
                    element_shortplaytime.SetText(1, "0.5h", 0.25f);
                    element_shortplaytime.SetText(2, "1h", 0.5f);
                    element_shortplaytime.SetText(3, "1.5h", 0.75f);
                    element_shortplaytime.SetText(4, "2h", 1f);
                    element_shortplaytime.SetText(5, "", 0f);
                    element_shortplaytime.SetText(6, "", 0f);
                    break;
                case 4:
                    element_shortplaytime.bar.numberOfSteps = 6;
                    shortTimeArr = new float[6] { 0, 0.5f, 1, 1.5f, 2 , 2.5f};
                    element_shortplaytime.SetText(0, "0h", 0f);
                    element_shortplaytime.SetText(1, "0.5h", 0.2f);
                    element_shortplaytime.SetText(2, "1h", 0.4f);
                    element_shortplaytime.SetText(3, "1.5h", 0.6f);
                    element_shortplaytime.SetText(4, "2h", 0.8f);
                    element_shortplaytime.SetText(5, "2.5h", 1f);
                    element_shortplaytime.SetText(6, "", 0f);
                    break;
                case 5:
                    element_shortplaytime.bar.numberOfSteps = 7;
                    shortTimeArr = new float[7] { 0, 0.5f, 1, 1.5f, 2, 2.5f , 3 };
                    element_shortplaytime.SetText(0, "0h", 0f);
                    element_shortplaytime.SetText(1, "0.5h", 0.166f);
                    element_shortplaytime.SetText(2, "1h", 0.332f);
                    element_shortplaytime.SetText(3, "1.5h", 0.498f);
                    element_shortplaytime.SetText(4, "2h", 0.664f);
                    element_shortplaytime.SetText(5, "2.5h", 0.83f);
                    element_shortplaytime.SetText(6, "3h", 1f);
                    break;
                case 6:
                    element_shortplaytime.bar.numberOfSteps = 7;
                    shortTimeArr = new float[7] { 0, 0.5f, 1, 1.5f, 2, 3 ,4 };
                    element_shortplaytime.SetText(0, "0h", 0f);
                    element_shortplaytime.SetText(1, "0.5h", 0.166f);
                    element_shortplaytime.SetText(2, "1h", 0.332f);
                    element_shortplaytime.SetText(3, "1.5h", 0.498f);
                    element_shortplaytime.SetText(4, "2h", 0.664f);
                    element_shortplaytime.SetText(5, "3h", 0.83f);
                    element_shortplaytime.SetText(6, "4h", 1f);
                    break;
                default:
                    element_shortplaytime.bar.numberOfSteps = 7;
                    shortTimeArr = new float[7] { 0, 0.5f, 1, 2, 3, 4 , 6};
                    element_shortplaytime.SetText(0, "0h", 0f);
                    element_shortplaytime.SetText(1, "0.5h", 0.166f);
                    element_shortplaytime.SetText(2, "1h", 0.332f);
                    element_shortplaytime.SetText(3, "2h", 0.498f);
                    element_shortplaytime.SetText(4, "3h", 0.664f);
                    element_shortplaytime.SetText(5, "4h", 0.83f);
                    element_shortplaytime.SetText(6, "6h", 1f);
                    break;
            }
        }
        #endregion

        void UpdateView()
        {
            UpdatePlayTimeInfo(false);
            this.UpdateBlindAndDragIn(0 , false);
        }

        void UpdatePlayTimeInfo(bool isEvent)
        {
            if (element_playtime.step > 0)
            {
                element_shortplaytime.bar.transform.parent.gameObject.SetActive(true);
            }
            else element_shortplaytime.bar.transform.parent.gameObject.SetActive(false);
            this.UpdateShortTime(element_playtime.step);

            if(isEvent)element_shortplaytime.bar.value = 0;
            SetPreferredHeight();
        }

        /// <summary>
        /// 刷新满人自动开局逻辑
        /// </summary>
        void UpdateAutoPeopleInfo() {

            if (!element_toggle_autop.toggle.isOn)
            {
                element_autopeople.bar.value = 0;
                element_autopeople.RemoveEvent();
            }
            else
            {
                if (element_autopeople.bar.value > element_people.bar.value) {

                    element_autopeople.bar.value = element_people.bar.value;
                }
                element_autopeople.AddEvent();
            }
        }

        /// <summary>
        /// 刷新空座自动开桌逻辑
        /// </summary>
        void UpdatePlanEnptyPeopleInfo()
        {
            Text textAutoNewDeskTip = rc.Get<GameObject>("Text_autoNewDeskTip").GetComponent<Text>();
            textAutoNewDeskTip.text = string.Format(LanguageManager.mInstance.GetLanguageForKey("UIClub_RoomCreat_planTips"), $"{element_planEnptyPeople.step + 2}");
        }

        #region 适配
        async void SetPreferredHeight() {

            ETModel.TimerComponent timer = ETModel.Game.Scene.GetComponent<TimerComponent>();
            await timer.WaitAsync(100);
            float preferredHeight = scrollrect.content.GetComponent<VerticalLayoutGroup>().preferredHeight;
            if (preferredHeight > 0)
            {
                scrollrect.content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, preferredHeight);
            }
        }
        #endregion

        #region 读取默认配置
        void SetDefaultConfig() {

            int crt = 0;
            if (GameCache.Instance.reconfigs == null) GameCache.Instance.reconfigs = new RcConfig[6];
            if (GameCache.Instance.reconfigs[crt] == null)
            {
                GameCache.Instance.reconfigs[crt] = new RcConfig()
                {
                    //默认配置
                    element_people_value = 1,
                    element_autopeople_value = 0,
                    element_sizeblind_value = 0,
                    element_dragin_value = 1,
                    element_playtime_value = 0,
                    element_shortplaytime_value = 0,
                    element_thinktime_value = 1f,
                    element_note_value = 0,
                    element_multiple0_value = 0,
                    element_multiple1_value = 1,
                    element_toggle0_value = true,
                    element_toggle1_value = false,
                    element_toggle2_value = true,
                    element_toggle3_value = false,
                    element_toggle4_value = false,
                    element_toggle5_value = false,
                    element_toggle6_value = true,
                    element_toggle7_value = true,
                    element_toggle8_value = true,
                    element_toggle9_value = false,
                    element_toggle_autop_value = false,
                    element_planEnptyPeople_value = 1/7.0f
                };
            }

            element_toggle_autop.toggle.isOn = GameCache.Instance.reconfigs[crt].element_toggle_autop_value;

            element_people.bar.value = GameCache.Instance.reconfigs[crt].element_people_value;
            element_autopeople.bar.value = GameCache.Instance.reconfigs[crt].element_autopeople_value;
            element_sizeblind.bar.value = GameCache.Instance.reconfigs[crt].element_sizeblind_value;
            element_dragin.bar.value = GameCache.Instance.reconfigs[crt].element_dragin_value;
            element_playtime.bar.value = GameCache.Instance.reconfigs[crt].element_playtime_value;
            element_shortplaytime.bar.value = GameCache.Instance.reconfigs[crt].element_shortplaytime_value;
            element_thinktime.bar.value = GameCache.Instance.reconfigs[crt].element_thinktime_value;
            element_note.bar.value = GameCache.Instance.reconfigs[crt].element_note_value;
            element_multiple.bar0.value = GameCache.Instance.reconfigs[crt].element_multiple0_value;
            element_multiple.bar1.value = GameCache.Instance.reconfigs[crt].element_multiple1_value;
            element_planEnptyPeople.bar.value = GameCache.Instance.reconfigs[crt].element_planEnptyPeople_value;
            element_fee.bar.value = GameCache.Instance.reconfigs[crt].element_fee_value;
            element_toggle0.toggle.isOn = GameCache.Instance.reconfigs[crt].element_toggle0_value;
            element_toggle1.toggle.isOn = GameCache.Instance.reconfigs[crt].element_toggle1_value;
            element_toggle2.toggle.isOn = GameCache.Instance.reconfigs[crt].element_toggle2_value;
            element_toggle3.toggle.isOn = GameCache.Instance.reconfigs[crt].element_toggle3_value;
            element_toggle4.toggle.isOn = GameCache.Instance.reconfigs[crt].element_toggle4_value;
            element_toggle5.toggle.isOn = GameCache.Instance.reconfigs[crt].element_toggle5_value;
            element_toggle6.toggle.isOn = GameCache.Instance.reconfigs[crt].element_toggle6_value;
            element_toggle7.toggle.isOn = GameCache.Instance.reconfigs[crt].element_toggle7_value;
            element_toggle8.toggle.isOn = GameCache.Instance.reconfigs[crt].element_toggle8_value;
            element_toggle9.toggle.isOn = GameCache.Instance.reconfigs[crt].element_toggle9_value;
        }

        void SaveConfig() {

            int crt = 0;
            GameCache.Instance.reconfigs[crt].element_toggle_autop_value = element_toggle_autop.toggle.isOn;

            GameCache.Instance.reconfigs[crt].element_people_value = element_people.bar.value;
            GameCache.Instance.reconfigs[crt].element_autopeople_value = element_autopeople.bar.value;
            GameCache.Instance.reconfigs[crt].element_sizeblind_value = element_sizeblind.bar.value;
            GameCache.Instance.reconfigs[crt].element_dragin_value = element_dragin.bar.value;
            GameCache.Instance.reconfigs[crt].element_playtime_value = element_playtime.bar.value;
            GameCache.Instance.reconfigs[crt].element_shortplaytime_value = element_shortplaytime.bar.value;
            GameCache.Instance.reconfigs[crt].element_thinktime_value = element_thinktime.bar.value;
            GameCache.Instance.reconfigs[crt].element_note_value = element_note.bar.value;
            GameCache.Instance.reconfigs[crt].element_multiple0_value = element_multiple.bar0.value;
            GameCache.Instance.reconfigs[crt].element_multiple1_value = element_multiple.bar1.value;
            GameCache.Instance.reconfigs[crt].element_planEnptyPeople_value = element_planEnptyPeople.bar.value;
            GameCache.Instance.reconfigs[crt].element_fee_value = element_fee.bar.value;
            GameCache.Instance.reconfigs[crt].element_toggle0_value = element_toggle0.toggle.isOn;
            GameCache.Instance.reconfigs[crt].element_toggle1_value = element_toggle1.toggle.isOn;
            GameCache.Instance.reconfigs[crt].element_toggle2_value = element_toggle2.toggle.isOn;
            GameCache.Instance.reconfigs[crt].element_toggle3_value = element_toggle3.toggle.isOn;
            GameCache.Instance.reconfigs[crt].element_toggle4_value = element_toggle4.toggle.isOn;
            GameCache.Instance.reconfigs[crt].element_toggle5_value = element_toggle5.toggle.isOn;
            GameCache.Instance.reconfigs[crt].element_toggle6_value = element_toggle6.toggle.isOn;
            GameCache.Instance.reconfigs[crt].element_toggle7_value = element_toggle7.toggle.isOn;
            GameCache.Instance.reconfigs[crt].element_toggle8_value = element_toggle8.toggle.isOn;
            GameCache.Instance.reconfigs[crt].element_toggle9_value = element_toggle9.toggle.isOn;
        }
        #endregion

        private async void AsyncEnterRoom()
        {
            UIComponent.Instance.ShowNoAnimation(UIType.UIMatch_Loading);
            UIComponent.Instance.Remove(UIType.UIClub_RoomCreat);
            if (createType == 1)
                UIComponent.Instance.Remove(UIType.UIClub_Info);

            UIComponent.Instance.Remove(UIType.UIClub_PlanRoomList);

            if (createType == 2)
            {
                await UIComponent.Instance.ShowAsyncNoAnimation(UIType.UITexas, UIType.UIMatch);
            }
            else
            {
                await UIComponent.Instance.ShowAsyncNoAnimation(UIType.UITexas, UIType.UIClub);
            }
        }
    }
}

