using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using SacredTools.Content.Items.Armor.Eerie;
using SacredTools.Items.Weapons;
using Microsoft.Xna.Framework.Graphics;
using FargowiltasSouls.Content.UI.Elements;
using CSE.Core;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.SoA.Projectiles;
using CSE.Content.SoA.Headers;
using Fargowiltas.Content.Items.Tiles;

namespace CSE.Content.SoA.Accessories.Enchantments
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class EerieEnchant : BaseEnchant
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = 2;
            Item.value = 70000;
        }

        public override Color nameColor => new(165, 37, 72);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<EerieEffect>(Item);
        }

        public class EerieEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<GenerationsForceHeader>();
            public override int ToggleItemType => ModContent.ItemType<EerieEnchant>();

            public int cd;
            public override void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo)
            {
                if (!player.HasEffectEnchant<EerieEffect>())
                    return;
                if (cd <= 0)
                {
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<EeriePulse>(), 0, 0);
                    cd = 60 * 15;
                }
            }   
            public override void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo)
            {
                if (!player.HasEffectEnchant<EerieEffect>())
                    return;
                if (cd <= 0)
                {
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<EeriePulse>(), 0, 0);
                    cd = 60 * 20;
                }
            }

            public override void PostUpdateEquips(Player player)
            {
                if (cd > 0)
                    cd--;
                CooldownBarManager.Activate("EerieEnchantCooldown", ModContent.Request<Texture2D>("CSE/Assets/Textures/Content/Items/Accessories/Enchantments/EerieEnchant").Value, new(237, 73, 78),
                    () => cd / (60f * 20), true, activeFunction: player.HasEffect<EerieEffect>);
            }
        }


        public override void AddRecipes()
        {
            Recipe recipe = this.CreateRecipe();
            recipe.AddIngredient<EerieChest>();
            recipe.AddIngredient<EerieLegs>();
            recipe.AddIngredient<EerieHelmet>();
            recipe.AddIngredient<EerieCane>();
            recipe.AddTile<EnchantedTreeSheet>();
            recipe.Register();
        }
    }
}
