using CSE.Core;
using Terraria;
using Terraria.ModLoader;

namespace CSE.Content.SoA.Buffs
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class FearBuff : ModBuff
    {
        public override string Texture => "CSE/Content/SoA/Buffs/BuffTemplate";

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }
    }
}
