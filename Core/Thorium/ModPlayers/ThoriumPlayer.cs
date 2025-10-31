using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
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

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            IEntitySource source_OnHit = proj.GetSource_OnHit(target);

            if (Player.GetThoriumPlayer().setTideCrown && proj.CountsAsClass<ThrowingDamageClass>() && tideDaggerCD < 0)
            {
                int num49 = ModContent.ProjectileType<TideDagger>();
                if (proj.type != num49 && Main.rand.NextBool(5))
                {
                    SoundEngine.PlaySound(in SoundID.Item43, Player.Center);
                    int damage2 = (int)(proj.damage * 0.7);
                    Projectile.NewProjectile(source_OnHit, Player.Center.X, Player.Center.Y, 3f, 0f, num49, damage2, 3f, Main.myPlayer, 0, 0, 1488);
                    Projectile.NewProjectile(source_OnHit, Player.Center.X, Player.Center.Y, -3f, 0f, num49, damage2, 3f, Main.myPlayer, 0, 0, 1488);
                    Projectile.NewProjectile(source_OnHit, Player.Center.X, Player.Center.Y, -1.5f, -2.15f, num49, damage2, 3f, Main.myPlayer, 0, 0, 1488);
                    Projectile.NewProjectile(source_OnHit, Player.Center.X, Player.Center.Y, 1.5f, -2.15f, num49, damage2, 3f, Main.myPlayer, 0, 0, 1488);
                    Projectile.NewProjectile(source_OnHit, Player.Center.X, Player.Center.Y, -1.5f, 2.15f, num49, damage2, 3f, Main.myPlayer, 0, 0, 1488);
                    Projectile.NewProjectile(source_OnHit, Player.Center.X, Player.Center.Y, 1.5f, 2.15f, num49, damage2, 3f, Main.myPlayer, 0, 0, 1488);

                    tideDaggerCD = 20;
                }
            }
        }
    }
}
