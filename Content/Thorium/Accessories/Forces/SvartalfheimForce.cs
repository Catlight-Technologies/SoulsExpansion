using Terraria;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using CSE.Core;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.Thorium.Accessories.Enchantments;
using static CSE.Content.Thorium.Accessories.Enchantments.DepthDiverEnchant;

namespace CSE.Content.Thorium.Forces
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class SvartalfheimForce : BaseForce
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Enchants[Type] =
            [
                //ModContent.ItemType<BronzeEnchant>(),
                //ModContent.ItemType<DarkSteelEnchant>(),
                ModContent.ItemType<DepthDiverEnchant>(),
                //ModContent.ItemType<GraniteEnchant>(),
                //ModContent.ItemType<SandstoneEnchant>()
            ];
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<SvartalfheimEffect>(Item);

            //player.AddEffect<BronzeEffect>(Item);
            //player.AddEffect<DarkSteelEffect>(Item);
            player.AddEffect<DepthDiverEffect>(Item);
            //player.AddEffect<GraniteEffect>(Item);
            //player.AddEffect<SandstoneEffect>(Item);
        }

        public class SvartalfheimEffect : AccessoryEffect
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
