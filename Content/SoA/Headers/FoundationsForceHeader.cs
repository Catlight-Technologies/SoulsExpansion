using CSE.Content.SoA.Accessories.Forces;
using FargowiltasSouls.Core.Toggler.Content;
using Terraria.ModLoader;

namespace CSE.Content.SoA.Headers
{
    public class FoundationsForceHeader : SoulHeader
    {
        public override float Priority => 6.7f;
        public override int Item => ModContent.ItemType<FoundationsForce>();
    }
}
