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
        private Animate _animator;
        private Vector2 _position;
        private Vector2 _cameraPos;
        private Vector2 _velocity = Vector2.Zero;
        private float _speed = 100;
        private float _hp = 500;
        private float _jumpDelayms = 0;
        private bool _jumping = false;
        private bool _inAir = true;
        private bool _zenithReached = false;


        

        public Player(Texture2D texture)
        {
            _animator = new Animate(texture,10,256,256,10f,9);
            _position = new Vector2(10,10);
        }

        public void Update(KeyboardState keyboardState, GameTime gameTime, Vector2 cameraDelta, Vector2 cameraPosition)
        {
            _cameraPos = cameraPosition;

            movement(keyboardState, gameTime);

            _animator.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _animator.Draw(spriteBatch, _position);
        }

        public void Land()
        {
            _velocity.Y = 0;
            if (_inAir)
            {
                _inAir = false;
                _animator.ChangeAnimation(9, 16, 10, 10f);
                _animator.AnimationTransition(5, 5, 3, 15);
            }
        }

        // Getters setters

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector2 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public float HP(float change = 0.0f)
        {
            _hp += change;
            if (_hp > 500) _hp = 500;
            return _hp;
        }

        public float Height
        {
            get { return 256; }
        }

        // Private functions

        private void movement(KeyboardState keyboardState, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f;
            float gravitation = deltaTime * 30;
            _velocity.Y += gravitation;
            _velocity.X = deltaTime * 180;

            //if (_velocity.Y > gravitation)
            //{
            //    _inAir = true;
            //}

            _jumpDelayms -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_jumpDelayms < 0 && _jumping)
            {
                _inAir = true;
                _velocity.Y -= 14f;
                _zenithReached = false;
                _jumping = false;
            }

            if (keyboardState.IsKeyDown(Keys.A)) // move left
                _velocity.X -= _speed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.D)) // move right
                _velocity.X += _speed * deltaTime;
            if (!_inAir)
            {
                if (_velocity.Y == gravitation)
                {
                    if (!_jumping)
                    {
                        if (keyboardState.IsKeyDown(Keys.W)) // move up
                        {
                            _animator.ChangeAnimation(1, 1, 1, 1);
                            _animator.AnimationTransition(1, 1, 4, 15);
                            _jumpDelayms = 120;
                            _jumping = true;
                        }
                    }
                }
            }
            else if (!_zenithReached && _velocity.Y > 0)
            {
                _animator.ChangeAnimation(5, 5, 1, 1);
            }
            //if (keyboardState.IsKeyDown(Keys.S)) // move down
            //    _movement.Y += _speed * deltaTime;

            _position += _velocity;
        }
    }
}
