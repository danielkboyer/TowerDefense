using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TowerDefense.Particles
{
    public class ThrustParticles
    {

        private MyRandom m_random = new MyRandom();

        private int m_speed;
        private TimeSpan m_lifetime;
        private int m_size;
        private Texture2D m_texture;
        public ThrustParticles(ContentManager content, int speed, TimeSpan lifetime, int size)
        {
            m_size = size;
            m_speed = speed;
            m_lifetime = lifetime;
            m_texture = content.Load<Texture2D>("thrustParticle");
        }


        public void ShipThrust(Vector2 shipPosition, float rotation, float gasLeft, GenericParticles particleSystem)
        {
            int amountOfParticles = (int)(2 * gasLeft);
            for (int x = 0; x < amountOfParticles; x++)
            {
                Particle part = new Particle(
                 m_random.Next(),
                 new Vector2(shipPosition.X, shipPosition.Y),
                 m_random.nextHalfCircle(rotation),
                 (float)m_random.nextGaussian(m_speed, Math.Sqrt(m_speed)),
                 m_lifetime,
                 m_texture,
                 m_size);

                particleSystem.AddParticle(part.name, part);
            }

        }
       
    }
}
