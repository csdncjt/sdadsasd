using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.ResOffLineDispose)]
    public class ResOffLine_Dispose : AEvent
    {
        public override void Run()
        {
            UI mUI = UIComponent.Instance.Get(UIType.UITexas);
            if (null != mUI)
            {
                UITexasComponent mUITexasComponent = mUI.UiBaseComponent as UITexasComponent;
                if (null != mUITexasComponent)
                {
                    mUITexasComponent.isOffline = true;
                }
            }
        }
    }
}
