using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarLanding.Particles
{
    public class ShipDestroyedParticles : GenericParticles
    {

        private MyRandom m_random = new MyRandom();

        private int m_start_lifetime;
        private int m_collapse_lifetime;
        private int m_burst_lifetime;
        private int m_size;

        private int m_start_speed;
        private int m_collapse_speed;
        private int m_burst_speed;

        private Texture2D redTexture;
        private Texture2D orangeTexture;
        private Texture2D yellowTexture;

        private Dictionary<int,ParticleSequenceTime> _elapsedTimes;

        public ShipDestroyedParticles(ContentManager content, int stasisTime, int inwardTime, int burstTime, int startSpeed, int collapseSpeed, int burstSpeed, int size)
        {
            m_size = size;
            m_texture = content.Load<Texture2D>("thrustParticle");
            redTexture = content.Load<Texture2D>("redThrustParticle");
            yellowTexture = content.Load<Texture2D>("yellowThrustParticle");
            orangeTexture = content.Load<Texture2D>("orangeThrustParticle");
            _elapsedTimes = new Dictionary<int, ParticleSequenceTime>();
            m_start_lifetime = stasisTime;
            m_collapse_lifetime = inwardTime;
            m_burst_lifetime = burstTime;

            this.m_start_speed = startSpeed;
            this.m_collapse_speed = collapseSpeed; ;
            this.m_burst_speed = burstSpeed;
        }


        public void ShipDestory(Vector2 shipPosition)
        {
            int randomGroup;
            do
            {
                randomGroup = m_random.Next();
            } while (_elapsedTimes.ContainsKey(randomGroup));


            _elapsedTimes.Add(randomGroup, new ParticleSequenceTime() {elapsedTime=0,sequenceNumber=0,key=randomGroup,shipPosition =shipPosition });

            for (int x = 0; x < 1000; x++)
            {
                Particle part = new Particle(
                 m_random.Next(),
                 new Vector2(shipPosition.X, shipPosition.Y),
                 m_random.nextCircleVector(),
                 (float)m_random.nextGaussian(m_start_speed, Math.Sqrt(m_start_speed)*1.5),
                 new TimeSpan(0,0,0,0,m_start_lifetime + m_collapse_lifetime + m_burst_lifetime),
                 m_texture,
                 m_size,
                 randomGroup);

                if (!m_particles.ContainsKey(part.name))
                {
                    m_particles.Add(part.name, part);
                }
            }

        }

        private void ParticlesCombine(int particleGroup)
        {
            foreach(Particle part in m_particles.Values.Where(t=>t.particleGroup == particleGroup)){
                part.direction *= -1;
                part.speed = m_collapse_speed;
                part.texture = yellowTexture;
            }
        }

        private void ParticlesBurst(int particleGroup)
        {
            bool everyOther = false;
            foreach (Particle part in m_particles.Values.Where(t => t.particleGroup == particleGroup))
            {
                part.direction *= -1;
                part.speed = m_burst_speed;
                part.particleSize = m_size * 2;
                if (!everyOther)
                {
                    part.texture = redTexture;
                    everyOther = true;
                }
                else
                {
                    part.texture = orangeTexture;
                    everyOther = false;
                }
            }
        }
        public override void Update(TimeSpan elapsedTime)
        {


            List<int> removeTimes = new List<int>();
            foreach(ParticleSequenceTime times in _elapsedTimes.Values)
            {
                times.elapsedTime += (float)elapsedTime.TotalMilliseconds;

                if(times.elapsedTime > m_start_lifetime && times.sequenceNumber == 0)
                {
                    ParticlesCombine(times.key);
                    times.sequenceNumber = 1;
                }
                else if(times.elapsedTime > m_collapse_lifetime + m_start_lifetime && times.sequenceNumber == 1)
                {
                    ParticlesBurst(times.key);
                    times.sequenceNumber = 2;
                }
                else if(times.elapsedTime > m_collapse_lifetime + m_start_lifetime + m_burst_lifetime)
                {
                    removeTimes.Add(times.key);
                }
            }

            foreach (int Key in removeTimes)
            {
                _elapsedTimes.Remove(Key);
            }
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
