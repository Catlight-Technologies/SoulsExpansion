using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.Geode;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler.Content;
using CSE.Core;
using FargowiltasSouls.Core.Toggler;

namespace CSE.Content.Thorium.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class GeodeEnchant : BaseEnchant
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = 4;
            Item.value = 120000;
        }

        public override Color nameColor => new(243, 111, 247);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<GeodeEffect>(Item);
        }

        public class GeodeEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<WorldShaperHeader>();
            public override int ToggleItemType => ModContent.ItemType<GeodeEnchant>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<GeodeHelmet>());
            recipe.AddIngredient(ModContent.ItemType<GeodeChestplate>());
            recipe.AddIngredient(ModContent.ItemType<GeodeGreaves>());
            recipe.AddIngredient(ModContent.ItemType<GeodeGatherer>());
            recipe.AddIngredient(ModContent.ItemType<GeodeHamaxe>());
            recipe.AddIngredient(ModContent.ItemType<GeodePickaxe>());

            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();
        }
    }
}
