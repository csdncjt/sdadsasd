using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_WalletTransBeansAlipayComponentSystem : AwakeSystem<UIMine_WalletTransBeansAlipayComponent>
    {
        public override void Awake(UIMine_WalletTransBeansAlipayComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名:我的 钱包 支付宝 </summary>
    public class UIMine_WalletTransBeansAlipayComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Text textWalletValue;
        private Text textNoUsing;
        private Text textCanUsing;
        private Text textAlipayNums;
        private Text textRecord;

        private InputField inputfieldCardGetNum;
        private InputField inputfieldCardGetMoney;
        private GameObject ProblemShow;

        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_WalletTransBeansAlipay);
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
            UIEventListener.Get(rc.Get<GameObject>("ModifyBtn")).onClick = ClickModifyBtn;
            UIEventListener.Get(rc.Get<GameObject>("Image_Problem")).onClick = ClickImage_Problem;
            ProblemShow = rc.Get<GameObject>("ProblemShow");
            ProblemShow.SetActive(false);

            textCanUsing = rc.Get<GameObject>("Text_CanUsing").GetComponent<Text>();
            textNoUsing = rc.Get<GameObject>("Text_NoUsing").GetComponent<Text>();
            textWalletValue = rc.Get<GameObject>("Text_WalletValue").GetComponent<Text>();
            textAlipayNums = rc.Get<GameObject>("Text_AlipayNums").GetComponent<Text>();
            textRecord = rc.Get<GameObject>("Text_Record").GetComponent<Text>();

            inputfieldCardGetNum = rc.Get<GameObject>("InputField_CardGetNum").GetComponent<InputField>();
            inputfieldCardGetMoney = rc.Get<GameObject>("InputField_CardGetMoney").GetComponent<InputField>();
            UIEventListener.Get(rc.Get<GameObject>("Button_Commit")).onClick = OnClickCommit;
            ShowMoney();
            ShowAccount();
            inputfieldCardGetNum.onEndEdit.AddListener(OnGetNumEndEdit);
        }



        private int mGetGlodNum = 0;
        private void OnGetNumEndEdit(string arg0)
        {
            var tNumStr = inputfieldCardGetNum.text.Trim();
            if (tNumStr.Length <= 0 || tNumStr.Length > 9) return;
            var tNum = 0;
            if (int.TryParse(tNumStr, out tNum))
            {//◆提取数量，最少100豆，最多5000000豆，同时必须为100豆的整数倍
                if (tNum < 1 || tNum > 50000)
               // if (tNum < 1 || tNum > 50000 || (tNum % 100 != 0))
                {
                    UIComponent.Instance.ToastLanguage("WalletAlipay11");//Toast("您输入的USDT数量有误，请重新输入");
                    inputfieldCardGetNum.text = "";
                    inputfieldCardGetMoney.text = "";
                    mGetGlodNum = 0;
                }
                else
                {
                    mGetGlodNum = tNum;
                    inputfieldCardGetMoney.text = tNum.ToString();
                    inputfieldCardGetNum.text = tNum.ToString();

                    //inputfieldCardGetNum.contentType = InputField.ContentType.Standard;
                    //inputfieldCardGetNum.ForceLabelUpdate();
                    //mGetGlodNum = tNum;
                    //inputfieldCardGetMoney.text = "";
                    //inputfieldCardGetNum.text = "";
                    //inputfieldCardGetMoney.text = (tNum / 100).ToString() + LanguageManager.mInstance.GetLanguageForKey("WalletTransBeansCard_1002");//元
                    //inputfieldCardGetNum.text = tNum.ToString() + LanguageManager.mInstance.GetLanguageForKey("UIMine_WalletSC001");//USDT
                    //inputfieldCardGetNum.contentType = InputField.ContentType.IntegerNumber;
                    //inputfieldCardGetNum.ForceLabelUpdate();
                }
            }
            else
            {
                UIComponent.Instance.ToastLanguage("WalletAlipay12");//.Toast("非法值");
                inputfieldCardGetNum.text = "";
                mGetGlodNum = 0;
            }
        }



        private void OnClickCommit(GameObject go)
        {
            var tNum = inputfieldCardGetNum.text.Trim();
            var tMoney = inputfieldCardGetMoney.text.Trim();
            if (MethodHelper.IsStrNotNull(new string[] { tNum, tMoney }))
            {
                int num = StringHelper.GetIntGold(float.Parse(tNum));
                var tSend = string.Format(LanguageManager.mInstance.GetLanguageForKey("UIMine_WalletPlatform_A01"), num.ToString(), tMoney.ToString(), UIMineModel.mInstance.mBindingAlipayName, UIMineModel.mInstance.mBindingAlipayAcc);
                UIMineModel.mInstance.ShowSDKCustom(tSend.Trim());
            }
            else
            {
                UIComponent.Instance.ToastLanguage("WalletAlipay14");//Toast("请填写完整信息");
            }
        }

        /// <summary>显示账号</summary>
        public void ShowAccount()
        {
            textAlipayNums.text = UIMineModel.mInstance.HideStarString(UIMineModel.mInstance.mBindingAlipayAcc, 2);
        }

        void ShowMoney()
        {
            if (UIMineModel.mInstance.walletMainDto != null)
            {
                textWalletValue.text = StringHelper.ShowGold(UIMineModel.mInstance.walletMainDto.chip + UIMineModel.mInstance.walletMainDto.notExtractChip, false);
                textCanUsing.text = StringHelper.ShowGold(UIMineModel.mInstance.walletMainDto.chip, false);
                textNoUsing.text = StringHelper.ShowGold(UIMineModel.mInstance.walletMainDto.notExtractChip, false);
                textRecord.text = StringHelper.ShowGold((int)UIMineModel.mInstance.walletMainDto.plCount);
            }
        }


        private void ClickImage_Problem(GameObject go)
        {
            ProblemShow.SetActive(!ProblemShow.activeInHierarchy);
        }

        private void ClickModifyBtn(GameObject go)
        {
            UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletTransBeansBindingAlipay, 1);
        }
    }
}


