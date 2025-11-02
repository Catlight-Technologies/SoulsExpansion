using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using SacredTools.Content.Items.Accessories;
using SacredTools.Content.Items.Armor.Lapis;
using SacredTools.Items.Weapons;
using SacredTools.Items.Weapons.Special;
using FargowiltasSouls;
using SacredTools.Content.Buffs;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.SoA.Headers;
using CSE.Core;
using SacredTools.Common.Types;
using System;
using Fargowiltas.Content.Items.Tiles;

namespace CSE.Content.SoA.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class LapisEnchant : BaseEnchant
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

        public override Color nameColor => new(46, 66, 163);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<LapisSpeedEffect>(Item);
            player.AddEffect<LapisDefenseEffect>(Item);
        }

        public class LapisSpeedEffect : AccessoryEffect
        {
            public int lapisSpeedTimer;
            public override Header ToggleHeader => Header.GetHeader<FoundationsForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<LapisEnchant>();
            public override void PostUpdateEquips(Player player)
            {
                if (lapisSpeedTimer > 0)
                {
                    lapisSpeedTimer--;
                    player.moveSpeed += 0.25f;
                }
                player.moveSpeed += player.ForceEffect<LapisSpeedEffect>() ? 0.2f : 0.1f;
            }

            public override void OnHurt(Player player, Player.HurtInfo info)
            {
                if (!player.dead)
                {
                    lapisSpeedTimer = 300;
                }
            }
        }
        public class LapisDefenseEffect : AccessoryEffect
        {
            private int attackCounter = 0;
            public override Header ToggleHeader => Header.GetHeader<FoundationsForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<LapisEnchant>();

            public override void OnHitNPCEither(Player player, NPC target, NPC.HitInfo hitInfo, DamageClass damageClass, int baseDamage, Projectile projectile, Item item)
            {
                attackCounter++;

                if (attackCounter >= (player.ForceEffect<LapisDefenseEffect>() ? 3 : 5))
                {
                    attackCounter = 0;
                    player.AddLeveledBuff<LapisShieldBuff>(600);
                }
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = this.CreateRecipe();
            recipe.AddIngredient<LapisHelmet>();
            recipe.AddIngredient<LapisChest>();
            recipe.AddIngredient<LapisLegs>();
            recipe.AddIngredient<LapisPendant>();
            recipe.AddIngredient<LapisStaff>();
            recipe.AddIngredient<Haven>();
            recipe.AddTile<EnchantedTreeSheet>();
            recipe.Register();
        }
    }

    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public static class SoAUtils
    {
        //Just make that methods public
        //Why the fuck i have to copy them
        public static void AddLeveledBuff(this Player player, int type, int time, bool quiet = false, bool foodHack = false)
        {
            player.AddBuff(type, time, quiet, foodHack);
            if (BuffLoader.GetBuff(type) is LeveledBuff leveledBuff)
            {
                leveledBuff.BuffLevel(player) = Math.Max(leveledBuff.BuffLevel(player), 1);
            }
        }
        internal static void AddLeveledBuff<T>(this Player player, int time, bool quiet = false, bool foodHack = false) where T : LeveledBuff
        {
            player.AddLeveledBuff(ModContent.BuffType<T>(), time, quiet, foodHack);
        }
    }
}
