using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense.Input
{
    public class KeyInfo<T>
    {
        public KeyInformation KeyTriggerInfo { get; set; }
        public Action<T> Handler { get; set; }


        public KeyInfo(KeyInformation trigger, Action<T> handler)
        {
            this.KeyTriggerInfo = trigger;
            this.Handler = handler;
        }
    }
}
