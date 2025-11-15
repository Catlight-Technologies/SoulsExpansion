using Fargowiltas.Content.Items.Summons.VanillaCopy;
using Fargowiltas.Content.Items.Tiles;
using FargowiltasSouls.Content.Items.Materials;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Fargowiltas.Content.Items.Summons.SwarmSummons;

namespace CSE.Content.Common.Summons
{
    public class TruffleWormEX : ModItem
    {
        public override string Texture => "Terraria/Images/Item_2673";

        public override void SetDefaults()
        {
            Item.maxStack = 20;
            Item.rare = 11;
            Item.width = 12;
            Item.height = 12;
            Item.bait = 1487;
            Item.value = Item.sellPrice(0, 17, 0, 0);
        }
        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.Mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.OverrideColor = new Color(Main.DiscoR, 51, 255 - (int)(Main.DiscoR * 0.4));
                }
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient<OverloadFish>(10);
            recipe.AddIngredient(ItemID.ShrimpyTruffle);
            //recipe.AddIngredient<EternalEnergy>(5); //no longer post mutant
            recipe.AddIngredient<AbomEnergy>(5);
            recipe.AddIngredient<DeviatingEnergy>(5);
            recipe.AddIngredient<Eridanium>(5);
            recipe.AddTile(ModContent.TileType<CrucibleCosmosSheet>());
            recipe.Register();
        }
    }
}
