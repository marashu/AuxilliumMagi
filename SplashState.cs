using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AuxilliumMagi
{
    class SplashState : GameState
    {
        private Sprite titleSprite;
        
        private Sprite startSprite;

        private Sprite FASSprite;
     

        
        


        public SplashState(Game game)
            : base(game)
        {
            titleSprite = new Sprite(GameCore.PublicSplashTexture);
            //startSprite = "PRESS START";

            startSprite = new Sprite(GameCore.PublicLogoTexture);
            startSprite.Position = new Vector2(520, 160);

            FASSprite = new Sprite(GameCore.PublicFASTexture);
            FASSprite.Position = new Vector2(startSprite.Position.X + startSprite.Size.X + 50 + (FASSprite.Size.X / 2), 500);
            //startPosition = new Vector2(530, 450);

        }

     

        public override void Update(GameTime gameTime)
        {
           
    


            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch sBatch)
        {
            titleSprite.Draw(sBatch);
            startSprite.Draw(sBatch);
            FASSprite.Draw(sBatch);
            //sBatch.DrawString(GameCore.PublicFontTitle, startSprite, startPosition, Color.Red);
            



            base.Draw(sBatch);
        }
    }
}
