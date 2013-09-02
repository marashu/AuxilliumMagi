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
    class HighScoreState : GameState
    {
        private Sprite titleSprite;
        private static String[] highScores;

        private Sprite gameOverSprite;
        //private string scoreSprite;

        //private Vector2 scorePosition;

        //private static bool bTwoPlayers = false;
        //public static bool TwoPlayers { get { return bTwoPlayers; } set { bTwoPlayers = value; } }

        //private static String sPlayerWon = "";
        //public static String PlayerWon { get { return sPlayerWon; } set { sPlayerWon = value; } }



        public HighScoreState(Game game)
            : base(game)
        {
            titleSprite = new Sprite(GameCore.PublicTitleTexture);
            gameOverSprite = new Sprite(GameCore.PublicHighScoresTexture);
            gameOverSprite.Position = new Vector2(180, 136);
            //scoreSprite = "Your score was: ";
            highScores = new String[10];
            //scorePosition = new Vector2(530, 550);

        }



        public override void Update(GameTime gameTime)
        {




            base.Update(gameTime);
        }

        public static void GetHighScoreString()
        {
//#if(XBOX360)
            if (SaveManager.MyDevice.IsConnected)
            {
//#endif
                HighScoreData data = GameCore.LoadHighScores(GameCore.HighScoresFilename);

                highScores[0] = "Score: " + data.Score[0].ToString() + " on " + data.Times[0].ToString();
                highScores[1] = "Score: " + data.Score[1].ToString() + " on " + data.Times[1].ToString();
                highScores[2] = "Score: " + data.Score[2].ToString() + " on " + data.Times[2].ToString();
                highScores[3] = "Score: " + data.Score[3].ToString() + " on " + data.Times[3].ToString();
                highScores[4] = "Score: " + data.Score[4].ToString() + " on " + data.Times[4].ToString();
                highScores[5] = "Score: " + data.Score[5].ToString() + " on " + data.Times[5].ToString();
                highScores[6] = "Score: " + data.Score[6].ToString() + " on " + data.Times[6].ToString();
                highScores[7] = "Score: " + data.Score[7].ToString() + " on " + data.Times[7].ToString();
                highScores[8] = "Score: " + data.Score[8].ToString() + " on " + data.Times[8].ToString();
                highScores[9] = "Score: " + data.Score[9].ToString() + " on " + data.Times[9].ToString();
//#if(XBOX360)
            }
//#endif
        }
        public override void Draw(SpriteBatch sBatch)
        {
            titleSprite.Draw(sBatch);
            gameOverSprite.Draw(sBatch);
#if(XBOX360)
            if (Guide.IsTrialMode)
                sBatch.DrawString(GameCore.Pericles, "BUY FULL VERSION TO STORE HIGH SCORES", new Vector2(260, 353), Color.White);

            else
            {
                if (SaveManager.MyDevice.IsConnected)
                {
#endif
                    sBatch.DrawString(GameCore.Pericles, highScores[0], new Vector2(260, 220), Color.White);
                    sBatch.DrawString(GameCore.Pericles, highScores[1], new Vector2(260, 253), Color.White);
                    sBatch.DrawString(GameCore.Pericles, highScores[2], new Vector2(260, 287), Color.White);
                    sBatch.DrawString(GameCore.Pericles, highScores[3], new Vector2(260, 320), Color.White);
                    sBatch.DrawString(GameCore.Pericles, highScores[4], new Vector2(260, 353), Color.White);
                    sBatch.DrawString(GameCore.Pericles, highScores[5], new Vector2(260, 387), Color.White);
                    sBatch.DrawString(GameCore.Pericles, highScores[6], new Vector2(260, 420), Color.White);
                    sBatch.DrawString(GameCore.Pericles, highScores[7], new Vector2(260, 453), Color.White);
                    sBatch.DrawString(GameCore.Pericles, highScores[8], new Vector2(260, 487), Color.White);
                    sBatch.DrawString(GameCore.Pericles, highScores[9], new Vector2(260, 520), Color.White);
#if(XBOX360)
                }
                else
                {
                    sBatch.DrawString(GameCore.Pericles, "STORAGE DEVICE WAS DISCONNECTED", new Vector2(260, 353), Color.White);
                }
            }
#endif
            //if (ActionState.Level < 0 && !bTwoPlayers)
            //    sBatch.DrawString(GameCore.Pericles, scoreSprite + ActionState.Score.ToString(), scorePosition, Color.Red);
            //else if (bTwoPlayers)
            //    sBatch.DrawString(GameCore.Pericles, sPlayerWon, scorePosition, Color.Red);


            base.Draw(sBatch);
        }
    }
}
