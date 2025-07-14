using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;


namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_WalletServiceChargeComponentSystem : AwakeSystem<UIMine_WalletServiceChargeComponent>
    {
        public override void Awake(UIMine_WalletServiceChargeComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UIMine_WalletServiceChargeComponentUpdateSystem : UpdateSystem<UIMine_WalletServiceChargeComponent>
    {
        public override void Update(UIMine_WalletServiceChargeComponent self)
        {
            self.Update();
        }
    }

    /// <summary> 页面名: 手续费dialog</summary>
    public class UIMine_WalletServiceChargeComponent : UIBaseComponent
    {
        public sealed class WalletServiceCharge
        {
            public Action mSureSuccAction;
            public long[] mSerChargeData;
            public string mGongShi;
            public double mRate;
            public bool mIsTransfer;//是否转USDT     false=默认提取
        }

        private ReferenceCollector rc;

        private Transform Image4;//gas
        private Transform Image5;
        private Transform Image6;
        private Transform Image8;//比例

        private Text textValue1Num;
        private Text textValue2Record;
        private Text textValue3SerCharge;
        private WalletServiceCharge mWalletSerChargeData;
        private Text text3;
        private Text text1;
        private Text text4;
        private Text Text_gongshi;
        private Text Text_gas;
        private Text Text_daozhang;
        private Text Text_rate;


        public void Awake()
        {
            InitUI();
        }

        public void Update()
        {

        }

        public override void OnShow(object obj)
        {
            if (obj != null && obj is WalletServiceCharge)
            {
                mWalletSerChargeData = obj as WalletServiceCharge;
                var tSC = LanguageManager.mInstance.GetLanguageForKey("UIMine_WalletSC001");
                textValue1Num.text = StringHelper.ShowGold((int)mWalletSerChargeData.mSerChargeData[0]) + tSC;
                textValue2Record.text = StringHelper.ShowGold((int)mWalletSerChargeData.mSerChargeData[1]) + tSC;
                textValue3SerCharge.text = StringHelper.ShowGold((int)mWalletSerChargeData.mSerChargeData[2]) + tSC;

                if (mWalletSerChargeData.mIsTransfer == true)//转账
                {
                    text1.text = LanguageManager.mInstance.GetLanguageForKey("WalletServiceCharge_GRvZTIYV2");
                    text3.text = LanguageManager.mInstance.GetLanguageForKey("WalletServiceCharge_YzYnYaRH2");
                    text4.text = LanguageManager.mInstance.GetLanguageForKey("WalletServiceCharge_0CuQrQWn2");
                    Image4.gameObject.SetActive(false);
                    Image5.gameObject.SetActive(false);
                    Image6.gameObject.SetActive(false);
                    Image8.gameObject.SetActive(false);
                }
                else
                {
                    
                    Image5.gameObject.SetActive(true);
                    Image6.gameObject.SetActive(true);
                    Text_gongshi.text = mWalletSerChargeData.mGongShi;
                    if (UIMineModel.mInstance.mBindType == 1)
                    {
                        Image4.gameObject.SetActive(false);
                        Image8.gameObject.SetActive(true);
                        Text_rate.text = "1:"+mWalletSerChargeData.mRate;
                        Text_daozhang.text = StringHelper.ShowGold((int)mWalletSerChargeData.mSerChargeData[4]) + "CNY";
                    }
                    else if (UIMineModel.mInstance.mBindType == 3)
                    {
                        Image4.gameObject.SetActive(true);
                        Image8.gameObject.SetActive(false);
                        Text_gas.text = StringHelper.ShowGold((int)mWalletSerChargeData.mSerChargeData[3]) + tSC;
                        Text_daozhang.text = StringHelper.ShowGold((int)mWalletSerChargeData.mSerChargeData[4]) + tSC;
                    }
                }
            }
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

            Image4 = rc.Get<GameObject>("Image4").transform;
            Image5 = rc.Get<GameObject>("Image5").transform;
            Image6 = rc.Get<GameObject>("Image6").transform;
            Image8 = rc.Get<GameObject>("Image8").transform;

            UIEventListener.Get(rc.Get<GameObject>("CloseBtn")).onClick = OnClickClose;
            textValue1Num = rc.Get<GameObject>("Text_Value1Num").GetComponent<Text>();
            textValue2Record = rc.Get<GameObject>("Text_Value2Record").GetComponent<Text>();
            textValue3SerCharge = rc.Get<GameObject>("Text_Value3SerCharge").GetComponent<Text>();
            UIEventListener.Get(rc.Get<GameObject>("SureBtn")).onClick = OnClickSure;


            text1 = rc.Get<GameObject>("Text_1").GetComponent<Text>();
            text3 = rc.Get<GameObject>("Text_3").GetComponent<Text>();
            text4 = rc.Get<GameObject>("Text_4").GetComponent<Text>();
            Text_gongshi = rc.Get<GameObject>("Text_gongshi").GetComponent<Text>();
            Text_gas = rc.Get<GameObject>("Text_gas").GetComponent<Text>();
            Text_rate = rc.Get<GameObject>("Text_rate").GetComponent<Text>();
            Text_daozhang = rc.Get<GameObject>("Text_daozhang").GetComponent<Text>();
            
        }

        private void OnClickClose(GameObject go)
        {
            UIComponent.Instance.Remove(UIType.UIMine_WalletServiceCharge);
        }


        private void OnClickSure(GameObject go)
        {
            if (mWalletSerChargeData != null)
                mWalletSerChargeData.mSureSuccAction();
            OnClickClose(null);
        }
    }
}


