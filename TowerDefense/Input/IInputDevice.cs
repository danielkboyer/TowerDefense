using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense.Input
{
    /// <summary>
    /// Abstract base class that defines how input is presented to game code.
    /// </summary>
    public interface IInputDevice
    {
        void Update(TimeSpan gameTime);
    }

    public class InputDeviceHelper
    {
        public delegate void CommandDelegate(TimeSpan gameTime, float value);
        public delegate void CommandDelegatePosition(TimeSpan GameTime, int x, int y);
    }
}
