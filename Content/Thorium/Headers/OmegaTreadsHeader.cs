using CSE.Content.Thorium.Accessories.Other;
using FargowiltasSouls.Core.Toggler.Content;
using Terraria.ModLoader;

namespace CSE.Content.Thorium.Headers
{
    public class OmegaTreadsHeader : SoulHeader
    {
        public override float Priority => 12.2f;
        public override int Item => ModContent.ItemType<OmegaTreads>();
    }
}
