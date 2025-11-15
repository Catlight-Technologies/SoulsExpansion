using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using SacredTools.Content.Items.Armor.Lunar.Nebula;
using SacredTools.Items.Weapons.Lunatic;
using SacredTools.Content.Items.Weapons.Asthraltite;
using SacredTools.Content.Projectiles.Armors.Nuba;
using FargowiltasSouls;
using FargowiltasSouls.Core.Toggler;
using CSE.Core;
using CSE.Content.SoA.Headers;
using Fargowiltas.Content.Items.Tiles;

namespace CSE.Content.SoA.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class SavingGraceEnchant : BaseEnchant
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

        public override Color nameColor => new(206, 7, 221);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<SavingGraceEffect>(Item);
        }

        public class SavingGraceEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<SoranForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<SavingGraceEnchant>();

            public int cd;
            public override void PostUpdateMiscEffects(Player player)
            {
                if(cd > 0)
                {
                    cd--;
                }
            }
            public override void OnHitNPCEither(Player player, NPC target, NPC.HitInfo hitInfo, DamageClass damageClass, int baseDamage, Projectile projectile, Item item)
            {
                if (!target.immortal && Main.rand.NextBool(10) && cd < 1)
                {
                    float num2 = (float)Main.rand.Next(-35, 36) * 0.02f;
                    float num3 = (float)Main.rand.Next(-35, 36) * 0.02f;
                    num2 *= 10f;
                    num3 *= 10f;
                    int[] array0 =
                    {
                        ModContent.ProjectileType<NubaFlameDamage>(),
                        ModContent.ProjectileType<NubaFlameDefense>(),
                        ModContent.ProjectileType<NubaFlameHealth>(),
                    };
                    int[] array =
                    {
                        ModContent.ProjectileType<NubaFlameDamage>(),
                        ModContent.ProjectileType<NubaFlameDefense>(),
                        ModContent.ProjectileType<NubaFlameHealth>(),
                        ModContent.ProjectileType<NubaFlameMana>(),
                        ModContent.ProjectileType<NubaFlameSpeed>()
                    };
                    Projectile.NewProjectile(target.GetSource_OnHurt(player), target.Center.X, target.Center.Y, num2, num3, player.ForceEffect<SavingGraceEffect>() ? array[Main.rand.Next(5)] : array0[Main.rand.Next(3)], 0, 0f, projectile.owner);
                    cd += 5;
                }
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = this.CreateRecipe();
            recipe.AddIngredient<NubaHood>();
            recipe.AddIngredient<NubaChest>();
            recipe.AddIngredient<NubaRobe>();
            //recipe.AddIngredient<NubasBlessing>();
            recipe.AddIngredient<LunaticBurstStaff>();
            //recipe.AddIngredient<AsthralStaff>();
            recipe.AddTile<EnchantedTreeSheet>();
            recipe.Register();
        }
    }
}
