using Terraria.ModLoader;

namespace CSE.Core.Thorium.ModPlayers
{
    public class ThoriumPlayer : ModPlayer
    {
        public int ivoryFlameCD;
        public int infernoLordCD;
        public override void ResetEffects()
        {
            if(ivoryFlameCD > 0)
                ivoryFlameCD--;
            if (infernoLordCD > 0)
                infernoLordCD--;
        }
    }
}
