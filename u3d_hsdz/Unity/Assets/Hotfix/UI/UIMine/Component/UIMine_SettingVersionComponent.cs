using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_SettingVersionComponentSystem : AwakeSystem<UIMine_SettingVersionComponent>
    {
        public override void Awake(UIMine_SettingVersionComponent self)
        {
            self.Awake();
        }
    }

    public class UIMine_SettingVersionComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Image imageicon;
        private Text texttitle;

        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            SetUpNav( UIType.UIMine_SettingVersion);

            texttitle.text = $"V {UnityEngine.Application.version}";
        }

        public override void OnHide()
        {

        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            base.Dispose();
        }

        #region UI
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            imageicon = rc.Get<GameObject>("Image_icon").GetComponent<Image>();
            texttitle = rc.Get<GameObject>("Text_title").GetComponent<Text>();

        }



        #endregion


    }
}

