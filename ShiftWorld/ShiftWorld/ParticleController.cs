using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FuncWorks.XNA.XTiled;
using System.Diagnostics;
using System.Media;
using System.Text;

namespace ShiftWorld
{

    class ParticleController
    {
        Texture2D _particleTexture;
        Vector2 _cameraPosition;
        List<Particle> Particles = new List<Particle>();

        float _timer = 0;

        public ParticleController(Texture2D particleTexture)
        {
            _particleTexture = particleTexture;
        }

        public void Update(GameTime gameTime, Vector2 cameraPosition, Vector2 MouseRtCamera)
        { 
            _cameraPosition = cameraPosition;

            _timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_timer < 0)
            {
                _timer += 10;
                Particles.Add(new CursorParticle(_particleTexture, cameraPosition + MouseRtCamera));
            }

            foreach (var particle in Particles)
            {
                particle.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var particle in Particles)
            {
                particle.Draw(spriteBatch);
            }
        }
    }
}
