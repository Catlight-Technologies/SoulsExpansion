using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SacredTools;
using Microsoft.Xna.Framework;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using SacredTools.Content.Items.Armor.Lunar.Stardust;
using SacredTools.Items.Weapons.Lunatic;
using SacredTools.Items.Weapons.Oblivion;
using CSE.Core;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.SoA.Headers;


namespace CSE.Content.SoA.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class SpiritsMediumEnchant : BaseEnchant
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

        public override Color nameColor => new(108, 116, 204);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<SpiritsMediumEffect>(Item);
        }

        public class SpiritsMediumEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<SoranForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<SpiritsMediumEnchant>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<StellarPriestHead>();
            recipe.AddIngredient<StellarPriestChest>();
            recipe.AddIngredient<StellarPriestLegs>();
            recipe.AddIngredient<GalaxyScepter>();
            recipe.AddIngredient<LunarCrystalStaff>();
            recipe.AddIngredient<OblivionRod>();
            recipe.AddTile(412);
            recipe.Register();
        }
    }
}
