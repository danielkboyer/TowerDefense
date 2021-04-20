using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TowerDefense.Grid;

namespace TowerDefense.GamePlay
{
    public class EnemyPacket
    {
        public Enemy enemy;
        public int Amount;
        public float StartTime;
        public float SpawnRate;

        private TimeSpan _currentSpawnTime;

        private bool _canSpawn;

        private ShortestPath _shortestPath;


        public EnemyPacket(Enemy enemy, int amount, float startTime, float spawnRate,ShortestPath _shortestPath)
        {
            this.enemy = enemy;
            this.Amount = amount;
            this.StartTime = startTime;
            this.SpawnRate = spawnRate;
            this._shortestPath = _shortestPath;
            _canSpawn = false;
        }


        public Enemy Spawn(TimeSpan elapsedTime)
        {
            _currentSpawnTime += elapsedTime;

            if(!_canSpawn && _currentSpawnTime.TotalMilliseconds >= StartTime)
            {
                _canSpawn = true;
                _currentSpawnTime -= TimeSpan.FromMilliseconds(StartTime);
            }

            if (_canSpawn && Amount > 0)
            {
                if(_currentSpawnTime.TotalMilliseconds >= SpawnRate)
                {
                    Amount--;
                    _currentSpawnTime -= TimeSpan.FromMilliseconds(SpawnRate);
                    return enemy.Copy(_shortestPath.Path.ToList());
                }
            }

            return null;
        }
    }
}
