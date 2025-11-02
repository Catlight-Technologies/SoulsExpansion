using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.BardItems;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler;
using CSE.Core;
using CSE.Content.Thorium.Projectiles;
using CSE.Content.Thorium.Headers;


namespace CSE.Content.Thorium.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class JesterEnchant : BaseEnchant
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

        public override Color nameColor => new(104, 31, 91);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<JesterEffect>(Item);
        }
        public class JesterEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<MuspelheimForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<JesterEnchant>();
            public override bool MinionEffect => true;
            public override void PostUpdateMiscEffects(Player player)
            {
                if (player.ownedProjectileCounts[ModContent.ProjectileType<MinionBellProj>()] < 1)
                {
                    Projectile.NewProjectile(
                        player.GetSource_FromThis(),
                        player.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<MinionBellProj>(),
                        0,
                        0,
                        player.whoAmI
                    );
                }
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = this.CreateRecipe();

            recipe.AddRecipeGroup("CSE:AnyJesterMask");
            recipe.AddRecipeGroup("CSE:AnyJesterShirt");
            recipe.AddRecipeGroup("CSE:AnyJesterLeggings");
            recipe.AddRecipeGroup("CSE:AnyLetter");
            recipe.AddRecipeGroup("CSE:AnyTambourine");
            recipe.AddIngredient(ModContent.ItemType<SkywareLute>());

            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
