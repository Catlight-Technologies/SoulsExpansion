using FargowiltasSouls;
using FargowiltasSouls.Common.Graphics.Particles;
using FargowiltasSouls.Content.Buffs.Boss;
using FargowiltasSouls.Content.Buffs.Eternity;
using FargowiltasSouls.Core.Systems;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSE.Content.Common.Projectiles.MutantEX
{
    public class MutantCursedFlame : ModProjectile
    {
        public override string Texture => FargoSoulsUtil.VanillaTextureProjectile(ProjectileID.CursedFlameHostile);

        const int hitboxSize = 64;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 8000;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = hitboxSize;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.Opacity = 0f;
            Projectile.scale = 1f;
            CooldownSlot = ImmunityCooldownID.Bosses;

            Projectile.FargoSouls().GrazeCheck =
                Projectile =>
                {
                    float dummy = 0f;
                    if (!spawn)
                        return false;
                    if (CanDamage() != false && Collision.CheckAABBvLineCollision(Main.LocalPlayer.Hitbox.TopLeft(), Main.LocalPlayer.Hitbox.Size(), origin,
                        Projectile.Center, hitboxSize / 2 * Projectile.scale + Main.LocalPlayer.FargoSouls().GrazeRadius * 2f + Player.defaultHeight, ref dummy))
                    {
                        return true;
                    }
                    return false;
                };
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (!spawn)
                return false;
            float dummy = 0;
            return Collision.CheckAABBvLineCollision(Main.LocalPlayer.Hitbox.TopLeft(), Main.LocalPlayer.Hitbox.Size(), origin,
                Projectile.Center, hitboxSize / 2 * Projectile.scale, ref dummy);
        }

        bool spawn;
        Vector2 origin;

        ref float TelegraphTimer => ref Projectile.ai[0];
        ref float OriginalTelegraphTime => ref Projectile.localAI[0];

        public override void AI()
        {
            if (!spawn)
            {
                spawn = true;
                origin = Projectile.Center;
                OriginalTelegraphTime = TelegraphTimer;
            }

            if (Projectile.FargoSouls().GrazeCD > 15)
                Projectile.FargoSouls().GrazeCD = 15;

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (--TelegraphTimer > 0)
            {
                Projectile.position -= Projectile.velocity;
                Projectile.Opacity = 1f - TelegraphTimer / OriginalTelegraphTime;
                Projectile.timeLeft = 40;
            }
            else if (origin != Projectile.Center)
            {
                Projectile.Opacity = System.Math.Max(0f, Projectile.Opacity - 1f / 10f);

                //please dont ask
                float dist = Projectile.Distance(origin);
                const float jump = 8;
                for (float i = 0; i < dist; i += jump)
                {
                    Vector2 spawnPos = origin + Projectile.velocity.SafeNormalize(Vector2.Zero) * i;
                    Particle p = new ExpandingBloomParticle(spawnPos + Main.rand.NextVector2Circular(jump / 2, jump / 2),
                        Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(2)), new(96, 248, 2), startScale: Vector2.Zero, endScale: Vector2.One * 2f, lifetime: 30);
                    p.Spawn();
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (target.FargoSouls().BetsyDashing)
                return;
            target.AddBuff(ModContent.BuffType<CurseoftheMoonBuff>(), 240);
                target.AddBuff(ModContent.BuffType<MutantFangBuff>(), 180);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (spawn)
            {
                Asset<Texture2D> line = TextureAssets.Extra[178];
                float opacity = Projectile.Opacity;
                Main.EntitySpriteDraw(line.Value, origin - Main.screenPosition + new Vector2(0, Projectile.gfxOffY),
                    null, new Color(150, 255, 25) * opacity, Projectile.rotation,
                    new Vector2(0, line.Height() * 0.5f), new Vector2(8f, 12f), SpriteEffects.None);
            }
            return false;
        }
    }
}