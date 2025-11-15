using CSE.Content.Common.Bosses.MutantEX;
using FargowiltasSouls;
using FargowiltasSouls.Common.Graphics.Particles;
using FargowiltasSouls.Content.Buffs.Boss;
using FargowiltasSouls.Content.Buffs.Eternity;
using FargowiltasSouls.Core.Globals;
using FargowiltasSouls.Core.Systems;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSE.Content.Common.Projectiles.MutantEX
{
    public class MutantWofPart : ModProjectile
    {
        public override string Texture => "FargowiltasSouls/Assets/Textures/EModeResprites/NPC_113";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 2;
            CooldownSlot = ImmunityCooldownID.Bosses;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Projectile.Distance(FargoSoulsUtil.ClosestPointInHitbox(targetHitbox, Projectile.Center)) < Projectile.width / 2;
        }

        bool spawn;

        ref float MyType => ref Projectile.ai[0];
        ref float TimeToTravel => ref Projectile.ai[1];
        ref float TimeToLive => ref Projectile.ai[2];

        ref float Timer => ref Projectile.localAI[0];
        ref float AboveOrBelowEye => ref Projectile.localAI[1];

        void MouthDust()
        {
            bool UseCorruptAttack = MyType == 0;

            int type = UseCorruptAttack ? 75 : 170; //corruption dust, then crimson dust
            Color color = UseCorruptAttack ? new(96, 248, 2) : Color.Gold;
            int speed = UseCorruptAttack ? 10 : 8;
            float scale = UseCorruptAttack ? 6f : 4f;
            float speedModifier = UseCorruptAttack ? 12f : 5f;

            Vector2 direction = Projectile.rotation.ToRotationVector2().RotatedByRandom(MathHelper.Pi / 10);
            Vector2 vel = speed * direction * Main.rand.NextFloat(0.4f, 0.8f);

            Particle p = new ExpandingBloomParticle(Projectile.Center + 32f * direction + vel * 50, -vel / 2, color, startScale: Vector2.Zero, endScale: Vector2.One * scale, lifetime: 25);
            p.Velocity *= 2f;
            p.Spawn();
        }

        public override void AI()
        {
            NPC mutant = FargoSoulsUtil.NPCExists(EModeGlobalNPC.mutantBoss, ModContent.NPCType<MutantEXBoss>());
            if (mutant == null)
            {
                Projectile.Kill();
                return;
            }

            if (!spawn)
            {
                spawn = true;
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.localAI[2] = Projectile.direction = Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
                Projectile.timeLeft = (int)TimeToLive;
                AboveOrBelowEye = Math.Sign(Projectile.Center.Y - mutant.Center.Y);
            }

            Projectile.direction = Projectile.spriteDirection = (int)Projectile.localAI[2];

            if (--TimeToTravel < 0)
            {
                Projectile.velocity = Vector2.Zero;

                if (mutant.HasValidTarget)
                {
                    Player player = Main.player[mutant.target];

                    switch (MyType)
                    {
                        case 0: //mouth 1, cursed flames
                            {
                                Projectile.frameCounter++;
                                Projectile.rotation = Projectile.DirectionTo(Main.player[mutant.target].Center).ToRotation();
                                MouthDust();

                                int timeInterval = WorldSavingSystem.MasochistModeReal && Main.getGoodWorld ? 24 : 30;
                                int attacksToDo = 4;
                                if (Timer % timeInterval == 1 && FargoSoulsUtil.HostCheck)
                                {
                                    float spacing = WorldSavingSystem.MasochistModeReal ? 250 : 300;
                                    int step = (int)Timer / timeInterval % attacksToDo;
                                    float halfOffset = (int)Timer / timeInterval % (attacksToDo * 2) >= attacksToDo ? spacing / 2 : 0;
                                    for (int j = -1; j <= 1; j += 2)
                                    {
                                        if (j == -1 && !WorldSavingSystem.MasochistModeReal) 
                                            continue;
                                        Vector2 spawnPos = Projectile.Center;
                                        spawnPos.X += Projectile.direction * (spacing * step - halfOffset);
                                        spawnPos.Y += j * 1600;
                                        Vector2 vel = 64f * -Vector2.UnitY.RotatedBy(MathHelper.ToRadians(10) * Projectile.direction);
                                        vel.Y *= j;
                                        int tellTime = 50;
                                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), spawnPos, vel, ModContent.ProjectileType<MutantCursedFlame>(), Projectile.damage, Projectile.knockBack, Projectile.owner, tellTime);
                                    }
                                }
                            }
                            break;

                        case 1: //eyes
                            {
                                float maxRotationToDo = MathHelper.ToRadians(30) * -AboveOrBelowEye;
                                bool useAltBehavior = Projectile.direction < 0;

                                Projectile.rotation = Projectile.direction < 0 ? MathHelper.Pi : 0;
                                //if (useAltBehavior) Projectile.rotation += maxRotationToDo;

                                if (Timer == 0 && FargoSoulsUtil.HostCheck)
                                {
                                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Projectile.rotation.ToRotationVector2(), ModContent.ProjectileType<MutantWofDeathray>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, Projectile.identity, Projectile.timeLeft);
                                }

                                const int waveTime = 120;
                                float sineTimer = Timer;
                                if (useAltBehavior) sineTimer += waveTime / 2;
                                float offset = WorldSavingSystem.MasochistModeReal ? 0.2f : 1.5f;
                                float oscillation = -maxRotationToDo * Projectile.direction * (offset + (float)Math.Sin(MathHelper.TwoPi / waveTime * sineTimer));
                                Projectile.rotation += oscillation;
                            }
                            break;

                        case 2: //mouth 2, ichor
                            {
                                Projectile.rotation = Projectile.direction < 0 ? MathHelper.Pi : 0;
                                Projectile.rotation += MathHelper.Pi * 0.35f * -Projectile.direction;

                                float maxRotationToDo = MathHelper.ToRadians(20);
                                const float waveTime = 60;
                                float oscillation = -maxRotationToDo * Projectile.direction * (float)Math.Sin(MathHelper.TwoPi / waveTime * Timer);
                                Projectile.rotation += oscillation;

                                MouthDust();

                                int attackThreshold = WorldSavingSystem.MasochistModeReal && Main.getGoodWorld ? 8 : 10;
                                if (Timer % attackThreshold == 0 && FargoSoulsUtil.HostCheck)
                                {
                                    const int max = 1;
                                    Vector2 baseTarget = mutant.Center;
                                    baseTarget.Y -= 16 * (WorldSavingSystem.MasochistModeReal ? 30 : 40);
                                    float xVariance = mutant.ai[3];
                                    if (Main.rand.NextBool(6))
                                    {
                                        baseTarget.X = Main.player[mutant.target].Center.X;
                                        xVariance = 16 * 3;
                                    }
                                    for (int i = 0; i < max; i++)
                                    {
                                        const int timeToFinishArc = 60;
                                        const float gravity = 0.4f;
                                        float velX = (baseTarget.X + Main.rand.NextFloat(-xVariance, xVariance) - Projectile.Center.X) / timeToFinishArc;
                                        float velY = (baseTarget.Y /*+ Main.rand.NextFloat(-32, 32)*/ - Projectile.Center.Y) / timeToFinishArc - gravity * timeToFinishArc / 2;
                                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, new Vector2(velX, velY), ModContent.ProjectileType<MutantIchor>(), Projectile.damage, Projectile.knockBack, Projectile.owner, gravity, timeToFinishArc);
                                    }
                                }
                            }
                            break;
                    }

                    Timer++;
                }
            }

            if (++Projectile.frameCounter > 8)
            {
                Projectile.frameCounter = 0;
                if (Projectile.frame >= Main.projFrames[Type])
                    Projectile.frame = 0;
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (!Main.dedServ)
            {
                //ripped vanilla wof gores idk
                int myGoreType = MyType == 1 ? 138 : 139;
                Vector2 position = Projectile.position;
                const float spd = 8f;
                float scale = Projectile.scale;
                int width = Projectile.width;
                int height = Projectile.height;

                void OhThatsGoreOfMyComfortCharacter(Vector2 pos, int goreType)
                {
                    Vector2 vel = Main.rand.NextVector2Circular(spd, spd);
                    vel.X -= spd * Projectile.localAI[2] * 0.8f; //so they dont fall in your way
                    Gore.NewGore(Projectile.InheritSource(Projectile), pos, vel, goreType, scale);
                }

                OhThatsGoreOfMyComfortCharacter(new Vector2(position.X, position.Y), 137);
                OhThatsGoreOfMyComfortCharacter(new Vector2(position.X, position.Y + (float)(height / 2)), myGoreType);
                OhThatsGoreOfMyComfortCharacter(new Vector2(position.X + (float)(width / 2), position.Y), myGoreType);
                OhThatsGoreOfMyComfortCharacter(new Vector2(position.X + (float)(width / 2), position.Y + (float)(height / 2)), 137);
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
            Texture2D texture2D13 = TextureAssets.Projectile[Projectile.type].Value;
            if (MyType == 1)
                texture2D13 = ModContent.Request<Texture2D>("FargowiltasSouls/Assets/Textures/EModeResprites/NPC_" + NPCID.WallofFleshEye).Value;
            int num156 = TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            SpriteEffects effects = SpriteEffects.None;
            Color color = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, Projectile.rotation + MathHelper.Pi, origin2, Projectile.scale, effects, 0);
            return false;
        }
    }
}