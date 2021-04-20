﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense
{
    public class TowerDefenseSettings
    {
        #region GridSettings
        //The size of the x and y length
        public float GRID_X_LENGTH = 50;
        public float GRID_Y_LENGTH = 50;

        //the number of grids 
        public readonly int LENGTH_OF_GRID = 16;

        //the amount before we start the grid
        public int WallSkipAmount = 1;

        //must be even if length_of_grid is even.
        //Has to be atleast 2
        //the size of the openings in the walls.
        public int WallOpeningAmount = 4;


        public Vector2 TILE_SCALE = new Vector2(1, 1);

        

        #endregion


        #region CannonSettings

        public readonly Vector2 CREEP_SCALE = new Vector2(1f, 1f);

        public Vector2 PLATFORM_SCALE = new Vector2(1.5f, 1.5f);
        #endregion
    }
}
