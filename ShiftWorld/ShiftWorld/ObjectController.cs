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
    class ObjectController
    {
        Texture2D _barrelTexture;
        public List<Object> RZObjects = new List<Object>();

        public ObjectController(Texture2D barrelTexture)
        {
            _barrelTexture = barrelTexture;

            RZObjects.Add(new Object(_barrelTexture, new Vector2(800, 800)));
        }

        public void AddObject(int type, Vector2 position)
        {
            if (type == 1)
            {
                RZObjects.Add(new Object(_barrelTexture, position));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var o in RZObjects)
            {
                o.Draw(spriteBatch);
            }
        }

        public void Reset()
        {
            for (int i = (RZObjects.Count - 1); i >= 0; --i)
            {
                RZObjects.RemoveAt(i);
            }
        }
    }
}
