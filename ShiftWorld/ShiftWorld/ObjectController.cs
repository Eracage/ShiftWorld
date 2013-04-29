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
        }

        public void AddObject(Resizable type, Vector2 position)
        {
            switch (type)
            {
                case Resizable.Nothing:
                    break;
                case Resizable.BarrelS:
                    RZObjects.Add(new Object(_barrelTexture, position, Size.Small));
                    break;
                case Resizable.BarrelM:
                    RZObjects.Add(new Object(_barrelTexture, position, Size.Medium));
                    break;
                case Resizable.BarrelL:
                    RZObjects.Add(new Object(_barrelTexture, position, Size.Large));
                    break;
                default:
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var o in RZObjects)
            {
                o.Update(gameTime);
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
