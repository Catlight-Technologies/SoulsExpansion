using ContinentOfJourney.Items.Armor;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using FargowiltasSouls.Content.Items;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

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
                entity.type == ModContent.ItemType<EquilibriumBreastplate>()
                || entity.type == ModContent.ItemType<EquilibriumLeggings>()
                || entity.type == ModContent.ItemType<EquilibriumMask>()
                //biologic
                || entity.type == ModContent.ItemType<BiologicalBreastplate>()
                || entity.type == ModContent.ItemType<BiologicalHelmet>()
                || entity.type == ModContent.ItemType<BiologicalLeggings>()
                //chrono
                || entity.type == ModContent.ItemType<PerpetualHelmet>()
                || entity.type == ModContent.ItemType<PerpetualLeggings>()
                || entity.type == ModContent.ItemType<PerpetualPlate>()
                //chrono
                || entity.type == ModContent.ItemType<HeliologyHelmet>()
                || entity.type == ModContent.ItemType<HeliologyLeggings>()
                || entity.type == ModContent.ItemType<HeliologyPlate>()
                //sun
                || entity.type == ModContent.ItemType<SunlightBreastplate>()
                || entity.type == ModContent.ItemType<SunlightHelmet>()
                || entity.type == ModContent.ItemType<SunlightLegging>()
                )
            {
                entity.defense = entity.defense / 2;
            }
            if (
                //aurora
                entity.type == ModContent.ItemType<AuroraBoots>()
                || entity.type == ModContent.ItemType<AuroraHeadwear>()
                || entity.type == ModContent.ItemType<AuroraRobe>()
                //watchman
                || entity.type == ModContent.ItemType<WatchmanDress>()
                || entity.type == ModContent.ItemType<WatchmanHat>()
                || entity.type == ModContent.ItemType<WatchmanShirt>()
                //forest
                || entity.type == ModContent.ItemType<ForestBreastplate>()
                || entity.type == ModContent.ItemType<ForestHelmet>()
                || entity.type == ModContent.ItemType<ForestLeggings>()
                //reflector
                || entity.type == ModContent.ItemType<ReflectorBreastplate>()
                || entity.type == ModContent.ItemType<ReflectorHelmet>()
                || entity.type == ModContent.ItemType<ReflectorLeggings>()
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
        public static float BalanceChange(Item item)
        {
            if (CSESets.GetValue(CSESets.Items.AbomTierFargoWeapon, item.type))
                return ModCompatibility.Crossmod.Loaded || ModCompatibility.SacredTools.Loaded ? 1f : 1.7f;

            return 1;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            int tt0 = tooltips.FindIndex(line => line.Name == "Tooltip0");
            if (item.type == ModContent.ItemType<UniverseSoul>())
            {
                if (SoulsItem.IsNotRuminating(item))
                {
                    var conjurists = "[i:FargowiltasSouls/ConjuristsSoul]";
                    int extraeff = tooltips.FindIndex(t => t.Text.Contains(conjurists));
                    tooltips[extraeff - 1].Text = tooltips[extraeff - 1].Text.Replace("3", "4");
                    tooltips[extraeff].Text = tooltips[extraeff].Text.Replace(conjurists, conjurists + "[i:CSE/GuardianAngelsSoul]" + "[i:CSE/BardSoul]");
                }
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