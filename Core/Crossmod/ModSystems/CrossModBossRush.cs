using CalamityMod.NPCs.Bumblebirb;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.NPCs.ExoMechs;
using static CalamityMod.Events.BossRushEvent;
using Terraria.ModLoader;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.CeaselessVoid;
using CalamityMod.NPCs.StormWeaver;
using CalamityMod.NPCs.Signus;
using CalamityMod.NPCs.Polterghast;
using CalamityMod.NPCs.OldDuke;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.Yharon;
using Terraria;
using Redemption.WorldGeneration;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace CSE.Core.Crossmod.ModSystems
{
    [ExtendsFromMod(ModCompatibility.Crossmod.Name)]
    [JITWhenModsEnabled(ModCompatibility.Crossmod.Name)]
    public class CrossModBossRush : ModSystem
    {
        internal static void SpawnTP()
        {
            ActiveEntityIterator<Player>.Enumerator enumerator = Main.ActivePlayers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Player player = enumerator.Current;
                player.Spawn(PlayerSpawnContext.RecallFromItem);
                SoundStyle style = TeleportSound with { Volume = 1.6f };
                SoundEngine.PlaySound(in style, player.Center);
            }
        }

        [JITWhenModsEnabled(ModCompatibility.Redemption.Name)]
        public Vector2 PZRoom()
        {
            ActiveEntityIterator<Player>.Enumerator enumerator = Main.ActivePlayers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                _ = enumerator.Current;
                Vector2 labVector = RedeGen.LabVector;
                int rectX = (int)(labVector.X + 109f) * 16;
                int rectY = (int)(labVector.Y + 170f) * 16;
                int width = 1120;
                int height = 672;

                Vector2 centerPos = new Vector2(
                    rectX + width / 2f,
                    rectY + height / 2f
                );

                return centerPos;
            }
            return new Vector2(0f, 0f);
        }
        public override void PostSetupContent()
        {
            for (int i = Bosses.Count - 1; i >= 0; i--)
            {
                if (Bosses[i].EntityID == ModContent.NPCType<ProfanedGuardianCommander>())
                {
                }
                if (Bosses[i].EntityID == ModContent.NPCType<Bumblefuck>())
                {
                    if (ModCompatibility.Redemption.Loaded)
                    {
                        Bosses.Insert(i, new Boss(ModCompatibility.Redemption.Mod.Find<ModNPC>("OO").Type, TimeChangeContext.Night));
                    }
                    if (ModCompatibility.Homeward.Loaded)
                    {
                        Bosses.Insert(i, new Boss(ModCompatibility.Homeward.Mod.Find<ModNPC>("SlimeGod").Type));
                        Bosses.Insert(i, new Boss(ModCompatibility.Homeward.Mod.Find<ModNPC>("TheMaterealizer").Type));
                        Bosses.Insert(i, new Boss(ModCompatibility.Homeward.Mod.Find<ModNPC>("TheOverwatcher").Type));
                        Bosses.Insert(i, new Boss(ModCompatibility.Homeward.Mod.Find<ModNPC>("TheLifebringer").Type));
                    }
                }
                if (Bosses[i].EntityID == ModContent.NPCType<Providence>())
                {
                    if (ModCompatibility.SacredTools.Loaded)
                    {
                        Bosses.Insert(i, new Boss(ModCompatibility.SacredTools.Mod.Find<ModNPC>("Abaddon").Type, TimeChangeContext.Night));
                    }
                }
                if (Bosses[i].EntityID == ModContent.NPCType<CeaselessVoid>())
                {
                }
                if (Bosses[i].EntityID == ModContent.NPCType<StormWeaverHead>())
                {
                }
                if (Bosses[i].EntityID == ModContent.NPCType<Signus>())
                {
                }
                if (Bosses[i].EntityID == ModContent.NPCType<Polterghast>())
                {
                    if (ModCompatibility.SacredTools.Loaded)
                    {
                        Bosses[i].HostileNPCsToNotDelete.Add(ModCompatibility.SacredTools.Mod.Find<ModNPC>("AraghurMinion").Type);
                        Bosses[i].HostileNPCsToNotDelete.Add(ModCompatibility.SacredTools.Mod.Find<ModNPC>("AraghurBody").Type);
                        Bosses[i].HostileNPCsToNotDelete.Add(ModCompatibility.SacredTools.Mod.Find<ModNPC>("AraghurTail").Type);
                        Bosses.Insert(i, new Boss(ModCompatibility.SacredTools.Mod.Find<ModNPC>("AraghurHead").Type));
                    }
                }
                if (Bosses[i].EntityID == ModContent.NPCType<OldDuke>())
                {
                    if (ModCompatibility.Redemption.Loaded)
                    {
                        Bosses[i].HostileNPCsToNotDelete.Add(ModCompatibility.Redemption.Mod.Find<ModNPC>("PZ_Body_Holo").Type);
                        Bosses[i].HostileNPCsToNotDelete.Add(ModCompatibility.Redemption.Mod.Find<ModNPC>("PZ_Kari").Type);
                        Bosses[i].HostileNPCsToNotDelete.Add(ModCompatibility.Redemption.Mod.Find<ModNPC>("Akka").Type);
                        Bosses.Insert(i, new Boss(ModCompatibility.Redemption.Mod.Find<ModNPC>("Ukko").Type));
                        Bosses.Insert(i, new Boss(ModCompatibility.Redemption.Mod.Find<ModNPC>("PZ").Type, TimeChangeContext.None, spawnContext: type =>
                        {
                            int num8 = Player.FindClosest(new Vector2(Main.maxTilesX, Main.maxTilesY) * 16f * 0.5f, 1, 1);
                            num8.ToPlayer().Teleport(PZRoom());
                            _ = Main.player[num8];
                            NPC.SpawnOnPlayer(num8, ModCompatibility.Redemption.Mod.Find<ModNPC>("PZ_Body_Holo").Type);
                        }));

                        BossDeathEffects.Add(ModCompatibility.Redemption.Mod.Find<ModNPC>("PZ").Type, delegate
                        {
                            SpawnTP();
                        });
                    }
                    if (ModCompatibility.SacredTools.Loaded)
                    {
                        Bosses[i].HostileNPCsToNotDelete.Add(ModCompatibility.SacredTools.Mod.Find<ModNPC>("Novaniel").Type);
                        //BossIDsAfterDeath.Add(ModCompatibility.Thorium.Mod.Find<ModNPC>("Omnicide").Type, [ModCompatibility.Thorium.Mod.Find<ModNPC>("DreamEater").Type]);
                        Bosses.Insert(i, new Boss(ModCompatibility.SacredTools.Mod.Find<ModNPC>("Nuba").Type, TimeChangeContext.Night, type =>
                        {
                            NPC.SpawnOnPlayer(ClosestPlayerToWorldCenter, ModCompatibility.SacredTools.Mod.Find<ModNPC>("Nuba").Type);
                            NPC.SpawnOnPlayer(ClosestPlayerToWorldCenter, ModCompatibility.SacredTools.Mod.Find<ModNPC>("Voxa").Type);
                            NPC.SpawnOnPlayer(ClosestPlayerToWorldCenter, ModCompatibility.SacredTools.Mod.Find<ModNPC>("Dustite").Type);
                            NPC.SpawnOnPlayer(ClosestPlayerToWorldCenter, ModCompatibility.SacredTools.Mod.Find<ModNPC>("Solarius").Type);
                        }));
                    }
                }
                if (Bosses[i].EntityID == ModContent.NPCType<DevourerofGodsHead>())
                {
                    if (ModCompatibility.Thorium.Loaded)
                    {
                        BossIDsAfterDeath.Add(ModCompatibility.Thorium.Mod.Find<ModNPC>("Omnicide").Type, [ModCompatibility.Thorium.Mod.Find<ModNPC>("DreamEater").Type]);
                        Bosses[i].HostileNPCsToNotDelete.Add(ModCompatibility.Thorium.Mod.Find<ModNPC>("DreamEater").Type);
                        Bosses.Insert(i, new Boss(ModCompatibility.Thorium.Mod.Find<ModNPC>("Omnicide").Type, TimeChangeContext.Night, type =>
                        {
                            NPC.SpawnOnPlayer(ClosestPlayerToWorldCenter, ModCompatibility.Thorium.Mod.Find<ModNPC>("Omnicide").Type);
                            NPC.SpawnOnPlayer(ClosestPlayerToWorldCenter, ModCompatibility.Thorium.Mod.Find<ModNPC>("SlagFury").Type);
                            NPC.SpawnOnPlayer(ClosestPlayerToWorldCenter, ModCompatibility.Thorium.Mod.Find<ModNPC>("Aquaius").Type);
                        }));
                    }
                }
                if (Bosses[i].EntityID == ModContent.NPCType<Yharon>())
                {
                    if (ModCompatibility.Homeward.Loaded)
                    {
                        Bosses.Insert(i, new Boss(ModCompatibility.Homeward.Mod.Find<ModNPC>("WorldsEndEverlastingFallingWhale").Type));
                    }
                }
                if (Bosses[i].EntityID == ModContent.NPCType<Draedon>())
                {
                    if (ModCompatibility.Redemption.Loaded)
                    {
                        Bosses.Insert(i, new Boss(ModCompatibility.Redemption.Mod.Find<ModNPC>("Nebuleus_Clone").Type));
                    }
                    if (ModCompatibility.SacredTools.Loaded)
                    {
                        Bosses.Insert(i, new Boss(ModCompatibility.SacredTools.Mod.Find<ModNPC>("ErazorBoss").Type));
                    }
                }
                if (Bosses[i].EntityID == ModContent.NPCType<SupremeCalamitas>())
                {
                    if (ModCompatibility.Homeward.Loaded)
                    {
                        Bosses.Insert(i, new Boss(ModCompatibility.Homeward.Mod.Find<ModNPC>("TheSon").Type));
                    }
                }
                //if (Bosses[i].EntityID == ModContent.NPCType<Yharim>())
                //{
                //}
            }
        }
    }
}
