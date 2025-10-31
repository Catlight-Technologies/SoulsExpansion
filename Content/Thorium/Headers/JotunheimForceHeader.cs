using FargowiltasSouls.Core.Toggler.Content;
using Terraria.ModLoader;
using CSE.Content.Thorium.Forces;

namespace CSE.Content.Thorium.Headers
{
    public class JotunheimForceHeader : SoulHeader
    {
        public override float Priority => 70.1f;
        public override int Item => ModContent.ItemType<JotunheimForce>();
    }
}
