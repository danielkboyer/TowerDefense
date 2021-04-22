using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense.Particles
{
    public class TurretParticles
    {
        private MyRandom m_random = new MyRandom();

        private Texture2D m_texture;
        private int m_speed;
        private TimeSpan m_lifetime;
        private int m_size;

        private int _lifeTimeAddOn = 3000;
        public TurretParticles(ContentManager content)
        {
            m_size = (int)(8 * Settings.SCALE.X);
            m_speed = 0;
            m_lifetime = new TimeSpan(0, 0, 1);
            m_texture = content.Load<Texture2D>("Sprites/Particles/greenThrustParticle");
        }


        public void TurretSold(Vector2 center, GenericParticles particleSystem)
        {
            int amountOfParticles = 23;
            int addOn = 1;

            int xPos = 0;
            int yPos = 0;
            
            for(int x = 0; x < amountOfParticles; x++)
            {
                for(int y = 0;y< amountOfParticles; y++)
                {
                    Particle part = new Particle(
                     m_random.Next(),
                     new Vector2(center.X+xPos, center.Y+yPos),
                     m_random.nextCircleVector(),
                     (float)m_random.nextGaussian(m_speed, Math.Sqrt(m_speed)),
                     m_lifetime + TimeSpan.FromMilliseconds(m_random.Next(0, _lifeTimeAddOn)),
                     m_texture,
                     m_size);

                    particleSystem.AddParticle(part.name, part);

                    yPos+= addOn;
                }
                xPos+= addOn;
                yPos = 0;
            }

        }
    }
}
