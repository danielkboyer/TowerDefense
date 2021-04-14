using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace LunarLanding.Particles
{
    public class ThrustParticles : GenericParticles
    {

        private MyRandom m_random = new MyRandom();

        private int m_speed;
        private TimeSpan m_lifetime;
        private int m_size;
        public ThrustParticles(ContentManager content, int speed, TimeSpan lifetime, int size)
        {
            m_size = size;
            m_speed = speed;
            m_lifetime = lifetime;
            m_texture = content.Load<Texture2D>("thrustParticle");
        }


        public void ShipThrust(Vector2 shipPosition, float rotation, float gasLeft)
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

                if (!m_particles.ContainsKey(part.name))
                {
                    m_particles.Add(part.name, part);
                }
            }

        }
        public override void Update(TimeSpan elapsedTime)
        {

         


            //
            // For any existing particles, update them, if we find ones that have expired, add them
            // to the remove list.
            List<int> removeMe = new List<int>();
            foreach (Particle p in m_particles.Values)
            {
                p.lifetime -= elapsedTime;
                if (p.lifetime < TimeSpan.Zero)
                {
                    //
                    // Add to the remove list
                    removeMe.Add(p.name);
                }
                else
                {
                    //
                    // Only if we have enough elapsed time, and then move/rotate things
                    // based upon elapsed time, not just the fact that we have received an update.
                    if (elapsedTime.Milliseconds > 0)
                    {
                        //
                        // Update its position
                        p.position += (p.direction * (p.speed * (elapsedTime.Milliseconds / 1000.0f)));

                        //
                        // Have it rotate proportional to its speed
                        p.rotation += (p.speed * (elapsedTime.Milliseconds / 100000.0f));
                    }


                    //
                    // Apply some gravity
                    //p.direction += new Vector2(0,Settings.LunarSettings.GRAVITY);
                }
            }

            //
            // Remove any expired particles
            foreach (int Key in removeMe)
            {
                m_particles.Remove(Key);
            }
        }
    }
}
