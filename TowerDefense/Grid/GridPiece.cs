using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense.Grid
{
    public class GridPiece
    {
        public float XPos { get; set; }
        public float YPos { get; set; }
        public bool Occupied { get; set; }

        public Color Color { get; set; }
        

        public GridPiece(float xPos, float yPos, bool occupied, Color color)
        {
            this.XPos = xPos;
            this.YPos = yPos;
            this.Occupied = occupied;
            this.Color = color;
        }

        public GridPiece(GridPiece gridPiece)
        {
            this.XPos = gridPiece.XPos;
            this.YPos = gridPiece.YPos;
            this.Occupied = gridPiece.Occupied;
        }
    }
}
