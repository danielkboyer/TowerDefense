using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TowerDefense.Grid;
using static TowerDefense.Grid.ShortestPath;

namespace TowerDefense.GamePlay.Creeps
{
    public class FlyingCreep : Enemy
    {


        public FlyingCreep(List<Texture2D> textures, Texture2D greenHealthBar, Texture2D redHealthBar, List<GridPos> gridPositions, int healthAddon = 0, int speedAddOn = 0)
        {
            this._textures = textures;
            this.Health = 200 + healthAddon;
            this.Speed = 1000 + speedAddOn;
            this.gridPositions = CalculateFlyPath(gridPositions);

            this.CurrentHealth = Health;
            this._greenHealthBar = greenHealthBar;
            this._redHealthBar = redHealthBar;
            this.Height = textures[0].Height * Settings.TowerDefenseSettings.CREEP_SCALE.Y * Settings.SCALE.Y;
            this.Width = textures[0].Width * Settings.TowerDefenseSettings.CREEP_SCALE.X * Settings.SCALE.X;
            this.AwardAmount = 100;
            this.CanFly = true;
        }
        public List<GridPos> CalculateFlyPath(List<GridPos> gridPositions)
        {
            if (gridPositions == null)
                return null;
            GridPos start = new GridPos(gridPositions[0]);
            GridPos end = new GridPos(gridPositions[gridPositions.Count - 1]);

            List<GridPos> newGridPosition = new List<GridPos>();
            newGridPosition.Add(gridPositions[0]);

            while(start.XPos != end.XPos || end.YPos != start.YPos)
            {
                int xDiff = end.XPos - start.XPos;
                int yDiff = end.YPos - start.YPos;
                if (xDiff < 0)
                {
                    
                    start.XPos--;
                }
                else if(xDiff > 0)
                {
                    start.XPos++;
                }
                else if(yDiff < 0)
                {
                    start.YPos--;
                }
                else if (yDiff > 0)
                {
                    start.YPos++;
                }

                newGridPosition.Add(new GridPos(start.XPos, start.YPos, false, null));
            }

            return newGridPosition;

            

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





            _animatedSprite = new AnimatedSprite(Settings.TowerDefenseSettings.CREEP_SCALE, MapGrid.GetPosition(gridPositions[gridIndex].XPos, gridPositions[gridIndex++].YPos), MapGrid.GetPosition(gridPositions[gridIndex].XPos, gridPositions[gridIndex].YPos), rotation, Speed, _textures, new int[] { 1000});
        }

        public override Enemy Copy(List<GridPos> shortestPath)
        {
            var greenCreep = new FlyingCreep(_textures, this._greenHealthBar, this._redHealthBar, shortestPath);
            greenCreep.Health = this.Health;
            greenCreep.Speed = this.Speed;
            greenCreep.CurrentHealth = this.Health;
            greenCreep.Init();
            return greenCreep;
        }
    }
}
