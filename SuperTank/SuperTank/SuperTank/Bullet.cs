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

        public Bullet(Texture2D tex, Vector2 pos, Vector2 tar)
        {
            texture = tex;
            width = 9;
            height = 9;
            origin = new Vector2(width / 2, height / 2);
            position = pos;
            target = tar;
            lifeTime = 530;
        }

        public void Move()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, new Rectangle(0, 0, 9, 9), Color.White, 0f, Origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}
