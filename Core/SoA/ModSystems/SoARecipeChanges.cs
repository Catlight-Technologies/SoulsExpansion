using FargowiltasSouls.Content.Items.Accessories.Eternity;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using FargowiltasSouls.Content.Items.Materials;
using SacredTools.Content.Items.Accessories;
using SacredTools.Content.Items.Accessories.Sigils;
using SacredTools.Content.Items.Accessories.Wings;
using SacredTools.Content.Items.Armor.Asthraltite;
using SacredTools.Content.Items.Armor.Dragon;
using SacredTools.Content.Items.Armor.Oblivion;
using SacredTools.Content.Items.Materials;
using SacredTools.Content.Items.Placeable.Obelisks;
using SacredTools.Content.Items.Weapons.Relic;
using SacredTools.Items.Weapons;
using SacredTools.Items.Weapons.Lunatic;
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

                #region souls
                if (recipe.HasResult<BerserkerSoul>())
                {
                    //gauntlet replacement
                    if (!ModCompatibility.Calamity.Loaded && !ModCompatibility.Homeward.Loaded)
                    {
                        recipe.RemoveIngredient(ItemID.FireGauntlet);
                        recipe.AddIngredient<FloraFist>();
                    }

                    recipe.AddIngredient<TrueMoonEdgedPandolarra>();

                    recipe.AddIngredient<SolarSigil>();
                }
                if (recipe.HasResult<SnipersSoul>())
                {
                    recipe.AddIngredient<DolphinGun>();
                    recipe.RemoveIngredient(ItemID.Megashark);

                    recipe.AddIngredient<VortexSigil>();
                }
                if (recipe.HasResult<ArchWizardsSoul>())
                {
                    recipe.AddIngredient<NubasBlessing>();

                    recipe.AddIngredient<LunaticBurstStaff>();

                    recipe.AddIngredient<NebulaSigil>();
                }
                if (recipe.HasResult<ConjuristsSoul>())
                {
                    recipe.RemoveIngredient(ItemID.MonkBelt);
                    recipe.RemoveIngredient(ItemID.SquireShield);
                    recipe.RemoveIngredient(ItemID.HuntressBuckler);
                    recipe.RemoveIngredient(ItemID.ApprenticeScarf);
                    recipe.RemoveRecipeGroup(RecipeGroup.recipeGroupIDs["FargowiltasSouls:AnySentryAccessory"]);
                    recipe.AddIngredient<StarstreamVeil>();

                    recipe.AddIngredient<GalaxyScepter>();

                    recipe.AddIngredient<StardustSigil>();
                }
                if (recipe.HasResult<TrawlerSoul>())
                {
                    recipe.AddIngredient<DecreeCharm>();
                    recipe.AddIngredient<LunarRing>();
                    recipe.RemoveIngredient(ItemID.GreedyRing);
                    recipe.RemoveIngredient(ItemID.CelestialShell);
                }
                if (recipe.HasResult<SupersonicSoul>())
                {
                    recipe.AddIngredient<HeartOfThePlough>();
                    if (!ModCompatibility.Homeward.Loaded)
                    {
                        recipe.RemoveIngredient(ModContent.ItemType<AeolusBoots>());
                        recipe.AddIngredient<VoidSpurs>();
                    }
                }
                if (recipe.HasResult<FlightMasterySoul>() && !recipe.HasIngredient<GrandWings>())
                {
                    recipe.AddIngredient<GrandWings>();
                    recipe.AddIngredient<DespairBoosters>();
                    recipe.AddIngredient<AuroraWings>();
                    recipe.AddIngredient<FlariumWings>();
                }
                #endregion

                if (recipe.HasResult<CosmoForce>() && !recipe.HasIngredient<LuminousEnergy>())
                {
                    recipe.AddIngredient<LuminousEnergy>(5);
                }

                if (recipe.HasResult<VoidSpurs>())
                {
                    if (ModCompatibility.Thorium.Loaded)
                    {
                        recipe.RemoveIngredient(ModContent.ItemType<RoyalRunners>());
                        recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Thorium.Name, "TerrariumParticleSprinters"));
                    }
                    else
                    {
                        recipe.AddIngredient<AeolusBoots>();
                        recipe.RemoveIngredient(ModContent.ItemType<RoyalRunners>());
                    }
                }

                if (recipe.HasResult<AeolusBoots>())
                {
                    recipe.AddIngredient<RoyalRunners>();
                    if (!ModCompatibility.Calamity.Loaded)
                    {
                        recipe.RemoveIngredient(ModContent.ItemType<ZephyrBoots>());
                    }
                    else
                    {
                        recipe.RemoveIngredient(ModContent.Find<ModItem>(ModCompatibility.Calamity.Name, "AngelTreads").Type);
                    }
                }

                if (recipe.HasResult<RoyalRunners>())
                {
                    if (ModCompatibility.Calamity.Loaded)
                    {
                        recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Calamity.Name, "AngelTreads"));
                        recipe.RemoveIngredient(ItemID.TerrasparkBoots);
                    }
                    else
                    {
                        recipe.RemoveIngredient(ItemID.TerrasparkBoots);
                        recipe.AddIngredient<ZephyrBoots>();
                    }
                }

                if (recipe.HasResult<NihilusObelisk>() && !recipe.HasIngredient<AbomEnergy>())
                {
                    recipe.AddIngredient<AbomEnergy>(5);
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

                if (recipe.HasIngredient<CelestialShield>() && ModCompatibility.Homeward.Loaded)
                {
                    recipe.RemoveIngredient(ItemID.CelestialShell);
                    recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Homeward.Name, "AncientBlessing"));
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

                if (ModCompatibility.Redemption.Loaded)
                {
                    if (recipe.HasResult<NebulaSigil>())
                    {
                        recipe.RemoveIngredient(ItemID.SorcererEmblem);
                        recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Redemption.Name, "MutagenMagic"));
                    }
                    if (recipe.HasResult<SolarSigil>())
                    {
                        recipe.RemoveIngredient(ItemID.WarriorEmblem);
                        recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Redemption.Name, "MutagenMelee"));
                    }
                    if (recipe.HasResult<StardustSigil>())
                    {
                        recipe.RemoveIngredient(ItemID.SummonerEmblem);
                        recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Redemption.Name, "MutagenSummon"));
                    }
                    if (recipe.HasResult<VortexSigil>())
                    {
                        recipe.RemoveIngredient(ItemID.SorcererEmblem);
                        recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Redemption.Name, "MutagenRanged"));
                    }
                }
            }
        }
    }
}