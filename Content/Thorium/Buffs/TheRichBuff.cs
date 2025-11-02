using CSE.Core;
using Terraria;
using Terraria.ModLoader;

namespace CSE.Content.Thorium.Buffs
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class TheRichBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 0.05f;
            player.GetDamage(DamageClass.Generic) += 0.1f;
        }
    }
}
