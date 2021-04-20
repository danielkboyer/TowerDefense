using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TowerDefense.GamePlay
{
    public class AnimatedSprite
    {
        private Vector2 m_scale;

        private float m_currentRotation;
        private float m_startRotation;
        private float m_targetRotation;


        private List<Texture2D> m_spriteSheet;
        private int[] m_spriteTime;


        private TimeSpan m_animationTime;
        private int m_subImageIndex;

        /// <summary>
        /// the amount of times the target lerps towards the target position before reaching it
        /// </summary>
        private int m_currentUpdate = 0;

        private int m_currentRotUpdate = 0;

        private Vector2 m_currentPosition;
        private Vector2 m_startPosition;
        private Vector2 m_targetPosition;

        private float m_moveRate;
        private float m_moveSpeed;
        private TimeSpan m_moveTime;


        public Vector2 Position
        {
            get
            {
                return m_currentPosition;
            }
        }
        public AnimatedSprite(Vector2 scale, Vector2 center, Vector2 targetPosition, float rotation, float moveRate, List<Texture2D> spriteSheet, int[] spriteTime)
        {
            this.m_scale = scale;
            this.m_currentPosition = center;
            this.m_spriteSheet = spriteSheet;
            this.m_spriteTime = spriteTime;
            this.m_moveRate = moveRate / (float)Math.Sqrt(moveRate);
            this.m_moveSpeed = moveRate;
            this.m_startPosition = center;
            this.m_currentRotation = rotation;
            this.m_startRotation = rotation;
            UpdateTargetPosition(targetPosition);
            UpdateTargetRotation(rotation);
        }

        public void UpdateTargetPosition(Vector2 position)
        {

            this.m_targetPosition = position;
            this.m_startPosition = new Vector2(m_currentPosition.X, m_currentPosition.Y);


            m_currentUpdate = 0;
        }

        public void UpdateTargetRotation(float rotation)
        {

            this.m_targetRotation = rotation;


            this.m_startRotation = m_currentRotation;

            m_currentRotUpdate = 0;

        }

        public static float LerpDegrees(float start, float end, float amount)
        {
            float difference = Math.Abs(end - start);
            if (difference > 180)
            {
                // We need to add on to one of the values.
                if (end > start)
                {
                    // We'll add it on to start...
                    start += 360;
                }
                else
                {
                    // Add it on to end.
                    end += 360;
                }
            }

            // Interpolate it.
            float value = (start + ((end - start) * amount));

            // Wrap it..
            float rangeZero = 360;

            if (value >= 0 && value <= 360)
                return value;

            return (value % rangeZero);
        }

        public void update(TimeSpan elapsedTime)
        {
            m_animationTime += elapsedTime;
            if (m_animationTime.TotalMilliseconds >= m_spriteTime[m_subImageIndex])
            {
                m_animationTime -= TimeSpan.FromMilliseconds(m_spriteTime[m_subImageIndex]);
                m_subImageIndex++;
                m_subImageIndex = m_subImageIndex % m_spriteTime.Length;
            }

            m_moveTime += elapsedTime;
            if (m_moveTime.TotalMilliseconds >= m_moveRate)
            {

                //For movement
                float moveAmount = m_moveRate * m_currentUpdate / m_moveSpeed;
                m_currentPosition = Vector2.Lerp(m_startPosition, m_targetPosition, moveAmount);

                moveAmount = m_moveRate * m_currentRotUpdate / m_moveSpeed;
                if (moveAmount < 1)
                    m_currentRotation = MathHelper.ToRadians(LerpDegrees(MathHelper.ToDegrees(m_startRotation), MathHelper.ToDegrees(m_targetRotation), moveAmount));


                m_moveTime -= TimeSpan.FromMilliseconds(m_moveRate);
                m_currentRotUpdate++;
                m_currentUpdate++;

            }


        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_spriteSheet[m_subImageIndex], new Vector2(m_currentPosition.X, m_currentPosition.Y), null, Color.White, m_currentRotation, new Vector2(m_spriteSheet[m_subImageIndex].Width / 2, m_spriteSheet[m_subImageIndex].Height / 2), m_scale * Settings.SCALE, SpriteEffects.None, 0);


        }
    }
}
