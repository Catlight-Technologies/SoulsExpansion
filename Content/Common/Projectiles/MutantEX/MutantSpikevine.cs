using CSE.Content.Common.Bosses.MutantEX;
using FargowiltasSouls;
using FargowiltasSouls.Assets.Textures;
using FargowiltasSouls.Content.Bosses.MutantBoss;
using FargowiltasSouls.Content.Buffs.Boss;
using FargowiltasSouls.Content.Buffs.Eternity;
using FargowiltasSouls.Core;
using FargowiltasSouls.Core.Systems;
using Luminance.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSE.Content.Common.Projectiles.MutantEX
{
    public class MutantSpikevine : ModProjectile
    {
        public override string Texture => FargoAssets.GetAssetString("Content/Projectiles/Eternity/Bosses/Plantera", "PlanteraSpikevine");
        const int TrailSaveLength = 1000;
        Vector2[] Trail = new Vector2[TrailSaveLength];
        public int TrailLength = 800;
        public override void SetStaticDefaults()
        {
            //ProjectileID.Sets.TrailCacheLength[Type] = 240;
            //ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;
        }
        const int SpriteLength = 20;
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60 * 30;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;

            Projectile.light = 1;
            Projectile.scale = 1;
            Projectile.Opacity = 0;
            Projectile.hide = true;
            CooldownSlot = ImmunityCooldownID.Bosses;
        }
        ref float Timer => ref Projectile.ai[0];
        ref float MutantID => ref Projectile.ai[1];
        ref float Flip => ref Projectile.ai[2];
        ref float Distance => ref Projectile.localAI[0];
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 position = Projectile.position;
            int index = 0;
            Vector2 difference = Trail[index + 1] - Trail[index];
            float lengthLeft = difference.Length();

            Texture2D spikeTexture = ModContent.Request<Texture2D>(Texture + "End").Value;
            if (targetHitbox.Intersects(projHitbox))
                return true;

            for (int i = 0; i < TrailLength; i++)
            {
                position += difference.SafeNormalize(Vector2.Zero) * SpriteLength * Projectile.scale;
                for (int j = 0; j < 1000; i++)
                {
                    lengthLeft -= SpriteLength * Projectile.scale;
                    if (lengthLeft < 0)
                    {
                        index++;
                        if (Trail[index + 1] != Vector2.Zero && Timer > index)
                        {
                            Vector2 dif = Trail[index + 1] - position;
                            if (dif != Vector2.Zero)
                            {
                                difference = dif;
                                lengthLeft = difference.Length();
                            }
                        }
                        else
                            return false;
                    }
                    else
                        break;
                }
                Rectangle hitbox = new((int)(position.X - projHitbox.Width / 2), (int)(position.Y - projHitbox.Height / 2), projHitbox.Width, projHitbox.Height);
                if (targetHitbox.Intersects(hitbox))
                    return true;
            }
            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Trail[0] = Projectile.Center;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<CurseoftheMoonBuff>(), 240);
            if (WorldSavingSystem.EternityMode)
                target.AddBuff(ModContent.BuffType<MutantFangBuff>(), 180);
        }
        public override void AI()
        {
            if (Timer == 0)
            {
                //Target = Player.FindClosest(Projectile.Center, 0, 0);
                Projectile.Opacity = 1;
            }
            if (Timer == 60)
                SoundEngine.PlaySound(SoundID.Item63 with { Pitch = -1f, Volume = 10 }, Projectile.Center);
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
            int mutantID = (int)MutantID;
            if (Timer >= 60 && mutantID.IsWithinBounds(Main.maxNPCs) && Projectile.velocity != Vector2.Zero)
            {
                NPC mutant = Main.npc[(int)mutantID];
                var p = FargoSoulsUtil.ProjectileExists(mutant.As<MutantEXBoss>().ritualProj, ModContent.ProjectileType<MutantRitual>());
                if (p != null)
                {
                    if (Projectile.Distance(p.Center) >= p.As<MutantRitual>().threshold - 60)
                    {
                        //Projectile.position = Projectile.velocity;
                        Projectile.velocity *= 0;
                        SoundEngine.PlaySound(SoundID.Item49, Projectile.Center);
                    }
                }
            }

            if (Projectile.velocity != Vector2.Zero)
            {
                const float maxExpectedDistance = 1140;
                const float numberOfUndulations = 5;

                Vector2 waveAmplitude = 32f * Flip * Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2);

                // need to undo the previous tick's undulation so that we're back along the original "line" of the real velocity
                float oldUndulation = (float)System.Math.Sin(MathHelper.TwoPi * Distance / maxExpectedDistance * numberOfUndulations);
                Projectile.position -= waveAmplitude * oldUndulation;

                Distance += Projectile.velocity.Length();

                float undulation = (float)System.Math.Sin(MathHelper.TwoPi * Distance / maxExpectedDistance * numberOfUndulations);
                Projectile.position += waveAmplitude * undulation;
            }

            if (Timer > 300)
            {
                TrailLength -= 4;
                if (TrailLength <= 0)
                {
                    Projectile.Kill();
                }
            }
            if (Projectile.velocity != Vector2.Zero)
            {
                for (int i = TrailSaveLength - 1; i >= 0; i--)
                {
                    if (i == 0)
                        Trail[i] = Projectile.Center;
                    else
                        Trail[i] = Trail[i - 1];
                }
            }
            Timer++;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (Projectile.hide)
                behindNPCs.Add(index);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = true;
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Center += Projectile.velocity;
            Projectile.velocity *= 0;
            SoundEngine.PlaySound(SoundID.Item49, Projectile.Center);
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            bool recolor = true; // SoulConfig.Instance.BossRecolors && WorldSavingSystem.EternityMode;

            Vector2 position = Projectile.Center;
            int index = 0;
            Vector2 difference = Trail[index + 1] - Trail[index];
            float lengthLeft = difference.Length();
            string variant = recolor ? "" : "Vanilla";
            Texture2D spikeTexture = ModContent.Request<Texture2D>(Texture + "End" + variant).Value;
            Texture2D texture = ModContent.Request<Texture2D>(Texture + variant).Value;

            int height = spikeTexture.Height;
            Rectangle rectangle = new(0, 0, spikeTexture.Width, height);
            Vector2 origin = rectangle.Size() / 2f;
            SpriteEffects spriteEffects = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Color color = Lighting.GetColor(position.ToTileCoordinates());
            Main.EntitySpriteDraw(spikeTexture, position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), rectangle, Projectile.GetAlpha(color),
                (-difference).ToRotation(), origin, Projectile.scale, spriteEffects, 0);

            for (int i = 0; i < TrailLength; i++)
            {
                float opacity = 1f;
                int bound = 25;
                if (Timer > 200 && i > TrailLength - bound)
                    opacity *= 1 - ((i - (TrailLength - bound)) / bound);
                position += difference.SafeNormalize(Vector2.Zero) * SpriteLength * Projectile.scale;
                for (int j = 0; j < 1000; i++)
                {
                    lengthLeft -= SpriteLength * Projectile.scale;
                    if (lengthLeft < 0)
                    {
                        index++;
                        if (Trail[index + 1] != Vector2.Zero && Timer > index)
                        {
                            Vector2 dif = Trail[index + 1] - position;
                            if (dif != Vector2.Zero)
                            {
                                difference = dif;
                                lengthLeft = difference.Length();
                            }
                        }
                        else
                            return false;
                    }
                    else
                        break;
                }
               
                int frame = i % 4;
                height = texture.Height / 4;
                rectangle = new(0, height * frame, texture.Width, height);
                origin = rectangle.Size() / 2f;
                color = Lighting.GetColor(position.ToTileCoordinates());
                Main.EntitySpriteDraw(texture, position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), rectangle, Projectile.GetAlpha(color) * opacity,
                    (-difference).ToRotation(), origin, Projectile.scale, spriteEffects, 0);
            }
            return false;
        }
    }
}
