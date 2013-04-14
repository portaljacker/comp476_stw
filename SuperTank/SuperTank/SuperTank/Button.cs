using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SuperTank
{
    public class Button
    {
        Texture2D btnTexture;
        Vector2 btnPos;
        Rectangle btnRect;
        public bool btnActive;

        Color btnColor = new Color(255, 255, 255, 255);

        public Vector2 size;

        GraphicsDevice graph;

        public Button(Texture2D btnText, GraphicsDevice graphics)
        {
            btnTexture = btnText;
            size = new Vector2(graphics.Viewport.Width / 4, graphics.Viewport.Height / 6);
        }

        public bool isClicked;

        public void Update(MouseState mouse)
        {
            btnRect = new Rectangle((int)btnPos.X, (int)btnPos.Y, (int)size.X, (int)size.Y);

            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(btnRect))
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                    isClicked = true;
            }
        }

        public void SetPosition(Vector2 newPosition)
        {
            btnPos = newPosition;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(btnTexture, btnRect, btnColor);
        }
    }
}
