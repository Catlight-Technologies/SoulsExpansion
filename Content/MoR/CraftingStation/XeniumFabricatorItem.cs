using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria;
using Redemption.Items.Materials.PostML;
using Redemption.Items.Placeable.Furniture.Lab;
using Redemption.Items.Placeable.Furniture.SlayerShip;
using Redemption.Items.Placeable.Furniture.Misc;
using CSE.Core;
using Fargowiltas.Content.Items.Tiles;

namespace CSE.Content.MoR.CraftingStation
{
    [ExtendsFromMod(ModCompatibility.Redemption.Name)]
    [JITWhenModsEnabled(ModCompatibility.Redemption.Name)]
    public class XeniumFabricatorItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemType<CrucibleCosmos>());
            Item.value = Item.buyPrice(1, 0, 0, 0);
            Item.createTile = TileType<XeniumFabricatorTile>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<XeniumSmelter>());
            recipe.AddIngredient(ItemType<XeniumRefinery>());
            recipe.AddIngredient(ItemType<SlayerFabricator>());
            recipe.AddIngredient(ItemType<GathicCryoFurnace>());
            recipe.AddIngredient(ItemType<GirusCorruptor>());
            recipe.AddIngredient(ItemType<EnergyStation>());
            recipe.AddIngredient(ItemType<LifeFragment>(), 15);
            recipe.Register();
        }
    }
}
