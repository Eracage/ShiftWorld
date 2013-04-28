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
    class BeamParticle : Particle
    {
        bool _minimalism;
        public BeamParticle(Texture2D texture, Vector2 position, Vector2 direction, bool minimalism)
            : base(texture, position, direction, 4000f,0.0f, 500)
        {
            _scale = 0.0f;
            _minimalism = minimalism;
        }

        public override bool Update(GameTime gameTime)
        {
            if (_minimalism)
            {
                _color.X = 0.2f; //Red
                _color.Y = 1.0f; //Green
                _color.Z = 0.2f; //Blue
                _color.W = 1.0f; //Alpha
            }
            else
            {
                _color.X = 0.2f; //Red
                _color.Y = 0.2f; //Green
                _color.Z = 1.0f; //Blue
                _color.W = 1.0f; //Alpha
            }
            _scale = (1 - (_life / _startingLife)) * 10;
            _depth += 0.0000001f;

            return base.Update(gameTime);
        }
    }
}
