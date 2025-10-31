using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.HealerItems;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using ThoriumMod.Items.BossThePrimordials.Dream;
using CSE.Core;
using CSE.Content.Thorium.Headers;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler;

namespace CSE.Content.Thorium.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class DreamWeaverEnchant : BaseEnchant
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

        public override Color nameColor => new(92, 31, 122);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<DreamWeaverEffect>(Item);
        }

        public class DreamWeaverEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<AsgardForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<DreamWeaverEnchant>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = this.CreateRecipe();

            recipe.AddRecipeGroup("CSE:AnyDreamWeaversHelmet");
            recipe.AddIngredient(ModContent.ItemType<DreamWeaversTabard>());
            recipe.AddIngredient(ModContent.ItemType<DreamWeaversTreads>());
            recipe.AddIngredient(ModContent.ItemType<DragonHeartWand>());
            recipe.AddIngredient(ModContent.ItemType<SnackLantern>());
            recipe.AddIngredient(ModContent.ItemType<ChristmasCheer>());


            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
