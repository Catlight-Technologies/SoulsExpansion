using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using CSE.Core;

namespace CSE.Content.Thorium.Forces
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class SvartalfheimForce : BaseForce
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = 11;
            Item.value = 600000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
        }

        //public override void AddRecipes()
        //{
        //    Recipe recipe = this.CreateRecipe();

        //    recipe.AddIngredient(null, "GraniteEnchant");
        //    recipe.AddIngredient(null, "YewWoodEnchant");
        //    recipe.AddIngredient(null, "JesterEnchant");
        //    recipe.AddIngredient(null, "ConduitEnchant");
        //    recipe.AddIngredient(null, "TitanEnchant");
        //    recipe.AddIngredient(null, "AstroEnchant");
        //    recipe.AddIngredient(null, "DarksteelEnchant");

        //    recipe.AddTile<CrucibleCosmosSheet>();

        //    recipe.Register();
        //}
    }
}
