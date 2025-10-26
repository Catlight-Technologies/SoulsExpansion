using System.Collections.Generic;
using System;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using SacredTools.Common.Types;
using SacredTools.Content.Items.Weapons.Dreamscape.Nihilus;
using SacredTools.Content.Items.Weapons.Relic;
using SacredTools.Items.Weapons.Relic;
using SacredTools.Content.Items.Weapons.Dev;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using System.Text.RegularExpressions;
using FargowiltasSouls.Content.Items;
using System.Linq;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using static CSE.Core.SoulsRecipes.SniperSoulEffects;
using static CSE.Core.SoulsRecipes.WorldShaperSoulEffects;

namespace CSE.Core.SoA.Globals
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class SoAGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public static float BalanceChange(Item item)
        {
            //Relic and EoO items

            if (CSESets.GetValue(CSESets.Items.AbomTierFargoWeapon, item.type) && !ModCompatibility.Calamity.Loaded)
                return ModCompatibility.Crossmod.Loaded ? 1f : 2f;

            // Melee
            if (item.type == ItemType<FlamesOfCondemnation>()) return ModCompatibility.Calamity.Loaded ? 3f : 2f;
            if (item.type == ItemType<SwordOfGreed>()) return 1.5f;
            if (item.type == ItemType<Devilsknife>()) return 1f;
            if (item.type == ItemType<CeruleanCyclone>()) return 1f;
            if (item.type == ItemType<BloodKunai>()) return 1f;
            // Ranged
            if (item.type == ItemType<NeedlerRelic>()) return 1f;
            if (item.type == ItemType<Desperatio>()) return ModCompatibility.Calamity.Loaded ? 1.6f : 1.2f;
            if (item.type == ItemType<Malevolence>()) return 1f;
            if (item.type == ItemType<QueenSwarm>()) return 1f;
            if (item.type == ItemType<Avalanche>()) return 1f;
            if (item.type == ItemType<Sharpshooter>()) return 1f;
            if (item.type == ItemType<Gunblade>()) return 1f;
            // Magic
            if (item.type == ItemType<Tenebris>()) return ModCompatibility.Calamity.Loaded ? 0.9f : 0.8f;
            if (item.type == ItemType<PaleRuin>()) return 1f;
            if (item.type == ItemType<LampOfCinders>()) return 1f;
            if (item.type == ItemType<FatesLament>()) return 1f;
            if (item.type == ItemType<MightyTorch>()) return 1f;
            // Summoner
            if (item.type == ItemType<AshenWake>()) return 1f;
            if (item.type == ItemType<Malice>()) return ModCompatibility.Calamity.Loaded ? 2f : 1.5f;
            if (item.type == ItemType<MaxDesertStaff>()) return 1f;
            // Thrower
            if (item.type == ItemType<RogueWave>()) return 1f;
            if (item.type == ItemType<DimensionalCrusher>()) return 1f;
            if (item.type == ItemType<BlindJusticeMK2>()) return 1f;
            if (item.type == ItemType<Eschaton>()) return ModCompatibility.Calamity.Loaded ? 1.6f : 1.2f;

            return 1;
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemType<ColossusSoul>())
            {
                player.statLifeMax2 += 50;
            }
            if (item.type == ItemType<TrawlerSoul>())
            {
                player.AddEffect<LunarRingEffect>(item);
            }
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (item.ModItem is ReloadWeapon reloadWeapon && player.HasEffect<InfAmmoEffect>())
            {
                //reloadWeapon.MaxMagazine *= 3;
                reloadWeapon.RefillMagazine(player);
            }
        }
        public override void SetDefaults(Item item)
        {
            float balance = BalanceChange(item);
            if (balance != 1)
            {
                item.damage = (int)(item.damage * balance);
            }
        }
        void ItemBalanceStat(List<TooltipLine> tooltips, EModeChanges change, string key, int amount = 0, string mod = "FargowiltasSouls")
        {
            string prefix = Language.GetTextValue($"Mods.FargowiltasSouls.EModeBalance.{change}");
            string change2 = Language.GetTextValue($"Mods.{mod}.EModeBalance.{key}", amount == 0 ? null : amount);
            tooltips.Add(new TooltipLine(Mod, $"{change}{key}", $"{prefix}{change2}"));
        }
        void ItemBalance(List<TooltipLine> tooltips, EModeChanges change, string mod = "CSE")
        {
            tooltips.Add(new TooltipLine(Mod, $"{change}", $"{Language.GetTextValue($"Mods.{mod}.EModeBalance.{change}")}"));
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemType<ColossusSoul>())
            {
                for (int i = 0; i < tooltips.Count; i++)
                {
                    tooltips[i].Text = Regex.Replace(tooltips[i].Text, "50", "100", RegexOptions.IgnoreCase);
                }
            }

            string BalanceLine = Language.GetTextValue($"Mods.CSE.EModeBalance.CrossBalance");
            
            float balance = BalanceChange(item);
            string BalanceUpLine = $"[c/00A36C:{BalanceLine}]";
            string BalanceDownLine = $"[c/FF0000:{BalanceLine}]";

            if (item.type == ItemType<Desperatio>())
                ItemBalance(tooltips, EModeChanges.Homing);
            if (item.type == ItemType<Tenebris>())
                ItemBalance(tooltips, EModeChanges.Homing);
            if (item.type == ItemType<Eschaton>())
                ItemBalance(tooltips, EModeChanges.Homing);

            if (item.type == ItemType<TrawlerSoul>())
            {
                tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue("Mods.CSE.AddedEffects.SoAWorldshaper")));
            }

            if (item.type == ItemType<Malevolence>())
            {
                tooltips.Add(new TooltipLine(Mod, "Nerf", Language.GetTextValue("Mods.CSE.EModeBalance.MalevolenceGrenadeNerf")));
            }

            if (item.type == ItemType<Desperatio>())
            {
                tooltips.Add(new TooltipLine(Mod, "Nerf", Language.GetTextValue("Mods.CSE.EModeBalance.DesperatioColumnNerf")));
            }

            static string BalanceTooltips(string key) => Language.GetTextValue($"Mods.CSE.EModeBalance.{key}");
            void BuffTooltip(string key) => tooltips.Add(new TooltipLine(Mod, "BalanceUp", $"{BalanceUpLine}" + BalanceTooltips(key)));
            void NerfTooltip(string key) => tooltips.Add(new TooltipLine(Mod, "BalanceDown", $"{BalanceDownLine}" + BalanceTooltips(key)));

            if (balance > 1)
                tooltips.Add(new TooltipLine(Mod, "DamageUp", $"{BalanceUpLine}" + Language.GetText($"Mods.CSE.EModeBalance.DamageUpGeneric").Format(Math.Round((balance - 1) * 100))));
            else if (balance < 1)
                tooltips.Add(new TooltipLine(Mod, "DamageDown", $"{BalanceDownLine}" + Language.GetText($"Mods.CSE.EModeBalance.DamageDownGeneric").Format(Math.Round((1 - balance) * 100))));

            int tt0 = tooltips.FindIndex(line => line.Name == "Tooltip0");
            if (item.type == ItemType<UniverseSoul>() )
            {
                if (!SoulsItem.IsNotRuminating(item))
                {
                    string key = "Mods.CSE.AddedEffects.";
                    var lines = tooltips[tt0].Text.Split("\n").ToList();
                    lines.Insert(4, Language.GetTextValue(key + "SoABerserker"));
                    lines.Insert(2, Language.GetTextValue(key + "SoAConjurist"));
                    lines.Insert(3, Language.GetTextValue(key + "SoASniper"));
                    lines.Insert(4, Language.GetTextValue(key + "SoAArchWizard"));
                    tooltips[tt0].Text = string.Join("\n", lines);
                }
            }
        }
    }
}