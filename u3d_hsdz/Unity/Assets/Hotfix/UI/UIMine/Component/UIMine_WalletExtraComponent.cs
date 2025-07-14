using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_WalletExtraComponentSystem : AwakeSystem<UIMine_WalletExtraComponent>
    {
        public override void Awake(UIMine_WalletExtraComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名:提取 总 </summary>
    public class UIMine_WalletExtraComponent : UIBaseComponent
    {
        private ReferenceCollector rc;

        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_WalletExtra, ClickRightAction, ClickLeftAction);
        }

        private void ClickLeftAction()
        {
            var tUI = UIComponent.Instance.Get(UIType.UIMine_WalletMy);
            if (tUI != null)
            {
                var tWalletMy = tUI.UiBaseComponent as UIMine_WalletMyComponent;
                tWalletMy.ShowRefresh();
            }
            UIComponent.Instance.Remove(UIType.UIMine_WalletExtra);
        }



        private void ClickRightAction()
        {
            UIMineModel.mInstance.ShowSDKCustom();
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
            UIEventListener.Get(rc.Get<GameObject>("ItemInfo_3")).onClick = ClickInfoCardOTC;
            //UIEventListener.Get(rc.Get<GameObject>("ItemInfo_2")).onClick = ClickInfoAlipay;
            UIEventListener.Get(rc.Get<GameObject>("ItemInfo_1")).onClick = ClickInfoCard;
        }

        private void ClickInfoAlipay(GameObject go)
        {
            //UIMineModel.mInstance.APIClubTransType(pType =>
            //{
            //    if (pType == 0)
            //    {
            //        //   交易模式为俱乐部充值  选择支付宝提取，弹出第三方客服
            //        UIMineModel.mInstance.ShowSDKCustom();
            //    }
            //    else
            //    {
            //        UIMineModel.mInstance.APIAccountPayeInfo(pDto =>
            //        {
            //            if (string.IsNullOrEmpty(UIMineModel.mInstance.mBindingAlipayAcc))
            //                UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletTransBeansBindingAlipay);
            //            else
            //                UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletTransBeansAlipay);
            //        });
            //    }
            //});

        }

        //快捷
        private void ClickInfoCard(GameObject go)
        {
            turnToNext(1);
        }

        private void ClickInfoCardOTC(GameObject go)
        {
            turnToNext(3);
        }

        private void turnToNext(int type)
        {
            UIMineModel.mInstance.mBindingCardAcc = null;
            UIMineModel.mInstance.mBindType = type;
            UIMineModel.mInstance.APIAccountPayeInfo(pDto =>
            {
                if (UIMineModel.mInstance.mBindingCardAcc == null || UIMineModel.mInstance.mBindingCardAcc.Length == 0 || string.IsNullOrEmpty(UIMineModel.mInstance.mBindingCardAcc[0]))
                    UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletTransBeansBindingCard);
                else
                    UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletTransBeansCard);
            });
        }


    }
}


