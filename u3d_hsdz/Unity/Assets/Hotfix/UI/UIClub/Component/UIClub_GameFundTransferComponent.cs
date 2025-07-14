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
    public class UIClub_GameFundTransferComponentSystem : AwakeSystem<UIClub_GameFundTransferComponent>
    {
        public override void Awake(UIClub_GameFundTransferComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_GameFundTransferComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        int rechargeNum;
        int iClubId;
        int iDou;
        bool isRefresh;
        int itype;//1转入2转出
        public void Awake()
        {
            isRefresh = false;
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
        }

        public override void OnShow(object obj)
        {
            object[] arr = obj as object[];
            iClubId = (int)arr[0];
            long fund = (long)arr[1];
            long gfund = (long)arr[2];
            itype= (int)arr[3];
            string title = "";
            Text Text1 = rc.Get<GameObject>("Text1").GetComponent<Text>();
            Text Text2 = rc.Get<GameObject>("Text2").GetComponent<Text>();

            if (itype == 1)
            {
                title = LanguageManager.mInstance.GetLanguageForKey("UIClub_GameFundIn");
                iDou = (int)fund;

                Text1.text = LanguageManager.mInstance.GetLanguageForKey("UIClub_GameFundDetail");
                Text2.text = LanguageManager.mInstance.GetLanguageForKey("UIClub_FundDetail");
            }
            else
            {
                title = LanguageManager.mInstance.GetLanguageForKey("UIClub_GameFundOut");
                iDou = (int)gfund;

                Text1.text = LanguageManager.mInstance.GetLanguageForKey("UIClub_FundDetail");
                Text2.text = LanguageManager.mInstance.GetLanguageForKey("UIClub_GameFundDetail");
            }
            SetUpNav(title, UIType.UIClub_GameFundTransfer);

            rc.Get<GameObject>(UIClubAssitent.fundrecharge_inputfield_limt).GetComponent<InputField>().text = string.Empty;
            rc.Get<GameObject>(UIClubAssitent.fundrecharge_text_cost).GetComponent<Text>().text = "0";
            RefreshView();
            //btn
            rc.Get<GameObject>(UIClubAssitent.fundrecharge_inputfield_limt).GetComponent<InputField>().onValueChanged.AddListener((v)=> {

                if (v.Length == 0)
                {
                    rc.Get<GameObject>(UIClubAssitent.fundrecharge_text_cost).GetComponent<Text>().text = $"{0}";
                    return;
                }
                string strNum = v;
                if (strNum.Length > 9) {

                    strNum = strNum.Substring(0, 9);
                }
                float iNum = float.Parse(strNum);//Debug.Log(iNum);

                rc.Get<GameObject>(UIClubAssitent.fundrecharge_inputfield_limt).GetComponent<InputField>().text = strNum.ToString();
                rechargeNum = StringHelper.GetIntGold(iNum);
                rc.Get<GameObject>(UIClubAssitent.fundrecharge_text_cost).GetComponent<Text>().text = $"{iNum}";
                //适配长度
                RectTransform rectTransform = rc.Get<GameObject>(UIClubAssitent.fundrecharge_layout_cost).GetComponent<RectTransform>();
                Vector2 offmax = rectTransform.offsetMax;
                offmax.x = -Mathf.Max(strNum.Length - 1, 0) * 23;
                rectTransform.offsetMax = offmax;
            });


            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.fundrecharge_btn_recharge)).onClick = (go) =>
            {
                string strNum = rc.Get<GameObject>(UIClubAssitent.fundrecharge_inputfield_limt).GetComponent<InputField>().text;
                if (strNum.Length <= 0)
                {
                    return;
                }
                float num = float.Parse(strNum);
                if (num <= 0)
                {
                    return;
                }

                num = StringHelper.GetIntGold(num);
                if (iDou < num)
                {
                    if (itype == 1)
                    {
                        UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("error1209"));
                    }
                    else
                    {
                        UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_gfund_4"));
                    }
                    return;
                }

                UIComponent.Instance.ShowNoAnimation(UIType.UIDialog,
                            new UIDialogComponent.DialogData()
                            {
                                type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                                title = LanguageManager.mInstance.GetLanguageForKey("club_trans_0"),//"系统消息",
                                content = LanguageManager.mInstance.GetLanguageForKey("club_gfund_5"),//"是否进行转账操作?",
                                contentCommit = LanguageManager.mInstance.GetLanguageForKey("club_trans_3"),//"是",
                                contentCancel = LanguageManager.mInstance.GetLanguageForKey("club_trans_4"),//否,
                                actionCommit = () =>
                                {

                                    try
                                    {
                                        WEB2_club_fund_trade.RequestData recharge_req = new WEB2_club_fund_trade.RequestData()
                                        {

                                            clubId = iClubId,
                                            fund = StringHelper.GetIntGold(float.Parse(strNum)),
                                            fundType = itype - 1
                                        };
                                        HttpRequestComponent.Instance.Send(
                                                            WEB2_club_fund_trade.API,
                                                            WEB2_club_fund_trade.Request(recharge_req),
                                                            this.RechargeCall);
                                    }
                                    catch (Exception e)
                                    {
                                        UIComponent.Instance.Toast($"{e}");
                                    }
                                }
                            });


            };

            UITrManager.Instance.Append(UIComponent.Instance.Get(UIType.UIClub_GameFundDetail).GameObject, rc.gameObject);
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

            //刷新信息
            if(isRefresh) Game.EventSystem.Run(UIClubEventIdType.CLUB_GAMEFUNDINFO, iClubId);
        }

        void RechargeCall(string resData) {

            WEB2_club_fund_trade.ResponseData clubrecharge_res = WEB2_club_fund_trade.Response(resData);
            if (clubrecharge_res.status == 0)
            {
                UIComponent.Instance.Toast(LanguageManager.mInstance.GetLanguageForKey("club_gfund_6"));//充值成功
                //成功后获取刷新USDT信息
                iDou -= rechargeNum;
                RefreshView();
                isRefresh = true;
            }
            else
            {
                Log.Error($"Error status = {clubrecharge_res.status} msg = {clubrecharge_res.msg}");
                UIComponent.Instance.Toast(clubrecharge_res.status);
            }
        }

        void RefreshView() {

            rc.Get<GameObject>(UIClubAssitent.fundrecharge_text_remain).GetComponent<Text>().text = StringHelper.ShowGold(iDou, false);
        }
    }
}



