using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using CSE.Core;

namespace CSE.Content.Thorium.Accessories.Souls
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class SoulOfYggdrasil : BaseSoul
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.RegisterItemAnimation(Item.type, new DrawAnimationHorizontal(61, 6));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 50;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.value = 5000000;
            Item.rare = -12;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
        }
        //public override void AddRecipes()
        //{
        //    Recipe recipe = this.CreateRecipe();

        //    if (!ModCompatibility.Calamity.Loaded) { recipe.AddIngredient<AbomEnergy>(10); }
        //    recipe.AddIngredient(null, "MuspelheimForce");
        //    recipe.AddIngredient(null, "JotunheimForce");
        //    recipe.AddIngredient(null, "AlfheimForce");
        //    recipe.AddIngredient(null, "NiflheimForce");
        //    recipe.AddIngredient(null, "SvartalfheimForce");
        //    recipe.AddIngredient(null, "MidgardForce");
        //    recipe.AddIngredient(null, "VanaheimForce");
        //    recipe.AddIngredient(null, "HelheimForce");
        //    recipe.AddIngredient(null, "AsgardForce");
        //    recipe.AddIngredient(null, "MotDE");

        //    recipe.AddTile<DreamersForgeTile>();

        //    recipe.Register();
        //}
    }
}