using CSE.Content.SoA.Projectiles;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using Microsoft.Xna.Framework;
using SacredTools.Content.Projectiles.Weapons.Dreamscape.Nihilus;
using SacredTools.Content.Projectiles.Weapons.Relic;
using SacredTools.Projectiles.Dreamscape;
using Terraria;
using Terraria.ModLoader;
using static CSE.Content.SoA.Accessories.Enchantments.FlariumEnchant;

namespace CSE.Core.SoA.Globals
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class SoAGlobalProjectile : GlobalProjectile
    {
        public int eerieBoost;
        public override bool InstancePerEntity => true;
        public override void AI(Projectile projectile)
        {
            if (eerieBoost > 0 && projectile.minion)
            {
                projectile.damage = (int)(projectile.damage * 1.1);
                eerieBoost--;
            }

            if (projectile.type == ModContent.ProjectileType<TenebrisLink2>())
            {
                if ((projectile.ai[1] += 1f) >= 20f)
                {
                    CSEUtils.HomeInOnNPC(projectile, true, 1600, 10, 2);
                }
            }

            if (projectile.type == ModContent.ProjectileType<SpookGrenade>())
            {
                projectile.velocity *= 1.05f;
                if ((projectile.ai[1] += 1f) >= 20f)
                {
                    CSEUtils.HomeInOnNPC(projectile, true, 700, 10, 2);
                }
            }

            if (projectile.type == ModContent.ProjectileType<DesperatioBullet>())
            {
                if ((projectile.ai[2] += 1f) >= 20f)
                {
                    CSEUtils.HomeInOnNPC(projectile, true, 200, 10, 3);
                }
            }
        }

        public override bool PreAI(Projectile projectile)
        {
            //if (projectile.type == ModContent.ProjectileType<DesperatioFlame>() && Main.rand.NextBool())
            //{
            //    projectile.damage = 0;
            //    projectile.hide = true;
            //    projectile.timeLeft = 0;
            //    projectile.netUpdate = true;
            //}
            return base.PreAI(projectile);
        }
        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.type == ModContent.ProjectileType<MalevolenceGrenadeMode>())
                projectile.damage = (int)(projectile.damage * 0.2f);
            if (projectile.type == ModContent.ProjectileType<DesperatioFlame>())
                projectile.damage = (int)(projectile.damage * 0.5f);
        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.owner.ToPlayer().ownedProjectileCounts[ModContent.ProjectileType<FlariumTornado>()] < 6)
            {
                if (projectile.owner.ToPlayer().HasEffect<FlariumEffect>() && Main.rand.NextFloat() < 0.15f && projectile.damage > 0 && !projectile.minion && projectile.tileCollide)
                {
                    Vector2 spawnPosition = projectile.Center;
                    Projectile.NewProjectile(
                        projectile.GetSource_FromThis(),
                        spawnPosition,
                        Vector2.Zero,
                        ModContent.ProjectileType<FlariumTornado>(),
                        FlariumEffect.BaseDamage(projectile.owner.ToPlayer()),
                        0,
                        projectile.owner
                    );
                }
            }

            return base.OnTileCollide(projectile, oldVelocity);
        }
    }
}