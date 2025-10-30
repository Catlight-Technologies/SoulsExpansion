using ContinentOfJourney;
using FargowiltasSouls.Content.Bosses.AbomBoss;
using FargowiltasSouls.Content.Bosses.Champions.Cosmos;
using FargowiltasSouls.Content.Bosses.MutantBoss;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CSE.Core.HWJ.ModPlayers
{
    [ExtendsFromMod(ModCompatibility.Homeward.Name)]
    [JITWhenModsEnabled(ModCompatibility.Homeward.Name)]
    public class HWJModPlayer : ModPlayer
    {
        public override void PostUpdateEquips()
        {
            if (NPC.AnyNPCs(NPCType<MutantBoss>()) || NPC.AnyNPCs(NPCType<CosmosChampion>()) || NPC.AnyNPCs(NPCType<AbomBoss>()))
            {
                Player.GetModPlayer<TemplatePlayer>().GrindStone_Type = 0;
                Player.GetModPlayer<TemplatePlayer>().GrindStone_Time = 0;
            }
        }
    }
}
