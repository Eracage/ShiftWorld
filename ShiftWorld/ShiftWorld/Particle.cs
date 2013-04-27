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
        protected Vector2 _position = Vector2.Zero;
        protected Vector4 _color = new Vector4(1.0f);

        public Particle(Texture2D texture, Vector2 position, float life = 500, float depth = 0.0f)
        {
            _depth = depth;
            _texture = texture;
            _position = position;
            _startingLife = _life = life;
        }

        public virtual bool Update(GameTime gameTime)
        {
            _life -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            return (_life < 0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture,_position, null, new Color(_color),0, new Vector2(_texture.Width/2),_scale,SpriteEffects.None, _depth);
        }
    }
}
