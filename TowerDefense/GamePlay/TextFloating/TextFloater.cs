using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TowerDefense.GamePlay.TextFloating;

namespace TowerDefense.GamePlay
{
    public class TextFloater
    {

        private List<TextFloat> _floatingTexts;

        private SpriteFont _font;
        public TextFloater(SpriteFont font)
        {
            this._font = font;
            _floatingTexts = new List<TextFloat>();
        }

        public void AddFloatingText(int xPos, int yPos, string text)
        {
            _floatingTexts.Add(new TextFloat(_font, xPos, yPos, text));
        }

        public void Update(TimeSpan elapedTime)
        {
            foreach(var text in _floatingTexts)
            {
                text.Update(elapedTime);
            }

            _floatingTexts.RemoveAll(t => t.DeleteMe);
        }

        public void Draw(SpriteBatch graphics)
        {
            foreach (var text in _floatingTexts)
            {
                text.Draw(graphics);
            }
        }
    }
}
