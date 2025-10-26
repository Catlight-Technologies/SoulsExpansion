using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using SacredTools.Content.Items.Armor.Exodus;
using SacredTools.Content.Items.LuxShards;
using SacredTools.Items.Weapons.Luxite;
using SacredTools.Common.GlobalItems;
using CSE.Core;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.SoA.Headers;

namespace CSE.Content.SoA.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class ExitumLuxEnchant : BaseEnchant
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

        public override Color nameColor => new(137, 154, 178);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<ExitumLuxEffect>(Item);
        }

        public class ExitumLuxEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<FrostburnForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<ExitumLuxEnchant>();

            public override void PostUpdateEquips(Player player)
            {
                if(player.HeldItem.ModItem is IRelicItem)
                {
                    player.GetDamage<GenericDamageClass>() += 0.1f;
                    player.GetCritChance<GenericDamageClass>() += 0.1f;
                    player.GetAttackSpeed<GenericDamageClass>() += 0.1f;
                }
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = this.CreateRecipe();
            recipe.AddIngredient<ExodusHelmet>();
            recipe.AddIngredient<ExodusChest>();
            recipe.AddIngredient<ExodusLegs>();
            recipe.AddIngredient<LuxDustThrown>();
            recipe.AddIngredient<Claymarine>();
            recipe.AddTile(412);
            recipe.Register();
        }
    }
}
