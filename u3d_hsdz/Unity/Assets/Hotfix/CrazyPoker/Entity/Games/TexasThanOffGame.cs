using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class TexasThanOffGameAwakeSystem : AwakeSystem<TexasThanOffGame, Component>
    {
        public override void Awake(TexasThanOffGame self, Component component)
        {
            self.Awake(component);
        }
    }

    [ObjectSystem]
    public class TexasThanOffGameUpdateSystem : UpdateSystem<TexasThanOffGame>
    {
        public override void Update(TexasThanOffGame self)
        {
            self.Update();
        }
    }

    public partial class TexasThanOffGame : TexasGame
    {
        protected override string GetRoomTypeDes()
        {
            // return "德州(必下场)";
            return CPErrorCode.LanguageDescription(20029);
        }
    }
}
