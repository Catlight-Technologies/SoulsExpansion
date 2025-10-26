using CSE.Core.Thorium.ModPlayers;
using FargowiltasSouls.Core.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Thrower;

namespace CSE.Core.Thorium.Globals
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class ThoriumGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool immuneToCD;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.type == ModContent.ProjectileType<WhiteFlare>())
            {
                if (Main.player[projectile.owner].GetModPlayer<ThoriumPlayer>().ivoryFlameCD > 0)
                {
                    projectile.damage = 0;
                    projectile.hide = true;
                    projectile.timeLeft = 0;
                    projectile.netUpdate = true;
                }
            }
            if (projectile.type == ModContent.ProjectileType<InfernoLordsFocusPro>() && projectile.owner.ToPlayer().GetModPlayer<ThoriumPlayer>().infernoLordCD < 1)
            {
                immuneToCD = true;
                projectile.owner.ToPlayer().GetModPlayer<ThoriumPlayer>().infernoLordCD = 5;
            }
            if (projectile.type == ModContent.ProjectileType<InfernoLordsFocusPro>() && projectile.owner.ToPlayer().GetModPlayer<ThoriumPlayer>().infernoLordCD > 0 && !immuneToCD)
            {
                projectile.damage = 0;
                projectile.hide = true;
                projectile.timeLeft = 0;
                projectile.netUpdate = true;
            }
        }

        //public override bool PreAI(Projectile projectile)
        //{
        //    if (projectile.type == ModContent.ProjectileType<InfernoLordsFocusPro>() && projectile.owner.ToPlayer().GetModPlayer<ThoriumPlayer>().infernoLordCD > 0)
        //    {
        //        projectile.damage = 0;
        //        projectile.hide = true;
        //        projectile.timeLeft = 0;
        //        projectile.netUpdate = true;
        //        return false;
        //    }
        //    return base.PreAI(projectile);
        //}
        public override void SetDefaults(Projectile projectile)
        {
            //if (projectile.type == ModContent.ProjectileType<InfernoLordsFocusPro>() && !ModCompatibility.Calamity.Loaded)
            //{
            //    if (!WorldSavingSystem.DownedAbom && WorldSavingSystem.DownedBoss[8])
            //    {
            //        projectile.damage = (int)(projectile.damage * 0.75f);
            //    }
            //    if (!WorldSavingSystem.DownedBoss[8])
            //    {
            //        projectile.damage = (int)(projectile.damage * 0.5f);
            //    }
            //}
            //if (projectile.type == ModContent.ProjectileType<TerrariansLastKnifePro>())
             //   projectile.scale = 1.5f;
            //ts doubles my spear dps
            //most balanced and vanilla friendly mod
            //sure sure
            if (projectile.type == ModContent.ProjectileType<SpearExtraCrystal>())
                projectile.damage = (int)(projectile.damage * 0.5f);
        }
    }
}
