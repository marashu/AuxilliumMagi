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
using System.Xml.Serialization;

namespace AuxilliumMagi
{
    class GameOverState : GameState
    {
        private Sprite titleSprite;

        private Sprite gameOverSprite;
        private string scoreSprite;

        private Vector2 scorePosition;
        private String ScoreString;

        private static bool bTwoPlayers = false;
        public static bool TwoPlayers { get { return bTwoPlayers; } set { bTwoPlayers = value; } }

        private static String sPlayerWon = "";
        public static String PlayerWon { get { return sPlayerWon; } set { sPlayerWon = value; } }

        private String NoDeviceString = "No device connected.";
        private bool noDevice = false;

        private bool bNewHighScore;



        public GameOverState(Game game)
            : base(game)
        {
            titleSprite = new Sprite(GameCore.PublicTitleTexture);
            gameOverSprite = new Sprite(GameCore.PublicGameOverTexture);
            gameOverSprite.Position = new Vector2(465, 236);
            scoreSprite = "Your score was: ";

            scorePosition = new Vector2(530, 550);

        }

        public void CheckScore()
        {
           

            ScoreString = "Score: " + ActionState.Score.ToString();
#if(XBOX360)
            if (!Guide.IsTrialMode)
            {
                if (SaveManager.MyDevice.IsConnected)
                {
                    noDevice = false;
                    SaveHighScore();
                }
                else
                {
                    noDevice = true;
                    bNewHighScore = false;
                }
            }
#else
            SaveHighScore();
#endif
            //handle checking and saving high score here
        }

        private void SaveHighScore()
        {
            // Create the data to save
            HighScoreData data = GameCore.LoadHighScores(GameCore.HighScoresFilename);

            int scoreIndex = -1;
            for (int i = 0; i < data.Count; i++)
            {
                if (ActionState.Score > data.Score[i])
                {
                    scoreIndex = i;
                    break;
                }
            }

            if (scoreIndex > -1)
            {
                //New high score found ... do swaps
                for (int i = data.Count - 1; i > scoreIndex; i--)
                {
                    data.Times[i] = data.Times[i - 1];
                    data.Score[i] = data.Score[i - 1];

                }

                data.Times[scoreIndex] = System.DateTime.Now;
                data.Score[scoreIndex] = ActionState.Score;


                GameCore.SaveHighScores(data, GameCore.HighScoresFilename);
                bNewHighScore = true;
                HighScoreState.GetHighScoreString();
            }
            else
            {
                bNewHighScore = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            



            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch sBatch)
        {
            titleSprite.Draw(sBatch);
            gameOverSprite.Draw(sBatch);
            if (ActionState.Level < 0 && !bTwoPlayers)
            {
                sBatch.DrawString(GameCore.Pericles, scoreSprite + ActionState.Score.ToString(), scorePosition, Color.Red);
                if (bNewHighScore)
                {
                    sBatch.DrawString(GameCore.Pericles, "New high score!", scorePosition + new Vector2(0, 30), Color.Red);
                }
            }
            else if (bTwoPlayers)
                sBatch.DrawString(GameCore.Pericles, sPlayerWon, scorePosition, Color.Red);


            base.Draw(sBatch);
        }
    }
}
