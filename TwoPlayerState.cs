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
    public class TwoPlayerState : GameState
    {
        protected const short BASEADDBULLETTIME = 700;
        protected const byte BASEADDBULLETPERCENT = 5;
        protected const short ADDBULLETTIME = 150;
        protected const byte ADDBULLETPERCENT = 3;
        protected const short BASEADDARROWTIME = 1600;
        protected const byte MAXREFLECTPERCENT = 100;
        protected const byte DIFFICULTYTIME = 100;
        protected const byte BULLETSTOADDDIFFICULTY = 20;

        //a byte for how much the difficulty time will affect the time
        protected byte difficultyMod = 0;
        protected byte difficultyCounter = 0;

        protected byte difficultyMod2 = 0;
        protected byte difficultyCounter2 = 0;

        //button press boolean
        protected bool bStartPressed = true;
        protected bool bIsPaused = false;

        protected byte stoneColour = 255;
        protected byte stoneColour2 = 255;

        protected bool bAPressed = true;
        protected bool bAPressed2 = true;

        //level info
        protected static sbyte level = 0;
        public static sbyte Level { get { return level; } set { level = value; } }

        protected sbyte health = 100;
        public sbyte Health { get { return health; } }
        protected sbyte health2 = 100;
        public sbyte Health2 { get { return health2; } }
       

        protected bool levelComplete = false;
        public bool LevelComplete { get { return levelComplete; } }

        //background texture
        protected Texture2D bgTexture;

        //center stuff
        protected AnimatedSprite centerRune;
        protected AnimatedSprite centerStone;
        protected Sprite centerElder;
        protected Sprite centerDruid;
        protected Sprite centerArchitect;
        protected AnimatedSprite centerRune2;
        protected AnimatedSprite centerStone2;
        protected Sprite centerDot;
        protected Sprite centerDot2;

        protected AnimatedSprite iceShield;
        protected AnimatedSprite iceShieldOver;
        protected AnimatedSprite fireShield;
        protected AnimatedSprite fireShieldOver;

        protected AnimatedSprite iceShield2;
        protected AnimatedSprite iceShieldOver2;
        protected AnimatedSprite fireShield2;
        protected AnimatedSprite fireShieldOver2;

        //spell blasts
        protected AnimatedSprite spellBlastUp;
        protected AnimatedSprite spellBlastDown;
        protected AnimatedSprite spellBlastLeft;
        protected AnimatedSprite spellBlastRight;

        protected AnimatedSprite spellBlastUp2;
        protected AnimatedSprite spellBlastDown2;
        protected AnimatedSprite spellBlastLeft2;
        protected AnimatedSprite spellBlastRight2;

        //lightning
        protected AnimatedSprite lightningUp;
        protected AnimatedSprite lightningDown;
        protected AnimatedSprite lightningLeft;
        protected AnimatedSprite lightningRight;
        protected AnimatedSprite lightningUp2;
        protected AnimatedSprite lightningDown2;
        protected AnimatedSprite lightningLeft2;
        protected AnimatedSprite lightningRight2;

        //and for animating
        protected bool bAnimateUp = false;
        protected bool bAnimateDown = false;
        protected bool bAnimateLeft = false;
        protected bool bAnimateRight = false;
        protected bool bAnimateUp2 = false;
        protected bool bAnimateDown2 = false;
        protected bool bAnimateLeft2 = false;
        protected bool bAnimateRight2 = false;

        protected bool bLightningUp = false;
        protected bool bLightningDown = false;
        protected bool bLightningLeft = false;
        protected bool bLightningRight = false;
        protected bool bLightningUp2 = false;
        protected bool bLightningDown2 = false;
        protected bool bLightningLeft2 = false;
        protected bool bLightningRight2 = false;


        //angle stuff
        protected float iceAngle = 0;
        protected float fireAngle = 0;
        protected float iceAngle2 = 0;
        protected float fireAngle2 = 0;

        private float Angle = 0;
        private float lowerLimit = 337.5f;
        private float upperLimit = 22.5f;

        protected Random random;

        //reflection stuff
        protected bool bIsReflecting = false;
        protected sbyte reflectPercent = 100;
        protected bool bIsReflecting2 = false;
        protected sbyte reflectPercent2 = 100;

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
        protected byte numSpellBlasts2 = 0;
        protected byte numDeflectCounter2 = 0;
        protected bool bUpPressed2 = true;
        protected bool bDownPressed2 = true;
        protected bool bLeftPressed2 = true;
        protected bool bRightPressed2 = true;
        protected bool bSpellBlast2 = false;
        protected bool bSpellBlastPressed2 = true;
        protected Texture2D SpellBlastIcon;

        //timer stuff
        protected TimeSpan myTimer = TimeSpan.Zero;
        protected TimeSpan arrowTimer = TimeSpan.Zero;
        protected TimeSpan fireStunTimer = TimeSpan.Zero;
        protected TimeSpan iceStunTimer = TimeSpan.Zero;
        protected TimeSpan fireCounterTimer = TimeSpan.Zero;
        protected TimeSpan iceCounterTimer = TimeSpan.Zero;
        protected TimeSpan reflectTimer = TimeSpan.Zero;

        protected TimeSpan myTimer2 = TimeSpan.Zero;
        protected TimeSpan arrowTimer2 = TimeSpan.Zero;
        protected TimeSpan fireStunTimer2 = TimeSpan.Zero;
        protected TimeSpan iceStunTimer2 = TimeSpan.Zero;
        protected TimeSpan fireCounterTimer2 = TimeSpan.Zero;
        protected TimeSpan iceCounterTimer2 = TimeSpan.Zero;
        protected TimeSpan reflectTimer2 = TimeSpan.Zero;

        //timer for the gameplay countdown
        protected TimeSpan countdownTimer = new TimeSpan(0, 2, 0);
        protected String countdownString;

        //timer for patterns
        protected TimeSpan patternTimer = TimeSpan.Zero;
        protected TimeSpan patternTimer2 = TimeSpan.Zero;

        //timing stuff for bullet adding
        protected short addBulletRate = 0;
        protected byte addBulletPercent = 0;
        protected short addArrowRate = 0;
        protected byte addArrowPercent = 0;
        protected byte patternCounter = 0;
        protected byte patternCounter2 = 0;

        protected short addBulletRate2 = 0;
        protected byte addBulletPercent2 = 0;
        protected short addArrowRate2 = 0;
        protected byte addArrowPercent2 = 0;


        protected ProjectilePattern myPattern = ProjectilePattern.NONE;
        protected ProjectilePattern myPattern2 = ProjectilePattern.NONE;


        protected List<Projectile> Projectiles;
        protected List<Projectile> Projectiles2;

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

        protected byte fireRed2 = 255;
        protected byte fireGreen2 = 66;
        protected byte fireBlue2 = 31;
        protected byte fireAlpha2 = 210;

        protected byte fireOverRed2 = 255;
        protected byte fireOverGreen2 = 228;
        protected byte fireOverBlue2 = 0;


        protected byte iceRed2 = 100;
        protected byte iceGreen2 = 233;
        protected byte iceBlue2 = 255;
        protected byte iceAlpha2 = 255;

        protected byte tempfireRed2 = 255;
        protected byte tempfireGreen2 = 66;
        protected byte tempfireBlue2 = 31;

        protected byte tempfireOverRed2 = 255;
        protected byte tempfireOverGreen2 = 228;
        protected byte tempfireOverBlue2 = 0;

        protected byte tempiceRed2 = 100;
        protected byte tempiceGreen2 = 233;
        protected byte tempiceBlue2 = 255;

        //stuff for getting stunned
        protected bool fireStun = false;
        protected bool iceStun = false;
        protected bool fireStun2 = false;
        protected bool iceStun2 = false;
        protected const short STUNRECOVERY = 2000;

        protected TimeSpan rumbleTimer = TimeSpan.Zero;
        protected bool isHit = false;

        protected TimeSpan rumbleTimer2 = TimeSpan.Zero;
        protected bool isHit2 = false;

        protected enum ProjectilePattern
        {
            NONE,
            COUNTER_SPIN,
            BALL_STUN_COUNTER
        }

        public TwoPlayerState(Game game)
            : base(game)
        {
            iceShield = new AnimatedSprite(GameCore.PublicIceShieldTexture);
            iceShieldOver = new AnimatedSprite(GameCore.PublicIceShieldOverTexture);
            fireShield = new AnimatedSprite(GameCore.PublicFireShieldTexture);
            fireShieldOver = new AnimatedSprite(GameCore.PublicFireShieldOverTexture);

            iceShield2 = new AnimatedSprite(GameCore.PublicIceShieldTexture);
            iceShieldOver2 = new AnimatedSprite(GameCore.PublicIceShieldOverTexture);
            fireShield2 = new AnimatedSprite(GameCore.PublicFireShieldTexture);
            fireShieldOver2 = new AnimatedSprite(GameCore.PublicFireShieldOverTexture);

            centerRune = new AnimatedSprite(GameCore.PublicCenterRuneTexture);
            centerStone = new AnimatedSprite(GameCore.PublicCenterStoneTexture);
            centerRune.Position = new Vector2(320 - (centerRune.Size.X / 4), 360 - (centerRune.Size.Y / 4));
            centerStone.Position = new Vector2(320 - (centerStone.Size.X / 4), 360 - (centerStone.Size.Y / 4));
            centerDot = new Sprite(GameCore.PublicRedDot);
            centerDot.Position = new Vector2(320 - (centerDot.Size.X / 4), 360 - (centerDot.Size.Y / 4));

            centerRune2 = new AnimatedSprite(GameCore.PublicCenterRuneTexture);
            centerStone2 = new AnimatedSprite(GameCore.PublicCenterStoneTexture);
            centerRune2.Position = new Vector2(320 - (centerRune.Size.X / 4) + 635, 360 - (centerRune.Size.Y / 4));
            centerStone2.Position = new Vector2(320 - (centerStone.Size.X / 4) + 640, 360 - (centerStone.Size.Y / 4));
            centerDot2 = new Sprite(GameCore.PublicRedDot);
            centerDot2.Position = new Vector2(320 - (centerDot2.Size.X / 4) + 640, 360 - (centerDot2.Size.Y / 4));

            centerStone.SizeModX = 0.5f;
            centerStone.SizeModY = 0.5f;
            centerStone2.SizeModY = 0.5f;
            centerStone2.SizeModX = 0.5f;
            centerDot.SizeModX = 0.5f;
            centerDot.SizeModY = 0.5f;
            centerDot2.SizeModX = 0.5f;
            centerDot2.SizeModY = 0.5f;
            

            SpellBlastIcon = GameCore.PublicSpellBlastIcon;
            spellBlastDown = new AnimatedSprite(GameCore.PublicVerticalSpellBlast);
            spellBlastUp = new AnimatedSprite(GameCore.PublicVerticalSpellBlast);
            spellBlastLeft = new AnimatedSprite(GameCore.PublicHorizontalSpellBlast);
            spellBlastRight = new AnimatedSprite(GameCore.PublicHorizontalSpellBlast);
            spellBlastDown2 = new AnimatedSprite(GameCore.PublicVerticalSpellBlast);
            spellBlastUp2 = new AnimatedSprite(GameCore.PublicVerticalSpellBlast);
            spellBlastLeft2 = new AnimatedSprite(GameCore.PublicHorizontalSpellBlast);
            spellBlastRight2 = new AnimatedSprite(GameCore.PublicHorizontalSpellBlast);

          
            spellBlastDown.SizeModX = 0.5f;
            spellBlastUp.SizeModX = 0.5f;
            spellBlastLeft.SizeModX = 0.5f;
            spellBlastRight.SizeModX = 0.5f;
            spellBlastDown2.SizeModX = 0.5f;
            spellBlastUp2.SizeModX = 0.5f;
            spellBlastLeft2.SizeModX = 0.5f;
            spellBlastRight2.SizeModX = 0.5f;

            spellBlastLeft.Position = new Vector2(-50, 65);
            spellBlastRight.Position = new Vector2(345, 65);
            spellBlastDown.Position = new Vector2(77, 410);
            spellBlastUp.Position = new Vector2(77, -50);
            spellBlastLeft2.Position = new Vector2(630, 65);
            spellBlastRight2.Position = new Vector2(980, 65);
            spellBlastDown2.Position = new Vector2(700, 410);
            spellBlastUp2.Position = new Vector2(700, -50);

            //spellBlastLeft.Position = new Vector2(630, 65);
            //spellBlastRight.Position = new Vector2(985, 65);
            //spellBlastDown.Position = new Vector2(700, 410);
            //spellBlastUp.Position = new Vector2(700, -50);

            spellBlastDown.FrameMod = 0.5f;
            spellBlastUp.FrameMod = 0.5f;
            spellBlastLeft.FrameMod = 0.5f;
            spellBlastRight.FrameMod = 0.5f;
            spellBlastDown2.FrameMod = 0.5f;
            spellBlastUp2.FrameMod = 0.5f;
            spellBlastLeft2.FrameMod = 0.5f;
            spellBlastRight2.FrameMod = 0.5f;

            spellBlastRight.Effect = SpriteEffects.FlipHorizontally;
            spellBlastDown.Effect = SpriteEffects.FlipVertically;
            spellBlastRight2.Effect = SpriteEffects.FlipHorizontally;
            spellBlastDown2.Effect = SpriteEffects.FlipVertically;

            lightningDown = new AnimatedSprite(GameCore.PublicVerticalLightning);
            lightningUp = new AnimatedSprite(GameCore.PublicVerticalLightning);
            lightningLeft = new AnimatedSprite(GameCore.PublicHorizontalLightning);
            lightningRight = new AnimatedSprite(GameCore.PublicHorizontalLightning);

            lightningLeft.Position = new Vector2(130, 350);
            lightningRight.Position = new Vector2(355, 350);
            lightningDown.Position = new Vector2(307, 430);
            lightningUp.Position = new Vector2(307, 118);

            lightningDown.FrameMod = 0.5f;
            lightningUp.FrameMod = 0.5f;
            lightningLeft.FrameMod = 0.5f;
            lightningRight.FrameMod = 0.5f;

            lightningDown.SizeModX = 0.5f;
            lightningUp.SizeModX = 0.5f;
            lightningLeft.SizeModX = 0.5f;
            lightningRight.SizeModX = 0.5f;
            lightningDown.SizeModY = 0.5f;
            lightningUp.SizeModY = 0.5f;
            lightningLeft.SizeModY = 0.5f;
            lightningRight.SizeModY = 0.5f;

            lightningRight.Effect = SpriteEffects.FlipHorizontally;
            lightningDown.Effect = SpriteEffects.FlipVertically;

            lightningDown2 = new AnimatedSprite(GameCore.PublicVerticalLightning);
            lightningUp2 = new AnimatedSprite(GameCore.PublicVerticalLightning);
            lightningLeft2 = new AnimatedSprite(GameCore.PublicHorizontalLightning);
            lightningRight2 = new AnimatedSprite(GameCore.PublicHorizontalLightning);

            lightningLeft2.Position = new Vector2(763, 350);
            lightningRight2.Position = new Vector2(1007, 350);
            lightningDown2.Position = new Vector2(952, 430);
            lightningUp2.Position = new Vector2(952, 118);

            lightningDown2.FrameMod = 0.5f;
            lightningUp2.FrameMod = 0.5f;
            lightningLeft2.FrameMod = 0.5f;
            lightningRight2.FrameMod = 0.5f;

            lightningDown2.SizeModX = 0.5f;
            lightningUp2.SizeModX = 0.5f;
            lightningLeft2.SizeModX = 0.5f;
            lightningRight2.SizeModX = 0.5f;
            lightningDown2.SizeModY = 0.5f;
            lightningUp2.SizeModY = 0.5f;
            lightningLeft2.SizeModY = 0.5f;
            lightningRight2.SizeModY = 0.5f;

            lightningRight2.Effect = SpriteEffects.FlipHorizontally;
            lightningDown2.Effect = SpriteEffects.FlipVertically;

            healthMeter = GameCore.PublicHealthMeterTexture;
            reflectMeter = GameCore.PublicReflectMeterTexture;
            

            //iceShield.Position = new Vector2(300, 300);
            //fireShield.Position = new Vector2(300, 300);

            iceShield.MyColour = new Color(iceRed, iceGreen, iceBlue);
            iceShield.Pivot = new Vector2(iceShield.Size.X / 2, iceShield.Size.Y/2 + 120);
            iceShield.Position = new Vector2(640 / 2 - iceShield.Size.X / 4 + 45, 720 / 2 - iceShield.Size.Y / 2 + 45);
            iceShield.SizeModX = 0.5f;
            iceShield.SizeModY = 0.5f;

            fireShield.MyColour = new Color(fireRed, fireGreen, fireBlue);
            fireShield.Pivot = new Vector2(fireShield.Size.X / 2, fireShield.Size.Y/2 + 120);
            fireShield.Position = new Vector2(640 / 2 - fireShield.Size.X / 4 + 45, 720 / 2 - fireShield.Size.Y / 2 + 45);
            fireShield.SizeModX = 0.5f;
            fireShield.SizeModY = 0.5f;

            iceShieldOver.MyColour = Color.White;
            iceShieldOver.Pivot = new Vector2(iceShieldOver.Size.X / 2, iceShieldOver.Size.Y /2 + 120);
            iceShieldOver.Position = new Vector2(640 / 2 - iceShieldOver.Size.X / 4 + 45, 720 / 2 - iceShieldOver.Size.Y/2 + 45);
            iceShieldOver.SizeModX = 0.5f;
            iceShieldOver.SizeModY = 0.5f;

            fireShieldOver.MyColour = new Color(fireOverRed, fireOverGreen, fireOverBlue);
            fireShieldOver.Pivot = new Vector2(fireShieldOver.Size.X / 2, fireShieldOver.Size.Y / 2 + 120);
            fireShieldOver.Position = new Vector2(640 / 2 - fireShieldOver.Size.X / 4 + 45, 720 / 2 - fireShieldOver.Size.Y/2 + 45);
            fireShieldOver.SizeModX = 0.5f;
            fireShieldOver.SizeModY = 0.5f;

            iceShield2.MyColour = new Color(iceRed, iceGreen, iceBlue);
            iceShield2.Pivot = new Vector2(iceShield2.Size.X / 2, iceShield2.Size.Y / 2+ 120);
            iceShield2.Position = new Vector2(1280 / 2 + 300 - iceShield2.Size.X / 4 + 63, 720 / 2 - iceShield2.Size.Y/2 + 45);
            iceShield2.SizeModX = 0.5f;
            iceShield2.SizeModY = 0.5f;

            fireShield2.MyColour = new Color(fireRed, fireGreen, fireBlue);
            fireShield2.Pivot = new Vector2(fireShield2.Size.X / 2, fireShield2.Size.Y / 2+ 120);
            fireShield2.Position = new Vector2(1280 / 2 + 300 - fireShield2.Size.X / 4 + 63, 720 / 2 - fireShield2.Size.Y/2 + 45);
            fireShield2.SizeModX = 0.5f;
            fireShield2.SizeModY = 0.5f;

            iceShieldOver2.MyColour = Color.White;
            iceShieldOver2.Pivot = new Vector2(iceShieldOver2.Size.X / 2, iceShieldOver2.Size.Y / 2+ 120);
            iceShieldOver2.Position = new Vector2(1280 / 2 + 300 - iceShieldOver2.Size.X / 4 + 63, 720 / 2 - iceShieldOver2.Size.Y/2 + 45);
            iceShieldOver2.SizeModX = 0.5f;
            iceShieldOver2.SizeModY = 0.5f;

            fireShieldOver2.MyColour = new Color(fireOverRed, fireOverGreen, fireOverBlue);
            fireShieldOver2.Pivot = new Vector2(fireShieldOver2.Size.X / 2, fireShieldOver2.Size.Y / 2 + 120);
            fireShieldOver2.Position = new Vector2(1280 / 2 + 300 - fireShieldOver2.Size.X / 4 + 63, 720 / 2 - fireShieldOver2.Size.Y/2 + 45);
            fireShieldOver2.SizeModX = 0.5f;
            fireShieldOver2.SizeModY = 0.5f;

            Projectiles = new List<Projectile>();
            Projectiles2 = new List<Projectile>();
            random = new Random();
        }

        public override void Update(GameTime gameTime)
        {
            //For now, just use player 1 and player 2
            GamePadState PlayerOne = GamePad.GetState(SelectTwoPlayerState.PublicPOne);
            GamePadState PlayerTwo = GamePad.GetState(SelectTwoPlayerState.PublicPTwo);

            if (bIsPaused)
            {
                if (PlayerOne.Buttons.Start == ButtonState.Pressed || PlayerTwo.Buttons.Start == ButtonState.Pressed)
                {
                    if (!bStartPressed && bIsPaused)
                    {
                        bIsPaused = false;
                        bStartPressed = true;
                        GamePad.SetVibration(SelectTwoPlayerState.PublicPOne, 0, 0);
                        GamePad.SetVibration(SelectTwoPlayerState.PublicPTwo, 0, 0);
                        MediaPlayer.Resume();
                        return;
                    }
                }
                else if (bStartPressed)
                {
                    bStartPressed = false;
                }
            }
            if(!bIsPaused && !Guide.IsVisible)
            {
                if (PlayerOne.Buttons.Start == ButtonState.Pressed || PlayerTwo.Buttons.Start == ButtonState.Pressed)
                {
                    if (!bStartPressed && !bIsPaused && level != 0)
                    {
                        bIsPaused = true;
                        bStartPressed = true;
                        GamePad.SetVibration(SelectTwoPlayerState.PublicPOne, 0, 0);
                        GamePad.SetVibration(SelectTwoPlayerState.PublicPTwo, 0, 0);
                        MediaPlayer.Pause();
                        return;

                    }
                }
                else if (bStartPressed)
                {
                    bStartPressed = false;
                }
                if (isHit)
                {
                    rumbleTimer += gameTime.ElapsedGameTime;
                    if (rumbleTimer > TimeSpan.FromMilliseconds(600))
                    {
                        rumbleTimer = TimeSpan.Zero;
                        isHit = false;
                        GamePad.SetVibration(SelectTwoPlayerState.PublicPOne, 0, 0);
                    }
                }
                if (isHit2)
                {
                    rumbleTimer2 += gameTime.ElapsedGameTime;
                    if (rumbleTimer2 > TimeSpan.FromMilliseconds(600))
                    {
                        rumbleTimer2 = TimeSpan.Zero;
                        isHit2 = false;
                        GamePad.SetVibration(SelectTwoPlayerState.PublicPTwo, 0, 0);
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
                if (iceStun2)
                {
                    iceStunTimer2 += gameTime.ElapsedGameTime;
                    if (iceStunTimer2 > TimeSpan.FromMilliseconds(STUNRECOVERY))
                    {
                        iceStunTimer2 = TimeSpan.Zero;
                        iceStun2 = false;
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
                if (fireStun2)
                {
                    fireStunTimer2 += gameTime.ElapsedGameTime;
                    if (fireStunTimer2 > TimeSpan.FromMilliseconds(STUNRECOVERY))
                    {
                        fireStunTimer2 = TimeSpan.Zero;
                        fireStun2 = false;
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

                if (!iceStun2)
                {
                    if (PlayerTwo.ThumbSticks.Left.Length() > 0.1f)
                    {
                        iceShield2.Rotation = (float)Math.Atan2(PlayerTwo.ThumbSticks.Left.X, PlayerTwo.ThumbSticks.Left.Y);
                        iceShieldOver2.Rotation = (float)Math.Atan2(PlayerTwo.ThumbSticks.Left.X, PlayerTwo.ThumbSticks.Left.Y);
                        iceAngle2 = MathHelper.ToDegrees((float)Math.Atan2(PlayerTwo.ThumbSticks.Left.X, -PlayerTwo.ThumbSticks.Left.Y));
                        iceAngle2 += 180;
                        if (iceAngle2 == 360)
                            iceAngle2 = 0;
                        else if (iceAngle < 0)
                            iceAngle += 360;
                    }
                }
                if (!fireStun2)
                {
                    if (PlayerTwo.ThumbSticks.Right.Length() > 0.1f)
                    {
                        fireShield2.Rotation = (float)Math.Atan2(PlayerTwo.ThumbSticks.Right.X, PlayerTwo.ThumbSticks.Right.Y);
                        fireShieldOver2.Rotation = (float)Math.Atan2(PlayerTwo.ThumbSticks.Right.X, PlayerTwo.ThumbSticks.Right.Y);
                        fireAngle2 = MathHelper.ToDegrees((float)Math.Atan2(PlayerTwo.ThumbSticks.Right.X, -PlayerTwo.ThumbSticks.Right.Y));
                        fireAngle2 += 180;
                        if (fireAngle2 == 360)
                            fireAngle2 = 0;
                        else if (fireAngle2 < 0)
                            fireAngle2 += 360;
                    }
                }

                HandleReflection(gameTime, PlayerOne, SelectTwoPlayerState.PublicPOne);
                HandleReflection(gameTime, PlayerTwo, SelectTwoPlayerState.PublicPTwo);

                HandleAddProjectiles(gameTime);
                //HandleAddProjectiles(gameTime);

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
                if (!bSpellBlastPressed2 && numSpellBlasts2 > 0)
                {
                    if (PlayerTwo.DPad.Left == ButtonState.Pressed && !bLeftPressed2)
                    {
                        bLeftPressed2 = true;
                        bSpellBlast2 = true;
                        numSpellBlasts2--;
                        bSpellBlastPressed2 = true;
                        bAnimateLeft2 = true;
                        if (GameCore.PlaySoundEffects)
                            GameCore.PublicSFXBlastSound.Play();
                        spellBlastLeft2.Animate(0, 9);
                    }
                    else if (PlayerTwo.DPad.Right == ButtonState.Pressed && !bRightPressed2)
                    {
                        bRightPressed2 = true;
                        bSpellBlast2 = true;
                        numSpellBlasts2--;
                        bSpellBlastPressed2 = true;
                        bAnimateRight2 = true;
                        if (GameCore.PlaySoundEffects)
                            GameCore.PublicSFXBlastSound.Play();
                        spellBlastRight2.Animate(0, 9);
                    }
                    else if (PlayerTwo.DPad.Down == ButtonState.Pressed && !bDownPressed2)
                    {
                        bDownPressed2 = true;
                        bSpellBlast2 = true;
                        numSpellBlasts2--;
                        bSpellBlastPressed2 = true;
                        bAnimateDown2 = true;
                        if (GameCore.PlaySoundEffects)
                            GameCore.PublicSFXBlastSound.Play();
                        spellBlastDown2.Animate(0, 9);
                    }
                    else if (PlayerTwo.DPad.Up == ButtonState.Pressed && !bUpPressed2)
                    {
                        bUpPressed2 = true;
                        bSpellBlast2 = true;
                        numSpellBlasts2--;
                        bSpellBlastPressed2 = true;
                        bAnimateUp2 = true;
                        if (GameCore.PlaySoundEffects)
                            GameCore.PublicSFXBlastSound.Play();
                        spellBlastUp2.Animate(0, 9);
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
                if (bAnimateUp2)
                {
                    spellBlastUp2.Update(gameTime);
                    if (spellBlastUp2.CurrentFrame == 9)
                    {
                        bAnimateUp2 = false;
                    }
                }
                if (bAnimateDown2)
                {
                    spellBlastDown2.Update(gameTime);
                    if (spellBlastDown2.CurrentFrame == 9)
                    {
                        bAnimateDown2 = false;
                    }
                }
                if (bAnimateLeft2)
                {
                    spellBlastLeft2.Update(gameTime);
                    if (spellBlastLeft2.CurrentFrame == 9)
                    {
                        bAnimateLeft2 = false;
                    }
                }
                if (bAnimateRight2)
                {
                    spellBlastRight2.Update(gameTime);
                    if (spellBlastRight2.CurrentFrame == 9)
                    {
                        bAnimateRight2 = false;
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

                if (!bLightningDown && PlayerOne.Buttons.A == ButtonState.Pressed && !bAPressed)
                {
                    bLightningDown = true;
                    if (GameCore.PlaySoundEffects)
                        GameCore.PublicSFXLightningSound.Play();
                    lightningDown.Animate(0, 8);
                }
                if (!bLightningLeft && PlayerOne.Buttons.X == ButtonState.Pressed && !bAPressed)
                {
                    bLightningLeft = true;
                    if (GameCore.PlaySoundEffects)
                        GameCore.PublicSFXLightningSound.Play();
                    lightningLeft.Animate(0, 9);
                }
                if (!bLightningRight && PlayerOne.Buttons.B == ButtonState.Pressed && !bAPressed)
                {
                    bLightningRight = true;
                    if (GameCore.PlaySoundEffects)
                        GameCore.PublicSFXLightningSound.Play();
                    lightningRight.Animate(0, 9);
                }
                if (!bLightningUp && PlayerOne.Buttons.Y == ButtonState.Pressed && !bAPressed)
                {
                    bLightningUp = true;
                    if (GameCore.PlaySoundEffects)
                        GameCore.PublicSFXLightningSound.Play();
                    lightningUp.Animate(0, 8);
                }

                if (bLightningDown2)
                {
                    lightningDown2.Update(gameTime);
                    if (lightningDown2.CurrentFrame == 8)
                        bLightningDown2 = false;
                }
                if (bLightningUp2)
                {
                    lightningUp2.Update(gameTime);
                    if (lightningUp2.CurrentFrame == 8)
                        bLightningUp2 = false;
                }
                if (bLightningLeft2)
                {
                    lightningLeft2.Update(gameTime);
                    if (lightningLeft2.CurrentFrame == 9)
                        bLightningLeft2 = false;
                }
                if (bLightningRight2)
                {
                    lightningRight2.Update(gameTime);
                    if (lightningRight2.CurrentFrame == 9)
                        bLightningRight2 = false;
                }

                if (!bLightningDown2 && PlayerTwo.Buttons.A == ButtonState.Pressed && !bAPressed2)
                {
                    bLightningDown2 = true;
                    if (GameCore.PlaySoundEffects)
                        GameCore.PublicSFXLightningSound.Play();
                    lightningDown2.Animate(0, 8);
                }
                if (!bLightningLeft2 && PlayerTwo.Buttons.X == ButtonState.Pressed && !bAPressed2)
                {
                    bLightningLeft2 = true;
                    if (GameCore.PlaySoundEffects)
                        GameCore.PublicSFXLightningSound.Play();
                    lightningLeft2.Animate(0, 9);
                }
                if (!bLightningRight2 && PlayerTwo.Buttons.B == ButtonState.Pressed && !bAPressed2)
                {
                    bLightningRight2 = true;
                    if (GameCore.PlaySoundEffects)
                        GameCore.PublicSFXLightningSound.Play();
                    lightningRight2.Animate(0, 9);
                }
                if (!bLightningUp2 && PlayerTwo.Buttons.Y == ButtonState.Pressed && !bAPressed2)
                {
                    bLightningUp2 = true;
                    if (GameCore.PlaySoundEffects)
                        GameCore.PublicSFXLightningSound.Play();
                    lightningUp2.Animate(0, 8);
                }


                //handle collision logic here
                for (int i = 0; i < Projectiles.Count; i++)
                {
                    Projectiles[i].Update(gameTime);
                    Vector2 targetVector = new Vector2(320, 360);

                    Projectiles[i].Move(targetVector);
                    targetVector = Projectiles[i].MySprite.Position - targetVector;
                    Vector2 AngleVector = Projectiles[i].MySprite.Position - new Vector2(320, 360);
                    AngleVector.Normalize();
                    /*
                    Projectiles[i].MySprite.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
                    if (Projectiles[i].Reflected)
                    {
                        Projectiles[i].MySprite.Rotation += 179;
                        if (Projectiles[i].MySprite.Rotation >= 360)
                            Projectiles[i].MySprite.Rotation -= 360;
                    }
                    */
                    Angle = MathHelper.ToDegrees((float)Math.Atan2(-AngleVector.X, -AngleVector.Y));
                    if (Angle < 0)
                        Angle += 360;
                    //Angle += 180;
                    lowerLimit = Angle - 40;
                    //if it goes below 0, make it go full circle
                    if (lowerLimit < 0)
                    {
                        lowerLimit = 360 + lowerLimit;
                    }
                    upperLimit = Angle + 40;
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
                                //score += 30;
                                continue;
                            }
                        }
                        else
                        {
                            if ((Projectiles[i].MovementDir.X > 0 && bLightningLeft) || (Projectiles[i].MovementDir.X < 0 && bLightningRight))
                            {
                                Projectiles.Remove(Projectiles[i]);
                                i--;
                                //score += 30;
                                continue;
                            }
                        }
                    }
                    //is the projectile in range to be countered?
                    if (targetVector.Length() > 50 && targetVector.Length() < 95 && !Projectiles[i].Reflected)
                    {



                        switch (Projectiles[i].MyType)
                        {


                            case Projectile.ProjectileType.Antipode:
                                //in the normal part of the circle
                                if (iceAlpha > 220 && fireAlpha > 190)
                                {
                                    if (upperLimit - lowerLimit > 79.99f && upperLimit - lowerLimit < 80.01f)
                                    {
                                        if (fireAngle > lowerLimit && fireAngle < upperLimit && iceAngle > lowerLimit && iceAngle < upperLimit)
                                        {
                                            if (bIsReflecting)
                                            {
                                                Projectiles[i].Reflected = true;
                                                Projectiles[i].BoostSpeed();
                                                Vector2 tempAngleVector = Projectiles[i].MySprite.Position - new Vector2(320, 360);
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
                                            //if (level != 0)
                                              //  score += (ushort)(15 + (ushort)(targetVector.Length() - 180));
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
                                                Projectiles[i].BoostSpeed();
                                                Vector2 tempAngleVector = Projectiles[i].MySprite.Position - new Vector2(320, 360);
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
                                            //if (level != 0)
                                              //  score += (ushort)(15 + (ushort)(targetVector.Length() - 180));
                                            continue;

                                        }
                                    }
                                }
                                break;
                            case Projectile.ProjectileType.Fire:
                                //in the normal part of the circle
                                if (fireAlpha > 190)
                                {
                                    if (upperLimit - lowerLimit > 79.99f && upperLimit - lowerLimit < 80.01f)
                                    {
                                        if (fireAngle > lowerLimit && fireAngle < upperLimit)
                                        {
                                            if (bIsReflecting)
                                            {
                                                Projectiles[i].Reflected = true;
                                                Projectiles[i].BoostSpeed();
                                                Vector2 tempAngleVector = Projectiles[i].MySprite.Position - new Vector2(320, 360);
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
                                            //if (level != 0)
                                                //score += (ushort)(10 + (ushort)((targetVector.Length() - 180) / 2));
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
                                                Projectiles[i].BoostSpeed();
                                                Vector2 tempAngleVector = Projectiles[i].MySprite.Position - new Vector2(320, 360);
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
                                            //if (level != 0)
                                              //  score += (ushort)(10 + (ushort)((targetVector.Length() - 180) / 2));
                                            continue;

                                        }
                                    }
                                }
                                break;
                            case Projectile.ProjectileType.Ice:
                                if (iceAlpha > 220)
                                {
                                    if (upperLimit - lowerLimit > 79.99f && upperLimit - lowerLimit < 80.01f)
                                    {
                                        if (iceAngle > lowerLimit && iceAngle < upperLimit)
                                        {
                                            if (bIsReflecting)
                                            {
                                                Projectiles[i].Reflected = true;
                                                Projectiles[i].BoostSpeed();
                                                Vector2 tempAngleVector = Projectiles[i].MySprite.Position - new Vector2(320, 360);
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
                                           // if (level != 0)
                                             //   score += (ushort)(10 + (ushort)((targetVector.Length() - 180) / 2));
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
                                                Projectiles[i].BoostSpeed();
                                                Vector2 tempAngleVector = Projectiles[i].MySprite.Position - new Vector2(320, 360);
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
                                            //if (level != 0)
                                              //  score += (ushort)(10 + (ushort)((targetVector.Length() - 180) / 2));
                                            continue;
                                        }
                                    }
                                }
                                break;
                            case Projectile.ProjectileType.Stun:
                                bool hasCollided = false;
                                if (upperLimit - lowerLimit > 79.99f && upperLimit - lowerLimit < 80.01f)
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
                                if (upperLimit - lowerLimit > 79.99f && upperLimit - lowerLimit < 80.01f)
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
                    else if (targetVector.Length() < 40)
                    {
                        
                            switch (Projectiles[i].MyType)
                            {
                                case Projectile.ProjectileType.Ice:
                                case Projectile.ProjectileType.Fire:
                                    health -= 5;
                                    GamePad.SetVibration(SelectTwoPlayerState.PublicPOne, 0.6f, 0.6f);
                                    isHit = true;
                                    rumbleTimer = TimeSpan.Zero;
                                    break;
                                case Projectile.ProjectileType.Antipode:
                                    health -= 8;
                                    GamePad.SetVibration(SelectTwoPlayerState.PublicPOne, 0.6f, 0.6f);
                                    isHit = true;
                                    rumbleTimer = TimeSpan.Zero;
                                    break;
                                case Projectile.ProjectileType.Arrow:
                                    health -= 10;
                                    GamePad.SetVibration(SelectTwoPlayerState.PublicPOne, 0.6f, 0.6f);
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
                    else if (Projectiles[i].Reflected && (Projectiles[i].MySprite.Position.X < -50 || Projectiles[i].MySprite.Position.X > 640 || Projectiles[i].MySprite.Position.Y < -50 || Projectiles[i].MySprite.Position.Y > 720))
                    {
                        Projectiles[i].Reflected = false;
                        Projectiles[i].MySprite.Position = new Vector2(Projectiles[i].MySprite.Position.X + 640, Projectiles[i].MySprite.Position.Y);
                        Vector2 tempAngleVector = Projectiles[i].MySprite.Position - new Vector2(320, 360);
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
                        Projectiles2.Add(Projectiles[i]);
                        Projectiles.Remove(Projectiles[i]);
                        i--;
                    }

                }// end for projectiles 1

                for (int i = 0; i < Projectiles2.Count; i++)
                {
                    Projectiles2[i].Update(gameTime);
                    Vector2 targetVector = new Vector2(960, 360);

                    Projectiles2[i].Move(targetVector);
                    targetVector = Projectiles2[i].MySprite.Position - targetVector;
                    Vector2 AngleVector = Projectiles2[i].MySprite.Position - new Vector2(960, 360);
                    AngleVector.Normalize();
                    /*
                    Projectiles2[i].MySprite.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
                    if (Projectiles2[i].Reflected)
                    {
                        Projectiles2[i].MySprite.Rotation += 179;
                        if (Projectiles2[i].MySprite.Rotation >= 360)
                            Projectiles2[i].MySprite.Rotation -= 360;
                    }*/
                    Angle = MathHelper.ToDegrees((float)Math.Atan2(-AngleVector.X, -AngleVector.Y));
                    if (Angle < 0)
                        Angle += 360;
                    //Angle += 180;
                    lowerLimit = Angle - 40;
                    //if it goes below 0, make it go full circle
                    if (lowerLimit < 0)
                    {
                        lowerLimit = 360 + lowerLimit;
                    }
                    upperLimit = Angle + 40;
                    //if it goes beyond 360, wrap it around
                    if (upperLimit >= 360)
                    {
                        upperLimit -= 360;
                    }

                    //handle spell blast logic here
                    if (bSpellBlast2)
                    {
                        if (Angle >= 45 && Angle <= 135 && bLeftPressed2)
                        {
                            Projectiles2.Remove(Projectiles2[i]);
                            i--;
                            continue;

                        }
                        else if (Angle >= 225 && Angle <= 315 && bRightPressed2)
                        {
                            Projectiles2.Remove(Projectiles2[i]);
                            i--;
                            continue;
                        }
                        else if (Angle >= 135 && Angle <= 225 && bDownPressed2)
                        {
                            Projectiles2.Remove(Projectiles2[i]);
                            i--;
                            continue;
                        }
                        else if ((Angle >= 315 || Angle <= 45) && bUpPressed2)
                        {
                            Projectiles2.Remove(Projectiles2[i]);
                            i--;
                            continue;
                        }
                    }

                    if (Projectiles2[i].MyType == Projectile.ProjectileType.Arrow)
                    {
                        if (Math.Abs(Projectiles2[i].MovementDir.X) < 50)
                        {
                            if ((Projectiles2[i].MovementDir.Y > 0 && bLightningUp2) || (Projectiles2[i].MovementDir.Y < 0 && bLightningDown2))
                            {
                                Projectiles2.Remove(Projectiles2[i]);
                                i--;
                                //score += 30;
                                continue;
                            }
                        }
                        else
                        {
                            if ((Projectiles2[i].MovementDir.X > 0 && bLightningLeft2) || (Projectiles2[i].MovementDir.X < 0 && bLightningRight2))
                            {
                                Projectiles2.Remove(Projectiles2[i]);
                                i--;
                                //score += 30;
                                continue;
                            }
                        }
                    }
                    //is the projectile in range to be countered?
                    if (targetVector.Length() > 50 && targetVector.Length() < 95 && !Projectiles2[i].Reflected)
                    {



                        switch (Projectiles2[i].MyType)
                        {


                            case Projectile.ProjectileType.Antipode:
                                //in the normal part of the circle
                                if (iceAlpha2 > 220 && fireAlpha2 > 190)
                                {
                                    if (upperLimit - lowerLimit > 79.99f && upperLimit - lowerLimit < 80.01f)
                                    {
                                        if (fireAngle2 > lowerLimit && fireAngle2 < upperLimit && iceAngle2 > lowerLimit && iceAngle2 < upperLimit)
                                        {
                                            if (bIsReflecting2)
                                            {
                                                Projectiles2[i].Reflected = true;
                                                Projectiles2[i].BoostSpeed(); 
                                                Vector2 tempAngleVector = Projectiles2[i].MySprite.Position - new Vector2(960, 360);
                                                tempAngleVector.Normalize();
                                                if (!(Projectiles2[i].MyType == Projectile.ProjectileType.Stun))
                                                {
                                                    Projectiles2[i].MySprite.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
                                                    if (Projectiles2[i].Reflected)
                                                    {
                                                        Projectiles2[i].MySprite.Rotation += 179;
                                                        if (Projectiles2[i].MySprite.Rotation >= 360)
                                                            Projectiles2[i].MySprite.Rotation -= 360;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Projectiles2.Remove(Projectiles2[i]);
                                                i--;
                                            }
                                            //fireHitColour = 255;
                                            //iceHitColour = 255;
                                            if (numSpellBlasts2 < 3)
                                                numDeflectCounter2++;
                                            if (numDeflectCounter2 == 30)
                                            {
                                                numDeflectCounter2 = 0;
                                                numSpellBlasts2++;
                                            }
                                            fireShield2.MyColour = new Color(255, 255, 255);
                                            iceShield2.MyColour = new Color(255, 255, 255);
                                            fireShieldOver2.MyColour = new Color(255, 255, 255);
                                            if (++difficultyCounter2 >= BULLETSTOADDDIFFICULTY)
                                            {
                                                difficultyCounter2 = 0;
                                                difficultyMod2++;
                                            }

                                            
                                            continue;
                                        }
                                    }
                                    //lower limit is in the 300's
                                    else
                                    {
                                        if ((fireAngle2 > lowerLimit || fireAngle2 < upperLimit) && (iceAngle2 > lowerLimit || iceAngle2 < upperLimit))
                                        {
                                            if (bIsReflecting2)
                                            {
                                                Projectiles2[i].Reflected = true;
                                                Projectiles2[i].BoostSpeed();
                                                Vector2 tempAngleVector = Projectiles2[i].MySprite.Position - new Vector2(960, 360);
                                                tempAngleVector.Normalize();
                                                if (!(Projectiles2[i].MyType == Projectile.ProjectileType.Stun))
                                                {
                                                    Projectiles2[i].MySprite.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
                                                    if (Projectiles2[i].Reflected)
                                                    {
                                                        Projectiles2[i].MySprite.Rotation += 179;
                                                        if (Projectiles2[i].MySprite.Rotation >= 360)
                                                            Projectiles2[i].MySprite.Rotation -= 360;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Projectiles2.Remove(Projectiles2[i]);
                                                i--;
                                            }
                                            //fireHitColour = 255;
                                            //iceHitColour = 255;
                                            if (numSpellBlasts2 < 3)
                                                numDeflectCounter2++;
                                            if (numDeflectCounter2 == 30)
                                            {
                                                numDeflectCounter2 = 0;
                                                numSpellBlasts2++;
                                            }
                                            fireShield2.MyColour = new Color(255, 255, 255);
                                            iceShield2.MyColour = new Color(255, 255, 255);
                                            fireShieldOver2.MyColour = new Color(255, 255, 255);
                                            if (++difficultyCounter2 >= BULLETSTOADDDIFFICULTY)
                                            {
                                                difficultyCounter2 = 0;
                                                difficultyMod2++;
                                            }
                                            
                                            continue;

                                        }
                                    }
                                }
                                break;
                            case Projectile.ProjectileType.Fire:
                                //in the normal part of the circle
                                if (fireAlpha2 > 190)
                                {
                                    if (upperLimit - lowerLimit > 79.99f && upperLimit - lowerLimit < 80.01f)
                                    {
                                        if (fireAngle2 > lowerLimit && fireAngle2 < upperLimit)
                                        {
                                            if (bIsReflecting2)
                                            {
                                                Projectiles2[i].Reflected = true;
                                                Projectiles2[i].BoostSpeed();
                                                Vector2 tempAngleVector = Projectiles2[i].MySprite.Position - new Vector2(960, 360);
                                                tempAngleVector.Normalize();
                                                if (!(Projectiles2[i].MyType == Projectile.ProjectileType.Stun))
                                                {
                                                    Projectiles2[i].MySprite.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
                                                    if (Projectiles2[i].Reflected)
                                                    {
                                                        Projectiles2[i].MySprite.Rotation += 179;
                                                        if (Projectiles2[i].MySprite.Rotation >= 360)
                                                            Projectiles2[i].MySprite.Rotation -= 360;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Projectiles2.Remove(Projectiles2[i]);
                                                i--;
                                            }
                                            if (numSpellBlasts2 < 3)
                                                numDeflectCounter2++;
                                            if (numDeflectCounter2 == 30)
                                            {
                                                numDeflectCounter2 = 0;
                                                numSpellBlasts2++;
                                            }
                                            //fireHitColour = 255;
                                            fireShield2.MyColour = new Color(255, 255, 255);
                                            fireShieldOver2.MyColour = new Color(255, 255, 255);
                                            if (++difficultyCounter2 >= BULLETSTOADDDIFFICULTY)
                                            {
                                                difficultyCounter2 = 0;
                                                difficultyMod2++;
                                            }
                                            
                                            continue;
                                        }
                                    }
                                    //lower limit is in the 300's
                                    else
                                    {
                                        if (fireAngle2 > lowerLimit || fireAngle2 < upperLimit)
                                        {
                                            if (bIsReflecting2)
                                            {
                                                Projectiles2[i].Reflected = true;
                                                Projectiles2[i].BoostSpeed();
                                                Vector2 tempAngleVector = Projectiles2[i].MySprite.Position - new Vector2(960, 360);
                                                tempAngleVector.Normalize();
                                                if (!(Projectiles2[i].MyType == Projectile.ProjectileType.Stun))
                                                {
                                                    Projectiles2[i].MySprite.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
                                                    if (Projectiles2[i].Reflected)
                                                    {
                                                        Projectiles2[i].MySprite.Rotation += 179;
                                                        if (Projectiles2[i].MySprite.Rotation >= 360)
                                                            Projectiles2[i].MySprite.Rotation -= 360;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Projectiles2.Remove(Projectiles2[i]);
                                                i--;
                                            }
                                            if (numSpellBlasts2 < 3)
                                                numDeflectCounter2++;
                                            if (numDeflectCounter2 == 30)
                                            {
                                                numDeflectCounter2 = 0;
                                                numSpellBlasts2++;
                                            }
                                            //fireHitColour = 255;
                                            fireShield2.MyColour = new Color(255, 255, 255);
                                            fireShieldOver2.MyColour = new Color(255, 255, 255);
                                            if (++difficultyCounter2 >= BULLETSTOADDDIFFICULTY)
                                            {
                                                difficultyCounter2 = 0;
                                                difficultyMod2++;
                                            }
                                            
                                            continue;

                                        }
                                    }
                                }
                                break;
                            case Projectile.ProjectileType.Ice:
                                if (iceAlpha2 > 220)
                                {
                                    if (upperLimit - lowerLimit > 79.99f && upperLimit - lowerLimit < 80.01f)
                                    {
                                        if (iceAngle2 > lowerLimit && iceAngle2 < upperLimit)
                                        {
                                            if (bIsReflecting2)
                                            {
                                                Projectiles2[i].Reflected = true;
                                                Projectiles2[i].BoostSpeed();
                                                Vector2 tempAngleVector = Projectiles2[i].MySprite.Position - new Vector2(960, 360);
                                                tempAngleVector.Normalize();
                                                if (!(Projectiles2[i].MyType == Projectile.ProjectileType.Stun))
                                                {
                                                    Projectiles2[i].MySprite.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
                                                    if (Projectiles2[i].Reflected)
                                                    {
                                                        Projectiles2[i].MySprite.Rotation += 179;
                                                        if (Projectiles2[i].MySprite.Rotation >= 360)
                                                            Projectiles2[i].MySprite.Rotation -= 360;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Projectiles2.Remove(Projectiles2[i]);
                                                i--;
                                            }
                                            if (numSpellBlasts2 < 3)
                                                numDeflectCounter2++;
                                            if (numDeflectCounter2 == 30)
                                            {
                                                numDeflectCounter2 = 0;
                                                numSpellBlasts2++;
                                            }
                                            //iceHitColour = 255;
                                            iceShield2.MyColour = new Color(255, 255, 255);

                                            if (++difficultyCounter2 >= BULLETSTOADDDIFFICULTY)
                                            {
                                                difficultyCounter2 = 0;
                                                difficultyMod2++;
                                            }
                                            
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        if (iceAngle2 > lowerLimit || iceAngle2 < upperLimit)
                                        {
                                            if (bIsReflecting2)
                                            {
                                                Projectiles2[i].Reflected = true;
                                                Projectiles2[i].BoostSpeed();
                                                Vector2 tempAngleVector = Projectiles2[i].MySprite.Position - new Vector2(960, 360);
                                                tempAngleVector.Normalize();
                                                if (!(Projectiles2[i].MyType == Projectile.ProjectileType.Stun))
                                                {
                                                    Projectiles2[i].MySprite.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
                                                    if (Projectiles2[i].Reflected)
                                                    {
                                                        Projectiles2[i].MySprite.Rotation += 179;
                                                        if (Projectiles2[i].MySprite.Rotation >= 360)
                                                            Projectiles2[i].MySprite.Rotation -= 360;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Projectiles2.Remove(Projectiles2[i]);
                                                i--;
                                            }
                                            if (numSpellBlasts2 < 3)
                                                numDeflectCounter2++;
                                            if (numDeflectCounter2 == 30)
                                            {
                                                numDeflectCounter2 = 0;
                                                numSpellBlasts2++;
                                            }
                                            //iceHitColour = 255;
                                            iceShield2.MyColour = new Color(255, 255, 255);
                                            if (++difficultyCounter2 >= BULLETSTOADDDIFFICULTY)
                                            {
                                                difficultyCounter2 = 0;
                                                difficultyMod2++;
                                            }
                                            
                                            continue;
                                        }
                                    }
                                }
                                break;
                            case Projectile.ProjectileType.Stun:
                                bool hasCollided = false;
                                if (upperLimit - lowerLimit > 79.99f && upperLimit - lowerLimit < 80.01f)
                                {
                                    if (iceAngle2 > lowerLimit && iceAngle2 < upperLimit && iceAlpha2 > 220)
                                    {
                                        //Projectiles.Remove(Projectiles2[i]);
                                        //i--;
                                        //iceHitColour = 255;
                                        //iceShield.MyColour = new Color(255, 255, 255);
                                        //continue;
                                        if (bIsReflecting2)
                                        {
                                            reflectPercent2 -= 15;
                                            if (reflectPercent2 < 0)
                                            {
                                                reflectPercent2 = 0;
                                                reflectTimer2 = TimeSpan.Zero;
                                                bIsReflecting2 = false;
                                            }
                                        }
                                        else
                                            iceStun2 = true;
                                        hasCollided = true;

                                    }
                                    if (fireAngle2 > lowerLimit && fireAngle2 < upperLimit && fireAlpha2 > 190)
                                    {
                                        //Projectiles.Remove(Projectiles2[i]);
                                        //i--;
                                        //iceHitColour = 255;
                                        //iceShield.MyColour = new Color(255, 255, 255);
                                        //continue;
                                        if (bIsReflecting2)
                                        {
                                            reflectPercent2 -= 15;
                                            if (reflectPercent2 < 0)
                                            {
                                                reflectPercent2 = 0;
                                                reflectTimer2 = TimeSpan.Zero;
                                                bIsReflecting2 = false;
                                            }
                                        }
                                        else
                                            fireStun2 = true;
                                        hasCollided = true;

                                    }

                                }
                                else
                                {
                                    if ((iceAngle2 > lowerLimit || iceAngle2 < upperLimit) && iceAlpha2 > 220)
                                    {
                                        //Projectiles.Remove(Projectiles2[i]);
                                        //i--;
                                        //iceHitColour = 255;
                                        //iceShield.MyColour = new Color(255, 255, 255);
                                        //continue;
                                        if (bIsReflecting2)
                                        {
                                            reflectPercent2 -= 15;
                                            if (reflectPercent2 < 0)
                                            {
                                                reflectPercent2 = 0;
                                                reflectTimer2 = TimeSpan.Zero;
                                                bIsReflecting2 = false;
                                            }
                                        }
                                        else
                                            iceStun2 = true;
                                        hasCollided = true;

                                    }
                                    if ((fireAngle2 > lowerLimit || fireAngle2 < upperLimit) && fireAlpha2 > 190)
                                    {
                                        //Projectiles.Remove(Projectiles2[i]);
                                        //i--;
                                        //iceHitColour = 255;
                                        //iceShield.MyColour = new Color(255, 255, 255);
                                        //continue;
                                        if (bIsReflecting2)
                                        {
                                            reflectPercent2 -= 15;
                                            if (reflectPercent2 < 0)
                                            {
                                                reflectPercent2 = 0;
                                                reflectTimer2 = TimeSpan.Zero;
                                                bIsReflecting2 = false;
                                            }
                                        }
                                        else
                                            fireStun2 = true;
                                        hasCollided = true;
                                    }
                                }
                                if (hasCollided)
                                {
                                    Projectiles2.Remove(Projectiles2[i]);
                                    i--;
                                    continue;
                                }
                                break;
                            case Projectile.ProjectileType.Counter:
                                hasCollided = false;
                                if (upperLimit - lowerLimit > 79.99f && upperLimit - lowerLimit < 80.01f)
                                {
                                    if (iceAngle2 > lowerLimit && iceAngle2 < upperLimit && iceAlpha2 > 220)
                                    {
                                        //Projectiles.Remove(Projectiles2[i]);
                                        //i--;
                                        //iceHitColour = 255;
                                        //iceShield.MyColour = new Color(255, 255, 255);
                                        //continue;
                                        if (bIsReflecting2)
                                        {
                                            reflectPercent2 -= 15;
                                            if (reflectPercent2 < 0)
                                            {
                                                reflectPercent2 = 0;
                                                reflectTimer2 = TimeSpan.Zero;
                                                bIsReflecting2 = false;
                                            }
                                        }
                                        else
                                            iceAlpha2 = 0;
                                        hasCollided = true;
                                    }
                                    if (fireAngle2 > lowerLimit && fireAngle2 < upperLimit && fireAlpha2 > 190)
                                    {
                                        //Projectiles.Remove(Projectiles2[i]);
                                        //i--;
                                        //iceHitColour = 255;
                                        //iceShield.MyColour = new Color(255, 255, 255);
                                        //continue;
                                        if (bIsReflecting2)
                                        {
                                            reflectPercent2 -= 15;
                                            if (reflectPercent2 < 0)
                                            {
                                                reflectPercent2 = 0;
                                                reflectTimer2 = TimeSpan.Zero;
                                                bIsReflecting2 = false;
                                            }
                                        }
                                        else
                                            fireAlpha2 = 0;
                                        hasCollided = true;
                                    }

                                }
                                else
                                {
                                    if ((iceAngle2 > lowerLimit || iceAngle2 < upperLimit) && iceAlpha2 > 220)
                                    {
                                        //Projectiles.Remove(Projectiles2[i]);
                                        //i--;
                                        //iceHitColour = 255;
                                        //iceShield.MyColour = new Color(255, 255, 255);
                                        //continue;
                                        if (bIsReflecting2)
                                        {
                                            reflectPercent2 -= 15;
                                            if (reflectPercent2 < 0)
                                            {
                                                reflectPercent2 = 0;
                                                reflectTimer2 = TimeSpan.Zero;
                                                bIsReflecting2 = false;
                                            }
                                        }
                                        else
                                            iceAlpha2 = 0;
                                        hasCollided = true;
                                    }
                                    if ((fireAngle2 > lowerLimit || fireAngle2 < upperLimit) && fireAlpha2 > 190)
                                    {
                                        //Projectiles.Remove(Projectiles2[i]);
                                        //i--;
                                        //iceHitColour = 255;
                                        //iceShield.MyColour = new Color(255, 255, 255);
                                        //continue;
                                        if (bIsReflecting2)
                                        {
                                            reflectPercent2 -= 15;
                                            if (reflectPercent2 < 0)
                                            {
                                                reflectPercent2 = 0;
                                                reflectTimer2 = TimeSpan.Zero;
                                                bIsReflecting2 = false;
                                            }
                                        }
                                        else
                                            fireAlpha2 = 0;
                                        hasCollided = true;
                                    }
                                }
                                if (hasCollided)
                                {
                                    Projectiles2.Remove(Projectiles2[i]);
                                    i--;
                                    continue;
                                }
                                break;

                        }
                    }
                    else if (targetVector.Length() < 40)
                    {
                        
                            switch (Projectiles2[i].MyType)
                            {
                                case Projectile.ProjectileType.Ice:
                                case Projectile.ProjectileType.Fire:
                                    health2 -= 5;
                                    GamePad.SetVibration(SelectTwoPlayerState.PublicPTwo, 0.6f, 0.6f);
                                    isHit2 = true;
                                    rumbleTimer2 = TimeSpan.Zero;
                                    break;
                                case Projectile.ProjectileType.Antipode:
                                    health2 -= 8;
                                    GamePad.SetVibration(SelectTwoPlayerState.PublicPTwo, 0.6f, 0.6f);
                                    isHit2 = true;
                                    rumbleTimer2 = TimeSpan.Zero;
                                    break;
                                case Projectile.ProjectileType.Arrow:
                                    health2 -= 10;
                                    GamePad.SetVibration(SelectTwoPlayerState.PublicPTwo, 0.6f, 0.6f);
                                    isHit2 = true;
                                    rumbleTimer2 = TimeSpan.Zero;
                                    break;
                                case Projectile.ProjectileType.Counter:
                                case Projectile.ProjectileType.Stun:
                                    if (++difficultyCounter2 >= BULLETSTOADDDIFFICULTY)
                                    {
                                        difficultyCounter2 = 0;
                                        difficultyMod2++;
                                    }
                                    break;
                            }
                       
                        Projectiles2.Remove(Projectiles2[i]);
                        i--;
                        if (health2 <= 0)
                        {
                            health2 = 0;
                            break;
                        }
                        // hitCount++;
                        continue;
                    }
                        //CHANGE THIS TO ADD A PROJECTILE TO THE OPPONENT
                    else if (Projectiles2[i].Reflected && (Projectiles2[i].MySprite.Position.X < 630 || Projectiles2[i].MySprite.Position.X > 1280 || Projectiles2[i].MySprite.Position.Y < -50 || Projectiles2[i].MySprite.Position.Y > 720))
                    {
                        Projectiles2[i].Reflected = false;
                        Projectiles2[i].MySprite.Position = new Vector2(Projectiles2[i].MySprite.Position.X - 640, Projectiles2[i].MySprite.Position.Y);
                        Vector2 tempAngleVector = Projectiles2[i].MySprite.Position - new Vector2(960, 360);
                        tempAngleVector.Normalize();
                        if (!(Projectiles2[i].MyType == Projectile.ProjectileType.Stun))
                        {
                            Projectiles2[i].MySprite.Rotation = (float)Math.Atan2(AngleVector.X, -AngleVector.Y);
                            if (Projectiles2[i].Reflected)
                            {
                                Projectiles2[i].MySprite.Rotation += 179;
                                if (Projectiles2[i].MySprite.Rotation >= 360)
                                    Projectiles2[i].MySprite.Rotation -= 360;
                            }
                        }
                        Projectiles.Add(Projectiles2[i]);
                        Projectiles2.Remove(Projectiles2[i]);
                        i--;
                    }

                }//end for projectiles 2

                //stuff for the center
                
                centerStone.Update(gameTime);
                       
                bSpellBlast = false;
                bSpellBlast2 = false;
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

                if (fireShield2.MyColour.R > fireRed2)
                    tempfireRed2 = (byte)(fireShield2.MyColour.R - 2);
                else if (fireShield2.MyColour.R < fireRed2)
                    tempfireRed2 = fireRed2;
                if (fireShield2.MyColour.G > fireGreen2)
                    tempfireGreen2 = (byte)(fireShield2.MyColour.G - 2);
                else if (fireShield2.MyColour.G < fireGreen2)
                    tempfireGreen2 = fireGreen2;
                if (fireShield2.MyColour.B > fireBlue2)
                    tempfireBlue2 = (byte)(fireShield2.MyColour.B - 2);
                else if (fireShield2.MyColour.B < fireBlue2)
                    tempfireBlue2 = fireBlue2;

                if (fireShieldOver2.MyColour.R > fireOverRed2)
                    tempfireOverRed2 = (byte)(fireShieldOver2.MyColour.R - 2);
                else if (fireShieldOver2.MyColour.R < fireOverRed2)
                    tempfireOverRed2 = fireOverRed2;
                if (fireShieldOver2.MyColour.G > fireOverGreen2)
                    tempfireOverGreen2 = (byte)(fireShieldOver2.MyColour.G - 2);
                else if (fireShieldOver2.MyColour.G < fireOverGreen2)
                    tempfireOverGreen2 = fireOverGreen2;
                if (fireShieldOver2.MyColour.B > 30)
                    tempfireOverBlue2 = (byte)(fireShieldOver2.MyColour.B - 2);
                else
                    tempfireOverBlue2 = fireOverBlue2;

                if (iceAlpha < 255)
                {
                    //iceCounterTimer += gameTime.ElapsedGameTime;
                    //if(iceCounterTimer >= TimeSpan.FromMilliseconds(80))
                    //{
                    iceAlpha++;
                    //    iceCounterTimer -= TimeSpan.FromMilliseconds(80);
                    //}
                }
                if (iceAlpha2 < 255)
                {
                    //iceCounterTimer += gameTime.ElapsedGameTime;
                    //if(iceCounterTimer >= TimeSpan.FromMilliseconds(80))
                    //{
                    iceAlpha2++;
                    //    iceCounterTimer -= TimeSpan.FromMilliseconds(80);
                    //}
                }

                

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
                if (iceShield2.MyColour.R > iceRed2)
                    tempiceRed2 = (byte)(iceShield2.MyColour.R - 2);
                else if (iceShield2.MyColour.R < iceRed2)
                    tempiceRed2 = iceRed2;
                if (iceShield2.MyColour.G > iceGreen2)
                    tempiceGreen2 = (byte)(iceShield2.MyColour.G - 2);
                else if (iceShield2.MyColour.G < iceGreen2)
                    tempiceGreen2 = iceGreen2;
                if (iceShield2.MyColour.B > iceBlue2)
                    tempiceBlue2 = (byte)(iceShield2.MyColour.B - 2);
                else if (iceShield2.MyColour.B < iceBlue2)
                    tempiceBlue2 = iceBlue2;

                if (fireAlpha2 < 210)
                {
                    //fireCounterTimer += gameTime.ElapsedGameTime;
                    //if (fireCounterTimer >= TimeSpan.FromMilliseconds(30))
                    //{
                    fireAlpha2++;
                    //    fireCounterTimer -= TimeSpan.FromMilliseconds(30);
                    //}
                }

                fireShield.MyColour = new Color(tempfireRed, tempfireGreen, tempfireBlue);
                fireShield.MyAlpha = (float)(fireAlpha) / 255;
                fireShieldOver.MyColour = new Color(tempfireOverRed, tempfireOverGreen, tempfireOverBlue);
                fireShieldOver.MyAlpha = (float)(fireAlpha) / 255;
                iceShield.MyColour = new Color(tempiceRed, tempiceGreen, tempiceBlue);
                iceShield.MyAlpha = (float)(iceAlpha) / 255;
                iceShieldOver.MyColour = new Color(255,255,255);
                iceShieldOver.MyAlpha = (float)(iceAlpha) / 255;
                fireShield2.MyColour = new Color(tempfireRed2, tempfireGreen2, tempfireBlue2);
                fireShield2.MyAlpha = (float)(fireAlpha2) / 255;
                fireShieldOver2.MyColour = new Color(tempfireOverRed2, tempfireOverGreen2, tempfireOverBlue2);
                fireShieldOver2.MyAlpha = (float)(fireAlpha2) / 255;
                iceShield2.MyColour = new Color(tempiceRed2, tempiceGreen2, tempiceBlue2);
                iceShield2.MyAlpha = (float)(iceAlpha2) / 255;
                iceShieldOver2.MyColour = new Color(255, 255, 255);
                iceShieldOver2.MyAlpha = (float)(iceAlpha2) / 255;

                if (PlayerOne.Buttons.Start == ButtonState.Released && PlayerTwo.Buttons.Start == ButtonState.Released)
                    bStartPressed = false;
                if (PlayerOne.DPad.Down == ButtonState.Released)
                    bDownPressed = false;
                if (PlayerOne.DPad.Up == ButtonState.Released)
                    bUpPressed = false;
                if (PlayerOne.DPad.Left == ButtonState.Released)
                    bLeftPressed = false;
                if (PlayerOne.DPad.Right == ButtonState.Released)
                    bRightPressed = false;
                if (PlayerTwo.DPad.Down == ButtonState.Released)
                    bDownPressed2 = false;
                if (PlayerTwo.DPad.Up == ButtonState.Released)
                    bUpPressed2 = false;
                if (PlayerTwo.DPad.Left == ButtonState.Released)
                    bLeftPressed2 = false;
                if (PlayerTwo.DPad.Right == ButtonState.Released)
                    bRightPressed2 = false;

                if (!bDownPressed && !bUpPressed && !bLeftPressed && !bRightPressed)
                {
                    bSpellBlastPressed = false;
                }
                if (!bDownPressed2 && !bUpPressed2 && !bLeftPressed2 && !bRightPressed2)
                {
                    bSpellBlastPressed2 = false;
                }
                /*
                if(health < 100)
                stoneColour = (byte)(255 - Math.Floor((double)(health / 100) * 255));
                if(health2 < 100)
                stoneColour2 = (byte)(255 - Math.Floor((double)(health2 / 100) * 255));

                centerStone.MyColour = new Color(255, stoneColour, stoneColour);
                centerStone2.MyColour = new Color(255, stoneColour2, stoneColour2);
                */
                centerDot.MyColour = new Color(new Vector4(255,255,255, (byte)(((float)(100 - health) / 100) * 255)));
                centerDot2.MyColour = new Color(new Vector4(255, 255, 255, (byte)(((float)(100 - health2) / 100) * 255)));

                if (bAPressed && PlayerOne.Buttons.A == ButtonState.Released)
                    bAPressed = false;
                if (bAPressed2 && PlayerTwo.Buttons.A == ButtonState.Released)
                    bAPressed2 = false;
                base.Update(gameTime);
            }//end if not paused
        }//end update

        public void HandleAddProjectiles(GameTime gameTime)
        {
            //handle timer stuff
            myTimer += gameTime.ElapsedGameTime;
            switch (myPattern)
            {
                case ProjectilePattern.NONE:
                    if (myTimer > TimeSpan.FromMilliseconds(BASEADDBULLETTIME * 2 + addBulletRate - (DIFFICULTYTIME * difficultyMod)))
                    {
                        if (random.Next(100) < (BASEADDBULLETPERCENT + addBulletPercent))
                        {

                            AddBullet(SelectTwoPlayerState.PublicPOne);
                            //rotation += 0.1f;
                            myTimer -= TimeSpan.FromMilliseconds(BASEADDBULLETTIME + addBulletRate - (DIFFICULTYTIME * difficultyMod));
                            addBulletRate = 0;
                            addBulletPercent = 0;
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
                            AddBullet(Projectile.ProjectileType.Fire, new Vector2(-50, 345), SelectTwoPlayerState.PublicPOne);
                            AddBullet(Projectile.ProjectileType.Ice, new Vector2(640, 345), SelectTwoPlayerState.PublicPOne);
                        }
                        else if (patternCounter == 1)
                        {
                            AddBullet(Projectile.ProjectileType.Fire, new Vector2(-50, -50), SelectTwoPlayerState.PublicPOne);
                            AddBullet(Projectile.ProjectileType.Ice, new Vector2(640, 720), SelectTwoPlayerState.PublicPOne);
                            AddBullet(Projectile.ProjectileType.Counter, new Vector2(-50, 345), SelectTwoPlayerState.PublicPOne);
                            AddBullet(Projectile.ProjectileType.Counter, new Vector2(640, 345), SelectTwoPlayerState.PublicPOne);
                        }
                        else if (patternCounter == 2)
                        {
                            AddBullet(Projectile.ProjectileType.Fire, new Vector2(313, -50), SelectTwoPlayerState.PublicPOne);
                            AddBullet(Projectile.ProjectileType.Ice, new Vector2(313, 720), SelectTwoPlayerState.PublicPOne);
                            AddBullet(Projectile.ProjectileType.Counter, new Vector2(-50, -50), SelectTwoPlayerState.PublicPOne);
                            AddBullet(Projectile.ProjectileType.Counter, new Vector2(640, 720), SelectTwoPlayerState.PublicPOne);
                        }
                        else if (patternCounter == 3)
                        {
                            AddBullet(Projectile.ProjectileType.Fire, new Vector2(640, -50), SelectTwoPlayerState.PublicPOne);
                            AddBullet(Projectile.ProjectileType.Ice, new Vector2(-50, 720), SelectTwoPlayerState.PublicPOne);
                            AddBullet(Projectile.ProjectileType.Counter, new Vector2(313, -50), SelectTwoPlayerState.PublicPOne);
                            AddBullet(Projectile.ProjectileType.Counter, new Vector2(313, 720), SelectTwoPlayerState.PublicPOne);
                        }
                        else if (patternCounter == 4)
                        {
                            AddBullet(Projectile.ProjectileType.Fire, new Vector2(640, 345), SelectTwoPlayerState.PublicPOne);
                            AddBullet(Projectile.ProjectileType.Ice, new Vector2(-50, 345), SelectTwoPlayerState.PublicPOne);
                            AddBullet(Projectile.ProjectileType.Counter, new Vector2(-50, -50), SelectTwoPlayerState.PublicPOne);
                            AddBullet(Projectile.ProjectileType.Counter, new Vector2(640, 720), SelectTwoPlayerState.PublicPOne);
                        }
                        else if (patternCounter == 5)
                        {
                            AddBullet(Projectile.ProjectileType.Counter, new Vector2(640, 345), SelectTwoPlayerState.PublicPOne);
                            AddBullet(Projectile.ProjectileType.Counter, new Vector2(-50, 345), SelectTwoPlayerState.PublicPOne);
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
                            AddBullet(Projectile.ProjectileType.Fire, new Vector2(-50, 360), SelectTwoPlayerState.PublicPOne);
                            AddBullet(Projectile.ProjectileType.Ice, new Vector2(640, 360), SelectTwoPlayerState.PublicPOne);
                        }
                        else if (patternCounter == 2)
                        {
                            AddBullet(Projectile.ProjectileType.Stun, new Vector2(-50, 360), SelectTwoPlayerState.PublicPOne);
                            AddBullet(Projectile.ProjectileType.Stun, new Vector2(640, 360), SelectTwoPlayerState.PublicPOne);
                        }
                        else if (patternCounter == 3)
                        {
                            AddBullet(Projectile.ProjectileType.Ice, new Vector2(-50, 360), SelectTwoPlayerState.PublicPOne);
                            AddBullet(Projectile.ProjectileType.Fire, new Vector2(640, 360), SelectTwoPlayerState.PublicPOne);
                        }
                        else if (patternCounter == 4)
                        {
                            AddBullet(Projectile.ProjectileType.Stun, new Vector2(-50, 360), SelectTwoPlayerState.PublicPOne);
                            AddBullet(Projectile.ProjectileType.Stun, new Vector2(640, 360), SelectTwoPlayerState.PublicPOne);
                        }
                        else if (patternCounter == 5)
                        {
                            AddBullet(Projectile.ProjectileType.Counter, new Vector2(-50, 360), SelectTwoPlayerState.PublicPOne);
                            AddBullet(Projectile.ProjectileType.Counter, new Vector2(640, 360), SelectTwoPlayerState.PublicPOne);
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

                    myTimer2 += gameTime.ElapsedGameTime;
                    switch (myPattern2)
                    {
                        case ProjectilePattern.NONE:
                            if (myTimer2 > TimeSpan.FromMilliseconds(BASEADDBULLETTIME * 2 + addBulletRate2 - (DIFFICULTYTIME * difficultyMod2)))
                            {
                                if (random.Next(100) < (BASEADDBULLETPERCENT + addBulletPercent2))
                                {

                                    AddBullet(SelectTwoPlayerState.PublicPTwo);
                                    //rotation += 0.1f;
                                    myTimer2 -= TimeSpan.FromMilliseconds(BASEADDBULLETTIME + addBulletRate2 - (DIFFICULTYTIME * difficultyMod2));
                                    addBulletRate2 = 0;
                                    addBulletPercent2 = 0;
                                    patternCounter2++;
                                    if (patternCounter2 == 15)
                                    {
                                        patternCounter2 = 0;
                                        int ran = random.Next(100);
                                        if (ran < 15)
                                            myPattern2 = ProjectilePattern.COUNTER_SPIN;
                                        else if (ran < 30)
                                            myPattern2 = ProjectilePattern.BALL_STUN_COUNTER;
                                    }
                                }
                                else
                                {
                                    addBulletRate2 += ADDBULLETTIME;
                                    addBulletPercent2 += ADDBULLETPERCENT;
                                }
                            }
                            break;
                        case ProjectilePattern.COUNTER_SPIN:
                            if (myTimer2 > TimeSpan.FromMilliseconds(1300))
                            {
                                myTimer2 -= TimeSpan.FromMilliseconds(1300);
                                if (patternCounter2 == 0)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(640, 345), SelectTwoPlayerState.PublicPTwo);
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(1280, 345), SelectTwoPlayerState.PublicPTwo);
                                }
                                else if (patternCounter2 == 1)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(640, -50), SelectTwoPlayerState.PublicPTwo);
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(1280, 720), SelectTwoPlayerState.PublicPTwo);
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(640, 345), SelectTwoPlayerState.PublicPTwo);
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(1280, 345), SelectTwoPlayerState.PublicPTwo);
                                }
                                else if (patternCounter2 == 2)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(965, -50), SelectTwoPlayerState.PublicPTwo);
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(965, 720), SelectTwoPlayerState.PublicPTwo);
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(640, -50), SelectTwoPlayerState.PublicPTwo);
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(1280, 720), SelectTwoPlayerState.PublicPTwo);
                                }
                                else if (patternCounter2 == 3)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(1280, -50), SelectTwoPlayerState.PublicPTwo);
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(640, 720), SelectTwoPlayerState.PublicPTwo);
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(965, -50), SelectTwoPlayerState.PublicPTwo);
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(965, 720), SelectTwoPlayerState.PublicPTwo);
                                }
                                else if (patternCounter2 == 4)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(1280, 345), SelectTwoPlayerState.PublicPTwo);
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(640, 345), SelectTwoPlayerState.PublicPTwo);
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(640, -50), SelectTwoPlayerState.PublicPTwo);
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(1280, 720), SelectTwoPlayerState.PublicPTwo);
                                }
                                else if (patternCounter2 == 5)
                                {
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(1280, 345), SelectTwoPlayerState.PublicPTwo);
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(640, 345), SelectTwoPlayerState.PublicPTwo);
                                }
                                else if (patternCounter2 == 6)
                                {
                                    patternCounter2 = 0;
                                    myPattern2 = ProjectilePattern.NONE;
                                }
                                patternCounter2++;
                            }
                            break;
                        case ProjectilePattern.BALL_STUN_COUNTER:
                            if (myTimer2 > TimeSpan.FromMilliseconds(900))
                            {
                                myTimer2 -= TimeSpan.FromMilliseconds(900);
                                if (patternCounter2 == 1)
                                {
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(640, 360), SelectTwoPlayerState.PublicPTwo);
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(1280, 360), SelectTwoPlayerState.PublicPTwo);
                                }
                                else if (patternCounter2 == 2)
                                {
                                    AddBullet(Projectile.ProjectileType.Stun, new Vector2(640, 360), SelectTwoPlayerState.PublicPTwo);
                                    AddBullet(Projectile.ProjectileType.Stun, new Vector2(1280, 360), SelectTwoPlayerState.PublicPTwo);
                                }
                                else if (patternCounter2 == 3)
                                {
                                    AddBullet(Projectile.ProjectileType.Ice, new Vector2(640, 360), SelectTwoPlayerState.PublicPTwo);
                                    AddBullet(Projectile.ProjectileType.Fire, new Vector2(1280, 360), SelectTwoPlayerState.PublicPTwo);
                                }
                                else if (patternCounter2 == 4)
                                {
                                    AddBullet(Projectile.ProjectileType.Stun, new Vector2(640, 360), SelectTwoPlayerState.PublicPTwo);
                                    AddBullet(Projectile.ProjectileType.Stun, new Vector2(1280, 360), SelectTwoPlayerState.PublicPTwo);
                                }
                                else if (patternCounter2 == 5)
                                {
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(640, 360), SelectTwoPlayerState.PublicPTwo);
                                    AddBullet(Projectile.ProjectileType.Counter, new Vector2(1280, 360), SelectTwoPlayerState.PublicPTwo);
                                }
                                else if (patternCounter2 == 6)
                                {
                                    patternCounter2 = 0;
                                    myPattern2 = ProjectilePattern.NONE;
                                }
                                patternCounter2++;
                            }
                            break;
                    }


                    if (difficultyMod > 1)
                    {
                        arrowTimer += gameTime.ElapsedGameTime;

                        if (arrowTimer > TimeSpan.FromMilliseconds(BASEADDARROWTIME * 2 + addArrowRate - (DIFFICULTYTIME * difficultyMod)))
                        {
                            if (random.Next(100) < (BASEADDBULLETPERCENT + addArrowPercent))
                            {

                                AddArrow(SelectTwoPlayerState.PublicPOne);
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
                    }
                    if (difficultyMod2 > 1)
                    {
                        arrowTimer2 += gameTime.ElapsedGameTime;

                        if (arrowTimer2 > TimeSpan.FromMilliseconds(BASEADDARROWTIME * 2 + addArrowRate2 - (DIFFICULTYTIME * difficultyMod2)))
                        {
                            if (random.Next(100) < (BASEADDBULLETPERCENT + addArrowPercent))
                            {

                                AddArrow(SelectTwoPlayerState.PublicPTwo);
                                //rotation += 0.1f;
                                arrowTimer2 -= TimeSpan.FromMilliseconds(BASEADDARROWTIME * 2 + addArrowRate2 - (DIFFICULTYTIME * difficultyMod2));
                                addArrowRate2 = 0;
                                addArrowPercent2 = 0;
                            }
                            else
                            {
                                addArrowRate2 += ADDBULLETTIME;
                                addArrowPercent2 += ADDBULLETPERCENT;
                            }
                        }
                    }

        }

        public void AddBullet(Projectile.ProjectileType type, Vector2 pos, PlayerIndex pIndex)
        {
            Projectile tempProjectile = new Projectile(type);
            if (type == Projectile.ProjectileType.Fire || type == Projectile.ProjectileType.Ice)
            {
                tempProjectile.MySprite.Animate(0, 5);
                tempProjectile.MySprite.FrameMod = 1.5f;
            }
            tempProjectile.MySprite.Position = pos;
            tempProjectile.MySprite.Pivot = new Vector2(tempProjectile.MySprite.Size.X / 4, tempProjectile.MySprite.Size.Y / 4);
            tempProjectile.MySprite.SizeModX = 0.5f;
            tempProjectile.MySprite.SizeModY = 0.5f;

            tempProjectile.MySprite.Pivot = new Vector2(tempProjectile.MySprite.Size.X / 4, tempProjectile.MySprite.Size.Y / 4);
            if (pIndex == SelectTwoPlayerState.PublicPOne)
            {
                Vector2 AngleVector = tempProjectile.MySprite.Position - new Vector2(320, 360);
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
            }
            else if (pIndex == SelectTwoPlayerState.PublicPTwo)
            {
                Vector2 AngleVector = tempProjectile.MySprite.Position - new Vector2(960, 360);
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
            }
            if (pIndex == SelectTwoPlayerState.PublicPOne)
                Projectiles.Add(tempProjectile);
            else if (pIndex == SelectTwoPlayerState.PublicPTwo)
                Projectiles2.Add(tempProjectile);
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

        public void AddBullet(PlayerIndex pIndex)
        {

            Vector2 addPosition = Vector2.Zero;
            Projectile tempProjectile;

            if(pIndex == SelectTwoPlayerState.PublicPOne)
            {
            if (random.Next(100) > 49)
            {
                //in this case, add it from the left or right.  Left or right should be either 0 or screen limit
                if (random.Next(100) > 49)
                    addPosition.X = -50;
                else
                    addPosition.X = 640;

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
                addPosition.X = random.Next(0, 640);
            }

            //addPosition = new Vector2(450, 720);

            //TODO:  Make the bullet here
            //each level has different odds
            
                    int nextRand = random.Next(250);
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
            


            //TODO:  set the bullet position here
            tempProjectile.MySprite.Position = addPosition;
            tempProjectile.MySprite.SizeModX = 0.5f;
            tempProjectile.MySprite.SizeModY = 0.5f;
            //TODO:  add it to the list here
            tempProjectile.MySprite.Pivot = new Vector2(tempProjectile.MySprite.Size.X / 4, tempProjectile.MySprite.Size.Y / 4);
            Vector2 AngleVector = tempProjectile.MySprite.Position - new Vector2(320, 360);
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
            }//end if player 1

            else
            {
                
            if (random.Next(100) > 49)
            {
                //in this case, add it from the left or right.  Left or right should be either 0 or screen limit
                if (random.Next(100) > 49)
                    addPosition.X = 640;
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
                addPosition.X = random.Next(641, 1281);
            }

            //addPosition = new Vector2(450, 720);

            //TODO:  Make the bullet here
            //each level has different odds

            int nextRand = random.Next(250);
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



            //TODO:  set the bullet position here
            tempProjectile.MySprite.Position = addPosition;
            tempProjectile.MySprite.SizeModX = 0.5f;
            tempProjectile.MySprite.SizeModY = 0.5f;

            tempProjectile.MySprite.Pivot = new Vector2(tempProjectile.MySprite.Size.X / 4, tempProjectile.MySprite.Size.Y / 4);
            Vector2 AngleVector = tempProjectile.MySprite.Position - new Vector2(960, 360);
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
            Projectiles2.Add(tempProjectile);
            }
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
            
        }//end add bullet

        //add the arrows that use the XABY buttons here
        public void AddArrow(PlayerIndex pIndex)
        {
            Vector2 addPosition = Vector2.Zero;
            Projectile tempProjectile = new Projectile(Projectile.ProjectileType.Arrow);

            if(pIndex == SelectTwoPlayerState.PublicPOne)
            {
                
            int nextRand = random.Next(100);
            if (nextRand < 25)
            {
                addPosition.X = -50;
                addPosition.Y = 340;
            }
            else if (nextRand < 50)
            {
                addPosition.X = 640;
                addPosition.Y = 340;
            }
            else if (nextRand < 75)
            {
                addPosition.X = 300;
                addPosition.Y = -50;
            }
            else
            {
                addPosition.X = 300;
                addPosition.Y = 720;
            }
            tempProjectile.MySprite.Position = addPosition;
            tempProjectile.MySprite.SizeModX = 0.5f;
            tempProjectile.MySprite.SizeModY = 0.5f;
            tempProjectile.MySprite.Pivot = new Vector2(tempProjectile.MySprite.Size.X / 4, tempProjectile.MySprite.Size.Y / 4);
            Vector2 AngleVector = tempProjectile.MySprite.Position - new Vector2(320, 360);
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
            
            else if(pIndex == SelectTwoPlayerState.PublicPTwo)
            {
                int nextRand = random.Next(100);
            if (nextRand < 25)
            {
                addPosition.X = 640;
                addPosition.Y = 340;
            }
            else if (nextRand < 50)
            {
                addPosition.X = 1280;
                addPosition.Y = 340;
            }
            else if (nextRand < 75)
            {
                addPosition.X = 930;
                addPosition.Y = -50;
            }
            else
            {
                addPosition.X = 930;
                addPosition.Y = 720;
            }
            tempProjectile.MySprite.Position = addPosition;
            tempProjectile.MySprite.SizeModY = 0.5f;
            tempProjectile.MySprite.SizeModX = 0.5f;
            tempProjectile.MySprite.Pivot = new Vector2(tempProjectile.MySprite.Size.X / 4, tempProjectile.MySprite.Size.Y / 4);
            Vector2 AngleVector = tempProjectile.MySprite.Position - new Vector2(960, 360);
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
            Projectiles2.Add(tempProjectile);
            
        }
        }


        public void Reset()
        {
            levelComplete = false;
            bIsPaused = false;
            bAPressed = true;
            bAPressed2 = true;
                    numSpellBlasts = 3;
                    numSpellBlasts2 = 3;
                    numDeflectCounter = 0;
                    numDeflectCounter2 = 0;
                    
                    centerRune.Animate(0, 3);
                    int nextRand = random.Next(0, 100);
                    if (nextRand < 33)
                        bgTexture = GameCore.PublicRoadBGTexture;
                    else if (nextRand < 66)
                        bgTexture = GameCore.PublicMountainBGTexture;
                    else
                        bgTexture = GameCore.PublicTownBGTexture;
                
            Projectiles.Clear();
            Projectiles2.Clear();
            myTimer = TimeSpan.Zero;
            myTimer2 = TimeSpan.Zero;
            arrowTimer = TimeSpan.Zero;
            arrowTimer2 = TimeSpan.Zero;
            fireStunTimer = TimeSpan.Zero;
            iceStunTimer = TimeSpan.Zero;
            fireCounterTimer = TimeSpan.Zero;
            iceCounterTimer = TimeSpan.Zero;
            reflectTimer = TimeSpan.Zero;
            fireStunTimer2 = TimeSpan.Zero;
            iceStunTimer2 = TimeSpan.Zero;
            fireCounterTimer2 = TimeSpan.Zero;
            iceCounterTimer2 = TimeSpan.Zero;
            reflectTimer2 = TimeSpan.Zero;
            bStartPressed = true;
            fireStun = false;
            iceStun = false;
            fireStun2 = false;
            iceStun2 = false;
            difficultyMod = 0;
            difficultyMod2 = 0;
            difficultyCounter = 0;
            difficultyCounter2 = 0;

            health = 100;
            health2 = 100;
            reflectPercent = 100;
            reflectPercent2 = 100;

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

            fireRed2 = 255;
            fireGreen2 = 66;
            fireBlue2 = 31;
            fireAlpha2 = 210;

            fireOverRed2 = 255;
            fireOverGreen2 = 228;
            fireOverBlue2 = 0;

            iceRed2 = 100;
            iceGreen2 = 233;
            iceBlue2 = 255;
            iceAlpha2 = 255;

            tempfireRed2 = 255;
            tempfireGreen2 = 66;
            tempfireBlue2 = 31;

            tempiceRed2 = 100;
            tempiceGreen2 = 233;
            tempiceBlue2 = 255;

            tempfireOverRed2 = 255;
            tempfireOverGreen2 = 228;
            tempfireOverBlue2 = 0;

            fireShield.MyColour = new Color(tempfireRed, tempfireGreen, tempfireBlue);
            fireShield.MyAlpha = (float)(fireAlpha) / 255;
            fireShieldOver.MyColour = new Color(tempfireOverRed, tempfireOverGreen, tempfireOverBlue);
            fireShieldOver.MyAlpha = (float)(fireAlpha) / 255;
            iceShield.MyColour = new Color(tempiceRed, tempiceGreen, tempiceBlue);
            iceShield.MyAlpha = (float)(iceAlpha) / 255;
            iceShieldOver.MyColour = new Color(255, 255, 255);
            iceShieldOver.MyAlpha = (float)(iceAlpha) / 255;
            fireShield2.MyColour = new Color(tempfireRed2, tempfireGreen2, tempfireBlue2);
            fireShield2.MyAlpha = (float)(fireAlpha2) / 255;
            fireShieldOver2.MyColour = new Color(tempfireOverRed2, tempfireOverGreen2, tempfireOverBlue2);
            fireShieldOver2.MyAlpha = (float)(fireAlpha2) / 255;
            iceShield2.MyColour = new Color(tempiceRed2, tempiceGreen2, tempiceBlue2);
            iceShield2.MyAlpha = (float)(iceAlpha2) / 255;
            iceShieldOver2.MyColour = new Color(255, 255, 255);
            iceShieldOver2.MyAlpha = (float)(iceAlpha2) / 255;

            fireShield.Rotation = 0;
            iceShield.Rotation = 0;
            fireShieldOver.Rotation = 0;
            iceShieldOver.Rotation = 0;

            fireShield2.Rotation = 0;
            iceShield2.Rotation = 0;
            fireShieldOver2.Rotation = 0;
            iceShieldOver2.Rotation = 0;


            iceAngle = 0;
            fireAngle = 0;
            iceAngle2 = 0;
            fireAngle2 = 0;

            bUpPressed = true;
            bDownPressed = true;
            bLeftPressed = true;
            bRightPressed = true;
            bUpPressed2 = true;
            bDownPressed2 = true;
            bLeftPressed2 = true;
            bRightPressed2 = true;
            patternCounter = 0;
            patternTimer = TimeSpan.Zero;
            patternCounter2 = 0;
            patternTimer2 = TimeSpan.Zero;
            myPattern = ProjectilePattern.NONE;
            myPattern2 = ProjectilePattern.NONE;
        }

        public void HandleReflection(GameTime gameTime, GamePadState myPlayer, PlayerIndex pIndex)
        {
            if (pIndex == SelectTwoPlayerState.PublicPOne)
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
                    if (reflectPercent < MAXREFLECTPERCENT)
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
            else
            {
                if (!bIsReflecting2 && myPlayer.Buttons.RightShoulder == ButtonState.Pressed && reflectPercent2 >= 25)
                {
                    bIsReflecting2 = true;
                }
                if (bIsReflecting2 && myPlayer.Buttons.RightShoulder == ButtonState.Pressed)
                {
                    reflectTimer2 += gameTime.ElapsedGameTime;
                    if (reflectTimer2 > TimeSpan.FromMilliseconds(80))
                    {
                        reflectTimer2 -= TimeSpan.FromMilliseconds(80);
                        reflectPercent2--;
                        if (reflectPercent2 == 0)
                        {
                            reflectTimer2 = TimeSpan.Zero;
                            bIsReflecting2 = false;
                        }
                    }
                }
                else if (!bIsReflecting2 && myPlayer.Buttons.RightShoulder == ButtonState.Released)
                {
                    if (reflectPercent2 < MAXREFLECTPERCENT)
                    {
                        reflectTimer2 += gameTime.ElapsedGameTime;
                        if (reflectTimer2 > TimeSpan.FromMilliseconds(160))
                        {
                            reflectTimer2 -= TimeSpan.FromMilliseconds(160);
                            reflectPercent2++;
                            if (reflectPercent2 == MAXREFLECTPERCENT)
                                reflectTimer2 = TimeSpan.Zero;

                        }
                    }
                }

                if (bIsReflecting2 && myPlayer.Buttons.RightShoulder == ButtonState.Released)
                {
                    bIsReflecting2 = false;
                    reflectTimer = TimeSpan.Zero;
                }
            }

        }

        public override void Draw(SpriteBatch sBatch)
        {
            sBatch.Draw(bgTexture, Vector2.Zero, Color.White);
            iceShield.Draw(sBatch);
            iceShieldOver.Draw(sBatch);
            fireShield.Draw(sBatch);
            fireShieldOver.Draw(sBatch);

            iceShield2.Draw(sBatch);
            iceShieldOver2.Draw(sBatch);
            fireShield2.Draw(sBatch);
            fireShieldOver2.Draw(sBatch);

            
            centerStone.Draw(sBatch);
            centerStone2.Draw(sBatch);

            //centerDot.Draw(sBatch);
            //centerDot2.Draw(sBatch);
                   
            for (byte i = 0; i < Projectiles.Count; i++)
                Projectiles[i].Draw(sBatch);
            for (byte i = 0; i < Projectiles2.Count; i++)
                Projectiles2[i].Draw(sBatch);

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

            if (bAnimateUp2)
                spellBlastUp2.Draw(sBatch);
            if (bAnimateDown2)
                spellBlastDown2.Draw(sBatch);
            if (bAnimateLeft2)
                spellBlastLeft2.Draw(sBatch);
            if (bAnimateRight2)
                spellBlastRight2.Draw(sBatch);
            if (bLightningUp2)
                lightningUp2.Draw(sBatch);
            if (bLightningDown2)
                lightningDown2.Draw(sBatch);
            if (bLightningLeft2)
                lightningLeft2.Draw(sBatch);
            if (bLightningRight2)
                lightningRight2.Draw(sBatch);
             

            /*
            if (Projectiles.Count() > 0)
            {
                sBatch.DrawString(GameCore.Pericles, Angle.ToString(), new Vector2(130, 130), Color.DarkGreen);
                sBatch.DrawString(GameCore.Pericles, lowerLimit.ToString(), new Vector2(130, 230), Color.DarkGreen);
                sBatch.DrawString(GameCore.Pericles, upperLimit.ToString(), new Vector2(130, 330), Color.DarkGreen);
            }
            */
            
            //sBatch.Draw(GameCore.PublicHUDBackground, new Rectangle(975, 75, (int)(GameCore.PublicHUDBackground.Width ), (int)(GameCore.PublicHUDBackground.Height)), Color.White);
            //sBatch.Draw(GameCore.PublicHUDHealth, new Rectangle(1000, 80, (int)(GameCore.PublicHUDHealth.Width ), (int)(GameCore.PublicHUDHealth.Height * 2)), Color.White);
            //sBatch.Draw(GameCore.PublicHUDTime, new Rectangle(1000, 160, (int)(GameCore.PublicHUDTime.Width ), (int)(GameCore.PublicHUDTime.Height * 2)), Color.White);
            //sBatch.Draw(GameCore.PublicHUDScore, new Rectangle(1000, 240, (int)(GameCore.PublicHUDScore.Width ), (int)(GameCore.PublicHUDScore.Height * 2)), Color.White);

            //sBatch.Draw(GameCore.PublicHUDBackground, new Rectangle(475, 75, (int)(GameCore.PublicHUDBackground.Width ), (int)(GameCore.PublicHUDBackground.Height)), Color.White);
            //sBatch.Draw(GameCore.PublicHUDHealth, new Rectangle(500, 80, (int)(GameCore.PublicHUDHealth.Width ), (int)(GameCore.PublicHUDHealth.Height * 2)), Color.White);
            //sBatch.Draw(GameCore.PublicHUDTime, new Rectangle(500, 160, (int)(GameCore.PublicHUDTime.Width ), (int)(GameCore.PublicHUDTime.Height * 2)), Color.White);
            //sBatch.Draw(GameCore.PublicHUDScore, new Rectangle(500, 240, (int)(GameCore.PublicHUDScore.Width ), (int)(GameCore.PublicHUDScore.Height * 2)), Color.White);
               
            //sBatch.DrawString(GameCore.Pericles, score2.ToString(), new Vector2(1015, 600), Color.White);
            //sBatch.DrawString(GameCore.Pericles, score.ToString(), new Vector2(132, 600), Color.White);

            //sBatch.Draw(GameCore.PublicHUDScore, new Rectangle(0, 597, (int)(GameCore.PublicHUDScore.Width), (int)(GameCore.PublicHUDScore.Height)), Color.White);
            //sBatch.Draw(GameCore.PublicHUDScore, new Rectangle((int)(1280 - GameCore.PublicHUDScore.Width), 597, (int)(GameCore.PublicHUDScore.Width), (int)(GameCore.PublicHUDScore.Height)), new Rectangle(0,0,GameCore.PublicHUDScore.Width,GameCore.PublicHUDScore.Height), Color.White,0,Vector2.Zero,SpriteEffects.FlipHorizontally,0);

            if (numSpellBlasts2 > 0)
                sBatch.Draw(SpellBlastIcon, new Vector2(1100, 75), Color.White);
            if (numSpellBlasts2 > 1)
                sBatch.Draw(SpellBlastIcon, new Vector2(1100, 137), Color.White);
            if (numSpellBlasts2 > 2)
                sBatch.Draw(SpellBlastIcon, new Vector2(1100, 200), Color.White);

            if (numSpellBlasts > 0)
                sBatch.Draw(SpellBlastIcon, new Vector2(128, 75), Color.White);
            if (numSpellBlasts > 1)
                sBatch.Draw(SpellBlastIcon, new Vector2(128, 137), Color.White);
            if (numSpellBlasts > 2)
                sBatch.Draw(SpellBlastIcon, new Vector2(128, 200), Color.White);
            //sBatch.DrawString(GameCore.Pericles, "OFFSCREEN", new Vector2(1152, 600), Color.White);

            
            sBatch.Draw(GameCore.PublicReflectMeterBackdrop, new Vector2(570, 131), Color.White);
            sBatch.Draw(healthMeter, new Rectangle(570, (int)(131 + (healthMeter.Height - healthMeter.Height * ((double)health / 100))), healthMeter.Width, (int)(healthMeter.Height * ((double)health / 100))), new Rectangle(0, (int)(healthMeter.Height - healthMeter.Height * ((double)health / 100)), healthMeter.Width, (int)(healthMeter.Height * ((double)health / 100))), Color.White);
            sBatch.Draw(GameCore.PublicReflectMeterOutline, new Vector2(570, 131), Color.White);
           
            sBatch.Draw(GameCore.PublicReflectMeterBackdrop, new Vector2(570, 331), Color.White);
            sBatch.Draw(reflectMeter, new Rectangle(570, (int)(331 + (reflectMeter.Height - reflectMeter.Height * ((double)reflectPercent / 100))), reflectMeter.Width, (int)(reflectMeter.Height * ((double)reflectPercent / 100))), new Rectangle(0, (int)(reflectMeter.Height - reflectMeter.Height * ((double)reflectPercent / 100)), reflectMeter.Width, (int)(reflectMeter.Height * ((double)reflectPercent / 100))), Color.White);
            sBatch.Draw(GameCore.PublicReflectMeterOutline, new Vector2(570, 331), Color.White);

            sBatch.Draw(GameCore.PublicReflectMeterBackdrop, new Vector2(670, 131), Color.White);
            sBatch.Draw(healthMeter, new Rectangle(670, (int)(131 + (healthMeter.Height - healthMeter.Height * ((double)health2 / 100))), healthMeter.Width, (int)(healthMeter.Height * ((double)health2 / 100))), new Rectangle(0, (int)(healthMeter.Height - healthMeter.Height * ((double)health2 / 100)), healthMeter.Width, (int)(healthMeter.Height * ((double)health2 / 100))), Color.White);
            sBatch.Draw(GameCore.PublicReflectMeterOutline, new Vector2(670, 131), Color.White);

            sBatch.Draw(GameCore.PublicReflectMeterBackdrop, new Vector2(670, 331), Color.White);
            sBatch.Draw(reflectMeter, new Rectangle(670, (int)(331 + (reflectMeter.Height - reflectMeter.Height * ((double)reflectPercent2 / 100))), reflectMeter.Width, (int)(reflectMeter.Height * ((double)reflectPercent2 / 100))), new Rectangle(0, (int)(reflectMeter.Height - reflectMeter.Height * ((double)reflectPercent2 / 100)), reflectMeter.Width, (int)(reflectMeter.Height * ((double)reflectPercent2 / 100))), Color.White);
            sBatch.Draw(GameCore.PublicReflectMeterOutline, new Vector2(670, 331), Color.White);
          
            /*
            sBatch.Draw(GameCore.PublicReflectMeterBackdrop, new Rectangle(570, 80, (int)(GameCore.PublicReflectMeterBackdrop.Width / 2), (int)(GameCore.PublicReflectMeterBackdrop.Height)), Color.White);
            sBatch.Draw(reflectMeter, new Rectangle(570, (int)(80 + (reflectMeter.Height - reflectMeter.Height * ((double)health / 100))), (int)(reflectMeter.Width / 2), (int)(reflectMeter.Height * ((double)health / 100))), new Rectangle(0, (int)(reflectMeter.Height - reflectMeter.Height * ((double)health / 100)), reflectMeter.Width, (int)(reflectMeter.Height * ((double)health / 100))), Color.White);
            sBatch.Draw(GameCore.PublicReflectMeterOutline, new Rectangle(570, 80,(int)(GameCore.PublicReflectMeterOutline.Width / 2), (int)(GameCore.PublicReflectMeterOutline.Height)), Color.White);

            sBatch.Draw(GameCore.PublicReflectMeterBackdrop, new Rectangle(663, 80, (int)(GameCore.PublicReflectMeterBackdrop.Width / 2), (int)(GameCore.PublicReflectMeterBackdrop.Height)), Color.White);
            sBatch.Draw(reflectMeter, new Rectangle(663, (int)(80 + (reflectMeter.Height - reflectMeter.Height * ((double)health2 / 100))), (int)(reflectMeter.Width / 2), (int)(reflectMeter.Height * ((double)health2 / 100))), new Rectangle(0, (int)(reflectMeter.Height - reflectMeter.Height * ((double)health2 / 100)), reflectMeter.Width, (int)(reflectMeter.Height * ((double)health2 / 100))), Color.White);
            sBatch.Draw(GameCore.PublicReflectMeterOutline, new Rectangle(663, 80, (int)(GameCore.PublicReflectMeterOutline.Width / 2), (int)(GameCore.PublicReflectMeterOutline.Height)), Color.White);
            */
            sBatch.Draw(GameCore.PublicScreenDivide, new Vector2(625, 0), Color.White);
            //sBatch.Draw(GameCore.Dot, new Vector2(320 - 2.5f, 360 - 2.5f), Color.White);
            //sBatch.Draw(GameCore.Dot, new Vector2(960 - 2.5f, 360 - 2.5f), Color.White);

            base.Draw(sBatch);
        }
    }//end class
}//end namespace
