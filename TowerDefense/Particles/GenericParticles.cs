using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense.Particles
{
    public class GenericParticles
    {
        public Dictionary<int, Particle> m_particles = new Dictionary<int, Particle>();
        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (Particle p in m_particles.Values)
            {
                Rectangle r = new Rectangle(0, 0, p.particleSize, p.particleSize);
                r.X = (int)p.position.X;
                r.Y = (int)p.position.Y;
                spriteBatch.Draw(
                    p.texture,
                    r,
                    null,
                    Color.White,
                    p.rotation,
                    new Vector2(p.texture.Width / 2, p.texture.Height / 2),
                    SpriteEffects.None,
                    0);
            }
        }

        public void AddParticle(int key, Particle particle)
        {
            if (!m_particles.ContainsKey(key))
            {
                m_particles.Add(key, particle);
            }
        }
    }
}
