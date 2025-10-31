using FargowiltasSouls.Core.Toggler.Content;
using Terraria.ModLoader;
using CSE.Content.Thorium.Forces;

namespace CSE.Content.Thorium.Headers
{
    public class HelheimForceHeader : SoulHeader
    {
        public override float Priority => 70f;
        public override int Item => ModContent.ItemType<HelheimForce>();
    }
}
