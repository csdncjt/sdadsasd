using System;
using ETModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_WalletTransBeansCardComponentSystem : AwakeSystem<UIMine_WalletTransBeansCardComponent>
    {
        public override void Awake(UIMine_WalletTransBeansCardComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名:我的 钱包 银行卡 </summary>
    public class UIMine_WalletTransBeansCardComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Text textWalletValue;
        private Text textNoUsing;
        private Text textCanUsing;
        private Text textCardNums;
        private Text textCardName;
        private InputField inputfieldCardGetNum;
        //private InputField inputfieldCardGetMoney;

        private GameObject gasObject;
        private Text Text_gas;
        private Text Text_fee;
        private Text Text_rate;

        private InputField inputfieldCardMoblie;
        private InputField inputfieldCardCode;
        private Text textCodeCard;
        private GameObject ProblemShow;
        private Text textRecord;
        private Text TextDesc;

        int minValue = 50;
        int maxValue = 10000;

        public void Awake()
        {
            mDisLoad = false;
            InitUI();
            CheckCurrentSupport();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_WalletTransBeansCard);
            // TextDesc.text = string.Format(LanguageManager.mInstance.GetLanguageForKey("WalletTransBeansCard_1001"), UIMineModel.mInstance.mUserType == 1 ? "0%" : "3%");
            OnGetGas();
        }

        public override void OnHide()
        {
            mDisLoad = true;
        }

        public override void Dispose()
        {
            base.Dispose();
            mDisLoad = true;
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
            textCardNums = rc.Get<GameObject>("Text_CardNums").GetComponent<Text>();
            textCardName = rc.Get<GameObject>("Text_CardName").GetComponent<Text>();

            inputfieldCardGetNum = rc.Get<GameObject>("InputField_CardGetNum").GetComponent<InputField>();
            //inputfieldCardGetMoney = rc.Get<GameObject>("InputField_CardGetMoney").GetComponent<InputField>();

            gasObject = rc.Get<GameObject>("Gas");
            Text_gas = rc.Get<GameObject>("Text_gas").GetComponent<Text>();
            Text_fee = rc.Get<GameObject>("Text_fee").GetComponent<Text>();
            Text_rate = rc.Get<GameObject>("Text_rate").GetComponent<Text>();

            inputfieldCardMoblie = rc.Get<GameObject>("InputField_CardMoblie").GetComponent<InputField>();
            inputfieldCardCode = rc.Get<GameObject>("InputField_CardCode").GetComponent<InputField>();
            textCodeCard = rc.Get<GameObject>("Text_CodeCard").GetComponent<Text>();
            UIEventListener.Get(rc.Get<GameObject>("BtnCode")).onClick = OnClickCode;
            UIEventListener.Get(rc.Get<GameObject>("Button_Commit")).onClick = OnClickCommit;
            textRecord = rc.Get<GameObject>("Text_Record").GetComponent<Text>();
            TextDesc = rc.Get<GameObject>("TextDesc").GetComponent<Text>();

            ShowMoney();
            ShowAccount();
            inputfieldCardGetNum.onEndEdit.AddListener(OnGetNumEndEdit);
            inputfieldCardMoblie.text = UIMineModel.mInstance.GetMobileStarHide();
            mIsCanClickCode = true;

            UIEventListener.Get(rc.Get<GameObject>("Button_max")).onClick = OnClickMax;

            gasObject.SetActive(false);
            if (UIMineModel.mInstance.mBindType == 3)
            {
                gasObject.SetActive(true);
            }
            //if (LanguageManager.mInstance.mCurLanguage != 1)
            //{
            //    var local = rc.Get<GameObject>("Button_Commit").transform.localPosition;
            //    rc.Get<GameObject>("Button_Commit").transform.localPosition = new Vector3(local.x, local.y + 40, local.z);
            //}
        }

        private int mGetGlodNum = 0;
        private int mGetGlodFee = 0;
        private int mGetGlodGas = 0;
        private int mIsBoss = 0;
        private double mRate = 1.0f;
        private string mCalGongshi;

        //获取gas
        private void OnGetGas()
        {
            Text_gas.text = "";
            var requestData = new WEB2_wallet_getGas.RequestData()
            {
                type = UIMineModel.mInstance.mBindType
            };
            HttpRequestComponent.Instance.Send(
                WEB2_wallet_getGas.API,
                WEB2_wallet_getGas.Request(requestData), str =>
                {
                    var tDto = WEB2_wallet_getGas.Response(str);
                    if (tDto.status == 0)
                    {
                        if (tDto.data != null)
                        {
                            mGetGlodGas = tDto.data.gas;
                            mIsBoss = tDto.data.isboss;
                            mRate = tDto.data.rate;
                            Text_gas.text = StringHelper.ShowGold(mGetGlodGas);

                            Text_rate.text = string.Format(LanguageManager.mInstance.GetLanguageForKey("WalletTransBeansCard_6aaXxV"), UIMineModel.mInstance.mBindType == 1 ? mRate : 1);
                            
                        }
                    }
                });
        }

        private void OnGetNumEndEdit(string arg0)
        {
            var tNumStr = inputfieldCardGetNum.text.Trim();
            if (tNumStr.Length <= 0 || tNumStr.Length > 9) return;
            var tNum = float.Parse(tNumStr);
            if (tNum < minValue || tNum > maxValue)
            {
                UIComponent.Instance.ToastLanguage("WalletCardTT41");//("您输入的USDT数量有误，请重新输入");
                inputfieldCardGetNum.text = "";
                Text_fee.text = "0";
                mGetGlodNum = 0;
            }
            else
            {
                mGetGlodNum = StringHelper.GetIntGold(tNum);
                //inputfieldCardGetMoney.text = tNum.ToString();
                inputfieldCardGetNum.text = tNum.ToString();
                var tSC = LanguageManager.mInstance.GetLanguageForKey("UIMine_WalletSC001");
                //手续费
                mGetGlodFee = 0;
                int disnum = mGetGlodNum - (int)UIMineModel.mInstance.walletMainDto.plCount;
                if (disnum <= 0 || mIsBoss == 1)
                {
                    mGetGlodFee = (int)Math.Ceiling(mGetGlodNum * 0.02);
                    mCalGongshi = StringHelper.ShowGold(mGetGlodFee) + tSC;
                }
                else //（战绩流水数）* 2% +（提U-战绩流水）* 10 %
                {
                    int fee1 = (int)Math.Ceiling(UIMineModel.mInstance.walletMainDto.plCount * 0.02);
                    int fee2 = (int)Math.Ceiling(disnum * 0.1);
                    mGetGlodFee = fee1 + fee2;
                    mCalGongshi = StringHelper.ShowGold(fee1)+ tSC+ "+" + StringHelper.ShowGold(fee2) + tSC;
                }

                Text_fee.text = StringHelper.ShowGold(mGetGlodFee);
            }
        }

        private void OnClickMax(GameObject go)
        {
            if (UIMineModel.mInstance.walletMainDto.chip > maxValue * 100)
            {
                inputfieldCardGetNum.text = maxValue.ToString();
            }
            else
            {
                inputfieldCardGetNum.text = textCanUsing.text.Trim();
            }

            OnGetNumEndEdit("");
        }

        private void OnClickCommit(GameObject go)
        {
            var tMobile = inputfieldCardMoblie.text.Trim();
            var tCode = inputfieldCardCode.text.Trim();

            if (MethodHelper.IsStrNotNull(new string[] { tMobile, tCode }) && mGetGlodNum != 0)
            {
                if (UIMineModel.mInstance.walletMainDto != null)
                {
                    var tPlCount = UIMineModel.mInstance.walletMainDto.plCount;
                    var t = mGetGlodFee;

                    if (UIMineModel.mInstance.mBindType == 1)
                    {
                        var ynum = (long)((mGetGlodNum - t) * mRate);
                        var tWalletSC = new UIMine_WalletServiceChargeComponent.WalletServiceCharge()
                        {
                            mSerChargeData = new long[] { mGetGlodNum, tPlCount, t, 0, ynum },
                            mGongShi = mCalGongshi,
                            mRate = mRate,
                            mSureSuccAction = delegate
                            {
                                APIGetMoney(mGetGlodNum, tCode);
                            }
                        };
                        UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletServiceCharge, tWalletSC);
                    }
                    else if (UIMineModel.mInstance.mBindType == 3)
                    {
                        var ynum = mGetGlodNum - t - mGetGlodGas;
                        var tWalletSC = new UIMine_WalletServiceChargeComponent.WalletServiceCharge()
                        {
                            mSerChargeData = new long[] { mGetGlodNum, tPlCount, t, mGetGlodGas, ynum },
                            mGongShi = mCalGongshi,
                            mRate = 1.0f,
                            mSureSuccAction = delegate
                            {
                                APIGetMoney(mGetGlodNum, tCode);
                            }
                        };
                        UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletServiceCharge, tWalletSC);
                    }

                        
                   
                }
            }
            else
            {
                UIComponent.Instance.ToastLanguage("WalletCardTT44");
            }
        }

        void APIGetMoney(int pGoldNum, string pCode)
        {
            UIMineModel.mInstance.APIWithDraw(pGoldNum, pCode, async pDto =>
           {
               UIComponent.Instance.ToastLanguage("WalletCardTT43");//("提取已发出申请");

               UIComponent.Instance.Remove(UIType.UIMine_WalletTransBeansBindingCard);

               mGetGlodNum = 0;
               inputfieldCardCode.text = "";
               inputfieldCardGetNum.text = "";

               UIMineModel.mInstance.walletMainDto.plCount -= pGoldNum;
               if (UIMineModel.mInstance.walletMainDto.plCount < 0)
                   UIMineModel.mInstance.walletMainDto.plCount = 0;
               await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(1000);
               UIComponent.Instance.Remove(UIType.UIMine_WalletTransBeansCard);
           });
        }


        bool mIsCanClickCode = true;
        private void OnClickCode(GameObject go)
        {
            if (MethodHelper.IsStrNotNull(new string[] { GameCache.Instance.strPhone, GameCache.Instance.strPhoneFirst }))
            {
                if (textCodeCard.text.Equals(LanguageManager.mInstance.GetLanguageForKey("UILogin_GetCode")) == false)
                {
                    UIComponent.Instance.ToastLanguage("UIMine_Setting106");//("请稍等再发");
                    return;
                }
                if (mIsCanClickCode == false)
                {
                    UIComponent.Instance.ToastLanguage("UIMine_Setting106");//("请稍等再发");
                    return;
                }

                mIsCanClickCode = false;
                UILoginModel.mInstance.APISendCode(GameCache.Instance.strPhoneFirst, GameCache.Instance.strPhone, 5, tDto =>
                {
                    if (tDto.status == 0)
                    {
                        UIComponent.Instance.ToastLanguage("WalletCardTT45");//Toast("验证码已发送");
                        UILoginModel.mInstance.ShowTimes(textCodeCard);
                        mIsCanClickCode = true;
                    }
                    else
                    {
                        UIComponent.Instance.Toast(tDto.msg);
                    }
                });
            }
            else
            {
                UIComponent.Instance.Toast("有误");
            }
        }

        public void ShowAccount()
        {
            if (UIMineModel.mInstance.mBindingCardAcc != null && UIMineModel.mInstance.mBindingCardAcc.Length > 0)
            {
                if (UIMineModel.mInstance.mBindType == 1)
                {
                    textCardName.text = UIMineModel.mInstance.GetCardNameDics()[UIMineModel.mInstance.mBindingCardAcc[0]];
                    textCardNums.text = UIMineModel.mInstance.HideStarString(UIMineModel.mInstance.mBindingCardAcc[1], 4);
                }
                else
                {
                    textCardName.text = UIMineModel.mInstance.GetTokenName(UIMineModel.mInstance.mBindingCardAcc[0]);
                    textCardNums.text = UIMineModel.mInstance.HideStarString(UIMineModel.mInstance.mBindingCardAcc[1], 4);
                }
            }
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
            UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletTransBeansBindingCard, 1);
        }

        /// <summary>         检测目前支持银行卡         </summary>
        private void CheckCurrentSupport()
        {
            UIMineModel.mInstance.APIGetBeans(tDtos =>
            {
                bool isSupport = false;
                for (int i = 0; i < tDtos.Count; i++)
                {
                    if (tDtos[i].bankCode == UIMineModel.mInstance.mBindingCardAcc[0])
                    {
                        isSupport = true;
                    }
                }
                if (isSupport == false)
                {
                    ShowShareGoldBean();
                    WaitTimeClick();
                }
            });
        }

        bool mDisLoad = false;
        async void WaitTimeClick()
        {   
            await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(30000);
            if (mDisLoad == false)
            {
                UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletTransBeansBindingCard, 100);
                UIComponent.Instance.Remove(UIType.UIDialog);
            }
        }

        public void ShowShareGoldBean()
        {
            var tLangs = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIBeanModify001");
            if (tLangs == null || tLangs.Length == 0) return;
            UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
            {
                type = UIDialogComponent.DialogData.DialogType.Commit,
                title = tLangs[0],
                content = string.Format(tLangs[1], UIMineModel.mInstance.GetTokenName(UIMineModel.mInstance.mBindingCardAcc[0])),
                contentCommit = tLangs[2],
                actionCommit = () =>
                {
                    mDisLoad = true;
                    UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletTransBeansBindingCard, 100);
                },
                actionCancel = null
            });
        }
    }
}




