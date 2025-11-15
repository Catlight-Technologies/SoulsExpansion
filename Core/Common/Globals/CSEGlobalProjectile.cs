using FargowiltasSouls;
using FargowiltasSouls.Content.Items.Weapons.FinalUpgrades;
using FargowiltasSouls.Content.Projectiles.Eternity.Bosses.DukeFishron;
using FargowiltasSouls.Core.Globals;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSE.Core.Common.Globals
{
    public class CSEGlobalProjectiles : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override bool PreAI(Projectile projectile)
        {
            if (projectile.type == 55 && Main.LocalPlayer.HeldItem.type == ModContent.ItemType<TheBiggestSting>())
            {
                projectile.damage = 0;
                projectile.hide = true;
                projectile.timeLeft = 0;
                projectile.netUpdate = true;
            }

            return base.PreAI(projectile);
        }

        public override void SetDefaults(Projectile entity)
        {
            //impossible pre mutant?
            if ((entity.type == ModContent.ProjectileType<FishronRitual>() || entity.type == ModContent.ProjectileType<FishronRitual2>()) && FargoSoulsUtil.BossIsAlive(ref EModeGlobalNPC.fishBossEX, NPCID.DukeFishron))
            {
                entity.damage = 0;
                entity.hostile = false;
            }

        }
    }
}
