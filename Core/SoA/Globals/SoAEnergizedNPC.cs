using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using SacredTools.Content.NPCs.Boss.Decree;
using SacredTools.NPCs.Boss.Pumpkin;
using SacredTools.Content.NPCs.Boss.Jensen;
using SacredTools.NPCs.Boss.Araneas;
using SacredTools.NPCs.Boss.Raynare;
using SacredTools.NPCs.Boss.Primordia;
using SacredTools.NPCs.Boss.Abaddon;
using SacredTools.NPCs.Boss.Araghur;
using SacredTools.NPCs.Boss.Erazor;
using SacredTools.NPCs.Boss.Lunarians;
using SacredTools.NPCs.Boss.Obelisk.Nihilus;

namespace CSE.Core.SoA.Globals
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class SoAEnergizedGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool SwarmHealth = false;

        internal static int[] SoABosses = [
            ModContent.NPCType<Decree>(),
            ModContent.NPCType<Ralnek>(),
            ModContent.NPCType<Jensen>(),
            ModContent.NPCType<Araneas>(),
            ModContent.NPCType<Raynare>(),
            ModContent.NPCType<Primordia>(),
            ModContent.NPCType<Abaddon>(),
            ModContent.NPCType<AraghurHead>(),
            ModContent.NPCType<Novaniel>(),
            ModContent.NPCType<ErazorBoss>(),
            ModContent.NPCType<Nihilus>()
        ];

        public override void SetDefaults(NPC npc)
        {
            const int k = 1000;
            const int m = k * k;
            int baseHealth = 18 * k;
            int baseHealthHM = 48 * k;
            int baseHealthML = 120 * k;
            bool validBoss = true;

            if (Fargowiltas.Fargowiltas.SwarmSetDefaults)
            {
                if (npc.type == ModContent.NPCType<Decree>())
                    npc.lifeMax = baseHealth;

                else if (npc.type == ModContent.NPCType<Ralnek>())
                    npc.lifeMax = baseHealth;

                else if (npc.type == ModContent.NPCType<Ralnek2>())
                    npc.lifeMax = baseHealth;

                else if (npc.type == ModContent.NPCType<Jensen>())
                    npc.lifeMax = baseHealth;

                else if (npc.type == ModContent.NPCType<Araneas>())
                    npc.lifeMax = baseHealth;

                else if (npc.type == ModContent.NPCType<Raynare>())
                {
                    npc.lifeMax = baseHealthHM;
                    Fargowiltas.Fargowiltas.HardmodeSwarmActive = true;
                }

                else if (npc.type == ModContent.NPCType<Primordia>())
                {
                    npc.lifeMax = baseHealthHM;
                    Fargowiltas.Fargowiltas.HardmodeSwarmActive = true;
                    Fargowiltas.Fargowiltas.LateHardmodeSwarmActive = true;
                }

                else if (npc.type == ModContent.NPCType<Abaddon>())
                {
                    npc.lifeMax = baseHealthML;
                    Fargowiltas.Fargowiltas.HardmodeSwarmActive = true;
                    Fargowiltas.Fargowiltas.LateHardmodeSwarmActive = true;
                }

                else if (npc.type == ModContent.NPCType<AraghurHead>())
                {
                    npc.lifeMax = baseHealthML;
                    Fargowiltas.Fargowiltas.HardmodeSwarmActive = true;
                    Fargowiltas.Fargowiltas.LateHardmodeSwarmActive = true;
                }

                else if (npc.type == ModContent.NPCType<Novaniel>())
                {
                    npc.lifeMax = baseHealthML;
                    Fargowiltas.Fargowiltas.HardmodeSwarmActive = true;
                    Fargowiltas.Fargowiltas.LateHardmodeSwarmActive = true;
                }

                else if (npc.type == ModContent.NPCType<ErazorBoss>())
                {
                    npc.lifeMax = baseHealthML;
                    Fargowiltas.Fargowiltas.HardmodeSwarmActive = true;
                    Fargowiltas.Fargowiltas.LateHardmodeSwarmActive = true;
                }

                else if (npc.type == ModContent.NPCType<Nihilus>())
                {
                    npc.lifeMax = baseHealthML;
                    Fargowiltas.Fargowiltas.HardmodeSwarmActive = true;
                    Fargowiltas.Fargowiltas.LateHardmodeSwarmActive = true;
                }

                else
                    validBoss = false;
            }
            else
                validBoss = false;

            if (Fargowiltas.Fargowiltas.SwarmActive)
            {
                if (!validBoss)
                {
                    validBoss = true;

                    //if stats will go wrong

                    //if (npc.type == ModContent.NPCType<>())
                    //    npc.lifeMax = (int)(0.4 * k);

                    //else
                        validBoss = false;
                }

                if (validBoss && Fargowiltas.Fargowiltas.SwarmItemsUsed > 1)
                {
                    npc.lifeMax *= Fargowiltas.Fargowiltas.SwarmItemsUsed;
                    SwarmHealth = true;
                }

                int minDamage = Fargowiltas.Fargowiltas.SwarmMinDamage * 2;
                if (!npc.townNPC && npc.lifeMax > 10 && npc.damage > 0 && npc.damage < minDamage)
                    npc.damage = minDamage;
            }
        }

        private int go = 1;
        public override bool PreAI(NPC npc)
        {
            if (Fargowiltas.Fargowiltas.SwarmNoHyperActive)
                return true;
            if (Fargowiltas.Fargowiltas.LateHardmodeSwarmActive && Main.GameUpdateCount % 3 == 0)
                return true;
            if (Fargowiltas.Fargowiltas.HardmodeSwarmActive && Main.GameUpdateCount % 2 == 0)
                return true;

            //if speed will go wrong

            //if (Fargowiltas.Fargowiltas.SwarmActive && npc.type == ModContent.NPCType<>() && go < 2)
            //{
            //    go++;
            //    npc.AI();
            //    float speedToRemove = -0.25f;
            //    Vector2 newPos = npc.position + npc.velocity * speedToRemove;
            //    if (!Collision.SolidCollision(newPos, npc.width, npc.height))
            //    {
            //        npc.position = newPos;
            //    }
            //}

            return true;
        }

        private bool _logged;
        public override void PostAI(NPC npc)
        {
            if (go == 2)
            {
                go = 1;
            }

            if (!Fargowiltas.Fargowiltas.SwarmActive) return;

            if (!_logged)
            {
                _logged = true;
                if (Main.netMode != NetmodeID.Server)
                    Main.NewText($"[Swarm] Scaling Advisor (id {npc.whoAmI})", 175, 75, 255);
            }

            const int k = 1000;
            int desiredLifeMax = 28 * k * Math.Max(1, Fargowiltas.Fargowiltas.SwarmItemsUsed);

            if (npc.lifeMax != desiredLifeMax)
            {
                float ratio = npc.lifeMax > 0 ? npc.life / (float)npc.lifeMax : 1f;
                npc.lifeMax = desiredLifeMax;
                npc.life = Math.Clamp((int)(desiredLifeMax * ratio), 1, desiredLifeMax);
                npc.netUpdate = true;
            }

            int minDamage = Fargowiltas.Fargowiltas.SwarmMinDamage * 2;
            if (!npc.townNPC && npc.damage > 0 && npc.damage < minDamage)
                npc.damage = minDamage;

            /*
            if (!Fargowiltas.Fargowiltas.SwarmNoHyperActive)
                npc.velocity *= 1.08f;
            */

            npc.boss = true;
            npc.dontTakeDamage = false;
            npc.dontCountMe = false;
        }
    }
}
