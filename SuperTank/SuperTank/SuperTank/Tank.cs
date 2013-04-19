//This is the Tank class. It stores data common to both player and AI tanks.

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
    class Tank
    {
        private Texture2D texture;
        public static Texture2D texture2;
        private Vector2 position;
        private int width;
        private int height;
        private Vector2 origin;
        private Rectangle chassis;
        private Rectangle cannon;
        private int currentFrame;
        private int tankColor;
        private float chassisAngle;
        private float cannonAngle;
        private List<BoundingSphere> spheres; //List of spheres for collision detection.
        public int livesRemaining = 5;
        private BoundingSphere avoidanceSphere; //Sphere in front of tank for wall avoidance.
        private BoundingSphere shootSphere; //Large sphere for detecting other nearby tanks.
        public static bool DrawBSPheres { get; set; }

        //Accessors and mutators.
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

        public int TankColor
        {
            get
            {
                return tankColor;
            }
            set
            {
                tankColor = value;
            }
        }

        public int CurrentFrame
        {
            get
            {
                return currentFrame;
            }
            set
            {
                currentFrame = value;
            }
        }

        public float ChassisAngle
        {
            get
            {
                return chassisAngle;
            }

            set
            {
                chassisAngle = value;
            }
        }

        public float CannonAngle
        {
            get
            {
                return cannonAngle;
            }

            set
            {
                cannonAngle = value;
            }
        }
        public List<BoundingSphere> Spheres
        {
            get
            {
                return spheres;
            }
        }

        public int LivesRemaining
        {
            get
            {
                return livesRemaining;
            }

            set
            {
                livesRemaining = value;
            }
        }

        public BoundingSphere AvoidanceSphere
        {
            get
            {
                return avoidanceSphere;
            }
        }

        public BoundingSphere ShootSphere
        {
            get
            {
                return shootSphere;
            }
        }

        //Constructor
        public Tank(Texture2D tex, Vector2 pos, int color)
        {
            texture = tex;
            width = 62;
            height = 42;
            origin = new Vector2(width / 2, height / 2);
            position = pos;
            tankColor = color;
            chassis = new Rectangle(width * currentFrame, height * tankColor, width, height);
            cannon = new Rectangle(width * 2, height * tankColor, width, height);
            currentFrame = 0;
            chassisAngle = 0;
            cannonAngle = 0;
            spheres = new List<BoundingSphere>();
            //Add the six spheres to the list of spheres.
            spheres.Add(new BoundingSphere(new Vector3(position.X - 20, position.Y - 10, 0), 10));
            spheres.Add(new BoundingSphere(new Vector3(position.X - 20, position.Y + 10, 0), 10));
            spheres.Add(new BoundingSphere(new Vector3(position.X, position.Y - 10, 0), 10));
            spheres.Add(new BoundingSphere(new Vector3(position.X, position.Y + 10, 0), 10));
            spheres.Add(new BoundingSphere(new Vector3(position.X + 20, position.Y - 10, 0), 10));
            spheres.Add(new BoundingSphere(new Vector3(position.X + 20, position.Y + 10, 0), 10));
            avoidanceSphere = new BoundingSphere(new Vector3(position.X + 30, position.Y, 0), 30);
            shootSphere = new BoundingSphere(new Vector3(position.X, position.Y, 0), 320);
        }

        //This method updates the source rectangles for the frames.
        public void updateRectangles()
        {
            //If tank is defeated, do nothing.
            if (this.LivesRemaining <= 0)
            {
                return;
            }

            chassis = new Rectangle(width * currentFrame, height * tankColor, width, height);
            cannon = new Rectangle(width * 2, height * tankColor, width, height);
        }

        //This method updates the positions of the tanks bounding spheres.
        public void updateSpheres()
        {
            //if tank is defeated, do nothing.
            if (this.LivesRemaining <= 0)
            {
                return;
            }

            //Update positions of the six bounding spheres based on current position and angle of tank.
            spheres[0] = new BoundingSphere(new Vector3((float)(Math.Cos(chassisAngle) * ((position.X - 20) - position.X) - Math.Sin(chassisAngle) * ((position.Y - 10) - position.Y) + position.X), 
                (float)(Math.Sin(chassisAngle) * ((position.X - 20) - position.X) + Math.Cos(chassisAngle) * ((position.Y - 10) - position.Y) + position.Y), 0), 10);
           
            spheres[1] = new BoundingSphere(new Vector3((float)(Math.Cos(chassisAngle) * ((position.X - 20) - position.X) - Math.Sin(chassisAngle) * ((position.Y + 10) - position.Y) + position.X),
                (float)(Math.Sin(chassisAngle) * ((position.X - 20) - position.X) + Math.Cos(chassisAngle) * ((position.Y + 10) - position.Y) + position.Y), 0), 10);
            
            spheres[2] = new BoundingSphere(new Vector3((float)(Math.Cos(chassisAngle) * ((position.X) - position.X) - Math.Sin(chassisAngle) * ((position.Y - 10) - position.Y) + position.X),
                (float)(Math.Sin(chassisAngle) * ((position.X) - position.X) + Math.Cos(chassisAngle) * ((position.Y - 10) - position.Y) + position.Y), 0), 10);
            
            spheres[3] = new BoundingSphere(new Vector3((float)(Math.Cos(chassisAngle) * ((position.X) - position.X) - Math.Sin(chassisAngle) * ((position.Y + 10) - position.Y) + position.X),
                (float)(Math.Sin(chassisAngle) * ((position.X) - position.X) + Math.Cos(chassisAngle) * ((position.Y + 10) - position.Y) + position.Y), 0), 10);
            
            spheres[4] = new BoundingSphere(new Vector3((float)(Math.Cos(chassisAngle) * ((position.X + 20) - position.X) - Math.Sin(chassisAngle) * ((position.Y - 10) - position.Y) + position.X),
                (float)(Math.Sin(chassisAngle) * ((position.X + 20) - position.X) + Math.Cos(chassisAngle) * ((position.Y - 10) - position.Y) + position.Y), 0), 10);
            
            spheres[5] = new BoundingSphere(new Vector3((float)(Math.Cos(chassisAngle) * ((position.X + 20) - position.X) - Math.Sin(chassisAngle) * ((position.Y + 10) - position.Y) + position.X),
                (float)(Math.Sin(chassisAngle) * ((position.X + 20) - position.X) + Math.Cos(chassisAngle) * ((position.Y + 10) - position.Y) + position.Y), 0), 10);

            //Update position of wall avoidance sphere based on position and angle of tank.
            avoidanceSphere = new BoundingSphere(new Vector3((float)(Math.Cos(chassisAngle) * ((position.X + 30) - position.X) - Math.Sin(chassisAngle) * ((position.Y) - position.Y) + position.X),
                (float)(Math.Sin(chassisAngle) * ((position.X + 30) - position.X) + Math.Cos(chassisAngle) * ((position.Y) - position.Y) + position.Y), 0), 30);

            //Update the position of the detection sphere based on tank's position.
            shootSphere = new BoundingSphere(new Vector3(position.X, position.Y, 0), 320);
        }

        //Draw method, using above data.
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw the components of the tank (chassis and cannon).
            spriteBatch.Draw(Texture, Position, chassis, Color.White, chassisAngle, Origin, 1.0f, SpriteEffects.None, 0);
            spriteBatch.Draw(Texture, Position, cannon, Color.White, cannonAngle, Origin, 1.0f, SpriteEffects.None, 0);

            //Draws bounding sphere locations on tank for debugging.
            if (DrawBSPheres)
            {
                for (int i = 0; i < spheres.Count; i++)
                {
                    Color c = Color.Black;

                    switch (i)
                    {
                        case 0:
                            c = Color.White;
                            break;
                        case 1:
                            c = Color.Red;
                            break;
                        case 2:
                            c = Color.Green;
                            break;
                        case 3:
                            c = Color.Blue;
                            break;
                        case 4:
                            c = Color.Yellow;
                            break;
                        case 5:
                            c = Color.Purple;
                            break;
                    }


                    spriteBatch.Draw(texture2, new Vector2(spheres[i].Center.X, spheres[i].Center.Y), new Rectangle(0, 0, 9, 9), c, 0f, new Vector2(9 / 2, 9 / 2), 1.0f, SpriteEffects.None, 0);

                }
                //Draw the position of the wall avoidance sphere.
                spriteBatch.Draw(texture2, new Vector2(avoidanceSphere.Center.X, avoidanceSphere.Center.Y), new Rectangle(0, 0, 9, 9), Color.Cyan, 0f, new Vector2(9 / 2, 9 / 2), 1.0f, SpriteEffects.None, 0);
            }
        }

        //If the tank is defeated, use this method instead.
        //Only draw the chassis, darker in color to represent inactivity.
        public void DrawDead(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, chassis, Color.Gray, chassisAngle, Origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}
