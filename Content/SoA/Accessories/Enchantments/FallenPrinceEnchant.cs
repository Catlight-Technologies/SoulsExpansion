using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using SacredTools.Content.Items.Armor.Lunar.Quasar;
using SacredTools.Items.Weapons.Lunatic;
using CSE.Core;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.SoA.Headers;
using static CSE.Content.SoA.Accessories.Enchantments.QuasarEnchant;

namespace CSE.Content.SoA.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class FallenPrinceEnchant : BaseEnchant
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = 11;
            Item.value = 350000;
        }

        public override Color nameColor => new(91, 94, 122);


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<FallenPrinceEffect>(Item);
            player.AddEffect<QuasarEffect>(Item);
        }

        public class FallenPrinceEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<SoranForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<FallenPrinceEnchant>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = this.CreateRecipe();
            recipe.AddIngredient<FallenPrinceHelm>();
            recipe.AddIngredient<FallenPrinceChest>();
            recipe.AddIngredient<FallenPrinceBoots>();
            recipe.AddIngredient<QuasarEnchant>();
            recipe.AddIngredient<CosmicDesolation>();
            recipe.AddIngredient<LunaticsGamble>();
            recipe.AddTile(412);
            recipe.Register();
        }
    }
}
