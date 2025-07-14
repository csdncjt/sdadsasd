using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.Hongbaoyu_Dispatch)]
    public class Hongbaoyu_Dispatch : AEvent<int, string>
    {
        public override void Run(int userId, string content)
        {
            UI mUI = UIComponent.Instance.Get(UIType.UIHongBaoYu);
            if (null != mUI)
            {
                UIHongBaoYuComponent mUIHongBaoYuComponent = mUI.UiBaseComponent as UIHongBaoYuComponent;
                if (null != mUIHongBaoYuComponent)
                {
                    mUIHongBaoYuComponent.Play(userId, content);
                }
            }
        }
    }

    [Event(EventIdType.Hongbaoyu_Stop)]
    public class Hongbaoyu_Stop: AEvent<int>
    {
        public override void Run(int userId)
        {
            UI mUI = UIComponent.Instance.Get(UIType.UIHongBaoYu);
            if (null != mUI)
            {
                UIHongBaoYuComponent mUIHongBaoYuComponent = mUI.UiBaseComponent as UIHongBaoYuComponent;
                if (null != mUIHongBaoYuComponent)
                {
                    mUIHongBaoYuComponent.Stop(userId);
                }
            }
        }
    }

    [Event(EventIdType.Hongbaoyu_AllStop)]
    public class Hongbaoyu_AllStop: AEvent
    {
        public override void Run()
        {
            UI mUI = UIComponent.Instance.Get(UIType.UIHongBaoYu);
            if (null != mUI)
            {
                UIHongBaoYuComponent mUIHongBaoYuComponent = mUI.UiBaseComponent as UIHongBaoYuComponent;
                if (null != mUIHongBaoYuComponent)
                {
                    mUIHongBaoYuComponent.AllStop();
                }
            }
        }
    }
}
