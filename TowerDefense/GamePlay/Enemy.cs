using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
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

        public int CurrentX { get; set; }
        public int CurrentY { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }
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
            Health -= damage;
            if(Health <= 0)
            {
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
