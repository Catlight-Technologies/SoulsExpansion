using FargowiltasSouls.Core.Toggler.Content;
using Terraria.ModLoader;
using CSE.Content.Thorium.Forces;

namespace CSE.Content.Thorium.Headers
{
    public class NiflheimForceHeader : SoulHeader
    {
        public override float Priority => 70.4f;
        public override int Item => ModContent.ItemType<NiflheimForce>();
    }
}
