using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense.GamePlay
{
    public interface IEnemyNotification
    {
        public void EarnMoney(int money,Vector2 deathPosition, int topY);

        public void EnemyMadeIt();
    }
}
