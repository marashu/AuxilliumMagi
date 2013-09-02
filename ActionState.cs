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
    public class ActionState:GameState
    {
        protected const short BASEADDBULLETTIME = 700;
        protected const byte BASEADDBULLETPERCENT = 5;
        protected const short ADDBULLETTIME = 150;
        protected const byte ADDBULLETPERCENT = 3;
        protected const short BASEADDARROWTIME = 1600;
        protected const sbyte MAXREFLECTPERCENT = 100;
        protected const byte DIFFICULTYTIME = 100;
        protected const byte BULLETSTOADDDIFFICULTY = 20;
        
        //a byte for how much the difficulty time will affect the time
        protected byte difficultyMod = 0;
        protected byte difficultyCounter = 0;

        //a time span for when the lightning doesn't happen.  Also, a boolean.
        protected TimeSpan lightningTime = TimeSpan.Zero;
        protected const short LIGHTNINGLIMIT = 300;
        protected bool bLightningTime = true;

        //button press boolean
        protected bool bStartPressed = true;
        protected bool bIsPaused = false;

        //if the tutorial is showing
        protected bool bShowTutorial = false;

        //level info
        protected static sbyte level = 0;
        public static sbyte Level { get { return level; } set { level = value; } }

        protected sbyte health = 100;
        public sbyte Health { get { return health; } }
        protected static ushort score = 0;
        public static ushort Score { get { return score; } }

        protected bool levelComplete = false;
        public bool LevelComplete { get { return levelComplete; } }

        //background texture
        protected Texture2D bgTexture;

        //the player
        private static PlayerIndex POne;
        public static PlayerIndex PublicPOne { get { return POne; } }

        //center stuff
        protected AnimatedSprite centerRune;
        protected AnimatedSprite centerStone;
        protected Sprite centerElder;
        protected Sprite centerDruid;
        protected Sprite centerArchitect;
        protected Sprite centerDot;

        protected AnimatedSprite iceShield;
        protected AnimatedSprite iceShieldOver;
        protected AnimatedSprite fireShield;
        protected AnimatedSprite fireShieldOver;

        //tutorial stuff
        protected Sprite tutorialBG;
        protected List<Projectile> tutorialList;
        protected String tutorialString;
        protected Vector2 tutorialStringPosition;

        //spell blasts
        protected AnimatedSprite spellBlastUp;
        protected AnimatedSprite spellBlastDown;
        protected AnimatedSprite spellBlastLeft;
        protected AnimatedSprite spellBlastRight;

        //lightning
        protected AnimatedSprite lightningUp;
        protected AnimatedSprite lightningDown;
        protected AnimatedSprite lightningLeft;
        protected AnimatedSprite lightningRight;

        //and for animating
        protected bool bAnimateUp = false;
        protected bool bAnimateDown = false;
        protected bool bAnimateLeft = false;
        protected bool bAnimateRight = false;

        protected bool bLightningUp = false;
        protected bool bLightningDown = false;
        protected bool bLightningLeft = false;
        protected bool bLightningRight = false;


        //angle stuff
        protected float iceAngle = 0;
        protected float fireAngle = 0;

        private float Angle = 0;
        private float lowerLimit = 337.5f;
        private float upperLimit = 22.5f;

        protected Random random;

        //reflection stuff
        protected bool bIsReflecting = false;
        protected sbyte reflectPercent = MAXREFLECTPERCENT;
        protected Texture2D healthMeter;
        protected Texture2D reflectMeter;

        //spell blast stuff
        protected byte numSpellBlasts = 0;
        protected byte numDeflectCounter = 0;
        protected bool bUpPressed = true;
        protected bool bDownPressed = true;
        protected bool bLeftPressed = true;
        protected bool bRightPressed = true;
        protected bool bSpellBlast = false;
        protected bool bSpellBlastPressed = true;
        protected Texture2D SpellBlastIcon;

        //timer stuff
        protected TimeSpan myTimer = TimeSpan.Zero;
        protected TimeSpan arrowTimer = TimeSpan.Zero;
        protected TimeSpan fireStunTimer = TimeSpan.Zero;
        protected TimeSpan iceStunTimer = TimeSpan.Zero;
        protected TimeSpan fireCounterTimer = TimeSpan.Zero;
        protected TimeSpan iceCounterTimer = TimeSpan.Zero;
        protected TimeSpan reflectTimer = TimeSpan.Zero;
        //timer for the gameplay countdown
        protected TimeSpan countdownTimer = new TimeSpan(0, 2, 0);
        protected String countdownString;

        //timer for patterns
        protected TimeSpan patternTimer = TimeSpan.Zero;

        //timing stuff for bullet adding
        protected short addBulletRate = 0;
        protected byte addBulletPercent = 0;
        protected short addArrowRate = 0;
        protected byte addArrowPercent = 0;
        protected byte patternCounter = 0;

        protected ProjectilePattern myPattern = ProjectilePattern.NONE;


        protected List<Projectile> Projectiles;
        

        //colour stuff for the two shields
        protected byte fireRed = 255;
        protected byte fireGreen = 66;
        protected byte fireBlue = 31;
        protected byte fireAlpha = 210;

        protected byte fireOverRed = 255;
        protected byte fireOverGreen = 228;
        protected byte fireOverBlue = 0;


        protected byte iceRed = 100;
        protected byte iceGreen = 233;
        protected byte iceBlue = 255;
        protected byte iceAlpha = 255;

        protected byte tempfireRed = 255;
        protected byte tempfireGreen = 66;
        protected byte tempfireBlue = 31;

        protected byte tempfireOverRed = 255;
        protected byte tempfireOverGreen = 228;
        protected byte tempfireOverBlue = 0;

        protected byte tempiceRed = 100;
        protected byte tempiceGreen = 233;
        protected byte tempiceBlue = 255;

        //stuff for getting stunned
        protected bool fireStun = false;
        protected bool iceStun = false;
        protected const short STUNRECOVERY = 2000;

        protected TimeSpan rumbleTimer = TimeSpan.Zero;
        protected bool isHit = false;


        private Dragon tempDragon;
        private Priest sunPriest;

        protected enum ProjectilePattern
        {
            NONE,
            COUNTER_SPIN,
            BALL_STUN_COUNTER
        }

        public ActionState(Game game) : base(game)
        {
            iceShield = new AnimatedSprite(GameCore.PublicIceShieldTexture);
            iceShieldOver = new AnimatedSprite(GameCore.PublicIceShieldOverTexture);
            fireShield = new AnimatedSprite(GameCore.PublicFireShieldTexture);
            fireShieldOver = new AnimatedSprite(GameCore.PublicFireShieldOverTexture);

            centerRune = new AnimatedSprite(GameCore.PublicCenterRuneTexture);
            centerStone = new AnimatedSprite(GameCore.PublicCenterStoneTexture);
            centerRune.Position = new Vector2(640 - (centerRune.Size.X / 2), 360 - (centerRune.Size.Y / 2));
            centerStone.Position = new Vector2(640 - (centerStone.Size.X / 2), 360 - (centerStone.Size.Y / 2));
            centerArchitect = new Sprite(GameCore.PublicCenterArchitect);
            centerDruid = new Sprite(GameCore.PublicCenterDruid);
            centerElder = new Sprite(GameCore.PublicCenterElder);
            centerDot = new Sprite(GameCore.PublicRedDot);
            centerDot.Position = new Vector2(640 - (centerDot.Size.X / 2), 360 - (centerDot.Size.Y / 2));

            centerArchitect.Position = new Vector2(640 - (centerArchitect.Size.X / 2), 360 - (centerArchitect.Size.Y / 2));
            centerElder.Position = new Vector2(640 - (centerElder.Size.X / 2), 360 - (centerElder.Size.Y / 2));
            centerDruid.Position = new Vector2(640 - (centerDruid.Size.X / 2), 360 - (centerDruid.Size.Y / 2));

            tutorialBG = new Sprite(GameCore.PublicTutorialTexture);
            tutorialBG.Position = new Vector2(640 - (tutorialBG.Size.X / 2), 360 - (tutorialBG.Size.Y / 2));
            tutorialList = new List<Projectile>();
            tutorialStringPosition = new Vector2(tutorialBG.Position.X + 150, tutorialBG.Position.Y + 20);

            SpellBlastIcon = GameCore.PublicSpellBlastIcon;
            spellBlastDown = new AnimatedSprite(GameCore.PublicVerticalSpellBlast);
            spellBlastUp = new AnimatedSprite(GameCore.PublicVerticalSpellBlast);
            spellBlastLeft = new AnimatedSprite(GameCore.PublicHorizontalSpellBlast);
            spellBlastRight = new AnimatedSprite(GameCore.PublicHorizontalSpellBlast);

            spellBlastLeft.Position = new Vector2(-50, 65);
            spellBlastRight.Position = new Vector2(690, 65);
            spellBlastDown.Position = new Vector2(157, 410);
            spellBlastUp.Position = new Vector2(157, -50);

            spellBlastDown.FrameMod = 0.5f;
            spellBlastUp.FrameMod = 0.5f;
            spellBlastLeft.FrameMod = 0.5f;
            spellBlastRight.FrameMod = 0.5f;

            spellBlastRight.Effect = SpriteEffects.FlipHorizontally;
            spellBlastDown.Effect = SpriteEffects.FlipVertically;

            lightningDown = new AnimatedSprite(GameCore.PublicVerticalLightning);
            lightningUp = new AnimatedSprite(GameCore.PublicVerticalLightning);
            lightningLeft = new AnimatedSprite(GameCore.PublicHorizontalLightning);
            lightningRight = new AnimatedSprite(GameCore.PublicHorizontalLightning);

            lightningLeft.Position = new Vector2(280, 330);
            lightningRight.Position = new Vector2(690, 330);
            lightningDown.Position = new Vector2(612, 430);
            lightningUp.Position = new Vector2(612, 78);

            lightningDown.FrameMod = 0.5f;
            lightningUp.FrameMod = 0.5f;
            lightningLeft.FrameMod = 0.5f;
            lightningRight.FrameMod = 0.5f;

            lightningRight.Effect = SpriteEffects.FlipHorizontally;
            lightningDown.Effect = SpriteEffects.FlipVertically;

            healthMeter = GameCore.PublicHealthMeterTexture;
            reflectMeter = GameCore.PublicReflectMeterTexture;

            //iceShield.Position = new Vector2(300, 300);
            //fireShield.Position = new Vector2(300, 300);

            iceShield.MyColour = new Color(iceRed, iceGreen, iceBlue);
            iceShield.Pivot = new Vector2(iceShield.Size.X/ 2 , iceShield.Size.Y + 100);
            iceShield.Position = new Vector2(1280 / 2 - iceShield.Size.X / 2 + 100, 720 / 2 - iceShield.Size.Y + 80);

            fireShield.MyColour = new Color(fireRed, fireGreen, fireBlue);
            fireShield.Pivot = new Vector2(fireShield.Size.X / 2, fireShield.Size.Y + 100);
            fireShield.Position = new Vector2(1280 / 2 - fireShield.Size.X / 2 + 100, 720 / 2 - fireShield.Size.Y + 80);

            iceShieldOver.MyColour = Color.White;
            iceShieldOver.Pivot = new Vector2(iceShieldOver.Size.X / 2, iceShieldOver.Size.Y + 100);
            iceShieldOver.Position = new Vector2(1280 / 2 - iceShield.Size.X / 2 + 100, 720 / 2 - iceShield.Size.Y + 80);

            fireShieldOver.MyColour = new Color(fireOverRed, fireOverGreen, fireOverBlue);
            fireShieldOver.Pivot = new Vector2(fireShieldOver.Size.X / 2, fireShieldOver.Size.Y + 100);
            fireShieldOver.Position = new Vector2(1280 / 2 - fireShieldOver.Size.X / 2 + 100, 720 / 2 - fireShieldOver.Size.Y + 80);

            Projectiles = new List<Projectile>();
            random = new Random();


            tempDragon = new Dragon();
            sunPriest = new Priest();

        }

        public void SetPlayerIndex(PlayerIndex pIndex)
        {
            POne = pIndex;
        }

        public override void Update(GameTime gameTime)
        {
            //For now, just use player 1
            GamePadState PlayerOne = GamePad.GetState(POne);
            if (bIsPaused)
            {
                if (countdownTimer.Seconds > 9)
                    countdownString = countdownTimer.Minutes.ToString() + ":" + countdownTimer.Seconds.ToString();
                else
                    countdownString = countdownTimer.Minutes.ToString() + ":0" + countdownTimer.Seconds.ToString();
                if (PlayerOne.Buttons.Start == ButtonState.Pressed)
                {
                    if (!bStartPressed && bIsPaused)
                    {
                        bIsPaused = false;
                        bStartPressed = true;
                        GamePad.SetVibration(POne, 0, 0);
                        MediaPlayer.Resume();
                        return;
                    }
                }
                else if (bStartPressed)
                {
                    bStartPressed = false;
                }
            }
            else if (bShowTutorial)
            {
                if (countdownTimer.Seconds > 9)
                    countdownString = countdownTimer.Minutes.ToString() + ":" + countdownTimer.Seconds.ToString();
                else
                    countdownString = countdownTimer.Minutes.ToString() + ":0" + countdownTimer.Seconds.ToString();
                foreach (Projectile proj in tutorialList)
                    proj.Update(gameTime);
                if (PlayerOne.Buttons.Start == ButtonState.Pressed)
                {
                    if (!bStartPressed && bShowTutorial)
                    {
                        bShowTutorial = false;
                        bStartPressed = true;
                        GamePad.SetVibration(POne, 0, 0);
                        tutorialList.Clear();
                        //MediaPlayer.Resume();
                        return;
                    }
                }
                else if (bStartPressed)
                {
                    bStartPressed = false;
                }
                if (PlayerOne.Buttons.A == ButtonState.Pressed)
                {
                    if (!bLightningTime && bShowTutorial)
                    {
                        bShowTutorial = false;
                        bLightningTime = true;
                        GamePad.SetVibration(POne, 0, 0);
                        tutorialList.Clear();
                        //MediaPlayer.Resume();
                        return;
                    }
                }
                if (bLightningTime)
                {
                    lightningTime += gameTime.ElapsedGameTime;
                    if (lightningTime > TimeSpan.FromMilliseconds(LIGHTNINGLIMIT))
                    {
                        lightningTime = TimeSpan.Zero;
                        bLightningTime = false;
                    }
                }
            }
            if(!bIsPaused && !Guide.IsVisible && !bShowTutorial)
            {
                if (PlayerOne.Buttons.Start == ButtonState.Pressed)
                {
                    if (!bStartPressed && !bIsPaused && level != 0)
                    {
                        bIsPaused = true;
                        bStartPressed = true;
                        GamePad.SetVibration(POne, 0, 0);
                        MediaPlayer.Pause();
                        return;
                        
                    }
                }
                else if (bStartPressed)
                {
                    bStartPressed = false;
                }
                //update lightning timer
                if (bLightningTime)
                {
                    lightningTime += gameTime.ElapsedGameTime;
                    if (lightningTime > TimeSpan.FromMilliseconds(LIGHTNINGLIMIT))
                    {
                        lightningTime = TimeSpan.Zero;
                        bLightningTime = false;
                    }
                }
                if (isHit)
                {
                    rumbleTimer += gameTime.ElapsedGameTime;
                    if (rumbleTimer > TimeSpan.FromMilliseconds(600))
                    {
                        rumbleTimer = TimeSpan.Zero;
                        isHit = false;
                        GamePad.SetVibration(POne, 0, 0);
                    }
                }
                if (iceStun)
                {
                    iceStunTimer += gameTime.ElapsedGameTime;
                    if (iceStunTimer > TimeSpan.FromMilliseconds(STUNRECOVERY))
                    {
                        iceStunTimer = TimeSpan.Zero;
                        iceStun = false;
                    }
                }

                if (level > 0 && level != 6 && level != 10)
                {
                    countdownTimer -= gameTime.ElapsedGameTime;
                    if (countdownTimer.Seconds > 9)
                        countdownString = countdownTimer.Minutes.ToString() + ":" + countdownTimer.Seconds.ToString();
                    else
                        countdownString = countdownTimer.Minutes.ToString() + ":0" + countdownTimer.Seconds.ToString();
                    if (countdownTimer.TotalSeconds <= 0) //||//GET RID OF THIS NEXT PART FOR THE GAME!!!!!
                    //PlayerOne.Buttons.LeftShoulder == ButtonState.Pressed)
                    {
                        countdownString = "0:00";
                        if(Projectiles.Count() == 0)
                            levelComplete = true;
                    }
                }
                else if (level == 6)
                {
                    if (tempDragon.Dead)// ||//GET RID OF THIS NEXT PART FOR THE GAME!!!!!
                        //PlayerOne.Buttons.LeftShoulder == ButtonState.Pressed)
                    {
                        levelComplete = true;
                    }
                }
                else if (level == 10)
                {
                    if (sunPriest.CurrentState == Priest.BossStates.Dead)// ||//GET RID OF THIS NEXT PART FOR THE GAME!!!!!
                        //PlayerOne.Buttons.LeftShoulder == ButtonState.Pressed)
                    {
                        levelComplete = true;
                    }
                }
                else
                {
                    if (PlayerOne.Buttons.Start == ButtonState.Pressed && !bStartPressed)
                    {
                        bStartPressed = true;
                        levelComplete = true;
                    }
                }
                if (fireStun)
                {
                    fireStunTimer += gameTime.ElapsedGameTime;
                    if (fireStunTimer > TimeSpan.FromMilliseconds(STUNRECOVERY))
                    {
                        fireStunTimer = TimeSpan.Zero;
                        fireStun = false;
                    }
                }

                if (!iceStun)
                {
                    if (PlayerOne.ThumbSticks.Left.Length() > 0.1f)
                    {
                        iceShield.Rotation = (float)Math.Atan2(PlayerOne.ThumbSticks.Left.X, PlayerOne.ThumbSticks.Left.Y);
                        iceShieldOver.Rotation = (float)Math.Atan2(PlayerOne.ThumbSticks.Left.X, PlayerOne.ThumbSticks.Left.Y);
                        iceAngle = MathHelper.ToDegrees((float)Math.Atan2(PlayerOne.ThumbSticks.Left.X, -PlayerOne.ThumbSticks.Left.Y));
                        iceAngle += 180;
                        if (iceAngle == 360)
                            iceAngle = 0;
                        else if (iceAngle < 0)
                            iceAngle += 360;
                    }
                }
                if (!fireStun)
                {
                    if (PlayerOne.ThumbSticks.Right.Length() > 0.1f)
                    {
                        fireShield.Rotation = (float)Math.Atan2(PlayerOne.ThumbSticks.Right.X, PlayerOne.ThumbSticks.Right.Y);
                        fireShieldOver.Rotation = (float)Math.Atan2(PlayerOne.ThumbSticks.Right.X, PlayerOne.ThumbSticks.Right.Y);
                        fireAngle = MathHelper.ToDegrees((float)Math.Atan2(PlayerOne.ThumbSticks.Right.X, -PlayerOne.ThumbSticks.Right.Y));
                        fireAngle += 180;
                        if (fireAngle == 360)
                            fireAngle = 0;
                        else if (fireAngle < 0)
                            fireAngle += 360;
                    }
                }

                if(level == 6 || level == 10)
                    HandleReflection(gameTime, PlayerOne);

                HandleAddProjectiles(gameTime);

                if (!bSpellBlastPressed && numSpellBlasts > 0)
                {
                    if (PlayerOne.DPad.Left == ButtonState.Pressed && !bLeftPressed)
                    {
                        bLeftPressed = true;
                        bSpellBlast = true;
                        numSpellBlasts--;
                        bSpellBlastPressed = true;
                        bAnimateLeft = true;
                        if (GameCore.PlaySoundEffects)
                            GameCore.PublicSFXBlastSound.Play();
                        spellBlastLeft.Animate(0, 9);
                    }
                    else if (PlayerOne.DPad.Right == ButtonState.Pressed && !bRightPressed)
                    {
                        bRightPressed = true;
                        bSpellBlast = true;
                        numSpellBlasts--;
                        bSpellBlastPressed = true;
                        bAnimateRight = true;
                        if (GameCore.PlaySoundEffects)
                            GameCore.PublicSFXBlastSound.Play();
                        spellBlastRight.Animate(0, 9);
                    }
                    else if (PlayerOne.DPad.Down == ButtonState.Pressed && !bDownPressed)
                    {
                        bDownPressed = true;
                        bSpellBlast = true;
                        numSpellBlasts--;
                        bSpellBlastPressed = true;
                        bAnimateDown = true;
                        if (GameCore.PlaySoundEffects)
                            GameCore.PublicSFXBlastSound.Play();
                        spellBlastDown.Animate(0, 9);
                    }
                    else if (PlayerOne.DPad.Up == ButtonState.Pressed && !bUpPressed)
                    {
                        bUpPressed = true;
                        bSpellBlast = true;
                        numSpellBlasts--;
                        bSpellBlastPressed = true;
                        bAnimateUp = true;
                        if (GameCore.PlaySoundEffects)
                            GameCore.PublicSFXBlastSound.Play();
                        spellBlastUp.Animate(0, 9);
                    }

                }
                if (bAnimateUp)
                {
                    spellBlastUp.Update(gameTime);
                    if (spellBlastUp.CurrentFrame == 9)
                    {
                        bAnimateUp = false;
                    }
                }
                if (bAnimateDown)
                {
                    spellBlastDown.Update(gameTime);
                    if (spellBlastDown.CurrentFrame == 9)
                    {
                        bAnimateDown = false;
                    }
                }
                if (bAnimateLeft)
                {
                    spellBlastLeft.Update(gameTime);
                    if (spellBlastLeft.CurrentFrame == 9)
                    {
                        bAnimateLeft = false;
                    }
                }
                if (bAnimateRight)
                {
                    spellBlastRight.Update(gameTime);
                    if (spellBlastRight.CurrentFrame == 9)
                    {
                        bAnimateRight = false;
                    }
                }

                if (bLightningDown)
                {
                    lightningDown.Update(gameTime);
                    if (lightningDown.CurrentFrame == 8)
                        bLightningDown = false;
                }
                if (bLightningUp)
                {
                    lightningUp.Update(gameTime);
                    if (lightningUp.CurrentFrame == 8)
                        bLightningUp = false;
                }
                if (bLightningLeft)
                {
                    lightningLeft.Update(gameTime);
                    if (lightningLeft.CurrentFrame == 9)
                        bLightningLeft = false;
                }
                if (bLightningRight)
                {
                    lightningRight.Update(gameTime);
                    if (lightningRight.CurrentFrame == 9)
                        bLightningRight = false;
                }

                if (!bLightningDown && PlayerOne.Buttons.A == ButtonState.Pressed && !bLightningTime)
                {
                    bLightningDown = true;
                    if(GameCore.PlaySoundEffects)
                    GameCore.PublicSFXLightningSound.Play();
                    lightningDown.Animate(0, 8);
                }
                if (!bLightningLeft && PlayerOne.Buttons.X == ButtonState.Pressed && !bLightningTime)
                {
                    bLightningLeft = true;
                    if (GameCore.PlaySoundEffects)
                    GameCore.PublicSFXLightningSound.Play();
                    lightningLeft.Animate(0, 9);
                }
                if (!bLightningRight && PlayerOne.Buttons.B == ButtonState.Pressed && !bLightningTime)
                {
                    bLightningRight = true;
                    if (GameCore.PlaySoundEffects)
                    GameCore.PublicSFXLightningSound.Play();
                    lightningRight.Animate(0, 9);
                }
                if (!bLightningUp && PlayerOne.Buttons.Y == ButtonState.Pressed && !bLightningTime)
                {
                    bLightningUp = true;
                    if (GameCore.PlaySoundEffects)
                    GameCore.PublicSFXLightningSound.Play();
                    lightningUp.Animate(0, 8);
                }


                //handle collision logic here
                for (int i = 0; i < Projectiles.Count; i++)
                {
                    Projectiles[i].Update(gameTime);
                    Vector2 targetVector = new Vector2(640, 360);

                    Projectiles[i].Move(targetVector);
                    targetVector = Projectiles[i].MySprite.Position - targetVector;
                    Vector2 AngleVector = Projectiles[i].MySprite.Position - new Vector2(640, 360);
                    AngleVector.Normalize();
                    /*
                    if (!(Projectiles[i].MyType == Projectile.ProjectileType.Stun))
                    {
                        Projectiles[i].MySprite.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
                        if (Projectiles[i].Reflected)
                        {
                            Projectiles[i].MySprite.Rotation += 179;
                            if (Projectiles[i].MySprite.Rotation >= 360)
                                Projectiles[i].MySprite.Rotation -= 360;
                        }
                    }*/
                    Angle = MathHelper.ToDegrees((float)Math.Atan2(-AngleVector.X, -AngleVector.Y));
                    if (Angle < 0)
                        Angle += 360;
                    //Angle += 180;
                    lowerLimit = Angle - 45;
                    //if it goes below 0, make it go full circle
                    if (lowerLimit < 0)
                    {
                        lowerLimit = 360 + lowerLimit;
                    }
                    upperLimit = Angle + 45;
                    //if it goes beyond 360, wrap it around
                    if (upperLimit >= 360)
                    {
                        upperLimit -= 360;
                    }

                    //handle spell blast logic here
                    if (bSpellBlast)
                    {
                        if (Angle >= 45 && Angle <= 135 && bLeftPressed)
                        {
                            Projectiles.Remove(Projectiles[i]);
                            i--;
                            continue;

                        }
                        else if (Angle >= 225 && Angle <= 315 && bRightPressed)
                        {
                            Projectiles.Remove(Projectiles[i]);
                            i--;
                            continue;
                        }
                        else if (Angle >= 135 && Angle <= 225 && bDownPressed)
                        {
                            Projectiles.Remove(Projectiles[i]);
                            i--;
                            continue;
                        }
                        else if ((Angle >= 315 || Angle <= 45) && bUpPressed)
                        {
                            Projectiles.Remove(Projectiles[i]);
                            i--;
                            continue;
                        }
                    }

                    if (Projectiles[i].MyType == Projectile.ProjectileType.Arrow)
                    {
                        if (Math.Abs(Projectiles[i].MovementDir.X) < 50)
                        {
                            if ((Projectiles[i].MovementDir.Y > 0 && bLightningUp) || (Projectiles[i].MovementDir.Y < 0 && bLightningDown))
                            {
                                Projectiles.Remove(Projectiles[i]);
                                i--;
                                score += 30;
                                continue;
                            }
                        }
                        else
                        {
                            if ((Projectiles[i].MovementDir.X > 0 && bLightningLeft) || (Projectiles[i].MovementDir.X < 0 && bLightningRight))
                            {
                                Projectiles.Remove(Projectiles[i]);
                                i--;
                                score += 30;
                                continue;
                            }
                        }
                    }
                    //is the projectile in range to be countered?
                    if (targetVector.Length() > 140 && targetVector.Length() < 200 && !Projectiles[i].Reflected)
                    {



                        switch (Projectiles[i].MyType)
                        {


                            case Projectile.ProjectileType.Antipode:
                                //in the normal part of the circle
                                if (iceAlpha > 220 && fireAlpha > 190)
                                {
                                    if (upperLimit - lowerLimit > 89.99f && upperLimit - lowerLimit < 90.01f)
                                    {
                                        if (fireAngle > lowerLimit && fireAngle < upperLimit && iceAngle > lowerLimit && iceAngle < upperLimit)
                                        {
                                            if (bIsReflecting)
                                            {
                                                Projectiles[i].Reflected = true;
                                                Vector2 tempAngleVector = Projectiles[i].MySprite.Position - new Vector2(640, 360);
                                                tempAngleVector.Normalize();
                                                if (!(Projectiles[i].MyType == Projectile.ProjectileType.Stun))
                                                {
                                                    Projectiles[i].MySprite.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
                                                    if (Projectiles[i].Reflected)
                                                    {
                                                        Projectiles[i].MySprite.Rotation += 179;
                                                        if (Projectiles[i].MySprite.Rotation >= 360)
                                                            Projectiles[i].MySprite.Rotation -= 360;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Projectiles.Remove(Projectiles[i]);
                                                i--;
                                            }
                                            //fireHitColour = 255;
                                            //iceHitColour = 255;
                                            if (numSpellBlasts < 3)
                                                numDeflectCounter++;
                                            if (numDeflectCounter == 30)
                                            {
                                                numDeflectCounter = 0;
                                                numSpellBlasts++;
                                            }
                                            fireShield.MyColour = new Color(255, 255, 255);
                                            iceShield.MyColour = new Color(255, 255, 255);
                                            fireShieldOver.MyColour = new Color(255, 255, 255);
                                            if (++difficultyCounter >= BULLETSTOADDDIFFICULTY)
                                            {
                                                difficultyCounter = 0;
                                                difficultyMod++;
                                            }

                                            //increment score
                                            if (level != 0)
                                                score += (ushort)(15 + (ushort)(targetVector.Length() - 180));
                                            continue;
                                        }
                                    }
                                    //lower limit is in the 300's
                                    else
                                    {
                                        if ((fireAngle > lowerLimit || fireAngle < upperLimit) && (iceAngle > lowerLimit || iceAngle < upperLimit))
                                        {
                                            if (bIsReflecting)
                                            {
                                                Projectiles[i].Reflected = true;
                                                Vector2 tempAngleVector = Projectiles[i].MySprite.Position - new Vector2(640, 360);
                                                tempAngleVector.Normalize();
                                                if (!(Projectiles[i].MyType == Projectile.ProjectileType.Stun))
                                                {
                                                    Projectiles[i].MySprite.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
                                                    if (Projectiles[i].Reflected)
                                                    {
                                                        Projectiles[i].MySprite.Rotation += 179;
                                                        if (Projectiles[i].MySprite.Rotation >= 360)
                                                            Projectiles[i].MySprite.Rotation -= 360;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Projectiles.Remove(Projectiles[i]);
                                                i--;
                                            }
                                            //fireHitColour = 255;
                                            //iceHitColour = 255;
                                            if (numSpellBlasts < 3)
                                                numDeflectCounter++;
                                            if (numDeflectCounter == 30)
                                            {
                                                numDeflectCounter = 0;
                                                numSpellBlasts++;
                                            }
                                            fireShield.MyColour = new Color(255, 255, 255);
                                            iceShield.MyColour = new Color(255, 255, 255);
                                            fireShieldOver.MyColour = new Color(255, 255, 255);
                                            if (++difficultyCounter >= BULLETSTOADDDIFFICULTY)
                                            {
                                                difficultyCounter = 0;
                                                difficultyMod++;
                                            }
                                            //increment score
                                            if (level != 0)
                                                score += (ushort)(15 + (ushort)(targetVector.Length() - 180));
                                            continue;

                                        }
                                    }
                                }
                                break;
                            case Projectile.ProjectileType.Fire:
                                //in the normal part of the circle
                                if (fireAlpha > 190)
                                {
                                    if (upperLimit - lowerLimit > 89.99f && upperLimit - lowerLimit < 90.01f)
                                    {
                                        if (fireAngle > lowerLimit && fireAngle < upperLimit)
                                        {
                                            if (bIsReflecting)
                                            {
                                                Projectiles[i].Reflected = true;
                                                Vector2 tempAngleVector = Projectiles[i].MySprite.Position - new Vector2(640, 360);
                                                tempAngleVector.Normalize();
                                                if (!(Projectiles[i].MyType == Projectile.ProjectileType.Stun))
                                                {
                                                    Projectiles[i].MySprite.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
                                                    if (Projectiles[i].Reflected)
                                                    {
                                                        Projectiles[i].MySprite.Rotation += 179;
                                                        if (Projectiles[i].MySprite.Rotation >= 360)
                                                            Projectiles[i].MySprite.Rotation -= 360;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Projectiles.Remove(Projectiles[i]);
                                                i--;
                                            }
                                            if (numSpellBlasts < 3)
                                                numDeflectCounter++;
                                            if (numDeflectCounter == 30)
                                            {
                                                numDeflectCounter = 0;
                                                numSpellBlasts++;
                                            }
                                            //fireHitColour = 255;
                                            fireShield.MyColour = new Color(255, 255, 255);
                                            fireShieldOver.MyColour = new Color(255, 255, 255);
                                            if (++difficultyCounter >= BULLETSTOADDDIFFICULTY)
                                            {
                                                difficultyCounter = 0;
                                                difficultyMod++;
                                            }
                                            //increment score
                                            if (level != 0)
                                                score += (ushort)(10 + (ushort)((targetVector.Length() - 180) / 2));
                                            continue;
                                        }
                                    }
                                    //lower limit is in the 300's
                                    else
                                    {
                                        if (fireAngle > lowerLimit || fireAngle < upperLimit)
                                        {
                                            if (bIsReflecting)
                                            {
                                                Projectiles[i].Reflected = true;
                                                Vector2 tempAngleVector = Projectiles[i].MySprite.Position - new Vector2(640, 360);
                                                tempAngleVector.Normalize();
                                                if (!(Projectiles[i].MyType == Projectile.ProjectileType.Stun))
                                                {
                                                    Projectiles[i].MySprite.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
                                                    if (Projectiles[i].Reflected)
                                                    {
                                                        Projectiles[i].MySprite.Rotation += 179;
                                                        if (Projectiles[i].MySprite.Rotation >= 360)
                                                            Projectiles[i].MySprite.Rotation -= 360;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Projectiles.Remove(Projectiles[i]);
                                                i--;
                                            }
                                            if (numSpellBlasts < 3)
                                                numDeflectCounter++;
                                            if (numDeflectCounter == 30)
                                            {
                                                numDeflectCounter = 0;
                                                numSpellBlasts++;
                                            }
                                            //fireHitColour = 255;
                                            fireShield.MyColour = new Color(255, 255, 255);
                                            fireShieldOver.MyColour = new Color(255, 255, 255);
                                            if (++difficultyCounter >= BULLETSTOADDDIFFICULTY)
                                            {
                                                difficultyCounter = 0;
                                                difficultyMod++;
                                            }
                                            //increment score
                                            if (level != 0)
                                                score += (ushort)(10 + (ushort)((targetVector.Length() - 180) / 2));
                                            continue;

                                        }
                                    }
                                }
                                break;
                            case Projectile.ProjectileType.Ice:
                                if (iceAlpha > 220)
                                {
                                    if (upperLimit - lowerLimit > 89.99f && upperLimit - lowerLimit < 90.01f)
                                    {
                                        if (iceAngle > lowerLimit && iceAngle < upperLimit)
                                        {
                                            if (bIsReflecting)
                                            {
                                                Projectiles[i].Reflected = true;
                                                Vector2 tempAngleVector = Projectiles[i].MySprite.Position - new Vector2(640, 360);
                                                tempAngleVector.Normalize();
                                                if (!(Projectiles[i].MyType == Projectile.ProjectileType.Stun))
                                                {
                                                    Projectiles[i].MySprite.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
                                                    if (Projectiles[i].Reflected)
                                                    {
                                                        Projectiles[i].MySprite.Rotation += 179;
                                                        if (Projectiles[i].MySprite.Rotation >= 360)
                                                            Projectiles[i].MySprite.Rotation -= 360;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Projectiles.Remove(Projectiles[i]);
                                                i--;
                                            }
                                            if (numSpellBlasts < 3)
                                                numDeflectCounter++;
                                            if (numDeflectCounter == 30)
                                            {
                                                numDeflectCounter = 0;
                                                numSpellBlasts++;
                                            }
                                            //iceHitColour = 255;
                                            iceShield.MyColour = new Color(255, 255, 255);

                                            if (++difficultyCounter >= BULLETSTOADDDIFFICULTY)
                                            {
                                                difficultyCounter = 0;
                                                difficultyMod++;
                                            }
                                            //increment score
                                            if (level != 0)
                                                score += (ushort)(10 + (ushort)((targetVector.Length() - 180) / 2));
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        if (iceAngle > lowerLimit || iceAngle < upperLimit)
                                        {
                                            if (bIsReflecting)
                                            {
                                                Projectiles[i].Reflected = true;
                                                Vector2 tempAngleVector = Projectiles[i].MySprite.Position - new Vector2(640, 360);
                                                tempAngleVector.Normalize();
                                                if (!(Projectiles[i].MyType == Projectile.ProjectileType.Stun))
                                                {
                                                    Projectiles[i].MySprite.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
                                                    if (Projectiles[i].Reflected)
                                                    {
                                                        Projectiles[i].MySprite.Rotation += 179;
                                                        if (Projectiles[i].MySprite.Rotation >= 360)
                                                            Projectiles[i].MySprite.Rotation -= 360;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Projectiles.Remove(Projectiles[i]);
                                                i--;
                                            }
                                            if (numSpellBlasts < 3)
                                                numDeflectCounter++;
                                            if (numDeflectCounter == 30)
                                            {
                                                numDeflectCounter = 0;
                                                numSpellBlasts++;
                                            }
                                            //iceHitColour = 255;
                                            iceShield.MyColour = new Color(255, 255, 255);
                                            if (++difficultyCounter >= BULLETSTOADDDIFFICULTY)
                                            {
                                                difficultyCounter = 0;
                                                difficultyMod++;
                                            }
                                            //increment score
                                            if (level != 0)
                                                score += (ushort)(10 + (ushort)((targetVector.Length() - 180) / 2));
                                            continue;
                                        }
                                    }
                                }
                                break;
                            case Projectile.ProjectileType.Stun:
                                bool hasCollided = false;
                                if (upperLimit - lowerLimit > 89.99f && upperLimit - lowerLimit < 90.01f)
                                {
                                    if (iceAngle > lowerLimit && iceAngle < upperLimit && iceAlpha > 220)
                                    {
                                        //Projectiles.Remove(Projectiles[i]);
                                        //i--;
                                        //iceHitColour = 255;
                                        //iceShield.MyColour = new Color(255, 255, 255);
                                        //continue;
                                        if (bIsReflecting)
                                        {
                                            reflectPercent -= 15;
                                            if (reflectPercent < 0)
                                            {
                                                reflectPercent = 0;
                                                reflectTimer = TimeSpan.Zero;
                                                bIsReflecting = false;
                                            }
                                        }
                                        else
                                            iceStun = true;
                                        hasCollided = true;

                                    }
                                    if (fireAngle > lowerLimit && fireAngle < upperLimit && fireAlpha > 190)
                                    {
                                        //Projectiles.Remove(Projectiles[i]);
                                        //i--;
                                        //iceHitColour = 255;
                                        //iceShield.MyColour = new Color(255, 255, 255);
                                        //continue;
                                        if (bIsReflecting)
                                        {
                                            reflectPercent -= 15;
                                            if (reflectPercent < 0)
                                            {
                                                reflectPercent = 0;
                                                reflectTimer = TimeSpan.Zero;
                                                bIsReflecting = false;
                                            }
                                        }
                                        else
                                            fireStun = true;
                                        hasCollided = true;

                                    }

                                }
                                else
                                {
                                    if ((iceAngle > lowerLimit || iceAngle < upperLimit) && iceAlpha > 220)
                                    {
                                        //Projectiles.Remove(Projectiles[i]);
                                        //i--;
                                        //iceHitColour = 255;
                                        //iceShield.MyColour = new Color(255, 255, 255);
                                        //continue;
                                        if (bIsReflecting)
                                        {
                                            reflectPercent -= 15;
                                            if (reflectPercent < 0)
                                            {
                                                reflectPercent = 0;
                                                reflectTimer = TimeSpan.Zero;
                                                bIsReflecting = false;
                                            }
                                        }
                                        else
                                            iceStun = true;
                                        hasCollided = true;

                                    }
                                    if ((fireAngle > lowerLimit || fireAngle < upperLimit) && fireAlpha > 190)
                                    {
                                        //Projectiles.Remove(Projectiles[i]);
                                        //i--;
                                        //iceHitColour = 255;
                                        //iceShield.MyColour = new Color(255, 255, 255);
                                        //continue;
                                        if (bIsReflecting)
                                        {
                                            reflectPercent -= 15;
                                            if (reflectPercent < 0)
                                            {
                                                reflectPercent = 0;
                                                reflectTimer = TimeSpan.Zero;
                                                bIsReflecting = false;
                                            }
                                        }
                                        else
                                            fireStun = true;
                                        hasCollided = true;
                                    }
                                }
                                if (hasCollided)
                                {
                                    Projectiles.Remove(Projectiles[i]);
                                    i--;
                                    continue;
                                }
                                break;
                            case Projectile.ProjectileType.Counter:
                                hasCollided = false;
                                if (upperLimit - lowerLimit > 89.99f && upperLimit - lowerLimit < 90.01f)
                                {
                                    if (iceAngle > lowerLimit && iceAngle < upperLimit && iceAlpha > 220)
                                    {
                                        //Projectiles.Remove(Projectiles[i]);
                                        //i--;
                                        //iceHitColour = 255;
                                        //iceShield.MyColour = new Color(255, 255, 255);
                                        //continue;
                                        if (bIsReflecting)
                                        {
                                            reflectPercent -= 15;
                                            if (reflectPercent < 0)
                                            {
                                                reflectPercent = 0;
                                                reflectTimer = TimeSpan.Zero;
                                                bIsReflecting = false;
                                            }
                                        }
                                        else
                                            iceAlpha = 0;
                                        hasCollided = true;
                                    }
                                    if (fireAngle > lowerLimit && fireAngle < upperLimit && fireAlpha > 190)
                                    {
                                        //Projectiles.Remove(Projectiles[i]);
                                        //i--;
                                        //iceHitColour = 255;
                                        //iceShield.MyColour = new Color(255, 255, 255);
                                        //continue;
                                        if (bIsReflecting)
                                        {
                                            reflectPercent -= 15;
                                            if (reflectPercent < 0)
                                            {
                                                reflectPercent = 0;
                                                reflectTimer = TimeSpan.Zero;
                                                bIsReflecting = false;
                                            }
                                        }
                                        else
                                            fireAlpha = 0;
                                        hasCollided = true;
                                    }

                                }
                                else
                                {
                                    if ((iceAngle > lowerLimit || iceAngle < upperLimit) && iceAlpha > 220)
                                    {
                                        //Projectiles.Remove(Projectiles[i]);
                                        //i--;
                                        //iceHitColour = 255;
                                        //iceShield.MyColour = new Color(255, 255, 255);
                                        //continue;
                                        if (bIsReflecting)
                                        {
                                            reflectPercent -= 15;
                                            if (reflectPercent < 0)
                                            {
                                                reflectPercent = 0;
                                                reflectTimer = TimeSpan.Zero;
                                                bIsReflecting = false;
                                            }
                                        }
                                        else
                                            iceAlpha = 0;
                                        hasCollided = true;
                                    }
                                    if ((fireAngle > lowerLimit || fireAngle < upperLimit) && fireAlpha > 190)
                                    {
                                        //Projectiles.Remove(Projectiles[i]);
                                        //i--;
                                        //iceHitColour = 255;
                                        //iceShield.MyColour = new Color(255, 255, 255);
                                        //continue;
                                        if (bIsReflecting)
                                        {
                                            reflectPercent -= 15;
                                            if (reflectPercent < 0)
                                            {
                                                reflectPercent = 0;
                                                reflectTimer = TimeSpan.Zero;
                                                bIsReflecting = false;
                                            }
                                        }
                                        else
                                            fireAlpha = 0;
                                        hasCollided = true;
                                    }
                                }
                                if (hasCollided)
                                {
                                    Projectiles.Remove(Projectiles[i]);
                                    i--;
                                    continue;
                                }
                                break;

                        }
                    }
                    else if (targetVector.Length() < 90)
                    {
                        if (level != 0)
                        {
                            switch (Projectiles[i].MyType)
                            {
                                case Projectile.ProjectileType.Ice:
                                case Projectile.ProjectileType.Fire:
                                    health -= 5;
                                    GamePad.SetVibration(POne, 0.6f, 0.6f);
                                    isHit = true;
                                    rumbleTimer = TimeSpan.Zero;
                                    break;
                                case Projectile.ProjectileType.Antipode:
                                    health -= 8;
                                    GamePad.SetVibration(POne, 0.6f, 0.6f);
                                    isHit = true;
                                    rumbleTimer = TimeSpan.Zero;
                                    break;
                                case Projectile.ProjectileType.Arrow:
                                    health -= 10;
                                    GamePad.SetVibration(POne, 0.6f, 0.6f);
                                    isHit = true;
                                    rumbleTimer = TimeSpan.Zero;
                                    break;
                                case Projectile.ProjectileType.Counter:
                                case Projectile.ProjectileType.Stun:
                                    if (++difficultyCounter >= BULLETSTOADDDIFFICULTY)
                                    {
                                        difficultyCounter = 0;
                                        difficultyMod++;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            GamePad.SetVibration(POne, 0.6f, 0.6f);
                            isHit = true;
                            rumbleTimer = TimeSpan.Zero;
                        }
                        Projectiles.Remove(Projectiles[i]);
                        i--;
                        if (health <= 0)
                        {
                            health = 0;
                            break;
                        }
                        // hitCount++;
                        continue;
                    }
                    else if (Projectiles[i].Reflected && (Projectiles[i].MySprite.Position.X < -50 || Projectiles[i].MySprite.Position.X > 1280 || Projectiles[i].MySprite.Position.Y < -50 || Projectiles[i].MySprite.Position.Y > 720 ||
                        (level == 6 && tempDragon.GetCollision(Projectiles[i].MySprite)) || (level == 10 && sunPriest.GetCollision(Projectiles[i].MySprite))))
                    {
                        Projectiles.Remove(Projectiles[i]);
                        i--;
                    }

                }

                //stuff for the center
                switch (level)
                {
                    case 3:
                    case 5:
                        centerRune.Update(gameTime);
                        break;
                    case 2:
                    case 4:
                    
                    case 8:
                    case 9:
                    default:
                        centerStone.Update(gameTime);
                        break;
                    case 6:
                        centerStone.Update(gameTime);
                        tempDragon.Update(gameTime);
                        break;
                    case 10:
                        sunPriest.Update(gameTime);
                        centerStone.Update(gameTime);
                        break;
                }

                //centerDot.MyColour = new Color((new Vector4(255,255,255, (byte)(((float)(100 - health) / 100) * 255)) ));

                //tempDragon.Update(gameTime);
                //sunPriest.Update(gameTime);

                bSpellBlast = false;
                //handle colour effects
                if (fireShield.MyColour.R > fireRed)
                    tempfireRed = (byte)(fireShield.MyColour.R - 2);
                else if (fireShield.MyColour.R < fireRed)
                    tempfireRed = fireRed;
                if (fireShield.MyColour.G > fireGreen)
                    tempfireGreen = (byte)(fireShield.MyColour.G - 2);
                else if (fireShield.MyColour.G < fireGreen)
                    tempfireGreen = fireGreen;
                if (fireShield.MyColour.B > fireBlue)
                    tempfireBlue = (byte)(fireShield.MyColour.B - 2);
                else if (fireShield.MyColour.B < fireBlue)
                    tempfireBlue = fireBlue;

                if (fireShieldOver.MyColour.R > fireOverRed)
                    tempfireOverRed = (byte)(fireShieldOver.MyColour.R - 2);
                else if (fireShieldOver.MyColour.R < fireOverRed)
                    tempfireOverRed = fireOverRed;
                if (fireShieldOver.MyColour.G > fireOverGreen)
                    tempfireOverGreen = (byte)(fireShieldOver.MyColour.G - 2);
                else if (fireShieldOver.MyColour.G < fireOverGreen)
                    tempfireOverGreen = fireOverGreen;
                if (fireShieldOver.MyColour.B > 30)
                    tempfireOverBlue = (byte)(fireShieldOver.MyColour.B - 2);
                else
                    tempfireOverBlue = fireOverBlue;

                if (iceAlpha < 255)
                {
                    //iceCounterTimer += gameTime.ElapsedGameTime;
                    //if(iceCounterTimer >= TimeSpan.FromMilliseconds(80))
                    //{
                    iceAlpha++;
                    //    iceCounterTimer -= TimeSpan.FromMilliseconds(80);
                    //}
                }

                fireShield.MyColour = new Color((int)(tempfireRed), (int)(tempfireGreen), (int)(tempfireBlue));
                fireShield.MyAlpha = (float)(fireAlpha)/255;
                fireShieldOver.MyColour = new Color(tempfireOverRed, tempfireOverGreen, tempfireOverBlue);
                fireShieldOver.MyAlpha = (float)(fireAlpha)/255;

                if (iceShield.MyColour.R > iceRed)
                    tempiceRed = (byte)(iceShield.MyColour.R - 2);
                else if (iceShield.MyColour.R < iceRed)
                    tempiceRed = iceRed;
                if (iceShield.MyColour.G > iceGreen)
                    tempiceGreen = (byte)(iceShield.MyColour.G - 2);
                else if (iceShield.MyColour.G < iceGreen)
                    tempiceGreen = iceGreen;
                if (iceShield.MyColour.B > iceBlue)
                    tempiceBlue = (byte)(iceShield.MyColour.B - 2);
                else if (iceShield.MyColour.B < iceBlue)
                    tempiceBlue = iceBlue;

                if (fireAlpha < 210)
                {
                    //fireCounterTimer += gameTime.ElapsedGameTime;
                    //if (fireCounterTimer >= TimeSpan.FromMilliseconds(30))
                    //{
                    fireAlpha++;
                    //    fireCounterTimer -= TimeSpan.FromMilliseconds(30);
                    //}
                }

                iceShield.MyColour = new Color(tempiceRed, tempiceGreen, tempiceBlue);
                iceShield.MyAlpha = (float)(iceAlpha)/255;
                iceShieldOver.MyColour = new Color(255, 255, 255);
                iceShieldOver.MyAlpha = (float)(iceAlpha)/255;
                //if (iceAlpha < 100 && iceAlpha > 50)
                //    iceShield.MyAlpha = iceShield.MyAlpha;

                if (PlayerOne.Buttons.Start == ButtonState.Released)
                    bStartPressed = false;
                if (PlayerOne.DPad.Down == ButtonState.Released)
                    bDownPressed = false;
                if (PlayerOne.DPad.Up == ButtonState.Released)
                    bUpPressed = false;
                if (PlayerOne.DPad.Left == ButtonState.Released)
                    bLeftPressed = false;
                if (PlayerOne.DPad.Right == ButtonState.Released)
                    bRightPressed = false;

                if (!bDownPressed && !bUpPressed && !bLeftPressed && !bRightPressed)
                {
                    bSpellBlastPressed = false;
                }



                base.Update(gameTime);
            }//end if not paused
        }//end update

        public void HandleAddProjectiles(GameTime gameTime)
        {
            //handle timer stuff
            myTimer += gameTime.ElapsedGameTime;
            
            switch (level)
            {
                case 0:
                    
                    if (myTimer > TimeSpan.FromMilliseconds(BASEADDBULLETTIME * 3))
                    {
                        //return this after testing
                        AddBullet();
                        //AddBullet(Projectile.ProjectileType.Fire, tempDragon.LeftSpawn);
                        //AddBullet(Projectile.ProjectileType.Ice, tempDragon.RightSpawn);
                        //rotation += 0.1f;
                        myTimer -= TimeSpan.FromMilliseconds(BASEADDBULLETTIME * 3);
                    }
                   
                    break;
                case 1:
                case 2:
                    if (myTimer > TimeSpan.FromMilliseconds(BASEADDBULLETTIME * 2 + addBulletRate))
                    {
                        if (random.Next(100) < (BASEADDBULLETPERCENT * 2+ addBulletPercent))
                        {

                            AddBullet();
                            //rotation += 0.1f;
                            myTimer -= TimeSpan.FromMilliseconds(BASEADDBULLETTIME * 2+ addBulletRate);
                            addBulletRate = 0;
                            addBulletPercent = 0;
                        }
                        else
                        {
                            addBulletRate += ADDBULLETTIME;
                            addBulletPercent += ADDBULLETPERCENT;
                        }
                    }
                    break;
                case 3:
                case 4:
                    switch (myPattern)
                    {
                        case ProjectilePattern.NONE:
                            if (myTimer > TimeSpan.FromMilliseconds(BASEADDBULLETTIME * 1.7 + addBulletRate))
                            {
                                if (random.Next(100) < (BASEADDBULLETPERCENT * 1.7 + addBulletPercent))
                                {

                                    AddBullet();
                                    //rotation += 0.1f;
                                    myTimer -= TimeSpan.FromMilliseconds(BASEADDBULLETTIME * 1.7 + addBulletRate);
                                    addBulletRate = 0;
                                    addBulletPercent = 0;
                                    //INCLUDE THIS FOR ATTACK PATTERNS
                                    patternCounter++;
                                    if (patternCounter == 15)
                                    {
                                        patternCounter = 0;

                                        if (random.Next(100) < 30)
                                            myPattern = ProjectilePattern.COUNTER_SPIN;
                                        
                                    }
                                }
                                else
                                {
                                    addBulletRate += ADDBULLETTIME;
                                    addBulletPercent += ADDBULLETPERCENT;
                                }
                            }
                            break;
                        case ProjectilePattern.COUNTER_SPIN:
                            if (myTimer > TimeSpan.FromMilliseconds(1500))
                            {
                                myTimer -= TimeSpan.FromMilliseconds(1500);
                                if (patternCounter == 0)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(-50, 345));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(1280, 345));
                                }
                                else if (patternCounter == 1)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(-50, -50));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(1280, 720));
                                    
                                }
                                else if (patternCounter == 2)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(625, -50));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(625, 720));
                                    
                                }
                                else if (patternCounter == 3)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(1280, -50));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(-50, 720));
                                    
                                }
                                else if (patternCounter == 4)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(1280, 345));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(-50, 345));
                                    
                                }
                                else if (patternCounter == 5)
                                {
                                    patternCounter = 0;
                                    myPattern = ProjectilePattern.NONE;
                                }
                                
                                patternCounter++;
                            }
                            break;
                    }
                    break;
                case 5:
                    if (myTimer > TimeSpan.FromMilliseconds(BASEADDBULLETTIME * 1.5 + addBulletRate))
                    {
                        if (random.Next(100) < (BASEADDBULLETPERCENT * 1.5 + addBulletPercent))
                        {

                            AddBullet();
                            //rotation += 0.1f;
                            myTimer -= TimeSpan.FromMilliseconds(BASEADDBULLETTIME * 1.5 + addBulletRate);
                            addBulletRate = 0;
                            addBulletPercent = 0;
                        }
                        else
                        {
                            addBulletRate += ADDBULLETTIME;
                            addBulletPercent += ADDBULLETPERCENT;
                        }
                    }
                    arrowTimer += gameTime.ElapsedGameTime;

                    if (arrowTimer > TimeSpan.FromMilliseconds(BASEADDARROWTIME + addArrowRate))
                    {
                        if (random.Next(100) < (BASEADDBULLETPERCENT + addArrowPercent))
                        {

                            AddArrow();
                            //rotation += 0.1f;
                            arrowTimer -= TimeSpan.FromMilliseconds(BASEADDARROWTIME + addArrowRate);
                            addArrowRate = 0;
                            addArrowPercent = 0;
                        }
                        else
                        {
                            addArrowRate += ADDBULLETTIME;
                            addArrowPercent += ADDBULLETPERCENT;
                        }
                    }
                    break;
                case 6:
                    if (tempDragon.Health > 0)
                    {
                        if (myTimer > TimeSpan.FromMilliseconds(BASEADDBULLETTIME + addBulletRate))
                        {
                            if (random.Next(100) < (BASEADDBULLETPERCENT + addBulletPercent))
                            {
                                if (random.Next(100) > 50)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, tempDragon.LeftSpawn);
                                    AddBullet(Projectile.ProjectileType.Ice, tempDragon.RightSpawn);
                                }
                                else
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, tempDragon.RightSpawn);
                                    AddBullet(Projectile.ProjectileType.Ice, tempDragon.LeftSpawn);
                                }
                                //AddBullet();
                                //rotation += 0.1f;
                                myTimer -= TimeSpan.FromMilliseconds(BASEADDBULLETTIME + addBulletRate);
                                addBulletRate = 0;
                                addBulletPercent = 0;
                            }
                            else
                            {
                                addBulletRate += ADDBULLETTIME;
                                addBulletPercent += ADDBULLETPERCENT;
                            }
                        }
                        //arrowTimer += gameTime.ElapsedGameTime;

                        if (arrowTimer > TimeSpan.FromMilliseconds(BASEADDARROWTIME + addArrowRate))
                        {
                            if (random.Next(100) < (BASEADDBULLETPERCENT + addArrowPercent))
                            {

                                AddArrow();
                                //rotation += 0.1f;
                                arrowTimer -= TimeSpan.FromMilliseconds(BASEADDARROWTIME + addArrowRate);
                                addArrowRate = 0;
                                addArrowPercent = 0;
                            }
                            else
                            {
                                addArrowRate += ADDBULLETTIME;
                                addArrowPercent += ADDBULLETPERCENT;
                            }
                        }
                    }
                    break;
                case 7:
                    switch(myPattern)
                    {
                        case ProjectilePattern.NONE:
                            if (myTimer > TimeSpan.FromMilliseconds(BASEADDBULLETTIME + addBulletRate))
                            {
                                if (random.Next(100) < (BASEADDBULLETPERCENT + addBulletPercent))
                                {

                                    AddBullet();
                                    //rotation += 0.1f;
                                    myTimer -= TimeSpan.FromMilliseconds(BASEADDBULLETTIME + addBulletRate);
                                    addBulletRate = 0;
                                    addBulletPercent = 0;
                                    //INCLUDE THIS FOR ATTACK PATTERNS
                                    patternCounter++;
                                    if (patternCounter == 15)
                                    {
                                        patternCounter = 0;

                                        if (random.Next(100) < 10)
                                            myPattern = ProjectilePattern.COUNTER_SPIN;
                                        else if (random.Next(100) < 40)
                                            myPattern = ProjectilePattern.BALL_STUN_COUNTER;
                                    }
                                }
                                else
                                {
                                    addBulletRate += ADDBULLETTIME;
                                    addBulletPercent += ADDBULLETPERCENT;
                                }
                            }
                            break;
                        case ProjectilePattern.COUNTER_SPIN:
                            if (myTimer > TimeSpan.FromMilliseconds(1500))
                            {
                                myTimer -= TimeSpan.FromMilliseconds(1500);
                                if (patternCounter == 0)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(-50, 345));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(1280, 345));
                                }
                                else if (patternCounter == 1)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(-50, -50));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(1280, 720));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(-50, 345));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(1280, 345));
                                }
                                else if (patternCounter == 2)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(625, -50));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(625, 720));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(-50, -50));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(1280, 720));
                                }
                                else if (patternCounter == 3)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(1280, -50));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(-50, 720));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(625, -50));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(625, 720));
                                }
                                else if (patternCounter == 4)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(1280, 345));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(-50, 345));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(-50, -50));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(1280, 720));
                                }
                                else if (patternCounter == 5)
                                {
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(1280, 345));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(-50, 345));
                                }
                                else if (patternCounter == 6)
                                {
                                    patternCounter = 0;
                                    myPattern = ProjectilePattern.NONE;
                                }
                                patternCounter++;
                            }
                            break;
                        case ProjectilePattern.BALL_STUN_COUNTER:
                            if (myTimer > TimeSpan.FromMilliseconds(1300))
                            {
                                myTimer -= TimeSpan.FromMilliseconds(1300);
                                if (patternCounter == 1)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(-50, 360));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(1330, 360));
                                }
                                else if (patternCounter == 2)
                                {
                                    AddBullet(Projectile.ProjectileType.Stun, new Vector2(-50, 360));
                                    AddBullet(Projectile.ProjectileType.Stun, new Vector2(1330, 360));
                                }
                                else if (patternCounter == 3)
                                {
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(-50, 360));
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(1330, 360));
                                }
                                else if (patternCounter == 4)
                                {
                                    AddBullet(Projectile.ProjectileType.Stun, new Vector2(-50, 360));
                                    AddBullet(Projectile.ProjectileType.Stun, new Vector2(1330, 360));
                                }
                                else if (patternCounter == 5)
                                {
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(-50, 360));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(1330, 360));
                                }
                                else if (patternCounter == 6)
                                {
                                    patternCounter = 0;
                                    myPattern = ProjectilePattern.NONE;
                                }
                                patternCounter++;
                            }
                            break;
                    }
                    break;

                case 8:
                    switch (myPattern)
                    {
                        case ProjectilePattern.NONE:
                            if (myTimer > TimeSpan.FromMilliseconds(BASEADDBULLETTIME + addBulletRate))
                            {
                                if (random.Next(100) < (BASEADDBULLETPERCENT + addBulletPercent))
                                {

                                    AddBullet();
                                    //rotation += 0.1f;
                                    myTimer -= TimeSpan.FromMilliseconds(BASEADDBULLETTIME + addBulletRate);
                                    addBulletRate = 0;
                                    addBulletPercent = 0;
                                    //INCLUDE THIS FOR ATTACK PATTERNS
                                    patternCounter++;
                                    if (patternCounter == 15)
                                    {
                                        patternCounter = 0;

                                        if (random.Next(100) < 20)
                                            myPattern = ProjectilePattern.COUNTER_SPIN;
                                        else if (random.Next(100) < 50)
                                            myPattern = ProjectilePattern.BALL_STUN_COUNTER;
                                    }
                                }
                                else
                                {
                                    addBulletRate += ADDBULLETTIME;
                                    addBulletPercent += ADDBULLETPERCENT;
                                }
                            }
                            break;
                        case ProjectilePattern.COUNTER_SPIN:
                            if (myTimer > TimeSpan.FromMilliseconds(1500))
                            {
                                myTimer -= TimeSpan.FromMilliseconds(1500);
                                if (patternCounter == 0)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(-50, 345));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(1280, 345));
                                }
                                else if (patternCounter == 1)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(-50, -50));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(1280, 720));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(-50, 345));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(1280, 345));
                                }
                                else if (patternCounter == 2)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(625, -50));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(625, 720));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(-50, -50));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(1280, 720));
                                }
                                else if (patternCounter == 3)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(1280, -50));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(-50, 720));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(625, -50));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(625, 720));
                                }
                                else if (patternCounter == 4)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(1280, 345));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(-50, 345));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(-50, -50));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(1280, 720));
                                }
                                else if (patternCounter == 5)
                                {
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(1280, 345));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(-50, 345));
                                }
                                else if (patternCounter == 6)
                                {
                                    patternCounter = 0;
                                    myPattern = ProjectilePattern.NONE;
                                }
                                patternCounter++;
                            }
                            break;
                        case ProjectilePattern.BALL_STUN_COUNTER:
                            if (myTimer > TimeSpan.FromMilliseconds(1300))
                            {
                                myTimer -= TimeSpan.FromMilliseconds(1300);
                                if (patternCounter == 1)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(-50, 360));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(1330, 360));
                                }
                                else if (patternCounter == 2)
                                {
                                    AddBullet(Projectile.ProjectileType.Stun, new Vector2(-50, 360));
                                    AddBullet(Projectile.ProjectileType.Stun, new Vector2(1330, 360));
                                }
                                else if (patternCounter == 3)
                                {
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(-50, 360));
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(1330, 360));
                                }
                                else if (patternCounter == 4)
                                {
                                    AddBullet(Projectile.ProjectileType.Stun, new Vector2(-50, 360));
                                    AddBullet(Projectile.ProjectileType.Stun, new Vector2(1330, 360));
                                }
                                else if (patternCounter == 5)
                                {
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(-50, 360));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(1330, 360));
                                }
                                else if (patternCounter == 6)
                                {
                                    patternCounter = 0;
                                    myPattern = ProjectilePattern.NONE;
                                }
                                patternCounter++;
                            }
                            break;
                    }
                    break;
                case 9:
                    if (myTimer > TimeSpan.FromMilliseconds(BASEADDBULLETTIME + addBulletRate))
                    {
                        if (random.Next(100) < (BASEADDBULLETPERCENT + addBulletPercent))
                        {

                            AddBullet();
                            //rotation += 0.1f;
                            myTimer -= TimeSpan.FromMilliseconds(BASEADDBULLETTIME + addBulletRate);
                            addBulletRate = 0;
                            addBulletPercent = 0;
                        }
                        else
                        {
                            addBulletRate += ADDBULLETTIME;
                            addBulletPercent += ADDBULLETPERCENT;
                        }
                    }

                    
                    arrowTimer += gameTime.ElapsedGameTime;

                    if (arrowTimer > TimeSpan.FromMilliseconds(BASEADDARROWTIME + addArrowRate))
                    {
                        if (random.Next(100) < (BASEADDBULLETPERCENT + addArrowPercent))
                        {

                            AddArrow();
                            //rotation += 0.1f;
                            arrowTimer -= TimeSpan.FromMilliseconds(BASEADDARROWTIME + addArrowRate);
                            addArrowRate = 0;
                            addArrowPercent = 0;
                        }
                        else
                        {
                            addArrowRate += ADDBULLETTIME;
                            addArrowPercent += ADDBULLETPERCENT;
                        }
                    }
                    break;
                case 10:
                    if (sunPriest.Health > 0)
                    {
                        if (myTimer > TimeSpan.FromMilliseconds(BASEADDBULLETTIME + addBulletRate))
                        {
                            if (random.Next(100) < (BASEADDBULLETPERCENT + addBulletPercent))
                            {

                                AddBullet();
                                //rotation += 0.1f;
                                myTimer -= TimeSpan.FromMilliseconds(BASEADDBULLETTIME + addBulletRate);
                                addBulletRate = 0;
                                addBulletPercent = 0;
                            }
                            else
                            {
                                addBulletRate += ADDBULLETTIME;
                                addBulletPercent += ADDBULLETPERCENT;
                            }
                        }
                        if (sunPriest.Attacking)
                        {
                            Vector2 tempVector = sunPriest.GetSpawn();
                            AddBullet(Projectile.ProjectileType.Counter, tempVector);
                            AddBullet(Projectile.ProjectileType.Stun, new Vector2(tempVector.X, tempVector.Y - 80));
                            AddBullet(Projectile.ProjectileType.Stun, new Vector2(tempVector.X, tempVector.Y + 80));
                        }
                    }
                    break;
                default:
                    switch (myPattern)
                    {
                        case ProjectilePattern.NONE:
                            if (myTimer > TimeSpan.FromMilliseconds(BASEADDBULLETTIME + addBulletRate))
                            {
                                if (random.Next(100) < (BASEADDBULLETPERCENT + addBulletPercent))
                                {

                                    AddBullet();
                                    //rotation += 0.1f;
                                    myTimer -= TimeSpan.FromMilliseconds(BASEADDBULLETTIME + addBulletRate);
                                    addBulletRate = 0;
                                    addBulletPercent = 0;
                                    //INCLUDE THIS FOR ATTACK PATTERNS
                                    patternCounter++;
                                    if (patternCounter == 15)
                                    {
                                        patternCounter = 0;
                                        int ran = random.Next(100);
                                        if (ran < 15)
                                            myPattern = ProjectilePattern.COUNTER_SPIN;
                                        else if (ran < 30)
                                            myPattern = ProjectilePattern.BALL_STUN_COUNTER;
                                    }
                                }
                                else
                                {
                                    addBulletRate += ADDBULLETTIME;
                                    addBulletPercent += ADDBULLETPERCENT;
                                }
                            }
                            break;
                        case ProjectilePattern.COUNTER_SPIN:
                            if (myTimer > TimeSpan.FromMilliseconds(1300))
                            {
                                myTimer -= TimeSpan.FromMilliseconds(1300);
                                if (patternCounter == 0)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(-50, 345));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(1280, 345));
                                }
                                else if (patternCounter == 1)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(-50, -50));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(1280, 720));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(-50, 345));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(1280, 345));
                                }
                                else if (patternCounter == 2)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(625, -50));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(625, 720));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(-50, -50));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(1280, 720));
                                }
                                else if (patternCounter == 3)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(1280, -50));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(-50, 720));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(625, -50));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(625, 720));
                                }
                                else if (patternCounter == 4)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(1280, 345));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(-50, 345));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(-50, -50));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(1280, 720));
                                }
                                else if (patternCounter == 5)
                                {
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(1280, 345));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(-50, 345));
                                }
                                else if (patternCounter == 6)
                                {
                                    patternCounter = 0;
                                    myPattern = ProjectilePattern.NONE;
                                }
                                patternCounter++;
                            }
                            break;
                        case ProjectilePattern.BALL_STUN_COUNTER:
                            if (myTimer > TimeSpan.FromMilliseconds(900))
                            {
                                myTimer -= TimeSpan.FromMilliseconds(900);
                                if (patternCounter == 1)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(-50, 360));
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(1330, 360));
                                }
                                else if (patternCounter == 2)
                                {
                                    AddBullet(Projectile.ProjectileType.Stun, new Vector2(-50, 360));
                                    AddBullet(Projectile.ProjectileType.Stun, new Vector2(1330, 360));
                                }
                                else if (patternCounter == 3)
                                {
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(-50, 360));
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(1330, 360));
                                }
                                else if (patternCounter == 4)
                                {
                                    AddBullet(Projectile.ProjectileType.Stun, new Vector2(-50, 360));
                                    AddBullet(Projectile.ProjectileType.Stun, new Vector2(1330, 360));
                                }
                                else if (patternCounter == 5)
                                {
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(-50, 360));
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(1330, 360));
                                }
                                else if (patternCounter == 6)
                                {
                                    patternCounter = 0;
                                    myPattern = ProjectilePattern.NONE;
                                }
                                patternCounter++;
                            }
                            break;
                    }

                    arrowTimer += gameTime.ElapsedGameTime;

                    if (arrowTimer > TimeSpan.FromMilliseconds(BASEADDARROWTIME * 2 + addArrowRate - (DIFFICULTYTIME * difficultyMod)))
                    {
                        if (random.Next(100) < (BASEADDBULLETPERCENT + addArrowPercent))
                        {

                            AddArrow();
                            //rotation += 0.1f;
                            arrowTimer -= TimeSpan.FromMilliseconds(BASEADDARROWTIME * 2 + addArrowRate - (DIFFICULTYTIME * difficultyMod));
                            addArrowRate = 0;
                            addArrowPercent = 0;
                        }
                        else
                        {
                            addArrowRate += ADDBULLETTIME;
                            addArrowPercent += ADDBULLETPERCENT;
                        }
                    }
                    break;
            }
            
        }

        public void AddBullet(Projectile.ProjectileType type, Vector2 pos)
        {
            if (level < 1 || level == 6 || level == 10 || countdownTimer.TotalSeconds > 0)
            {
                Projectile tempProjectile = new Projectile(type);
                if (type == Projectile.ProjectileType.Fire || type == Projectile.ProjectileType.Ice)
                {
                    tempProjectile.MySprite.Animate(0, 5);
                    tempProjectile.MySprite.FrameMod = 1.5f;
                }
                tempProjectile.MySprite.Position = pos;
                tempProjectile.MySprite.Pivot = new Vector2(tempProjectile.MySprite.Size.X / 2, tempProjectile.MySprite.Size.Y / 2);
                Vector2 AngleVector = tempProjectile.MySprite.Position - new Vector2(640, 360);
                AngleVector.Normalize();
                if (!(tempProjectile.MyType == Projectile.ProjectileType.Stun))
                {
                    tempProjectile.MySprite.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
                    if (tempProjectile.Reflected)
                    {
                        tempProjectile.MySprite.Rotation += 179;
                        if (tempProjectile.MySprite.Rotation >= 360)
                            tempProjectile.MySprite.Rotation -= 360;
                    }
                }
                Projectiles.Add(tempProjectile);
                if (GameCore.PlaySoundEffects)
                {
                    switch (type)
                    {
                        case Projectile.ProjectileType.Antipode:
                            GameCore.PublicSFXFireSound.Play();
                            GameCore.PublicSFXIceSound.Play();
                            break;
                        case Projectile.ProjectileType.Fire:
                            GameCore.PublicSFXFireSound.Play();
                            break;
                        case Projectile.ProjectileType.Ice:
                            GameCore.PublicSFXIceSound.Play();
                            break;
                        case Projectile.ProjectileType.Stun:
                            GameCore.PublicSFXStunSound.Play();
                            break;
                        case Projectile.ProjectileType.Counter:
                            GameCore.PublicSFXCounterSound.Play();
                            break;
                    }
                }
            }
        }

        public void AddBullet()
        {
            if (level < 1 || level == 6 || level == 10 || countdownTimer.TotalSeconds > 0)
            {
                Vector2 addPosition = Vector2.Zero;
                Projectile tempProjectile;


                if (random.Next(100) > 49)
                {
                    //in this case, add it from the left or right.  Left or right should be either 0 or screen limit
                    if (random.Next(100) > 49)
                        addPosition.X = -50;
                    else
                        addPosition.X = 1280;

                    //randomly determine where along the side they spawn
                    addPosition.Y = random.Next(0, 721);
                }
                else
                {
                    //in this case, it adds from top or bottom.  Top or bottom should be either 0 or screen limit
                    if (random.Next(100) > 49)
                        addPosition.Y = -50;
                    else
                        addPosition.Y = 720;

                    //randomly determine where along the top they spawn
                    addPosition.X = random.Next(0, 1281);
                }

                //addPosition = new Vector2(450, 720);

                //TODO:  Make the bullet here
                //each level has different odds
                switch (level)
                {
                    case 0:
                        int nextRand = random.Next(100);
                        if (nextRand < 50)
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Fire);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                        }
                        else
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Ice);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                            //tempProjectile.Center = new Vector2(addPosition.X + 30, addPosition.Y + 90);
                        }
                        break;
                    case 1:
                    case 10:
                        nextRand = random.Next(130);
                        if (nextRand < 50)
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Fire);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                        }
                        else if (nextRand < 100)
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Ice);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                            //tempProjectile.Center = new Vector2(addPosition.X + 30, addPosition.Y + 90);
                        }
                        else
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Antipode);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                        }
                        break;
                    case 2:
                    case 5:
                        nextRand = random.Next(180);
                        if (nextRand < 50)
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Fire);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                        }
                        else if (nextRand < 100)
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Ice);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                            //tempProjectile.Center = new Vector2(addPosition.X + 30, addPosition.Y + 90);
                        }
                        else
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Antipode);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                        }
                        break;
                    case 3:
                        nextRand = random.Next(200);
                        if (nextRand < 53)
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Fire);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                        }
                        else if (nextRand < 106)
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Ice);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                            //tempProjectile.Center = new Vector2(addPosition.X + 30, addPosition.Y + 90);
                        }
                        else if (nextRand < 159)
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Antipode);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                        }
                        else
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Stun);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                        }
                        break;
                    case 4:

                        nextRand = random.Next(200);
                        if (nextRand < 40)
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Fire);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                        }
                        else if (nextRand < 80)
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Ice);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                            //tempProjectile.Center = new Vector2(addPosition.X + 30, addPosition.Y + 90);
                        }
                        else if (nextRand < 140)
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Antipode);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                        }
                        else
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Stun);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                        }
                        break;
                    //add case 6 here
                    case 7:
                        nextRand = random.Next(200);
                        if (nextRand < 53)
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Fire);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                        }
                        else if (nextRand < 106)
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Ice);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                            //tempProjectile.Center = new Vector2(addPosition.X + 30, addPosition.Y + 90);
                        }
                        else if (nextRand < 159)
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Antipode);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                        }
                        else
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Counter);
                            tempProjectile.MySprite.Animate(0, 4);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                        }
                        break;
                    case 8:
                    case 9:
                    default:
                        nextRand = random.Next(250);
                        if (nextRand < 40)
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Fire);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                        }
                        else if (nextRand < 80)
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Ice);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                            //tempProjectile.Center = new Vector2(addPosition.X + 30, addPosition.Y + 90);
                        }
                        else if (nextRand < 140)
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Antipode);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                        }
                        else if (nextRand < 200)
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Stun);
                            tempProjectile.MySprite.Animate(0, 5);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                        }
                        else
                        {
                            tempProjectile = new Projectile(Projectile.ProjectileType.Counter);
                            tempProjectile.MySprite.Animate(0, 4);
                            tempProjectile.MySprite.FrameMod = 1.5f;
                        }
                        break;
                    /*
                default:
                    nextRand = random.Next(250);
                    if (nextRand < 50)
                    {
                        tempProjectile = new Projectile(Projectile.ProjectileType.Fire);
                        tempProjectile.MySprite.Animate(0, 5);
                        tempProjectile.MySprite.FrameMod = 1.5f;
                    }
                    else if (nextRand < 100)
                    {
                        tempProjectile = new Projectile(Projectile.ProjectileType.Ice);
                        tempProjectile.MySprite.Animate(0, 5);
                        tempProjectile.MySprite.FrameMod = 1.5f;
                        //tempProjectile.Center = new Vector2(addPosition.X + 30, addPosition.Y + 90);
                    }
                    else if (nextRand < 150)
                        tempProjectile = new Projectile(Projectile.ProjectileType.Antipode);
                    else if (nextRand < 200)
                        tempProjectile = new Projectile(Projectile.ProjectileType.Stun);
                    else
                        tempProjectile = new Projectile(Projectile.ProjectileType.Counter);
                    break;
                    */
                }


                //TODO:  set the bullet position here
                tempProjectile.MySprite.Position = addPosition;
                tempProjectile.MySprite.Pivot = new Vector2(tempProjectile.MySprite.Size.X / 2, tempProjectile.MySprite.Size.Y / 2);

                Vector2 AngleVector = tempProjectile.MySprite.Position - new Vector2(640, 360);
                AngleVector.Normalize();
                if (!(tempProjectile.MyType == Projectile.ProjectileType.Stun))
                {
                    tempProjectile.MySprite.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
                    if (tempProjectile.Reflected)
                    {
                        tempProjectile.MySprite.Rotation += 179;
                        if (tempProjectile.MySprite.Rotation >= 360)
                            tempProjectile.MySprite.Rotation -= 360;
                    }
                }

                //TODO:  add it to the list here
                Projectiles.Add(tempProjectile);
                if (GameCore.PlaySoundEffects)
                {
                    switch (tempProjectile.MyType)
                    {
                        case Projectile.ProjectileType.Antipode:
                            GameCore.PublicSFXFireSound.Play();
                            GameCore.PublicSFXIceSound.Play();
                            break;
                        case Projectile.ProjectileType.Fire:
                            GameCore.PublicSFXFireSound.Play();
                            break;
                        case Projectile.ProjectileType.Ice:
                            GameCore.PublicSFXIceSound.Play();
                            break;
                        case Projectile.ProjectileType.Stun:
                            GameCore.PublicSFXStunSound.Play();
                            break;
                        case Projectile.ProjectileType.Counter:
                            GameCore.PublicSFXCounterSound.Play();
                            break;
                    }
                }
            }
        }//end add bullet

        //add the arrows that use the XABY buttons here
        public void AddArrow()
        {
            if (level < 1 || level == 6 || level == 10 || countdownTimer.TotalSeconds > 0)
            {
                Vector2 addPosition = Vector2.Zero;
                Projectile tempProjectile = new Projectile(Projectile.ProjectileType.Arrow);

                int nextRand = random.Next(100);
                if (nextRand < 25)
                {
                    addPosition.X = -50;
                    addPosition.Y = 340;
                }
                else if (nextRand < 50)
                {
                    addPosition.X = 1280;
                    addPosition.Y = 340;
                }
                else if (nextRand < 75)
                {
                    addPosition.X = 610;
                    addPosition.Y = -50;
                }
                else
                {
                    addPosition.X = 610;
                    addPosition.Y = 720;
                }
                tempProjectile.MySprite.Position = addPosition;
                Vector2 AngleVector = tempProjectile.MySprite.Position - new Vector2(640, 360);
                AngleVector.Normalize();
                if (!(tempProjectile.MyType == Projectile.ProjectileType.Stun))
                {
                    tempProjectile.MySprite.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
                    if (tempProjectile.Reflected)
                    {
                        tempProjectile.MySprite.Rotation += 179;
                        if (tempProjectile.MySprite.Rotation >= 360)
                            tempProjectile.MySprite.Rotation -= 360;
                    }
                }
                Projectiles.Add(tempProjectile);
            }
        }


        public void Reset()
        {
            levelComplete = false;
            bLightningTime = true;
            bIsPaused = false;

            //handle level specific logic here
            switch (level)
            {
                default:
                    countdownTimer = TimeSpan.Zero;
                    numSpellBlasts = 3;
                    numDeflectCounter = 0;
                    score = 0;
                    centerRune.Animate(0, 3);
                    int nextRand = random.Next(0, 100);
                    if (nextRand < 50)
                        bgTexture = GameCore.PublicRoadBGTexture;
                    else
                        bgTexture = GameCore.PublicMountainBGTexture;
                    break;
                case 0:
                    countdownTimer = TimeSpan.Zero;
                    numSpellBlasts = 3;
                    numDeflectCounter = 0;
                    score = 0;
                    if (GameCore.IsTutorial)
                    {
                        bShowTutorial = true;
                        tutorialStringPosition = new Vector2(tutorialBG.Position.X + 150, tutorialBG.Position.Y + 20);
                        tutorialString = "Block fire balls with your\nfire shield and ice balls\nwith your ice shield.\nStart button will end\npractice session.";
                        Projectile fire = new Projectile(Projectile.ProjectileType.Fire);
                        Projectile ice = new Projectile(Projectile.ProjectileType.Ice);
                        fire.MySprite.Position = new Vector2(tutorialBG.Position.X + 30, tutorialBG.Position.Y + 55);
                        ice.MySprite.Position = new Vector2(tutorialBG.Position.X + 70, tutorialBG.Position.Y + 50);
                        fire.MySprite.Animate(0, 5);
                        ice.MySprite.Animate(0, 5);
                        tutorialList.Add(fire);
                        tutorialList.Add(ice);
                    }
                    bgTexture = GameCore.PublicTownBGTexture;
                    break;
                case 1:
                    countdownTimer = new TimeSpan(0, 0, 30);
                    numSpellBlasts = 3;
                    numDeflectCounter = 0;
                    if (GameCore.IsTutorial)
                    {
                        bShowTutorial = true;
                        tutorialStringPosition = new Vector2(tutorialBG.Position.X + 85, tutorialBG.Position.Y + 20);
                        tutorialString = "NEW THREAT: Dual Balls.\nHurt more than regular spells. \nUse both shields to stop these.";
                        Projectile fire = new Projectile(Projectile.ProjectileType.Antipode);
                        fire.MySprite.Position = new Vector2(tutorialBG.Position.X + 30, tutorialBG.Position.Y + 50);
                        fire.MySprite.Animate(0, 5);
                        tutorialList.Add(fire);
                        
                    }
                    
                    bgTexture = GameCore.PublicRoadBGTexture;
                    break;
                case 2:
                    countdownTimer = new TimeSpan(0, 0, 40);
                    centerStone.Animate(0, 3);
                    bgTexture = GameCore.PublicRoadBGTexture;
                    break;
                case 3:
                    countdownTimer = new TimeSpan(0, 0, 50);
                    centerRune.Animate(0, 9);
                    bgTexture = GameCore.PublicTownBGTexture;
                    if (GameCore.IsTutorial)
                    {
                        bShowTutorial = true;
                        tutorialStringPosition = new Vector2(tutorialBG.Position.X + 85, tutorialBG.Position.Y + 20);
                        tutorialString = "NEW THREAT: Stun Balls.\nThese cause no damage if you\nlet them pass,but lock your\nshield in place if you hit one.";
                        Projectile fire = new Projectile(Projectile.ProjectileType.Stun);
                        
                        fire.MySprite.Position = new Vector2(tutorialBG.Position.X + 20, tutorialBG.Position.Y + 50);

                        fire.MySprite.Animate(0, 5);
                        tutorialList.Add(fire);
                       
                    }
                    break;
                case 4:
                    countdownTimer = new TimeSpan(0, 1, 0);
                    centerStone.Animate(0, 3);
                    bgTexture = GameCore.PublicTownBGTexture;
                    break;
                case 5:
                    countdownTimer = new TimeSpan(0, 1, 10);
                    centerRune.Animate(0, 9);
                    if (GameCore.IsTutorial)
                    {
                        bShowTutorial = true;
                        tutorialStringPosition = new Vector2(tutorialBG.Position.X + 85, tutorialBG.Position.Y + 20);
                        tutorialString = "NEW THREAT: Arrows.\nThese fly above your shields.\nShoot them using A/B/X/Y. \n(A shoots down, X shoots \nleft, etc.)";
                        Projectile fire = new Projectile(Projectile.ProjectileType.Arrow);
                        
                        fire.MySprite.Position = new Vector2(tutorialBG.Position.X + 20, tutorialBG.Position.Y + 50);
                       
                        tutorialList.Add(fire);
                        
                    }
                    bgTexture = GameCore.PublicMountainBGTexture;
                    break;
                case 6:
                    //tempDragon = new Dragon();
                    countdownTimer = new TimeSpan(0, 1, 20);
                    centerStone.Animate(0, 3);
                    bgTexture = GameCore.PublicMountainBGTexture;
                    if (GameCore.IsTutorial)
                    {
                        bShowTutorial = true;
                        tutorialStringPosition = new Vector2(tutorialBG.Position.X + 85, tutorialBG.Position.Y + 20);
                        tutorialString = "BOSS FIGHT!\nHold the Right Trigger to \nreflect projectiles. Keep an \neye on your reflection meter.";
                        
                    }
                    break;
                case 7:
                    countdownTimer = new TimeSpan(0, 1, 30);
                    
                    bgTexture = GameCore.PublicRoadBGTexture;
                    if (GameCore.IsTutorial)
                    {
                        bShowTutorial = true;
                        tutorialStringPosition = new Vector2(tutorialBG.Position.X + 85, tutorialBG.Position.Y + 20);
                        tutorialString = "NEW THREAT: Counter Spells.\nThese cause no damage if you \nlet them pass, but disable \nyour shield if you hit one.";
                        Projectile fire = new Projectile(Projectile.ProjectileType.Counter);
                        
                        fire.MySprite.Position = new Vector2(tutorialBG.Position.X + 20, tutorialBG.Position.Y + 50);
                        fire.MySprite.Animate(0, 4);
                        tutorialList.Add(fire);
                        
                    }
                    break;
                case 8:
                    countdownTimer = new TimeSpan(0, 1, 40);
                    centerStone.Animate(0, 3);
                    bgTexture = GameCore.PublicMountainBGTexture;
                    break;
                case 9:
                    //sunPriest = new Priest();
                    countdownTimer = new TimeSpan(0, 1, 55);
                    centerStone.Animate(0, 3);
                    bgTexture = GameCore.PublicTownBGTexture;
                    break;
                case 10:
                    sunPriest = new Priest();
                    countdownTimer = new TimeSpan(0, 1, 55);
                    centerStone.Animate(0, 3);
                    bgTexture = GameCore.PublicTownBGTexture;
                    if (GameCore.IsTutorial)
                    {
                        bShowTutorial = true;
                        tutorialStringPosition = new Vector2(tutorialBG.Position.X + 85, tutorialBG.Position.Y + 20);
                        tutorialString = "BOSS FIGHT!\nYou cannot reflect stun balls\nor counter spells, but they \ndrain your reflect meter \ninstead of their usual effects.";

                    }
                    break;

            }
            Projectiles.Clear();
            myTimer = TimeSpan.Zero;
            fireStunTimer = TimeSpan.Zero;
            iceStunTimer = TimeSpan.Zero;
            fireCounterTimer = TimeSpan.Zero;
            iceCounterTimer = TimeSpan.Zero;
            reflectTimer = TimeSpan.Zero;
            bIsReflecting = false;
            bStartPressed = true;
            fireStun = false;
            iceStun = false;
            difficultyMod = 0;

            health = 100;
            reflectPercent = MAXREFLECTPERCENT;

            fireRed = 255;
            fireGreen = 66;
            fireBlue = 31;
            fireAlpha = 210;

            fireOverRed = 255;
            fireOverGreen = 228;
            fireOverBlue = 0;

            iceRed = 100;
            iceGreen = 233;
            iceBlue = 255;
            iceAlpha = 255;

            tempfireRed = 255;
            tempfireGreen = 66;
            tempfireBlue = 31;

            tempiceRed = 100;
            tempiceGreen = 233;
            tempiceBlue = 255;

            tempfireOverRed = 255;
            tempfireOverGreen = 228;
            tempfireOverBlue = 0;

            fireShield.MyColour = new Color(tempfireRed, tempfireGreen, tempfireBlue);
            fireShield.MyAlpha = (float)(fireAlpha)/255;
            fireShieldOver.MyColour = new Color(tempfireOverRed, tempfireOverGreen, tempfireOverBlue);
            fireShieldOver.MyAlpha = (float)(fireAlpha)/255;
            iceShield.MyColour = new Color(tempiceRed, tempiceGreen, tempiceBlue);
            iceShield.MyAlpha = (float)(iceAlpha)/255;
            iceShieldOver.MyColour = new Color(255, 255, 255);
            iceShieldOver.MyAlpha = (float)(iceAlpha)/255;

            fireShield.Rotation = 0;
            iceShield.Rotation = 0;
            fireShieldOver.Rotation = 0;
            iceShieldOver.Rotation = 0;


            iceAngle = 0;
            fireAngle = 0;

            bUpPressed = true;
            bDownPressed = true;
            bLeftPressed = true;
            bRightPressed = true;
            myPattern = ProjectilePattern.NONE;
            patternCounter = 0;
            patternTimer = TimeSpan.Zero;
        }
        //a function for dealing with reflections for boss levels
        public void HandleReflection(GameTime gameTime, GamePadState myPlayer)
        {
            if (!bIsReflecting && myPlayer.Buttons.RightShoulder == ButtonState.Pressed && reflectPercent >= 25)
            {
                bIsReflecting = true;
            }
            if (bIsReflecting && myPlayer.Buttons.RightShoulder == ButtonState.Pressed)
            {
                reflectTimer += gameTime.ElapsedGameTime;
                if (reflectTimer > TimeSpan.FromMilliseconds(80))
                {
                    reflectTimer -= TimeSpan.FromMilliseconds(80);
                    reflectPercent--;
                    if (reflectPercent == 0)
                    {
                        reflectTimer = TimeSpan.Zero;
                        bIsReflecting = false;
                    }
                }
            }
            else if (!bIsReflecting && myPlayer.Buttons.RightShoulder == ButtonState.Released)
            {
                if(reflectPercent < MAXREFLECTPERCENT)
                {
                    reflectTimer += gameTime.ElapsedGameTime;
                    if (reflectTimer > TimeSpan.FromMilliseconds(160))
                    {
                        reflectTimer -= TimeSpan.FromMilliseconds(160);
                        reflectPercent++;
                        if (reflectPercent == MAXREFLECTPERCENT)
                            reflectTimer = TimeSpan.Zero;
                            
                    }
                }
            }

            if (bIsReflecting && myPlayer.Buttons.RightShoulder == ButtonState.Released)
            {
                bIsReflecting = false;
                reflectTimer = TimeSpan.Zero;
            }

        }

        public override void Draw(SpriteBatch sBatch)
        {
            sBatch.Draw(bgTexture, Vector2.Zero, Color.White);
            iceShield.Draw(sBatch);
            iceShieldOver.Draw(sBatch);
            fireShield.Draw(sBatch);
            fireShieldOver.Draw(sBatch);

            switch (level)
            {
                case 0:
                    centerElder.Draw(sBatch);
                    break;
                case 1:
                    centerArchitect.Draw(sBatch);
                    break;
                case 7:
                    centerDruid.Draw(sBatch);
                    break;
                case 3:
                case 5:
                    centerRune.Draw(sBatch);
                    break;
                case 2:
                case 4:
                case 6:
                case 8:
                case 9:
                default:
                    centerStone.Draw(sBatch);
                    break;
            }
            //centerDot.Draw(sBatch);
            for (byte i = 0; i < Projectiles.Count; i++)
                Projectiles[i].Draw(sBatch);

            if (bAnimateUp)
                spellBlastUp.Draw(sBatch);
            if (bAnimateDown)
                spellBlastDown.Draw(sBatch);
            if (bAnimateLeft)
                spellBlastLeft.Draw(sBatch);
            if (bAnimateRight)
                spellBlastRight.Draw(sBatch);
            if (bLightningUp)
                lightningUp.Draw(sBatch);
            if (bLightningDown)
                lightningDown.Draw(sBatch);
            if (bLightningLeft)
                lightningLeft.Draw(sBatch);
            if (bLightningRight)
                lightningRight.Draw(sBatch);
            if(level == 6)
            tempDragon.Draw(sBatch);

            if(level == 10)
            sunPriest.Draw(sBatch);
            /*
            if (Projectiles.Count() > 0)
            {
                sBatch.DrawString(GameCore.Pericles, Angle.ToString(), new Vector2(130, 130), Color.DarkGreen);
                sBatch.DrawString(GameCore.Pericles, lowerLimit.ToString(), new Vector2(130, 230), Color.DarkGreen);
                sBatch.DrawString(GameCore.Pericles, upperLimit.ToString(), new Vector2(130, 330), Color.DarkGreen);
            }
            */
            //sBatch.Draw(GameCore.PublicHUDBackground, new Rectangle(975, 75, (int)(GameCore.PublicHUDBackground.Width * 1.3), (int)(GameCore.PublicHUDBackground.Height)), Color.White);
            //sBatch.Draw(GameCore.PublicHUDHealth, new Rectangle(1000, 80,(int)(GameCore.PublicHUDHealth.Width * 2), (int)(GameCore.PublicHUDHealth.Height * 2)), Color.White);
            if (level > 0 && level != 6 && level != 10)
                sBatch.Draw(GameCore.PublicHUDTime, new Rectangle(487, 85, (int)(GameCore.PublicHUDTime.Width), (int)(GameCore.PublicHUDTime.Height)), Color.White);
            if (level < 0)
            {
                sBatch.Draw(GameCore.PublicHUDScore, new Rectangle(0, 597, (int)(GameCore.PublicHUDScore.Width), (int)(GameCore.PublicHUDScore.Height)), Color.White);
                sBatch.DrawString(GameCore.Pericles, score.ToString(), new Vector2(132, 595), Color.White);
            }
            if (level > 0 && level != 6 && level != 10 && !bShowTutorial)
            {
                //sBatch.DrawString(GameCore.Pericles, health.ToString(), new Vector2(1000, 110), Color.White);
                sBatch.DrawString(GameCore.Pericles, countdownString, new Vector2(570, 84), Color.White);
                //sBatch.DrawString(GameCore.Pericles, score.ToString(), new Vector2(132, 600), Color.White);
            }
            else if (level == 0 && !bShowTutorial)
            {
               
                sBatch.DrawString(GameCore.Pericles, "PRESS START TO CONTINUE", new Vector2(400, 500), Color.White);
                
            }
            else
            {
                //sBatch.DrawString(GameCore.Pericles, health.ToString(), new Vector2(1000, 110), Color.White);
                //sBatch.DrawString(GameCore.Pericles, score.ToString(), new Vector2(132, 595), Color.White);
            }
            if (bShowTutorial)
            {
                tutorialBG.Draw(sBatch);
                sBatch.DrawString(GameCore.Pericles, tutorialString, tutorialStringPosition, Color.White);
                foreach (Projectile proj in tutorialList)
                {
                    proj.Draw(sBatch);
                }
            }
            if (numSpellBlasts > 0)
                sBatch.Draw(SpellBlastIcon, new Vector2(128, 75), Color.White);
            if (numSpellBlasts > 1)
                sBatch.Draw(SpellBlastIcon, new Vector2(128, 137), Color.White);
            if (numSpellBlasts > 2)
                sBatch.Draw(SpellBlastIcon, new Vector2(128, 200), Color.White);
            //sBatch.DrawString(GameCore.Pericles, "OFFSCREEN", new Vector2(1152, 600), Color.White);
            if (level != 0)
            {
                sBatch.Draw(GameCore.PublicReflectMeterBackdrop, new Vector2(1100, 131), Color.White);
                sBatch.Draw(healthMeter, new Rectangle(1100, (int)(131 + (healthMeter.Height - healthMeter.Height * ((double)health / 100))), healthMeter.Width, (int)(healthMeter.Height * ((double)health / 100))), new Rectangle(0, (int)(healthMeter.Height - healthMeter.Height * ((double)health / 100)), healthMeter.Width, (int)(healthMeter.Height * ((double)health / 100))), Color.White);
                sBatch.Draw(GameCore.PublicReflectMeterOutline, new Vector2(1100, 131), Color.White);
            }
            if (level == 6 || level == 10)
            {
                sBatch.Draw(GameCore.PublicReflectMeterBackdrop, new Vector2(1100, 331), Color.White);
                sBatch.Draw(reflectMeter, new Rectangle(1100, (int)(331 + (reflectMeter.Height - reflectMeter.Height * ((double)reflectPercent / 100))), reflectMeter.Width, (int)(reflectMeter.Height * ((double)reflectPercent / 100))), new Rectangle(0, (int)(reflectMeter.Height - reflectMeter.Height * ((double)reflectPercent / 100)), reflectMeter.Width, (int)(reflectMeter.Height * ((double)reflectPercent / 100))), Color.White);
                sBatch.Draw(GameCore.PublicReflectMeterOutline, new Vector2(1100, 331), Color.White);
            }
            base.Draw(sBatch);
        }
    }//end class
}//end namespace
