using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TowerDefense.GamePlay;
using TowerDefense.Storage;

namespace TowerDefense
{
    public class HighScoresView : GameStateView
    {
        private SpriteFont m_font;
        private const string MESSAGE = "These are the high scores";

        private PlayerStats _playerStats;
        private PersistentStorage _persistentStorage;
        private float[] _highScores = new float[5];
        public override void loadContent(ContentManager contentManager)
        {
            m_font = contentManager.Load<SpriteFont>("Fonts/menu");
            _persistentStorage = new PersistentStorage();
            _playerStats = _persistentStorage.Load<PlayerStats>("DEFAULT");
            if (_playerStats != null)
            {
                for (int x = 0; x < _playerStats.HighScores.Count; x++)
                {
                    _highScores[x] = _playerStats.HighScores[x];
                }
            }
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }

            return GameStateEnum.HighScores;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            Vector2 stringSize = m_font.MeasureString(MESSAGE);
            m_spriteBatch.DrawString(m_font, MESSAGE,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, m_graphics.PreferredBackBufferHeight / 2 - stringSize.Y), Color.Black);

            Vector2 totalSize = new Vector2(0, 0);
            for (int x = 0; x < _highScores.Length; x++)
            {
                string MESSAGE = $"{x + 1}: {_highScores[x]}";
                totalSize += m_font.MeasureString(MESSAGE);
                m_spriteBatch.DrawString(m_font, MESSAGE,
                    new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, m_graphics.PreferredBackBufferHeight / 2 + totalSize.Y), Color.Black);

            }



            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
        }
    }
}
