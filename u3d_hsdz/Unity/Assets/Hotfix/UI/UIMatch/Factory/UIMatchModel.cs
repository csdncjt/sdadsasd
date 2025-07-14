using System.Collections.Generic;
using BannerEle = ETHotfix.WEB_common_banner.DataElement;
using BlindDto = ETHotfix.WEB_room_blindData.GameBlindElement;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	//关于 api接口 往这边迁移
	public class UIMatchModel
    {
        private static UIMatchModel instance;
        public static UIMatchModel mInstance { get { if (instance == null) instance = new UIMatchModel(); return instance; } }

        public Dictionary<int, Dictionary<int, List<BlindDto>>> mBindDtoDict = new Dictionary<int, Dictionary<int, List<BlindDto>>>();

        public void Dispose()
        {

        }


        /// <summary>
        ///  根据游戏类型和房间类型获取所有小盲 roomType -1所有 0小 1中 2大
        /// </summary>
        public List<int> GetRoomBlindsByGameAndRoomType(int pRType, int roomType)
        {
            List<int> list = new List<int>();

            Dictionary<int, List<BlindDto>> dict;
            mBindDtoDict.TryGetValue(pRType, out dict);
            if (dict == null)
            {
                return list;
            }

            for (int i = 0; i < 3 ;i++)
            {
                List<BlindDto> dtolist;
                dict.TryGetValue(i, out dtolist);
                if (dtolist != null)
                {
                    for (int j = 0; j < dtolist.Count; j++)
                    {
                        if (dtolist[j].pattern == roomType || roomType == -1)
                        {
                            int minValue = int.Parse(dtolist[j].blindnote.Split('/')[0]);
                            list.Add(minValue);
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        ///  牌局类型:61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
        /// </summary>
        public string GetRoomPathStr(int pRType)
        {
            var tLangs = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIMatchModel_101");
            if (pRType == 61)
                return tLangs[0];//return "德州";
            else if (pRType == 91)
                return tLangs[1];//"奥马哈";
            else if (pRType == 51)
                return tLangs[2];//"大菠萝";
            else if (pRType == 41)
                return tLangs[3];//"必下场";
            else if (pRType == 31)
                return "AOF";
            else if (pRType == 81)
                return "SNG";
            else if (pRType == 71)
                return "MMT";
            else if (pRType == 21)
                return tLangs[4];
            return "";
        }

        /// <summary>
        ///  RoomPath:61 = 德州 62 = 德州-必下场 63 = 德州-AOF 91 = 奥马哈 92 = 奥马哈-必下场 93 = 奥马哈-AOF 51 = 大菠萝 81 = SNG 71 = MTT
        /// </summary>
        public string GetRealRoomPathStr(int roomPath)
        {
            var tLangs = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIMatchModel_101");
            if (roomPath == 61)
                return tLangs[0];//return "德州";
            else if (roomPath == 91)
                return tLangs[1];//"奥马哈";
            else if (roomPath == 51)
                return tLangs[2];//"大菠萝";
            else if (roomPath == 62 || roomPath == 92)
                return tLangs[3];//"必下场";
            else if (roomPath == 63 || roomPath == 93)
                return "AOF";
            else if (roomPath == 81)
                return "SNG";
            else if (roomPath == 71)
                return "MMT";
            else if (roomPath == 21)
                return tLangs[4];
            return "";
        }


        /// <summary>
        /// 昵称长度的显示  返回值(曾小六SunSi的天空...)      pContainerWidth不设置 则是Text的宽度
        /// </summary>
        public void SetLargeTextContent(Text pText, string pStr, float pContainerWidth = 0,float lineHeight=1)
        {
            if (string.IsNullOrEmpty(pStr)) return;
            if (pText == null) return;

            if (pContainerWidth == 0)
                pContainerWidth = pText.GetComponent<RectTransform>().rect.width;

            var font = pText.font;
            font.RequestCharactersInTexture(pStr, pText.fontSize, pText.fontStyle);
            CharacterInfo characterInfo;
            float width = 0f;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < pStr.Length; i++)
            {
                font.GetCharacterInfo(pStr[i], out characterInfo, pText.fontSize);
                width += characterInfo.advance;
                if (width < pContainerWidth*lineHeight)
                {
                    sb.Append(pStr[i]);
                }
            }

            if (pContainerWidth > width)//本来能容纳
                pText.text = sb.ToString();
            else
                pText.text = sb.Remove(sb.Length - 1, 1).Append("...").ToString();
        }


        #region Banner
        /// <summary>
        ///   0：大厅Banner，1：发现页Banner，2：发现页主图，3：游戏公平，4：游戏商城主图
        /// </summary>
        /// <param name="pLang">zh_CN：简体中文，zh_TW：繁体中文，en_US：英文</param>
        /// <param name="pType">0：大厅Banner，1：发现页Banner，2：发现页主图，3：游戏公平，4：游戏商城主图</param>
        /// <param name="pProduct">上架产品，0：疯狂扑克</param>
        public void APIGetBanner(int pType, int pProduct, Action<List<BannerEle>> pAct)
        {
            var tLangs = new string[] { "zh_CN", "en_US", "zh_HK" };

            var tReq = new WEB_common_banner.RequestData()
            {
                lang = tLangs[LanguageManager.mInstance.mCurLanguage],
                type = pType,
                product = pProduct
            };
            HttpRequestComponent.Instance.Send(WEB_common_banner.API, WEB_common_banner.Request(tReq), resData =>
                    {
                        var tDto = WEB_common_banner.Response(resData);
                        if (tDto.status == 0)
                        {
                            if (tDto.data != null)//&& tDto.data.Count > 0)
                                pAct(tDto.data);
                        }
                        else
                        {
                            UIComponent.Instance.Toast(tDto.status);
                        }
                    });
        }
        #endregion

        #region Blind 
        //获取盲注详情
        public void APIGetBlinds(Action<WEB_room_blindData.ResponseData> pAct)
        {
            var tReq = new WEB_room_blindData.RequestData() { };
            HttpRequestComponent.Instance.Send(WEB_room_blindData.API, WEB_room_blindData.Request(tReq), resData =>
            {
                var tResp = WEB_room_blindData.Response(resData);
                if (pAct != null)
                {
                    pAct(tResp);

                }
            });
        }
        #endregion

        /// <summary>         大厅 列表(tab 大厅1  德州3 奥马哈4)         </summary>
        public void APIHallRoomListFilter(int pDir, int listSize, Action<  WEB2_room_list.ResponseData> pAct, List<int> pFilter, List<int> pFilter2, Action pActFinishedError)
        {
            int pRoomPath = pFilter[0],
                pBlind = pFilter[1],
                pPeople = pFilter[2],
                pFirst = pFilter[3],
                pInsurance = pFilter[4],
                pSeat = pFilter[5];

            if (pRoomPath == -1)
            {
                pRoomPath = 61;
            }

            var web = new WEB2_room_list.RequestData()
            {
                act = pDir,
                rtype = Convert.ToInt32(pRoomPath),//61 = 德州   91 = 奥马哈     51 = 大菠萝      41 = 必下场  31 = AOF   81 = SNG     71 = MTT
                mz = pBlind,// 盲注级别,不需此条件，传值：-1 <br/>5 = 5/10以下<br/>10 = 10/20 ~ 50/100<br/>100 = 100/200以上
                seat = pSeat,
                insurance = pInsurance,
                lrt = 0,
                rpu = pPeople,
                qzhu = pFirst,
                mzlist = pFilter2,
                listSize = listSize

            };
            HttpRequestComponent.Instance.Send(WEB2_room_list.API, WEB2_room_list.Request(web), (Action<string>)(resData =>
            {
                var tResp = WEB2_room_list.Response(resData);
                if (pAct != null)
                {
                    pAct(tResp);
                }
            }), null, pActFinishedError);
        }
        /// <summary>        /// 我的牌局 无多页请求        /// </summary>
        public void APIMyRoomList(Action<WEB2_room_mylist.ResponseData> pAct)
        {
            var paramasRoom = new WEB2_room_mylist.RequestData() { };
            HttpRequestComponent.Instance.Send(WEB2_room_mylist.API, WEB2_room_mylist.Request(paramasRoom), resData =>
            {
                var tResp = WEB2_room_mylist.Response(resData);
                if (pAct != null)
                {
                    pAct(tResp);
                }
            });
        }

        /// <summary>         MTT官方赛列表         </summary>
        public void APIMTTRoomList(int matchType, int pageNum, int pageSize, Action<WEB2_room_mtt_list.ResponseData> pAct)
        {
            var requestData = new WEB2_room_mtt_list.RequestData()
            {
                matchType = matchType,
                pageNum = pageNum,
                pageSize = pageSize
            };
            HttpRequestComponent.Instance.Send(
                WEB2_room_mtt_list.API,
                WEB2_room_mtt_list.Request(requestData), str =>
                {
                    var tDto = WEB2_room_mtt_list.Response(str);
                    if (pAct != null)
                    {
                        pAct(tDto);
                    }
                });
        }

        /// <summary>         MTT奖励列表         </summary>
        public void APIMTTRewardList(int matchID, int pageNum, Action<WEB2_room_mtt_reward_info.ResponseData> pAct)
        {
            var requestData = new WEB2_room_mtt_reward_info.RequestData()
            {
                page = pageNum,
                rmid = matchID,
                rpath = (int)RoomPath.MTT
            };
            HttpRequestComponent.Instance.Send(
                WEB2_room_mtt_reward_info.API,
                WEB2_room_mtt_reward_info.Request(requestData), str =>
                {
                    var tDto = WEB2_room_mtt_reward_info.Response(str);
                    if (pAct != null)
                    {
                        pAct(tDto);
                    }
                });
        }

        /// <summary>         MTT牌桌列表         </summary>
        public void APIMTTDeskList(int matchID, int pageNum, Action<WEB2_room_mtt_desk_info.ResponseData> pAct)
        {
            var requestData = new WEB2_room_mtt_desk_info.RequestData()
            {
                page = pageNum,
                rmid = matchID,
                rpath = (int)RoomPath.MTT
            };
            HttpRequestComponent.Instance.Send(
                WEB2_room_mtt_desk_info.API,
                WEB2_room_mtt_desk_info.Request(requestData), str =>
                {
                    var tDto = WEB2_room_mtt_desk_info.Response(str);
                    if (pAct != null)
                    {
                        pAct(tDto);
                    }
                });
        }


        /// <summary>        /// 备注列表(玩家给玩家B起别名之类的)        /// </summary>
        public void APIGetBatchRelationSNS()
        {
            var web = new WEB2_sns_batch_relations.RequestData();
            HttpRequestComponent.Instance.Send(WEB2_sns_batch_relations.API,
                WEB2_sns_batch_relations.Request(web), json =>
                {
                    var tDto = WEB2_sns_batch_relations.Response(json);
                    if (tDto.status == 0)
                    {
                        CacheDataManager.mInstance.SetSNSBatchRelations(tDto.data);
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
        }
        public void APIEnterRoom(int pRoomPath, int pRoomId, Action<WEB2_room_enter.Data> pAct, Action pActFinishedError = null)
        {
            HttpRequestComponent.Instance.Send(WEB2_room_enter.API,
                                                   WEB2_room_enter.Request(new WEB2_room_enter.RequestData()
                                                   {
                                                       rpath = pRoomPath,//这个数据....
                                                       rmid = pRoomId
                                                   }), json =>
                                                   {
                                                       var tDto = WEB2_room_enter.Response(json);
                                                       if (tDto.status == 0)
                                                       {
                                                           if (pAct != null)
                                                           {
                                                               pAct(tDto.data);
                                                           }
                                                       }
                                                       else
                                                       {
                                                           UIComponent.Instance.Toast(tDto.msg);
                                                           if (pActFinishedError != null)
                                                           {
                                                               pActFinishedError();
                                                           }
                                                       }
                                                   }, null, pActFinishedError);
        }
        /// <summary>
        /// 进入普通局
        /// </summary>
        public void APIEnterNormalRoom(int pRoomPath, int pRoomId, Action<WEB2_room_dz_enter.Data> pAct, Action pActFinishedError = null)
        {
            HttpRequestComponent.Instance.Send(WEB2_room_dz_enter.API,
                                                   WEB2_room_dz_enter.Request(new WEB2_room_dz_enter.RequestData()
                                                   {
                                                       rpath = pRoomPath,//这个数据....
                                                       rmid = pRoomId
                                                   }), json =>
                                                   {
                                                       var tDto = WEB2_room_dz_enter.Response(json);
                                                       if (tDto.status == 0)
                                                       {
                                                           if (pAct != null)
                                                           {
                                                               pAct(tDto.data);
                                                           }
                                                       }
                                                       else
                                                       {
                                                           UIComponent.Instance.Toast(tDto.status);
                                                           if (pActFinishedError != null)
                                                           {
                                                               pActFinishedError();
                                                           }
                                                       }
                                                   }, null, pActFinishedError);
        }
        /// <summary>
        /// 进入短牌
        /// </summary>
        public void APIEnterShortRoom(int pRoomPath, int pRoomId, Action<WEB2_room_dp_enter.Data> pAct, Action pActFinishedError = null)
        {
            HttpRequestComponent.Instance.Send(WEB2_room_dp_enter.API,
                                                   WEB2_room_dp_enter.Request(new WEB2_room_dp_enter.RequestData()
                                                   {
                                                       rpath = pRoomPath,//这个数据....
                                                       rmid = pRoomId
                                                   }), json =>
                                                   {
                                                       var tDto = WEB2_room_dp_enter.Response(json);
                                                       if (tDto.status == 0)
                                                       {
                                                           if (pAct != null)
                                                           {
                                                               pAct(tDto.data);
                                                           }
                                                       }
                                                       else
                                                       {
                                                           UIComponent.Instance.Toast(tDto.status);
                                                           if (pActFinishedError != null)
                                                           {
                                                               pActFinishedError();
                                                           }
                                                       }
                                                   }, null, pActFinishedError);
        }


        /// <summary>
        /// 进入奥马哈
        /// </summary>
        public void APIEnterOmahaRoom(int pRoomPath, int pRoomId, Action<WEB2_room_omaha_enter.Data> pAct, Action pActFinishedError = null)
        {
            HttpRequestComponent.Instance.Send(WEB2_room_omaha_enter.API,
                                                   WEB2_room_omaha_enter.Request(new WEB2_room_omaha_enter.RequestData()
                                                   {
                                                       rpath = pRoomPath,
                                                       rmid = pRoomId
                                                   }), json =>
                                                   {
                                                       var tDto = WEB2_room_omaha_enter.Response(json);
                                                       if (tDto.status == 0)
                                                       {
                                                           if (pAct != null)
                                                           {
                                                               pAct(tDto.data);
                                                           }
                                                       }
                                                       else
                                                       {
                                                           UIComponent.Instance.Toast(tDto.status);
                                                           if (pActFinishedError != null)
                                                           {
                                                               pActFinishedError();
                                                           }
                                                       }
                                                   }, null, pActFinishedError);
        }

        /// <summary>
        /// 进入MTT
        /// </summary>
        public void APIEnterMTTRoom(int pRoomPath, int pRoomId, Action<WEB2_room_mtt_enter.Data> pAct, Action pActFinishedError = null)
        {
            HttpRequestComponent.Instance.Send(WEB2_room_mtt_enter.API,
                                                   WEB2_room_mtt_enter.Request(new WEB2_room_mtt_enter.RequestData()
                                                   {
                                                       rpath = pRoomPath,
                                                       rmid = pRoomId,
                                                   }), json =>
                                                   {
                                                       var tDto = WEB2_room_mtt_enter.Response(json);
                                                       if (tDto.status == 0)
                                                       {
                                                           if (pAct != null)
                                                           {
                                                               pAct(tDto.data);
                                                           }
                                                       }
                                                       else
                                                       {
                                                           UIComponent.Instance.Toast(tDto.status);
                                                           if (pActFinishedError != null)
                                                           {
                                                               pActFinishedError();
                                                           }
                                                       }
                                                   }, null, pActFinishedError);
        }

        /// <summary>
        /// 注册分享赚USDT 弹框
        /// </summary>
        public void ShowShareGoldBean(string count)
        {
            var tLangs = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIMatch_ShareBeans100");
            if (tLangs == null || tLangs.Length == 0) return;
            UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
            {
                type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                title = tLangs[0],
                content = string.Format(tLangs[1], count),
                contentCommit = tLangs[2],
                contentCancel = tLangs[3],
                actionCommit = () =>
                {
                    UIComponent.Instance.Toast("跳转哪呢");
                    //UIComponent.Instance.ShowNoAnimation(UIType.UIMine_UserInfoSetting);
                },
                actionCancel = () =>
                {
                }
            });
        }


        /// <summary>         是否 要弹出 修改 昵称的弹框         </summary>
        public void CheckOpenNickName()
        {
            UIMineModel.mInstance.CheckOpenDialog(delegate
            {
                var tLangs = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIMatch_Script101");
                if (tLangs == null || tLangs.Length == 0) return;
                UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                {
                    type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                    // title = $"温馨提示",content = $"首次修改头像及昵称赠送300USDT",contentCommit = "前往",contentCancel = "取消",
                    title = tLangs[0],
                    content = tLangs[1],
                    contentCommit = tLangs[2],
                    contentCancel = tLangs[3],
                    actionCommit = () =>
                    {
                        UIComponent.Instance.ShowNoAnimation(UIType.UIMine_UserInfoSetting);
                    },
                    actionCancel = () =>
                    {
                    }
                });
            });
        }

        /// <summary>
        ///   摇摇乐 获取奖励
        ///  </summary>
        public void APIActivityKDouGain()
        {
            HttpRequestComponent.Instance.Send(WEB2_activity_kdouGain.API,
                                                 WEB2_activity_kdouGain.Request(new WEB2_activity_kdouGain.RequestData()), json =>
                                                 {
                                                     var tDto = WEB2_activity_kdouGain.Response(json);
                                                     if (tDto.status == 0)
                                                     {
                                                         UIComponent.Instance.Toast(LanguageManager.Get("UILobby_Activity_success"));
                                                     }
                                                     else
                                                     {
                                                         UIComponent.Instance.Toast(tDto.status);
                                                     }
                                                 });
            //45cfdf7e8f5d9281f2105c526bc332f5
        }

        /// <summary>
        /// 摇摇乐 参加活动
        ///  </summary>
        public void APIActivityKdouApply(int gps)
        {
            var str = $"{GameCache.Instance.nUserId}86FA75CC4332B3B94DB81C4B96FE8F64";
            str = str.Substring(0, 16);
            HttpRequestComponent.Instance.Send(WEB2_activity_kdouApply.API,
                WEB2_activity_kdouApply.Request(new WEB2_activity_kdouApply.RequestData()
                {
                    i = GameUtil.AESEncrypt(SystemInfoUtil.getDeviceUniqueIdentifier(), str),
                    g = GameUtil.AESEncrypt(gps.ToString(), str),
                    m = GameUtil.AESEncrypt(SystemInfoUtil.getDeviceMode(), str),
                    s = GameUtil.AESEncrypt(SystemInfoUtil.isSimulator.ToString(), str)                  
                }), json =>
                {
                    var tDto = WEB2_activity_kdouApply.Response(json);
                    if (tDto.status == 0)
                    {

                    }
                    else
                    {
                    }
                });
        }

        /// <summary>
        /// 参加首充活动
        /// </summary>
        /// <param name="gps"></param>
        public void APIActivityRechargeApply(int gps)
        {
            var str = $"{GameCache.Instance.nUserId}86FA75CC4332B3B94DB81C4B96FE8F64";
            str = str.Substring(0, 16);
            HttpRequestComponent.Instance.Send(WEB2_activity_recharge_apply.API,
                                               WEB2_activity_recharge_apply.Request(new WEB2_activity_recharge_apply.RequestData()
                                               {
                                                       i = GameUtil.AESEncrypt(SystemInfoUtil.getDeviceUniqueIdentifier(), str),
                                                       g = GameUtil.AESEncrypt(gps.ToString(), str),
                                                       m = GameUtil.AESEncrypt(SystemInfoUtil.getDeviceMode(), str),
                                                       s = GameUtil.AESEncrypt(SystemInfoUtil.isSimulator.ToString(), str)
                                               }), json =>
                                               {
                                                   var tDto = WEB2_activity_recharge_apply.Response(json);
                                                   if (tDto.status == 0)
                                                   {
                                                       
                                                   }
                                                   else
                                                   {

                                                   }
                                               });
        }

        /// <summary>
        /// 首充活动获取奖励
        /// </summary>
        public void APIActivityRechargeGain()
        {
            HttpRequestComponent.Instance.Send(WEB2_activity_recharge_gain.API,
                                               WEB2_activity_recharge_gain.Request(new WEB2_activity_recharge_gain.RequestData()), json =>
                                               {
                                                   var tDto = WEB2_activity_recharge_gain.Response(json);
                                                   if (tDto.status == 0)
                                                   {
                                                       GameCache.Instance.isActivity = 0;
                                                       // UIComponent.Instance.Toast(LanguageManager.Get("UILobby_Activity_success"));
                                                       // APIActivityRechargeQurey();
                                                   }
                                                   else
                                                   {
                                                       // UIComponent.Instance.Toast(tDto.status);
                                                   }
                                               });
        }

        public void APIActivityRechargeQurey()
        {
            HttpRequestComponent.Instance.Send(WEB2_activity_recharge_query.API,
                                               WEB2_activity_recharge_query.Request(new WEB2_activity_recharge_query.RequestData()), json =>
                                               {
                                                   var tDto = WEB2_activity_recharge_query.Response(json);
                                                   if (tDto.status == 0)
                                                   {
                                                       if (tDto.data.num > 0)
                                                       {
                                                           GameCache.Instance.isActivity = 3;
                                                           UIComponent.Instance.ShowNoAnimation(UIType.UIFirstRechargeActivitySuccess, tDto.data.num);
                                                       }
                                                       else if (tDto.data.num == -2)
                                                       {
                                                           GameCache.Instance.isActivity = 2;
                                                       }

                                                       // UIComponent.Instance.Toast(LanguageManager.Get("UILobby_Activity_success"));
                                                   }
                                                   else
                                                   {
                                                       // UIComponent.Instance.Toast(tDto.status);
                                                   }
                                               });
        }

		/// <summary>         mtt比赛详情         </summary>
		public void APIMTTView(int matchID, Action<WEB2_room_mtt_view.ResponseData> pAct)
		{
			var requestData = new WEB2_room_mtt_view.RequestData()
			{
				rmid = matchID,
				rpath = (int)RoomPath.MTT
			};
			HttpRequestComponent.Instance.Send(
				WEB2_room_mtt_view.API,
				WEB2_room_mtt_view.Request(requestData), str =>
				{
					var tDto = WEB2_room_mtt_view.Response(str);
					if (tDto.status == 0)
					{
						if (pAct != null)
						{
							pAct(tDto);
						}
					}
					else
					{
						UIComponent.Instance.Toast(tDto.status);
					}

				});
		}
        
        /// <summary>         自动开房列表         </summary>
        public void APIPlanRoomList(Action<WEB2_roomplan_list.ResponseData> pAct)
        {
            var web = new WEB2_roomplan_list.RequestData()
            {

            };
            HttpRequestComponent.Instance.Send(WEB2_roomplan_list.API, WEB2_roomplan_list.Request(web), (Action<string>)(resData =>
            {
                var tResp = WEB2_roomplan_list.Response(resData);
                if (tResp.status == 0)
                {
                    if (pAct != null)
                    {
                        pAct(tResp);
                    }
                }
                else
                {
                    UIComponent.Instance.Toast(tResp.status);
                }
                
            }), null, null);
        }

        public void APIClubRoomList(Action<WEB2_room_clublist.ResponseData> pAct)
        {
            var web = new WEB2_room_clublist.RequestData()
            {
            };
            HttpRequestComponent.Instance.Send(WEB2_room_clublist.API, WEB2_room_clublist.Request(web), (Action<string>)(resData =>
            {
                var tResp = WEB2_room_clublist.Response(resData);
                if (tResp.status == 0)
                {
                    if (pAct != null)
                    {
                        pAct(tResp);
                    }
                }
                else
                {
                    UIComponent.Instance.Toast(tResp.status);
                }

            }), null, null);
        }

        /// <summary>         自动开房列表         </summary>
        public void APIRoomPlanOperate(int planID, int status, Action<WEB2_roomplan_operate.ResponseData> pAct)
        {
            var web = new WEB2_roomplan_operate.RequestData()
            {
                id = planID,
                status = status
            };
            HttpRequestComponent.Instance.Send(WEB2_roomplan_operate.API, WEB2_roomplan_operate.Request(web), (Action<string>)(resData =>
            {
                var tResp = WEB2_roomplan_operate.Response(resData);
                if (tResp.status == 0)
                {
                    if (pAct != null)
                    {
                        pAct(tResp);
                    }
                }
                else
                {
                    UIComponent.Instance.Toast(tResp.status);
                }

            }));
        }

    }
}
