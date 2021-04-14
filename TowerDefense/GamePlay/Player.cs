using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TowerDefense.GamePlay
{
    public class Player
    {

        Texture2D _cannonTexture { get; set; }

        private int _money;
       
        public int Money
        {
            get
            {
                return _money;
            }
            set
            {
                _money = value;
            }
        }
        public Player(int money)
        {
            this._money = money;
            
        }
        public bool Update(TimeSpan elapsedTime)
        {
           
            

            return true;
        }

      
     
      
    }
}
