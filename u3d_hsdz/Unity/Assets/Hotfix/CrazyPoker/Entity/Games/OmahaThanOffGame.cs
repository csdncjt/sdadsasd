using System.Collections.Generic;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class OmahaThanOffGameAwakeSystem : AwakeSystem<OmahaThanOffGame, Component>
    {
        public override void Awake(OmahaThanOffGame self, Component component)
        {
            self.Awake(component);
        }
    }

    [ObjectSystem]
    public class OmahaThanOffGameUpdateSystem : UpdateSystem<OmahaThanOffGame>
    {
        public override void Update(OmahaThanOffGame self)
        {
            self.Update();
        }
    }

    public partial class OmahaThanOffGame : OmahaGame
    {
        protected override string GetRoomTypeDes()
        {
            // return "奥马哈(必下场)";
            return CPErrorCode.LanguageDescription(20031);
        }

    }
}
