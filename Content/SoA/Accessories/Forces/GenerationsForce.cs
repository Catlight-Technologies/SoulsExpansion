using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using CSE.Content.SoA.Accessories.Enchantments;
using static CSE.Content.SoA.Accessories.Enchantments.PrairieEnchant;
using static CSE.Content.SoA.Accessories.Enchantments.VulcanReaperEnchant;
using static CSE.Content.SoA.Accessories.Enchantments.EerieEnchant;
using FargowiltasSouls.Core.Toggler;
using CSE.Core;
using static CSE.Content.SoA.Accessories.Enchantments.CairoCrusaderEnchant;

namespace CSE.Content.SoA.Accessories.Forces
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class GenerationsForce : BaseForce
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = 11;
            Item.value = 600000;
        }

        public override void SetStaticDefaults()
        {
            Enchants[Type] =
            [
                ModContent.ItemType<EerieEnchant>(),
                ModContent.ItemType<PrairieEnchant>(),
                ModContent.ItemType<VulcanReaperEnchant>(),
                ModContent.ItemType<CairoCrusaderEnchant>()
            ];
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<EerieEffect>(Item);
            player.AddEffect<PrairieEffect>(Item);
            player.AddEffect<VulcanReaperEffect>(Item);
            player.AddEffect<CairoCrusaderEffect>(Item);

            player.AddEffect<GenerationsEffect>(Item);
        }

        public class GenerationsEffect : AccessoryEffect
        {
            public override Header ToggleHeader => null;
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
