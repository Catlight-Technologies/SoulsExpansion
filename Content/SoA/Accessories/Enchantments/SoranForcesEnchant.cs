using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using SacredTools.Content.Items.Armor.Lunar.Vortex;
using SacredTools.Items.Weapons.Lunatic;
using SacredTools.Content.Items.Weapons.Oblivion;
using Microsoft.Xna.Framework.Graphics;
using FargowiltasSouls.Content.UI.Elements;
using CSE.Core;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.SoA.Buffs;
using Fargowiltas.Content.Items.Tiles;

namespace CSE.Content.SoA.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class SoranForcesEnchant : BaseEnchant
    {
        public override List<AccessoryEffect> ActiveSkillTooltips =>
            [AccessoryEffectLoader.GetEffect<SoranForcesEffect>()];
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = 11;
            Item.value = 350000;
        }

        public override Color nameColor => new(21, 142, 100);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var SoranForcesPlayer = player.GetModPlayer<SoranForcesPlayer>();
            player.AddEffect<SoranForcesEffect>(Item);
            SoranForcesPlayer.HasSoranForcesEnchantThisFrame = true;
        }

        public class SoranForcesEffect : AccessoryEffect
        {
            public override Header ToggleHeader => null;
            public override int ToggleItemType => ModContent.ItemType<SoranForcesEnchant>();
            public override bool ActiveSkill => true;

            public override void ActiveSkillJustPressed(Player player, bool stunned)
            {
                var SoranForcesPlayer = player.GetModPlayer<SoranForcesPlayer>();

                if (!player.HasBuff(ModContent.BuffType<SniperCooldownBuff>()) && !player.HasBuff(ModContent.BuffType<SniperBuff>()) && SoranForcesPlayer.SniperStateCharge == 15 * 60)
                {
                    SoranForcesPlayer.SniperStateActive = true;
                    SoranForcesPlayer.SniperStateRecharging = false;
                    player.AddBuff(ModContent.BuffType<SniperBuff>(), 15 * 60);
                }
            }

            private void DeactivateSniperState(Player player)
            {
                var SoranForcesPlayer = player.GetModPlayer<SoranForcesPlayer>();
                SoranForcesPlayer.SniperStateActive = false;
                SoranForcesPlayer.SniperStateRecharging = true;
                SoranForcesPlayer.SniperStateCharge = 1;
                player.AddBuff(ModContent.BuffType<SniperCooldownBuff>(), 15 * 60);
            }
            public override void PostUpdate(Player player)
            {
                var SoranForcesPlayer = player.GetModPlayer<SoranForcesPlayer>();
                if (SoranForcesPlayer.SniperStateActive)
                {
                    if (SoranForcesPlayer.SniperStateCharge-- <= 0)
                    {
                        DeactivateSniperState(player);
                    }
                }

                if (SoranForcesPlayer.SniperStateRecharging)
                {
                    if (SoranForcesPlayer.SniperStateCharge++ >= ((15 * 60) - 1))
                    {
                        SoranForcesPlayer.SniperStateRecharging = false;
                    }
                }
            }

            public override void PostUpdateEquips(Player player)
            {
                var SoranForcesPlayer = player.GetModPlayer<SoranForcesPlayer>();

                if (SoranForcesPlayer.SniperStateActive)
                {
                    player.aggro -= (int)(player.aggro * 0.5f);
                    player.statDefense = player.statDefense *= 0.75f;
                }
                else if (SoranForcesPlayer.SniperStateRecharging)
                {
                    player.statDefense = player.statDefense *= 1.30f;
                }

                CooldownBarManager.Activate("SoranForcesCooldown", ModContent.Request<Texture2D>("CSE/Assets/Textures/Content/Items/Accessories/Enchantments/SoranForcesEnchant").Value, new(21, 142, 100),
                    () => SoranForcesPlayer.SniperStateCharge / (60f * 15), true, activeFunction: player.HasEffect<SoranForcesEffect>);
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = this.CreateRecipe();
            recipe.AddIngredient<VortexCommanderHat>();
            recipe.AddIngredient<VortexCommanderSuit>();
            recipe.AddIngredient<VortexCommanderGreaves>();
            recipe.AddIngredient<DolphinGun>();
            recipe.AddIngredient<LightningRifle>();
            recipe.AddIngredient<PGMUltimaRatioHecateII>();
            recipe.AddTile<EnchantedTreeSheet>();
            recipe.Register();
        }
    }

    public class SoranForcesPlayer : ModPlayer
    {
        private bool HadSoranForcesEnchantLastFrame;
        public bool HasSoranForcesEnchantThisFrame;
        public bool SniperStateActive = false;
        public int SniperStateCharge = 0;
        public bool SniperStateRecharging = true;

        public override void ResetEffects()
        {
            HasSoranForcesEnchantThisFrame = false;
        }

        public override void UpdateEquips()
        {
            if (!HadSoranForcesEnchantLastFrame && HasSoranForcesEnchantThisFrame)
            {
                SniperStateActive = false;
                SniperStateRecharging = true;
                SniperStateCharge = 0;
            }

            if (HadSoranForcesEnchantLastFrame && !HasSoranForcesEnchantThisFrame)
            {
                SniperStateActive = false;
                SniperStateRecharging = false;
                SniperStateCharge = 0;
                Player.ClearBuff(ModContent.BuffType<SniperCooldownBuff>());
                Player.ClearBuff(ModContent.BuffType<SniperBuff>());
            }

            HadSoranForcesEnchantLastFrame = HasSoranForcesEnchantThisFrame;
        }

        public override void OnEnterWorld()
        {
            SniperStateActive = false;
            SniperStateRecharging = true;
            SniperStateCharge = 0;
        }
    }
}