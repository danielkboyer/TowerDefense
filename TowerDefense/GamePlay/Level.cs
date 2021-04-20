using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefense.GamePlay
{
    public class Level
    {
        public bool LevelOver
        {
            get
            {
                return !EnemyPackets.Any(t => t.Amount > 0);
            }
        }
        public List<EnemyPacket> EnemyPackets;

        public (int x, int y) Start;
        public (int x, int y) End;

        public Level(List<EnemyPacket> enemyPackets, (int x, int y) start, (int x, int y) end)
        {
            this.EnemyPackets = enemyPackets;
            this.Start = start;
            this.End = end;
        }


        public List<Enemy> Spawn(TimeSpan elapsedTime)
        {
            List<Enemy> enemies = new List<Enemy>();
            foreach(var enemyPacket in EnemyPackets)
            {
                Enemy spawned = enemyPacket.Spawn(elapsedTime);
                if(spawned != null)
                {
                    enemies.Add(spawned);
                }
            }

            if (enemies.Count == 0)
                return null;
            return enemies;
        }
    }
}
