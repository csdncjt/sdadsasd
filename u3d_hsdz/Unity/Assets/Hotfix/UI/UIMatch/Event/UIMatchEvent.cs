using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.UIMatch_RefreshList)]
    public class UIMatch_RefreshList : AEvent
    {
        public override void Run()
        {
            var tUI = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIMatch);
            if (tUI != null && tUI.GameObject != null && tUI.GameObject.activeInHierarchy)
            {
                var tMatch = tUI.UiBaseComponent as UIMatchComponent;
                tMatch.NewLoadPanel();
            }
        }
    }


}
