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
    class InstructionState : GameState
    {
        private Sprite titleSprite;

        private Sprite gameOverSprite;
        //private string scoreSprite;

        //private Vector2 scorePosition;

        //private static bool bTwoPlayers = false;
        //public static bool TwoPlayers { get { return bTwoPlayers; } set { bTwoPlayers = value; } }

        //private static String sPlayerWon = "";
        //public static String PlayerWon { get { return sPlayerWon; } set { sPlayerWon = value; } }



        public InstructionState(Game game)
            : base(game)
        {
            titleSprite = new Sprite(GameCore.PublicTitleTexture);
            gameOverSprite = new Sprite(GameCore.PublicInstructionTexture);
            gameOverSprite.Position = new Vector2(180, 136);
            //scoreSprite = "Your score was: ";

            //scorePosition = new Vector2(530, 550);

        }



        public override void Update(GameTime gameTime)
        {




            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch sBatch)
        {
            titleSprite.Draw(sBatch);
            gameOverSprite.Draw(sBatch);
            //if (ActionState.Level < 0 && !bTwoPlayers)
            //    sBatch.DrawString(GameCore.Pericles, scoreSprite + ActionState.Score.ToString(), scorePosition, Color.Red);
            //else if (bTwoPlayers)
            //    sBatch.DrawString(GameCore.Pericles, sPlayerWon, scorePosition, Color.Red);


            base.Draw(sBatch);
        }
    }
}
