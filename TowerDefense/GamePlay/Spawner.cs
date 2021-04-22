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

        private List<Texture2D> _flyingCreepTextures;

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

        
        public Spawner(ContentManager content, MapGrid mapGrid, ShortestPath shortestPath, IEnemyNotification notification)
        {
            _enemies = new List<Enemy>();
            _greenHealthBar = content.Load<Texture2D>("Sprites/GreenHealthBar");
            _redHealthBar = content.Load<Texture2D>("Sprites/RedHealthBar");
            _basicCreepTextures = LoadBasicCreepTextures(content);
            _tankCreepTextures = LoadTankCreepTextures(content);
            _flyingCreepTextures = LoadFlyingCreepTextures(content);
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
            else if (level == 2)
            {
                _currentLevel = LevelTwo(_shortestPath);
            }
            else if (level == 3)
            {
                _currentLevel = LevelThree(_shortestPath);
            }
            else if (level == 4)
            {
                _currentLevel = LevelFour(_shortestPath);
            }
            else if (level == 5)
            {
                _currentLevel = LevelFive(_shortestPath);
            }
            else if(level == 6)
            {
                _currentLevel = LevelSix(_shortestPath);
            }
            else if (level == 7)
            {
                _currentLevel = LevelSeven(_shortestPath);
            }
            else if (level == 8)
            {
                _currentLevel = LevelEight(_shortestPath);
            }
            else if (level == 9)
            {
                _currentLevel = LevelNine(_shortestPath);
            }
            else if (level == 10)
            {
                _currentLevel = LevelTen(_shortestPath);
            }
            else 
            {
                _currentLevel = LevelX(_shortestPath, level);
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

            EnemyPacket packet = new EnemyPacket(new BasicCreep(_basicCreepTextures, _greenHealthBar, _redHealthBar, null), 5, 0, 2000, shortestPath);

            return new Level(new List<EnemyPacket>() { packet }, LeftSpawnPoint, RightSpawnPoint);
        }

        private Level LevelTwo(ShortestPath shortestPath)
        {
            EnemyPacket packet = new EnemyPacket(new BasicCreep(_basicCreepTextures, _greenHealthBar, _redHealthBar, null), 5, 0, 1000, shortestPath);


            return new Level(new List<EnemyPacket>() { packet }, TopSpawnPoint, BottomSpawnPoint);
        }

        private Level LevelThree(ShortestPath shortestPath)
        {

            EnemyPacket packet = new EnemyPacket(new BasicCreep(_basicCreepTextures, _greenHealthBar, _redHealthBar, null), 5, 0, 1000, shortestPath);
            EnemyPacket tankPacket = new EnemyPacket(new TankCreep(_tankCreepTextures, _greenHealthBar, _redHealthBar, null), 1, 4000, 1000, shortestPath);


            return new Level(new List<EnemyPacket>() { packet, tankPacket }, RightSpawnPoint, LeftSpawnPoint);
        }
        private Level LevelFour(ShortestPath shortestPath)
        {

            EnemyPacket tankPacket = new EnemyPacket(new TankCreep(_tankCreepTextures, _greenHealthBar, _redHealthBar, null), 3, 0, 3000, shortestPath);


            return new Level(new List<EnemyPacket>() { tankPacket }, BottomSpawnPoint, TopSpawnPoint);
        }

        private Level LevelFive(ShortestPath shortestPath)
        {

            EnemyPacket flyingPacket = new EnemyPacket(new FlyingCreep(_flyingCreepTextures, _greenHealthBar, _redHealthBar, null), 5, 0, 1000, shortestPath);


            return new Level(new List<EnemyPacket>() { flyingPacket }, LeftSpawnPoint, RightSpawnPoint);
        }

        private Level LevelSix(ShortestPath shortestPath)
        {
            EnemyPacket packet = new EnemyPacket(new BasicCreep(_basicCreepTextures, _greenHealthBar, _redHealthBar, null), 25, 0, 1000, shortestPath);


            return new Level(new List<EnemyPacket>() { packet }, RightSpawnPoint, LeftSpawnPoint);
        }

        private Level LevelSeven(ShortestPath shortestPath)
        {
            EnemyPacket packet = new EnemyPacket(new BasicCreep(_basicCreepTextures, _greenHealthBar, _redHealthBar, null), 25, 0, 600, shortestPath);


            return new Level(new List<EnemyPacket>() { packet }, BottomSpawnPoint, TopSpawnPoint);
        }

        private Level LevelEight(ShortestPath shortestPath)
        {
            EnemyPacket packet = new EnemyPacket(new BasicCreep(_basicCreepTextures, _greenHealthBar, _redHealthBar, null), 25, 1500, 1000, shortestPath);
            EnemyPacket tankPacket = new EnemyPacket(new TankCreep(_tankCreepTextures, _greenHealthBar, _redHealthBar, null), 10, 0, 3000, shortestPath);

            return new Level(new List<EnemyPacket>() { packet , tankPacket }, TopSpawnPoint, BottomSpawnPoint);
        }
        private Level LevelNine(ShortestPath shortestPath)
        {
            EnemyPacket packet = new EnemyPacket(new BasicCreep(_basicCreepTextures, _greenHealthBar, _redHealthBar, null), 25, 2000, 1000, shortestPath);
            EnemyPacket tankPacket = new EnemyPacket(new TankCreep(_tankCreepTextures, _greenHealthBar, _redHealthBar, null), 10, 0, 3000, shortestPath);
            EnemyPacket flyingPacket = new EnemyPacket(new FlyingCreep(_flyingCreepTextures, _greenHealthBar, _redHealthBar, null), 5, 0, 1000, shortestPath);
            return new Level(new List<EnemyPacket>() { packet, tankPacket, flyingPacket }, LeftSpawnPoint, RightSpawnPoint);
        }

        private Level LevelTen(ShortestPath shortestPath)
        {
            EnemyPacket packet = new EnemyPacket(new BasicCreep(_basicCreepTextures, _greenHealthBar, _redHealthBar, null), 30, 1000, 1000, shortestPath);
            EnemyPacket tankPacket = new EnemyPacket(new TankCreep(_tankCreepTextures, _greenHealthBar, _redHealthBar, null), 10, 0, 500, shortestPath);
            EnemyPacket flyingPacket = new EnemyPacket(new FlyingCreep(_flyingCreepTextures, _greenHealthBar, _redHealthBar, null), 5, 0, 1000, shortestPath);
            return new Level(new List<EnemyPacket>() { packet, tankPacket, flyingPacket }, RightSpawnPoint, LeftSpawnPoint);
        }

        private Level LevelX(ShortestPath shortestPath, int level)
        {
            int xLevel = level - 11;
            xLevel = (int)Math.Pow(xLevel, 1.2);
            int basicMultiplier = level * 3;

            int basicTime = 11 * (60 - (level-11) * 5);
            if (basicTime < 50)
                basicTime = 50;

            int basicSpeed = -(50 * (level - 11));
            if (basicSpeed < -750)
                basicSpeed = -750;

            int basicHealth = 10 * (xLevel);


            int tankHealth = 80 * (xLevel);
            int tankSpeed = -(50 * (level - 11));
            if (tankSpeed < -1750)
                tankSpeed = -1750;
            int tankMultiplier =(int) (level * 1.5f);


            int flyHealth = 10 * (xLevel);
            int flyingMultiplier = (int)(level * .78f);


            EnemyPacket packet = new EnemyPacket(new BasicCreep(_basicCreepTextures, _greenHealthBar, _redHealthBar, null, basicHealth, basicSpeed), basicMultiplier, 0, basicTime, shortestPath);
            EnemyPacket tankPacket = new EnemyPacket(new TankCreep(_tankCreepTextures, _greenHealthBar, _redHealthBar, null, tankHealth, tankSpeed), tankMultiplier, 1000, 500, shortestPath);
            EnemyPacket flyingPacket = new EnemyPacket(new FlyingCreep(_flyingCreepTextures, _greenHealthBar, _redHealthBar, null, flyHealth), flyingMultiplier, 5000, 1000, shortestPath);
            var randomSpawnPoint = GetRandomSpawnPoint();
            return new Level(new List<EnemyPacket>() { packet, tankPacket, flyingPacket }, (randomSpawnPoint.startX,randomSpawnPoint.startY), (randomSpawnPoint.endX, randomSpawnPoint.endY));
        }

        public (int startX, int startY, int endX, int endY) GetRandomSpawnPoint()
        {
            int randomSpawnStart = _random.Next(0, 3);
            int randomSpawnEnd = _random.Next(0, 3);
            if (randomSpawnEnd == randomSpawnStart)
                return GetRandomSpawnPoint();
            (int startX, int startY, int endX, int endY) toReturn;

            (int x, int y) spawn1 = GetSpawn(randomSpawnStart);
            (int x, int y) spawn2 = GetSpawn(randomSpawnEnd);
            toReturn.startX = spawn1.x;
            toReturn.startY = spawn1.y; 
            toReturn.endX = spawn2.x;
            toReturn.endY = spawn2.y;

            return toReturn;
        }


        private (int x, int y) GetSpawn(int x)
        {
            if(x == 0)
            {
                return LeftSpawnPoint;
            }
            else if (x == 1)
            {
                return RightSpawnPoint;
            }
            else if (x == 2)
            {
                return BottomSpawnPoint;
            }
            else 
            {
                return TopSpawnPoint;
            }
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
                List<(int index, List<GridPos> path)> enemyPaths = new List<(int index, List<GridPos> path)>();
                //first we have to make sure all enemies can get to a path out
                foreach (var enemy in _enemies)
                {
                    if (enemy.Alive && !enemy.CanFly)
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
                    if (enemy.Alive && !enemy.CanFly)
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
        public List<Texture2D> LoadFlyingCreepTextures(ContentManager content)
        {
            _flyingCreepTextures = new List<Texture2D>();

            _flyingCreepTextures.Add(content.Load<Texture2D>("Sprites/FlyingCreep/hangar-ship"));

            return _flyingCreepTextures;
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
            if (newEnemies != null)
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
            while (x < _enemies.Count)
            {
                if (!_enemies[x].Alive)
                {
                    if (_enemies[x].GotToEnd)
                    {
                        notificationChannel.EnemyMadeIt();
                    }
                    else
                    {
                        notificationChannel.EarnMoney(_enemies[x].AwardAmount, _enemies[x].Position, (int)_enemies[x].Height / 2);
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
                    foreach (var enemyPath in enemy.gridPositions)
                    {
                        graphics.Draw(_basicCreepTextures[0], new Vector2(MapGrid.GetPosition(enemyPath.XPos, enemyPath.YPos).X, MapGrid.GetPosition(enemyPath.XPos, enemyPath.YPos).Y), null, Color.White, 0, new Vector2(_basicCreepTextures[0].Width / 2, _basicCreepTextures[0].Height / 2), .1f * Settings.SCALE, SpriteEffects.None, 0);

                    }
                }
            }
        }
    }
}
