using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TowerDefense.Grid;

namespace TowerDefense.GamePlay
{
    public class BasicTurret:Turret
    {
        public BasicTurret(Texture2D texture,Texture2D bulletTexture, Texture2D platformTexture, int damage, int price,float shootSpeed, float shootRate, int range, int xPos, int yPos, IProjectileHandler handler, List<Enemy> enemies)
        {
            this._textures = new List<Texture2D>() { texture };
            this._bulletTexture = bulletTexture;
            this.Damage = damage;
            this.Price = price;
            this._shootSpeed = shootSpeed;
            this.shootRate = shootRate;
            this.Range = range;
            
            this.XPos = xPos;
            this.YPos = yPos;
            this._handler = handler;
            var position = MapGrid.GetPosition(XPos, YPos);
            _animatedSprite = new AnimatedSprite(new Vector2(1, 1), position, position, 0, 200, _textures, new int[] { 1000 });
            this._enemies = enemies;
            this._platformTexture = platformTexture;
        }

       
    }
}
