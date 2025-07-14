using ETModel;
namespace ETHotfix
{

    public class UIClubEvent
    {

        public enum PassType
        {
            OPEN = 0,
            REFEARSH = 1,
        }
    }

    public static class UIClubEventIdType
    {

        public const string CLUB_LIST = "CLUB_LIST";//刷新大厅俱乐部列表信息

        public const string CLUB_HOT = "CLUB_HOT";//推荐俱乐部
        public const string CLUB_HOT_LIST = "CLUB_HOT_LIST";//推荐俱乐部列表

        public const string CLUB_INFO = "CLUB_INFO";
        public const string CLUB_TRIBE = "CLUB_TRIBE";
        public const string CLUB_TRIBE_INFO = "CLUB_TRIBE_INFO";
        public const string CLUB_TRIBE_CLUBINFO = "CLUB_TRIBE_CLUBINFO";

        public const string CLUB_FUNDINFO = "CLUB_FUNDINFO";
        public const string CLUB_FUNDINFO_HISTORY = "CLUB_FUNDINFO_HISTORY";
        public const string CLUB_FUNDGIVE = "CLUB_FUNDGIVE";
        public const string CLUB_MLIST = "CLUB_MLIST";
        public const string CLUB_MLISTINFO = "CLUB_MLISTINFO";

        public const string CLUB_GAMEFUNDINFO = "CLUB_GAMEFUNDINFO";
        public const string CLUB_GAMEFUNDINFO_HISTORY = "CLUB_GAMEFUNDINFO_HISTORY";

        public const string CLUB_ROOMCREAT_TIP = "CLUB_ROOMCREAT_TIP";//创建牌局提示
        public const string CLUB_MLISTINFO_MARKER = "CLUB_MLISTINFO_MARKER";//俱乐部成员打法备注修改
    }


    #region 大厅俱乐部列表信息
    /// <summary>
    /// 刷新大厅俱乐部列表信息
    /// </summary>
    [Event(UIClubEventIdType.CLUB_LIST)]
    public class UIClubEvent_ClubList : AEvent
    {
        public override void Run()
        {
            UI u = UIComponent.Instance.Get(UIType.UIClub);
            if (u != null)
            {
                u.GetComponent<UIClubComponent>().GetClubListInfo();
            }
        }
    }
    #endregion

    #region 推荐俱乐部

    [Event(UIClubEventIdType.CLUB_HOT)]
    public class UIClubEvent_ClubHot : AEvent
    {
        public override void Run()
        {
            Log.Debug($"请求推荐俱乐部信息");
            WEB2_club_hotClubs.RequestData clubhot_req = new WEB2_club_hotClubs.RequestData()
            {
                reqType = 1,
            };
            HttpRequestComponent.Instance.Send(
                                WEB2_club_hotClubs.API,
                                WEB2_club_hotClubs.Request(clubhot_req),
                                this.ClubHotCall);
        }
        async void ClubHotCall(string resData)
        {
            WEB2_club_hotClubs.ResponseData clubhot_res = WEB2_club_hotClubs.Response(resData);
            if (clubhot_res.status == 0)
            {
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_Join, clubhot_res, () => { });
            }
            else
            {
                UIComponent.Instance.Toast(clubhot_res.status);
            }
        }
    }

    [Event(UIClubEventIdType.CLUB_HOT_LIST)]
    public class UIClubEvent_ClubHotList : AEvent
    {
        public override void Run()
        {
            Log.Debug($"请求推荐俱乐部列表信息");
            WEB2_club_hotClubs.RequestData clubhot_req = new WEB2_club_hotClubs.RequestData()
            {
                reqType = 2,
            };
            HttpRequestComponent.Instance.Send(
                                WEB2_club_hotClubs.API,
                                WEB2_club_hotClubs.Request(clubhot_req),
                                this.ClubHotCall);
        }
        async void ClubHotCall(string resData)
        {
            WEB2_club_hotClubs.ResponseData clubhot_res = WEB2_club_hotClubs.Response(resData);
            if (clubhot_res.status == 0)
            {
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_JoinRecommend, clubhot_res, () => { });
            }
            else
            {
                UIComponent.Instance.Toast(clubhot_res.status);
            }
        }
    }

    #endregion

    #region 俱乐部详情

    [Event(UIClubEventIdType.CLUB_INFO)]
    public class UIClubEvent_ClubInfo : AEvent<int>
    {
        public override void Run(int _iClubId)
        {
            Log.Debug($"请求俱乐部{_iClubId}详情信息");
            WEB2_club_view.RequestData clubview_req = new WEB2_club_view.RequestData()
            {
                clubId = _iClubId.ToString()
            };
            HttpRequestComponent.Instance.Send(
                                WEB2_club_view.API,
                                WEB2_club_view.Request(clubview_req),
                                this.OpenClubInfo);
        }
        async void OpenClubInfo(string resData) { 

            WEB2_club_view.ResponseData clubview_res = WEB2_club_view.Response(resData);
            if (clubview_res.status == 0)
            {

                UI u = UIComponent.Instance.Get(UIType.UIClub_Info);
                if (u == null)
                {
                    await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_Info, clubview_res, () => { });
                }
                else
                {
                    //已经打开了就刷新信息
                    u.GetComponent<UIClub_InfoComponent>().UpdateClubInfo(clubview_res);
                }
            }
            else
            {
                Log.Debug($"请求俱乐部信息失败");
                UIComponent.Instance.Toast(clubview_res.status);
            }
        }
    }

    #endregion

    #region 俱乐部的联盟列表
    [Event(UIClubEventIdType.CLUB_TRIBE)]
    public class UIClubEvent_Tribe : AEvent<int>
    {
        public override void Run(int _iClubId)
        {
            Log.Debug($"请求{_iClubId}的联盟信息");
            WEB2_tribe_mine.RequestData alliview_req = new WEB2_tribe_mine.RequestData()
            {
                clubId = _iClubId
            };
            HttpRequestComponent.Instance.Send(
                WEB2_tribe_mine.API,
                WEB2_tribe_mine.Request(alliview_req),
                async(resData) =>{

                    WEB2_tribe_mine.ResponseData alliview_res = WEB2_tribe_mine.Response(resData);
                    if (alliview_res.status == 0)
                    {
                        UI u = UIComponent.Instance.Get(UIType.UIClub_Tribe);
                        if (u == null)
                        {
                            await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_Tribe, new object[2] { _iClubId, alliview_res }, () => { });
                        }
                        else
                        {
                            //已经打开了就刷新信息
                            u.GetComponent<UIClub_TribeComponent>().UpdateViewInfo(alliview_res);
                        }
                        
                    }
                    else
                    {
                        Log.Debug($"Error status = {alliview_res.status} msg = {alliview_res.msg}");
                        UIComponent.Instance.Toast(alliview_res.status);
                    }
                });
        }
    }

    #endregion

    #region 联盟详细信息

    /// <summary>
    /// 联盟详细信息
    /// </summary>
    [Event(UIClubEventIdType.CLUB_TRIBE_INFO)]
    public class UIClubEvent_TribeInfo : AEvent<int , string>
    {

        int iTribeId;
        string strTribeName;

        public override void Run(int _tribeId , string _tribeName)
        {
            Log.Debug($"请求{_tribeId}联盟详情信息");
            iTribeId = _tribeId;
            strTribeName = _tribeName;
            WEB2_tribe_detail.RequestData tribeclulist_req = new WEB2_tribe_detail.RequestData()
            {
                tribeId = iTribeId
            };
            HttpRequestComponent.Instance.Send(WEB2_tribe_detail.API,WEB2_tribe_detail.Request(tribeclulist_req), TribeDetailCall);
        }

        void TribeDetailCall(string resData) {

            Log.Debug(resData);
            WEB2_tribe_detail.ResponseData tribedetail_res = WEB2_tribe_detail.Response(resData);
            if (tribedetail_res.status == 0)
            {
                WEB2_tribe_member.RequestData tribemenber_req = new WEB2_tribe_member.RequestData()
                {
                    tribeId = iTribeId
                };
                HttpRequestComponent.Instance.Send(WEB2_tribe_member.API, WEB2_tribe_member.Request(tribemenber_req), async (res) => {

                    Log.Debug(res);
                    WEB2_tribe_member.ResponseData tribemenber_res = WEB2_tribe_member.Response(res);
                    if (tribemenber_res.status == 0)
                    {
                        await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_TribeInfo, new object[4] { iTribeId, strTribeName, tribedetail_res , tribemenber_res }, () => { });
                    }
                    else
                    {
                        Log.Debug($"Error status = {tribemenber_res.status} msg = {tribemenber_res.msg}");
                        UIComponent.Instance.Toast(tribemenber_res.status);
                    }

                });
            }
            else
            {
                Log.Debug($"Error status = {tribedetail_res.status} msg = {tribedetail_res.msg}");
                UIComponent.Instance.Toast(tribedetail_res.status);
            }
        }
    }

    #endregion

    #region 基金信息
    //刷新基金信息
    [Event(UIClubEventIdType.CLUB_FUNDINFO)]
    public class UIClubEvent_FundInfo : AEvent<int>
    {
        public override void Run(int _iClubId)
        {
            Log.Debug($"俱乐部{_iClubId}的基金详情信息");
            WEB2_club_fund_detail.RequestData funddetail_req = new WEB2_club_fund_detail.RequestData()
            {
                clubId = _iClubId,
            };
            HttpRequestComponent.Instance.Send(
                    WEB2_club_fund_detail.API,
                    WEB2_club_fund_detail.Request(funddetail_req),
                    async (resData) => {

                        WEB2_club_fund_detail.ResponseData funddetail_res = WEB2_club_fund_detail.Response(resData);
                        if (funddetail_res.status == 0)
                        {

                            UI u = UIComponent.Instance.Get(UIType.UIClub_FundDetail);
                            if (u == null)
                            {
                                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_FundDetail, new object[2] { _iClubId, funddetail_res }, () => { });
                            }
                            else
                            {
                                //已经打开了就刷新信息
                                u.GetComponent<UIClub_FundDetailComponent>().OnShow(new object[2] { _iClubId, funddetail_res });
                            }
                        }
                        else
                        {
                            Log.Debug($"Error status = {funddetail_res.status} msg = {funddetail_res.msg}");
                            UIComponent.Instance.Toast(funddetail_res.status);
                        }
                    });
        }
    }

    //刷新基金交易列表信息
    [Event(UIClubEventIdType.CLUB_FUNDINFO_HISTORY)]
    public class UIClubEvent_FundInfoRefresh : AEvent<int>
    {
        public override void Run(int _clubId)
        {
            UI u = UIComponent.Instance.Get(UIType.UIClub_FundDetail);
            if (u != null)
            {
                u.GetComponent<UIClub_FundDetailComponent>().GetDetailInfo(_clubId, 0, 1);
            }
        }
    }

    //打开基金成员列表
    [Event(UIClubEventIdType.CLUB_FUNDGIVE)]
    public class UIClubEvent_FundGive : AEvent<UIClubEvent.PassType, int , long>
    {
        public override void Run(UIClubEvent.PassType type , int _clubId, long fund)
        {
            WEB2_club_mlist.RequestData clubmlist_req = new WEB2_club_mlist.RequestData()
            {
                clubId = _clubId
            };
            HttpRequestComponent.Instance.Send(
                    WEB2_club_mlist.API,
                    WEB2_club_mlist.Request(clubmlist_req),
                    async (resData) => {

                        Log.Debug(resData);
                        WEB2_club_mlist.ResponseData clubmlist_res = WEB2_club_mlist.Response(resData);
                        if (clubmlist_res.status == 0)
                        {
                            if (type == UIClubEvent.PassType.OPEN) {

                                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_FundGive, new object[3] { fund, _clubId, clubmlist_res }, () => { });
                            }
                            else
                            {
                                Game.Scene.GetComponent<UIComponent>().Get(UIType.UIClub_FundGive).GetComponent<UIClub_FundGiveComponent>().RefreshView(fund, _clubId ,clubmlist_res);
                            }
                        }
                        else
                        {
                            Log.Debug($"Error status = {clubmlist_res.status} msg = {clubmlist_res.msg}");
                            UIComponent.Instance.Toast(clubmlist_res.status);
                        }
                    });
        }
    }
    #endregion

    #region 对赌基金信息
    //刷新基金信息
    [Event(UIClubEventIdType.CLUB_GAMEFUNDINFO)]
    public class UIClubEvent_GameFundInfo : AEvent<int>
    {
        public override void Run(int _iClubId)
        {
            Log.Debug($"俱乐部{_iClubId}的对赌基金详情信息");
            WEB2_club_fund_detail.RequestData funddetail_req = new WEB2_club_fund_detail.RequestData()
            {
                clubId = _iClubId,
            };
            HttpRequestComponent.Instance.Send(
                    WEB2_club_fund_detail.API,
                    WEB2_club_fund_detail.Request(funddetail_req),
                    async (resData) => {

                        WEB2_club_fund_detail.ResponseData funddetail_res = WEB2_club_fund_detail.Response(resData);
                        if (funddetail_res.status == 0)
                        {

                            UI u = UIComponent.Instance.Get(UIType.UIClub_GameFundDetail);
                            if (u == null)
                            {
                                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_GameFundDetail, new object[2] { _iClubId, funddetail_res }, () => { });
                            }
                            else
                            {
                                //已经打开了就刷新信息
                                u.GetComponent<UIClub_GameFundDetailComponent>().OnShow(new object[2] { _iClubId, funddetail_res });
                            }
                        }
                        else
                        {
                            Log.Debug($"Error status = {funddetail_res.status} msg = {funddetail_res.msg}");
                            UIComponent.Instance.Toast(funddetail_res.status);
                        }
                    });
        }
    }

    //刷新基金交易列表信息
    [Event(UIClubEventIdType.CLUB_GAMEFUNDINFO_HISTORY)]
    public class UIClubEvent_GameFundInfoRefresh : AEvent<int>
    {
        public override void Run(int _clubId)
        {
            UI u = UIComponent.Instance.Get(UIType.UIClub_GameFundDetail);
            if (u != null)
            {
                u.GetComponent<UIClub_GameFundDetailComponent>().GetDetailInfo(_clubId, 0, 1);
            }
        }
    }

    #endregion

    [Event(UIClubEventIdType.CLUB_MLIST)]
    public class UIClubEvent_Mlist : AEvent<UIClubEvent.PassType, int>
    {
        public override void Run(UIClubEvent.PassType type, int _clubId)
        {
            WEB2_club_mlist.RequestData clubmlist_req = new WEB2_club_mlist.RequestData()
            {
                clubId = _clubId
            };
            HttpRequestComponent.Instance.Send(
                    WEB2_club_mlist.API,
                    WEB2_club_mlist.Request(clubmlist_req),
                    async (resData) => {

                        Log.Debug(resData);
                        WEB2_club_mlist.ResponseData clubmlist_res = WEB2_club_mlist.Response(resData);
                        if (clubmlist_res.status == 0)
                        {
                            if (type == UIClubEvent.PassType.OPEN)
                            {

                                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_Mlist, new object[2] {_clubId, clubmlist_res }, () => { });
                            }
                            else
                            {
                                Game.Scene.GetComponent<UIComponent>().Get(UIType.UIClub_FundGive).GetComponent<UIClub_MlistComponent>().RefreshView(_clubId, clubmlist_res);
                            }
                        }
                        else
                        {
                            Log.Debug($"Error status = {clubmlist_res.status} msg = {clubmlist_res.msg}");
                            UIComponent.Instance.Toast(clubmlist_res.msg);
                        }
                    });
        }
    }

    #region 俱乐部成员信息

    /// <summary>
    /// //成员信息获取
    /// </summary>
    [Event(UIClubEventIdType.CLUB_MLISTINFO)]
    public class UIClubEvent_MlistInfo : AEvent<string>
    {

        string strUserId;
        public override void Run(string _userId)
        {
            strUserId = _userId;
            //请求用户信息
           WEB2_user_public_info.RequestData userinfo_req = new WEB2_user_public_info.RequestData()
            {
                randomId = strUserId,
                userId = 0 
            };
            HttpRequestComponent.Instance.Send(WEB2_user_public_info.API,WEB2_user_public_info.Request(userinfo_req), UserInfoCall);
        }

        //用户信息回调
        void UserInfoCall(string resData) {

            WEB2_user_public_info.ResponseData userinfo_res = WEB2_user_public_info.Response(resData);
            if (userinfo_res.status == 0)
            {
                //请求个人数据
                WEB2_data_stat_person.RequestData dataperson_req = new WEB2_data_stat_person.RequestData()
                {
                    breakRandomId = strUserId,
                    breakUserId = 0,
                    roomPath = -1
                };
                HttpRequestComponent.Instance.Send(WEB2_data_stat_person.API, WEB2_data_stat_person.Request(dataperson_req), async (res) => {

                    //个人数据回调
                    WEB2_data_stat_person.ResponseData dataperson_res = WEB2_data_stat_person.Response(res);
                    if (dataperson_res.status == 0)
                    {
                        await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_MlistInfo, new object[3] { strUserId, userinfo_res , dataperson_res }, () => { });
                    }
                    else
                    {
                        Log.Debug($"Error status = {dataperson_res.status} msg = {dataperson_res.msg}");
                        UIComponent.Instance.Toast(dataperson_res.status);
                    }
                });
            }
            else
            {
                Log.Debug($"Error status = {userinfo_res.status} msg = {userinfo_res.msg}");
                UIComponent.Instance.Toast(userinfo_res.status);
            }
        }
    }

    #endregion

    /// <summary>
    /// 组局事件  保险提示
    /// </summary>
    [Event(UIClubEventIdType.CLUB_ROOMCREAT_TIP)]
    public class UIClubEvent_RoomCreatTip : AEvent<int>
    {
        public override void Run(int type)
        {
            UI u = UIComponent.Instance.Get(UIType.UIClub_RoomCreat);
            if (u != null)
            {
                u.GetComponent<UIClub_RoomCreateComponent>().ShowTip(type);
            }
        }
    }

    /// <summary>
    /// 组局事件 血战提示
    /// </summary>
    [Event(UIClubEventIdType.CLUB_MLISTINFO_MARKER)]
    public class UIClubEvent_MlistInfoRemark: AEvent
    {
        public override void Run()
        {
            UI u = UIComponent.Instance.Get(UIType.UIClub_MlistInfo);
            if (u != null)
            {
                u.GetComponent<UIClub_MlistInfoComponent>().RefreshMark();
            }
        }
    }
}

