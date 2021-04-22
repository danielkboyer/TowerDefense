using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense
{
    public class HelpView : GameStateView
    {
        private SpriteFont m_font;
        private const string MESSAGE = "There are four turrets:\n" +
            "Basic Turret, Bomb Turret, Missle Turret, and Pellet Turret\n" +
            "Basic Turret is your cheap basic Turret\n" +
            "Bomb Turret explodes dealing area damage\n" +
            "Missle Turret is air only, but is cheap for high damage\n" +
            "Pellet Turret is ground and air but expensive\n" +
            "Controls are located in game and settable in game\n";
        private Texture2D background;
        public override void loadContent(ContentManager contentManager)
        {
            m_font = contentManager.Load<SpriteFont>("Fonts/menu");

            background = contentManager.Load<Texture2D>("Backgrounds/tower-defense-background-stars");
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }

            return GameStateEnum.Help;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();
            m_spriteBatch.Draw(background, new Rectangle(0, 0, Settings.GameSettings.WINDOW_WIDTH, Settings.GameSettings.WINDOW_HEIGHT), Color.White);

            Vector2 stringSize = m_font.MeasureString(MESSAGE) * .5f;
            m_spriteBatch.DrawString(m_font, MESSAGE,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, m_graphics.PreferredBackBufferHeight / 2 - stringSize.Y), Color.LightGray,0,new Vector2(0,0),.5f,SpriteEffects.None,0);

            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
        }
    }
}
