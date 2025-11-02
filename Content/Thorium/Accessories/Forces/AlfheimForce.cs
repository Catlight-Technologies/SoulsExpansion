using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using CSE.Core;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler;

namespace CSE.Content.Thorium.Forces
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class AlfheimForce : BaseForce
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //Enchants[Type] =
            //[
                //ModContent.ItemType<FallenPalladinEnchant>(),
                //ModContent.ItemType<LifeBinderEnchant>(),
                //ModContent.ItemType<SacredEnchant>(),
                //ModContent.ItemType<WhiteKnightEnchant>(),
                //ModContent.ItemType<OrnateEnchant>(),
                //ModContent.ItemType<MaestroEnchant>()
            //];
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<AlfheimEffect>(Item);

            //player.AddEffect<FallenPalladinEffect>(Item);
            //player.AddEffect<LifeBinderEffect>(Item);
            //player.AddEffect<SacredEffect>(Item);
            //player.AddEffect<WhiteKnightEffect>(Item);
            //player.AddEffect<OrnateEffect>(Item);
            //player.AddEffect<MaestroEffect>(Item);
        }
        public class AlfheimEffect : AccessoryEffect
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
