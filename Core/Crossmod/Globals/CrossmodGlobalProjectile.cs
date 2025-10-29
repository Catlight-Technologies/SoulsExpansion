using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ExoMechs.Thanatos;
using Terraria;
using Terraria.ModLoader;

namespace CSE.Core.Crossmod.Globals
{
    [ExtendsFromMod(ModCompatibility.Crossmod.Name)]
    [JITWhenModsEnabled(ModCompatibility.Crossmod.Name)]
    public class CrossmodGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            //naaaaaah 2 minutes killtime
            if(ModCompatibility.Thorium.Loaded && (
                target.type == ModContent.NPCType<ThanatosBody1>()
                || target.type == ModContent.NPCType<ThanatosBody2>()
                || target.type == ModContent.NPCType<ThanatosHead>()
                || target.type == ModContent.NPCType<ThanatosTail>()

                || target.type == ModContent.NPCType<AresBody>()
                || target.type == ModContent.NPCType<AresGaussNuke>()
                || target.type == ModContent.NPCType<AresLaserCannon>()
                || target.type == ModContent.NPCType<AresPlasmaFlamethrower>()
                || target.type == ModContent.NPCType<AresTeslaCannon>()))
            {
                if (projectile.type == ModCompatibility.Thorium.Mod.Find<ModItem>("JavelinPro").Type
                    || projectile.type == ModCompatibility.Thorium.Mod.Find<ModItem>("JavelinClusterPro").Type)
                {
                    hit.Damage = (int)(hit.Damage * 0.7f);
                }
            }
            base.OnHitNPC(projectile, target, hit, damageDone);
        }
    }
}
