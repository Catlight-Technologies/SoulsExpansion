using FargowiltasSouls.Core.Systems;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Thrower;
using ThoriumMod.Utilities;

namespace CSE.Core.Thorium.ModPlayers
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class ThoriumPlayer : ModPlayer
    {
        public int ivoryFlameCD;
        public int infernoLordCD;
        public int tideDaggerCD;
        public override void ResetEffects()
        {
            if(ivoryFlameCD > 0)
                ivoryFlameCD--;
            if (infernoLordCD > 0)
                infernoLordCD--;
            if (tideDaggerCD > 0)
                tideDaggerCD--;
        }

        public override void PostUpdateEquips()
        {
            //temp solution until thrower rework
            if (WorldSavingSystem.EternityMode)
            {
                Player.GetThoriumPlayer().throwerExhaustionMax *= 2;
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Player.GetThoriumPlayer().setTideCrown && proj.CountsAsClass<ThrowingDamageClass>() && tideDaggerCD < 0)
            {
                if (proj.type != ModContent.ProjectileType<TideDagger>() && Main.rand.NextBool(5))
                {
                    SoundEngine.PlaySound(in SoundID.Item43, Player.Center);

                    Projectile.NewProjectile(proj.GetSource_OnHit(target), Player.Center.X, Player.Center.Y, 3f, 0f, ModContent.ProjectileType<TideDagger>(), (int)(proj.damage * 0.5), 3f, Main.myPlayer, 1488);
                    Projectile.NewProjectile(proj.GetSource_OnHit(target), Player.Center.X, Player.Center.Y, -3f, 0f, ModContent.ProjectileType<TideDagger>(), (int)(proj.damage * 0.5), 3f, Main.myPlayer, 1488);
                    Projectile.NewProjectile(proj.GetSource_OnHit(target), Player.Center.X, Player.Center.Y, -1.5f, -2.15f, ModContent.ProjectileType<TideDagger>(), (int)(proj.damage * 0.5), 3f, Main.myPlayer, 1488);
                    Projectile.NewProjectile(proj.GetSource_OnHit(target), Player.Center.X, Player.Center.Y, 1.5f, -2.15f, ModContent.ProjectileType<TideDagger>(), (int)(proj.damage * 0.5), 3f, Main.myPlayer, 1488);
                    Projectile.NewProjectile(proj.GetSource_OnHit(target), Player.Center.X, Player.Center.Y, -1.5f, 2.15f, ModContent.ProjectileType<TideDagger>(), (int)(proj.damage * 0.5), 3f, Main.myPlayer, 1488);
                    Projectile.NewProjectile(proj.GetSource_OnHit(target), Player.Center.X, Player.Center.Y, 1.5f, 2.15f, ModContent.ProjectileType<TideDagger>(), (int)(proj.damage * 0.5), 3f, Main.myPlayer, 1488);

                    tideDaggerCD = 40;
                }
            }
        }
    }
}
