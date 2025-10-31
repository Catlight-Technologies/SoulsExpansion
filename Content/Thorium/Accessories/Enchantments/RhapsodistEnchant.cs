using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.BossThePrimordials.Rhapsodist;
using ThoriumMod.Items.BardItems;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using CSE.Core;
using CSE.Content.Thorium.Headers;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler;

namespace CSE.Content.Thorium.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class RhapsodistEnchant : BaseEnchant
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = 10;
            Item.value = 400000;
        }

        public override Color nameColor => new(243, 194, 48);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<RhapsodistEffect>(Item);
        }

        public class RhapsodistEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<AsgardForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<RhapsodistEnchant>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = this.CreateRecipe();

            recipe.AddRecipeGroup("CSE:AnyRhapsodistHelmet");
            recipe.AddIngredient(ModContent.ItemType<RhapsodistChestWoofer>());
            recipe.AddIngredient(ModContent.ItemType<RhapsodistBoots>());
            recipe.AddIngredient(ModContent.ItemType<JingleBells>());
            recipe.AddIngredient(ModContent.ItemType<Sousaphone>());
            recipe.AddIngredient(ModContent.ItemType<EdgeofImagination>());

            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
