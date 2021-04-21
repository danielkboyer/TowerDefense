using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TowerDefense.Grid;
using static TowerDefense.Grid.ShortestPath;

namespace TowerDefense.GamePlay.Creeps
{
    public class TankCreep : Enemy
    {


        public TankCreep(List<Texture2D> textures, Texture2D greenHealthBar, Texture2D redHealthBar,  List<GridPos> gridPositions)
        {
            //SETTINGS
            this.Health = 800;
            this.Speed = 2000;
            this.AwardAmount = 500;

            this._textures = textures;
            this.gridPositions = gridPositions;
            this.CurrentHealth = Health;
            this._greenHealthBar = greenHealthBar;
            this._redHealthBar = redHealthBar;
            this.Height = textures[0].Height * Settings.TowerDefenseSettings.CREEP_SCALE.Y * Settings.SCALE.Y;
            this.Width = textures[0].Width * Settings.TowerDefenseSettings.CREEP_SCALE.X * Settings.SCALE.X;
            
        }

        public void Init()
        {
            int xDirection = gridPositions[gridIndex + 1].XPos - gridPositions[gridIndex].XPos;
            int yDirection = gridPositions[gridIndex + 1].YPos - gridPositions[gridIndex].YPos;

            float rotation = 0;

            if (xDirection == -1)
            {
                rotation = MathHelper.ToRadians(180);
            }
            else if (xDirection == 1)
            {
                rotation = MathHelper.ToRadians(0);
            }
            else if (yDirection == -1)
            {
                rotation = MathHelper.ToRadians(270);
            }
            else if (yDirection == 1)
            {
                rotation = MathHelper.ToRadians(90);
            }





            _animatedSprite = new AnimatedSprite(Settings.TowerDefenseSettings.CREEP_SCALE, MapGrid.GetPosition(gridPositions[gridIndex].XPos, gridPositions[gridIndex++].YPos), MapGrid.GetPosition(gridPositions[gridIndex].XPos, gridPositions[gridIndex].YPos), rotation, Speed, _textures, new int[] { 200, 1000, 200, 600 });
        }

        public override Enemy Copy(List<GridPos> shortestPath)
        {
            var greenCreep = new TankCreep(_textures, this._greenHealthBar, this._redHealthBar,  shortestPath);
            greenCreep.Init();
            return greenCreep;
        }
    }
}
