using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense.Particles
{
    public class BombTrailParticles
    {
        private MyRandom m_random = new MyRandom();

        private Texture2D m_texture;
        private Texture2D m_texture2;
        private int m_speed;
        private TimeSpan m_lifetime;
        private int m_size;

        private int _lifeTimeAddOn = 300;
        public BombTrailParticles(ContentManager content)
        {
            m_size = (int)(4 * Settings.SCALE.X);
            m_speed = (int)(Settings.SCALE.X);
            m_lifetime = new TimeSpan(0, 0,0,0,300);
            m_texture = content.Load<Texture2D>("Sprites/Particles/redThrustParticle");
            m_texture2 = content.Load<Texture2D>("Sprites/Particles/grayThrustParticle");
        }


        public void BombTrail(Vector2 center, GenericParticles particleSystem)
        {
            int amountOfParticles = 10;
            for (int x = 0; x < amountOfParticles; x++)
            {
                Texture2D texture = m_texture;
                if (x % 2 == 0)
                {
                    texture = m_texture2;
                }
                Particle part = new Particle(
                 m_random.Next(),
                 new Vector2(center.X, center.Y),
                 m_random.nextCircleVector(),
                 (float)m_random.nextGaussian(m_speed, Math.Sqrt(m_speed)),
                 m_lifetime + TimeSpan.FromMilliseconds(m_random.Next(0, _lifeTimeAddOn)),
                 texture,
                 m_size);

                particleSystem.AddParticle(part.name, part);
            }

        }
    }
}
