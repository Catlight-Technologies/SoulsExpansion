using CSE.Content.Thorium.Headers;
using CSE.Core;
using Fargowiltas.Content.Items.Tiles;
using FargowiltasSouls;
using FargowiltasSouls.Content.Items.Materials;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items.BasicAccessories;
using ThoriumMod.Items.BossForgottenOne;
using ThoriumMod.Items.Donate;

namespace CSE.Content.Thorium.Accessories.Other
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class MotDE : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 50, 0, 0);
            Item.rare = 10;
            Item.accessory = true;
            Item.defense = 5;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.AddEffect<InfernoLordsFocusEffect>(Item))
                ModContent.GetInstance<InfernoLordsFocus>().UpdateAccessory(player, hideVisual);
            if (player.AddEffect<PlasmaGeneratorEffect>(Item))
                ModContent.GetInstance<PlasmaGenerator>().UpdateAccessory(player, hideVisual);
            if (player.AddEffect<CapeoftheSurvivorEffect>(Item))
                ModContent.GetInstance<CapeoftheSurvivor>().UpdateAccessory(player, hideVisual);

            ModContent.GetInstance<TheRing>().UpdateAccessory(player, hideVisual);
            if (!player.FargoSouls().MutantPresence)
            {
                ModContent.GetInstance<AbyssalShell>().UpdateAccessory(player, hideVisual);
            }
            player.GetDamage<GenericDamageClass>() += 0.04f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);

            recipe.AddIngredient<PlasmaGenerator>();
            recipe.AddIngredient<InfernoLordsFocus>();
            recipe.AddIngredient<CapeoftheSurvivor>();
            recipe.AddIngredient<AbyssalShell>();
            recipe.AddIngredient<TheRing>();
            //recipe.AddIngredient<TheShield>();

            recipe.AddIngredient<AbomEnergy>(10);

            recipe.AddTile<CrucibleCosmosSheet>();

            recipe.Register();
        }

        [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
        [ExtendsFromMod(ModCompatibility.Thorium.Name)]
        public class InfernoLordsFocusEffect : AccessoryEffect
        {
            public override int ToggleItemType => ModContent.ItemType<InfernoLordsFocus>();
            public override Header ToggleHeader => Header.GetHeader<MotDEHeader>();
        }
        public class CapeoftheSurvivorEffect : AccessoryEffect
        {
            public override int ToggleItemType => ModContent.ItemType<CapeoftheSurvivor>();

            public override Header ToggleHeader => Header.GetHeader<MotDEHeader>();
        }
        public class PlasmaGeneratorEffect : AccessoryEffect
        {
            public override int ToggleItemType => ModContent.ItemType<PlasmaGenerator>();

            public override Header ToggleHeader => Header.GetHeader<MotDEHeader>();
            public override bool MutantsPresenceAffects => true;
        }
    }
}
