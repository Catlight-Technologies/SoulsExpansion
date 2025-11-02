using Terraria;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using CSE.Core;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler;

namespace CSE.Content.Thorium.Forces
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class JotunheimForce : BaseForce
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //Enchants[Type] =
            //[
            //    ModContent.ItemType<AstroEnchant>(),
            //    ModContent.ItemType<CelestialEnchant>(),
            //    ModContent.ItemType<CyberpunkEnchant>(),
            //    ModContent.ItemType<MartianConduitEnchant>(),
            //    ModContent.ItemType<ShootingStarEnchant>(),
            //    ModContent.ItemType<WhiteDwarfEnchant>()
            //];
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<JotunheimEffect>(Item);

            //player.AddEffect<AstroEffect>(Item);
            //player.AddEffect<CelestialEffect>(Item);
            //player.AddEffect<CyberpunkEffect>(Item);
            //player.AddEffect<MartianConduitEffect>(Item);
            //player.AddEffect<ShootingStarEffect>(Item);
            //player.AddEffect<WhiteDwarfEffect>(Item);
        }

        public class JotunheimEffect : AccessoryEffect
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
