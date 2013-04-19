//The Bullet and BulletManager classes were based on the following tutorial: http://rbwhitaker.wikidot.com/2d-particle-engine-1

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
        //Holds a list of bullet objects and the texture to use for them.
        private List<Bullet> bullets;
        private Texture2D tex;

        public List<Bullet> Bullets
        {
            get
            {
                return bullets;
            }
        }

        //Constructor.
        public BulletManager(Texture2D text)
        {
            tex = text;
            bullets = new List<Bullet>();
        }

        //Creates a new bullet object.
        public void createBullet(int bColor, Vector2 pos, Vector2 target)
        {
            bullets.Add(new Bullet(tex, bColor, pos, target));
        }

        //This method updates the active bullets.
        public void update(Map m, Tank t1, List<EnemyTanks> others)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i] != null)
                {
                    Vector2 direction = bullets[i].Target - bullets[i].Position; //Move in target's direction.

                    if (direction.Length() <= 1.5) //Don't fire if aimed very close to the firing tank.
                    {
                        bullets[i].LifeTime = 0;
                    }

                    direction.Normalize();
                    bullets[i].Position += direction * 2.7f;

                    bullets[i].LifeTime--;

                    int x = (int)Math.Floor(bullets[i].Position.X / 20);
                    int y = (int)Math.Floor(bullets[i].Position.Y / 20);

                    //If bullet is out of bounds, over a wall tile or out of lifetime(fire range), remove bullet object.
                    if (x < 0 || x > 63 || y < 0 || y > 35 || m.getTileType(x, y) == 1 || bullets[i].LifeTime < 1)
                    {
                        bullets.RemoveAt(i);
                        i--;
                        break;

                    }

                    if (i >= 0)
                    {
                        //If bullet is within range of player tank, if it overlaps wth any of its spheres, remove bullet object.
                        if ((bullets[i].Position - t1.Position).Length() <= 31 && bullets[i].BulletColor != t1.TankColor)
                        {
                            foreach (BoundingSphere s in t1.Spheres)
                            {
                                Vector2 spherePos = new Vector2(s.Center.X, s.Center.Y);
                                if ((bullets[i].Position - spherePos).Length() <= 10)
                                {
                                    if (t1.LivesRemaining > 0)
                                    {
                                        t1.LivesRemaining--;
                                    }
                                    bullets.RemoveAt(i);
                                    i--;
                                    break;

                                }
                            }
                        }
                    }

                    if (i >= 0)
                    {
                        foreach (EnemyTanks e in others)
                        {
                            if (i >= 0)
                            {
                                //If bullet is within range of another AI tank, if it overlaps wth any of its spheres, remove bullet object.
                                if ((bullets[i].Position - e.Position).Length() <= 31 && bullets[i].BulletColor != e.TankColor)
                                {
                                    foreach (BoundingSphere s in e.Spheres)
                                    {
                                        Vector2 spherePos = new Vector2(s.Center.X, s.Center.Y);
                                        if ((bullets[i].Position - spherePos).Length() <= 10)
                                        {
                                            if (e.LivesRemaining > 0)
                                            {
                                                e.LivesRemaining--;
                                            }
                                            bullets.RemoveAt(i);
                                            i--;
                                            break;
                                        }
                                    }
                                }
                            }

                            else
                            {
                                break;
                            }
                        }
                    }
                    
                }
            }


        }
    }
}

