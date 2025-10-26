using FargowiltasSouls.Content.Buffs.Eternity;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler.Content;
using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using FargowiltasSouls.Content.Items;
using FargowiltasSouls.Core.Toggler;
using FargowiltasSouls.Content.Items.Armor.Eternal;
using FargowiltasSouls.Content.Rarities;
using CSE.Core.Common.ModPlayers;
using CSE.Content.Common.Projectiles;

namespace CSE.Content.Common.Accessories.Other
{
    public class CyclonicFin : SoulsItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<EternitySoulRarity>();
            Item.value = Item.sellPrice(0, 17);
        }

        public class CuteFishEXEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<DeviEnergyHeader>();
            public override int ToggleItemType => ModContent.ItemType<CyclonicFin>();
            public override bool MutantsPresenceAffects => true;
        }

        public class SpectralFishEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<DeviEnergyHeader>();
            public override int ToggleItemType => ModContent.ItemType<CyclonicFin>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[ModContent.BuffType<OceanicMaulBuff>()] = true;
            player.buffImmune[ModContent.BuffType<CurseoftheMoonBuff>()] = true;

            if (player.AddEffect<SpectralFishEffect>(Item))
            {
                player.GetModPlayer<CSEPlayer>().CyclonicFin = true;

                if (player.GetModPlayer<CSEPlayer>().CyclonicFinCD > 0)
                    player.GetModPlayer<CSEPlayer>().CyclonicFinCD--;
            }

            if (player.AddEffect<CuteFishEXEffect>(Item))
            {
                if (player.mount.Active && player.mount.Type == MountID.CuteFishron)
                {
                    if (player.ownedProjectileCounts[ModContent.ProjectileType<CuteFishronRitual>()] < 1 && player.whoAmI == Main.myPlayer)
                        Projectile.NewProjectile(Item.GetSource_FromThis(), player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<CuteFishronRitual>(), 0, 0f, Main.myPlayer);

                    player.MountFishronSpecialCounter = 300;
                    player.GetDamage<GenericDamageClass>() += 0.15f;
                    player.GetCritChance<GenericDamageClass>() += 30f;
                    player.statDefense += 30;
                    player.lifeRegen += 3;
                    player.lifeRegenCount += 3;
                    player.lifeRegenTime += 3;

                    if (player.controlLeft == player.controlRight)
                    {
                        if (player.velocity.X != 0)
                            player.velocity.X -= player.mount.Acceleration * Math.Sign(player.velocity.X);
                        if (player.velocity.X != 0)
                            player.velocity.X -= player.mount.Acceleration * Math.Sign(player.velocity.X);
                    }

                    else if (player.controlLeft)
                    {
                        player.velocity.X -= player.mount.Acceleration * 5f;
                        if (player.velocity.X < -14f)
                            player.velocity.X = -14f;
                        if (!player.controlUseItem)
                            player.direction = -1;
                    }

                    else if (player.controlRight)
                    {
                        player.velocity.X += player.mount.Acceleration * 5f;
                        if (player.velocity.X > 14f)
                            player.velocity.X = 14f;
                        if (!player.controlUseItem)
                            player.direction = 1;
                    }

                    if (player.controlUp == player.controlDown)
                    {
                        if (player.velocity.Y != 0)
                            player.velocity.Y -= player.mount.Acceleration * Math.Sign(player.velocity.Y);
                        if (player.velocity.Y != 0)
                            player.velocity.Y -= player.mount.Acceleration * Math.Sign(player.velocity.Y);
                    }

                    else if (player.controlUp)
                    {
                        player.velocity.Y -= player.mount.Acceleration * 5f;
                        if (player.velocity.Y < -14f)
                            player.velocity.Y = -14f;
                    }

                    else if (player.controlDown)
                    {
                        player.velocity.Y += player.mount.Acceleration * 5f;
                        if (player.velocity.Y > 14f)
                            player.velocity.Y = 14f;
                    }
                }
            }
        }
    }
}
