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
    class Player
    {
        private Texture2D _texture;
        private Vector2 _position;
        private Animate _animator;

        public Player(Texture2D texture)
        {
            _animator = new Animate(texture,1,256,256,2);
            _texture = texture;
            _position = new Vector2(50,50);
        }

        public void Update(KeyboardState keyboardState, GameTime gameTime, Vector2 cameraDelta)
        {
            _position += cameraDelta;
            if (keyboardState.IsKeyDown(Keys.A)) // move left
                _position.X -= 1;
            if (keyboardState.IsKeyDown(Keys.D)) // move right
                _position.X += 1;
            if (keyboardState.IsKeyDown(Keys.W)) // move up
                _position.Y -= 1;
            if (keyboardState.IsKeyDown(Keys.S)) // move down
                _position.Y += 1;

            _animator.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _animator.Draw(spriteBatch, _position);
        }
    }
}
