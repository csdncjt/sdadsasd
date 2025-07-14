using ETModel;
using UnityEngine;
using UnityEngine.UI;


namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_WalletAddBeansAlipayComponentSystem : AwakeSystem<UIMine_WalletAddBeansAlipayComponent>
    {
        public override void Awake(UIMine_WalletAddBeansAlipayComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名: 我的钱包 充值-充值-支付宝</summary>
    public class UIMine_WalletAddBeansAlipayComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private InputField inputfieldName;
        private InputField inputfieldAccount;
        private InputField inputfieldServerWX;

        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            SetUpNav( UIType.UIMine_WalletAddBeansAlipay);
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
            inputfieldName = rc.Get<GameObject>("InputField_ID").GetComponent<InputField>();
            inputfieldAccount = rc.Get<GameObject>("InputField_Account").GetComponent<InputField>();
            inputfieldServerWX = rc.Get<GameObject>("InputField_ServerWX").GetComponent<InputField>();

            UIEventListener.Get(rc.Get<GameObject>("Button_CopyName"), 1).onIntClick = ClickSmallCopy;
            UIEventListener.Get(rc.Get<GameObject>("Button_CopyAccount"), 2).onIntClick = ClickSmallCopy;
            UIEventListener.Get(rc.Get<GameObject>("Button_CopyServerWX"), 3).onIntClick = ClickSmallCopy;
            UIEventListener.Get(rc.Get<GameObject>("Button_Big")).onClick = ClickBigCopy;
        }

        private void ClickBigCopy(GameObject go)
        {
            var tName = inputfieldName.text;
            var tAccount = inputfieldAccount.text;
            var tWX = inputfieldServerWX.text;
        }

        private void ClickSmallCopy(GameObject go, int index)
        {
            if (index == 1)
            {
                var tName = inputfieldName.text;
            }
            else if (index == 2)
            {
                var tAccount = inputfieldAccount.text;
            }
            else if (index == 3)
            {
                var tWX = inputfieldServerWX.text;
            }

        }
    }
}


