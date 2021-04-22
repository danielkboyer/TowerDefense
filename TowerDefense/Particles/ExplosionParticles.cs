using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense.Particles
{
    public class ExplosionParticles
    {

        private MyRandom m_random = new MyRandom();

        private Texture2D m_texture;
        private int m_size;

        public ExplosionParticles(ContentManager content)
        {
            m_size = (int)(5 * Settings.SCALE.X);
            m_texture = content.Load<Texture2D>("Sprites/Particles/redThrustParticle");
        }

        public void MissleExplosion(Vector2 center, float rotation, GenericParticles particleSystem)
        {
            var speed = (int)(100 * Settings.SCALE.X);
            var m_lifetime = new TimeSpan(0, 0, 0, 0, 500);
            int amountOfParticles = 500;
            for (int x = 0; x < amountOfParticles; x++)
            {
                Particle part = new Particle(
                 m_random.Next(),
                 new Vector2(center.X, center.Y),
                 m_random.nextHalfCircle(rotation),
                 (float)m_random.nextGaussian(speed, Math.Sqrt(speed)) + m_random.Next(-50, 0),
                 m_lifetime,
                 m_texture,
                 m_size);

                particleSystem.AddParticle(part.name, part);
            }
        }

        public void BombExplosion(Vector2 center, int range, GenericParticles particleSystem)
        {
            var speed = (int)(200 * Settings.SCALE.X);
            var m_lifetime =  new TimeSpan(0, 0, 0,0, 5 * range); 
            int amountOfParticles = 500;
            for (int x = 0; x < amountOfParticles; x++)
            {
                Particle part = new Particle(
                 m_random.Next(),
                 new Vector2(center.X, center.Y),
                 m_random.nextCircleVector(),
                 (float)m_random.nextGaussian(speed, Math.Sqrt(speed)) + m_random.Next(-100,0),
                 m_lifetime,
                 m_texture,
                 m_size);

                particleSystem.AddParticle(part.name, part);
            }

        }

    }
}
