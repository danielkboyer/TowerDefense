using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TowerDefense.GamePlay;

namespace TowerDefense
{
    public class GamePlayView : GameStateView
    {

        private GameModel _model;
        public override void loadContent(ContentManager contentManager)
        {

            _model = new GameModel(contentManager);
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {

                return GameStateEnum.MainMenu;
            }

            return GameStateEnum.GamePlay;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

           m_spriteBatch.End();

            _model.Render(gameTime, m_spriteBatch);




        }
       
        public override void update(GameTime gameTime)
        {
            _model.Update(gameTime.ElapsedGameTime);
        }
    }
}
