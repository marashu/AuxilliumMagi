using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.GamerServices;

namespace AuxilliumMagi
{
    class LevelWinState : GameState
    {
        private Sprite titleSprite;
        private Sprite gameOverSprite;
        private string scoreSprite;

        private Vector2 scorePosition;






        public LevelWinState(Game game)
            : base(game)
        {
            titleSprite = new Sprite(GameCore.PublicTitleTexture);
            gameOverSprite = new Sprite(GameCore.PublicLevelWinTexture);
            gameOverSprite.Position = new Vector2(465, 236);
            scoreSprite = "Purchase Full Game To Continue.";

            scorePosition = new Vector2(530, 550);

        }



        public override void Update(GameTime gameTime)
        {




            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch sBatch)
        {
            titleSprite.Draw(sBatch);
            gameOverSprite.Draw(sBatch);
#if(XBOX360)
                    if (Guide.IsTrialMode && ActionState.Level == 4)
                    {

                        sBatch.DrawString(GameCore.Pericles, scoreSprite, scorePosition, Color.Red);
                    }
                   
#endif
            //if (ActionState.Level > 1)
            //    sBatch.DrawString(GameCore.Pericles, scoreSprite + ActionState.Score.ToString(), scorePosition, Color.Red);



            base.Draw(sBatch);
        }
    }
}
