using FargowiltasSouls.Content.Items.Accessories.Souls;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler;
using FargowiltasSouls.Core.Toggler.Content;
using Redemption.Items.Accessories.HM;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CSE.Core.MoR.Globals
{
    [ExtendsFromMod(ModCompatibility.Redemption.Name)]
    [JITWhenModsEnabled(ModCompatibility.Redemption.Name)]
    public class MoRGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemType<ColossusSoul>() || item.type == ItemType<DimensionSoul>() || item.type == ItemType<EternitySoul>())
            {
                ModCompatibility.Redemption.Mod.Find<ModItem>("HEVSuit").UpdateAccessory(player, true);
                //player.AddEffect<PocketShieldGeneratorEffect>(item);
            }
        }

        public class PocketShieldGeneratorEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<ColossusHeader>();
            public override int ToggleItemType => ItemType<PocketShieldGenerator>();

            public override void PostUpdateEquips(Player player)
            {
                ModCompatibility.Redemption.Mod.Find<ModItem>("PocketShieldGenerator").UpdateAccessory(player, true);
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemType<ColossusSoul>())
            {
                tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue("Mods.CSE.AddedEffects.RedColossus")));
               // tooltips.Insert(5, new TooltipLine(Mod, "mayo2", Language.GetTextValue("Mods.CSE.AddedEffects.RedColossus2")));
            }
        }
    }
}