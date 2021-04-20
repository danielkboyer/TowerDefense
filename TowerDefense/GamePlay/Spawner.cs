using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using TowerDefense.Grid;
using static TowerDefense.Grid.ShortestPath;

namespace TowerDefense.GamePlay
{
    public class Spawner
    {

        public bool LevelOver
        {
            get
            {
                return _enemies.Count == 0 && (_currentLevel == null || _currentLevel.LevelOver);
            }
        }
        public List<Enemy> _enemies { get; protected set; }

        #region textures

        private List<Texture2D> _greenCreepTextures;

        #endregion

        private MapGrid _mapGrid;
        private ShortestPath _shortestPath;

        private Random _random;

        private Level _currentLevel;

        public (int x, int y) TopSpawnPoint
        {
            get
            {
                var middlePlusOne = Settings.TowerDefenseSettings.LENGTH_OF_GRID / 2;
                return (middlePlusOne + GetRandomWallOpeningPoint(), 0);
            }
        }
        public (int x, int y) BottomSpawnPoint
        {
            get
            {
                var middlePlusOne = Settings.TowerDefenseSettings.LENGTH_OF_GRID / 2;
                return (middlePlusOne + GetRandomWallOpeningPoint(), Settings.TowerDefenseSettings.LENGTH_OF_GRID - 1);
            }
        }
        public (int x, int y) LeftSpawnPoint
        {
            get
            {
                var middlePlusOne = Settings.TowerDefenseSettings.LENGTH_OF_GRID / 2;
                return (0, middlePlusOne + GetRandomWallOpeningPoint());
            }
        }
        public (int x, int y) RightSpawnPoint
        {
            get
            {
                var middlePlusOne = Settings.TowerDefenseSettings.LENGTH_OF_GRID / 2;
                return (Settings.TowerDefenseSettings.LENGTH_OF_GRID - 1, middlePlusOne + GetRandomWallOpeningPoint());
            }
        }
        public Spawner(ContentManager content,MapGrid mapGrid, ShortestPath shortestPath)
        {
            _enemies = new List<Enemy>();
            _greenCreepTextures = LoadGreenCreepTextures(content);
            _random = new Random();
            this._mapGrid = mapGrid;
            this._shortestPath = shortestPath;
            
        }

        public bool StartLevel(int level)
        {
            
            if (level == 1)
            {
                _currentLevel = LevelOne(_shortestPath);
            }

            var canComputePath = _shortestPath.CalculateShortestPath(_mapGrid, _currentLevel.Start.x, _currentLevel.Start.y, _currentLevel.End.x, _currentLevel.End.y);
            if (canComputePath != null)
            {
                _shortestPath.Path = canComputePath;
                return true;
            }
            return false;
        }


        private Level LevelOne(ShortestPath shortestPath)
        {
            
            EnemyPacket packet = new EnemyPacket(new GreenCreep(_greenCreepTextures, 100, 1000, null), 5, 1000, 2000,  shortestPath);
            

            return new Level(new List<EnemyPacket>() { packet }, LeftSpawnPoint, RightSpawnPoint);
        }

        public bool UpdatePath()
        {
            //we need to make sure the left side can reach both the top right and bottom, this means all sides can reach eachother
            var leftSpawnPoint = LeftSpawnPoint;
            var topSpawnPoint = TopSpawnPoint;
            var rightSpawnPoint = RightSpawnPoint;
            var bottomSpawnPoint = BottomSpawnPoint;

            if (_shortestPath.CalculateShortestPath(_mapGrid, leftSpawnPoint.x, leftSpawnPoint.y, rightSpawnPoint.x, rightSpawnPoint.y) == null)
                return false;
            if (_shortestPath.CalculateShortestPath(_mapGrid, leftSpawnPoint.x, leftSpawnPoint.y, topSpawnPoint.x, topSpawnPoint.y) == null)
                return false;
            if (_shortestPath.CalculateShortestPath(_mapGrid, leftSpawnPoint.x, leftSpawnPoint.y, bottomSpawnPoint.x, bottomSpawnPoint.y) == null)
                return false;
            if (_currentLevel == null || _currentLevel.LevelOver)
                return true;
            var canComputePath = _shortestPath.CalculateShortestPath(_mapGrid, _currentLevel.Start.x, _currentLevel.Start.y, _currentLevel.End.x, _currentLevel.End.y);
            if (canComputePath != null)
            {
                var canComputeToList = canComputePath.ToList();
                List<(int index,List<GridPos> path)> enemyPaths = new List<(int index, List<GridPos> path)>();
                //first we have to make sure all enemies can get to a path out
                foreach(var enemy in _enemies)
                {
                    if (enemy.Alive)
                    {
                        var getXYCoord = MapGrid.GetXYFromCoordinates(enemy.Position.X, enemy.Position.Y);
                        var pathForEnemy = _shortestPath.PathToShortest(getXYCoord.x, getXYCoord.y, _mapGrid, canComputeToList);
                        if (pathForEnemy.list == null) 
                            return false;

                        enemyPaths.Add(pathForEnemy);
                    }
                }

                _shortestPath.Path = canComputePath;
                //if we get here we can add them.
                int index = 0;
                foreach (var enemy in _enemies)
                {
                    if (enemy.Alive)
                    {
                        enemy.UpdatePath(enemyPaths[index].path, enemyPaths[index++].index);
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }
        private int GetRandomWallOpeningPoint()
        {
            return _random.Next(-(Settings.TowerDefenseSettings.WallOpeningAmount / 2), Settings.TowerDefenseSettings.WallOpeningAmount / 2 - 1);
        }
        public List<Texture2D> LoadGreenCreepTextures(ContentManager content)
        {
            _greenCreepTextures = new List<Texture2D>();
            for (int x = 1; x < 7; x++)
            {
                _greenCreepTextures.Add(content.Load<Texture2D>("Sprites/GreenCreep/" + x));
            }
            return _greenCreepTextures;
        }
        public void Update(TimeSpan elapsedTime)
        {
            var newEnemies = _currentLevel.Spawn(elapsedTime);
            if(newEnemies != null)
            {
                _enemies.AddRange(newEnemies);
            }
            foreach (var enemy in _enemies)
            {
                if (enemy.Alive)
                {
                    enemy.Update(elapsedTime);
                }
            }
            _enemies.RemoveAll(t => !t.Alive);

            
        }

        public void Draw(SpriteBatch graphics, TimeSpan elapsedTime)
        {
            foreach (var enemy in _enemies)
            {
                if (enemy.Alive)
                {
                    enemy.Draw(graphics, elapsedTime);
                    foreach(var enemyPath in enemy.gridPositions)
                    {
                        graphics.Draw(_greenCreepTextures[0], new Vector2(MapGrid.GetPosition(enemyPath.XPos, enemyPath.YPos).X, MapGrid.GetPosition(enemyPath.XPos, enemyPath.YPos).Y), null, Color.White, 0, new Vector2(_greenCreepTextures[0].Width / 2, _greenCreepTextures[0].Height / 2), .1f * Settings.SCALE, SpriteEffects.None, 0);

                    }
                }
            }
        }
    }
}
