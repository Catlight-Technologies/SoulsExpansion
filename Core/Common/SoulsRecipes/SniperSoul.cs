using FargowiltasSouls.Content.Items.Accessories.Souls;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using ContinentOfJourney.Items.Accessories;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler.Content;
using System.Collections.Generic;
using Terraria.Localization;
using FargowiltasSouls.Core.Toggler;
using ThoriumMod.Items.Donate;
using ContinentOfJourney.Items.Accessories.Bookmarks;

namespace CSE.Core.SoulsRecipes
{
    public class SniperSoulRecipe : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe.HasResult(ModContent.ItemType<SnipersSoul>()))
                {
                    if (ModCompatibility.Calamity.Loaded) { recipe.AddIngredient(ModCompatibility.Calamity.Mod.Find<ModItem>("ReaperToothNecklace"), 1);  recipe.RemoveIngredient(ItemID.SharkToothNecklace); recipe.RemoveIngredient(ItemID.StingerNecklace); }
                    if (ModCompatibility.SacredTools.Loaded) { recipe.AddIngredient(ModCompatibility.SacredTools.Mod.Find<ModItem>("DolphinGun"), 1); recipe.AddIngredient(ModCompatibility.SacredTools.Mod.Find<ModItem>("VortexSigil"), 1); recipe.RemoveIngredient(ItemID.RangerEmblem); }
                    if (ModCompatibility.Homeward.Loaded) { /*recipe.AddIngredient(ModCompatibility.Homeward.Mod.Find<ModItem>("WalnutOnFire"), 1);*/ recipe.AddIngredient(ModCompatibility.Homeward.Mod.Find<ModItem>("Duality"), 1); recipe.AddIngredient(ModCompatibility.Homeward.Mod.Find<ModItem>("TheBatter"), 1); recipe.AddIngredient(ModCompatibility.Homeward.Mod.Find<ModItem>("CrossbowScope"), 1); recipe.RemoveIngredient(ItemID.ReconScope); recipe.RemoveIngredient(ItemID.SniperScope); recipe.RemoveIngredient(ItemID.MagicQuiver); recipe.RemoveIngredient(ItemID.MoltenQuiver); recipe.RemoveIngredient(ItemID.StalkersQuiver); }
                    if (ModCompatibility.Redemption.Loaded) { recipe.AddIngredient(ModCompatibility.Redemption.Mod.Find<ModItem>("SwarmerCannon"), 1); recipe.AddIngredient(ModCompatibility.Redemption.Mod.Find<ModItem>("MutagenRanged"), 1); recipe.RemoveIngredient(ItemID.RangerEmblem); }
                    if (ModCompatibility.Thorium.Loaded) { recipe.AddIngredient(ModCompatibility.Thorium.Mod.Find<ModItem>("ConcussiveWarhead"), 1); recipe.AddIngredient(ModCompatibility.Thorium.Mod.Find<ModItem>("QuasarsFlare"), 1); /*recipe.AddIngredient(ModCompatibility.Thorium.Mod.Find<ModItem>("OmniBow"), 1);*/ }
                }
                if (ModCompatibility.Calamity.Loaded)
                {
                    if (recipe.HasResult(ModCompatibility.Calamity.Mod.Find<ModItem>("ElementalQuiver")))
                    {
                        if (ModCompatibility.Thorium.Loaded)
                        {
                            if (!recipe.HasIngredient(ModCompatibility.Thorium.Mod.Find<ModItem>("TerrariumCore")))
                            {
                                recipe.AddIngredient(ModCompatibility.Thorium.Mod.Find<ModItem>("TerrariumCore"), 3);
                            }
                        }
                    }
                }
                if (ModCompatibility.Homeward.Loaded)
                {
                    if (recipe.HasResult(ModCompatibility.Homeward.Mod.Find<ModItem>("CrossbowScope")))
                    {
                        if (ModCompatibility.Redemption.Loaded) { recipe.AddIngredient(ModCompatibility.Redemption.Mod.Find<ModItem>("XeniumAlloy"), 3); }
                        if (ModCompatibility.Thorium.Loaded && !ModCompatibility.Calamity.Loaded) { recipe.AddIngredient(ModCompatibility.Thorium.Mod.Find<ModItem>("DeathEssence"), 3); }
                    }
                }
                if (ModCompatibility.SacredTools.Loaded)
                {
                    if (recipe.HasResult(ModCompatibility.SacredTools.Mod.Find<ModItem>("VortexSigil")))
                    {
                        if (ModCompatibility.Redemption.Loaded) { recipe.AddIngredient(ModCompatibility.Redemption.Mod.Find<ModItem>("XeniumAlloy"), 3); }
                        if (ModCompatibility.Homeward.Loaded) { recipe.AddIngredient(ModCompatibility.Homeward.Mod.Find<ModItem>("BullseyeBadge"), 1); recipe.RemoveIngredient(ItemID.RangerEmblem); }
                    }
                }
            }
        }
    }
    public class SniperSoulEffects : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override void UpdateAccessory(Item Item, Player player, bool hideVisual)
        {
            if (Item.type == ModContent.ItemType<SnipersSoul>() || Item.type == ModContent.ItemType<UniverseSoul>() || Item.type == ModContent.ItemType<EternitySoul>())
            {
                if (ModCompatibility.Homeward.Loaded)
                {
                    //player.AddEffect<WalnutOnFireEffect>(Item);
                    player.AddEffect<StarQuiverEffect>(Item);
                    player.AddEffect<TheBatterEffect>(Item);
                }
                if (ModCompatibility.Thorium.Loaded)
                {
                    player.AddEffect<WarheadEffect>(Item);
                }
                if (ModCompatibility.SacredTools.Loaded)
                {
                    player.AddEffect<InfAmmoEffect>(Item);
                }
            }
            if (ModCompatibility.Calamity.Loaded)
            {
                if (Item.type == ModCompatibility.Calamity.Mod.Find<ModItem>("ElementalQuiver").Type)
                {
                    if (ModCompatibility.Homeward.Loaded)
                    {
                        //player.AddEffect<StarQuiverEffect>(Item);
                    }
                }
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string key = "Mods.CSE.AddedEffects.";

            if (item.type == ModContent.ItemType<SnipersSoul>() )
            {
                if (ModCompatibility.Homeward.Loaded)
                {
                    tooltips.Insert(6, new TooltipLine(Mod, "mayo1", Language.GetTextValue(key + "HWJSniper")));
                    //tooltips.Insert(6, new TooltipLine(Mod, "mayo2", Language.GetTextValue(key + "HWJStarSniper")));
                }
                if (ModCompatibility.SacredTools.Loaded)
                {
                    tooltips.Insert(6, new TooltipLine(Mod, "mayo2", Language.GetTextValue(key + "SoASniper")));
                }
                if (ModCompatibility.Thorium.Loaded)
                {
                    tooltips.Insert(6, new TooltipLine(Mod, "mayo2", Language.GetTextValue(key + "ThoriumSniper")));
                }
            }
            if (ModCompatibility.Calamity.Loaded)
            {
                if (item.type == ModCompatibility.Calamity.Mod.Find<ModItem>("ElementalQuiver").Type )
                {
                    if (ModCompatibility.Homeward.Loaded)
                    {
                        //tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue(key + "HWJStarSniper")));
                    }
                }
            }
        }
        //[ExtendsFromMod(ModCompatibility.Homeward.Name)]
        //public class WalnutOnFireEffect : AccessoryEffect
        //{
        //    public override Header ToggleHeader => Header.GetHeader<UniverseHeader>();
        //    public override int ToggleItemType => ModContent.ItemType<WalnutOnFire>();

        //    public override void PostUpdateEquips(Player player)
        //    {
        //        ModCompatibility.Homeward.Mod.Find<ModItem>("WalnutOnFire").UpdateAccessory(player, true);
        //    }
        //}
        [ExtendsFromMod(ModCompatibility.Thorium.Name)]
        public class WarheadEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<UniverseHeader>();
            public override int ToggleItemType => ModContent.ItemType<ConcussiveWarhead>();

            public override void PostUpdateEquips(Player player)
            {
                ModCompatibility.Thorium.Mod.Find<ModItem>("ConcussiveWarhead").UpdateAccessory(player, true);
            }
        }
        [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
        public class InfAmmoEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<UniverseHeader>();
            public override int ToggleItemType => ModContent.ItemType<SnipersSoul>();
        }
        [ExtendsFromMod(ModCompatibility.Homeward.Name)]
        public class StarQuiverEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<UniverseHeader>();
            public override int ToggleItemType => ModContent.ItemType<StarQuiver>();

            public override void PostUpdateEquips(Player player)
            {
                ModCompatibility.Homeward.Mod.Find<ModItem>("StarQuiver").UpdateAccessory(player, true);
            }
        }
        [ExtendsFromMod(ModCompatibility.Homeward.Name)]
        public class TheBatterEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<UniverseHeader>();
            public override int ToggleItemType => ModContent.ItemType<TheBatter>();

            public override void PostUpdateEquips(Player player)
            {
                ModCompatibility.Homeward.Mod.Find<ModItem>("TheBatter").UpdateAccessory(player, true);
            }
        }
    }
}
