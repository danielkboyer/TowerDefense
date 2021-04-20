using Microsoft.Xna.Framework.Graphics;
using System;

namespace TowerDefense.GamePlay
{
    public interface IProjectileHandler
    {
        public void AddProjectile(Projectile projectile);

        public void Update(TimeSpan elapsedTime);

        public void Draw(SpriteBatch graphics, TimeSpan elapsedTime);
    }
}