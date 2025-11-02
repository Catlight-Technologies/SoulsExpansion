using Terraria;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using CSE.Core;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.Thorium.Accessories.Enchantments;
using static CSE.Content.Thorium.Accessories.Enchantments.NobleEnchant;

namespace CSE.Content.Thorium.Forces
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class MuspelheimForce : BaseForce
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Enchants[Type] =
            [
                //ModContent.ItemType<DragonscaleEnchant>(),
                //ModContent.ItemType<FleshEnchant>(),
                ModContent.ItemType<NobleEnchant>(),
                //ModContent.ItemType<JesterEnchant>()
            ];
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<MuspelheimEffect>(Item);

            //player.AddEffect<DragonscaleEffect>(Item);
            //player.AddEffect<FleshEffect>(Item);
            player.AddEffect<NobleEffect>(Item);
            //player.AddEffect<JesterEffect>(Item);
        }

        public class MuspelheimEffect : AccessoryEffect
        {
            public override Header ToggleHeader => null;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            foreach (int ench in Enchants[Type])
                recipe.AddIngredient(ench);

            recipe.AddTile(ModContent.Find<ModTile>("Fargowiltas", "CrucibleCosmosSheet"));
            recipe.Register();
        }
    }
}
