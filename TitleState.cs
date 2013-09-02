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
    class TitleState : GameState
    {
        private Sprite titleSprite;
        private Sprite titleBG;
        //private string creditSprite;
        private Texture2D instructSprite;
        private Texture2D startSprite;
        private Texture2D scoreSprite;
        private Texture2D enduranceSprite;
        private Texture2D twoPlayersSprite;
        private Texture2D optionsSprite;
        private Texture2D buySprite;

        private Color startColour;
        private Color enduranceColour;
        private Color twoPlayersColour;
        private Color instructColour;
        private Color scoresColour;
        private Color optionsColour;
        private Color buyColour;

        //private Vector2 creditPosition;
        private Vector2 instructPosition;
        private Vector2 startPosition;
        private Vector2 scorePosition;
        private Vector2 endurancePosition;
        private Vector2 twoPlayersPosition;
        private Vector2 optionsPosition;
        private Vector2 buyPosition;

        //private Color creditColour = Color.White;
        //private Color instructColour = Color.White;
        //private Color startColour = Color.White;
        //private Color scoreColour = Color.White;
        //private Color enduranceColour = Color.White;

        private TimeSpan elapsedTime = TimeSpan.Zero;
        private bool moved = false;
        private const int MOVEDELAY = 250;
        


        public enum Selections
        {
            Start,
            Scores,
            TwoPlayers,
            Instructions,
            Endurance,
            Options,
            Buy
        }

        private static Selections currentSelection;

        public static Selections CurrentSelection
        {
            get { return currentSelection; }
        }


        public TitleState(Game game)
            : base(game)
        {
            titleSprite = new Sprite(GameCore.PublicTitleTexture);
            titleBG = new Sprite(GameCore.PublicTitleBG);
            titleBG.Position = new Vector2(330, 170);

            twoPlayersSprite = GameCore.PublicTwoPlayersButtonTexture;
            twoPlayersPosition = new Vector2(628, 300);

            instructSprite = GameCore.PublicInstructionsButtonTexture;
            instructPosition = new Vector2(628, 350);
            startSprite = GameCore.PublicStoryButtonTexture;
            startPosition = new Vector2(670, 180);
            enduranceSprite = GameCore.PublicEnduranceButtonTexture;
            endurancePosition = new Vector2(640, 250);
            scoreSprite = GameCore.PublicHighScoresButtonTexture;
            scorePosition = new Vector2(645, 400);
            optionsSprite = GameCore.PublicOptionsButtonTexture;
            optionsPosition = new Vector2(660, 450);

            buySprite = GameCore.PublicBuyButtonTexture;
            buyPosition = new Vector2(628, 500);
            
            currentSelection = Selections.Start;

            startColour = Color.Yellow;
            enduranceColour = Color.White;
            twoPlayersColour = Color.White;
            instructColour = Color.White;
            scoresColour = Color.White;
            optionsColour = Color.White;
            buyColour = Color.White;
        }

        public void Reset()
        {
            currentSelection = Selections.Start;
            titleBG.Position = new Vector2(330, 170);
            twoPlayersPosition = new Vector2(628, 300);
            
            instructPosition = new Vector2(628, 350);
            
            startPosition = new Vector2(670, 170);
            
            endurancePosition = new Vector2(640, 250);
            
            scorePosition = new Vector2(645, 400);
            
            optionsPosition = new Vector2(660, 450);

            buyPosition = new Vector2(650, 500);

            startColour = Color.Yellow;
            enduranceColour = Color.White;
            twoPlayersColour = Color.White;
            instructColour = Color.White;
            scoresColour = Color.White;
            optionsColour = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            GamePadState gamePadState2 = GamePad.GetState(PlayerIndex.Two);
            GamePadState gamePadState3 = GamePad.GetState(PlayerIndex.Three);
            GamePadState gamePadState4 = GamePad.GetState(PlayerIndex.Four);
            if (!Guide.IsTrialMode && currentSelection == Selections.Buy)
            {
                currentSelection = Selections.Start;
                startPosition -= new Vector2(10, 10);
                startSprite = GameCore.PublicStoryButtonActiveTexture;
                startColour = Color.Yellow;
            }
            if (moved)
            {
                elapsedTime += gameTime.ElapsedGameTime;
                if (elapsedTime > TimeSpan.FromMilliseconds(MOVEDELAY))
                {
                    elapsedTime = TimeSpan.Zero;
                    moved = false;
                }
            }
            else if(!Guide.IsVisible)
            {
                if (((gamePadState.ThumbSticks.Left.Y > 0 || gamePadState.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState2.ThumbSticks.Left.Y > 0 || gamePadState2.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState3.ThumbSticks.Left.Y > 0 || gamePadState3.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState4.ThumbSticks.Left.Y > 0 || gamePadState4.DPad.Up == ButtonState.Pressed)) &&
                    currentSelection == Selections.Start)
                {
                    if (Guide.IsTrialMode)
                    {
                        currentSelection = Selections.Buy;
                        startPosition += new Vector2(10, 10);
                        buyPosition -= new Vector2(10, 10);

                        startSprite = GameCore.PublicStoryButtonTexture;
                        buySprite = GameCore.PublicBuyButtonActiveTexture;
                        startColour = Color.White;
                        buyColour = Color.Yellow;
                    }
                    else
                    {
                        currentSelection = Selections.Options;
                        startPosition += new Vector2(10, 10);
                        optionsPosition -= new Vector2(10, 10);

                        startSprite = GameCore.PublicStoryButtonTexture;
                        optionsSprite = GameCore.PublicOptionsButtonActiveTexture;
                        startColour = Color.White;
                        optionsColour = Color.Yellow;
                    }
                    moved = true;
                }
                else if (((gamePadState.ThumbSticks.Left.Y < 0 || gamePadState.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState2.ThumbSticks.Left.Y < 0 || gamePadState2.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState3.ThumbSticks.Left.Y < 0 || gamePadState3.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState4.ThumbSticks.Left.Y < 0 || gamePadState4.DPad.Down == ButtonState.Pressed)) &&
                    currentSelection == Selections.Start)
                {
                    if (!Guide.IsTrialMode)
                    {
                        currentSelection = Selections.Endurance;
                        endurancePosition -= new Vector2(10, 10);
                        enduranceSprite = GameCore.PublicEnduranceButtonActiveTexture;
                        enduranceColour = Color.Yellow;
                    }
                    else
                    {
                        currentSelection = Selections.Instructions;
                        instructPosition -= new Vector2(10, 10);
                        instructSprite = GameCore.PublicInstructionsButtonActiveTexture;
                        instructColour = Color.Yellow;
                    }
                    startPosition += new Vector2(10, 10);
                    startSprite = GameCore.PublicStoryButtonTexture;
                    startColour = Color.White;
                    

                    moved = true;
                }
                else if (((gamePadState.ThumbSticks.Left.Y > 0 || gamePadState.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState2.ThumbSticks.Left.Y > 0 || gamePadState2.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState3.ThumbSticks.Left.Y > 0 || gamePadState3.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState4.ThumbSticks.Left.Y > 0 || gamePadState4.DPad.Up == ButtonState.Pressed)) &&
                    currentSelection == Selections.Endurance)
                {
                    currentSelection = Selections.Start;
                    startPosition -= new Vector2(10, 10);
                    endurancePosition += new Vector2(10, 10);

                    startSprite = GameCore.PublicStoryButtonActiveTexture;
                    enduranceSprite = GameCore.PublicEnduranceButtonTexture;
                    startColour = Color.Yellow;
                    enduranceColour = Color.White;
                    moved = true;
                }
                    /*
                else if (((gamePadState.ThumbSticks.Left.Y < 0 || gamePadState.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState2.ThumbSticks.Left.Y < 0 || gamePadState2.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState3.ThumbSticks.Left.Y < 0 || gamePadState3.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState4.ThumbSticks.Left.Y < 0 || gamePadState4.DPad.Down == ButtonState.Pressed)) &&
                    currentSelection == Selections.Scores)
                {
                    currentSelection = Selections.Instructions;
                    moved = true;
                }
                else if (((gamePadState.ThumbSticks.Left.Y > 0 || gamePadState.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState2.ThumbSticks.Left.Y > 0 || gamePadState2.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState3.ThumbSticks.Left.Y > 0 || gamePadState3.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState4.ThumbSticks.Left.Y > 0 || gamePadState4.DPad.Up == ButtonState.Pressed)) &&
                    currentSelection == Selections.Instructions)
                {
                    currentSelection = Selections.Scores;
                    moved = true;
                }
                else if (((gamePadState.ThumbSticks.Left.Y < 0 || gamePadState.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState2.ThumbSticks.Left.Y < 0 || gamePadState2.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState3.ThumbSticks.Left.Y < 0 || gamePadState3.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState4.ThumbSticks.Left.Y < 0 || gamePadState4.DPad.Down == ButtonState.Pressed)) &&
                    currentSelection == Selections.Instructions)
                {
                    currentSelection = Selections.Credits;
                    moved = true;
                }
                else if (((gamePadState.ThumbSticks.Left.Y > 0 || gamePadState.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState2.ThumbSticks.Left.Y > 0 || gamePadState2.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState3.ThumbSticks.Left.Y > 0 || gamePadState3.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState4.ThumbSticks.Left.Y > 0 || gamePadState4.DPad.Up == ButtonState.Pressed)) &&
                    currentSelection == Selections.Credits)
                {
                    currentSelection = Selections.Instructions;
                    moved = true;
                }
                */
                else if (((gamePadState.ThumbSticks.Left.Y < 0 || gamePadState.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState2.ThumbSticks.Left.Y < 0 || gamePadState2.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState3.ThumbSticks.Left.Y < 0 || gamePadState3.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState4.ThumbSticks.Left.Y < 0 || gamePadState4.DPad.Down == ButtonState.Pressed)) &&
                    currentSelection == Selections.Endurance)
                {
                    currentSelection = Selections.TwoPlayers;
                    twoPlayersPosition -= new Vector2(10, 10);
                    endurancePosition += new Vector2(10, 10);

                    twoPlayersSprite = GameCore.PublicTwoPlayersButtonActiveTexture;
                    enduranceSprite = GameCore.PublicEnduranceButtonTexture;
                    twoPlayersColour = Color.Yellow;
                    enduranceColour = Color.White;
                    moved = true;
                }
                else if (((gamePadState.ThumbSticks.Left.Y < 0 || gamePadState.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState2.ThumbSticks.Left.Y < 0 || gamePadState2.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState3.ThumbSticks.Left.Y < 0 || gamePadState3.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState4.ThumbSticks.Left.Y < 0 || gamePadState4.DPad.Down == ButtonState.Pressed)) &&
                    currentSelection == Selections.Instructions)
                {
                    if (!Guide.IsTrialMode)
                    {
                        currentSelection = Selections.Scores;
                        scorePosition -= new Vector2(10, 10);
                        scoreSprite = GameCore.PublicHighScoresButtonActiveTexture;
                        scoresColour = Color.Yellow;
                    }
                    else
                    {
                        currentSelection = Selections.Options;
                        optionsPosition -= new Vector2(10, 10);
                        optionsSprite = GameCore.PublicOptionsButtonActiveTexture;
                        optionsColour = Color.Yellow;
                    }
                    
                    instructPosition += new Vector2(10, 10);

                    
                    instructSprite = GameCore.PublicInstructionsButtonTexture;
                    
                    instructColour = Color.White;
                    moved = true;
                }
                else if (((gamePadState.ThumbSticks.Left.Y > 0 || gamePadState.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState2.ThumbSticks.Left.Y > 0 || gamePadState2.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState3.ThumbSticks.Left.Y > 0 || gamePadState3.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState4.ThumbSticks.Left.Y > 0 || gamePadState4.DPad.Up == ButtonState.Pressed)) &&
                    currentSelection == Selections.TwoPlayers)
                {
                    currentSelection = Selections.Endurance;
                    twoPlayersPosition += new Vector2(10, 10);
                    endurancePosition -= new Vector2(10, 10);

                    twoPlayersSprite = GameCore.PublicTwoPlayersButtonTexture;
                    enduranceSprite = GameCore.PublicEnduranceButtonActiveTexture;
                    twoPlayersColour = Color.White;
                    enduranceColour = Color.Yellow;

                    moved = true;
                }
                else if (((gamePadState.ThumbSticks.Left.Y < 0 || gamePadState.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState2.ThumbSticks.Left.Y < 0 || gamePadState2.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState3.ThumbSticks.Left.Y < 0 || gamePadState3.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState4.ThumbSticks.Left.Y < 0 || gamePadState4.DPad.Down == ButtonState.Pressed)) &&
                    currentSelection == Selections.TwoPlayers)
                {
                    currentSelection = Selections.Instructions;
                    instructPosition -= new Vector2(10, 10);
                    twoPlayersPosition += new Vector2(10, 10);

                    instructSprite = GameCore.PublicInstructionsButtonActiveTexture;
                    twoPlayersSprite = GameCore.PublicTwoPlayersButtonTexture;
                    instructColour = Color.Yellow;
                    twoPlayersColour = Color.White;
                    moved = true;
                }
                else if (((gamePadState.ThumbSticks.Left.Y > 0 || gamePadState.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState2.ThumbSticks.Left.Y > 0 || gamePadState2.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState3.ThumbSticks.Left.Y > 0 || gamePadState3.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState4.ThumbSticks.Left.Y > 0 || gamePadState4.DPad.Up == ButtonState.Pressed)) &&
                    currentSelection == Selections.Instructions)
                {
                    if (!Guide.IsTrialMode)
                    {
                        currentSelection = Selections.TwoPlayers;
                        twoPlayersPosition -= new Vector2(10, 10);
                        twoPlayersSprite = GameCore.PublicTwoPlayersButtonActiveTexture;
                        twoPlayersColour = Color.Yellow;
                    }
                    else
                    {
                        currentSelection = Selections.Start;
                        startPosition -= new Vector2(10, 10);
                        startSprite = GameCore.PublicStoryButtonActiveTexture;
                        startColour = Color.Yellow;
                    }
                    instructPosition += new Vector2(10, 10);

                    
                    instructSprite = GameCore.PublicInstructionsButtonTexture;
                    
                    instructColour = Color.White;
                    moved = true;
                }
                else if (((gamePadState.ThumbSticks.Left.Y < 0 || gamePadState.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState2.ThumbSticks.Left.Y < 0 || gamePadState2.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState3.ThumbSticks.Left.Y < 0 || gamePadState3.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState4.ThumbSticks.Left.Y < 0 || gamePadState4.DPad.Down == ButtonState.Pressed)) &&
                    currentSelection == Selections.Options)
                {
                    if (Guide.IsTrialMode)
                    {
                        currentSelection = Selections.Buy;
                        buyPosition -= new Vector2(10, 10);
                        optionsPosition += new Vector2(10, 10);

                        buySprite = GameCore.PublicBuyButtonActiveTexture;
                        optionsSprite = GameCore.PublicOptionsButtonTexture;
                        buyColour = Color.Yellow;
                        optionsColour = Color.White;
                    }
                    else
                    {
                        currentSelection = Selections.Start;
                        startPosition -= new Vector2(10, 10);
                        optionsPosition += new Vector2(10, 10);

                        startSprite = GameCore.PublicStoryButtonActiveTexture;
                        optionsSprite = GameCore.PublicOptionsButtonTexture;
                        startColour = Color.Yellow;
                        optionsColour = Color.White;
                    }
                    moved = true;
                }
                else if (((gamePadState.ThumbSticks.Left.Y > 0 || gamePadState.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState2.ThumbSticks.Left.Y > 0 || gamePadState2.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState3.ThumbSticks.Left.Y > 0 || gamePadState3.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState4.ThumbSticks.Left.Y > 0 || gamePadState4.DPad.Up == ButtonState.Pressed)) &&
                    currentSelection == Selections.Options)
                {
                    if (!Guide.IsTrialMode)
                    {
                        currentSelection = Selections.Scores;
                        scorePosition -= new Vector2(10, 10);
                        scoreSprite = GameCore.PublicHighScoresButtonActiveTexture;
                        scoresColour = Color.Yellow;
                    }
                    else
                    {
                        currentSelection = Selections.Instructions;
                        instructPosition -= new Vector2(10, 10);
                        instructSprite = GameCore.PublicInstructionsButtonActiveTexture;
                        instructColour = Color.Yellow;
                    }
                    optionsPosition += new Vector2(10, 10);

                    
                    optionsSprite = GameCore.PublicOptionsButtonTexture;
                    
                    optionsColour = Color.White;
                    moved = true;
                }
                else if (((gamePadState.ThumbSticks.Left.Y > 0 || gamePadState.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState2.ThumbSticks.Left.Y > 0 || gamePadState2.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState3.ThumbSticks.Left.Y > 0 || gamePadState3.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState4.ThumbSticks.Left.Y > 0 || gamePadState4.DPad.Up == ButtonState.Pressed)) &&
                    currentSelection == Selections.Scores)
                {
                    currentSelection = Selections.Instructions;
                    instructPosition -= new Vector2(10, 10);
                    scorePosition += new Vector2(10, 10);

                    instructSprite = GameCore.PublicInstructionsButtonActiveTexture;
                    scoreSprite = GameCore.PublicHighScoresButtonTexture;
                    instructColour = Color.Yellow;
                    scoresColour = Color.White;
                    moved = true;
                }
                else if (((gamePadState.ThumbSticks.Left.Y < 0 || gamePadState.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState2.ThumbSticks.Left.Y < 0 || gamePadState2.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState3.ThumbSticks.Left.Y < 0 || gamePadState3.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState4.ThumbSticks.Left.Y < 0 || gamePadState4.DPad.Down == ButtonState.Pressed)) &&
                    currentSelection == Selections.Scores)
                {
                    currentSelection = Selections.Options;
                    optionsPosition -= new Vector2(10, 10);
                    scorePosition += new Vector2(10, 10);

                    optionsSprite = GameCore.PublicOptionsButtonActiveTexture;
                    scoreSprite = GameCore.PublicHighScoresButtonTexture;
                    optionsColour = Color.Yellow;
                    scoresColour = Color.White;
                    moved = true;
                }
                else if (((gamePadState.ThumbSticks.Left.Y < 0 || gamePadState.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState2.ThumbSticks.Left.Y < 0 || gamePadState2.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState3.ThumbSticks.Left.Y < 0 || gamePadState3.DPad.Down == ButtonState.Pressed) ||
                    (gamePadState4.ThumbSticks.Left.Y < 0 || gamePadState4.DPad.Down == ButtonState.Pressed)) &&
                    currentSelection == Selections.Buy)
                {
                   
                        currentSelection = Selections.Start;
                        startPosition -= new Vector2(10, 10);
                        buyPosition += new Vector2(10, 10);

                        startSprite = GameCore.PublicStoryButtonActiveTexture;
                        buySprite = GameCore.PublicBuyButtonTexture;
                        startColour = Color.Yellow;
                        buyColour = Color.White;
                   
                    moved = true;
                }
                else if (((gamePadState.ThumbSticks.Left.Y > 0 || gamePadState.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState2.ThumbSticks.Left.Y > 0 || gamePadState2.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState3.ThumbSticks.Left.Y > 0 || gamePadState3.DPad.Up == ButtonState.Pressed) ||
                    (gamePadState4.ThumbSticks.Left.Y > 0 || gamePadState4.DPad.Up == ButtonState.Pressed)) &&
                    currentSelection == Selections.Buy)
                {

                    currentSelection = Selections.Options;
                    optionsPosition -= new Vector2(10, 10);
                    buyPosition += new Vector2(10, 10);

                    optionsSprite = GameCore.PublicOptionsButtonActiveTexture;
                    buySprite = GameCore.PublicBuyButtonTexture;
                    optionsColour = Color.Yellow;
                    buyColour = Color.White;

                    moved = true;
                }
            }
            
            


            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch sBatch)
        {
            titleSprite.Draw(sBatch);
            titleBG.Draw(sBatch);
            
            sBatch.Draw(startSprite, startPosition, startColour);
            sBatch.Draw(enduranceSprite, endurancePosition, enduranceColour);
            sBatch.Draw(twoPlayersSprite, twoPlayersPosition, twoPlayersColour);
            sBatch.Draw(optionsSprite, optionsPosition, optionsColour);
            sBatch.Draw(scoreSprite, scorePosition, scoresColour);
            sBatch.Draw(instructSprite, instructPosition, instructColour);
            if(Guide.IsTrialMode)
                sBatch.Draw(buySprite, buyPosition, buyColour);

            //sBatch.Draw(GameCore.PublicComingSoonTexture, new Vector2(twoPlayersPosition.X - 107, twoPlayersPosition.Y - 10), Color.White);
            //sBatch.Draw(GameCore.PublicComingSoonTexture, new Vector2(optionsPosition.X - 107, optionsPosition.Y - 10), Color.White);
            //sBatch.Draw(GameCore.PublicComingSoonTexture, new Vector2(scorePosition.X - 107, scorePosition.Y - 10), Color.White);
            //sBatch.Draw(GameCore.PublicComingSoonTexture, new Vector2(instructPosition.X - 107, instructPosition.Y - 10), Color.White);
            //sBatch.DrawString(GameCore.PublicFontTitle, instructSprite, instructPosition, instructColour);
            //sBatch.DrawString(GameCore.PublicFontTitle, creditSprite, creditPosition, creditColour);
            //sBatch.DrawString(GameCore.PublicFontTitle, scoreSprite, scorePosition, scoreColour);
            //sBatch.DrawString(GameCore.StoryPericles, enduranceSprite, endurancePosition, enduranceColour);
            

            base.Draw(sBatch);
        }
    }
}
