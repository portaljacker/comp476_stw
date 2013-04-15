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
        private float theta;        // point in circle for wander behavior
        private const float MAXSPEED = 0.01f;
        //private float orientation; //Used to allign character with target.

        new public int LivesRemaining = 3;

        public EnemyTanks(Texture2D tex, Vector2 pos, int color)
            : base(tex, pos, color)
        {

        }

        public void Wander()
        {
            Random rand = new Random();

            // parameters for the target circle the wandering bot will be seeking
            float radius = 15f;
            float distance = 10f;
            float rotStep = 0.2f;

            // place a circle in front of the wandering bot
            Vector2 targetCircle = new Vector2((float)Math.Cos((ChassisAngle)) * 1.5f, (float)Math.Sin((ChassisAngle)) * 1.5f); ;
            targetCircle.Normalize();
            targetCircle *= distance;
            targetCircle += Position;

            // calculate the angle where the point on the circle will be
            ChassisAngle += (float)rand.NextDouble() * (rotStep - (-rotStep)) - rotStep;

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
            Position += new Vector2((float)Math.Cos((ChassisAngle)) * 1f, (float)Math.Sin((ChassisAngle)) * 1f); ;

            CannonAngle = ChassisAngle;

            updateSpheres();
        }

    }
}
