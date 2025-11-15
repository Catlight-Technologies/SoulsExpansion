using CalamityMod.Items.Materials;
using CalamityMod.Items.SummonItems;
using Fargowiltas;
using MonoMod.RuntimeDetour;
using SacredTools.NPCs.Boss.Obelisk.Nihilus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static CSE.Core.Common.Globals.CSEPointsBalanceNPC;

namespace CSE.Core.Crossmod.ModSystems
{
    [ExtendsFromMod(ModCompatibility.Crossmod.Name)]
    [JITWhenModsEnabled(ModCompatibility.Crossmod.Name)]
    public class CrossmodPostSetupContent : ModSystem
    {
        public override void Load()
        {
            if (ModLoader.HasMod("NoxusPort"))
            {
                AddBossConfig(
                    bossType: ModLoader.GetMod("NoxusPort").Find<ModNPC>("EntropicGod").Type,
                    affectingMods: new List<ModMultiplier>
                    {
                        new ModMultiplier { ModName = "FargowiltasSouls", DamageMultiplier = 0f, HealthMultiplier = 0.5f },
                        //new ModMultiplier { ModName = "SacredTools", DamageMultiplier = 0.25f, HealthMultiplier = 0.5f },
                        //new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.25f, HealthMultiplier = 0.75f },
                        //new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.25f, HealthMultiplier = 0.25f }
                    }
                );
                AddBossConfig(
                    bossType: ModLoader.GetMod("NoxusPort").Find<ModNPC>("NoxusEgg").Type,
                    affectingMods: new List<ModMultiplier>
                    {
                        new ModMultiplier { ModName = "FargowiltasSouls", DamageMultiplier = 0f, HealthMultiplier = 0.5f },
                        //new ModMultiplier { ModName = "SacredTools", DamageMultiplier = 0.25f, HealthMultiplier = 0.5f },
                        //new ModMultiplier { ModName = "ThoriumMod", DamageMultiplier = 0.25f, HealthMultiplier = 0.75f },
                        //new ModMultiplier { ModName = "ContinentOfJourney", DamageMultiplier = 0.25f, HealthMultiplier = 0.25f }
                    }
                );
            }
        }

        public override void PostSetupContent()
        {
            FargoSets.Items.DuplicatableItems.SetValue(FargoSets.Items.DupeType.NotDupableFromDupable, ModContent.ItemType<MiracleMatter>());
            FargoSets.Items.DuplicatableItems.SetValue(FargoSets.Items.DupeType.NotDupableFromDupable, ModContent.ItemType<ShadowspecBar>());
        }
    }

    [ExtendsFromMod("NoxusBoss")]
    [JITWhenModsEnabled("NoxusBoss")]
    public class TerminusHook : ModSystem
    {
        private static Hook _terminusHook;

        public override void Load()
        {
            var targetType = ModCompatibility.Crossmod.Mod.Code?.GetType("FargowiltasCrossmod.Core.Calamity.Globals.CalDLCItemChanges");
            if (targetType is null)
                return;

            var target = targetType.GetMethod(
                "CanUseItem",
                BindingFlags.Instance | BindingFlags.Public
            );
            if (target is null)
                return;

            _terminusHook = new Hook(target, Detour);
        }

        public override void Unload()
        {
            _terminusHook?.Dispose();
            _terminusHook = null;
        }

        private delegate bool OrigDelegate(object self, Item item, Player player);
        private static bool Detour(OrigDelegate orig, object self, Item item, Player player)
        {
            if (item.type == ModContent.ItemType<Terminus>())
                return true;

            return orig(self, item, player);
        }
    }
}
