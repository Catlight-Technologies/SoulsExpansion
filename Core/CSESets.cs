using FargowiltasSouls.Content.Items.Weapons.FinalUpgrades;
using FargowiltasSouls.Content.Items.Weapons.SwarmDrops;
using FargowiltasSouls.Content.Patreon.DemonKing;
using FargowiltasSouls.Content.Patreon.DevAesthetic;
using FargowiltasSouls.Content.Patreon.Duck;
using FargowiltasSouls.Content.Patreon.GreatestKraken;
using FargowiltasSouls.Content.Patreon.Sasha;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CSE.Core
{
    public class CSESets : ModSystem
    {
        public static bool GetValue(bool[] set, int index) => set != null && set[index];
        public class Items
        {
            public static bool[] ChampionTierFargoWeapon;
            public static bool[] AbomTierFargoWeapon;
            public static bool[] MutantTierFargoWeapon;
            public static bool[] SiblingsTierFargoWeapon;

            public static bool[] HWJFinalBarWeapon;
        }

        public override void PostSetupContent()
        {
            SetFactory itemFactory = ItemID.Sets.Factory;

            Items.ChampionTierFargoWeapon = itemFactory.CreateBoolSet(false,
                ItemType<EaterLauncher>(),
                ItemType<Regurgitator>(),
                ItemType<HellZone>(),
                ItemType<LeashofCthulhu>(),
                ItemType<SlimeSlingingSlasher>(),
                ItemType<TheBigSting>(),
                ItemType<BigBrainBuster>(),
                ItemType<ScientificRailgun>(),
                ItemType<VortexMagnetRitual>(),
                ItemType<MissDrakovisFishingPole>(),
                ItemType<DeviousAestheticus>()
            );
            Items.AbomTierFargoWeapon = itemFactory.CreateBoolSet(false,
                ItemType<DragonsDemise>(),
                ItemType<DestructionCannon>(),
                ItemType<Landslide>(),
                ItemType<GeminiGlaives>(),
                ItemType<Blender>(),
                ItemType<DiffractorBlaster>(),
                ItemType<NukeFishron>(),
                ItemType<StaffOfUnleashedOcean>(),
                ItemType<TheDestroyer>(),
                ItemType<UmbraRegalia>()
            );
            Items.MutantTierFargoWeapon = itemFactory.CreateBoolSet(false,
                ItemType<GuardianTome>(),
                ItemType<TheBiggestSting>(),
                ItemType<SlimeRain>(),
                ItemType<PhantasmalLeashOfCthulhu>()
            );
            Items.SiblingsTierFargoWeapon = itemFactory.CreateBoolSet(false,
                ItemType<Penetrator>(),
                ItemType<SparklingLove>(),
                ItemType<StyxGazer>()
            );
        }
    }
}
