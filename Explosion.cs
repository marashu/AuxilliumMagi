using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AuxilliumMagi
{
    public class Explosion
    {
        private AnimatedSprite mySprite;
        public AnimatedSprite MySprite { get { return mySprite; } set { mySprite = value; } }
        private TimeSpan myTimeSpan = TimeSpan.Zero;
        private bool bCanEnd = false;
        public bool CanEnd { get { return bCanEnd; } }
        public Explosion()
        {
            mySprite = new AnimatedSprite(GameCore.PublicExplosionTexture);
            mySprite.Animate(0, 1);
            if(GameCore.PlaySoundEffects)
                GameCore.PublicSFXBoomSound.Play(0.5f,0,0);
        }
        public void Update(GameTime gameTime)
        {
            mySprite.Update(gameTime);
            myTimeSpan += gameTime.ElapsedGameTime;
            if (myTimeSpan > TimeSpan.FromMilliseconds(200))
                bCanEnd = true;
        }
        public void Draw(SpriteBatch sBatch)
        {
            mySprite.Draw(sBatch);
        }
    }
}
