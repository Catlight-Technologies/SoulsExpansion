using CalamityMod.Items.Placeables.Furniture;
using Fargowiltas;
using Fargowiltas.Content.Items.CaughtNPCs;
using FargowiltasSouls.Content.Items;
using FargowiltasSouls.Content.Projectiles.Weapons;
using Luminance.Common.Utilities;
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
using ThoriumMod.Items.Terrarium;
using ThoriumMod.Items.Thorium;
using ThoriumMod.Projectiles;
using ThoriumMod.Utilities;

namespace CSE.Core.Thorium.ModSystems
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class ThoriumPostSetupContent : ModSystem
    {
        public override void PostSetupContent()
        {
            //if (ModCompatibility.BossChecklist.Loaded)
            //{
            //    var changes = new List<(string, float)>
            //    {
            //        (ModContent.NPCType<DreamEater>(), 18.7f)
            //    };

            //    CSEUtils.ChangeBossProgressions(changes.ToArray());
            //}

            //FargoSets.Items.BuffStation[ItemType<ResilientCandle>()] = true;
            //FargoSets.Items.BuffStation[ItemType<SpitefulCandle>()] = true;
            //FargoSets.Items.BuffStation[ItemType<VigorousCandle>()] = true;
            //FargoSets.Items.BuffStation[ItemType<WeightlessCandle>()] = true;

            //FargoSets.Items..SetValue(FargoSets.Items.DupeType.Dupable, ModContent.ItemType<GadgetCoat>());

            //FargoSets.Items.DuplicatableItems.SetValue(FargoSets.Items.DupeType.MaterialsDupable, sots.Find<ModItem>("SubspaceBoosters").Type);
            //FargoSets.Items.DuplicatableItems.SetValue(FargoSets.Items.DupeType.MaterialsDupable, sots.Find<ModItem>("ChallengerRing").Type);

            //CSE.AddNPC("Blacksmith", ModContent.NPCType<Blacksmith>());
            //CSE.AddNPC("Cook", ModContent.NPCType<Cook>());
            //CSE.AddNPC("Cobbler", ModContent.NPCType<Cobbler>());
            //CSE.AddNPC("ConfusedZombie", ModContent.NPCType<ConfusedZombie>());
            //CSE.AddNPC("DesertAcolyte", ModContent.NPCType<DesertAcolyte>());
            //CSE.AddNPC("Diverman", ModContent.NPCType<Diverman>());
            //CSE.AddNPC("Druid", ModContent.NPCType<Druid>());
            //CSE.AddNPC("Spiritualist", ModContent.NPCType<Spiritualist>());
            //CSE.AddNPC("Tracker", ModContent.NPCType<Tracker>());
            //CSE.AddNPC("WeaponMaster", ModContent.NPCType<WeaponMaster>());

            int[] swordsToRemove = { ModContent.ItemType<BackStabber>(), ModContent.ItemType<EbonHammer>() };
            SwordGlobalItem.AllowedModdedSwords = SwordGlobalItem.AllowedModdedSwords.Where(x => !swordsToRemove.Contains(x)).ToArray();

            SpearRework.ReworkedSpears.AddRange(new List<int> {
                ModContent.ProjectileType<CoralPolearmPro>(),
                ModContent.ProjectileType<DemonBloodSpearPro>(),
                ModContent.ProjectileType<DragonToothPro>(),
                ModContent.ProjectileType<EnergyStormPartisanPro>(),
                ModContent.ProjectileType<FleshSkewerPro>(),
                ModContent.ProjectileType<ForkPro>(),
                ModContent.ProjectileType<HarpyTalonPro>(),
                ModContent.ProjectileType<HellishHalberdPro>(),
                ModContent.ProjectileType<IceLancePro>(),
                ModContent.ProjectileType<IllumiteSpearPro>(),
                ModContent.ProjectileType<MoonlightPro>(),
                ModContent.ProjectileType<PearlPikePro>(),
                ModContent.ProjectileType<PollenPikePro>(),
                ModContent.ProjectileType<PoseidonChargePro>(),
                ModContent.ProjectileType<RifleSpearPro>(),
                ModContent.ProjectileType<SandStoneSpearPro>(),
                ModContent.ProjectileType<TerrariumSpearPro>(),
                ModContent.ProjectileType<ThoriumSpearPro>(),
                ModContent.ProjectileType<ValadiumSpearPro>(),
            });

            ModCompatibility.Mutant.Mod.Call("AddTreeTreasure", new Func<bool>(Main.LocalPlayer.InModBiome<ThoriumMod.Biomes.Depths.DepthsBiome>), new Action(() =>
            {
                int itemType = ModContent.ItemType<ThoriumMod.Items.Depths.WaterChestnut>();
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_OpenItem(itemType), itemType, 5);
                itemType = ModContent.ItemType<ThoriumMod.Items.Depths.MarineBlock>();
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_OpenItem(itemType), itemType, 50);
            }), "Ph'nglui mglw'nafh Cthulhu R'lyeh wgah'nagl fhtagn", 0);

            double Damage(DamageClass damageClass) => Math.Round(Main.LocalPlayer.GetTotalDamage(damageClass).Additive * Main.LocalPlayer.GetTotalDamage(damageClass).Multiplicative * 100 - 100);
            int Crit(DamageClass damageClass) => (int)Main.LocalPlayer.GetTotalCritChance(damageClass);

            void Add<T>(Func<string> func) where T : ModItem => ModCompatibility.Mutant.Mod.Call("AddStat", ModContent.ItemType<T>(), func);

            Add<WoodenBaton>(() => $"{Language.GetTextValue("Mods.CSE.StatSheet.HealerDamage")}: {Damage(ModContent.GetInstance<HealerDamage>())}%");
            Add<WoodenBaton>(() => $"{Language.GetTextValue("Mods.CSE.StatSheet.HealerCrit")}: {Crit(ModContent.GetInstance<HealerDamage>())}%");

            Add<WoodenWhistle>(() => $"{Language.GetTextValue("Mods.CSE.StatSheet.BardDamage")}: {Damage(ModContent.GetInstance<BardDamage>())}%");
            Add<WoodenWhistle>(() => $"{Language.GetTextValue("Mods.CSE.StatSheet.BardCrit")}: {Crit(ModContent.GetInstance<BardDamage>())}%");

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
