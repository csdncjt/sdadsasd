using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_ShardProblemComponentSystem : AwakeSystem<UIMine_ShardProblemComponent>
    {
        public override void Awake(UIMine_ShardProblemComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 常见问题
    /// </summary>
    public class UIMine_ShardProblemComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private RawImage RawPng;
        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_ShareProblem);
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
            RawPng = rc.Get<GameObject>("RawImage").GetComponent<RawImage>();
            UIMatchModel.mInstance.APIGetBanner( 5, 0, pDto =>
            {
                WebImageHelper.SetHttpBanner(RawPng, pDto[0].bannerImg, delegate
                         {
                             RawPng.color = new Color32(255, 255, 255, 255);
                         });
            });
        }

        #endregion




    }
}
