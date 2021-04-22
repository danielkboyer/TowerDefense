using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense.Particles
{
    public class Particle
    {
        public Particle(int name, Vector2 position, Vector2 direction, float speed, TimeSpan lifetime, Texture2D texture, int particleSize, int particleGroup = 0)
        {
            this.name = name;
            this.position = position;
            this.direction = direction;
            this.speed = speed;
            this.lifetime = lifetime;
            this.texture = texture;

            this.particleSize = particleSize;
            this.rotation = 0;
            this.particleGroup = particleGroup;
        }

        public int particleGroup;
        public int particleSize;
        public int name;
        public Vector2 position;
        public float rotation;
        public Vector2 direction;
        public float speed;
        public TimeSpan lifetime;
        public Texture2D texture;

    }
}
