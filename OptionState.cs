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
    class OptionState : GameState
    {
        private Sprite titleSprite;

        private Sprite gameOverSprite;
       

        //private Vector2 scorePosition;

        private static Selection curSelection = Selection.Music;
        private bool moved = false;
        private const int MOVEDELAY = 250;

        private Sprite musicSprite;
        private Sprite sfxSprite;
        private Sprite backSprite;
        private Sprite helpSprite;
        private AnimatedSprite onSprite;
        private AnimatedSprite offSprite;
        private AnimatedSprite helpOnSprite;
        private AnimatedSprite helpOffSprite;
        private Sprite slideBarSprite;
        private Sprite ballSprite;
        private const int BALLMIN = 222;
        private const int BALLMAX = 474;
        private TimeSpan elapsedTime = TimeSpan.Zero;

        public enum Selection
        {
            Music,
            Sfx,
            Help,
            Back
        }
        public static Selection CurrentSelection { get { return curSelection; } }


        public OptionState(Game game)
            : base(game)
        {
            titleSprite = new Sprite(GameCore.PublicTitleTexture);
            gameOverSprite = new Sprite(GameCore.PublicOptionsTexture);
            gameOverSprite.Position = new Vector2(465, 236);

            musicSprite = new Sprite(GameCore.PublicMusicTexture);
            sfxSprite = new Sprite(GameCore.PublicSFXTexture);
            backSprite = new Sprite(GameCore.PublicBackTexture);
            helpSprite = new Sprite(GameCore.PublicHelpTexture);
            onSprite = new AnimatedSprite(GameCore.PublicOnTexture);
            offSprite = new AnimatedSprite(GameCore.PublicOffTexture);
            helpOnSprite = new AnimatedSprite(GameCore.PublicOnTexture);
            helpOffSprite = new AnimatedSprite(GameCore.PublicOffTexture);
            slideBarSprite = new Sprite(GameCore.PublicSliderTexture);
            ballSprite = new Sprite(GameCore.PublicBallTexture);
            //scorePosition = new Vector2(530, 550);

            musicSprite.Position = gameOverSprite.Position + new Vector2(124, 92);
            musicSprite.MyColour = Color.Yellow;
            sfxSprite.Position = gameOverSprite.Position + new Vector2(124, 147);
            helpSprite.Position = gameOverSprite.Position + new Vector2(124, 202);
            backSprite.Position = gameOverSprite.Position + new Vector2(222, 258);
            ballSprite.Position = gameOverSprite.Position + new Vector2(BALLMIN + ((BALLMAX - BALLMIN) * MediaPlayer.Volume), 80);
            slideBarSprite.Position = gameOverSprite.Position + new Vector2(241, 100);
            onSprite.Position = new Vector2(slideBarSprite.Position.X, sfxSprite.Position.Y);
            onSprite.Animate(1);
            onSprite.MyColour = Color.Yellow;
            offSprite.Position = new Vector2(gameOverSprite.Position.X + 345, sfxSprite.Position.Y);
            helpOnSprite.Position = new Vector2(slideBarSprite.Position.X, helpSprite.Position.Y);
            helpOnSprite.Animate(1);
            helpOnSprite.MyColour = Color.Yellow;
            helpOffSprite.Position = new Vector2(gameOverSprite.Position.X + 345, helpSprite.Position.Y);
        }



        public override void Update(GameTime gameTime)
        {
            GamePadState myState = GamePad.GetState(PlayerIndex.One);
            GamePadState myState2 = GamePad.GetState(PlayerIndex.Two);
            GamePadState myState3 = GamePad.GetState(PlayerIndex.Three);
            GamePadState myState4 = GamePad.GetState(PlayerIndex.Four);
            if (moved)
            {
                elapsedTime += gameTime.ElapsedGameTime;
                if (elapsedTime > TimeSpan.FromMilliseconds(MOVEDELAY))
                {
                    elapsedTime = TimeSpan.Zero;
                    moved = false;
                }
            }
            else
            {
                if (myState.DPad.Down == ButtonState.Pressed || myState2.DPad.Down == ButtonState.Pressed ||
                    myState3.DPad.Down == ButtonState.Pressed || myState4.DPad.Down == ButtonState.Pressed ||
                    myState.ThumbSticks.Left.Y <- 0.1f || myState2.ThumbSticks.Left.Y <- 0.1f || myState3.ThumbSticks.Left.Y <- 0.1f ||
                    myState4.ThumbSticks.Left.Y <- 0.1f)
                {
                    moved = true;
                    switch (curSelection)
                    {
                        case Selection.Music:
                            curSelection = Selection.Sfx;
                            musicSprite.MyColour = Color.White;
                            sfxSprite.MyColour = Color.Yellow;
                            break;
                        case Selection.Sfx:
                            curSelection = Selection.Help;
                            helpSprite.MyColour = Color.Yellow;
                            sfxSprite.MyColour = Color.White;
                            break;
                        case Selection.Help:
                            curSelection = Selection.Back;
                            backSprite.MyColour = Color.Yellow;
                            helpSprite.MyColour = Color.White;
                            break;
                        case Selection.Back:
                            curSelection = Selection.Music;
                            musicSprite.MyColour = Color.Yellow;
                            backSprite.MyColour = Color.White;
                            break;
                    }
                    
                }
                else if (myState.DPad.Up == ButtonState.Pressed || myState2.DPad.Up == ButtonState.Pressed ||
                    myState3.DPad.Up == ButtonState.Pressed || myState4.DPad.Up == ButtonState.Pressed ||
                    myState.ThumbSticks.Left.Y > 0.1f || myState2.ThumbSticks.Left.Y > 0.1f || myState3.ThumbSticks.Left.Y > 0.1f ||
                    myState4.ThumbSticks.Left.Y > 0.1f)
                {
                    moved = true;
                    switch (curSelection)
                    {
                        case Selection.Music:
                            curSelection = Selection.Back;
                            musicSprite.MyColour = Color.White;
                            backSprite.MyColour = Color.Yellow;
                            break;
                        case Selection.Sfx:
                            curSelection = Selection.Music;
                            musicSprite.MyColour = Color.Yellow;
                            sfxSprite.MyColour = Color.White;
                            break;
                        case Selection.Help:
                            curSelection = Selection.Sfx;
                            sfxSprite.MyColour = Color.Yellow;
                            helpSprite.MyColour = Color.White;
                            break;
                        case Selection.Back:
                            curSelection = Selection.Help;
                            helpSprite.MyColour = Color.Yellow;
                            backSprite.MyColour = Color.White;
                            break;
                    }
                    
                }
            }
            switch (curSelection)
            {
                case Selection.Music:
                    if (myState.DPad.Right == ButtonState.Pressed || myState2.DPad.Right == ButtonState.Pressed ||
                    myState3.DPad.Right == ButtonState.Pressed || myState4.DPad.Right == ButtonState.Pressed ||
                    myState.ThumbSticks.Left.X > 0 || myState2.ThumbSticks.Left.X > 0 || myState3.ThumbSticks.Left.X > 0 ||
                    myState4.ThumbSticks.Left.X > 0)
                    {
                        MediaPlayer.Volume = Math.Min(1, (MediaPlayer.Volume + 0.01f));
                        ballSprite.Position = gameOverSprite.Position + new Vector2(BALLMIN + ((BALLMAX - BALLMIN) * MediaPlayer.Volume), 80);
                    }
                    else if (myState.DPad.Left == ButtonState.Pressed || myState2.DPad.Left == ButtonState.Pressed ||
                    myState3.DPad.Left == ButtonState.Pressed || myState4.DPad.Left == ButtonState.Pressed ||
                    myState.ThumbSticks.Left.X < 0 || myState2.ThumbSticks.Left.X < 0 || myState3.ThumbSticks.Left.X < 0 ||
                    myState4.ThumbSticks.Left.X < 0)
                    {
                        MediaPlayer.Volume = Math.Max(0, (MediaPlayer.Volume - 0.01f));
                        ballSprite.Position = gameOverSprite.Position + new Vector2(BALLMIN + ((BALLMAX - BALLMIN) * MediaPlayer.Volume), 80);
                    }
                    break;
                case Selection.Sfx:
                    if (myState.DPad.Right == ButtonState.Pressed || myState2.DPad.Right == ButtonState.Pressed ||
                    myState3.DPad.Right == ButtonState.Pressed || myState4.DPad.Right == ButtonState.Pressed ||
                    myState.ThumbSticks.Left.X > 0 || myState2.ThumbSticks.Left.X > 0 || myState3.ThumbSticks.Left.X > 0 ||
                    myState4.ThumbSticks.Left.X > 0)
                    {
                        GameCore.PlaySoundEffects = false;
                        onSprite.Animate(0);
                        offSprite.Animate(1);
                        onSprite.MyColour = Color.White;
                        offSprite.MyColour = Color.Yellow;
                    }
                    else if (myState.DPad.Left == ButtonState.Pressed || myState2.DPad.Left == ButtonState.Pressed ||
                    myState3.DPad.Left == ButtonState.Pressed || myState4.DPad.Left == ButtonState.Pressed ||
                    myState.ThumbSticks.Left.X < 0 || myState2.ThumbSticks.Left.X < 0 || myState3.ThumbSticks.Left.X < 0 ||
                    myState4.ThumbSticks.Left.X < 0)
                    {
                        GameCore.PlaySoundEffects = true;
                        onSprite.Animate(1);
                        offSprite.Animate(0);
                        onSprite.MyColour = Color.Yellow;
                        offSprite.MyColour = Color.White;
                    }
                    break;
                case Selection.Help:
                    if (myState.DPad.Right == ButtonState.Pressed || myState2.DPad.Right == ButtonState.Pressed ||
                    myState3.DPad.Right == ButtonState.Pressed || myState4.DPad.Right == ButtonState.Pressed ||
                    myState.ThumbSticks.Left.X > 0 || myState2.ThumbSticks.Left.X > 0 || myState3.ThumbSticks.Left.X > 0 ||
                    myState4.ThumbSticks.Left.X > 0)
                    {
                        GameCore.IsTutorial = false;
                        helpOnSprite.Animate(0);
                        helpOffSprite.Animate(1);
                        helpOnSprite.MyColour = Color.White;
                        helpOffSprite.MyColour = Color.Yellow;
                    }
                    else if (myState.DPad.Left == ButtonState.Pressed || myState2.DPad.Left == ButtonState.Pressed ||
                    myState3.DPad.Left == ButtonState.Pressed || myState4.DPad.Left == ButtonState.Pressed ||
                    myState.ThumbSticks.Left.X < 0 || myState2.ThumbSticks.Left.X < 0 || myState3.ThumbSticks.Left.X < 0 ||
                    myState4.ThumbSticks.Left.X < 0)
                    {
                        GameCore.IsTutorial = true;
                        helpOnSprite.Animate(1);
                        helpOffSprite.Animate(0);
                        helpOnSprite.MyColour = Color.Yellow;
                        helpOffSprite.MyColour = Color.White;
                    }
                    break;
                case Selection.Back:
                    break;
            }

            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch sBatch)
        {
            titleSprite.Draw(sBatch);
            gameOverSprite.Draw(sBatch);

            musicSprite.Draw(sBatch);
            sfxSprite.Draw(sBatch);
            backSprite.Draw(sBatch);
            slideBarSprite.Draw(sBatch);
            ballSprite.Draw(sBatch);
            helpSprite.Draw(sBatch);
            helpOnSprite.Draw(sBatch);
            helpOffSprite.Draw(sBatch);
            onSprite.Draw(sBatch);
            offSprite.Draw(sBatch);


            base.Draw(sBatch);
        }

        public void Reset()
        {
            curSelection = Selection.Music;
            musicSprite.MyColour = Color.Yellow;
            sfxSprite.MyColour = Color.White;
            backSprite.MyColour = Color.White;
            
        }
    }
}
