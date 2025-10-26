using FargowiltasSouls;
using Redemption.Items.Placeable.Furniture.Lab;
using Redemption.Items.Placeable.Furniture.PetrifiedWood;
using Redemption.NPCs.Bosses.Cleaver;
using Redemption.NPCs.Bosses.Gigapora;
using Redemption.NPCs.Bosses.Obliterator;
using Redemption.NPCs.Bosses.PatientZero;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using FargowiltasSouls.Core.Systems;
using Terraria;
using FargowiltasSouls.Core.Globals;

namespace CSE.Core.MoR.Globals
{
    [ExtendsFromMod(ModCompatibility.Redemption.Name)]
    [JITWhenModsEnabled(ModCompatibility.Redemption.Name)]
    public class MoRGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (WorldSavingSystem.EternityMode)
            {
                LeadingConditionRule firstKillRule = new(new FirstKillCondition());
                npcLoot.Add(firstKillRule);

                if (npc.type == ModContent.NPCType<PZ>())
                {
                    firstKillRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ModContent.ItemType<PetrifiedCrate>(), 5));
                }
                else if (npc.type == ModContent.NPCType<OmegaCleaver>() || npc.type == ModContent.NPCType<Gigapora>())
                {
                    firstKillRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ModContent.ItemType<LabCrate>(), 5));
                }
                else if (npc.type == ModContent.NPCType<OO>())
                {
                    firstKillRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ModContent.ItemType<LabCrate2>(), 5));
                }
            }
        }
    }
}