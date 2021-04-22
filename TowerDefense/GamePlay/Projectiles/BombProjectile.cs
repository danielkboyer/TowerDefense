using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TowerDefense.GamePlay.Sound;
using TowerDefense.Particles;

namespace TowerDefense.GamePlay.Projectiles
{
    public class BombProjectile : Projectile
    {
        private int _radius;
        public BombProjectile(Texture2D texture, Vector2 position, Vector2 direction, float speed, int Damage, bool hitsAir, bool hitsGround, int radius) : base(texture,position,direction,speed,Damage,hitsAir,hitsGround)
        {
            this._radius = radius;
        }

        public override bool CollideWithEnemies(List<Enemy> _enemies)
        {
            foreach (var enemy in _enemies)
            {
                if (enemy.Alive)
                {
                    if (Collides(enemy) && CanHitEnemy(enemy))
                    {
                        //TODO: add particle system for explosion
                        Explode(enemy.Position, _enemies);
                        Alive = false;
                        SoundManager.BombExplosion();
                        return true;
                    }
                }
            }

            return false;
        }

        public override void ParticleEffect()
        {
            ParticleSystem.BombTrail(this.Position);
        }
        public void Explode(Vector2 position,List<Enemy> _enemies)
        {
            float startX = position.X - _radius;
            float endX = position.X + _radius;
            float startY = position.Y - _radius;
            float endY = position.Y + _radius;
            foreach(var enemy in _enemies)
            {
                if(enemy.Position.X >= startX && enemy.Position.X <= endX && enemy.Position.Y >= startY && enemy.Position.Y <= endY)
                {
                    
                    enemy.TakeDamage(this.Damage);
                }
            }

            ParticleSystem.BombExplosion(position,this._radius);
        }

    }
}
