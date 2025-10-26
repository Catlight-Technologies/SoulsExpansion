﻿using FargowiltasSouls.Content.Bosses.AbomBoss;
using FargowiltasSouls.Content.Bosses.MutantBoss;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace CSE.Core.Common.ModSystems
{
    public class CSEPostSetupContent : ModSystem
    {
        public static List<(int, float)> changes = new List<(int, float)>
        {
            (ModContent.NPCType<MutantBoss>(), 9999f),
            (ModContent.NPCType<AbomBoss>(), 22.9f)
        };
        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
            {
                CSEUtils.ChangeBossProgressions(changes.ToArray());
            }
        }
    }
}
