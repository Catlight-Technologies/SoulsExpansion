using CSE.Content.Common.Bosses.MutantEX;
using FargowiltasSouls;
using FargowiltasSouls.Core.Globals;
using Luminance.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSE.Content.Common.Projectiles.MutantEX
{
    public class MutantSlimeSpike : MutantSlimeBall
    {
        public override string Texture => FargoSoulsUtil.AprilFools ?
            "FargowiltasSouls/Content/Bosses/MutantBoss/MutantSlimeSpike_April" :
            "Terraria/Images/Projectile_920";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.timeLeft *= 2;
        }

        public override void AI()
        {
            if (++Projectile.localAI[1] > 10 && FargoSoulsUtil.BossIsAlive(ref EModeGlobalNPC.mutantBoss, ModContent.NPCType<MutantEXBoss>()))
            {
                float yOffset = Projectile.Center.Y - Main.npc[EModeGlobalNPC.mutantBoss].Center.Y;
                if (Math.Sign(yOffset) == Math.Sign(Projectile.velocity.Y) && Projectile.Distance(Main.npc[EModeGlobalNPC.mutantBoss].Center) > 1200 + Projectile.ai[0])
                    Projectile.timeLeft = 0;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.localAI[2] == 0)
            {
                Projectile.localAI[2] = 1;
                Projectile.frame = Main.rand.Next(3);
            }
            Projectile.frame = (int)MathHelper.Clamp(Projectile.frame, 0, 2);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Projectile.GetTexture();
            Vector2 drawPos = Projectile.GetDrawPosition();
            Rectangle frame = Projectile.GetDefaultFrame();
            SpriteEffects spriteEffects = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Color color26 = lightColor;
            color26 = Projectile.GetAlpha(color26);

            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Color color27 = color26 * 0.5f;
                color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                Vector2 value4 = Projectile.oldPos[i];
                float num165 = Projectile.oldRot[i];
                Main.EntitySpriteDraw(texture, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), frame, color27, num165, frame.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            }

            Main.spriteBatch.UseBlendState(BlendState.Additive);
            for (int j = 0; j < 12; j++)
            {
                Vector2 afterimageOffset = (MathHelper.TwoPi * j / 12).ToRotationVector2() * 2f * Projectile.scale;
                Color glowColor = Color.DeepPink;

                Main.EntitySpriteDraw(texture, drawPos + afterimageOffset, frame, Projectile.GetAlpha(glowColor), Projectile.rotation, frame.Size() / 2, Projectile.scale, spriteEffects);
            }
            Main.spriteBatch.ResetToDefault();

            Main.EntitySpriteDraw(texture, drawPos, frame, Projectile.GetAlpha(lightColor), Projectile.rotation, frame.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}