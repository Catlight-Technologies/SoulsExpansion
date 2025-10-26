using Terraria.ModLoader;
using Terraria;
using ThoriumMod.Items.NPCItems;
using ThoriumMod.Items.ArcaneArmor;
using Terraria.ID;
using CSE.Core;
using ThoriumMod.Items.BardItems;

namespace CSE.Content.Thorium.Accessories.Other
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class CorruptedWarShield : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            Item.value = 1000000;
            Item.rare = 11;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ModContent.GetInstance<GoblinWarshield>().UpdateAccessory(player, hideVisual);
            ModContent.GetInstance<AstroBeetleHusk>().UpdateAccessory(player, hideVisual);
            ModContent.GetInstance<BloomingShield>().UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<GoblinWarshield>();
            recipe.AddIngredient<AstroBeetleHusk>();
            recipe.AddIngredient<BloomingShield>();

            recipe.AddTile(TileID.TinkerersWorkbench);

            recipe.Register();
        }
    }
}