using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ETModel;
using UnityEngine;
using UnityEngine.UI;


namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_WalletTransBeansBindingAlipayComponentSystem : AwakeSystem<UIMine_WalletTransBeansBindingAlipayComponent>
    {
        public override void Awake(UIMine_WalletTransBeansBindingAlipayComponent self)
        {
            self.Awake();
        }
    }



    /// <summary> 页面名: 我的钱包 提取 绑定支付宝</summary>
    public class UIMine_WalletTransBeansBindingAlipayComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Dictionary<string, Sprite> mDicStrSprite;
        private Text textCommit;
        private InputField inputFieldAlipayName;
        private InputField inputFieldAlipayNum;
        private Image imageCommit;

        public void Awake()
        {
            mModifyOldData = null;
            InitUI();
            mDicStrSprite = new Dictionary<string, Sprite>() {
                { "ImgRedBtn",rc.Get<Sprite>("ImgRedBtn")},
                { "ImgGrayBtn",rc.Get<Sprite>("ImgGrayBtn")}
            };
        }
        /// <summary>
        /// 0 用户名    1支付宝账号
        /// </summary>
        string[] mModifyOldData;

        public override void OnShow(object obj)
        {
            if (obj != null)
            {
                UIMineModel.mInstance.APIAccountPayeInfo(pDto =>
                {
                    inputFieldAlipayName.text = pDto.alipayRealName;
                    inputFieldAlipayNum.text = pDto.alipayAcc;
                    mModifyOldData = new string[] { pDto.alipayRealName, pDto.alipayAcc };
                });
                SetUpNav(LanguageManager.mInstance.GetLanguageForKey("BindingAlipayMoidify"), UIType.UIMine_WalletTransBeansBindingAlipay);
                textCommit.text = LanguageManager.mInstance.GetLanguageForKey("WalletBinding11");// "立即修改";
            }
            else
            {
                SetUpNav(LanguageManager.mInstance.GetLanguageForKey("BindingAlipay"), UIType.UIMine_WalletTransBeansBindingAlipay);
                textCommit.text = LanguageManager.mInstance.GetLanguageForKey("WalletBinding12");//"立即绑定";
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
            var tCommit = rc.Get<GameObject>("Button_Commit");
            UIEventListener.Get(tCommit).onClick = ClickCommit;
            imageCommit = tCommit.GetComponent<Image>();
            textCommit = rc.Get<GameObject>("Text_Commit").GetComponent<Text>();
            inputFieldAlipayName = rc.Get<GameObject>("InputField_AlipayName").GetComponent<InputField>();
            inputFieldAlipayNum = rc.Get<GameObject>("InputField_AlipayNum").GetComponent<InputField>();
            inputFieldAlipayName.onEndEdit.AddListener(OnInputModify);
            inputFieldAlipayNum.onEndEdit.AddListener(OnInputModify);
        }

        private void OnInputModify(string arg0)
        {
            var tUser = inputFieldAlipayName.text.Trim();
            var tNum = inputFieldAlipayNum.text.Trim();
            if (mModifyOldData == null)//添加绑定的
            {
                if (MethodHelper.IsStrNotNull(new string[] { tUser, tNum }))
                {
                    SetRed();
                }
                else
                {
                    SetGray();
                }
            }
            else
            {//修改信息的
                if (MethodHelper.IsStrNotNull(new string[] { tUser, tNum }))
                {
                    if (tUser != mModifyOldData[0] || tNum != mModifyOldData[1])
                    {
                        SetRed();
                    }
                    else
                    {
                        SetGray(); ;
                    }
                }
                else
                {
                    SetGray();
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


        private void ClickCommit(GameObject go)
        {
            var tUser = inputFieldAlipayName.text.Trim();
            var tNum = inputFieldAlipayNum.text.Trim();

            if (MethodHelper.IsStrNotNull(new string[] { tUser, tNum }) == false)
            {
                UIComponent.Instance.ToastLanguage("WalletBinding13");//Toast("请选填入信息");
                return;
            }

            if (UIMineModel.mInstance.IsNumLetter(inputFieldAlipayNum.text) == false && UIMineModel.mInstance.IsEmail(inputFieldAlipayNum.text) == false)
            {
                UIComponent.Instance.ToastLanguage("WalletBinding14");//Toast("请输入正确的支付宝账号");
                return;
            }

            if (UIMineModel.mInstance.IsTextShow(inputFieldAlipayName.text) == false)
            {
                UIComponent.Instance.ToastLanguage("WalletBinding15");//Toast("请输入正确的用户名");
                return;
            }

            if (mModifyOldData != null && tUser == mModifyOldData[0] && tNum == mModifyOldData[1])
            {
                UIComponent.Instance.ToastLanguage("WalletBinding16");//Toast("您并没有修改信息");
                return;
            }

            UIMineModel.mInstance.APIAccountPayeeSaveAlipay(tUser, tNum, delegate
        {
            if (mModifyOldData != null && mModifyOldData.Length == 2)
            {
                UIComponent.Instance.ToastLanguage("WalletBinding17");//Toast("提款账号信息正在审核中");
            }
            else
            {
                UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletTransBeansAlipay);
            }
            UIComponent.Instance.Remove(UIType.UIMine_WalletTransBeansBindingAlipay);
        });
        }
    }
}

