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
        float _size = 1.0f;

        public Object(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
        }

        public void Update(GameTime gameTime)
        { }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(_texture.Width, _texture.Height), _size, SpriteEffects.None, 0);
        }
    }
}
