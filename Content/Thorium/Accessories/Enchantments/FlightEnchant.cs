using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.ArcaneArmor;
using ThoriumMod.Items.ThrownItems;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using CSE.Core;
using FargowiltasSouls.Core.ModPlayers;
using FargowiltasSouls.Core.Toggler;
using System;
using CSE.Content.Thorium.Projectiles;
using FargowiltasSouls;
using CSE.Content.Thorium.Headers;
using ThoriumMod.Items.BossBuriedChampion;
using static CSE.Content.Thorium.Accessories.Enchantments.YewWoodEnchant;
using static CSE.Content.Thorium.Forces.VanaheimForce;
using static CSE.Content.Thorium.Accessories.Souls.SoulOfYggdrasil;
using Fargowiltas.Content.Items.Tiles;

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
            public override Header ToggleHeader => Header.GetHeader<VanaheimForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<FlightEnchant>();

            public int featherTimer;
            public int featherCount;

            public override void PostUpdateEquips(Player player)
            {
                //FargoSoulsPlayer fargoPlayer = player.GetModPlayer<FargoSoulsPlayer>();
                //fargoPlayer.WingTimeModifier += 0.1f;

                featherTimer++;
                if(featherTimer > 60 && featherCount < 11)
                {
                    featherCount++;
                    featherTimer = 0;
                }
            }

            public static int BaseDamage(Player player)
            {
                int dmg = 15;
                if (player.HasEffect<frostburnEffect>())
                    dmg = 300;
                return (int)(dmg * player.ActualClassDamage(DamageClass.Throwing));
            }
            public override void OnHitByEither(Player player, NPC npc, Projectile proj)
            {
                for (int i = 0; i < featherCount; i++)
                {
                    Vector2 velocity = Vector2.Zero;
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, velocity.RotatedBy(2 * Math.PI / 10 * i), ModContent.ProjectileType<FeatherProj>(), BaseDamage(player), 0);
                }
            }
        }

        public override int DamageTooltip(out DamageClass damageClass, out Color? tooltipColor, out int? scaling)
        {
            damageClass = DamageClass.Throwing;
            tooltipColor = null;
            scaling = null;
            return FlightEffect.BaseDamage(Main.LocalPlayer);
        }
        public override void AddRecipes()
        {
            Recipe recipe = this.CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<FlightMask>());
            recipe.AddIngredient(ModContent.ItemType<FlightMail>());
            recipe.AddIngredient(ModContent.ItemType<FlightBoots>());
            recipe.AddIngredient(ModContent.ItemType<ChampionWing>());
            //recipe.AddIngredient(ModContent.ItemType<Flight>());
            recipe.AddIngredient(ModContent.ItemType<Bolas>(), 300);

            recipe.AddTile<EnchantedTreeSheet>();
            recipe.Register();
        }
    }
}
