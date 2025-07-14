using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_SettingNoticeComponentSystem : AwakeSystem<UIMine_SettingNoticeComponent>
    {
        public override void Awake(UIMine_SettingNoticeComponent self)
        {
            self.Awake();
        }
    }

    public class UIMine_SettingNoticeComponent : UIBaseComponent
    {

        private ReferenceCollector rc;

        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            SetUpNav( UIType.UIMine_SettingNotice);
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
        }



        #endregion


    }
}

