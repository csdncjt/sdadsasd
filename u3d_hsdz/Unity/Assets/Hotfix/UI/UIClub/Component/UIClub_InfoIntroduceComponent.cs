
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
    public class UIClub_InfoIntroduceComponentAwakeSystem : AwakeSystem<UIClub_InfoIntroduceComponent>
    {
        public override void Awake(UIClub_InfoIntroduceComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_InfoIntroduceComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        int iClubId;
        public void Awake()
        {

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIClub_InfoIntroduce);

            object[] arr = obj as object[];
            iClubId = (int)arr[0];
            string clubDetail = arr[1] as string;
            rc.Get<GameObject>(UIClubAssitent.infointroduce_inputfield_detail).GetComponent<InputField>().text = clubDetail;

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.infointroduce_btn_sumbit)).onClick = (go) =>
            {
                string strDetail = rc.Get<GameObject>(UIClubAssitent.infointroduce_inputfield_detail).GetComponent<InputField>().text;
                //限制50个字符长度
                int len = StringUtil.GetStringLength(strDetail);
                if (len == 0) return;
                if (len > 50)
                {
                    UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_info_7"));//简介长度超出
                    return;
                }

                if (SensitiveWordComponent.Instance.SensitiveWordJudge(LanguageManager.mInstance.GetLanguageForKey("UIClub_InfoIntroduce_44vQpjkd"), strDetail))
                {
                    return;
                }

                WEB2_club_modify.RequestData update_req = new WEB2_club_modify.RequestData()
                {
                    clubId = iClubId,
                    content = strDetail,
                    type = 2,
                };
                HttpRequestComponent.Instance.Send(
                                    WEB2_club_modify.API,
                                    WEB2_club_modify.Request(update_req),
                                    this.UpdateCall);
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
            UITrManager.Instance.Remove(rc.gameObject);
            rc = null;
        }

        void UpdateCall(string resData)
        {
            Log.Debug(resData);
            WEB2_club_modify.ResponseData update_res = WEB2_club_modify.Response(resData);
            if (update_res.status == 0) {

                Log.Debug("修改成功");
                Game.EventSystem.Run(UIClubEventIdType.CLUB_INFO, iClubId);
                UIComponent.Instance.Remove(UIType.UIClub_InfoIntroduce);
                UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_info_8"));//修改成功
            }
            else
            {
                Log.Error($"Error status = {update_res.status} , msg = {update_res.msg}");
                UIComponent.Instance.Toast(update_res.status);
            }
        }
    }
}



