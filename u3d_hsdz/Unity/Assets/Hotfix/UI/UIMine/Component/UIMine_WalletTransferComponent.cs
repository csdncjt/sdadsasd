using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_WalletTransferComponentSystem : AwakeSystem<UIMine_WalletTransferComponent>
    {
        public override void Awake(UIMine_WalletTransferComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 我的钱包 转USDT(已设置2级密码)
    /// </summary>
    public class UIMine_WalletTransferComponent : UIBaseComponent
    {

        private ReferenceCollector rc;
        private InputField inputfieldID;
        private InputField inputfieldNum;
        private InputField inputfieldPassword;
        private Image spriteCommit;
        private Text textCommit;
        private GameObject Nick;
        private Text textNick;
        private Text textPerformance;
        private Dictionary<string, Sprite> mDicStrSprite;

        public void Awake()
        {
            InitUI();
            mDicStrSprite = new Dictionary<string, Sprite>();
            mDicStrSprite["ImgRedBtn"] = rc.Get<Sprite>("ImgRedBtn");
            mDicStrSprite["ImgGrayBtn"] = rc.Get<Sprite>("ImgGrayBtn");
            mIsCanClick = true;
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_WalletTransfer, null, ClickLeftAction);
        }
        private void ClickLeftAction()
        {
            var tUI = UIComponent.Instance.Get(UIType.UIMine_WalletMy);
            if (tUI != null)
            {
                var tWalletMy = tUI.UiBaseComponent as UIMine_WalletMyComponent;
                tWalletMy.ShowRefresh();
            }
            UIComponent.Instance.Remove(UIType.UIMine_WalletTransfer);
        }

        public override void OnHide()
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }

        #region UI
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            inputfieldID = rc.Get<GameObject>("InputField_ID").GetComponent<InputField>();
            inputfieldNum = rc.Get<GameObject>("InputField_Num").GetComponent<InputField>();
            inputfieldPassword = rc.Get<GameObject>("InputField_Password").GetComponent<InputField>();
            spriteCommit = rc.Get<GameObject>("Button_Commit").GetComponent<Image>();
            textCommit = rc.Get<GameObject>("Text_Commit").GetComponent<Text>();
            UIEventListener.Get(spriteCommit.gameObject).onClick = OnClickSubmit;
            Nick = rc.Get<GameObject>("Nick");
            Nick.SetActive(false);
            textNick = rc.Get<GameObject>("Text_Nick").GetComponent<Text>();

            inputfieldID.onEndEdit.AddListener(InputIDEndEdited);
            inputfieldID.onValueChanged.AddListener(OnChangeValueInput);
            inputfieldNum.onValueChanged.AddListener(OnChangeValueInput);
            inputfieldPassword.onValueChanged.AddListener(OnChangeValueInput);
            textPerformance = rc.Get<GameObject>("Text_Performance").GetComponent<Text>();
            UIEventListener.Get(rc.Get<GameObject>("TextForget")).onClick = ClickTextForget;

            if (UIMineModel.mInstance.walletMainDto != null)
            {
                textPerformance.text = StringHelper.ShowGold((int)UIMineModel.mInstance.walletMainDto.plCount);
            }
        }

        private void ClickTextForget(GameObject go)
        {
            UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletSetting, UIType.UIMine_WalletTransfer + ":4");
        }

        private void InputIDEndEdited(string arg0)
        {
            if (inputfieldID.text.Trim().Length > 0)
            {
                //Log.Debug(inputfieldID.text);
                UIMineModel.mInstance.APIGetNickName(inputfieldID.text, pStr =>
                {
                    if (string.IsNullOrEmpty(pStr))
                    {
                        Nick.SetActive(false);
                    }
                    else
                    {
                        Nick.SetActive(true);
                        textNick.text = pStr;
                    }
                });
            }
            else
            {
                Nick.SetActive(false);
            }
        }


        private void OnChangeValueInput(string arg0)
        {
            if (MethodHelper.IsStrNotNull(new string[] { inputfieldID.text, inputfieldNum.text, inputfieldPassword.text }))
            {
                spriteCommit.sprite = mDicStrSprite["ImgRedBtn"];
                textCommit.color = new Color32(249, 243, 189, 255);
                // spriteCommit.SetNativeSize();
            }
            else
            {
                spriteCommit.sprite = mDicStrSprite["ImgGrayBtn"];
                textCommit.color = new Color32(175, 175, 175, 255);
                //spriteCommit.SetNativeSize();
            }
        }

        private void OnClickSubmit(GameObject go)
        {
            if (mIsCanClick == false) return;

            var id = inputfieldID.text;
            var num = inputfieldNum.text;
            var pw = inputfieldPassword.text;


            if (MethodHelper.IsStrNotNull(new string[] { id, num, pw }))
            {
                int nummm;
                if (int.TryParse(num, out nummm) == false)
                {
                    UIComponent.Instance.ToastLanguage("WalletTransferTT51");//Toast("单笔转USDT1000~5000000USDT");
                    return;
                }
                nummm = StringHelper.GetIntGold(nummm);
                if (nummm < 1000)
                {
                    UIComponent.Instance.ToastLanguage("WalletTransferTT52");//Toast("最低转1000豆");
                    return;
                }
                else if (nummm > 5000000)
                {
                    UIComponent.Instance.ToastLanguage("WalletTransferTT53");//Toast("最高转5000000豆");
                    return;
                }
                var tPlCount = UIMineModel.mInstance.walletMainDto.plCount;

                if (tPlCount < nummm)
                {
                    //var tNumIIInt = Mathf.CeilToInt((float)(nummm - tPlCount) / 200);
                    var tWalletSC = new UIMine_WalletServiceChargeComponent.WalletServiceCharge()
                    {
                        mSerChargeData = new long[] { nummm, tPlCount, 0 },
                        mIsTransfer = true,
                        mSureSuccAction = delegate
                        {
                            var tPwdMd5 = MD5Helper.StringMD5(pw);
                            UIMineModel.mInstance.APIWalletFlow(id, nummm, tPwdMd5, delegate
                            {
                                UIComponent.Instance.ToastLanguage("WalletTransferTT54");//WalletTransferTT51Toast("转USDT成功");
                                RemoveThisPanel();
                            });
                        }
                    };
                    UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletServiceCharge, tWalletSC);
                }
                else
                {
                    var tPwdMd5 = MD5Helper.StringMD5(pw);
                    UIMineModel.mInstance.APIWalletFlow(id, nummm, tPwdMd5, delegate
                    {
                        UIComponent.Instance.ToastLanguage("WalletTransferTT54");//WalletTransferTT51Toast("转USDT成功");
                        RemoveThisPanel();
                    });
                }
            }
            else
            {
                UIComponent.Instance.ToastLanguage("WalletTransferTT55");//Toast("请填完信息");
            }
        }
        bool mIsCanClick = true;
        async void RemoveThisPanel()
        {
            mIsCanClick = false;
            var tUI = UIComponent.Instance.Get(UIType.UIMine_WalletMy);
            if (tUI != null)
            {
                var tWalletMy = tUI.UiBaseComponent as UIMine_WalletMyComponent;
                tWalletMy.ShowRefresh();
            }
            await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(1000);
            UIComponent.Instance.Remove(UIType.UIMine_WalletTransfer);
        }

        #endregion




    }
}
