using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    public abstract class GameStateView : IGameState
    {
        protected GraphicsDeviceManager m_graphics;
        protected SpriteBatch m_spriteBatch;

        public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
            m_graphics = graphics;
            m_spriteBatch = new SpriteBatch(graphicsDevice);
        }
        public abstract void loadContent(ContentManager contentManager);
        public abstract GameStateEnum processInput(GameTime gameTime);
        public abstract void render(GameTime gameTime);
        public abstract void update(GameTime gameTime);
    }
}
