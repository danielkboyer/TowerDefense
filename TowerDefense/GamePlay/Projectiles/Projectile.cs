using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TowerDefense.GamePlay.Projectiles;
using TowerDefense.Grid;

namespace TowerDefense.GamePlay
{
    public class Projectile:IProjectile
    {
        public Vector2 direction { get; set; }
        public int Damage { get; set; }

        public Vector2 Position { get; set; }

        public float Speed { get; set; }

        public TimeSpan CurrentRate { get; set; }

        private float _bulletUpdateRate { get; set; } = 30;

        public bool Alive { get; set; }

        private Texture2D _texture;

        private bool _hitsAir;
        private bool _hitsGround;
        protected float _rotation;
        public Projectile(Texture2D texture,Vector2 position,Vector2 direction, float speed, int Damage, bool hitsAir, bool hitsGround)
        {
            this.direction = direction;
            this.Position = position;
            this.Damage = Damage;
            this.Speed = speed;
            Alive = true;
            this._texture = texture;
            this._hitsAir = hitsAir;
            this._hitsGround = hitsGround;
            this._rotation = (float)(Math.Atan2(direction.Y, direction.X) + Math.PI / 2);
        }

        private bool OutOfBounds()
        {
            var coordinates = MapGrid.GetXYFromCoordinates(this.Position.X, this.Position.Y);
            if (MapGrid.IsOnGrid(coordinates.x, coordinates.y))
            {
                return false;
            }

            return true;
        }
        public virtual void Update(TimeSpan elapsedTime,List<Enemy> _enemies)
        {
            
            if (Alive)
            {
                if(OutOfBounds())
                {
                    Alive = false;
                    return;
                }
                CurrentRate += elapsedTime;
                if (CurrentRate.TotalMilliseconds >= _bulletUpdateRate)
                {
                    CurrentRate -= TimeSpan.FromMilliseconds(_bulletUpdateRate);
                    Position += direction *Speed;
                    ParticleEffect();
                }

                CollideWithEnemies(_enemies);

                
            }
        }

        public virtual void ParticleEffect()
        {
            return;
        }
        public virtual bool CollideWithEnemies(List<Enemy> _enemies)
        {
            foreach (var enemy in _enemies)
            {
                if (enemy.Alive && CanHitEnemy(enemy))
                {
                    if (Collides(enemy))
                    {
                        enemy.TakeDamage(Damage);
                        Alive = false;
                        return true;
                    }
                }
            }

            return false;
        }
        public bool CanHitEnemy(Enemy enemy)
        {
            if (enemy.CanFly)
            {
                return _hitsAir;
            }

            return _hitsGround;
        }
        public virtual bool Collides(Enemy enemy)
        {
            return Position.X >= enemy.Position.X - enemy.Width / 2 && Position.X <= enemy.Position.X + enemy.Width / 2 && Position.Y >= enemy.Position.Y - enemy.Height / 2 && Position.Y <= enemy.Position.Y + enemy.Height / 2;
      
        }

        public void Draw(SpriteBatch graphics)
        {
            graphics.Draw(_texture, Position, null, Color.White, _rotation, new Vector2(_texture.Width / 2, _texture.Height / 2),  Settings.SCALE, SpriteEffects.None, 0);


        }
    }
}
