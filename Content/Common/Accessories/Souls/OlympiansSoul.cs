using CalamityMod.CalPlayer;
using CalamityMod;
using CSE.Core.Common.ModPlayers;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using CSE.Core;
using CalamityMod.Items.Accessories;
using ThoriumMod.Items.Donate;
using System.Collections.Generic;
using Terraria.Localization;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using Fargowiltas.Content.Items.Tiles;
using FargowiltasSouls.Content.Items.Materials;
using FargowiltasSouls.Core.Toggler;
using FargowiltasSouls.Core.Toggler.Content;
using CSE.Content.Thorium.Materials;
using ThoriumMod.Utilities;

namespace CSE.Content.Common.Accessories.Souls
{
    public class OlympianSoul : BaseSoul
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModCompatibility.SacredTools.Loaded || ModCompatibility.Crossmod.Loaded || ModCompatibility.Thorium.Loaded;
        }

        public static readonly Color ItemColor = new(0, 255, 0);
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<ThrowingDamageClass>() += 0.25f;
            //player.GetModPlayer<CSEPlayer>().throwerVelocity += 0.2f;
            player.GetCritChance<ThrowingDamageClass>() += 10;
            player.ThrownVelocity += 0.1f;

            if (ModCompatibility.Thorium.Loaded)
            {
                player.AddEffect<PlagueLordEffect>(Item);
                ModContent.Find<ModItem>(ModCompatibility.Thorium.Name, "ThrowingGuideVolume3").UpdateAccessory(player, false);
            }
            if (ModCompatibility.Crossmod.Loaded)
            {
                player.AddEffect<NanotechEffectCSE>(Item);
            }
        }
        public override void SafeModifyTooltips(List<TooltipLine> tooltips)
        {
            if (ModCompatibility.Calamity.Loaded)
            {
                tooltips.Insert(6, new TooltipLine(Mod, "cal", Language.GetTextValue("Mods.CSE.AddedEffects.CalThrower")));
                tooltips.Insert(6, new TooltipLine(Mod, "cal", Language.GetTextValue("Mods.CSE.AddedEffects.CalThrower2")));
                tooltips.Insert(6, new TooltipLine(Mod, "cal", Language.GetTextValue("Mods.CSE.AddedEffects.CalThrower3")));
            }
            if (ModCompatibility.Thorium.Loaded)
            {
                tooltips.Insert(6, new TooltipLine(Mod, "thorium", Language.GetTextValue("Mods.CSE.AddedEffects.ThoriumThrower")));
                tooltips.Insert(6, new TooltipLine(Mod, "thorium", Language.GetTextValue("Mods.CSE.AddedEffects.ThoriumThrower2")));
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            if (ModCompatibility.Calamity.Loaded)
            {
                recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Calamity.Name, "Nanotech"));
                recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Calamity.Name, "Valediction"));
                recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Calamity.Name, "GodsParanoia"));
                recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Calamity.Name, "Eradicator"));
                recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Calamity.Name, "Wrathwing"));
                recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Calamity.Name, "Seraphim"));

                recipe.AddIngredient<AbomEnergy>(10);
            }

            if (ModCompatibility.Thorium.Loaded)
            {
                recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Thorium.Name, "PlagueLordFlask"));
                recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Thorium.Name, "ThrowingGuideVolume3"));
                recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Thorium.Name, "TerrariumRippleKnife"));

                recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Thorium.Name, "TerraKnife"));
                recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Thorium.Name, "AngelsEnd"));
                recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Thorium.Name, "Brinefang"));
                recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Thorium.Name, "DragonFang"));

                recipe.AddIngredient<DreamEssence>(5);
            }

            if (ModCompatibility.Homeward.Loaded)
            {
                recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Homeward.Name, "FinalBar"), 5);
            }

            if (ModCompatibility.SacredTools.Loaded)
            {
                recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.SacredTools.Name, "QuasarSigil"));
                recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.SacredTools.Name, "BlindJustice"));

                recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.SacredTools.Name, "OrbFlayer"));
                recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.SacredTools.Name, "Ainfijarnar"));
            }

            recipe.AddTile<CrucibleCosmosSheet>();
            recipe.Register();
        }
    }

    [JITWhenModsEnabled(ModCompatibility.Crossmod.Name)]
    [ExtendsFromMod(ModCompatibility.Crossmod.Name)]
    public class NanotechEffectCSE : AccessoryEffect
    {
        public override Header ToggleHeader => Header.GetHeader<UniverseHeader>();
        public override int ToggleItemType => ModContent.ItemType<Nanotech>();

        public override void PostUpdateEquips(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.nanotech = true;
            modPlayer.raiderTalisman = true;
            modPlayer.electricianGlove = true;
            modPlayer.filthyGlove = true;
            modPlayer.bloodyGlove = true;
        }
    }

    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    public class PlagueLordEffect : AccessoryEffect
    {
        public override Header ToggleHeader => Header.GetHeader<UniverseHeader>();
        public override int ToggleItemType => ModContent.ItemType<PlagueLordFlask>();

        public override void PostUpdateEquips(Player player)
        {
            player.GetThoriumPlayer().blightAcc = true;
        }
    }
}