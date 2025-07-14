using System;
using System.Collections.Generic;
using System.Net;
using BestHTTP;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{

    struct FundItemInfo
    {
        public string strTypeName;
        public string strNum;
        public string strOpeartName;
        public string strOpeart;
    }

    [ObjectSystem]
    public class UIClub_FundDetailComponentSystem : AwakeSystem<UIClub_FundDetailComponent>
    {
        public override void Awake(UIClub_FundDetailComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_FundDetailComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        long lastTime;
        int iClubId;
        int iDou;//USDT 可提取USDT
        List<WEB2_club_fund_history.ListElement> list;
        CZScrollRect scrollcomponent;
        string[] lanKeys;
        public void Awake()
        {
            list = new List<WEB2_club_fund_history.ListElement>();
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            scrollcomponent = new CZScrollRect();
            scrollcomponent.prefab = rc.Get<GameObject>(UIClubAssitent.funddetail_info);
            scrollcomponent.scrollRect = rc.Get<GameObject>(UIClubAssitent.funddetail_scrollview).GetComponent<ScrollRect>();
            scrollcomponent.text_up = scrollcomponent.scrollRect.content.GetChild(0).gameObject;
            scrollcomponent.text_down = scrollcomponent.scrollRect.content.GetChild(1).gameObject;
            scrollcomponent.onRefresh = OnRefresh;
            scrollcomponent.onAppend = OnAppend;
            scrollcomponent.onScrollObj = OnScrollObj;
            scrollcomponent.onUpdateTextObj = OnUpdateTextObj;
            scrollcomponent.interval = 230;
            scrollcomponent.Init();

            lanKeys = new string[5] {

                LanguageManager.mInstance.GetLanguageForKey("club_fund_2"),
                LanguageManager.mInstance.GetLanguageForKey("club_fund_3"),
                LanguageManager.mInstance.GetLanguageForKey("club_fund_4"),
                LanguageManager.mInstance.GetLanguageForKey("club_fund_5"),
                LanguageManager.mInstance.GetLanguageForKey("club_fund_6"),
            };
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIClub_FundDetail);

            object[] arr = obj as object[];
            iClubId = (int)arr[0];
            WEB2_club_fund_detail.ResponseData res = arr[1] as WEB2_club_fund_detail.ResponseData;
            long fund = res.data.fund;
            iDou = (int)res.data.userChip;

            rc.Get<GameObject>(UIClubAssitent.funddetail_text_remain).GetComponent<Text>().text = StringHelper.ShowGold((int)res.data.fund, false); ;
            //RefreshView(res);
            GetDetailInfo(iClubId, 0, 1);

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.funddetail_btn_grant)).onClick = (go) =>
            {

                Game.EventSystem.Run(UIClubEventIdType.CLUB_FUNDGIVE, UIClubEvent.PassType.OPEN, iClubId, fund);
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.funddetail_btn_recharge)).onClick = async(go) =>
            {
                //打开充值界面
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_FundRecharge, new object[3] { iClubId, fund , iDou }, () => { });
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
            scrollcomponent.Dispose();
            UITrManager.Instance.Remove(rc.gameObject);
            rc = null;
        }

        public void RefreshView(WEB2_club_fund_history.ResponseData res) {

            scrollcomponent.hasMore = true;
            //direction  hasMore
            int direction = res.data.direction;
            if (direction == 1)
            {
                //刷新
                // Debug.Log($"刷新res.data.list = {res.data.list.Count}");
                list.Clear();
                lastTime = 0;
                if (res.data.list.Count > 0)
                {

                    foreach (WEB2_club_fund_history.ListElement l in res.data.list)
                    {
                        list.Add(l);
                        //Debug.Log($"刷新res lastTime = {l.time}");
                    }
                    lastTime = res.data.list[res.data.list.Count - 1].time;
                }
                scrollcomponent.Refresh(list.Count);
                //没有数据时提示
                if (list.Count == 0)
                {
                    rc.Get<GameObject>(UIClubAssitent.funddetail_scrollview_text_tip).transform.parent.gameObject.SetActive(true);
                    rc.Get<GameObject>(UIClubAssitent.funddetail_scrollview_text_tip).GetComponent<Text>().text = "暂无数据";
                }
                else
                {
                    rc.Get<GameObject>(UIClubAssitent.funddetail_scrollview_text_tip).transform.parent.gameObject.SetActive(false);
                    rc.Get<GameObject>(UIClubAssitent.funddetail_scrollview_text_tip).GetComponent<Text>().text = string.Empty;
                }
            }
            else if (direction == 0)
            {

                //追加
                // Debug.Log($"追加res.data.list = {res.data.list.Count}");
                if (res.data.list.Count > 0)
                {
                    foreach (WEB2_club_fund_history.ListElement l in res.data.list)
                    {
                        list.Add(l);
                        //Debug.Log($"追加res lastTime = {l.time}");
                    }
                    lastTime = res.data.list[res.data.list.Count - 1].time;
                }
                scrollcomponent.Append(res.data.list.Count);
            }
        }

        void OnScrollObj(GameObject obj , int index) {

            //Debug.Log($"OnScrollObj -- {obj.name} index = {index}");
            FundItemInfo fItemInfo = GetStringFundDetail(list[index].type, list[index].amount , list[index].userName , list[index].operatorName);
            GameObject ctxObject = obj.transform.GetChild(1).GetChild(0).gameObject;
            obj.transform.GetChild(0).GetComponent<Text>().text = $"{fItemInfo.strTypeName}";
            int len = StringUtil.GetStringLength(fItemInfo.strOpeartName);
            ctxObject.GetComponent<LayoutElement>().minWidth = Math.Max(500, len * 20);
            ctxObject.GetComponent<Text>().text = fItemInfo.strOpeartName;
            obj.transform.GetChild(2).GetComponent<Text>().text = $"{TimeHelper.GetDateTimer(list[index].time)}";
            obj.transform.GetChild(3).GetComponent<Text>().text = $"{fItemInfo.strNum}";
            if (!fItemInfo.strOpeart.Equals(string.Empty)) {

                obj.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                obj.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = fItemInfo.strOpeart;
            }
            else obj.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        }

        void OnUpdateTextObj(GameObject obj, CZScrollRect.TipType t) {

            Text txt = obj.GetComponent<Text>();
            switch (t) {

                case CZScrollRect.TipType.UNDO_REFRESH:
                    txt.text = lanKeys[0];
                    break;
                case CZScrollRect.TipType.PULL_REFRESH:
                    txt.text = lanKeys[1];
                    break;
                case CZScrollRect.TipType.UNDO_APPEND:
                    txt.text = lanKeys[2];
                    break;
                case CZScrollRect.TipType.NODATA:
                    txt.text = lanKeys[3];
                    break;
                case CZScrollRect.TipType.PULL_AAPEND:
                    txt.text = lanKeys[4];
                    break;
                default:
                    txt.text = "";
                    break;
            }
        }

        void OnRefresh() {

            GetDetailInfo(iClubId, 0, 1);
        }

        void OnAppend()
        {
            GetDetailInfo(iClubId, lastTime, 0);
        }

        FundItemInfo GetStringFundDetail(int type , int intAmount , string userName , string operatorName) {

            FundItemInfo fItemInfo;
            fItemInfo.strTypeName = string.Empty;
            fItemInfo.strNum = string.Empty;
            fItemInfo.strOpeartName = string.Empty;
            fItemInfo.strOpeart = string.Empty;
            List<object> strs;

            string amount = StringHelper.ShowGold(intAmount, false);

            switch (type) {
                case 1://发放基金到用户可提账户中
                    strs = new List<object>() { userName , amount };
                    fItemInfo.strTypeName = CPErrorCode.LanguageDescription(20073, strs);//给XX发放基金
                    fItemInfo.strNum = $"-{amount}";
                    fItemInfo.strOpeartName = $"{LanguageManager.mInstance.GetLanguageForKey("club_fund_1")}:{operatorName}";//操作人
                    fItemInfo.strOpeart = string.Empty;
                    break;
                case 9://发放基金到用户不可提账户中
                    strs = new List<object>() { userName , amount };
                    fItemInfo.strTypeName = CPErrorCode.LanguageDescription(20074, strs);//给XX发放基金
                    fItemInfo.strNum = $"-{amount}";
                    fItemInfo.strOpeartName = $"{LanguageManager.mInstance.GetLanguageForKey("club_fund_1")}:{operatorName}";//操作人
                    fItemInfo.strOpeart = string.Empty;
                    break;
                case 2:
                    fItemInfo.strTypeName = LanguageManager.mInstance.GetLanguageForKey("club_fund_8");//充值基金池
                    if (intAmount >= 0) fItemInfo.strNum = $"+{amount}";
                    else fItemInfo.strNum = $"{amount}";
                    fItemInfo.strOpeartName = $"{LanguageManager.mInstance.GetLanguageForKey("club_fund_1")}:{operatorName}";//操作人
                    fItemInfo.strOpeart = string.Empty;
                    break;
                case 3:
                    fItemInfo.strTypeName = LanguageManager.mInstance.GetLanguageForKey("club_fund_9");//俱乐部牌局收益
                    if (intAmount >= 0) fItemInfo.strNum = $"+{amount}";
                    else fItemInfo.strNum = $"{amount}";
                    strs = new List<object>() { operatorName };
                    fItemInfo.strOpeartName = CPErrorCode.LanguageDescription(20066, strs);//牌局##分润收益
                    fItemInfo.strOpeart = StringHelper.ShowBlindName(userName);
                    break;
                case 8:
                    fItemInfo.strTypeName = LanguageManager.mInstance.GetLanguageForKey("club_fund_19");//联盟牌局收益
                    if (intAmount >= 0) fItemInfo.strNum = $"+{amount}";
                    else fItemInfo.strNum = $"{amount}";
                    strs = new List<object>() { operatorName };
                    fItemInfo.strOpeartName = CPErrorCode.LanguageDescription(20066, strs);//牌局##分润收益
                    
                    fItemInfo.strOpeart = StringHelper.ShowBlindName(userName);
                    break;
                case 4:
                    fItemInfo.strTypeName = LanguageManager.mInstance.GetLanguageForKey("club_fund_10");//战绩流水返佣
                    if (intAmount >= 0) fItemInfo.strNum = $"+{amount}";
                    else fItemInfo.strNum = $"{amount}";
                    fItemInfo.strOpeart = string.Empty;
                    break;
                case 5:
                    fItemInfo.strTypeName = LanguageManager.mInstance.GetLanguageForKey("club_fund_11");//平台代充支付渠道充值
                    if (intAmount >= 0) fItemInfo.strNum = $"+{amount}";
                    else fItemInfo.strNum = $"{amount}";
                    fItemInfo.strOpeartName = $"{LanguageManager.mInstance.GetLanguageForKey("club_fund_1")}:{operatorName}";//操作人
                    fItemInfo.strOpeart = string.Empty;
                    break;
                case 6:
                    fItemInfo.strTypeName = LanguageManager.mInstance.GetLanguageForKey("club_fund_12");//提取记录
                    if (intAmount >= 0) fItemInfo.strNum = $"+{amount}";
                    else fItemInfo.strNum = $"{amount}";
                    fItemInfo.strOpeartName = $"{LanguageManager.mInstance.GetLanguageForKey("club_fund_1")}:{operatorName}";//操作人
                    fItemInfo.strOpeart = string.Empty;
                    break;
                case 7:
                    fItemInfo.strTypeName = LanguageManager.mInstance.GetLanguageForKey("club_fund_13");//分享赚USDT返还
                    if (intAmount >= 0) fItemInfo.strNum = $"+{amount}";
                    else fItemInfo.strNum = $"{amount}";
                    strs = new List<object>() { userName };
                    fItemInfo.strOpeartName = CPErrorCode.LanguageDescription(20067, strs);//给XX发放USDT
                    fItemInfo.strOpeart = string.Empty;
                    break;
                case 10:
                    strs = new List<object>() { operatorName };
                    if (intAmount >= 0)
                    {
                        fItemInfo.strNum = $"+{amount}";
                        fItemInfo.strTypeName = LanguageManager.mInstance.GetLanguageForKey("club_fund_22");//俱乐部保险返利收益
                        fItemInfo.strOpeartName = CPErrorCode.LanguageDescription(20173, strs);//牌局##分润收益
                    }
                    else
                    {
                        fItemInfo.strNum = $"{amount}";
                        fItemInfo.strTypeName = LanguageManager.mInstance.GetLanguageForKey("club_fund_23");//俱乐部保险返利支出,
                        fItemInfo.strOpeartName = CPErrorCode.LanguageDescription(20174, strs);//牌局##分润收益
                    }
                    
                    fItemInfo.strOpeart = userName;
                    break;
                case 11:
                    strs = new List<object>() { operatorName };
                    if (intAmount >= 0)
                    {
                        fItemInfo.strNum = $"+{amount}";
                        fItemInfo.strTypeName = LanguageManager.mInstance.GetLanguageForKey("club_fund_20");//联盟保险返利收益
                        fItemInfo.strOpeartName = CPErrorCode.LanguageDescription(20173, strs);//牌局##分润收益
                    }
                    else
                    {
                        fItemInfo.strNum = $"{amount}";
                        fItemInfo.strTypeName = LanguageManager.mInstance.GetLanguageForKey("club_fund_21");//联盟保险返利支出,
                        fItemInfo.strOpeartName = CPErrorCode.LanguageDescription(20174, strs);//牌局##分润收益
                    }
                    fItemInfo.strOpeart = userName;
                    break;
                case 12:
                    fItemInfo.strTypeName = LanguageManager.mInstance.GetLanguageForKey("club_fund_25");//俱乐部彩金返利收益
                    if (intAmount >= 0) fItemInfo.strNum = $"+{amount}";
                    else fItemInfo.strNum = $"{amount}";
                    strs = new List<object>() { operatorName };
                    fItemInfo.strOpeartName = CPErrorCode.LanguageDescription(20175, strs);//牌局##分润收益
                    fItemInfo.strOpeart = userName;
                    break;
                case 13:
                    fItemInfo.strTypeName = LanguageManager.mInstance.GetLanguageForKey("club_fund_24");//联盟彩金返利收益
                    if (intAmount >= 0) fItemInfo.strNum = $"+{amount}";
                    else fItemInfo.strNum = $"{amount}";
                    strs = new List<object>() { operatorName };
                    fItemInfo.strOpeartName = CPErrorCode.LanguageDescription(20175, strs);//牌局##分润收益
                    fItemInfo.strOpeart = userName;
                    break;
                case 14:
                    fItemInfo.strTypeName = LanguageManager.mInstance.GetLanguageForKey("club_fund_26");//俱乐部返佣收益
                    if (intAmount >= 0) fItemInfo.strNum = $"+{amount}";
                    else fItemInfo.strNum = $"{amount}";
                    strs = new List<object>() { operatorName };
                    fItemInfo.strOpeartName = string.Empty;
                    fItemInfo.strOpeart = string.Empty;
                    break;
                case 15:
                    fItemInfo.strTypeName = LanguageManager.mInstance.GetLanguageForKey("club_fund_27");//联盟返佣收益
                    if (intAmount >= 0) fItemInfo.strNum = $"+{amount}";
                    else fItemInfo.strNum = $"{amount}";
                    strs = new List<object>() { operatorName };
                    fItemInfo.strOpeartName = string.Empty;
                    fItemInfo.strOpeart = string.Empty;
                    break;
                default:
                    fItemInfo.strTypeName = $"unknow = {type}";
                    fItemInfo.strNum = string.Empty;
                    fItemInfo.strOpeartName = string.Empty;
                    fItemInfo.strOpeart = string.Empty;
                    break;
            }
            return fItemInfo;
        }

        /// <summary>
        /// 刷新交易列表的信息
        /// </summary>
        /// <param name="_iClubId">俱乐部ID</param>
        /// <param name="_time">时间戳</param>
        /// <param name="_direction">0加载更多 1刷新</param>
        public void GetDetailInfo(int _iClubId , long _time , int _direction) {

            WEB2_club_fund_history.RequestData history_req = new WEB2_club_fund_history.RequestData()
            {
                clubId = _iClubId,
                time = _time,
                direction = _direction
            };
            HttpRequestComponent.Instance.Send(

                WEB2_club_fund_history.API,
                WEB2_club_fund_history.Request(history_req),
                (resData) =>
                {
                    WEB2_club_fund_history.ResponseData history_res = WEB2_club_fund_history.Response(resData);
                    if (history_res.status == 0)
                    {
                        RefreshView(history_res);
                    }
                    else
                    {
                        Log.Error($"Error status = {history_res.status} msg = {history_res.msg}");
                        UIComponent.Instance.Toast(history_res.status);
                        scrollcomponent.vertical = true;
                    }
                });
        }
    }
}

