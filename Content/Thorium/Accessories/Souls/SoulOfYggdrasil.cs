using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using CSE.Core;
using System.Collections.Generic;
using CSE.Content.Thorium.Forces;
using FargowiltasSouls.Core.ModPlayers;
using FargowiltasSouls;
using Fargowiltas.Content.Items.Tiles;
using FargowiltasSouls.Content.Items.Materials;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler;

namespace CSE.Content.Thorium.Accessories.Souls
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class SoulOfYggdrasil : BaseSoul
    {
        public static List<int> Forces =
            [
            ModContent.ItemType<AlfheimForce>(),
            ModContent.ItemType<AsgardForce>(),
            ModContent.ItemType<HelheimForce>(),
            ModContent.ItemType<JotunheimForce>(),
            ModContent.ItemType<MidgardForce>(),
            ModContent.ItemType<MuspelheimForce>(),
            ModContent.ItemType<NiflheimForce>(),
            ModContent.ItemType<SvartalfheimForce>(),
            ModContent.ItemType<VanaheimForce>()
            ];
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.RegisterItemAnimation(Item.type, new DrawAnimationHorizontal(61, 6));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 50;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.value = 5000000;
            Item.rare = -12;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoSoulsPlayer modPlayer = player.FargoSouls();

            foreach (int force in Forces)
                modPlayer.ForceEffects.Add(force);


            player.AddEffect<YggdrasilEffect>(Item);


            ModContent.GetInstance<AlfheimForce>().UpdateAccessory(player, hideVisual);

            ModContent.GetInstance<AsgardForce>().UpdateAccessory(player, hideVisual);

            ModContent.GetInstance<HelheimForce>().UpdateAccessory(player, hideVisual);

            ModContent.GetInstance<JotunheimForce>().UpdateAccessory(player, hideVisual);

            ModContent.GetInstance<MidgardForce>().UpdateAccessory(player, hideVisual);

            ModContent.GetInstance<MuspelheimForce>().UpdateAccessory(player, hideVisual);

            ModContent.GetInstance<NiflheimForce>().UpdateAccessory(player, hideVisual);

            ModContent.GetInstance<SvartalfheimForce>().UpdateAccessory(player, hideVisual);

            ModContent.GetInstance<VanaheimForce>().UpdateAccessory(player, hideVisual);
        }

        public class YggdrasilEffect : AccessoryEffect
        {
            public override Header ToggleHeader => null;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            foreach (int force in Forces)
                recipe.AddIngredient(force);

            if (!ModCompatibility.Calamity.Loaded) { recipe.AddIngredient<AbomEnergy>(10); }

            recipe.AddTile<CrucibleCosmosSheet>();
            recipe.Register();
        }
    }
}