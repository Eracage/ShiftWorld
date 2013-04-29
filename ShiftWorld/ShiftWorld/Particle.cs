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
        protected float _scale = 1.0f;
        protected float _life;
        protected float _depth;
        protected float _startingLife;
        protected float _speed;
        protected Vector2 _direction;
        protected Vector2 _position;
        protected Vector4 _color = new Vector4(1.0f);

        public Particle(Texture2D texture, Vector2 position, Vector2 direction, float life = 500, float depth = 0.0f, float speed = 2.0f)
        {
            _depth = depth;
            _texture = texture;
            _position = position;
            _startingLife = _life = life;
            _speed = speed;
            _direction = direction;
        }

        public virtual bool Update(GameTime gameTime)
        {

            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            _position.X += dt * _speed * _direction.X;
            _position.Y += dt * _speed *_direction.Y;

            _life -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            return (_life < 0);
        }

        public float Life
        {
            set { _life = value; }
            get { return _life; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture,_position, null, new Color(_color),0, new Vector2(_texture.Width/2),_scale/4,SpriteEffects.None, _depth);
        }
    }
}
