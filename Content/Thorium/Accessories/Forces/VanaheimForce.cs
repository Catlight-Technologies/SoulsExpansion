using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using CSE.Core;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.Thorium.Accessories.Enchantments;
using static CSE.Content.Thorium.Accessories.Enchantments.FlightEnchant;
using static CSE.Content.Thorium.Accessories.Enchantments.FungusEnchant;
using static CSE.Content.Thorium.Accessories.Enchantments.YewWoodEnchant;
using static CSE.Content.Thorium.Accessories.Enchantments.CrierEnchant;

namespace CSE.Content.Thorium.Forces
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class VanaheimForce : BaseForce
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Enchants[Type] =
            [
                //ModContent.ItemType<BiotechEnchant>(),
                //ModContent.ItemType<BloomingEnchant>(),
                ModContent.ItemType<CrierEnchant>(),
                ModContent.ItemType<FlightEnchant>(),
                ModContent.ItemType<FungusEnchant>(),
                //ModContent.ItemType<LifeBloomEnchant>(),
                ModContent.ItemType<YewWoodEnchant>()
            ];
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<VanaheimEffect>(Item);

            //player.AddEffect<BiotechEffect>(Item);
            //player.AddEffect<BloomingEffect>(Item);
            player.AddEffect<CrierEffect>(Item);
            player.AddEffect<FlightEffect>(Item);
            player.AddEffect<FungusEffect>(Item);
            //player.AddEffect<LifeBloomEffect>(Item);
            player.AddEffect<YewWoodEffect>(Item);
        }

        public class VanaheimEffect : AccessoryEffect
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
