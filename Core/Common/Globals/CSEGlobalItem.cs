using CalamityMod.CalPlayer;
using CSE.Core.Common.ModPlayers;
using FargowiltasCrossmod.Core;
using FargowiltasSouls.Content.Items;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using FargowiltasSouls.Content.Items.Armor.Styx;
using FargowiltasSouls.Content.Items.Weapons.FinalUpgrades;
using FargowiltasSouls.Content.Items.Weapons.SwarmDrops;
using Microsoft.Xna.Framework;
using SacredTools.Common.Types.Revenant;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod.Utilities;
using static Terraria.ModLoader.ModContent;

namespace CSE.Core.Common.Globals
{
    public class CSEGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Item entity)
        {
            if (entity.type == ItemType<GuardianTome>())
            {
                entity.damage = 1500;
            }
            if (entity.type == ItemType<TheBiggestSting>())
            {
                entity.damage = 9750;
            }
            if (entity.type == ItemType<PhantasmalLeashOfCthulhu>())
            {
                entity.damage = 9000;
            }
            if (entity.type == ItemType<SlimeRain>())
            {
                entity.damage = 6800;
            }

            if (entity.type == ItemType<Penetrator>()
                || entity.type == ItemType<StyxGazer>()
                || entity.type == ItemType<SparklingLove>())
            {
                entity.damage *= 5;
            }

            if (entity.type == ItemType<DimensionSoul>())
            {
                entity.defense = 15;
            }

            if(ModCompatibility.SacredTools.Loaded || ModCompatibility.Calamity.Loaded || ModCompatibility.Homeward.Loaded)
            {
                //+35 defence from base
                if (entity.type == ItemType<StyxCrown>())
                {
                    entity.defense = 30;
                }
                if (entity.type == ItemType<StyxChestplate>())
                {
                    entity.defense = 45;
                }
                if (entity.type == ItemType<StyxLeggings>())
                {
                    entity.defense = 30;
                }
            }

            //helll naaaaah
            if (entity.CountsAsClass(DamageClass.Melee) && entity.damage > 0 && entity.pick == 0 && entity.axe == 0 && entity.hammer == 0 && entity.useStyle == 1 && !entity.noMelee && !entity.noUseGraphic)
            {
                Array.Resize(ref SwordGlobalItem.AllowedModdedSwords, SwordGlobalItem.AllowedModdedSwords.Length + 1);
                SwordGlobalItem.AllowedModdedSwords[SwordGlobalItem.AllowedModdedSwords.Length - 1] = entity.type;
            }
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (item.damage > 0)
            {
                //no cal inheritance because mutant enrages
                if (item.damage < 100000 && item.damage > 10000 && !CSEUtils.IsModItem(item, "CalamityHunt") && !CSEUtils.IsModItem(item, "CalamityInheritance") && !CSEUtils.IsModItem(item, "SacredTools") && !CSEUtils.IsModItem(item, "FargowiltasSouls") && !CSEUtils.IsModItem(item, "ThoriumMod") && !CSEUtils.IsModItem(item, "CaamityMod"))
                {
                    damage *= 0.1f;
                }
                if (item.damage > 100000 && !CSEUtils.IsModItem(item, "CalamityHunt") && !CSEUtils.IsModItem(item, "CalamityInheritance") && !CSEUtils.IsModItem(item, "SacredTools") && !CSEUtils.IsModItem(item, "FargowiltasSouls") && !CSEUtils.IsModItem(item, "ThoriumMod") && !CSEUtils.IsModItem(item, "CaamityMod"))
                {
                    damage *= 0.05f;
                }
            }
        }
        public override void UpdateAccessory(Item Item, Player player, bool hideVisual)
        {
            if (Item.type == ItemType<BerserkerSoul>())
            {
                player.GetDamage<MeleeDamageClass>() += 0.03f;
            }
            if (Item.type == ItemType<SnipersSoul>())
            {
                player.GetDamage<RangedDamageClass>() += 0.03f;
            }
            if (Item.type == ItemType<ConjuristsSoul>())
            {
                player.GetDamage<MeleeDamageClass>() += 0.03f;
            }
            if (Item.type == ItemType<ArchWizardsSoul>())
            {
                player.GetDamage<RangedDamageClass>() += 0.03f;
            }
        }

        public override void UpdateEquip(Item item, Player player)
        {
            if (item.type == ItemType<StyxChestplate>() && (ModCompatibility.Homeward.Loaded || ModCompatibility.SacredTools.Loaded || ModCompatibility.Calamity.Loaded || ModCompatibility.Homeward.Loaded))
            {
                player.GetDamage<GenericDamageClass>() += 0.05f;
            }
            if (item.type == ItemType<StyxCrown>() && (ModCompatibility.Homeward.Loaded || ModCompatibility.SacredTools.Loaded || ModCompatibility.Calamity.Loaded))
            {
                player.GetDamage<GenericDamageClass>() += 0.05f;
                player.GetCritChance<GenericDamageClass>() += 0.05f;
            }
            if (item.type == ItemType<StyxLeggings>() && (ModCompatibility.Homeward.Loaded || ModCompatibility.SacredTools.Loaded || ModCompatibility.Calamity.Loaded))
            {
                player.GetDamage<GenericDamageClass>() += 0.05f;
                player.GetCritChance<GenericDamageClass>() += 0.05f;
            }
        }
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockBack)
        {
            if (item.CountsAsClass<ThrowingDamageClass>())
            {
                velocity *= player.GetModPlayer<CSEPlayer>().throwerVelocity;
            }
        }
        void ItemBalance(List<TooltipLine> tooltips, EModeChanges change, string mod = "CSE")
        {
            tooltips.Add(new TooltipLine(Mod, $"{change}", $"{Language.GetTextValue($"Mods.{mod}.EModeBalance.{change}")}"));
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string BalanceLine = Language.GetTextValue($"Mods.CSE.EModeBalance.CrossBalance");
            string BalanceDownLine = $"[c/FF0000:{BalanceLine}]";

            if (item.damage < 100000 && item.damage > 10000 && !CSEUtils.IsModItem(item, "CalamityInheritance") && !CSEUtils.IsModItem(item, "SacredTools") && !CSEUtils.IsModItem(item, "FargowiltasSouls") && !CSEUtils.IsModItem(item, "ThoriumMod") && !CSEUtils.IsModItem(item, "CaamityMod"))
            {
                tooltips.Add(new TooltipLine(Mod, "DamageDown", $"{BalanceDownLine}" + Language.GetText($"Mods.CSE.EModeBalance.DamageDownGeneric").Format(90)));
            }
            if (item.damage > 100000 && !CSEUtils.IsModItem(item, "CalamityInheritance") && !CSEUtils.IsModItem(item, "SacredTools") && !CSEUtils.IsModItem(item, "FargowiltasSouls") && !CSEUtils.IsModItem(item, "ThoriumMod") && !CSEUtils.IsModItem(item, "CaamityMod"))
            {
                tooltips.Add(new TooltipLine(Mod, "DamageDown", $"{BalanceDownLine}" + Language.GetText($"Mods.CSE.EModeBalance.DamageDownGeneric").Format(95)));
            }

            if (item.ModItem is BaseSoul)
            {
                for (int i = 0; i < tooltips.Count; i++)
                {
                    tooltips[i].Text = Regex.Replace(tooltips[i].Text, "22%", "25%", RegexOptions.IgnoreCase);
                }
            }
            if (item.type == ItemType<StyxChestplate>() && (ModCompatibility.Homeward.Loaded || ModCompatibility.SacredTools.Loaded || ModCompatibility.Calamity.Loaded || ModCompatibility.Homeward.Loaded))
            {
                for (int i = 0; i < tooltips.Count; i++)
                {
                    tooltips[i].Text = Regex.Replace(tooltips[i].Text, "15%", "20%", RegexOptions.IgnoreCase);
                }
            }
            if (item.type == ItemType<StyxLeggings>() && (ModCompatibility.Homeward.Loaded || ModCompatibility.SacredTools.Loaded || ModCompatibility.Calamity.Loaded))
            {
                for (int i = 0; i < tooltips.Count; i++)
                {
                    tooltips[i].Text = Regex.Replace(tooltips[i].Text, "10%", "15%", RegexOptions.IgnoreCase);
                }
            }
            if (item.type == ItemType<StyxCrown>() && (ModCompatibility.Homeward.Loaded || ModCompatibility.SacredTools.Loaded || ModCompatibility.Calamity.Loaded))
            {
                for (int i = 0; i < tooltips.Count; i++)
                {
                    tooltips[i].Text = Regex.Replace(tooltips[i].Text, "10%", "15%", RegexOptions.IgnoreCase);
                }
            }

            if (ModCompatibility.Crossmod.Loaded)
            {
                if (item.type == ItemType<GuardianTome>()
                    || item.type == ItemType<TheBiggestSting>()
                    || item.type == ItemType<PhantasmalLeashOfCthulhu>()
                    || item.type == ItemType<SlimeRain>())
                    ItemBalance(tooltips, EModeChanges.Unnerf);
            }
        }

        [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
        public void BardAndHealer(Player Player, short bardBuffDuration, int techPointsMax, float throwerExhaustionRegenBonus, int throwerExhaustionMax, int bardResourceDropBoost, int inspirationRegenBonus, int bardResourceMax2, int healBonus)
        {
            Player.GetThoriumPlayer().bardBuffDuration += bardBuffDuration;
            Player.GetThoriumPlayer().techPointsMax += techPointsMax;
            Player.GetThoriumPlayer().throwerExhaustionRegenBonus += throwerExhaustionRegenBonus;
            Player.GetThoriumPlayer().throwerExhaustionMax += throwerExhaustionMax;
            Player.GetThoriumPlayer().bardResourceDropBoost += bardResourceDropBoost;
            Player.GetThoriumPlayer().inspirationRegenBonus += inspirationRegenBonus;
            Player.GetThoriumPlayer().bardResourceMax2 += bardResourceMax2;
            Player.GetThoriumPlayer().healBonus += healBonus;
        }

        [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
        public void ThrowerCal(Player Player, float stealth)
        {
            Player.GetModPlayer<CalamityPlayer>().wearingRogueArmor = true;
            Player.GetModPlayer<CalamityPlayer>().rogueStealthMax += stealth / 100;
        }
    }
}