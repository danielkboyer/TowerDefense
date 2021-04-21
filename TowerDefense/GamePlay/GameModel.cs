using LunarLanding.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using TowerDefense.GamePlay.Turrets;
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
    public class GameModel:IEnemyNotification
    {

        private PersistentStorage _persistentStorage;


        Action<TimeSpan> m_updateFunction;
        private ParticleSystem _particleSystem;

        private SpriteFont m_font;

        private (int x, int y) MouseOn;

        #region Input

        MouseInput _mouseInput;

        #endregion
        #region Player

        private PlayerStats _playerStats;

        private int Money;

        private int Lives;

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

        private Texture2D _projectileTexture;

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
            _projectileTexture = content.Load<Texture2D>("Sprites/thrustParticle");
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
            _particleSystem = new ParticleSystem(content);
            //grid
            _mapGrid = new MapGrid();
            _mapGrid.LoadContent(content);
            _mapGrid.Generate();
            //BFS
            _shortestPath = new ShortestPath();
            //Spawner
            _spawner = new Spawner(content,_mapGrid,_shortestPath,this);
            //Projectile Factory
            _projectileHandler = new ProjectileHandler(_spawner._enemies);
            //Turret Factory
            _turretHandler = new TurretHandler(_spawner._enemies);
            //Mouse Input
            _mouseInput = new MouseInput();
            RegisterInput();
            m_updateFunction = MapUpdate;
            //*************************************************
            //*************************************************



            //*************************************************
            //Load Variables 
            //*************************************************
            Level = 1;
            Money = 1000;
            Lives = 10;
            //*************************************************
            //*************************************************

            InitializeMapRenderCoordinates();

        }

        

        private void RegisterInput()
        {
            _mouseInput.registerCommand(MouseInput.MouseEvent.MouseDown, new InputDeviceHelper.CommandDelegatePosition(OnMouseDown));
            _mouseInput.registerCommand(MouseInput.MouseEvent.MouseUp, new InputDeviceHelper.CommandDelegatePosition(OnMouseUp));
            _mouseInput.registerCommand(MouseInput.MouseEvent.MouseMove, new InputDeviceHelper.CommandDelegatePosition(OnMouseMove));

            InputHandling.RegisterCommand(Microsoft.Xna.Framework.Input.Keys.G, StartNextLevel,new KeyInformation(KeyTrigger.KEY_DOWN,"START LEVEL"));
     
        }

        private void StartNextLevel(TimeSpan elapsedTime)
        {
            if (_spawner.LevelOver)
            {
                _spawner.StartLevel(Level);
                m_updateFunction = GameUpdate;
            }
        }

        private void OnMouseDown(TimeSpan gameTime, int x, int y)
        {
            var buyTurretSelected = GetBuyTurret(x, y);
            if(buyTurretSelected != null)
            {
                _buyTurret = buyTurretSelected;
                if (_selectedTurret != null)
                {
                    PaintTurretRange(_selectedTurret,Color.White, _selectedTurret.XPos, _selectedTurret.YPos);
                }
                _selectedTurret = null;
                
                return;
            }

            var mapGridCoordinates = MapGrid.GetXYFromCoordinates(x, y);
            var turret = _turretHandler.GetTurret(mapGridCoordinates.x, mapGridCoordinates.y);
            if(turret != null)
            {
                if(_selectedTurret != null)
                {
                    PaintTurretRange(_selectedTurret, Color.White, _selectedTurret.XPos, _selectedTurret.YPos);
                    if(_selectedTurret == turret)
                    {
                        UpdgradeTurret(turret);
                    }
                }
                _selectedTurret = turret;
                PaintTurretRange(_selectedTurret,_rangeColor, mapGridCoordinates.x, mapGridCoordinates.y);
                return;
            }
            if (_selectedTurret != null)
            {
                PaintTurretRange(_selectedTurret,Color.White, _selectedTurret.XPos, _selectedTurret.YPos);
            }
            _selectedTurret = null;

        }

        private void UpdgradeTurret(Turret turret)
        {
            if(turret.UpgradeLevel == 1)
            {
                if(Money >= turret.Upgrade1Price)
                {
                    turret.Updgrade1();
                    Money -= turret.Upgrade1Price;
                }
            }
            else if(turret.UpgradeLevel == 2)
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
            if(x >= basicRectangle.X && x <= basicRectangle.X + basicRectangle.Width && y >= basicRectangle.Y && y <= basicRectangle.Y + basicRectangle.Height)
            {
                return new BasicTurret(_basicTurretTexture, _basicTurretTexture2,_basicTurretTexture3, _projectileTexture, _platformTexture, 0, 0, _projectileHandler, _spawner._enemies);
            }
            else if (x >= bombRectangle.X && x <= bombRectangle.X + bombRectangle.Width && y >= bombRectangle.Y && y <= bombRectangle.Y + bombRectangle.Height)
            {
                return new BombTurret(_bombTurretTexture, _bombTurretTexture2, _bombTurretTexture3, _projectileTexture, _platformTexture, 0, 0, _projectileHandler, _spawner._enemies);
            }
            else if (x >= missleRectangle.X && x <= missleRectangle.X + missleRectangle.Width && y >= missleRectangle.Y && y <= missleRectangle.Y + missleRectangle.Height)
            {
                return new MissleTurret(_missleTurretTexture, _missleTurretTexture2, _missleTurretTexture3, _projectileTexture, _platformTexture, 0, 0, _projectileHandler, _spawner._enemies);
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
            if(MouseOn.x != mapGridCoordinates.x || MouseOn.y != mapGridCoordinates.y)
            {
                if (_selectedTurret == null || _mapGrid.GetHighlightColor(MouseOn.x, MouseOn.y) != _rangeColor)
                {
                    _mapGrid.HighlightPiece(MouseOn.x, MouseOn.y, Color.White);

                }
                if(_buyTurret != null)
                {
                    PaintTurretRange(_buyTurret, Color.White,MouseOn.x,MouseOn.y);
                }
                MouseOn = (mapGridCoordinates.x, mapGridCoordinates.y);
                
            }
            if (_buyTurret != null)
            {

                PaintTurretRange(_buyTurret,_placeColor,MouseOn.x,MouseOn.y);
            }
            else
            {
                if(_selectedTurret == null || _mapGrid.GetHighlightColor(mapGridCoordinates.x,mapGridCoordinates.y) != _rangeColor)
                {
                    _mapGrid.HighlightPiece(mapGridCoordinates.x, mapGridCoordinates.y, _hoverColor);
                }
                
            }
        }

        private void PaintTurretRange(Turret turret,Color color, int x, int y)
        {
            if(turret != null)
            {
                for(int i = x- turret.Range;i<= x + turret.Range; i++)
                {
                    for (int t = y - turret.Range;t <= y + turret.Range; t++)
                    {
                        _mapGrid.HighlightPiece(i, t, color);
                    }
                }
            }
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





        #region updateFunctions

        public void GameUpdate(TimeSpan elapsedTime)
        {
            InputHandling.Update(elapsedTime);
            _mouseInput.Update(elapsedTime);
            
            
           
            _turretHandler.Update(elapsedTime);
            _spawner.Update(elapsedTime);
            _projectileHandler.Update(elapsedTime);
            _particleSystem.Update(elapsedTime);

            if (_spawner.LevelOver)
            {
                Level++;
                m_updateFunction = MapUpdate;
            }
        }

        public void MapUpdate(TimeSpan elapsedTime)
        {
            InputHandling.Update(elapsedTime);
            _mouseInput.Update(elapsedTime);
            
            _turretHandler.Update(elapsedTime);
            _projectileHandler.Update(elapsedTime);
            _particleSystem.Update(elapsedTime);

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
            _projectileHandler.Draw(graphics, gameTime.ElapsedGameTime);
            RenderTurretStats(graphics);
            RenderStats(graphics, gameTime.ElapsedGameTime);
            _particleSystem.Render(graphics, gameTime.ElapsedGameTime);
            graphics.End();


        }

        public void RenderMenu(SpriteBatch graphics)
        {
            if(_buyTurret != null)
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
            yCoordinates = new int[]{ 0,tileHeight, (int)(600 * Settings.SCALE.Y) };
            
        }
        public void RenderMap(SpriteBatch graphics)
        {
            //draw the main background
            graphics.Draw(_mainBackground, new Rectangle(0, 0, Settings.GameSettings.WINDOW_WIDTH, Settings.GameSettings.WINDOW_HEIGHT), Color.White);
            graphics.Draw(_lifeSensorGreen, HealthStatusRectangle(), Color.White);

            graphics.Draw(_infoBackground, TurretRectangleBuyScreen(), Color.White);
            graphics.Draw(_basicTurretTexture, TurretRectangleBuy(0,10),Color.White);
            graphics.Draw(_bombTurretTexture, TurretRectangleBuy(1, 10), Color.White);
            graphics.Draw(_missleTurretTexture, TurretRectangleBuy(2, 10), Color.White);


            graphics.Draw(_infoBackground, StatsRectangleScreen(), Color.White);

            

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
                graphics.DrawString(m_font, name, new Vector2(rectangle.X + xOffset,rectangle.Y + yOffset), Color.LimeGreen, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);

                string level = String.Format("{0} {1}", "LEVEL",_selectedTurret.UpgradeLevel);
                graphics.DrawString(m_font, level, new Vector2(rectangle.X + xOffset, rectangle.Y + yOffset + yAddOn), Color.Gray, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);

                int statSymbolOffSet = 10;
                int statSymbolYOffset = 2;
                string damage = String.Format("{0}", "DMG");
                graphics.DrawString(m_font, damage, new Vector2(rectangle.X + xOffset, rectangle.Y + yOffset + yAddOn*2), Color.Gray, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);
                DrawTurretStatSymbols(graphics, rectangle.X + xOffset + statSymbolOffSet*4, rectangle.Y + yOffset + yAddOn * 2 + statSymbolYOffset, statSymbolOffSet, _selectedTurret.Damage, Settings.TowerDefenseSettings.TURRET_DAMAGES);

                string rate = String.Format("{0}", "RATE");
                graphics.DrawString(m_font, rate, new Vector2(rectangle.X + xOffset, rectangle.Y + yOffset + yAddOn * 3), Color.Gray, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);
                DrawTurretStatSymbols(graphics, rectangle.X + xOffset + statSymbolOffSet * 4, rectangle.Y + yOffset + yAddOn * 3 + statSymbolYOffset, statSymbolOffSet, (int)_selectedTurret.ShootRate, Settings.TowerDefenseSettings.TURRET_FIRE_RATES);

                string range = String.Format("{0}", "RANGE");
                graphics.DrawString(m_font, range, new Vector2(rectangle.X + xOffset, rectangle.Y + yOffset + yAddOn * 4), Color.Gray, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);
                DrawTurretStatSymbols(graphics, rectangle.X + xOffset + statSymbolOffSet * 4, rectangle.Y + yOffset + yAddOn * 4 + statSymbolYOffset, statSymbolOffSet, _selectedTurret.Range, Settings.TowerDefenseSettings.TURRET_RANGES);

                string price = _selectedTurret.Upgrade1Price.ToString();
                if(_selectedTurret.UpgradeLevel == 2)
                {
                    price = _selectedTurret.Upgrade2Price.ToString();
                }
                else if(_selectedTurret.UpgradeLevel == 3)
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
            for(int x = 0; x < values.Length; x++)
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
            return new Rectangle(startX, yCoordinates[2],width, (int)(200 * Settings.SCALE.Y));
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
            return new Rectangle(startX, yCoordinates[1], width, tileHeight*3);
        }

        public void RenderStats(SpriteBatch graphics, TimeSpan elapsedTime)
        {
            string moneyMessage = String.Format("{0} : {1,5}", "MONEY", Money);
            Vector2 moneyMessageSize = m_font.MeasureString(moneyMessage);
            graphics.DrawString(m_font, moneyMessage,
                new Vector2(startX + 20, yCoordinates[1] + 10), Color.White, 0, new Vector2(0, 0), .5f, SpriteEffects.None, 0);

            string livesMessage = String.Format("{0,-6}{1,-3}{2,-9}", "LIVES", ":", Lives);
            Vector2 livesMessageSize = m_font.MeasureString(livesMessage);
            graphics.DrawString(m_font, livesMessage,
                new Vector2(startX + 20, yCoordinates[1] + moneyMessageSize.Y + 10), Color.White,0,new Vector2(0,0),.5f,SpriteEffects.None,0);

        }

        public void EarnMoney(int money)
        {
            this.Money += money;
        }

        public void EnemyMadeIt()
        {
            Lives--;
        }

        #endregion
    }
}
