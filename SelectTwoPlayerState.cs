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
    class SelectTwoPlayerState : GameState
    {
        private Sprite titleSprite;

        private Sprite gameOverSprite;


        //private Vector2 scorePosition;

        private static PlayerIndex POne;
        private static PlayerIndex PTwo;

        public static PlayerIndex PublicPOne { get { return POne; } }
        public static PlayerIndex PublicPTwo { get { return PTwo; } }

        private String PlayerOneString;
        private String PlayerTwoString;

        private Vector2 PlayerOneVector;
        private Vector2 PlayerTwoVector;

        

        public SelectTwoPlayerState(Game game)
            : base(game)
        {
            titleSprite = new Sprite(GameCore.PublicTitleTexture);
            gameOverSprite = new Sprite(GameCore.PublicSelectTwoPlayerTexture);
            gameOverSprite.Position = new Vector2(195, 136);

            PlayerOneVector = gameOverSprite.Position + new Vector2(176, 237);
            PlayerTwoVector = gameOverSprite.Position + new Vector2(484, 237);
        }

        public void SetPlayerOne(PlayerIndex pIndex)
        {
            POne = pIndex;
            PlayerOneString = POne.ToString();
        }

        public void SetPlayerTwo(PlayerIndex pIndex)
        {
            PTwo = pIndex;
            PlayerTwoString = PTwo.ToString();
        }

        public override void Update(GameTime gameTime)
        {
            
            //set up the second player's remote
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && POne != PlayerIndex.One)
                SetPlayerTwo(PlayerIndex.One);
            else if (GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed && POne != PlayerIndex.Two)
                SetPlayerTwo(PlayerIndex.Two);
            else if (GamePad.GetState(PlayerIndex.Three).Buttons.A == ButtonState.Pressed && POne != PlayerIndex.Three)
                SetPlayerTwo(PlayerIndex.Three);
            else if (GamePad.GetState(PlayerIndex.Four).Buttons.A == ButtonState.Pressed && POne != PlayerIndex.Four)
                SetPlayerTwo(PlayerIndex.Four);
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch sBatch)
        {
            titleSprite.Draw(sBatch);
            gameOverSprite.Draw(sBatch);

            sBatch.DrawString(GameCore.Pericles, "Player " + PlayerOneString, PlayerOneVector, Color.White);
            sBatch.DrawString(GameCore.Pericles, "is ready", PlayerOneVector + new Vector2(25, 25), Color.White);

            if (POne == PTwo)
            {
                sBatch.DrawString(GameCore.Pericles, "Waiting...", PlayerTwoVector, Color.White);
                sBatch.DrawString(GameCore.Pericles, "Press A to join", PlayerTwoVector + new Vector2(-55, 25), Color.White);
            }
            else
            {
                sBatch.DrawString(GameCore.Pericles, "Player " + PlayerTwoString, PlayerTwoVector, Color.White);
                sBatch.DrawString(GameCore.Pericles, "is ready", PlayerTwoVector + new Vector2(25, 25), Color.White);
            }


            base.Draw(sBatch);
        }

        
    }
}
