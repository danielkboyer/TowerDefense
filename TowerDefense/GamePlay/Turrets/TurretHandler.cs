using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefense.GamePlay
{
    public class TurretHandler
    {
        private List<Turret> _turrets;

        private List<Enemy> _enemies;
        public TurretHandler(List<Enemy> enemies)
        {
            _turrets = new List<Turret>();
            _enemies = enemies;
        }

        public void ReloadGame()
        {
            _turrets.Clear();
        }
        public void RemoveTurret(int x, int y)
        {
            _turrets.RemoveAll(t => t.XPos == x && t.YPos == y);
        }
        public Turret GetTurret(int x, int y)
        {
            return _turrets.FirstOrDefault(t=>t.XPos == x && t.YPos == y);
        }

        public void AddTurret(Turret turret)
        {
            _turrets.Add(turret);
        }
        public void Update(TimeSpan elapsedTime)
        {
            foreach(var turret in _turrets)
            {
                turret.Update(elapsedTime);
            }
        }

        public void Draw(SpriteBatch graphics,TimeSpan elapsedTime)
        {
            foreach(var turret in _turrets)
            {
                turret.Draw(graphics, elapsedTime);
            }
        }
    }
}
