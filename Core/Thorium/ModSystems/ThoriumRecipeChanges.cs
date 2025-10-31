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

namespace CSE.Core.Thorium.ModSystems
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class ThoriumRecipeChanges : ModSystem
    {
        public override void AddRecipeGroups()
        {
            //evil wood tambourine
            RecipeGroup group = new RecipeGroup(() => Lang.misc[37] + " Evil Wood Tambourine", ModContent.ItemType<EbonWoodTambourine>(), ModContent.ItemType<ShadeWoodTambourine>());
            RecipeGroup.RegisterGroup("CSE:AnyTambourine", group);
            //bugle horn
            group = new RecipeGroup(() => Lang.misc[37] + " Bugle Horn", ModContent.ItemType<GoldBugleHorn>(), ModContent.ItemType<PlatinumBugleHorn>());
            RecipeGroup.RegisterGroup("CSE:AnyBugleHorn", group);
            //titan 
            group = new RecipeGroup(() => Lang.misc[37] + " Titan Headgear", ModContent.ItemType<TitanHelmet>(), ModContent.ItemType<TitanMask>(), ModContent.ItemType<TitanHeadgear>());
            RecipeGroup.RegisterGroup("CSE:AnyTitanHelmet", group);
            //any gem
            group = new RecipeGroup(() => Lang.misc[37] + " Gem", ModContent.ItemType<Opal>(), ModContent.ItemType<Aquamarine>());
            RecipeGroup.RegisterGroup("CSE:AnyThoriumGem", group);
            // rhapsodist
            group = new RecipeGroup(() => Lang.misc[37] + " Rhapsodist Helmet", ModContent.ItemType<SoloistHat>(), ModContent.ItemType<InspiratorsHelmet>());
            RecipeGroup.RegisterGroup("CSE:AnyRhapsodistHelmet", group);
            // tide turner
            group = new RecipeGroup(() => Lang.misc[37] + " Tide Turner Helmet", ModContent.ItemType<TideTurnerHelmet>(), ModContent.ItemType<TideTurnersGaze>());
            RecipeGroup.RegisterGroup("CSE:AnyTideTurnerHelmet", group);
            // dream weaver
            group = new RecipeGroup(() => Lang.misc[37] + " Dream Weaver Helmet", ModContent.ItemType<DreamWeaversHelmet>(), ModContent.ItemType<DreamWeaversHood>());
            RecipeGroup.RegisterGroup("CSE:AnyDreamWeaversHelmet", group);
            // assassin
            group = new RecipeGroup(() => Lang.misc[37] + " Assassin Helmet", ModContent.ItemType<MasterMarksmansScouter>(), ModContent.ItemType<MasterArbalestHood>());
            RecipeGroup.RegisterGroup("CSE:AnyAssassinHelmet", group);
            // pyromancer
            group = new RecipeGroup(() => Lang.misc[37] + " Pyromancer Helmet", ModContent.ItemType<PyromancerCowl>(), ModContent.ItemType<PyromancerTabard>());
            RecipeGroup.RegisterGroup("CSE:AnyPyromancerHelmet", group);
            // seraph idols
            group = new RecipeGroup(() => Lang.misc[37] + " Seraphim Idol Upgrade", ModContent.ItemType<ArchDemonCurse>(), ModContent.ItemType<ArchangelHeart>());
            RecipeGroup.RegisterGroup("CSE:AnyIdolUpgrade", group);
            // bard accessories
            group = new RecipeGroup(() => Lang.misc[37] + " Instrument Type Accessory", ModContent.ItemType<DigitalTuner>(), ModContent.ItemType<EpicMouthpiece>(), ModContent.ItemType<StraightMute>(), ModContent.ItemType<GuitarPickClaw>());
            RecipeGroup.RegisterGroup("CSE:AnyInstrumentTypeAccessory", group);
            // evil wings
            group = new RecipeGroup(() => Lang.misc[37] + " Evil Wings", ModContent.ItemType<DragonWings>(), ModContent.ItemType<FleshWings>());
            RecipeGroup.RegisterGroup("CSE:AnyEvilWings", group);
        }

        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<CoffinSummon>())
                .AddIngredient(ItemID.ClayBlock, 15)
                .AddIngredient(ItemID.FossilOre, 8)
                .AddRecipeGroup("CSE:AnyThoriumGem", 4)
                .AddTile(TileID.DemonAltar)
                .Register();
            Recipe.Create(ModContent.ItemType<HallowedPendant>())
                .AddIngredient<SweetVengeance>()
                .AddIngredient(ItemID.HallowedBar, 5)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            int depthsCrate = ModContent.ItemType<AquaticDepthsCrate>();
            int depthsCrateHm = ModContent.ItemType<AbyssalCrate>();

            //CSEUtils.CreateCrateRecipe(ModContent.ItemType<MagicConch>(), depthsCrate, 5, depthsCrateHm);
            CSEUtils.CreateCrateRecipe(ModContent.ItemType<SeaTurtlesBulwark>(), depthsCrate, 5, depthsCrateHm);
            CSEUtils.CreateCrateRecipe(ModContent.ItemType<AnglerBowl>(), depthsCrate, 5, depthsCrateHm);
            CSEUtils.CreateCrateRecipe(ModContent.ItemType<RainStone>(), depthsCrate, 5, depthsCrateHm);
            CSEUtils.CreateCrateRecipe(ModContent.ItemType<SteelDrum>(), depthsCrate, 5, depthsCrateHm);

            int scarletCrate = ModContent.ItemType<ScarletCrate>();
            int scarletCrateHM = ModContent.ItemType<SinisterCrate>();

            CSEUtils.CreateCrateRecipe(ModContent.ItemType<MixTape>(), scarletCrate, 5, scarletCrateHM);
            CSEUtils.CreateCrateRecipe(ModContent.ItemType<LootRang>(), scarletCrate, 5, scarletCrateHM);
            CSEUtils.CreateCrateRecipe(ModContent.ItemType<MagmaCharm>(), scarletCrate, 5, scarletCrateHM);
            CSEUtils.CreateCrateRecipe(ModContent.ItemType<SpringSteps>(), scarletCrate, 5, scarletCrateHM);
            CSEUtils.CreateCrateRecipe(ModContent.ItemType<DeepStaff>(), scarletCrate, 5, scarletCrateHM);
            CSEUtils.CreateCrateRecipe(ItemID.LavaCharm, scarletCrate, 5, scarletCrateHM);
            CSEUtils.CreateCrateRecipe(ModContent.ItemType<SpringHook>(), scarletCrate, 5, scarletCrateHM);
            CSEUtils.CreateCrateRecipe(ModContent.ItemType<MagmaLocket>(), scarletCrate, 5, scarletCrateHM);

            int strangeCrate = ModContent.ItemType<StrangeCrate>();
            int strangeCrateHM = ModContent.ItemType<WondrousCrate>();

            CSEUtils.CreateCrateRecipe(ModContent.ItemType<HightechSonarDevice>(), strangeCrate, 5, strangeCrateHM);
            CSEUtils.CreateCrateRecipe(ModContent.ItemType<DrownedDoubloon>(), strangeCrate, 5, strangeCrateHM);
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
                    recipe.AddIngredient<Phylactery>();

                    if (!ModCompatibility.Calamity.Loaded && !ModCompatibility.Homeward.Loaded && !ModCompatibility.SacredTools.Loaded)
                    {
                        recipe.RemoveIngredient(ModContent.ItemType<Devilshield>());
                        recipe.RemoveIngredient(ItemID.AnkhShield);
                        recipe.AddIngredient<TerrariumDefender>();
                    }
                }
                if (recipe.HasResult<SupersonicSoul>())
                {
                    recipe.RemoveIngredient(ItemID.SweetheartNecklace);
                    recipe.RemoveIngredient(ItemID.PanicNecklace);
                    if (!ModCompatibility.Homeward.Loaded && !ModCompatibility.SacredTools.Loaded)
                    {
                        recipe.RemoveIngredient(ModContent.ItemType<AeolusBoots>());
                        recipe.AddIngredient<TerrariumParticleSprinters>();
                    }
                }
                if (recipe.HasResult<FlightMasterySoul>() && !recipe.HasIngredient<TerrariumWings>())
                {
                    recipe.AddIngredient<FlightEnchant>();
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
                        recipe.RemoveIngredient(ModContent.ItemType<HolyAegis>());
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
                    recipe.AddIngredient(ModContent.ItemType<DeathEssence>(), 10);
                }
                if (recipe.HasResult<StyxLeggings>() && recipe.HasIngredient(547))
                {
                    recipe.RemoveIngredient(547);
                    recipe.AddIngredient(ModContent.ItemType<OceanEssence>(), 10);
                }
                if (recipe.HasResult<StyxChestplate>() && recipe.HasIngredient(548))
                {
                    recipe.RemoveIngredient(548);
                    recipe.AddIngredient(ModContent.ItemType<InfernoEssence>(), 10);
                }

                if (recipe.HasResult<GaiaHelmet>() && !recipe.HasIngredient<DarkMatter>())
                {
                    recipe.AddIngredient(ModContent.ItemType<HolyKnightsAlloy>(), 6);
                    recipe.AddIngredient(ModContent.ItemType<DarkMatter>(), 6);
                    recipe.AddIngredient(ModContent.ItemType<BloomWeave>(), 6);
                }
                if (recipe.HasResult<GaiaGreaves>() && !recipe.HasIngredient<DarkMatter>())
                {
                    recipe.AddIngredient(ModContent.ItemType<HolyKnightsAlloy>(), 6);
                    recipe.AddIngredient(ModContent.ItemType<DarkMatter>(), 6);
                    recipe.AddIngredient(ModContent.ItemType<BloomWeave>(), 6);
                }
                if (recipe.HasResult<GaiaPlate>() && !recipe.HasIngredient<DarkMatter>())
                {
                    recipe.AddIngredient(ModContent.ItemType<HolyKnightsAlloy>(), 9);
                    recipe.AddIngredient(ModContent.ItemType<DarkMatter>(), 9);
                    recipe.AddIngredient(ModContent.ItemType<BloomWeave>(), 9);
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