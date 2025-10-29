using FargowiltasCrossmod.Content.Calamity.Items.Accessories.Souls;
using Terraria.ModLoader;
using Terraria;
using FargowiltasSouls.Content.Items.Summons;
using System.Collections.Generic;
using Terraria.Localization;
using NoxusBoss.Content.NPCs.Bosses.NamelessDeity;
using NoxusBoss.Core.World.WorldSaving;

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
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModLoader.HasMod("NoxusBoss"))
            {
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