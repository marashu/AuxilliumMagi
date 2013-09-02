//A class for images that do not animate

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
    public class Sprite
    {

        //The image to be drawn
        private Texture2D myTexture = null;
        private Rectangle myTarget;
        //the x/y co-ordinates for the top left point of the image
        protected Vector2 position;
        protected float alpha = 1;
        public float MyAlpha { get { return alpha; } set { alpha = value; } }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        protected float rotation;
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        private Vector2 size;
        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }
        protected Vector2 pivot;
        public Vector2 Pivot { get { return pivot; } set { pivot = value; } }

        public Vector2 Center { get { return new Vector2(Position.X + (Size.X * iSizeModX / 2), Position.Y + (Size.Y* iSizeModY / 2)); } }

        private Color myColour = Color.White;
        public Color MyColour { get { return myColour; } set { myColour = value; } }

        protected float iSizeModX = 1;
        protected float iSizeModY = 1;

        public float SizeModX { get { return iSizeModX; } set { iSizeModX = value; } }
        public float SizeModY { get { return iSizeModY; } set { iSizeModY = value; } }

        protected SpriteEffects myEffect = SpriteEffects.None;
        public SpriteEffects Effect { get { return myEffect; } set { myEffect = value; } }


        public Sprite()
        {
            position = Vector2.Zero;
            myTarget = Rectangle.Empty;
        }

        public Sprite(Texture2D texture)
        {
            myTexture = texture;
            position = Vector2.Zero;
            size = new Vector2(texture.Width, texture.Height);
            myTarget = new Rectangle(0, 0, (int)(size.X), (int)(size.Y));
        }



        public virtual void Draw(SpriteBatch sBatch)
        {
            sBatch.Draw(myTexture, new Rectangle((int)(position.X), (int)(position.Y), (int)(size.X * iSizeModX), (int)(size.Y * iSizeModY)), myTarget, myColour * alpha, rotation,pivot,SpriteEffects.None, 0);
        }
    }
}
