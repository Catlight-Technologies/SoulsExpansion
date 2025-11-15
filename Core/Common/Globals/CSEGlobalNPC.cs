using FargowiltasSouls.Content.Bosses.AbomBoss;
using FargowiltasSouls.Content.Bosses.MutantBoss;
using System;
using Terraria.ModLoader;
using Terraria;
using CSE.Core;
using FargowiltasSouls.Content.Bosses.DeviBoss;
using FargowiltasSouls.Core.Globals;
using Terraria.ID;
using FargowiltasSouls;
using CSE.Content.Common.Accessories.Other;
using CSE.Content.Common.Materials;
using FargowiltasSouls.Content.Items.Materials;
using FargowiltasSouls.Core.Systems;

namespace CatTech.Core.Crossmod.Fargos
{
    public class CSEGlobalNPC : GlobalNPC
    {
        public int timerCSE1;
        public override bool InstancePerEntity => true;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.ImmuneToRegularBuffs[ModContent.NPCType<MutantBoss>()] = true;
            NPCID.Sets.ImmuneToRegularBuffs[ModContent.NPCType<DeviBoss>()] = true;
            NPCID.Sets.ImmuneToRegularBuffs[ModContent.NPCType<AbomBoss>()] = true;

            if (EModeGlobalNPC.spawnFishronEX)
            {
                NPCID.Sets.ImmuneToRegularBuffs[NPCID.DukeFishron] = true;
            }
        }

        public override void AI(NPC npc)
        {
            timerCSE1++;
            if (EModeGlobalNPC.fishBossEX == npc.whoAmI)
            {
                NPC mutantBoss = new NPC();
                mutantBoss.SetDefaults(ModContent.NPCType<MutantBoss>());

                npc.damage = (int)(mutantBoss.damage * 0.5f);
                npc.lifeMax = (int)(mutantBoss.lifeMax * 0.3f);
                npc.defense = 0;
            }
            if (npc.type == NPCID.Sharkron || npc.type == NPCID.Sharkron2)
            {
                if (FargoSoulsUtil.BossIsAlive(ref EModeGlobalNPC.fishBossEX, NPCID.DukeFishron))
                {
                    npc.lifeMax /= 500;
                }
            }
            base.AI(npc);
        }

        public override bool CheckDead(NPC npc)
        {
            if (!(npc.ai[0] <= 9))
            {
                if (EModeGlobalNPC.fishBossEX == npc.whoAmI)
                {
                    if (WorldSavingSystem.EternityMode)
                    {
                        npc.DropItemInstanced(npc.position, npc.Size, ModContent.ItemType<CyclonicFin>());
                    }
                    int maxEX = Main.rand.Next(5) + 10;
                    for (int i = 0; i < maxEX; i++)
                        npc.DropItemInstanced(npc.position, npc.Size, ModContent.ItemType<EternalScale>());
                    int maxAbom = Main.rand.Next(50) + 100;
                    for (int i = 0; i < maxAbom; i++)
                        npc.DropItemInstanced(npc.position, npc.Size, ModContent.ItemType<AbomEnergy>());
                    int maxDevi = Main.rand.Next(100) + 200;
                    for (int i = 0; i < maxDevi; i++)
                        npc.DropItemInstanced(npc.position, npc.Size, ModContent.ItemType<DeviatingEnergy>());

                    return false;
                }
            }
            return base.CheckDead(npc);
        }
    }
}
