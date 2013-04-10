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

        public void createBullet(Vector2 pos, Vector2 target)
        {
           bullets.Add(new Bullet(tex, pos, target));
        }

        public void update()
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i] != null)
                {
                    Vector2 direction = bullets[i].Target - bullets[i].Position;
                    direction.Normalize();

                    bullets[i].Position += direction*2.7f;

                    bullets[i].LifeTime--;

                    if (bullets[i].Position == bullets[i].Target || bullets[i].LifeTime < 1)
                    {
                        bullets.RemoveAt(i);
                        i--;
                    }
                }
            }


        }
    }
}
