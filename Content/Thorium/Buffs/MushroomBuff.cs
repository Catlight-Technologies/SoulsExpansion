using Terraria.ModLoader;
using Terraria;
using CSE.Core;

namespace CSE.Content.Thorium.Buffs
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class MushroomBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 0.1f;
            player.statDefense -= 10;
        }
    }
}
