using CSE.Content.SoA.Accessories.Souls;
using FargowiltasSouls.Core.Toggler.Content;
using Terraria.ModLoader;

namespace CSE.Content.SoA.Headers
{
    public class CreationsHeader : SoulHeader
    {
        public override float Priority => 16.1f;
        public override int Item => ModContent.ItemType<SoulOfCreations>();
    }
}
