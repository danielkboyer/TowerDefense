using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TowerDefense.GamePlay.Sound;
using TowerDefense.Grid;
using TowerDefense.Particles;
using static TowerDefense.GamePlay.Sound.SoundManager;

namespace TowerDefense.GamePlay.Projectiles
{
    public class MissleProjectile : Projectile
    {
        SoundPackage _currentPackage = new SoundPackage(false, null);
        public MissleProjectile(Texture2D texture, Vector2 position, Vector2 direction, float speed, int Damage, bool hitsAir, bool hitsGround) : base(texture, position, direction, speed, Damage, hitsAir, hitsGround)
        {
        }
        public override void ParticleEffect()
        {
            ParticleSystem.MissleTrail(this.Position);

            if (_currentPackage.Play == false && (_currentPackage.Sound == null || _currentPackage.Sound.State == Microsoft.Xna.Framework.Audio.SoundState.Stopped))
            {
                _currentPackage = SoundManager.Missle();
            }
        }

        public override bool CollideWithEnemies(List<Enemy> _enemies)
        {
            if (base.CollideWithEnemies(_enemies))
            {
                ParticleSystem.MissleExplosion(this.Position, (float)(this._rotation - Math.PI / 2));
            }
            return false;
        }
        public override void Update(TimeSpan elapsedTime, List<Enemy> _enemies)
        {
            Vector2 direction = new Vector2(float.MaxValue, float.MaxValue);
            var missleCoordinates = this.Position;
            bool changeDirection = false;
            foreach (var enemy in _enemies)
            {
                if (!enemy.Alive || !enemy.CanFly)
                {
                    continue;
                }
                var enemyCoords = MapGrid.GetXYFromCoordinates(enemy.Position.X, enemy.Position.Y);
                var vector = enemy.Position - missleCoordinates;
                if (vector.Length() < direction.Length())
                {
                    direction = vector;
                    changeDirection = true;
                }

            }
            if (changeDirection)
            {
                direction.Normalize();

                this.direction = direction;
                this._rotation = (float)(Math.Atan2(direction.Y, direction.X) + Math.PI / 2);
            }
            base.Update(elapsedTime, _enemies);
        }
    }
}
