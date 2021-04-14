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

        public GridPiece(float xPos, float yPos, bool occupied)
        {
            this.XPos = xPos;
            this.YPos = yPos;
            this.Occupied = occupied;
        }
    }
}
