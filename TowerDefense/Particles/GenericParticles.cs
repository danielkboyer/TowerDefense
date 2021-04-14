using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace LunarLanding.Particles
{
    public abstract class GenericParticles
    {
        protected Dictionary<int, Particle> m_particles = new Dictionary<int, Particle>();

        protected Texture2D m_texture;
        public abstract void Update(TimeSpan elapsedTime);
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
    }
}
