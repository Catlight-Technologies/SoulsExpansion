using Redemption.NPCs.Bosses.ADD;
using Redemption.NPCs.Bosses.Neb;
using Redemption.NPCs.Bosses.Neb.Clone;
using Redemption.NPCs.Bosses.Neb.Phase2;
using Redemption.NPCs.Bosses.Obliterator;
using Redemption.NPCs.Bosses.PatientZero;
using System.Collections.Generic;
using Terraria.ModLoader;
using static CSE.Core.Common.Globals.CSEPointsBalanceNPC;
using static Terraria.ModLoader.ModContent;

namespace CSE.Core.MoR.ModSystems
{
    [ExtendsFromMod(ModCompatibility.Redemption.Name)]
    [JITWhenModsEnabled(ModCompatibility.Redemption.Name)]
    public class MoRPostSetupContent : ModSystem
    {
        public override void Load()
        {
            //nebuleus 
            AddBossConfig(
                bossType: NPCType<Nebuleus>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0.3f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f },
                new ModMultiplier { ModName = "FargowiltasSouls", DamageMultiplier = 0.1f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f }
                }
            );
            AddBossConfig(
                bossType: NPCType<Nebuleus2>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0.3f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f },
                new ModMultiplier { ModName = "FargowiltasSouls", DamageMultiplier = 0.1f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f }
                }
            );
            AddBossConfig(
                bossType: NPCType<Nebuleus_Clone>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0.3f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f },
                new ModMultiplier { ModName = "FargowiltasSouls", DamageMultiplier = 0.1f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f }
                }
            );
            AddBossConfig(
                bossType: NPCType<Nebuleus2_Clone>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0.3f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f },
                new ModMultiplier { ModName = "FargowiltasSouls", DamageMultiplier = 0.1f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f }
                }
            );

            //obliterator
            AddBossConfig(
                bossType: NPCType<OO>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0.3f, HealthMultiplier = 2f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f },
                new ModMultiplier { ModName = "FargowiltasSouls", DamageMultiplier = 0.1f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f }
                }
            );

            //ukko & akka
            AddBossConfig(
                bossType: NPCType<Ukko>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0.3f, HealthMultiplier = 2f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f },
                new ModMultiplier { ModName = "FargowiltasSouls", DamageMultiplier = 0.1f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f }
                }
            );
            AddBossConfig(
                bossType: NPCType<Akka>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0.3f, HealthMultiplier = 2f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f },
                new ModMultiplier { ModName = "FargowiltasSouls", DamageMultiplier = 0.1f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f }
                }
            );

            //patient zero
            AddBossConfig(
                bossType: NPCType<PZ_Kari>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0.3f, HealthMultiplier = 2f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f },
                new ModMultiplier { ModName = "FargowiltasSouls", DamageMultiplier = 0.1f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f }
                }
            );
            AddBossConfig(
                bossType: NPCType<PZ>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0.3f, HealthMultiplier = 2f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f },
                new ModMultiplier { ModName = "FargowiltasSouls", DamageMultiplier = 0.1f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f }
                }
            );
        }
    }
}
