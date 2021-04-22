using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense.Particles
{
    public static class ParticleSystem {
        private static ExplosionParticles _explosionParticles;
        private static CreepDeathParticles _creepDeathParticleSystem;
        private static BombTrailParticles _bombTrailParticles;
        private static MissleTrailParticles _missleTrailParticles;
        private static TurretParticles _turretParticles;

        private static GenericParticles _genericParticles;
        public static void LoadContent(ContentManager content)
        {
            _explosionParticles = new ExplosionParticles(content);

            _creepDeathParticleSystem = new CreepDeathParticles(content);
            _bombTrailParticles = new BombTrailParticles(content);
            _genericParticles = new GenericParticles();
            _missleTrailParticles = new MissleTrailParticles(content);
            _turretParticles = new TurretParticles(content);
        }
        public static void TurretSold(Vector2 center)
        {
            _turretParticles.TurretSold(center, _genericParticles);
        }
        public static void MissleTrail(Vector2 center)
        {
            _missleTrailParticles.MissleTrail(center, _genericParticles);
        }
        public static void BombTrail(Vector2 center)
        {
            _bombTrailParticles.BombTrail(center, _genericParticles);
        }
        public static void CreepDeath(Vector2 center)
        {
            _creepDeathParticleSystem.CreepDeath(center, _genericParticles);
        }

        public static void BombExplosion(Vector2 center,int range)
        {
            _explosionParticles.BombExplosion(center,range, _genericParticles);
        }
        public static void MissleExplosion(Vector2 center, float rotation)
        {
            _explosionParticles.MissleExplosion(center, rotation, _genericParticles);
        }
        public static void Render(SpriteBatch spriteBatch, TimeSpan elapsedTime)
        {
            _genericParticles.Draw(spriteBatch);
        }
        public static void Update(TimeSpan elapsedTime)
        {
            //
            // For any existing particles, update them, if we find ones that have expired, add them
            // to the remove list.
            List<int> removeMe = new List<int>();
            foreach (Particle p in _genericParticles.m_particles.Values)
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
                    //p.direction += new Vector2(0, Settings.CoyoteSettings.GRAVITY);
                }
            }

            //
            // Remove any expired particles
            foreach (int Key in removeMe)
            {
                _genericParticles.m_particles.Remove(Key);
            }
        }
    }
}
