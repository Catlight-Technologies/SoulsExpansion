using FargowiltasSouls;
using FargowiltasSouls.Content.Buffs.Boss;
using FargowiltasSouls.Content.Buffs.Eternity;
using FargowiltasSouls.Core.Systems;
using Luminance.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSE.Content.Common.Projectiles.MutantEX
{
    public class MutantSansHead : ModProjectile
    {
        public override string Texture => "FargowiltasSouls/Assets/Textures/EModeResprites/NPC_246B";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 70;
            Projectile.height = 70;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            CooldownSlot = ImmunityCooldownID.Bosses;

            Projectile.timeLeft = 420;
            Projectile.hide = true;

            Projectile.FargoSouls().DeletionImmuneRank = 1;
        }

        public override bool CanHitPlayer(Player target)
        {
            return target.hurtCooldowns[1] == 0;
        }

        public override bool? CanDamage() => Projectile.ai[0] < 0;

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }
        Vector2 startPos = Vector2.Zero;
        Vector2 endPos = Vector2.Zero;
        public float travelTime;
        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1;
                Projectile.rotation = Projectile.ai[2] == 1 ? 0 : MathHelper.Pi;
            }
            if (Projectile.ai[0] > 0)
            {
                if (travelTime == 0)
                {
                    travelTime = Projectile.ai[0];
                    startPos = Projectile.Center;
                    endPos = startPos + Projectile.velocity * travelTime;
                }
                float progress = 1 - (Projectile.ai[0] / travelTime);
                Vector2 nextStep = Vector2.SmoothStep(startPos, endPos, progress);
                Projectile.velocity = nextStep - Projectile.Center;
            }

            if (--Projectile.ai[0] == 0)
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.netUpdate = true;

                if (FargoSoulsUtil.HostCheck)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center,
                        Vector2.UnitY * Projectile.ai[2],
                        ModContent.ProjectileType<MutantSansBeam>(),
                        Projectile.damage, Projectile.knockBack, Projectile.owner,
                        0f, Projectile.identity);
                }
            }
            else if (Projectile.ai[0] < -50 - 120)
            {
                Projectile.velocity.X *= 1.025f;
            }
            else if (Projectile.ai[0] < -50)
            {
                Projectile.velocity.X = Projectile.ai[1];
                Projectile.velocity.Y = 0;
            }

            Projectile.frame = 1;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<CurseoftheMoonBuff>(), 240);
            if (WorldSavingSystem.EternityMode)
                target.AddBuff(ModContent.BuffType<MutantFangBuff>(), 180);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = TextureAssets.Projectile[Projectile.type].Value;
            int num156 = TextureAssets.Projectile[Projectile.type].Value.Height / 6; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = lightColor;
            color26 = Projectile.GetAlpha(color26);

            SpriteEffects effects = SpriteEffects.None;


            Main.spriteBatch.UseBlendState(BlendState.Additive);
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Color trailColor = Projectile.GetAlpha(Color.LightSkyBlue) * 1f;
                trailColor *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                Vector2 oldPos = Projectile.oldPos[i];
                float oldRot = Projectile.oldRot[i];
                Main.EntitySpriteDraw(texture2D13, oldPos + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), rectangle, trailColor, oldRot, origin2, Projectile.scale, effects, 0);
            }
            Main.spriteBatch.ResetToDefault();

            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), rectangle, color26, Projectile.rotation, origin2, Projectile.scale, effects, 0);

            //idk why these arent working boohoo
            /*
            Texture2D eyes = TextureAssets.Golem[1].Value;
            Rectangle eyeRectangle = new(0, eyes.Height / 2, eyes.Width, eyes.Height / 2);
            Vector2 eyeOrigin = eyeRectangle.Size() / 2f;
            eyeOrigin.Y -= 4;
            Main.EntitySpriteDraw(eyes, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(eyeRectangle), Color.White * Projectile.Opacity, Projectile.rotation, eyeOrigin, Projectile.scale, effects, 0);
            */
            return false;
        }
    }
}

