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

        #region Enemies

        private Spawner _spawner;
        #endregion
        #region Map

        private MapGrid _mapGrid;
        private ShortestPath _shortestPath;

        #endregion

        #region Cannons
        private IProjectileHandler _projectileHandler;
        private TurretHandler _turretHandler;

        private Texture2D _basicTurretTexture;
        private Texture2D _projectileTexture;
        #endregion
        public GameModel(ContentManager content)
        {

            _persistentStorage = new PersistentStorage();
            LoadPlayerStats();

            _particleSystem = new ParticleSystem(content);
            m_font = content.Load<SpriteFont>("Fonts/menu");
            _basicTurretTexture = content.Load<Texture2D>("Sprites/BasicTurret/turret-1-1");
            _projectileTexture = content.Load<Texture2D>("Sprites/thrustParticle");
            _mapGrid = new MapGrid();
            _mapGrid.LoadContent(content);
            _mapGrid.Generate();

            _shortestPath = new ShortestPath();
            
            _spawner = new Spawner(content,_mapGrid,_shortestPath);
            _spawner.StartLevel(0);

            _projectileHandler = new ProjectileHandler(_spawner._enemies);
            _turretHandler = new TurretHandler(_spawner._enemies);

            _mouseInput = new MouseInput();
            RegisterInput();

        }

        private void RegisterInput()
        {
            _mouseInput.registerCommand(MouseInput.MouseEvent.MouseDown, new InputDeviceHelper.CommandDelegatePosition(OnMouseDown));
            _mouseInput.registerCommand(MouseInput.MouseEvent.MouseUp, new InputDeviceHelper.CommandDelegatePosition(OnMouseUp));
            _mouseInput.registerCommand(MouseInput.MouseEvent.MouseMove, new InputDeviceHelper.CommandDelegatePosition(OnMouseMove));
        }


        private void OnMouseDown(TimeSpan gameTime, int x, int y)
        {

        }

        private void OnMouseUp(TimeSpan gameTime, int x, int y)
        {
            var mapGridCoordinates = MapGrid.GetXYFromCoordinates(x, y);

            int shortest = Settings.TowerDefenseSettings.WallSkipAmount;
            int longest = Settings.TowerDefenseSettings.LENGTH_OF_GRID - Settings.TowerDefenseSettings.WallSkipAmount;
            if (!_mapGrid.IsPieceOccupied(mapGridCoordinates.x, mapGridCoordinates.y) && mapGridCoordinates.x >= shortest && mapGridCoordinates.x < longest && mapGridCoordinates.y >= shortest && mapGridCoordinates.y < longest)
            {
                _mapGrid.OccupyPiece(mapGridCoordinates.x, mapGridCoordinates.y, true);
                if (!_spawner.UpdatePath())
                {
                    _mapGrid.OccupyPiece(mapGridCoordinates.x, mapGridCoordinates.y, false);
                }
                else
                {

                    _turretHandler.AddTurret(new BasicTurret(_basicTurretTexture, _projectileTexture, 10, 10, 10, 500, 2, mapGridCoordinates.x, mapGridCoordinates.y, _projectileHandler, _spawner._enemies));
                }
            }
        }

        private void OnMouseMove(TimeSpan gameTime, int x, int y)
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
            _spawner.Update(elapsedTime);
            _particleSystem.Update(elapsedTime);
            _mouseInput.Update(elapsedTime);
            _turretHandler.Update(elapsedTime);
            _projectileHandler.Update(elapsedTime);
        }
 
       

        public void Render(GameTime gameTime, SpriteBatch graphics)
        {
          
            graphics.Begin();

          
            //render the particle system
            _particleSystem.Render(graphics, gameTime.ElapsedGameTime);
            _mapGrid.Render(graphics);
            _spawner.Draw(graphics, gameTime.ElapsedGameTime);
            _turretHandler.Draw(graphics, gameTime.ElapsedGameTime);
            _projectileHandler.Draw(graphics, gameTime.ElapsedGameTime);
            RenderStats(graphics, gameTime.ElapsedGameTime);
            graphics.End();


        }


        public void RenderStats(SpriteBatch graphics, TimeSpan elapsedTime)
        {
           
        }
    }
}
