using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMatch_LoadingComponentSystem : AwakeSystem<UIMatch_LoadingComponent>
    {
        public override void Awake(UIMatch_LoadingComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名:加载中 </summary>
    public class UIMatch_LoadingComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
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
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();           
        }  
    }
}


