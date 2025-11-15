using CSE.Content.Common.Bosses.MutantEX;
using FargowiltasSouls;
using FargowiltasSouls.Content.Buffs.Boss;
using FargowiltasSouls.Content.Buffs.Eternity;
using FargowiltasSouls.Content.Buffs.Souls;
using FargowiltasSouls.Core.Globals;
using FargowiltasSouls.Core.Systems;
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
    public class MutantSphereHelix : MutantSphereRing
    {
        public override string Texture => FargoSoulsUtil.AprilFools ?
            "FargowiltasSouls/Content/Bosses/MutantBoss/MutantSphere_April" :
            "FargowiltasSouls/Content/Bosses/MutantBoss/MutantSphere";

        protected override float GlowLerpToClear => 0.9f;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            DieOutsideArena = true;
            Projectile.width = Projectile.height = 30;
        }

        ref float MutantID => ref Projectile.ai[0];
        ref float Flip => ref Projectile.ai[1];
        ref float FullRotationIncrement => ref Projectile.ai[2];
        ref float Distance => ref Projectile.localAI[0];
        ref float OriginX => ref Projectile.localAI[1];
        ref float OriginY => ref Projectile.localAI[2];

        public override void AI()
        {
            NPC mutant = FargoSoulsUtil.NPCExists(MutantID, ModContent.NPCType<MutantEXBoss>());
            if (mutant == null)
            {
                Projectile.Kill();
                return;
            }

            //float rotationModifier = (WorldSavingSystem.MasochistModeReal ? 0.026f : 0.0005f) * Flip;
            //Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver2 * rotationModifier);

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
                    //debug dust to track the true position
                    /*for (int i = 0; i < 50; i++)
                    {
                        int d = Dust.NewDust(Projectile.Center, 2, 2, DustID.Vortex);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].velocity = Vector2.Zero;
                        Main.dust[d].scale = 3f;
                    }*/

                    //make the entire pattern rotate around mutant
                    Vector2 mutantOriginalPos = new Vector2(OriginX, OriginY);
                    Vector2 mutantToMe = Projectile.Center - mutantOriginalPos;
                    Projectile.Center = mutantOriginalPos + mutantToMe.RotatedBy(FullRotationIncrement);
                    Projectile.velocity = mutantToMe.SafeNormalize(Vector2.Zero) * Projectile.velocity.Length();
                }

                Distance += Projectile.velocity.Length();

                // yes we need to recalculate these entirely
                float waveAmp = baseAmp * (1f + ampBonus * Distance / maxExpectedDistance);
                Vector2 waveAmplitude = waveAmp * Flip * Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2);
                float undulation = (float)Math.Sin(MathHelper.TwoPi * Distance / maxExpectedDistance * numberOfUndulations);
                Projectile.position += waveAmplitude * undulation;
            }

            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 20;
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;
            }
            Projectile.scale = 1f - Projectile.alpha / 255f;

            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame > 1)
                    Projectile.frame = 0;
            }

            if (DieOutsideArena)
            {
                if (ritualID == -1) //identify the ritual CLIENT SIDE
                {
                    ritualID = -2; //if cant find it, give up and dont try every tick

                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<MutantRitual>())
                        {
                            ritualID = i;
                            break;
                        }
                    }
                }

                Projectile ritual = FargoSoulsUtil.ProjectileExists(ritualID, ModContent.ProjectileType<MutantRitual>());
                if (ritual != null && Projectile.Distance(ritual.Center) > 1200f) //despawn faster
                    Projectile.timeLeft = 0;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (FargoSoulsUtil.BossIsAlive(ref EModeGlobalNPC.mutantBoss, ModContent.NPCType<MutantEXBoss>()))
            {
                    target.AddBuff(ModContent.BuffType<MutantFangBuff>(), 180);
            }
            target.AddBuff(ModContent.BuffType<CurseoftheMoonBuff>(), 240);
        }
    }
}