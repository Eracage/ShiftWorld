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
        private Vector2 _movement = Vector2.Zero;
        private float _speed = 200;
        

        public Player(Texture2D texture)
        {
            _animator = new Animate(texture,10,256,256,12.5f);
            _position = new Vector2(50,50);
        }

        public void Update(KeyboardState keyboardState, GameTime gameTime, Vector2 cameraDelta)
        {
            _movement = cameraDelta;

            movement(keyboardState, gameTime);

            _animator.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _animator.Draw(spriteBatch, _position);
        }

        private void movement(KeyboardState keyboardState, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f;
            if (keyboardState.IsKeyDown(Keys.A)) // move left
                _movement.X -= _speed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.D)) // move right
                _movement.X += _speed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.W)) // move up
                _movement.Y -= _speed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.S)) // move down
                _movement.Y += _speed * deltaTime;

            _position += _movement;
        }
    }
}
