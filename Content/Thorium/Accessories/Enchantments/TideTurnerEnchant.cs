using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.BossThePrimordials.Aqua;
using ThoriumMod.Items.BossMini;
using ThoriumMod.Items.Donate;
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
    public class TideTurnerEnchant : BaseEnchant
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

        public override Color nameColor => new Color(0, 119, 170);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<TideTurnerEffect>(Item);
        }

        public class TideTurnerEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<AsgardForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<TideTurnerEnchant>();
        }

        public override void AddRecipes()
        {


            Recipe recipe = this.CreateRecipe();

            recipe.AddRecipeGroup("CSE:AnyTideTurnerHelmet");
            recipe.AddIngredient(ModContent.ItemType<TideTurnerBreastplate>());
            recipe.AddIngredient(ModContent.ItemType<TideTurnerGreaves>());
            recipe.AddIngredient(ModContent.ItemType<PlagueLordFlask>());
            recipe.AddIngredient(ModContent.ItemType<PoseidonCharge>());
            recipe.AddIngredient(ModContent.ItemType<TidalWave>());

            recipe.AddTile<EnchantedTreeSheet>();
            recipe.Register();
        }
    }
}
