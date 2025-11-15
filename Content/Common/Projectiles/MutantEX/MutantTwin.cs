using CSE.Core;
using FargowiltasSouls;
using FargowiltasSouls.Common.Graphics.Particles;
using FargowiltasSouls.Content.Buffs.Boss;
using FargowiltasSouls.Content.Buffs.Eternity;
using FargowiltasSouls.Content.Projectiles.Eternity.Bosses.MechanicalBosses;
using FargowiltasSouls.Core.Systems;
using Luminance.Common.Utilities;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSE.Content.Common.Projectiles.MutantEX
{
    public class MutantTwin : ModProjectile
    {
        public bool Spazmatism => Projectile.ai[2] == 0 ? false : true;
        public override string Texture => "FargowiltasSouls/Assets/Textures/EModeResprites/NPC_" + NPCID.Retinazer;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = Main.npcFrameCount[NPCID.Retinazer];
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            CooldownSlot = ImmunityCooldownID.Bosses;

            Projectile.timeLeft = 60 * 10;

            Projectile.FargoSouls().DeletionImmuneRank = 2;
        }

        bool spawned;
        public ref float Angle => ref Projectile.ai[1];
        public ref float Timer => ref Projectile.localAI[0];
        Vector2 targetPos = Vector2.Zero;
        public override void AI()
        {
            Player player = FargoSoulsUtil.PlayerExists(Projectile.ai[0]);
            if (player == null)
            {
                Projectile.Kill();
                return;
            }

            if (!spawned)
            {
                spawned = true;

                //if (FargoSoulsUtil.HostCheck)
                //    Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<GlowRing>(), 0, 0f, Main.myPlayer, -1, NPCID.Retinazer);
            }

            float prepTime = 40;
            float reelback = 15;
            float angle = Angle;
            if (Timer >= prepTime)
                angle = Projectile.DirectionFrom(targetPos).ToRotation();
            Vector2 dir = angle.ToRotationVector2();
            if (Timer < prepTime)
            {
                Vector2 desiredPos = player.Center + dir * 400;
                Projectile.velocity = FargoSoulsUtil.SmartAccel(Projectile.Center, desiredPos, Projectile.velocity, 2f, 2f);
                Projectile.rotation = Projectile.DirectionTo(player.Center).ToRotation() - MathHelper.PiOver2;
                targetPos = player.Center;
            }
            else if (Timer < prepTime + reelback)
            {
                Projectile.velocity *= 0.7f;
                Projectile.velocity += dir * 3f;
                Projectile.rotation = Projectile.rotation.ToRotationVector2().RotateTowards(angle + MathHelper.PiOver2, 0.1f).ToRotation();
            }
            else
            {
                Projectile.rotation = Projectile.rotation.ToRotationVector2().RotateTowards(angle + MathHelper.PiOver2, 0.1f).ToRotation();
                if (Timer == prepTime + reelback)
                    SoundEngine.PlaySound(SoundID.ForceRoarPitched with { Volume = 0.3f }, Projectile.Center);
                Projectile.velocity -= dir * 2f;
                foreach (Projectile p in Main.ActiveProjectiles)
                {
                    if (p.whoAmI != Projectile.whoAmI && p.type == Projectile.type && Projectile.Colliding(Projectile.Hitbox, p.Hitbox))
                    {
                        Vector2 center = (p.Center + Projectile.Center) / 2;
                        if (FargoSoulsUtil.HostCheck)
                        {
                            float wholeAttackOffset = Main.rand.NextBool() ? 0 : MathHelper.PiOver2;

                            for (int i = -2; i <= 2; i++)
                            {
                                for (int j = -1; j <= 1; j += 2)
                                {
                                    float offset = wholeAttackOffset + Main.rand.NextFloat(0.15f);

                                    float speedModifier = j * (WorldSavingSystem.MasochistModeReal ? 1.1f : 1f);

                                    Vector2 projDir = (MathHelper.PiOver4 / 2 * i + Projectile.rotation + offset).ToRotationVector2();
                                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), center, projDir * 1f * speedModifier, ModContent.ProjectileType<MechElectricOrbSpaz>(),
                                        Projectile.damage, 0f, Main.myPlayer, ai0: player.whoAmI, ai2: Main.rand.Next(4));

                                    projDir = projDir.RotatedBy(MathHelper.PiOver2);
                                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), center, projDir * 0.8f * speedModifier, ModContent.ProjectileType<MechElectricOrbSpaz>(),
                                        Projectile.damage, 0f, Main.myPlayer, ai0: player.whoAmI, ai2: Main.rand.Next(4));

                                    /*if (WorldSavingSystem.MasochistModeReal)
                                    {
                                        speedModifier *= 0.4f; // WorldSavingSystem.MasochistModeReal ? 0.65f : 0.4f;

                                        projDir = projDir.RotatedBy(MathHelper.PiOver2);
                                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), center, projDir * 0.8f * speedModifier, ModContent.ProjectileType<MechElectricOrbSpaz>(),
                                            Projectile.damage, 0f, Main.myPlayer, ai0: player.whoAmI, ai2: MechElectricOrb.Green);

                                        projDir = projDir.RotatedBy(MathHelper.PiOver2);
                                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), center, projDir * 1f * speedModifier, ModContent.ProjectileType<MechElectricOrbSpaz>(),
                                            Projectile.damage, 0f, Main.myPlayer, ai0: player.whoAmI, ai2: MechElectricOrb.Yellow);
                                    }*/
                                }
                            }

                            Projectile.NewProjectile(Projectile.InheritSource(Projectile), center, Vector2.Zero, ModContent.ProjectileType<MutantNukeBomb>(), 0, 0f, Projectile.owner);
                        }
                        for (int i = 0; i < 30; i++)
                        {
                            float j = Main.rand.NextBool() ? 1 : -1;
                            Color color = j switch
                            {
                                1 => Color.Green,
                                _ => Color.Yellow,
                            };
                            Vector2 sparkDir = (Projectile.rotation - MathHelper.PiOver2 + j * MathHelper.PiOver2).ToRotationVector2().RotatedByRandom(MathHelper.PiOver2 * 0.28f);
                            float spd = Main.rand.NextFloat(14f, 20f);
                            Particle spark = new ElectricSpark(center, sparkDir * spd, color, Main.rand.NextFloat(0.7f, 1f), 40);
                            spark.Spawn();
                        }
                        Projectile.Kill();
                        p.Kill();
                    }
                }

                if (Timer > prepTime + reelback + 60)
                {
                    Projectile.active = false;
                    return;
                }
            }
            Timer++;

            if (++Projectile.frameCounter > 6)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            

            if (Projectile.frame >= 3)
                Projectile.frame = 0;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<CurseoftheMoonBuff>(), 240);
                target.AddBuff(ModContent.BuffType<MutantFangBuff>(), 180);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 50; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Spazmatism ? DustID.CursedTorch : DustID.IchorTorch, 0, 0, 0, default, 2f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 5f;
            }

            if (!Main.dedServ)
            {
                const float spd = 8f;
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position + new Vector2(Main.rand.NextFloat(Projectile.width), Main.rand.NextFloat(Projectile.height)), Main.rand.NextVector2Circular(-spd, spd), ModContent.Find<ModGore>(ModCompatibility.Souls.Name, $"Gore_{(Spazmatism ? 144 : 143)}").Type);
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position + new Vector2(Main.rand.NextFloat(Projectile.width), Main.rand.NextFloat(Projectile.height)), Main.rand.NextVector2Circular(-spd, spd), ModContent.Find<ModGore>(ModCompatibility.Souls.Name, $"Gore_{(Spazmatism ? 145 : 146)}").Type);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Spazmatism ? ModContent.Request<Texture2D>("FargowiltasSouls/Assets/Textures/EModeResprites/NPC_" + NPCID.Spazmatism).Value : Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = Projectile.GetAlpha(lightColor);

            Main.spriteBatch.UseBlendState(BlendState.Additive);

            float scale = (Main.mouseTextColor / 200f - 0.35f) * 0.3f + 0.9f;
            scale *= Projectile.scale;

            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Color color27 = color26 * 0.75f;
                color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                Vector2 value4 = Projectile.oldPos[i];
                float num165 = Projectile.oldRot[i];
                Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, scale, SpriteEffects.None, 0);
            }

            Main.spriteBatch.ResetToDefault();

            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color26, Projectile.rotation, origin2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}

