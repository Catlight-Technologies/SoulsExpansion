using CSE.Content.SoA.Buffs;
using Fargowiltas.Content.NPCs;
using FargowiltasSouls.Core.Globals;
using FargowiltasSouls.Core.Systems;
using FargowiltasSouls;
using SacredTools.Common.Systems;
using SacredTools.Content.Items.Placeable.Solid;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using SacredTools.NPCs.Boss.Araghur;
using SacredTools.Items.Placeable;
using SacredTools.NPCs.Boss.Obelisk.Nihilus;
using FargowiltasSouls.Content.Bosses.MutantBoss;
using FargowiltasSouls.Content.Buffs.Boss;
using static CSE.Core.Common.Globals.CSEPointsBalanceNPC;

namespace CSE.Core.SoA.Globals
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class SoAGlobalNPC : GlobalNPC
    {
        private int attackCooldown = 0;
        private const int CooldownTime = 20;
        public override bool InstancePerEntity => true;
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == ModContent.NPCType<LumberJack>())
            {
                shop.Add(new Item(ModContent.ItemType<Emberbark>()) { shopCustomPrice = Item.buyPrice(copper: 600) }, new Condition("Mods.CSE.Conditions.AbaddonDowned", () => DownedSystem.DownedAbaddon));
            }
            base.ModifyShop(shop);
        }

        public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
        {
            if (npc.type == ModContent.NPCType<Nihilus>())
            {
                float damageMultiplier = 1f;
                float healthMultiplier = 1.1f;

                foreach (var boss in BossConfigs)
                {
                    if (boss.BossType == ModContent.NPCType<Nihilus>())
                    {
                        boss.CalculateMultipliers();

                        damageMultiplier += boss.DamageMultiplier;
                        healthMultiplier += boss.HealthMultiplier;
                    }
                }

                if (!TrueModeSystem.TrueMode)
                {
                    npc.damage = (int)(440 * damageMultiplier);
                    npc.lifeMax = (int)((1200000 + 300000 * numPlayers) * healthMultiplier);
                }
                else
                {
                    npc.damage = (int)(550 * damageMultiplier);
                    npc.lifeMax = (int)((1500000 + 750000 * numPlayers) * healthMultiplier);
                }

                if (Main.masterMode)
                {
                    npc.lifeMax = (int)(npc.lifeMax * 1.5f);
                }

                npc.defense = 300;
            }
            if (npc.type == ModContent.NPCType<Nihilus2>())
            {
                float damageMultiplier = 1f;
                float healthMultiplier = 1.1f;

                foreach (var boss in BossConfigs)
                {
                    if (boss.BossType == ModContent.NPCType<Nihilus2>())
                    {
                        boss.CalculateMultipliers();

                        damageMultiplier += boss.DamageMultiplier;
                        healthMultiplier += boss.HealthMultiplier;
                    }
                }

                if (!TrueModeSystem.TrueMode)
                {
                    npc.damage = (int)(500 * damageMultiplier);
                    npc.lifeMax = (int)((2000000 + 300000 * numPlayers) * healthMultiplier);
                }
                else
                {
                    npc.damage = (int)(680 * damageMultiplier);
                    npc.lifeMax = (int)((2400000 + 750000 * numPlayers) * healthMultiplier);
                }

                if (Main.masterMode)
                {
                    npc.lifeMax = (int)(npc.lifeMax * 1.5f);
                }

                npc.defense = 300;
            }
        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (WorldSavingSystem.EternityMode)
            {
                LeadingConditionRule firstKillRule = new(new FirstKillCondition());
                npcLoot.Add(firstKillRule);

                if (npc.type == ModContent.NPCType<AraghurHead>())
                {
                    firstKillRule.OnSuccess(FargoSoulsUtil.BossBagDropCustom(ModContent.ItemType<FlariumCrate>(), 5));
                }
            }
        }
        public override bool PreAI(NPC npc)
        {
            if (npc.type == ModContent.NPCType<Nihilus>() || npc.type == ModContent.NPCType<Nihilus2>())
            {
                if (Main.expertMode && Main.LocalPlayer.active && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost)
                    Main.LocalPlayer.AddBuff(ModContent.BuffType<NihilityPresenceBuff>(), 2);
            }
            if (npc.type == ModContent.NPCType<MutantBoss>() /*&& ModCompatibility.Calamity.Loaded*/)
            {
                if (Main.expertMode && Main.LocalPlayer.active && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost && !Main.LocalPlayer.HasBuff<MutantPresenceBuff>())
                    Main.LocalPlayer.AddBuff(ModContent.BuffType<NihilityPresenceBuff>(), 2);
            }
            return base.PreAI(npc);
        }
        private void CheckNPCCollisions(NPC sourceNpc)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC targetNpc = Main.npc[i];

                if (!targetNpc.active || targetNpc.friendly || targetNpc.life <= 0)
                    continue;

                if (sourceNpc.whoAmI == targetNpc.whoAmI)
                    continue;

                if (sourceNpc.Hitbox.Intersects(targetNpc.Hitbox))
                {
                    AttackOtherNPC(sourceNpc, targetNpc);
                    attackCooldown = CooldownTime;
                    break;
                }
            }
        }
        private void AttackOtherNPC(NPC attacker, NPC target)
        {
            int direction = attacker.position.X < target.position.X ? 1 : -1;

            target.SimpleStrikeNPC(attacker.damage, direction);

            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, target.whoAmI, attacker.damage, 2f, direction);
            }

            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(target.position, target.width, target.height,
                            DustID.Blood, 0f, 0f, 100, default, 1.5f);
            }
        }
        public override void ResetEffects(NPC npc)
        {
            if (!npc.HasBuff(ModContent.BuffType<FearBuff>()))
            {
                attackCooldown = 0;
            }
        }

        public override void AI(NPC npc)
        {
            if (npc.HasBuff(ModContent.BuffType<FearBuff>()))
            {
                if (attackCooldown > 0)
                    attackCooldown--;
                if (attackCooldown <= 0)
                {
                    CheckNPCCollisions(npc);
                }
            }
        }
    }
}