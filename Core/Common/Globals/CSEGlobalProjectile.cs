using FargowiltasSouls.Content.Items.Weapons.FinalUpgrades;
using Terraria;
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
    }
}
