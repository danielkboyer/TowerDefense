using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TowerDefense.GamePlay.TextFloating
{
    public class TextFloat
    {
        private string _message;
        private Vector2 _position;

        private float _updateRate = 100;
        private Vector2 _direction = new Vector2(0, 1);
        private float _speed = -10;
        private TimeSpan _currentSpeedTime;

        private SpriteFont _font;

        private TimeSpan _lifeTime = new TimeSpan(0, 0, 1);

        private float _textScale = .2f;
        public bool DeleteMe
        {
            get
            {
                return _lifeTime.TotalSeconds <= 0;
            }
        }
        public TextFloat(SpriteFont font, int xPos, int yPos, string text)
        {
            _position = new Vector2(xPos, yPos);
            this._message = text;
            this._font = font;
        }

        public void Update(TimeSpan elapsedTime)
        {
            _currentSpeedTime += elapsedTime;
            _lifeTime -= elapsedTime;
            if (_currentSpeedTime.TotalMilliseconds >= _updateRate)
            {
                _currentSpeedTime -= TimeSpan.FromMilliseconds(_updateRate);
                _position += _direction * _speed;

            }


        }

        public void Draw(SpriteBatch graphics)
        {

            graphics.DrawString(_font, _message, _position, Color.GreenYellow, 0, new Vector2(0, 0), _textScale, SpriteEffects.None, 0);
        }
    }
}
