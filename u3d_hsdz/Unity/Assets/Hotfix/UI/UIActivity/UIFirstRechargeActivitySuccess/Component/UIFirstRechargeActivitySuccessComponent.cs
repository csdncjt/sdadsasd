using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIFirstRechargeActivitySuccessComponentSystem : AwakeSystem<UIFirstRechargeActivitySuccessComponent>
    {
        public override void Awake(UIFirstRechargeActivitySuccessComponent self)
        {
            self.Awake();
        }
    }

    public class UIFirstRechargeActivitySuccessComponent : UIBaseComponent
    {
        private static readonly string[] contents = new string[] { "sccg_zt_01", "sccg_zt_03", "sccg_zt_02" };
        private static readonly string[] titles = new string[] { "sccg_fzt_01", "sccg_fzt_03", "sccg_fzt_02" };

        private ReferenceCollector rc;
        private Button buttonClose;
        private Button buttonRoll;
        private Image imageTitle;
        private Image imageContent;

        private int num;


        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            buttonClose = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            buttonRoll = rc.Get<GameObject>("Button_Roll").GetComponent<Button>();
            imageTitle = rc.Get<GameObject>("Image_Title").GetComponent<Image>();
            imageContent = rc.Get<GameObject>("Image_Content").GetComponent<Image>();

            UIEventListener.Get(buttonClose.gameObject).onClick = onClickClose;
            UIEventListener.Get(buttonRoll.gameObject).onClick = onClickRoll;
        }

        private void onClickRoll(GameObject go)
        {
            UIMatchModel.mInstance.APIActivityRechargeGain();
            if (num >= 0)
                UIComponent.Instance.ShowNoAnimation(UIType.UIYaoDouEffect, num.ToString());
            UIComponent.Instance.Remove(UIType.UIFirstRechargeActivitySuccess);
        }

        private void onClickClose(GameObject go)
        {
            UIComponent.Instance.Remove(UIType.UIFirstRechargeActivitySuccess);
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

            if (null != param)
            {
                num = (int) param;
            }
            else
            {
                num = -1;
            }
        }
    }
}
