using CSE.Content.Common.Bosses.MutantEX;
using FargowiltasSouls;
using FargowiltasSouls.Assets.Textures;
using FargowiltasSouls.Content.Bosses.VanillaEternity;
using FargowiltasSouls.Content.Buffs.Boss;
using FargowiltasSouls.Content.Buffs.Eternity;
using FargowiltasSouls.Core;
using FargowiltasSouls.Core.Globals;
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
    public class MutantWallOfFlesh : ModProjectile
    {
        public override string Texture => "FargowiltasSouls/Assets/Textures/EModeResprites/WallOfFlesh";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 2000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 110;
            Projectile.height = 420;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 2;
            Projectile.hide = true;
            CooldownSlot = ImmunityCooldownID.Bosses;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }

        bool spawn;

        ref float MyPriority => ref Projectile.ai[0];
        ref float TimeToTravel => ref Projectile.ai[1];
        ref float TimeToLive => ref Projectile.ai[2];

        int direction;

        public override void AI()
        {
            if (!spawn)
            {
                spawn = true;
                direction = Projectile.direction = Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
                Projectile.timeLeft = (int)TimeToLive;

                if (MyPriority == 0 && FargoSoulsUtil.HostCheck)
                {
                    Vector2 spawnPos = Projectile.Center;// Projectile.direction < 0 ? Projectile.Left : Projectile.Right;
                    spawnPos.X += Projectile.width * 0.2f * Projectile.direction;
                    float mouthType = Projectile.direction < 0 ? 0 : 2;
                    NPC mutant = FargoSoulsUtil.NPCExists(EModeGlobalNPC.mutantBoss, ModContent.NPCType<MutantEXBoss>());
                    if (mutant != null && Math.Sign(Main.player[mutant.target].Center.X - mutant.Center.X) < 0)
                    {
                        if (mouthType == 0)
                            mouthType = 2;
                        else
                            mouthType = 0;
                    }
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), spawnPos, Projectile.velocity, ModContent.ProjectileType<MutantWofPart>(), Projectile.damage, Projectile.knockBack, Projectile.owner, mouthType, TimeToTravel, TimeToLive);
                    for (int j = -1; j <= 1; j += 2)
                    {
                        const float yWallEyeOffset = 600; //make sure this matches mutant's
                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), spawnPos + yWallEyeOffset * Vector2.UnitY * j, Projectile.velocity, ModContent.ProjectileType<MutantWofPart>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 1f, TimeToTravel, TimeToLive);
                    }
                }
            }

            Projectile.direction = Projectile.spriteDirection = direction;

            if (--TimeToTravel < 0)
            {
                Projectile.velocity = Vector2.Zero;
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (!Main.dedServ && MyPriority == 0 && Math.Abs(Projectile.Center.Y - Main.LocalPlayer.Center.Y) < Projectile.height / 2)
            {
                // eyes use 137, 139
                // mouth uses 137, 138
                /*Gore.NewGore(new Vector2(position.X, position.Y), velocity, 137, scale);
                Gore.NewGore(new Vector2(position.X, position.Y + (float)(height / 2)), velocity, 138, scale);
                Gore.NewGore(new Vector2(position.X + (float)(width / 2), position.Y), velocity, 138, scale);
                Gore.NewGore(new Vector2(position.X + (float)(width / 2), position.Y + (float)(height / 2)), velocity, 137, scale);*/

                //ripped code for vanilla wof death
                int screenPosY = (int)Main.screenPosition.Y;
                int bottomOfScreen = screenPosY + Main.screenHeight;
                int posX = (int)Projectile.Center.X;

                int maxPosX = posX + 140;
                int goreX = posX;
                for (int i = screenPosY; i < bottomOfScreen; i += 100)
                {
                    for (; goreX < maxPosX; goreX += 48)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            Dust.NewDust(new Vector2(goreX, i), 32, 32, 5, (float)Main.rand.Next(-60, 61) * 0.1f, (float)Main.rand.Next(-60, 61) * 0.1f);
                        }
                        Vector2 vel = new Vector2((float)Main.rand.Next(-80, 81) * 0.1f, (float)Main.rand.Next(-60, 21) * 0.1f);
                        vel.X -= 7 * direction; //so they dont fall in your way
                        Gore.NewGore(Projectile.InheritSource(Projectile), Velocity: vel, Position: new Vector2(goreX, i), Type: Main.rand.Next(140, 143), Scale: Projectile.scale);
                    }

                    goreX = posX;
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (target.FargoSouls().BetsyDashing)
                return;
            target.AddBuff(ModContent.BuffType<CurseoftheMoonBuff>(), 240);
            if (WorldSavingSystem.EternityMode)
                target.AddBuff(ModContent.BuffType<MutantFangBuff>(), 180);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = TextureAssets.Projectile[Projectile.type].Value;
            int num156 = TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            SpriteEffects effects = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Color color = Projectile.GetAlpha(lightColor);

            Vector2 drawPos = Projectile.Center;
            Color newColor = color;
            Main.EntitySpriteDraw(texture2D13, drawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Projectile.GetAlpha(newColor), Projectile.rotation, origin2, Projectile.scale, effects, 0);

            return false;
        }
    }
}