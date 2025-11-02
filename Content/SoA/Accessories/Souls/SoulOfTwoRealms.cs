using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using FargowiltasSouls.Content.Items.Materials;
using SacredTools.Content.Items.Materials;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using System.Collections.Generic;
using CSE.Core;
using static CSE.Content.SoA.Accessories.Enchantments.MarstechEnchant;
using static CSE.Content.SoA.Accessories.Enchantments.DreadfireEnchant;
using static CSE.Content.SoA.Accessories.Enchantments.SoranForcesEnchant;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.SoA.Accessories.Forces;
using CSE.Content.SoA.CraftingStation;
using FargowiltasSouls.Core.ModPlayers;
using FargowiltasSouls;

namespace CSE.Content.SoA.Accessories.Souls
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class SoulOfTwoRealms : BaseSoul
    {
        public override List<AccessoryEffect> ActiveSkillTooltips =>
            [AccessoryEffectLoader.GetEffect<MarstechEffect>(),
            AccessoryEffectLoader.GetEffect<DreadfireEffect>(),
            AccessoryEffectLoader.GetEffect<SoranForcesEffect>()];
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.RegisterItemAnimation(Item.type, new DrawAnimationHorizontal(60, 6));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }

        public static List<int> Forces =
            [
            ModContent.ItemType<FrostburnForce>(),
            ModContent.ItemType<SoranForce>(),
            ModContent.ItemType<SyranForce>(),
            ModContent.ItemType<GenerationsForce>(),
            ModContent.ItemType<FoundationsForce>()
            ];
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.value = 5000000;
            Item.rare = -12;
            Item.width = 59;
            Item.height = 51;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoSoulsPlayer modPlayer = player.FargoSouls();
            foreach (int force in Forces)
                modPlayer.ForceEffects.Add(force);


            ModContent.GetInstance<FrostburnForce>().UpdateAccessory(player, hideVisual);

            ModContent.GetInstance<SoranForce>().UpdateAccessory(player, hideVisual);

            ModContent.GetInstance<SyranForce>().UpdateAccessory(player, hideVisual);

            ModContent.GetInstance<GenerationsForce>().UpdateAccessory(player, hideVisual);

            ModContent.GetInstance<FoundationsForce>().UpdateAccessory(player, hideVisual);

            //player.buffImmune[ModContent.Find<ModBuff>(Mod.Name, "NihilityPresenceBuff").Type] = true;
            player.AddEffect<TwoRealmsEffect>(Item);
        }
        public class TwoRealmsEffect : AccessoryEffect
        {
            public override Header ToggleHeader => null;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            foreach (int force in Forces)
                recipe.AddIngredient(force);

            if (!ModCompatibility.Calamity.Loaded) { recipe.AddIngredient<AbomEnergy>(10); }
            if (ModCompatibility.Calamity.Loaded) { recipe.AddIngredient(ModCompatibility.Calamity.Mod.Find<ModItem>("ShadowspecBar"), 5); }
            if (ModCompatibility.Calamity.Loaded) { recipe.AddIngredient(ModCompatibility.Calamity.Mod.Find<ModItem>("MiracleMatter")); }

            recipe.AddIngredient<EmberOfOmen>(5);
            recipe.AddTile<SyranCraftingStationTile>();
            recipe.Register();
        }
    }
}
