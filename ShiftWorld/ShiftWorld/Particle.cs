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
    class Particle
    {
        Texture2D _texture;
        float _life;

        public Particle(Texture2D texture)
        {
            _texture = texture;
            _life = 500;
        }

        public void Update(GameTime gameTime)
        {
            _life -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public float Life
        {
            get { return _life; }
        }

        public void Draw()
        {

        }
    }
}
