using Fargowiltas.Content.NPCs;
using FargowiltasSouls.Content.Bosses.AbomBoss;
using FargowiltasSouls.Content.Bosses.DeviBoss;
using FargowiltasSouls.Content.Bosses.MutantBoss;
using Terraria;
using Terraria.ModLoader;
using NoxusBoss.Content.Items;
using NoxusBoss.Content.Items.MiscOPTools;
using NoxusBoss.Content.NPCs.Bosses.Avatar.SecondPhaseForm;
using NoxusBoss.Content.NPCs.Bosses.Draedon;
using NoxusBoss.Content.NPCs.Bosses.NamelessDeity;

namespace CSE.Core.Crossmod.Globals
{
    [ExtendsFromMod("NoxusBoss")]
    [JITWhenModsEnabled("NoxusBoss")]
    public class CrossmodGlobalNPC : GlobalNPC
    {
        public override void SetDefaults(NPC entity)
        {
            EmptinessSprayer.NPCsThatReflectSpray[ModContent.NPCType<LumberJack>()] = true;
            EmptinessSprayer.NPCsThatReflectSpray[ModContent.NPCType<Squirrel>()] = true;

            EmptinessSprayer.NPCsThatReflectSpray[ModContent.NPCType<MutantBoss>()] = true;
            EmptinessSprayer.NPCsThatReflectSpray[ModContent.NPCType<DeviBoss>()] = true;
            EmptinessSprayer.NPCsThatReflectSpray[ModContent.NPCType<AbomBoss>()] = true;
            EmptinessSprayer.NPCsThatReflectSpray[ModContent.NPCType<Mutant>()] = true;
            EmptinessSprayer.NPCsThatReflectSpray[ModContent.NPCType<Abominationn>()] = true;
            EmptinessSprayer.NPCsThatReflectSpray[ModContent.NPCType<Deviantt>()] = true;
        }
    }
}
