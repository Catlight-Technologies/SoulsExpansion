using Fargowiltas.Content.NPCs;
using FargowiltasSouls;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.ArcaneArmor;
using ThoriumMod.Items.Depths;
using ThoriumMod.Items.Misc;
using ThoriumMod.Items.Placeable;
using ThoriumMod.Items.Thorium;
using ThoriumMod.NPCs.BossBuriedChampion;
using ThoriumMod.NPCs.BossForgottenOne;
using ThoriumMod.NPCs.BossGraniteEnergyStorm;
using ThoriumMod.NPCs.BossLich;
using ThoriumMod.NPCs.BossQueenJellyfish;
using ThoriumMod.NPCs.BossStarScouter;
using ThoriumMod.NPCs.BossTheGrandThunderBird;
using ThoriumMod.NPCs.BossViscount;
using static Terraria.ModLoader.ModContent;
using ThoriumMod.NPCs.BossBoreanStrider;
using ThoriumMod.NPCs.BossThePrimordials;
using FargowiltasSouls.Core.Globals;
using FargowiltasSouls.Core.Systems;
using FargowiltasSouls.Core.ItemDropRules.Conditions;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.Consumable;
using ThoriumMod.Items.ThrownItems;
using ThoriumMod.NPCs.Depths;
using ThoriumMod.NPCs;

namespace CSE.Core.Thorium.Globals
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class ThoriumGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCType<LumberJack>())
            {
                shop.Add(new Item(ItemType<Deadwood>()) { shopCustomPrice = Item.buyPrice(copper: 20) }, Condition.InGraveyard);
                shop.Add(new Item(ItemType<EvergreenBlock>()) { shopCustomPrice = Item.buyPrice(copper: 20) }, Condition.DownedEverscream);
                shop.Add(new Item(ItemType<YewWood>()) { shopCustomPrice = Item.buyPrice(copper: 20) }, Condition.DownedGoblinArmy);
            }
            base.ModifyShop(shop);
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            void TimsConcoctionDrop(IItemDropRule rule)
            {
                TimsConcoctionDropCondition dropCondition = new();
                IItemDropRule conditionalRule = new LeadingConditionRule(dropCondition);
                conditionalRule.OnSuccess(rule);
                npcLoot.Add(conditionalRule);
            }

            if (npc.type == NPCType<GildedBat>() || npc.type == NPCType<GildedSlime>() || npc.type == NPCType<GildedLycan>())
            {
                TimsConcoctionDrop(ItemDropRule.Common(ItemType<GlowingPotion>(), 1, 1, 6));
            }
            if (npc.type == NPCID.Pixie)
            {
                TimsConcoctionDrop(ItemDropRule.Common(ItemType<HolyPotion>(), 1, 0, 3));
            }
            if (npc.type == NPCType<Hammerhead>())
            {
                TimsConcoctionDrop(ItemDropRule.Common(ItemType<AquaPotion>(), 1, 1, 6));
            }
            if (npc.type == NPCType<Spectrumite>())
            {
                TimsConcoctionDrop(ItemDropRule.Common(ItemType<ArcanePotion>(), 1, 1, 6));
            }
            if (npc.type == NPCType<MoltenMortar>())
            {
                TimsConcoctionDrop(ItemDropRule.Common(ItemType<ArtilleryPotion>(), 1, 1, 3));
            }
            if (npc.type == NPCType<AncientArcher>() || npc.type == NPCType<AncientCharger>())
            {
                TimsConcoctionDrop(ItemDropRule.Common(ItemType<AssassinPotion>(), 1, 1, 3));
            }
            if (npc.type == NPCType<LifeCrystalMimic>())
            {
                TimsConcoctionDrop(ItemDropRule.Common(ItemType<BloodPotion>(), 1, 1, 8));
            }
            if (npc.type == NPCType<BoneFlayer>() || npc.type == NPCType<InfernalHound>())
            {
                TimsConcoctionDrop(ItemDropRule.Common(ItemType<ConflagrationPotion>(), 1, 1, 3));
            }
            if (npc.type == NPCType<UnderworldPot1>() || npc.type == NPCType<UnderworldPot2>())
            {
                TimsConcoctionDrop(ItemDropRule.Common(ItemType<BouncingFlamePotion>(), 1, 1, 6));
            }
            if (npc.type == NPCType<GoblinDrummer>())
            {
                TimsConcoctionDrop(ItemDropRule.Common(ItemType<CreativityPotion>(), 1, 1, 6));
            }
            if (npc.type == NPCID.GiantWormHead || npc.type == NPCID.DiggerHead)
            {
                TimsConcoctionDrop(ItemDropRule.Common(ItemType<EarwormPotion>(), 1, 1, 6));
            }
            if (npc.type == NPCType<HellBringerMimic>())
            {
                TimsConcoctionDrop(ItemDropRule.Common(ItemType<WarmongerPotion>(), 1, 1, 3));
            }
            if (npc.type == NPCType<Barracuda>() || npc.type == NPCType<Sharptooth>())
            {
                TimsConcoctionDrop(ItemDropRule.Common(ItemType<FrenzyPotion>(), 1, 1, 6));
            }
            if (npc.type == NPCType<SnowBall>())
            {
                TimsConcoctionDrop(ItemDropRule.Common(ItemType<HydrationPotion>(), 1, 1, 3));
            }
            if (npc.type == NPCType<SeaShantySinger>())
            {
                TimsConcoctionDrop(ItemDropRule.Common(ItemType<InspirationReachPotion>(), 1, 1, 6));
            }

            if (WorldSavingSystem.EternityMode)
            {
                LeadingConditionRule firstKillRule = new(new FirstKillCondition());
                npcLoot.Add(firstKillRule);

                if (npc.type == NPCType<TheGrandThunderBird>())
                {
                    firstKillRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ItemType<StrangeCrate>(), 5));
                }
                if (npc.type == NPCType<QueenJellyfish>())
                {
                    firstKillRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ItemType<AquaticDepthsCrate>(), 5));
                    //firstKillRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ItemType<JellyfishCoil>(), 1));
                }
                if (npc.type == NPCType<Viscount>())
                {
                    firstKillRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ItemType<ScarletCrate>(), 5));
                    //firstKillRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ItemType<VampiresBlessing>(), 1));
                }
                if (npc.type == NPCType<GraniteEnergyStorm>())
                {
                    firstKillRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ItemID.GoldenCrate, 5));
                    //firstKillRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ItemType<GraniteMaterializer>(), 1));
                }
                if (npc.type == NPCType<BuriedChampion>())
                {
                    firstKillRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ItemID.GoldenCrate, 5));
                    //firstKillRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ItemType<ChampionHeadband>(), 1));
                }
                if (npc.type == NPCType<StarScouter>())
                {
                    firstKillRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ItemID.FloatingIslandFishingCrate, 5));
                }
                if (npc.type == NPCType<BoreanStriderPopped>())
                {
                    firstKillRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ItemID.FrozenCrateHard, 5));
                }
                if (npc.type == NPCType<Lich>())
                {
                    firstKillRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ItemID.DungeonFishingCrateHard, 5));
                }
                if (npc.type == NPCType<ForgottenOne>())
                {
                    firstKillRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ItemType<AbyssalCrate>(), 5));
                }
                if (npc.type == NPCType<DreamEater>())
                {
                    firstKillRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ItemType<WondrousCrate>(), 5));
                }
            }
        }
    }
}