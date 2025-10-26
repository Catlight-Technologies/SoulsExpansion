using FargowiltasSouls.Content.Items.Accessories.Souls;
using Terraria.ModLoader;
using Terraria;
using System.Collections.Generic;
using Terraria.Localization;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler.Content;
using SacredTools.Content.Items.Accessories;
using ContinentOfJourney.Items.Accessories;
using Redemption.Items.Accessories.HM;
using ThoriumMod.Items.BossThePrimordials;
using Terraria.ID;
using FargowiltasSouls.Core.Toggler;
using ThoriumMod.Items.Terrarium;

namespace CSE.Core.SoulsRecipes
{
    public class SupersonicSoulRecipe : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe.HasResult(ModContent.ItemType<SupersonicSoul>()))
                {
                    if (ModCompatibility.SacredTools.Loaded) { recipe.AddIngredient(ModCompatibility.SacredTools.Mod.Find<ModItem>("HeartOfThePlough"), 1); }
                    if (ModCompatibility.Homeward.Loaded) { recipe.AddIngredient(ModCompatibility.Homeward.Mod.Find<ModItem>("HourHand"), 1); /*recipe.AddIngredient(ModCompatibility.Homeward.Mod.Find<ModItem>("Edgewalker"), 1);*/ }
                    if (ModCompatibility.Redemption.Loaded) { recipe.AddIngredient(ModCompatibility.Redemption.Mod.Find<ModItem>("InfectionShield"), 1); recipe.RemoveIngredient(ItemID.EoCShield); }
                    if (ModCompatibility.Thorium.Loaded) { /*recipe.AddIngredient(ModCompatibility.Thorium.Mod.Find<ModItem>("TheOmegaCore"), 1);*/ /*recipe.AddIngredient(ModCompatibility.Thorium.Mod.Find<ModItem>("SweetVengeance"), 1);*/ recipe.RemoveIngredient(ItemID.SweetheartNecklace); recipe.RemoveIngredient(ItemID.PanicNecklace); }
                }
            }
        }
    }
    public class SupersonicSoulEffects : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override void UpdateAccessory(Item Item, Player player, bool hideVisual)
        {
            if (Item.type == ModContent.ItemType<SupersonicSoul>() || Item.type == ModContent.ItemType<DimensionSoul>() || Item.type == ModContent.ItemType<EternitySoul>())
            {
                if (ModCompatibility.Redemption.Loaded)
                {
                    player.AddEffect<InfectionShieldEffect>(Item);
                }
                if (ModCompatibility.Homeward.Loaded)
                {
                    player.AddEffect<HourHandEffect>(Item);
                    //player.AddEffect<EdgewalkerEffect>(Item);
                }
                if (ModCompatibility.SacredTools.Loaded)
                {
                    player.AddEffect<HeartOfThePloughEffect>(Item);
                }
                if (ModCompatibility.Thorium.Loaded)
                {
                    //player.AddEffect<TheOmegaCoreEffect>(Item);
                }
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string key = "Mods.CSE.AddedEffects.";

            if (item.type == ModContent.ItemType<SupersonicSoul>() )
            {
                if (ModCompatibility.Redemption.Loaded)
                {
                    //tooltips.Insert(5, new TooltipLine(Mod, "mayo1", Language.GetTextValue(key + "RedSupersonic")));
                }
                if (ModCompatibility.SacredTools.Loaded)
                {
                    tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue(key + "SoASupersonic")));
                }
                if (ModCompatibility.Homeward.Loaded)
                {
                    //tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue(key + "HWJSupersonic")));
                }
                if (ModCompatibility.Thorium.Loaded)
                {
                    //tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue(key + "ThoriumSupersonic")));
                }
            }
            if (ModCompatibility.Homeward.Loaded)
            {
                if (item.type == ModCompatibility.Homeward.Mod.Find<ModItem>("Horizon").Type )
                {
                    tooltips.Insert(5, new TooltipLine(Mod, "mayo3", Language.GetTextValue("Mods.CSE.AddedEffects.Aeolus")));
                }
            }
            if (ModCompatibility.Thorium.Loaded)
            {
                if (item.type == ModCompatibility.Thorium.Mod.Find<ModItem>("TerrariumParticleSprinters").Type)
                {
                    tooltips.Insert(5, new TooltipLine(Mod, "mayo3", Language.GetTextValue("Mods.CSE.AddedEffects.Aeolus")));
                }
            }
            if (ModCompatibility.SacredTools.Loaded)
            {
                if (item.type == ModCompatibility.SacredTools.Mod.Find<ModItem>("VoidSpurs").Type)
                {
                    tooltips.Insert(5, new TooltipLine(Mod, "mayo3", Language.GetTextValue("Mods.CSE.AddedEffects.Aeolus")));
                }
                if (item.type == ModCompatibility.SacredTools.Mod.Find<ModItem>("RoyalRunners").Type)
                {
                    tooltips.Insert(5, new TooltipLine(Mod, "mayo3", Language.GetTextValue("Mods.CSE.AddedEffects.Zephyr")));
                }
            }
        }
        [ExtendsFromMod(ModCompatibility.Thorium.Name)]
        public class TheOmegaCoreEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<SupersonicHeader>();
            public override int ToggleItemType => ModContent.ItemType<TheOmegaCore>();

            public override void PostUpdateEquips(Player player)
            {
                ModCompatibility.Thorium.Mod.Find<ModItem>("TheOmegaCore").UpdateAccessory(player, true);
            }
        }
        [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
        public class HeartOfThePloughEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<SupersonicHeader>();
            public override int ToggleItemType => ModContent.ItemType<HeartOfThePlough>();

            public override void PostUpdateEquips(Player player)
            {
                ModCompatibility.SacredTools.Mod.Find<ModItem>("HeartOfThePlough").UpdateAccessory(player, true);
            }
        }
        [ExtendsFromMod(ModCompatibility.Redemption.Name)]
        public class InfectionShieldEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<SupersonicHeader>();
            public override int ToggleItemType => ModContent.ItemType<InfectionShield>();

            public override void PostUpdateEquips(Player player)
            {
                ModCompatibility.Redemption.Mod.Find<ModItem>("InfectionShield").UpdateAccessory(player, true);
            }
        }
        [ExtendsFromMod(ModCompatibility.Homeward.Name)]
        public class HourHandEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<SupersonicHeader>();
            public override int ToggleItemType => ModContent.ItemType<HourHand>();

            public override void PostUpdateEquips(Player player)
            {
                ModCompatibility.Homeward.Mod.Find<ModItem>("HourHand").UpdateAccessory(player, true);
            }
        }
        //[ExtendsFromMod(ModCompatibility.Homeward.Name)]
        //public class EdgewalkerEffect : AccessoryEffect
        //{
        //    public override Header ToggleHeader => Header.GetHeader<SupersonicHeader>();
        //    public override int ToggleItemType => ModContent.ItemType<Edgewalker>();

        //    public override void PostUpdateEquips(Player player)
        //    {
        //        ModCompatibility.Homeward.Mod.Find<ModItem>("Edgewalker").UpdateAccessory(player, true);
        //    }
        //}
    }
}
