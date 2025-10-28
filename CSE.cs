using FargowiltasSouls.Core.Globals;
using System.IO;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Fargowiltas.Content.Items.CaughtNPCs;
using System.Collections.Generic;
using System.Reflection;
using Luminance.Common.Utilities;
using Terraria.DataStructures;
using FargowiltasSouls.Assets.Textures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace CSE
{
    public partial class CSE : Mod
	{
        internal static CSE Instance;
        public static List<TitleLinkButton> CSETitleLinks = new List<TitleLinkButton>();
        public static void AddNPC(string internalName, int id)
        {
            if (Instance == null)
            {
                Instance = ModContent.GetInstance<CSE>();
            }
            CaughtNPCItem item = new(internalName, id);
            Instance.AddContent(item);
            FieldInfo info = typeof(CaughtNPCItem).GetField("CaughtTownies", Utilities.UniversalBindingFlags);
            Dictionary<int, int> list = (Dictionary<int, int>)info.GetValue(info);
            list.Add(id, item.Type);
            info.SetValue(info, list);
        }
        public static void DrawTitleLinks(Color menuColor, float upBump)
        {
            List<TitleLinkButton> titleLinks = CSETitleLinks;
            Vector2 anchorPosition = new Vector2(18f, (float)(Main.screenHeight - 85 - 22) - upBump);
            for (int i = 0; i < titleLinks.Count; i++)
            {
                titleLinks[i].Draw(Main.spriteBatch, anchorPosition);
                anchorPosition.X += 30f;
            }
        }
        public override void Load()
        {
            Instance = this;

            List<TitleLinkButton> titleLinks = CSETitleLinks;
            titleLinks.Add(MakeSimpleButton("TitleLinks.Discord", "https://discord.gg/frUcz2dQAy", 0));
            titleLinks.Add(MakeSimpleButton("TitleLinks.Wiki", "https://terrariamods.wiki.gg/wiki/Community_Souls_Expansion", 1));
            titleLinks.Add(MakeSimpleButton("Mods.FargowiltasSouls.UI.TitleLinks.Github", "https://github.com/Catlight-Technologies/SoulsExpansion", 3));

            //FargowiltasSouls.BossChecklistValues["MutantBoss"] = int.MaxValue - 1;
            //BossChecklistValues["AbomBoss"] = ModCompatibility.CatTech.Loaded ? 50 : 22.9f;

            LoadDetours();
        }

        public static TitleLinkButton MakeSimpleButton(string textKey, string linkUrl, int horizontalFrameIndex)
        {
            Asset<Texture2D> val = FargoAssets.UI.MainMenu.TitleLinkButtons;
            Rectangle value = val.Frame(4, 2, horizontalFrameIndex);
            Rectangle value2 = val.Frame(4, 2, horizontalFrameIndex, 1);
            value.Width--;
            value.Height--;
            value2.Width--;
            value2.Height--;
            return new TitleLinkButton
            {
                TooltipTextKey = textKey,
                LinkUrl = linkUrl,
                FrameWehnSelected = value2,
                FrameWhenNotSelected = value,
                Image = val
            };
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            byte data = reader.ReadByte();
            if (Enum.IsDefined(typeof(PacketID), data))
            {
                switch ((PacketID)data)
                {
                    case PacketID.SpawnFishronEX:
                        HandleSpawnFishronEX(reader, whoAmI);
                        break;
                }
            }
        }
        public enum PacketID : byte
        {
            SpawnFishronEX
        }

        public override void Unload()
        {
            Instance = null;
            UnloadDetours();
        }
        private void HandleSpawnFishronEX(BinaryReader reader, int whoAmI)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                byte target = reader.ReadByte();
                int x = reader.ReadInt32();
                int y = reader.ReadInt32();
                EModeGlobalNPC.spawnFishronEX = true;
                NPC.NewNPC(NPC.GetBossSpawnSource(target), x, y, NPCID.DukeFishron, 0, 0f, 0f, 0f, 0f, target);
                ChatHelper.BroadcastChatMessage(
                    NetworkText.FromKey("Announcement.HasAwoken",
                    Language.GetTextValue("Mods.FargowiltasSouls.NPCs.DukeFishronEX.DisplayName")),
                    new Color(50, 100, 255));
            }
        }
    }
}
