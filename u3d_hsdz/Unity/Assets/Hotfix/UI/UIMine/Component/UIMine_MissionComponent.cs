using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TaskItem = ETHotfix.WEB2_user_task_list.TaskItem;
using TaskElement = ETHotfix.WEB2_user_task_list.TaskElement;

namespace ETHotfix
{

    #region 刷新任务信息
    /// <summary>
    /// 刷新大厅俱乐部列表信息
    /// </summary>
    [Event(EventIdType.Mission_Refresh)]
    public class UIMissionEvent_Refresh : AEvent
    {
        public override void Run()
        {
            UI u = UIComponent.Instance.Get(UIType.UIMine_Mission);
            if (u != null)
            {
                u.GetComponent<UIMine_MissionComponent>().RefreshView();
            }
        }
    }
    #endregion

    [ObjectSystem]
    public class UIMine_MissionComponentAwakeSystem : AwakeSystem<UIMine_MissionComponent>
    {
        public override void Awake(UIMine_MissionComponent self)
        {
            self.Awake();
        }
    }

    public class UIMine_MissionComponent : UIBaseComponent
    {

        enum eMCStatus
        {
            GET = 3,//待领取
            UNCOMPLETED =  2,//未完成
            COMPLETED = 1,//已完成
        }

        class MisConf {

            public int id { get; set; }
            public string taskCode { get; set; }//任务的唯一code   服务器定义de
            public string icon { get; set; }
            public string name { get; set; }//对应多语言字段
            public string part1 { get; set; }//对应多语言字段
            public string part2 { get; set; }//对应多语言字段
            public int platform { get; set; }//平台  -1 全平台  0android 1ios
        }
        //config
        static Dictionary<string, MisConf> mDicMission = new Dictionary<string, MisConf>();

        private ReferenceCollector rc;
        CZScrollRect scrollcomponent;
        //list
        private List<TaskItem> taskItems;
        //
        private int startPos;
        int deviceType = 0;
        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            SetUpNav(UIType.UIMine_Mission);
            scrollcomponent = new CZScrollRect();
            scrollcomponent.prefab = rc.Get<GameObject>("uimine_mission_item");
            scrollcomponent.scrollRect = rc.Get<GameObject>("uimine_mission_sorcollview").GetComponent<ScrollRect>();
            scrollcomponent.onScrollObj = OnScrollObj;
            scrollcomponent.interval = 302;
            scrollcomponent.limitNum = 8;
            scrollcomponent.spacing = 5;
            scrollcomponent.Init();

            if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.IPhonePlayer) deviceType = 1;

            //初始化
            taskItems = new List<TaskItem>();
            //获取配置
            if (mDicMission.Count == 0) {

                TextAsset t = rc.Get<TextAsset>("MissionConfig");
                string[] lines = t.ToString().Split(new string[] { "\n" }, StringSplitOptions.None);
                foreach (string line in lines)
                {
                    if (line.Trim().Length > 0) {
                        Log.Debug(line);
                        MisConf mc = JsonHelper.FromJson<MisConf>(line);
                        mDicMission.Add(mc.taskCode, mc);
                    }
                }
            }
        }

        public override void OnShow(object obj)
        {
            //请求列表信息
            RefreshView();
            UISegmentControlComponent.SetUp(rc.transform, new UISegmentControlComponent.SegmentData()
            {
                Titles = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIMine_Mission_2"),
                OnClick = OnSegmentClick,
                N_S_Fonts = new[] { 48, 48 },
                N_S_Color = new Color32[] { new Color32(179, 142, 86, 255), new Color32(255, 255, 255, 255) },
                IsEffer = true
            });

            UIEventListener.Get(rc.Get<GameObject>("uimine_mission_declare")).onClick = (go) =>
            {
                UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("UIMine_Mission_12"));
            };

            //UIEventListener.Get(rc.Get<GameObject>("uimine_mission_btn_complete")).onClick = (go) =>
            //{

            //    //Log.Debug("已完成");
            //    UpdateSelect(1);

            //};

            //scrollcomponent.Refresh(10);
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
            scrollcomponent.Dispose();
            scrollcomponent = null;
            rc = null;
            UIMineModel.mInstance.RemoveMsgSummary(1);
        }

        void OnSegmentClick(GameObject go, int index)
        {

            if (index == 0) {

                UpdateSelect(0);
            }
            else
            {
                UpdateSelect(1);
            }
        }

        void OnScrollObj(GameObject obj, int index)
        {

            int dex = startPos + index; //Log.Debug($"show id = {taskItems[dex].taskId}");
            TaskItem taskItem = taskItems[dex];
            MisConf mc;
            if (!mDicMission.TryGetValue(taskItem.taskCode, out mc)) {

                Log.Error($"未知任务 id = {taskItem.taskId}");
                obj.transform.GetChild(2).GetComponent<Text>().text = string.Empty;
                obj.transform.GetChild(3).GetComponent<Text>().text = string.Empty;
                obj.transform.GetChild(5).GetComponent<Image>().color = new Color(1, 1, 1, 0);
                obj.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = string.Empty;
                return;
            }

            string taskName = LanguageManager.mInstance.GetLanguageForKey(mc.name);
            string taskReward = $"{LanguageManager.mInstance.GetLanguageForKey("UIMine_Mission_6")}:{StringHelper.ShowGold(taskItem.chip)}";

            string taskDetail = string.Empty;
            int max = Math.Min(taskItem.userTaskItems.Count, 2);
            string[] part = {mc.part1, mc.part2};
            for (int i = 0; i < max; ++i)
            {
                taskDetail += $"{LanguageManager.mInstance.GetLanguageForKey(part[i])}:{taskItem.userTaskItems[i].finishTimes}/{taskItem.userTaskItems[i].reqTimes}     ";
            }

            int taskStatus = taskItem.status;
            int action = taskItem.userTaskItems[0].action;

            obj.transform.GetChild(2).GetComponent<Text>().text = taskName;
            obj.transform.GetChild(3).GetComponent<Text>().text = taskReward;
            obj.transform.GetChild(4).GetComponent<Text>().text = taskDetail;

            if (taskStatus == (int)eMCStatus.COMPLETED) {
                //已完成
                obj.transform.GetChild(5).GetComponent<Image>().color = new Color(1,1,1,0);
                obj.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = $"{LanguageManager.mInstance.GetLanguageForKey("UIMine_Mission_3")}";
                UIEventListener.Get(obj.transform.GetChild(5).gameObject).onClick = (go) => {

                    //do nothing
                };

            } else if (taskStatus == (int)eMCStatus.UNCOMPLETED) {
                //前往
                obj.transform.GetChild(5).GetComponent<Image>().color = new Color(1, 1, 1, 0);
                if (mc.platform == -1 || mc.platform == deviceType)
                {

                    obj.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = $"{LanguageManager.mInstance.GetLanguageForKey("UIMine_Mission_7")}>>";
                    UIEventListener.Get(obj.transform.GetChild(5).gameObject).onClick = (go) => {

                        //take action
                        Log.Debug("take action");
                        for (int i = 0; i < max; ++i)
                        {
                            if (!taskItem.userTaskItems[i].finishTimes.Equals(taskItem.userTaskItems[0].reqTimes))
                            {
                                OnAction(taskItem.userTaskItems[i].action);
                                break;
                            }
                        }
                    };
                }
                else
                {
                    //不需要执行该任务
                    obj.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = string.Empty;
                    UIEventListener.Get(obj.transform.GetChild(5).gameObject).onClick = (go) => {};
                }
            }
            else{
                //待领取
                obj.transform.GetChild(5).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                obj.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = $"{LanguageManager.mInstance.GetLanguageForKey("YaoDou_receive")}";
                UIEventListener.Get(obj.transform.GetChild(5).gameObject).onClick = (go) => {

                    //take reward
                    Log.Debug("take reward");
                    GetReeward(taskItem.taskId);
                };
            }
        }

        //子任务下action的对应操作
        async void OnAction(int act) {

            switch (act) {

                case 1://修改名称跳转
                    await UIComponent.Instance.ShowAsyncNoAnimation(UIType.UIMine_UserInfoSetting);
                    break;
                case 2://修改头像跳转
                    await UIComponent.Instance.ShowAsyncNoAnimation(UIType.UIMine_UserInfoSetting);
                    break;
                case 3://打开网页  保存官网
                    UILobby_Triggle.CallOfficialWeb();
                    break;
                default:
                    Log.Debug($"error act = {act}");
                    UIComponent.Instance.Toast($"error act = {act}");
                    break;
            }
        }

        //获取任务列表刷新菜单
        public void RefreshView() {

            startPos = 0;
            taskItems.Clear();

            WEB2_user_task_list.RequestData taskList_req = new WEB2_user_task_list.RequestData()
            {
                deviceType = deviceType
            };
            HttpRequestComponent.Instance.Send(

                WEB2_user_task_list.API,
                WEB2_user_task_list.Request(taskList_req),
                (resData) =>
                {
                    WEB2_user_task_list.ResponseData taskList_res = WEB2_user_task_list.Response(resData);
                    if (taskList_res.status == 0)
                    {
                        
                        if (taskList_res.data != null && taskList_res.data.Count > 0) {

                            taskItems = taskList_res.data;Log.Debug($"taskItems.Count = {taskItems.Count}");
                            Sort();
                        }
                        else
                        {
                            Log.Error("Error taskList_res.data.Count = 0");
                        }
                    }
                    else
                    {
                        //获取列表失败
                        Log.Error($"Error status = {taskList_res.status} msg = {taskList_res.msg}");
                        UIComponent.Instance.Toast(taskList_res.status);
                    }
                });

            //模拟接口
            //taskItems = new List<TaskItem>();
            //for (int i = 0; i < 10; i++)
            //{

            //    TaskItem item = new TaskItem();
            //    item.chip = 200;
            //    item.checksum = "abc";
            //    item.status = UnityEngine.Random.Range(0, 4);
            //    item.taskCode = "dfa";
            //    item.taskId = 1000 + UnityEngine.Random.Range(1, 3);
            //    item.userTaskItems = new List<TaskElement>();
            //    TaskElement element = new TaskElement()
            //    {
            //        id = 0,
            //        action = UnityEngine.Random.Range(1, 4),
            //        finishTimes = UnityEngine.Random.Range(0, 2),
            //        reqTimes = UnityEngine.Random.Range(2, 4)
            //    };
            //    item.userTaskItems.Add(element); item.userTaskItems.Add(element);
            //    taskItems.Add(item);
            //}
            //Srot();
        }


        void GetReeward(int taskId) {

            WEB2_user_task_accepting_an_award.RequestData taskaward_req = new WEB2_user_task_accepting_an_award.RequestData()
            {
                taskId = taskId
            };
            HttpRequestComponent.Instance.Send(

                WEB2_user_task_accepting_an_award.API,
                WEB2_user_task_accepting_an_award.Request(taskaward_req),
                (resData) =>
                {
                    WEB2_user_task_accepting_an_award.ResponseData taskaward_res = WEB2_user_task_accepting_an_award.Response(resData);
                    if (taskaward_res.status == 0)
                    {
                        Log.Debug("领取成功");
                        UIComponent.Instance.Toast($"{LanguageManager.mInstance.GetLanguageForKey("UIMine_Mission_11")}"); 
                        //刷新列表
                        RefreshView();
                    }
                    else
                    {
                        Log.Debug("领取失败");
                        Log.Error($"Error status = {taskaward_res.status} msg = {taskaward_res.msg}");
                        UIComponent.Instance.Toast(taskaward_res.status);
                    }
                });
        }

        void Sort() {

            //sort
            if (taskItems.Count > 1)
            {
                for (int i = 0; i < taskItems.Count; ++i)
                {
                    for (int j = 0; j < taskItems.Count; ++j)
                    {
                        if (taskItems[i].status > taskItems[j].status)
                        {
                            var temp = taskItems[i];
                            taskItems[i] = taskItems[j];
                            taskItems[j] = temp;
                        }
                        else if (taskItems[i].status == taskItems[j].status && taskItems[i].taskId < taskItems[j].taskId)
                        {

                            var temp = taskItems[i];
                            taskItems[i] = taskItems[j];
                            taskItems[j] = temp;
                        }
                    }
                }
            }

            //debug
            //for (int i = 0; i < taskItems.Count; ++i)
            //{
            //    Log.Debug($"id = {taskItems[i].taskId} status = {taskItems[i].status}");
            //}
            UpdateSelect(0);
        }

        void UpdateSelect(int selectType) {

            int count = 0;
            for (int i = taskItems.Count - 1; i >= 0; --i)
            {
                
                if (taskItems[i].status == (int)eMCStatus.COMPLETED)//筛选是否已完成
                {
                    count++; //Log.Debug($"UpdateSelect id = {taskItems[i].taskId} status = {taskItems[i].status}");
                }
                else break;
            }
            //Log.Debug($"count = {count}");
            //show on scrollview
            if (selectType == 0) {

                startPos = 0;
                scrollcomponent.Refresh(taskItems.Count - count);
            }
            else
            {
                startPos = taskItems.Count - count; //Log.Debug($"startPos = {startPos}");
                scrollcomponent.Refresh(count);
            }
        }
    }
}
