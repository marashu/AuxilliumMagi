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
    class StoryState : GameState
    {
        

        private bool bAPressed = true;
        private bool bStartPressed = true;

        public bool BoolAPressed { set { bAPressed = value; } }
        public bool BoolStartPressed { set { bStartPressed = value; } }

        private bool gotoNextLevel = false;
        public bool GoToNextLevel { get { return gotoNextLevel; } set { gotoNextLevel = value; } }

        private Texture2D roadTexture;
        private Texture2D mountainTexture;
        private Texture2D townTexture;

        private Texture2D textBG;

        public StoryState(Game game)
            : base(game)
        {
            //titleSprite = new Sprite(GameCore.PublicGameOverTexture);
            //startSprite = "PRESS START";

            //startPosition = new Vector2(530, 450);
            textBG = GameCore.PublicStoryTextBG;
            roadTexture = GameCore.PublicStoryRoadBG;
            mountainTexture = GameCore.PublicStoryMountainBG;
            townTexture = GameCore.PublicStoryTownBG;
        }



        public override void Update(GameTime gameTime)
        {

            GamePadState PlayerOne = GamePad.GetState(ActionState.PublicPOne);
            if (PlayerOne.Buttons.Start == ButtonState.Pressed && !bStartPressed)
            {
                bStartPressed = true;
                gotoNextLevel = true;
            }
            if (PlayerOne.Buttons.A == ButtonState.Pressed && !bAPressed)
            {
                if (StoryManager.Story.Count > 1)
                {
                    if (StoryManager.Story[1].Level == ActionState.Level)
                    {
                        StoryManager.Story.Remove(StoryManager.Story[0]);
                    }
                    else
                    {
                        gotoNextLevel = true;
                    }
                }
                //here, it's the last element of the story, aka end game.
                else
                {
                    gotoNextLevel = true;
                }
                bAPressed = true;
            }


            if (PlayerOne.Buttons.A == ButtonState.Released)
                bAPressed = false;
            if (PlayerOne.Buttons.Start == ButtonState.Released)
                bStartPressed = false;

            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch sBatch)
        {
            switch (StoryManager.Story[0].Level)
            {
                    //city are 0,3,4,9
                case 0:
                case 3:
                case 4:
                case 9:
                case 10:
                    sBatch.Draw(townTexture, Vector2.Zero, Color.White);
                    break;
                case 1:
                case 2:
                
                case 7:
                    sBatch.Draw(roadTexture, Vector2.Zero, Color.White);
                    break;
                
                case 5:
                case 6:
                case 8:

                default:
                    sBatch.Draw(mountainTexture, Vector2.Zero, Color.White);
                    break;
            }
            sBatch.Draw(textBG, new Vector2(120, 445), Color.White);
            StoryManager.Story[0].Draw(sBatch);



            base.Draw(sBatch);
        }
    }
}
