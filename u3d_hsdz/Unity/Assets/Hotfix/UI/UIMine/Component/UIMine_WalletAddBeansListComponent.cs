using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;


namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_WalletAddBeansListComponentSystem : AwakeSystem<UIMine_WalletAddBeansListComponent>
    {
        public override void Awake(UIMine_WalletAddBeansListComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名:我的钱包 充值-平台与俱乐部 </summary>
    public class UIMine_WalletAddBeansListComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        //private Dictionary<string, Sprite> mDicStringSprite;
        private Text textItem;
        private Image imageLeftItem;

        /// <summary> 俱乐部充值模式下        平台         </summary>
        private GameObject ItemInfo_2;

        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_WalletAddBeansList, null, ClickLeftAction);
            //if (obj != null && obj is int)
            //{
            //    var tType = (int)obj;
            //    //0俱乐部充值(默认),1平台充值
            //    if (tType == 0)//充值模式   0俱乐部充值
            //    {//第一项 俱乐部  第二项平台
            //        textItem.text = LanguageManager.mInstance.GetLanguageForKey("Wallet_AddItem1");
            //        ItemInfo_2.SetActive(true);
            //        textItem2.text = LanguageManager.mInstance.GetLanguageForKey("Wallet_AddItem2");

            //        UIEventListener.Get(rc.Get<GameObject>("ItemInfo_1")).onClick = go =>//选择俱乐部充值，点击以后弹出客服联系方式
            //        {
            //            UIMineModel.mInstance.APIGetClubInfos(pDto =>
            //            {
            //                if (pDto.clubStatus == 0)//0是正常
            //                {
            //                    if (pDto.contactList == null || pDto.contactList.Count == 0)
            //                    {
            //                        UIComponent.Instance.ToastLanguage("UIWallet_ShowNoClub005");
            //                    }
            //                    else
            //                    {
            //                        UIComponent.Instance.ShowNoAnimation(UIType.UICustomerServer, pDto.contactList);
            //                    }
            //                }
            //                else
            //                {
            //                    var tLangs = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIWallet_ClubStop_b01");
            //                    UIMineModel.mInstance.SetDialogShow(tLangs, delegate
            //                    {
            //                        UIMineModel.mInstance.ShowSDKCustom();
            //                    });
            //                }
            //            });
            //        };
            //        UIEventListener.Get(rc.Get<GameObject>("ItemInfo_2")).onClick = async go =>//选择平台充值，会让玩家选择具体的充值方式，(两个子项)
            //        {
            //            //UIMineModel.mInstance.APIPromotionIsOpen(pDto =>
            //            //{
            //            //  UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletAddPlatform);
            //            // });
            //            await UIComponent.Instance.ShowAsyncNoAnimation(UIType.UIMine_Store);
            //        };
            //    }
            //    else//充值模式 平台充值
            //    {
            //        textItem.text = LanguageManager.mInstance.GetLanguageForKey("Wallet_AddItem2");
            //        imageLeftItem.sprite = mDicStringSprite["image_pingtai"];
            //        UIEventListener.Get(rc.Get<GameObject>("ItemInfo_1")).onClick = async go =>
            //        {
            //            //UIMineModel.mInstance.APIPromotionIsOpen(pDto =>
            //            //{
            //            //UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletAddPlatform);
            //            //});
            //            await UIComponent.Instance.ShowAsyncNoAnimation(UIType.UIMine_Store);
            //        };
            //    }
            //}
        }


        private void ClickLeftAction()
        {
            var tUI = UIComponent.Instance.Get(UIType.UIMine_WalletMy);
            if (tUI != null)
            {
                var tWalletMy = tUI.UiBaseComponent as UIMine_WalletMyComponent;
                tWalletMy.ShowRefresh();
            }
            UIComponent.Instance.Remove(UIType.UIMine_WalletAddBeansList);
        }

        public override void OnHide()
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            UIEventListener.Get(rc.Get<GameObject>("ItemInfo_1")).onClick = go =>
            {
                WEB_pay_recharge_apply.RequestData requestData = new WEB_pay_recharge_apply.RequestData()
                {
                    pay_type = 3,
                    pdt_id = "",
                    lang = LanguageManager.mInstance.mCurLanguage
                };
                HttpRequestComponent.Instance.Send(
                    WEB_pay_recharge_apply.API,
                    WEB_pay_recharge_apply.Request(requestData),
                    this.OpenOrderInfo,
                    GlobalData.Instance.PayURL);
            };

            UIEventListener.Get(rc.Get<GameObject>("ItemInfo_3")).onClick = go =>
            {
                UIComponent.Instance.Toast(LanguageManager.Get("adaptation10301"));
            };

            UIEventListener.Get(rc.Get<GameObject>("ItemInfo_2")).onClick = async go =>
            {
                await UIComponent.Instance.ShowAsyncNoAnimation(UIType.UIMine_Store);
            };
        }

        void OpenOrderInfo(string resData)
        {
            WEB_pay_recharge_apply.ResponseData responseData = WEB_pay_recharge_apply.Response(resData);
            if (responseData.status == 0)
            {
                string url = responseData.data.pay_info;
                Application.OpenURL(url);
            }
        }

        private Text textItem2;


    }
}
