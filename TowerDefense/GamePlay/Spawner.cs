using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using TowerDefense.GamePlay.Creeps;
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

        private List<Texture2D> _basicCreepTextures;

        private List<Texture2D> _tankCreepTextures;

        private Texture2D _greenHealthBar;
        private Texture2D _redHealthBar;

        #endregion

        private MapGrid _mapGrid;
        private ShortestPath _shortestPath;

        private Random _random;

        private Level _currentLevel;

        private IEnemyNotification notificationChannel;
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
        public Spawner(ContentManager content,MapGrid mapGrid, ShortestPath shortestPath,IEnemyNotification notification)
        {
            _enemies = new List<Enemy>();
            _greenHealthBar = content.Load<Texture2D>("Sprites/GreenHealthBar");
            _redHealthBar = content.Load<Texture2D>("Sprites/RedHealthBar");
            _basicCreepTextures = LoadBasicCreepTextures(content);
            _tankCreepTextures = LoadTankCreepTextures(content);
            _random = new Random();
            this._mapGrid = mapGrid;
            this._shortestPath = shortestPath;
            this.notificationChannel = notification;
            
        }

        public bool StartLevel(int level)
        {
            
            if (level == 1)
            {
                _currentLevel = LevelOne(_shortestPath);
            }
            else if(level == 2)
            {
                _currentLevel = LevelTwo(_shortestPath);
            }
            else if(level == 3)
            {
                _currentLevel = LevelThree(_shortestPath);
            }
            else if(level == 4)
            {
                _currentLevel = LevelFour(_shortestPath);
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
            
            EnemyPacket packet = new EnemyPacket(new BasicCreep(_basicCreepTextures,_greenHealthBar,_redHealthBar, null), 5, 0, 2000,  shortestPath);
            

            return new Level(new List<EnemyPacket>() { packet }, LeftSpawnPoint, RightSpawnPoint);
        }

        private Level LevelTwo(ShortestPath shortestPath)
        {

            EnemyPacket packet = new EnemyPacket(new BasicCreep(_basicCreepTextures, _greenHealthBar, _redHealthBar, null), 5, 0, 1000, shortestPath);


            return new Level(new List<EnemyPacket>() { packet }, TopSpawnPoint, BottomSpawnPoint);
        }

        private Level LevelThree(ShortestPath shortestPath)
        {

            EnemyPacket packet = new EnemyPacket(new BasicCreep(_basicCreepTextures, _greenHealthBar, _redHealthBar,  null), 5, 0, 1000, shortestPath);
            EnemyPacket tankPacket = new EnemyPacket(new TankCreep(_tankCreepTextures, _greenHealthBar, _redHealthBar,  null), 1, 4000, 1000, shortestPath);


            return new Level(new List<EnemyPacket>() { packet, tankPacket }, RightSpawnPoint, LeftSpawnPoint);
        }
        private Level LevelFour(ShortestPath shortestPath)
        {

            EnemyPacket tankPacket = new EnemyPacket(new TankCreep(_tankCreepTextures, _greenHealthBar, _redHealthBar, null), 3, 0, 3000, shortestPath);


            return new Level(new List<EnemyPacket>() { tankPacket }, BottomSpawnPoint, TopSpawnPoint);
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
            if (_currentLevel == null || LevelOver)
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
        public List<Texture2D> LoadBasicCreepTextures(ContentManager content)
        {
            _basicCreepTextures = new List<Texture2D>();
            for (int x = 1; x < 7; x++)
            {
                _basicCreepTextures.Add(content.Load<Texture2D>("Sprites/BasicCreep/" + x));
            }
            return _basicCreepTextures;
        }

        public List<Texture2D> LoadTankCreepTextures(ContentManager content)
        {
            _tankCreepTextures = new List<Texture2D>();
            for (int x = 1; x < 5; x++)
            {
                _tankCreepTextures.Add(content.Load<Texture2D>("Sprites/TankCreep/" + x));
            }
            return _tankCreepTextures;
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
            int x = 0;
            while(x < _enemies.Count)
            {
                if (!_enemies[x].Alive)
                {
                    if (_enemies[x].GotToEnd)
                    {
                        notificationChannel.EnemyMadeIt();
                    }
                    else
                    {
                        notificationChannel.EarnMoney(_enemies[x].AwardAmount);
                    }

                    _enemies.RemoveAt(x);
                }
                else
                {
                    x++;
                }
            }

            
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
                        graphics.Draw(_basicCreepTextures[0], new Vector2(MapGrid.GetPosition(enemyPath.XPos, enemyPath.YPos).X, MapGrid.GetPosition(enemyPath.XPos, enemyPath.YPos).Y), null, Color.White, 0, new Vector2(_basicCreepTextures[0].Width / 2, _basicCreepTextures[0].Height / 2), .1f * Settings.SCALE, SpriteEffects.None, 0);

                    }
                }
            }
        }
    }
}
