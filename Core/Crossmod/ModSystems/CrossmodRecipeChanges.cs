using CalamityMod.Items.Materials;
using CalamityMod.Items;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using FargowiltasSouls.Content.Items.Ammos;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CSE.Content.Common.CraftingStation;
using CSE.Content.Common.Materials;
using FargowiltasSouls.Content.Items.Summons;
using FargowiltasCrossmod.Content.Calamity.Items.Accessories;
using FargowiltasSouls.Content.Items.Armor.Styx;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Accessories;
using FargowiltasCrossmod.Content.Calamity.Items.Accessories.Souls;

namespace CSE.Core.Crossmod.ModSystems
{
    [ExtendsFromMod(ModCompatibility.Crossmod.Name)]
    [JITWhenModsEnabled(ModCompatibility.Crossmod.Name)]
    public class CrossmodRecipeChanges : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe.HasResult(ModContent.ItemType<EternitySoul>()) && recipe.HasTile<DraedonsForge>())
                {
                    recipe.RemoveTile(ModContent.TileType<DraedonsForge>());
                    recipe.AddTile<MutantsForgeTile>();
                }

                if (!recipe.HasIngredient<ShadowspecBar>() && recipe.HasIngredient<AshesofAnnihilation>())
                {
                    if (recipe.HasResult<UniverseSoul>() || recipe.HasResult<TerrariaSoul>() || recipe.HasResult<MasochistSoul>() || recipe.HasResult<DimensionSoul>())
                    {
                        recipe.RemoveIngredient(ModContent.ItemType<AbomEnergy>());
                        recipe.RemoveIngredient(ModContent.ItemType<ExoPrism>());
                        recipe.RemoveIngredient(ModContent.ItemType<AshesofAnnihilation>());
                        recipe.AddIngredient<ShadowspecBar>(5);
                        recipe.AddIngredient<MiracleMatter>();
                    }
                }

                if(recipe.HasIngredient<BrandoftheBrimstoneWitch>() && recipe.HasResult<EternitySoul>())
                {
                    recipe.RemoveIngredient(ModContent.ItemType<BrandoftheBrimstoneWitch>());
                }

                if (recipe.HasResult<ElementalGauntlet>())
                {
                    if (ModCompatibility.SacredTools.Loaded && !ModCompatibility.Homeward.Loaded)
                    {
                        recipe.RemoveIngredient(ItemID.FireGauntlet);
                        recipe.AddIngredient(ModCompatibility.SacredTools.Mod.Find<ModItem>("FloraFist"));
                    }
                    else if (ModCompatibility.Homeward.Loaded)
                    {
                        recipe.RemoveIngredient(ItemID.FireGauntlet);
                        recipe.AddIngredient(ModCompatibility.Homeward.Mod.Find<ModItem>("DivineTouch"));
                    }
                }

                if (recipe.HasResult<TracersCelestial>())
                {
                    recipe.DisableRecipe();
                }

                if (recipe.HasResult<VagabondsSoul>())
                {
                    recipe.DisableRecipe();
                }

                if (recipe.HasIngredient<VagabondsSoul>())
                {
                    recipe.RemoveIngredient(ModContent.ItemType<VagabondsSoul>());
                }

                if (recipe.HasResult<AbominationnVoodooDoll>() && !recipe.HasIngredient<ShadowspecBar>())
                {
                    recipe.RemoveIngredient(ModContent.ItemType<AbomEnergy>());
                    recipe.AddIngredient<ShadowspecBar>(5);
                }

                if (recipe.HasResult<MiracleMatter>())
                {
                    if (ModCompatibility.Homeward.Loaded && !recipe.HasIngredient(ModCompatibility.Homeward.Mod.Find<ModItem>("FinalBar")))
                    {
                        recipe.AddIngredient(ModCompatibility.Homeward.Mod.Find<ModItem>("FinalBar"), 3);
                    }
                    if (ModCompatibility.Thorium.Loaded && !ModCompatibility.Homeward.Loaded && !!recipe.HasIngredient(ModCompatibility.Thorium.Mod.Find<ModItem>("TerrariumCore")))
                    {
                        recipe.AddIngredient(ModCompatibility.Thorium.Mod.Find<ModItem>("TerrariumCore"), 3);
                    }
                }

                if (recipe.HasResult(ModContent.ItemType<ShadowspecBar>()))
                {
                    if (ModCompatibility.Homeward.Loaded)
                    {
                        recipe.AddIngredient(ModCompatibility.Homeward.Mod.Find<ModItem>("EssenceofBright"), 1);
                    }
                    if (ModCompatibility.Redemption.Loaded)
                    {
                        recipe.AddIngredient(ModCompatibility.Redemption.Mod.Find<ModItem>("LifeFragment"), 1);
                    }
                    //if (ModCompatibility.Thorium.Loaded)
                    //{
                    //    recipe.AddIngredient(Mod.Find<ModItem>("DreamEssence"), 1);
                    //}

                    recipe.AddIngredient<AbomEnergy>();
                    recipe.RemoveIngredient(ModContent.ItemType<EternalEnergy>());
                }

                if (ModLoader.HasMod("NoxusBoss"))
                {
                    if (recipe.HasResult(ModContent.ItemType<AbominationnVoodooDoll>()))
                    {
                        recipe.AddIngredient(ModContent.ItemType<Rock>(), 1);
                    }
                }

                if (recipe.HasResult(ModContent.ItemType<ShadowspecBar>()) && recipe.HasIngredient<tModLoadiumBar>())
                {
                    recipe.RemoveIngredient(ModContent.ItemType<tModLoadiumBar>());
                }

                if (recipe.HasResult<FargoArrow>() || recipe.HasResult<FargoBullet>())
                {
                    recipe.RemoveIngredient(ModContent.ItemType<Rock>());
                }

                if (recipe.HasResult<StyxChestplate>() || recipe.HasResult<StyxCrown>() || recipe.HasResult<StyxLeggings>())
                {
                    recipe.AddIngredient<AuricBar>(5);
                    recipe.RemoveIngredient(ItemID.LunarBar);
                }

                if (recipe.HasResult(ItemID.DrillContainmentUnit) && !recipe.HasIngredient<AerialiteBar>())
                {
                    recipe.AddIngredient<LifeAlloy>(20);
                    recipe.AddIngredient<AerialiteBar>(20);
                    recipe.AddIngredient<AstralBar>(20);
                }
            }
        }
    }
}