using CSE.Content.SoA.Accessories.Forces;
using FargowiltasSouls.Core.Toggler.Content;
using Terraria.ModLoader;

namespace CSE.Content.SoA.Headers
{
    public class SoranForceHeader : SoulHeader
    {
        public override float Priority => 6.4f;
        public override int Item => ModContent.ItemType<SoranForce>();
    }
}
