using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using CSE.Core;

namespace CSE.Content.Thorium.Forces
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class MuspelheimForce : BaseForce
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
        //    Recipe recipe = CreateRecipe();

        //    recipe.AddIngredient<CyberPunkEnchant>();
        //    recipe.AddIngredient<DemonBloodEnchant>();
        //    recipe.AddIngredient<SandstoneEnchant>();
        //    recipe.AddIngredient<NobleEnchant>();
        //    recipe.AddIngredient<PyromancerEnchant>();

        //    recipe.AddTile<CrucibleCosmosSheet>();

        //    recipe.Register();
        //}
    }
}
