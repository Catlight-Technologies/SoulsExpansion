using Fargowiltas.Content.Items.Tiles;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using FargowiltasSouls.Content.Items.Summons;
using Redemption.Items.Accessories.HM;
using Redemption.Items.Accessories.PostML;
using Redemption.Items.Materials.PostML;
using Redemption.Items.Materials.PreHM;
using Redemption.Items.Usable;
using Redemption.Items.Weapons.HM.Magic;
using Redemption.Items.Weapons.HM.Melee;
using Redemption.Items.Weapons.HM.Ranged;
using Redemption.Items.Weapons.PostML.Magic;
using Redemption.Items.Weapons.PostML.Melee;
using Redemption.Items.Weapons.PostML.Ranged;
using Redemption.Items.Weapons.PostML.Summon;
using Redemption.Items.Weapons.PreHM.Magic;
using Redemption.Items.Weapons.PreHM.Melee;
using Redemption.Items.Weapons.PreHM.Ranged;
using Redemption.Items.Weapons.PreHM.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CSE.Core.MoR.ModSystems
{
    [ExtendsFromMod(ModCompatibility.Redemption.Name)]
    [JITWhenModsEnabled(ModCompatibility.Redemption.Name)]
    public class MoRRecipeChanges : ModSystem
    {
        public override void AddRecipes()
        {
            Recipe.Create(ItemType<PureIronAlloy>())
                .AddIngredient<DragonLeadAlloy>()
                .AddTile<CrucibleCosmosSheet>()
                .Register();
            Recipe.Create(ItemType<DragonLeadAlloy>())
                .AddIngredient<PureIronAlloy>()
                .AddTile<CrucibleCosmosSheet>()
                .Register();

            #region boss bags
            CSEUtils.CreateBagRecipes(ItemType<ThornBag>(),
            [
                ItemType<AldersStaff>(),
                ItemType<CursedGrassBlade>(),
                ItemType<RootTendril>(),
                ItemType<CursedThornBow>(),
            ]);
            CSEUtils.CreateBagRecipes(ItemType<ErhanBag>(),
            [
                ItemType<Bindeklinge>(),
                ItemType<HolyBible>(),
                ItemType<HallowedHandGrenade>(),
            ]);
            CSEUtils.CreateBagRecipes(ItemType<KeeperBag>(),
            [
                ItemType<SoulScepter>(),
                ItemType<KeepersClaw>(),
                ItemType<FanOShivs>(),
            ]);
            CSEUtils.CreateBagRecipes(ItemType<SoIBag>(),
            [
                ItemType<XenoXyston>(),
                ItemType<CystlingSummon>(),
                ItemType<ContagionSpreader>(),
            ]);
            CSEUtils.CreateBagRecipes(ItemType<SlayerBag>(),
            [
                ItemType<SlayerGun>(),
                ItemType<Nanoswarmer>(),
                ItemType<SlayerFist>(),
            ]);
            CSEUtils.CreateBagRecipes(ItemType<PZBag>(),
            [
                ItemType<PZGauntlet>(),
                ItemType<SwarmerCannon>(),
                ItemType<Petridish>(),
                ItemType<PortableHoloProjector>(),
            ]);
            CSEUtils.CreateBagRecipes(ItemType<UkkoBag>(),
            [
                ItemType<Salamanisku>(),
                ItemType<Ukonvasara>(),
                ItemType<UkonRuno>(),
            ]);
            CSEUtils.CreateBagRecipes(ItemType<AkkaBag>(),
            [
                ItemType<PoemOfIlmatar>(),
                ItemType<Pihlajasauva>(),
            ]);
            #endregion
        }

        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];
                if (recipe.createItem.ModItem is BaseForce || recipe.HasResult<SigilOfChampions>())
                {
                    if (!recipe.HasIngredient<RoboBrain>())
                        recipe.AddIngredient<RoboBrain>();
                }

                if (recipe.HasResult(ItemID.Zenith) && recipe.HasIngredient<LifeFragment>())
                {
                    recipe.RemoveIngredient(ItemType<LifeFragment>());
                }

                if (!ModCompatibility.Calamity.Loaded)
                {
                    if ((recipe.HasResult<UniverseSoul>() || recipe.HasResult<TerrariaSoul>() || recipe.HasResult<MasochistSoul>() || recipe.HasResult<DimensionSoul>()) && !recipe.HasIngredient<LifeFragment>())
                    {
                        recipe.AddIngredient<LifeFragment>(5);
                    }
                }

                #region souls
                if (recipe.HasResult<BerserkerSoul>())
                {
                    if(!ModCompatibility.SacredTools.Loaded)
                        recipe.AddIngredient<MutagenMelee>();

                    if (ModCompatibility.Calamity.Loaded)
                    {
                        recipe.AddIngredient<PZGauntlet>();
                    }
                    else
                    {
                        recipe.AddIngredient<PiercingNebulaWeapon>();
                    }
                }
                if (recipe.HasResult<SnipersSoul>())
                {
                    if (!ModCompatibility.SacredTools.Loaded)
                        recipe.AddIngredient<MutagenRanged>();

                    if (ModCompatibility.Calamity.Loaded)
                    {
                        recipe.AddIngredient<SwarmerCannon>();
                    }
                    else
                    {
                        recipe.AddIngredient<Twinklestar>();
                    }
                }
                if (recipe.HasResult<ArchWizardsSoul>())
                {
                    if (!ModCompatibility.SacredTools.Loaded)
                        recipe.AddIngredient<MutagenMagic>();

                    if (ModCompatibility.Calamity.Loaded)
                    {
                        recipe.AddIngredient<Petridish>();
                    }
                    else
                    {
                        recipe.AddIngredient<Constellations>();
                    }
                }
                if (recipe.HasResult<ConjuristsSoul>())
                {
                    if (!ModCompatibility.SacredTools.Loaded)
                        recipe.AddIngredient<MutagenSummon>();

                    if (ModCompatibility.Calamity.Loaded)
                    {
                        recipe.AddIngredient<PortableHoloProjector>();
                    }
                    else
                    {
                        recipe.AddIngredient<CosmosChains>();
                    }
                }

                if (recipe.HasResult<ColossusSoul>())
                {
                    recipe.AddIngredient<HEVSuit>();
                    //recipe.AddIngredient<PocketShieldGenerator>();
                }
                if (recipe.HasResult<SupersonicSoul>())
                {
                    if (!ModCompatibility.Calamity.Loaded)
                    {
                        recipe.RemoveIngredient(ItemID.EoCShield);
                        recipe.AddIngredient<InfectionShield>();
                    }
                }
                #endregion
            }
        }
    }
}