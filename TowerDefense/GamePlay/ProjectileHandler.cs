using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense.GamePlay
{
    public class ProjectileHandler : IProjectileHandler
    {
        private List<Projectile> _projectiles;
        private List<Enemy> _enemies;
        public ProjectileHandler(List<Enemy> enemies)
        {
            this._enemies = enemies;
            _projectiles = new List<Projectile>();
        }
        public void AddProjectile(Projectile projectile)
        {
            _projectiles.Add(projectile);
        }

        public void Update(TimeSpan elapsed)
        {
            foreach(var projectile in _projectiles)
            {
                projectile.Update(elapsed,_enemies);
            }
            _projectiles.RemoveAll(t => !t.Alive);
        }

        public void Draw(SpriteBatch graphics,TimeSpan elapsedTime)
        {
            foreach (var projectile in _projectiles)
            {
                projectile.Draw(graphics);
            }
        }
    }
}
