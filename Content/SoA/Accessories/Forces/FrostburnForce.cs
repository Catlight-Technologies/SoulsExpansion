using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using CSE.Content.SoA.Accessories.Enchantments;
using CSE.Core;
using FargowiltasSouls.Core.Toggler;
using static CSE.Content.SoA.Accessories.Enchantments.BlightboneEnchant;
using static CSE.Content.SoA.Accessories.Enchantments.FrosthunterEnchant;
using static CSE.Content.SoA.Accessories.Enchantments.ExitumLuxEnchant;
using static CSE.Content.SoA.Accessories.Enchantments.FlariumEnchant;

namespace CSE.Content.SoA.Accessories.Forces
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class FrostburnForce : BaseForce
    {
        public override void SetStaticDefaults()
        {
            Enchants[Type] = new int[4]
            {
                ModContent.ItemType<BlightboneEnchant>(),
                ModContent.ItemType<FrosthunterEnchant>(),
                ModContent.ItemType<ExitumLuxEnchant>(),
                ModContent.ItemType<FlariumEnchant>()
            };
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = 11;
            Item.value = 600000;
        }

        public class FrostburnEffect : AccessoryEffect
        {
            public override Header ToggleHeader => null;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<BlightboneEffect>(Item);
            player.AddEffect<FrosthunterEffect>(Item);
            player.AddEffect<FlariumEffect>(Item);
            player.AddEffect<ExitumLuxEffect>(Item);

            player.AddEffect<FrostburnEffect>(Item);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            int[] array = Enchants[Type];
            foreach (int itemID in array)
            {
                recipe.AddIngredient(itemID);
            }

            recipe.AddTile(ModContent.Find<ModTile>("Fargowiltas", "CrucibleCosmosSheet"));
            recipe.Register();
        }
    }
}