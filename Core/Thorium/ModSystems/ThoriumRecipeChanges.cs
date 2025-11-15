using FargowiltasSouls.Content.Items.Accessories.Souls;
using Terraria.ModLoader;
using Terraria;
using CSE.Content.Thorium.Materials;
using CSE.Content.Thorium.Accessories.Souls;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.BossThePrimordials.Omni;
using ThoriumMod.Items.BossThePrimordials.Rhapsodist;
using ThoriumMod.Items.Titan;
using FargowiltasSouls.Content.Items.Summons;
using Terraria.ID;
using ThoriumMod.Items.Misc;
using ThoriumMod.Items.Dragon;
using ThoriumMod.Items.Flesh;
using ThoriumMod.Items.Terrarium;
using FargowiltasSouls.Content.Items.Armor.Styx;
using ThoriumMod.Items.BossThePrimordials.Aqua;
using ThoriumMod.Items.BossThePrimordials.Slag;
using FargowiltasSouls.Content.Items.Armor.Gaia;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Items.ThrownItems;
using ThoriumMod.Items.Darksteel;
using ThoriumMod.Items.Depths;
using ThoriumMod.Items.Illumite;
using ThoriumMod.Items.Lodestone;
using ThoriumMod.Items.Sandstone;
using ThoriumMod.Items.Valadium;
using FargowiltasSouls.Content.Items.Accessories;
using ThoriumMod.Items.Donate;
using ThoriumMod.Items.Thorium;
using ThoriumMod.Items.TransformItems;
using ThoriumMod.Items.BasicAccessories;
using ThoriumMod.Items.MagicItems;
using CSE.Content.Thorium.Accessories.Enchantments;
using CSE.Content.Thorium.Accessories.Other;
using ThoriumMod.Items.SummonItems;
using ThoriumMod.Items.BossLich;
using FargowiltasSouls.Content.Items.Accessories.Eternity;
using ThoriumMod.Items.BossThePrimordials.Dream;
using static Terraria.ModLoader.ModContent;
using ThoriumMod.Items.BossTheGrandThunderBird;
using ThoriumMod.Items.BossViscount;
using ThoriumMod.Items.BossQueenJellyfish;
using ThoriumMod.Items.BossStarScouter;
using ThoriumMod.Items.BossBoreanStrider;
using ThoriumMod.Items.BossForgottenOne;
using ThoriumMod.Items.BossThePrimordials;

namespace CSE.Core.Thorium.ModSystems
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class ThoriumRecipeChanges : ModSystem
    {
        public override void AddRecipeGroups()
        {
            //evil wood tambourine
            RecipeGroup group = new RecipeGroup(() => Lang.misc[37] + " Evil Wood Tambourine", ItemType<EbonWoodTambourine>(), ItemType<ShadeWoodTambourine>());
            RecipeGroup.RegisterGroup("CSE:AnyTambourine", group);
            //bugle horn
            group = new RecipeGroup(() => Lang.misc[37] + " Bugle Horn", ItemType<GoldBugleHorn>(), ItemType<PlatinumBugleHorn>());
            RecipeGroup.RegisterGroup("CSE:AnyBugleHorn", group);
            //titan 
            group = new RecipeGroup(() => Lang.misc[37] + " Titan Headgear", ItemType<TitanHelmet>(), ItemType<TitanMask>(), ItemType<TitanHeadgear>());
            RecipeGroup.RegisterGroup("CSE:AnyTitanHelmet", group);
            //any gem
            group = new RecipeGroup(() => Lang.misc[37] + " Gem", ItemType<Opal>(), ItemType<Aquamarine>());
            RecipeGroup.RegisterGroup("CSE:AnyThoriumGem", group);
            // rhapsodist
            group = new RecipeGroup(() => Lang.misc[37] + " Rhapsodist Helmet", ItemType<SoloistHat>(), ItemType<InspiratorsHelmet>());
            RecipeGroup.RegisterGroup("CSE:AnyRhapsodistHelmet", group);
            // tide turner
            group = new RecipeGroup(() => Lang.misc[37] + " Tide Turner Helmet", ItemType<TideTurnerHelmet>(), ItemType<TideTurnersGaze>());
            RecipeGroup.RegisterGroup("CSE:AnyTideTurnerHelmet", group);
            // dream weaver
            group = new RecipeGroup(() => Lang.misc[37] + " Dream Weaver Helmet", ItemType<DreamWeaversHelmet>(), ItemType<DreamWeaversHood>());
            RecipeGroup.RegisterGroup("CSE:AnyDreamWeaversHelmet", group);
            // assassin
            group = new RecipeGroup(() => Lang.misc[37] + " Assassin Helmet", ItemType<MasterMarksmansScouter>(), ItemType<MasterArbalestHood>());
            RecipeGroup.RegisterGroup("CSE:AnyAssassinHelmet", group);
            // pyromancer
            group = new RecipeGroup(() => Lang.misc[37] + " Pyromancer Helmet", ItemType<PyromancerCowl>(), ItemType<PyromancerTabard>());
            RecipeGroup.RegisterGroup("CSE:AnyPyromancerHelmet", group);
            // seraph idols
            group = new RecipeGroup(() => Lang.misc[37] + " Seraphim Idol Upgrade", ItemType<ArchDemonCurse>(), ItemType<ArchangelHeart>());
            RecipeGroup.RegisterGroup("CSE:AnyIdolUpgrade", group);
            // bard accessories
            group = new RecipeGroup(() => Lang.misc[37] + " Instrument Type Accessory", ItemType<DigitalTuner>(), ItemType<EpicMouthpiece>(), ItemType<StraightMute>(), ItemType<GuitarPickClaw>());
            RecipeGroup.RegisterGroup("CSE:AnyInstrumentTypeAccessory", group);
            // evil wings
            group = new RecipeGroup(() => Lang.misc[37] + " Evil Wings", ItemType<DragonWings>(), ItemType<FleshWings>());
            RecipeGroup.RegisterGroup("CSE:AnyEvilWings", group);
            //jester mask
            group = new RecipeGroup(() => Lang.misc[37] + " Evil Wood Tambourine", ItemType<EbonWoodTambourine>(), ItemType<ShadeWoodTambourine>());
            RecipeGroup.RegisterGroup("CSE:AnyJesterMask", group);
            //jester shirt
            group = new RecipeGroup(() => Lang.misc[37] + " Jester Shirt", ItemType<JestersShirt>(), ItemType<JestersShirt2>());
            RecipeGroup.RegisterGroup("CSE:AnyJesterShirt", group);
            //jester legging
            group = new RecipeGroup(() => Lang.misc[37] + " Jester Leggings", ItemType<JestersLeggings>(), ItemType<JestersLeggings2>());
            RecipeGroup.RegisterGroup("CSE:AnyJesterLeggings", group);
            //any letter
            group = new RecipeGroup(() => Lang.misc[37] + " Fan Letter", ItemType<FanLetter>(), ItemType<FanLetter2>());
            RecipeGroup.RegisterGroup("CSE:AnyLetter", group);
        }

        public override void AddRecipes()
        {
            Recipe.Create(ItemType<CoffinSummon>())
                .AddIngredient(ItemID.ClayBlock, 15)
                .AddIngredient(ItemID.FossilOre, 8)
                .AddRecipeGroup("CSE:AnyThoriumGem", 4)
                .AddTile(TileID.DemonAltar)
                .Register();
            Recipe.Create(ItemType<HallowedPendant>())
                .AddIngredient<SweetVengeance>()
                .AddIngredient(ItemID.HallowedBar, 5)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
            Recipe.Create(ItemType<DraculaFang>(), 300)
                .AddIngredient<ViscountTreasureBag>()
                .AddTile(TileID.Solidifier)
                .Register();

            #region crates
            int depthsCrate = ItemType<AquaticDepthsCrate>();
            int depthsCrateHm = ItemType<AbyssalCrate>();

            //CSEUtils.CreateCrateRecipe(ItemType<MagicConch>(), depthsCrate, 5, depthsCrateHm);
            CSEUtils.CreateCrateRecipe(ItemType<SeaTurtlesBulwark>(), depthsCrate, 5, depthsCrateHm);
            CSEUtils.CreateCrateRecipe(ItemType<AnglerBowl>(), depthsCrate, 5, depthsCrateHm);
            CSEUtils.CreateCrateRecipe(ItemType<RainStone>(), depthsCrate, 5, depthsCrateHm);
            CSEUtils.CreateCrateRecipe(ItemType<SteelDrum>(), depthsCrate, 5, depthsCrateHm);

            int scarletCrate = ItemType<ScarletCrate>();
            int scarletCrateHM = ItemType<SinisterCrate>();

            CSEUtils.CreateCrateRecipe(ItemType<MixTape>(), scarletCrate, 5, scarletCrateHM);
            CSEUtils.CreateCrateRecipe(ItemType<LootRang>(), scarletCrate, 5, scarletCrateHM);
            CSEUtils.CreateCrateRecipe(ItemType<MagmaCharm>(), scarletCrate, 5, scarletCrateHM);
            CSEUtils.CreateCrateRecipe(ItemType<SpringSteps>(), scarletCrate, 5, scarletCrateHM);
            CSEUtils.CreateCrateRecipe(ItemType<DeepStaff>(), scarletCrate, 5, scarletCrateHM);
            CSEUtils.CreateCrateRecipe(ItemID.LavaCharm, scarletCrate, 5, scarletCrateHM);
            CSEUtils.CreateCrateRecipe(ItemType<SpringHook>(), scarletCrate, 5, scarletCrateHM);
            CSEUtils.CreateCrateRecipe(ItemType<MagmaLocket>(), scarletCrate, 5, scarletCrateHM);

            int strangeCrate = ItemType<StrangeCrate>();
            int strangeCrateHM = ItemType<WondrousCrate>();

            CSEUtils.CreateCrateRecipe(ItemType<HightechSonarDevice>(), strangeCrate, 5, strangeCrateHM);
            CSEUtils.CreateCrateRecipe(ItemType<DrownedDoubloon>(), strangeCrate, 5, strangeCrateHM);
            #endregion

            #region boss bags
            CSEUtils.CreateBagRecipes(ItemType<TheGrandThunderBirdTreasureBag>(),
            [
                ItemType<ThunderTalon>(),
                ItemType<StormHatchlingStaff>(),
                ItemType<Didgeridoo>(),
                ItemType<TalonBurst>(),
            ]);
            CSEUtils.CreateBagRecipes(ItemType<ViscountTreasureBag>(),
            [
                ItemType<BatWing>(),
                ItemType<GuanoGunner>(),
                ItemType<VampireScepter>(),
                ItemType<ViscountCane>(),
                ItemType<BatScythe>(),
                ItemType<SonarCannon>(),
            ]);
            CSEUtils.CreateBagRecipes(ItemType<QueenJellyfishTreasureBag>(),
            [
                ItemType<SparkingJellyBall>(),
                ItemType<BuccaneerBlunderBuss>(),
                ItemType<GiantGlowstick>(),
                ItemType<JellyPondWand>(),
                ItemType<ConchShell>(),
            ]);
            CSEUtils.CreateBagRecipes(ItemType<StarScouterTreasureBag>(),
            [
                ItemType<StarTrail>(),
                ItemType<ParticleWhip>(),
                ItemType<HitScanner>(),
                ItemType<GaussFlinger>(),
                ItemType<DistressCaller>(),
                ItemType<StarRod>(),
                ItemType<Roboboe>(),
            ]);
            CSEUtils.CreateBagRecipes(ItemType<BoreanStriderTreasureBag>(),
            [
                ItemType<GlacialSting>(),
                ItemType<Glacier>(),
                ItemType<BoreanFangStaff>(),
                ItemType<FreezeRay>(),
                ItemType<TheCryoFang>(),
            ]);
            CSEUtils.CreateBagRecipes(ItemType<BoreanStriderTreasureBag>(),
            [
                ItemType<GlacialSting>(),
                ItemType<Glacier>(),
                ItemType<BoreanFangStaff>(),
                ItemType<FreezeRay>(),
                ItemType<TheCryoFang>(),
            ]);
            CSEUtils.CreateBagRecipes(ItemType<LichTreasureBag>(),
            [
                ItemType<SoulRender>(),
                ItemType<WitherStaff>(),
                ItemType<SoulBomb>(),
                ItemType<SoulCleaver>(),
                ItemType<CadaverCornet>(),
                ItemType<PhantomWand>(),
            ]);
            CSEUtils.CreateBagRecipes(ItemType<ForgottenOneTreasureBag>(),
            [
                ItemType<TheIncubator>(),
                ItemType<MantisShrimpPunch>(),
                ItemType<TrenchSpitter>(),
                ItemType<OldGodsVision>(),
                ItemType<SirensLyre>(),
                ItemType<RlyehLostRod>(),
                ItemType<WhisperingHood>(),
                ItemType<WhisperingLeggings>(),
                ItemType<WhisperingTabard>(),
            ]);
            CSEUtils.CreateBagRecipes(ItemType<ThePrimordialsTreasureBag>(),
            [
                ItemType<DormantHammer>(),
            ]);
            #endregion

        }
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                #region souls
                if (recipe.HasResult<BerserkerSoul>())
                {
                    recipe.AddIngredient<BlizzardPouch>();

                    if (ModCompatibility.Calamity.Loaded) { recipe.AddIngredient<TerrariansLastKnife>(); }
                    recipe.AddIngredient<TerrariumSaber>();
                }
                if (recipe.HasResult<SnipersSoul>())
                {
                    recipe.AddIngredient<ConcussiveWarhead>();

                    if (ModCompatibility.Calamity.Loaded) { recipe.AddIngredient<QuasarsFlare>(); }
                    recipe.AddIngredient<TerrariumPulseRifle>();
                }
                if (recipe.HasResult<ArchWizardsSoul>())
                {
                    recipe.AddIngredient<MurkyCatalyst>();

                    if (ModCompatibility.Calamity.Loaded) { recipe.AddIngredient<NorthernLight>(); }
                    recipe.AddIngredient<TerrariumSageStaff>();
                }
                if (recipe.HasResult<ConjuristsSoul>())
                {
                    recipe.AddIngredient<NecroticSkull>();
                    if (recipe.HasIngredient(ItemID.PygmyNecklace)){recipe.RemoveIngredient(ItemID.PygmyNecklace);}

                    recipe.AddIngredient<TerrariumEnigmaStaff>();
                }
                if (recipe.HasResult<TrawlerSoul>())
                {
                    recipe.RemoveIngredient(ItemID.SporeSac);
                    recipe.AddIngredient<HeartOfTheJungle>();
                    if (!ModCompatibility.SacredTools.Loaded)
                    {
                        recipe.AddIngredient<GreedyGoblet>();
                        recipe.RemoveIngredient(ItemID.GreedyRing);
                    }
                }
                if (recipe.HasResult<WorldShaperSoul>())
                {
                    recipe.AddIngredient<GeodeEnchant>();
                }
                if (recipe.HasResult<ColossusSoul>())
                {
                    recipe.RemoveIngredient(ItemID.AnkhShield);
                    recipe.AddIngredient<Phylactery>();

                    if (!ModCompatibility.Calamity.Loaded && !ModCompatibility.Homeward.Loaded && !ModCompatibility.SacredTools.Loaded)
                    {
                        recipe.RemoveIngredient(ItemType<Devilshield>());
                        recipe.AddIngredient<TerrariumDefender>();
                    }
                }
                if (recipe.HasResult<SupersonicSoul>())
                {
                    recipe.RemoveIngredient(ItemID.SweetheartNecklace);
                    recipe.RemoveIngredient(ItemID.PanicNecklace);
                    if (!ModCompatibility.Homeward.Loaded && !ModCompatibility.SacredTools.Loaded)
                    {
                        recipe.RemoveIngredient(ItemType<AeolusBoots>());
                        recipe.AddIngredient<TerrariumParticleSprinters>();
                    }
                }
                if (recipe.HasResult<FlightMasterySoul>() && !recipe.HasIngredient<TerrariumWings>())
                {
                    //recipe.AddIngredient<FlightEnchant>();
                    recipe.AddIngredient<TerrariumWings>();
                    recipe.AddRecipeGroup("CSE:AnyEvilWings");
                }
                #endregion

                if (recipe.HasResult<TerrariumDefender>())
                {
                    recipe.AddIngredient<CorruptedWarShield>();
                    if (!ModCompatibility.Calamity.Loaded && !ModCompatibility.Homeward.Loaded && !ModCompatibility.SacredTools.Loaded)
                    {
                        recipe.AddIngredient<Devilshield>();
                        recipe.RemoveIngredient(ItemType<HolyAegis>());
                        recipe.RemoveIngredient(ItemID.FrozenTurtleShell);
                    }
                }

                if (recipe.HasResult<TerrariumParticleSprinters>())
                {
                    recipe.AddIngredient<AeolusBoots>();
                    recipe.RemoveIngredient(ItemID.TerrasparkBoots);
                }

                if ((recipe.HasResult<BerserkerSoul>()
                    || recipe.HasResult<ArchWizardsSoul>()
                    || recipe.HasResult<SnipersSoul>()
                    || recipe.HasResult<ConjuristsSoul>()

                    || recipe.HasResult<ColossusSoul>()

                    //you NEED them in post ml
                    //|| recipe.HasResult<SupersonicSoul>()
                    || recipe.HasResult<FlightMasterySoul>() //well maybe not this one

                    //they do not benefit in combat butt.... uhhhh...
                    || recipe.HasResult<TrawlerSoul>()
                    || recipe.HasResult<WorldShaperSoul>()
                    ) && !recipe.HasIngredient<DreamEssence>())
                {
                    recipe.AddIngredient<DreamEssence>(5);
                }

                if (recipe.HasResult<UniverseSoul>())
                {
                    recipe.AddIngredient<BardSoul>();
                    recipe.AddIngredient<GuardianAngelsSoul>();
                }

                if (recipe.HasResult<AbomsCurse>())
                {
                    recipe.AddIngredient<DreamEssence>(2);
                }

                if (recipe.HasResult<HallowedPendant>() && !recipe.HasIngredient<SweetVengeance>())
                {
                    recipe.DisableRecipe();
                }

                if (recipe.HasResult<StyxCrown>() && recipe.HasIngredient(549))
                {
                    recipe.RemoveIngredient(549);
                    recipe.AddIngredient(ItemType<DeathEssence>(), 10);
                }
                if (recipe.HasResult<StyxLeggings>() && recipe.HasIngredient(547))
                {
                    recipe.RemoveIngredient(547);
                    recipe.AddIngredient(ItemType<OceanEssence>(), 10);
                }
                if (recipe.HasResult<StyxChestplate>() && recipe.HasIngredient(548))
                {
                    recipe.RemoveIngredient(548);
                    recipe.AddIngredient(ItemType<InfernoEssence>(), 10);
                }

                if (recipe.HasResult<GaiaHelmet>() && !recipe.HasIngredient<DarkMatter>())
                {
                    recipe.AddIngredient(ItemType<HolyKnightsAlloy>(), 6);
                    recipe.AddIngredient(ItemType<DarkMatter>(), 6);
                    recipe.AddIngredient(ItemType<BloomWeave>(), 6);
                }
                if (recipe.HasResult<GaiaGreaves>() && !recipe.HasIngredient<DarkMatter>())
                {
                    recipe.AddIngredient(ItemType<HolyKnightsAlloy>(), 6);
                    recipe.AddIngredient(ItemType<DarkMatter>(), 6);
                    recipe.AddIngredient(ItemType<BloomWeave>(), 6);
                }
                if (recipe.HasResult<GaiaPlate>() && !recipe.HasIngredient<DarkMatter>())
                {
                    recipe.AddIngredient(ItemType<HolyKnightsAlloy>(), 9);
                    recipe.AddIngredient(ItemType<DarkMatter>(), 9);
                    recipe.AddIngredient(ItemType<BloomWeave>(), 9);
                }

                if (recipe.HasResult(ItemID.DrillContainmentUnit) && !recipe.HasIngredient<TerrariumCore>())
                {
                    recipe.RemoveIngredient(ItemID.MeteoriteBar);
                    recipe.RemoveIngredient(ItemID.HellstoneBar);
                    recipe.RemoveIngredient(ItemID.ShroomiteBar);
                    recipe.RemoveIngredient(ItemID.SpectreBar);
                    recipe.RemoveIngredient(ItemID.ChlorophyteBar);
                    recipe.AddIngredient<TerrariumCore>(40);
                    recipe.AddIngredient<TitanicBar>(40);
                    recipe.AddIngredient<SandstoneIngot>(40);
                    recipe.AddIngredient<aDarksteelAlloy>(40);
                    recipe.AddIngredient<AquaiteBar>(40);
                    recipe.AddIngredient<LodeStoneIngot>(40);
                    recipe.AddIngredient<IllumiteIngot>(40);
                    recipe.AddIngredient<ValadiumIngot>(40);
                }
            }
        }
    }
}