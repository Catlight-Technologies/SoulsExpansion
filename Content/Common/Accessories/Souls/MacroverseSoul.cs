using CSE.Content.Common.Materials;
using Fargowiltas.Content.Items.Tiles;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CSE.Core;
using Microsoft.Xna.Framework;

namespace CSE.Content.Common.Accessories.Souls
{
    public class MacroverseSoul : BaseSoul
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            if (ModCompatibility.Homeward.Loaded
                || ModCompatibility.Redemption.Loaded
                || ModCompatibility.Thorium.Loaded
                || ModCompatibility.Calamity.Loaded
                || ModCompatibility.SacredTools.Loaded)
            {
                return true;
            }
            return false;
        }
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.value = 2500000;
            Item.rare = -12;
            Item.expert = true;
            Item.accessory = true;
        }

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

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ModContent.Find<ModItem>(ModCompatibility.Souls.Name, "TerrariaSoul").UpdateAccessory(player, false);

            //if (ModCompatibility.Calamity.Loaded && ModCompatibility.Crossmod.Loaded)
            //{
            //    ModContent.Find<ModItem>(Mod.Name, "SoulOfTyrant").UpdateAccessory(player, false);
            //}
            //if (ModCompatibility.Homeward.Loaded)
            //{
            //    ModContent.Find<ModItem>(Mod.Name, "SoulOfJourney").UpdateAccessory(player, false);
            //}
            //if (ModCompatibility.Redemption.Loaded)
            //{
            //    ModContent.Find<ModItem>(Mod.Name, "SoulOfTrueRedemption").UpdateAccessory(player, false);
            //}
            if (ModCompatibility.SacredTools.Loaded)
            {
                ModContent.Find<ModItem>(Mod.Name, "SoulOfTwoRealms").UpdateAccessory(player, false);
            }
            if (ModCompatibility.Thorium.Loaded)
            {
                ModContent.Find<ModItem>(Mod.Name, "SoulOfYggdrasil").UpdateAccessory(player, false);
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient<EternalScale>(5);

            if (ModLoader.HasMod("NoxusPort"))
            {
                recipe.AddIngredient(ModLoader.GetMod("NoxusPort").Find<ModItem>("EntropicBar"), 5);
            }
            if (ModLoader.HasMod("NoxusBoss"))
            {
                recipe.AddIngredient(ModLoader.GetMod("NoxusBoss").Find<ModItem>("MetallicChunk"), 5);
                //recipe.AddIngredient<NDMaterialPlaceholder>(5);
            }

            recipe.AddIngredient<TerrariaSoul>(1);

            //if (ModCompatibility.Crossmod.Loaded)
            //{
            //    recipe.AddIngredient(Mod.Find<ModItem>("CalamitySoul"), 1);
            //}
            if (ModCompatibility.Thorium.Loaded)
            {
                recipe.AddIngredient(Mod.Find<ModItem>("SoulOfYggdrasil"), 1);
            }
            //if (ModCompatibility.Homeward.Loaded)
            //{
            //    recipe.AddIngredient(Mod.Find<ModItem>("SoulOfJourney"), 1);
            //}
            //if (ModCompatibility.Redemption.Loaded)
            //{
            //    recipe.AddIngredient(Mod.Find<ModItem>("SoulOfTrueRedemption"), 1);
            //}
            if (ModCompatibility.SacredTools.Loaded)
            {
                recipe.AddIngredient(Mod.Find<ModItem>("SoulOfTwoRealms"), 1);
            }

            recipe.AddTile(ModContent.TileType<CrucibleCosmosSheet>());
            recipe.Register();
        }
    }
}