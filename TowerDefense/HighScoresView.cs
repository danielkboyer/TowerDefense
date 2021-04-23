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
        private const string HIGH_SCORE_MESSAGE = "HIGHSCORES";

        private const string TIME_MESSAGE = "BEST LEVEL";

        private PlayerStats _playerStats;
        private PersistentStorage _persistentStorage;
        private float[] _highScores = new float[5];

        private double[] _timeHighScores = new double[5];
        private Texture2D background;

        private double m_updateScoreTime;

        public override void loadContent(ContentManager contentManager)
        {
            m_font = contentManager.Load<SpriteFont>("Fonts/menu");
            background = contentManager.Load<Texture2D>("Backgrounds/tower-defense-background-stars");
            _persistentStorage = new PersistentStorage();

        }

        public void LoadPlayerStats()
        {
                _playerStats = _persistentStorage.Load<PlayerStats>("DEFAULT");
                if (_playerStats != null)
                {
                    for (int x = 0; x < _playerStats.HighScores.Count; x++)
                    {
                        _highScores[x] = _playerStats.HighScores[x];
                    }


                }
                m_updateScoreTime = 500;
           
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

            m_spriteBatch.Draw(background, new Rectangle(0, 0, Settings.GameSettings.WINDOW_WIDTH, Settings.GameSettings.WINDOW_HEIGHT), Color.White);


            float yStart = m_graphics.PreferredBackBufferHeight / 2 - 100;
            Vector2 stringSize = m_font.MeasureString(HIGH_SCORE_MESSAGE);
            m_spriteBatch.DrawString(m_font, HIGH_SCORE_MESSAGE,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, m_graphics.PreferredBackBufferHeight / 2 - stringSize.Y - yStart), Color.Lime);

            Vector2 totalSize = new Vector2(0, 0);
            for (int x = 0; x < _highScores.Length; x++)
            {
                string MESSAGE = $"{x + 1}: {_highScores[x]}";
                totalSize += m_font.MeasureString(MESSAGE);
                m_spriteBatch.DrawString(m_font, MESSAGE,
                    new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, m_graphics.PreferredBackBufferHeight / 2 + totalSize.Y - yStart), Color.LightGray);

            }
            string levelMessage = TIME_MESSAGE + ": " + _playerStats.Level;
            var timeStringSize = m_font.MeasureString(levelMessage);
            totalSize += timeStringSize;
            m_spriteBatch.DrawString(m_font, levelMessage,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, m_graphics.PreferredBackBufferHeight / 2 + totalSize.Y - yStart), Color.Lime);




            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
            m_updateScoreTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (m_updateScoreTime < 0)
            {
                m_updateScoreTime = 500;
                LoadPlayerStats();
            }
        }
    }
}
