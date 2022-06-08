using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using System;

namespace DefaultFarmer
{
    public class CharacterCustomizationDefaults : CharacterCustomization
    {
        private float loadScale = 1f;
        private readonly float loadBaseScale = 1f;

        private Color loadButttonColor = Game1.textColor;
        private ClickableComponent loadButton;

        private float saveScale = 1f;
        private readonly float saveBaseScale = 1f;

        private Color saveButttonColor = Game1.textColor;
        private ClickableComponent saveButton;

        public CharacterCustomizationDefaults(Clothing item) : base(item)
        {
            setUpPositions();
        }

        public CharacterCustomizationDefaults(Source source) : base(source)
        {
            setUpPositions();
        }

        public void SaveDefaults()
        {
            ModEntry.SaveDefaults(this);
        }

        public void LoadDefaults()
        {
            ModEntry.LoadDefaults(this);
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);
            setUpPositions();
        }

        public override void performHoverAction(int x, int y)
        {
            base.performHoverAction(x, y);

            if (loadButton.containsPoint(x, y))
            {
                loadScale = Math.Min(loadScale + 0.04f, loadBaseScale + 0.25f);
                loadButttonColor = Game1.unselectedOptionColor;
            }
            else if (saveButton.containsPoint(x, y))
            {
                saveScale = Math.Min(saveScale + 0.04f, saveBaseScale + 0.25f);
                saveButttonColor = Game1.unselectedOptionColor;
            }
            else
            {
                loadScale = Math.Max(loadScale - 0.04f, loadBaseScale);
                saveScale = Math.Max(saveScale - 0.04f, saveBaseScale);
                loadButttonColor = Game1.textColor;
                saveButttonColor = Game1.textColor;
            }
        }

        private void setUpPositions()
        {
            string loadText = "Load";
            string saveText = "Save";

            Vector2 loadTextSize = Game1.smallFont.MeasureString(loadText);
            loadButton = new(new(xPositionOnScreen + spaceToClearSideBorder + 300 - 48 + borderWidth, skipIntroButton.bounds.Y + 80, (int)loadTextSize.X, (int)loadTextSize.Y), "loadButton", loadText);

            Vector2 saveTextSize = Game1.smallFont.MeasureString(saveText);
            saveButton = new(new(xPositionOnScreen + spaceToClearSideBorder + 300 - 48 + borderWidth + loadButton.bounds.Width + 48, skipIntroButton.bounds.Y + 80, (int)saveTextSize.X, (int)saveTextSize.Y), "saveButton", saveText);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, playSound);

            if (loadButton != null && loadButton.containsPoint(x, y))
            {
                if (playSound)
                    Game1.playSound("bigSelect");
                LoadDefaults();
            }
            else if (saveButton != null && saveButton.containsPoint(x, y))
            {
                if (playSound)
                    Game1.playSound("bigSelect");
                SaveDefaults();
            }
        }

        public void DrawButtons(SpriteBatch b)
        {
            drawTextureBox(
                b,
                Game1.menuTexture,
                new Rectangle(0, 256, 60, 60),
                saveButton.bounds.X - 16,
                saveButton.bounds.Y - 16,
                saveButton.bounds.Width + 32,
                saveButton.bounds.Height + 24,
                Color.White,
                drawShadow: false,
                scale: saveScale
            );

            Utility.drawBoldText(
                b,
                saveButton.label,
                Game1.smallFont,
                new Vector2(
                    saveButton.bounds.X,
                    saveButton.bounds.Y
                ),
                saveButttonColor
            );

            drawTextureBox(
                b,
                Game1.menuTexture,
                new Rectangle(0, 256, 60, 60),
                loadButton.bounds.X - 16,
                loadButton.bounds.Y - 16,
                loadButton.bounds.Width + 32,
                loadButton.bounds.Height + 24,
                Color.White,
                drawShadow: false,
                scale: loadScale
            );

            Utility.drawBoldText(
                b,
                loadButton.label,
                Game1.smallFont,
                new Vector2(
                    loadButton.bounds.X,
                    loadButton.bounds.Y
                ),
                loadButttonColor
            );
        }

        public override void draw(SpriteBatch b)
        {
            base.draw(b);
            DrawButtons(b);
            drawMouse(b);
        }
    }
}
