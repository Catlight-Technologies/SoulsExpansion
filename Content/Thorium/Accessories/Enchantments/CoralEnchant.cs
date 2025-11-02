using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.BossQueenJellyfish;
using ThoriumMod.Items.Depths;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using ThoriumMod.Items.Coral;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using CSE.Core;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.Thorium.Headers;
using Fargowiltas.Content.Items.Tiles;

namespace CSE.Content.Thorium.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class CoralEnchant : BaseEnchant
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = 1;
            Item.value = 40000;
        }

        public override Color nameColor => new(198, 61, 75);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<CoralEffect>(Item);
        }

        public class CoralEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<NiflheimForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<CoralEnchant>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<CoralHelmet>());
            recipe.AddIngredient(ModContent.ItemType<CoralChestGuard>());
            recipe.AddIngredient(ModContent.ItemType<CoralGreaves>());
            recipe.AddIngredient(ModContent.ItemType<SeaBreezePendant>());
            recipe.AddIngredient(ModContent.ItemType<BubbleMagnet>());
            recipe.AddIngredient(ItemID.Swordfish);

            recipe.AddTile<EnchantedTreeSheet>();
            recipe.Register();
        }
    }
}
