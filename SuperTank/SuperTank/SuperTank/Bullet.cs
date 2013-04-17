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
    class Bullet
    {
        private Texture2D texture;
        private Vector2 position;
        private int width;
        private int height;
        private Vector2 origin;
        private Vector2 target;
        private int lifeTime;
        private int bulletColor;

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

        public Vector2 Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }

        public int LifeTime
        {
            get
            {
                return lifeTime;
            }
            set
            {
                lifeTime = value;
            }
        }

        public int BulletColor
        {
            get
            {
                return bulletColor;
            }
        }

        public Bullet(Texture2D tex, int c, Vector2 pos, Vector2 tar)
        {
            texture = tex;
            width = 9;
            height = 9;
            origin = new Vector2(width / 2, height / 2);
            position = pos;
            target = tar;
            lifeTime = 100;//530;
            bulletColor = c;
        }

        /*public void Move()
        {

        }*/

        public void Draw(SpriteBatch spriteBatch)
        {
            Color c = Color.Black;

            switch (bulletColor)
            {
                case 0:
                    c = Color.Blue;
                    break;
                case 1:
                    c = Color.Red;
                    break;
                case 2:
                    c = Color.Green;
                    break;
                case 3:
                    c = Color.Yellow;
                    break;
            }

            spriteBatch.Draw(Texture, Position, new Rectangle(0, 0, 9, 9), c, 0f, Origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}
