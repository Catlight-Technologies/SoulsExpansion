using ContinentOfJourney.NPCs.Boss_TheSon;
using System.Collections.Generic;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static CSE.Core.Common.Globals.CSEPointsBalanceNPC;
using Terraria.ID;
using static CSE.Core.CSESets;
using ContinentOfJourney.Items;
using ContinentOfJourney.Items.FielderSentries;
using ContinentOfJourney.Items.Flamethrowers;
using ContinentOfJourney.Items.Rockets;
using ContinentOfJourney.Items.ThrowerWeapons;
using ContinentOfJourney.Items.Whips;

namespace CSE.Core.HWJ.ModSystems
{
    [ExtendsFromMod(ModCompatibility.Homeward.Name)]
    [JITWhenModsEnabled(ModCompatibility.Homeward.Name)]
    public class HWJPostSetupContent : ModSystem
    {
        public override void Load()
        {
            AddBossConfig(
                bossType: NPCType<TheSon>(),
                affectingMods: new List<ModMultiplier>
                {
                new ModMultiplier { ModName = "CalamityMod", DamageMultiplier = 0.3f, HealthMultiplier = 1f },
                new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f },
                new ModMultiplier { ModName = "FargowiltasSouls", DamageMultiplier = 0.1f, HealthMultiplier = 2f },
                new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.1f, HealthMultiplier = 0.5f }
                }
            );
        }

        public override void PostSetupContent()
        {
            SetFactory itemFactory = ItemID.Sets.Factory;

            Items.ChampionTierFargoWeapon = itemFactory.CreateBoolSet(false,
                ItemType<Blackout>(),
                ItemType<Climax>(),
                ItemType<CosmicBoardsword>(),
                ItemType<DoublePlot>(),
                ItemType<FallingAction>(),
                ItemType<RisingAction>(),
                ItemType<PhantomStaff>(),
                ItemType<StaffDramaticIrony>(),
                ItemType<FT13Phlogistinator>(),
                ItemType<LandOfTheLustrous>(),
                ItemType<ShieldMachine>(),
                ItemType<SpearOfEscape>(),
                ItemType<ItemFragGrenade>(),
                ItemType<IncitingIncident>(),
                ItemType<QuartzObliterator>()
            );
        }
    }
}
