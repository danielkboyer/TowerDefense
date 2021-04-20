using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TowerDefense.Grid;
using static TowerDefense.Grid.ShortestPath;

namespace TowerDefense.GamePlay
{
    public class GreenCreep : Enemy
    {
        
        
        public GreenCreep(List<Texture2D> textures, int health,int speed,List<GridPos> gridPositions)
        {
            this._textures = textures;
            this.Health = health;
            this.Speed = speed;
            this.gridPositions = gridPositions;

           

            this.Height = textures[0].Height * Settings.TowerDefenseSettings.CREEP_SCALE.Y * Settings.SCALE.Y;
            this.Width = textures[0].Width * Settings.TowerDefenseSettings.CREEP_SCALE.X * Settings.SCALE.X;
        }

        public void Init()
        {
            _animatedSprite = new AnimatedSprite(Settings.TowerDefenseSettings.CREEP_SCALE, MapGrid.GetPosition(gridPositions[gridIndex].XPos, gridPositions[gridIndex++].YPos), MapGrid.GetPosition(gridPositions[gridIndex].XPos, gridPositions[gridIndex].YPos), 0, Speed, _textures, new int[] { 1000, 200, 100, 1000, 100, 200 });
        }

        public override Enemy Copy(List<GridPos> shortestPath)
        {
            var greenCreep = new GreenCreep(_textures, this.Health, this.Speed, shortestPath);
            greenCreep.Init();
            return greenCreep;
        }
    }
}
