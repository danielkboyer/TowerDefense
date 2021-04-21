using Microsoft.Xna.Framework.Graphics;
using System;
using TowerDefense.GamePlay.Projectiles;

namespace TowerDefense.GamePlay
{
    public interface IProjectileHandler
    {
        public void AddProjectile(IProjectile projectile);

        public void Update(TimeSpan elapsedTime);

        public void Draw(SpriteBatch graphics, TimeSpan elapsedTime);
    }
}