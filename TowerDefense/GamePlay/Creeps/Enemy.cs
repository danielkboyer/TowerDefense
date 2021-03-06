using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TowerDefense.GamePlay.Sound;
using TowerDefense.Grid;
using static TowerDefense.Grid.ShortestPath;

namespace TowerDefense.GamePlay
{
    public abstract class Enemy
    {
        protected AnimatedSprite _animatedSprite;
        public bool Alive { get; private set; } = true;
        protected int Speed { get; set; }

        protected double CurrentSpeedTime { get; set; }
        public int Health { get; set; }

        public int CurrentHealth { get; set; }

        public int AwardAmount { get; set; }
        public int CurrentX { get; set; }
        public int CurrentY { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public bool GotToEnd;

        protected Texture2D _greenHealthBar;
        protected Texture2D _redHealthBar;
        public Vector2 Position
        {
            get
            {
                return _animatedSprite.Position;
            }
        }
        

        public bool CanFly { get; protected set; }

        protected List<Texture2D> _textures;

        protected int gridIndex;
        public List<GridPos> gridPositions;
        public void Update(TimeSpan elapsedTime)
        {
            CurrentSpeedTime += elapsedTime.TotalMilliseconds;
            if(CurrentSpeedTime >= Speed)
            {
                CurrentSpeedTime -= Speed;
                Move();
            }

            _animatedSprite.update(elapsedTime);
        }
        public void UpdatePath(List<GridPos> gridPositions, int gridIndex)
        {
            this.gridPositions = gridPositions;
            this.gridIndex = gridIndex;
        }
        public virtual void Draw(SpriteBatch graphics,TimeSpan elapsedTime)
        {
            Vector2 middleBarPosition = new Vector2(Position.X, Position.Y - _textures[0].Height / 2 - 15);
            Vector2 greenOrigin = new Vector2(_greenHealthBar.Width / 2, _greenHealthBar.Height / 2);
            Vector2 greenScale = Settings.TowerDefenseSettings.LIFE_BAR_SCALE * Settings.SCALE;


            graphics.Draw(_greenHealthBar, middleBarPosition, null, Color.White, 0, greenOrigin, greenScale, SpriteEffects.None, 0);
            float Percentage = 1 - ((float)CurrentHealth )/ ((float)Health);

            Vector2 endBarPosition = new Vector2(middleBarPosition.X + (greenScale.X * _greenHealthBar.Width/2), middleBarPosition.Y);
            graphics.Draw(_redHealthBar, endBarPosition, null, Color.White, 0, new Vector2(_redHealthBar.Width, _redHealthBar.Height/2), new Vector2(Settings.TowerDefenseSettings.LIFE_BAR_SCALE.X * Percentage, Settings.TowerDefenseSettings.LIFE_BAR_SCALE.Y) * Settings.SCALE, SpriteEffects.None, 0);

            _animatedSprite.draw(graphics);
        }

        /// <summary>
        /// Gets called once the enemy should move to the next spot
        /// </summary>
        public virtual void Move()
        {
            gridIndex++;


            if (gridIndex < gridPositions.Count)
            {

                if (gridIndex - 1 >= 0)
                {

                    int xDirection = gridPositions[gridIndex].XPos - gridPositions[gridIndex - 1].XPos;
                    int yDirection = gridPositions[gridIndex].YPos - gridPositions[gridIndex - 1].YPos;

                    if((CurrentX == 0 && CurrentY == 0) || (CurrentX != xDirection || CurrentY !=yDirection))
                    {
                        CurrentX = xDirection;
                        CurrentY = yDirection;

                        if (xDirection == -1)
                        {
                            _animatedSprite.UpdateTargetRotation(MathHelper.ToRadians(180));
                        }
                        else if (xDirection == 1)
                        {
                            _animatedSprite.UpdateTargetRotation(MathHelper.ToRadians(0));
                        }
                        else if (yDirection == -1)
                        {
                            _animatedSprite.UpdateTargetRotation(MathHelper.ToRadians(270));
                        }
                        else if (yDirection == 1)
                        {
                            _animatedSprite.UpdateTargetRotation(MathHelper.ToRadians(90));
                        }
                    }
                  
                }

                _animatedSprite.UpdateTargetPosition(MapGrid.GetPosition(gridPositions[gridIndex].XPos, gridPositions[gridIndex].YPos));

            }
            else
            {
                GotToEnd = true;
                Die();
            }
        }

        /// <summary>
        /// Called when the enemy has died
        /// </summary>
        public virtual void Die()
        {
            Alive = false;
        }
        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            if(CurrentHealth <= 0)
            {
                SoundManager.CreepDeath();
                Die();
            }
        }

        /// <summary>
        /// clones the current enemy into a new one
        /// </summary>
        /// <returns></returns>
        public abstract Enemy Copy(List<GridPos> shortestPath);

        
    }
}
