using TowerDefense.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using TowerDefense.GamePlay.Turrets;
using TowerDefense.Grid;
using TowerDefense.Input;
using TowerDefense.Storage;
using Microsoft.Xna.Framework.Input;

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
    public class GameModel : IEnemyNotification
    {

        private PersistentStorage _persistentStorage;


        Action<TimeSpan> m_updateFunction;

        private SpriteFont m_font;

        private (int x, int y) MouseOn;

        #region Input

        MouseInput _mouseInput;

        #endregion
        #region Player

        private PlayerStats _playerStats;

        private int Money;

        private int Lives;

        private int Score;

        private bool GameOver;
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
        private Texture2D _basicTurretTexture2;
        private Texture2D _basicTurretTexture3;

        private Texture2D _bombTurretTexture;
        private Texture2D _bombTurretTexture2;
        private Texture2D _bombTurretTexture3;

        private Texture2D _missleTurretTexture;
        private Texture2D _missleTurretTexture2;
        private Texture2D _missleTurretTexture3;

        private Texture2D _pelletTurretTexture;
        private Texture2D _pelletTurretTexture2;
        private Texture2D _pelletTurretTexture3;

        private Texture2D _projectileTexture;
        private Texture2D _bombProjectileTexture;
        private Texture2D _pelletProjectileTexture;
        private Texture2D _missleProjectileTexture;

        private Texture2D _platformTexture;

        private Texture2D _turretStatUnfilled;
        private Texture2D _turretStatFilled;

        private Turret _buyTurret;
        private Turret _selectedTurret;


        private Color _placeColor = Color.Green;
        private Color _hoverColor = Color.Gray;
        private Color _rangeColor = Color.Coral;




        private int Level;
        #endregion

        #region Backgrounds

        private Texture2D _mainBackground;
        private Texture2D _lifeSensorGreen;
        private Texture2D _lifeSensorYellow;
        private Texture2D _lifeSensorRed;

        private Texture2D _infoBackground;

        #endregion

        #region FloatingText

        private TextFloater _textFloater;
        #endregion
        public GameModel(ContentManager content)
        {
            //*************************************************
            //Load Persistent Storage and Player Stats
            //*************************************************
            _persistentStorage = new PersistentStorage();
            LoadPlayerStats();
            //*************************************************
            //*************************************************



            //*************************************************
            //Load Textures 
            //*************************************************
            m_font = content.Load<SpriteFont>("Fonts/menu");
            _basicTurretTexture = content.Load<Texture2D>("Sprites/BasicTurret/turret-1-1");
            _basicTurretTexture2 = content.Load<Texture2D>("Sprites/BasicTurret/turret-1-2");
            _basicTurretTexture3 = content.Load<Texture2D>("Sprites/BasicTurret/turret-1-3");
            _bombTurretTexture = content.Load<Texture2D>("Sprites/BombTurret/turret-2-1");
            _bombTurretTexture2 = content.Load<Texture2D>("Sprites/BombTurret/turret-2-2");
            _bombTurretTexture3 = content.Load<Texture2D>("Sprites/BombTurret/turret-2-3");
            _missleTurretTexture = content.Load<Texture2D>("Sprites/MissleTurret/turret-3-1");
            _missleTurretTexture2 = content.Load<Texture2D>("Sprites/MissleTurret/turret-3-2");
            _missleTurretTexture3 = content.Load<Texture2D>("Sprites/MissleTurret/turret-3-3");
            _pelletTurretTexture = content.Load<Texture2D>("Sprites/PelletTurret/turret-5-1");
            _pelletTurretTexture2 = content.Load<Texture2D>("Sprites/PelletTurret/turret-5-2");
            _pelletTurretTexture3 = content.Load<Texture2D>("Sprites/PelletTurret/turret-5-3");

            _projectileTexture = content.Load<Texture2D>("Sprites/thrustParticle");
            _bombProjectileTexture = content.Load<Texture2D>("Sprites/BombTurret/Fireball");
            _pelletProjectileTexture = content.Load<Texture2D>("Sprites/PelletTurret/GreenPellet");
            _missleProjectileTexture = content.Load<Texture2D>("Sprites/MissleTurret/MissleProjectile");

            _platformTexture = content.Load<Texture2D>("Backgrounds/turret-base");
            _mainBackground = content.Load<Texture2D>("Backgrounds/tower-defense-background-stars");
            _lifeSensorGreen = content.Load<Texture2D>("Backgrounds/13-1");
            _lifeSensorYellow = content.Load<Texture2D>("Backgrounds/13-2");
            _lifeSensorRed = content.Load<Texture2D>("Backgrounds/13-3");
            _infoBackground = content.Load<Texture2D>("Backgrounds/stats-background");
            _turretStatUnfilled = content.Load<Texture2D>("Sprites/stat-current");
            _turretStatFilled = content.Load<Texture2D>("Sprites/stat-upgrade");
            //*************************************************
            //*************************************************


            //*************************************************
            //Load Systems 
            //*************************************************

            //particle
            ParticleSystem.LoadContent(content);
            
            //grid
            _mapGrid = new MapGrid();
            _mapGrid.LoadContent(content);

            //BFS
            _shortestPath = new ShortestPath();

            //Spawner
            _spawner = new Spawner(content, _mapGrid, _shortestPath, this);

            //Projectile Factory
            _projectileHandler = new ProjectileHandler(_spawner._enemies);
            //Turret Factory
            _turretHandler = new TurretHandler(_spawner._enemies);
            //Mouse Input
            _mouseInput = new MouseInput();

            _textFloater = new TextFloater(m_font);
            RegisterInput();
            //*************************************************
            //*************************************************
            InitializeMapRenderCoordinates();
            ReloadGame();
            

        }


        private void ReloadGame()
        {
            //*************************************************
            //Load Systems 
            //*************************************************
            

            //grid
            _mapGrid.ReloadMap();
            
            //BFS
            _shortestPath = new ShortestPath();

            _turretHandler.ReloadGame();
            _projectileHandler.ReloadGame();

            _spawner._enemies.Clear();
            m_updateFunction = MapUpdate;
      

            //*************************************************
            //Load Variables 
            //*************************************************
            Level = 1;
            Money = 1000;
            Lives = 10;
            Score = 0;
            //*************************************************
            //*************************************************

            
        }


        private void RegisterInput()
        {
            _mouseInput.registerCommand(MouseInput.MouseEvent.MouseDown, new InputDeviceHelper.CommandDelegatePosition(OnMouseDown));
            _mouseInput.registerCommand(MouseInput.MouseEvent.MouseUp, new InputDeviceHelper.CommandDelegatePosition(OnMouseUp));
            _mouseInput.registerCommand(MouseInput.MouseEvent.MouseMove, new InputDeviceHelper.CommandDelegatePosition(OnMouseMove));

            InputHandling.RegisterCommand(_playerStats.StartLevelKey, StartNextLevel, new KeyInformation(KeyTrigger.KEY_DOWN, "START LEVEL"));
            InputHandling.RegisterCommand(_playerStats.UpgradeKey, UpgradeTurretShortCut, new KeyInformation(KeyTrigger.KEY_DOWN, "UPGRADE"));
            InputHandling.RegisterCommand(_playerStats.SellKey, SellTurret, new KeyInformation(KeyTrigger.KEY_DOWN, "SELL"));

        }

        private void SellTurret(TimeSpan elapsedTime)
        {
            if (_selectedTurret != null)
            {
                int sellPrice = _selectedTurret.Price / 2;
                if (_selectedTurret.UpgradeLevel == 2)
                {
                    sellPrice += _selectedTurret.Upgrade1Price / 2;

                }
                if (_selectedTurret.UpgradeLevel == 3)
                {
                    sellPrice += _selectedTurret.Upgrade2Price / 2;
                }

                _mapGrid.OccupyPiece(_selectedTurret.XPos, _selectedTurret.YPos, false);
                PaintTurretRange(_selectedTurret, Color.White, _selectedTurret.XPos, _selectedTurret.YPos);
                _turretHandler.RemoveTurret(_selectedTurret.XPos, _selectedTurret.YPos);


                Money += sellPrice;
                var mapGridCoordinates = MapGrid.GetPosition(_selectedTurret.XPos, _selectedTurret.YPos);
                _textFloater.AddFloatingText((int)mapGridCoordinates.X, (int)mapGridCoordinates.Y - _selectedTurret.Textures[0].Height/2, "+" + sellPrice);
                ParticleSystem.TurretSold(new Vector2(mapGridCoordinates.X, mapGridCoordinates.Y) + new Vector2(-Settings.TowerDefenseSettings.GRID_X_LENGTH/4, -Settings.TowerDefenseSettings.GRID_Y_LENGTH / 4));
                _selectedTurret = null;
            }
        }

        private void UpgradeTurretShortCut(TimeSpan elapsedTime)
        {
            if (_selectedTurret != null)
            {
                UpgradeTurret(_selectedTurret);
            }
        }
        private void StartNextLevel(TimeSpan elapsedTime)
        {
            if (_spawner.LevelOver)
            {
                _spawner.StartLevel(Level);
                m_updateFunction = GameUpdate;
            }
        }
        private bool HotKeyClicked(int x, int y)
        {
         
            if (x >= sellStartX && x <= sellEndX  && y >= sellStartY && y <= sellEndY)
            {
                sellClicked = true;
                hotKeyClicked = true;
                return true;
            }
            else if (x >= upgradeStartX && x <= upgradeEndX && y >= upgradeStartY && y <= upgradeEndY)
            {
                upgradeClicked = true;
                hotKeyClicked = true;
                return true;
            }
            else if (x >= startLevelStartX && x <= startLevelEndX && y >= startLevelStartY && y <= startLevelEndY)
            {
                startLevelClicked = true;
                hotKeyClicked = true;
                return true;
            } 

            return false;
        }
        private void OnMouseDown(TimeSpan gameTime, int x, int y)
        {
            if (HotKeyClicked(x,y))
            {
                return;
            }

            if(GameOver == true)
            {
                GameOver = false;
                return;
            }
            var buyTurretSelected = GetBuyTurret(x, y);
            if (buyTurretSelected != null)
            {
                _buyTurret = buyTurretSelected;
                if (_selectedTurret != null)
                {
                    PaintTurretRange(_selectedTurret, Color.White, _selectedTurret.XPos, _selectedTurret.YPos);
                }
                _selectedTurret = null;

                return;
            }

            var mapGridCoordinates = MapGrid.GetXYFromCoordinates(x, y);
            var turret = _turretHandler.GetTurret(mapGridCoordinates.x, mapGridCoordinates.y);
            if (turret != null)
            {
                if (_selectedTurret != null)
                {
                    PaintTurretRange(_selectedTurret, Color.White, _selectedTurret.XPos, _selectedTurret.YPos);
                    if (_selectedTurret == turret)
                    {
                        UpgradeTurret(turret);
                    }
                }
                _selectedTurret = turret;
                PaintTurretRange(_selectedTurret, _rangeColor, mapGridCoordinates.x, mapGridCoordinates.y);
                return;
            }
            if (_selectedTurret != null)
            {
                PaintTurretRange(_selectedTurret, Color.White, _selectedTurret.XPos, _selectedTurret.YPos);
            }
            _selectedTurret = null;

            

        }

        private void UpgradeTurret(Turret turret)
        {
            if (turret.UpgradeLevel == 1)
            {
                if (Money >= turret.Upgrade1Price)
                {
                    turret.Updgrade1();
                    Money -= turret.Upgrade1Price;
                }
            }
            else if (turret.UpgradeLevel == 2)
            {
                if (Money >= turret.Upgrade2Price)
                {
                    turret.Upgrade2();
                    Money -= turret.Upgrade2Price;
                }
            }
        }

        private Turret GetBuyTurret(int x, int y)
        {
            var basicRectangle = TurretRectangleBuy(0, 10);
            var bombRectangle = TurretRectangleBuy(1, 10);
            var missleRectangle = TurretRectangleBuy(2, 10);
            var pelletTurret = TurretRectangleBuy(3, 10);
            if (x >= basicRectangle.X && x <= basicRectangle.X + basicRectangle.Width && y >= basicRectangle.Y && y <= basicRectangle.Y + basicRectangle.Height)
            {
                return new BasicTurret(_basicTurretTexture, _basicTurretTexture2, _basicTurretTexture3, _projectileTexture, _platformTexture, 0, 0, _projectileHandler, _spawner._enemies);
            }
            else if (x >= bombRectangle.X && x <= bombRectangle.X + bombRectangle.Width && y >= bombRectangle.Y && y <= bombRectangle.Y + bombRectangle.Height)
            {
                return new BombTurret(_bombTurretTexture, _bombTurretTexture2, _bombTurretTexture3, _bombProjectileTexture, _platformTexture, 0, 0, _projectileHandler, _spawner._enemies);
            }
            else if (x >= missleRectangle.X && x <= missleRectangle.X + missleRectangle.Width && y >= missleRectangle.Y && y <= missleRectangle.Y + missleRectangle.Height)
            {
                return new MissleTurret(_missleTurretTexture, _missleTurretTexture2, _missleTurretTexture3, _missleProjectileTexture, _platformTexture, 0, 0, _projectileHandler, _spawner._enemies);
            }
            else if (x >= pelletTurret.X && x <= pelletTurret.X + pelletTurret.Width && y >= pelletTurret.Y && y <= pelletTurret.Y + pelletTurret.Height)
            {
                return new PelletTurret(_pelletTurretTexture, _pelletTurretTexture2, _pelletTurretTexture3, _pelletProjectileTexture, _platformTexture, 0, 0, _projectileHandler, _spawner._enemies);
            }

            return null;
        }
        private void OnMouseUp(TimeSpan gameTime, int x, int y)
        {
            if (_buyTurret != null)
            {
                var mapGridCoordinates = MapGrid.GetXYFromCoordinates(x, y);

                if (_buyTurret.Price > Money)
                {
                    PaintTurretRange(_buyTurret, Color.White, mapGridCoordinates.x, mapGridCoordinates.y);
                    _buyTurret = null;

                    return;
                }


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
                        Money -= _buyTurret.Price;
                        _buyTurret.XPos = mapGridCoordinates.x;
                        _buyTurret.YPos = mapGridCoordinates.y;

                        _turretHandler.AddTurret(_buyTurret.Clone());
                    }

                }
                PaintTurretRange(_buyTurret, Color.White, mapGridCoordinates.x, mapGridCoordinates.y);
                _buyTurret = null;
            }
        }

        private void OnMouseMove(TimeSpan gameTime, int x, int y)
        {
            var mapGridCoordinates = MapGrid.GetXYFromCoordinates(x, y);
            if (MouseOn.x != mapGridCoordinates.x || MouseOn.y != mapGridCoordinates.y)
            {
                if (_selectedTurret == null || _mapGrid.GetHighlightColor(MouseOn.x, MouseOn.y) != _rangeColor)
                {
                    _mapGrid.HighlightPiece(MouseOn.x, MouseOn.y, Color.White);

                }
                if (_buyTurret != null)
                {
                    PaintTurretRange(_buyTurret, Color.White, MouseOn.x, MouseOn.y);
                }
                MouseOn = (mapGridCoordinates.x, mapGridCoordinates.y);

            }
            if (_buyTurret != null)
            {

                PaintTurretRange(_buyTurret, _placeColor, MouseOn.x, MouseOn.y);
            }
            else
            {
                if (_selectedTurret == null || _mapGrid.GetHighlightColor(mapGridCoordinates.x, mapGridCoordinates.y) != _rangeColor)
                {
                    _mapGrid.HighlightPiece(mapGridCoordinates.x, mapGridCoordinates.y, _hoverColor);
                }

            }
        }

        private void PaintTurretRange(Turret turret, Color color, int x, int y)
        {
            if (turret != null)
            {
                for (int i = x - turret.Range; i <= x + turret.Range; i++)
                {
                    for (int t = y - turret.Range; t <= y + turret.Range; t++)
                    {
                        _mapGrid.HighlightPiece(i, t, color);
                    }
                }
            }
        }


        private void LoadPlayerStats()
        {
            _playerStats = _persistentStorage.Load<PlayerStats>("DEFAULT");
            if (_playerStats == null)
            {
                _playerStats = new PlayerStats(new System.Collections.Generic.List<float>(), 0,Keys.S,Keys.U,Keys.G); ;
            }
        }
        private void SavePlayerStats()
        {
            _persistentStorage.Save("DEFAULT", _playerStats);
        }





        #region updateFunctions


        public void GameOverUpdate(TimeSpan elapsedTime)
        {
            _mouseInput.Update(elapsedTime);

            _turretHandler.Update(elapsedTime);
            _spawner.Update(elapsedTime);
            ParticleSystem.Update(elapsedTime);
            _textFloater.Update(elapsedTime);

            if (!GameOver)
            {
                ReloadGame();
                m_updateFunction = MapUpdate;
                GameOver = false;
            }
        }

        bool KeyUpdate()
        {
            if (hotKeyClicked)
            {
                var keys = Keyboard.GetState().GetPressedKeys();
                if(keys.Length == 0)
                {
                    return true;
                }

                var keyClicked = keys[0];
                if(_playerStats.SellKey == keyClicked || _playerStats.UpgradeKey == keyClicked || _playerStats.StartLevelKey == keyClicked)
                {
                    return true;
                }
                if(sellClicked)
                {
                    InputHandling.UpdateKey(_playerStats.SellKey, keyClicked);
                    _playerStats.SellKey = keyClicked;
                    
                }
                else if (upgradeClicked)
                {
                    
                    InputHandling.UpdateKey(_playerStats.UpgradeKey, keyClicked);
                    _playerStats.UpgradeKey = keyClicked;
                }
                else if (startLevelClicked)
                {
                   
                    InputHandling.UpdateKey(_playerStats.StartLevelKey, keyClicked);
                    _playerStats.StartLevelKey = keyClicked;
                }

                SavePlayerStats();
                hotKeyClicked = false;
                sellClicked = false;
                upgradeClicked = false;
                startLevelClicked = false;

                return false;
            }

            return false;

        }
        public void GameUpdate(TimeSpan elapsedTime)
        {
            if (KeyUpdate())
            {
                return;
            }
            
            InputHandling.Update(elapsedTime);
            _mouseInput.Update(elapsedTime);



            _turretHandler.Update(elapsedTime);
            _spawner.Update(elapsedTime);
            _projectileHandler.Update(elapsedTime);
            ParticleSystem.Update(elapsedTime);
            _textFloater.Update(elapsedTime);
            if (_spawner.LevelOver)
            {
                Level++;
                m_updateFunction = MapUpdate;
            }
        }

        public void MapUpdate(TimeSpan elapsedTime)
        {
            if (KeyUpdate())
            {
                return;
            }
            InputHandling.Update(elapsedTime);
            _mouseInput.Update(elapsedTime);

            _turretHandler.Update(elapsedTime);
            _projectileHandler.Update(elapsedTime);
            ParticleSystem.Update(elapsedTime);
            _textFloater.Update(elapsedTime);
        }
        public void Update(TimeSpan elapsedTime)
        {
            m_updateFunction(elapsedTime);
        }
        #endregion


        public void Render(GameTime gameTime, SpriteBatch graphics)
        {

            graphics.Begin();

            RenderMap(graphics);
            //render the particle system

            _mapGrid.Render(graphics);

            RenderMenu(graphics);
            _turretHandler.Draw(graphics, gameTime.ElapsedGameTime);
            _spawner.Draw(graphics, gameTime.ElapsedGameTime);
            //HACK: don't do this, make another render function.
            if(!GameOver)
            _projectileHandler.Draw(graphics, gameTime.ElapsedGameTime);
            RenderTurretStats(graphics);
            RenderStats(graphics, gameTime.ElapsedGameTime);
            ParticleSystem.Render(graphics, gameTime.ElapsedGameTime);
            _textFloater.Draw(graphics);

            if (GameOver)
            {
                DrawGameOver(graphics);
            }
            graphics.End();


        }

        public void RenderMenu(SpriteBatch graphics)
        {
            if (_buyTurret != null)
            {

                graphics.Draw(_buyTurret.Textures[0], MapGrid.GetPosition(MouseOn.x, MouseOn.y), null, Color.White, 0, new Vector2(_buyTurret.Textures[0].Width / 2, _buyTurret.Textures[0].Height / 2), Settings.SCALE, SpriteEffects.None, 0);

            }
        }


        #region RightMap


        private int startX = (int)(800 * Settings.SCALE.X);
        private int width = (int)(400 * Settings.SCALE.X);

        private int tileHeight = (int)(Settings.TowerDefenseSettings.TILE_SCALE.Y * Settings.SCALE.Y * Settings.TowerDefenseSettings.GRID_Y_LENGTH);

        private int[] yCoordinates;


        private void InitializeMapRenderCoordinates()
        {
            yCoordinates = new int[] { 0, tileHeight, (int)(600 * Settings.SCALE.Y) };

        }
        public void RenderMap(SpriteBatch graphics)
        {
            //draw the main background
            graphics.Draw(_mainBackground, new Rectangle(0, 0, Settings.GameSettings.WINDOW_WIDTH, Settings.GameSettings.WINDOW_HEIGHT), Color.White);
            //graphics.Draw(_lifeSensorGreen, HealthStatusRectangle(), Color.White);

            graphics.Draw(_infoBackground, TurretRectangleBuyScreen(), Color.White);
            graphics.Draw(_basicTurretTexture, TurretRectangleBuy(0, 10), Color.White);
            graphics.Draw(_bombTurretTexture, TurretRectangleBuy(1, 10), Color.White);
            graphics.Draw(_missleTurretTexture, TurretRectangleBuy(2, 10), Color.White);
            graphics.Draw(_pelletTurretTexture, TurretRectangleBuy(3, 10), Color.White);


            graphics.Draw(_infoBackground, StatsRectangleScreen(), Color.White);



        }


        public void DrawGameOver(SpriteBatch graphics)
        {
            
            string gameOverMessage = String.Format("{0}", "GAME OVER");
            Vector2 ySize = m_font.MeasureString(gameOverMessage) * 2;
            graphics.DrawString(m_font, gameOverMessage, new Vector2(Settings.GameSettings.WINDOW_WIDTH/2 - ySize.X/2,Settings.GameSettings.WINDOW_HEIGHT/2 - ySize.Y/2), Color.DarkRed, 0, new Vector2(0, 0), 2, SpriteEffects.None, 0);
            string click = "(CLICK TO RESTART)";
            Vector2 clickSize = m_font.MeasureString(click) * 1;
            graphics.DrawString(m_font, click, new Vector2(Settings.GameSettings.WINDOW_WIDTH / 2 - clickSize.X / 2, Settings.GameSettings.WINDOW_HEIGHT / 2 - clickSize.Y / 2 + 80), Color.DarkRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
        }
        public void RenderTurretStats(SpriteBatch graphics)
        {
            if (_selectedTurret != null)
            {
                var rectangle = TurretStatsPopUp(MapGrid.GetPosition(_selectedTurret.XPos, _selectedTurret.YPos));
                graphics.Draw(_infoBackground, rectangle, Color.White);
                int xOffset = 5;
                int yOffset = 5;

                int yAddOn = 15;

                float textScale = .2f;
                string name = String.Format("{0}", _selectedTurret.Name);
                graphics.DrawString(m_font, name, new Vector2(rectangle.X + xOffset, rectangle.Y + yOffset), Color.LimeGreen, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);

                string level = String.Format("{0} {1}", "LEVEL", _selectedTurret.UpgradeLevel);
                graphics.DrawString(m_font, level, new Vector2(rectangle.X + xOffset, rectangle.Y + yOffset + yAddOn), Color.Gray, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);

                int statSymbolOffSet = 10;
                int statSymbolYOffset = 2;
                string damage = String.Format("{0}", "DMG");
                graphics.DrawString(m_font, damage, new Vector2(rectangle.X + xOffset, rectangle.Y + yOffset + yAddOn * 2), Color.Gray, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);
                DrawTurretStatSymbols(graphics, rectangle.X + xOffset + statSymbolOffSet * 4, rectangle.Y + yOffset + yAddOn * 2 + statSymbolYOffset, statSymbolOffSet, _selectedTurret.Damage, Settings.TowerDefenseSettings.TURRET_DAMAGES);

                string rate = String.Format("{0}", "RATE");
                graphics.DrawString(m_font, rate, new Vector2(rectangle.X + xOffset, rectangle.Y + yOffset + yAddOn * 3), Color.Gray, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);
                DrawTurretStatSymbols(graphics, rectangle.X + xOffset + statSymbolOffSet * 4, rectangle.Y + yOffset + yAddOn * 3 + statSymbolYOffset, statSymbolOffSet, (int)_selectedTurret.ShootRate, Settings.TowerDefenseSettings.TURRET_FIRE_RATES);

                string range = String.Format("{0}", "RANGE");
                graphics.DrawString(m_font, range, new Vector2(rectangle.X + xOffset, rectangle.Y + yOffset + yAddOn * 4), Color.Gray, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);
                DrawTurretStatSymbols(graphics, rectangle.X + xOffset + statSymbolOffSet * 4, rectangle.Y + yOffset + yAddOn * 4 + statSymbolYOffset, statSymbolOffSet, _selectedTurret.Range, Settings.TowerDefenseSettings.TURRET_RANGES);

                string price = _selectedTurret.Upgrade1Price.ToString();
                if (_selectedTurret.UpgradeLevel == 2)
                {
                    price = _selectedTurret.Upgrade2Price.ToString();
                }
                else if (_selectedTurret.UpgradeLevel == 3)
                {
                    price = "MAX";
                }
                string upgrade = String.Format("{0} {1}", "UPGRADE", price);
                graphics.DrawString(m_font, upgrade, new Vector2(rectangle.X + xOffset, rectangle.Y + yOffset + yAddOn * 5), Color.Lime, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);

            }
        }

        private void DrawTurretStatSymbols(SpriteBatch graphics, int startX, int startY, int xOffset, int turretValue, int[] values)
        {
            bool filled = true;
            for (int x = 0; x < values.Length; x++)
            {
                if (!filled)
                {
                    graphics.Draw(_turretStatUnfilled, new Vector2(startX + x * xOffset, startY), Color.White);
                    continue;
                }
                graphics.Draw(_turretStatFilled, new Vector2(startX + x * xOffset, startY), Color.White);
                if (values[x] == turretValue)
                    filled = false;
            }
        }

        /// <summary>
        /// the turret actual position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Rectangle TurretStatsPopUp(Vector2 position)
        {
            return new Rectangle((int)position.X, (int)(position.Y - 90 * Settings.SCALE.Y), (int)(100 * Settings.SCALE.X), (int)(100 * Settings.SCALE.Y));
        }

        private Rectangle HealthStatusRectangle()
        {
            return new Rectangle(startX, yCoordinates[2], width, (int)(200 * Settings.SCALE.Y));
        }

        private Rectangle TurretRectangleBuy(int x, int xOffset)
        {
            xOffset = (x + 1) * xOffset;
            return new Rectangle(startX + (int)(x * Settings.TowerDefenseSettings.TILE_SCALE.X * tileHeight) + xOffset, yCoordinates[0], tileHeight, tileHeight);
        }
        private Rectangle TurretRectangleBuyScreen()
        {
            return new Rectangle(startX, yCoordinates[0], width, tileHeight);
        }


        private Rectangle StatsRectangleScreen()
        {
            return new Rectangle(startX, yCoordinates[1], width, (int)(tileHeight * 3.5));
        }

        private float sellStartX;
        private float sellEndX;
        private float sellStartY;
        private float sellEndY;

        private float upgradeStartX;
        private float upgradeEndX;
        private float upgradeStartY;
        private float upgradeEndY;

        private float startLevelStartX;
        private float startLevelEndX;
        private float startLevelStartY;
        private float startLevelEndY;

        private bool hotKeyClicked = false;

        private bool sellClicked = false;
        private bool upgradeClicked = false;
        private bool startLevelClicked = false;
        public void RenderStats(SpriteBatch graphics, TimeSpan elapsedTime)
        {
            float textScale = .3f;
            int yOffset = 10;
            int xOffset = 20;

            string moneyMessage = String.Format("{0} : {1,5}", "MONEY", Money);
            Vector2 ySize = m_font.MeasureString(moneyMessage) * textScale;
            graphics.DrawString(m_font, moneyMessage,new Vector2(startX + xOffset, yCoordinates[1] + 10), Color.Lime, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);

            string waveNumber = String.Format("{0} : {1,5}", "WAVE", Level);
            graphics.DrawString(m_font, waveNumber, new Vector2(startX + xOffset + 130, yCoordinates[1] + yOffset), Color.LightSteelBlue, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);



            string livesMessage = "LIVES:";
            
            graphics.DrawString(m_font, livesMessage,new Vector2(startX + xOffset, yCoordinates[1] + ySize.Y + yOffset), Color.LightGray, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);
            graphics.DrawString(m_font, Lives.ToString(), new Vector2(startX + 70 + xOffset, yCoordinates[1] + ySize.Y + yOffset), Color.Red, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);

            ySize += m_font.MeasureString(livesMessage) * textScale;
            ySize.Y += yOffset;

            ySize.Y += 14;

            
            string commandMessage = "CLICK COMMAND TO CHANGE KEY";
            if (hotKeyClicked == true)
            {
                commandMessage = "CLICK A KEY";
            }
            graphics.DrawString(m_font, commandMessage, new Vector2(startX + xOffset, yCoordinates[1] + ySize.Y + yOffset), Color.LightGray, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);
        
            ySize += m_font.MeasureString(commandMessage) * textScale;
            ySize.Y += yOffset;

            string sellMessage = "SELL:";
            sellStartX = startX + xOffset;
            sellStartY = yCoordinates[1] + ySize.Y + yOffset;
            graphics.DrawString(m_font, sellMessage, new Vector2(sellStartX, sellStartY ), Color.Yellow, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);
            graphics.DrawString(m_font, _playerStats.SellKey.ToString(), new Vector2(startX + 40 + xOffset, yCoordinates[1] + ySize.Y + yOffset), Color.Yellow, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);
           
            var sellMeasure = m_font.MeasureString(sellMessage) * textScale;
            sellEndY = sellStartY + sellMeasure.Y;
            sellEndX = sellMeasure.X + sellStartX;
            ySize += sellMeasure;
            ySize.Y += yOffset;

            
            string upgradeMessaage = "UPGRADE:";
            upgradeStartX = startX + xOffset;
            upgradeStartY = yCoordinates[1] + ySize.Y + yOffset;
            graphics.DrawString(m_font, upgradeMessaage, new Vector2(upgradeStartX, upgradeStartY), Color.Crimson, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);
            graphics.DrawString(m_font, _playerStats.UpgradeKey.ToString(), new Vector2(startX + 70 + xOffset, yCoordinates[1] + ySize.Y + yOffset), Color.Crimson, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);

            var upgradeMeasure = m_font.MeasureString(upgradeMessaage) * textScale;
            upgradeEndY = upgradeStartY + upgradeMeasure.Y;
            upgradeEndX = upgradeMeasure.X + upgradeStartX;
            ySize += upgradeMeasure;
            ySize.Y += yOffset;

            string startMessage = "START LEVEL:";
            startLevelStartX = startX + xOffset;
            startLevelStartY = yCoordinates[1] + ySize.Y + yOffset;
            graphics.DrawString(m_font, startMessage, new Vector2(startLevelStartX, startLevelStartY), Color.Green, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);
            graphics.DrawString(m_font, _playerStats.StartLevelKey.ToString(), new Vector2(startX + 95 + xOffset, yCoordinates[1] + ySize.Y + yOffset), Color.Green, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);

            var startMessageMeasure = m_font.MeasureString(startMessage) * textScale;
            startLevelEndY = startLevelStartY + startMessageMeasure.Y;
            startLevelEndX = startMessageMeasure.X + startLevelStartX;
            ySize += startMessageMeasure;
            ySize.Y += yOffset;
        }

        private Vector2 GetStatGridPos(int y)
        {
            
            return new Vector2(0,0);
        }

        public void EarnMoney(int money, Vector2 deathPosition, int topY)
        {
            this.Money += money;
            _textFloater.AddFloatingText((int)deathPosition.X, (int)deathPosition.Y - topY, "+"+money);
            ParticleSystem.CreepDeath(deathPosition);
            Score += money;
        }

        public void EnemyMadeIt()
        {
            Lives--;
            if(Lives < 0)
            {
                GameOver = true;
                _playerStats.AddScore(Score);
                if(Level > _playerStats.Level)
                {
                    _playerStats.Level = Level;
                }
                SavePlayerStats();
                m_updateFunction = GameOverUpdate;
            }
        }

        #endregion
    }
}
