using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.GameErrorReconnect)]
    public class GameError_Reconnect : AEvent
    {
        public override void Run()
        {
            if (null != UIComponent.Instance)
            {
                UI mUI = UIComponent.Instance.Get(UIType.UITexas);
                if (null != mUI && mUI.GameObject.activeInHierarchy)
                {
                    NetworkDetectionComponent mNetworkDetectionComponent = Game.Scene.GetComponent<NetworkDetectionComponent>();
                    if(null != mNetworkDetectionComponent)
                        mNetworkDetectionComponent.Reconnect();
                }
            }
        }
    }
}
