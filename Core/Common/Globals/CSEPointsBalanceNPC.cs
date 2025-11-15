using CSE.Content.Common.Bosses.MutantEX;
using FargowiltasSouls.Content.Bosses.AbomBoss;
using FargowiltasSouls.Content.Bosses.Champions.Cosmos;
using FargowiltasSouls.Content.Bosses.MutantBoss;
using System.Collections.Generic;
using System.Linq;
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

        public static string GetBossConfigInfo()
        {
            var sb = new System.Text.StringBuilder();
            foreach (var config in BossConfigs)
            {
                sb.AppendLine($"Boss Type: {config.BossType}");
                sb.AppendLine($"Final Damage Multiplier: {config.DamageMultiplier}");
                sb.AppendLine($"Final Health Multiplier: {config.HealthMultiplier}");
                sb.AppendLine("Affecting Mods:");
                foreach (var mod in config.AffectingMods)
                {
                    sb.AppendLine($"  - {mod.ModName}: Dmg={mod.DamageMultiplier}, HP={mod.HealthMultiplier} (Loaded: {ModLoader.HasMod(mod.ModName)})");
                }
                sb.AppendLine();
            }
            return sb.ToString();
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

        /// <summary>
        /// Adds modifier to already existing entry
        /// </summary>
        /// <param name="bossType">Boss Internal Name</param>
        /// <param name="modName">Mod Internal Name</param>
        /// <param name="damageMultiplier">Damage Multiplier</param>
        /// <param name="healthMultiplier">Max Life Multiplier</param>
        /// <returns>True if multiplier was added. Flase if boss was not found</returns>
        public static bool AddAffectingMod(int bossType, string modName, float damageMultiplier, float healthMultiplier)
        {
            var config = BossConfigs.FirstOrDefault(c => c.BossType == bossType);
            if (config == null)
                return false;

            var existingMod = config.AffectingMods.FirstOrDefault(m => m.ModName == modName);
            if (existingMod != null)
            {
                existingMod.DamageMultiplier = damageMultiplier;
                existingMod.HealthMultiplier = healthMultiplier;
            }
            else
            {
                config.AffectingMods.Add(new ModMultiplier
                {
                    ModName = modName,
                    DamageMultiplier = damageMultiplier,
                    HealthMultiplier = healthMultiplier
                });
            }

            config.CalculateMultipliers();
            return true;
        }

        /// <summary>
        /// Adds entry to BossConfigs list
        /// </summary>
        /// <param name="bossType">Boss Internal Name</param>
        /// <param name="affectingModse"> List of ModMultipliers to add with entry</param>
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

        /// <summary>
        /// Removes modifier from Boss Config
        /// </summary>
        /// <param name="bossType">Boss Internal Name</param>
        /// <param name="modName">Mod Internal Name</param>
        /// <returns>True if modifier was deleted, false if boss was not found</returns>
        public static bool RemoveAffectingMod(int bossType, string modName)
        {
            var config = BossConfigs.FirstOrDefault(c => c.BossType == bossType);
            if (config == null)
                return false;

            var modToRemove = config.AffectingMods.FirstOrDefault(m => m.ModName == modName);
            if (modToRemove == null)
                return false;

            config.AffectingMods.Remove(modToRemove);
            config.CalculateMultipliers();
            return true;
        }

        /// <summary>
        /// Gets BossConfig with internal name
        /// </summary>
        public static BossConfig GetBossConfig(int bossType)
        {
            return BossConfigs.FirstOrDefault(c => c.BossType == bossType);
        }

        public override void Load()
        {
            AddBossConfig(
                bossType: ModContent.NPCType<MutantEXBoss>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityInheritance", DamageMultiplier = 10f, HealthMultiplier = 100f }, //ech
                new ModMultiplier { ModName = "HeavenlyArsenal", DamageMultiplier = 0.5f, HealthMultiplier = 50f }, //ech
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 1f, HealthMultiplier = 2.3f },
                new ModMultiplier { ModName = "SacredTools", DamageMultiplier = 0.5f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.3f, HealthMultiplier = 0.5f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.3f, HealthMultiplier = 0.5f }
                }
            );

            AddBossConfig(
                bossType: ModContent.NPCType<MutantBoss>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityInheritance", DamageMultiplier = 10f, HealthMultiplier = 100f }, //ech
                new ModMultiplier { ModName = "HeavenlyArsenal", DamageMultiplier = 0.5f, HealthMultiplier = 50f }, //ech
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 1f, HealthMultiplier = 2.3f },
                new ModMultiplier { ModName = "SacredTools", DamageMultiplier = 0.5f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.3f, HealthMultiplier = 0.5f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.3f, HealthMultiplier = 0.5f }
                }
            );

            AddBossConfig(
                bossType: ModContent.NPCType<AbomBoss>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityInheritance", DamageMultiplier = 5f, HealthMultiplier = 20f }, //ech
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
                new ModMultiplier { ModName = "CalamityInheritance", DamageMultiplier = 2f, HealthMultiplier = 3f }, //ech
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0.5f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "SacredTools", DamageMultiplier = 0.25f, HealthMultiplier = 0.25f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.25f, HealthMultiplier = 0.25f }
                }
            );
        }
    }
}