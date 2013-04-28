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
        List<CursorParticle> _cursor = new List<CursorParticle>();
        List<BeamParticle> _beam = new List<BeamParticle>();
        

        //float _timer = 0;

        public ParticleController(Texture2D particleTexture)
        {
            _particleTexture = particleTexture;
        }

        public void Update(GameTime gameTime, Vector2 cameraPosition, Vector2 MouseRtCamera)
        { 
            _cameraPosition = cameraPosition;

            //_timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            //if (_timer < 0)
            //{
            //    _timer += 50;
                _cursor.Add(new CursorParticle(_particleTexture, MouseRtCamera, Vector2.Zero));
            //}
            
            for (int i = (_cursor.Count - 1); i >= 0; --i )
            {
                if (_cursor[i].Update(gameTime))
                    _cursor.RemoveAt(i);
            }

            for (int i = (_beam.Count - 1); i >= 0; --i)
            {
                if (_beam[i].Update(gameTime))
                    _beam.RemoveAt(i);
            }
        }

        public void AddBeam(Vector2 position, Vector2 direction, bool minimalism)
        {
            _beam.Add(new BeamParticle(_particleTexture, position, direction, minimalism));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var particle in _cursor)
            {
                particle.Draw(spriteBatch);
            }

            foreach (var particle in _beam)
            {
                particle.Draw(spriteBatch);
            }
        }

        public void Reset()
        {
            for (int i = (_beam.Count - 1); i >= 0; --i)
            {
                _beam.RemoveAt(i);
            }
        }
    }
}
