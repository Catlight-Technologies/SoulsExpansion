using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using FargowiltasSouls.Content.UI.Elements;
using CSE.Core;
using static CSE.Content.SoA.Accessories.Enchantments.SoranForcesEnchant;
using CSE.Content.SoA.Accessories.Enchantments;
using static CSE.Content.SoA.Accessories.Enchantments.UnrelentingRivalEnchant;
using static CSE.Content.SoA.Accessories.Enchantments.FallenPrinceEnchant;
using static CSE.Content.SoA.Accessories.Enchantments.SpiritsMediumEnchant;
using static CSE.Content.SoA.Accessories.Enchantments.SavingGraceEnchant;
using FargowiltasSouls.Core.Toggler;
using static CSE.Content.SoA.Accessories.Enchantments.QuasarEnchant;


namespace CSE.Content.SoA.Accessories.Forces
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class SoranForce : BaseForce
    {
        public override List<AccessoryEffect> ActiveSkillTooltips =>
            [AccessoryEffectLoader.GetEffect<SoranForcesEffect>()];
        public override void SetStaticDefaults()
        {
            Enchants[Type] = new int[5]
            {
                ModContent.ItemType<SoranForcesEnchant>(),
                ModContent.ItemType<UnrelentingRivalEnchant>(),
                ModContent.ItemType<SavingGraceEnchant>(),
                ModContent.ItemType<SpiritsMediumEnchant>(),
                ModContent.ItemType<FallenPrinceEnchant>()
            };
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = 11;
            Item.value = 600000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<SoranForcesEffect>(Item);
            player.AddEffect<UnrelentingRivalEffect>(Item);
            player.AddEffect<SavingGraceEffect>(Item);
            player.AddEffect<SpiritsMediumEffect>(Item);
            player.AddEffect<FallenPrinceEffect>(Item);
            player.AddEffect<QuasarEffect>(Item);

            player.AddEffect<SoranEffect>(Item);
        }

        public class SoranEffect : AccessoryEffect
        {
            public override Header ToggleHeader => null;

            public override void PostUpdateEquips(Player player)
            {
                var SoranForcesPlayer = player.GetModPlayer<SoranForcesPlayer>();

                if (SoranForcesPlayer.SniperStateActive)
                {
                    player.aggro -= (int)(player.aggro * 0.5f);
                    player.statDefense = player.statDefense *= 0.75f;
                }
                else if (SoranForcesPlayer.SniperStateRecharging)
                {
                    player.statDefense = player.statDefense *= 1.30f;
                }

                CooldownBarManager.Activate("SoranForcesCooldown", ModContent.Request<Texture2D>("CSE/Assets/Textures/Content/Items/Accessories/Enchantments/SoranForcesEnchant").Value, new(21, 142, 100),
                    () => SoranForcesPlayer.SniperStateCharge / (60f * 15), true, activeFunction: player.HasEffect<SoranForcesEffect>);
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            int[] array = Enchants[Type];
            foreach (int itemID in array)
            {
                recipe.AddIngredient(itemID);
            }

            recipe.AddTile(ModContent.Find<ModTile>("Fargowiltas", "CrucibleCosmosSheet"));
            recipe.Register();
        }
    }
}
