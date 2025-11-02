using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.Donate;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls;
using CSE.Core;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.Thorium.Headers;
using CSE.Content.Thorium.Buffs;

namespace CSE.Content.Thorium.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class CrierEnchant : BaseEnchant
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = 1;
            Item.value = 40000;
        }

        public override Color nameColor => new(53, 99, 33);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<CrierEffect>(Item);
        }

        public class CrierEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<VanaheimForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<CrierEnchant>();
            public override void OnHitNPCEither(Player player, NPC target, NPC.HitInfo hitInfo, DamageClass damageClass, int baseDamage, Projectile proj, Item item)
            {
                if (proj.CountsAsClass(BardDamage.Instance) || player.ForceEffect<CrierEffect>())
                {
                    if (Main.rand.NextFloat() < 0.1f) 
                    {
                        target.AddBuff(ModContent.BuffType<CryDebuff>(), 300); 
                    }
                }
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = this.CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<CriersCap>());
            recipe.AddIngredient(ModContent.ItemType<CriersSash>());
            recipe.AddIngredient(ModContent.ItemType<CriersLeggings>());
            recipe.AddIngredient(ModContent.ItemType<LuckyRabbitsFoot>());
            recipe.AddIngredient(ModContent.ItemType<WoodenWhistle>());
            recipe.AddRecipeGroup("CSE:AnyBugleHorn");

            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
