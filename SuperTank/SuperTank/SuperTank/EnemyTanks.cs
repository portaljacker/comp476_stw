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
using SuperTank;



namespace SuperTank
{
    class EnemyTanks : Tank
    {
        private int count;
        private bool foundTarget;
        private Tank target;
        private bool addedToWalls = false;
        private int seekCount;
        private bool still = false;

        new public int LivesRemaining = 3;

        public bool FoundTarget
        {
            get
            {
                return foundTarget;
            }
            set
            {
                foundTarget = value;
            }
        }

        public Tank Target
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

        public bool Still
        {
            get
            {
                return still;
            }
        }

        public EnemyTanks(Texture2D tex, Vector2 pos, int color)
            : base(tex, pos, color)
        {
            count = 10;
            seekCount = 100;
            this.CannonAngle = this.ChassisAngle;
        }

        

        public bool scan(Tank t, List<EnemyTanks>others)
        {
            if (this.LivesRemaining <= 0)
            {
                return false;
            }

            for (int j = 0; j < t.Spheres.Count; j++)
            {
                if (this.ShootSphere.Intersects(t.Spheres[j]) && foundTarget == false && t.LivesRemaining > 0 && seekCount == 100)
                {
                    this.foundTarget = true;
                    this.target = t;
                    return foundTarget;
                }
                
            }

            foreach (EnemyTanks et in others)
            {
                if (this.TankColor != et.TankColor)
                {

                   for (int j = 0; j < et.Spheres.Count; j++)
                   {
                       if (this.ShootSphere.Intersects(et.Spheres[j]) && foundTarget == false && et.LivesRemaining > 0)
                       {
                           this.foundTarget = true;
                           this.target = et;
                           return foundTarget;
                       }
                   }
 
                }
            }

            this.target = null;
            this.foundTarget = false;
            return foundTarget;
            
        }

        public void Seek(Map m, Tank t, List<EnemyTanks> others)
        {
            if (target != t)
            {
                return;
            }

            if (seekCount == 100)
            {
                Vector2 aim = new Vector2(this.Target.Position.X, this.Target.Position.Y) - new Vector2(this.Position.X, this.Position.Y);
                if (this.Target.Position.X > this.Position.X && this.Target.Position.Y > this.Position.Y)
                {
                    this.CannonAngle = (float)Math.Atan2(aim.Y, aim.X);
                }
                else
                {
                    this.CannonAngle = (float)Math.Atan2(aim.Y, aim.X);
                }

                Vector2 direction = target.Position - Position;


                direction.Normalize();
                Vector2 newPos = new Vector2((float)Math.Cos((ChassisAngle)) * direction.X, (float)Math.Sin((ChassisAngle)) * direction.Y);

                if ((this.Position - target.Position).Length() >= 105)
                {
                    this.still = false;
                    ChassisAngle = CannonAngle;
                    updateSpheres();
                    this.updatePosition(m, direction, t, others);
                }

                else
                {
                    this.still = true;
                }


            }

        }

        public void Wander(Map m, Tank t, List<EnemyTanks>others)
        {
            if(this.LivesRemaining <= 0)
            {
                if (!addedToWalls)
                {
                    foreach (BoundingSphere sp in this.Spheres)
                    {
                        m.Walls.Add(sp);
                    }

                    addedToWalls = true;
                }

               return;

            }

            Random rand = new Random();

            float rotStep = 1.0f;

            // calculate the angle where the point on the circle will be
            if (count == 10)
            {
                float tempAngle = (float)rand.NextDouble() * (rotStep - (-rotStep)) - rotStep;
                ChassisAngle += tempAngle;
                updateSpheres();

                foreach (BoundingSphere b in m.Walls)
                {
                    for (int i = 0; i < this.Spheres.Count; i++)
                    {

                        if (this.Spheres[i].Intersects(b) || this.AvoidanceSphere.Intersects(b))
                        {
                            ChassisAngle -= tempAngle;
                            updateSpheres();
                            break;
                        }
                    }
                }
            }

            // update position
            Vector2 newPos = new Vector2((float)Math.Cos((ChassisAngle)) * 1f, (float)Math.Sin((ChassisAngle)) * 1f);
            updatePosition(m, newPos, t, others);


            if (this.Target!=null)
            {
                Vector2 aim = new Vector2(this.Target.Position.X, this.Target.Position.Y) - new Vector2(this.Position.X, this.Position.Y);
                if (this.Target.Position.X > this.Position.X && this.Target.Position.Y > this.Position.Y)
                {
                    this.CannonAngle = (float)Math.Atan2(aim.Y, aim.X);
                }
                else
                {
                    this.CannonAngle = (float)Math.Atan2(aim.Y, aim.X);
                }
            }

            else
            {
                this.CannonAngle = this.ChassisAngle;
            }

            updateSpheres();
            count--;
            if (count == 0)
            {
                count = 10;
            }
        }

        public void updatePosition(Map m, Vector2 pos, Tank t, List<EnemyTanks> others)
        {
            Position += pos;

            foreach (BoundingSphere b in m.Walls)
            {
                for (int i = 0; i < this.Spheres.Count; i++)
                {
                    if (this.Spheres[i].Intersects(b))
                    {
                        if (seekCount == 100 && seekCount > 0)
                        {
                            seekCount--;
                        }
                       
                        this.target = null;
                        this.Position -= pos;
                        this.updateSpheres();
                        break;
                    }
                }
            }

            if ((this.Position - t.Position).Length() <= 65)
            {
                for (int i = 0; i < this.Spheres.Count; i++)
                {
                    for (int j = 0; j < t.Spheres.Count; j++)
                    {
                        if (this.Spheres[i].Intersects(t.Spheres[j]))
                        {
                            this.Position -= pos;
                            this.updateSpheres();
                            break;
                        }
                    }
                }
            }

            foreach (EnemyTanks et in others)
            {
                if (this.TankColor != et.TankColor)
                {
                    if ((this.Position - et.Position).Length() <= 65)
                    {
                        for (int i = 0; i < this.Spheres.Count; i++)
                        {
                            for (int j = 0; j < et.Spheres.Count; j++)
                            {
                                if (this.Spheres[i].Intersects(et.Spheres[j]))
                                {
                                    this.Position -= pos;
                                    this.updateSpheres();
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (seekCount < 100 && seekCount > 0)
            {
                seekCount--;
            }

            if (seekCount == 0)
            {
                seekCount = 100;
            }
        }



    }
}
