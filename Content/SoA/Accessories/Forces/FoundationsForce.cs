using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using CSE.Content.SoA.Accessories.Enchantments;
using CSE.Core;
using FargowiltasSouls.Core.Toggler;
using static CSE.Content.SoA.Accessories.Enchantments.MarstechEnchant;
using static CSE.Content.SoA.Accessories.Enchantments.LapisEnchant;
using static CSE.Content.SoA.Accessories.Enchantments.BismuthEnchant;
using System.Collections.Generic;
using static CSE.Content.SoA.Accessories.Enchantments.SpaceJunkEnchant;

namespace CSE.Content.SoA.Accessories.Forces
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class FoundationsForce : BaseForce
    {
        public override List<AccessoryEffect> ActiveSkillTooltips =>
            [AccessoryEffectLoader.GetEffect<MarstechEffect>()];
        public override void SetStaticDefaults()
        {
            Enchants[Type] = new int[3]
            {
                ModContent.ItemType<MarstechEnchant>(),
                ModContent.ItemType<BismuthEnchant>(),
                ModContent.ItemType<LapisEnchant>()
            };
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = 11;
            Item.value = 600000;
        }

        public class FoundationsEffect : AccessoryEffect
        {
            public override Header ToggleHeader => null;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<MarstechEffect>(Item);
            player.AddEffect<BismuthEffect>(Item);
            player.AddEffect<LapisDefenseEffect>(Item);
            player.AddEffect<LapisSpeedEffect>(Item);
            player.AddEffect<SpaceJunkEffect>(Item);

            player.AddEffect<FoundationsEffect>(Item);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            int[] array = Enchants[Type];
            foreach (int itemID in array)
            {
                recipe.AddIngredient(itemID);
            }

            recipe.AddTile(ModContent.Find<ModTile>("Fargowiltas", "CrucibleCosmosSheet"));
            recipe.Register();
        }
    }
}