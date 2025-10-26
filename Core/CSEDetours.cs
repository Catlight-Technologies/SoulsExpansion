using FargowiltasSouls.Content.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria;
using Luminance.Core.Hooking;

namespace CSE
{
    public partial class CSE : ICustomDetourProvider
    {
        void ICustomDetourProvider.ModifyMethods()
        {
        }
        public void LoadDetours()
        {
            On_Main.DrawMenu += DrawMenu;
        }

        public void UnloadDetours()
        {
            On_Main.DrawMenu -= DrawMenu;
        }
        public static void DrawMenu(On_Main.orig_DrawMenu orig, Main self, GameTime gameTime)
        {
            float upBump = 0;
            byte b = (byte)((255 + Main.tileColor.R * 2) / 3);
            Mod mod = Instance;
            Vector2 anchorPosition = new Vector2(18f, (Main.screenHeight - 116 - 22) - upBump);
            Color color = new Color(b, b, b, 255);
            upBump += 56f;
            if (MenuLoader.CurrentMenu is FargoMenuScreen)
            {
                if (!WorldGen.drunkWorldGen && Main.menuMode == 0)
                {
                    DrawTitleLinks(color, upBump);
                    upBump += 56f;
                }
                if (!WorldGen.drunkWorldGen)
                {
                    string text = mod.DisplayName + " " + mod.Version;
                    Vector2 origin = FontAssets.MouseText.Value.MeasureString(text);
                    origin.X *= 0.5f;
                    origin.Y *= 0.5f;
                    for (int i = 0; i < 5; i++)
                    {
                        Color color2 = Color.Black;
                        if (i == 4)
                        {
                            color2 = color;
                            color2.R = (byte)((255 + color2.R) / 2);
                            color2.G = (byte)((255 + color2.R) / 2);
                            color2.B = (byte)((255 + color2.R) / 2);
                        }
                        color2.A = (byte)((float)(int)color2.A * 0.3f);
                        int num = 0;
                        int num2 = 0;
                        if (i == 0)
                        {
                            num = -2;
                        }
                        if (i == 1)
                        {
                            num = 2;
                        }
                        if (i == 2)
                        {
                            num2 = -2;
                        }
                        if (i == 3)
                        {
                            num2 = 2;
                        }
                        DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.MouseText.Value, text, new Vector2(origin.X + (float)num + 10f, (float)Main.screenHeight - origin.Y + (float)num2 - (Main.menuMode == 0 ? 85f : 25f) - upBump), color2, 0f, origin, 1f, SpriteEffects.None, 0f);
                    }

                }
            }
            orig(self, gameTime);
        }
    }
}
