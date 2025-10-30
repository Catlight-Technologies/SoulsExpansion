using CSE.Content.Common.Projectiles;
using CSE.Content.Common.Summons;
using Fargowiltas.Content.Items.Summons.VanillaCopy;
using FargowiltasSouls.Content.Buffs.Boss;
using FargowiltasSouls.Content.Buffs.Eternity;
using FargowiltasSouls.Content.Buffs.Souls;
using FargowiltasSouls.Content.Projectiles.Weapons.BossWeapons;
using FargowiltasSouls.Core.Globals;
using FargowiltasSouls.Core.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CSE.Core.Common.ModPlayers
{
    public partial class CSEPlayer : ModPlayer
    {
        public float throwerVelocity = 1f;
        public bool CyclonicFin;
        public int CyclonicFinCD;

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (CyclonicFin)
            {
                target.AddBuff(ModContent.BuffType<OceanicMaulBuff>(), 900);
                target.AddBuff(ModContent.BuffType<CurseoftheMoonBuff>(), 900);

                if (hit.Crit && CyclonicFinCD <= 0 && proj.type != ModContent.ProjectileType<RazorbladeTyphoonFriendly>())
                {
                    CyclonicFinCD = 360;

                    float screenX = Main.screenPosition.X;
                    if (Player.direction < 0)
                        screenX += Main.screenWidth;
                    float screenY = Main.screenPosition.Y;
                    screenY += Main.rand.Next(Main.screenHeight);
                    Vector2 spawn = new Vector2(screenX, screenY);
                    Vector2 vel = target.Center - spawn;
                    vel.Normalize();
                    vel *= 27f;
                    Projectile.NewProjectile(proj.GetSource_FromThis(), spawn, vel, ModContent.ProjectileType<SpectralFishron>(), 200, 10f, proj.owner, target.whoAmI, 300);
                }
            }
        }
        public override void UpdateEquips()
        {
            if (Player.HasBuff<MutantPresenceBuff>())
            {
                Player.mount.Dismount(Player);
                //Player.AddBuff(ModContent.BuffType<TimeStopCDBuff>(), 300);
            }
        }
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            if (attempt.playerFishingConditions.BaitItemType == ModContent.ItemType<TruffleWormEX>())
            {
                itemDrop = 0;
                bool spawned = false;
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].bobber
                        && Main.projectile[i].owner == Player.whoAmI && Player.whoAmI == Main.myPlayer)
                    {
                        Main.projectile[i].ai[0] = 2f;
                        Main.projectile[i].netUpdate = true;

                        if (!spawned && Main.projectile[i].wet && WorldSavingSystem.EternityMode && !NPC.AnyNPCs(NPCID.DukeFishron))
                        {
                            spawned = true;
                            if (Main.netMode == NetmodeID.SinglePlayer)
                            {
                                EModeGlobalNPC.spawnFishronEX = true;
                                NPC.NewNPC(Main.projectile[i].GetSource_FromThis(), (int)Main.projectile[i].Center.X, (int)Main.projectile[i].Center.Y + 100, NPCID.DukeFishron, 0, 0f, 0f, 0f, 0f, Player.whoAmI);
                                Main.NewText("Duke Fishron EX has awoken!", 50, 100, 255);
                            }
                            else if (Main.netMode == NetmodeID.MultiplayerClient)
                            {
                                var netMessage = Mod.GetPacket();
                                netMessage.Write((byte)CSE.PacketID.SpawnFishronEX);
                                netMessage.Write((byte)Player.whoAmI);
                                netMessage.Write((int)Main.projectile[i].Center.X);
                                netMessage.Write((int)Main.projectile[i].Center.Y + 100);
                                netMessage.Send();
                            }
                            else if (Main.netMode == NetmodeID.Server)
                            {
                                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("???????"), Color.White);
                            }
                        }
                    }
                }
            }
        }
        public override void ResetEffects()
        {
            throwerVelocity = 1f;
            CyclonicFin = false;
            if (CyclonicFinCD > 0)
                CyclonicFinCD--;
        }
    }
}