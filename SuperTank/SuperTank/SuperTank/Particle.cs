//The Particle and ParticleSystem classes were based on the following tutorial: http://rbwhitaker.wikidot.com/2d-particle-engine-1

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperTank
{
    public class Particle
    {
        public Texture2D image { get; set; }
        public Vector2 position { get; set; }
        public Vector2 velocity { get; set; }
        public float angle { get; set; }
        public float angularVelocity { get; set; }
        public Color color { get; set; }
        public float size { get; set; }
        public int timeToLive { get; set; }

        //Particle constructor.
        public Particle(Texture2D image, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Color color, float size, int timeToLive)
        {
            this.image = image;
            this.position = position;
            this.velocity = velocity;
            this.angle = angle;
            this.angularVelocity = angularVelocity;
            this.color = color;
            this.size = size;
            this.timeToLive = timeToLive;
        }

        //Updates this particle.
        public void Update()
        {
            timeToLive--;
            position += velocity;
            angle += angularVelocity;
        }

        //Draw particles.
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, image.Width, image.Height);
            Vector2 origin = new Vector2(image.Width / 2, image.Height / 2);

            spriteBatch.Draw(image, position, sourceRectangle, color,
                angle, origin, size, SpriteEffects.None, 0f);
        }
    }
}