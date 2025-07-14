using System.Collections.Generic;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class OmahaAofGameAwakeSystem : AwakeSystem<OmahaAofGame, Component>
    {
        public override void Awake(OmahaAofGame self, Component component)
        {
            self.Awake(component);
        }
    }

    [ObjectSystem]
    public class OmahaAofGameUpdateSystem : UpdateSystem<OmahaAofGame>
    {
        public override void Update(OmahaAofGame self)
        {
            self.Update();
        }
    }

    public partial class OmahaAofGame : OmahaGame
    {
        protected override string GetRoomTypeDes()
        {
            // return "奥马哈(AOF)";
            return CPErrorCode.LanguageDescription(20033);
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
