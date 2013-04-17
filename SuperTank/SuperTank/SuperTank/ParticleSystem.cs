using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperTank
{
    public class ParticleSystem
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> particles;
        private List<Texture2D> images;
        private int type;
        private int speed;

        public ParticleSystem(List<Texture2D> images, Vector2 location, int type, int speed)
        {
            EmitterLocation = location;
            this.images = images;
            this.particles = new List<Particle>();
            random = new Random();
            this.type = type;
            this.speed = speed;
        }

        public void Update()
        {
            int total = 5;

            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateNewParticle());
            }

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].timeToLive <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        private Particle GenerateNewParticle()
        {
            Texture2D texture = images[random.Next(images.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(1f * (float)(random.NextDouble() * 2 - 1), 1f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            if (speed == 1)
            {
                angularVelocity = 0;
                velocity = new Vector2(0, 1f * (float)(random.NextDouble() * 2 - 1));
                if (velocity.Y < 0)
                    velocity.Y *= -1.0f;
            }


            Color color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());

            if (type == 1)
                color = new Color((float)random.NextDouble(), 0, 0);
            if (type == 2)
                color = new Color(0, 0, (float)random.NextDouble());
            if (type == 3)
                color = new Color(0, (float)random.NextDouble(), 0);
            if (type == 4)
                color = new Color((float)random.NextDouble(), (float)random.NextDouble(), 0);


            float size = (float)random.NextDouble();
            int timeToLive = 10;

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, timeToLive);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
        }
    }
}