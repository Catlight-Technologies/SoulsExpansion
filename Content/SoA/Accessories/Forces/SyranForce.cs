using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using CSE.Core;
using CSE.Content.SoA.Accessories.Enchantments;
using static CSE.Content.SoA.Accessories.Enchantments.AsthraltiteEnchant;
using static CSE.Content.SoA.Accessories.Enchantments.VoidWardenEnchant;
using static CSE.Content.SoA.Accessories.Enchantments.DreadfireEnchant;
using FargowiltasSouls.Core.Toggler;

namespace CSE.Content.SoA.Accessories.Forces
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class SyranForce : BaseForce
    {
        public override void SetStaticDefaults()
        {
            Enchants[Type] = new int[3]
            {
                ModContent.ItemType<AsthraltiteEnchant>(),
                ModContent.ItemType<VoidWardenEnchant>(),
                ModContent.ItemType<DreadfireEnchant>()
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

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<AsthraltiteEffect>(Item);
            player.AddEffect<VoidWardenEffect>(Item);
            player.AddEffect<DreadfireEffect>(Item);

            player.AddEffect<SyranEffect>(Item);
        }

        public class SyranEffect : AccessoryEffect
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
