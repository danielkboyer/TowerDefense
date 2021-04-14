using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense
{
    public class GameSettings
    {
        //This is what the game will be scaled to
        //any changes to WINDOW_HEIGHT or WINDOW_WIDTH will scale with this value
        public Vector2 BASE_GAME_WIDTH_HEIGHT = new Vector2(540, 540);

        public readonly int WINDOW_HEIGHT = 800;
        public readonly int WINDOW_WIDTH = 800;



    }
}
