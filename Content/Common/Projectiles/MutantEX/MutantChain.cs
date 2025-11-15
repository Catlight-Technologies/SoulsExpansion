using FargowiltasSouls;
using FargowiltasSouls.Content.Buffs.Boss;
using FargowiltasSouls.Content.Buffs.Eternity;
using FargowiltasSouls.Core;
using FargowiltasSouls.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSE.Content.Common.Projectiles.MutantEX
{
    public class MutantChain : ModProjectile
    {
        public override string Texture => "FargowiltasSouls/Assets/Textures/EModeResprites/NPC_" + NPCID.TheHungry;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = Main.npcFrameCount[NPCID.TheHungry];
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 8000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            CooldownSlot = ImmunityCooldownID.Bosses;

            Projectile.hide = true;
        }

        public override bool? CanDamage() => false;

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCs.Add(index);
        }

        bool spawn;
        Vector2 ChainOrigin;

        ref float TimeToTravelOut => ref Projectile.ai[0];
        ref float RetractVelX => ref Projectile.ai[1];
        ref float TimeToRetract => ref Projectile.ai[2];

        float maxTimeToRetract;

        public override void AI()
        {
            if (!spawn)
            {
                spawn = true;
                ChainOrigin = Projectile.Center;
                maxTimeToRetract = TimeToRetract;
            }

            if (TimeToTravelOut > 0)
            {
                TimeToTravelOut--;
            }
            else if (TimeToRetract > 0)
            {
                TimeToRetract--;
                Projectile.velocity = new Vector2(RetractVelX, 0f);
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
            }

            //if (Projectile.whoAmI == Main.projectile.First(p => p.active && p.type == Projectile.type).whoAmI)
            //    Main.NewText($"{Projectile.velocity.X} {TimeToTravelOut} {TimeToRetract} {RetractVelX}");

            Projectile.rotation = Projectile.DirectionFrom(ChainOrigin).ToRotation() + MathHelper.Pi;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (target.FargoSouls().BetsyDashing)
                return;
            target.AddBuff(ModContent.BuffType<CurseoftheMoonBuff>(), 240);
                target.AddBuff(ModContent.BuffType<MutantFangBuff>(), 180);
        }

        private static float X(float t, float x0, float x1, float x2)
        {
            return (float)(
                x0 * Math.Pow(1 - t, 2) +
                x1 * 2 * t * Math.Pow(1 - t, 1) +
                x2 * Math.Pow(t, 2)
            );
        }
        private static float Y(float t, float y0, float y1, float y2)
        {
            return (float)(
                 y0 * Math.Pow(1 - t, 2) +
                 y1 * 2 * t * Math.Pow(1 - t, 1) +
                 y2 * Math.Pow(t, 2)
             );
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (TextureAssets.Chain.IsLoaded)
            {
                Texture2D texture = TextureAssets.Chain12.Value;
                Vector2 position = Projectile.Center;
                Vector2 mountedCenter = ChainOrigin;
                Rectangle? sourceRectangle = new Rectangle?();
                Vector2 origin = new(texture.Width * 0.5f, texture.Height * 0.5f);
                float num1 = texture.Height;
                Vector2 vector24 = mountedCenter - position;
                float rotation = (float)Math.Atan2(vector24.Y, vector24.X) - 1.57f;
                bool flag = true;
                if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                    flag = false;
                if (float.IsNaN(vector24.X) && float.IsNaN(vector24.Y))
                    flag = false;
                while (flag)
                    if (vector24.Length() < num1 + 1.0)
                    {
                        flag = false;
                    }
                    else
                    {
                        Vector2 vector21 = vector24;
                        vector21.Normalize();
                        position += vector21 * num1;
                        vector24 = mountedCenter - position;
                        Color color2 = Lighting.GetColor((int)position.X / 16, (int)(position.Y / 16.0));
                        Main.EntitySpriteDraw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0);
                    }
                
                /*Texture2D texture = TextureAssets.Chain12.Value;
                Vector2 connector = Projectile.Center;
                Vector2 myOrigin = ChainOrigin;
                float chainsPerUse = 0.05f;
                float sag = TimeToTravelOut > 0 ? 0 : 80f * (1f - TimeToRetract / maxTimeToRetract);
                for (float j = 0; j <= 1; j += chainsPerUse)
                {
                    if (j == 0)
                        continue;
                    Vector2 distBetween = new(X(j, myOrigin.X, (myOrigin.X + connector.X) / 2, connector.X) -
                    X(j - chainsPerUse, myOrigin.X, (myOrigin.X + connector.X) / 2, connector.X),
                    Y(j, myOrigin.Y, myOrigin.Y + sag, connector.Y) -
                    Y(j - chainsPerUse, myOrigin.Y, myOrigin.Y + sag, connector.Y));
                    float chainDist = 36;
                    float chainUse = 0.05f;
                    if (distBetween.Length() > chainDist && chainsPerUse > chainUse)
                    {
                        chainsPerUse -= chainUse;
                        j -= chainsPerUse;
                        continue;
                    }
                    float projTrueRotation = distBetween.ToRotation() - (float)Math.PI / 2;
                    Vector2 lightPos = new(X(j, myOrigin.X, (myOrigin.X + connector.X) / 2, connector.X), Y(j, myOrigin.Y, myOrigin.Y + 50, connector.Y));
                    Main.spriteBatch.Draw(texture, new Vector2(X(j, myOrigin.X, (myOrigin.X + connector.X) / 2, connector.X) - Main.screenPosition.X, Y(j, myOrigin.Y, myOrigin.Y + 50, connector.Y) - Main.screenPosition.Y),
                    new Rectangle(0, 0, texture.Width, texture.Height), Projectile.GetAlpha(Lighting.GetColor((int)lightPos.X / 16, (int)lightPos.Y / 16)), projTrueRotation,
                    new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, connector.X < myOrigin.X ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }*/
            }

            if (TimeToTravelOut > 0)
            {
                Texture2D texture2D13 = TextureAssets.Projectile[Projectile.type].Value;
                int num156 = TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
                int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
                Rectangle rectangle = new(0, y3, texture2D13.Width, num156);
                Vector2 origin2 = rectangle.Size() / 2f;
                SpriteEffects effects = SpriteEffects.None;
                Color color = Projectile.GetAlpha(lightColor);

                Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Projectile.GetAlpha(lightColor), Projectile.rotation, origin2, Projectile.scale, effects, 0);
            }
            return false;
        }
    }
}