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

        public Player(Texture2D texture)
        {
            _texture = texture;
            _position = new Vector2(50,50);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture,_position, Color.White);
        }
    }
}
