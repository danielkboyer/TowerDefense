using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense.GamePlay
{
    public interface IEnemyNotification
    {
        public void EarnMoney(int money);

        public void EnemyMadeIt();
    }
}
