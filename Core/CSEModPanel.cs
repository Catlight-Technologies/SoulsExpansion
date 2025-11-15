using Daybreak.Common.Features.ModPanel;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Text;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria;

namespace CSE.Core
{
    [Autoload(Side = ModSide.Client)]
    public class CustomModPanel : ModPanelStyle
    {
        public class CSEPanelIcon : UIImage
        {
            private readonly Texture2D animationTexture;
            private int frame;
            private int frameCounter;
            private const int FrameDelay = 8;
            private const int FrameCount = 4;
            private Rectangle[] frameRectangles;

            public CSEPanelIcon() : base(TextureAssets.MagicPixel.Value)
            {
                animationTexture = ModContent.Request<Texture2D>("FargowiltasSouls/Content/Bosses/MutantBoss/MutantBoss", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

                int frameWidth = animationTexture.Width;
                int frameHeight = animationTexture.Height / FrameCount;

                frameRectangles = new Rectangle[FrameCount];
                for (int i = 0; i < FrameCount; i++)
                {
                    frameRectangles[i] = new Rectangle(0, i * frameHeight, frameWidth, frameHeight);
                }
            }

            public override void Update(GameTime gameTime)
            {
                base.Update(gameTime);

                frameCounter++;
                if (frameCounter >= FrameDelay)
                {
                    frameCounter = 0;
                    frame = (frame + 1) % FrameCount;
                }
            }

            protected override void DrawSelf(SpriteBatch spriteBatch)
            {
                CalculatedStyle dimensions = GetDimensions();

                

                Rectangle destinationRect = new Rectangle(
                    (int)(dimensions.X - (frameRectangles[frame].Width * 1.5f - frameRectangles[frame].Width) / 2),
                    (int)(dimensions.Y),
                    (int)(frameRectangles[frame].Width * 1.5f),
                    frameRectangles[frame].Height
                    );

                spriteBatch.Draw(
                    animationTexture,
                    dimensions.ToRectangle(),
                    frameRectangles[frame],
                    new(200, 200, 200, 255)
                );
            }
        }
        public class CSEPanelModName : UIText
        {
            private readonly string originalText;

            public CSEPanelModName(string text, float textScale = 1, bool large = false) : base(text, textScale, large)
            {
                originalText = text;
            }

            protected override void DrawSelf(SpriteBatch spriteBatch)
            {
                var formattedText = GetCSEPanelText(originalText, Main.GlobalTimeWrappedHourly);
                SetText(formattedText);
                base.DrawSelf(spriteBatch);
            }

            private static string GetCSEPanelText(string text, float time)
            {
                var sb = new StringBuilder(text.Length * 12);

                Color darkTeal = new Color(0, 80, 80);
                Color brightCyan = new Color(0, 255, 255);

                for (var i = 0; i < text.Length; i++)
                {
                    float wave = MathF.Sin(time * 1.0f + i * 0.2f) * 0.5f + 0.5f;

                    var color = Color.Lerp(darkTeal, brightCyan, wave);
                    color.A = 255;

                    sb.Append($"[c/{color.Hex3()}:{text[i]}]");
                }

                return sb.ToString();
            }
        }
        public override void Load()
        {
            base.Load();
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override UIImage ModifyModIcon(UIPanel element, UIImage modIcon, ref int modIconAdjust)
        {
            return new CSEPanelIcon()
            {
                Left = modIcon.Left,
                Top = modIcon.Top,
                Width = modIcon.Width,
                Height = modIcon.Height,
            };
        }

        public override UIText ModifyModName(UIPanel element, UIText modName)
        {
            return new CSEPanelModName(Language.GetTextValue("Mods.CSE.ModIconName") + $" v{Mod.Version}")
            {
                Left = modName.Left,
                Top = modName.Top,
            };
        }

        public override bool PreDrawPanel(UIPanel element, SpriteBatch sb, ref bool drawDivider)
        {
            sb.End();

            CalculatedStyle dims = element.GetDimensions();

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);

            Texture2D panelTexture = ModContent.Request<Texture2D>("FargowiltasSouls/Content/Sky/MutantSky", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            sb.Draw(panelTexture, dims.ToRectangle(), Color.Teal);

            Texture2D staticTexture = ModContent.Request<Texture2D>("FargowiltasSouls/Content/Sky/MutantStatic", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            int[] xPos = new int[10];
            int[] yPos = new int[10];
            Random rand = new Random(12345);

            for (int i = 0; i < 10; i++)
            {
                xPos[i] = rand.Next((int)dims.X, (int)(dims.X + dims.Width));
                yPos[i] = rand.Next((int)dims.Y, (int)(dims.Y + dims.Height));
            }

            Color color = Color.White;
            float lifeIntensity = 0.5f;

            for (int i = 0; i < 10; i++)
            {
                int width = Main.rand.Next(3, 251);
                Rectangle rect = new Rectangle(xPos[i] - width / 2, yPos[i], width, 3);
                sb.Draw(staticTexture, rect, color * lifeIntensity * 0.75f);
            }

            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);

            return false;
        }

        public override Color ModifyEnabledTextColor(bool enabled, Color color)
        {
            if (enabled)
            {
                return new Color(0, 191, 191);
            }
            else
            {
                return new Color(0, 128, 128);
            }
        }

        public override bool PreDrawModStateTextPanel(UIElement self, bool enabled)
        {
            return false;
        }
    }
}
