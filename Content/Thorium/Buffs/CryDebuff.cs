using CSE.Core;
using CSE.Core.Thorium.Globals;
using Terraria;
using Terraria.ModLoader;

namespace CSE.Content.Thorium.Buffs
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class CryDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<ThoriumGlobalNPC>().crying = true;
        }
    }
}
