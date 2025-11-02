using Terraria;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using CSE.Core;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.Thorium.Accessories.Enchantments;
using static CSE.Content.Thorium.Accessories.Enchantments.DreamWeaverEnchant;
using static CSE.Content.Thorium.Accessories.Enchantments.AssassinEnchant;
using static CSE.Content.Thorium.Accessories.Enchantments.TideTurnerEnchant;
using static CSE.Content.Thorium.Accessories.Enchantments.RhapsodistEnchant;
using static CSE.Content.Thorium.Accessories.Enchantments.PyromancerEnchant;

namespace CSE.Content.Thorium.Forces
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class AsgardForce : BaseForce
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Enchants[Type] =
            [
                ModContent.ItemType<DreamWeaverEnchant>(),
                ModContent.ItemType<AssassinEnchant>(),
                ModContent.ItemType<PyromancerEnchant>(),
                ModContent.ItemType<RhapsodistEnchant>(),
                ModContent.ItemType<TideTurnerEnchant>()
            ];
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<AsgardEffect>(Item);

            player.AddEffect<DreamWeaverEffect>(Item);
            player.AddEffect<AssassinEffect>(Item);
            player.AddEffect<PyromancerEffect>(Item);
            player.AddEffect<RhapsodistEffect>(Item);
            player.AddEffect<TideTurnerEffect>(Item);
        }

        public class AsgardEffect : AccessoryEffect
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
