using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TowerDefense.Input
{
    public static class InputHandling
    {
        static Dictionary<Keys, KeyInformation> keys = new Dictionary<Keys, KeyInformation>();
        static Dictionary<Keys, KeyInfo<TimeSpan>> handlers = new Dictionary<Keys, KeyInfo<TimeSpan>>();




        public static Dictionary<Keys, KeyInformation> Keys
        {
            get
            {
                return keys;
            }
        }

        public static Dictionary<Keys, KeyInfo<TimeSpan>> Handlers
        {
            get
            {
                return handlers;
            }
        }


        public static void UpdateKey(Keys currentKey, Keys newKey)
        {
            RegisterCommand(newKey, handlers[currentKey].Handler, handlers[currentKey].KeyTriggerInfo);
            UnRegisterCommand(currentKey);
        }
        public static void UnRegisterCommand(Keys key)
        {
            if (handlers.ContainsKey(key))
            {
                handlers.Remove(key);
            }

            if (keys.ContainsKey(key))
            {
                keys.Remove(key);
            }
        }
        public static void RegisterCommand(Keys key, Action<TimeSpan> handler, KeyInformation trigger)
        {
            //Make sure this key hasn't already been added.
            if (handlers.ContainsKey(key))
            {
                handlers[key] = new KeyInfo<TimeSpan>(trigger, handler);
            }
            else
            {
                handlers.Add(key, new KeyInfo<TimeSpan>(trigger, handler));

            }


            if (keys.ContainsKey(key))
            {
                keys[key] = new KeyInformation(trigger.KeyTrigger, trigger.Reason);
            }
            else
            {
                keys.Add(key, new KeyInformation(trigger.KeyTrigger, trigger.Reason));
            }
        }

        public static void Update(TimeSpan elapsedTime)
        {

            UpdateKeyValues();

            foreach (var key in keys)
            {
                if (handlers.ContainsKey(key.Key) && handlers[key.Key].KeyTriggerInfo.KeyTrigger == key.Value.KeyTrigger)
                {
                    handlers[key.Key].Handler(elapsedTime);
                }
            }
        }

        private static void UpdateKeyValues()
        {
            List<Keys> keyList = keys.Keys.ToList();
            foreach (var key in keyList)
            {
                //If this is the first time the key was pressed
                if (Keyboard.GetState().IsKeyDown(key) && keys[key].KeyTrigger == KeyTrigger.NOTHING)
                {
                    keys[key].KeyTrigger = KeyTrigger.KEY_DOWN;
                }
                //otherwise if this is  2+
                else if (Keyboard.GetState().IsKeyDown(key))
                {
                    keys[key].KeyTrigger = KeyTrigger.KEY_PRESSED;
                }
                else if (Keyboard.GetState().IsKeyUp(key) && keys[key].KeyTrigger != KeyTrigger.NOTHING && keys[key].KeyTrigger != KeyTrigger.KEY_UP)
                {
                    keys[key].KeyTrigger = KeyTrigger.KEY_UP;
                }
                else
                {
                    keys[key].KeyTrigger = KeyTrigger.NOTHING;
                }
            }
        }
    }
}
