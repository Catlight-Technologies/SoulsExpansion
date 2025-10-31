using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using FargowiltasSouls.Content.Items.Materials;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.Donate;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using ThoriumMod.Items.BossThePrimordials.Rhapsodist;
using CSE.Core;
using CSE.Content.Thorium.Materials;
using Fargowiltas.Content.Items.Tiles;
using Microsoft.Xna.Framework;

namespace CSE.Content.Thorium.Accessories.Souls
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class BardSoul : BaseSoul
    {
        public static readonly Color ItemColor = new(255, 0, 0);
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<BardDamage>() += 0.25f;
            player.GetCritChance<BardDamage>() += 0.10f;
            player.GetAttackSpeed<BardDamage>() += 0.15f;
            player.GetModPlayer<ThoriumPlayer>().bardBuffDuration += 3000;
            player.GetModPlayer<ThoriumPlayer>().inspirationRegenBonus += 1;
            player.GetModPlayer<ThoriumPlayer>().bardResourceDropBoost += 1;
            player.GetModPlayer<ThoriumPlayer>().bardResource += 20;
            player.GetModPlayer<ThoriumPlayer>().bardHomingSpeedBonus += 10;
            player.GetModPlayer<ThoriumPlayer>().bardHomingRangeBonus += 10;
            player.GetModPlayer<ThoriumPlayer>().bardBounceBonus = 10;

            player.GetModPlayer<ThoriumPlayer>().accWindHoming = true;
            player.GetModPlayer<ThoriumPlayer>().accBrassMute2 = true;
            player.GetModPlayer<ThoriumPlayer>().accPercussionTuner2 = true;

            if (ModLoader.HasMod("CalamityBardHealer"))
            {
                ModContent.Find<ModItem>("CalamityBardHealer", "OmniSpeaker").UpdateAccessory(player, false);
                ModContent.Find<ModItem>("CalamityBardHealer", "TreeWhispererAmulet").UpdateAccessory(player, false);
            }
            if (ModLoader.HasMod("ThoriumRework"))
            {
                ModContent.Find<ModItem>("ThoriumRework", "FanDonations").UpdateAccessory(player, false);
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            if (!ModLoader.HasMod("CalamityBardHealer"))
            {
                recipe.AddIngredient<BandKit>();
                recipe.AddIngredient<TerrariumSurroundSound>();
                recipe.AddRecipeGroup("CSE:AnyInstrumentTypeAccessory");
                recipe.AddIngredient<HauntingBassDrum>();
                recipe.AddIngredient<HellBell>();
                recipe.AddIngredient<JingleBells>();
                recipe.AddIngredient<CallofCthulhu>();
                //recipe.AddIngredient<DigitalTuner>();
                //recipe.AddIngredient<EpicMouthpiece>();
                if (ModCompatibility.Calamity.Loaded) { recipe.AddIngredient<BlackMIDI>(); }
                //recipe.AddIngredient<GuitarPickClaw>();
                //recipe.AddIngredient<StraightMute>();
                recipe.AddIngredient<Fishbone>();
                recipe.AddIngredient<SonicAmplifier>();
            }
            else
            {
                recipe.AddRecipeGroup("CSE:AnyInstrumentTypeAccessory");
                //recipe.AddIngredient<GuitarPickClaw>();
                //recipe.AddIngredient<EpicMouthpiece>();
                //recipe.AddIngredient<StraightMute>();
                //recipe.AddIngredient<DigitalTuner>();
                recipe.AddIngredient(ModContent.Find<ModItem>("CalamityBardHealer", "OmniSpeaker"));
                recipe.AddIngredient<BlackMIDI>();
                recipe.AddIngredient<TerrariumAutoharp>();
                recipe.AddIngredient(ModContent.Find<ModItem>("CalamityBardHealer", "TreeWhisperersHarp"));
                recipe.AddIngredient(ModContent.Find<ModItem>("CalamityBardHealer", "FeralKeytar"));
                recipe.AddIngredient(ModContent.Find<ModItem>("CalamityBardHealer", "FaceMelter"));
                recipe.AddIngredient(ModContent.Find<ModItem>("CalamityBardHealer", "SongoftheCosmos"));
                recipe.AddIngredient(ModContent.Find<ModItem>("CalamityBardHealer", "YharimsJam"));
            }

            if (ModLoader.HasMod("ThoriumRework"))
            {
                recipe.AddIngredient(ModContent.Find<ModItem>("ThoriumRework", "FanDonations"));
            }

            if (ModCompatibility.Calamity.Loaded) { recipe.AddIngredient<AbomEnergy>(10); }
            if (ModCompatibility.Homeward.Loaded)
            {
                recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Homeward.Name, "FinalBar"), 5);
            }

            recipe.AddIngredient<DreamEssence>(5);
            recipe.AddTile<CrucibleCosmosSheet>();

            recipe.Register();
        }
    }
}
