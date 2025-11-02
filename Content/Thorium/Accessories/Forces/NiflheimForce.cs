using Terraria;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using CSE.Core;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.Thorium.Accessories.Enchantments;
using static CSE.Content.Thorium.Accessories.Enchantments.CoralEnchant;

namespace CSE.Content.Thorium.Forces
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class NiflheimForce : BaseForce
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Enchants[Type] =
            [
                ModContent.ItemType<CoralEnchant>(),
                //ModContent.ItemType<CryomancerEnchant>(),
                //ModContent.ItemType<NagaskinEnchant>(),
                //ModContent.ItemType<TideHunterEnchant>(),
                //ModContent.ItemType<WhisperingEnchant>()
            ];
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<NiflheimEffect>(Item);

            player.AddEffect<CoralEffect>(Item);
            //player.AddEffect<CryomancerEffect>(Item);
            //player.AddEffect<NagaskinEffect>(Item);
            //player.AddEffect<TideHunterEffect>(Item);
            //player.AddEffect<WhisperingEffect>(Item);
        }

        public class NiflheimEffect : AccessoryEffect
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
