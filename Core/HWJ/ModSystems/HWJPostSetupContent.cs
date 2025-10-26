using ContinentOfJourney.NPCs.Boss_TheSon;
using System.Collections.Generic;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static CSE.Core.Common.Globals.CSEPointsBalanceNPC;

namespace CSE.Core.HWJ.ModSystems
{
    [ExtendsFromMod(ModCompatibility.Homeward.Name)]
    [JITWhenModsEnabled(ModCompatibility.Homeward.Name)]
    public class HWJPostSetupContent : ModSystem
    {
        public override void Load()
        {
            AddBossConfig(
                bossType: NPCType<TheSon>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0.3f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f },
                new ModMultiplier { ModName = "FargowiltasSouls", DamageMultiplier = 0.1f, HealthMultiplier = 2f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f }
                }
            );
        }
    }
}
