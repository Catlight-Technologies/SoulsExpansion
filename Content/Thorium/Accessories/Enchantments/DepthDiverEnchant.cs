using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.Depths;
using ThoriumMod.Items.Donate;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using CSE.Core;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.Thorium.Headers;
using FargowiltasSouls;
using static CSE.Content.Thorium.Forces.SvartalfheimForce;

namespace CSE.Content.Thorium.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class DepthDiverEnchant : BaseEnchant
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = 3;
            Item.value = 80000;
        }

        public override Color nameColor => new(11, 86, 255);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<DepthDiverEffect>(Item);
            //player.AddEffect<CoralEffect>(Item);
        }

        public class DepthDiverEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<SvartalfheimForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<DepthDiverEnchant>();

            public override void PostUpdateMiscEffects(Player player)
            {
                if (player.wet || player.ForceEffect<DepthDiverEffect>())
                {
                    float depthFactor = CalculateDepthFactor(player);

                    player.lifeRegen += (int)(1 + (player.HasEffect<SvartalfheimEffect>() ? 2 : 1) * depthFactor);
                    player.GetDamage(DamageClass.Generic) += (int)(0.02f + (player.HasEffect<SvartalfheimEffect>() ? 0.10 : 0.07) * depthFactor); 
                    player.statDefense += (int)(2 + (player.HasEffect<SvartalfheimEffect>() ? 0.10 : 0.07) * depthFactor); 
                }
            }

            private float CalculateDepthFactor(Player player)
            {
                float spaceHeight = (float)(Main.worldSurface * 0.35f * 16);

                float underworldHeight = (Main.maxTilesY - 200) * 16;

                float playerY = player.Center.Y;

                float depthFactor = (playerY - spaceHeight) / (underworldHeight - spaceHeight);

                return MathHelper.Clamp(depthFactor, 0f, 1f);
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<DepthDiverHelmet>());
            recipe.AddIngredient(ModContent.ItemType<DepthDiverChestplate>());
            recipe.AddIngredient(ModContent.ItemType<DepthDiverGreaves>());
            //recipe.AddIngredient(ModContent.ItemType<CoralEnchant>());
            //recipe.AddIngredient(ModContent.ItemType<DrownedDoubloon>());
            //recipe.AddIngredient(ModContent.ItemType<MagicConch>());

            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
