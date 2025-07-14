using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_WalletClubGetMoneyListComponentSystem : AwakeSystem<UIMine_WalletClubGetMoneyListComponent>
    {
        public override void Awake(UIMine_WalletClubGetMoneyListComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名: </summary>
    public class UIMine_WalletClubGetMoneyListComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Text textItem;
        private Text textItem2;


        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_WalletClubGetMoneyList);
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
            textItem = rc.Get<GameObject>("Text_Item").GetComponent<Text>();
            textItem2 = rc.Get<GameObject>("Text_Item2").GetComponent<Text>();

            UIEventListener.Get(rc.Get<GameObject>("ItemInfo_1")).onClick = ClubGetMoney;
            UIEventListener.Get(rc.Get<GameObject>("ItemInfo_2")).onClick = PlatformGetMoney;
            UIEventListener.Get(rc.Get<GameObject>("ItemInfo_3")).onClick = PlatformGetMoney;
        }
        /// <summary>
        /// 平台提取
        /// </summary>
        private void PlatformGetMoney(GameObject go)
        {
            UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletExtra);
        }
        /// <summary>
        /// 俱乐部提取
        /// </summary>
        private void ClubGetMoney(GameObject go)
        {
            UIMineModel.mInstance.APIGetClubInfos(pDto =>
            {
                if (pDto.clubStatus == 0)
                {
                    if (pDto.contactList == null || pDto.contactList.Count == 0)
                    {
                        UIComponent.Instance.ToastLanguage("UIWallet_ShowNoClub005");
                    }
                    else
                    {
                        UIComponent.Instance.ShowNoAnimation(UIType.UICustomerServer, pDto.contactList );
                    }
                }
                else if (pDto.clubStatus == 1)
                {
                    UIMineModel.mInstance.ShowSDKCustom();
                }
            });

        }
    }
}


