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
    class Mist
    {
        Texture2D _texture;
        Vector2 _position = new Vector2(0);
        Vector2 _movement = new Vector2(-100,0);
        float _zoom;

        public Mist(Texture2D texture, float zoom)
        {
            _texture = texture;
            _zoom = zoom;
            _position = new Vector2(1280 / zoom, 768 / zoom);
        }

        public void Update(GameTime gameTime, Vector2 CameraPosition)
        {
            _position += new Vector2(_movement.X * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f, _movement.Y * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);
            //_position = CameraPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture,_position, null, Color.Black, 0, new Vector2(_texture.Width,_texture.Height), (float)(1/_zoom),SpriteEffects.None, 0);
        }
    }
}
