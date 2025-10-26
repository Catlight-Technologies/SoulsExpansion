using Terraria.ModLoader;
using Terraria;
using CSE.Core;

namespace CSE.Content.SoA.Buffs
{
    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class SniperBuff : ModBuff
    {
        public override string Texture => "CSE/Content/SoA/Buffs/BuffTemplate";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }
    }

    [ExtendsFromMod(ModCompatibility.SacredTools.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name)]
    public class SniperCooldownBuff : ModBuff
    {
        public override string Texture => "CSE/Content/SoA/Buffs/BuffTemplate";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
    }
}
