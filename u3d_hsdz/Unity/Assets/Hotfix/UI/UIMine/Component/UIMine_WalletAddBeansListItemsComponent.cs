using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using RecConfigDto = ETHotfix.WEB2_recharge_config.Data;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_WalletAddBeansListItemsComponentSystem : AwakeSystem<UIMine_WalletAddBeansListItemsComponent>
    {
        public override void Awake(UIMine_WalletAddBeansListItemsComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名:我的钱包 充值-充值 </summary>
    public class UIMine_WalletAddBeansListItemsComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Text textRemain;
        private InputField inputfieldAddBeans;
        private Transform transGrids;
        private Transform transGridItem;
        private int mSelectMoney;
        bool mIsCanClick = true;
        private Text Placeholder_Add;
        private Text textBottomDesc;
        private int[] mMinMaxMoney;


        private List<int> mMoneys;

        public void Awake()
        {
            mMoneys = new List<int>();
            mSelectMoney = 0;
            InitUI();
        }

        List<GameObject> mGridItems;

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_WalletAddBeansListItems);
            if (obj != null && obj is RecConfigDto)
            {
                var tBigDto = obj as RecConfigDto;

                mGridItems = new List<GameObject>();
                Transform tTr = null;
                for (int i = 0; i < tBigDto.products.Count; i++)
                {
                    tTr = GameObject.Instantiate(transGridItem, transGrids);
                    tTr.localEulerAngles = Vector3.zero;
                    tTr.localScale = Vector3.one;
                    SetGridItemData(tTr.gameObject, tBigDto.products[i].amount);
                    mMoneys.Add(tBigDto.products[i].amount);
                    tTr.gameObject.SetActive(true);
                    UIEventListener.Get(tTr.gameObject, i).onIntClick = ClickGridItems;
                    mGridItems.Add(tTr.gameObject);
                }
                transGridItem.gameObject.SetActive(false);
                if (UIMineModel.mInstance.walletMainDto != null)
                {
                    var value = UIMineModel.mInstance.walletMainDto.chip + UIMineModel.mInstance.walletMainDto.notExtractChip;
                    textRemain.text = StringHelper.GetShortString(value);
                }

                var tMinMax = string.Format(LanguageManager.mInstance.GetLanguageForKey("UIMine_WalletAdd_ShYippxT"), tBigDto.minRecharge.ToString(), tBigDto.maxRecharge.ToString());
                Placeholder_Add.text = tMinMax;

                var tMax = string.Format(LanguageManager.mInstance.GetLanguageForKey("UIMine_WalletAdd_1heqE7jB"), tBigDto.maxRecharge.ToString());
                textBottomDesc.text = tMax;

                mMinMaxMoney = new int[] { tBigDto.minRecharge, tBigDto.maxRecharge };
            }
        }

        private void ClickGridItems(GameObject go, int index)
        {
            if (mGridItems != null && mGridItems.Count > 0)
            {
                for (int i = 0; i < mGridItems.Count; i++)
                {
                    mGridItems[i].transform.Find("Image_Select").gameObject.SetActive(index == i);
                }
                mSelectMoney = mMoneys[index];
                inputfieldAddBeans.text = mSelectMoney.ToString();
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
            textRemain = rc.Get<GameObject>("Text_Remain").GetComponent<Text>();
            inputfieldAddBeans = rc.Get<GameObject>("InputField_AddBeans").GetComponent<InputField>();
            transGrids = rc.Get<GameObject>("Grids").transform;
            transGridItem = rc.Get<GameObject>("GridItem").transform;
            UIEventListener.Get(rc.Get<GameObject>("Button_Add")).onClick = ClickAdd;
            Placeholder_Add = rc.Get<GameObject>("Placeholder_Add").GetComponent<Text>();
            inputfieldAddBeans.onEndEdit.AddListener(OnEndEditAddBeans);
            textBottomDesc = rc.Get<GameObject>("Text_BottomDesc").GetComponent<Text>();
        }

        private void OnEndEditAddBeans(string arg0)
        {
            for (int i = 0; i < mGridItems.Count; i++)
            {
                mGridItems[i].transform.Find("Image_Select").gameObject.SetActive(false);
            }
            mSelectMoney = 0;
        }

        void ResetNonSelect()
        {
            if (mGridItems != null && mGridItems.Count > 0)
            {
                for (int i = 0; i < mGridItems.Count; i++)
                {
                    mGridItems[i].transform.Find("Image_Select").gameObject.SetActive(false);
                }
            }
        }

        private void ClickAdd(GameObject go)
        {
            if (mIsCanClick == false) return;

            if (string.IsNullOrEmpty(inputfieldAddBeans.text))
            {
                UIComponent.Instance.ToastLanguage("Wallet_AddItem3");
                return;
            }
            var tAmount = int.Parse(inputfieldAddBeans.text);

            if (tAmount == 0)
            {
                UIComponent.Instance.ToastLanguage("Wallet_AddItem4");
                return;
            }
            if (tAmount > mMinMaxMoney[1])
            {
                UIComponent.Instance.ToastFormat1("Wallet_AddItem5", mMinMaxMoney[1].ToString());
                return;
            }
            else if (tAmount < mMinMaxMoney[0])
            {
                UIComponent.Instance.ToastFormat1("Wallet_AddItem6", mMinMaxMoney[0].ToString());
                return;
            }

            if (tAmount % 100 != 0)
            {
                UIComponent.Instance.ToastLanguage("UIWallet_AddMoney_v01");
                return;
            }


            UIMineModel.mInstance.APIRecharge(tAmount, pErrStr =>
            {
                var tLangs = LanguageManager.mInstance.GetLanguageForKeyMoreValues("Become103");
                UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                {
                    type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                    // "温馨提示",  content = 账户余额不足，您当前最多可购买{0}元USDT，如需购买更多数量，请联系客服,   "前往",    contentCancel = "取消",
                    title = tLangs[0],
                    content = pErrStr, // tLangs[1],
                    contentCommit = tLangs[2],
                    contentCancel = tLangs[3],
                    actionCommit = () =>
                    {
                        UIMineModel.mInstance.ShowSDKCustom();
                    },
                    actionCancel = () =>
                    {
                    }
                });
            }, async pDto =>
             {
                 UIComponent.Instance.Toast(string.Format(LanguageManager.mInstance.GetLanguageForKey("Wallet_AddItem7"), (tAmount * 100).ToString(), tAmount.ToString()));
                 mSelectMoney = 0;
                 inputfieldAddBeans.text = "";
                 mIsCanClick = false;
                 await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(3000);
                 mIsCanClick = true;
                 ResetNonSelect();
                 Application.OpenURL(pDto);
             });
        }

        /// <summary>
        /// 传入Money
        /// </summary>
        void SetGridItemData(GameObject pGo, int pMoney)
        {
            pGo.transform.Find("Text_Beans").GetComponent<Text>().text = (pMoney * 100).ToString();
            pGo.transform.Find("Text_Money").GetComponent<Text>().text = "¥" + pMoney.ToString();
            pGo.transform.Find("Image_Select").gameObject.SetActive(false);
        }
    }


}


