using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace LunarLanding.Particles
{
    public class ParticleSystem
    {
        private ThrustParticles _thrustParticleSystem;

        private ShipDestroyedParticles _shipDestroyParticleSystem;

        public ParticleSystem(ContentManager content)
        {
            //, TimeSpan rate, int sourceX, int sourceY, int size, int speed, TimeSpan lifetime, TimeSpan switchover
            //_thrustParticleSystem = new ThrustParticles(content,(int)(100*Settings.SCALE.Y),new TimeSpan(0,0,0,1,0), (int)(5 * Settings.SCALE.X));

            //, TimeSpan rate, int sourceX, int sourceY, int size, int speed, TimeSpan lifetime, TimeSpan switchover
            //_shipDestroyParticleSystem = new ShipDestroyedParticles(content,1500,250,3000,(int)(25*Settings.SCALE.X), (int)(200 * Settings.SCALE.X), (int)(400 * Settings.SCALE.X), (int)(5 *Settings.SCALE.X));

        }

        public void ShipThrust(Vector2 center,float rotation, float radius,float gasLeft)
        {
            rotation = rotation + MathF.PI / 2;
            var thruster = center + new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * radius;
            _thrustParticleSystem.ShipThrust(thruster, rotation,gasLeft);
        }

        public void ShipDestroy(Vector2 center)
        {
            _shipDestroyParticleSystem.ShipDestory(center);
        }

        public void Render(SpriteBatch spriteBatch,TimeSpan elapsedTime)
        {
            //_thrustParticleSystem.Draw(spriteBatch);
            //_shipDestroyParticleSystem.Draw(spriteBatch);
            //_counterParticleSystem.Draw(spriteBatch);
        }
        public void Update(TimeSpan elapsedTime)
        {
            //_thrustParticleSystem.Update(elapsedTime);
            //_shipDestroyParticleSystem.Update(elapsedTime);
            //_counterParticleSystem.Update(elapsedTime);
        }
    }
}
