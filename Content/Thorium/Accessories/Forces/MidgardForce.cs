using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using CSE.Core;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler;
using Microsoft.Xna.Framework.Graphics;

namespace CSE.Content.Thorium.Forces
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class MidgardForce : BaseForce
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //Enchants[Type] =
            //[
            //    ModContent.ItemType<DurasteelEnchant>(),
            //    ModContent.ItemType<IllumiteEnchant>(),
            //    ModContent.ItemType<LodestoneEnchant>(),
            //    ModContent.ItemType<TerrariumEnchant>(),
            //    ModContent.ItemType<TitanicEnchant>(),
            //    ModContent.ItemType<ValadiumEnchant>()
            //];
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<MidgardEffect>(Item);

            //player.AddEffect<DurasteelEffect>(Item);
            //player.AddEffect<IllumiteEffect>(Item);
            //player.AddEffect<LodestoneEffect>(Item);
            //player.AddEffect<TerrariumEffect>(Item);
            //player.AddEffect<TitanicEffect>(Item);
            //player.AddEffect<ValadiumEffect>(Item);
        }

        public class MidgardEffect : AccessoryEffect
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
