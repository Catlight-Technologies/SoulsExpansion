using CSE.Core;
using Fargowiltas.Content.Items.Tiles;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items.BossThePrimordials.Aqua;
using ThoriumMod.Items.BossThePrimordials.Omni;
using ThoriumMod.Items.BossThePrimordials.Slag;

namespace CSE.Content.Thorium.Materials
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class DreamEssence : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ModContent.ItemType<OceanEssence>());
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);

            recipe.AddIngredient<DeathEssence>();
            recipe.AddIngredient<InfernoEssence>();
            recipe.AddIngredient<OceanEssence>();

            recipe.AddTile<CrucibleCosmosSheet>();

            recipe.Register();
        }
    }

    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    internal class DreamEssenceReplacement : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];


                if (ModCompatibility.Calamity.Loaded)
                {
                    if (recipe.createItem == ModCompatibility.Calamity.Mod.Find<ModItem>("AuricBar").Item ||
                        recipe.createItem.type == ModContent.ItemType<DreamEssence>())
                    {
                        continue;
                    }
                }
                else
                {
                    if (recipe.createItem.type == ModContent.ItemType<DreamEssence>())
                    {
                        continue;
                    }
                }

                bool hasEssence = false;

                for (int j = 0; j < recipe.requiredItem.Count; j++)
                {
                    if (recipe.requiredItem[j].type == ModContent.ItemType<OceanEssence>() && recipe.HasIngredient<DeathEssence>() && recipe.HasIngredient<InfernoEssence>())
                    {
                        hasEssence = true;
                        break;
                    }
                }

                if (!hasEssence)
                    continue;

                recipe.requiredItem.RemoveAll(ingredient =>
                    ingredient.type == ModContent.ItemType<InfernoEssence>() ||
                    ingredient.type == ModContent.ItemType<DeathEssence>()
                );

                for (int j = 0; j < recipe.requiredItem.Count; j++)
                {
                    if (recipe.requiredItem[j].type == ModContent.ItemType<OceanEssence>())
                    {
                        int stack = recipe.requiredItem[j].stack;
                        recipe.requiredItem[j] = new Item();
                        recipe.requiredItem[j].SetDefaults(ModContent.ItemType<DreamEssence>());
                        recipe.requiredItem[j].stack = stack;
                    }
                }
            }
        }
    }
}