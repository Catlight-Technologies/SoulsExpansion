using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.ArcaneArmor;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls;
using CSE.Core;
using CSE.Content.Thorium.Headers;
using FargowiltasSouls.Core.Toggler;
using static CSE.Content.Thorium.Forces.VanaheimForce;
using static CSE.Content.Thorium.Accessories.Souls.SoulOfYggdrasil;
using Fargowiltas.Content.Items.Tiles;

namespace CSE.Content.Thorium.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class YewWoodEnchant : BaseEnchant
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = 2;
            Item.value = 60000;
        }

        public override Color nameColor => new(48, 63, 47);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<YewWoodEffect>(Item);
        }

        public class YewWoodEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<VanaheimForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<YewWoodEnchant>();
            public override bool ExtraAttackEffect => true;
            public static int BaseDamage(Player player)
            {
                int dmg = 2;
                if (player.HasEffect<VanaheimEffect>() && !player.HasEffect<YggdrasilEffect>())
                    dmg = 10;
                if (player.HasEffect<YggdrasilEffect>())
                    dmg = 100;
                return (int)(dmg * player.ActualClassDamage(DamageClass.Ranged));
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = this.CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<YewWoodHelmet>());
            recipe.AddIngredient(ModContent.ItemType<YewWoodBreastguard>());
            recipe.AddIngredient(ModContent.ItemType<YewWoodLeggings>());
            recipe.AddIngredient(ModContent.ItemType<ThumbRing>());
            recipe.AddIngredient(ModContent.ItemType<YewWoodBow>());
            recipe.AddIngredient(ModContent.ItemType<YewWoodFlintlock>());


            recipe.AddTile<EnchantedTreeSheet>();
            recipe.Register();
        }

        public override int DamageTooltip(out DamageClass damageClass, out Color? tooltipColor, out int? scaling)
        {
            damageClass = DamageClass.Ranged;
            tooltipColor = null;
            scaling = null;
            return YewWoodEffect.BaseDamage(Main.LocalPlayer);
        }
    }
}
