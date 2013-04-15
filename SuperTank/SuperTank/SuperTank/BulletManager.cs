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
    class BulletManager
    {
        private List<Bullet> bullets;
        private Texture2D tex;

        public List<Bullet> Bullets
        {
            get
            {
                return bullets;
            }
        }

        public BulletManager(Texture2D text)
        {
            tex = text;
            bullets = new List<Bullet>();
        }

        public void createBullet(int bColor, Vector2 pos, Vector2 target)
        {
            bullets.Add(new Bullet(tex, bColor, pos, target));
        }

        public void update(Map m, Tank t1, List<EnemyTanks> others)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i] != null)
                {
                    Vector2 direction = bullets[i].Target - bullets[i].Position;

                    if (direction.Length() <= 1.5)
                    {
                        bullets[i].LifeTime = 0;
                    }

                    direction.Normalize();
                    bullets[i].Position += direction * 2.7f;

                    bullets[i].LifeTime--;

                    int x = (int)Math.Floor(bullets[i].Position.X / 20);
                    int y = (int)Math.Floor(bullets[i].Position.Y / 20);

                    if (x < 0 || x > 63 || y < 0 || y > 35 || m.getTileType(x, y) == '1' || bullets[i].LifeTime < 1)
                    {
                        bullets.RemoveAt(i);
                        i--;
                        break;

                    }

                    if ((bullets[i].Position - t1.Position).Length() <= 31 && bullets[i].BulletColor != t1.TankColor)
                    {
                        foreach (BoundingSphere s in t1.Spheres)
                        {
                            Vector2 spherePos = new Vector2(s.Center.X, s.Center.Y);
                            if ((bullets[i].Position - spherePos).Length() <= 10)
                            {
                                bullets.RemoveAt(i);
                                i--;
                                break;

                            }
                        }
                    }

                    
                    foreach (EnemyTanks e in others)
                    {
                        if ((bullets[i].Position - e.Position).Length() <= 31 && bullets[i].BulletColor != e.TankColor)
                        {
                            foreach (BoundingSphere s in e.Spheres)
                            {
                                Vector2 spherePos = new Vector2(s.Center.X, s.Center.Y);
                                if ((bullets[i].Position - spherePos).Length() <= 10)
                                {
                                    bullets.RemoveAt(i);
                                    i--;
                                    break;
                                }
                            }
                        }
                    }
                    
                }
            }


        }
    }
}

