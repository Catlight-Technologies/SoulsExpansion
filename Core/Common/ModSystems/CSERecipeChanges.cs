using CSE.Content.Common.Accessories.Souls;
using CSE.Content.Common.CraftingStation;
using Fargowiltas.Content.Items.Tiles;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using Terraria;
using Terraria.ModLoader;

namespace CSE.Core.SoA.ModSystems
{
    public class CSERecipeChanges : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];
                if (recipe.HasResult(ModContent.ItemType<EternitySoul>()) && recipe.HasTile<CrucibleCosmosSheet>())
                {
                    recipe.RemoveTile(ModContent.TileType<CrucibleCosmosSheet>());
                    recipe.AddTile<MutantsForgeTile>();
                }
                if (ModCompatibility.Homeward.Loaded
                || ModCompatibility.Redemption.Loaded
                || ModCompatibility.Thorium.Loaded
                || ModCompatibility.Calamity.Loaded
                || ModCompatibility.SacredTools.Loaded)
                {
                    if (recipe.HasResult(ModContent.ItemType<EternitySoul>()))
                    {
                        recipe.RemoveIngredient(ModContent.ItemType<TerrariaSoul>());
                        recipe.AddIngredient<MacroverseSoul>();
                    }
                    if (recipe.HasResult(ModContent.ItemType<EternitySoul>()))
                    {
                        recipe.RemoveIngredient(ModContent.ItemType<MasochistSoul>());
                        recipe.AddIngredient<AccessoriesSoul>();
                    }
                }
            }
        }
    }
}