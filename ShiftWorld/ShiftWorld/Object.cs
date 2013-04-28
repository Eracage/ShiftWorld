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
        float _sizeX = 4.0f;
        float _sizeY = 4.0f;

        public Object(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
        }

        public void Update(GameTime gameTime)
        { }

        public virtual void ChangeSize(bool minimalise)
        {
        }

        public Rectangle Bounds()
        {
            return new Rectangle((int)(_position.X - _texture.Width * _sizeX / 8), (int)(_position.Y - _texture.Height * _sizeY / 8), (int)(_texture.Width*_sizeX/4), (int)(_texture.Height*_sizeY/4));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, 
                new Rectangle((int)(0), (int)(0), (int)(_texture.Width * _sizeX / 4), (int)(_texture.Height * _sizeY / 4)), 
                null, Color.White, 0, 
                new Vector2(_texture.Width * _sizeX / 8, _texture.Height * _sizeY / 4) - _position, 
                SpriteEffects.None, 0.0f);
        }
    }
}
