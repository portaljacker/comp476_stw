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
    class Map
    {
        private Texture2D tex;
        private char[,] grid;

        public int getTileType(int i, int j)
        {
            return grid[i, j];
        }

        public Map(Texture2D texture)
        {
            tex = texture;

            grid = new char[64, 36];

            for (int col = 0; col < 64; col++)
            {
                for (int row = 0; row < 36; row++)
                {
                    grid[col, row] = '0';
                }

            }

            grid[20, 15] = '1';
            grid[20, 16] = '1';
            grid[20, 17] = '1';
            grid[20, 18] = '1';
            grid[19, 15] = '1';
            grid[18, 15] = '1';
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int col = 0; col < 64; col++)
            {
                for (int row = 0; row < 36; row++)
                {
                    if (grid[col, row] == '0')
                    {
                        spriteBatch.Draw(tex, new Vector2(10 + (col * 20), 10 + (row * 20)), new Rectangle(0, 0, 20, 20), Color.White, 0f, new Vector2(10, 10), 1.0f, SpriteEffects.None, 0);
                    }

                    else
                    {
                        spriteBatch.Draw(tex, new Vector2(10 + (col * 20), 10 + (row * 20)), new Rectangle(20, 0, 20, 20), Color.White, 0f, new Vector2(10, 10), 1.0f, SpriteEffects.None, 0);
                    }
                }
            }
        }
    }
}

