//This is the class for AI tanks, which are children of the Tank class.
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

        new public int LivesRemaining = 5;

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

        //Constructor
        public EnemyTanks(Texture2D tex, Vector2 pos, int color)
            : base(tex, pos, color)
        {
            count = 10;
            seekCount = 100;
            this.CannonAngle = this.ChassisAngle;
        }

        
        //This method uses the bounding sphere in front of the tank to scan for nearby tanks.
        public bool scan(Tank t, List<EnemyTanks>others)
        {
            //Do nothing if this tank is defeated.
            if (this.LivesRemaining <= 0)
            {
                return false;
            }

            //If the player tank is spotted, it is the new target.
            for (int j = 0; j < t.Spheres.Count; j++)
            {
                //The player is only added as a target if the seek timer has been reset.
                if (this.ShootSphere.Intersects(t.Spheres[j]) && foundTarget == false && t.LivesRemaining > 0 && seekCount == 100)
                {
                    this.foundTarget = true;
                    this.target = t;
                    return foundTarget;
                }
                
            }

            //If enemy tank is spotted, it is the new target.
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

            //If no targets are found, reset scanning variables.
            this.target = null;
            this.foundTarget = false;
            return foundTarget;
            
        }

        //Seek method. The AI tank will seek the player until it gets close enough or hits a wall.
        public void Seek(Map m, Tank t, List<EnemyTanks> others)
        {
            //Don't seek if the target is not the player tank.
            if (target != t)
            {
                return;
            }

            //If seek counter is full, seek player.
            if (seekCount == 100)
            {
                //Update cannon angle.
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

                //Chase until close enough to player tank.
                if ((this.Position - target.Position).Length() >= 105)
                {
                    this.still = false;
                    ChassisAngle = CannonAngle;
                    updateSpheres();
                    this.updatePosition(m, direction, t, others);
                }

                //When close enough, stop chasing player.
                else
                {
                    this.still = true;
                }


            }

        }

        //Wander method.
        public void Wander(Map m, Tank t, List<EnemyTanks>others)
        {
            //If this AI tank is defeated, add its bounding spheres to the list of walls in the map.
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

            //calculate random angle of direction to move in. //Delayed by a counter to avoid jitter.
            if (count == 10)
            {
                //Calculate new angle.
                float tempAngle = (float)rand.NextDouble() * (rotStep - (-rotStep)) - rotStep;
                ChassisAngle += tempAngle;
                updateSpheres();

                //Check if new angle conflicts with walls. If so, undo angle.
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

            //If tank has a target, aim cannon in its direction to fire.
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

            //Else, allign cannon with chassis.
            else
            {
                this.CannonAngle = this.ChassisAngle;
            }

            //Update bounding spheres and delay counter..
            updateSpheres();
            count--;
            if (count == 0)
            {
                count = 10;
            }
        }

        //This method updates position.
        public void updatePosition(Map m, Vector2 pos, Tank t, List<EnemyTanks> others)
        {
            Position += pos; //Add new position.

            //If new position conflicts with walls, undo the position change.
            //Also set off seek delay counter. If tank is seeking player, it will lose sight of them upon collision for a short time.
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

            //If tank is close enough to player tank, check for collision. If collides, undo position change.
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

            //If tank is close enough to other AI tanks, check for collision. If collides, undo position change.
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

            //Update seek delay counter.
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
