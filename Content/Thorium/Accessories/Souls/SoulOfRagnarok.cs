using FargowiltasSouls.Content.Items.Accessories.Souls;
using FargowiltasSouls.Content.Items.Materials;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using Terraria.ModLoader;
using Terraria;
using CSE.Core;
using CSE.Content.Thorium.CraftingStation;
using CSE.Content.Thorium.Accessories.Other;
using FargowiltasSouls.Core.Toggler;

namespace CSE.Content.Thorium.Accessories.Souls
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class SoulOfRagnarok : BaseSoul
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.value = 5000000;
            Item.rare = -12;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ModContent.GetInstance<OmegaTreads>().UpdateAccessory(player, hideVisual);

            ModContent.GetInstance<MotDE>().UpdateAccessory(player, hideVisual);


            player.AddEffect<RagnarokEffect>(Item);
        }
        public class RagnarokEffect : AccessoryEffect
        {
            public override Header ToggleHeader => null;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient<OmegaTreads>();
            recipe.AddIngredient<MotDE>();

            if (!ModCompatibility.Calamity.Loaded) { recipe.AddIngredient<AbomEnergy>(10); }
            if (ModCompatibility.SacredTools.Loaded) { recipe.AddIngredient(ModCompatibility.SacredTools.Mod.Find<ModItem>("EmberOfOmen"), 5); }
            if (ModCompatibility.Calamity.Loaded) { recipe.AddIngredient(ModCompatibility.Calamity.Mod.Find<ModItem>("ShadowspecBar"), 5); }
            if (ModCompatibility.Calamity.Loaded) { recipe.AddIngredient(ModCompatibility.Calamity.Mod.Find<ModItem>("MiracleMatter")); }

            recipe.AddTile<DreamersForgeTile>();
            recipe.Register();
        }
    }
}
