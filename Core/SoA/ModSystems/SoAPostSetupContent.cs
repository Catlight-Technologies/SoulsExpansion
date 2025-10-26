using FargowiltasSouls.Content.Items;
using SacredTools.Common.Players;
using SacredTools.Content.Items.Accessories;
using SacredTools.Content.Items.Weapons.Dreadfire;
using SacredTools.Content.Items.Weapons.Harpy;
using SacredTools.Content.Items.Weapons.Mechs;
using SacredTools.Items.Dev;
using SacredTools.Items.Weapons.Flarium;
using SacredTools.Items.Weapons.Lunatic;
using SacredTools.Items.Weapons.Luxite;
using SacredTools.Items.Weapons.Marstech;
using SacredTools.Items.Weapons.Oblivion;
using SacredTools.Items.Weapons.Pigman;
using SacredTools.Items.Weapons.Special;
using SacredTools.Items.Weapons.Venomite;
using SacredTools.Items.Weapons;
using SacredTools;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static CSE.Core.Common.Globals.CSEPointsBalanceNPC;
using System.Collections.Generic;
using SacredTools.NPCs.Boss.Obelisk.Nihilus;

namespace CSE.Core.SoA.ModSystems
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class SoAPostSetupContent : ModSystem
    {
        public override void Load()
        {
            AddBossConfig(
                bossType: NPCType<Nihilus>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0.3f, HealthMultiplier = 2f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.1f, HealthMultiplier = 0.2f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.1f, HealthMultiplier = 0.2f }
                }
            );
            AddBossConfig(
                bossType: NPCType<Nihilus2>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0.3f, HealthMultiplier = 2f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.1f, HealthMultiplier = 0.2f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.1f, HealthMultiplier = 0.2f }
                }
            );
            AddBossConfig(
                bossType: NPCType<NihilusLanternRisen>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0, HealthMultiplier = 2f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0, HealthMultiplier = 0.2f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0, HealthMultiplier = 0.2f }
                }
            );
            AddBossConfig(
                bossType: NPCType<RelicShieldNihilus>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0, HealthMultiplier = 2f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0, HealthMultiplier = 0.2f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0, HealthMultiplier = 0.2f }
                }
            );
        }
        public override void PostSetupContent()
        {
            int bossdmgItem = ItemType<RageSuppressor>();
            int accuracyItem = ItemType<CasterArcanum>();
            Func<string> bardDamage = () => $"{Language.GetTextValue("Mods.CSE.StatSheet.BossDamage")}: {Main.LocalPlayer.GetModPlayer<MiscEffectsPlayer>().bossDamage.Multiplicative}%";
            Func<string> bardCrit = () => $"{Language.GetTextValue("Mods.CSE.StatSheet.Accuracy")}: {Main.LocalPlayer.GetModPlayer<ModdedPlayer>().accuracy}";
            ModCompatibility.Mutant.Mod.Call("AddStat", bossdmgItem, bardDamage);
            ModCompatibility.Mutant.Mod.Call("AddStat", accuracyItem, bardCrit);

            //int[] SoASwordsToApplyRework = [ItemType<Feathersword>(), ItemType<RedSword>(), ItemType<ChromaUltima>(),
            //ItemType<DragonslayerPandolarra>(), ItemType<PumpkinCarver>(), ItemType<CrimsonVeins>(),
            //ItemType<Eredhun>(),ItemType<NvidiaSword>(),ItemType<MidnightBlade>(),
            //ItemType<FlariumSword>(),ItemType<LapisSword>(),ItemType<Nyanmere>(),
            //ItemType<StarShower>(),ItemType<Claymarine>(),ItemType<PhaseSlasher>(),
            //ItemType<MoonEdgedPandolarra>(),ItemType<TrueMoonEdgedPandolarra>(),ItemType<Evanescense>(),
            //ItemType<OversizedFang>(),ItemType<Brandblade>(),ItemType<Pandolarra>(),
            //ItemType<Shaytnajima>(),ItemType<Skill_FuryForged>(),ItemType<Meenmourne>(),
            //ItemType<Yaldabaoth>(),ItemType<TruePandolarra>(),
            //ItemType<VenomiteSword>(),ItemType<HoariHemonga>(),];
            //SwordGlobalItem.AllowedModdedSwords = SwordGlobalItem.AllowedModdedSwords.Union(SoASwordsToApplyRework).ToArray();
        }
    }
}
