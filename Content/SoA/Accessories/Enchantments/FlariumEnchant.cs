using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using SacredTools.Content.Items.Armor.Dragon;
using SacredTools.Items.Weapons.Flarium;
using SacredTools.Items.Weapons.Special;
using SacredTools.Items.Mount;
using CSE.Core;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.SoA.Headers;
using Fargowiltas.Content.Items.Tiles;
using static CSE.Content.Thorium.Accessories.Enchantments.YewWoodEnchant;
using FargowiltasSouls;
using static CSE.Content.Thorium.Accessories.Souls.SoulOfYggdrasil;
using static CSE.Content.Thorium.Forces.VanaheimForce;
using static CSE.Content.SoA.Accessories.Forces.FrostburnForce;
using static CSE.Content.SoA.Accessories.Souls.SoulOfTwoRealms;

namespace CSE.Content.SoA.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class FlariumEnchant : BaseEnchant
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

        public override Color nameColor => new(204, 78, 40);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<FlariumEffect>(Item);
        }

        public class FlariumEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<FrostburnForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<FlariumEnchant>();

            public static int BaseDamage(Player player)
            {
                int dmg = 100;
                if (player.HasEffect<FrostburnEffect>() && !player.HasEffect<TwoRealmsEffect>())
                    dmg = 200;
                if (player.HasEffect<TwoRealmsEffect>())
                    dmg = 1000;
                return (int)(dmg * player.ActualClassDamage(DamageClass.Generic));
            }
        }

        public override int DamageTooltip(out DamageClass damageClass, out Color? tooltipColor, out int? scaling)
        {
            damageClass = DamageClass.Generic;
            tooltipColor = null;
            scaling = null;
            return FlariumEffect.BaseDamage(Main.LocalPlayer);
        }
        public override void AddRecipes()
        {
            Recipe recipe = this.CreateRecipe();
            recipe.AddRecipeGroup("CSE:FlariumHelms");
            recipe.AddIngredient<FlariumChest>();
            recipe.AddIngredient<FlariumLeggings>();
            recipe.AddIngredient<FlariumRocketLauncher>();
            recipe.AddIngredient<SolusKatana>();
            recipe.AddIngredient<SerpentSceptre>();
            recipe.AddTile<EnchantedTreeSheet>();
            recipe.Register();
        }
    }
}
