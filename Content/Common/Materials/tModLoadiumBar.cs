using Luminance.Core.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using FargowiltasSouls.Content.Items.Materials;
using CSE.Core;
using CSE.Content.Thorium.Materials;
using CSE.Content.Common.CraftingStation;

namespace CSE.Content.Common.Materials
{
    public class tModLoadiumBar : ModItem
    {
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if ((line.Mod == "Terraria" && line.Name == "ItemName") || line.Name == "FlavorText")
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);
                ManagedShader shader = ShaderManager.GetShader("FargowiltasSouls.Text");
                shader.TrySetParameter("mainColor", new Color(42, 66, 99));
                shader.TrySetParameter("secondaryColor", Main.DiscoColor);
                shader.Apply("PulseUpwards");
                Utils.DrawBorderString(Main.spriteBatch, line.Text, new Vector2(line.X, line.Y), Color.White, 1);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
                return false;
            }
            return true;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 24;
            Item.value = int.MaxValue/10;
            Item.rare = 11;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient<EternalEnergy>(1);
            recipe.AddIngredient<DeviatingEnergy>(1);
            recipe.AddIngredient<EternalScale>(1);

            if (ModLoader.HasMod("CalEntropy"))
            {
                recipe.AddIngredient(ModLoader.GetMod("CalEntropy").Find<ModItem>("WyrmTooth"), 1);
            }
            if (ModLoader.HasMod("NoxusPort"))
            {
                recipe.AddIngredient(ModLoader.GetMod("NoxusPort").Find<ModItem>("EntropicBar"), 1);
            }
            if (ModLoader.HasMod("NoxusBoss"))
            {
                recipe.AddIngredient(ModLoader.GetMod("NoxusBoss").Find<ModItem>("MetallicChunk"), 1);
                //recipe.AddIngredient<NDMaterialPlaceholder>(1);
            }
            if (ModCompatibility.Calamity.Loaded)
            {
                recipe.AddIngredient(ModCompatibility.Calamity.Mod.Find<ModItem>("ShadowspecBar"), 1);
                recipe.AddIngredient(ModCompatibility.Calamity.Mod.Find<ModItem>("MiracleMatter"), 1);
            }
            if (ModCompatibility.SacredTools.Loaded)
            {
                recipe.AddIngredient(ModCompatibility.SacredTools.Mod.Find<ModItem>("EmberOfOmen"), 1);
            }


            if (ModCompatibility.Homeward.Loaded && !ModCompatibility.Calamity.Loaded)
            {
                recipe.AddIngredient(ModCompatibility.Homeward.Mod.Find<ModItem>("FinalBar"), 1);
            }
            if (ModCompatibility.Thorium.Loaded /*&& !ModCompatibility.Calamity.Loaded*/)
            {
                recipe.AddIngredient<DreamEssence>(1);
            }
            if (ModCompatibility.Redemption.Loaded && !ModCompatibility.Calamity.Loaded)
            {
                recipe.AddIngredient(ModCompatibility.Redemption.Mod.Find<ModItem>("LifeFragment"), 1);
            }
            if (!ModCompatibility.Calamity.Loaded)
            {
                recipe.AddIngredient<AbomEnergy>(1);
            }

            recipe.AddTile(ModContent.TileType<MutantsForgeTile>());
            recipe.Register();
        }
    }
}
