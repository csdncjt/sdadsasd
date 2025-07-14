using System;
using System.Collections.Generic;
using MsgLastEle = ETHotfix.WEB2_message_nums_detail.DataElement;
using DtoResponse = ETHotfix.WEB2_message_list.ResponseData;
using DtoSelfWallet = ETHotfix.WEB2_user_self_info.WalletElement;
using System.Text;
using System.Text.RegularExpressions;
using RecordDetailDto = ETHotfix.WEB2_record_detail.ResponseData;
using RecordMttDetailDto = ETHotfix.WEB2_record_mtt_detail.ResponseData;

namespace ETHotfix
{
    //关于 api接口 往这边迁移
    public class UIMineModel
    {
        private static UIMineModel _instance;
        public static UIMineModel mInstance
        {
            get
            {
                if (_instance == null)
                    _instance = new UIMineModel();
                return _instance;
            }
        }

        public void Dispose()
        {
            IsPromotion = false;
            mIsRedPointShow = false;
            mBindingCardAcc = null;
            mBindingAlipayAcc = string.Empty;
            mBindingAlipayName = string.Empty;
            mIsSecondPW = false;
            mMyClubId = -1;
            mUserType = 0;
        }
        /// <summary>
        /// 后缀语言
        /// </summary>
        private string[] mUrl_Suffix = new string[] { "?lan=cn", "?lan=en", "?lan=ft" };

        /// <summary>
        /// 得到 后缀语言  ?lan=cn   ?lan=en    ?lan=ft
        /// </summary> 
        public string GetUrlSuffix()
        {
            return mUrl_Suffix[LanguageManager.mInstance.mCurLanguage];
        }
        private string mCacheProblemUrl = string.Empty;
        public void SetNullProblemUrl()
        {
            mCacheProblemUrl = string.Empty;
        }

        public void GetShareProblemUrl(Action<string> pUrl)
        {
            if (string.IsNullOrEmpty(mCacheProblemUrl) == false)
            {
                pUrl(mCacheProblemUrl);
            }
            else
            {
                APIPromotionIsOpen(pDto =>
                {
                    if (pDto.status == 0)
                    {
                        if (pDto.data != null && pDto.data.Count > 0)
                        {
                            for (int i = 0; i < pDto.data.Count; i++)
                            {
                                if (pDto.data[i].functionCode == 6)
                                {
                                    var tUrl = pDto.data[i].value + UIMineModel.mInstance.GetUrlSuffix();
                                    mCacheProblemUrl = tUrl;
                                    if (pUrl != null)
                                    {
                                        pUrl(tUrl);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        UIComponent.Instance.Toast(pDto.status);
                    }
                });
            }
        }


        public bool IsMatch(string pattern, string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            return Regex.IsMatch(str, pattern);

        }
        //密码规则大小写数字!@#* 任意两种或以上组合
        public bool IsPasdLetter(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            int matchNum = 0;

            if (Regex.Matches(str, "[a-z]").Count > 0)
            {
                matchNum++;
            }

            if (Regex.Matches(str, "[A-Z]").Count > 0)
            {
                matchNum++;
            }

            if (Regex.Matches(str, "[0-9]").Count > 0)
            {
                matchNum++;
            }

            if (Regex.Matches(str, "[!@#*]").Count > 0)
            {
                matchNum++;
            }


            var patt = "^[a-zA-Z0-9!@#*]+$";
            //var patt = "^[0-9]+$";
            var tNumLetter = IsMatch(patt, str);
            if (tNumLetter && matchNum > 1)
            {
                tNumLetter = true;
            }
            else
            {
                tNumLetter = false;
            }

            return tNumLetter;
        }

        /// <summary>
        /// 是否 只有字母,数字   或组合
        /// </summary>
        public bool IsNumLetter(string str)
        {
            var patt = "^[a-zA-Z0-9]+$";
            //var patt = "^[0-9]+$";
            var tNumLetter = IsMatch(patt, str);
            return tNumLetter;
        }

        /// <summary>
        /// 是否 邮箱 
        /// </summary>
        public bool IsEmail(string str)
        {
            var pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            return IsMatch(pattern, str);
        }

        /// <summary>
        /// 有标点或空格时 true
        /// </summary>
        public bool IsTextShow(string str)
        {
            var pattern = @"\p{P}|\s";
            return !IsMatch(pattern, str);
        }

        /// <summary>
        /// pStr传入的,pShow前后各显示多少位
        /// </summary>
        public string HideStarString(string pStr, int pShow)
        {
            if (string.IsNullOrEmpty(pStr) || pStr.Length <= pShow * 2)//不够位数,直接显示
                return pStr;
            StringBuilder hideStr = new StringBuilder();
            var hideCount = pStr.Length - pShow * 2;
            for (int i = 0; i < pStr.Length; i++)
            {
                if (i >= pShow && i < (pShow + hideCount))
                    hideStr.Append("*");
                else
                    hideStr.Append(pStr.Substring(i, 1));
            }
            return hideStr.ToString();
        }

        public string GetMobileStarHide()
        {
            var tStr = HideStarString("+"+GameCache.Instance.strPhoneFirst+GameCache.Instance.strPhone, 6);
            return tStr;
        }

        /// <summary> 毫秒         多少小时分钟局</summary>
        public string ShowGameCountTime(long pNum)
        {
            var tStrLanguages = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIMineModel101");
            string tStr = "";
            pNum = pNum / 1000;
            var tHour = pNum / 3600;
            var tMinutes = pNum % 3600 / 60;
            if (tHour > 0 && tMinutes == 0)
                tStr = tHour.ToString() + tStrLanguages[0];// "小时局";
            else if (tHour > 0 && tMinutes != 0)
                tStr = tHour.ToString() + tStrLanguages[1] + tMinutes.ToString() + tStrLanguages[2];   // tStr = tHour.ToString() + "小时" + tMinutes.ToString() + "分钟局";
            else
                tStr = tMinutes.ToString() + tStrLanguages[2];//"分钟局";
            return tStr;
        }

        #region ObtainUserInfo 自已的个人信息
        public void ObtainUserInfo(Action<WEB2_user_self_info.Data> pAct)
        {
            WEB2_user_self_info.RequestData requestData = new WEB2_user_self_info.RequestData()
            {
            };
            HttpRequestComponent.Instance.Send(WEB2_user_self_info.API,
                WEB2_user_self_info.Request(requestData), json =>
                {
                    var tDto = WEB2_user_self_info.Response(json);
                    if (tDto.status == 0)
                    {
                        mIsSecondPW = true;
                        walletMainDto = tDto.data.wallet;
                        modifyHeadTime = tDto.data.modifyHeadTime;
                        GameCache.Instance.modifyNickNum = tDto.data.modifyNickNum;
                        GameCache.Instance.gold = tDto.data.chip;
                        if (pAct != null)
                            pAct(tDto.data);
                        mIsSecondPW = tDto.data.payPwdStatus == 1;
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
        }
        #endregion

        Dictionary<string, string> mDicCardName = new Dictionary<string, string>();
        public Dictionary<string, string> GetCardNameDics()
        {
            if (mDicCardName == null || mDicCardName.Count == 0)
            {
                var tCardNames = LanguageManager.mInstance.GetLanguageForKeyMoreValues("CardNames");
                for (int i = 0; i < tCardNames.Length; i++)
                {
                    var tLangs = tCardNames[i].Split(',');
                    mDicCardName[tLangs[0]] = tLangs[1];
                }
            }
            return mDicCardName;
        }
        public void ClearCardName()
        {
            mDicCardName.Clear();
            mDiTokenName.Clear();
        }

        Dictionary<string, string> mDiTokenName = new Dictionary<string, string>();

        public Dictionary<string, string> GetTokenNameDics()
        {
            if (mDiTokenName == null || mDiTokenName.Count == 0)
            {
                var tCardNames = LanguageManager.mInstance.GetLanguageForKeyMoreValues("TokenNames");
                for (int i = 0; i < tCardNames.Length; i++)
                {
                    var tLangs = tCardNames[i].Split(',');
                    mDiTokenName[tLangs[0]] = tLangs[1].Trim();
                }
            }
            return mDiTokenName;
        }

        public string GetTokenName(string code)
        {
            string name = "";
            if (mDiTokenName == null || mDiTokenName.Count == 0)
            {
                var tCardNames = LanguageManager.mInstance.GetLanguageForKeyMoreValues("TokenNames");
                for (int i = 0; i < tCardNames.Length; i++)
                {
                    var tLangs = tCardNames[i].Split(',');
                    mDiTokenName[tLangs[0]] = tLangs[1].Trim();
                }
            }

            if (mDiTokenName.TryGetValue(code, out name))
            {
                return name;
            }

            return "";
        }

        public void ClearTokenName()
        {
            mDiTokenName.Clear();
        }


        #region 分享赚USDT
        /// <summary>         是否开启         functionCode//功能代号。1：分享赚USDT，2:钱包， 3：商城， 4: 备份下载链接   5:游戏公平链接  6常见问题页面链接  7转USDT开关 </summary>
        public void APIPromotionIsOpen(Action<WEB2_query_function_open.ResponseData> pAct)
        {
            var web = new WEB2_query_function_open.RequestData();
            HttpRequestComponent.Instance.Send(
                 WEB2_query_function_open.API,
                 WEB2_query_function_open.Request(web), json =>
                 {
                     var tDto = WEB2_query_function_open.Response(json);
                     if (tDto.status == 0)
                     {
                         if (pAct != null)
                             pAct(tDto);
                     }
                     else
                     {
                         UIComponent.Instance.Toast(tDto.status);
                     }
                 });
        }

        public bool mTransferOpen = false;
        /// <summary>         转USDT开关 true开了,false关闭了         </summary>
        public void GetTransferOpen(Action<bool> pAct)
        {
            APIPromotionIsOpen(tDto =>
            {
                for (int i = 0; i < tDto.data.Count; i++)
                {
                    if (tDto.data[i].functionCode == 7)
                    {
                        mTransferOpen = tDto.data[i].open;
                        if (pAct != null)
                            pAct(mTransferOpen);
                    }
                }
            });
        }

        /// <summary>         是否推广员       </summary>
        public bool IsPromotion = false;
        /// <summary>         是否推广员       </summary>
        public void APIIsPromotion(Action<WEB2_promotion_is_promoter.ResponseData> pAct)
        {
            var web = new WEB2_promotion_is_promoter.RequestData();
            HttpRequestComponent.Instance.Send(
                 WEB2_promotion_is_promoter.API,
                 WEB2_promotion_is_promoter.Request(web), json =>
                 {
                     var tDto = WEB2_promotion_is_promoter.Response(json);
                     if (pAct != null)
                         pAct(tDto);
                     if (tDto.status == 0)
                         IsPromotion = tDto.data;
                 });
        }

        /// <summary>         查询业绩         </summary>
        public void APIQueryAchievements(Action<WEB2_promotion_query_achievements.Data> pAct, int pSize = 15, int pStart = 0)
        {
            var web = new WEB2_promotion_query_achievements.RequestData()
            {
                size = pSize,
                start = pStart
            };
            HttpRequestComponent.Instance.Send(
                 WEB2_promotion_query_achievements.API,
                 WEB2_promotion_query_achievements.Request(web), json =>
                 {
                     var tDto = WEB2_promotion_query_achievements.Response(json);
                     if (tDto.status == 0)
                     {
                         if (pAct != null)
                             pAct(tDto.data);
                     }
                     else
                     {
                         UIComponent.Instance.Toast(tDto.status);
                     }
                 });
        }

        public void APIQueryMyPromoterSizePage(string pSearch, int pSize, int pStart, Action<WEB2_promotion_query_my_promoters.ResponseData> pAct)
        {
            var web = new WEB2_promotion_query_my_promoters.RequestData()
            {
                search = pSearch,
                size = pSize,
                start = pStart
            };
            HttpRequestComponent.Instance.Send(
                 WEB2_promotion_query_my_promoters.API,
                 WEB2_promotion_query_my_promoters.Request(web), json =>
                 {
                     var tDto = WEB2_promotion_query_my_promoters.Response(json);
                     if (tDto.status == 0)
                     {
                         if (pAct != null)
                             pAct(tDto);
                     }
                     else
                     {
                         UIComponent.Instance.Toast(tDto.status);
                     }
                 });
        }

        /// <summary>         成为推广员         </summary>
        public void APIToBePromoter(Action<WEB2_promotion_to_be_promoter.ResponseData> pAct)
        {
            var web = new WEB2_promotion_to_be_promoter.RequestData();
            HttpRequestComponent.Instance.Send(
                 WEB2_promotion_to_be_promoter.API,
                 WEB2_promotion_to_be_promoter.Request(web), json =>
                 {
                     var tDto = WEB2_promotion_to_be_promoter.Response(json);
                     if (pAct != null)
                         pAct(tDto);
                 });
        }

        #endregion

        #region 消息
        public event Action<int> CancelShowUIMine;//0大厅   1我的 
        public void RemoveMsgSummary(int pCurLoad)
        {
            if (CancelShowUIMine != null)
                CancelShowUIMine(pCurLoad);
        }

        public void AddListenerMsg()
        {
            //ETModel.Game.Hotfix.OnServerMes = OnServerMsg;
        }
        //void OnServerMsg(string s)//暂无用
        //{
        //    mIsRedPointShow = true;
        //}
        public int modifyHeadTime;
        /// <summary>      未修改昵称 或 未修改头像 才会走到Action       true 展开提示面板             </summary>
        public void CheckOpenDialog(Action pAct)
        {
            WEB2_user_self_info.RequestData requestData = new WEB2_user_self_info.RequestData()
            {
            };
            HttpRequestComponent.Instance.Send(
                WEB2_user_self_info.API,
                WEB2_user_self_info.Request(requestData), pDto =>
                {
                    var responseData = WEB2_user_self_info.Response(pDto);
                    if (responseData.status == 0)
                    {
                        if (responseData.data != null)
                        {
                            GameCache.Instance.modifyNickNum = responseData.data.modifyNickNum;
                            modifyHeadTime = responseData.data.modifyHeadTime;
                            if (GameCache.Instance.modifyNickNum <= 0 || responseData.data.modifyHeadTime <= 0)
                                pAct();
                        }
                    }
                    else
                    {
                        UIComponent.Instance.Toast(responseData.status);
                    }
                });
        }

        /// <summary>         是否存在未领取的任务         </summary>
        public void APIMissionReward(Action<bool> pAct)
        {
            var web = new WEB2_user_task_list.RequestData();
            HttpRequestComponent.Instance.Send(
                 WEB2_user_task_list.API,
                 WEB2_user_task_list.Request(web), json =>
                 {
                     var tDto = WEB2_user_task_list.Response(json);
                     if (tDto.status == 0)
                     {
                         bool isRed = false;
                         if (tDto.data != null && tDto.data.Count > 0)
                         {
                             for (int i = 0; i < tDto.data.Count; ++i)
                             {

                                 if (tDto.data[i].status == 3)//待领取时显示红点
                                 {
                                     isRed = true;
                                     break;
                                 }
                             }
                         }
                         if (pAct != null)
                             pAct(isRed);
                     }
                     else
                     {
                         UIComponent.Instance.Toast(tDto.status);
                     }
                 });
        }

        /// <summary>         是否有消息         </summary>
        public void APIMessagesNums(Action<bool> pAct)
        {
            var web = new WEB2_message_nums.RequestData();
            HttpRequestComponent.Instance.Send(
                 WEB2_message_nums.API,
                 WEB2_message_nums.Request(web), json =>
                 {
                     var tDto = WEB2_message_nums.Response(json);
                     if (tDto.status == 0)
                     {
                         int tCount = 0;
                         if (tDto.data != null)
                         {
                             tCount = tDto.data.unreadClubNum +
                                          tDto.data.unreadTribeNum +
                                          tDto.data.unreadSystemNum +
                                          tDto.data.unreadMoneyNum +
                                          tDto.data.unreadShareKdouNum +
                                          tDto.data.unreadBagNum;
                             mIsRedPointShow = tCount > 0;
                         }
                         if (pAct != null)
                             pAct(mIsRedPointShow);
                     }
                     else
                     {
                         UIComponent.Instance.Toast(tDto.status);
                     }
                 });
        }

        /// <summary>         是否有消息         </summary>
        public void APIMessagesNumsByType(int msgType, Action<int> pAct)
        {
            var web = new WEB2_message_nums.RequestData();
            HttpRequestComponent.Instance.Send(
                 WEB2_message_nums.API,
                 WEB2_message_nums.Request(web), json =>
                 {
                     var tDto = WEB2_message_nums.Response(json);
                     if (tDto.status == 0)
                     {
                         int tCount = 0;
                         if (tDto.data != null)
                         {
                             if (msgType == 1)
                             {
                                 tCount = tDto.data.unreadClubNum;
                             }
                         }
                         if (pAct != null)
                             pAct(tCount);
                     }
                     else
                     {
                         UIComponent.Instance.Toast(tDto.status);
                     }
                 });
        }


        /// <summary>
        /// 将 清空 某种类型的未读数量
        /// </summary>
        public void APIMsgClearNoRead(int pEn, Action pAct)
        {
            MsgLastEle tEle = null;
            if (mDicTypeEles.TryGetValue(pEn, out tEle))
            {
                if (tEle.num > 0)//有未读消息才去请求清空
                {
                    var requestData = new WEB2_message_clear.RequestData()
                    {
                        clearType = pEn
                    };
                    HttpRequestComponent.Instance.Send(
                        WEB2_message_clear.API,
                        WEB2_message_clear.Request(requestData), str =>
                        {
                            var tDto = WEB2_message_clear.Response(str);
                            if (tDto.status == 0)
                            {
                                UpdateDicNoReadMsg(pEn);
                                if (pAct != null)
                                    pAct();
                            }
                            else
                            {
                                UIComponent.Instance.Toast(tDto.status);
                            }
                        });
                }
                else
                {
                    if (pAct != null)
                        pAct();
                }
            }
        }

        Dictionary<int, MsgLastEle> mDicTypeEles = new Dictionary<int, MsgLastEle>();
        /// <summary>        若IM收到消息,将此设置为true    打开消息列表时才会重新请求 </summary>
        public bool mIsRedPointShow = false;
        public void APIGetMessageNumList(Action<Dictionary<int, MsgLastEle>> pActDto)
        {
            //if (mIsClearAll)
            //    mDicTypeEles.Clear();

            //if (mDicTypeEles.Count > 0)
            //{
            //    if (pActDto != null)
            //        pActDto(mDicTypeEles);
            //}
            //else
            //{
            mDicTypeEles = new Dictionary<int, MsgLastEle>();
            var requestData = new WEB2_message_nums_detail.RequestData()
            {
            };
            HttpRequestComponent.Instance.Send(
                WEB2_message_nums_detail.API,
                WEB2_message_nums_detail.Request(requestData), str =>
                {
                    var tDto = WEB2_message_nums_detail.Response(str);
                    mIsRedPointShow = false;
                    if (tDto.status == 0)
                    {
                        if (tDto.data != null)
                        {
                            for (int i = 0; i < tDto.data.Count; i++)
                            {
                                var key = tDto.data[i].messageType;
                                mDicTypeEles[key] = tDto.data[i];
                            }
                            if (pActDto != null)
                                pActDto(mDicTypeEles);
                        }
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
            //}
        }

        /// <summary>        /// 更新未读消息数量        /// </summary>
        public void UpdateDicNoReadMsg(int pMsgType)
        {
            MsgLastEle tEle = null;
            if (mDicTypeEles.TryGetValue(pMsgType, out tEle))
            {
                tEle.num = 0;
                mDicTypeEles[pMsgType] = tEle;
            }
        }

        /// <summary>         pDir=0上面的菊花         </summary>
        public void API2MsgData(int pMsgType, int pDir, long pTime, Action<DtoResponse> pActDto)
        {
            var requestData = new WEB2_message_list.RequestData()
            {
                time = pTime,
                direction = pDir,
                messageType = pMsgType,
                lanType = LanguageManager.mInstance.mCurLanguage
            };
            HttpRequestComponent.Instance.Send(
                WEB2_message_list.API,
                WEB2_message_list.Request(requestData), str =>
                {
                    var tDto = WEB2_message_list.Response(str);
                    if (pActDto != null)
                    {
                        pActDto(tDto);
                    }
                });
        }

        public void API2MsgOperate(string msgId, int operate, Action<DtoResponse> pActDto)
        {
            var requestData = new WEB2_message_handle.RequestData()
            {
                messageId = msgId,
                handleType = operate
            };
            HttpRequestComponent.Instance.Send(
                WEB2_message_handle.API,
                WEB2_message_handle.Request(requestData), str =>
                {
                    var tDto = WEB2_message_list.Response(str);
                    if (pActDto != null)
                    {
                        pActDto(tDto);
                    }
                });
        }

        public string GetRoomPathRecordListStr(int roomPath)
        {
            // 61 - 德州,62 - 德州必下场,63 - 德州AOF,91 - 奥马哈,92 - 奥马哈必下场, 93奥马哈 - AOF
            var tStrLanguage = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIRecordList");
            string str = "";
            if (roomPath == 61)
                str = tStrLanguage[0];// "德州";
            else if (roomPath == 21 || roomPath == 23)
                str = tStrLanguage[3];// "短牌";
            else if (roomPath == 91)
                str = tStrLanguage[1];// "奥马哈";
            else if (roomPath == 63 || roomPath == 93)
                str = tStrLanguage[2];//"AOF";

            return str;
        }

        public string GetMsg(int pType)
        {
            var tValue = LanguageManager.mInstance.GetLanguageForKey("MsgInfo_" + pType.ToString());
            return tValue;
        }


        #endregion

        #region 钱包
        /// <summary>         提取是否已绑定 银行卡了       相当于收货地址吧  new string [工商,账号]</summary>
        public string[] mBindingCardAcc;

        public int mBindType = 1;//1银行卡 3Token

        /// <summary>         提取是否已绑定 支付宝了         </summary>
        public string mBindingAlipayAcc = string.Empty;
        public string mBindingAlipayName = string.Empty;
        /// <summary>         是否已设置 二级密码了    也就是支付密码    </summary>
        public bool mIsSecondPW = false;

        public int mMyClubId = -1;

        public DtoSelfWallet walletMainDto;

        /// <summary>
        /// 【钱包模块】平台充值记录  0   pNextTime=下一个时间戳 若空 传当前时间
        /// </summary>
        public void APIRechargeLog_0(long pNextTime, Action<WEB2_wallet_recharge_log.Data> pAct)
        {
            var requestData = new WEB2_wallet_recharge_log.RequestData()
            {
                nextTime = pNextTime
            };
            HttpRequestComponent.Instance.Send(
                WEB2_wallet_recharge_log.API,
                WEB2_wallet_recharge_log.Request(requestData), str =>
                {
                    var tDto = WEB2_wallet_recharge_log.Response(str);
                    if (tDto.status == 0)
                    {
                        if (pAct != null)
                            pAct(tDto.data);
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
        }


        /// <summary>
        /// 【钱包模块】转USDT记录   1
        /// </summary>
        public void APIFlowListLog_1(int pNextId, Action<WEB2_wallet_flow_list.Data> pAct)
        {
            var requestData = new WEB2_wallet_flow_list.RequestData()
            {
                nextId = pNextId
            };
            HttpRequestComponent.Instance.Send(
                WEB2_wallet_flow_list.API,
                WEB2_wallet_flow_list.Request(requestData), str =>
                {
                    var tDto = WEB2_wallet_flow_list.Response(str);
                    if (tDto.status == 0)
                    {
                        if (pAct != null)
                            pAct(tDto.data);
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
        }

        /// <summary>
        /// 【钱包模块】-- 平台提取记录 2
        /// </summary>
        public void APIWithdrawLog_2(int pNextId, Action<WEB2_wallet_withdraw_log.Data> pAct)
        {
            var requestData = new WEB2_wallet_withdraw_log.RequestData()
            {
                nextId = pNextId
            };
            HttpRequestComponent.Instance.Send(
                WEB2_wallet_withdraw_log.API,
                WEB2_wallet_withdraw_log.Request(requestData), str =>
                {
                    var tDto = WEB2_wallet_withdraw_log.Response(str);
                    if (tDto.status == 0)
                    {
                        if (pAct != null)
                            pAct(tDto.data);
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
        }


        /// <summary>
        /// 【钱包模块】--返还记录 3  废弃
        /// </summary>
        //public void APIFeedBackLog_3(int pNextId, Action<WEB2_wallet_feedback_list.Data> pAct)
        //{
        //    var requestData = new WEB2_wallet_feedback_list.RequestData()
        //    {
        //        nextId = pNextId
        //    };
        //    HttpRequestComponent.Instance.Send(
        //        WEB2_wallet_feedback_list.API,
        //        WEB2_wallet_feedback_list.Request(requestData), str =>
        //        {
        //            var tDto = WEB2_wallet_feedback_list.Response(str);
        //            if (tDto.status == 0)
        //            {
        //                if (pAct != null)
        //                    pAct(tDto.data);
        //            }
        //            else
        //            {
        //                UIComponent.Instance.Toast(tDto.status);
        //            }
        //        });
        //}

        /// <summary>
        /// 钱包 其他记录
        /// </summary>
        public void APIOtherRecordLog_3(List<WEB2_wallet_other_income_list.idTable> pIdTabs, Action<WEB2_wallet_other_income_list.ResponseData> pAct)
        {
            var requestData = new WEB2_wallet_other_income_list.RequestData()
            {
                pageSize = 20,
                idTables = pIdTabs
            };
            HttpRequestComponent.Instance.Send(
                WEB2_wallet_other_income_list.API,
                WEB2_wallet_other_income_list.Request(requestData), str =>
                {
                    var tDto = WEB2_wallet_other_income_list.Response(str);
                    if (tDto.status == 0)
                    {
                        if (pAct != null)
                            pAct(tDto);
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
        }



        /// <summary>
        /// 平台提取 - 【钱包模块】  验证码 手机号 USDT数  交易方式:1-银行卡
        /// </summary>
        public void APIWithDraw(int chip, string pCode, Action<bool> pAct)
        {
            var requestData = new WEB2_wallet_withdraw.RequestData()
            {
                code = pCode,
                phone = GameCache.Instance.strPhone,
                chip = chip,
                transType = mBindType
            };
            HttpRequestComponent.Instance.Send(
                WEB2_wallet_withdraw.API,
                WEB2_wallet_withdraw.Request(requestData), str =>
                {
                    var tDto = WEB2_wallet_withdraw.Response(str);
                    if (tDto.status == 0)
                    {
                        if (pAct != null)
                            pAct(tDto.data);
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
        }


        /// <summary>
        /// 平台充值 - 【钱包模块】  amount充值金额
        /// </summary>
        public void APIRecharge(int amount, Action<string> pAct2213, Action<string> pAct)
        {
            var appType = 1;
            if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.Android)
            {
                appType = 1;
            }
            else if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.IPhonePlayer)
            {
                appType = 2;
            }

            var requestData = new WEB2_wallet_recharge.RequestData()
            {
                amount = amount,
                transType = 1,
                appType = appType
            };
            HttpRequestComponent.Instance.Send(
                WEB2_wallet_recharge.API,
                WEB2_wallet_recharge.Request(requestData), str =>
                {
                    var tDto = WEB2_wallet_recharge.Response(str);
                    if (tDto.status == 0)
                    {
                        if (pAct != null && tDto.data != null)
                            pAct(tDto.data.webPayUrl);
                    }
                    else if (tDto.status == 2213)
                    {
                        if (tDto.data.amount >= 10)
                        {
                            //賬戶余額不足，您當前最多可購買{0}元USDT，如需購買更多數量，請聯系客服
                            if (pAct2213 != null)
                                pAct2213(string.Format(LanguageManager.mInstance.GetLanguageForKey("UIMIneModel102"), tDto.data.amount.ToString()));
                        }
                        else
                        {
                            //账户余额不足，请联系客服
                            if (pAct2213 != null)
                                pAct2213(LanguageManager.mInstance.GetLanguageForKey("UIMIneModel1021"));
                        }
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
        }

        /// <summary>
        ///  保存用户收款账户信息 - 【钱包模块】 alipay支付宝
        /// </summary>
        public void APIAccountPayeeSaveAlipay(string alipayRealName, string alipayAcc,
            Action pAct)
        {
            var requestData = new WEB2_wallet_account_payee_save.RequestData()
            {
                type = 2,
                alipayRealName = alipayRealName,
                alipayAcc = alipayAcc
            };
            HttpRequestComponent.Instance.Send(
                WEB2_wallet_account_payee_save.API,
                WEB2_wallet_account_payee_save.Request(requestData), str =>
                {
                    var tDto = WEB2_wallet_account_payee_save.Response(str);
                    if (tDto.status == 0)
                    {
                        mBindingAlipayAcc = alipayAcc;
                        mBindingAlipayName = alipayRealName;
                        if (pAct != null)
                            pAct();
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
        }

        /// <summary>
        ///  保存用户收款账户信息 - 【钱包模块】 bank银行卡
        /// </summary>
        public void APIAccountPayeeSaveBank(string bankAccName, string bankAccNo, string bankName, string bankOfDeposit,
            string bankLocProvince, string bankLocCity, string bankCode, Action pAct)
        {
            var requestData = new WEB2_wallet_account_payee_save.RequestData()
            {
                type = mBindType,
                bankAccName = bankAccName,
                bankName = bankName,
                bankLocProvince = bankLocProvince,
                bankCode = bankCode,
                bankAccNo = bankAccNo,
                bankOfDeposit = bankOfDeposit,
                bankLocCity = bankLocCity
            };
            HttpRequestComponent.Instance.Send(
                WEB2_wallet_account_payee_save.API,
                WEB2_wallet_account_payee_save.Request(requestData), str =>
                {
                    var tDto = WEB2_wallet_account_payee_save.Response(str);
                    if (tDto.status == 0)
                    {
                        mBindingCardAcc = new string[] { bankCode, bankAccNo };
                        if (pAct != null)
                            pAct();
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
        }


        /// <summary>
        /// 转USDT - 【钱包模块】
        /// </summary>
        public void APIWalletFlow(string randomId, int chip, string payPassword, Action pAct)
        {
            if (walletMainDto == null)
            {
                UIComponent.Instance.Toast("有误");
                return;
            }

            if (walletMainDto.chip < chip)
            {
                UIComponent.Instance.ToastLanguage("UIMIneModel103");//Toast("USDT数不足");
                return;
            }

            var requestData = new WEB2_wallet_flow.RequestData()
            {
                randomId = randomId,
                chip = chip,
                payPassword = payPassword
            };
            HttpRequestComponent.Instance.Send(
                WEB2_wallet_flow.API,
                WEB2_wallet_flow.Request(requestData), str =>
                {
                    var tDto = WEB2_wallet_flow.Response(str);
                    if (tDto.status == 0)
                    {
                        walletMainDto.chip = walletMainDto.chip - chip;
                        if (pAct != null)
                            pAct();
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
        }

        /// <summary>
        /// 获取用户收款账户信息 - 【钱包模块】
        /// </summary>
        public void APIAccountPayeInfo(Action<WEB2_wallet_account_payee_info.Data> pAct)
        {
            var requestData = new WEB2_wallet_account_payee_info.RequestData()
            {
                type = mBindType
            };
            HttpRequestComponent.Instance.Send(
                WEB2_wallet_account_payee_info.API,
                WEB2_wallet_account_payee_info.Request(requestData), str =>
                {
                    var tDto = WEB2_wallet_account_payee_info.Response(str);
                    if (tDto.status == 0)
                    {
                        if (tDto.data != null)
                        {
                            mBindingAlipayAcc = tDto.data.alipayAcc;
                            mBindingAlipayName = tDto.data.alipayRealName;
                            if (string.IsNullOrEmpty(tDto.data.bankAccName) == false)
                            {
                                mBindingCardAcc = new string[] { tDto.data.bankCode, tDto.data.bankAccNo };
                            }
                        }

                        if (pAct != null)
                            pAct(tDto.data);
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
        }


        /// <summary>
        /// 设置支付密码 - 【钱包模块】
        /// </summary>
        public void APIPwdSave(string pCode, string pPwd, Action pAct)
        {
            var requestData = new WEB2_wallet_PayPWDSave.RequestData()
            {
                code = pCode,
                payPwd = pPwd
            };
            HttpRequestComponent.Instance.Send(
                WEB2_wallet_PayPWDSave.API,
                WEB2_wallet_PayPWDSave.Request(requestData), str =>
                {
                    var tDto = WEB2_wallet_account_payee_info.Response(str);
                    if (tDto.status == 0)
                    {
                        if (pAct != null)
                            pAct();
                        mIsSecondPW = true;
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
        }

        /// <summary>
        /// 获取当前用户使用的渠道充值提取配置信息 - 【钱包模块】
        /// </summary>
        public void APIRecharge_config(Action<WEB2_recharge_config.Data> pAct)
        {
            var requestData = new WEB2_recharge_config.RequestData()
            {
            };
            HttpRequestComponent.Instance.Send(
                WEB2_recharge_config.API,
                WEB2_recharge_config.Request(requestData), str =>
                {
                    var tDto = WEB2_recharge_config.Response(str);
                    if (tDto.status == 0)
                    {
                        if (pAct != null)
                            pAct(tDto.data);
                        mIsSecondPW = true;
                    }
                    else if (tDto.status == 2205)
                    {
                        UIMineModel.mInstance.ShowSDKCustom();
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
        }
        /// <summary>
        /// // 在此俱乐部的身份1 = 创建者   2 = 成员   3 = 非成员
        /// </summary>
        public int mUserType;
        /// <summary>
        /// transType	   0-俱乐部充值(默认),1-平台充值
        /// </summary>
        public void APIClubTransType(Action<int> pAct)
        {
            if (mMyClubId <= 0)
            {
                UIComponent.Instance.Toast("有误,请先请求APIGetClubId接口");
                return;
            }
            var requestData = new WEB2_club_view.RequestData()
            {
                clubId = mMyClubId.ToString()
            };
            HttpRequestComponent.Instance.Send(
                WEB2_club_view.API,
                WEB2_club_view.Request(requestData), str =>
                {
                    var tDto = WEB2_club_view.Response(str);
                    if (tDto.status == 0)
                    {
                        mUserType = tDto.data.userType;
                        if (pAct != null)
                            pAct(tDto.data.transType);
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
        }

        /// <summary>
        /// 获取 俱乐部 联系方式
        /// </summary>
        public void APIGetClubContactList(Action<List<WEB2_club_view.RecordArrElement>> pAct)
        {
            if (mMyClubId <= 0)
            {
                UIComponent.Instance.Toast("有误,请先请求APIGetClubId接口");
                return;
            }
            var requestData = new WEB2_club_view.RequestData()
            {
                clubId = mMyClubId.ToString()
            };
            HttpRequestComponent.Instance.Send(
                WEB2_club_view.API,
                WEB2_club_view.Request(requestData), str =>
                {
                    var tDto = WEB2_club_view.Response(str);
                    if (tDto.status == 0)
                    {
                        if (pAct != null)
                            pAct(tDto.data.contactList);
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
        }

        public void APIGetClubInfos(Action<WEB2_club_view.Data> pAct)
        {
            if (mMyClubId <= 0)
            {
                UIComponent.Instance.Toast("有误,请先请求APIGetClubId接口");
                return;
            }
            var requestData = new WEB2_club_view.RequestData()
            {
                clubId = mMyClubId.ToString()
            };
            HttpRequestComponent.Instance.Send(
                WEB2_club_view.API,
                WEB2_club_view.Request(requestData), str =>
                {
                    var tDto = WEB2_club_view.Response(str);
                    if (tDto.status == 0)
                    {
                        if (pAct != null)
                            pAct(tDto.data);
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
        }


        public void SetDialogShow(string[] tLangs, Action pCommitOk)
        {
            if (tLangs == null || tLangs.Length == 0) return;
            UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
            {
                type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                // 温馨提示^请联系客服^前往^取消
                title = tLangs[0],
                content = tLangs[1],
                contentCommit = tLangs[2],
                contentCancel = tLangs[3],
                actionCommit = pCommitOk,
                actionCancel = () =>
                {
                }
            });
        }




        /// <summary>        获取俱乐部ID  若有才去打开页面         </summary>
        public void APIGetClubId(Action<bool> pAct)
        {
            if (mMyClubId == -1)
            {
                WEB2_club_list.RequestData clublist_req = new WEB2_club_list.RequestData();
                HttpRequestComponent.Instance.Send(WEB2_club_list.API,
                                    WEB2_club_list.Request(clublist_req), json =>
                                    {
                                        var tDto = WEB2_club_list.Response(json);
                                        if (tDto.status == 0)
                                        {
                                            if (tDto.data != null && tDto.data.list != null && tDto.data.list.Count > 0)
                                            {
                                                mMyClubId = tDto.data.list[0].clubId;
                                            }
                                        }
                                        if (pAct != null)
                                        {
                                            pAct(mMyClubId != -1);
                                        }
                                    });
            }
            else
            {
                if (pAct != null)
                {
                    pAct(true);
                }
            }
        }


        /// <summary>
        /// 根据 randomId  得到昵称.....并不能缓存起来,昵称会被用户改呀
        /// </summary>      
        public void APIGetNickName(string randomId, Action<string> pAct)
        {
            if (string.IsNullOrEmpty(randomId))
            {
                UIComponent.Instance.Toast("ID不能为空");
                return;
            }
            var requestData = new WEB2_user_public_info.RequestData()
            {
                randomId = randomId
            };
            HttpRequestComponent.Instance.Send(
                WEB2_user_public_info.API,
                WEB2_user_public_info.Request(requestData), str =>
                {
                    var tDto = WEB2_user_public_info.Response(str);
                    if (tDto.status == 0)
                    {
                        if (pAct != null && tDto.data != null)
                            pAct(tDto.data.nickName);
                    }
                    else
                    {
                        if (pAct != null)
                        {
                            pAct(null);
                        }
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
        }

        #endregion

        #region 举报
        public void APIReportOther(string pContent, Action pActDto)
        {
            var requestData = new WEB2_Report_User.RequestData()
            {
                reportContent = pContent
            };
            HttpRequestComponent.Instance.Send(
                WEB2_Report_User.API,
                WEB2_Report_User.Request(requestData), str =>
                {
                    var tDto = WEB2_Report_User.Response(str);
                    if (tDto.status == 0)
                    {
                        if (pActDto != null)
                        {
                            pActDto();
                        }
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
        }
        #endregion


        #region 切换语言
        /// <summary>
        /// //0 = 中文    1 = 英文    2 = 繁体中文   
        /// </summary>
        public void APIUpdatePushInfo(int lanType, Action pActDto)
        {
            var deviceType = 0;
            if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.Android)
            {
                deviceType = 0;
            }
            else if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.IPhonePlayer)
            {
                deviceType = 1;
            }

            var requestData = new WEB2_UpdatePushInfo.RequestData()
            {
                lanType = lanType,
                deviceType = deviceType//0 = android    1 = ios
            };
            HttpRequestComponent.Instance.Send(
                WEB2_UpdatePushInfo.API,
                WEB2_UpdatePushInfo.Request(requestData), str =>
                {
                    var tDto = WEB2_UpdatePushInfo.Response(str);
                    if (tDto.status == 0)
                    {
                        if (pActDto != null)
                        {
                            pActDto();
                        }
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.status);
                    }
                });
        }
        #endregion

        public void API2RecordDetail(string pRoomId, Action<RecordDetailDto> pAct)
        {
            var req = new WEB2_record_detail.RequestData() { roomId = pRoomId };
            HttpRequestComponent.Instance.Send(WEB2_record_detail.API,
               WEB2_record_detail.Request(req), pStr =>
               {
                   var tDto = WEB2_record_detail.Response(pStr);
                   if (tDto.status == 0)
                   {
                       if (pAct != null)
                           pAct(tDto);
                   }
                   else
                   {
                       UIComponent.Instance.Toast(tDto.status);
                   }
               });
        }

        public void API2RecordMttDetail(int pRoomId, Action<RecordMttDetailDto> pAct)
        {
            var req = new WEB2_record_mtt_detail.RequestData() { gameID = pRoomId };
            HttpRequestComponent.Instance.Send(WEB2_record_mtt_detail.API,
               WEB2_record_mtt_detail.Request(req), pStr =>
               {
                   var tDto = WEB2_record_mtt_detail.Response(pStr);
                   if (tDto.status == 0)
                   {
                       if (pAct != null)
                           pAct(tDto);
                   }
                   else
                   {
                       UIComponent.Instance.Toast(tDto.status);
                   }
               });
        }


        public void ShowSDKCustom(string pFirst = "")
        {
            UIComponent.Instance.ShowNoAnimation(UIType.UILogin_LoadingWeb, new UILogin_LoadingWebComponent.LoadingWebData()
            {
                mWebUrl = "http://chat.chatpoker2.com/wap/chat.html?ttc=1&skc=1",
                mTitleTxt = LanguageManager.mInstance.GetLanguageForKey("UIMineMain01")
            });

            //var chatParams = new ChatParamsBody();
            //if (string.IsNullOrEmpty(pFirst) == false)
            //    chatParams.sendMsg = pFirst.Trim();
            //KeFuSdkComponent.Instance.StarChat(chatParams);
        }


        public string GetLanguageFormat(string key, params string[] value)
        {
            var str = string.Format(LanguageManager.mInstance.GetLanguageForKey(key), value);
            return str;
        }


        /// <summary>         得到银行卡 列表         </summary>
        public void APIGetBeans(Action<List<WEB2_GetBeanList.listData>> pAct)
        {
            var web = new WEB2_GetBeanList.RequestData() {
                type = mBindType
            };
            HttpRequestComponent.Instance.Send(
                 WEB2_GetBeanList.API,
                 WEB2_GetBeanList.Request(web), json =>
                 {
                     var tDto = WEB2_GetBeanList.Response(json);
                     var tDics = UIMineModel.mInstance.GetTokenNameDics();
                     if (mBindType == 1)
                     {
                         tDics = UIMineModel.mInstance.GetCardNameDics();
                     }
                     for (int i = 0; i < tDto.data.Count; i++)
                     {
                         var bankCode = tDto.data[i].bankCode;
                         string valueName;
                         if (tDics.TryGetValue(bankCode, out valueName))
                         {
                             tDto.data[i].bankName = valueName;
                         }
                         else
                         {
                             tDto.data[i].bankName = tDto.data[i].bankCode;
                         }
                     }
                     if (pAct != null)
                         pAct(tDto.data);
                 });
        }
    }

}