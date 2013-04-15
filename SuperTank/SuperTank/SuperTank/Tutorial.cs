using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SuperTank
{
    public class Tutorial : DrawableGameComponent
    {
        private Texture2D mouse;
        private Texture2D keys;

        private Viewport viewPort;
        private ContentManager content;

        private Vector2 mousePos;
        private Vector2 keysPos;

        private SpriteFont controlsText;

        private Dictionary<Keys, Line> controlsLines;
        private Line mouseLines;
        private Line mouseLines1;

        public string AKeyText = string.Empty;
        public string WKeyText = string.Empty;
        public string SKeyText = string.Empty;
        public string DKeyText = string.Empty;
        public string LeftMouseClickText = string.Empty;
        public string MouseMoveText = string.Empty;

        public Tutorial(Game game, ContentManager theContent, Viewport theViewport, GraphicsDevice theGraphicsDevice)
            : base(game)
        {
            viewPort = theViewport;
            content = theContent;

            mouse = theContent.Load<Texture2D>("mouse");
            keys = theContent.Load<Texture2D>("keyboardcontrols");
            controlsText = theContent.Load<SpriteFont>("ControllerText");

            PositionControllerImages();
            InitializeControllerLines();
        }

        private void PositionControllerImages()
        {
            int keysHeight = (keys.Height / 150) - (keys.Height / 2) - keys.Height / 5;

            int aYPosition = (int)((viewPort.Height - (keysHeight + mouse.Height + 100)));

            mousePos.X = (int)((viewPort.Width - mouse.Width) / 2);
            keysPos.X = (int)((viewPort.Width - keys.Width) / 2);

            keysPos.Y = aYPosition;
            mousePos.Y = (keysPos.Y / 8) - 150;
        }

        private void InitializeControllerLines()
        {
            controlsLines = new Dictionary<Keys, Line>();

            mouseLines = new Line(new Vector2(((Game1)Game).graphics.PreferredBackBufferWidth / 5,
                (((Game1)Game).graphics.PreferredBackBufferHeight / 2) - (((Game1)Game).graphics.PreferredBackBufferHeight / 4)),
                new Vector2(((Game1)Game).graphics.PreferredBackBufferWidth / 2 - (((Game1)Game).graphics.PreferredBackBufferWidth / 20),
                    (((Game1)Game).graphics.PreferredBackBufferHeight / 2) - (((Game1)Game).graphics.PreferredBackBufferHeight / 4) - (((Game1)Game).graphics.PreferredBackBufferHeight / 12)), content);

            mouseLines1 = new Line(new Vector2((2 * ((Game1)Game).graphics.PreferredBackBufferWidth / 3),
                (((Game1)Game).graphics.PreferredBackBufferHeight / 2) - (((Game1)Game).graphics.PreferredBackBufferHeight / 4)),
                new Vector2(((Game1)Game).graphics.PreferredBackBufferWidth / 2,
                    (((Game1)Game).graphics.PreferredBackBufferHeight / 2) - (((Game1)Game).graphics.PreferredBackBufferHeight / 8)), content);

            controlsLines.Add(Keys.A, new Line(new Vector2(((Game1)Game).graphics.PreferredBackBufferWidth / 7,
                (((Game1)Game).graphics.PreferredBackBufferHeight / 2) + (((Game1)Game).graphics.PreferredBackBufferHeight / 4)),
                new Vector2(((Game1)Game).graphics.PreferredBackBufferWidth / 2 - (((Game1)Game).graphics.PreferredBackBufferWidth / 12),
                    (((Game1)Game).graphics.PreferredBackBufferHeight / 2) + (((Game1)Game).graphics.PreferredBackBufferHeight / 4)), content));

            controlsLines.Add(Keys.W, new Line(new Vector2(((3 * ((Game1)Game).graphics.PreferredBackBufferWidth) / 5) + ((Game1)Game).graphics.PreferredBackBufferWidth / 10,
                (((Game1)Game).graphics.PreferredBackBufferHeight / 2) - ((Game1)Game).graphics.PreferredBackBufferHeight / 10),
                new Vector2(((4 * ((Game1)Game).graphics.PreferredBackBufferWidth) / 8),
                    (2 * ((Game1)Game).graphics.PreferredBackBufferHeight / 3) - (((Game1)Game).graphics.PreferredBackBufferHeight / 35)), content));

            //controlsLines.Add(Buttons.Y, new Line(new Vector2(896, 305 - aYOffset), new Vector2(777, 350 - aYOffset), content));

            controlsLines.Add(Keys.D, new Line(new Vector2(((3 * ((Game1)Game).graphics.PreferredBackBufferWidth) / 5) + ((Game1)Game).graphics.PreferredBackBufferWidth / 10,
                (3 * ((Game1)Game).graphics.PreferredBackBufferHeight / 4) + ((Game1)Game).graphics.PreferredBackBufferHeight / 10),
                new Vector2(((4 * ((Game1)Game).graphics.PreferredBackBufferWidth) / 8) + ((Game1)Game).graphics.PreferredBackBufferWidth / 14,
                    (2 * ((Game1)Game).graphics.PreferredBackBufferHeight / 3) + (((Game1)Game).graphics.PreferredBackBufferHeight / 15)), content));

            controlsLines.Add(Keys.S, new Line(new Vector2((((Game1)Game).graphics.PreferredBackBufferWidth) / 2,
                (2 * ((Game1)Game).graphics.PreferredBackBufferHeight / 3) + (((Game1)Game).graphics.PreferredBackBufferHeight / 5)),
                new Vector2((((Game1)Game).graphics.PreferredBackBufferWidth) / 2,
                    (2 * ((Game1)Game).graphics.PreferredBackBufferHeight / 3) + (((Game1)Game).graphics.PreferredBackBufferHeight / 12)), content));
        }

        public void Draw(SpriteBatch batch)
        {
            // reposition the controller images and reposition all the lines
            PositionControllerImages();
            InitializeControllerLines();

            // draw the top and front controller images. If any of the top
            // buttons have been assigned text, then the top controller
            // image will be drawn, otherwise, just the front view will be drawn

            batch.Draw(keys, keysPos, Color.White);

            batch.Draw(mouse, mousePos, Color.White);

            // draw the controller text assigned for each button on the controller
            DrawControllerText(WKeyText, Keys.W, batch, false);
            DrawControllerText(AKeyText, Keys.A, batch, true);
            DrawControllerText(DKeyText, Keys.D, batch, false);
            DrawControllerText(SKeyText, Keys.S, batch, false);

            DrawControllerText(LeftMouseClickText, batch, true);
            DrawControllerText1(MouseMoveText, batch, false);
        }

        // draw the line and the text that accompanies it
        private void DrawControllerText(string btnText, Keys btn, SpriteBatch batch, bool isLeft)
        {
            if (btnText != string.Empty)
            {
                controlsLines[btn].lineColor = Color.Black;
                controlsLines[btn].Draw(batch);

                int aXPos = 0;
                if (isLeft)
                    aXPos = (int)controlsLines[btn].EndPointOne.X - (int)controlsText.MeasureString(btnText).X;
                else
                    aXPos = (int)controlsLines[btn].EndPointOne.X;

                int aYPos = (int)controlsLines[btn].EndPointOne.Y - (int)(controlsText.MeasureString(btnText).Y / 2);
                batch.DrawString(controlsText, btnText, new Vector2(aXPos, aYPos), Color.Black);
            }
        }

        private void DrawControllerText(string mouseText, SpriteBatch batch, bool isLeft)
        {
            if (mouseText != string.Empty)
            {
                mouseLines.lineColor = Color.Black;
                mouseLines.Draw(batch);

                int aXPos = 0;
                if (isLeft)
                    aXPos = (int)mouseLines.EndPointOne.X - (int)controlsText.MeasureString(mouseText).X;
                else
                    aXPos = (int)mouseLines.EndPointOne.X;

                int aYPos = (int)mouseLines.EndPointOne.Y - (int)(controlsText.MeasureString(mouseText).Y / 2);
                batch.DrawString(controlsText, mouseText, new Vector2(aXPos, aYPos), Color.Black);
            }
        }

        private void DrawControllerText1(string mouseText, SpriteBatch batch, bool isLeft)
        {
            if (mouseText != string.Empty)
            {
                mouseLines1.lineColor = Color.Black;
                mouseLines1.Draw(batch);

                int aXPos1 = 0;
                if (isLeft)
                    aXPos1 = (int)mouseLines1.EndPointOne.X - (int)controlsText.MeasureString(mouseText).X;
                else
                    aXPos1 = (int)mouseLines1.EndPointOne.X;

                int aYPos1 = (int)mouseLines1.EndPointOne.Y - (int)(controlsText.MeasureString(mouseText).Y / 2);
                batch.DrawString(controlsText, mouseText, new Vector2(aXPos1, aYPos1), Color.Black);
            }
        }
    }
}


