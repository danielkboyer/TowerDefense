using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense.Grid
{
    public class MapGrid
    {
        private GridPiece[,] _grid;

        private Texture2D _wallVert;
        private Texture2D _wallHoriz;




        private Texture2D _tile;
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
            return new Vector2((Settings.TowerDefenseSettings.GRID_X_LENGTH * Settings.SCALE.Y * x) + Settings.TowerDefenseSettings.GRID_X_LENGTH * Settings.SCALE.Y / 2, (y * Settings.TowerDefenseSettings.GRID_Y_LENGTH * Settings.SCALE.Y) + Settings.TowerDefenseSettings.GRID_Y_LENGTH * Settings.SCALE.Y / 2);
        }
        /// <summary>
        /// gives the grid piece associated with coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static (int x, int y) GetXYFromCoordinates(float x, float y)
        {
            return ((int)(x / Settings.TowerDefenseSettings.GRID_X_LENGTH * Settings.SCALE.Y), (int)(y / Settings.TowerDefenseSettings.GRID_Y_LENGTH * Settings.SCALE.Y));
        }

        public void Generate()
        {
            float xPos = 0;
            float yPos = 0;
            for (int x = 0; x < Settings.TowerDefenseSettings.LENGTH_OF_GRID; x++)
            {
                for (int y = 0; y < Settings.TowerDefenseSettings.LENGTH_OF_GRID; y++)
                {
                    int lower = Settings.TowerDefenseSettings.LENGTH_OF_GRID / 2 - 1 - ((Settings.TowerDefenseSettings.WallOpeningAmount - 2) / 2);
                    int upper = Settings.TowerDefenseSettings.LENGTH_OF_GRID / 2 + ((Settings.TowerDefenseSettings.WallOpeningAmount - 2) / 2);
                    bool blockedByWall = x < Settings.TowerDefenseSettings.WallSkipAmount || x > Settings.TowerDefenseSettings.LENGTH_OF_GRID - Settings.TowerDefenseSettings.WallSkipAmount - 1 || y < Settings.TowerDefenseSettings.WallSkipAmount || y > Settings.TowerDefenseSettings.LENGTH_OF_GRID - Settings.TowerDefenseSettings.WallSkipAmount - 1;
                    if (x < Settings.TowerDefenseSettings.WallSkipAmount || x > Settings.TowerDefenseSettings.LENGTH_OF_GRID - Settings.TowerDefenseSettings.WallSkipAmount - 1)
                    {
                        if (y >= lower && y <= upper)
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
                    yPos += Settings.TowerDefenseSettings.GRID_Y_LENGTH * Settings.SCALE.Y;
                }
                xPos += Settings.TowerDefenseSettings.GRID_X_LENGTH * Settings.SCALE.Y;
                yPos = 0;
            }
        }

        public void LoadContent(ContentManager content)
        {

            this._wallHoriz = content.Load<Texture2D>("Sprites/WallHorizBlack");
            this._wallVert = content.Load<Texture2D>("Sprites/WallVertBlack");

            _tile = content.Load<Texture2D>("Backgrounds/tile-1-center");
        }


        public bool IsOnGrid(int x, int y)
        {
            if (x < 0)
                return false;
            if (x >= Settings.TowerDefenseSettings.LENGTH_OF_GRID)
                return false;
            if (y < 0)
                return false;
            if (y >= Settings.TowerDefenseSettings.LENGTH_OF_GRID)
                return false;

            return true;
        }
        public bool IsPieceOccupied(int x, int y)
        {
            if (IsOnGrid(x,y))
            {
                return _grid[x, y].Occupied;
            }
            return true;
        }

        public void OccupyPiece(int x, int y, bool state)
        {
            if (IsOnGrid(x, y))
            {
                _grid[x, y].Occupied = state;
            }
        }
        public bool CoordinatesInMap(int x, int y)
        {
            return x >= 0 && x < Settings.TowerDefenseSettings.LENGTH_OF_GRID && y >= 0 && y < Settings.TowerDefenseSettings.LENGTH_OF_GRID;
        }
        public void HighlightPiece(int x, int y, bool highlight)
        {
            if (IsOnGrid(x, y))
            {
                _grid[x, y].Highlight = highlight;
            }   
        }
        public void Render(SpriteBatch graphics)
        {
            foreach (var gridPiece in _grid)
            {
                if (!gridPiece.Occupied)
                {
                    Color color = Color.White;
                    if (gridPiece.Highlight)
                    {
                        color = Color.Transparent;
                    }
                    graphics.Draw(_tile, new Vector2(gridPiece.XPos, gridPiece.YPos), null, color, 0, new Vector2(0, 0), Settings.TowerDefenseSettings.TILE_SCALE * Settings.SCALE, SpriteEffects.None, 0);

                }
            }



        }

    }
}
