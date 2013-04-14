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
    public class Line
    {
        private Texture2D square;

        public Vector2 EndPointOne;
        public Vector2 EndPointTwo;
        public Color lineColor = Color.Black;

        public Line(Vector2 pointOne, Vector2 pointTwo, ContentManager content)
        {
            EndPointOne = pointOne;
            EndPointTwo = pointTwo;

            square = content.Load<Texture2D>("Square");
        }

        public void Draw(SpriteBatch batch)
        {
            float distance = Vector2.Distance(EndPointOne, EndPointTwo);
            float rotationAngle = (float)Math.Atan2((double)(EndPointTwo.Y - EndPointOne.Y),
                (double)(EndPointTwo.X - EndPointOne.X));

            batch.Draw(square, EndPointOne, new Rectangle(0, 0, 1, 4), lineColor,
                rotationAngle, Vector2.Zero, new Vector2(distance, 1), SpriteEffects.None, 0);
        }
    }
}
