using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using SacredTools.Content.Items.Armor.Lunar.Solar;
using SacredTools.Items.Weapons.Lunatic;
using SacredTools.Buffs;
using CSE.Core;
using FargowiltasSouls.Core.Toggler;
using CSE.Core.SoA.ModPlayers;
using CSE.Content.SoA.Headers;
using CSE.Content.SoA.Buffs;
using Fargowiltas.Content.Items.Tiles;

namespace CSE.Content.SoA.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class UnrelentingRivalEnchant : BaseEnchant
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = 11;
            Item.value = 350000;
        }

        public override Color nameColor => new(249, 75, 7);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<UnrelentingRivalEffect>(Item);
        }
        public class UnrelentingRivalEffect : AccessoryEffect
        {
            public int rivalKillCount = 0;
            public int rivalTimer = 0;

            public int rivalKillCountSniper = 0;
            public int rivalTimerSniper = 0;
            public override Header ToggleHeader => Header.GetHeader<SoranForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<UnrelentingRivalEnchant>();

            public override void PostUpdateEquips(Player player)
            {
                player.GetModPlayer<SoAPlayer>().rivalStreak = rivalKillCount;

                if (rivalKillCount > 0)
                {
                    player.AddBuff(ModContent.BuffType<RivalBuff>(), 60);
                    rivalTimer++;
                    player.GetDamage<GenericDamageClass>() += 0.2f * rivalKillCount;
                }
                if (rivalKillCountSniper > 0)
                {
                    player.AddBuff(ModContent.BuffType<RivalBuff>(), 60);
                    rivalTimerSniper++;
                    player.GetDamage<GenericDamageClass>() += 0.5f * rivalKillCount;
                }

                if (rivalTimer >= 300)
                {
                    rivalKillCount--;
                    rivalTimer = 0;
                }
                if (rivalTimerSniper >= 300)
                {
                    rivalKillCount--;
                    rivalTimerSniper = 0;
                }
            }

            public override void OnHitNPCEither(Player player, NPC target, NPC.HitInfo hitInfo, DamageClass damageClass, int baseDamage, Projectile projectile, Item item)
            {
                if (target.life <= 0 && !target.friendly && target.type != NPCID.TargetDummy && !player.HasBuff<SniperState>())
                {
                    if (rivalKillCount < 5)
                    {
                        rivalKillCount++;
                    }
                }
                if (target.life <= 0 && !target.friendly && target.type != NPCID.TargetDummy && player.HasBuff<SniperState>())
                {
                    if (rivalKillCountSniper < 5)
                    {
                        rivalKillCountSniper++;
                    }
                }
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = this.CreateRecipe();
            recipe.AddIngredient<BlazingBruteHelm>();
            recipe.AddIngredient<BlazingBrutePlate>();
            recipe.AddIngredient<BlazingBruteLegs>();
            recipe.AddIngredient<Nyanmere>();
            recipe.AddIngredient<StarShower>();
            recipe.AddIngredient<AsteroidShower>();
            recipe.AddTile<EnchantedTreeSheet>();
            recipe.Register();
        }
    }
}
