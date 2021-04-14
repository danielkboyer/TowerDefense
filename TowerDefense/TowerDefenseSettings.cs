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
        public float GRID_X_LENGTH = 20;
        public float GRID_Y_LENGTH = 20;

        //the number of grids 
        public readonly int LENGTH_OF_GRID = 40;

        //the amount before we start the grid
        public int WallSkipAmount = 2;

        //must be even if length_of_grid is even.
        //Has to be atleast 2
        //the size of the openings in the walls.
        public int WallOpeningAmount = 4;

        public Vector2 WALL_VERT_SIZE = new Vector2(2, 96);

        public Vector2 WALL_HORIZ_SIZE = new Vector2(96, 2);

        #endregion


        #region CannonSettings

        #endregion
    }
}