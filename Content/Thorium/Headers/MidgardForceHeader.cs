using FargowiltasSouls.Core.Toggler.Content;
using Terraria.ModLoader;
using CSE.Content.Thorium.Forces;

namespace CSE.Content.Thorium.Headers
{
    public class MidgardForceHeader : SoulHeader
    {
        public override float Priority => 70.2f;
        public override int Item => ModContent.ItemType<MidgardForce>();
    }
}
