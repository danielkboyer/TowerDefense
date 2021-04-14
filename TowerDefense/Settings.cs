using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense
{
    public class Settings
    {
        public static GameSettings GameSettings = new GameSettings();

        public static TowerDefenseSettings TowerDefenseSettings = new TowerDefenseSettings();

        public static Vector2 SCALE = new Vector2(GameSettings.WINDOW_WIDTH / GameSettings.BASE_GAME_WIDTH_HEIGHT.X, GameSettings.WINDOW_HEIGHT / GameSettings.BASE_GAME_WIDTH_HEIGHT.Y);
    }
}
