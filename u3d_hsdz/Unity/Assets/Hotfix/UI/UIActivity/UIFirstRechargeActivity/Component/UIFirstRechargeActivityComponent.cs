using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIFirstRechargeActivityComponentSystem : AwakeSystem<UIFirstRechargeActivityComponent>
    {
        public override void Awake(UIFirstRechargeActivityComponent self)
        {
            self.Awake();
        }
    }

    public class UIFirstRechargeActivityComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Button buttonRecharge;
        private Image imageContent;
        private Button buttonClose;
        private Image imageTitle;

        private static readonly string[] contents = new string[]{ "scyl_zt_01", "scyl_zt_03", "scyl_zt_02" };
        private static readonly string[] titles = new string[]{ "scyl_fzt_01", "scyl_fzt_03", "scyl_fzt_02" };

        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            buttonRecharge = rc.Get<GameObject>("Button_Recharge").GetComponent<Button>();
            imageContent = rc.Get<GameObject>("Image_Content").GetComponent<Image>();
            buttonClose = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            imageTitle = rc.Get<GameObject>("Image_Title").GetComponent<Image>();

            UIEventListener.Get(this.buttonClose.gameObject).onClick = onClickClose;
            UIEventListener.Get(this.buttonRecharge.gameObject).onClick = onClickRecharge;
        }

        private void onClickRecharge(GameObject go)
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
            // UIComponent.Instance.ShowNoAnimation(UIType.UIMine_WalletAddBeansList);
            UIComponent.Instance.Remove(UIType.UIFirstRechargeActivity);
        }

        private void onClickClose(GameObject go)
        {
            UIComponent.Instance.Remove(UIType.UIFirstRechargeActivity);
        }

        public override void OnShow(object param)
        {
            int mTmpLanguage = 2;
            int mTmpCurLanguage = LanguageManager.mInstance.mCurLanguage;
            if (mTmpCurLanguage <= contents.Length)
                mTmpLanguage = mTmpCurLanguage;
            string mContent = contents[mTmpLanguage];
            string mTitle = titles[mTmpLanguage];
            imageContent.sprite = this.rc.Get<Sprite>(mContent);
            imageTitle.sprite = this.rc.Get<Sprite>(mTitle);
        }
    }
}
