using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using FunctionOpenDto = ETHotfix.WEB2_query_function_open.ResponseData;

namespace ETHotfix
{

    [ObjectSystem]
    public class UIMineComponentSystem : AwakeSystem<UIMineComponent>
    {
        public override void Awake(UIMineComponent self)
        {
            self.Awake();
        }
    }

    public class UIMineComponent : UIBaseComponent
    {
        ReferenceCollector rc;
        GameObject head;
        RawImage headImage;
        Image genderImage;
        Text nameText;
        Text userIDText;
        Text vipText;
        Image vipIcon;
        Text text_gold_bean;
        private GameObject BtnShare;
        private GameObject RedPointMsg;
        private GameObject RedPointMsg_mission;//任务红点
        GameObject BtnWallet;
        GameObject BtnBeanStore;
        private GameObject imageAddBean;

        public void Awake()
        {
            InitUI();
            UIMineModel.mInstance.CancelShowUIMine += CancelShowUIMine;
            UIMatchModel.mInstance.CheckOpenNickName();//是否 要弹出 修改 昵称的弹框 
        }

        private void OnServerMsg(string obj)
        {
            if (obj.Equals("1"))
            {
                //新消息
                if (RedPointMsg != null)
                    RedPointMsg.SetActive(true);
                ObtainUserInfo();
            }
            else if (obj.Equals("2"))
            {
                if (RedPointMsg != null)
                    RedPointMsg.SetActive(true);
            }
            else
            {
                Log.Debug($"error OnServerMsg obj = {obj}");
            }
        }

        private void CancelShowUIMine(int pLoad)
        {
            if (pLoad == 0)
                return;

            UIMineModel.mInstance.APIMessagesNums(nums =>
            {
                if (RedPointMsg != null)
                    RedPointMsg.SetActive(nums);
            });

            //检查任务的红点
            UIMineModel.mInstance.APIMissionReward(nums =>
            {
                if (RedPointMsg_mission != null)
                    RedPointMsg_mission.SetActive(nums);
            });
        }

        string mURLGameFair = string.Empty;
        /// <summary>
        /// 分享赠USDT 是否开启
        /// </summary>
        private void UpdateOpenPromotion(FunctionOpenDto pDto)
        {
            for (int i = 0; i < pDto.data.Count; i++)
            {
                if (pDto.data[i].functionCode == 1)//分享赚USDT
                {
                    BtnShare.SetActive(pDto.data[i].open);
                }
                else if (pDto.data[i].functionCode == 2)//钱包
                {
                    BtnWallet.SetActive(pDto.data[i].open);
                }
                else if (pDto.data[i].functionCode == 5)//游戏公平
                {
                    mURLGameFair = pDto.data[i].value;
                }
                else if (pDto.data[i].functionCode == 3)//商城
                {
                    imageAddBean.SetActive(pDto.data[i].open);
                    mIsShopOpen = pDto.data[i].open;
                }
            }
        }
        public void ObtainUserInfo()
        {
            UIMineModel.mInstance.ObtainUserInfo(pDto =>
            {
                if (this.IsDisposed)
                {
                    return;
                }
                userIDText.text = $"ID:{pDto.randomNum}";
                GameCache.Instance.nick = pDto.nickName;
                GameCache.Instance.sex = (sbyte)pDto.sex;
                //GameCache.Instance.idou = responseData.data.idou;
                GameCache.Instance.headPic = pDto.head;
                GameCache.Instance.modifyNickNum = pDto.modifyNickNum;
                GameCache.Instance.vipLevel = (sbyte)pDto.vip;
                GameCache.Instance.vipEndDate = $"{pDto.vipEndDate}";

                UpdateUI();
            });
        }


        public override void OnShow(object obj)
        {
            if (obj != null && obj is FunctionOpenDto)//开关
            {
                var tDto = obj as FunctionOpenDto;
                UpdateOpenPromotion(tDto);
            }
            else
            {
                UIMineModel.mInstance.APIPromotionIsOpen(pDto =>
               {
                   UpdateOpenPromotion(pDto);
               });
            }

            ObtainUserInfo();
            UpdateUI();

            UIMineModel.mInstance.APIMessagesNums(nums =>
            {
                if (RedPointMsg != null)
                    RedPointMsg.SetActive(nums);
            });

            //检查任务的红点
            UIMineModel.mInstance.APIMissionReward(nums =>
            {
                if (RedPointMsg_mission != null)
                    RedPointMsg_mission.SetActive(nums);
            });

            SystemMesComponent.Instance.newMessageDelegate = OnServerMsg;
        }

        public override void OnHide()
        {
            SystemMesComponent.Instance.newMessageDelegate = null;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            UIMineModel.mInstance.CancelShowUIMine -= CancelShowUIMine;
            SystemMesComponent.Instance.newMessageDelegate = null;
        }


        private void UpdateUI()
        {
            nameText.text = GameCache.Instance.nick;
            text_gold_bean.text = StringHelper.ShowGold(GameCache.Instance.gold, true);
            genderImage.sprite = (UserGender)GameCache.Instance.sex == UserGender.Male ? rc.Get<Sprite>("my_boy_icon") : rc.Get<Sprite>("my_girl_icon");

            WebImageHelper.SetHeadImage(headImage, GameCache.Instance.headPic);

            VIPType myVipType = (VIPType)GameCache.Instance.vipLevel;
            if (myVipType != VIPType.VIP_None)
            {
                string endDate = TimeHelper.GetDateTimer(long.Parse(GameCache.Instance.vipEndDate)).ToString("yyyy/MM/dd");
                vipText.text = $"{endDate}" + LanguageManager.Get("UIMine_expire");
                vipIcon.gameObject.SetActive(true);
            }
            else
            {
                vipText.text = LanguageManager.Get("UIMine_becomeVIP");
                vipIcon.gameObject.SetActive(false);
            }

            // vipIcon.sprite = rc.Get<Sprite>(myVipType.ToString());
            if (myVipType == VIPType.VIP_A)
            {
                vipIcon.sprite = rc.Get<Sprite>("my_vip_huangjin_icon");
            }
            else if (myVipType == VIPType.VIP_B)
            {
                vipIcon.sprite = rc.Get<Sprite>("my_vip_bojin_icon");
            }
            else if (myVipType == VIPType.VIP_C)
            {
                vipIcon.sprite = rc.Get<Sprite>("my_vip_zuanshi_icon");
            }
        }

        bool mIsShopOpen = false;

        #region 同时点击事件   1.1秒后才能点击
        double lastClickTime = 0;//最后一次点击
        double GetNowTime()//当前时间
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return ts.TotalMilliseconds;
        }
        /// <summary>         是否能点击  true=能点击         </summary>
        bool CanClick()
        {
            if (GetNowTime() - lastClickTime > 1100)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region UI
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            GameObject scrollView = rc.Get<GameObject>("Scroll View");
            NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
            float realTop = safeArea.top * 1242 / safeArea.width;
            scrollView.GetComponent<RectTransform>().offsetMax = new Vector2(0, -realTop);
            float realBottom = safeArea.bottom * 1242 / safeArea.width;
            scrollView.GetComponent<RectTransform>().offsetMin = new Vector2(0, realBottom);

            headImage = rc.Get<GameObject>("img_head").GetComponent<RawImage>();
            head = rc.Get<GameObject>("UIHead");
            genderImage = rc.Get<GameObject>("img_gender").GetComponent<Image>();
            nameText = rc.Get<GameObject>("text_name").GetComponent<Text>();
            userIDText = rc.Get<GameObject>("text_userID").GetComponent<Text>();
            vipText = rc.Get<GameObject>("text_vip").GetComponent<Text>();
            vipIcon = rc.Get<GameObject>("img_vip_icon").GetComponent<Image>();
            text_gold_bean = rc.Get<GameObject>("text_gold_bean").GetComponent<Text>();
            RedPointMsg = rc.Get<GameObject>("RedPointMsg");
            RedPointMsg_mission = rc.Get<GameObject>("RedPointMsg_mission");
            imageAddBean = rc.Get<GameObject>("Image_AddBean");
            UIEventListener.Get(head).onClick = ClickUserInfo;
            UIEventListener.Get(nameText.gameObject).onClick = ClickUserInfo;

            mIsShopOpen = false;

            UIEventListener.Get(rc.Get<GameObject>("btn_help_service")).onClick = (go) =>
            {//客服
                if (CanClick() == false)
                    return;
                lastClickTime = GetNowTime();

                UIMineModel.mInstance.ShowSDKCustom();
            };
            BtnBeanStore = rc.Get<GameObject>("btn_bean_store");
            UIEventListener.Get(BtnBeanStore).onClick = (go) =>
           {//商城
               if (CanClick() == false)
                   return;
               lastClickTime = GetNowTime();

               if (mIsShopOpen)
                   UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletAddBeansList);
           };

            UIEventListener.Get(rc.Get<GameObject>("btn_vip_top_bg")).onClick = (go) =>
           {//VIP top
               if (CanClick() == false)
                   return;
               lastClickTime = GetNowTime();

               UIComponent.Instance.ShowNoAnimation(UIType.UIMine_VIP);
           };

            UIEventListener.Get(rc.Get<GameObject>("btn_VIP")).onClick = (go) =>
           {//VIP
               if (CanClick() == false)
                   return;
               lastClickTime = GetNowTime();

               UIComponent.Instance.ShowNoAnimation(UIType.UIMine_VIP);
           };

            UIEventListener.Get(rc.Get<GameObject>("btn_paipu")).onClick = (go) =>
           {//牌谱
               if (CanClick() == false)
                   return;
               lastClickTime = GetNowTime();

               UIComponent.Instance.ShowNoAnimation(UIType.UIMine_Paipu);
           };

            UIEventListener.Get(rc.Get<GameObject>("btn_setting")).onClick = (go) =>
           {//Setting
               if (CanClick() == false)
                   return;
               lastClickTime = GetNowTime();

               UIComponent.Instance.ShowNoAnimation(UIType.UIMine_Setting);
           };

            UIEventListener.Get(rc.Get<GameObject>("btn_fair")).onClick = (go) =>
               {
                   if (CanClick() == false)
                       return;
                   lastClickTime = GetNowTime();
                   UIComponent.Instance.ShowNoAnimation(UIType.UILogin_LoadingWeb,
                                            new UILogin_LoadingWebComponent.LoadingWebData { mTitleTxt = LanguageManager.Get("UIMine_btn_fair"), mWebUrl = mURLGameFair + UIMineModel.mInstance.GetUrlSuffix() });
               };
            UIEventListener.Get(rc.Get<GameObject>("btn_backpack")).onClick = (go) =>
           {//我的背包         
               if (CanClick() == false)
                   return;
               lastClickTime = GetNowTime();

               /*APIBackListInfos(pDto =>
              {
                  if (pDto.status == 0)
                  {
                      UIComponent.Instance.ShowNoAnimation(UIType.UIMine_BackpackList, pDto.data.gameBagGoods);
                  }
                  else
                  {
                      Log.Error("背包数据");
                  }
              });*/

               //背包系统入口转任务系统入口
               UIComponent.Instance.ShowNoAnimation(UIType.UIMine_Mission);
           };
            BtnShare = rc.Get<GameObject>("btn_share");
            UIEventListener.Get(BtnShare).onClick = (t) =>
            {
                if (CanClick() == false)
                    return;
                lastClickTime = GetNowTime();
                UIComponent.Instance.ShowNoAnimation(UIType.UIMine_ShareGolden);
            };

            UIEventListener.Get(rc.Get<GameObject>("btn_record")).onClick = (go) =>
           {
               if (CanClick() == false)
                   return;
               lastClickTime = GetNowTime();
               UIComponent.Instance.ShowNoAnimation(UIType.UIMine_RecordList, 0);
           };
            BtnWallet = rc.Get<GameObject>("btn_wallet");
            UIEventListener.Get(BtnWallet).onClick = (go) =>
           {
               if (CanClick() == false)
                   return;
               lastClickTime = GetNowTime();

               UIMineModel.mInstance.ObtainUserInfo(pDto =>
               {
                   if (pDto.wallet.status == 1)
                   {
                       UIMineModel.mInstance.GetTransferOpen(show =>
                       {
                           UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletMy, new object[] { pDto, show });
                       });
                   }
                   else if (pDto.wallet.status == 2)
                   {
                       var tLangs = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIWallet_ClubStop_D01");
                       UIMineModel.mInstance.SetDialogShow(tLangs, delegate
                       {
                           UIMineModel.mInstance.ShowSDKCustom();
                       });
                   }
               });
           };
            UIEventListener.Get(rc.Get<GameObject>("btn_message")).onClick = (go) =>
            {
                if (CanClick() == false)
                    return;
                lastClickTime = GetNowTime();
                UIMineModel.mInstance.APIGetMessageNumList(pDtos =>
                {
                    UIComponent.Instance.ShowNoAnimation(UIType.UIMine_MsgSummary, new UIMine_MsgSummaryComponent.MsgSummaryData()
                    {
                        mDicMsgLast = pDtos,
                        mOpenResource = 1
                    });
                });
            };
        }


        void APIBackListInfos(Action<WEB2_backpack_info.ResponseData> pAct)
        {
            var tReq = new WEB2_backpack_info.RequestData() { };
            HttpRequestComponent.Instance.Send(WEB2_backpack_info.API,
            WEB2_backpack_info.Request(tReq), (str) =>
            {
                var tDto = WEB2_backpack_info.Response(str);
                if (pAct != null)
                    pAct(tDto);
            });
        }

        #endregion

        #region Action
        private async void ClickUserInfo(GameObject go)
        {
            await UIComponent.Instance.ShowAsyncNoAnimation(UIType.UIMine_UserInfoSetting);
        }
        #endregion

    }
}
