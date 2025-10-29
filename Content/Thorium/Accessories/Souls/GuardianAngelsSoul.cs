using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;
using FargowiltasSouls.Content.Items.Materials;
using ThoriumMod.Items.Terrarium;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler.Content;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using ThoriumMod.Items.BossThePrimordials.Dream;
using CSE.Core;
using FargowiltasSouls.Core.Toggler;
using CSE.Content.Thorium.Materials;
using Fargowiltas.Content.Items.Tiles;
using Microsoft.Xna.Framework;

namespace CSE.Content.Thorium.Accessories.Souls
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class GuardianAngelsSoul : BaseSoul
    {
        public static readonly Color ItemColor = new(128, 128, 0);

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            Item.value = 750000;
            Item.rare = 11;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Thorium(player);
        }

        private void Thorium(Player player)
        {
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();

            player.GetDamage<HealerDamage>() += 0.25f;
            player.GetCritChance<HealerDamage>() += 0.12f;
            player.GetAttackSpeed<HealerDamage>() += 0.15f;
            player.GetAttackSpeed(ThoriumDamageBase<HealerTool>.Instance) += 0.20f;
            player.GetModPlayer<ThoriumPlayer>().healBonus += 20;

            thoriumPlayer.accSupportSash = true;

            thoriumPlayer.accVerdantOrnament = true;
            thoriumPlayer.accForgottenCrossNecklace = true;

            thoriumPlayer.darkAura = true;

            thoriumPlayer.healBonus += 20;

            //thoriumPlayer.medicalAcc = true;

            if (player.AddEffect<GuardianEffect>(Item))
            {
                ModContent.Find<ModItem>(ModCompatibility.Thorium.Name, "MedicalBag").UpdateAccessory(player, true);
            }

            if (ModLoader.HasMod("CalamityBardHealer"))
            {
                ModContent.Find<ModItem>("CalamityBardHealer", "ElementalBloom").UpdateAccessory(player, false);
            }

            if (ModLoader.HasMod("ThoriumRework"))
            {
                ModContent.Find<ModItem>("ThoriumRework", "SealedContract").UpdateAccessory(player, false);
            }
        }

        public class GuardianEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<UniverseHeader>();
            public override int ToggleItemType => ModContent.ItemType<GuardianAngelsSoul>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            if (!ModLoader.HasMod("CalamityBardHealer"))
            {
                recipe.AddIngredient<SupportSash>();
                recipe.AddIngredient<SavingGrace>();
                recipe.AddRecipeGroup("CSE:AnyIdolUpgrade");
                recipe.AddIngredient<TeslaDefibrillator>();
                recipe.AddIngredient<MoonlightStaff>();
                recipe.AddIngredient<TerrariumHolyScythe>();
                recipe.AddIngredient<TerraScythe>();
                recipe.AddIngredient<PhoenixStaff>();
                recipe.AddIngredient<ShieldDroneBeacon>();
                recipe.AddIngredient<LifeAndDeath>();
            }
            else
            {
                recipe.AddIngredient(ModContent.Find<ModItem>("CalamityBardHealer", "ElementalBloom"));
                recipe.AddIngredient<SupportSash>();
                recipe.AddIngredient<SavingGrace>();
                recipe.AddIngredient<UnboundFantasy>();
                recipe.AddIngredient(ModContent.Find<ModItem>("CalamityBardHealer", "BloomingSaintessDevotion"));
                recipe.AddIngredient(ModContent.Find<ModItem>("CalamityBardHealer", "SavingGrace"));
                recipe.AddIngredient(ModContent.Find<ModItem>("CalamityBardHealer", "WilloftheRagnarok"));
                recipe.AddIngredient(ModContent.Find<ModItem>("CalamityBardHealer", "PhoenicianBeak"));
                recipe.AddIngredient(ModContent.Find<ModItem>("CalamityBardHealer", "MilkyWay"));
            }
            if (ModLoader.HasMod("ThoriumRework"))
            {
                recipe.AddIngredient(ModContent.Find<ModItem>("ThoriumRework", "SealedContract"));
            }

            if (ModCompatibility.Homeward.Loaded)
            {
                recipe.AddIngredient(ModContent.Find<ModItem>(ModCompatibility.Homeward.Name, "FinalBar"), 5);
            }

            if (ModCompatibility.Calamity.Loaded) { recipe.AddIngredient<AbomEnergy>(10); }

            recipe.AddIngredient<DreamEssence>(5);
            recipe.AddTile<CrucibleCosmosSheet>();

            recipe.Register();
        }
    }
}
