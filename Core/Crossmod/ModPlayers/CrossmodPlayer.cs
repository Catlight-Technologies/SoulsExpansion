using CalamityMod;
using CalamityMod.Buffs.Summon;
using FargowiltasSouls.Content.Bosses.MutantBoss;
using FargowiltasSouls.Content.Buffs.Boss;
using Terraria;
using Terraria.ModLoader;

namespace CSE.Core.Crossmod.ModPlayers
{
    [ExtendsFromMod(ModCompatibility.Crossmod.Name)]
    [JITWhenModsEnabled(ModCompatibility.Crossmod.Name)]
    public class CrossmodPlayer : ModPlayer
    {
        public override void PostUpdateBuffs()
        {
            if (Player.HasBuff<MutantPresenceBuff>() && NPC.AnyNPCs(ModContent.NPCType<MutantBoss>()))
            {
                Player.buffImmune[ModContent.Find<ModBuff>("CalamityMod", "Enraged").Type] = true;
                Player.buffImmune[ModContent.Find<ModBuff>("CalamityMod", "RageMode").Type] = true;
                Player.buffImmune[ModContent.Find<ModBuff>("CalamityMod", "AdrenalineMode").Type] = true;
            }
            if (Player.HasBuff<DemonshadeSetDevilBuff>())
            {
                Player.Calamity().wearingRogueArmor = true;
                Player.Calamity().rogueStealthMax += 1.5f;
            }
        }
    }
}
