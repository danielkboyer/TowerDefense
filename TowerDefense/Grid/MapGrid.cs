using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense.Grid
{
    public class MapGrid
    {
        private GridPiece[,] _grid;

        private Texture2D _wallVert;
        private Texture2D _wallHoriz;

        


        public MapGrid()
        {
            _grid = new GridPiece[Settings.TowerDefenseSettings.LENGTH_OF_GRID, Settings.TowerDefenseSettings.LENGTH_OF_GRID];
            
        }

        /// <summary>
        /// gives the position of a grid piece
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Vector2 GetPosition(int x, int y)
        {
            return new Vector2((Settings.TowerDefenseSettings.GRID_X_LENGTH * Settings.SCALE.X * x) + Settings.TowerDefenseSettings.GRID_X_LENGTH * Settings.SCALE.X / 2, (y * Settings.TowerDefenseSettings.GRID_Y_LENGTH * Settings.SCALE.Y) + Settings.TowerDefenseSettings.GRID_Y_LENGTH * Settings.SCALE.Y / 2);
        }
        /// <summary>
        /// gives the grid piece associated with coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static (int x, int y) GetXYFromCoordinates(float x, float y)
        {
            return ((int)(x / Settings.TowerDefenseSettings.GRID_X_LENGTH * Settings.SCALE.X), (int)(y / Settings.TowerDefenseSettings.GRID_Y_LENGTH * Settings.SCALE.Y));
        }
        
        public void Generate()
        {
            float xPos = 0;
            float yPos = 0;
            for (int x = 0; x < Settings.TowerDefenseSettings.LENGTH_OF_GRID; x++)
            {
                for(int y = 0; y < Settings.TowerDefenseSettings.LENGTH_OF_GRID; y++)
                {
                    int lower = Settings.TowerDefenseSettings.LENGTH_OF_GRID / 2 - 1 -((Settings.TowerDefenseSettings.WallOpeningAmount-2)/2);
                    int upper = Settings.TowerDefenseSettings.LENGTH_OF_GRID / 2 + ((Settings.TowerDefenseSettings.WallOpeningAmount - 2) / 2);
                    bool blockedByWall = x < Settings.TowerDefenseSettings.WallSkipAmount || x > Settings.TowerDefenseSettings.LENGTH_OF_GRID - Settings.TowerDefenseSettings.WallSkipAmount - 1 || y < Settings.TowerDefenseSettings.WallSkipAmount || y > Settings.TowerDefenseSettings.LENGTH_OF_GRID - Settings.TowerDefenseSettings.WallSkipAmount - 1;
                    if(x < Settings.TowerDefenseSettings.WallSkipAmount || x > Settings.TowerDefenseSettings.LENGTH_OF_GRID - Settings.TowerDefenseSettings.WallSkipAmount - 1)
                    {
                        if(y >= lower && y <= upper)
                        {
                            blockedByWall = false;
                        }
                    }

                    if (y < Settings.TowerDefenseSettings.WallSkipAmount || y > Settings.TowerDefenseSettings.LENGTH_OF_GRID - Settings.TowerDefenseSettings.WallSkipAmount - 1)
                    {
                        if (x >= lower && x <= upper)
                        {
                            blockedByWall = false;
                        }
                    }
                    _grid[x, y] = new GridPiece(xPos, yPos, blockedByWall);
                    yPos += Settings.TowerDefenseSettings.GRID_Y_LENGTH* Settings.SCALE.Y;
                }
                xPos += Settings.TowerDefenseSettings.GRID_X_LENGTH * Settings.SCALE.X;
                yPos = 0;
            }
        }

        public void LoadContent(ContentManager content)
        {

            this._wallHoriz = content.Load<Texture2D>("Sprites/WallHorizBlack");
            this._wallVert = content.Load<Texture2D>("Sprites/WallVertBlack");
        }

        public bool IsPieceOccupied(int x, int y)
        {
            return _grid[x, y].Occupied;
        }

        public void OccupyPiece(int x, int y, bool state)
        {
            _grid[x, y].Occupied = state;
        }
        public bool CoordinatesInMap(int x, int y)
        {
            return x >= 0 && x < Settings.TowerDefenseSettings.LENGTH_OF_GRID && y >= 0 && y < Settings.TowerDefenseSettings.LENGTH_OF_GRID;
        }

        public void Render(SpriteBatch graphics)
        {

            var scale = Settings.TowerDefenseSettings.GRID_X_LENGTH * Settings.SCALE.X / Settings.TowerDefenseSettings.WALL_HORIZ_SIZE.X;
            for (int x = 0; x < Settings.TowerDefenseSettings.LENGTH_OF_GRID; x++)
            {
                if(x < Settings.TowerDefenseSettings.WallSkipAmount)
                {
                    continue;
                }

               
                if (x > Settings.TowerDefenseSettings.LENGTH_OF_GRID - Settings.TowerDefenseSettings.WallSkipAmount - 1)
                {
                    
                    continue;
                }
                for (int y = 0; y < Settings.TowerDefenseSettings.LENGTH_OF_GRID; y++)
                {
                    if (y < Settings.TowerDefenseSettings.WallSkipAmount)
                    {
                        continue;
                    }
                    if (y == Settings.TowerDefenseSettings.LENGTH_OF_GRID - Settings.TowerDefenseSettings.WallSkipAmount - 1)
                    {
                        //we have to switch x and y or it doesn't make sense.
                        graphics.Draw(_wallVert, new Vector2(_grid[x, y].YPos + Settings.TowerDefenseSettings.GRID_X_LENGTH, _grid[x, y].XPos ), null, Color.White, 0, new Vector2(0, 0), new Vector2(1, scale), SpriteEffects.None, 0);

                    }
                    if (y > Settings.TowerDefenseSettings.LENGTH_OF_GRID - Settings.TowerDefenseSettings.WallSkipAmount - 1)
                    {
                        
                        continue;
                    }
                    graphics.Draw(_wallHoriz, new Vector2(_grid[x, y].XPos, _grid[x, y].YPos), null, Color.White, 0, new Vector2(0, 0), new Vector2(scale, 1), SpriteEffects.None, 0);
                    graphics.Draw(_wallVert, new Vector2(_grid[x, y].XPos, _grid[x, y].YPos), null, Color.White, 0, new Vector2(0, 0), new Vector2(1, scale), SpriteEffects.None, 0);
                  
                }

                //draw the bottom part
                graphics.Draw(_wallHoriz, new Vector2(_grid[x, Settings.TowerDefenseSettings.LENGTH_OF_GRID - Settings.TowerDefenseSettings.WallSkipAmount - 1].XPos, _grid[x, Settings.TowerDefenseSettings.LENGTH_OF_GRID - Settings.TowerDefenseSettings.WallSkipAmount - 1].YPos + Settings.TowerDefenseSettings.GRID_Y_LENGTH), null, Color.White, 0, new Vector2(0, 0), new Vector2(scale, 1), SpriteEffects.None, 0);


            }
            //if even this is plus one the middle 
            int middle = Settings.TowerDefenseSettings.LENGTH_OF_GRID / 2;
            //Do this however many times the skip amount is 
            for (int x = 0;x<Settings.TowerDefenseSettings.WallSkipAmount; x++)
            {
                
            }
        }

    }
}
