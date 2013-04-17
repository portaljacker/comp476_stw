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
        //private Vector2 position;
        //private Vector2 velocity; = new Vector2((float)Math.Cos((ChassisAngle)) * 1f, (float)Math.Sin((ChassisAngle)) * 1f);
        //private float theta;        // point in circle for wander behavior
        private const float MAXSPEED = 0.01f;
        private int count;
        private bool foundTarget;
        private Tank target;
        //private float orientation; //Used to allign character with target.

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

        public EnemyTanks(Texture2D tex, Vector2 pos, int color)
            : base(tex, pos, color)
        {
            count = 10;
            this.CannonAngle = this.ChassisAngle;
        }

        

        public bool scan(Tank t, List<EnemyTanks>others)
        {
            for (int j = 0; j < t.Spheres.Count; j++)
            {
                if (this.ShootSphere.Intersects(t.Spheres[j]) && foundTarget == false)
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
                       if (this.ShootSphere.Intersects(et.Spheres[j]) && foundTarget == false)
                       {
                           this.foundTarget = true;
                           this.target = et;
                           return foundTarget;
                       }
                   }
 
                }
            }

            return false;
            
        }

        public void Wander(Map m, Tank t, List<EnemyTanks>others)
        {
            Random rand = new Random();

            // parameters for the target circle the wandering bot will be seeking
            float radius = 1.1f;
            float distance = 1.5f;
            float rotStep = 1.0f;

            // place a circle in front of the wandering bot
            Vector2 targetCircle = new Vector2((float)Math.Cos((ChassisAngle))*1.5f, (float)Math.Sin((ChassisAngle))*1.5f);
            targetCircle.Normalize();
            targetCircle *= distance;
            targetCircle += Position;

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

            // calculate the point on the circle and store it
            Vector2 targetOffset = new Vector2(radius * (float)Math.Cos(ChassisAngle), radius * (float)Math.Sin(ChassisAngle));
            Vector2 targetPos = targetCircle + targetOffset;

            // use Seek to get to the newly calculated point
            Vector2 targetDistance = targetPos - Position;
            targetDistance.Normalize();
            targetDistance *= new Vector2((float)Math.Cos((ChassisAngle)) * 1f, (float)Math.Sin((ChassisAngle)) * 1f);

            if (targetDistance.Length() > MAXSPEED)
            {
                targetDistance.Normalize();
                targetDistance.X *= MAXSPEED;
                targetDistance.Y *= MAXSPEED;
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
        }



    }
}
