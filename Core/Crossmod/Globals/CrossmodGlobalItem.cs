using FargowiltasCrossmod.Content.Calamity.Items.Accessories.Souls;
using Terraria.ModLoader;
using Terraria;
using FargowiltasSouls.Content.Items.Summons;
using System.Collections.Generic;
using Terraria.Localization;
using NoxusBoss.Content.NPCs.Bosses.NamelessDeity;
using NoxusBoss.Core.World.WorldSaving;
using CalamityMod.Items.SummonItems;

namespace CSE.Core.Crossmod.Globals
{
    [ExtendsFromMod(ModCompatibility.Crossmod.Name)]
    [JITWhenModsEnabled(ModCompatibility.Crossmod.Name)]
    public class CrossmodGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ModContent.ItemType<VagabondsSoul>())
            {
                player.GetDamage<ThrowingDamageClass>() += 0.03f;
            }
        }

        public static float BalanceChange(Item entity)
        {
            if (ModLoader.HasMod("CalamityInheritance"))
            {
                if (CSESets.GetValue(CSESets.Items.AbomTierFargoWeapon, entity.type))
                {
                    return 5;
                }
                if (CSESets.GetValue(CSESets.Items.MutantTierFargoWeapon, entity.type))
                {
                    return 10;
                }
                if (CSESets.GetValue(CSESets.Items.SiblingsTierFargoWeapon, entity.type))
                {
                    return 10;
                }
            }
            return 1;
        }

        public override void SetDefaults(Item item)
        {
            float balance = BalanceChange(item);
            if (balance != 1)
            {
                if (item.damage > 0)
                {
                    item.damage = (int)(item.damage * balance);
                }
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModLoader.HasMod("NoxusBoss"))
            {
                if (item.type == ModContent.ItemType<Terminus>())
                {
                    tooltips.RemoveAll(line => line.Name == "PostMutant");
                }
                if (item.type == ModLoader.GetMod("NoxusBoss").Find<ModItem>("CheatPermissionSlip").Type)
                {
                    tooltips.Add(new TooltipLine(Mod, "PostMutant", $"{Language.GetTextValue("Mods.CSE.EModeBalance.PostMutabt")}"));
                }
                if (item.type == ModContent.ItemType<MutantsCurse>())
                {
                    tooltips.Add(new TooltipLine(Mod, "PostND", $"{Language.GetTextValue("Mods.CSE.EModeBalance.PostND")}"));
                }
            }
        }
    }
    [ExtendsFromMod("NoxusBoss")]
    [JITWhenModsEnabled("NoxusBoss")]
    public class WotGGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool CanUseItem(Item item, Player player)
        {
            if (item.type == ModContent.ItemType<MutantsCurse>())
            {
                return BossDownedSaveSystem.HasDefeated<NamelessDeityBoss>();
            }
            return base.CanUseItem(item, player);
        }
    }
}