using FargowiltasSouls.Content.Items.Accessories.Souls;
using Terraria.ModLoader;
using Terraria;
using System.Collections.Generic;
using Terraria.Localization;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler.Content;
using SacredTools.Content.Items.Accessories;
using FargowiltasSouls.Core.Toggler;

namespace CSE.Core.SoulsRecipes
{
    public class WorldShaperSoulRecipe : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe.HasResult(ModContent.ItemType<WorldShaperSoul>()))
                {
                    if (ModCompatibility.SacredTools.Loaded) { /*recipe.AddIngredient(ModCompatibility.SacredTools.Mod.Find<ModItem>("RageSuppressor"), 1);*/ /*recipe.AddIngredient(ModCompatibility.SacredTools.Mod.Find<ModItem>("LunarRing"), 1);*/ }
                    if (ModCompatibility.Homeward.Loaded) { recipe.AddIngredient(ModCompatibility.Homeward.Mod.Find<ModItem>("TimelessMiner"), 1); }
                    if (ModCompatibility.Thorium.Loaded) { recipe.AddIngredient(ModCompatibility.Thorium.Mod.Find<ModItem>("WishingGlass"), 1); recipe.AddIngredient(Mod.Find<ModItem>("GeodeEnchant"), 1); recipe.AddIngredient(ModCompatibility.Thorium.Mod.Find<ModItem>("TerrariumCanyonSplitter"), 1); }
                }
                if (ModCompatibility.SacredTools.Loaded)
                {
                    if (recipe.HasResult(ModCompatibility.SacredTools.Mod.Find<ModItem>("LunarRing")))
                    {
                        if (ModCompatibility.Thorium.Loaded) { recipe.AddIngredient(ModCompatibility.Thorium.Mod.Find<ModItem>("GreedyGoblet"), 1); recipe.RemoveIngredient(3035); }
                    }
                }
            }
        }
    }
    public class WorldShaperSoulEffects : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override void UpdateAccessory(Item Item, Player player, bool hideVisual)
        {
            if (Item.type == ModContent.ItemType<WorldShaperSoul>() || Item.type == ModContent.ItemType<DimensionSoul>() || Item.type == ModContent.ItemType<EternitySoul>())
            {
                if (ModCompatibility.Thorium.Loaded)
                {
                    Mod.Find<ModItem>("GeodeEnchant").UpdateAccessory(player, true);
                }
                if (ModCompatibility.SacredTools.Loaded)
                {
                    //player.AddEffect<LunarRingEffect>(Item);
                    ModCompatibility.SacredTools.Mod.Find<ModItem>("RageSuppressor").UpdateAccessory(player, true);
                }
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string key = "Mods.CSE.AddedEffects.";

            if (item.type == ModContent.ItemType<WorldShaperSoul>() )
            {
                if (ModCompatibility.SacredTools.Loaded)
                {
                    //tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue(key + "SoAWorldshaper")));
                }
                if (ModCompatibility.Thorium.Loaded)
                {
                    tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue(key + "ThoriumWorldshaper")));
                }
            }
        }
        [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
        public class LunarRingEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<TrawlerHeader>();
            public override int ToggleItemType => ModContent.ItemType<LunarRing>();

            public override void PostUpdateEquips(Player player)
            {
                ModCompatibility.SacredTools.Mod.Find<ModItem>("LunarRing").UpdateAccessory(player, true);
            }
        }
    }
}
