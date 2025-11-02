using Terraria.ModLoader;
using Terraria;
using CSE.Core;

namespace CSE.Content.Thorium.Projectiles
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class FeatherProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_1516";
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.2f, 0.2f, 0.8f);
            Projectile.ai[1]++;
            if (Projectile.ai[1] > 60) 
            {
                CSEUtils.HomeInOnNPC(Projectile, false, 9000, 7, 2);
            }
        }
    }
}
