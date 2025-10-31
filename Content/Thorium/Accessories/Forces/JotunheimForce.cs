using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using CSE.Core;

namespace CSE.Content.Thorium.Forces
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class JotunheimForce : BaseForce
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

        //    recipe.AddIngredient(ModContent.ItemType<WhisperingEnchant>());
        //    recipe.AddIngredient(ModContent.ItemType<DepthDiverEnchant>());
        //    recipe.AddIngredient(ModContent.ItemType<TideHunterEnchant>());
        //    recipe.AddIngredient(ModContent.ItemType<NagaSkinEnchant>());
        //    recipe.AddIngredient(ModContent.ItemType<CryomancerEnchant>());
        //    recipe.AddIngredient(ModContent.ItemType<TideTurnerEnchant>());

        //    recipe.AddTile<CrucibleCosmosSheet>();

        //    recipe.Register();
        //}
    }
}
