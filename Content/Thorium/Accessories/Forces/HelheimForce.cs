using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using CSE.Core;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler;
using static CSE.Content.Thorium.Accessories.Enchantments.EbonEnchant;

namespace CSE.Content.Thorium.Forces
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class HelheimForce : BaseForce
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //Enchants[Type] =
            //[
            //    ModContent.ItemType<DemonbloodEnchant>(),
            //    ModContent.ItemType<DreadEnchant>(),
            //    ModContent.ItemType<LichEnchant>(),
            //    ModContent.ItemType<ShadeEnchant>(),
            //    ModContent.ItemType<SilkEnchant>(),
            //    ModContent.ItemType<WarlockEnchant>(),
            //    ModContent.ItemType<SpiritEnchant>()
            //];
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<HelheimEffect>(Item);

            //player.AddEffect<DemonbloodEffect>(Item);
            //player.AddEffect<DreadEffect>(Item);
            //player.AddEffect<LichEffect>(Item);
            //player.AddEffect<ShadeEffect>(Item);
            //player.AddEffect<SilkEffect>(Item);
            //player.AddEffect<WarlockEffect>(Item);
            player.AddEffect<EbonEffect>(Item);
            player.AddEffect<EbonEffectConversion>(Item);
            //player.AddEffect<SpiritEffect>(Item);
        }

        public class HelheimEffect : AccessoryEffect
        {
            public override Header ToggleHeader => null;
        }
        //public override void AddRecipes()
        //{
        //    Recipe recipe = CreateRecipe();
        //    foreach (int ench in Enchants[Type])
        //        recipe.AddIngredient(ench);

        //    recipe.AddTile(ModContent.Find<ModTile>("Fargowiltas", "CrucibleCosmosSheet"));
        //    recipe.Register();
        //}
    }
}
