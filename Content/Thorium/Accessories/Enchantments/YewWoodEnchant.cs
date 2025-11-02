using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.ArcaneArmor;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls;
using CSE.Core;
using CSE.Content.Thorium.Headers;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.Thorium.Projectiles;

namespace CSE.Content.Thorium.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class YewWoodEnchant : BaseEnchant
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = 2;
            Item.value = 60000;
        }

        public override Color nameColor => new(48, 63, 47);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<YewWoodEffect>(Item);
        }

        public class YewWoodEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<VanaheimForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<YewWoodEnchant>();
            public override bool ExtraAttackEffect => true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = this.CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<YewWoodHelmet>());
            recipe.AddIngredient(ModContent.ItemType<YewWoodBreastguard>());
            recipe.AddIngredient(ModContent.ItemType<YewWoodLeggings>());
            recipe.AddIngredient(ModContent.ItemType<ThumbRing>());
            recipe.AddIngredient(ModContent.ItemType<YewWoodBow>());
            recipe.AddIngredient(ModContent.ItemType<YewWoodFlintlock>());


            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
