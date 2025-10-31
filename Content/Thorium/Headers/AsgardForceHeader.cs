using FargowiltasSouls.Core.Toggler.Content;
using Terraria.ModLoader;
using CSE.Content.Thorium.Forces;

namespace CSE.Content.Thorium.Headers
{
    public class AsgardForceHeader : SoulHeader
    {
        public override float Priority => 60.9f;
        public override int Item => ModContent.ItemType<AsgardForce>();
    }
}
