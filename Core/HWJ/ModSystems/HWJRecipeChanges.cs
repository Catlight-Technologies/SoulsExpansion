using ContinentOfJourney.Items;
using ContinentOfJourney.Items.Accessories;
using ContinentOfJourney.Items.Accessories.MeleeExpansion;
using ContinentOfJourney.Items.Accessories.SummonerRings;
using ContinentOfJourney.Items.Material;
using Fargowiltas.Content.Items.Tiles;
using FargowiltasSouls.Content.Items.Accessories.Eternity;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSE.Core.HWJ.ModSystems
{
    [ExtendsFromMod(ModCompatibility.Homeward.Name)]
    [JITWhenModsEnabled(ModCompatibility.Homeward.Name)]
    public class HWJRecipeChanges : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe.HasResult<FinalBar>())
                {
                    if (ModCompatibility.Thorium.Loaded && !recipe.HasIngredient(ModCompatibility.Thorium.Mod.Find<ModItem>("TerrariumCore")))
                    {
                        recipe.DisableRecipe();
                    }
                }

                if (recipe.HasResult(ItemID.LongRainbowTrailWings))
                {
                    recipe.DisableRecipe();
                }

                if (recipe.createItem.ModItem is BaseForce)
                {
                    if (!recipe.HasIngredient<SolarFlareScoria>())
                        recipe.AddIngredient<SolarFlareScoria>(4);
                }

                if (recipe.HasResult<Horizon>() && !recipe.HasTile<CrucibleCosmosSheet>())
                {
                    recipe.DisableRecipe();
                }

                if ((//universe
                    recipe.HasResult<ArchWizardsSoul>() ||
                    recipe.HasResult<BerserkerSoul>() ||
                    recipe.HasResult<SnipersSoul>() ||
                    recipe.HasResult<ConjuristsSoul>() ||

                    //dimensions
                    recipe.HasResult<SupersonicSoul>() ||
                    recipe.HasResult<FlightMasterySoul>() ||
                    recipe.HasResult<WorldShaperSoul>() ||
                    recipe.HasResult<TrawlerSoul>() ||
                    recipe.HasResult<ColossusSoul>()
                    ) && !recipe.HasIngredient<FinalBar>())
                {
                    recipe.AddIngredient<FinalBar>(5);
                }
                if (recipe.HasResult(ItemID.Zenith) && recipe.HasIngredient<EssenceofBright>())
                {
                    recipe.RemoveIngredient(ModContent.ItemType<EssenceofBright>());
                }

                if (!ModCompatibility.Calamity.Loaded)
                {
                    if ((recipe.HasResult<UniverseSoul>() || recipe.HasResult<TerrariaSoul>() || recipe.HasResult<MasochistSoul>() || recipe.HasResult<DimensionSoul>()) && !recipe.HasIngredient<EssenceofBright>())
                    {
                        recipe.AddIngredient<EssenceofBright>(5);
                    }
                }

                #region souls
                if (recipe.HasResult<ArchWizardsSoul>())
                {
                    //flower replacement
                    recipe.RemoveIngredient(ItemID.ManaCloak);
                    recipe.RemoveIngredient(ItemID.MagnetFlower);
                    recipe.RemoveIngredient(ItemID.ArcaneFlower);
                    recipe.RemoveRecipeGroup(RecipeGroup.recipeGroupIDs["FargowiltasSouls:AnyManaFlower"]);
                    recipe.AddIngredient<Starflower>();

                    //final bar weapon
                    recipe.AddIngredient<Blackout>();
                }
                if (recipe.HasResult<SnipersSoul>())
                {
                    //scope and replacement
                    recipe.RemoveIngredient(ItemID.SniperScope);
                    recipe.RemoveIngredient(ItemID.ReconScope);

                    recipe.RemoveIngredient(ItemID.MagicQuiver);
                    recipe.RemoveIngredient(ItemID.MoltenQuiver);
                    recipe.RemoveIngredient(ItemID.StalkersQuiver);

                    recipe.RemoveRecipeGroup(RecipeGroup.recipeGroupIDs["FargowiltasSouls:AnyQuiver"]);
                    recipe.RemoveRecipeGroup(RecipeGroup.recipeGroupIDs["FargowiltasSouls:AnySniperScope"]);
                    recipe.AddIngredient<CrossbowScope>();

                    //final bar weapon
                    recipe.AddIngredient<QuartzObliterator>();
                }
                if (recipe.HasResult<BerserkerSoul>())
                {
                    //gauntlet replacement
                    if (!ModCompatibility.Calamity.Loaded) {
                        recipe.RemoveIngredient(ItemID.FireGauntlet);
                        recipe.AddIngredient<DivineTouch>();
                    }

                    //final bar weapon
                    recipe.AddIngredient<FallingAction>();

                    //other
                    recipe.AddIngredient<PhilosophersStone>();
                }
                if (recipe.HasResult<ConjuristsSoul>())
                {
                    //final bar weapon
                    recipe.AddIngredient<PhantomStaff>();

                    //other
                    recipe.AddIngredient<CommandersGaunlet>();
                }
                if (recipe.HasResult<FlightMasterySoul>())
                {
                    //wings replacement
                    recipe.RemoveIngredient(ItemID.WingsNebula);
                    recipe.RemoveIngredient(ItemID.WingsStardust);
                    recipe.RemoveIngredient(ItemID.WingsSolar);
                    recipe.RemoveIngredient(ItemID.WingsVortex);
                    recipe.RemoveIngredient(1131);
                    recipe.RemoveIngredient(1871);
                    recipe.RemoveIngredient(822);
                    recipe.RemoveIngredient(821);
                    recipe.AddIngredient<Altitude>();
                }
                if (recipe.HasResult<WorldShaperSoul>())
                {
                    recipe.AddIngredient<TimelessMiner>();
                }
                if (recipe.HasResult<TrawlerSoul>())
                {
                    if (!ModCompatibility.SacredTools.Loaded)
                    {
                        recipe.AddIngredient(ItemID.CelestialShell);
                        recipe.AddIngredient<AncientBlessing>();
                    }
                }
                if (recipe.HasResult<ColossusSoul>())
                {
                    recipe.AddIngredient<MasterShield>();
                    if (!ModCompatibility.Calamity.Loaded && !ModCompatibility.SacredTools.Loaded)
                    {
                        recipe.RemoveIngredient(ItemID.AnkhShield);
                        recipe.AddIngredient<VanguardBreastpiece>();
                    }
                }
                #endregion
            }
        }
        public override void AddRecipes()
        {
            if (ModCompatibility.Thorium.Loaded)
            {
                Recipe.Create(ModContent.ItemType<FinalBar>())
                .AddIngredient(ModCompatibility.Thorium.Mod.Find<ModItem>("TerrariumCore"), 1)
                .AddIngredient<FinalOre>(7)
                .AddIngredient<EternalBar>()
                .AddIngredient<LivingBar>()
                .AddIngredient<CubistBar>()
                .Register();
            }
            if (!ModCompatibility.Thorium.Loaded && !ModCompatibility.SacredTools.Loaded)
            {
                Recipe.Create(ModContent.ItemType<Horizon>())
                .AddIngredient<AeolusBoots>()
                .AddIngredient<FinalBar>(2)
                .AddIngredient<TankOfThePastJungle>(8)
                .AddTile<CrucibleCosmosSheet>()
                .Register();
            }
            if (ModCompatibility.Thorium.Loaded && !ModCompatibility.SacredTools.Loaded)
            {
                Recipe.Create(ModContent.ItemType<Horizon>())
                .AddIngredient(ModCompatibility.Thorium.Mod.Find<ModItem>("TerrariumParticleSprinters"), 1)
                .AddIngredient<FinalBar>(2)
                .AddIngredient<TankOfThePastJungle>(8)
                .AddTile<CrucibleCosmosSheet>()
                .Register();
            }
            if (ModCompatibility.SacredTools.Loaded)
            {
                Recipe.Create(ModContent.ItemType<Horizon>())
                .AddIngredient(ModCompatibility.SacredTools.Mod.Find<ModItem>("VoidSpurs"), 1)
                .AddIngredient<FinalBar>(2)
                .AddIngredient<TankOfThePastJungle>(8)
                .AddTile<CrucibleCosmosSheet>()
                .Register();
            }
        }
    }
}