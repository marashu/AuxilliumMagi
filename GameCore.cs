using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Xml.Serialization;

namespace AuxilliumMagi
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameCore : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //stuff for high scores
        private bool storagePending = false;
        public static readonly string HighScoresFilename = "AuxMagiHighScores.lst";
        public static string HighScoresFullPath = HighScoresFilename;
        private IAsyncResult iaResult;
        private SaveManager sManager;

        private GamerServicesComponent myGamerComponent;

        private ActionState actionState;
        private TwoPlayerState twoPlayerState;
        private GameState currentState;
        private SplashState splashState;
        private TitleState titleState;
        private GameOverState gameOverState;
        private LevelWinState levelWinState;
        private StoryState storyState;
        private CreditState creditState;
        private InstructionState instructionState;
        private OptionState optionState;
        private HighScoreState highScoreState;
        private SelectTwoPlayerState selectTwoPlayerState;

        //musics
        private Song titleSong;
        private Song storySong;
        private Song actionSong;

        //sfx
        private static bool bSoundEffects = true;
        private static SoundEffect fireSound;
        private static SoundEffect iceSound;
        private static SoundEffect stunSound;
        private static SoundEffect counterSound;
        private static SoundEffect boomSound;
        private static SoundEffect lightningSound;
        private static SoundEffect blastSound;

        public static bool PlaySoundEffects { get { return bSoundEffects; } set { bSoundEffects = value; } }
        public static SoundEffect PublicSFXFireSound { get { return fireSound; } }
        public static SoundEffect PublicSFXIceSound { get { return iceSound; } }
        public static SoundEffect PublicSFXStunSound { get { return stunSound; } }
        public static SoundEffect PublicSFXCounterSound { get { return counterSound; } }
        public static SoundEffect PublicSFXBoomSound { get { return boomSound; } }
        public static SoundEffect PublicSFXLightningSound { get { return lightningSound; } }
        public static SoundEffect PublicSFXBlastSound { get { return blastSound; } }

        //manager for the story
        private StoryManager storyManager;
        //public StoryManager PublicStoryManager { get { return storyManager; } }

        //bools for handling buttons
        private bool bBackPressed = true;
        private bool bAPressed = true;
        private bool bStartPressed = true;

        //the font
        private static SpriteFont fontPericles;
        private static SpriteFont storyPericles;
        public static SpriteFont Pericles { get { return fontPericles; } }
        public static SpriteFont StoryPericles { get { return storyPericles; } }

        //textures
        //title and splash screen
        private static Texture2D splashTexture;
        private static Texture2D titleTexture;
        private static Texture2D titleBG;
        private static Texture2D gameOverScreen;
        private static Texture2D levelWinTexture;
        private static Texture2D logoTexture;
        private static Texture2D creditsTexture;
        private static Texture2D instructionTexture;
        private static Texture2D optionsTexture;
        private static Texture2D highScoresTexture;
        private static Texture2D selectTwoPlayerTexture;
        private static Texture2D FASTexture;
        
        //Tutorial boolean and texture
        private static bool bIsTutorial;
        private static Texture2D tutorialTexture;

        public static bool IsTutorial { get { return bIsTutorial; } set { bIsTutorial = value; } }
        public static Texture2D PublicTutorialTexture { get { return tutorialTexture; } }

        //just in here for a moment
        private static Texture2D dot;
        public static Texture2D Dot { get { return dot; } }

        //goes over the center
        private static Texture2D redDot;
        public static Texture2D PublicRedDot { get { return redDot; } }

        public static Texture2D PublicSplashTexture { get { return splashTexture; } }
        public static Texture2D PublicTitleTexture { get { return titleTexture; } }
        public static Texture2D PublicTitleBG { get { return titleBG; } }
        public static Texture2D PublicGameOverTexture { get { return gameOverScreen; } }
        public static Texture2D PublicLevelWinTexture { get { return levelWinTexture; } }
        public static Texture2D PublicLogoTexture { get { return logoTexture; } }
        public static Texture2D PublicCreditsTexture { get { return creditsTexture; } }
        public static Texture2D PublicInstructionTexture { get { return instructionTexture; } }
        public static Texture2D PublicOptionsTexture { get { return optionsTexture; } }
        public static Texture2D PublicHighScoresTexture { get { return highScoresTexture; } }
        public static Texture2D PublicSelectTwoPlayerTexture { get { return selectTwoPlayerTexture; } }
        public static Texture2D PublicFASTexture { get { return FASTexture; } }

        //title buttons
        private static Texture2D storyButtonTexture;
        private static Texture2D twoPlayersButtonTexture;
        private static Texture2D enduranceButtonTexture;
        private static Texture2D optionsButtonTexture;
        private static Texture2D highScoresButtonTexture;
        private static Texture2D instructionsButtonTexture;
        private static Texture2D storyButtonActiveTexture;
        private static Texture2D twoPlayersButtonActiveTexture;
        private static Texture2D enduranceButtonActiveTexture;
        private static Texture2D optionsButtonActiveTexture;
        private static Texture2D highScoresButtonActiveTexture;
        private static Texture2D instructionsButtonActiveTexture;
        private static Texture2D comingSoonTexture;
        private static Texture2D buyButtonTexture;
        private static Texture2D buyButtonActiveTexture;


        public static Texture2D PublicStoryButtonTexture { get { return storyButtonTexture; } }
        public static Texture2D PublicTwoPlayersButtonTexture { get { return twoPlayersButtonTexture; } }
        public static Texture2D PublicEnduranceButtonTexture { get { return enduranceButtonTexture; } }
        public static Texture2D PublicOptionsButtonTexture { get { return optionsButtonTexture; } }
        public static Texture2D PublicHighScoresButtonTexture { get { return highScoresButtonTexture; } }
        public static Texture2D PublicInstructionsButtonTexture { get { return instructionsButtonTexture; } }
        public static Texture2D PublicStoryButtonActiveTexture { get { return storyButtonActiveTexture; } }
        public static Texture2D PublicTwoPlayersButtonActiveTexture { get { return twoPlayersButtonActiveTexture; } }
        public static Texture2D PublicEnduranceButtonActiveTexture { get { return enduranceButtonActiveTexture; } }
        public static Texture2D PublicOptionsButtonActiveTexture { get { return optionsButtonActiveTexture; } }
        public static Texture2D PublicHighScoresButtonActiveTexture { get { return highScoresButtonActiveTexture; } }
        public static Texture2D PublicInstructionsButtonActiveTexture { get { return instructionsButtonActiveTexture; } }
        public static Texture2D PublicComingSoonTexture { get { return comingSoonTexture; } }
        public static Texture2D PublicBuyButtonTexture { get { return buyButtonTexture; } }
        public static Texture2D PublicBuyButtonActiveTexture { get { return buyButtonActiveTexture; } }

        //stuff for the options page
        private static Texture2D musicTexture;
        private static Texture2D sfxTexture;
        private static Texture2D backTexture;
        private static Texture2D sliderTexture;
        private static Texture2D ballTexture;
        private static Texture2D helpTexture;
        private static Texture2D[] onTexture;
        private static Texture2D[] offTexture;

        public static Texture2D PublicMusicTexture { get { return musicTexture; } }
        public static Texture2D PublicSFXTexture { get { return sfxTexture; } }
        public static Texture2D PublicBackTexture { get { return backTexture; } }
        public static Texture2D PublicSliderTexture { get { return sliderTexture; } }
        public static Texture2D PublicBallTexture { get { return ballTexture; } }
        public static Texture2D PublicHelpTexture { get { return helpTexture; } }
        public static Texture2D[] PublicOnTexture { get { return onTexture; } }
        public static Texture2D[] PublicOffTexture { get { return offTexture; } }

        //story textures
        private static Texture2D storyTextBG;
        private static Texture2D storyFireMage;
        private static Texture2D storyIceMage;
        private static Texture2D storyElderMage;
        private static Texture2D storyRoadBG;
        private static Texture2D storyMountainBG;
        private static Texture2D storyTownBG;

        public static Texture2D PublicStoryTextBG { get { return storyTextBG; } }
        public static Texture2D PublicStoryFireMage { get { return storyFireMage; } }
        public static Texture2D PublicStoryIceMage { get { return storyIceMage; } }
        public static Texture2D PublicStoryElderMage { get { return storyElderMage; } }
        public static Texture2D PublicStoryRoadBG { get { return storyRoadBG; } }
        public static Texture2D PublicStoryMountainBG { get { return storyMountainBG; } }
        public static Texture2D PublicStoryTownBG { get { return storyTownBG; } }

        private static Texture2D[] iceShieldTexture;
        private static Texture2D[] fireShieldTexture;
        private static Texture2D[] iceShieldOverTexture;
        private static Texture2D[] fireShieldOverTexture;

        public static Texture2D[] PublicIceShieldTexture { get { return iceShieldTexture; } }
        public static Texture2D[] PublicFireShieldTexture { get { return fireShieldTexture; } }
        public static Texture2D[] PublicIceShieldOverTexture { get { return iceShieldOverTexture; } }
        public static Texture2D[] PublicFireShieldOverTexture { get { return fireShieldOverTexture; } }

        //gameplay bg's
        private static Texture2D roadBGTexture;
        private static Texture2D mountainBGTexture;
        private static Texture2D townBGTexture;

        public static Texture2D PublicRoadBGTexture { get { return roadBGTexture; } }
        public static Texture2D PublicMountainBGTexture { get { return mountainBGTexture; } }
        public static Texture2D PublicTownBGTexture { get { return townBGTexture; } }

        //stuff for the center
        private static Texture2D[] centerRuneTexture;
        private static Texture2D[] centerStoneTexture;
        private static Texture2D centerElder;
        private static Texture2D centerArchitect;
        private static Texture2D centerDruid;

        public static Texture2D[] PublicCenterRuneTexture { get { return centerRuneTexture; } }
        public static Texture2D[] PublicCenterStoneTexture { get { return centerStoneTexture; } }
        public static Texture2D PublicCenterElder { get { return centerElder; } }
        public static Texture2D PublicCenterArchitect { get { return centerArchitect; } }
        public static Texture2D PublicCenterDruid { get { return centerDruid; } }

        private static Texture2D[] fireballTexture;
        private static Texture2D[] iceballTexture;
        private static Texture2D[] fireiceballTexture;
        private static Texture2D[] arrowTexture;
        private static Texture2D[] stunTexture;
        private static Texture2D[] counterTexture;
        private static Texture2D[] explosionTexture;

        public static Texture2D[] PublicFireballTexture { get { return fireballTexture; } }
        public static Texture2D[] PublicIceballTexture { get { return iceballTexture; } }
        public static Texture2D[] PublicFireiceballTexture { get { return fireiceballTexture; } }
        public static Texture2D[] PublicArrowTexture { get { return arrowTexture; } }
        public static Texture2D[] PublicStunTexture { get { return stunTexture; } }
        public static Texture2D[] PublicCounterTexture { get { return counterTexture; } }
        public static Texture2D[] PublicExplosionTexture { get { return explosionTexture; } }

        private static Texture2D[] horizontalSpellBlast;
        private static Texture2D[] verticalSpellBlast;
        private static Texture2D[] horizontalLightning;
        private static Texture2D[] verticalLightning;

        public static Texture2D[] PublicHorizontalSpellBlast { get { return horizontalSpellBlast; } }
        public static Texture2D[] PublicVerticalSpellBlast { get { return verticalSpellBlast; } }
        public static Texture2D[] PublicHorizontalLightning { get { return horizontalLightning; } }
        public static Texture2D[] PublicVerticalLightning { get { return verticalLightning; } }
       

        private static Texture2D spellBlastIcon;

        public static Texture2D PublicSpellBlastIcon { get { return spellBlastIcon; } }

        //boss textures
        //dragon
        private static Texture2D[] dragonBody;
        private static Texture2D dragonNeckS;
        private static Texture2D dragonNeckM;
        private static Texture2D dragonNeckL;
        private static Texture2D[] dragonHead;

        public static Texture2D[] PublicDragonBody { get { return dragonBody; } }
        public static Texture2D PublicDragonNeckS { get { return dragonNeckS; } }
        public static Texture2D PublicDragonNeckM { get { return dragonNeckM; } }
        public static Texture2D PublicDragonNeckL { get { return dragonNeckL; } }
        public static Texture2D[] PublicDragonHead { get { return dragonHead; } }

        //sun priestess
        private static Texture2D[] priestBody;
        private static Texture2D[] teleport;

        public static Texture2D[] PublicPriestBody { get { return priestBody; } }
        public static Texture2D[] PublicTeleport { get { return teleport; } }

        //HUD textures
        private static Texture2D reflectMeterTexture;
        private static Texture2D healthMeterTexture;
        private static Texture2D reflectMeterOutline;
        private static Texture2D reflectMeterBackdrop;
        private static Texture2D HUDBackground;
        private static Texture2D HUDHealth;
        private static Texture2D HUDScore;
        private static Texture2D HUDTime;
        private static Texture2D screenDivide;

        public static Texture2D PublicReflectMeterTexture { get { return reflectMeterTexture; } }
        public static Texture2D PublicHealthMeterTexture { get { return healthMeterTexture; } }
        public static Texture2D PublicReflectMeterOutline { get { return reflectMeterOutline; } }
        public static Texture2D PublicReflectMeterBackdrop { get { return reflectMeterBackdrop; } }
        public static Texture2D PublicHUDBackground { get { return HUDBackground; } }
        public static Texture2D PublicHUDHealth { get { return HUDHealth; } }
        public static Texture2D PublicHUDScore { get { return HUDScore; } }
        public static Texture2D PublicHUDTime { get { return HUDTime; } }
        public static Texture2D PublicScreenDivide { get { return screenDivide; } }

        private static Texture2D OVERLAY;

        public GameCore()
        {
            //Guide.SimulateTrialMode = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            iceShieldTexture = new Texture2D[1];
            fireShieldTexture = new Texture2D[1];
            iceShieldOverTexture = new Texture2D[1];
            fireShieldOverTexture = new Texture2D[1];

            centerRuneTexture = new Texture2D[10];
            centerStoneTexture = new Texture2D[4];

            fireballTexture = new Texture2D[6];
            iceballTexture = new Texture2D[6];
            fireiceballTexture = new Texture2D[6];
            arrowTexture = new Texture2D[1];
            counterTexture = new Texture2D[5];
            stunTexture = new Texture2D[6];
            explosionTexture = new Texture2D[2];

            horizontalSpellBlast = new Texture2D[10];
            verticalSpellBlast = new Texture2D[10];
            horizontalLightning = new Texture2D[10];
            verticalLightning = new Texture2D[9];

            dragonHead = new Texture2D[5];
            dragonBody = new Texture2D[5];

            priestBody = new Texture2D[4];
            teleport = new Texture2D[8];

            onTexture = new Texture2D[2];
            offTexture = new Texture2D[2];

#if(XBOX360)
            myGamerComponent = new GamerServicesComponent(this);
            Components.Add(myGamerComponent);
#endif

            base.Initialize();

            bIsTutorial = true;

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(titleSong);
            //Guide.SimulateTrialMode = true;
        }

        //for the callback
        void StorageCallback(IAsyncResult result)
        {
            // note: I made an error in the first posting of this.  I've corrected it based on the suggestion Louis gave below.
            SaveManager.MyDevice = StorageDevice.EndShowSelector(result);
            // check for user cancelling the selection screen
            if (SaveManager.MyDevice != null && SaveManager.MyDevice.IsConnected == false)
            {
                SaveManager.MyDevice = null;
            }
            if (SaveManager.MyDevice != null)
            {
                string fullpath = HighScoresFilename;//Path.Combine(StorageContainer.TitleLocation, HighScoresFilename);
                HighScoresFullPath = fullpath;
                sManager = new SaveManager(fullpath);

                // Check to see if the save exists
                HighScoreData data = LoadHighScores(HighScoresFilename);
                //added Feb 16 2012
                HighScoreState.GetHighScoreString();
            }

            storagePending = false;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            

            //load the songs
            titleSong = Content.Load<Song>(@"Theme Magi");
            storySong = Content.Load<Song>(@"story theme");
            actionSong = Content.Load<Song>(@"Magi Battle Theme");

            fireSound = Content.Load<SoundEffect>(@"fire sound");
            iceSound = Content.Load<SoundEffect>(@"ice sound");
            stunSound = Content.Load<SoundEffect>(@"stun sound");
            counterSound = Content.Load<SoundEffect>(@"counter sound");
            boomSound = Content.Load<SoundEffect>(@"boom sound");
            lightningSound = Content.Load<SoundEffect>(@"lightning sound");
            blastSound = Content.Load<SoundEffect>(@"blast sound");

            //remove this
            dot = Content.Load<Texture2D>(@"dot");

            redDot = Content.Load<Texture2D>(@"RedDot");

            //load the font
            fontPericles = Content.Load<SpriteFont>(@"Pericles");
            storyPericles = Content.Load<SpriteFont>(@"StoryPericles");

            //load the title screens
            splashTexture = Content.Load<Texture2D>(@"BG");
            titleTexture = Content.Load<Texture2D>(@"BG");
            titleBG = Content.Load<Texture2D>(@"GUI_MainMenu");
            gameOverScreen = Content.Load<Texture2D>(@"GameOver01");
            levelWinTexture = Content.Load<Texture2D>(@"LevelClear01");
            logoTexture = Content.Load<Texture2D>(@"Logo");
            creditsTexture = Content.Load<Texture2D>(@"GUI_Credits");
            instructionTexture = Content.Load<Texture2D>(@"GUI_Instructions");
            optionsTexture = Content.Load<Texture2D>(@"GUI_Options");
            highScoresTexture = Content.Load<Texture2D>(@"GUI_HighScores");
            selectTwoPlayerTexture = Content.Load<Texture2D>(@"GUI_Players");
            FASTexture = Content.Load<Texture2D>(@"FASLogo2");

            storyButtonTexture = Content.Load<Texture2D>(@"BTN_StoryUp");
            twoPlayersButtonTexture = Content.Load<Texture2D>(@"BTN_TwoPlayersUp");
            optionsButtonTexture = Content.Load<Texture2D>(@"BTN_OptionsUp");
            enduranceButtonTexture = Content.Load<Texture2D>(@"BTN_EnduranceUp");
            highScoresButtonTexture = Content.Load<Texture2D>(@"BTN_HighScoresUp");
            instructionsButtonTexture = Content.Load<Texture2D>(@"BTN_InstructionsUp");
            storyButtonActiveTexture = Content.Load<Texture2D>(@"BTN_StoryOver");
            twoPlayersButtonActiveTexture = Content.Load<Texture2D>(@"BTN_TwoPlayersOver");
            optionsButtonActiveTexture = Content.Load<Texture2D>(@"BTN_OptionsOver");
            enduranceButtonActiveTexture = Content.Load<Texture2D>(@"BTN_EnduranceOver");
            highScoresButtonActiveTexture = Content.Load<Texture2D>(@"BTN_HighScoresOver");
            instructionsButtonActiveTexture = Content.Load<Texture2D>(@"BTN_InstructionsOver");
            buyButtonTexture = Content.Load<Texture2D>(@"BTN_BuyUp");
            buyButtonActiveTexture = Content.Load<Texture2D>(@"BTN_BuyOver");
            
            comingSoonTexture = Content.Load<Texture2D>(@"GUI_ComingSoon");

            //load option textures
            musicTexture = Content.Load<Texture2D>(@"BTN_Music");
            sfxTexture = Content.Load<Texture2D>(@"BTN_SFX");
            sliderTexture = Content.Load<Texture2D>(@"BTN_Slider");
            ballTexture = Content.Load<Texture2D>(@"GUI_OptionsSlider");
            backTexture = Content.Load<Texture2D>(@"BTN_BackOver");
            helpTexture = Content.Load<Texture2D>(@"BTN_HelpOver");
            onTexture[0] = Content.Load<Texture2D>(@"BTN_OnUp");
            onTexture[1] = Content.Load<Texture2D>(@"BTN_OnOver");
            offTexture[0] = Content.Load<Texture2D>(@"BTN_OffUp");
            offTexture[1] = Content.Load<Texture2D>(@"BTN_OffOver");


            //load story images
            storyTextBG = Content.Load<Texture2D>(@"TextBox");
            storyElderMage = Content.Load<Texture2D>(@"MageElder");
            storyFireMage = Content.Load<Texture2D>(@"MageFire");
            storyIceMage = Content.Load<Texture2D>(@"MageIce");
            storyMountainBG = Content.Load<Texture2D>(@"BG_Mountains");
            storyRoadBG = Content.Load<Texture2D>(@"BG_DirtRoad");
            storyTownBG = Content.Load<Texture2D>(@"BG_Town");

            tutorialTexture = Content.Load<Texture2D>(@"Box");

            //backgrounds
            roadBGTexture = Content.Load<Texture2D>(@"BGInGame_DirtRoad");
            mountainBGTexture = Content.Load<Texture2D>(@"BGInGame_Mountains");
            townBGTexture = Content.Load<Texture2D>(@"BGInGame_Town");

            iceShieldTexture[0] = Content.Load<Texture2D>(@"IceSheildBlueUnder");
            iceShieldOverTexture[0] = Content.Load<Texture2D>(@"IceSheildWhiteOver");

            fireShieldTexture[0] = Content.Load<Texture2D>(@"FireShieldRedUnder");
            fireShieldOverTexture[0] = Content.Load<Texture2D>(@"FireShieldYellowOver");

            centerRuneTexture[0] = Content.Load<Texture2D>(@"Centre_Rune1");
            centerRuneTexture[1] = Content.Load<Texture2D>(@"Centre_Rune2");
            centerRuneTexture[2] = Content.Load<Texture2D>(@"Centre_Rune3");
            centerRuneTexture[3] = Content.Load<Texture2D>(@"Centre_Rune4");
            centerRuneTexture[4] = Content.Load<Texture2D>(@"Centre_Rune5");
            centerRuneTexture[5] = Content.Load<Texture2D>(@"Centre_Rune6");
            centerRuneTexture[6] = Content.Load<Texture2D>(@"Centre_Rune5");
            centerRuneTexture[7] = Content.Load<Texture2D>(@"Centre_Rune4");
            centerRuneTexture[8] = Content.Load<Texture2D>(@"Centre_Rune3");
            centerRuneTexture[9] = Content.Load<Texture2D>(@"Centre_Rune2");

            centerStoneTexture[0] = Content.Load<Texture2D>(@"Centre_Stone1");
            centerStoneTexture[1] = Content.Load<Texture2D>(@"Centre_Stone2");
            centerStoneTexture[2] = Content.Load<Texture2D>(@"Centre_Stone3");
            centerStoneTexture[3] = Content.Load<Texture2D>(@"Centre_Stone4");

            centerArchitect = Content.Load<Texture2D>(@"Centre_Architect1");
            centerDruid = Content.Load<Texture2D>(@"Centre_Druid1");
            centerElder = Content.Load<Texture2D>(@"Centre_ElderlyMage1");

            fireballTexture[0] = Content.Load<Texture2D>(@"FireBall1");
            fireballTexture[1] = Content.Load<Texture2D>(@"FireBall2");
            fireballTexture[2] = Content.Load<Texture2D>(@"FireBall3");
            fireballTexture[3] = Content.Load<Texture2D>(@"FireBall4");
            fireballTexture[4] = Content.Load<Texture2D>(@"FireBall5");
            fireballTexture[5] = Content.Load<Texture2D>(@"FireBall6");

            iceballTexture[0] = Content.Load<Texture2D>(@"IceBall1");
            iceballTexture[1] = Content.Load<Texture2D>(@"IceBall2");
            iceballTexture[2] = Content.Load<Texture2D>(@"IceBall3");
            iceballTexture[3] = Content.Load<Texture2D>(@"IceBall4");
            iceballTexture[4] = Content.Load<Texture2D>(@"IceBall5");
            iceballTexture[5] = Content.Load<Texture2D>(@"IceBall6");

            fireiceballTexture[0] = Content.Load<Texture2D>(@"FireIceBall1");
            fireiceballTexture[1] = Content.Load<Texture2D>(@"FireIceBall2");
            fireiceballTexture[2] = Content.Load<Texture2D>(@"FireIceBall3");
            fireiceballTexture[3] = Content.Load<Texture2D>(@"FireIceBall4");
            fireiceballTexture[4] = Content.Load<Texture2D>(@"FireIceBall5");
            fireiceballTexture[5] = Content.Load<Texture2D>(@"FireIceBall6");

            arrowTexture[0] = Content.Load<Texture2D>(@"Arrow");

            stunTexture[0] = Content.Load<Texture2D>(@"StunBall1");
            stunTexture[1] = Content.Load<Texture2D>(@"StunBall2");
            stunTexture[2] = Content.Load<Texture2D>(@"StunBall3");
            stunTexture[3] = Content.Load<Texture2D>(@"StunBall4");
            stunTexture[4] = Content.Load<Texture2D>(@"StunBall5");
            stunTexture[5] = Content.Load<Texture2D>(@"StunBall6");

            counterTexture[0] = Content.Load<Texture2D>(@"CounterSpell1");
            counterTexture[1] = Content.Load<Texture2D>(@"CounterSpell2");
            counterTexture[2] = Content.Load<Texture2D>(@"CounterSpell3");
            counterTexture[3] = Content.Load<Texture2D>(@"CounterSpell4");
            counterTexture[4] = Content.Load<Texture2D>(@"CounterSpell5");

            explosionTexture[0] = Content.Load<Texture2D>(@"BossFire01");
            explosionTexture[1] = Content.Load<Texture2D>(@"BossFire01");

            horizontalSpellBlast[0] = Content.Load<Texture2D>(@"SpellBlastHorizontal01");
            horizontalSpellBlast[1] = Content.Load<Texture2D>(@"SpellBlastHorizontal02");
            horizontalSpellBlast[2] = Content.Load<Texture2D>(@"SpellBlastHorizontal03");
            horizontalSpellBlast[3] = Content.Load<Texture2D>(@"SpellBlastHorizontal04");
            horizontalSpellBlast[4] = Content.Load<Texture2D>(@"SpellBlastHorizontal05");
            horizontalSpellBlast[5] = Content.Load<Texture2D>(@"SpellBlastHorizontal06");
            horizontalSpellBlast[6] = Content.Load<Texture2D>(@"SpellBlastHorizontal07");
            horizontalSpellBlast[7] = Content.Load<Texture2D>(@"SpellBlastHorizontal08");
            horizontalSpellBlast[8] = Content.Load<Texture2D>(@"SpellBlastHorizontal09");
            horizontalSpellBlast[9] = Content.Load<Texture2D>(@"SpellBlastHorizontal10");

            verticalSpellBlast[0] = Content.Load<Texture2D>(@"SpellBlastVertical01");
            verticalSpellBlast[1] = Content.Load<Texture2D>(@"SpellBlastVertical02");
            verticalSpellBlast[2] = Content.Load<Texture2D>(@"SpellBlastVertical03");
            verticalSpellBlast[3] = Content.Load<Texture2D>(@"SpellBlastVertical04");
            verticalSpellBlast[4] = Content.Load<Texture2D>(@"SpellBlastVertical05");
            verticalSpellBlast[5] = Content.Load<Texture2D>(@"SpellBlastVertical06");
            verticalSpellBlast[6] = Content.Load<Texture2D>(@"SpellBlastVertical07");
            verticalSpellBlast[7] = Content.Load<Texture2D>(@"SpellBlastVertical08");
            verticalSpellBlast[8] = Content.Load<Texture2D>(@"SpellBlastVertical09");
            verticalSpellBlast[9] = Content.Load<Texture2D>(@"SpellBlastVertical10");

            horizontalLightning[0] = Content.Load<Texture2D>(@"LighteningH01");
            horizontalLightning[1] = Content.Load<Texture2D>(@"LighteningH02");
            horizontalLightning[2] = Content.Load<Texture2D>(@"LighteningH03");
            horizontalLightning[3] = Content.Load<Texture2D>(@"LighteningH04");
            horizontalLightning[4] = Content.Load<Texture2D>(@"LighteningH05");
            horizontalLightning[5] = Content.Load<Texture2D>(@"LighteningH06");
            horizontalLightning[6] = Content.Load<Texture2D>(@"LighteningH07");
            horizontalLightning[7] = Content.Load<Texture2D>(@"LighteningH08");
            horizontalLightning[8] = Content.Load<Texture2D>(@"LighteningH09");
            horizontalLightning[9] = Content.Load<Texture2D>(@"LighteningH10");

            verticalLightning[0] = Content.Load<Texture2D>(@"LighteningV1");
            verticalLightning[1] = Content.Load<Texture2D>(@"LighteningV2");
            verticalLightning[2] = Content.Load<Texture2D>(@"LighteningV3");
            verticalLightning[3] = Content.Load<Texture2D>(@"LighteningV4");
            verticalLightning[4] = Content.Load<Texture2D>(@"LighteningV5");
            verticalLightning[5] = Content.Load<Texture2D>(@"LighteningV6");
            verticalLightning[6] = Content.Load<Texture2D>(@"LighteningV7");
            verticalLightning[7] = Content.Load<Texture2D>(@"LighteningV8");
            verticalLightning[8] = Content.Load<Texture2D>(@"LighteningV9");


            //load boss textures
            //dragon boss
            dragonHead[0] = Content.Load<Texture2D>(@"BossDragon_Head");
            dragonHead[1] = Content.Load<Texture2D>(@"BossDragon_Head2");
            dragonHead[2] = Content.Load<Texture2D>(@"BossDragon_Head3");
            dragonHead[3] = Content.Load<Texture2D>(@"BossDragon_Head4");
            dragonHead[4] = Content.Load<Texture2D>(@"BossDragon_Head4");
            dragonBody[0] = Content.Load<Texture2D>(@"BossDragon_Body1");
            dragonBody[1] = Content.Load<Texture2D>(@"BossDragon_Body2");
            dragonBody[2] = Content.Load<Texture2D>(@"BossDragon_Body3");
            dragonBody[3] = dragonBody[1];
            dragonBody[4] = Content.Load<Texture2D>(@"BossDragon_Body4");
            dragonNeckS = Content.Load<Texture2D>(@"BossDragon_Neck1");
            dragonNeckM = Content.Load<Texture2D>(@"BossDragon_Neck2");
            dragonNeckL = Content.Load<Texture2D>(@"BossDragon_Neck3");

            //sun priestess
            priestBody[0] = Content.Load<Texture2D>(@"BossSun");
            priestBody[1] = Content.Load<Texture2D>(@"BossSun_Attack");
            priestBody[2] = Content.Load<Texture2D>(@"BossSun_Death1");
            priestBody[3] = Content.Load<Texture2D>(@"BossSun_Death2");

            teleport[0] = Content.Load<Texture2D>(@"Teleport01");
            teleport[1] = Content.Load<Texture2D>(@"Teleport02");
            teleport[2] = Content.Load<Texture2D>(@"Teleport03");
            teleport[3] = Content.Load<Texture2D>(@"Teleport04");
            teleport[4] = Content.Load<Texture2D>(@"Teleport05");
            teleport[5] = Content.Load<Texture2D>(@"Teleport06");
            teleport[6] = Content.Load<Texture2D>(@"Teleport07");
            teleport[7] = Content.Load<Texture2D>(@"Teleport08");

            healthMeterTexture = Content.Load<Texture2D>(@"HealthMeterFill2");
            reflectMeterTexture = Content.Load<Texture2D>(@"HealthMeterFill");
            reflectMeterOutline= Content.Load<Texture2D>(@"HealthMeterOutline");
            reflectMeterBackdrop = Content.Load<Texture2D>(@"HealthMeterBG");

            HUDBackground = Content.Load<Texture2D>(@"Text_BG");
            HUDHealth = Content.Load<Texture2D>(@"Text_Health");
            HUDScore = Content.Load<Texture2D>(@"HUD_BlackBar_Score");
            HUDTime = Content.Load<Texture2D>(@"HUD_BlackBar_Time");

            screenDivide = Content.Load<Texture2D>(@"ScreenDivide");

            spellBlastIcon = Content.Load<Texture2D>(@"SpellBlast");

            OVERLAY = Content.Load<Texture2D>(@"Overlay");
            
            MediaPlayer.Volume = 0.5f;
            storyManager = new StoryManager();

            splashState = new SplashState(this);
            titleState = new TitleState(this);
            actionState = new ActionState(this);
            gameOverState = new GameOverState(this);
            levelWinState = new LevelWinState(this);
            storyState = new StoryState(this);
            creditState = new CreditState(this);
            twoPlayerState = new TwoPlayerState(this);
            instructionState = new InstructionState(this);
            optionState = new OptionState(this);
            highScoreState = new HighScoreState(this);
            selectTwoPlayerState = new SelectTwoPlayerState(this);
           
            //twoPlayerState.Reset();
            //currentState = twoPlayerState;
            currentState = splashState;
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Two).Buttons.Back == ButtonState.Pressed ||
                    GamePad.GetState(PlayerIndex.Three).Buttons.Back == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Four).Buttons.Back == ButtonState.Pressed) && !bBackPressed)
            {
                if (currentState == splashState)
                {
                    MediaPlayer.Stop();
                    this.Exit();
                }
                else if(currentState != twoPlayerState)
                {
                    if (currentState != titleState)
                        MediaPlayer.Play(titleSong);
                    currentState = splashState;
                    bBackPressed = true;
                    GamePad.SetVibration(PlayerIndex.One, 0, 0);

                }
            }
#if(XBOX360)
            if (!Guide.IsVisible && !storagePending && (SaveManager.MyDevice == null || !SaveManager.MyDevice.IsConnected))
            {
                storagePending = true;
                // XNA 4.0
                // retrieve the storage device
                if (!Guide.IsVisible)
                {

                    try
                    {
                        storagePending = true;
                        StorageDevice.BeginShowSelector(StorageCallback, null);
                    }
                    catch
                    {
                        storagePending = false;
                        SaveManager.MyDevice = null;
                    }
                    //StorageDevice.BeginShowSelector(StorageCallback, null);
                }
                //Guide.BeginShowStorageDeviceSelector(StorageCallback, null);
            }
#endif

            if(currentState == splashState)
            {

                if (((GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed ||
                    GamePad.GetState(PlayerIndex.Three).Buttons.A == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Four).Buttons.A == ButtonState.Pressed) && !bAPressed) ||
                    ((GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Two).Buttons.Start == ButtonState.Pressed ||
                    GamePad.GetState(PlayerIndex.Three).Buttons.Start == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Four).Buttons.Start == ButtonState.Pressed) && !bStartPressed))
                {
                    if (currentState == gameOverState || currentState == gameOverState)
                    {
                        MediaPlayer.Play(titleSong);
                    }
                    titleState.Reset();
                    currentState = titleState;
                    bAPressed = true;
                   
                }
                else if ((GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Two).Buttons.B == ButtonState.Pressed ||
                    GamePad.GetState(PlayerIndex.Three).Buttons.B == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Four).Buttons.B == ButtonState.Pressed))
                {
                    if (currentState == gameOverState || currentState == gameOverState)
                    {
                        MediaPlayer.Play(titleSong);
                    }
                    titleState.Reset();
                    currentState = titleState;
                }
            }
            else if (currentState == gameOverState || currentState == creditState || currentState == instructionState || currentState == highScoreState)
            {

                if (((GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed ||
                    GamePad.GetState(PlayerIndex.Three).Buttons.A == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Four).Buttons.A == ButtonState.Pressed) && !bAPressed) ||
                    ((GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Two).Buttons.Start == ButtonState.Pressed ||
                    GamePad.GetState(PlayerIndex.Three).Buttons.Start == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Four).Buttons.Start == ButtonState.Pressed) && !bStartPressed))
                {
                    if (currentState == gameOverState || currentState == gameOverState)
                    {
                        MediaPlayer.Play(titleSong);
                    }
                    titleState.Reset();
                    currentState = titleState;
                    bAPressed = true;

                }
                else if ((GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Two).Buttons.B == ButtonState.Pressed ||
                    GamePad.GetState(PlayerIndex.Three).Buttons.B == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Four).Buttons.B == ButtonState.Pressed))
                {
                    if (currentState == gameOverState || currentState == gameOverState)
                    {
                        MediaPlayer.Play(titleSong);
                    }
                    titleState.Reset();
                    currentState = titleState;
                }
            }
            else if (currentState == optionState)
            {
                if ((GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed ||
                    GamePad.GetState(PlayerIndex.Three).Buttons.A == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Four).Buttons.A == ButtonState.Pressed) && !bAPressed && OptionState.CurrentSelection == OptionState.Selection.Back)
                {
                    
                    titleState.Reset();
                    currentState = titleState;
                    bAPressed = true;

                }
                else if ((GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Two).Buttons.B == ButtonState.Pressed ||
                    GamePad.GetState(PlayerIndex.Three).Buttons.B == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Four).Buttons.B == ButtonState.Pressed))
                {
                    
                    titleState.Reset();
                    currentState = titleState;
                }
            }
            else if (currentState == titleState)
            {
                if ((GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed ||
                    GamePad.GetState(PlayerIndex.Three).Buttons.A == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Four).Buttons.A == ButtonState.Pressed) && !bAPressed && !Guide.IsVisible)
                {
                    bAPressed = true;
                    PlayerIndex tempIndex;
                    if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                        tempIndex = PlayerIndex.One;
                    else if (GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed)
                        tempIndex = PlayerIndex.Two;
                    else if (GamePad.GetState(PlayerIndex.Three).Buttons.A == ButtonState.Pressed)
                        tempIndex = PlayerIndex.Three;
                    else
                        tempIndex = PlayerIndex.Four;

                    if (TitleState.CurrentSelection == TitleState.Selections.Start)
                    {
                        //ActionState.Level = 0;
                        if (ActionState.Level < 0)
                            ActionState.Level = 0;
                        if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                            actionState.SetPlayerIndex(PlayerIndex.One);
                        else if (GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed)
                            actionState.SetPlayerIndex(PlayerIndex.Two);
                        else if (GamePad.GetState(PlayerIndex.Three).Buttons.A == ButtonState.Pressed)
                            actionState.SetPlayerIndex(PlayerIndex.Three);
                        else if (GamePad.GetState(PlayerIndex.Four).Buttons.A == ButtonState.Pressed)
                            actionState.SetPlayerIndex(PlayerIndex.Four);
                        StoryManager.GenerateStory();
                        StoryManager.ClearPreviousStoryElements();
                        actionState.Reset();
                        storyState.BoolAPressed = true;
                        storyState.BoolStartPressed = true;
                        currentState = storyState;
                        MediaPlayer.Play(storySong);
                        GameOverState.TwoPlayers = false;
                    }
                    else if (TitleState.CurrentSelection == TitleState.Selections.Endurance)
                    {
                        ActionState.Level = -1;
                        if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                            actionState.SetPlayerIndex(PlayerIndex.One);
                        else if (GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed)
                            actionState.SetPlayerIndex(PlayerIndex.Two);
                        else if (GamePad.GetState(PlayerIndex.Three).Buttons.A == ButtonState.Pressed)
                            actionState.SetPlayerIndex(PlayerIndex.Three);
                        else if (GamePad.GetState(PlayerIndex.Four).Buttons.A == ButtonState.Pressed)
                            actionState.SetPlayerIndex(PlayerIndex.Four);
                        actionState.Reset();
                        currentState = actionState;
                        MediaPlayer.Play(actionSong);
                        GameOverState.TwoPlayers = false;
                        
                    }
                    else if (TitleState.CurrentSelection == TitleState.Selections.TwoPlayers)
                    {
                        if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                            selectTwoPlayerState.SetPlayerOne(PlayerIndex.One);
                        else if (GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed)
                            selectTwoPlayerState.SetPlayerOne(PlayerIndex.Two);
                        else if (GamePad.GetState(PlayerIndex.Three).Buttons.A == ButtonState.Pressed)
                            selectTwoPlayerState.SetPlayerOne(PlayerIndex.Three);
                        else if (GamePad.GetState(PlayerIndex.Four).Buttons.A == ButtonState.Pressed)
                            selectTwoPlayerState.SetPlayerOne(PlayerIndex.Four);
                        selectTwoPlayerState.SetPlayerTwo(SelectTwoPlayerState.PublicPOne);
                        TwoPlayerState.Level = -1;
                        twoPlayerState.Reset();
                        currentState = selectTwoPlayerState;
                    }
                    else if (TitleState.CurrentSelection == TitleState.Selections.Instructions)
                    {
                        currentState = instructionState;
                    }
                    else if (TitleState.CurrentSelection == TitleState.Selections.Options)
                    {
                        optionState.Reset();
                        currentState = optionState;
                    }
                    else if (TitleState.CurrentSelection == TitleState.Selections.Scores)
                    {
                        
                        currentState = highScoreState;
                    }
                    else if (TitleState.CurrentSelection == TitleState.Selections.Buy)
                    {
                        if(CanPurchase(tempIndex))
                               Guide.ShowMarketplace(tempIndex);
                    }
                }
            }
            else if (currentState == storyState)
            {
                if (storyState.GoToNextLevel)
                {
                    while (StoryManager.Story[0].Level == ActionState.Level)
                    {
                        StoryManager.Story.Remove(StoryManager.Story[0]);
                        if (StoryManager.Story.Count == 0)
                            break;
                    }
                    if (ActionState.Level < 11)
                    {
                        currentState = actionState;
                        MediaPlayer.Play(actionSong);
                    }
                    else
                    {
                        bAPressed = true;
                        currentState = creditState;
                        ActionState.Level = 0;

                    }
                    storyState.GoToNextLevel = false;
                }
            }
            else if (currentState == actionState)
            {
                if (GamePad.GetState(ActionState.PublicPOne).Buttons.Start == ButtonState.Pressed)
                    bStartPressed = true;
                if (actionState.Health <= 0)
                {
                    if (ActionState.Level < 0)
                        gameOverState.CheckScore();
                    currentState = gameOverState;

                    GamePad.SetVibration(ActionState.PublicPOne, 0, 0);
                }
                else if (actionState.LevelComplete)
                {
                    ActionState.Level++;
                    currentState = levelWinState;
                    GamePad.SetVibration(ActionState.PublicPOne, 0, 0);
                }
            }
            else if (currentState == selectTwoPlayerState)
            {
                if (GamePad.GetState(SelectTwoPlayerState.PublicPOne).Buttons.B == ButtonState.Pressed)
                {
                    titleState.Reset();
                    currentState = titleState;
                }
                else if (SelectTwoPlayerState.PublicPOne != SelectTwoPlayerState.PublicPTwo)
                {
                    currentState = twoPlayerState;
                    MediaPlayer.Play(actionSong);
                    GameOverState.TwoPlayers = true;
                }
            }
            else if (currentState == twoPlayerState)
            {

                if ((twoPlayerState.Health <= 0 && twoPlayerState.Health2 <= 0) || (GamePad.GetState(SelectTwoPlayerState.PublicPOne).Buttons.Back == ButtonState.Pressed && GamePad.GetState(SelectTwoPlayerState.PublicPTwo).Buttons.Back == ButtonState.Pressed))
                {
                    GameOverState.PlayerWon = "It's a draw!";
                    currentState = gameOverState;
                    bBackPressed = true;
                    GamePad.SetVibration(SelectTwoPlayerState.PublicPOne, 0, 0);
                    GamePad.SetVibration(SelectTwoPlayerState.PublicPTwo, 0, 0);
                }
                else if (twoPlayerState.Health <= 0 || GamePad.GetState(SelectTwoPlayerState.PublicPOne).Buttons.Back == ButtonState.Pressed)
                {
                    GameOverState.PlayerWon = "Player " + SelectTwoPlayerState.PublicPTwo.ToString() + " Wins!";
                    currentState = gameOverState;
                    bBackPressed = true;
                    GamePad.SetVibration(SelectTwoPlayerState.PublicPOne, 0, 0);
                    GamePad.SetVibration(SelectTwoPlayerState.PublicPTwo, 0, 0);
                }
                else if (twoPlayerState.Health2 <= 0 || GamePad.GetState(SelectTwoPlayerState.PublicPTwo).Buttons.Back == ButtonState.Pressed)
                {
                    GameOverState.PlayerWon = "Player " + SelectTwoPlayerState.PublicPOne.ToString() + " Wins!";
                    currentState = gameOverState;
                    bBackPressed = true;
                    GamePad.SetVibration(SelectTwoPlayerState.PublicPOne, 0, 0);
                    GamePad.SetVibration(SelectTwoPlayerState.PublicPTwo, 0, 0);
                }

            }
            else if (currentState == levelWinState)
            {
                if ((GamePad.GetState(ActionState.PublicPOne).Buttons.A == ButtonState.Pressed && !bAPressed) || (GamePad.GetState(ActionState.PublicPOne).Buttons.Start == ButtonState.Pressed && !bStartPressed))
                {
                    if (GamePad.GetState(ActionState.PublicPOne).Buttons.A == ButtonState.Pressed)
                        bAPressed = true;
                    if (GamePad.GetState(ActionState.PublicPOne).Buttons.Start == ButtonState.Pressed)
                        bStartPressed = true;
#if(XBOX360)
                    if (Guide.IsTrialMode && ActionState.Level == 4)
                    {

                        currentState = titleState;
                        ActionState.Level = 0;
                        MediaPlayer.Play(titleSong);
                    }
                    else
                    {
#endif
                        if (StoryManager.Story.Count == 0)
                        {
                            currentState = titleState;
                            MediaPlayer.Play(titleSong);
                        }
                        else
                        {
                            actionState.Reset();
                            storyState.BoolAPressed = true;
                            storyState.BoolStartPressed = true;
                            currentState = storyState;
                            MediaPlayer.Play(storySong);
                        }
#if(XBOX360)
                    }
#endif
                }
            }
            currentState.Update(gameTime);

            CheckAReleased();
            CheckBackReleased();
            CheckStartReleased();

            base.Update(gameTime);
        }

        protected void CheckAReleased()
        {
            for (PlayerIndex p = PlayerIndex.One; p <= PlayerIndex.Four; p++)
            {
                if (GamePad.GetState(p).Buttons.A == ButtonState.Pressed)
                    return;
            }
            bAPressed = false;
        }

        protected void CheckBackReleased()
        {
            for (PlayerIndex p = PlayerIndex.One; p <= PlayerIndex.Four; p++)
            {
                if (GamePad.GetState(p).Buttons.Back == ButtonState.Pressed)
                    return;
            }
            bBackPressed = false;
        }

        protected void CheckStartReleased()
        {
            for (PlayerIndex p = PlayerIndex.One; p <= PlayerIndex.Four; p++)
            {
                if (GamePad.GetState(p).Buttons.Start == ButtonState.Pressed)
                    return;
            }
            bStartPressed = false;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            currentState.Draw(spriteBatch);
            //spriteBatch.Draw(OVERLAY, new Rectangle(0, 0, (int)(OVERLAY.Width), (int)(OVERLAY.Height)), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public static void SaveHighScores(HighScoreData data, string filename)
        {
            /*OLD WAY OF DOING THINGS
            // Get the path of the save game
            string fullpath = Path.Combine(StorageContainer.TitleLocation, filename);

            // Open the file, creating it if necessary
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate);
            try
            {
                // Convert the object to XML data and put it in the stream
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                serializer.Serialize(stream, data);
            }
            finally
            {
                // Close the file
                stream.Close();
            }
             * */
            //NEW WAY OF DOING THINGS
#if(XBOX360)
            Stream stream;
            StorageDevice myDevice = SaveManager.MyDevice;
            //myDevice.DeleteContainer(filename);
            using (StorageContainer container = OpenContainer(myDevice, filename))
            {
                string fullpath = filename;//Path.Combine(container.Path, filename);
                //Open the file, creating it if necessary
                HighScoresFullPath = fullpath;
                //Console.Write(fullpath);
                if (container.FileExists(filename))
                container.DeleteFile(filename);
                stream = container.CreateFile(fullpath);

                try
                {

                    //generate xml data


                    // Convert the object to XML data and put it in the stream
                    XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                    serializer.Serialize(stream, data);
                }
                finally
                {
                    // Close the file
                    stream.Close();
                    //container.Dispose();
                }

            }
#else
            // Get the path of the save game
            string fullpath = Path.Combine(StorageContainer.TitleLocation, filename);

            // Open the file, creating it if necessary
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate);
            try
            {
                // Convert the object to XML data and put it in the stream
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                serializer.Serialize(stream, data);
            }
            finally
            {
                // Close the file
                stream.Close();
            }
#endif
        }//end SaveHighScores

        public static HighScoreData LoadHighScores(string filename)
        {
#if(XBOX360)
            HighScoreData data = new HighScoreData();
            Stream stream;

            // Get the path of the save game
            //string fullpath = Path.Combine(StorageContainer.TitleLocation, filename);

            StorageDevice myDevice = SaveManager.MyDevice;
            //myDevice.DeleteContainer(filename);
            using (StorageContainer container = OpenContainer(myDevice, filename))
            {
                string fullpath = filename;//Path.Combine(container.Path, filename);
                HighScoresFullPath = fullpath;
                // Open the file
                
                stream = container.OpenFile(fullpath, FileMode.OpenOrCreate);

                try
                {


                    // Read the data from the file
                    XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                    try
                    {
                        data = (HighScoreData)serializer.Deserialize(stream);
                    }
                    catch (Exception ex)
                    {
                        System.Console.Write(ex);
                        data = new HighScoreData(10);
                        data.Times[0] = System.DateTime.Now;
                        data.Score[0] = 5000;

                        data.Times[1] = System.DateTime.Now;
                        data.Score[1] = 4500;

                        data.Times[2] = System.DateTime.Now;
                        data.Score[2] = 4000;

                        data.Times[3] = System.DateTime.Now;
                        data.Score[3] = 3500;

                        data.Times[4] = System.DateTime.Now;
                        data.Score[4] = 3000;

                        data.Times[5] = System.DateTime.Now;
                        data.Score[5] = 2500;

                        data.Times[6] = System.DateTime.Now;
                        data.Score[6] = 2000;

                        data.Times[7] = System.DateTime.Now;
                        data.Score[7] = 1500;

                        data.Times[8] = System.DateTime.Now;
                        data.Score[8] = 1000;

                        data.Times[9] = System.DateTime.Now;
                        data.Score[9] = 500;
                    }
                    if (data.Count == 0)
                    {
                        data = new HighScoreData(10);
                        data.Times[0] = System.DateTime.Now;
                        data.Score[0] = 5000;

                        data.Times[1] = System.DateTime.Now;
                        data.Score[1] = 4500;

                        data.Times[2] = System.DateTime.Now;
                        data.Score[2] = 4000;

                        data.Times[3] = System.DateTime.Now;
                        data.Score[3] = 3500;

                        data.Times[4] = System.DateTime.Now;
                        data.Score[4] = 3000;

                        data.Times[5] = System.DateTime.Now;
                        data.Score[5] = 2500;

                        data.Times[6] = System.DateTime.Now;
                        data.Score[6] = 2000;

                        data.Times[7] = System.DateTime.Now;
                        data.Score[7] = 1500;

                        data.Times[8] = System.DateTime.Now;
                        data.Score[8] = 1000;

                        data.Times[9] = System.DateTime.Now;
                        data.Score[9] = 500;

                        //if(SaveManager.MyDevice.IsConnected)
                        if (SaveManager.MyDevice.IsConnected)
                            SaveHighScores(data, HighScoresFilename);
                    }
                }
                catch (Exception ex)
                {
                    System.Console.Write(ex);
                    if (data.Count == 0)
                    {
                        data = new HighScoreData(10);
                        data.Times[0] = System.DateTime.Now;
                        data.Score[0] = 5000;

                        data.Times[1] = System.DateTime.Now;
                        data.Score[1] = 4500;

                        data.Times[2] = System.DateTime.Now;
                        data.Score[2] = 4000;

                        data.Times[3] = System.DateTime.Now;
                        data.Score[3] = 3500;

                        data.Times[4] = System.DateTime.Now;
                        data.Score[4] = 3000;

                        data.Times[5] = System.DateTime.Now;
                        data.Score[5] = 2500;

                        data.Times[6] = System.DateTime.Now;
                        data.Score[6] = 2000;

                        data.Times[7] = System.DateTime.Now;
                        data.Score[7] = 1500;

                        data.Times[8] = System.DateTime.Now;
                        data.Score[8] = 1000;

                        data.Times[9] = System.DateTime.Now;
                        data.Score[9] = 500;
                        stream.Close();
                        container.Dispose();
                        //if(SaveManager.MyDevice.IsConnected)
                        if (SaveManager.MyDevice.IsConnected)
                            SaveHighScores(data, HighScoresFilename);
                    }
                }
                finally
                {
                    // Close the file
                    stream.Close();
                    //container.Dispose();

                }

            }
            //if (data.Equals(null))
            //data = new HighScoreData();
            return (data);
            //return new HighScoreData();

#else
        HighScoreData data;

            // Get the path of the save game
            string fullpath = Path.Combine(StorageContainer.TitleLocation, filename);

            // Open the file
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate,
            FileAccess.Read);
            try
            {

                // Read the data from the file
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                data = (HighScoreData)serializer.Deserialize(stream);
                if (data.Count == 0)
                {
                    data = new HighScoreData(5);
                    data.Times[0] = System.DateTime.Now;
                    data.Score[0] = 40000;

                    data.Times[1] = System.DateTime.Now;
                    data.Score[1] = 20000;

                    data.Times[2] = System.DateTime.Now;
                    data.Score[2] = 11000;

                    data.Times[3] = System.DateTime.Now;
                    data.Score[3] = 5100;

                    data.Times[4] = System.DateTime.Now;
                    data.Score[4] = 1000;

                    SaveHighScores(data, HighScoresFilename);
                }
            }
            finally
            {
                // Close the file
                stream.Close();
            }

            return (data);
#endif
        }

        /// <summary>
        /// Synchronously opens storage container
        /// </summary>
        private static StorageContainer OpenContainer(StorageDevice storageDevice, string saveGameName)
        {
            IAsyncResult result = storageDevice.BeginOpenContainer(saveGameName, null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = storageDevice.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            return container;
        }
        public bool CanPurchase(PlayerIndex CurrPlayer)
        {
            foreach (SignedInGamer SG in Gamer.SignedInGamers)
            {
                if ((SG.PlayerIndex == CurrPlayer) && (SG.Privileges.AllowPurchaseContent))
                    return true;
            }
            return false;
        } 
    }
}
