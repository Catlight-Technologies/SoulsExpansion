using CSE.Content.SoA.CraftingStation;
using CSE.Content.SoA.Headers;
using CSE.Core;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using FargowiltasSouls.Content.Items.Materials;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler;
using SacredTools.Content.Items.Accessories;
using SacredTools.Content.Items.Materials;
using Terraria;
using Terraria.ModLoader;

namespace CSE.Content.SoA.Accessories.Souls
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class SoulOfCreations : BaseSoul
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.value = 5000000;
            Item.rare = -12;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.AddEffect<YataMirrorEffect>(Item))
                ModContent.GetInstance<YataMirror>().UpdateAccessory(player, hideVisual);

            player.AddEffect<CreationsEffect>(Item);
        }
        public class CreationsEffect : AccessoryEffect
        {
            public override Header ToggleHeader => null;
        }

        public class YataMirrorEffect : AccessoryEffect
        {
            public override int ToggleItemType => ModContent.ItemType<YataMirror>();
            public override Header ToggleHeader => Header.GetHeader<CreationsHeader>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient<YataMirror>();

            if (!ModCompatibility.Calamity.Loaded) { recipe.AddIngredient<AbomEnergy>(10); }
            if (ModCompatibility.Calamity.Loaded) { recipe.AddIngredient(ModCompatibility.Calamity.Mod.Find<ModItem>("ShadowspecBar"), 5); }
            if (ModCompatibility.Calamity.Loaded) { recipe.AddIngredient(ModCompatibility.Calamity.Mod.Find<ModItem>("MiracleMatter")); }

            recipe.AddIngredient<EmberOfOmen>(5);
            recipe.AddTile<SyranCraftingStationTile>();
            recipe.Register();
        }
    }
}
