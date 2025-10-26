using SacredTools.NPCs.Boss.Obelisk.Nihilus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using static CSE.Core.Common.Globals.CSEPointsBalanceNPC;

namespace CSE.Core.Crossmod.ModSystems
{
    [ExtendsFromMod(ModCompatibility.Crossmod.Name)]
    [JITWhenModsEnabled(ModCompatibility.Crossmod.Name)]
    public class CrossmodPostSetupContent : ModSystem
    {
        public override void Load()
        {
            //if (ModLoader.HasMod("NoxusPort"))
            //{
            //    AddBossConfig(
            //        bossType: ModLoader.GetMod("NoxusPort").Find<ModNPC>("EntropicGod").Type,
            //        affectingMods: new List<ModMultiplier>
            //        {
            //            new ModMultiplier { ModName = "FargowiltasSouls", DamageMultiplier = 0f, HealthMultiplier = 0.5f },
            //            //new ModMultiplier { ModName = "SacredTools", DamageMultiplier = 0.25f, HealthMultiplier = 0.5f },
            //            //new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.25f, HealthMultiplier = 0.75f },
            //            //new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.25f, HealthMultiplier = 0.25f }
            //        }
            //    );
            //    AddBossConfig(
            //        bossType: ModLoader.GetMod("NoxusPort").Find<ModNPC>("NoxusEgg").Type,
            //        affectingMods: new List<ModMultiplier>
            //        {
            //            new ModMultiplier { ModName = "FargowiltasSouls", DamageMultiplier = 0f, HealthMultiplier = 0.5f },
            //            //new ModMultiplier { ModName = "SacredTools", DamageMultiplier = 0.25f, HealthMultiplier = 0.5f },
            //            //new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.25f, HealthMultiplier = 0.75f },
            //            //new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.25f, HealthMultiplier = 0.25f }
            //        }
            //    );
            //}
        }
    }
}
