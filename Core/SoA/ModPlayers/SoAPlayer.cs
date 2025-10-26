using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CSE.Content.SoA.Buffs;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using CSE.Content.SoA.Projectiles;
using static CSE.Content.SoA.Accessories.Enchantments.FlariumEnchant;

namespace CSE.Core.SoA.ModPlayers
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class SoAPlayer : ModPlayer
    {
        public int rivalStreak;
        public int SpaceJunkCooldown;
        public override void PreUpdateMovement()
        {
            if (Player.HasBuff(ModContent.BuffType<SniperBuff>()))
            {
                Player.controlLeft = false;
                Player.controlRight = false;
                Player.controlJump = false;
                Player.controlDown = false;
                Player.controlUp = false;
                Player.velocity = Vector2.Zero;
                Player.gravity = 0f;
                Player.fallStart = (int)(Player.position.Y / 16f);
            }
        }

        public override void PostUpdateBuffs()
        {
            if (Player.HasBuff<NihilityPresenceBuff>())
            {
                Player.buffImmune[ModContent.Find<ModBuff>("CalamityMod", "Enraged").Type] = true;
                //Player.buffImmune[ModContent.Find<ModBuff>("CalamityMod", "RageMode").Type] = true;
                //Player.buffImmune[ModContent.Find<ModBuff>("CalamityMod", "AdrenalineMode").Type] = true;
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (proj.owner.ToPlayer().ownedProjectileCounts[ModContent.ProjectileType<FlariumTornado>()] < 6)
            {
                if (proj.owner.ToPlayer().HasEffect<FlariumEffect>() && Main.rand.NextFloat() < 0.05f && proj.damage > 0 && !proj.minion)
                {
                    Vector2 spawnPosition = proj.Center;
                    Projectile.NewProjectile(
                    proj.GetSource_FromThis(),
                    spawnPosition,
                    Vector2.Zero,
                    ModContent.ProjectileType<FlariumTornado>(),
                    100,
                    0,
                        proj.owner
                    );
                }
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (Player.HasBuff(ModContent.BuffType<SniperBuff>()))
            {
                damage *= 2.25f;
            }
            else if (Player.HasBuff(ModContent.BuffType<SniperCooldownBuff>()))
            {
                damage *= 0.75f;
            }
        }
    }
}