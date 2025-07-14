using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ETModel;
using UnityEngine;
using UnityEngine.UI;


namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_WalletTransBeansBindingCardComponentSystem : AwakeSystem<UIMine_WalletTransBeansBindingCardComponent>
    {
        public override void Awake(UIMine_WalletTransBeansBindingCardComponent self)
        {
            self.Awake();
        }
    }


    /// <summary> 页面名: </summary>
    public class UIMine_WalletTransBeansBindingCardComponent : UIBaseComponent
    {
        private ReferenceCollector rc;

        private GameObject Panel_token;
        private GameObject Panel_card;

        private InputField inputfieldUserName;
        private InputField inputfieldCardNum;
        private InputField inputfieldCardOpen;
        private InputField inputfieldTokenAddr;//卡号
        private Image imageCommit;
        private Text textCommit;
        private Text textCardName;
        private Text textCardProvince;
        private Text textCardCity;
        private Text textTokenName;//名称
        private Dictionary<string, Sprite> mDicStrSprite;
        DBArea mProvinceDB;
        DBArea mCityDB;
        /// <summary>
        /// 0用户名  1银行卡号 2 银行名称  3开户行  4省  5市
        /// </summary>
        string[] mModifyOldData;


        public void Awake()
        {
            mModifyOldData = null;
            InitUI();
            mDicStrSprite = new Dictionary<string, Sprite>() {
                { "ImgRedBtn",rc.Get<Sprite>("ImgRedBtn")},
                { "ImgGrayBtn",rc.Get<Sprite>("ImgGrayBtn")}
            };
        }
        string mCardNameKey;
        /// <summary> 请选择 </summary>
        string mTempStr;

        /// <summary>
        /// obj=1 修改  obj=100无效银行卡
        /// </summary>
        public override void OnShow(object obj)
        {
            if (obj != null)
            {
                UIMineModel.mInstance.APIAccountPayeInfo(pDto =>
                {
                    if (UIMineModel.mInstance.mBindType == 1)
                    {
                        inputfieldUserName.text = pDto.bankAccName;
                        inputfieldCardNum.text = pDto.bankAccNo;
                        mCardNameKey = pDto.bankCode;
                        if ((int)obj == 1)
                            textCardName.text = UIMineModel.mInstance.GetCardNameDics()[mCardNameKey];
                        else
                            textCardName.text = "";
                        textCardName.color = new Color32(255, 255, 255, 255);
                        inputfieldCardOpen.text = pDto.bankOfDeposit;
                        EndEditInputValue(null);//变为红色
                        mProvinceDB = DBAreaComponent.Instance.QueryProvincesByWallet(pDto.bankLocProvince);
                        mCityDB = DBAreaComponent.Instance.QueryCityByWallet(pDto.bankLocCity);

                        textCardProvince.text = LanguageManager.mInstance.GetLanguageForKey(("area" + pDto.bankLocProvince));
                        textCardCity.text = LanguageManager.mInstance.GetLanguageForKey(("area" + pDto.bankLocCity));
                        mCityDB.walletLoad = true;
                        mProvinceDB.walletLoad = true;

                        mModifyOldData = new string[] { inputfieldUserName.text, inputfieldCardNum.text, textCardName.text, inputfieldCardOpen.text, textCardProvince.text, textCardCity.text };
                    }
                    else
                    {
                        textTokenName.text = pDto.bankAccName;
                        inputfieldTokenAddr.text = pDto.bankAccNo;
                        mCardNameKey = pDto.bankCode;
                        if ((int)obj == 1)
                            textTokenName.text = UIMineModel.mInstance.GetTokenName(mCardNameKey);
                        else
                            textTokenName.text = "";

                        textCardName.color = new Color32(255, 255, 255, 255);

                        mModifyOldData = new string[] { "", inputfieldTokenAddr.text, textTokenName.text, "", "", "" };
                    }
                  
                });

                if (UIMineModel.mInstance.mBindType == 1)
                {
                    textCardProvince.color = new Color32(255, 255, 255, 255);
                    textCardCity.color = new Color32(255, 255, 255, 255);
                    SetUpNav(LanguageManager.mInstance.GetLanguageForKey("BindingCardMoidify"), UIType.UIMine_WalletTransBeansBindingCard);
                }
                else
                {
                    SetUpNav(LanguageManager.mInstance.GetLanguageForKey("BindingCardMoidify2"), UIType.UIMine_WalletTransBeansBindingCard);
                }

                
                textCommit.text = LanguageManager.mInstance.GetLanguageForKey("WalletBinding11"); //"立即修改";
            }
            else
            {
                if (UIMineModel.mInstance.mBindType == 1)
                {
                    SetUpNav(LanguageManager.mInstance.GetLanguageForKey("BindingCard"), UIType.UIMine_WalletTransBeansBindingCard);
                }
                else
                {
                    SetUpNav(LanguageManager.mInstance.GetLanguageForKey("BindingCard2"), UIType.UIMine_WalletTransBeansBindingCard);
                }

                textCommit.text = LanguageManager.mInstance.GetLanguageForKey("WalletBinding12"); // //"立即绑定";
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

            Panel_token = rc.Get<GameObject>("Panel_token");
            Panel_card = rc.Get<GameObject>("Panel_card");

            inputfieldUserName = rc.Get<GameObject>("InputField_UserName").GetComponent<InputField>();
            inputfieldCardNum = rc.Get<GameObject>("InputField_CardNum").GetComponent<InputField>();
            inputfieldCardOpen = rc.Get<GameObject>("InputField_CardOpen").GetComponent<InputField>();
            var tCommit = rc.Get<GameObject>("Button_Commit");
            UIEventListener.Get(tCommit).onClick = ClickCommit;
            imageCommit = tCommit.GetComponent<Image>();

            textTokenName = rc.Get<GameObject>("Text_TokenName").GetComponent<Text>();
            inputfieldTokenAddr = rc.Get<GameObject>("InputField_Addr").GetComponent<InputField>();

            textCommit = rc.Get<GameObject>("Text_Commit").GetComponent<Text>();
            inputfieldUserName.onEndEdit.AddListener(EndEditInputValue);
            inputfieldCardNum.onEndEdit.AddListener(EndEditInputValue);
            inputfieldCardOpen.onEndEdit.AddListener(EndEditInputValue);
            inputfieldTokenAddr.onEndEdit.AddListener(EndEditInputValue);

            UIEventListener.Get(rc.Get<GameObject>("CardCityBtnSelected")).onClick = ClickCardCityBtnSelected;
            UIEventListener.Get(rc.Get<GameObject>("CardProvinceBtnSelected")).onClick = ClickCardProvinceBtnSelected;
            UIEventListener.Get(rc.Get<GameObject>("CardNameBtnSelected")).onClick = ClickCardNameBtnSelected;
            UIEventListener.Get(rc.Get<GameObject>("TokenNameBtnSelected")).onClick = ClickCardNameBtnSelected;
            textCardName = rc.Get<GameObject>("Text_CardName").GetComponent<Text>();
            textCardProvince = rc.Get<GameObject>("Text_CardProvince").GetComponent<Text>();
            textCardCity = rc.Get<GameObject>("Text_CardCity").GetComponent<Text>();
            mProvinceDB = new DBArea();
            mCityDB = new DBArea();

            textCardProvince.color = new Color32(255, 255, 255, 76);
            textCardCity.color = new Color32(255, 255, 255, 76);
            mTempStr = LanguageManager.mInstance.GetLanguageForKey("WalletBindingCard21");// "请选择";
            textCardProvince.text = mTempStr;// "请选择";
            textCardCity.text = mTempStr;// "请选择";
            textCardName.text = mTempStr;

            Panel_token.SetActive(UIMineModel.mInstance.mBindType == 3);
            Panel_card.SetActive(UIMineModel.mInstance.mBindType == 1);
        }

        private async void ClickCardCityBtnSelected(GameObject go)
        {
            if (mProvinceDB.id == 0 || textCardProvince.text.Equals(mTempStr))
            {
                UIComponent.Instance.ToastLanguage("WalletBindingCard22");//Toast("请先选择省份");
                return;
            }
            if (mProvinceDB.isNl == 1)
            {
                UIComponent.Instance.ToastLanguage("WalletBindingCard23");//Toast("已是直辖市啦");
                return;
            }
            await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_CreatCity, mProvinceDB, () => { });
        }

        private async void ClickCardProvinceBtnSelected(GameObject go)
        {
            await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_CreatProvince, true, () => { });
        }

        /// <summary>
        /// 选择了省份
        /// </summary>
        public void ShowWalletProvince(DBArea db)
        {
            textCardProvince.text = LanguageManager.mInstance.GetLanguageForKey(("area" + db.id.ToString()));
            mProvinceDB = db;
            mProvinceDB.walletLoad = true;
            textCardProvince.color = new Color32(255, 255, 255, 255);

            if (db.isNl != 1)
            {
                textCardCity.text = mTempStr;//"请选择";
                textCardCity.color = new Color32(255, 255, 255, 76);
                mCityDB = new DBArea();
            }
            else if (db.isNl == 1)
            {
                //1 = 直辖市   北京 上海
                textCardCity.text = LanguageManager.mInstance.GetLanguageForKey(("area" + db.id.ToString()));//db.name_zh;
                textCardCity.color = new Color32(255, 255, 255, 255);
            }
            EndEditInputValue(null);
        }

        /// <summary>
        /// 选择了 市区
        /// </summary>
        public void ShowWalletCity(DBArea db)
        {
            textCardCity.text = LanguageManager.mInstance.GetLanguageForKey(("area" + db.id.ToString()));
            mCityDB = db;
            textCardCity.color = new Color32(255, 255, 255, 255);
            EndEditInputValue(null);
        }

        private void ClickCardNameBtnSelected(GameObject go)
        {
            UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletCardNames, new UIMine_WalletCardNamesComponent.LocalCardNameData()
            {
                mCardNameKey = mCardNameKey, //textCardName.text,
                successDelegate = OnSuccessAction
            });
        }

        private void OnSuccessAction(string pKey)
        {
            if (string.IsNullOrEmpty(pKey))
                return;

            if (textCardName != null)
            {
                if (UIMineModel.mInstance.mBindType == 1)
                    textCardName.text = UIMineModel.mInstance.GetCardNameDics()[pKey];
                else
                {
                    textTokenName.text = UIMineModel.mInstance.GetTokenName(pKey);
                }
                textCardName.color = new Color32(255, 255, 255, 255);
                mCardNameKey = pKey;
            }
            EndEditInputValue(null);
        }

        private void ClickCommit(GameObject go)
        {
            if (UIMineModel.mInstance.mBindType == 1)
            {
                var tCardNum = inputfieldCardNum.text.Trim();
                var tCardUser = inputfieldUserName.text.Trim();
                var tCardOpen = inputfieldCardOpen.text.Trim();
                if (MethodHelper.IsStrNotNull(new string[] { tCardNum, tCardUser, tCardOpen }) && textCardName.text != mTempStr && textCardProvince.text != mTempStr && textCardCity.text != mTempStr)
                {
                    if (UIMineModel.mInstance.IsNumLetter(inputfieldCardNum.text) == false)
                    {
                        UIComponent.Instance.ToastLanguage("WalletBindingCard24");//Toast("银行卡号输入有误,请重新输入");
                        return;
                    }
                    if (UIMineModel.mInstance.IsTextShow(inputfieldCardOpen.text) == false)
                    {
                        UIComponent.Instance.ToastLanguage("WalletBindingCard25");//Toast("开户行输入有误,请重新输入");
                        return;
                    }

                    if (mModifyOldData != null && tCardUser == mModifyOldData[0] && tCardNum == mModifyOldData[1] && textCardName.text == mModifyOldData[2] &&
                        tCardOpen == mModifyOldData[3] && textCardProvince.text == mModifyOldData[4] && textCardCity.text == mModifyOldData[5])
                    {
                        UIComponent.Instance.ToastLanguage("WalletBindingCard26");////Toast("您并没有修改信息");
                        return;
                    }

                    UIMineModel.mInstance.APIAccountPayeeSaveBank(tCardUser, tCardNum, textCardName.text, tCardOpen,
                        mProvinceDB.id.ToString(), mCityDB.id.ToString(), mCardNameKey, delegate
                        {
                            if (mModifyOldData != null && mModifyOldData.Length == 6)
                            {
                                UIComponent.Instance.ToastLanguage("WalletBindingCard27");//Toast("您提交的修改信息正在审核中");
                            }
                            else
                            {
                                UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletTransBeansCard);
                            }
                            UIComponent.Instance.Remove(UIType.UIMine_WalletTransBeansBindingCard);
                        });
                }
                else
                {
                    UIComponent.Instance.ToastLanguage("WalletCardTT44");
                }
            }
            else
            {
                var tCardNum = inputfieldTokenAddr.text.Trim();
                if (string.IsNullOrEmpty(tCardNum) == false)
                {
                    if (mModifyOldData != null  && tCardNum == mModifyOldData[1] && textTokenName.text == mModifyOldData[2])
                    {
                        UIComponent.Instance.ToastLanguage("WalletBindingCard26");////Toast("您并没有修改信息");
                        return;
                    }

                    UIMineModel.mInstance.APIAccountPayeeSaveBank(mCardNameKey, tCardNum, textTokenName.text, mCardNameKey,
                        mCardNameKey, mCardNameKey, mCardNameKey, delegate
                        {
                            if (mModifyOldData != null && mModifyOldData.Length == 6)
                            {
                                UIComponent.Instance.ToastLanguage("WalletBindingCard27");//Toast("您提交的修改信息正在审核中");
                            }
                            else
                            {
                                UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletTransBeansCard);
                            }
                            UIComponent.Instance.Remove(UIType.UIMine_WalletTransBeansBindingCard);
                        });
                }
                else
                {
                    UIComponent.Instance.ToastLanguage("WalletCardTT44");
                }
            }
           
        }

        private void EndEditInputValue(string arg0)
        {
            if (UIMineModel.mInstance.mBindType == 1)
            {
                var tCardNum = inputfieldCardNum.text.Trim();
                var tCardUser = inputfieldUserName.text.Trim();
                var tCardOpen = inputfieldCardOpen.text.Trim();
                if (mModifyOldData == null)
                {
                    if (MethodHelper.IsStrNotNull(new string[] { tCardNum, tCardUser, tCardOpen }) && textCardName.text != mTempStr && textCardProvince.text != mTempStr && textCardCity.text != mTempStr)
                    {
                        SetRed();
                    }
                    else
                    {
                        SetGray();
                    }
                }
                else
                {
                    if (MethodHelper.IsStrNotNull(new string[] { tCardNum, tCardUser, tCardOpen }) && textCardName.text != mTempStr && textCardProvince.text != mTempStr && textCardCity.text != mTempStr)
                    {
                        if (tCardUser != mModifyOldData[0] || tCardNum != mModifyOldData[1] || textCardName.text != mModifyOldData[2] ||
            tCardOpen != mModifyOldData[3] || textCardProvince.text != mModifyOldData[4] || textCardCity.text != mModifyOldData[5])
                        {
                            SetRed();
                        }
                        else
                        {
                            SetGray();
                        }
                    }
                    else
                    {
                        SetGray();
                    }
                }
            }
            else
            {
                var tCardNum = inputfieldTokenAddr.text.Trim();
                if (mModifyOldData == null)
                {

                    if (string.IsNullOrEmpty(tCardNum) == false && textTokenName.text != mTempStr)
                    {
                        SetRed();
                    }
                    else
                    {
                        SetGray();
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(tCardNum) == false && textTokenName.text != mTempStr)
                    {
                        if (tCardNum != mModifyOldData[1] || textTokenName.text != mModifyOldData[2])
                        {
                            SetRed();
                        }
                        else
                        {
                            SetGray();
                        }
                    }
                    else
                    {
                        SetGray();
                    }
                }
            }
        }

        void SetRed()
        {
            imageCommit.sprite = mDicStrSprite["ImgRedBtn"];
            textCommit.color = new Color32(249, 243, 189, 255);
        }
        void SetGray()
        {
            imageCommit.sprite = mDicStrSprite["ImgGrayBtn"];
            textCommit.color = new Color32(175, 175, 175, 255);
        }
    }
}


