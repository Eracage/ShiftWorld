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
    class Object
    {
        Texture2D _texture;
        Vector2 _position;
        float _size;
        float _delayedSize;

        public Object(Texture2D texture, Vector2 position, Size size)
        {
            _texture = texture;
            _position = position;
            _delayedSize = _size = (float)size;
        }

        public void ChangeSize(bool minimalise)
        {
            if (minimalise)
	        {
                _size /= 2;
                if (_size<1)
	            {
                    _size=1;
	            }
	        }
            else
            {
                _size *= 2;
                if (_size > 4)
                {
                    _size = 4;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            _delayedSize += (_size - _delayedSize)*((float)gameTime.ElapsedGameTime.TotalMilliseconds/2000) ;
        }

        public Rectangle Bounds()
        {
            return new Rectangle((int)(_position.X - _texture.Width * _delayedSize / 8f), (int)(_position.Y - _texture.Height * _delayedSize / 4f), (int)(_texture.Width*_delayedSize/4f), (int)(_texture.Height*_delayedSize/4f));
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(_texture,
                new Rectangle((int)(_position.X - _texture.Width * _delayedSize / 8f), (int)(_position.Y - _texture.Height * _delayedSize / 4f), (int)(_texture.Width * _delayedSize / 4f), (int)(_texture.Height * _delayedSize/4f)), 
                null, Color.White, 0, 
                Vector2.Zero, 
                SpriteEffects.None, 0.0f);
        }
    }
}
