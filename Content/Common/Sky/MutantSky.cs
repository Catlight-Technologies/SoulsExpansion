using CSE.Content.Common.Bosses.MutantEX;
using FargowiltasSouls;
using FargowiltasSouls.Assets.Textures;
using FargowiltasSouls.Core.Globals;
using FargowiltasSouls.Core.Systems;
using Luminance.Common.Utilities;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace CSE.Content.Common.Sky
{
    public class MutantEXSky : CustomSky
    {
        private bool isActive = false;
        private float intensity = 0f;
        private float lifeIntensity = 0f;
        private float shaderIntensity = 0f;
        private float specialColorLerp = 0f;
        private float extraElementsIntensity = 0f;
        private Color? specialColor = null;
        private int delay = 0;
        const int amountOfStatic = 200;
        private readonly int[] xPos = new int[amountOfStatic];
        private readonly int[] yPos = new int[amountOfStatic];

        public override void Update(GameTime gameTime)
        {
            const float increment = 0.01f;

            bool useSpecialColor = false;

            if (FargoSoulsUtil.BossIsAlive(ref EModeGlobalNPC.mutantBoss, ModContent.NPCType<MutantEXBoss>())
                && (Main.npc[EModeGlobalNPC.mutantBoss].ai[0] < 0 || Main.npc[EModeGlobalNPC.mutantBoss].ai[0] >= 10))
            {
                intensity += increment;
                lifeIntensity = Main.npc[EModeGlobalNPC.mutantBoss].ai[0] < 0 ? 1f : 1f - (float)Main.npc[EModeGlobalNPC.mutantBoss].life / Main.npc[EModeGlobalNPC.mutantBoss].lifeMax;

                void ChangeColorIfDefault(Color color) //waits for bg to return to default first
                {
                    if (specialColor == null)
                        specialColor = color;
                    if (specialColor != null && specialColor == color)
                        useSpecialColor = true;
                }

                bool raiseShader = true;

                // reduce aurora and tv bands intensity during final spark because spark shader makes contrast go way up so these become really dazzling
                if (MutantEXBoss.DoingSuperFinalSpark)
                {
                    extraElementsIntensity -= increment;
                    if (extraElementsIntensity < 0.5f)
                        extraElementsIntensity = 0.5f;
                }
                else
                {
                    extraElementsIntensity += increment;
                    if (extraElementsIntensity > 1)
                        extraElementsIntensity = 1;
                }

                switch ((int)Main.npc[EModeGlobalNPC.mutantBoss].ai[0])
                {
                    case -5:
                        if (Main.npc[EModeGlobalNPC.mutantBoss].ai[2] >= 420)
                            ChangeColorIfDefault(FargoSoulsUtil.AprilFools ? new Color(255, 180, 50) : Color.Cyan);
                        break;

                    case 10: //p2 transition, smash to black
                        useSpecialColor = true;
                        specialColor = Color.Black;
                        specialColorLerp = 1f;
                        raiseShader = false;
                        break;

                    case 27: //ray fan
                        ChangeColorIfDefault(Color.Red);
                        break;

                    case 36: //slime rain
                        if (WorldSavingSystem.MasochistModeReal && Main.npc[EModeGlobalNPC.mutantBoss].ai[2] > 180 * 3 - 60)
                            ChangeColorIfDefault(Color.Blue);
                        break;

                    case 44: //empress
                        ChangeColorIfDefault(Color.DeepPink);
                        break;

                    case 48: //queen slime
                        ChangeColorIfDefault(Color.Purple);
                        break;

                    default:
                        break;
                }

                if (raiseShader)
                {
                    shaderIntensity += increment;
                    if (shaderIntensity > 1f)
                        shaderIntensity = 1f;
                }

                if (intensity > 1f)
                    intensity = 1f;
            }
            else
            {
                lifeIntensity -= increment;
                if (lifeIntensity < 0f)
                    lifeIntensity = 0f;

                shaderIntensity -= increment;
                if (shaderIntensity < 0f)
                    shaderIntensity = 0f;

                specialColorLerp -= increment * 2;
                if (specialColorLerp < 0)
                    specialColorLerp = 0;

                extraElementsIntensity -= increment;
                if (extraElementsIntensity < 0)
                    extraElementsIntensity = 0;

                intensity -= increment;
                if (intensity < 0f)
                {
                    intensity = 0f;
                    shaderIntensity = 0f;
                    lifeIntensity = 0f;
                    specialColorLerp = 0f;
                    specialColor = null;
                    delay = 0;
                    Deactivate();
                    return;
                }
            }

            if (useSpecialColor)
            {
                specialColorLerp += increment * 2;
                if (specialColorLerp > 1)
                    specialColorLerp = 1;
            }
            else
            {
                specialColorLerp -= increment * 2;
                if (specialColorLerp < 0)
                {
                    specialColorLerp = 0;
                    specialColor = null;
                }
            }
        }

        private Color ColorToUse(ref float opacity)
        {
            Color color = FargoSoulsUtil.AprilFools ? Color.OrangeRed : new(50, 255, 200);
            opacity = intensity * 1f;

            if (specialColorLerp > 0 && specialColor != null)
            {
                color = Color.Lerp(color, (Color)specialColor, specialColorLerp);
                if (specialColor == Color.Black)
                    opacity = System.Math.Min(1f, opacity + System.Math.Min(intensity, lifeIntensity) * 0.5f);
            }

            color = Color.Lerp(color, Color.Black, 0.3f);

            return color;
        }
        public struct LightRay(Vector2 position, Vector2 velocity, float rotation, float rotationSpeed, int timeLeft)
        {
            public Vector2 Position = position;
            public Vector2 Velocity = velocity;
            public float Rotation = rotation;
            public float RotationSpeed = rotationSpeed;
            public int TimeLeft = timeLeft;
            public int MaxTimeLeft = timeLeft;
        }
        public List<LightRay> LightRays = [];
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth < 0 && minDepth >= 0)
                return;
            //if (maxDepth < float.MaxValue || minDepth >= float.MaxValue)
            //return;

            Vector2 screenCenter = Main.screenPosition + Vector2.UnitX * Main.screenWidth / 2 + Vector2.UnitY * Main.screenHeight / 2;

            float opacity = 0f;
            Color color = ColorToUse(ref opacity);

            spriteBatch.Draw(ModContent.Request<Texture2D>($"FargowiltasSouls/Content/Sky/MutantSky{FargoSoulsUtil.TryAprilFoolsTexture}", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
                new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), color * opacity * 1.2f);

            var blackTile = TextureAssets.MagicPixel;
            var noise = ModContent.Request<Texture2D>("FargowiltasSouls/Content/Sky/deepspace");
            var rayTexture = FargoAssets.LightRayTexture;
            if (!blackTile.IsLoaded)
                return;
            if (!noise.IsLoaded)
                return;
            if (!rayTexture.IsLoaded)
                return;

            /// TODO: make the star background and auroras appear fixed in place while scrolling between 100% and 200% zoom.
            /// i.e. make it so no matter how much you zoom in, the background always looks the same.
            /// rn it seems like the shader origin is glued to the pre-zoom screenPosition? idk
            float zoomValue = Math.Clamp(Main.GameZoomTarget, 1f, 2f);
            Vector2 zoomedScreenSize = Main.ScreenSize.ToVector2() / zoomValue;
            float zoomedScreenWidth = zoomedScreenSize.X;
            float zoomedScreenHeight = zoomedScreenSize.Y;
            Vector2 zoomedScreenPos = screenCenter - zoomedScreenSize / 2;

            //if (!Main.gamePaused) Main.NewText($"{Main.screenPosition} {Main.screenWidth} {Main.screenHeight} {Main.GameZoomTarget} {zoomedScreenSize} {zoomedScreenPos}");

            ManagedShader blackShader = ShaderManager.GetShader("FargowiltasSouls.MutantNewBackgroundShader");
            blackShader.TrySetParameter("radius", Main.screenHeight * 1.6f);
            blackShader.TrySetParameter("time", Main.GlobalTimeWrappedHourly);
            blackShader.TrySetParameter("anchorPoint", screenCenter - Vector2.UnitY * Main.screenHeight * 2);
            blackShader.TrySetParameter("screenPosition", Main.screenPosition);
            blackShader.TrySetParameter("screenSize", Main.ScreenSize.ToVector2());
            blackShader.TrySetParameter("maxOpacity", opacity * shaderIntensity);

            spriteBatch.GraphicsDevice.Textures[1] = noise.Value;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, blackShader.WrappedEffect, Main.GameViewMatrix.TransformationMatrix);
            Rectangle rekt = new(Main.screenWidth / 2, Main.screenHeight / 2, Main.screenWidth, Main.screenHeight);
            spriteBatch.Draw(blackTile.Value, rekt, null, default, 0f, blackTile.Value.Size() * 0.5f, 0, 0f);

            // aurora

            
            spriteBatch.UseBlendState(BlendState.Additive);

            int timer = (int)(Main.GlobalTimeWrappedHourly * 60f);
            if (timer % 4 == 0)
            {
                Vector2 rayPos = Vector2.UnitX * Main.rand.NextFloat(-Main.screenWidth * 1.3f, Main.screenWidth * 1.3f);
                Vector2 velocity = Vector2.UnitX * Main.rand.NextFloat(-16, 16);
                float maxRot = MathHelper.PiOver2 * 0.05f;
                float rayRot = Main.rand.NextFloat(-maxRot, maxRot);
                int rayTime = 600;
                float rayRotSpeed = Main.rand.NextFloat(0.25f * maxRot / rayTime, maxRot / rayTime);
                rayRotSpeed /= 14f;
                rayRotSpeed *= -rayRot.NonZeroSign();
                var ray = new LightRay(rayPos, velocity, rayRot, rayRotSpeed, rayTime);
                LightRays.Add(ray);
            }
            

            Vector2 lightRayOrigin = Vector2.UnitX * rayTexture.Width() / 2;
            List<LightRay> removeRays = [];
            for (int i = 0; i <  LightRays.Count; i++)
            {
                var ray = LightRays[i];
                ray.TimeLeft--;
                ray.Rotation += ray.RotationSpeed;
                LightRays[i] = ray; // because it's a struct, non-reference type
                if (ray.TimeLeft <= 0)
                {
                    removeRays.Add(ray);
                    continue;
                }
                float rayOpacity = opacity * shaderIntensity * extraElementsIntensity * 0.85f;
                float fadeTime = 120;
                if (ray.TimeLeft <= fadeTime)
                {
                    rayOpacity *= ray.TimeLeft / fadeTime;
                }
                float fadeThreshold = ray.MaxTimeLeft - fadeTime;
                if (ray.TimeLeft >= fadeThreshold)
                {
                    rayOpacity *= 1 - (ray.TimeLeft - fadeThreshold) / fadeTime;
                }
                Vector2 pos = new(screenCenter.X + ray.Position.X, screenCenter.Y + 1362);
                float sin = MathF.Sin(MathF.PI * ray.TimeLeft / (float)ray.MaxTimeLeft);
                int amp = 60;
                pos.Y += amp - sin * amp * 2;
                spriteBatch.Draw(rayTexture.Value, pos - Main.screenPosition, rayTexture.Value.Bounds, Color.White * rayOpacity * 0.4f, ray.Rotation + MathHelper.Pi, lightRayOrigin, 0.59f, SpriteEffects.None, 0);
            }

            foreach (var ray in removeRays)
                LightRays.Remove(ray);

            DoTvBands(spriteBatch, opacity);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);



            float[] scalers = [0.1f, 0.15f, 0.2f, 0.2f];
            float[] yOffset = [0f, 80f, 240f, 640f];
            float[] colorLerps = [0.2f, 0.5f, 0.9f, 1f];

            float yLerp = Utilities.InverseLerp(0, (float)Main.worldSurface * 16, screenCenter.Y);

            var bg = ModContent.Request<Texture2D>($"FargowiltasSouls/Assets/Textures/Misc/MutantBackground").Value;

            for (int i = 0; i < 4; i++)
            {
                Color bgColor = color;
                bgColor = Color.Lerp(bgColor, Color.Black, colorLerps[i]);

                Rectangle frame = new Rectangle(0, i * bg.Height / 3, bg.Width, bg.Height / 3);

                Vector2 pos = new(
                    -screenCenter.X * scalers[i] % bg.Width,
                    MathHelper.Lerp(Main.screenHeight / 2 + yOffset[i], Main.screenHeight * 0.85f + yOffset[i] / 2, 1 - yLerp)
                    );

                for (int j = 0; j < 5; j++)
                {
                    Vector2 drawPos = pos;
                    drawPos.X += bg.Width * j;
                    spriteBatch.Draw(bg, drawPos, frame, bgColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                }
            }

            spriteBatch.Draw(
                ModContent.Request<Texture2D>($"FargowiltasSouls/Content/Sky/MutantVignette", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
                new Rectangle((int)(zoomedScreenPos.X - Main.screenPosition.X), (int)(zoomedScreenPos.Y - Main.screenPosition.Y), (int)zoomedScreenWidth, (int)zoomedScreenHeight), 
                (FargoSoulsUtil.AprilFools ? Color.Red : Color.Blue) * shaderIntensity * 0.15f);
        }

        void DoTvBands(SpriteBatch spriteBatch, float opacity)
        {
            var blackTile = TextureAssets.MagicPixel;
            var risingFlame = ModContent.Request<Texture2D>("FargowiltasSouls/Content/Sky/MutantFlame");
            if (!blackTile.IsLoaded || !risingFlame.IsLoaded)
                return;

            ManagedShader wavyTvShader = ShaderManager.GetShader("FargowiltasSouls.MutantBackgroundShader");
            wavyTvShader.TrySetParameter("globalTime", Main.GlobalTimeWrappedHourly);
            wavyTvShader.TrySetParameter("screenPosition", Main.screenPosition);
            wavyTvShader.TrySetParameter("screenSize", Main.ScreenSize.ToVector2());
            wavyTvShader.TrySetParameter("scrollSpeed", opacity);
            wavyTvShader.TrySetParameter("opacity", shaderIntensity * lifeIntensity * extraElementsIntensity);

            spriteBatch.GraphicsDevice.Textures[1] = risingFlame.Value;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, wavyTvShader.WrappedEffect, Main.GameViewMatrix.TransformationMatrix);

            Rectangle rekt = new(Main.screenWidth / 2, Main.screenHeight / 2, Main.screenWidth, Main.screenHeight);
            spriteBatch.Draw(blackTile.Value, rekt, null, default, 0f, blackTile.Value.Size() * 0.5f, 0, 0f);
        }

        public override float GetCloudAlpha()
        {
            return 1f - intensity;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            isActive = true;
        }

        public override void Deactivate(params object[] args)
        {
            isActive = false;
        }

        public override void Reset()
        {
            isActive = false;
        }

        public override bool IsActive()
        {
            return isActive;
        }

        public override Color OnTileColor(Color inColor)
        {
            float dummy = 0f;
            Color skyColor = Color.Lerp(Color.White, ColorToUse(ref dummy), 0.5f);
            return Color.Lerp(skyColor, inColor, 1f - intensity);
        }
    }
}