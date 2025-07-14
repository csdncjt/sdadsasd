using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_WalletMyComponentSystem : AwakeSystem<UIMine_WalletMyComponent>
    {
        public override void Awake(UIMine_WalletMyComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 我的钱包 主页面
    /// </summary>
    public class UIMine_WalletMyComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Text textWalletValue;
        private Text textCanUsing;
        private Text textNoUsing;
        private Text textRecord;

        private GameObject ProblemShow;
        public void Awake()
        {
            InitUI();
            InitLanguage();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_WalletMy);
            if (obj != null && obj is object[])//特定的 我的主页 进去的
            {
                var o = obj as object[];//[0]个人数据               [1]显示转USDT是否开启

                BtnTransferBeans.SetActive((bool)o[1]);
                ShowWalletNum();
            }

            ShowRefresh();
        }

        public GameObject BtnTransferBeans;

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
            ProblemShow = rc.Get<GameObject>("ProblemShow");
            ProblemShow.SetActive(false);

            //充值
            UIEventListener.Get(rc.Get<GameObject>("BtnMyRecharge")).onClick = go =>
            {
                UIMineModel.mInstance.APIGetClubId(tDtoHasClub =>
                {
                    if (tDtoHasClub)
                    {
                        UIMineModel.mInstance.APIClubTransType(pDto =>
                        {
                            UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletAddBeansList, pDto);
                        });
                    }
                    else
                    {
                        UIComponent.Instance.ToastLanguage("WalletMy11");
                    }
                });
            };
            //转USDT
            BtnTransferBeans = rc.Get<GameObject>("BtnTransferBeans");
            UIEventListener.Get(BtnTransferBeans).onClick = go =>
            {
                if (UIMineModel.mInstance.mIsSecondPW)
                    UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletTransfer);
                else
                    UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletSetting, UIType.UIMine_WalletTransfer + ":3");
            };
            //提取
            UIEventListener.Get(rc.Get<GameObject>("BtnTransCash")).onClick = go =>
            {
                UIMineModel.mInstance.APIGetClubId(tDtoHasClub =>
                {
                    if (tDtoHasClub)
                    {
                        UIMineModel.mInstance.APIClubTransType(pAct =>
                        {//pAct=0俱乐部充值模式   1平台充值模式
                            if (pAct == 1)
                            {
                                UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletExtra);
                            }
                            else if (pAct == 0)
                            {//0俱乐部充值模式
                                UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletClubGetMoneyList);
                            }
                        });
                    }
                    else
                    {//未加入俱乐部
                        UIComponent.Instance.ToastLanguage("WalletMy11");
                    }
                });
            };
            //记录
            UIEventListener.Get(rc.Get<GameObject>("BtnCashFlow")).onClick = go =>
            {
                UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletFlow);
            };
            textWalletValue = rc.Get<GameObject>("Text_WalletValue").GetComponent<Text>();
            textCanUsing = rc.Get<GameObject>("Text_CanUsing").GetComponent<Text>();
            textNoUsing = rc.Get<GameObject>("Text_NoUsing").GetComponent<Text>();
            textRecord = rc.Get<GameObject>("Text_Record").GetComponent<Text>();

            textWalletValue.text = "";
            textCanUsing.text = "";
            textNoUsing.text = "";
            textRecord.text = "";

            UIEventListener.Get(rc.Get<GameObject>("Image_ProblemBtn")).onClick = ClickImage_ProblemBtn;
        }

        public void ShowRefresh()
        {
            UIMineModel.mInstance.ObtainUserInfo(pDto =>
            {
                ShowWalletNum();
            });
        }

        private void ShowWalletNum()
        {
            if (UIMineModel.mInstance.walletMainDto != null)
            {
                textWalletValue.text = StringHelper.ShowGold(UIMineModel.mInstance.walletMainDto.chip + UIMineModel.mInstance.walletMainDto.notExtractChip, false);
                textCanUsing.text = StringHelper.ShowGold(UIMineModel.mInstance.walletMainDto.chip, false);
                textNoUsing.text = StringHelper.ShowGold(UIMineModel.mInstance.walletMainDto.notExtractChip, false);
                textRecord.text = StringHelper.ShowGold((int)UIMineModel.mInstance.walletMainDto.plCount);
            }
        }



        private void ClickImage_ProblemBtn(GameObject go)
        {
            ProblemShow.SetActive(!ProblemShow.activeInHierarchy);
        }
        #endregion




    }
}
