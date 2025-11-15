using FargowiltasSouls;
using FargowiltasSouls.Assets.Textures;
using FargowiltasSouls.Content.Buffs.Boss;
using FargowiltasSouls.Content.Buffs.Eternity;
using FargowiltasSouls.Core.Systems;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSE.Content.Common.Projectiles.MutantEX
{
    public class MutantIchor : ModProjectile, IPixelatedPrimitiveRenderer
    {
        public override string Texture => FargoSoulsUtil.VanillaTextureProjectile(ProjectileID.GoldenShowerHostile);

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = -1;
            //Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 360;
            Projectile.hostile = true;
            Projectile.scale = 1f;
            CooldownSlot = ImmunityCooldownID.Bosses;
        }

        /*public override bool? CanDamage()
        {
            return Projectile.velocity.Y >= 0;
        }*/

        ref float Gravity => ref Projectile.ai[0];
        ref float TimeToFinishArc => ref Projectile.ai[1];

        public override void AI()
        {
            if (Projectile.localAI[1] == 0)
            {
                Projectile.localAI[1] = 1;
                SoundEngine.PlaySound(SoundID.Item17, Projectile.Center);
            }

            if (--TimeToFinishArc > 0)
            {
                Projectile.velocity.Y += Gravity;
            }
            else
            {
                Projectile.velocity.X *= 0.9f;
                if (Projectile.velocity.Y < 12f)
                    Projectile.velocity.Y += Gravity;
            }
            Projectile.Opacity = 1f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (target.FargoSouls().BetsyDashing)
                return;
            target.AddBuff(ModContent.BuffType<CurseoftheMoonBuff>(), 240);
                target.AddBuff(ModContent.BuffType<MutantFangBuff>(), 180);
        }

        public float WidthFunction(float completionRatio)
        {
            float baseWidth = Projectile.scale * Projectile.width * 1.3f;
            float width = MathHelper.SmoothStep(baseWidth, 3.5f, completionRatio);
            float start = 0.2f;
            if (completionRatio < start)
                width = MathHelper.SmoothStep(3.5f, baseWidth, completionRatio / start);
            return width;
        }

        public static Color ColorFunction(float completionRatio)
        {
            return Color.Lerp(new(250, 250, 0), Color.Transparent, completionRatio) * 0.7f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 auraPos = Projectile.Center + new Vector2(0f, Projectile.gfxOffY);
            float radius = Projectile.width * Projectile.scale / 2;
            var blackTile = TextureAssets.MagicPixel;
            var diagonalNoise = FargoAssets.WavyNoise;
            if (!blackTile.IsLoaded || !diagonalNoise.IsLoaded)
                return false;
            var maxOpacity = Projectile.Opacity * 1f;

            Vector4 shaderColor = new(250, 250, 0, 1);
            ManagedShader borderShader = ShaderManager.GetShader("FargowiltasSouls.IchorDropletShader");
            borderShader.TrySetParameter("radius", radius);
            borderShader.TrySetParameter("midColor", shaderColor);
            borderShader.TrySetParameter("anchorPoint", auraPos);
            borderShader.TrySetParameter("screenPosition", Main.screenPosition);
            borderShader.TrySetParameter("screenSize", Main.ScreenSize.ToVector2());
            borderShader.TrySetParameter("maxOpacity", maxOpacity);
            borderShader.TrySetParameter("direction", Projectile.velocity.SafeNormalize(Vector2.UnitY));

            Main.spriteBatch.GraphicsDevice.Textures[1] = diagonalNoise.Value;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearWrap, DepthStencilState.None, Main.Rasterizer, borderShader.WrappedEffect, Main.GameViewMatrix.TransformationMatrix);
            Rectangle rekt = new(Main.screenWidth / 2, Main.screenHeight / 2, Main.screenWidth, Main.screenHeight);
            Main.spriteBatch.Draw(blackTile.Value, rekt, null, default, 0f, blackTile.Value.Size() * 0.5f, 0, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        public void RenderPixelatedPrimitives(SpriteBatch spriteBatch)
        {
            ManagedShader shader = ShaderManager.GetShader("FargowiltasSouls.BlobTrail");
            FargoAssets.FadedStreak.Value.SetTexture1();
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, new(WidthFunction, ColorFunction, _ => Projectile.Size * 0.5f, Pixelate: true, Shader: shader), 5);
        }
    }
}