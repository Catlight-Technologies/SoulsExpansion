using CSE.Content.Thorium.Headers;
using CSE.Core;
using Fargowiltas.Content.Items.Tiles;
using FargowiltasSouls.Content.Items.Materials;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.BasicAccessories;
using ThoriumMod.Items.DD;
using ThoriumMod.Items.Donate;
using ThoriumMod.Items.Misc;
using ThoriumMod.Items.ThrownItems;

namespace CSE.Content.Thorium.Accessories.Other
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class OmegaTreads : ModItem
    {
        public override string Texture => "ThoriumMod/Items/BossThePrimordials/TheOmegaCore";
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 10);
            Item.rare = 11;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.AddEffect<AirWalkersEffect>(Item))
                ModContent.GetInstance<AirWalkers>().UpdateAccessory(player, hideVisual);
            if (player.AddEffect<SurvivalistBootsEffect>(Item))
                ModContent.GetInstance<SurvivalistBoots>().UpdateAccessory(player, hideVisual);
            if (player.AddEffect<CrashBootsEffect>(Item))
                ModContent.GetInstance<CrashBoots>().UpdateAccessory(player, hideVisual);
            if (player.AddEffect<JetBootsEffect>(Item))
                ModContent.GetInstance<JetBoots>().UpdateAccessory(player, hideVisual);
            if (player.AddEffect<SlagStompersEffect>(Item))
                ModContent.GetInstance<SlagStompers>().UpdateAccessory(player, hideVisual);
            if (player.AddEffect<WeightedWingletsEffect>(Item))
                ModContent.GetInstance<WeightedWinglets>().UpdateAccessory(player, hideVisual);
            if (player.AddEffect<BeeBootiesEffect>(Item))
                ModContent.GetInstance<BeeBooties>().UpdateAccessory(player, hideVisual);
            if (player.AddEffect<TravelersBootsEffect>(Item))
                ModContent.GetInstance<TravelersBoots>().UpdateAccessory(player, hideVisual);
            if (player.AddEffect<SpartanSandlesEffect>(Item))
                ModContent.GetInstance<SpartanSandles>().UpdateAccessory(player, hideVisual);
            if (player.AddEffect<SpringStepsEffect>(Item))
                ModContent.GetInstance<SpringSteps>().UpdateAccessory(player, hideVisual);
            if (player.AddEffect<OgreSandalEffect>(Item))
                ModContent.GetInstance<OgreSandal>().UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);

            recipe.AddIngredient<AirWalkers>();
            recipe.AddIngredient<SurvivalistBoots>();
            recipe.AddIngredient<CrashBoots>();
            recipe.AddIngredient<JetBoots>();
            recipe.AddIngredient<SlagStompers>();
            recipe.AddIngredient<WeightedWinglets>();
            recipe.AddIngredient<BeeBooties>();
            recipe.AddIngredient<TravelersBoots>();
            recipe.AddIngredient<SpartanSandles>();
            recipe.AddIngredient<SpringSteps>();
            recipe.AddIngredient<OgreSandal>();

            recipe.AddIngredient<DeviatingEnergy>(10);
            recipe.AddIngredient(ItemID.LunarBar, 10);

            recipe.AddTile<CrucibleCosmosSheet>();

            recipe.Register();
        }


        //here goes nothing...
        public class AirWalkersEffect : AccessoryEffect
        {
            public override int ToggleItemType => ModContent.ItemType<AirWalkers>();
            public override Header ToggleHeader => Header.GetHeader<OmegaTreadsHeader>();
        }
        public class SurvivalistBootsEffect : AccessoryEffect
        {
            public override int ToggleItemType => ModContent.ItemType<SurvivalistBoots>();

            public override Header ToggleHeader => Header.GetHeader<OmegaTreadsHeader>();
        }
        public class CrashBootsEffect : AccessoryEffect
        {
            public override int ToggleItemType => ModContent.ItemType<CrashBoots>();
            public override Header ToggleHeader => Header.GetHeader<OmegaTreadsHeader>();
        }
        public class JetBootsEffect : AccessoryEffect
        {
            public override int ToggleItemType => ModContent.ItemType<JetBoots>();

            public override Header ToggleHeader => Header.GetHeader<OmegaTreadsHeader>();
        }
        public class SlagStompersEffect : AccessoryEffect
        {
            public override int ToggleItemType => ModContent.ItemType<SlagStompers>();
            public override Header ToggleHeader => Header.GetHeader<OmegaTreadsHeader>();
        }
        public class WeightedWingletsEffect : AccessoryEffect
        {
            public override int ToggleItemType => ModContent.ItemType<WeightedWinglets>();

            public override Header ToggleHeader => Header.GetHeader<OmegaTreadsHeader>();
        }
        public class BeeBootiesEffect : AccessoryEffect
        {
            public override int ToggleItemType => ModContent.ItemType<BeeBooties>();
            public override Header ToggleHeader => Header.GetHeader<OmegaTreadsHeader>();
        }
        public class TravelersBootsEffect : AccessoryEffect
        {
            public override int ToggleItemType => ModContent.ItemType<TravelersBoots>();

            public override Header ToggleHeader => Header.GetHeader<OmegaTreadsHeader>();
        }
        public class SpartanSandlesEffect : AccessoryEffect
        {
            public override int ToggleItemType => ModContent.ItemType<SpartanSandles>();
            public override Header ToggleHeader => Header.GetHeader<OmegaTreadsHeader>();
        }
        public class SpringStepsEffect : AccessoryEffect
        {
            public override int ToggleItemType => ModContent.ItemType<SpringSteps>();

            public override Header ToggleHeader => Header.GetHeader<OmegaTreadsHeader>();
        }
        public class OgreSandalEffect : AccessoryEffect
        {
            public override int ToggleItemType => ModContent.ItemType<OgreSandal>();
            public override Header ToggleHeader => Header.GetHeader<OmegaTreadsHeader>();
        }
    }
}
