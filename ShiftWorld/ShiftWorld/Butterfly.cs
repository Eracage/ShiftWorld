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
using System.Text;

namespace ShiftWorld
{
    class Butterfly
    {
        private Animate _animator;
        private Vector2 _position;


        public Butterfly(Texture2D texture)
        {
            _animator = new Animate(texture, 10, 128, 128, 10f, 1, 0.55f, 0.75f);
        }

        public void Update(Vector2 CameraPosition, GameTime gameTime)
        {
            _position = CameraPosition + new Vector2(500, -500);

            _animator.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _animator.Draw(spriteBatch, _position);
        }
    }
}
