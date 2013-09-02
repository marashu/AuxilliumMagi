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
    class CreditState : GameState
    {
        private Sprite titleSprite;
        private Sprite gameOverSprite;
        






        public CreditState(Game game)
            : base(game)
        {
            titleSprite = new Sprite(GameCore.PublicTitleTexture);
            gameOverSprite = new Sprite(GameCore.PublicCreditsTexture);

            gameOverSprite.Position = new Vector2(240, 170);
            

        }



        public override void Update(GameTime gameTime)
        {




            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch sBatch)
        {
            titleSprite.Draw(sBatch);
            gameOverSprite.Draw(sBatch);
            



            base.Draw(sBatch);
        }
    }
}
