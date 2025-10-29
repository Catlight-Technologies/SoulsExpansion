using CSE.Core.Common.ModSystems;
using FargowiltasSouls.Content.Items;
using FargowiltasSouls.Content.Projectiles.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Items.NPCItems;
using ThoriumMod.Items.Thorium;
using ThoriumMod.NPCs.BossThePrimordials;
using ThoriumMod.Projectiles;
using ThoriumMod.Utilities;
using static CSE.Core.Common.Globals.CSEPointsBalanceNPC;
using static Terraria.ModLoader.ModContent;

namespace CSE.Core.Thorium.ModSystems
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class ThoriumPostSetupContent : ModSystem
    {
        public override void Load()
        {
            AddBossConfig(
                bossType: NPCType<DreamEater>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0.5f, HealthMultiplier = 4f },
                new ModMultiplier { ModName = "SacredTools", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f },
                new ModMultiplier { ModName = "FargowiltasSouls", DamageMultiplier = 0.1f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f }
                }
            );
            AddBossConfig(
               bossType: NPCType<Omnicide>(),
               affectingMods: new List<ModMultiplier>
               {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0.5f, HealthMultiplier = 5f },
                new ModMultiplier { ModName = "SacredTools", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f },
                new ModMultiplier { ModName = "FargowiltasSouls", DamageMultiplier = 0.1f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f }
               }
            );
            AddBossConfig(
               bossType: NPCType<SlagFury>(),
               affectingMods: new List<ModMultiplier>
               {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0.5f, HealthMultiplier = 5f },
                new ModMultiplier { ModName = "SacredTools", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f },
                new ModMultiplier { ModName = "FargowiltasSouls", DamageMultiplier = 0.1f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f }
               }
            );
            AddBossConfig(
               bossType: NPCType<Aquaius>(),
               affectingMods: new List<ModMultiplier>
               {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0.5f, HealthMultiplier = 5f },
                new ModMultiplier { ModName = "SacredTools", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f },
                new ModMultiplier { ModName = "FargowiltasSouls", DamageMultiplier = 0.1f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f }
               }
            );

            CSEPostSetupContent.changes.Add((NPCType<DreamEater>(), 21.1f));
        }
        public override void PostSetupContent()
        {
            //FargoSets.Items.BuffStation[ItemType<ResilientCandle>()] = true;
            //FargoSets.Items.BuffStation[ItemType<SpitefulCandle>()] = true;
            //FargoSets.Items.BuffStation[ItemType<VigorousCandle>()] = true;
            //FargoSets.Items.BuffStation[ItemType<WeightlessCandle>()] = true;

            //FargoSets.Items..SetValue(FargoSets.Items.DupeType.Dupable, ItemType<GadgetCoat>());

            //FargoSets.Items.DuplicatableItems.SetValue(FargoSets.Items.DupeType.MaterialsDupable, sots.Find<ModItem>("SubspaceBoosters").Type);
            //FargoSets.Items.DuplicatableItems.SetValue(FargoSets.Items.DupeType.MaterialsDupable, sots.Find<ModItem>("ChallengerRing").Type);

            //CSE.AddNPC("Blacksmith", NPCType<Blacksmith>());
            //CSE.AddNPC("Cook", NPCType<Cook>());
            //CSE.AddNPC("Cobbler", NPCType<Cobbler>());
            //CSE.AddNPC("ConfusedZombie", NPCType<ConfusedZombie>());
            //CSE.AddNPC("DesertAcolyte", NPCType<DesertAcolyte>());
            //CSE.AddNPC("Diverman", NPCType<Diverman>());
            //CSE.AddNPC("Druid", NPCType<Druid>());
            //CSE.AddNPC("Spiritualist", NPCType<Spiritualist>());
            //CSE.AddNPC("Tracker", NPCType<Tracker>());
            //CSE.AddNPC("WeaponMaster", NPCType<WeaponMaster>());

            int[] swordsToRemove = { ItemType<BackStabber>(), ItemType<EbonHammer>() };
            SwordGlobalItem.AllowedModdedSwords = SwordGlobalItem.AllowedModdedSwords.Where(x => !swordsToRemove.Contains(x)).ToArray();

            SpearRework.ReworkedSpears.AddRange(new List<int> {
                ProjectileType<CoralPolearmPro>(),
                ProjectileType<DemonBloodSpearPro>(),
                ProjectileType<DragonToothPro>(),
                ProjectileType<EnergyStormPartisanPro>(),
                ProjectileType<FleshSkewerPro>(),
                ProjectileType<ForkPro>(),
                ProjectileType<HarpyTalonPro>(),
                ProjectileType<HellishHalberdPro>(),
                ProjectileType<IceLancePro>(),
                ProjectileType<IllumiteSpearPro>(),
                ProjectileType<MoonlightPro>(),
                ProjectileType<PearlPikePro>(),
                ProjectileType<PollenPikePro>(),
                ProjectileType<PoseidonChargePro>(),
                ProjectileType<RifleSpearPro>(),
                ProjectileType<SandStoneSpearPro>(),
                ProjectileType<TerrariumSpearPro>(),
                ProjectileType<ThoriumSpearPro>(),
                ProjectileType<ValadiumSpearPro>(),
            });

            ModCompatibility.Mutant.Mod.Call("AddTreeTreasure", new Func<bool>(Main.LocalPlayer.InModBiome<ThoriumMod.Biomes.Depths.DepthsBiome>), new Action(() =>
            {
                int itemType = ItemType<ThoriumMod.Items.Depths.WaterChestnut>();
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_OpenItem(itemType), itemType, 5);
                itemType = ItemType<ThoriumMod.Items.Depths.MarineBlock>();
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_OpenItem(itemType), itemType, 50);
            }), "Ph'nglui mglw'nafh Cthulhu R'lyeh wgah'nagl fhtagn", 0);

            double Damage(DamageClass damageClass) => Math.Round(Main.LocalPlayer.GetTotalDamage(damageClass).Additive * Main.LocalPlayer.GetTotalDamage(damageClass).Multiplicative * 100 - 100);
            int Crit(DamageClass damageClass) => (int)Main.LocalPlayer.GetTotalCritChance(damageClass);

            void Add<T>(Func<string> func) where T : ModItem => ModCompatibility.Mutant.Mod.Call("AddStat", ItemType<T>(), func);

            Add<WoodenBaton>(() => $"{Language.GetTextValue("Mods.CSE.StatSheet.HealerDamage")}: {Damage(GetInstance<HealerDamage>())}%");
            Add<WoodenBaton>(() => $"{Language.GetTextValue("Mods.CSE.StatSheet.HealerCrit")}: {Crit(GetInstance<HealerDamage>())}%");

            Add<WoodenWhistle>(() => $"{Language.GetTextValue("Mods.CSE.StatSheet.BardDamage")}: {Damage(GetInstance<BardDamage>())}%");
            Add<WoodenWhistle>(() => $"{Language.GetTextValue("Mods.CSE.StatSheet.BardCrit")}: {Crit(GetInstance<BardDamage>())}%");

            if (!ModCompatibility.Crossmod.Loaded)
            {
                Add<ThoriumMod.Items.ThrownItems.StoneThrowingSpear>(() => $"{Language.GetTextValue("Mods.CSE.StatSheet.ThrowerDamage")}: {Damage(DamageClass.Throwing)}%");
                Add<ThoriumMod.Items.ThrownItems.StoneThrowingSpear>(() => $"{Language.GetTextValue("Mods.CSE.StatSheet.ThrowerCrit")}: {Crit(DamageClass.Throwing)}%");
            }

            Add<ThoriumMod.Items.BossMini.TheGoodBook>(() => $"{Language.GetTextValue("Mods.CSE.StatSheet.HealerAddHeal")}: {Main.LocalPlayer.GetThoriumPlayer().healBonus}");
            Add<ThoriumShield>(() => $"{Language.GetTextValue("Mods.CSE.StatSheet.LifeShield")}: { Main.LocalPlayer.GetThoriumPlayer().shieldHealth}");

            Add<InspirationFragment>(() => $"{Language.GetTextValue("Mods.CSE.StatSheet.BardResource")}: {Main.LocalPlayer.GetThoriumPlayer().bardResourceMax2}");
            Add<InspirationNote>(() => $"{Language.GetTextValue("Mods.CSE.StatSheet.BardResourceRegen")}: {Main.LocalPlayer.GetThoriumPlayer().inspirationRegenBonus}/ sec");
        }
    }
}
