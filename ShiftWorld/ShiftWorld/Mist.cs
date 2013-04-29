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
        public Vector2 _position = new Vector2(0);
        Vector2 _movement = new Vector2(-100,0);
        float _zoom;

        public Mist(Texture2D texture, float zoom)
        {
            _texture = texture;
            _zoom = zoom;
            _position = Vector2.Zero;
        }

        public void Update(GameTime gameTime, Vector2 CameraPosition)
        {
            _position += new Vector2(_movement.X * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f, _movement.Y * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);
            //_position = CameraPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 5; i++)
            {
                spriteBatch.Draw(_texture, _position + new Vector2(2*i * 1280 / _zoom, 0), null, Color.White, 0, Vector2.Zero, (float)(1 / _zoom), SpriteEffects.None, 0);
                spriteBatch.Draw(_texture, _position + new Vector2((2*i+1) * 1280 / _zoom, 0), null, Color.White, 0, Vector2.Zero, (float)(1 / _zoom), SpriteEffects.FlipHorizontally, 0);
            }
        }
    }
}
