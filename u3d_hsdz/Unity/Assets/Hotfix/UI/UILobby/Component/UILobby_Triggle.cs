using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

//用于进入大厅后触发任务类作业
namespace ETHotfix
{
    [ObjectSystem]
    public class UILobby_TriggleAwakeSystem : AwakeSystem<UILobby_Triggle>
    {
        public override void Awake(UILobby_Triggle self)
        {
            self.Awake();
        }
    }

    public class UILobby_Triggle : Component
    {

        public void Awake()
        {
            Log.Debug("triggle ---------------");
            IOSWebSaveMsg();
        }

        public override void Dispose()
        {
            base.Dispose();
        }


        #region IOS官方网站保存提示
        public void IOSWebSaveMsg() {

            if (UnityEngine.Application.platform != UnityEngine.RuntimePlatform.IPhonePlayer)
            {
                //非Iphone 不触发
                return;
            }
            WEB2_user_task_list.RequestData taskList_req = new WEB2_user_task_list.RequestData()
            {
                deviceType = 1
            };
            HttpRequestComponent.Instance.Send(

                WEB2_user_task_list.API,
                WEB2_user_task_list.Request(taskList_req),
                (resData) =>
                {
                    WEB2_user_task_list.ResponseData taskList_res = WEB2_user_task_list.Response(resData);
                    if (taskList_res.status == 0)
                    {

                        if (taskList_res.data != null && taskList_res.data.Count > 0)
                        {

                            for (int i = 0; i < taskList_res.data.Count; ++i) {

                                if (taskList_res.data[i].taskCode == "saveWebSite" && taskList_res.data[i].status == 2) {

                                    //调起web
                                    Log.Debug("调起web");
                                    CallOfficialWeb();
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        //获取列表失败
                        Log.Error($"Error status = {taskList_res.status} msg = {taskList_res.msg}");
                        UIComponent.Instance.Toast(taskList_res.status);
                    }
                });
        }

        //调起官方网站
        static public void CallOfficialWeb() {
            UIComponent.Instance.ShowNoAnimation(UIType.UICollectPageAlert);
        }

        #endregion
    }
}

