using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SuperTank
{
    class Tank
    {
        private Texture2D texture;
        private Vector2 position;
        private int width;
        private int height;
        private Vector2 origin;
        private int hp;
        private Rectangle chassis;
        private Rectangle cannon;
        private int currentFrame;
        private int tankColor;
        private float chassisAngle;
        private float cannonAngle;
        //private BoundingBox tankBox;

        //Accessors and mutators.
        public Texture2D Texture
        {
            get
            {
                return texture;
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public Vector2 Origin
        {
            get
            {
                return origin;
            }

            set
            {
                origin = value;
            }
        }

        public int Width
        {
            get
            {
                return width;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
        }

        public int TankColor
        {
            get
            {
                return tankColor;
            }
        }

        public int CurrentFrame
        {
            get
            {
                return currentFrame;
            }
            set
            {
                currentFrame = value;
            }
        }

        public float ChassisAngle
        {
            get
            {
                return chassisAngle;
            }

            set
            {
                chassisAngle = value;
            }
        }

        public float CannonAngle
        {
            get
            {
                return cannonAngle;
            }

            set
            {
                cannonAngle = value;
            }
        }

        public Tank(Texture2D tex, Vector2 pos, int color)
        {
            texture = tex;
            width = 62;
            height = 42;
            origin = new Vector2(width / 2, height / 2);
            hp = 100;
            position = pos;
            tankColor = color;
            chassis = new Rectangle(width * currentFrame, height * tankColor, width, height);
            cannon = new Rectangle(width * 2, height * tankColor, width, height);
            currentFrame = 0;
            chassisAngle = 0;
            cannonAngle = 0;
            //tankBox = new BoundingBox(,);
        }

        public void updateRectangles()
        {
            chassis = new Rectangle(width * currentFrame, height * tankColor, width, height);
            cannon = new Rectangle(width * 2, height * tankColor, width, height);
        }

        //Draw method, using above data.
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, chassis, Color.White, chassisAngle, Origin, 1.0f, SpriteEffects.None, 0);
            spriteBatch.Draw(Texture, Position, cannon, Color.White, cannonAngle, Origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}
