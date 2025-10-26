using CSE.Content.Thorium.Accessories.Other;
using FargowiltasSouls.Core.Toggler.Content;
using Terraria.ModLoader;

namespace CSE.Content.Thorium.Headers
{
    public class MotDEHeader : SoulHeader
    {
        public override float Priority => 12.1f;
        public override int Item => ModContent.ItemType<MotDE>();
    }
}
