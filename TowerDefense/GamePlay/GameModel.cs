using LunarLanding.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Linq;
using System.Threading.Tasks;
using TowerDefense.Grid;
using TowerDefense.Input;
using TowerDefense.Storage;

namespace TowerDefense.GamePlay
{
    /*
     * player : Lander
     * -collides(obj1, obj1) : boolean-level : int
     * -particleSystem : ParticleSystem
     * -particleSystemRenderer : ParticleSystemRenderer
     * +update(elapsedTime) :void
     * +landerThrust(elapsedTime) : void
     * +landerRotateRight(elapsedTime) : void
     * +landerRotateLeft(elapsedTime) : void
     * */
    public class GameModel
    {

        private PersistentStorage _persistentStorage;
    


        private ParticleSystem _particleSystem;

        private SpriteFont m_font;


        #region Input

        MouseInput _mouseInput;

        #endregion
        #region Player

        private PlayerStats _playerStats;

        #endregion


        #region Map

        private MapGrid _mapGrid;

        #endregion

        public GameModel(ContentManager content)
        {

            _persistentStorage = new PersistentStorage();
            LoadPlayerStats();

            _particleSystem = new ParticleSystem(content);
            m_font = content.Load<SpriteFont>("Fonts/menu");

            _mapGrid = new MapGrid();
            _mapGrid.LoadContent(content);
            _mapGrid.Generate();

            _mouseInput = new MouseInput();
            RegisterInput();

        }

        private void RegisterInput()
        {
            _mouseInput.registerCommand(MouseInput.MouseEvent.MouseDown, new InputDeviceHelper.CommandDelegatePosition(OnMouseDown));
            _mouseInput.registerCommand(MouseInput.MouseEvent.MouseUp, new InputDeviceHelper.CommandDelegatePosition(OnMouseUp));
            _mouseInput.registerCommand(MouseInput.MouseEvent.MouseMove, new InputDeviceHelper.CommandDelegatePosition(OnMouseMove));
        }


        private void OnMouseDown(GameTime gameTime, int x, int y)
        {

        }

        private void OnMouseUp(GameTime gameTime, int x, int y)
        {

        }

        private void OnMouseMove(GameTime gameTime, int x, int y)
        {

        }


        private void LoadPlayerStats()
        {
            _playerStats = _persistentStorage.Load<PlayerStats>("DEFAULT");
            if(_playerStats == null)
            {
                _playerStats = new PlayerStats(new System.Collections.Generic.List<float>(), 0); ;
            }
        }
        private void SavePlayerStats()
        {
            _persistentStorage.Save("DEFAULT", _playerStats);
        }

       
  

     
     
        public void Update(TimeSpan elapsedTime)
        {
            
            _particleSystem.Update(elapsedTime);
        }
 
       

        public void Render(GameTime gameTime, SpriteBatch graphics)
        {
          
            graphics.Begin();

          
            //render the particle system
            _particleSystem.Render(graphics, gameTime.ElapsedGameTime);
            _mapGrid.Render(graphics);
            RenderStats(graphics, gameTime.ElapsedGameTime);
            graphics.End();


        }


        public void RenderStats(SpriteBatch graphics, TimeSpan elapsedTime)
        {
           
        }
    }
}
