using CSE.Content.Common.Projectiles.MutantEX;
using Fargowiltas.Content.NPCs;
using FargowiltasSouls;
using FargowiltasSouls.Assets.Sounds;
using FargowiltasSouls.Assets.Textures;
using FargowiltasSouls.Common.Utilities;
using FargowiltasSouls.Content.BossBars;
using FargowiltasSouls.Content.Bosses.VanillaEternity;
using FargowiltasSouls.Content.Buffs.Boss;
using FargowiltasSouls.Content.Buffs.Eternity;
using FargowiltasSouls.Content.Buffs.Souls;
using FargowiltasSouls.Content.Items.BossBags;
using FargowiltasSouls.Content.Items.Materials;
using FargowiltasSouls.Content.Items.Pets;
using FargowiltasSouls.Content.Items.Placables.Relics;
using FargowiltasSouls.Content.Items.Placables.Trophies;
using FargowiltasSouls.Content.Items.Summons;
using FargowiltasSouls.Content.Projectiles;
using FargowiltasSouls.Content.Projectiles.Eternity;
using FargowiltasSouls.Content.Projectiles.Eternity.Bosses;
using FargowiltasSouls.Content.Projectiles.Eternity.Bosses.BrainOfCthulhu;
using FargowiltasSouls.Content.Projectiles.Eternity.Bosses.MechanicalBosses;
using FargowiltasSouls.Core.Globals;
using FargowiltasSouls.Core.ItemDropRules.Conditions;
using FargowiltasSouls.Core.Systems;
using Luminance.Common.Utilities;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CSE.Content.Common.Bosses.MutantEX
{
    [AutoloadBossHead]
    public class MutantEXBoss : ModNPC
    {
        //public override string Texture => $"CSE/Content/Common/Bosses/MutantEX/MutantEX{FargoSoulsUtil.TryAprilFoolsTexture}";

        public SlotId? TelegraphSound = null;

        Player player => Main.player[NPC.target];

        public bool playerInvulTriggered;
        public int ritualProj, spriteProj, ringProj;
        private bool droppedSummon = false;

        public List<P1Attacks> P1AvailableAttacks = [];

        public Queue<float> attackHistory = new();
        public int attackCount;

        public int hyper;

        public float endTimeVariance;

        public bool ShouldDrawAura;
        public float AuraScale = 1f;

        public ref float AttackChoice => ref NPC.ai[0];
        public ref float PhaseState => ref NPC.localAI[3];

        public Vector2 AuraCenter = Vector2.Zero;
        public bool ShouldMoveArena = true;

        string TownNPCName;

        public const int HyperMax = 5;

        public static bool DoingSuperFinalSpark;

        public enum P1Attacks
        {
            SpearTossDirect = 0,
            Spheres = 1,
            SpearTossDiagonal = 2,
            SpearTossDiagonalEnd = 3,
            BoundaryDash = 4,
            BoundaryDash2 = 5,
            BoundaryDash3 = 6,
            BoundaryDash4 = 7,
            VoidRays = 8,
            Sword = 9
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(NPC.type);
            NPCID.Sets.MustAlwaysDraw[Type] = true;
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange([
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement($"Mods.FargowiltasSouls.Bestiary.{Name}")
            ]);
        }

        public override void SetDefaults()
        {
            NPC.width = 120;
            NPC.height = 120;
            NPC.damage = 400;
            NPC.defense = 200;
            NPC.value = Item.buyPrice(15);
            NPC.lifeMax = Main.expertMode ? 7500000 : 5000000;
            NPC.HitSound = SoundID.NPCHit57;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.npcSlots = 50f;
            NPC.knockBackResist = 0f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.aiStyle = -1;
            NPC.netAlways = true;
            NPC.timeLeft = NPC.activeTime * 30;
            NPC.BossBar = ModContent.GetInstance<MutantBossBar>();
            if (WorldSavingSystem.AngryMutant)
            {
                NPC.damage *= 17;
                NPC.defense *= 10;
            }

            if (ModLoader.TryGetMod("FargowiltasMusic", out Mod musicMod))
            {
                Music = MusicLoader.GetMusicSlot(musicMod,
                    WorldSavingSystem.MasochistModeReal ? "Assets/Music/rePrologue" : "Assets/Music/SteelRed");
            }
            else
            {
                Music = MusicID.OtherworldlyTowers;
            }

            SceneEffectPriority = SceneEffectPriority.BossHigh;

            if (FargoSoulsUtil.AprilFools)
                NPC.GivenName = Language.GetTextValue("Mods.FargowiltasSouls.NPCs.MutantBoss_April.DisplayName");
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.damage = (int)Math.Round(NPC.damage * 0.5);
            NPC.lifeMax = (int)Math.Round(NPC.lifeMax * 0.5 * balance);
        }
        public override bool CanHitPlayer(Player target, ref int CooldownSlot)
        {
            CooldownSlot = ImmunityCooldownID.Bosses;

            if (AttackChoice <= -1)
                return false;

            if (WorldSavingSystem.MasochistModeReal && Main.getGoodWorld)
                return base.CanHitPlayer(target, ref CooldownSlot);

            if (WorldSavingSystem.MasochistModeReal)
                return NPC.Distance(FargoSoulsUtil.ClosestPointInHitbox(target, NPC.Center)) < Player.defaultHeight;

            return false;
        }

        public override bool CanHitNPC(NPC target)
        {
            if (target.type == ModContent.NPCType<Deviantt>() || target.type == ModContent.NPCType<Abominationn>() || target.type == ModContent.NPCType<Mutant>())
                return false;
            return base.CanHitNPC(target);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.localAI[0]);
            writer.Write(NPC.localAI[1]);
            writer.Write(NPC.localAI[2]);
            writer.Write(endTimeVariance);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[0] = reader.ReadSingle();
            NPC.localAI[1] = reader.ReadSingle();
            NPC.localAI[2] = reader.ReadSingle();
            endTimeVariance = reader.ReadSingle();
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (ModContent.TryFind("Fargowiltas", "Mutant", out ModNPC modNPC))
            {
                int n = NPC.FindFirstNPC(modNPC.Type);
                if (n != -1 && n != Main.maxNPCs)
                {
                    NPC.Bottom = Main.npc[n].Bottom;
                    TownNPCName = Main.npc[n].GivenName;

                    Main.npc[n].life = 0;
                    Main.npc[n].active = false;
                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, n);
                }
            }
            AuraCenter = NPC.Center;
        }

        public override bool PreAI()
        {
            if (WorldSavingSystem.MasochistModeReal)
            {
                if (!Main.dedServ && !Main.LocalPlayer.ItemTimeIsZero && (Main.LocalPlayer.HeldItem.type == ItemID.RodofDiscord || Main.LocalPlayer.HeldItem.type == ItemID.RodOfHarmony))
                    Main.LocalPlayer.AddBuff(ModContent.BuffType<TimeFrozenBuff>(), 600);
            }
            return base.PreAI();
        }

        public override void AI()
        {
            EModeGlobalNPC.mutantBoss = NPC.whoAmI;

            NPC.dontTakeDamage = AttackChoice < 0;

            if (AttackChoice != -5)
                DoingSuperFinalSpark = false;

            ShouldDrawAura = false;

            ManageAurasAndPreSpawn();
            ManageNeededProjectiles();

            NPC.direction = NPC.spriteDirection = NPC.Center.X < player.Center.X ? 1 : -1;

            bool drainLifeInP3 = true;

            if (TelegraphSound != null)
            {
                if (SoundEngine.TryGetActiveSound(TelegraphSound.Value, out ActiveSound s))
                {
                    s.Position = NPC.Center;
                }
            }

            switch ((int)AttackChoice)
            {
                #region phase 1

                case 0: SpearTossDirectP1AndChecks(); break;

                case 1: SpheresAndDiveP1(); break;

                case 2: PrepareDiagonalSpearThrow(); break;

                case 3: DiagonalSpearThrow(); break;

                case 4: PrepareSpearDashDirectP1(); break;
                case 5: SpearDashDirectP1(); break;
                case 6: WhileDashingP1(); break;

                case 7: ApproachForNextAttackP1(); break;
                case 8: VoidRaysP1(); break;

                case 9: BoundaryBulletHellAndSwordP1(); break;

                #endregion

                #region phase 2

                case 10: Phase2Transition(); break;

                case 11: goto case 35;
                case 12: QueenSlimeRain(); break;

                case 13: PrepareSpearDashPredictiveP2(); break;
                case 14: SpearDashPredictiveP2(); break;
                case 15: WhileDashingP2(); break;

                case 16: SANSGOLEM(); break;

                case 17: AttackChoice++; break;
                case 18: AttackChoice++; break; //new attack can be put here

                case 19: PillarDunk(); break;

                case 20: EOCStarSickles(); break;

                case 21: PrepareSpearDashDirectP2(); break;
                case 22: SpearDashDirectP2(); break;
                case 23: //while dashing
                    if (NPC.ai[1] % 3 == 0)
                        NPC.ai[1]++;
                    goto case 15;

                case 24: SpawnDestroyersForPredictiveThrow(); break;
                case 25: SpearTossPredictiveP2(); break;

                case 26: PrepareMechRayFan(); break;
                case 27: MechRayFan(); break;

                case 28: RoomOfFlesh(); break;

                case 29: PrepareFishron1(); break;
                case 30: SpawnFishrons(); break;

                case 31: goto case 39;
                case 32: BoundaryBulletHellP2(); break;

                case 33: PrepareNuke(); break;
                case 34: Nuke(); break;

                case 35: PrepareSlimeRain(); break;
                case 36: SlimeRain(); break;

                case 37: PrepareFishron2(); break;
                case 38: goto case 30; //spawn shadow hands

                case 39: PrepareMyBallsP2(); break;
                case 40: MyBallsP2(); break;

                case 41: SpearTossDirectP2(); break;

                case 42: PrepareTwinRangsAndCrystals(); break;
                case 43: TwinRangsAndCrystals(); break;

                case 44: EmpressSwordWave(); break;

                case 45: PrepareMutantSword(); break;
                case 46: MutantSword(); break;

                //gap in the numbers here so the ai loops right
                //when adding a new attack, remember to make ChooseNextAttack() point to the right case!

                case 52: P2NextAttackPause(); break;

                #endregion

                #region phase 3

                case -1: drainLifeInP3 = Phase3Transition(); break;

                case -2: VoidRaysP3(); break;

                case -3: MyBallsP3(); break;

                case -4: BoundaryBulletHellP3(); break;

                case -5: FinalSpark(); break;

                case -6: DyingDramaticPause(); break;
                case -7: DyingAnimationAndHandling(); break;

                #endregion

                default: AttackChoice = 13; goto case 13; //return to first phase 2 attack
            }
            //manage aura scale
            if (AttackChoice == (int)P1Attacks.Spheres)
                AuraScale = MathHelper.Lerp(AuraScale, 0.5f, 0.0125f);
            else
                AuraScale = MathHelper.Lerp(AuraScale, 0.8f, 0.05f);

            //manage arena position
            if (ShouldMoveArena) //spear dash direct p1
            {
                AuraCenter = Vector2.Lerp(AuraCenter, NPC.Center, 0.1f);
            }
            ShouldMoveArena = true;

            //in emode p2
            if (AttackChoice < 0 || AttackChoice > 10 || AttackChoice == 10 && NPC.ai[1] > 150)
            {
                Main.dayTime = false;
                Main.time = 16200; //midnight, for empress visuals

                Main.raining = false; //disable rain
                Main.rainTime = 0;
                Main.maxRaining = 0;

                Main.bloodMoon = false; //disable blood moon
            }

            if (AttackChoice < 0 && NPC.life > 1 && drainLifeInP3) //in desperation
            {
                int time = 480 + 240 + 420 + 480 + 1020 - 60;
                if (WorldSavingSystem.MasochistModeReal)
                    time = Main.getGoodWorld ? 5000 : 4350;
                int drain = NPC.lifeMax / time;
                NPC.life -= drain;
                if (NPC.life < 1)
                    NPC.life = 1;
            }

            if (player.immune || player.hurtCooldowns[0] != 0 || player.hurtCooldowns[1] != 0)
                playerInvulTriggered = true;
            //drop summon
            if (NPC.HasPlayerTarget)
            {
                EModeUtils.DropSummon(NPC, ModContent.ItemType<MutantsCurse>(), WorldSavingSystem.DownedMutant, ref droppedSummon, NPC.downedMoonlord);
            }
            

            if (WorldSavingSystem.MasochistModeReal && Main.getGoodWorld && ++hyper > HyperMax + 1)
            {
                hyper = 0;
                NPC.AI();
            }
        }

        #region helper functions

        bool spawned;
        void ManageAurasAndPreSpawn()
        {
            if (!spawned)
            {
                spawned = true;

                int prevLifeMax = NPC.lifeMax;
                if (WorldSavingSystem.AngryMutant) //doing it here to avoid overflow i think
                {
                    NPC.lifeMax *= 100;
                    if (NPC.lifeMax < prevLifeMax)
                        NPC.lifeMax = int.MaxValue;
                }
                NPC.life = NPC.lifeMax;

                if (player.FargoSouls().TerrariaSoul && WorldSavingSystem.MasochistModeReal)
                    EdgyBossText(GFBQuote(1));
            }

            if (WorldSavingSystem.MasochistModeReal && Main.LocalPlayer.active && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost)
                Main.LocalPlayer.AddBuff(ModContent.BuffType<MutantPresenceBuff>(), 2);

            Main.dayTime = false;
            Main.time = 16200; //midnight

            if (PhaseState == 0)
            {
                NPC.TargetClosest();
                if (NPC.timeLeft < 30)
                    NPC.timeLeft = 30;

                AuraCenter = NPC.Center;

                if (NPC.Distance(Main.player[NPC.target].Center) < 1500)
                {
                    PhaseState = 1;
                    SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                    EdgyBossText(GFBQuote(2));
                }
            }
            else if (PhaseState == 1)
            {
                ShouldDrawAura = true;
                // -1 means no dust is drawn, as it looks ugly.
                ArenaAura(AuraCenter, 2000f * AuraScale, true, -1, default, ModContent.BuffType<GodEaterBuff>(), ModContent.BuffType<MutantFangBuff>());

                if (!SkyManager.Instance["CSE:MutantEXBoss1"].IsActive())
                    SkyManager.Instance.Activate("CSE:MutantEXBoss1");
            }
            else
            {
                if (Main.LocalPlayer.active && NPC.Distance(Main.LocalPlayer.Center) < 3000f)
                {
                    Main.LocalPlayer.AddBuff(ModContent.BuffType<MutantPresenceBuff>(), 2);
                    if (Main.getGoodWorld)
                        Main.LocalPlayer.AddBuff(ModContent.BuffType<GoldenStasisCDBuff>(), 2);

                    if (AttackChoice < 0 && AttackChoice > -6)
                    {
                        Main.LocalPlayer.AddBuff(ModContent.BuffType<GoldenStasisCDBuff>(), 2);
                        if (WorldSavingSystem.MasochistModeReal)
                        {
                            Main.LocalPlayer.AddBuff(ModContent.BuffType<TimeStopCDBuff>(), 2);
                            Main.LocalPlayer.AddBuff(ModContent.BuffType<MutantDesperationBuff>(), 2);
                        }
                            
                    }
                }
            }
        }

        void ManageNeededProjectiles()
        {
            if (FargoSoulsUtil.HostCheck) //checks for needed projs
            {
                if (AttackChoice != -7 && (AttackChoice < 0 || AttackChoice > 10) && FargoSoulsUtil.ProjectileExists(ritualProj, ModContent.ProjectileType<MutantRitual>()) == null)
                    ritualProj = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantRitual>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, 0f, NPC.whoAmI);

                if (FargoSoulsUtil.ProjectileExists(ringProj, ModContent.ProjectileType<MutantRitual5>()) == null)
                    ringProj = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantRitual5>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                if (FargoSoulsUtil.ProjectileExists(spriteProj, ModContent.ProjectileType<MutantBossProjectile>()) == null)
                {
                    /*if (Main.netMode == NetmodeID.Server)
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("wheres my sprite"), Color.LimeGreen);
                    else
                        Main.NewText("wheres my sprite");*/
                    if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        int number = 0;
                        for (int index = 999; index >= 0; --index)
                        {
                            if (!Main.projectile[index].active)
                            {
                                number = index;
                                break;
                            }
                        }
                        if (number >= 0)
                        {
                            Projectile projectile = Main.projectile[number];
                            projectile.SetDefaults(ModContent.ProjectileType<MutantBossProjectile>());
                            projectile.Center = NPC.Center;
                            projectile.owner = Main.myPlayer;
                            projectile.velocity.X = 0;
                            projectile.velocity.Y = 0;
                            projectile.damage = 0;
                            projectile.knockBack = 0f;
                            projectile.identity = number;
                            projectile.gfxOffY = 0f;
                            projectile.stepSpeed = 1f;
                            projectile.ai[1] = NPC.whoAmI;

                            spriteProj = number;
                        }
                    }
                    else //server
                    {
                        spriteProj = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantBossProjectile>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                        /*if (Main.netMode == NetmodeID.Server)
                            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"got sprite {spriteProj}"), Color.LimeGreen);
                        else
                            Main.NewText($"got sprite {spriteProj}");*/
                    }
                }
            }
        }

        void ChooseNextAttack(params int[] args)
        {
            float buffer = AttackChoice + 1;
            AttackChoice = 52;
            NPC.ai[1] = 0;
            NPC.ai[2] = buffer;
            NPC.ai[3] = 0;
            NPC.localAI[0] = 0;
            NPC.localAI[1] = 0;
            NPC.localAI[2] = 0;
            //NPC.TargetClosest();
            NPC.netUpdate = true;

            EdgyBossText(RandomObnoxiousQuote());

            /*string text = "-------------------------------------------------";
            Main.NewText(text);

            text = "";
            foreach (float f in attackHistory)
                text += f.ToString() + " ";
            Main.NewText($"history: {text}");*/

                //become more likely to use randoms as life decreases
                bool useRandomizer = PhaseState >= 3 && (WorldSavingSystem.MasochistModeReal || Main.rand.NextFloat(0.8f) + 0.2f > (float)Math.Pow((float)NPC.life / NPC.lifeMax, 2));

                if (FargoSoulsUtil.HostCheck)
                {
                    Queue<float> recentAttacks = new(attackHistory); //copy of attack history that i can remove elements from freely

                    //if randomizer, start with a random attack, else use the previous state + 1 as starting attempt BUT DO SOMETHING ELSE IF IT'S ALREADY USED
                    if (useRandomizer)
                        NPC.ai[2] = Main.rand.Next(args);

                    //Main.NewText(useRandomizer ? "(Starting with random)" : "(Starting with regular next attack)");

                    while (recentAttacks.Count > 0)
                    {
                        bool foundAttackToUse = false;

                        for (int i = 0; i < 5; i++) //try to get next attack that isnt in this queue
                        {
                            if (!recentAttacks.Contains(NPC.ai[2]))
                            {
                                foundAttackToUse = true;
                                break;
                            }
                            NPC.ai[2] = Main.rand.Next(args);
                        }

                        if (foundAttackToUse)
                            break;

                        //couldn't find an attack to use after those attempts, forget 1 attack and repeat
                        recentAttacks.Dequeue();

                        //Main.NewText("REDUCE");
                    }

                    /*text = "";
                    foreach (float f in recentAttacks)
                        text += f.ToString() + " ";
                    Main.NewText($"recent: {text}");*/
                }

            if (FargoSoulsUtil.HostCheck)
            {
                int maxMemory = WorldSavingSystem.MasochistModeReal ? 12 : 18;

                //after doing this many attacks, shorten queue so i can be more random again
                //dont clear entire queue, keep the most recent attacks. this way i dont immediately reuse an attack when i refresh
                if (attackCount++ > maxMemory * 1.25)
                {
                    attackCount = 0;
                    maxMemory /= 4;
                }

                attackHistory.Enqueue(NPC.ai[2]);
                while (attackHistory.Count > maxMemory)
                    attackHistory.Dequeue();
            }

            endTimeVariance = WorldSavingSystem.MasochistModeReal ? Main.rand.NextFloat(-0.5f, 1f) : 0;

            /*text = "";
            foreach (float f in attackHistory)
                text += f.ToString() + " ";
            Main.NewText($"after: {text}");*/

            //NPC.ai[2] = 28; // debug
        }

        void P1NextAttackOrMasoOptions(float sourceAI)
        {
            List<P1Attacks> GetAttacks()
            {
                var attacks = new List<P1Attacks>(P1AvailableAttacks);

                attacks.Remove((P1Attacks)sourceAI); // just to make sure
                if (sourceAI == (int)P1Attacks.SpearTossDiagonalEnd)
                    attacks.Remove(P1Attacks.SpearTossDiagonal);
                if (sourceAI == (int)P1Attacks.BoundaryDash4)
                    attacks.Remove(P1Attacks.BoundaryDash);

                // remove bad combos
                if (sourceAI == (int)P1Attacks.SpearTossDirect)
                    attacks.Remove(P1Attacks.SpearTossDiagonal);
                if (sourceAI == (int)P1Attacks.SpearTossDiagonalEnd)
                    attacks.Remove(P1Attacks.SpearTossDirect);

                if (attacks.Count == 0)
                {
                    P1AvailableAttacks = [P1Attacks.SpearTossDirect, P1Attacks.Spheres, P1Attacks.SpearTossDiagonal, P1Attacks.BoundaryDash, P1Attacks.VoidRays, P1Attacks.Sword];
                    P1AvailableAttacks.Remove((P1Attacks)sourceAI);
                    if (sourceAI == (int)P1Attacks.SpearTossDiagonalEnd)
                        P1AvailableAttacks.Remove(P1Attacks.SpearTossDiagonal);
                    if (sourceAI == (int)P1Attacks.BoundaryDash4)
                        P1AvailableAttacks.Remove(P1Attacks.BoundaryDash);
                    attacks = GetAttacks();
                }
                return attacks;
            }

            if (AttackChoice == 9 && NPC.localAI[2] == 0)
            {
                NPC.localAI[2] = 1;
            }
            else
            {
                if (FargoSoulsUtil.HostCheck) //only run for host in mp, will sync to others
                {
                    var attacks = GetAttacks();

                    /*
                    string text = "";
                    foreach (var attack in attacks)
                        text += attack + " ";
                    Main.NewText(text);
                    */

                    AttackChoice = (int)Main.rand.NextFromCollection(attacks);
                    P1AvailableAttacks.Remove((P1Attacks)AttackChoice);
                    NPC.localAI[2] = 0f;
                }
            }


            NPC.ai[1] = 0;
            NPC.ai[2] = 0;
            NPC.ai[3] = 0;
            NPC.localAI[0] = 0;
            NPC.localAI[1] = 0;
            //NPC.localAI[2] = 0; //excluded because sword logic
            NPC.netUpdate = true;

            EdgyBossText(RandomObnoxiousQuote());
        }

        void SpawnSphereRing(int max, float speed, int damage, float rotationModifier, float offset = 0, float alt = 0)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;
            float rotation = 2f * (float)Math.PI / max;
            int type = ModContent.ProjectileType<MutantSphereRing>();
            for (int i = 0; i < max; i++)
            {
                Vector2 vel = speed * Vector2.UnitY.RotatedBy(rotation * i + offset);
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel, type, damage, 0f, Main.myPlayer, rotationModifier * NPC.spriteDirection, speed, ai2: alt);
            }
            SoundEngine.PlaySound(SoundID.Item84, NPC.Center);
        }

        bool AliveCheck(Player p, bool forceDespawn = false)
        {
            if (WorldSavingSystem.SwarmActive || forceDespawn || (!p.active || p.dead || Vector2.Distance(NPC.Center, p.Center) > 3000f) && PhaseState > 0)
            {
                NPC.TargetClosest();
                p = Main.player[NPC.target];
                if (WorldSavingSystem.SwarmActive || forceDespawn || !p.active || p.dead || Vector2.Distance(NPC.Center, p.Center) > 3000f)
                {
                    if (NPC.timeLeft > 30)
                        NPC.timeLeft = 30;
                    NPC.velocity.Y -= 1f;
                    if (NPC.timeLeft == 1)
                    {
                        EdgyBossText(GFBQuote(36));
                        if (NPC.position.Y < 0)
                            NPC.position.Y = 0;
                        if (FargoSoulsUtil.HostCheck && ModContent.TryFind("Fargowiltas", "Mutant", out ModNPC modNPC) && !NPC.AnyNPCs(modNPC.Type))
                        {
                            FargoSoulsUtil.ClearHostileProjectiles(2, NPC.whoAmI);

                            int n = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, modNPC.Type);
                            if (n != Main.maxNPCs)
                            {
                                Main.npc[n].homeless = true;
                                if (TownNPCName != default)
                                    Main.npc[n].GivenName = TownNPCName;
                                if (Main.netMode == NetmodeID.Server)
                                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, n);
                            }
                        }
                    }
                    return false;
                }
            }

            if (NPC.timeLeft < 3600)
                NPC.timeLeft = 3600;

            if (player.Center.Y / 16f > Main.worldSurface)
            {
                NPC.velocity.X *= 0.95f;
                NPC.velocity.Y -= 1f;
                if (NPC.velocity.Y < -32f)
                    NPC.velocity.Y = -32f;
                return false;
            }

            return true;
        }

        bool Phase2Check()
        {
            if (Main.expertMode && NPC.life < NPC.lifeMax * (2f / 3))
            {
                if (FargoSoulsUtil.HostCheck)
                {
                    AttackChoice = 10;
                    NPC.ai[1] = 0;
                    NPC.ai[2] = 0;
                    NPC.ai[3] = 0;
                    NPC.netUpdate = true;
                    FargoSoulsUtil.ClearHostileProjectiles(1, NPC.whoAmI);
                    EdgyBossText(GFBQuote(3));
                }
                return true;
            }
            return false;
        }

        void Movement(Vector2 target, float speed, bool fastX = true, bool obeySpeedCap = true)
        {
            float turnaroundModifier = 1f;
            float maxSpeed = 24;

            if (WorldSavingSystem.MasochistModeReal)
            {
                speed *= 2;
                turnaroundModifier *= 2f;
                maxSpeed *= 1.5f;
            }

            if (Math.Abs(NPC.Center.X - target.X) > 10)
            {
                if (NPC.Center.X < target.X)
                {
                    NPC.velocity.X += speed;
                    if (NPC.velocity.X < 0)
                        NPC.velocity.X += speed * (fastX ? 2 : 1) * turnaroundModifier;
                }
                else
                {
                    NPC.velocity.X -= speed;
                    if (NPC.velocity.X > 0)
                        NPC.velocity.X -= speed * (fastX ? 2 : 1) * turnaroundModifier;
                }
            }
            if (NPC.Center.Y < target.Y)
            {
                NPC.velocity.Y += speed;
                if (NPC.velocity.Y < 0)
                    NPC.velocity.Y += speed * 2 * turnaroundModifier;
            }
            else
            {
                NPC.velocity.Y -= speed;
                if (NPC.velocity.Y > 0)
                    NPC.velocity.Y -= speed * 2 * turnaroundModifier;
            }

            if (obeySpeedCap)
            {
                if (Math.Abs(NPC.velocity.X) > maxSpeed)
                    NPC.velocity.X = maxSpeed * Math.Sign(NPC.velocity.X);
                if (Math.Abs(NPC.velocity.Y) > maxSpeed)
                    NPC.velocity.Y = maxSpeed * Math.Sign(NPC.velocity.Y);
            }
        }

        void DramaticTransition(bool fightIsOver, bool normalAnimation = true)
        {
            NPC.velocity = Vector2.Zero;

            if (fightIsOver)
            {
                Main.player[NPC.target].ClearBuff(ModContent.BuffType<MutantFangBuff>());
                Main.player[NPC.target].ClearBuff(ModContent.BuffType<AbomRebirthBuff>());
            }

            SoundEngine.PlaySound(SoundID.Item27 with { Volume = 1.5f }, NPC.Center);

            if (normalAnimation)
            {
                if (FargoSoulsUtil.HostCheck)
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantBomb>(), 0, 0f, Main.myPlayer);
            }

            const int max = 40;
            float totalAmountToHeal = fightIsOver
                ? Main.player[NPC.target].statLifeMax2 / 4f
                : NPC.lifeMax - NPC.life + NPC.lifeMax * 0.1f;
            for (int i = 0; i < max; i++)
            {
                int heal = (int)(Main.rand.NextFloat(0.9f, 1.1f) * totalAmountToHeal / max);
                Vector2 vel = normalAnimation
                    ? Main.rand.NextFloat(2f, 18f) * -Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) //looks messier normally
                    : 0.1f * -Vector2.UnitY.RotatedBy(MathHelper.TwoPi / max * i); //looks controlled during mutant p1 skip
                float ai0 = fightIsOver ? -Main.player[NPC.target].whoAmI - 1 : NPC.whoAmI; //player -1 necessary for edge case of player 0
                float ai1 = vel.Length() / Main.rand.Next(fightIsOver ? 90 : 150, 180); //window in which they begin homing in
                if (FargoSoulsUtil.HostCheck)
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel, ModContent.ProjectileType<MutantHeal>(), heal, 0f, Main.myPlayer, ai0, ai1);
            }
        }

        void EModeSpecialEffects()
        {
                //because this breaks the background???
                if (Main.GameModeInfo.IsJourneyMode && CreativePowerManager.Instance.GetPower<CreativePowers.FreezeTime>().Enabled)
                    CreativePowerManager.Instance.GetPower<CreativePowers.FreezeTime>().SetPowerInfo(false);

                if (!SkyManager.Instance["CSE:MutantEXBoss"].IsActive())
                    SkyManager.Instance.Activate("CSE:MutantEXBoss");

            if (ModLoader.TryGetMod("FargowiltasMusic", out Mod musicMod))
            {
                if (WorldSavingSystem.MasochistModeReal && musicMod.Version >= Version.Parse("0.1.1"))
                    Music = MusicLoader.GetMusicSlot(musicMod, "Assets/Music/Storia");
                else
                    Music = MusicLoader.GetMusicSlot(musicMod, "Assets/Music/rePrologue");
            }

        }

        void FancyFireballs(int repeats)
        {
            float modifier = 0;
            for (int i = 0; i < repeats; i++)
                modifier = MathHelper.Lerp(modifier, 1f, 0.08f);

            float distance = 1600 * (1f - modifier);
            float rotation = MathHelper.TwoPi * modifier;
            const int max = 6;
            for (int i = 0; i < max; i++)
            {
                int d = Dust.NewDust(NPC.Center + distance * Vector2.UnitX.RotatedBy(rotation + MathHelper.TwoPi / max * i), 0, 0, FargoSoulsUtil.AprilFools ? DustID.SolarFlare : DustID.Vortex, NPC.velocity.X * 0.3f, NPC.velocity.Y * 0.3f, newColor: Color.White);
                Main.dust[d].noGravity = true;
                Main.dust[d].scale = 6f - 4f * modifier;
            }
        }

        private void EdgyBossText(string text)
        {
            Color color = Color.Cyan;
            FargoSoulsUtil.PrintText(text, color);
            CombatText.NewText(NPC.Hitbox, color, text, true);
        }
        const int ObnoxiousQuoteCount = 71;
        const string GFBLocPath = $"Mods.FargowiltasSouls.NPCs.MutantBoss.GFBText.";
        private string RandomObnoxiousQuote() => Language.GetTextValue($"{GFBLocPath}Random{Main.rand.Next(ObnoxiousQuoteCount)}");
        private string GFBQuote(int num) => Language.GetTextValue($"{GFBLocPath}Quote{num}");

        #endregion

        #region p1

        void SpearTossDirectP1AndChecks()
        {
            if (!AliveCheck(player))
                return;
            if (Phase2Check())
                return;
            NPC.localAI[2] = 0;
            Vector2 targetPos = player.Center;
            targetPos.X += 500 * (NPC.Center.X < targetPos.X ? -1 : 1);
            if (NPC.Distance(targetPos) > 50)
            {
                Movement(targetPos, PhaseState > 0 ? 0.5f : 2f, true, PhaseState > 0);
            }

            if (NPC.ai[3] == 0)
            {
                NPC.ai[3] = WorldSavingSystem.MasochistModeReal ? Main.rand.Next(2, 6) : Main.rand.Next(3, 5);
                NPC.netUpdate = true;
            }

            if (PhaseState > 0) //dont begin proper ai timer until in range to begin fight
                NPC.ai[1]++;

            int attackTime = WorldSavingSystem.MasochistModeReal ? 150 : 180;

            if (NPC.ai[1] < attackTime - 5) //track player up until just before attack
            {
                NPC.localAI[0] = NPC.SafeDirectionTo(player.Center + player.velocity * 30f).ToRotation();
            }

            if (NPC.ai[1] > attackTime)
            {
                NPC.netUpdate = true;
                //NPC.TargetClosest();
                NPC.ai[1] = attackTime - (WorldSavingSystem.MasochistModeReal ? 90 : 120);
                if (++NPC.ai[2] > NPC.ai[3])
                {
                    P1NextAttackOrMasoOptions(AttackChoice);
                    NPC.velocity = NPC.SafeDirectionTo(player.Center) * 2f;
                }
                else if (FargoSoulsUtil.HostCheck)
                {
                    Vector2 vel = NPC.localAI[0].ToRotationVector2() * 25f;
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel, ModContent.ProjectileType<MutantSpearThrown>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.target, ai2: 1);
                    if (WorldSavingSystem.MasochistModeReal)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Normalize(vel), ModContent.ProjectileType<MutantDeathray2>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, -Vector2.Normalize(vel), ModContent.ProjectileType<MutantDeathray2>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer);
                    }
                }
                NPC.localAI[0] = 0;
            }
            else if (NPC.ai[1] == attackTime - 90 + 1 && NPC.ai[2] < NPC.ai[3] && FargoSoulsUtil.HostCheck)
            {
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.SafeDirectionTo(player.Center + player.velocity * 30f), ModContent.ProjectileType<MutantDeathrayAim>(), 0, 0f, Main.myPlayer, 85f, NPC.whoAmI);
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpearAim>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI, 3);
            }
        }

        void SpheresAndDiveP1()
        {
            if (Phase2Check())
                return;
            ShouldMoveArena = false;

            float sphereTime = 90;
            float divePrepTime = 120;
            float diveTime = 60;
            float diveDuration = 15;

            ref float timer = ref NPC.ai[1];
            if (timer < sphereTime)
            {
                ShouldMoveArena = true;
                NPC.velocity *= 0.8f;
                if (timer == 1 || timer == sphereTime - 1)
                {
                    NPC.netUpdate = true;
                    int max = WorldSavingSystem.MasochistModeReal ? 7 : 5;
                    float speed = 5f;
                    float sign = 1f;
                    SpawnSphereRing(max, speed, (int)(0.8 * FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage)), 1f * sign, alt: 1);
                    SpawnSphereRing(max, speed, (int)(0.8 * FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage)), -0.5f * sign, alt: 1);

                    EdgyBossText(RandomObnoxiousQuote());
                }
            }
            else if (timer < sphereTime + divePrepTime)
            {
                if (timer == sphereTime)
                {
                    if (FargoSoulsUtil.HostCheck)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpearAim>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI, -2);
                    }
                }
                if (timer < sphereTime + divePrepTime - 35)
                {
                    // position diagonal up before flying up
                    Vector2 targetPos = player.Center - Vector2.UnitY * 350;
                    targetPos.X += 350 * Math.Sign(NPC.Center.X - player.Center.X);
                    Movement(targetPos, 1f, true, true);
                }
                else
                {
                    if (timer < sphereTime + divePrepTime - 20)
                    {
                        // fuck off to the top of the screen
                        Vector2 targetPos = player.Center - Vector2.UnitY * 1200;
                        if (NPC.Center != targetPos) //check to prevent edge case NaN
                            NPC.velocity = NPC.DirectionTo(targetPos) * MathF.Min(1700 / diveDuration, NPC.Distance(targetPos));
                    }
                    else
                    {
                        NPC.velocity *= 0.8f;
                    }
                    if (timer == sphereTime + divePrepTime - 20 && FargoSoulsUtil.HostCheck)
                    {
                        //Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.SafeDirectionTo(player.Center + player.velocity * 30f), ModContent.ProjectileType<MutantDeathrayAim>(), 0, 0f, Main.myPlayer, 20f, NPC.whoAmI, ai2: 1);
                    }
                }
            }
            else
            {

                float endAttackTime = sphereTime + divePrepTime + diveDuration;
                if (timer < endAttackTime)
                {
                    NPC.velocity = Vector2.UnitY * 1700 / diveDuration;
                    float triggerHeight = player.Center.Y + 16 * 3;
                    if (NPC.Center.Y > triggerHeight && timer < endAttackTime - 1)
                        timer = endAttackTime - 1;
                }
                else if (timer == endAttackTime) // impact
                {
                    NPC.velocity = Vector2.Zero;

                    if (FargoSoulsUtil.HostCheck)
                    {
                        Vector2 spawnPos = NPC.Center;
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnPos, Vector2.Zero,
                                ModContent.ProjectileType<MutantPenetratorNuke>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer);

                        int projs = WorldSavingSystem.MasochistModeReal ? 12 : 12;
                        for (int i = 0; i < projs; i++)
                        {
                            int extras = WorldSavingSystem.MasochistModeReal ? 2 : 1;
                            for (int j = -extras; j <= extras; j++)
                            {
                                float angle = MathHelper.TwoPi * (float)i / projs;
                                float increment = MathHelper.TwoPi * 1f / projs;
                                angle += increment * j * 0.15f;

                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0f, -13f).RotatedBy(angle),
                                    ModContent.ProjectileType<MutantEye>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer);

                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0f, -10f).RotatedBy(angle + increment * 0.5f),
                                    ModContent.ProjectileType<MutantEye>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer);
                            }

                        }
                    }
                }
                else
                {
                    NPC.velocity = Vector2.Zero;
                    int extraEndlag = WorldSavingSystem.MasochistModeReal ? 0 : 30;
                    if (timer >= sphereTime + divePrepTime + diveTime + extraEndlag)
                    {
                        P1NextAttackOrMasoOptions(AttackChoice);
                    }
                }
            }
            timer++;
        }

        void PrepareDiagonalSpearThrow()
        {
            if (!AliveCheck(player))
                return;
            if (Phase2Check())
                return;

            // this replaces an old prep state, but not necessary anymore.
            // leaves index space for an additional p1 attack though

            AttackChoice++;
            NPC.ai[1] = 0;
            NPC.ai[2] = 0;
            NPC.netUpdate = true;
        }

        void DiagonalSpearThrow()
        {
            Vector2 targetPos = player.Center;
            targetPos.X += 200 * (NPC.Center.X < targetPos.X ? -1 : 1);
            if (NPC.Center.Y > player.Top.Y)
                targetPos.X += 200 * Math.Sign(NPC.Center.X - player.Center.X);
            targetPos.Y -= 400;

            if (NPC.ai[3] == 0)
            {
                NPC.ai[3] = WorldSavingSystem.MasochistModeReal ? Main.rand.Next(2, 6) : Main.rand.Next(3, 5);
                NPC.netUpdate = true;
            }

            targetPos = player.Center + player.DirectionTo(targetPos) * 500;
            if (NPC.Center.Y > player.Top.Y)
                targetPos.X += 200 * Math.Sign(NPC.Center.X - player.Center.X);
            if (NPC.Distance(targetPos) > 50)
            {
                Movement(targetPos, PhaseState > 0 ? 0.5f : 2f, true, PhaseState > 0);
            }

            NPC.ai[1]++;

            int attackTime = WorldSavingSystem.MasochistModeReal ? 150 : 180;

            if (NPC.ai[1] < attackTime - 5) //track player up until just before attack
            {
                NPC.localAI[0] = NPC.SafeDirectionTo(player.Center + player.velocity * 30f).ToRotation();
            }

            if (NPC.ai[1] > attackTime)
            {
                NPC.netUpdate = true;
                //NPC.TargetClosest();
                NPC.ai[1] = attackTime - (WorldSavingSystem.MasochistModeReal ? 90 : 120);
                if (++NPC.ai[2] > NPC.ai[3])
                {
                    P1NextAttackOrMasoOptions(AttackChoice);
                    NPC.velocity = NPC.SafeDirectionTo(player.Center) * 2f;
                }
                else if (FargoSoulsUtil.HostCheck)
                {
                    Vector2 vel = NPC.localAI[0].ToRotationVector2() * 25f;
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel, ModContent.ProjectileType<MutantSpearThrown>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.target, ai2: 1);
                    if (WorldSavingSystem.MasochistModeReal)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Normalize(vel), ModContent.ProjectileType<MutantDeathray2>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, -Vector2.Normalize(vel), ModContent.ProjectileType<MutantDeathray2>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer);
                    }
                }
                NPC.localAI[0] = 0;
            }

            if (NPC.ai[1] == attackTime - 90 + 1 && NPC.ai[2] < NPC.ai[3] && FargoSoulsUtil.HostCheck)
            {
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.SafeDirectionTo(player.Center + player.velocity * 30f), ModContent.ProjectileType<MutantDeathrayAim>(), 0, 0f, Main.myPlayer, 85f, NPC.whoAmI);
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpearAim>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI, 3);
            }
        }

        void PrepareSpearDashDirectP1()
        {
            if (Phase2Check())
                return;
            
            if (NPC.ai[3] == 0)
            {
                if (!AliveCheck(player))
                    return;
                NPC.ai[3] = 1;
                if (FargoSoulsUtil.HostCheck)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpearSpin>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI, 240, ai2: 1); // 250);
                    TelegraphSound = SoundEngine.PlaySound(FargosSoundRegistry.MutantUnpredictive with {Volume = 2f}, NPC.Center);
                }
                    

                EdgyBossText(GFBQuote(4));
            }

            if (NPC.ai[1] == 40)
            {
                if (FargoSoulsUtil.HostCheck)
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<GlowRing>(), 0, 0f, Main.myPlayer, NPC.whoAmI, -2);
            }

            if (NPC.ai[1] > 80)
            {
                ShouldMoveArena = false;
                NPC.velocity *= 0.8f;
                int freq = WorldSavingSystem.MasochistModeReal ? 4 : 5;
                if (NPC.ai[1] % freq == 0)
                {
                    SoundEngine.PlaySound(SoundID.Item12, NPC.Center);
                    //ai3 - 300 so that when attack ends, the projs will behave like at start of attack normally (straight streams)
                    float angle = NPC.ai[1] * MathHelper.Pi / 37f;

                    if (FargoSoulsUtil.HostCheck)
                    {
                        int max = WorldSavingSystem.MasochistModeReal ? 4 : 4;
                        float spd = 10f;
                        for (int i = 0; i < max; i++)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0f, -spd).RotatedBy(angle + MathHelper.TwoPi / max * i),
                                ModContent.ProjectileType<MutantEye>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer);
                        }
                    }
                }
            }
            else
            {
                Vector2 targetPos = player.Center;
                if (NPC.Top.Y < player.Bottom.Y)
                    targetPos.X += 600f * Math.Sign(NPC.Center.X - player.Center.X);
                targetPos.Y += 600f;
                Movement(targetPos, 0.9f, false);
            }

            if (++NPC.ai[1] > 240)
            {
                if (!AliveCheck(player))
                    return;
                AttackChoice++;
                NPC.ai[3] = 0;
                NPC.netUpdate = true;
            }

        }

        void SpearDashDirectP1()
        {
            if (Phase2Check())
                return;
            ShouldMoveArena = false;
            NPC.velocity *= 0.9f;

            if (NPC.ai[3] == 0)
                NPC.ai[3] = 30;

            if (++NPC.ai[1] > NPC.ai[3])
            {
                if (++NPC.ai[2] > 3)
                {

                    if (NPC.ai[1] < NPC.ai[3] + 30)
                    {
                        NPC.velocity /= 0.9f; // negate slowdown
                        Vector2 targetPos = player.Center + player.DirectionTo(NPC.Center) * 550;
                        Movement(targetPos, 0.6f, false);
                    }
                    else
                    {
                        ShouldMoveArena = true;
                        NPC.velocity *= 0.9f;
                    }
                    if (NPC.ai[1] > NPC.ai[3] + 40)
                        P1NextAttackOrMasoOptions(4); //go to next attack after dashes
                }
                else
                {
                    NPC.netUpdate = true;
                    AttackChoice++;
                    NPC.ai[1] = 0;
                    float speed = WorldSavingSystem.MasochistModeReal ? 45f : 30f;
                    NPC.velocity = speed * NPC.SafeDirectionTo(player.Center + player.velocity);
                    if (FargoSoulsUtil.HostCheck)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpearDash>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI);

                        if (WorldSavingSystem.MasochistModeReal)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Normalize(NPC.velocity), ModContent.ProjectileType<MutantDeathray2>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, -Vector2.Normalize(NPC.velocity), ModContent.ProjectileType<MutantDeathray2>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer);
                        }
                    }

                    EdgyBossText(GFBQuote(5));
                }
            }
        }

        void WhileDashingP1()
        {
            ShouldMoveArena = false;
            NPC.direction = NPC.spriteDirection = Math.Sign(NPC.velocity.X);
            if (++NPC.ai[1] > 30)
            {
                if (!AliveCheck(player))
                    return;
                NPC.netUpdate = true;
                AttackChoice--;
                NPC.ai[1] = 0;
            }
        }

        void ApproachForNextAttackP1()
        {
            if (!AliveCheck(player))
                return;
            if (Phase2Check())
                return;
            Vector2 targetPos = player.Center + player.SafeDirectionTo(NPC.Center) * 250;
            if (NPC.Distance(targetPos) > 50 && ++NPC.ai[2] < 180)
            {
                Movement(targetPos, 0.5f);
            }
            else
            {
                NPC.netUpdate = true;
                AttackChoice++;
                NPC.ai[1] = 0;
                NPC.ai[2] = player.SafeDirectionTo(NPC.Center).ToRotation();
                NPC.ai[3] = (float)Math.PI / 10f;
                if (player.Center.X < NPC.Center.X)
                    NPC.ai[3] *= -1;
            }
        }

        void VoidRaysP1()
        {
            if (Phase2Check())
                return;

            ref float timer = ref NPC.ai[1];
            ref float lockAngle = ref NPC.ai[2];
            ref float lockDirection = ref NPC.ai[3];

            if (lockDirection == 0)
                lockDirection = Main.rand.NextBool() ? 1 : -1;

            int bursts = 3;
            float burstTime = 56;
            float burstDelay = 50;

            float cycleTime = burstDelay + burstTime;

            float cycleTimer = timer % cycleTime;
            if (cycleTimer < burstDelay) // wait period
            {
                if (cycleTimer == burstDelay - 1) // prepare variables
                {
                    lockAngle = NPC.DirectionTo(player.Center).ToRotation();

                    NPC.netUpdate = true;
                    EdgyBossText(GFBQuote(6));
                }
                if (cycleTimer >= burstDelay - 5)
                {
                    NPC.velocity *= 0.8f;
                }
                else
                {
                    // movement
                    Vector2 offset = player.SafeDirectionTo(NPC.Center) * 600;
                    offset = offset.RotatedBy(MathHelper.PiOver2 * 0.05f * lockDirection);
                    Vector2 targetPos = player.Center + offset;
                    Movement(targetPos, 0.7f);
                }
            }
            else // void rays
            {
                NPC.velocity *= 0.8f;
                float progress = (cycleTimer - burstDelay) / burstTime;

                if (cycleTimer % 2 == 0)
                {
                    if (FargoSoulsUtil.HostCheck)
                    {
                        float increment = MathHelper.PiOver2 * 0.07f;
                        float i = (int)((cycleTimer - burstDelay) - (burstTime / 2));
                        float angle = NPC.DirectionTo(player.Center).ToRotation() + i * increment;
                        Vector2 dir = angle.ToRotationVector2();
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, dir * 2, ModContent.ProjectileType<MutantMark1>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, ai2: 1);
                    }
                }
            }

            if (timer >= cycleTime * bursts + burstDelay - 1) // extra end period
            {
                P1NextAttackOrMasoOptions(7);
            }
            timer++;
        }

        const int MUTANT_SWORD_SPACING = 80;
        const int MUTANT_SWORD_MAX = 12;

        void BoundaryBulletHellAndSwordP1()
        {
            if (NPC.localAI[2] == 0)
                NPC.localAI[2] = 1;
            switch ((int)NPC.localAI[2])
            {
                case 1:
                    PrepareMutantSword();
                    break;

                case 2:
                    MutantSword();
                    break;

                default:
                    break;
            }
        }

        void PrepareMutantSword()
        {
            //can alternate directions
            int sign = AttackChoice != 9 && NPC.localAI[2] % 2 == 1 ? -1 : 1;

            if (NPC.ai[2] == 0) //move onscreen so player can see
            {
                if (!AliveCheck(player))
                    return;

                Vector2 targetPos = player.Center;
                targetPos.X += 420 * Math.Sign(NPC.Center.X - player.Center.X);
                targetPos.Y -= 210 * sign;
                Movement(targetPos, 1.2f);

                if ((++NPC.localAI[0] > 30 || WorldSavingSystem.MasochistModeReal) && NPC.Distance(targetPos) < 64)
                {
                    NPC.velocity = Vector2.Zero;
                    NPC.netUpdate = true;

                    SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

                    NPC.localAI[1] = Math.Sign(player.Center.X - NPC.Center.X);
                    float startAngle = MathHelper.PiOver4 * -NPC.localAI[1];
                    NPC.ai[2] = startAngle * -4f / 20 * sign; //travel the full arc over number of ticks
                    if (sign < 0)
                        startAngle += MathHelper.PiOver2 * -NPC.localAI[1];

                    if (FargoSoulsUtil.HostCheck)
                    {
                        Vector2 offset = Vector2.UnitY.RotatedBy(startAngle) * -MUTANT_SWORD_SPACING;

                        void MakeSword(Vector2 pos, float spacing, float rotation = 0)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + pos, Vector2.Zero, ModContent.ProjectileType<MutantSword>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage, 4f / 3f), 0f, Main.myPlayer, NPC.whoAmI, spacing);
                        }

                        for (int i = 0; i < MUTANT_SWORD_MAX; i++)
                        {
                            MakeSword(offset * i, MUTANT_SWORD_SPACING * i);
                        }

                        for (int i = -1; i <= 1; i += 2)
                        {
                            MakeSword(offset.RotatedBy(MathHelper.ToRadians(26.5f * i)), 60 * 3);
                            MakeSword(offset.RotatedBy(MathHelper.ToRadians(40 * i)), 60 * 4f);
                        }
                    }

                    EdgyBossText(GFBQuote(8));
                }
            }
            else
            {
                NPC.velocity = Vector2.Zero;

                int endtime = 90;

                FancyFireballs((int)(NPC.ai[1] / endtime * 60f));

                if (++NPC.ai[1] > endtime)
                {
                    if (AttackChoice != 9)
                        AttackChoice++;

                    NPC.localAI[2]++; //progresses state in p1, counts swings in p2

                    Vector2 targetPos = player.Center;
                    targetPos.X -= 300 * NPC.ai[2];
                    NPC.velocity = (targetPos - NPC.Center) / 20;
                    NPC.ai[1] = 0;
                    NPC.netUpdate = true;
                }

                NPC.direction = NPC.spriteDirection = Math.Sign(NPC.localAI[1]);
            }
        }

        void MutantSword()
        {
            NPC.ai[3] += NPC.ai[2];
            NPC.direction = NPC.spriteDirection = Math.Sign(NPC.localAI[1]);

            if (NPC.ai[1] == 20)
            {
                if (!Main.dedServ && Main.LocalPlayer.active)
                    ScreenShakeSystem.StartShake(10, shakeStrengthDissipationIncrement: 10f / 30);

                //moon chain explosions
                int explosions = 5;
                if (AttackChoice != 9 || WorldSavingSystem.MasochistModeReal)
                    explosions = 8;
                if (explosions > 0)
                {
                    if (!Main.dedServ)
                        SoundEngine.PlaySound(SoundID.Thunder with { Pitch = -0.5f }, NPC.Center);

                    float lookSign = Math.Sign(NPC.localAI[1]);
                    float arcSign = Math.Sign(NPC.ai[2]);
                    Vector2 offset = lookSign * Vector2.UnitX.RotatedBy(MathHelper.PiOver4 * arcSign);

                    const float length = MUTANT_SWORD_SPACING * MUTANT_SWORD_MAX / 2f;
                    Vector2 spawnPos = NPC.Center + length * offset;
                    Vector2 baseDirection = player.DirectionFrom(spawnPos);

                    int max = explosions; //spread
                    baseDirection = baseDirection.RotatedBy(MathHelper.TwoPi * Main.rand.NextFloat(0.5f) / max); // offset
                    for (int i = 0; i < max; i++)
                    {
                        Vector2 angle = baseDirection.RotatedBy(MathHelper.TwoPi / max * i);
                        float ai1 = i <= 2 || i == max - 2 ? 48 : 24;
                        if (FargoSoulsUtil.HostCheck)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnPos + Main.rand.NextVector2Circular(NPC.width / 2, NPC.height / 2), Vector2.Zero, FargoSoulsUtil.AprilFools ? ModContent.ProjectileType<MoonLordSunBlast>() : ModContent.ProjectileType<MoonLordMoonBlast>(),
                                FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage, 4f / 3f), 0f, Main.myPlayer, MathHelper.WrapAngle(angle.ToRotation()), ai1);
                        }
                    }
                }
            }

            if (++NPC.ai[1] > 25)
            {
                if (AttackChoice == 9)
                {
                    P1NextAttackOrMasoOptions(AttackChoice);
                }
                else if (WorldSavingSystem.MasochistModeReal && NPC.localAI[2] < 3 * (endTimeVariance + 0.5))
                {
                    AttackChoice--;
                    NPC.ai[1] = 0;
                    NPC.ai[2] = 0;
                    NPC.ai[3] = 0;
                    NPC.localAI[1] = 0;
                    NPC.netUpdate = true;
                }
                else
                {
                    ChooseNextAttack(13, 21, 24, 28, 29, 31, 33, 37, 41, 42, 44, 11, 16);
                }
            }
        }

        #endregion

        #region p2

        void Phase2Transition()
        {
            NPC.velocity *= 0.9f;
            NPC.dontTakeDamage = true;

            if (NPC.buffType[0] != 0)
                NPC.DelBuff(0);

            EModeSpecialEffects();

            if (NPC.ai[2] == 0)
            {
                if (NPC.ai[1] < 60 && !Main.dedServ && Main.LocalPlayer.active)
                    FargoSoulsUtil.ScreenshakeRumble(6);
            }
            else
            {
                NPC.velocity = Vector2.Zero;
            }

            if (NPC.ai[1] < 240)
            {
                //make you stop attacking
                if (Main.LocalPlayer.active && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost && NPC.Distance(Main.LocalPlayer.Center) < 3000)
                {
                    Main.LocalPlayer.controlUseItem = false;
                    Main.LocalPlayer.controlUseTile = false;
                    Main.LocalPlayer.FargoSouls().NoUsingItems = 2;
                }
            }

            if (NPC.ai[1] == 0)
            {
                FargoSoulsUtil.ClearAllProjectiles(2, NPC.whoAmI);

                    DramaticTransition(false, NPC.ai[2] == 0);

                    if (FargoSoulsUtil.HostCheck)
                    {
                        ritualProj = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantRitual>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, 0f, NPC.whoAmI);

                        if (WorldSavingSystem.MasochistModeReal)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantRitual2>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantRitual3>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantRitual4>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                        }
                    }
            }
            else if (NPC.ai[1] == 150)
            {
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

                if (FargoSoulsUtil.HostCheck)
                {
                    //Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.GlowRingHollow>(), 0, 0f, Main.myPlayer, 5);
                    //Projectile.NewProjectile(npc.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.GlowRing>(), 0, 0f, Main.myPlayer, NPC.whoAmI, -22);
                }

                if (WorldSavingSystem.SkipMutantP1 <= 10)
                {
                    WorldSavingSystem.SkipMutantP1++;
                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.WorldData);
                }

                for (int i = 0; i < 50; i++)
                {
                    int d = Dust.NewDust(Main.LocalPlayer.position, Main.LocalPlayer.width, Main.LocalPlayer.height, FargoSoulsUtil.AprilFools ? DustID.SolarFlare : DustID.Vortex, 0f, 0f, 0, default, 2.5f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].noLight = true;
                    Main.dust[d].velocity *= 9f;
                }
                if (player.FargoSouls().TerrariaSoul)
                    EdgyBossText(GFBQuote(1));
            }
            else if (NPC.ai[1] > 150)
            {
                PhaseState = 3;
            }

            if (++NPC.ai[1] > 270)
            {
                NPC.life = NPC.lifeMax;
                AttackChoice = Main.rand.Next(new int[] { /*11, */13, 16, 19, 20, 21, 24, 26, 29, 31, 35, 37, 39, 42, 11}); //force a random choice
                NPC.ai[1] = 0;
                NPC.ai[2] = 0;
                NPC.ai[3] = 0;
                NPC.localAI[0] = 0;
                NPC.localAI[1] = 0;
                NPC.localAI[2] = 0;
                //NPC.TargetClosest();
                NPC.netUpdate = true;

                attackHistory.Enqueue(AttackChoice);
            }
        }

        void PrepareSpearDashPredictiveP2()
        {
            if (NPC.ai[3] == 0)
            {
                if (!AliveCheck(player))
                    return;
                NPC.ai[3] = 1;
                //NPC.velocity = NPC.DirectionFrom(player.Center) * NPC.velocity.Length();
                if (FargoSoulsUtil.HostCheck)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpearSpin>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI, 180); // + 60);
                    TelegraphSound = SoundEngine.PlaySound(FargosSoundRegistry.MutantPredictive with { Volume = 8f }, NPC.Center);
                }
                    
                EdgyBossText(GFBQuote(9));
            }

            if (++NPC.ai[1] > 180)
            {
                if (!AliveCheck(player))
                    return;
                NPC.netUpdate = true;
                AttackChoice++;
                NPC.ai[1] = 0;
                NPC.ai[3] = 0;
                //NPC.TargetClosest();
            }

            Vector2 targetPos = player.Center;
            targetPos.Y += 400f * Math.Sign(NPC.Center.Y - player.Center.Y); //can be above or below
            Movement(targetPos, 0.7f, false);
            if (NPC.Distance(player.Center) < 200)
                Movement(NPC.Center + NPC.DirectionFrom(player.Center), 1.4f);
        }

        void SpearDashPredictiveP2()
        {
            if (NPC.localAI[1] == 0) //max number of attacks
            {
                    NPC.localAI[1] = Main.rand.Next(WorldSavingSystem.MasochistModeReal ? 3 : 5, 9);
            }

            if (NPC.ai[1] == 0) //telegraph
            {
                if (!AliveCheck(player))
                    return;

                if (NPC.ai[2] == NPC.localAI[1] - 1)
                {
                    if (NPC.Distance(player.Center) > 450) //get closer for last dash
                    {
                        Movement(player.Center, 0.6f);
                        return;
                    }

                    NPC.velocity *= 0.75f; //try not to bump into player
                }

                if (NPC.ai[2] < NPC.localAI[1])
                {
                    if (FargoSoulsUtil.HostCheck)
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.SafeDirectionTo(player.Center + player.velocity * 30f), ModContent.ProjectileType<MutantDeathrayAim>(), 0, 0f, Main.myPlayer, 55, NPC.whoAmI);

                    if (NPC.ai[2] == NPC.localAI[1] - 1)
                    {
                        SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

                        if (FargoSoulsUtil.HostCheck)
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpearAim>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI, 4);
                    }
                }
            }

            NPC.velocity *= 0.9f;

            if (NPC.ai[1] < 55) //track player up until just before dash
            {
                NPC.localAI[0] = NPC.SafeDirectionTo(player.Center + player.velocity * 30f).ToRotation();
            }

            int endTime = 60;
            if (NPC.ai[2] == NPC.localAI[1] - 1)
                endTime = 80;
            if (WorldSavingSystem.MasochistModeReal && (NPC.ai[2] == 0 || NPC.ai[2] >= NPC.localAI[1]))
                endTime = 0;
            if (++NPC.ai[1] > endTime)
            {
                NPC.netUpdate = true;
                AttackChoice++;
                NPC.ai[1] = 0;
                NPC.ai[3] = 0;
                if (++NPC.ai[2] > NPC.localAI[1])
                {
                    ChooseNextAttack(16, 19, 20, 26, 28, 29, 31, 33, 39, 42, 44, 45);
                }
                else
                {
                    NPC.velocity = NPC.localAI[0].ToRotationVector2() * 45f;
                    float spearAi = 0f;
                    if (NPC.ai[2] == NPC.localAI[1])
                        spearAi = -2f;

                    if (FargoSoulsUtil.HostCheck)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Normalize(NPC.velocity), ModContent.ProjectileType<MutantDeathray2>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, -Vector2.Normalize(NPC.velocity), ModContent.ProjectileType<MutantDeathray2>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpearDash>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI, spearAi);
                    }

                    EdgyBossText(GFBQuote(10));
                }
                NPC.localAI[0] = 0;
            }
        }

        void WhileDashingP2()
        {
            NPC.direction = NPC.spriteDirection = Math.Sign(NPC.velocity.X);
            if (++NPC.ai[1] > 30)
            {
                if (!AliveCheck(player))
                    return;
                NPC.netUpdate = true;
                AttackChoice--;
                NPC.ai[1] = 0;

                //quickly bounce back towards player
                if (AttackChoice == 14 && NPC.ai[2] == NPC.localAI[1] - 1 && NPC.Distance(player.Center) > 450)
                    NPC.velocity = NPC.SafeDirectionTo(player.Center) * 16f;
            }
        }
        
        void BoundaryBulletHellP2()
        {
            int endTime = 300 + 60 + (int)(360 * endTimeVariance);

            NPC.velocity = Vector2.Zero;
            if (NPC.localAI[0] == 0)
            {
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

                NPC.localAI[0] = Math.Sign(NPC.Center.X - player.Center.X);
                //if (WorldSavingSystem.MasochistMode) NPC.ai[2] = NPC.SafeDirectionTo(player.Center).ToRotation(); //starting rotation offset to avoid hitting at close range
                if (FargoSoulsUtil.HostCheck)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<GlowRing>(), 0, 0f, Main.myPlayer, NPC.whoAmI, -2);
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpearSpin>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI, endTime, 2);
                }

                EdgyBossText(GFBQuote(11));

                if (WorldSavingSystem.MasochistModeReal)
                    NPC.ai[2] = Main.rand.NextFloat(MathHelper.Pi);
            }

            if (NPC.ai[3] > 60 && ++NPC.ai[1] > 2)
            {
                SoundEngine.PlaySound(SoundID.Item12, NPC.Center);
                NPC.ai[1] = 0;
                NPC.ai[2] += (float)Math.PI / 8 / 480 * NPC.ai[3] * NPC.localAI[0];
                if (NPC.ai[2] > (float)Math.PI)
                    NPC.ai[2] -= (float)Math.PI * 2;
                if (FargoSoulsUtil.HostCheck)
                {
                    int max = 5;
                    if (WorldSavingSystem.MasochistModeReal)
                        max += 2;
                    for (int i = 0; i < max; i++)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0f, -6f).RotatedBy(NPC.ai[2] + Math.PI * 2 / max * i),
                            ModContent.ProjectileType<MutantEye>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer);
                    }
                }
            }

            if (++NPC.ai[3] > endTime)
            {
                ChooseNextAttack(13, 19, 20, 21, 24, WorldSavingSystem.MasochistModeReal ? 31 : 26, 33, 41, 44);
            }
        }

        void PillarDunk()
        {
            if (!AliveCheck(player))
            {
                return;
            }
            int pillarAttackDelay = 60;
            if (NPC.ai[2] == 0f && NPC.ai[3] == 0f)
            {
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                if (Main.netMode != 1)
                {
                    Clone(-1f, 1f, pillarAttackDelay * 4);
                    Clone(1f, -1f, pillarAttackDelay * 2);
                    Clone(1f, 1f, pillarAttackDelay * 3);
                    Clone(1f, 1f, pillarAttackDelay * 6);
                }
                NPC.netUpdate = true;
                NPC.ai[2] = NPC.Center.X;
                NPC.ai[3] = NPC.Center.Y;
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<MutantRitual>() && Main.projectile[i].ai[1] == (float)NPC.whoAmI)
                    {
                        NPC.ai[2] = Main.projectile[i].Center.X;
                        NPC.ai[3] = Main.projectile[i].Center.Y;
                        break;
                    }
                }
                Vector2 offset = 1000f * Vector2.UnitX.RotatedBy(MathHelper.ToRadians(45f));
                if (Main.rand.NextBool())
                {
                    if (player.Center.X > NPC.ai[2])
                    {
                        offset.X *= -1f;
                    }
                    if (Main.rand.NextBool())
                    {
                        offset.Y *= -1f;
                    }
                }
                else
                {
                    if (Main.rand.NextBool())
                    {
                        offset.X *= -1f;
                    }
                    if (player.Center.Y > NPC.ai[3])
                    {
                        offset.Y *= -1f;
                    }
                }
                NPC.localAI[1] = NPC.ai[2];
                NPC.localAI[2] = NPC.ai[3];
                NPC.ai[2] = offset.Length();
                NPC.ai[3] = offset.ToRotation();
            }
            Vector2 targetPos = player.Center;
            targetPos.X += ((NPC.Center.X < player.Center.X) ? (-700) : 700);
            targetPos.Y += ((NPC.ai[1] < 240f) ? 400 : 150);
            if (NPC.Distance(targetPos) > 50f)
            {
                Movement(targetPos, 1f);
            }
            int endTime = 240 + pillarAttackDelay * 4 + 60;
            endTime += pillarAttackDelay * 2;
            NPC.localAI[0] = (float)endTime - NPC.ai[1];
            NPC.localAI[0] += 60f + 60f * (1f - NPC.ai[1] / (float)endTime);
            if (NPC.ai[1] == 95f || NPC.ai[1] == 135f || NPC.ai[1] == (float)(endTime - 30))
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 dir = player.Center - NPC.Center;
                    float ai1New = (Main.rand.NextBool() ? 1 : (-1));
                    Vector2 vel = Vector2.Normalize(dir.RotatedByRandom(Math.PI / 4.0)) * 38f;
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel, ModContent.ProjectileType<HostileLightning>(), 30, 0f, Main.myPlayer, dir.ToRotation(), ai1New);
                }
            }
            if ((NPC.ai[1] += 1f) > (float)endTime)
            {
                ChooseNextAttack(11, 13, 20, 21, 26, 33, 41, 44, 47, 49);
            }
            else if (NPC.ai[1] == (float)pillarAttackDelay)
            {
                if (Main.netMode != 1)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitY * -5f, ModContent.ProjectileType<MutantPillar>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.damage, 1.3333334f), 0f, Main.myPlayer, 3f, (float)NPC.whoAmI);
                }
            }
            else if (NPC.ai[1] == (float)(pillarAttackDelay * 5) && Main.netMode != 1)
            {
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitY * -5f, ModContent.ProjectileType<MutantPillar>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.damage, 1.3333334f), 0f, Main.myPlayer, 1f, (float)NPC.whoAmI);
            }
            void Clone(float ai1, float ai2, float ai3)
            {
                FargoSoulsUtil.NewNPCEasy(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<MutantIllusion>(), NPC.whoAmI, NPC.whoAmI, ai1, ai2, ai3);
            }
        }

        void EOCStarSickles()
        {
            if (!AliveCheck(player))
            {
                return;
            }
            if (NPC.ai[1] == 0f)
            {
                if (WorldSavingSystem.MasochistModeReal)
                {
                    player.AddBuff(BuffID.Confused, 60 * 10);
                }
                float ai1 = 30f;
                NPC.ai[1] = 30f;
                if (Main.netMode != 1)
                {
                    int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, -Vector2.UnitY, ModContent.ProjectileType<MutantEyeOfCthulhu>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.damage), 0f, Main.myPlayer, (float)NPC.target, ai1);
                    if (p != 1000)
                    {
                        Main.projectile[p].timeLeft -= 30;
                    }
                }
            }
            if (NPC.ai[1] < 120f)
            {
                NPC.ai[2] = player.Center.X;
                NPC.ai[3] = player.Center.Y;
            }
            if (NPC.ai[1] == 120f || NPC.ai[1] == 156f)
            {
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                Vector2 offset = NPC.Center - player.Center;
                Vector2 spawnPos = player.Center;
                switch (Main.rand.Next(4))
                {
                    case 0:
                        LaserSpread(new Vector2(spawnPos.X + offset.X, spawnPos.Y + offset.Y));
                        LaserSpread(new Vector2(spawnPos.X + offset.X, spawnPos.Y - offset.Y));
                        TelegraphConfusion(new Vector2(spawnPos.X + offset.X, spawnPos.Y - offset.Y));
                        TelegraphConfusion(new Vector2(spawnPos.X + offset.X, spawnPos.Y + offset.Y));
                        break;
                    case 1:
                        LaserSpread(new Vector2(spawnPos.X + offset.X, spawnPos.Y - offset.Y));
                        TelegraphConfusion(new Vector2(spawnPos.X + offset.X, spawnPos.Y - offset.Y));
                        LaserSpread(new Vector2(spawnPos.X - offset.X, spawnPos.Y + offset.Y));
                        TelegraphConfusion(new Vector2(spawnPos.X - offset.X, spawnPos.Y + offset.Y));
                        break;
                    case 2:
                        LaserSpread(new Vector2(spawnPos.X - offset.X, spawnPos.Y + offset.Y));
                        TelegraphConfusion(new Vector2(spawnPos.X - offset.X, spawnPos.Y + offset.Y));
                        LaserSpread(new Vector2(spawnPos.X - offset.X, spawnPos.Y - offset.Y));
                        TelegraphConfusion(new Vector2(spawnPos.X - offset.X, spawnPos.Y - offset.Y));
                        break;
                    case 3:
                        LaserSpread(new Vector2(spawnPos.X - offset.X, spawnPos.Y - offset.Y));
                        TelegraphConfusion(new Vector2(spawnPos.X - offset.X, spawnPos.Y - offset.Y));
                        LaserSpread(new Vector2(spawnPos.X + offset.X, spawnPos.Y + offset.Y));
                        TelegraphConfusion(new Vector2(spawnPos.X + offset.X, spawnPos.Y + offset.Y));
                        break;
                }
            }
            Vector2 targetPos = new Vector2(NPC.ai[2], NPC.ai[3]);
            targetPos += NPC.DirectionFrom(targetPos).RotatedBy(MathHelper.ToRadians(-5f)) * 450f;
            if (NPC.Distance(targetPos) > 50f)
            {
                Movement(targetPos, 0.25f);
            }
            if ((NPC.ai[1] += 1f) > 450f)
            {
                player.DelBuff(BuffID.Confused);
                ChooseNextAttack(11, 13, 16, 21, 26, 29, 31, 33, 35, 37, 41, 44, 45, 47, 49);
            }
            void LaserSpread(Vector2 spawn)
            {
                int max = 3;
                int degree = 1;
                int laserDamage = FargoSoulsUtil.ScaledProjectileDamage(NPC.damage, 1.3333334f);
                Projectile.NewProjectile(NPC.GetSource_FromThis(), spawn, new Vector2(0f, -4f), ModContent.ProjectileType<BrainofConfusion>(), 0, 0f, Main.myPlayer, 0f, 0f);
                for (int i = -max; i <= max; i++)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), spawn, 0.2f * player.DirectionFrom(spawn).RotatedBy(MathHelper.ToRadians(degree) * (float)i), ModContent.ProjectileType<DestroyerLaser>(), laserDamage, 0f, Main.myPlayer, 0f, 0f);
                }
            }
            void TelegraphConfusion(Vector2 spawn)
            {
                Projectile.NewProjectile(NPC.GetSource_FromThis(), spawn, Vector2.Zero, ModContent.ProjectileType<GlowRingHollow>(), 0, 0f, Main.myPlayer, 8f, 180f);
                Projectile.NewProjectile(NPC.GetSource_FromThis(), spawn, Vector2.Zero, ModContent.ProjectileType<GlowRingHollow>(), 0, 0f, Main.myPlayer, 8f, 200f);
                Projectile.NewProjectile(NPC.GetSource_FromThis(), spawn, Vector2.Zero, ModContent.ProjectileType<GlowRingHollow>(), 0, 0f, Main.myPlayer, 8f, 220f);
            }
        }

        void PrepareSpearDashDirectP2()
        {
            if (NPC.ai[3] == 0)
            {
                if (!AliveCheck(player))
                    return;
                NPC.ai[3] = 1;
                if (WorldSavingSystem.MasochistModeReal)
                    NPC.localAI[0] = Main.rand.Next(2);
                
                if (FargoSoulsUtil.HostCheck)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpearSpin>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI, 180);// + (WorldSavingSystem.MasochistMode ? 10 : 20));
                    TelegraphSound = SoundEngine.PlaySound(FargosSoundRegistry.MutantUnpredictive with { Volume = 2f }, NPC.Center);
                }
                    
                EdgyBossText(GFBQuote(14));
            }

            if (++NPC.ai[1] > 180)
            {
                if (!AliveCheck(player))
                    return;
                NPC.netUpdate = true;
                AttackChoice++;
                NPC.ai[1] = 0;
                NPC.ai[3] = 0;
                //NPC.TargetClosest();
            }

            Vector2 targetPos = player.Center;
            if (NPC.localAI[0] == 0)
            {
                targetPos.Y += 450f * Math.Sign(NPC.Center.Y - player.Center.Y); //can be above or below
            }
            else
            {
                targetPos.X += 600 * Math.Sign(NPC.Center.X - player.Center.X); //can be left or right
                targetPos.Y += 100f * Math.Sign(NPC.Center.Y - player.Center.Y); //always do it starting from a bit diagonal to make fall dodge easier
            }
            Movement(targetPos, 0.7f, false);
            if (NPC.Distance(player.Center) < 200)
                Movement(NPC.Center + NPC.DirectionFrom(player.Center), 1.4f);
        }

        void SpearDashDirectP2()
        {
            NPC.velocity *= 0.9f;

            if (NPC.localAI[1] == 0) //max number of attacks
            {
                    NPC.localAI[1] = Main.rand.Next(3, 9);
                    if (WorldSavingSystem.MasochistModeReal)
                    {
                        if (NPC.localAI[0] == 0)
                            NPC.localAI[1] = Main.rand.Next(5, 9);
                        else
                            NPC.localAI[1] = Main.rand.Next(4, 7);
                    }
                else
                {
                    NPC.localAI[1] = 5;
                }
            }

            if (++NPC.ai[1] > 5)
            {
                NPC.netUpdate = true;
                AttackChoice++;
                NPC.ai[1] = 0;
                if (++NPC.ai[2] > NPC.localAI[1])
                {
                    if (WorldSavingSystem.MasochistModeReal)
                        ChooseNextAttack(11, 13, 19, 20, 28, 31, 33, 35, 39, 42, 44);
                    else
                        ChooseNextAttack(11, 26, 28, 29, 31, 35, 37, 39, 42, 44);
                }
                else
                {
                    NPC.velocity = NPC.SafeDirectionTo(player.Center) * (WorldSavingSystem.MasochistModeReal ? 60f : 45f);
                    if (FargoSoulsUtil.HostCheck)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Normalize(NPC.velocity), ModContent.ProjectileType<MutantDeathray2>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage, 0.8f), 0f, Main.myPlayer);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, -Vector2.Normalize(NPC.velocity), ModContent.ProjectileType<MutantDeathray2>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage, 0.8f), 0f, Main.myPlayer);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpearDash>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI);
                    }
                }

                EdgyBossText(GFBQuote(15));
            }
        }

        void SpawnDestroyersForPredictiveThrow()
        {
            if (!AliveCheck(player))
                return;

                Vector2 targetPos = player.Center + NPC.DirectionFrom(player.Center) * 500;
                if (Math.Abs(targetPos.X - player.Center.X) < 150) //avoid crossing up player
                {
                    targetPos.X = player.Center.X + 150 * Math.Sign(targetPos.X - player.Center.X);
                    Movement(targetPos, 0.3f);
                }
                if (NPC.Distance(targetPos) > 50)
                {
                    Movement(targetPos, 0.9f);
                }

            if (NPC.localAI[1] == 0) //max number of attacks
            {
                NPC.localAI[1] = Main.rand.Next(WorldSavingSystem.MasochistModeReal ? 3 : 5, 9);

                NPC.localAI[2] = Main.rand.Next(2);

                EdgyBossText(GFBQuote(16));
            }

            if (++NPC.ai[1] > 60)
            {
                NPC.netUpdate = true;
                NPC.ai[1] = 30;
                int cap = 5;
                if (WorldSavingSystem.MasochistModeReal)
                {
                    cap += 2;
                    NPC.ai[1] += 15; //faster
                }

                if (++NPC.ai[2] > cap)
                {
                    //NPC.TargetClosest();
                    AttackChoice++;
                    NPC.ai[1] = 0;
                    NPC.ai[2] = 0;
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.NPCDeath13, NPC.Center);
                    if (FargoSoulsUtil.HostCheck) //spawn worm
                    {
                        Vector2 vel = NPC.DirectionFrom(player.Center).RotatedByRandom(MathHelper.ToRadians(120)) * 10f;
                        float ai1 = 0.8f + 0.4f * NPC.ai[2] / 5f;
                        if (WorldSavingSystem.MasochistModeReal)
                            ai1 += 0.4f;
                        float appearance = NPC.localAI[2];
                        if (FargoSoulsUtil.AprilFools)
                            appearance = 0;
                        int current = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel, ModContent.ProjectileType<MutantDestroyerHead>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.target, ai1, appearance);
                        //timeleft: remaining duration of this case + duration of next case + extra delay after + successive death
                        Main.projectile[current].timeLeft = 30 * (cap - (int)NPC.ai[2]) + 60 * (int)NPC.localAI[1] + 30 + (int)NPC.ai[2] * 6;
                        int max = Main.rand.Next(8, 19);
                        for (int i = 0; i < max; i++)
                            current = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel, ModContent.ProjectileType<MutantDestroyerBody>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, Main.projectile[current].identity, 0f, appearance);
                        int previous = current;
                        current = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel, ModContent.ProjectileType<MutantDestroyerTail>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, Main.projectile[current].identity, 0f, appearance);
                        Main.projectile[previous].localAI[1] = Main.projectile[current].identity;
                        Main.projectile[previous].netUpdate = true;
                    }
                }
            }
        }

        void SpearTossPredictiveP2()
        {
            if (!AliveCheck(player))
            {
                return;
            }
            Vector2 targetPos = player.Center;
            targetPos.X += 500 * ((!(NPC.Center.X < targetPos.X)) ? 1 : (-1));
            if (NPC.Distance(targetPos) > 25f)
            {
                Movement(targetPos, 0.8f);
            }
            if ((NPC.ai[1] += 1f) > 60f)
            {
                NPC.netUpdate = true;
                NPC.ai[1] = 0f;
                bool shouldAttack = true;
                if ((NPC.ai[2] += 1f) > NPC.localAI[1])
                {
                    shouldAttack = false;
                    if (WorldSavingSystem.MasochistModeReal)
                    {
                        ChooseNextAttack(11, 19, 20, 29, 31, 33, 35, 37, 39, 42, 44, 45);
                    }
                    else
                    {
                        ChooseNextAttack(11, 19, 20, 26, 26, 26, 29, 31, 33, 35, 37, 39, 42, 44);
                    }
                }
                if ((shouldAttack || WorldSavingSystem.MasochistModeReal) && Main.netMode != 1)
                {
                    Vector2 vel = NPC.DirectionTo(player.Center + player.velocity * 30f) * 30f;
                    for (int i = -1; i <= 1; i++)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Normalize(vel).RotatedBy((float)i * 0.12f), ModContent.ProjectileType<MutantDeathray2>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.damage, 0.8f), 0f, Main.myPlayer, 0f, 0f);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, -Vector2.Normalize(vel).RotatedBy((float)i * 0.12f), ModContent.ProjectileType<MutantDeathray2>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.damage, 0.8f), 0f, Main.myPlayer, 0f, 0f);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel.RotatedBy((float)i * 0.12f), ModContent.ProjectileType<MutantSpearThrown>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.damage), 0f, Main.myPlayer, (float)NPC.target, 1f);
                    }
                }
            }
            else if (NPC.ai[1] == 1f && (NPC.ai[2] < NPC.localAI[1] || WorldSavingSystem.MasochistModeReal) && Main.netMode != 1)
            {
                for (int i = -1; i <= 1; i++)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(player.Center + player.velocity * 30f).RotatedBy((float)i * 0.12f), ModContent.ProjectileType<MutantDeathrayAim>(), 0, 0f, Main.myPlayer, 60f, (float)NPC.whoAmI);
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpearAim>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.damage), 0f, Main.myPlayer, (float)NPC.whoAmI, 2f);
                }
            }
        }

        void PrepareMechRayFan()
        {
            if (NPC.ai[1] == 0)
            {
                if (!AliveCheck(player))
                    return;

                if (WorldSavingSystem.MasochistModeReal)
                    NPC.ai[1] = 31; //skip the pause, skip the telegraph
            }

            if (NPC.ai[1] == 30)
            {
                SoundEngine.PlaySound(SoundID.ForceRoarPitched, NPC.Center); //eoc roar
                if (FargoSoulsUtil.HostCheck)
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<GlowRing>(), 0, 0f, Main.myPlayer, NPC.whoAmI, NPCID.Retinazer);

                EdgyBossText(GFBQuote(17));
            }

            Vector2 targetPos;
            if (NPC.ai[1] < 30)
            {
                targetPos = player.Center + NPC.DirectionFrom(player.Center).RotatedBy(MathHelper.ToRadians(15)) * 500f;
                if (NPC.Distance(targetPos) > 50)
                    Movement(targetPos, 0.3f);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    int d = Dust.NewDust(NPC.Center, 0, 0, DustID.Torch, 0f, 0f, 0, default, 3f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].noLight = true;
                    Main.dust[d].velocity *= 12f;
                }

                targetPos = player.Center;
                targetPos.X += 600 * (NPC.Center.X < targetPos.X ? -1 : 1);
                Movement(targetPos, 1.2f, false);
            }

            if (++NPC.ai[1] > 150 || WorldSavingSystem.MasochistModeReal && NPC.Distance(targetPos) < 64)
            {
                NPC.netUpdate = true;
                AttackChoice++;
                NPC.ai[1] = 0;
                NPC.ai[2] = 0;
                NPC.ai[3] = 0;
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                //NPC.TargetClosest();
            }
        }

        void MechRayFan()
        {
            NPC.velocity = Vector2.Zero;
            if (NPC.ai[2] == 0f)
            {
                NPC.ai[2] = ((!Main.rand.NextBool()) ? 1 : (-1));
            }
            if (NPC.ai[3] == 0f && Main.netMode != 1)
            {
                int max = 7;
                for (int i = 0; i <= max; i++)
                {
                    Vector2 dir = Vector2.UnitX.RotatedBy(NPC.ai[2] * (float)i * (float)Math.PI / (float)max) * 6f;
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + dir, Vector2.Zero, ModContent.ProjectileType<MutantGlowything>(), 0, 0f, Main.myPlayer, dir.ToRotation(), (float)NPC.whoAmI);
                }
            }
            int endTime = 390;
            int timeBeforeAttackEnds;
            if (NPC.ai[3] > (float)(45) && NPC.ai[3] < 240f && (NPC.ai[1] += 1f) > 10f)
            {
                NPC.ai[1] = 0f;
                if (Main.netMode != 1)
                {
                    float rotation = MathHelper.ToRadians(245f) * NPC.ai[2] / 80f;
                    timeBeforeAttackEnds = endTime - (int)NPC.ai[3];
                    SpawnRay(NPC.Center, 8f * NPC.ai[2], rotation);
                    SpawnRay(NPC.Center, -8f * NPC.ai[2] + 180f, 0f - rotation);
                    Vector2 spawnPos = NPC.Center + NPC.ai[2] * -1200f * Vector2.UnitY;
                    SpawnRay(spawnPos, 8f * NPC.ai[2] + 180f, rotation);
                    SpawnRay(spawnPos, -8f * NPC.ai[2], 0f - rotation);
                }
            }
            if (NPC.ai[3] > 210f && Main.netMode != 1)
            {
                float spawnOffset = (float)((!Main.rand.NextBool()) ? 1 : (-1)) * Main.rand.NextFloat(1400f, 1800f);
                float maxVariance = MathHelper.ToRadians(16f);
                Vector2 aimPoint = NPC.Center - Vector2.UnitY * NPC.ai[2] * 600f;
                Vector2 spawnPos = aimPoint + spawnOffset * Vector2.UnitX.RotatedByRandom(maxVariance).RotatedBy(MathHelper.ToRadians(0f));
                Vector2 vel = 32f * Vector2.Normalize(aimPoint - spawnPos);
                Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnPos, vel, ModContent.ProjectileType<MutantGuardian>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.damage, 1.3333334f), 0f, Main.myPlayer, 0f, 0f);
            }
            if (NPC.ai[3] < 180f && (NPC.localAI[0] += 1f) > 1f)
            {
                NPC.localAI[0] = 0f;
                SpawnPrime(15f, 0f);
            }
            if ((NPC.ai[3] += 1f) > (float)endTime)
            {
                ChooseNextAttack(11, 13, 16, 19, 21, 24, 29, 31, 33, 35, 37, 39, 41, 42, 45, 47, 49);
                NPC.netUpdate = true;
            }
            void SpawnPrime(float varianceInDegrees, float rotationInDegrees)
            {
                SoundEngine.PlaySound(SoundID.Item21, NPC.Center);
                if (Main.netMode != 1)
                {
                    float spawnOffset = (float)((!Main.rand.NextBool()) ? 1 : (-1)) * Main.rand.NextFloat(1400f, 1800f);
                    float maxVariance = MathHelper.ToRadians(varianceInDegrees);
                    Vector2 aimPoint = NPC.Center - Vector2.UnitY * NPC.ai[2] * 600f;
                    Vector2 spawnPos = aimPoint + spawnOffset * Vector2.UnitY.RotatedByRandom(maxVariance).RotatedBy(MathHelper.ToRadians(rotationInDegrees));
                    Vector2 vel = 32f * Vector2.Normalize(aimPoint - spawnPos);
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnPos, vel, ModContent.ProjectileType<MutantGuardian>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.damage, 1.3333334f), 0f, Main.myPlayer, 0f, 0f);
                }
            }
            void SpawnRay(Vector2 pos, float angleInDegrees, float turnRotation)
            {
                int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), pos, MathHelper.ToRadians(angleInDegrees).ToRotationVector2(), ModContent.ProjectileType<FargowiltasSouls.Content.Bosses.MutantBoss.MutantDeathray3>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.damage), 0f, Main.myPlayer, turnRotation, (float)NPC.whoAmI);
                if (p != 1000 && Main.projectile[p].timeLeft > timeBeforeAttackEnds)
                {
                    Main.projectile[p].timeLeft = timeBeforeAttackEnds;
                }
            }
        }

        //what if mutant was betrayed and trapped in the hyperbolic flesh chamber...
        void RoomOfFlesh()
        {
            NPC.velocity = Vector2.Zero;

            const int chainTimeToTravel = 45;
            const int wofTimeToTravel = 75;

            float waitTime = chainTimeToTravel + wofTimeToTravel + 360;
            if (WorldSavingSystem.MasochistModeReal)
                waitTime += 300 * endTimeVariance;

            float xWallStopOffset = WorldSavingSystem.MasochistModeReal ? 450 : 500;

            NPC.ai[3] = xWallStopOffset; //for wof eyes to know
            
            const float xWallSpawnOffset = 2200f;
            const float yWallEyeOffset = 600;

            if (NPC.ai[1] == 0)
            {
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

                if (FargoSoulsUtil.HostCheck)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<GlowRing>(), 0, 0f, Main.myPlayer, NPC.whoAmI, NPCID.WallofFleshEye);
                    for (int j = -1; j <= 1; j += 2)
                    {
                        for (int i = -2; i <= 2; i++)
                        {
                            if (i == 0)
                                continue;

                            const float offsetForWallWidth = 120 - 32;
                            float xDistToTravel = (xWallSpawnOffset - offsetForWallWidth) * j;
                            float yDistToTravel = yWallEyeOffset * (i - 0.5f * Math.Sign(i));
                            Vector2 vel = new Vector2(xDistToTravel, yDistToTravel) / chainTimeToTravel;
                            float wofVelX = (xWallStopOffset - xWallSpawnOffset) / wofTimeToTravel * j;
                            int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel, ModContent.ProjectileType<MutantChain>(), 1, 0f, Main.myPlayer, chainTimeToTravel, wofVelX, wofTimeToTravel);
                            if (p != Main.maxProjectiles)
                                Main.projectile[p].timeLeft = (int)waitTime;
                        }
                    }
                }
            }

            if (NPC.ai[1] % 25 == 1 && NPC.ai[1] < chainTimeToTravel + wofTimeToTravel)
            {
                if (!Main.dedServ)
                    SoundEngine.PlaySound(new SoundStyle("FargowiltasSouls/Assets/Sounds/VanillaEternity/Golem/GolemBeam"), NPC.Center);
            }
            
            if (NPC.ai[1] == chainTimeToTravel)
            {
                if (!Main.dedServ)
                    SoundEngine.PlaySound(SoundID.NPCDeath10, NPC.Center);

                if (FargoSoulsUtil.HostCheck)
                {
                    for (int j = -1; j <= 1; j += 2)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            for (int k = -3; k <= 3; k++)
                            {
                                Vector2 spawnPos = NPC.Center;
                                spawnPos.X += xWallSpawnOffset * j;
                                spawnPos.X += j * i * 140 * 2; //account for width & scale
                                spawnPos.Y += 420 * k * 2;
                                float wofVelX = (xWallStopOffset - xWallSpawnOffset) / wofTimeToTravel * j;
                                int wallValue = i + Math.Abs(k); //only 0s will have eyes and mouth
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnPos, wofVelX * Vector2.UnitX, ModContent.ProjectileType<MutantWallOfFlesh>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0, Main.myPlayer, wallValue, wofTimeToTravel, waitTime - chainTimeToTravel);
                            }
                        }
                    }
                }
            }

            if (NPC.ai[1] == chainTimeToTravel + wofTimeToTravel)
            {
                if (!Main.dedServ && Main.LocalPlayer.active)
                    ScreenShakeSystem.StartShake(10, shakeStrengthDissipationIncrement: 10f / 30);
            }

            float trueWaitTime = waitTime;
            //to reduce the overlap when in maso ftw. doesnt fully negate it, leaves a bit of overlap
            if (WorldSavingSystem.MasochistModeReal && Main.getGoodWorld)
            {
                trueWaitTime *= 1.25f;
                trueWaitTime -= 60 * 1.25f;
            }
            if (++NPC.ai[1] > trueWaitTime)
            {
                if (!Main.dedServ && Main.LocalPlayer.active)
                {
                    SoundEngine.PlaySound(SoundID.NPCDeath10, NPC.Center);
                    ScreenShakeSystem.StartShake(10, shakeStrengthDissipationIncrement: 10f / 30);
                }

                ChooseNextAttack(11, 13, 19, 21, 24, 33, 41, 42, 44, 45);
            }
        }

        void PrepareFishron1()
        {
            if (!AliveCheck(player))
                return;
            Vector2 targetPos = new(player.Center.X, player.Center.Y + 600 * Math.Sign(NPC.Center.Y - player.Center.Y));
            Movement(targetPos, 1.4f, false);

            if (NPC.ai[1] == 0) //always dash towards same side i started on
                NPC.ai[2] = Math.Sign(NPC.Center.X - player.Center.X);

            if (++NPC.ai[1] > 60 || NPC.Distance(targetPos) < 64) //dive here
            {
                NPC.velocity.X = 30f * NPC.ai[2];
                NPC.velocity.Y = 0f;
                AttackChoice++;
                NPC.ai[1] = 0;
                NPC.netUpdate = true;

                EdgyBossText(GFBQuote(18));
            }
        }
        
        void SpawnFishrons()
        {
            NPC.velocity *= 0.97f;
            if (AttackChoice == 30) // fishron variant
            {
                if (NPC.ai[1] == 0)
                {
                    NPC.ai[2] = Main.rand.NextBool() ? 1 : 0;
                }
                const int fishronDelay = 3;
                int maxFishronSets = WorldSavingSystem.MasochistModeReal ? 3 : 2;
                if (NPC.ai[1] % fishronDelay == 0 && NPC.ai[1] <= fishronDelay * maxFishronSets)
                {
                    if (FargoSoulsUtil.HostCheck)
                    {
                        int projType = ModContent.ProjectileType<MutantFishron>();
                        int max = (int)NPC.ai[1] / fishronDelay;

                        // Оригинальные направления (сверху/снизу или слева/справа)
                        for (int j = -1; j <= 1; j += 2) //to both sides of player
                        {
                            for (int i = -max; i <= max; i++) //fan of fishron
                            {
                                if (Math.Abs(i) != max) //only spawn the outmost ones
                                    continue;
                                float spread = MathHelper.Pi / 3 / (maxFishronSets + 1);
                                Vector2 offset = NPC.ai[2] == 0 ?
                                    Vector2.UnitY.RotatedBy(spread * i) * -450f * j :
                                    Vector2.UnitX.RotatedBy(spread * i) * 475f * j;
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, projType, FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, offset.X, offset.Y);
                            }
                        }

                        // ex fishrons
                        for (int j = -1; j <= 1; j += 2) //to both sides of player
                        {
                            for (int i = -max; i <= max; i++) //fan of fishron
                            {
                                if (Math.Abs(i) != max) //only spawn the outmost ones
                                    continue;
                                float spread = MathHelper.Pi / 3 / (maxFishronSets + 1);
                                Vector2 offset = NPC.ai[2] == 0 ?
                                    Vector2.UnitX.RotatedBy(spread * i) * 475f * j : 
                                    Vector2.UnitY.RotatedBy(spread * i) * -450f * j; 
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, projType, FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, offset.X, offset.Y, 2);
                            }
                        }
                    }
                    for (int i = 0; i < 30; i++)
                    {
                        int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.IceTorch, 0f, 0f, 0, default, 3f);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].noLight = true;
                        Main.dust[d].velocity *= 12f;
                    }
                }
            }
            else // shadow hand variant
            {
                if (NPC.ai[1] == 0)
                {
                    NPC.ai[2] = Main.rand.NextFromList(0, 1, 2, 3);
                    SoundEngine.PlaySound(SoundID.DD2_GhastlyGlaiveImpactGhost, player.Center);
                }
                const int handDelay = 4;
                float hands = WorldSavingSystem.MasochistModeReal ? 8 : 8;
                if (NPC.ai[1] <= hands * handDelay && NPC.ai[1] % handDelay == 0)
                {
                    if (FargoSoulsUtil.HostCheck)
                    {
                        int projType = ModContent.ProjectileType<MutantShadowHand>();
                        for (int j = -1; j <= 1; j += 2)
                        {
                            float gap = MathHelper.TwoPi * 0.25f;
                            float baseAngle = MathHelper.PiOver4 + j * gap / 2;
                            baseAngle += MathHelper.PiOver2 * NPC.ai[2];
                            float coverage = MathHelper.Pi - gap;
                            float angle = baseAngle + j * coverage * NPC.ai[1] / (hands * handDelay);
                            Vector2 offset = angle.ToRotationVector2() * 550f;
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, projType, FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, offset.X, offset.Y);
                        }
                    }
                    for (int i = 0; i < 30; i++)
                    {
                        int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.IceTorch, 0f, 0f, 0, default, 3f);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].noLight = true;
                        Main.dust[d].velocity *= 12f;
                    }
                }

            }


            if (++NPC.ai[1] > (WorldSavingSystem.MasochistModeReal ? 60 : 120))
            {
                ChooseNextAttack(13, 19, 20, 21, WorldSavingSystem.MasochistModeReal ? 44 : 26, 28, 33, 35, 39, 41, 42, 44, 11, 16);
            }
        }
        
        void PrepareTrueEyeDiveP2()
        {
            if (!AliveCheck(player))
                return;
            ChooseNextAttack(13, 19, 21, 24, 33, 33, 33, 39, 41, 44);
        }
        
        void TrueEyeDive()
        {
            ChooseNextAttack(13, 19, 21, 24, 33, 33, 33, 39, 41, 44);
        }
        
        void PrepareNuke()
        {
            if (!AliveCheck(player))
                return;
            Vector2 targetPos = player.Center;
            targetPos.X += 400 * (NPC.Center.X < targetPos.X ? -1 : 1);
            targetPos.Y -= 400;
            Movement(targetPos, 1.2f, false);
            if (++NPC.ai[1] > 60)
            {
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                if (FargoSoulsUtil.HostCheck)
                {
                    float gravity = 0.2f;
                    float time = WorldSavingSystem.MasochistModeReal ? 120f : 180f;
                    Vector2 distance = player.Center - NPC.Center;
                    distance.X /= time;
                    distance.Y = distance.Y / time - 0.5f * gravity * time;
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, distance, ModContent.ProjectileType<MutantNuke>(), WorldSavingSystem.MasochistModeReal ? FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage, 4f / 3f) : 0, 0f, Main.myPlayer, gravity);
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantFishronRitual>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage, 4f / 3f), 0f, Main.myPlayer, NPC.whoAmI);
                }
                AttackChoice++;
                NPC.ai[1] = 0;

                if (Math.Sign(player.Center.X - NPC.Center.X) == Math.Sign(NPC.velocity.X))
                    NPC.velocity.X *= -1f;
                if (NPC.velocity.Y < 0)
                    NPC.velocity.Y *= -1f;
                NPC.velocity.Normalize();
                NPC.velocity *= 3f;

                NPC.netUpdate = true;

                EdgyBossText(GFBQuote(19));
                //NPC.TargetClosest();
            }
        }

        void Nuke()
        {
            if (!AliveCheck(player))
                return;

            Vector2 target = NPC.Bottom.Y < player.Top.Y
                ? player.Center + 300f * Vector2.UnitX * Math.Sign(NPC.Center.X - player.Center.X)
                : NPC.Center + 30 * NPC.DirectionFrom(player.Center).RotatedBy(MathHelper.ToRadians(60) * Math.Sign(player.Center.X - NPC.Center.X));
            Movement(target, 0.1f);
            int maxSpeed = WorldSavingSystem.MasochistModeReal ? 3 : 2;
            if (NPC.velocity.Length() > maxSpeed)
                NPC.velocity = Vector2.Normalize(NPC.velocity) * maxSpeed;

            if (NPC.ai[1] > (WorldSavingSystem.MasochistModeReal ? 120 : 180))
            {
                if (!Main.dedServ && Main.LocalPlayer.active)
                    FargoSoulsUtil.ScreenshakeRumble(6);

                if (FargoSoulsUtil.HostCheck)
                {
                    Vector2 safeZone = NPC.Center;
                    safeZone.Y -= 100;
                    const float safeRange = 150 + 200;
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 spawnPos = NPC.Center + Main.rand.NextVector2Circular(1200, 1200);
                        if (Vector2.Distance(safeZone, spawnPos) < safeRange)
                        {
                            Vector2 directionOut = spawnPos - safeZone;
                            directionOut.Normalize();
                            spawnPos = safeZone + directionOut * Main.rand.NextFloat(safeRange, 1200);
                        }
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnPos, Vector2.Zero, ModContent.ProjectileType<MutantNukeBomb>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage, 4f / 3f), 0f, Main.myPlayer);
                    }
                }
            }

            if (++NPC.ai[1] > 360 + 210 * endTimeVariance)
            {
                ChooseNextAttack(11, 13, 16, 19, 24, WorldSavingSystem.MasochistModeReal ? 26 : 29, 28, 35, 37, 39, 41, 42);
            }

            if (NPC.ai[1] > 45)
            {
                for (int i = 0; i < 20; i++)
                {
                    Vector2 offset = new();
                    offset.Y -= 100;
                    double angle = Main.rand.NextDouble() * 2d * Math.PI;
                    offset.X += (float)(Math.Sin(angle) * 150);
                    offset.Y += (float)(Math.Cos(angle) * 150);
                    Dust dust = Main.dust[Dust.NewDust(NPC.Center + offset - new Vector2(4, 4), 0, 0, FargoSoulsUtil.AprilFools ? DustID.SolarFlare : DustID.Vortex, 0, 0, 100, Color.White, 1.5f)];
                    dust.velocity = NPC.velocity;
                    if (Main.rand.NextBool(3))
                        dust.velocity += Vector2.Normalize(offset) * 5f;
                    dust.noGravity = true;
                }
            }
        }

        void PrepareSlimeRain()
        {
            if (!AliveCheck(player))
                return;
            Vector2 targetPos = player.Center;
            targetPos.X += 700 * (NPC.Center.X < targetPos.X ? -1 : 1);
            targetPos.Y += 200;
            if (AttackChoice == 11)
                targetPos.Y -= 300;
            Movement(targetPos, 2f);

            if (++NPC.ai[2] > 30 || WorldSavingSystem.MasochistModeReal && NPC.Distance(targetPos) < 64)
            {
                AttackChoice++;
                NPC.ai[1] = 0;
                NPC.ai[2] = 0;
                NPC.ai[3] = 0;
                NPC.netUpdate = true;
                //NPC.TargetClosest();

                EdgyBossText(GFBQuote(20));
            }
        }

        void SlimeRain()
        {
            if (NPC.ai[3] == 0)
            {
                NPC.ai[3] = 1;
                //Main.NewText(NPC.position.Y);
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                if (FargoSoulsUtil.HostCheck)
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSlimeRain>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI);
            }

            if (NPC.ai[1] == 0) //telegraphs for where slime will fall
            {
                bool first = NPC.localAI[0] == 0;
                NPC.localAI[0] = Main.rand.Next(5, 9) * 120;
                if (first) //always start on the same side as the player
                {
                    if (player.Center.X < NPC.Center.X && NPC.localAI[0] > 1200)
                        NPC.localAI[0] -= 1200;
                    else if (player.Center.X > NPC.Center.X && NPC.localAI[0] < 1200)
                        NPC.localAI[0] += 1200;
                }
                else //after that, always be on opposite side from player
                {
                    if (player.Center.X < NPC.Center.X && NPC.localAI[0] < 1200)
                        NPC.localAI[0] += 1200 - 120; //slight reduction to bias it closer to player
                    else if (player.Center.X > NPC.Center.X && NPC.localAI[0] > 1200)
                        NPC.localAI[0] -= 1200 - 120;
                }
                NPC.localAI[0] += 60;

                Vector2 basePos = NPC.Center;
                basePos.X -= 1200;
                for (int i = -360; i <= 2760; i += 120) //spawn telegraphs
                {
                    if (FargoSoulsUtil.HostCheck)
                    {
                        if (i + 60 == (int)NPC.localAI[0])
                            continue;
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), basePos.X + i + 60, basePos.Y, 0f, 0f, ModContent.ProjectileType<MutantReticle>(), 0, 0f, Main.myPlayer);
                    }
                }

                    NPC.ai[1] += 20; //less startup
                    NPC.ai[2] += 20; //stay synced
            }

            if (NPC.ai[1] > 120 && NPC.ai[1] % 5 == 0) //rain down slime balls
            {
                SoundEngine.PlaySound(SoundID.Item34, player.Center);
                if (FargoSoulsUtil.HostCheck)
                {
                    void Slime(Vector2 pos, float off, Vector2 vel)
                    {
                        //dont flip in maso wave 3
                        int flip = NPC.ai[2] < 180 * 2 && Main.rand.NextBool() ? -1 : 1;
                        Vector2 spawnPos = pos + off * Vector2.UnitY * flip;
                        float ai0 = FargoSoulsUtil.ProjectileExists(ritualProj, ModContent.ProjectileType<MutantRitual>()) == null ? 0f : NPC.Distance(Main.projectile[ritualProj].Center);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnPos, vel * flip * 2 /* x2 to compensate for removed extraUpdates */, ModContent.ProjectileType<MutantSlimeBall>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, ai0);
                    }

                    Vector2 basePos = NPC.Center;
                    basePos.X -= 1200;
                    float yOffset = -1300;

                    const float safeRange = 110;
                    for (int i = -360; i <= 2760; i += 75)
                    {
                        float xOffset = i + Main.rand.Next(75);
                        if (Math.Abs(xOffset - NPC.localAI[0]) < safeRange) //dont fall over safespot
                            continue;

                        Vector2 spawnPos = basePos;
                        spawnPos.X += xOffset;
                        Vector2 velocity = Vector2.UnitY * Main.rand.NextFloat(15f, 20f);

                        Slime(spawnPos, yOffset, velocity);
                    }

                    //spawn right on safespot borders
                    Slime(basePos + Vector2.UnitX * (NPC.localAI[0] + safeRange), yOffset, Vector2.UnitY * 20f);
                    Slime(basePos + Vector2.UnitX * (NPC.localAI[0] - safeRange), yOffset, Vector2.UnitY * 20f);
                }
            }
            if (++NPC.ai[1] > 180)
            {
                if (!AliveCheck(player))
                    return;
                NPC.ai[1] = 0;
            }

            const int masoMovingRainAttackTime = 180 * 3 - 60;
            if (NPC.ai[1] == 120 && NPC.ai[2] < masoMovingRainAttackTime && Main.rand.NextBool(3))
                NPC.ai[2] = masoMovingRainAttackTime;

            NPC.velocity = Vector2.Zero;

            const int timeToMove = 240;
            if (NPC.ai[2] == masoMovingRainAttackTime)
            {
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

                EdgyBossText(GFBQuote(21));
            }


            if (NPC.ai[2] > masoMovingRainAttackTime + 30)
            {
                if (NPC.ai[1] > 170) //let the balls keep falling
                    NPC.ai[1] -= 30;

                if (NPC.localAI[1] == 0) //direction to move safespot towards
                {
                    float safespotX = NPC.Center.X - 1200f + NPC.localAI[0];
                    NPC.localAI[1] = Math.Sign(NPC.Center.X - safespotX);
                }

                //move the safespot
                //NPC.localAI[0] += 1000f / timeToMove * NPC.localAI[1];

                NPC.Center += Vector2.UnitX * 1000f / timeToMove * NPC.localAI[1]; //move along with the movement
            }

            int endTime = 180 * 3;
            endTime += timeToMove + (int)(300 * endTimeVariance) - 30;
            if (++NPC.ai[2] > endTime)
            {
                ChooseNextAttack(19, 20, 26, 28, 33, 37, 39, 41, 42, 45);
            }

            if (NPC.ai[2] > 510f)
            {
                if (NPC.ai[2] % 3f == 1f && NPC.ai[2] < (float)(endTime - 80))
                {
                    Vector2 range = player.Center + new Vector2(((float)Main.rand.Next(2) - 0.5f) * 2200f, 0f);
                    Vector2 vel = (player.Center - range).SafeNormalize(Vector2.Zero) * (15f + ((float)Main.rand.Next(2) - 0.5f) * 4f);
                    Projectile projectile = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), range + new Vector2(0f, ((float)Main.rand.Next(2) - 0.5f) * 12f), vel, ModContent.ProjectileType<MutantBigSting22>(), 500, 0f, 0, 256f, 0f);
                    projectile.hostile = true;
                    projectile.friendly = false;
                }
                if (NPC.ai[1] > 170f)
                {
                    NPC.ai[1] -= 30f;
                }
                if (NPC.localAI[1] == 0f)
                {
                    float safespotX = NPC.Center.X - 1200f + NPC.localAI[0];
                    NPC.localAI[1] = Math.Sign(NPC.Center.X - safespotX);
                }
                NPC.localAI[0] += 4.1666665f * NPC.localAI[1];
            }
        }

        void QueenSlimeRain()
        {
            if (NPC.ai[3] == 0)
            {
                NPC.ai[3] = 1;
                //Main.NewText(NPC.position.Y);
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                if (FargoSoulsUtil.HostCheck)
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSlimeRain>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI);
            }

            float safeRange = /*WorldSavingSystem.MasochistModeReal ? 192.5f :*/ 220;
            float spacing = safeRange;

            if (NPC.ai[1] == 0) //telegraphs for where slime will fall
            {
                NPC.localAI[0] = Main.rand.Next(6, 9) * 120;
                //always start on the same side as the player
                if (player.Center.X > NPC.Center.X)
                    NPC.localAI[0] += 600;
                NPC.localAI[0] += 60;

                Vector2 basePos = NPC.Center;
                int j = 0;
                for (float i = -1540; i <= 1540; i += spacing / 2) //spawn telegraphs
                {
                    j++;
                    if (WorldSavingSystem.MasochistModeReal
                        ? Math.Abs(i) > spacing * 1f && Math.Abs(i) < spacing * 2f
                        : Math.Abs(i) < spacing * 1.5f && j % 2 == 0)
                        continue;
                    if (FargoSoulsUtil.HostCheck)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), basePos.X + i, basePos.Y, 0f, 0f, ModContent.ProjectileType<MutantReticle>(), 0, 0f, Main.myPlayer, ai2: 1);
                    }
                }
            }

            const int masoMovingRainAttackTime = 60;
            int timeToMove = WorldSavingSystem.MasochistModeReal ? 420 : 360;
            int endTime = masoMovingRainAttackTime + timeToMove + (int)(100 * endTimeVariance);

            if (NPC.ai[1] > masoMovingRainAttackTime && NPC.ai[1] % 6 == 0) //rain down slime balls
            {
                SoundEngine.PlaySound(SoundID.Item34, player.Center);
                if (FargoSoulsUtil.HostCheck)
                {
                    //int frame = Main.rand.Next(3);

                    void Slime(Vector2 pos, float off, Vector2 vel)
                    {
                        const int flip = 1;
                        Vector2 spawnPos = pos + off * Vector2.UnitY * flip;
                        float ai0 = FargoSoulsUtil.ProjectileExists(ritualProj, ModContent.ProjectileType<MutantRitual>()) == null ? 0f : NPC.Distance(Main.projectile[ritualProj].Center);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnPos, vel * flip, ModContent.ProjectileType<MutantSlimeSpike>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, ai0);
                    }

                    Vector2 basePos = NPC.Center;
                    float lerper = Utilities.InverseLerp(masoMovingRainAttackTime + 30, endTime, NPC.ai[1]);
                    lerper = Utilities.Saturate(lerper);
                    float angle = MathHelper.TwoPi * lerper * 1.12f;
                    Vector2 xDir = Vector2.UnitX.RotatedBy(angle);
                    Vector2 yDir = Vector2.UnitY.RotatedBy(angle);
                    for (float i = -1540; i <= 1540; i += spacing)
                    {
                        Vector2 pos = basePos - yDir * 1300 + xDir * i;
                        Vector2 vel = yDir * 20;
                        Slime(pos, 0f, yDir * 20f);
                    }
                    if (WorldSavingSystem.MasochistModeReal)
                    {
                        for (float i = -spacing / 2; i <= spacing / 2; i += spacing)
                        {
                            Vector2 pos = basePos - yDir * 1300 + xDir * i;
                            Vector2 vel = yDir * 20;
                            Slime(pos, 0f, yDir * 20f);
                        }
                    }
                }
            }

            NPC.velocity = Vector2.Zero;

            if (NPC.ai[1] == masoMovingRainAttackTime)
            {
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

                EdgyBossText(GFBQuote(21));
            }

            if (NPC.ai[1] > masoMovingRainAttackTime && --NPC.ai[2] < 0)
            {
                float safespotMoveSpeed = WorldSavingSystem.MasochistModeReal ? 6f : 4f;

                if (--NPC.localAI[2] < 0) //reset and recalibrate for the other direction
                {
                    float safespotX = NPC.Center.X - 1200f + NPC.localAI[0];
                    NPC.localAI[1] = Math.Sign(NPC.Center.X - safespotX); //direction to move safespot towards

                    float farSideArenaBorder = NPC.Center.X + 1200f * NPC.localAI[1];
                    float distanceToBorder = Math.Abs(farSideArenaBorder - safespotX);
                    float minRequiredDistance = Math.Abs(NPC.Center.X - safespotX) + 100;

                    float distanceToTravel = MathHelper.Lerp(minRequiredDistance, distanceToBorder, Main.rand.NextFloat(0.6f));

                    NPC.localAI[2] = distanceToTravel / safespotMoveSpeed;
                    NPC.ai[2] = WorldSavingSystem.MasochistModeReal ? 15 : 30; //adds a pause when turning around
                }

                //move the safespot
                NPC.localAI[0] += safespotMoveSpeed * NPC.localAI[1];
            }


            if (++NPC.ai[1] > endTime)
            {
                ChooseNextAttack(16, 19, 20, 28, 29, 31, 33, 37, 39, 41, 42, 45);
            }
        }

        void PrepareFishron2()
        {
            if (!AliveCheck(player))
                return;

            Vector2 targetPos = player.Center;
            targetPos.X += 400 * (NPC.Center.X < targetPos.X ? -1 : 1);
            targetPos.Y -= 400;
            Movement(targetPos, 0.9f);

            if (++NPC.ai[1] > 60 || WorldSavingSystem.MasochistModeReal && NPC.Distance(targetPos) < 32) //dive here
            {
                NPC.velocity.X = 35f * (NPC.position.X < player.position.X ? 1 : -1);
                NPC.velocity.Y = 10f;
                AttackChoice++;
                NPC.ai[1] = 0;
                NPC.netUpdate = true;
                //NPC.TargetClosest();

                EdgyBossText(GFBQuote(18));
            }
        }

        void PrepareMyBallsP2()
        {
            if (!AliveCheck(player))
                return;
            Vector2 targetPos = player.Center + player.SafeDirectionTo(NPC.Center) * 450;
            if (++NPC.ai[1] < 180 && NPC.Distance(targetPos) > 50)
            {
                Movement(targetPos, 0.8f);
            }
            else
            {
                NPC.netUpdate = true;
                AttackChoice++;
                NPC.ai[1] = 0;
                NPC.ai[2] = 0;
                NPC.ai[3] = 0;
            }
        }

        void MyBallsP2()
        {
            NPC.velocity = Vector2.Zero;

            int endTime = 420 + (int)(300 * endTimeVariance);
            int attackEndTime = endTime - 60;

            if (NPC.ai[2] == 0)
            {
                NPC.ai[2] = Main.rand.NextBool() ? -1 : 1;
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                if (FargoSoulsUtil.HostCheck)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<GlowRing>(), 0, 0f, Main.myPlayer, NPC.whoAmI, -2);
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpearSpin>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI, attackEndTime, 2);
                }
                EdgyBossText(GFBQuote(22));
            }

            const int startupDelay = 60;
            if (WorldSavingSystem.MasochistModeReal) //helix
            {
                int maxTime = WorldSavingSystem.MasochistModeReal ? 6 : 10;
                if (++NPC.ai[1] > maxTime)
                {
                    NPC.ai[1] = 0;
                    const int max = 12;
                    float speed = WorldSavingSystem.MasochistModeReal ? 7f : 5f;

                    for (int j = -1; j <= 1; j += 2)
                    {
                        for (int i = 0; i < max; i++)
                        {
                            float totalDegreesToRotate = WorldSavingSystem.MasochistModeReal ? 120 : 90;
                            float rotation = MathHelper.ToRadians(totalDegreesToRotate) / 300 * NPC.ai[2];
                            float spawnRotation = rotation * NPC.ai[3];
                            Vector2 vel = speed * spawnRotation.ToRotationVector2().RotatedBy(MathHelper.TwoPi / max * i);
                            if (NPC.ai[3] > startupDelay && NPC.ai[3] < attackEndTime)
                            {
                                SoundEngine.PlaySound(SoundID.Item84, NPC.Center);
                                if (FargoSoulsUtil.HostCheck)
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel, ModContent.ProjectileType<MutantSphereHelix>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI, j, rotation);
                            }
                            if (NPC.ai[3] < startupDelay * 2 && !Main.getGoodWorld)
                            {
                                if (FargoSoulsUtil.HostCheck)
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel, ModContent.ProjectileType<MutantFancyMovementTell>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, rotation, NPC.whoAmI, j);
                            }
                        }
                    }
                }
            }
            else //okuu
            {
                if (++NPC.ai[1] > 9 && NPC.ai[3] > startupDelay && NPC.ai[3] < attackEndTime)
                {
                    NPC.ai[1] = 0;
                    float rotationPerTick = MathHelper.ToRadians(90) / 240 * NPC.ai[2];
                    float rotation = rotationPerTick * (NPC.ai[3] - 45);
                    int max = WorldSavingSystem.MasochistModeReal ? 11 : 10;
                    float speed = WorldSavingSystem.MasochistModeReal ? 11f : 10f;
                    SpawnSphereRing(max, speed, FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), -1f, rotation, alt: rotationPerTick);
                    SpawnSphereRing(max, speed, FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 1f, rotation, alt: rotationPerTick);
                }
            }

            //Main.NewText(Main.projectile.Count(p => p.active));

            if (++NPC.ai[3] > endTime)
            {
                ChooseNextAttack(13, 19, 20, WorldSavingSystem.MasochistModeReal ? 13 : 26, WorldSavingSystem.MasochistModeReal ? 44 : 33, 41, 44, 16);
            }

            for (int i = 0; i < 5; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, FargoSoulsUtil.AprilFools ? DustID.SolarFlare : DustID.Vortex, 0f, 0f, 0, default, 1.5f);
                Main.dust[d].noGravity = true;
                Main.dust[d].noLight = true;
                Main.dust[d].velocity *= 4f;
            }
        }

        void SpawnSpearTossDirectP2Attack()
        {
            if (FargoSoulsUtil.HostCheck)
            {
                Vector2 vel = NPC.SafeDirectionTo(player.Center) * 30f;
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Normalize(vel), ModContent.ProjectileType<MutantDeathray2>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage, 0.8f), 0f, Main.myPlayer);
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, -Vector2.Normalize(vel), ModContent.ProjectileType<MutantDeathray2>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage, 0.8f), 0f, Main.myPlayer);

                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel, ModContent.ProjectileType<MutantSpearThrown>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.target);
            }

            EdgyBossText(RandomObnoxiousQuote());
        }

        void SpearTossDirectP2()
        {
            if (!AliveCheck(player))
            {
                return;
            }
            if (NPC.ai[1] == 0f)
            {
                NPC.localAI[0] = MathHelper.WrapAngle((NPC.Center - player.Center).ToRotation());
                NPC.localAI[1] = Main.rand.Next(3, 9);
                NPC.localAI[1] += 3f;
                NPC.localAI[2] = ((!Main.rand.NextBool()) ? 1 : (-1));
                NPC.netUpdate = true;
            }
            Vector2 targetPos = player.Center + 500f * Vector2.UnitX.RotatedBy((float)Math.PI / 150f * NPC.ai[3] * NPC.localAI[2] + NPC.localAI[0]);
            if (NPC.Distance(targetPos) > 25f)
            {
                Movement(targetPos, 0.6f);
            }
            NPC.ai[3] += 1f;
            if ((NPC.ai[1] += 1f) > 180f)
            {
                NPC.netUpdate = true;
                NPC.ai[1] = 150f;
                bool shouldAttack = true;
                if ((NPC.ai[2] += 1f) > NPC.localAI[1])
                {
                    int[] obj = new int[11]
                    {
                    11, 16, 19, 20, 0, 31, 33, 35, 42, 44,
                    45
                    };
                    obj[4] = 44;
                    ChooseNextAttack(obj);
                    shouldAttack = false;
                }
                Attack();
            }
            else if (NPC.ai[1] == 160f)
            {
                Attack();
            }
            else if (NPC.ai[1] == 165f)
            {
                Attack();
            }
            else if (NPC.ai[1] == 170f)
            {
                Attack();
            }
            else if (NPC.ai[1] == 175f)
            {
                Attack();
            }
            else if (NPC.ai[1] == 151f)
            {
                if (Main.netMode != 1)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpearAim>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.damage), 0f, Main.myPlayer, (float)NPC.whoAmI, 1f);
                }
            }
            else if (NPC.ai[1] == 1f && Main.netMode != 1)
            {
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpearAim>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.damage), 0f, Main.myPlayer, (float)NPC.whoAmI, -1f);
            }
            void Attack()
            {
                if (Main.netMode != 1)
                {
                    Vector2 vel = NPC.DirectionTo(player.Center) * 30f;
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Normalize(vel), ModContent.ProjectileType<MutantDeathray2>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.damage, 0.8f), 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, -Vector2.Normalize(vel), ModContent.ProjectileType<MutantDeathray2>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.damage, 0.8f), 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel, ModContent.ProjectileType<MutantSpearThrown>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.damage), 0f, Main.myPlayer, (float)NPC.target, 0f);
                }
            }
        
            //if (!AliveCheck(player))
            //    return;

            //if (NPC.ai[1] == 0)
            //{
            //    NPC.localAI[0] = MathHelper.WrapAngle((NPC.Center - player.Center).ToRotation()); //remember initial angle offset

            //    //random max number of attacks
            //        NPC.localAI[1] = Main.rand.Next(WorldSavingSystem.MasochistModeReal ? 3 : 5, 9);

            //    if (WorldSavingSystem.MasochistModeReal)
            //    {
            //        NPC.localAI[1] += Main.rand.Next(6);
            //        if (Main.getGoodWorld)
            //            NPC.localAI[1] += 5;
            //    }
            //    NPC.localAI[2] = Main.rand.NextBool() ? -1 : 1; //pick a random rotation direction
            //    NPC.netUpdate = true;
            //}

            ////slowly rotate in full circle around player
            //Vector2 targetPos = player.Center + 500f * Vector2.UnitX.RotatedBy(MathHelper.TwoPi / 300 * NPC.ai[3] * NPC.localAI[2] + NPC.localAI[0]);
            //if (NPC.Distance(targetPos) > 25)
            //    Movement(targetPos, 0.6f);

            //++NPC.ai[3]; //for keeping track of how much time has actually passed (ai1 jumps around)

            //if (++NPC.ai[1] > 180)
            //{
            //    NPC.netUpdate = true;
            //    NPC.ai[1] = 150;

            //    bool shouldAttack = true;
            //    if (++NPC.ai[2] > NPC.localAI[1])
            //    {
            //        if (Main.getGoodWorld) // Can't combo into slime rain in ftw
            //            ChooseNextAttack(19, 20, WorldSavingSystem.MasochistModeReal ? 44 : 26, 28, 31, 33, 42, 44, 45, 11);
            //        else
            //            ChooseNextAttack(19, 20, WorldSavingSystem.MasochistModeReal ? 44 : 26, 28, 31, 33, 35, 42, 44, 45, 11);
            //        shouldAttack = false;
            //    }

            //    if (shouldAttack || WorldSavingSystem.MasochistModeReal)
            //    {
            //        SpawnSpearTossDirectP2Attack();
            //    }
            //}
            //else if (WorldSavingSystem.MasochistModeReal && NPC.ai[1] == 165)
            //{
            //    SpawnSpearTossDirectP2Attack();
            //}
            //else if (NPC.ai[1] == 151)
            //{
            //    if (NPC.ai[2] > 0 && (NPC.ai[2] < NPC.localAI[1] || WorldSavingSystem.MasochistModeReal) && FargoSoulsUtil.HostCheck)
            //        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpearAim>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI, 1);
            //}
            //else if (NPC.ai[1] == 1)
            //{
            //    if (FargoSoulsUtil.HostCheck)
            //        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpearAim>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI, -1);
            //}
        }

        void PrepareTwinRangsAndCrystals()
        {
            if (!AliveCheck(player))
                return;
            Vector2 targetPos = player.Center;
            targetPos.X += 600 * (NPC.Center.X < targetPos.X ? -1 : 1);
            if (NPC.Distance(targetPos) > 20)
                Movement(targetPos, 1.0f);
            if (++NPC.ai[1] > 45)
            {
                NPC.netUpdate = true;
                AttackChoice++;
                NPC.ai[1] = 0;
                NPC.ai[2] = 0;
                NPC.ai[3] = 0;
                //NPC.TargetClosest();

                EdgyBossText(GFBQuote(23));
            }
        }

        void TwinRangsAndCrystals()
        {
            NPC.velocity = Vector2.Zero;

            ref float timer = ref NPC.ai[3];

            if (timer == 1)
            {
                SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
                SoundEngine.PlaySound(Plantera.VineGrowth with { Volume = 3 }, NPC.Center);
                int areas = 6;
                if (FargoSoulsUtil.HostCheck)
                {
                    for (int i = 0; i <= areas; i++)
                    {
                        Vector2 dir = NPC.DirectionTo(player.Center).RotatedBy(MathF.Tau * i / areas);

                        for (int j = -1; j <= 1; j += 2)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + dir, dir * 12,
                                ModContent.ProjectileType<MutantSpikevine>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, ai1: NPC.whoAmI, ai2: j);
                            if (!Main.getGoodWorld)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + dir, dir * 12,
                                    ModContent.ProjectileType<MutantFancyMovementTell>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, ai1: NPC.whoAmI, ai2: j);
                            }
                        }
                    }
                }
            }

            int twinFrequency = 90;

            if (timer % twinFrequency == twinFrequency - 10 && timer < 450 - twinFrequency)
            {
                SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
                if (FargoSoulsUtil.HostCheck)
                {
                    int rand = Main.rand.NextBool() ? 1 : -1;
                    for (int i = -1; i <= 1; i += 2)
                    {
                        int spaz = i == rand ? 1 : 0;
                        Vector2 dir = NPC.DirectionTo(player.Center).RotatedBy(i * MathHelper.PiOver2);
                        int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, -dir, ModContent.ProjectileType<MutantTwin>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.target, dir.ToRotation(), spaz);
                    }
                }
            }

            if (++timer > 450)
            {
                ChooseNextAttack(13, 16, 21, 24, 26, 28, 29, 33, 35, 39, 41, 44, 45, 11);
            }
        }

        void EmpressSwordWave()
        {
            if (!AliveCheck(player))
                return;

            //if (!WorldSavingSystem.EternityMode)
            //{
            //    AttackChoice++; //dont do this attack in expert
            //    return;
            //}

            //Vector2 targetPos = player.Center + 360 * NPC.DirectionFrom(player.Center).RotatedBy(MathHelper.ToRadians(10)); Movement(targetPos, 0.25f);
            NPC.velocity = Vector2.Zero;

            int attackThreshold = WorldSavingSystem.MasochistModeReal ? 48 : 60;
            int timesToAttack = 4 + (int)Math.Round(3 * endTimeVariance);
            int startup = 90;

            if (NPC.ai[1] == 0)
            {
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                NPC.ai[3] = Main.rand.NextFloat(MathHelper.TwoPi);

                EdgyBossText(GFBQuote(24));
            }

            void Sword(Vector2 pos, float ai0, float ai1, Vector2 vel)
            {
                if (FargoSoulsUtil.HostCheck)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), pos - vel * 60f, vel,
                        ProjectileID.FairyQueenLance, FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, ai0, ai1);
                }
            }

            if (NPC.ai[1] >= startup && NPC.ai[1] < startup + attackThreshold * timesToAttack && --NPC.ai[2] < 0) //walls of swords
            {
                NPC.ai[2] = attackThreshold;

                SoundEngine.PlaySound(SoundID.Item163, player.Center);

                if (Math.Abs(MathHelper.WrapAngle(NPC.DirectionFrom(player.Center).ToRotation() - NPC.ai[3])) > MathHelper.PiOver2)
                    NPC.ai[3] += MathHelper.Pi; //swords always spawn closer to player

                const int maxHorizSpread = 1600 * 2;
                const int arenaRadius = 1200;
                int max = WorldSavingSystem.MasochistModeReal ? 16 : 12;
                float gap = maxHorizSpread / max;

                float attackAngle = NPC.ai[3];// + Main.rand.NextFloat(MathHelper.ToDegrees(10)) * (Main.rand.NextBool() ? -1 : 1);
                Vector2 spawnOffset = -attackAngle.ToRotationVector2();

                //start by focusing on player
                Vector2 focusPoint = player.Center;

                //move focus point along grid closer so attack stays centered
                Vector2 home = NPC.Center;// FargoSoulsUtil.ProjectileExists(ritualProj, ModContent.ProjectileType<MutantRitual>()) == null ? NPC.Center : Main.projectile[ritualProj].Center;
                for (float i = 0; i < arenaRadius; i += gap)
                {
                    Vector2 newFocusPoint = focusPoint + gap * attackAngle.ToRotationVector2();
                    if ((home - newFocusPoint).Length() > (home - focusPoint).Length())
                        break;
                    focusPoint = newFocusPoint;
                }

                //doing it this way to guarantee it always remains aligned to grid
                float spawnDistance = 0;
                while (spawnDistance < arenaRadius)
                    spawnDistance += gap;

                float mirrorLength = 2f * (float)Math.Sqrt(2f * spawnDistance * spawnDistance);
                int swordCounter = 0;
                for (int i = -max; i <= max; i++)
                {
                    Vector2 spawnPos = focusPoint + spawnOffset * spawnDistance + spawnOffset.RotatedBy(MathHelper.PiOver2) * gap * i;
                    float Ai1 = swordCounter++ / (max * 2f + 1);

                    Vector2 randomOffset = Main.rand.NextVector2Unit();
                    if (WorldSavingSystem.MasochistModeReal)
                    {
                        if (randomOffset.Length() < 0.5f)
                            randomOffset = 0.5f * randomOffset.SafeNormalize(Vector2.UnitX);
                        randomOffset *= 2f;
                    }

                    Sword(spawnPos, attackAngle + MathHelper.PiOver4, Ai1, randomOffset);
                    Sword(spawnPos, attackAngle - MathHelper.PiOver4, Ai1, randomOffset);

                    if (WorldSavingSystem.MasochistModeReal)
                    {
                        Sword(spawnPos + mirrorLength * (attackAngle + MathHelper.PiOver4).ToRotationVector2(), attackAngle + MathHelper.PiOver4 + MathHelper.Pi, Ai1, randomOffset);
                        Sword(spawnPos + mirrorLength * (attackAngle - MathHelper.PiOver4).ToRotationVector2(), attackAngle - MathHelper.PiOver4 + MathHelper.Pi, Ai1, randomOffset);
                    }
                }

                NPC.ai[3] += MathHelper.PiOver4 * (Main.rand.NextBool() ? -1 : 1) //rotate 90 degrees
                    + Main.rand.NextFloat(MathHelper.PiOver4 / 2) * (Main.rand.NextBool() ? -1 : 1); //variation

                NPC.netUpdate = true;
            }

            void MegaSwordSwarm(Vector2 target)
            {
                SoundEngine.PlaySound(SoundID.Item164, player.Center);

                float safeAngle = NPC.ai[3];
                float safeRange = MathHelper.ToRadians(10);
                int max = 60;
                for (int i = 0; i < max; i++)
                {
                    float rotationOffset = Main.rand.NextFloat(safeRange, MathHelper.Pi - safeRange);
                    Vector2 offset = Main.rand.NextFloat(600f, 2400f) * (safeAngle + rotationOffset).ToRotationVector2();
                    if (Main.rand.NextBool())
                        offset *= -1;

                    //if (WorldSavingSystem.MasochistModeReal) //block one side so only one real exit exists
                    //    target += Main.rand.NextFloat(600) * safeAngle.ToRotationVector2();

                    Vector2 spawnPos = target + offset;
                    Vector2 vel = (target - spawnPos) / 60f;
                    Sword(spawnPos, vel.ToRotation(), (float)i / max, -vel * 0.75f);
                }
                EdgyBossText(GFBQuote(25)); //you really didn't
            }

            //massive sword barrage
            int swordSwarmTime = startup + attackThreshold * timesToAttack + 40;
            if (NPC.ai[1] == swordSwarmTime)
            {
                MegaSwordSwarm(player.Center);
                NPC.localAI[0] = player.Center.X;
                NPC.localAI[1] = player.Center.Y;
            }

            if (WorldSavingSystem.MasochistModeReal && NPC.ai[1] == swordSwarmTime + 30)
            {
                for (int i = -1; i <= 1; i += 2)
                {
                    MegaSwordSwarm(new Vector2(NPC.localAI[0], NPC.localAI[1]) + 600 * i * NPC.ai[3].ToRotationVector2());
                }
            }

            if (++NPC.ai[1] > swordSwarmTime + (WorldSavingSystem.MasochistModeReal ? 60 : 30))
            {
                ChooseNextAttack(11, 13, 16, 21, WorldSavingSystem.MasochistModeReal ? 26 : 24, 28, 29, 35, 37, 39, 41, 45);
            }
        }

        void SANSGOLEM()
        {
            if (NPC.ai[3] == 0)
            {
                NPC.ai[3] = Main.rand.NextBool() ? 1 : -1;

                SoundEngine.PlaySound(SoundID.ForceRoarPitched, NPC.Center);
                if (FargoSoulsUtil.HostCheck)
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<GlowRing>(), 0, 0f, Main.myPlayer, NPC.whoAmI, -26);
            }


            int attackDelay = 16;
            if (WorldSavingSystem.MasochistModeReal)
                attackDelay = 14;

            if (NPC.ai[1] < attackDelay - 10)
            {
                Vector2 targetPos = player.Center - Vector2.UnitY * 50;
                targetPos.X += 300 * player.HorizontalDirectionTo(NPC.Center);
                Movement(targetPos, 1f);
                NPC.ai[1] -= 0.8f;
            }
            else
            {
                NPC.ai[1] = (int)NPC.ai[1];
                NPC.velocity *= 0f;
            }

            int attacksToDo = 12 + 1;
            if (WorldSavingSystem.MasochistModeReal)
                attacksToDo += 4;
            if (WorldSavingSystem.MasochistModeReal && Main.getGoodWorld)
                attacksToDo += 8;
            int attackEndTime = attackDelay * attacksToDo + 50 + attackDelay * (int)Math.Round(4 * endTimeVariance);
            int endTime = attackEndTime + 60;

            if (NPC.ai[1] > 0 && NPC.ai[1] % attackDelay == 0 && NPC.ai[1] < attackDelay * attacksToDo)
            {
                EdgyBossText(GFBQuote(35));

                float oldOffset = NPC.ai[2];
                //basically, whenever deciding whether to go up or down, may use that decision for 2 steps
                bool reusedPreviousStep = false;
                if (Main.rand.NextBool() && NPC.localAI[0] != 0)
                {
                    reusedPreviousStep = true;
                    NPC.ai[2] += NPC.localAI[0];
                    NPC.localAI[0] = 0;
                }
                const int maxRangeOfVariance = 2;
                int increment = 0;
                while (NPC.ai[2] == oldOffset)
                {
                    increment = Main.rand.NextBool() ? -1 : 1;
                    NPC.ai[2] += increment;
                    if (Math.Abs(NPC.ai[2]) > maxRangeOfVariance) //if went out of bounds, forcibly turn around
                    {
                        if (NPC.ai[2] > 0)
                            increment = -1;
                        else
                            increment = 1;
                        NPC.ai[2] += increment;
                        break;
                    }
                }
                if (!reusedPreviousStep)
                    NPC.localAI[0] = increment;

                Vector2 auraPos = FargoSoulsUtil.ProjectileExists(ritualProj, ModContent.ProjectileType<MutantRitual>()) == null ? NPC.Center : Main.projectile[ritualProj].Center;
                Vector2 centerPoint = new(auraPos.X, auraPos.Y);
                float maxVariance = 90;
                float maxOffsetWithinStep = maxVariance / 3 * .75f; //x.75 so player always has to move a noticeable amount
                centerPoint.Y += maxVariance * NPC.ai[2]; //choose one of 7 base heights
                centerPoint.Y += Main.rand.NextFloat(-maxOffsetWithinStep, maxOffsetWithinStep);

                float xSpeedWhenAttacking = 16f;

                float i = NPC.ai[3];
                int sides = /*WorldSavingSystem.MasochistModeReal ? 2 :*/ 1;
                for (int side = 0; side < sides; side++)
                {
                    //if (WorldSavingSystem.MasochistModeReal) i *= -1;
                    for (int j = -1; j <= 1; j += 2) //flappy bird tubes
                    {
                        float gapRadiusHeight = WorldSavingSystem.MasochistModeReal ? 130 : 142;
                        Vector2 sansTargetPos = centerPoint;
                        const int timeToReachMiddle = 60;
                        sansTargetPos.X += xSpeedWhenAttacking * timeToReachMiddle * i;
                        sansTargetPos.Y += gapRadiusHeight * j;

                        int travelTime = 50;
                        Vector2 vel = (sansTargetPos - NPC.Center) / travelTime;

                        if (FargoSoulsUtil.HostCheck && NPC.ai[1] > attackDelay) // skip first one
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel,
                                ModContent.ProjectileType<MutantSansHead>(),
                                FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer,
                                travelTime, xSpeedWhenAttacking * -i, j);
                        }
                    }
                }
                
            }

            if (++NPC.ai[1] > endTime)
            {
                ChooseNextAttack(13, 19, 20, 21, 24, 28, 31, 35, 41, 44);
            }
        }

        void P2NextAttackPause() //choose next attack but actually, this also gives breathing space for mp to sync up
        {
            if (!AliveCheck(player))
                return;

            EModeSpecialEffects(); //manage these here, for case where players log out/rejoin in mp

            Vector2 targetPos = player.Center + NPC.DirectionFrom(player.Center) * 400;
            Movement(targetPos, 0.3f);
            if (NPC.Distance(targetPos) > 200) //faster if offscreen
                Movement(targetPos, 0.3f);

            float minWaitTime = 15;
            float maxFailSafeWaitTime = 90;
            float timeToWait = MathHelper.Lerp(minWaitTime, maxFailSafeWaitTime, (float)NPC.life / NPC.lifeMax);

            if (WorldSavingSystem.MasochistModeReal)
            {
                minWaitTime = timeToWait = 15;
                maxFailSafeWaitTime = 60;
            }

            if (++NPC.ai[1] > maxFailSafeWaitTime || (NPC.Distance(targetPos) < 200 && NPC.ai[1] > timeToWait))
            {
                /*EModeGlobalNPC.PrintAI(npc);
                string output = "";
                foreach (float attack in attackHistory)
                    output += attack.ToString() + " ";
                Main.NewText(output);*/

                NPC.velocity *= WorldSavingSystem.MasochistModeReal ? 0.25f : 0.75f;

                //NPC.TargetClosest();
                AttackChoice = NPC.ai[2];
                NPC.ai[1] = 0;
                NPC.ai[2] = 0;
                NPC.netUpdate = true;

                EdgyBossText(RandomObnoxiousQuote());
            }
        }

        #endregion

        #region p3

        bool Phase3Transition()
        {
            bool retval = true;

            PhaseState = 3;

            EModeSpecialEffects();

            //NPC.damage = 0;
            if (NPC.buffType[0] != 0)
                NPC.DelBuff(0);

            NPC.buffImmune[ModContent.BuffType<TimeFrozenBuff>()] = true;

            if (NPC.ai[1] == 0) //entering final phase, give healing
            {
                NPC.life = NPC.lifeMax;

                DramaticTransition(true);
            }

            if (NPC.ai[1] < 60 && !Main.dedServ && Main.LocalPlayer.active)
                FargoSoulsUtil.ScreenshakeRumble(6);

            if (NPC.ai[1] == 360)
            {
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
            }

            if (++NPC.ai[1] > 480)
            {
                retval = false; //dont drain life during this time, ensure it stays synced

                if (!AliveCheck(player))
                    return retval;
                Vector2 targetPos = player.Center;
                targetPos.Y -= 300;
                Movement(targetPos, 1f, true, false);
                if (NPC.Distance(targetPos) < 50 || NPC.ai[1] > 720)
                {
                    NPC.netUpdate = true;
                    NPC.velocity = Vector2.Zero;
                    NPC.localAI[0] = 0;
                    AttackChoice--;
                    NPC.ai[1] = 0;
                    NPC.ai[2] = NPC.DirectionFrom(player.Center).ToRotation();
                    NPC.ai[3] = (float)Math.PI / 20f;
                    SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                    if (player.Center.X < NPC.Center.X)
                        NPC.ai[3] *= -1;
                    EdgyBossText(GFBQuote(26));
                }
            }
            else
            {
                NPC.velocity *= 0.9f;

                //make you stop attacking
                if (Main.LocalPlayer.active && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost && NPC.Distance(Main.LocalPlayer.Center) < 3000)
                {
                    Main.LocalPlayer.controlUseItem = false;
                    Main.LocalPlayer.controlUseTile = false;
                    Main.LocalPlayer.FargoSouls().NoUsingItems = 2;
                }

                if (--NPC.localAI[0] < 0)
                {
                    NPC.localAI[0] = Main.rand.Next(15);
                    if (FargoSoulsUtil.HostCheck)
                    {
                        Vector2 spawnPos = NPC.position + new Vector2(Main.rand.Next(NPC.width), Main.rand.Next(NPC.height));
                        int type = ModContent.ProjectileType<MutantBombSmall>();
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnPos, Vector2.Zero, type, 0, 0f, Main.myPlayer);
                    }
                }
            }

            for (int i = 0; i < 5; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, FargoSoulsUtil.AprilFools ? DustID.SolarFlare : DustID.Vortex, 0f, 0f, 0, default, 1.5f);
                Main.dust[d].noGravity = true;
                Main.dust[d].noLight = true;
                Main.dust[d].velocity *= 4f;
            }

            return retval;
        }

        void VoidRaysP3()
        {
            if (--NPC.ai[1] < 0)
            {
                if (FargoSoulsUtil.HostCheck)
                {
                    float speed = WorldSavingSystem.MasochistModeReal && NPC.localAI[0] <= 40 ? 4f : 2f;
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, speed * Vector2.UnitX.RotatedBy(NPC.ai[2]), ModContent.ProjectileType<MutantMark1>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer);
                }
                NPC.ai[1] = 1;
                NPC.ai[2] += NPC.ai[3];

                if (NPC.localAI[0] < 30)
                {
                    EModeSpecialEffects();
                    //TryMasoP3Theme();
                }

                if (NPC.localAI[0]++ == 40 || NPC.localAI[0] == 80 || NPC.localAI[0] == 120)
                {
                    NPC.netUpdate = true;
                    NPC.ai[2] -= NPC.ai[3] / (WorldSavingSystem.MasochistModeReal ? 3 : 2);
                }
                else if (NPC.localAI[0] >= (WorldSavingSystem.MasochistModeReal ? 160 : 120))
                {
                    NPC.netUpdate = true;
                    AttackChoice--;
                    NPC.ai[1] = 0;
                    NPC.ai[2] = 0;
                    NPC.ai[3] = 0;
                    NPC.localAI[0] = 0;
                    EdgyBossText(GFBQuote(27));
                }
            }
            for (int i = 0; i < 5; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, FargoSoulsUtil.AprilFools ? DustID.SolarFlare : DustID.Vortex, 0f, 0f, 0, default, 1.5f);
                Main.dust[d].noGravity = true;
                Main.dust[d].noLight = true;
                Main.dust[d].velocity *= 4f;
            }

            NPC.velocity = Vector2.Zero;
        }

        void MyBallsP3()
        {
            int endTime = 360 + 120;
            if (WorldSavingSystem.MasochistModeReal)
                endTime += 360;

            int attackEndTime = endTime - 120;

            if (NPC.ai[2] == 0)
            {
                if (!AliveCheck(player))
                    return;
                NPC.ai[2] = Main.rand.NextBool() ? -1 : 1;
                NPC.ai[3] = Main.rand.NextFloat((float)Math.PI * 2);
                if (FargoSoulsUtil.HostCheck)
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpearSpin>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI, attackEndTime, 2);
            }

            if (NPC.ai[2] == 0)
            {
                NPC.ai[2] = Main.rand.NextBool() ? -1 : 1;
                if (FargoSoulsUtil.HostCheck)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<GlowRing>(), 0, 0f, Main.myPlayer, NPC.whoAmI, -2);
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpearSpin>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI, attackEndTime, 2);
                }

                EdgyBossText(GFBQuote(22));
            }

            const int startupDelay = 60;
            if (WorldSavingSystem.MasochistModeReal) //helix
            {
                int maxTime = WorldSavingSystem.MasochistModeReal ? 6 : 10;
                if (++NPC.ai[1] > maxTime)
                {
                    NPC.ai[1] = 0;
                    const int max = 12;
                    float speed = WorldSavingSystem.MasochistModeReal ? 7f : 5f;

                    for (int j = -1; j <= 1; j += 2)
                    {
                        for (int i = 0; i < max; i++)
                        {
                            float totalDegreesToRotate = WorldSavingSystem.MasochistModeReal ? 120 : 90;
                            float rotation = MathHelper.ToRadians(totalDegreesToRotate) / 300 * NPC.ai[2];
                            float spawnRotation = rotation * NPC.ai[3];
                            Vector2 vel = speed * spawnRotation.ToRotationVector2().RotatedBy(MathHelper.TwoPi / max * i);
                            if (NPC.ai[3] > startupDelay && NPC.ai[3] < attackEndTime)
                            {
                                SoundEngine.PlaySound(SoundID.Item84, NPC.Center);
                                if (FargoSoulsUtil.HostCheck)
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel, ModContent.ProjectileType<MutantSphereHelix>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI, j, rotation);
                            }
                            if (NPC.ai[3] < startupDelay * 2 && !Main.getGoodWorld)
                            {
                                if (FargoSoulsUtil.HostCheck)
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel, ModContent.ProjectileType<MutantFancyMovementTell>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, rotation, NPC.whoAmI, j);
                            }
                        }
                    }
                }
            }
            else //okuu
            {
                if (++NPC.ai[1] > 9 && NPC.ai[3] > startupDelay && NPC.ai[3] < attackEndTime)
                {
                    NPC.ai[1] = 0;
                    float rotationPerTick = MathHelper.ToRadians(90) / 240 * NPC.ai[2];
                    float rotation = rotationPerTick * (NPC.ai[3] - 45);
                    int max = WorldSavingSystem.MasochistModeReal ? 11 : 10;
                    float speed = WorldSavingSystem.MasochistModeReal ? 11f : 10f;
                    SpawnSphereRing(max, speed, FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), -1f, rotation, alt: rotationPerTick);
                    SpawnSphereRing(max, speed, FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 1f, rotation, alt: rotationPerTick);
                }
            }

            if (NPC.ai[3] < 30)
            {
                EModeSpecialEffects();
                //TryMasoP3Theme();
            }
            if (NPC.ai[3] == (int)(endTime / 2))
            {
                EdgyBossText(GFBQuote(28));
            }
            if (++NPC.ai[3] > endTime)
            {
                NPC.netUpdate = true;
                AttackChoice--;
                NPC.ai[1] = 0;
                NPC.ai[2] = 0;
                NPC.ai[3] = 0;
                EdgyBossText(GFBQuote(29));
                //NPC.TargetClosest();
            }
            for (int i = 0; i < 5; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, FargoSoulsUtil.AprilFools ? DustID.SolarFlare : DustID.Vortex, 0f, 0f, 0, default, 1.5f);
                Main.dust[d].noGravity = true;
                Main.dust[d].noLight = true;
                Main.dust[d].velocity *= 4f;
            }

            NPC.velocity = Vector2.Zero;
        }

        void BoundaryBulletHellP3()
        {
            int endTime = 360;
            if (WorldSavingSystem.MasochistModeReal)
                endTime += 360;

            if (NPC.localAI[0] == 0)
            {
                if (!AliveCheck(player))
                    return;
                NPC.localAI[0] = Math.Sign(NPC.Center.X - player.Center.X);
                if (FargoSoulsUtil.HostCheck)
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpearSpin>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer, NPC.whoAmI, endTime, 2);
            }

            if (++NPC.ai[1] > 3)
            {
                SoundEngine.PlaySound(SoundID.Item12, NPC.Center);
                NPC.ai[1] = 0;
                NPC.ai[2] += (float)Math.PI / 5 / 420 * NPC.ai[3] * NPC.localAI[0] * (WorldSavingSystem.MasochistModeReal ? 2f : 1);
                if (NPC.ai[2] > (float)Math.PI)
                    NPC.ai[2] -= (float)Math.PI * 2;
                if (FargoSoulsUtil.HostCheck)
                {
                    int max = WorldSavingSystem.MasochistModeReal ? 10 : 8;
                    for (int i = 0; i < max; i++)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0f, -6f).RotatedBy(NPC.ai[2] + MathHelper.TwoPi / max * i),
                            ModContent.ProjectileType<MutantEye>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage), 0f, Main.myPlayer);
                    }
                }
            }

            if (NPC.ai[3] < 30)
            {
                EModeSpecialEffects();
                //TryMasoP3Theme();
            }

            if (NPC.ai[3] == (int)endTime / 2)
            {
                EdgyBossText(GFBQuote(30));
            }
            if (++NPC.ai[3] > endTime)
            {
                //NPC.TargetClosest();
                AttackChoice--;
                NPC.ai[1] = 0;
                NPC.ai[2] = 0;
                NPC.ai[3] = 0;
                NPC.localAI[0] = 0;
                NPC.netUpdate = true;
            }

            for (int i = 0; i < 5; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, FargoSoulsUtil.AprilFools ? DustID.SolarFlare : DustID.Vortex, 0f, 0f, 0, default, 1.5f);
                Main.dust[d].noGravity = true;
                Main.dust[d].noLight = true;
                Main.dust[d].velocity *= 4f;
            }

            NPC.velocity = Vector2.Zero;
        }

        void FinalSpark()
        {
            void SpinLaser(bool useMasoSpeed)
            {
                float newRotation = NPC.SafeDirectionTo(Main.player[NPC.target].Center).ToRotation();
                float difference = MathHelper.WrapAngle(newRotation - NPC.ai[3]);
                float rotationDirection = 2f * (float)Math.PI * 1f / 6f / 60f;
                rotationDirection *= useMasoSpeed ? 1.1f : 1f;
                float change = Math.Min(rotationDirection, Math.Abs(difference)) * Math.Sign(difference);
                if (useMasoSpeed)
                {
                    change *= 1.1f;
                    float angleLerp = NPC.ai[3].AngleLerp(newRotation, 0.015f) - NPC.ai[3];
                    if (Math.Abs(MathHelper.WrapAngle(angleLerp)) > Math.Abs(MathHelper.WrapAngle(change)))
                        change = angleLerp;
                }
                NPC.ai[3] += change;

                EdgyBossText(GFBQuote(31));
            }

            /*
            //if targets are all dead, will despawn much more aggressively to reduce respawn cheese
            if (NPC.localAI[2] > 30)
            {
                NPC.localAI[2] += 1; //after 30 ticks of no target, despawn can't be stopped
                if (NPC.localAI[2] > 120)
                    AliveCheck(player, true);
                return;
            }
            */
            if (!AliveCheck(player))
                return;

            if (--NPC.localAI[0] < 0) //just visual explosions
            {
                NPC.localAI[0] = Main.rand.Next(30);
                if (FargoSoulsUtil.HostCheck)
                {
                    Vector2 spawnPos = NPC.position + new Vector2(Main.rand.Next(NPC.width), Main.rand.Next(NPC.height));
                    int type = ModContent.ProjectileType<MutantBombSmall>();
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnPos, Vector2.Zero, type, 0, 0f, Main.myPlayer);
                }
            }

            bool harderRings = WorldSavingSystem.MasochistModeReal && NPC.ai[2] >= 420 - 90;
            int ringTime = harderRings ? 100 : 120;
            if (++NPC.ai[1] > ringTime)
            {
                NPC.ai[1] = 0;

                EModeSpecialEffects();
                //TryMasoP3Theme();

                if (FargoSoulsUtil.HostCheck)
                {
                    int max = /*harderRings ? 11 :*/ 10;
                    int damage = FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage);
                    SpawnSphereRing(max, 6f, damage, 0.5f);
                    SpawnSphereRing(max, 6f, damage, -.5f);
                }
            }

            if (NPC.ai[2] == 0)
            {
                if (!WorldSavingSystem.MasochistModeReal)
                    NPC.localAI[1] = 1;
            }
            else if (NPC.ai[2] == 420 - 90) //dramatic telegraph
            {
                DoingSuperFinalSpark = true;

                if (NPC.localAI[1] == 0) //maso do ordinary spark
                {
                    NPC.localAI[1] = 1;
                    NPC.ai[2] -= 600 + 180;

                    //bias in one direction
                    NPC.ai[3] -= MathHelper.ToRadians(20);

                    if (FargoSoulsUtil.HostCheck)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitX.RotatedBy(NPC.ai[3]),
                            ModContent.ProjectileType<MutantGiantDeathray2>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage, 0.5f), 0f, Main.myPlayer, 0, NPC.whoAmI);
                    }

                    NPC.netUpdate = true;
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

                    if (FargoSoulsUtil.HostCheck)
                    {
                        const int max = 8;
                        for (int i = 0; i < max; i++)
                        {
                            float offset = i - 0.5f;
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, (NPC.ai[3] + MathHelper.TwoPi / max * offset).ToRotationVector2(), ModContent.ProjectileType<GlowLine>(), 0, 0f, Main.myPlayer, 13f, NPC.whoAmI);
                        }
                    }
                }
            }

            if (NPC.ai[2] < 420)
            {
                //disable it while doing maso's first ray
                if (NPC.localAI[1] == 0 || NPC.ai[2] > 420 - 90)
                    NPC.ai[3] = NPC.DirectionFrom(player.Center).ToRotation(); //hold it here for glow line effect
            }
            else
            {
                if (!Main.dedServ)
                {
                    ManagedScreenFilter filter = ShaderManager.GetFilter("FargowiltasSouls.FinalSpark");
                    filter.Activate();
                    Main.WaveQuality = 1;
                }

                if (NPC.ai[1] % 3 == 0 && FargoSoulsUtil.HostCheck)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, 24f * Vector2.UnitX.RotatedBy(NPC.ai[3]), ModContent.ProjectileType<MutantEyeWavy>(), 0, 0f, Main.myPlayer,
                      Main.rand.NextFloat(0.5f, 1.25f) * (Main.rand.NextBool() ? -1 : 1), Main.rand.Next(10, 60));
                }
            }

            int endTime = 1020;
            if (WorldSavingSystem.MasochistModeReal)
                endTime += 180;
            if (++NPC.ai[2] > endTime && NPC.life <= 1)
            {
                NPC.netUpdate = true;
                AttackChoice--;
                NPC.ai[1] = 0;
                NPC.ai[2] = 0;
                FargoSoulsUtil.ClearAllProjectiles(2, NPC.whoAmI);
            }
            else if (NPC.ai[2] == 420)
            {
                NPC.netUpdate = true;

                //bias it in one direction
                NPC.ai[3] += MathHelper.ToRadians(20) * (WorldSavingSystem.MasochistModeReal ? 1 : -1);

                if (FargoSoulsUtil.HostCheck)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitX.RotatedBy(NPC.ai[3]),
                        ModContent.ProjectileType<MutantGiantDeathray2>(), FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage, 0.5f), 0f, Main.myPlayer, 0, NPC.whoAmI);
                }
            }
            else if (NPC.ai[2] < 300 && NPC.localAI[1] != 0) //charging up dust
            {
                float num1 = 0.99f;
                if (NPC.ai[2] >= 60)
                    num1 = 0.79f;
                if (NPC.ai[2] >= 120)
                    num1 = 0.58f;
                if (NPC.ai[2] >= 180)
                    num1 = 0.43f;
                if (NPC.ai[2] >= 240)
                    num1 = 0.33f;
                for (int i = 0; i < 9; ++i)
                {
                    if (Main.rand.NextFloat() >= num1)
                    {
                        float f = Main.rand.NextFloat() * 6.283185f;
                        float num2 = Main.rand.NextFloat();
                        Dust dust = Dust.NewDustPerfect(NPC.Center + f.ToRotationVector2() * (110 + 600 * num2), 229, (f - 3.141593f).ToRotationVector2() * (14 + 8 * num2), 0, default, 1f);
                        dust.scale = 0.9f;
                        dust.fadeIn = 1.15f + num2 * 0.3f;
                        //dust.color = new Color(1f, 1f, 1f, num1) * (1f - num1);
                        dust.noGravity = true;
                        //dust.noLight = true;
                    }
                }
            }

            SpinLaser(WorldSavingSystem.MasochistModeReal && NPC.ai[2] >= 420);

            if (AliveCheck(player))
                NPC.localAI[2] = 0;
            else
                NPC.localAI[2]++;

            NPC.velocity = Vector2.Zero; //prevents mutant from moving despite calling AliveCheck()
        }

        void DyingDramaticPause()
        {
            if (!AliveCheck(player))
                return;
            NPC.ai[3] -= (float)Math.PI / 6f / 60f;
            NPC.velocity = Vector2.Zero;
            //in maso, if player got timestopped at very end of final spark, fucking kill them
            bool killPlayer = WorldSavingSystem.MasochistModeReal && Main.player[NPC.target].HasBuff(ModContent.BuffType<TimeFrozenBuff>());
            if (killPlayer)
            {
                if (++NPC.ai[2] > 15)
                {
                    NPC.ai[2] -= 15;
                    int realDefDamage = NPC.defDamage;
                    NPC.defDamage *= 10;
                    SpawnSpearTossDirectP2Attack();
                    NPC.defDamage = realDefDamage;
                }
            }
            else if (++NPC.ai[1] > 120)
            {
                NPC.netUpdate = true;
                AttackChoice--;
                NPC.ai[1] = 0;
                NPC.ai[2] = 0;
                NPC.ai[3] = (float)-Math.PI / 2;
                NPC.netUpdate = true;
                if (FargoSoulsUtil.HostCheck) //shoot death anim mega ray
                {
                    int damage = WorldSavingSystem.MasochistModeReal ? FargoSoulsUtil.ScaledProjectileDamage(NPC.defDamage, 0.5f) : 0;
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitY * -1,
                        ModContent.ProjectileType<MutantGiantDeathray2>(),
                        damage, 0f, Main.myPlayer, 1, NPC.whoAmI);
                }
                EdgyBossText(GFBQuote(32));
            }
            if (--NPC.localAI[0] < 0)
            {
                NPC.localAI[0] = Main.rand.Next(15);
                if (FargoSoulsUtil.HostCheck)
                {
                    Vector2 spawnPos = NPC.position + new Vector2(Main.rand.Next(NPC.width), Main.rand.Next(NPC.height));
                    int type = ModContent.ProjectileType<MutantBomb>();
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnPos, Vector2.Zero, type, 0, 0f, Main.myPlayer);
                }
            }
            for (int i = 0; i < 5; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, FargoSoulsUtil.AprilFools ? DustID.SolarFlare : DustID.Vortex, 0f, 0f, 0, default, 1.5f);
                Main.dust[d].noGravity = true;
                Main.dust[d].noLight = true;
                Main.dust[d].velocity *= 4f;
            }
        }

        void DyingAnimationAndHandling()
        {
            /*if (WorldSavingSystem.MasochistModeReal)
            {
                if (!AliveCheck(player))
                    return;
                i'm not THAT fucked up
            }*/
            NPC.velocity = Vector2.Zero;
            for (int i = 0; i < 5; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, FargoSoulsUtil.AprilFools ? DustID.SolarFlare : DustID.Vortex, 0f, 0f, 0, default, 2.5f);
                Main.dust[d].noGravity = true;
                Main.dust[d].noLight = true;
                Main.dust[d].velocity *= 12f;
            }
            if (--NPC.localAI[0] < 0)
            {
                NPC.localAI[0] = Main.rand.Next(5);
                if (FargoSoulsUtil.HostCheck)
                {
                    Vector2 spawnPos = NPC.Center + Main.rand.NextVector2Circular(240, 240);
                    int type = ModContent.ProjectileType<MutantBomb>();
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnPos, Vector2.Zero, type, 0, 0f, Main.myPlayer);
                }
            }
            if (++NPC.ai[1] % 3 == 0 && FargoSoulsUtil.HostCheck)
            {
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, 24f * Vector2.UnitX.RotatedBy(NPC.ai[3]), ModContent.ProjectileType<MutantEyeWavy>(), 0, 0f, Main.myPlayer,
                    Main.rand.NextFloat(0.75f, 1.5f) * (Main.rand.NextBool() ? -1 : 1), Main.rand.Next(10, 90));
            }
            if (++NPC.alpha > 255)
            {
                NPC.alpha = 255;
                NPC.life = 0;
                NPC.dontTakeDamage = false;
                NPC.checkDead();
                if (FargoSoulsUtil.HostCheck && ModContent.TryFind("Fargowiltas", "Mutant", out ModNPC modNPC) && !NPC.AnyNPCs(modNPC.Type))
                {
                    int n = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, modNPC.Type);
                    if (n != Main.maxNPCs)
                    {
                        Main.npc[n].homeless = true;
                        if (TownNPCName != default)
                            Main.npc[n].GivenName = TownNPCName;
                        if (Main.netMode == NetmodeID.Server)
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, n);
                    }
                }
                EdgyBossText(GFBQuote(33));
            }
        }

        #endregion

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<CurseoftheMoonBuff>(), 240);
            target.AddBuff(ModContent.BuffType<MutantFangBuff>(), 180);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 3; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, FargoSoulsUtil.AprilFools ? DustID.SolarFlare : DustID.Vortex, 0f, 0f, 0, default, 1f);
                Main.dust[d].noGravity = true;
                Main.dust[d].noLight = true;
                Main.dust[d].velocity *= 3f;
            }
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            if (WorldSavingSystem.AngryMutant)
                modifiers.FinalDamage *= 0.07f;
        }

        public override bool CheckDead()
        {
            if (AttackChoice == -7)
                return true;

            NPC.life = 1;
            NPC.active = true;
            if (FargoSoulsUtil.HostCheck && AttackChoice > -1)
            {
                AttackChoice =AttackChoice >= 10 ? -1 : 10;
                NPC.ai[1] = 0;
                NPC.ai[2] = 0;
                NPC.ai[3] = 0;
                NPC.localAI[0] = 0;
                NPC.localAI[1] = 0;
                NPC.localAI[2] = 0;
                NPC.dontTakeDamage = true;
                NPC.netUpdate = true;
                FargoSoulsUtil.ClearAllProjectiles(2, NPC.whoAmI, AttackChoice < 0);
                EdgyBossText(GFBQuote(34));
            }
            return false;
        }

        public override void OnKill()
        {
            base.OnKill();

            if (WorldSavingSystem.MasochistModeReal || (!playerInvulTriggered))
            {
                Item.NewItem(NPC.GetSource_Loot(), NPC.Hitbox, ModContent.ItemType<BrokenSpearhead>());
            }

                if (Main.LocalPlayer.active)
                {
                    if (!Main.LocalPlayer.FargoSouls().Toggler.CanPlayMaso && Main.netMode != NetmodeID.Server)
                        Main.NewText(Language.GetTextValue($"Mods.{Mod.Name}.Message.MasochistModeUnlocked"), new Color(51, 255, 191, 0));
                    Main.LocalPlayer.FargoSouls().Toggler.CanPlayMaso = true;
                }
                WorldSavingSystem.CanPlayMaso = true;

            WorldSavingSystem.SkipMutantP1 = 0;

            WorldSavingSystem.SetMutantDowned();
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            base.ModifyNPCLoot(npcLoot);

            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<MutantBag>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MutantTrophy>(), 10));

            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<MutantRelic>()));
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<SpawnSack>(), 4));

            LeadingConditionRule emodeRule = new(new EModeDropCondition());
            emodeRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ModContent.ItemType<FargowiltasSouls.Content.Items.Accessories.Eternity.MutantEye>()));
            npcLoot.Add(emodeRule);
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void FindFrame(int frameHeight)
        {
            if (++NPC.frameCounter > 4)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
                    NPC.frame.Y = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture2D13 = TextureAssets.Npc[NPC.type].Value;
            Vector2 position = NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY);
            Rectangle rectangle = NPC.frame;
            Vector2 origin2 = rectangle.Size() / 2f;

            SpriteEffects effects = NPC.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(texture2D13, position, new Rectangle?(rectangle), NPC.GetAlpha(drawColor), NPC.rotation, origin2, NPC.scale, effects, 0);

            Vector2 auraPosition = AuraCenter;
            if (ShouldDrawAura)
                DrawAura(spriteBatch, auraPosition, AuraScale);

            return false;
        }

        public void DrawAura(SpriteBatch spriteBatch, Vector2 position, float auraScale)
        {
            Color outerColor = FargoSoulsUtil.AprilFools ? Color.Red : Color.CadetBlue;
            outerColor.A = 0;

            Color darkColor = outerColor;
            Color mediumColor = Color.Lerp(outerColor, Color.White, 0.75f);
            Color lightColor2 = Color.Lerp(outerColor, Color.White, 0.5f);

            Vector2 auraPos = position;
            float radius = 2000f * auraScale;
            var target = Main.LocalPlayer;
            var blackTile = TextureAssets.MagicPixel;
            var diagonalNoise = FargoAssets.WavyNoise;
            if (!blackTile.IsLoaded || !diagonalNoise.IsLoaded)
                return;
            var maxOpacity = NPC.Opacity;

            ManagedShader borderShader = ShaderManager.GetShader("FargowiltasSouls.MutantP1Aura");
            borderShader.TrySetParameter("colorMult", 7.35f);
            borderShader.TrySetParameter("time", Main.GlobalTimeWrappedHourly);
            borderShader.TrySetParameter("radius", radius);
            borderShader.TrySetParameter("anchorPoint", auraPos);
            borderShader.TrySetParameter("screenPosition", Main.screenPosition);
            borderShader.TrySetParameter("screenSize", Main.ScreenSize.ToVector2());
            borderShader.TrySetParameter("playerPosition", target.Center);
            borderShader.TrySetParameter("maxOpacity", maxOpacity);
            borderShader.TrySetParameter("darkColor", darkColor.ToVector4());
            borderShader.TrySetParameter("midColor", mediumColor.ToVector4());
            borderShader.TrySetParameter("lightColor", lightColor2.ToVector4());

            spriteBatch.GraphicsDevice.Textures[1] = diagonalNoise.Value;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearWrap, DepthStencilState.None, Main.Rasterizer, borderShader.WrappedEffect, Main.GameViewMatrix.TransformationMatrix);
            Rectangle rekt = new(Main.screenWidth / 2, Main.screenHeight / 2, Main.screenWidth, Main.screenHeight);
            spriteBatch.Draw(blackTile.Value, rekt, null, default, 0f, blackTile.Value.Size() * 0.5f, 0, 0f);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }
        
        public static void ArenaAura(Vector2 center, float distance, bool reverse = false, int dustid = -1, Color color = default, params int[] buffs)
        {
            Player p = Main.LocalPlayer;


            if (buffs.Length == 0 || buffs[0] < 0)
                return;

            float range = center.Distance(p.Center);
            if (p.active && !p.dead && !p.ghost && (reverse ? range > distance && range < Math.Max(3000f, distance * 2) : range < distance))
            {
                foreach (int buff in buffs)
                {
                    FargoSoulsUtil.AddDebuffFixedDuration(p, buff, 2);
                }
            }
        }
    }
}