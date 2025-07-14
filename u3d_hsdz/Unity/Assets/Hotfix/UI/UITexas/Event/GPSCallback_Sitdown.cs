using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.GPSCallbackSitdown)]
    public class GPSCallback_Sitdown : AEvent<string>
    {
        public override void Run(string status)
        {
            //获取GPS回调成功后，通知活动上报
            UI uI = UIComponent.Instance.Get(UIType.UIMatch);
            if (null != uI)
            {
                UIMatchComponent match = uI.UiBaseComponent as UIMatchComponent;
                match.OnGPSCallBack(status);
            }

            UIMatchMTTModel.mInstance.OnGPSCallBack(status);

            //牌局内回调
            if (null != GameCache.Instance.CurGame)
            {
                if (status == "0")
                {
                    if (GameCache.Instance.CurGame.waittingGPSCallback)
                    {
                        GameCache.Instance.CurGame.GPSCallback_Sitdown();
                    }
                }
                GameCache.Instance.CurGame.waittingGPSCallback = false;
            }
        }
    }
}
