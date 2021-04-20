using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense.Input
{
    /// <summary>
    /// Derived input device for the mouse
    /// </summary>
    public class MouseInput : IInputDevice
    {
        public void registerCommand(MouseEvent evt, InputDeviceHelper.CommandDelegatePosition callback)
        {
            if (m_commandEntries.ContainsKey(evt))
            {
                m_commandEntries.Remove(evt);
            }
            m_commandEntries.Add(evt, new CommandEntry(evt, callback));
        }

        public void Update(TimeSpan gameTime)
        {
            MouseState state = Mouse.GetState();
            foreach (CommandEntry entry in this.m_commandEntries.Values)
            {
                // Transitioning from mouse up to mouse down
                if (entry.evt == MouseEvent.MouseDown && state.LeftButton == ButtonState.Pressed && !m_mouseDown)
                {
                    entry.callback(gameTime, state.X, state.Y);
                }
                // Transitioning from mouse down to mouse up
                if (entry.evt == MouseEvent.MouseUp && state.LeftButton == ButtonState.Released && m_mouseDown)
                {
                    entry.callback(gameTime, state.X, state.Y);
                }
                if (entry.evt == MouseEvent.MouseMove)
                {
                    if (state.X != m_mousePreviousState.X || state.Y != m_mousePreviousState.Y)
                    {
                        entry.callback(gameTime, state.X, state.Y);
                    }
                }
            }

            m_mouseDown = (state.LeftButton == ButtonState.Pressed);
            m_mousePreviousState = state;
        }

        public enum MouseEvent
        {
            MouseDown,
            MouseUp,
            MouseMove
        }

        private bool m_mouseDown = false;
        private MouseState m_mousePreviousState = Mouse.GetState();

        private struct CommandEntry
        {
            public CommandEntry(MouseEvent evt, InputDeviceHelper.CommandDelegatePosition callback)
            {
                this.evt = evt;
                this.callback = callback;
            }

            public MouseEvent evt;
            public InputDeviceHelper.CommandDelegatePosition callback;
        }

        private Dictionary<MouseEvent, CommandEntry> m_commandEntries = new Dictionary<MouseEvent, CommandEntry>();
    }
}
