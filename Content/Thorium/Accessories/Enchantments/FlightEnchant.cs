using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.ArcaneArmor;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.ThrownItems;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using ThoriumMod.Items.BossBuriedChampion;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler.Content;
using CSE.Core;
using FargowiltasSouls.Core.ModPlayers;
using FargowiltasSouls.Core.Toggler;
using System;
using CSE.Content.Thorium.Projectiles;
using FargowiltasSouls;

namespace CSE.Content.Thorium.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class FlightEnchant : BaseEnchant
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = 2;
            Item.value = 60000;
        }

        public override Color nameColor => new(105, 63, 163);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<FlightEffect>(Item);
        }

        public class FlightEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<FlightMasteryHeader>();
            public override int ToggleItemType => ModContent.ItemType<FlightEnchant>();

            public int featherTimer;
            public int featherCount;

            public override void PostUpdateEquips(Player player)
            {
                FargoSoulsPlayer fargoPlayer = player.GetModPlayer<FargoSoulsPlayer>();
                fargoPlayer.WingTimeModifier += 0.1f;

                featherTimer++;
                if(featherTimer > 60 && featherCount < 11)
                {
                    featherCount++;
                    featherTimer = 0;
                }
            }
            public override void OnHitByEither(Player player, NPC npc, Projectile proj)
            {
                for (int i = 0; i < featherCount; i++)
                {
                    Vector2 velocity = Vector2.Zero;
                    Projectile.NewProjectile(player.GetSource_GiftOrReward(), player.Center, velocity.RotatedBy(2 * Math.PI / 10 * i), ModContent.ProjectileType<FeatherProj>(), (int)player.GetDamage(DamageClass.Generic).ApplyTo(player.FargoSouls().FlightMasterySoul ? 300 : 30), 0);
                }
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = this.CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<FlightMask>());
            recipe.AddIngredient(ModContent.ItemType<FlightMail>());
            recipe.AddIngredient(ModContent.ItemType<FlightBoots>());
            //recipe.AddIngredient(ModContent.ItemType<ChampionWing>());
            //recipe.AddIngredient(ModContent.ItemType<Flight>());
            recipe.AddIngredient(ModContent.ItemType<Bolas>(), 300);

            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
