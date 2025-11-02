using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.ThrownItems;
using ThoriumMod.Items.Consumable;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using CSE.Core;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.Thorium.Headers;
using Fargowiltas.Content.Items.Tiles;

namespace CSE.Content.Thorium.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class FungusEnchant : BaseEnchant
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

        public override Color nameColor => new(113, 146, 245);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<FungusEffect>(Item);
        }

        public class FungusEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<VanaheimForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<FungusEnchant>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = this.CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<FungusHat>());
            recipe.AddIngredient(ModContent.ItemType<FungusGuard>());
            recipe.AddIngredient(ModContent.ItemType<FungusLeggings>());

            //recipe.AddIngredient(ModContent.ItemType<sporeb>());
            recipe.AddIngredient(ModContent.ItemType<SwampSpike>());
            recipe.AddIngredient(ModContent.ItemType<SporeCoatingItem>(), 10);

            recipe.AddTile<EnchantedTreeSheet>();
            recipe.Register();
        }
    }
}
