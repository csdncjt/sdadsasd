using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class TexasAofGameAwakeSystem : AwakeSystem<TexasAofGame, Component>
    {
        public override void Awake(TexasAofGame self, Component component)
        {
            self.Awake(component);
        }
    }

    [ObjectSystem]
    public class TexasAofGameUpdateSystem : UpdateSystem<TexasAofGame>
    {
        public override void Update(TexasAofGame self)
        {
            self.Update();
        }
    }

    public partial class TexasAofGame : TexasGame
    {
        protected override string GetRoomTypeDes()
        {
            // return "德州(AOF)";
            if (roomType == 23)
            {
                return CPErrorCode.LanguageDescription(20188);
            }
            return CPErrorCode.LanguageDescription(20030); 
        }

        /// <summary>
        /// 最小可玩的筹码，少于等于此数需要带入才能玩
        /// </summary>
        protected override int GetMinPlayChips()
        {
            return currentMinRate * GameCache.Instance.carry_small - 1;
        }
    }
}
