using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense.Input
{
    public enum KeyTrigger
    {
        KEY_UP = 0,
        KEY_DOWN = 1,
        KEY_PRESSED = 2,
        NOTHING = 3
    }
    public class KeyInformation
    {
        public KeyTrigger KeyTrigger;
        public string Reason;

        public KeyInformation(KeyTrigger keyTrigger, string reason)
        {
            this.KeyTrigger = keyTrigger;
            this.Reason = reason;
        }
    }
}
