using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense.GamePlay.Projectiles
{
    public interface IProjectile
    {
        bool Alive { get; set; }
        void Update(TimeSpan elapsedTime, List<Enemy> _enemies);
        bool Collides(Enemy enemy);
        void Draw(SpriteBatch graphics);
    }
}
