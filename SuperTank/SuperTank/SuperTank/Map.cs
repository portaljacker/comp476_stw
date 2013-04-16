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
        private int[,] grid;
        private List<BoundingSphere> walls;

        public int getTileType(int i, int j)
        {
            return grid[i, j];
        }

        public List<BoundingSphere> Walls
        {
            get
            {
                return walls;
            }
        }

        public Map(Texture2D texture)
        {
            tex = texture;

            walls = new List<BoundingSphere>();

            grid = new int[64, 36];

            for (int col = 0; col < 64; col++)
            {
                for (int row = 0; row < 36; row++)
                {
                    grid[col, row] = 0;
                }

            }

            for (int row = 0; row < 35; row++)
            {
                grid[0, row] = 1;
                walls.Add(new BoundingSphere(new Vector3(10 + (0 * 20), 10 + (row * 20), 0), 10));

                grid[63, row] = 1;
                walls.Add(new BoundingSphere(new Vector3(10 + (63 * 20), 10 + (row * 20), 0), 10));
            }

            for (int col = 0; col < 64; col++)
            {
                grid[col, 0] = 1;
                walls.Add(new BoundingSphere(new Vector3(10 + (col * 20), 10 + (0 * 20), 0), 10));

                grid[col, 35] = 5;
                walls.Add(new BoundingSphere(new Vector3(10 + (col * 20), 10 + (35 * 20), 0), 10));
            }

            for (int col = 1; col < 55; col++)
            {
                grid[col, 34] = 4;

            }

            for (int col = 56; col < 63; col++)
            {
                grid[col, 34] = 4;

            }

            for (int col = 1; col < 55; col++)
            {
                grid[col, 33] = 3;

            }

            for (int col = 56; col < 63; col++)
            {
                grid[col, 33] = 3;

            }

            for (int row = 1; row < 5; row++)
            {
                grid[9, row] = 1;
                walls.Add(new BoundingSphere(new Vector3(10 + (9 * 20), 10 + (row * 20), 0), 10));
            }

            for (int col = 1; col < 9; col++)
            {
                grid[col, 27] = 1;
                walls.Add(new BoundingSphere(new Vector3(10 + (col * 20), 10 + (27 * 20), 0), 10));
            }

            for (int col = 5; col < 8; col++)
            {
                for (int row = 15; row < 20; row++)
                {
                    grid[col, row] = 1;
                    walls.Add(new BoundingSphere(new Vector3(10 + (col * 20), 10 + (row * 20), 0), 10));
                }
            }

            for (int col = 56; col < 63; col++)
            {
                grid[col, 8] = 1;
                walls.Add(new BoundingSphere(new Vector3(10 + (col * 20), 10 + (8 * 20), 0), 10));
            }

            for (int row = 28; row < 35; row++)
            {
                grid[55, row] = 1;
                walls.Add(new BoundingSphere(new Vector3(10 + (55 * 20), 10 + (row * 20), 0), 10));

            }

            for (int col = 43; col < 48; col++)
            {
                grid[col, 27] = 1;
                walls.Add(new BoundingSphere(new Vector3(10 + (col * 20), 10 + (27 * 20), 0), 10));
            }

            for (int row = 23; row < 28; row++)
            {
                grid[48, row] = 1;
                walls.Add(new BoundingSphere(new Vector3(10 + (48 * 20), 10 + (row * 20), 0), 10));
            }

            for (int col = 19; col < 49; col++)
            {
                grid[col, 9] = 1;
                walls.Add(new BoundingSphere(new Vector3(10 + (col * 20), 10 + (9 * 20), 0), 10));
            }

            for (int row = 10; row < 19; row++)
            {
                grid[48, row] = 1;
                walls.Add(new BoundingSphere(new Vector3(10 + (48 * 20), 10 + (row * 20), 0), 10));
            }

            for (int row = 6; row < 9; row++)
            {
                grid[48, row] = 1;
                walls.Add(new BoundingSphere(new Vector3(10 + (48 * 20), 10 + (row * 20), 0), 10));
            }

            for (int row = 4; row < 6; row++)
            {
                grid[37, row] = 2;
                walls.Add(new BoundingSphere(new Vector3(10 + (37 * 20), 10 + (row * 20), 0), 10));
            }

            for (int row = 15; row < 17; row++)
            {
                grid[56, row] = 2;
                walls.Add(new BoundingSphere(new Vector3(10 + (56 * 20), 10 + (row * 20), 0), 10));
            }

            grid[29, 14] = 2;
            walls.Add(new BoundingSphere(new Vector3(10 + (29 * 20), 10 + (14 * 20), 0), 10));

            for (int col = 41; col < 43; col++)
            {
                grid[col, 18] = 2;
                walls.Add(new BoundingSphere(new Vector3(10 + (col * 20), 10 + (18 * 20), 0), 10));

            }

            for (int col = 39; col < 41; col++)
            {
                grid[col, 28] = 2;
                walls.Add(new BoundingSphere(new Vector3(10 + (col * 20), 10 + (28 * 20), 0), 10));

            }

            for (int col = 33; col < 35; col++)
            {
                grid[col, 28] = 2;
                walls.Add(new BoundingSphere(new Vector3(10 + (col * 20), 10 + (28 * 20), 0), 10));

            }

            grid[32, 27] = 2;
            walls.Add(new BoundingSphere(new Vector3(10 + (32 * 20), 10 + (27 * 20), 0), 10));

            for (int row = 25; row < 28; row++)
            {
                grid[16, row] = 2;
                walls.Add(new BoundingSphere(new Vector3(10 + (16 * 20), 10 + (row * 20), 0), 10));

            }

            for (int row = 14; row < 20; row++)
            {
                grid[15, row] = 1;
                walls.Add(new BoundingSphere(new Vector3(10 + (15 * 20), 10 + (row * 20), 0), 10));

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int col = 0; col < 64; col++)
            {
                for (int row = 0; row < 36; row++)
                {
                    if (grid[col, row] == 0)
                    {
                        spriteBatch.Draw(tex, new Vector2(10 + (col * 20), 10 + (row * 20)), new Rectangle(0, 0, 20, 20), Color.White, 0f, new Vector2(10, 10), 1.0f, SpriteEffects.None, 0);
                    }

                    else if (grid[col, row] == 1)
                    {
                        spriteBatch.Draw(tex, new Vector2(10 + (col * 20), 10 + (row * 20)), new Rectangle(20, 0, 20, 20), Color.White, 0f, new Vector2(10, 10), 1.0f, SpriteEffects.None, 0);
                    }

                    else if (grid[col, row] == 2)
                    {
                        spriteBatch.Draw(tex, new Vector2(10 + (col * 20), 10 + (row * 20)), new Rectangle(40, 0, 20, 20), Color.White, 0f, new Vector2(10, 10), 1.0f, SpriteEffects.None, 0);
                    }

                    else if (grid[col, row] == 3)
                    {
                        spriteBatch.Draw(tex, new Vector2(10 + (col * 20), 10 + (row * 20)), new Rectangle(60, 0, 20, 20), Color.White, 0f, new Vector2(10, 10), 1.0f, SpriteEffects.None, 0);
                    }

                    else if (grid[col, row] == 4)
                    {
                        spriteBatch.Draw(tex, new Vector2(10 + (col * 20), 10 + (row * 20)), new Rectangle(80, 0, 20, 20), Color.White, 0f, new Vector2(10, 10), 1.0f, SpriteEffects.None, 0);
                    }

                    else
                    {
                        spriteBatch.Draw(tex, new Vector2(10 + (col * 20), 10 + (row * 20)), new Rectangle(100, 0, 20, 20), Color.White, 0f, new Vector2(10, 10), 1.0f, SpriteEffects.None, 0);
                    }
                }
            }
        }
    }
}

