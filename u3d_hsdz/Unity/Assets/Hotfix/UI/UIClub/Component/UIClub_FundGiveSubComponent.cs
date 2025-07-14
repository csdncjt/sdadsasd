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
    public class UIClub_FundGiveSubComponentSystem : AwakeSystem<UIClub_FundGiveSubComponent>
    {
        public override void Awake(UIClub_FundGiveSubComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_FundGiveSubComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        public void Awake()
        {

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
        }

        public override void OnShow(object obj)
        {
            object[] arr = obj as object[];
            string nickName = arr[0] as string;
            string userid = arr[1] as string;
            int iClubId = (int)arr[2];
            string avatar = arr[3] as string;

            //设置头像
            Head h = HeadCache.GetHead(eHeadType.USER, userid);
            if (h.headId != string.Empty)
            {
                rc.Get<GameObject>(UIClubAssitent.fundgivesub_img_icon).GetComponent<RawImage>().texture = h.t2d;
            }

            rc.Get<GameObject>(UIClubAssitent.fundgivesub_text_name).GetComponent<Text>().text = nickName;
            rc.Get<GameObject>(UIClubAssitent.fundgivesub_text_nick).GetComponent<Text>().text = string.Empty;
            rc.Get<GameObject>(UIClubAssitent.fundgivesub_text_id).GetComponent<Text>().text = $"ID:{userid}";

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.fundgivesub_img_bg)).onClick = (go) =>
            {
                Game.Scene.GetComponent<UIComponent>().Remove(UIType.UIClub_FundGiveSub);
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.fundgivesub_btn_close)).onClick = (go) =>
            {
                Game.Scene.GetComponent<UIComponent>().Remove(UIType.UIClub_FundGiveSub);
            };

            rc.Get<GameObject>(UIClubAssitent.fundgivesub_inputfield_num).GetComponent<InputField>().onValueChanged.AddListener((v)=> {

                if (v.Length == 0)
                {
                    rc.Get<GameObject>(UIClubAssitent.fundgivesub_inputfield_num).GetComponent<Text>().text = $"{0}";
                    return;
                }
                string strNum = v;
                if (strNum.Length > 9)
                {

                    strNum = strNum.Substring(0, 9);
                }
                //int iNum = int.Parse(strNum);//Debug.Log(iNum);
                rc.Get<GameObject>(UIClubAssitent.fundgivesub_inputfield_num).GetComponent<InputField>().text = strNum;
            });

            //设置下拉框中英文
            Dropdown dropdown = rc.Get<GameObject>(UIClubAssitent.fundgivesub_dropdown_type).GetComponent<Dropdown>();
            dropdown.ClearOptions();
            List<string> options = new List<string>() { LanguageManager.mInstance.GetLanguageForKey("UIClub_FundGiveSub_11"), LanguageManager.mInstance.GetLanguageForKey("UIClub_FundGiveSub_12") };
            dropdown.AddOptions(options);

            rc.Get<GameObject>(UIClubAssitent.fundgivesub_inputfield_num).GetComponent<InputField>().text = string.Empty;
            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.fundgivesub_btn_comfrim)).onClick = (go) =>
            {

                var str = rc.Get<GameObject>(UIClubAssitent.fundgivesub_inputfield_num).GetComponent<InputField>().text;
                if (str.Length <= 0) {

                    Log.Debug("请输入金额");
                    return;
                }

                float fnum = float.Parse(str);
                if (fnum <= 0)
                {
                    return;
                }

                int num = StringHelper.GetIntGold(fnum);

                //获取类型
                int givetype = rc.Get<GameObject>(UIClubAssitent.fundgivesub_dropdown_type).GetComponent<Dropdown>().value + 1;//服务器指定类型从1开始

                int dectnum = 20076;
                if (givetype == 2) dectnum = 20077;

                List<object> list = new List<object>() { nickName  , str};
                UIComponent.Instance.ShowNoAnimation(UIType.UIDialog,
                            new UIDialogComponent.DialogData()
                            {
                                type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                                title = LanguageManager.mInstance.GetLanguageForKey("adaptation10007"),//"温馨提示",
                                content = CPErrorCode.LanguageDescription(dectnum, list), //"您是否确定要给XXX发放XXXUSDT?",
                                contentCommit = LanguageManager.mInstance.GetLanguageForKey("club_trans_3"),//"是",
                                contentCancel = LanguageManager.mInstance.GetLanguageForKey("club_trans_4"),//否,
                                actionCommit = () => {

                                    try
                                    {
                                        WEB2_club_fund_give.RequestData fivefund_req = new WEB2_club_fund_give.RequestData()
                                        {
                                            amount = num,
                                            memberId = userid,
                                            clubId = iClubId,
                                            type = givetype
                                        };
                                        HttpRequestComponent.Instance.Send(
                                                WEB2_club_fund_give.API,
                                                WEB2_club_fund_give.Request(fivefund_req),
                                                (resData) => {

                                                    WEB2_club_fund_give.ResponseData fivefund_res = WEB2_club_fund_give.Response(resData);
                                                    if (fivefund_res.status == 0)
                                                    {
                                                        Log.Debug("发放成功");
                                                        UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_fund_14"));
                                                        //刷新信息
                                                        Game.EventSystem.Run(UIClubEventIdType.CLUB_FUNDINFO_HISTORY, iClubId);
                                                        Game.EventSystem.Run(UIClubEventIdType.CLUB_FUNDINFO, iClubId);
                                                        UIComponent.Instance.Remove(UIType.UIClub_FundGiveSub);
                                                    }
                                                    else
                                                    {
                                                        Log.Error($"Error status = {fivefund_res.status} msg = {fivefund_res.msg}");
                                                        UIComponent.Instance.Toast(fivefund_res.status);
                                                    }
                                                });
                                    }
                                    catch
                                    {

                                        Log.Debug("请输入正确的金额");
                                    }
                                }
                            });
            };
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
            rc = null;
        }
    }
}



