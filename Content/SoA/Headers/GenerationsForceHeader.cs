using CSE.Content.SoA.Accessories.Forces;
using FargowiltasSouls.Core.Toggler.Content;
using Terraria.ModLoader;

namespace CSE.Content.SoA.Headers
{
    public class GenerationsForceHeader : SoulHeader
    {
        public override float Priority => 6.6f;
        public override int Item => ModContent.ItemType<GenerationsForce>();
    }
}
