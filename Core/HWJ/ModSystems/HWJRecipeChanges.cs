using ContinentOfJourney.Items.Accessories;
using ContinentOfJourney.Items.Material;
using Fargowiltas.Content.Items.Tiles;
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

                if ((recipe.HasResult<BerserkerSoul>() 
                    || recipe.HasResult<ArchWizardsSoul>()
                    || recipe.HasResult<SnipersSoul>()
                    || recipe.HasResult<ConjuristsSoul>()
                    || recipe.HasResult<TrawlerSoul>()
                    || recipe.HasResult<WorldShaperSoul>()
                    || recipe.HasResult<ColossusSoul>()
                    || recipe.HasResult<SupersonicSoul>()
                    ) && !recipe.HasIngredient<FinalBar>())
                {
                    recipe.AddIngredient<FinalBar>(5);
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

                if (recipe.HasResult<FlightMasterySoul>() && !recipe.HasIngredient<Altitude>())
                {
                    recipe.RemoveIngredient(1131);
                    recipe.RemoveIngredient(1871);
                    recipe.RemoveIngredient(822);
                    recipe.RemoveIngredient(821);
                    recipe.AddIngredient<Altitude>();
                    recipe.AddIngredient<FinalBar>(5);
                }
                if ((recipe.HasResult<ColossusSoul>() ||
                    recipe.HasResult<ArchWizardsSoul>() ||
                    recipe.HasResult<BerserkerSoul>() ||
                    recipe.HasResult<SnipersSoul>() ||
                    recipe.HasResult<ConjuristsSoul>()
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
        }
    }
}