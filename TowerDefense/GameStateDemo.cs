using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TowerDefense
{
    public class GameStateDemo : Game
    {
        private GraphicsDeviceManager m_graphics;
        private IGameState m_currentState;
        private GameStateEnum m_nextStateEnum = GameStateEnum.MainMenu;
        private Dictionary<GameStateEnum, IGameState> m_states;

        public GameStateDemo()
        {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            m_graphics.PreferredBackBufferWidth = Settings.GameSettings.WINDOW_WIDTH;
            m_graphics.PreferredBackBufferHeight = Settings.GameSettings.WINDOW_HEIGHT;

            m_graphics.ApplyChanges();

            // Create all the game states here
            m_states = new Dictionary<GameStateEnum, IGameState>();
            m_states.Add(GameStateEnum.MainMenu, new MainMenuView());
            m_states.Add(GameStateEnum.GamePlay, new GamePlayView());
            m_states.Add(GameStateEnum.HighScores, new HighScoresView());
            m_states.Add(GameStateEnum.Help, new HelpView());
            m_states.Add(GameStateEnum.About, new AboutView());

            // We are stating with the main menu
            m_currentState = m_states[GameStateEnum.MainMenu];

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Give all game states a chance to load their content
            foreach (var item in m_states)
            {
                item.Value.initialize(this.GraphicsDevice, m_graphics);
                item.Value.loadContent(this.Content);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            m_nextStateEnum = m_currentState.processInput(gameTime);
            // Special case for exiting the game
            if (m_nextStateEnum == GameStateEnum.Exit)
            {
                Exit();
            }

            m_currentState.update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            m_currentState.render(gameTime);

            m_currentState = m_states[m_nextStateEnum];

            base.Draw(gameTime);
        }
    }
}
