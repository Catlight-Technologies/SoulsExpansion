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
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler.Content;
using FargowiltasSouls.Core.Toggler;
using static Terraria.ModLoader.ModContent;
using static CSE.Content.Thorium.Accessories.Enchantments.FlightEnchant;
using static CSE.Content.Thorium.Accessories.Enchantments.GeodeEnchant;
using CSE.Content.Common.Accessories.Souls;
using ThoriumMod.Utilities;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.BossThePrimordials.Slag;
using Terraria.DataStructures;
using CSE.Content.Thorium.Projectiles;
using FargowiltasSouls;
using static CSE.Content.Thorium.Accessories.Enchantments.YewWoodEnchant;
using CSE.Core.Thorium.ModPlayers;

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
            if (item.type == ItemType<UniverseSoul>())
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

            if (item.type == ItemType<WhiteDwarfMask>() ||
                item.type == ItemType<WhiteDwarfGreaves>() ||
                item.type == ItemType<WhiteDwarfGuard>())
            {
                tooltips.Add(new TooltipLine(Mod, "BalanceDown", Language.GetTextValue($"{Language.GetText($"Mods.CSE.EModeBalance.WhiteDwarfNerf")}")));
            }

            if (item.type == ItemType<BlackMIDI>())
            {
                tooltips.Add(new TooltipLine(Mod, "BalanceDown", Language.GetTextValue($"{Language.GetText($"Mods.CSE.EModeBalance.MidiHealNerf")}")));
            }

            if (item.type == ItemType<TideTurnersGaze>())
            {
                tooltips.Add(new TooltipLine(Mod, "BalanceDown", Language.GetTextValue($"{Language.GetText($"Mods.CSE.EModeBalance.TideDaggersNerf")}")));
            }

            if (item.type == ItemType<ColossusSoul>())
            {
                tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue("Mods.CSE.AddedEffects.ThoriumColossus1")));
                tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue("Mods.CSE.AddedEffects.ThoriumColossus2")));
                tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue("Mods.CSE.AddedEffects.ThoriumColossus4")));
            }
            if (item.type == ItemType<WorldShaperSoul>())
                tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue("Mods.CSE.AddedEffects.ThoriumWorldshaper")));

            if (item.type == ItemType<SnipersSoul>())
                tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue("Mods.CSE.AddedEffects.ThoriumSniper")));
            if (item.type == ItemType<ArchWizardsSoul>())
                tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue("Mods.CSE.AddedEffects.ThoriumArchWizard")));
            //if (item.type == ItemType<ConjuristsSoul>())
                //tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue("Mods.CSE.AddedEffects.ThoriumConjurist")));
            if (item.type == ItemType<BerserkerSoul>())
                tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue("Mods.CSE.AddedEffects.ThoriumBerserker")));

            if (item.type == ItemType<CoralPolearm>() ||
                item.type == ItemType<DemonBloodSpear>() ||
                item.type == ItemType<DragonTooth>() ||
                item.type == ItemType<EnergyStormPartisan>() ||
                item.type == ItemType<FleshSkewer>() ||
                item.type == ItemType<Fork>() ||
                item.type == ItemType<HarpyTalon>() ||
                item.type == ItemType<HellishHalberd>() ||
                item.type == ItemType<IceLance>() ||
                item.type == ItemType<IllumiteSpear>() ||
                item.type == ItemType<Moonlight>() ||
                item.type == ItemType<PearlPike>() ||
                item.type == ItemType<PollenPike>() ||
                item.type == ItemType<PoseidonCharge>() ||
                item.type == ItemType<RifleSpear>() ||
                item.type == ItemType<fSandStoneSpear>() ||
                item.type == ItemType<TerrariumSpear>() ||
                item.type == ItemType<ThoriumSpear>() ||
                item.type == ItemType<ValadiumSpear>())
            {
                tooltips.Add(new TooltipLine(Mod, "BalanceUP", Language.GetTextValue($"{Language.GetText($"Mods.FargowiltasSouls.EModeBalance.SpearRework")}")));
            }

            if (item.type == ItemType<OmniCannon>() ||
                item.type == ItemType<OmniBow>() ||
                item.type == ItemType<QuasarsFlare>() ||
                item.type == ItemType<DeitysTrefork>() ||
                item.type == ItemType<OceansJudgement>() ||
                item.type == ItemType<TidalWave>())
            {
                tooltips.Add(new TooltipLine(Mod, "BalanceUP", Language.GetTextValue($"{Language.GetText($"Mods.CSE.EModeBalance.VelUP")}")));
            }

            //if (item.type == ItemType<TerrariansLastKnife>())
            //{
            //    tooltips.Add(new TooltipLine(Mod, "BalanceUP", Language.GetTextValue($"{Language.GetText($"Mods.FargowiltasSouls.EModeBalance.ScalePositive").Format(30)}")));
            //}

            if (item.type == ItemType<InfernoLordsFocus>())
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
            //if (item.type == ItemType<TerrariansLastKnife>()) return 1.1f;
            if (item.type == ItemType<TerrariumSaber>()) return 1.2f;
            if (item.type == ItemType<SevenSeasDevastator>()) return 1.4f;
            if (item.type == ItemType<OceansJudgement>()) return 1.1f;

            // Ranged
            if (item.type == ItemType<TheJavelin>()) return 0.7f;
            if (item.type == ItemType<OmniBow>()) return 1.5f;
            if (item.type == ItemType<OmniCannon>()) return 1.2f;

            // Thrower
            if (item.type == ItemType<TidalWave>()) return 1.3f;
            if (item.type == ItemType<DeitysTrefork>()) return 0.8f;

            // Bard
            if (item.type == ItemType<Holophonor>()) return 1.2f;
            if (item.type == ItemType<TheSet>()) return 1.2f;
            if (item.type == ItemType<Sousaphone>()) return 1.2f;
            if (item.type == ItemType<BlackMIDI>()) return 0.9f;

            // Healer
            if (item.type == ItemType<Lucidity>()) return 1.4f;
            if (item.type == ItemType<RealitySlasher>()) return 1.1f;

            // Magic
            if (item.type == ItemType<DevilsClaw>()) return 0.9f;

            // Summon
            if (item.type == ItemType<EmberStaff>()) return 1.1f;

            // Other
            if (item.type == ItemType<CrystalSpearTip>()) return 0.5f;
            if (CSESets.GetValue(CSESets.Items.AbomTierFargoWeapon, item.type))
                return ModCompatibility.Crossmod.Loaded || ModCompatibility.SacredTools.Loaded || ModCompatibility.Homeward.Loaded ? 1f : 1.5f;

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
            if(item.type == ItemType<TerrariumDefender>())
            {
                item.defense = 8;
            }
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.GetModPlayer<ThoriumPlayer>().yewWoodCD <= 0 && player.HasEffect<YewWoodEffect>())
            {
                if (Main.rand.NextBool(5))
                {
                    Projectile.NewProjectile(
                        source,
                        player.Center,
                        velocity,
                        ProjectileType<VileArrow>(),
                        YewWoodEffect.BaseDamage(player),
                        0f,
                        player.whoAmI
                    );
                    //0.5 sec
                    player.GetModPlayer<ThoriumPlayer>().yewWoodCD = 30;
                }
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemType<ColossusSoul>() || item.type == ItemType<DimensionSoul>() || item.type == ItemType<EternitySoul>())
            {
                ModCompatibility.Thorium.Mod.Find<ModItem>("Phylactery").UpdateAccessory(player, true);
                ModCompatibility.Thorium.Mod.Find<ModItem>("TerrariumDefender").UpdateAccessory(player, true);
            }
            if (item.type == ItemType<FlightMasterySoul>() || item.type == ItemType<DimensionSoul>() || item.type == ItemType<EternitySoul>())
            {
                player.AddEffect<FlightEffect>(item);
            }
            if (item.type == ItemType<WorldShaperSoul>() || item.type == ItemType<DimensionSoul>() || item.type == ItemType<EternitySoul>())
            {
                player.AddEffect<GeodeEffect>(item);
            }

            if (item.type == ItemType<OlympianSoul>() || item.type == ItemType<UniverseSoul>() || item.type == ItemType<EternitySoul>())
            {
                player.GetThoriumPlayer().throwerExhaustion = 0;
            }

            if (item.type == ItemType<SnipersSoul>() || item.type == ItemType<UniverseSoul>() || item.type == ItemType<EternitySoul>())
            {
                player.AddEffect<WarheadEffect>(item);
            }
            if (item.type == ItemType<ArchWizardsSoul>()|| item.type == ItemType<UniverseSoul>() || item.type == ItemType<EternitySoul>())
            {
                ModCompatibility.Thorium.Mod.Find<ModItem>("MurkyCatalyst").UpdateAccessory(player, true);
            }
            if (item.type == ItemType<ConjuristsSoul>() || item.type == ItemType<UniverseSoul>() || item.type == ItemType<EternitySoul>())
            {

            }
            if (item.type == ItemType<BerserkerSoul>() || item.type == ItemType<UniverseSoul>() || item.type == ItemType<EternitySoul>())
            {
                ModCompatibility.Thorium.Mod.Find<ModItem>("FrostburnPouch").UpdateAccessory(player, true);
            }
        }
        public class WarheadEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<UniverseHeader>();
            public override int ToggleItemType => ItemType<ConcussiveWarhead>();

            public override void PostUpdateEquips(Player player)
            {
                ModCompatibility.Thorium.Mod.Find<ModItem>("ConcussiveWarhead").UpdateAccessory(player, true);
            }
        }
    }
}
