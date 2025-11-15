using FargowiltasSouls;
using FargowiltasSouls.Assets.Sounds;
using FargowiltasSouls.Assets.Textures;
using FargowiltasSouls.Content.Buffs.Boss;
using FargowiltasSouls.Content.Buffs.Eternity;
using FargowiltasSouls.Content.Projectiles.Deathrays;
using FargowiltasSouls.Core.Systems;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSE.Content.Common.Projectiles.MutantEX
{
	public class MutantWofDeathray : BaseDeathray, IPixelatedPrimitiveRenderer
    {
        public override string Texture => FargoAssets.GetAssetString("Content/Projectiles/Deathrays", "PhantasmalDeathrayWOF");
        public MutantWofDeathray() : base(1200) { }

        public override bool? CanDamage()
        {
            return Projectile.scale >= 1;
        }

        public override bool CanHitPlayer(Player target)
        {
            return target.hurtCooldowns[1] == 0;
        }

        bool spawn;

        public override void AI()
        {
            if (!spawn)
            {
                spawn = true;
                maxTime = Projectile.ai[2];
                Projectile.timeLeft = (int)Projectile.ai[2];
            }


            Vector2? vector78 = null;
            if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
            {
                Projectile.velocity = -Vector2.UnitY;
            }
            Projectile parent = FargoSoulsUtil.ProjectileExists(FargoSoulsUtil.GetProjectileByIdentity(Projectile.owner, Projectile.ai[1]), ModContent.ProjectileType<MutantWofPart>());
            if (parent != null)
            {
                Projectile.Center = parent.Center + (parent.width - 36 * parent.scale) * Vector2.UnitX.RotatedBy(parent.rotation);
            }
            if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
            {
                Projectile.velocity = -Vector2.UnitY;
            }
            if (Projectile.localAI[0] == 0f)
            {
                SoundEngine.PlaySound(FargosSoundRegistry.Zombie104, Projectile.Center);
            }
            float maxScale = 2f;
            if (WorldSavingSystem.MasochistModeReal)
                maxScale = 10f;
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] >= maxTime)
            {
                Projectile.Kill();
                return;
            }
            Projectile.scale = (float)Math.Sin(Projectile.localAI[0] * 3.14159274f / maxTime) * 5f * maxScale;
            if (Projectile.scale > maxScale)
            {
                Projectile.scale = maxScale;
            }
            //float num804 = Projectile.velocity.ToRotation();
            //num804 += Projectile.ai[0];
            //Projectile.rotation = num804 - 1.57079637f;

            Projectile.velocity = parent.rotation.ToRotationVector2();
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

            float num805 = 3f;
            float num806 = Projectile.width;
            Vector2 samplingPoint = Projectile.Center;
            if (vector78.HasValue)
            {
                samplingPoint = vector78.Value;
            }
            float[] array3 = new float[(int)num805];
            //Collision.LaserScan(samplingPoint, Projectile.velocity, num806 * Projectile.scale, 3000f, array3);
            for (int i = 0; i < array3.Length; i++)
                array3[i] = 3000f;
            float num807 = 0f;
            int num3;
            for (int num808 = 0; num808 < array3.Length; num808 = num3 + 1)
            {
                num807 += array3[num808];
                num3 = num808;
            }
            num807 /= num805;
            float amount = 0.5f;
            Projectile.localAI[1] = MathHelper.Lerp(Projectile.localAI[1], num807, amount);
            Vector2 vector79 = Projectile.Center + Projectile.velocity * (Projectile.localAI[1] - 14f);
            for (int num809 = 0; num809 < 2; num809 = num3 + 1)
            {
                float num810 = Projectile.velocity.ToRotation() + (Main.rand.NextBool(2) ? -1f : 1f) * 1.57079637f;
                float num811 = (float)Main.rand.NextDouble() * 2f + 2f;
                Vector2 vector80 = new((float)Math.Cos((double)num810) * num811, (float)Math.Sin((double)num810) * num811);
                int num812 = Dust.NewDust(vector79, 0, 0, DustID.CopperCoin, vector80.X, vector80.Y, 0, default, 1f);
                Main.dust[num812].noGravity = true;
                Main.dust[num812].scale = 1.7f;
                num3 = num809;
            }
            if (Main.rand.NextBool(5))
            {
                Vector2 value29 = Projectile.velocity.RotatedBy(1.5707963705062866, default) * ((float)Main.rand.NextDouble() - 0.5f) * Projectile.width;
                int num813 = Dust.NewDust(vector79 + value29 - Vector2.One * 4f, 8, 8, DustID.CopperCoin, 0f, 0f, 100, default, 1.5f);
                Dust dust = Main.dust[num813];
                dust.velocity *= 0.5f;
                Main.dust[num813].velocity.Y = -Math.Abs(Main.dust[num813].velocity.Y);
            }
            //DelegateMethods.v3_1 = new Vector3(0.3f, 0.65f, 0.7f);
            //Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.localAI[1], (float)Projectile.width * Projectile.scale, new Utils.PerLinePoint(DelegateMethods.CastLight));

            /*if (FargoSoulsUtil.HostCheck)
            {
                Vector2 offset = new Vector2(Main.rand.NextFloat(array3[0]), Main.rand.NextFloat(-22 * Projectile.scale, 22 * Projectile.scale));
                Projectile.NewProjectile(Projectile.Center + offset, Vector2.Zero, 
                    ModContent.ProjectileType<WOFBlast2>(), 0, 0f, Main.myPlayer, 0f, Projectile.whoAmI);
            }*/

            const int increment = 100;
            for (int i = 0; i < array3[0]; i += increment)
            {
                if (!Main.rand.NextBool(5))
                    continue;
                float offset = i + Main.rand.NextFloat(-increment, increment);
                if (offset < 0)
                    offset = 0;
                if (offset > array3[0])
                    offset = array3[0];
                int d = Dust.NewDust(Projectile.position + Projectile.velocity * offset,
                    Projectile.width, Projectile.height, DustID.Frost, 0f, 0f, 0, default, 1.5f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 3f;
            }
        }

        public override void PostAI()
        {
            base.PostAI();
            Projectile.hide = true;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            //empty, disables inherited function
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<CurseoftheMoonBuff>(), 240);
            if (WorldSavingSystem.EternityMode)
                target.AddBuff(ModContent.BuffType<MutantFangBuff>(), 180);
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }

        public override bool PreDraw(ref Color lightColor) => false;

        public float WidthFunction(float _) => Projectile.width * Projectile.scale * (WorldSavingSystem.MasochistModeReal ? 0.7f : 2.2f);

        public static Color ColorFunction(float _)
        {
            Color color = Color.Blue; //new(232, 140, 240);
            color.A = 150;
            return color;
        }
        public void RenderPixelatedPrimitives(SpriteBatch spriteBatch)
        {
            ManagedShader shader = ShaderManager.GetShader("FargowiltasSouls.WoFDeathray");

            // Get the laser end position.
            Vector2 laserEnd = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitY) * Projectile.localAI[1] * 1.5f;

            // Create 8 points that span across the draw distance from the projectile center.
            Vector2 initialDrawPoint = Projectile.Center - Projectile.velocity * 15f;
            Vector2[] baseDrawPoints = new Vector2[8];
            for (int i = 0; i < baseDrawPoints.Length; i++)
                baseDrawPoints[i] = Vector2.Lerp(initialDrawPoint, laserEnd, i / (float)(baseDrawPoints.Length - 1f));


            // Set shader parameters.
            shader.TrySetParameter("mainColor", new Color(240, 220, 240, 0));
            FargoSoulsUtil.SetTexture1(FargoAssets.DeviInnerStreak.Value);
            shader.TrySetParameter("stretchAmount", 0.25);
            shader.TrySetParameter("scrollSpeed", 2f);
            shader.TrySetParameter("uColorFadeScaler", 0.8f);
            shader.TrySetParameter("useFadeIn", true);
            shader.TrySetParameter("realopacity", 1); // do not change this.

            PrimitiveRenderer.RenderTrail(baseDrawPoints, new(WidthFunction, ColorFunction, Pixelate: true, Shader: shader), WorldSavingSystem.MasochistModeReal ? 15 : 15);
        }
    }
}