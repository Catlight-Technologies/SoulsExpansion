using FargowiltasSouls.Content.Bosses.AbomBoss;
using FargowiltasSouls.Content.Bosses.Champions.Cosmos;
using FargowiltasSouls.Content.Bosses.MutantBoss;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CSE.Core.Common.Globals
{
    public class CSEPointsBalanceNPC : GlobalNPC
    {
        public static List<BossConfig> BossConfigs = new List<BossConfig>();
        public class BossConfig
        {
            public int BossType;
            public float DamageMultiplier;
            public float HealthMultiplier;
            public float DamageReductionCSE;
            public void CalculateMultipliers()
            {
                float damageMultiplier = 0f;
                float healthMultiplier = 0f;

                foreach (var mod in AffectingMods)
                {
                    if (ModLoader.HasMod(mod.ModName))
                    {
                        damageMultiplier += mod.DamageMultiplier;
                        healthMultiplier += mod.HealthMultiplier;
                    }
                }

                DamageMultiplier = damageMultiplier;
                HealthMultiplier = healthMultiplier;
            }

            public List<ModMultiplier> AffectingMods = new List<ModMultiplier>();
        }
        public class ModMultiplier
        {
            public string ModName;
            public float DamageMultiplier;
            public float HealthMultiplier;
        }
        public override void SetDefaults(NPC npc)
        {
            foreach (var config in BossConfigs)
            {
                if (npc.type == config.BossType)
                {
                    config.CalculateMultipliers();

                    npc.damage += (int)(npc.damage * config.DamageMultiplier);
                    npc.lifeMax += (int)(npc.lifeMax * config.HealthMultiplier);
                    break;
                }
            }
        }
        public static void AddBossConfig(int bossType, List<ModMultiplier> affectingMods)
        {
            var config = new BossConfig
            {
                BossType = bossType,
                AffectingMods = affectingMods
            };

            config.CalculateMultipliers();
            BossConfigs.Add(config);
        }
        public override void Load()
        {
            AddBossConfig(
                bossType: ModContent.NPCType<MutantBoss>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalLegacy", DamageMultiplier = 10f, HealthMultiplier = 100f }, //ech
                new ModMultiplier { ModName = "HeavenlyArsenal", DamageMultiplier = 10f, HealthMultiplier = 100f }, //ech
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 1f, HealthMultiplier = 2.5f },
                new ModMultiplier { ModName = "SacredTools", DamageMultiplier = 0.5f, HealthMultiplier = 0.75f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.3f, HealthMultiplier = 0.5f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.3f, HealthMultiplier = 0.5f }
                }
            );

            AddBossConfig(
                bossType: ModContent.NPCType<AbomBoss>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0.5f, HealthMultiplier = 1.5f },
                new ModMultiplier { ModName = "SacredTools", DamageMultiplier = 0.25f, HealthMultiplier = 0.75f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.25f, HealthMultiplier = 0.3f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.25f, HealthMultiplier = 0.5f }
                }
            );

            AddBossConfig(
                bossType: ModContent.NPCType<CosmosChampion>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0.75f, HealthMultiplier = 1.5f },
                new ModMultiplier { ModName = "SacredTools", DamageMultiplier = 0.25f, HealthMultiplier = 0.25f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0f, HealthMultiplier = 0f }, //no bosses before eridanus
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.25f, HealthMultiplier = 0.25f }
                }
            );
        }
    }
}