using FargowiltasSouls.Content.Bosses.AbomBoss;
using FargowiltasSouls.Content.Bosses.MutantBoss;
using System;
using Terraria.ModLoader;
using Terraria;
using CSE.Core;
using FargowiltasSouls.Content.Bosses.DeviBoss;
using FargowiltasSouls.Core.Globals;
using Terraria.ID;

namespace CatTech.Core.Crossmod.Fargos
{
    public class CSEGlobalNPC : GlobalNPC
    {
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
    }
}
