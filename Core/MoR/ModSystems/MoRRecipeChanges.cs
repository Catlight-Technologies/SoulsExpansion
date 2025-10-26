using Fargowiltas.Content.Items.Tiles;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using Redemption.Items.Accessories.PostML;
using Redemption.Items.Materials.PostML;
using Redemption.Items.Materials.PreHM;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSE.Core.MoR.ModSystems
{
    [ExtendsFromMod(ModCompatibility.Redemption.Name)]
    [JITWhenModsEnabled(ModCompatibility.Redemption.Name)]
    public class MoRRecipeChanges : ModSystem
    {
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<PureIronAlloy>())
                .AddIngredient<DragonLeadAlloy>()
                .AddTile<CrucibleCosmosSheet>()
                .Register();
            Recipe.Create(ModContent.ItemType<DragonLeadAlloy>())
                .AddIngredient<PureIronAlloy>()
                .AddTile<CrucibleCosmosSheet>()
                .Register();
        }

        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];
                if (recipe.createItem.ModItem is BaseForce)
                {
                    if (!recipe.HasIngredient<RoboBrain>())
                        recipe.AddIngredient<RoboBrain>();
                }

                if (recipe.HasResult(ItemID.Zenith) && recipe.HasIngredient<LifeFragment>())
                {
                    recipe.RemoveIngredient(ModContent.ItemType<LifeFragment>());
                }

                if (!ModCompatibility.Calamity.Loaded)
                {
                    if ((recipe.HasResult<UniverseSoul>() || recipe.HasResult<TerrariaSoul>() || recipe.HasResult<MasochistSoul>() || recipe.HasResult<DimensionSoul>()) && !recipe.HasIngredient<LifeFragment>())
                    {
                        recipe.AddIngredient<LifeFragment>(5);
                    }
                }
            }
        }
    }
}