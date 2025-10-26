using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using SacredTools.Items.Weapons;
using SacredTools.Content.Items.Armor.Quasar;
using SacredTools.Items.Weapons.Primordia;
using CSE.Core;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.SoA.Headers;

namespace CSE.Content.SoA.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class QuasarEnchant : BaseEnchant
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = 10;
            Item.value = 300000;
        }

        public override Color nameColor => new(69, 95, 109);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<QuasarEffect>(Item);
        }

        public class QuasarEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<SoranForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<QuasarEnchant>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = this.CreateRecipe();
            recipe.AddIngredient<NovaHelmet>();
            recipe.AddIngredient<NovaBreastplate>();
            recipe.AddIngredient<NovaLegs>();
            recipe.AddIngredient<Ainfijarnar>();
            recipe.AddIngredient<NovaknifePack>();
            recipe.AddIngredient<NovaLance>();
            recipe.AddTile(412);
            recipe.Register();
        }
    }
}
