using FargowiltasSouls.Content.Items.Accessories.Forces;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using FargowiltasSouls.Content.Items.Materials;
using SacredTools.Content.Items.Accessories;
using SacredTools.Content.Items.Accessories.Wings;
using SacredTools.Content.Items.Armor.Asthraltite;
using SacredTools.Content.Items.Armor.Dragon;
using SacredTools.Content.Items.Armor.Oblivion;
using SacredTools.Content.Items.Materials;
using SacredTools.Content.Items.Placeable.Obelisks;
using SacredTools.Content.Items.Weapons.Relic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CSE.Core.SoA.ModSystems
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class SoARecipeChanges : ModSystem
    {
        public override void AddRecipeGroups()
        {
            RecipeGroup rec = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Asthral Helmet", ModContent.ItemType<AsthralMage>(), ModContent.ItemType<AsthralRanged>(), ModContent.ItemType<AsthralMelee>(), ModContent.ItemType<AsthralSummon>(), ModContent.ItemType<AsthraltiteHelmetRevenant>());
            RecipeGroup.RegisterGroup("CSE:AsthralHelms", rec);
            RecipeGroup rec2 = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Flarium Helmet", ModContent.ItemType<FlariumCrown>(), ModContent.ItemType<FlariumMask>(), ModContent.ItemType<FlariumCowl>());
            RecipeGroup.RegisterGroup("CSE:FlariumHelms", rec2);
            RecipeGroup rec3 = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Void Warden Chestplate", ModContent.ItemType<VoidChest>(), ModContent.ItemType<VoidChestOffense>());
            RecipeGroup.RegisterGroup("CSE:VoidWardenChest", rec3);
        }
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe.HasResult<TrawlerSoul>())
                {
                    recipe.AddIngredient<LunarRing>();
                    recipe.RemoveIngredient(ItemID.GreedyRing);
                }

                if (recipe.HasResult<CosmoForce>() && !recipe.HasIngredient<LuminousEnergy>())
                {
                    recipe.AddIngredient<LuminousEnergy>(5);
                }

                if (recipe.HasResult<NihilusObelisk>() && !recipe.HasIngredient<AbomEnergy>())
                {
                    recipe.AddIngredient<AbomEnergy>(5);
                }

                if (recipe.HasResult<FlightMasterySoul>() && !recipe.HasIngredient<GrandWings>())
                {
                    recipe.AddIngredient<GrandWings>();
                    recipe.AddIngredient<DespairBoosters>();
                    recipe.AddIngredient<AuroraWings>();
                    recipe.AddIngredient<FlariumWings>();
                }

                if (recipe.createItem.ModItem is BaseForce)
                {
                    if (!recipe.HasIngredient<TraceOfChaos>())
                        recipe.AddIngredient<TraceOfChaos>(4);
                }

                if (recipe.HasResult<UniverseSoul>() || recipe.HasResult<TerrariaSoul>() || recipe.HasResult<MasochistSoul>() || recipe.HasResult<DimensionSoul>())
                {
                    if (!recipe.HasIngredient<EmberOfOmen>())
                    {
                        recipe.AddIngredient<EmberOfOmen>(5);
                    }
                }

                if ((recipe.HasResult<PaleRuin>() ||
                    recipe.HasResult<AshenWake>() ||
                    recipe.HasResult<CeruleanCyclone>() ||
                    recipe.HasResult<Malevolence>() ||
                    recipe.HasResult<NightTerror>() ||
                    recipe.HasResult<RogueWave>() ||
                    recipe.HasResult<Sharpshooter>() ||
                    recipe.HasResult<SwordOfGreed>()) && !recipe.HasIngredient<AbomEnergy>())
                {
                    if (!ModCompatibility.Calamity.Loaded)
                    {
                        recipe.AddIngredient<AbomEnergy>(5);
                    }
                    else
                    {
                        recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Calamity.Name, "ShadowspecBar"), 5);
                    }
                }

                //if ((recipe.HasResult<AsthraltiteHelmetRevenant>() ||
                //    recipe.HasResult<AsthralRanged>() ||
                //    recipe.HasResult<AsthralMelee>() ||
                //    recipe.HasResult<AsthralChest>() ||
                //    recipe.HasResult<AsthralMage>() ||
                //    recipe.HasResult<AsthralLegs>() ||
                //    recipe.HasResult<AsthralSummon>()) && !recipe.HasIngredient<AbomEnergy>())
                //{
                //    recipe.AddIngredient<AbomEnergy>(5);
                //}
            }
        }
    }
}