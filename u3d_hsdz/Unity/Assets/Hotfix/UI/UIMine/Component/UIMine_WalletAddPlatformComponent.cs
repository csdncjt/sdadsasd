using System;
using ETModel;
using UnityEngine;


namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_WalletAddPlatformComponentSystem : AwakeSystem<UIMine_WalletAddPlatformComponent>
    {
        public override void Awake(UIMine_WalletAddPlatformComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 充值页面  平台充值
    /// </summary>
    public class UIMine_WalletAddPlatformComponent : UIBaseComponent
    {

        private ReferenceCollector rc;
        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_WalletAddPlatform, OnClickCopyKefu);

            //不需要判断 商场是否关闭
            //if (obj != null && obj is WEB2_query_function_open.ResponseData)
            //{
            //    var pDto = obj as WEB2_query_function_open.ResponseData;
            //    for (int i = 0; i < pDto.data.Count; i++)
            //    {
            //        if (pDto.data[i].functionCode == 3)//商城
            //        {
            //            tItemInfo3Shop.SetActive(pDto.data[i].open);
            //            tItemInfo2Shop.SetActive(pDto.data[i].open);
            //            tShowYes = pDto.data[i].open;
            //        }
            //    }
            //}
            //if (tShowYes == false)
            //{
            //    rc.Get<GameObject>("ItemInfo_4").transform.localPosition = tItemInfo2Shop.transform.localPosition;
            //}
        }

        private void OnClickCopyKefu()
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

        bool tShowYes = true;
        GameObject tItemInfo2Shop;
        GameObject tItemInfo3Shop;
        GameObject tItemInfo4Shop;
        #region UI
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            tShowYes = true;
            UIEventListener.Get(rc.Get<GameObject>("ItemInfo_1")).onClick = OnClickCardPay;
            tItemInfo2Shop = rc.Get<GameObject>("ItemInfo_2");
            tItemInfo3Shop = rc.Get<GameObject>("ItemInfo_3");
            tItemInfo4Shop = rc.Get<GameObject>("ItemInfo_4");

            UIEventListener.Get(tItemInfo3Shop).onClick = OnClickItemShop;
            UIEventListener.Get(tItemInfo2Shop).onClick = OnClickItemShop;
            UIEventListener.Get(tItemInfo4Shop).onClick = OnClickItemShop;
            UIEventListener.Get(rc.Get<GameObject>("ItemInfo_5")).onClick = OnClickKefu;
        }

        private async void OnClickItemShop(GameObject go)
        {
            await UIComponent.Instance.ShowAsyncNoAnimation(UIType.UIMine_Store);
        }

        private void OnClickCardPay(GameObject go)
        {
            UIMineModel.mInstance.APIRecharge_config(pDto =>
            {
                UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletAddBeansListItems, pDto);
            });
        }

        private void OnClickKefu(GameObject go)
        {
            UIMineModel.mInstance.ShowSDKCustom();
        }

        #endregion




    }
}
