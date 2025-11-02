using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using SacredTools.Content.Items.Armor.Blightbone;
using SacredTools.Content.Items.Accessories;
using SacredTools.Content.Items.Weapons.Dreadfire;
using FargowiltasSouls;
using System;
using CSE.Core;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.SoA.Headers;
using CSE.Content.SoA.Projectiles;
using Fargowiltas.Content.Items.Tiles;
using static CSE.Content.SoA.Accessories.Forces.FrostburnForce;
using static CSE.Content.SoA.Accessories.Souls.SoulOfTwoRealms;

namespace CSE.Content.SoA.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class BlightboneEnchant : BaseEnchant
    {

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = 3;
            Item.value = 100000;
        }

        public override Color nameColor => new(124, 10, 10);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<BlightboneEffect>(Item);
        }
        public class BlightboneEffect : AccessoryEffect
        {
            public int boneCD;
            public override Header ToggleHeader => Header.GetHeader<FrostburnForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<BlightboneEnchant>();
            public override bool ExtraAttackEffect => true;
            public override void TryAdditionalAttacks(Player player, int damage, DamageClass damageType)
            {
                if (boneCD > 0)
                {
                    return;
                }

                boneCD = 30;
                Vector2 center = player.Center;
                Vector2 vector = Vector2.Normalize(Main.MouseWorld - center);
                for (int i = 0; i < (player.ForceEffect<BlightboneEffect>() ? 3 : 1); i++)
                {
                    Projectile.NewProjectile(GetSource_EffectItem(player), center, vector.RotatedByRandom(Math.PI / 6.0) * Main.rand.NextFloat(6f, 10f) * 2, ModContent.ProjectileType<Blightbone>(), BaseDamage(player), 9f, player.whoAmI);
                }
            }
            public static int BaseDamage(Player player)
            {
                int dmg = 30;
                if (player.HasEffect<FrostburnEffect>() && !player.HasEffect<TwoRealmsEffect>())
                    dmg = 90;
                if (player.HasEffect<TwoRealmsEffect>())
                    dmg = 130;
                return (int)(dmg * player.ActualClassDamage(DamageClass.Throwing));
            }
            public override void PostUpdateEquips(Player player)
            {
                if (boneCD > 0)
                {
                    boneCD--;
                }
            }
        }

        public override int DamageTooltip(out DamageClass damageClass, out Color? tooltipColor, out int? scaling)
        {
            damageClass = DamageClass.Throwing;
            tooltipColor = null;
            scaling = null;
            return BlightboneEffect.BaseDamage(Main.LocalPlayer);
        }
        public override void AddRecipes()
        {
            Recipe recipe = this.CreateRecipe();
            recipe.AddIngredient<BlightMask>();
            recipe.AddIngredient<BlightChest>();
            recipe.AddIngredient<BlightLegs>();
            recipe.AddIngredient<PumpkinAmulet>();
            recipe.AddIngredient<FeatherHairpin>();
            recipe.AddIngredient<PumpGlove>();
            recipe.AddTile<EnchantedTreeSheet>();
            recipe.Register();
        }
    }
}
