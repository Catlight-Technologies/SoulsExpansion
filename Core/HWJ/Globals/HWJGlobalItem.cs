using ContinentOfJourney.Items.Armor;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using FargowiltasSouls.Content.Items;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler.Content;
using FargowiltasSouls.Core.Toggler;
using ContinentOfJourney.Items.Accessories.MeleeExpansion;
using ContinentOfJourney;
using FargowiltasSouls.Content.Bosses.MutantBoss;
using static Terraria.ModLoader.ModContent;
using ContinentOfJourney.Items.Accessories;
using ContinentOfJourney.Items;
using ContinentOfJourney.Items.Flamethrowers;
using ContinentOfJourney.Items.FielderSentries;
using FargowiltasSouls.Core.Systems;

namespace CSE.Core.HWJ.Globals
{
    [ExtendsFromMod(ModCompatibility.Homeward.Name)]
    [JITWhenModsEnabled(ModCompatibility.Homeward.Name)]
    public class HWJGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Item entity)
        {
            //defense bloat removal
            if (
                //equilibrium
                entity.type == ItemType<EquilibriumBreastplate>()
                || entity.type == ItemType<EquilibriumLeggings>()
                || entity.type == ItemType<EquilibriumMask>()
                //biologic
                || entity.type == ItemType<BiologicalBreastplate>()
                || entity.type == ItemType<BiologicalHelmet>()
                || entity.type == ItemType<BiologicalLeggings>()
                //chrono
                || entity.type == ItemType<PerpetualHelmet>()
                || entity.type == ItemType<PerpetualLeggings>()
                || entity.type == ItemType<PerpetualPlate>()
                //helio
                || entity.type == ItemType<HeliologyHelmet>()
                || entity.type == ItemType<HeliologyLeggings>()
                || entity.type == ItemType<HeliologyPlate>()
                //sun
                || entity.type == ItemType<SunlightBreastplate>()
                || entity.type == ItemType<SunlightHelmet>()
                || entity.type == ItemType<SunlightLegging>()
                )
            {
                entity.defense = entity.defense / 2;
            }
            if (
                //aurora
                entity.type == ItemType<AuroraBoots>()
                || entity.type == ItemType<AuroraHeadwear>()
                || entity.type == ItemType<AuroraRobe>()
                //watchman
                || entity.type == ItemType<WatchmanDress>()
                || entity.type == ItemType<WatchmanHat>()
                || entity.type == ItemType<WatchmanShirt>()
                //forest
                || entity.type == ItemType<ForestBreastplate>()
                || entity.type == ItemType<ForestHelmet>()
                || entity.type == ItemType<ForestLeggings>()
                //reflector
                || entity.type == ItemType<ReflectorBreastplate>()
                || entity.type == ItemType<ReflectorHelmet>()
                || entity.type == ItemType<ReflectorLeggings>()
                )
            {
                entity.defense = entity.defense / 3 * 2;
            }

            float balance = BalanceChange(entity);
            if (balance != 1)
            {
                if (entity.damage > 0)
                {
                    entity.damage = (int)(entity.damage * balance);
                }
            }
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if(item.type == ItemType<SunsHeart>())
            {
                return WorldSavingSystem.DownedAbom;
            }
            return base.CanUseItem(item, player);
        }
        public static float BalanceChange(Item item)
        {
            if (CSESets.GetValue(CSESets.Items.AbomTierFargoWeapon, item.type))
                return ModCompatibility.Crossmod.Loaded || ModCompatibility.SacredTools.Loaded ? 1f : 2f;

            //...
            if (CSESets.GetValue(CSESets.Items.HWJFinalBarWeapon, item.type))
                return 0.9f;

            //melee
            if (item.type == ItemType<CosmicBoardsword>()) return 1.1f;

            //magic
            if (item.type == ItemType<SurpriseEnding>()) return 1f;
            if (item.type == ItemType<DoublePlot>()) return 0.7f;

            //ranged
            if (item.type == ItemType<FT13Phlogistinator>()) return 1f;

            //summoner
            if (item.type == ItemType<StaffDramaticIrony>()) return 1f;
            if (item.type == ItemType<PhantomStaff>()) return 1.1f;

            return 1;
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemType<ColossusSoul>() || item.type == ItemType<DimensionSoul>() || item.type == ItemType<EternitySoul>())
            {
                ModCompatibility.Homeward.Mod.Find<ModItem>("MasterShield").UpdateAccessory(player, true);
                ModCompatibility.Homeward.Mod.Find<ModItem>("VanguardBreastpiece").UpdateAccessory(player, true);
            }

            if (item.type == ItemType<SnipersSoul>() || item.type == ItemType<UniverseSoul>() || item.type == ItemType<EternitySoul>())
            {
                ModCompatibility.Homeward.Mod.Find<ModItem>("TheBatter").UpdateAccessory(player, true);
            }
            if (item.type == ItemType<ArchWizardsSoul>() || item.type == ItemType<UniverseSoul>() || item.type == ItemType<EternitySoul>())
            {
                ModCompatibility.Homeward.Mod.Find<ModItem>("EruditeBookmark").UpdateAccessory(player, true);
            }
            if (item.type == ItemType<ConjuristsSoul>() || item.type == ItemType<UniverseSoul>() || item.type == ItemType<EternitySoul>())
            {
                ModCompatibility.Homeward.Mod.Find<ModItem>("DivineEmblem").UpdateAccessory(player, true);
                ModCompatibility.Homeward.Mod.Find<ModItem>("CommandersGaunlet").UpdateAccessory(player, true);
            }
            if (item.type == ItemType<BerserkerSoul>() || item.type == ItemType<UniverseSoul>() || item.type == ItemType<EternitySoul>())
            {
                ModCompatibility.Homeward.Mod.Find<ModItem>("DivineEmblem").UpdateAccessory(player, true);
                player.AddEffect<PhilosophersStoneEffect>(item);
            }

            if (ModCompatibility.Thorium.Loaded)
            {
                if (item.type == ItemType<VanguardBreastpiece>())
                {
                    ModCompatibility.Thorium.Mod.Find<ModItem>("TerrariumDefender").UpdateAccessory(player, true);
                }
            }
            if (ModCompatibility.SacredTools.Loaded)
            {
                if (item.type == ItemType<VanguardBreastpiece>())
                {
                    ModCompatibility.SacredTools.Mod.Find<ModItem>("CelestialShield").UpdateAccessory(player, true);
                }
            }
        }
        public class PhilosophersStoneEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<UniverseHeader>();
            public override int ToggleItemType => ItemType<PhilosophersStone>();
            public override bool MutantsPresenceAffects => true;
            public override void PostUpdateEquips(Player player)
            {
                if (!NPC.AnyNPCs(NPCType<MutantBoss>()))
                {
                    TemplatePlayer modPlayer = player.GetModPlayer<TemplatePlayer>();
                    modPlayer.GrindStone_Type = 4;
                    modPlayer.GrindStone_Time = 480;
                }
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            int tt0 = tooltips.FindIndex(line => line.Name == "Tooltip0");
            if (item.type == ItemType<UniverseSoul>())
            {
                if (SoulsItem.IsNotRuminating(item))
                {
                    var conjurists = "[i:FargowiltasSouls/ConjuristsSoul]";
                    int extraeff = tooltips.FindIndex(t => t.Text.Contains(conjurists));
                    tooltips[extraeff - 1].Text = tooltips[extraeff - 1].Text.Replace("3", "4");
                    tooltips[extraeff].Text = tooltips[extraeff].Text.Replace(conjurists, conjurists + "[i:CSE/GuardianAngelsSoul]" + "[i:CSE/BardSoul]");
                }
            }

            if (item.type == ItemType<ColossusSoul>()) {
                tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue("Mods.CSE.AddedEffects.HWJColossus")));
                tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue("Mods.CSE.AddedEffects.HWJColossus2"))); }

            if (item.type == ItemType<SnipersSoul>())
                tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue("Mods.CSE.AddedEffects.HWJSniper")));
            if (item.type == ItemType<ArchWizardsSoul>())
                tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue("Mods.CSE.AddedEffects.HWJArchWizard")));
            if (item.type == ItemType<ConjuristsSoul>())
                tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue("Mods.CSE.AddedEffects.HWJConjurist")));
            if (item.type == ItemType<BerserkerSoul>())
                tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue("Mods.CSE.AddedEffects.HWJBerserker")));

            if (item.type == ItemType<SunsHeart>())
                tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue("Mods.CSE.EModeBalance.PostAbomSlot")));

            if (
                //equilibrium
                item.type == ItemType<EquilibriumBreastplate>()
                || item.type == ItemType<EquilibriumLeggings>()
                || item.type == ItemType<EquilibriumMask>()
                //biologic
                || item.type == ItemType<BiologicalBreastplate>()
                || item.type == ItemType<BiologicalHelmet>()
                || item.type == ItemType<BiologicalLeggings>()
                //chrono
                || item.type == ItemType<PerpetualHelmet>()
                || item.type == ItemType<PerpetualLeggings>()
                || item.type == ItemType<PerpetualPlate>()
                //helio
                || item.type == ItemType<HeliologyHelmet>()
                || item.type == ItemType<HeliologyLeggings>()
                || item.type == ItemType<HeliologyPlate>()
                //sun
                || item.type == ItemType<SunlightBreastplate>()
                || item.type == ItemType<SunlightHelmet>()
                || item.type == ItemType<SunlightLegging>()
                )
            {
                tooltips.Add(new TooltipLine(Mod, "BalanceDown", Language.GetTextValue($"{Language.GetText($"Mods.CSE.EModeBalance.HWJDefenseNerf1")}")));
            }

            if (
                //aurora
                item.type == ItemType<AuroraBoots>()
                || item.type == ItemType<AuroraHeadwear>()
                || item.type == ItemType<AuroraRobe>()
                //watchman
                || item.type == ItemType<WatchmanDress>()
                || item.type == ItemType<WatchmanHat>()
                || item.type == ItemType<WatchmanShirt>()
                //forest
                || item.type == ItemType<ForestBreastplate>()
                || item.type == ItemType<ForestHelmet>()
                || item.type == ItemType<ForestLeggings>()
                //reflector
                || item.type == ItemType<ReflectorBreastplate>()
                || item.type == ItemType<ReflectorHelmet>()
                || item.type == ItemType<ReflectorLeggings>()
                )
            {
                tooltips.Add(new TooltipLine(Mod, "BalanceDown", Language.GetTextValue($"{Language.GetText($"Mods.CSE.EModeBalance.HWJDefenseNerf2")}")));
            }

            string BalanceLine = Language.GetTextValue($"Mods.CSE.EModeBalance.CrossBalance");

            float balance = BalanceChange(item);
            string BalanceUpLine = $"[c/00A36C:{BalanceLine}]";
            string BalanceDownLine = $"[c/FF0000:{BalanceLine}]";

            static string BalanceTooltips(string key) => Language.GetTextValue($"Mods.CSE.EModeBalance.{key}");
            void BuffTooltip(string key) => tooltips.Add(new TooltipLine(Mod, "BalanceUp", $"{BalanceUpLine}" + BalanceTooltips(key)));
            void NerfTooltip(string key) => tooltips.Add(new TooltipLine(Mod, "BalanceDown", $"{BalanceDownLine}" + BalanceTooltips(key)));

            if (balance > 1)
                tooltips.Add(new TooltipLine(Mod, "DamageUp", $"{BalanceUpLine}" + Language.GetText($"Mods.CSE.EModeBalance.DamageUpGeneric").Format(Math.Round((balance - 1) * 100))));
            else if (balance < 1)
                tooltips.Add(new TooltipLine(Mod, "DamageDown", $"{BalanceDownLine}" + Language.GetText($"Mods.CSE.EModeBalance.DamageDownGeneric").Format(Math.Round((1 - balance) * 100))));
        }
    }
}