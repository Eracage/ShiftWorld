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
        private Vector2 _velocity = Vector2.Zero;
        private float _speed = 100;
        private float _hp = 500;
        private float _jumpDelayms = 0;
        private bool _jumping = false;
        private bool _inAir = true;
        private bool _zenithReached = false;
        private bool _alive = true;

        public Player(Texture2D texture)
        {
            _animator = new Animate(texture, 10, 256, 256, 10f, 9, 0.5f);
            Reset();
        }

        public void Update(KeyboardState keyboardState, GameTime gameTime, Vector2 cameraDelta)
        {
            HP(2);

            if (!_alive)
                cameraDelta = Vector2.Zero;
            movement(keyboardState, gameTime, cameraDelta);

            if (keyboardState.IsKeyDown(Keys.K))
                _hp = -100;

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
                if (_alive)
                {
                    _animator.ChangeAnimation(9, 16, 10, 10f);
                    _animator.AnimationTransition(5, 5, 3, 15);
                }
            }
        }

        public void Die()
        {
            if (_alive)
            {
                _animator.ChangeAnimation(23, 23, 1, 1);
                _animator.AnimationTransition(20, 20, 4, 6);
                _alive = false;
            }
        }

        public void Fall()
        {
            _alive = false;
        }

        public void Reset()
        {
            _hp = 500;
            _jumpDelayms = 0;
            _jumping = false;
            _inAir = true;
            _zenithReached = false;
            _alive = true;
            
            _velocity = Vector2.Zero;

            _animator.ChangeAnimation(9, 9, 10, 10f);
            _position = new Vector2(160/0.6f, 640);
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

        private void movement(KeyboardState keyboardState, GameTime gameTime, Vector2 cameraDelta)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f;
            float gravitation = deltaTime * 30;
            _velocity.Y += gravitation;
            _velocity.X = cameraDelta.X;

            //if (_velocity.Y > gravitation)
            //{
            //    _inAir = true;
            //}
            if (_alive)
            {
                _jumpDelayms -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_jumpDelayms < 0 && _jumping)
                {
                    _inAir = true;
                    _velocity.Y -= 13f;
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
                            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Space)) // move up
                            {
                                _animator.ChangeAnimation(4, 4, 1, 0.1f);
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
                    _animator.AnimationTransition(19, 19, 1, 12f);
                    _zenithReached = true;
                }
                //if (keyboardState.IsKeyDown(Keys.S)) // move down
                //    _movement.Y += _speed * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.Divide))
            {
                _position = new Vector2(_position.X, 0);
                _velocity.Y = 0;
                _speed = 400;
            }
            else
            {
                _speed = 100;
            }
            _position += _velocity;
        }
    }
}
