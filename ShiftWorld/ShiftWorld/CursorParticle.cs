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
    class CursorParticle : Particle
    {
        public CursorParticle(Texture2D texture, Vector2 position)
            : base(texture,position) 
        { }

        public void Update(GameTime gameTime)
        {
            _color.X = 1.0f;
            _color.Y = _life / _startingLife;
            _color.Z = _life / _startingLife;
            _color.W = _life / _startingLife;

            base.Update(gameTime);
        }
    }
}
