using CSE.Content.SoA.Accessories.Forces;
using FargowiltasSouls.Core.Toggler.Content;
using Terraria.ModLoader;

namespace CSE.Content.SoA.Headers
{
    public class FrostburnForceHeader : SoulHeader
    {
        public override float Priority => 6.8f;
        public override int Item => ModContent.ItemType<FrostburnForce>();
    }
}
