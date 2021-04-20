using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense.GamePlay
{
    public class Projectile
    {
        public Vector2 direction { get; set; }
        public int Damage { get; set; }

        public Vector2 Position { get; set; }

        public float Speed { get; set; }

        public TimeSpan CurrentRate { get; set; }

        private float _bulletUpdateRate { get; set; } = 30;

        public bool Alive { get; set; }

        private Texture2D _texture;
        public Projectile(Texture2D texture,Vector2 position,Vector2 direction, float speed, int Damage)
        {
            this.direction = direction;
            this.Position = position;
            this.Damage = Damage;
            this.Speed = speed;
            Alive = true;
            this._texture = texture;
        }

        public void Update(TimeSpan elapsedTime,List<Enemy> _enemies)
        {
            if (Alive)
            {
                CurrentRate += elapsedTime;
                if (CurrentRate.TotalMilliseconds >= _bulletUpdateRate)
                {
                    CurrentRate -= TimeSpan.FromMilliseconds(_bulletUpdateRate);
                    Position += direction *Speed;
                }

                foreach (var enemy in _enemies)
                {
                    if (enemy.Alive)
                    {
                        if (Collides(enemy))
                        {
                            enemy.TakeDamage(Damage);
                            Alive = false;
                            break;
                        }
                    }
                }
            }
        }

        public bool Collides(Enemy enemy)
        {
            return Position.X >= enemy.Position.X - enemy.Width / 2 && Position.X <= enemy.Position.X + enemy.Width / 2 && Position.Y >= enemy.Position.Y - enemy.Height / 2 && Position.Y <= enemy.Position.Y + enemy.Height / 2;
      
        }

        public void Draw(SpriteBatch graphics)
        {
            graphics.Draw(_texture, Position, null, Color.White, 0, new Vector2(_texture.Width / 2, _texture.Height / 2),  Settings.SCALE, SpriteEffects.None, 0);


        }
    }
}
