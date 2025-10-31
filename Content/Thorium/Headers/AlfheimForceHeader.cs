using CSE.Content.Thorium.Forces;
using FargowiltasSouls.Core.Toggler.Content;
using Terraria.ModLoader;

namespace CSE.Content.Thorium.Headers
{
    public class AlfheimForceHeader : SoulHeader
    {
        public override float Priority => 60.8f;
        public override int Item => ModContent.ItemType<AlfheimForce>();
    }
}
