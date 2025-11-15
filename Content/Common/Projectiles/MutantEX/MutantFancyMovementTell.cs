using CSE.Content.Common.Bosses.MutantEX;
using FargowiltasSouls;
using FargowiltasSouls.Assets.Textures;
using FargowiltasSouls.Core.Systems;
using Luminance.Common.Utilities;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSE.Content.Common.Projectiles.MutantEX
{
    public class MutantFancyMovementTell : ModProjectile, IPixelatedPrimitiveRenderer
    {
        public override string Texture => "FargowiltasSouls/Assets/Textures/Content/Projectiles/GlowRing";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            CooldownSlot = ImmunityCooldownID.Bosses;

            Projectile.extraUpdates = 1;

            Projectile.timeLeft = 60 * 10;
        }

        public override bool? CanDamage() => false;

        // listen i dont have enough array fields and i dont want to duplicate this proj code ok
        ref float ModeAndFullRotationIncrement => ref Projectile.ai[0];
        ref float MutantID => ref Projectile.ai[1];
        ref float Flip => ref Projectile.ai[2];

        float Timer;
        float Distance;
        float OriginX;
        float OriginY;

        int TimeToPersistAfterStopping = 30 * 2; //todo: remove, unneeded

        public override void AI()
        {
            if (ModeAndFullRotationIncrement == 0) //spikevine
            {
                if (Timer < 60)
                {
                    Projectile.velocity *= 0.97f;
                    //Projectile.velocity = Projectile.velocity.RotateTowards(Projectile.DirectionTo(target.Center).ToRotation(), 0.1f);
                }
                else if (Timer < 110)
                {
                    Projectile.tileCollide = true;
                    Vector2 dir = Projectile.velocity.SafeNormalize(Vector2.Zero);
                    Projectile.velocity += dir * 1f;
                    if (Projectile.velocity.Length() > 40)
                        Projectile.velocity = dir * 40;
                }
                float rotationModifier = (WorldSavingSystem.MasochistModeReal ? 0.026f : 0.0005f) * Flip;
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver2 * rotationModifier);

                if (Projectile.velocity != Vector2.Zero)
                {
                    const float maxExpectedDistance = 1140;
                    const float numberOfUndulations = 5;

                    Vector2 waveAmplitude = 32f * Flip * Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2);

                    // need to undo the previous tick's undulation so that we're back along the original "line" of the real velocity
                    float oldUndulation = (float)Math.Sin(MathHelper.TwoPi * Distance / maxExpectedDistance * numberOfUndulations);
                    Projectile.position -= waveAmplitude * oldUndulation;

                    Distance += Projectile.velocity.Length();

                    float undulation = (float)Math.Sin(MathHelper.TwoPi * Distance / maxExpectedDistance * numberOfUndulations);
                    Projectile.position += waveAmplitude * undulation;
                }
            }
            else //helix
            {
                if (Projectile.velocity != Vector2.Zero)
                {
                    const float maxExpectedDistance = 1200;
                    float numberOfUndulations = WorldSavingSystem.MasochistModeReal ? 1.6f : 2f;
                    float baseAmp = WorldSavingSystem.MasochistModeReal ? 120f : 80f;
                    float ampBonus = WorldSavingSystem.MasochistModeReal ? 4f : 1.75f;

                    // need to undo the previous tick's undulation so that we're back along the original "line" of the real velocity
                    float oldWaveAmp = baseAmp * (1f + ampBonus * Distance / maxExpectedDistance);
                    Vector2 oldWaveAmplitude = oldWaveAmp * Flip * Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2);
                    float oldUndulation = (float)Math.Sin(MathHelper.TwoPi * Distance / maxExpectedDistance * numberOfUndulations);
                    Projectile.position -= oldWaveAmplitude * oldUndulation;

                    if (Distance == 0) //store because mutant can move before attack ends
                    {
                        OriginX = Projectile.Center.X;
                        OriginY = Projectile.Center.Y;
                    }
                    else
                    {
                        //make the entire pattern rotate around mutant
                        Vector2 mutantOriginalPos = new Vector2(OriginX, OriginY);
                        Vector2 mutantToMe = Projectile.Center - mutantOriginalPos;
                        Projectile.Center = mutantOriginalPos + mutantToMe.RotatedBy(ModeAndFullRotationIncrement);
                        Projectile.velocity = mutantToMe.SafeNormalize(Vector2.Zero) * Projectile.velocity.Length();

                        for (int i = 0; i < Math.Min(Timer, ProjectileID.Sets.TrailCacheLength[Projectile.type]); i++)
                        {
                            Vector2 mutantToOldPos = Projectile.oldPos[i] - mutantOriginalPos + Projectile.Size / 2;
                            Projectile.oldPos[i] = mutantOriginalPos + mutantToOldPos.RotatedBy(ModeAndFullRotationIncrement) - Projectile.Size / 2;
                        }
                    }

                    Distance += Projectile.velocity.Length();

                    // yes we need to recalculate these entirely
                    float waveAmp = baseAmp * (1f + ampBonus * Distance / maxExpectedDistance);
                    Vector2 waveAmplitude = waveAmp * Flip * Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2);
                    float undulation = (float)Math.Sin(MathHelper.TwoPi * Distance / maxExpectedDistance * numberOfUndulations);
                    Projectile.position += waveAmplitude * undulation;
                }
            }

            Timer++;

            int mutantID = (int)MutantID;
            if (Timer >= 60 && mutantID.IsWithinBounds(Main.maxNPCs) && Projectile.velocity != Vector2.Zero)
            {
                NPC mutant = Main.npc[mutantID];
                var p = FargoSoulsUtil.ProjectileExists(mutant.As<MutantEXBoss>().ritualProj, ModContent.ProjectileType<MutantRitual>());
                if (p != null)
                {
                    if (Projectile.Distance(p.Center) >= p.As<MutantRitual>().threshold - 60)
                    {
                        Projectile.velocity *= 0;
                    }
                }
            }

            if (Projectile.velocity == Vector2.Zero && Projectile.position == Projectile.oldPos[ProjectileID.Sets.TrailCacheLength[Projectile.type] - 1])
            {
                Projectile.timeLeft = 0;
            }
        }

        public float WidthFunction(float completionRatio)
        {
            float baseWidth = Projectile.scale * Projectile.width * 1.3f;
            return MathHelper.SmoothStep(baseWidth, 3.5f, completionRatio);
        }

        static Color MyColor => new Color(51, 255, 154);

        public static Color ColorFunction(float completionRatio)
        {
            return Color.Lerp(MyColor, Color.Transparent, completionRatio) * 0.7f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int num156 = TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new(0, y3, texture.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            Color color = MyColor;
            float scale = Projectile.scale * 0.1f;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, Projectile.rotation, origin2, scale, SpriteEffects.None, 0);
            return false;
        }

        public void RenderPixelatedPrimitives(SpriteBatch spriteBatch)
        {
            ManagedShader shader = ShaderManager.GetShader("FargowiltasSouls.BlobTrail");
            FargoAssets.FadedStreak.Value.SetTexture1();
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, new(WidthFunction, ColorFunction, _ => Projectile.Size * 0.5f, Pixelate: true, Shader: shader), 30);
        }
    }
}

