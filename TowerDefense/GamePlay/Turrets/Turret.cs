using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TowerDefense.Grid;

namespace TowerDefense.GamePlay
{
    public abstract class Turret
    {
        protected AnimatedSprite _animatedSprite;

        public int Damage { get; set; }

        public string Name { get; set; }
        public int Price { get; set; }


        public int UpgradeLevel;

        public int Upgrade1Price { get; set; }

        public int Upgrade2Price { get; set; }

        public int XPos { get; set; }
        public int YPos { get; set; }

        public float ShootRate;
        private TimeSpan _currentShootTime;

        protected bool _shootsGround;
        protected bool _shootsAir;

        protected float _shootSpeed;

        protected IProjectileHandler _handler;

        protected Texture2D _bulletTexture;
        public List<Texture2D> Textures;

        protected Texture2D _upgrade1;
        protected Texture2D _upgrade2;
        protected Texture2D _upgrade3;

        protected Texture2D _platformTexture;

        public int Range;

        protected List<Enemy> _enemies;
        public void Update(TimeSpan elapsedTime)
        {
            _animatedSprite.update(elapsedTime);

            _currentShootTime += elapsedTime;
            if (_currentShootTime.TotalMilliseconds >= ShootRate)
            {
                _currentShootTime -= TimeSpan.FromMilliseconds(ShootRate);
                var direction = ShouldShoot();
                if (direction.shouldShoot)
                {
                    _animatedSprite.UpdateTargetRotation((float)(Math.Atan2(direction.direction.Y, direction.direction.X) + Math.PI/2));
                    Shoot(direction.direction);
                }
            }
        }

        
        public virtual void Updgrade1()
        {
            this.UpgradeLevel++;
            Textures.Add(_upgrade2);
            Textures.RemoveAt(0);
        }

        public virtual void Upgrade2()
        {
            this.UpgradeLevel++;
            Textures.Add(_upgrade3);
            Textures.RemoveAt(0);

        }
        public virtual void Draw(SpriteBatch graphics, TimeSpan elapsedTime)
        {
            graphics.Draw(_platformTexture, MapGrid.GetPosition(XPos,YPos), null, Color.White, 0, new Vector2(_platformTexture.Width / 2, _platformTexture.Height / 2), Settings.TowerDefenseSettings.PLATFORM_SCALE * Settings.SCALE, SpriteEffects.None, 0);

            _animatedSprite.draw(graphics);
        }

        public virtual (bool shouldShoot,Vector2 direction) ShouldShoot()
        {
            Vector2 toReturn = new Vector2(float.MaxValue, float.MaxValue);
            var turretCoordinates = MapGrid.GetPosition(XPos, YPos);
            bool shouldShoot = false;
            foreach (var enemy in _enemies)
            {
                if (!enemy.Alive || !CanShootEnemy(enemy))
                {
                    continue;
                }
                var enemyCoords = MapGrid.GetXYFromCoordinates(enemy.Position.X, enemy.Position.Y);
                if (Math.Abs(enemyCoords.x - XPos) <= Range && Math.Abs(enemyCoords.y - YPos) <= Range)
                {
                    var vector =  enemy.Position - turretCoordinates;
                    if(vector.Length() < toReturn.Length())
                    {
                        shouldShoot = true;
                        toReturn = vector;
                    }
                }
            }
            toReturn.Normalize();
            return (shouldShoot,toReturn);
        }

        public bool CanShootEnemy(Enemy enemy)
        {
            if (enemy.CanFly)
            {
                return _shootsAir;

            }

            return _shootsGround;
        }

        public virtual void Shoot(Vector2 direction)
        {
            _handler.AddProjectile(new Projectile(_bulletTexture, MapGrid.GetPosition(XPos, YPos), direction, _shootSpeed, Damage,this._shootsAir,this._shootsGround));
        }
       

        /// <summary>
        /// Called when the enemy has died
        /// </summary>
        public virtual void Sell()
        {
        }


        public abstract Turret Clone();


    }
}

