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
    class Animate
    {
        public Texture2D texture;
        int frames;
        int width;
        int height;
        float fps;

        float timer = 0;
        int currentFrame = 0;
        int firstFrame = 0;
        int counter = 0;
        bool transition = false;


        int memFirstFrame;
        int memStartingFrame;
        int memFrames;
        float memFPS;

        public Animate(Texture2D Texture, int Frames, int FrameWidth, int FrameHeight, float FPS, int FirstFrame = 1)
        {
            texture = Texture;
            frames = Frames;
            width = FrameWidth;
            height = FrameHeight;
            fps = FPS;
            firstFrame = FirstFrame-1;
            timer = 0;
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer > 1000 / fps)
            {
                currentFrame++;
                if (currentFrame >= firstFrame + frames)
                {
                    currentFrame = firstFrame;
                }
                timer -= 1000 / fps;
                counter++;
            }

            if (transition)
            {
                if (!(counter < frames))
                {
                    firstFrame = memFirstFrame;
                    frames = memFrames;
                    currentFrame = memStartingFrame;
                    fps = memFPS;
                    transition = false;
                }
            }
        }

        public void ChangeAnimation(int FirstFrame, int StartingFrame, int Frames, float FPS)
        {
            memFirstFrame = firstFrame = FirstFrame-1;
            memStartingFrame = currentFrame = StartingFrame-1;
            memFrames = frames = Frames;
            memFPS = fps = FPS;
            timer = 0;
        }

        public void AnimationTransition(int FirstFrame, int StartingFrame, int Frames, float FPS)
        { 
            firstFrame = FirstFrame-1;
            currentFrame = StartingFrame-1;
            frames = Frames;
            fps = FPS;
            timer = 0;
            counter = 0;
            transition = true;
        }

        public void Draw(SpriteBatch SB, Vector2 Position)
        {
            var currentFrameX = (currentFrame % (texture.Width / width)) * width;
            var currentFrameY = (int)((Math.Floor((double)currentFrame / ((double)texture.Width / (double)width))) * height);

            SB.Draw(texture, Position, new Rectangle(currentFrameX, currentFrameY, width, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f); // ,0f, new Vector2(width/2, height/2), SpriteEffects.None, 0f);
        }
    }
}
