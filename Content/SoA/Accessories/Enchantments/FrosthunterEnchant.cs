using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using SacredTools.Content.Items.Armor.Decree;
using SacredTools.Content.Items.Accessories;
using SacredTools.Items.Weapons.Decree;
using SacredTools.Items.Weapons;
using FargowiltasSouls;
using CSE.Core;
using CSE.Content.SoA.Projectiles;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.SoA.Headers;
using static CSE.Content.SoA.Accessories.Forces.FrostburnForce;
using Fargowiltas.Content.Items.Tiles;
using static CSE.Content.SoA.Accessories.Souls.SoulOfTwoRealms;
using static CSE.Content.SoA.Accessories.Enchantments.BlightboneEnchant;

namespace CSE.Content.SoA.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class FrosthunterEnchant : BaseEnchant
    {

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = 1;
            Item.value = 50000;
        }

        public override Color nameColor => new(73, 94, 174);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<FrosthunterEffect>(Item);
        }
        public class FrosthunterEffect : AccessoryEffect
        {
            public int frosthunterCooldown = 0;
            public void CreateFrostExplosion(Vector2 pos, bool isCluster, Projectile proj, Player player)
            {
                float radius = isCluster ? 100f : 150f;
                int damage = (int)(BaseDamage(player) * 1.5f);
                float knockback = 3f;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && npc.Distance(pos) <= radius)
                    {
                        player.ApplyDamageToNPC(npc, damage, knockback, player.direction);
                        npc.AddBuff(player.ForceEffect<FrosthunterEffect>() ? BuffID.Frostburn2 : BuffID.Frostburn, 180);
                    }
                }

                Projectile.NewProjectile(proj.GetSource_FromThis(), pos, Vector2.Zero, ModContent.ProjectileType<FrosthunterExplosion>(), 0, 0);

                if (isCluster)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 clusterPos = pos + Main.rand.NextVector2Circular(radius * 0.5f, radius * 0.5f);
                        CreateSmallFrostExplosion(pos, proj, player);
                    }
                }
            }
            public void CreateSmallFrostExplosion(Vector2 pos, Projectile proj, Player player)
            {
                float radius = 60f;
                int damage = BaseDamage(player);
                float knockback = 1.5f;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && npc.Distance(pos) <= radius)
                    {
                        player.ApplyDamageToNPC(npc, damage, knockback, player.direction);
                        npc.AddBuff(player.ForceEffect<FrosthunterEffect>() ? BuffID.Frostburn2 : BuffID.Frostburn, 180);
                    }
                }

                Projectile.NewProjectile(proj.GetSource_FromThis(), pos, Vector2.Zero, ModContent.ProjectileType<FrosthunterExplosion>(), 0, 0);
            }

            public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
            {
                bool isCluster = frosthunterCooldown <= 0;

                CreateFrostExplosion(target.Center, isCluster, proj, player);

                if (isCluster)
                {
                    frosthunterCooldown = 120;
                }
            }

            public static int BaseDamage(Player player)
            {
                int dmg = 10;
                if (player.HasEffect<FrostburnEffect>() && !player.HasEffect<TwoRealmsEffect>())
                    dmg = 100;
                if (player.HasEffect<TwoRealmsEffect>())
                    dmg = 300;
                return (int)(dmg * player.ActualClassDamage(DamageClass.Ranged));
            }
            public override void PostUpdateEquips(Player player)
            {
                if (frosthunterCooldown > 0)
                {
                    frosthunterCooldown--;
                }
            }
            public override Header ToggleHeader => Header.GetHeader<FrostburnForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<FrosthunterEnchant>();
            public override bool ExtraAttackEffect => true;
        }

        public override int DamageTooltip(out DamageClass damageClass, out Color? tooltipColor, out int? scaling)
        {
            damageClass = DamageClass.Ranged;
            tooltipColor = null;
            scaling = null;
            return FrosthunterEffect.BaseDamage(Main.LocalPlayer);
        }
        public override void AddRecipes()
        {
            Recipe recipe = this.CreateRecipe();
            recipe.AddIngredient<FrosthunterHeaddress>();
            recipe.AddIngredient<FrosthunterWrappings>();
            recipe.AddIngredient<FrosthunterBoots>();
            recipe.AddIngredient<DecreeCharm>();
            recipe.AddIngredient<FrostGlobeStaff>();
            recipe.AddIngredient<FrostBeam>();
            recipe.AddTile<EnchantedTreeSheet>();
            recipe.Register();
        }
    }
}
