//A class for all animating images
//Inherits from Sprite

//File created by Michael C. A. Patoine
//Created Tuesday, January 19, 2010

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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace AuxilliumMagi
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class AnimatedSprite : Sprite
    {
        //an array of textures, to form animation
        private Texture2D[] myTexture = null;

        //frames of animation.  number between 0 and 255
        private byte iStartFrame = 0;
        private byte iEndFrame = 0;

      

        private Rectangle myTarget { get { return new Rectangle(0, 0, (int)(Size.X), (int)(Size.Y)); } }

        private Vector2 size;
        public Vector2 Size
        {
            get { return new Vector2(myTexture[iCurrentFrame].Width, myTexture[iCurrentFrame].Height); }

        }
        public Vector2 Center { get { return new Vector2(Position.X + (Size.X * iSizeModX / 2), Position.Y + (Size.Y * iSizeModY / 2)); } }
        

        /// <summary>
        /// /////////////////////////ONLY FOR THIS GAME
        /// </summary>
        public Texture2D[] MyTexture { get { return myTexture; } set { myTexture = value; } }

        private byte iCurrentFrame = 0;
        public byte CurrentFrame
        {
            get { return iCurrentFrame; }
        }

        //frame delay, in milliseconds
        private const byte FRAMEDELAY = 100;

        private float frameMod = 1;
        public float FrameMod
        {
            get { return frameMod; }
            set { frameMod = MathHelper.Clamp(value, 0.5f, 2.0f); }
        }
        //a timer
        private TimeSpan elapsedTime = TimeSpan.Zero;

        public int Length
        {
            get { return myTexture.Length; }
        }



        public AnimatedSprite(Texture2D[] texture)
            : base()
        {
            myTexture = texture;
        }



        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            
            if (iStartFrame < iEndFrame)
            {
                elapsedTime += gameTime.ElapsedGameTime;
                if (elapsedTime > TimeSpan.FromMilliseconds(FRAMEDELAY * FrameMod))
                {
                    elapsedTime -= TimeSpan.FromMilliseconds(FRAMEDELAY * FrameMod);
                    iCurrentFrame++;
                    if (iCurrentFrame > iEndFrame)
                        iCurrentFrame = iStartFrame;
                    if (iCurrentFrame > iStartFrame)
                    {
                        position = new Vector2(position.X + (myTexture[iCurrentFrame - 1].Width / 2) - (myTexture[iCurrentFrame].Width / 2), position.Y + myTexture[iCurrentFrame - 1].Height - myTexture[iCurrentFrame].Height);
                    }
                    else
                    {
                        position = new Vector2(position.X + (myTexture[iEndFrame].Width / 2) - (myTexture[iCurrentFrame].Width / 2), position.Y + myTexture[iEndFrame].Height - myTexture[iCurrentFrame].Height);
                    }
                }//end if frame delay
            }//end if counting up
            else if (iStartFrame > iEndFrame)
            {
                elapsedTime += gameTime.ElapsedGameTime;
                if (elapsedTime > TimeSpan.FromMilliseconds(FRAMEDELAY * FrameMod))
                {
                    elapsedTime -= TimeSpan.FromMilliseconds(FRAMEDELAY * FrameMod);
                    iCurrentFrame--;
                    if (iCurrentFrame < iEndFrame)
                        iCurrentFrame = iStartFrame;
                    if (iCurrentFrame < iStartFrame)
                    {
                        position = new Vector2(position.X + (myTexture[iCurrentFrame + 1].Width / 2) - (myTexture[iCurrentFrame].Width / 2), position.Y + myTexture[iCurrentFrame + 1].Height - myTexture[iCurrentFrame].Height);
                    }
                    else
                    {
                        position = new Vector2(position.X + (myTexture[iEndFrame].Width / 2) - (myTexture[iCurrentFrame].Width / 2), position.Y + myTexture[iEndFrame].Height - myTexture[iCurrentFrame].Height);
                    }
                }//end if frame delay
            }//end if counting down
        }//end update

        public override void Draw(SpriteBatch sBatch)
        {
            sBatch.Draw(myTexture[iCurrentFrame], new Rectangle((int)(position.X), (int)(position.Y), (int)(Size.X * iSizeModX), (int)(Size.Y * iSizeModY)), myTarget, MyColour * alpha, rotation, pivot, myEffect, 0);
        }


        public void Animate(byte frame)
        {
            position = new Vector2(position.X + (myTexture[iCurrentFrame].Width / 2) - (myTexture[frame].Width / 2), position.Y + myTexture[iCurrentFrame].Height - myTexture[frame].Height);
            iStartFrame = frame;
            iEndFrame = frame;
            iCurrentFrame = frame;
            elapsedTime = TimeSpan.Zero;
        }

        public void Animate(byte startFrame, byte endFrame)
        {
            iStartFrame = startFrame;
            iEndFrame = endFrame;
            position = new Vector2(position.X + (myTexture[iCurrentFrame].Width / 2) - (myTexture[iStartFrame].Width / 2), position.Y + myTexture[iCurrentFrame].Height - myTexture[iStartFrame].Height);
            iCurrentFrame = iStartFrame;
            elapsedTime = TimeSpan.Zero;
        }
    }
}
