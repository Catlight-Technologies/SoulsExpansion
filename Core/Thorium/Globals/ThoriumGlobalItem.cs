using FargowiltasSouls.Content.Items.Accessories.Souls;
using FargowiltasSouls.Content.Items;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using System;
using ThoriumMod.Items.ThrownItems;
using ThoriumMod.Items.Donate;
using ThoriumMod.Items.BasicAccessories;
using ThoriumMod.Items.Terrarium;
using ThoriumMod.Items.BossThePrimordials.Omni;
using FargowiltasSouls.Core.Systems;
using ThoriumMod.Items.Coral;
using ThoriumMod.Items.DemonBlood;
using ThoriumMod.Items.Dragon;
using ThoriumMod.Items.BossGraniteEnergyStorm;
using ThoriumMod.Items.Flesh;
using ThoriumMod.Items.NPCItems;
using ThoriumMod.Items.BossFallenBeholder;
using ThoriumMod.Items.Icy;
using ThoriumMod.Items.Illumite;
using ThoriumMod.Items.ArcaneArmor;
using ThoriumMod.Items.Depths;
using ThoriumMod.Items.BossMini;
using ThoriumMod.Items.Thorium;
using ThoriumMod.Items.Valadium;
using ThoriumMod.Items.Sandstone;
using ThoriumMod.Items.BossThePrimordials.Rhapsodist;
using ThoriumMod.Items.BossThePrimordials.Dream;
using ThoriumMod.Items.BossThePrimordials.Aqua;

namespace CSE.Core.Thorium.Globals
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class ThoriumGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            int tt0 = tooltips.FindIndex(line => line.Name == "Tooltip0");
            if (item.type == ModContent.ItemType<UniverseSoul>())
            {
                if (SoulsItem.IsNotRuminating(item))
                {
                    var conjurists = "[i:FargowiltasSouls/SnipersSoul]";
                    int extraeff = tooltips.FindIndex(t => t.Text.Contains(conjurists));
                    tooltips[extraeff - 1].Text = tooltips[extraeff - 1].Text.Replace("3", "4");
                    tooltips[extraeff].Text = tooltips[extraeff].Text.Replace(conjurists, conjurists + "[i:CSE/GuardianAngelsSoul]" + "[i:CSE/BardSoul]");
                }
            }

            string BalanceLine = Language.GetTextValue($"Mods.CSE.EModeBalance.CrossBalance");

            float balance = BalanceChange(item);
            string BalanceUpLine = $"[c/00A36C:{BalanceLine}]";
            string BalanceDownLine = $"[c/FF0000:{BalanceLine}]";

            if (item.type == ModContent.ItemType<WhiteDwarfMask>() ||
                item.type == ModContent.ItemType<WhiteDwarfGreaves>() ||
                item.type == ModContent.ItemType<WhiteDwarfGuard>())
            {
                tooltips.Add(new TooltipLine(Mod, "BalanceDown", Language.GetTextValue($"{Language.GetText($"Mods.CSE.EModeBalance.WhiteDwarfNerf")}")));
            }

            if (item.type == ModContent.ItemType<CoralPolearm>() ||
                item.type == ModContent.ItemType<DemonBloodSpear>() ||
                item.type == ModContent.ItemType<DragonTooth>() ||
                item.type == ModContent.ItemType<EnergyStormPartisan>() ||
                item.type == ModContent.ItemType<FleshSkewer>() ||
                item.type == ModContent.ItemType<Fork>() ||
                item.type == ModContent.ItemType<HarpyTalon>() ||
                item.type == ModContent.ItemType<HellishHalberd>() ||
                item.type == ModContent.ItemType<IceLance>() ||
                item.type == ModContent.ItemType<IllumiteSpear>() ||
                item.type == ModContent.ItemType<Moonlight>() ||
                item.type == ModContent.ItemType<PearlPike>() ||
                item.type == ModContent.ItemType<PollenPike>() ||
                item.type == ModContent.ItemType<PoseidonCharge>() ||
                item.type == ModContent.ItemType<RifleSpear>() ||
                item.type == ModContent.ItemType<fSandStoneSpear>() ||
                item.type == ModContent.ItemType<TerrariumSpear>() ||
                item.type == ModContent.ItemType<ThoriumSpear>() ||
                item.type == ModContent.ItemType<ValadiumSpear>())
            {
                tooltips.Add(new TooltipLine(Mod, "BalanceUP", Language.GetTextValue($"{Language.GetText($"Mods.FargowiltasSouls.EModeBalance.SpearRework")}")));
            }

            //if (item.type == ModContent.ItemType<TerrariansLastKnife>())
            //{
            //    tooltips.Add(new TooltipLine(Mod, "BalanceUP", Language.GetTextValue($"{Language.GetText($"Mods.FargowiltasSouls.EModeBalance.ScalePositive").Format(30)}")));
            //}

            if (item.type == ModContent.ItemType<InfernoLordsFocus>())
            {
                tooltips.Add(new TooltipLine(Mod, "BalanceDOWN", Language.GetTextValue($"{Language.GetText("Mods.CSE.EModeBalance.InfernoNerf")}")));
                //if (WorldSavingSystem.DownedAbom || ModCompatibility.Calamity.Loaded)
                //{
                //    tooltips.Add(new TooltipLine(Mod, "BalanceUP", Language.GetTextValue($"{Language.GetText("Mods.CSE.EModeBalance.InfernoNone")}")));
                //}
                //else
                //{
                //    string bossesToKill = "";
                //    if (!WorldSavingSystem.DownedBoss[8])
                //    {
                //        bossesToKill += $"{Language.GetTextValue("Mods.FargowiltasSouls.NPCs.CosmosChampion.DisplayName")}, ";
                //    }
                //    bossesToKill += $"{Language.GetTextValue("Mods.FargowiltasSouls.NPCs.AbomBoss.DisplayName")}";

                //    tooltips.Add(new TooltipLine(Mod, "BalanceUP", Language.GetTextValue($"{Language.GetText("Mods.CSE.EModeBalance.InfernoDamage") + bossesToKill}")));
                //}
            }

            static string BalanceTooltips(string key) => Language.GetTextValue($"Mods.CSE.EModeBalance.{key}");
            void BuffTooltip(string key) => tooltips.Add(new TooltipLine(Mod, "BalanceUp", $"{BalanceUpLine}" + BalanceTooltips(key)));
            void NerfTooltip(string key) => tooltips.Add(new TooltipLine(Mod, "BalanceDown", $"{BalanceDownLine}" + BalanceTooltips(key)));

            if (balance > 1)
                tooltips.Add(new TooltipLine(Mod, "DamageUp", $"{BalanceUpLine}" + Language.GetText($"Mods.CSE.EModeBalance.DamageUpGeneric").Format(Math.Round((balance - 1) * 100))));
            else if (balance < 1)
                tooltips.Add(new TooltipLine(Mod, "DamageDown", $"{BalanceDownLine}" + Language.GetText($"Mods.CSE.EModeBalance.DamageDownGeneric").Format(Math.Round((1 - balance) * 100))));
        }

        public static float BalanceChange(Item item)
        {
            // Melee
            if (item.type == ModContent.ItemType<TerrariansLastKnife>()) return 1.1f;

            // Ranged
            if (item.type == ModContent.ItemType<TheJavelin>()) return 0.9f;
            if (item.type == ModContent.ItemType<OmniBow>()) return 1.2f;

            // Thrower
            if (item.type == ModContent.ItemType<TidalWave>()) return 1.2f;

            // Bard
            if (item.type == ModContent.ItemType<Holophonor>()) return 1.2f;
            if (item.type == ModContent.ItemType<TheSet>()) return 1.2f;
            if (item.type == ModContent.ItemType<Sousaphone>()) return 1.2f;
            if (item.type == ModContent.ItemType<BlackMIDI>()) return 0.9f;

            // Healer
            if (item.type == ModContent.ItemType<Lucidity>()) return 1.1f;

            // Other
            if (item.type == ModContent.ItemType<CrystalSpearTip>()) return 0.5f;
            if (CSESets.GetValue(CSESets.Items.AbomTierFargoWeapon, item.type))
                return ModCompatibility.Crossmod.Loaded || ModCompatibility.SacredTools.Loaded || ModCompatibility.Homeward.Loaded ? 1f : 1.4f;

            return 1;
        }
        public override void SetDefaults(Item item)
        {
            float balance = BalanceChange(item);
            if (balance != 1)
            {
                if (item.damage > 0)
                {
                    item.damage = (int)(item.damage * balance);
                }
            }
            if(item.type == ModContent.ItemType<TerrariumDefender>())
            {
                item.defense = 8;
            }
        }
    }
}
