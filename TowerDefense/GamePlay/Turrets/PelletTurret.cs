using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TowerDefense.GamePlay.Sound;
using TowerDefense.Grid;

namespace TowerDefense.GamePlay.Turrets
{
    public class PelletTurret : Turret
    {
        public PelletTurret(Texture2D upgrade1, Texture2D upgrade2, Texture2D upgrade3, Texture2D bulletTexture, Texture2D platformTexture, int xPos, int yPos, IProjectileHandler handler, List<Enemy> enemies)
        {
            this.Textures = new List<Texture2D>() { upgrade1 };
            this._bulletTexture = bulletTexture;



            //Settings
            this.Name = "PELLET";
            this.Price = 600;
            this._shootSpeed = 20;
            this.Upgrade1Price = 1000;
            this.Upgrade2Price = 2500;
            this._shootsAir = true;
            this._shootsGround = true;
            this.Damage = Settings.TowerDefenseSettings.TURRET_DAMAGES[0];
            this.ShootRate = Settings.TowerDefenseSettings.TURRET_FIRE_RATES[3];
            this.Range = Settings.TowerDefenseSettings.TURRET_RANGES[2];

            this.UpgradeLevel = 1;
            this._upgrade1 = upgrade1;
            this._upgrade2 = upgrade2;
            this._upgrade3 = upgrade3;



            this.XPos = xPos;
            this.YPos = yPos;
            this._handler = handler;

            var position = MapGrid.GetPosition(XPos, YPos);
            _animatedSprite = new AnimatedSprite(new Vector2(1, 1), position, position, 0, 200, Textures, new int[] { 1000 });
            this._enemies = enemies;
            this._platformTexture = platformTexture;
        }
        public override void Shoot(Vector2 direction)
        {
            SoundManager.ShootPellet();
            base.Shoot(direction);
        }
        public override Turret Clone()
        {
            return new PelletTurret(_upgrade1, _upgrade2, _upgrade3, this._bulletTexture, this._platformTexture, this.XPos, this.YPos, this._handler, this._enemies);
        }

        public override void Upgrade2()
        {
            this.Damage = Settings.TowerDefenseSettings.TURRET_DAMAGES[1];
            this.ShootRate = Settings.TowerDefenseSettings.TURRET_FIRE_RATES[4];
            this.Range = Settings.TowerDefenseSettings.TURRET_RANGES[4];
            base.Upgrade2();
        }

        public override void Updgrade1()
        {
            this.Damage = Settings.TowerDefenseSettings.TURRET_DAMAGES[0];
            this.ShootRate = Settings.TowerDefenseSettings.TURRET_FIRE_RATES[4];
            this.Range = Settings.TowerDefenseSettings.TURRET_RANGES[2];

            base.Updgrade1();
        }
    }
}
