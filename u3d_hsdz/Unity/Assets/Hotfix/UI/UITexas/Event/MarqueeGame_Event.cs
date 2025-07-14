using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.TexasMarqueeGame_Dispatch)]
    public class MarqueeGame_Dispatch : AEvent<int, string>
    {
        public override void Run(int userId, string content)
        {
            if (null != GameCache.Instance.CurGame)
            {
                GameCache.Instance.CurGame.PlayMarqueeGameByUserId(userId, content);
            }
        }
    }
}
