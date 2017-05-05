using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Threading;

namespace Fade
{
    enum GameState
    {
        Menu,
        Controls,
        Game,
        GamePause,
        GameOver
    }


    public class Game1 : Game
    //Trinity Parcero, Shawn Clark, Grant Terdoslavich, Ian Davis
    //FADE main game class
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //TEXT
        SpriteFont titleFont;
        SpriteFont textFont;

        int hiScore = 0;
        int currentScore;

        //SELECTABLES
        SelectText mStart;
        SelectText mControls;
        SelectText mQuit;
        SelectText cReturn;
        SelectText pContinue;
        SelectText pMenu;
        SelectText gRetry;
        SelectText gMenu;

        //SOUND
        Song backgroundMusic;
        SoundEffect swordSwing;
        SoundEffect gruntDie;
        SoundEffect tankDie;
        SoundEffect roar;
        SoundEffect playerDamage;
        SoundEffect playerDeath;

        //IMAGES
        Texture2D mainMenuImage;
        Texture2D pauseImage;
        Texture2D controlsImage;
        Texture2D gameOverImage;
        Texture2D uIBar;
        Texture2D fogSprite;
        Texture2D bg;
        Texture2D floor;
        Texture2D spriteSheet;
        Texture2D gruntSheet;
        Texture2D tankSheet;
        Texture2D tankRoar;
        Texture2D sword;
        Texture2D swordSprite;
        Texture2D heart;
        Texture2D jumpAttack;
        Texture2D deathFog;

        //OBJECTS
        Player p1;
        Fog fog;
        Camera2D camera;
        ExternalTool tool = new ExternalTool();
        Enemy enemy;
        Tank testTank;
        EnemySpawner spawner = new EnemySpawner();
        FadeData data;

        //ENUMS
        GameState currentState = GameState.Menu;

        //KB & MOUSE
        KeyboardState ks;
        KeyboardState previousState = Keyboard.GetState();
        MouseState oldState = Mouse.GetState();

        //ANIMATION
        int frame;
        public int swordFrame { get; set; }//has to be public so we can access it for player attack method
        int gruntFrame;
        int tankFrame;
        double timeCounter;
        double fps;
        int count;
        double timePerFrame;
        double longTimePerFrame; //longer time per frame for slow animations    
        Vector2 playerLoc;
        public Vector2 swordPos;

        // player rectangle
        const int WALK_FRAME_COUNT = 6;         // The number of frames in the animation
        const int PLAYER_RECT_Y_OFFSET = 0;    // How far down in the image are the frames?
        const int PLAYER_RECT_HEIGHT = 142;       // The height of a single frame
        const int PLAYER_RECT_WIDTH = 137;        // The width of a single frame

        // grunt rectangle
        const int GRUNT_FRAME_COUNT = 6;         // The number of frames in the animation
        const int GRUNT_RECT_Y_OFFSET = 0;    // How far down in the image are the frames?
        const int GRUNT_RECT_HEIGHT = 102;       // The height of a single frame
        const int GRUNT_RECT_WIDTH = 150;        // The width of a single frame

        // tank rectangle
        const int TANK_FRAME_COUNT = 3;         // The number of frames in the animation
        const int TANK_RECT_Y_OFFSET = 0;    // How far down in the image are the frames?
        const int TANK_RECT_HEIGHT = 120;       // The height of a single frame
        const int TANK_RECT_WIDTH = 275;        // The width of a single frame

        //sword rectangle
        const int SWORD_FRAME_COUNT = 3;         // The number of frames in the animation
        const int SWORD_RECT_Y_OFFSET = 0;    // How far down in the image are the frames?
        const int SWORD_RECT_HEIGHT = 180;       // The height of a single frame
        const int SWORD_RECT_WIDTH = 180;        // The width of a single frame

        //DISTANCE
        int startPoint;
        int farPoint;
        //current distance traveled is the currentScore variable


        // CONSTRUCTOR ///////////////////////////////////
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        //INITIALIZE /////////////////////////////////////
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            camera = new Camera2D(GraphicsDevice.Viewport);
            data = new FadeData();
            data.newHighScore(0);
            fps = 8.0;
            swordFrame = 1;
            timePerFrame = 1.0 / fps;
            startPoint = 200;
            farPoint = 200;
            count = 0;
            tool.writeFile();
            base.Initialize();
        }

        //LOAD ///////////////////////////////////////////
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //textures
            //playerSprite = Content.Load<Texture2D>("char1sword");
            fogSprite = Content.Load<Texture2D>("fogfull");
            bg = Content.Load<Texture2D>("bg2");
            mainMenuImage = Content.Load<Texture2D>("menus/menu2");
            pauseImage = Content.Load<Texture2D>("menus/pausebg");
            controlsImage = Content.Load<Texture2D>("menus/controls");
            gameOverImage = Content.Load<Texture2D>("menus/gameoverbg");
            uIBar = Content.Load<Texture2D>("menus/uiBar");
            spriteSheet = Content.Load<Texture2D>("characters/charsprite");
            gruntSheet = Content.Load<Texture2D>("characters/grunt2");
            tankSheet = Content.Load<Texture2D>("characters/tank");
            tankRoar = Content.Load<Texture2D>("characters/tankRoar");
            swordSprite = Content.Load<Texture2D>("characters/swordSprite");
            sword = Content.Load<Texture2D>("characters/sword");
            floor = Content.Load<Texture2D>("floor");
            heart = Content.Load<Texture2D>("menus/hearts");
            jumpAttack = Content.Load<Texture2D>("characters/jumpAttack");
            //deathFog = Content.Load<Texture2D>("fogpuff");

            //type
            textFont = Content.Load<SpriteFont>("textFont");
            titleFont = Content.Load<SpriteFont>("titleFont");

            //SOUND
            backgroundMusic = Content.Load<Song>("audio/Amelita");
            swordSwing = Content.Load<SoundEffect>("audio/swordSwing");
            gruntDie = Content.Load<SoundEffect>("audio/gruntDie");
            roar = Content.Load<SoundEffect>("audio/tankRoar");
            tankDie = Content.Load<SoundEffect>("audio/tankDie");
            playerDamage = Content.Load<SoundEffect>("audio/playerDamaged");
            playerDeath = Content.Load<SoundEffect>("audio/playerDie");

            //objects
            p1 = new Player(spriteSheet, new Rectangle(200, 330, 120, 140), playerDamage, playerDeath);
            mStart = new SelectText(true, Color.White, Color.Black);
            mQuit = new SelectText();
            mControls = new SelectText();
            cReturn = new SelectText(true, Color.White, Color.Magenta);
            pContinue = new SelectText(true, Color.Black, Color.Magenta);
            pMenu = new SelectText(false, Color.Black, Color.Magenta);
            gRetry = new SelectText(true, Color.White, Color.Magenta);
            gMenu = new SelectText(false, Color.White, Color.Magenta);
            fog = new Fog(fogSprite, new Rectangle(-800, 0, 1000, 500), new Rectangle(-600, 0, 350, 700), 2, 0);
            enemy = new Grunt(gruntSheet, new Rectangle(0, 380, 0, 0), new Rectangle(0, 372, 50, 50), 1, 3, 0.5, gruntDie);
            testTank = new Tank(tankSheet, new Rectangle(0, 360, 0, 0), new Rectangle(0, 372, 50, 50), 1, 3, 1, tankDie);

            MediaPlayer.Volume = 0.08f;
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        }

        private void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(backgroundMusic);
            }
        }

        //UNLOAD /////////////////////////////////////////
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        bool startSpawn = false;

        // UPDATE ////////////////////////////////////////
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            ks = Keyboard.GetState();

            //GAMESTATE CHANGES
            if (currentState == GameState.Menu && mStart.IsSelected && SingleKeyPress(Keys.Enter))
            {
                currentState = GameState.Game;
            }
            else if (currentState == GameState.Menu && mControls.IsSelected && SingleKeyPress(Keys.Enter))
            {
                currentState = GameState.Controls;
            }
            else if (currentState == GameState.Menu && mQuit.IsSelected && SingleKeyPress(Keys.Enter))
            {
                Exit();
            }
            else if (currentState == GameState.Controls && SingleKeyPress(Keys.Enter))
            {
                currentState = GameState.Menu;
            }
            else if (currentState == GameState.Game && SingleKeyPress(Keys.P))
            {
                currentState = GameState.GamePause;
            }
            else if (currentState == GameState.Game && (p1.isDead))  //B key is temp for testing til we have damage going
            {
                playerDeath.Play();
                currentState = GameState.GameOver;
            }
            else if (currentState == GameState.GamePause && pContinue.IsSelected && SingleKeyPress(Keys.Enter))
            {
                currentState = GameState.Game;
            }
            else if (currentState == GameState.GamePause && pMenu.IsSelected && SingleKeyPress(Keys.Enter))
            {
                ResetGame();
                currentState = GameState.Menu;
            }
            else if (currentState == GameState.GameOver && gRetry.IsSelected && SingleKeyPress(Keys.Enter))
            {
                ResetGame();
                currentState = GameState.Game;
            }
            else if (currentState == GameState.GameOver && gMenu.IsSelected && SingleKeyPress(Keys.Enter))
            {
                ResetGame();
                currentState = GameState.Menu;
            }


            //MAIN MENU
            if (currentState == GameState.Menu)
            {
                if (mStart.IsSelected && SingleKeyPress(Keys.S)) //down start to controls
                {
                    mControls.IsSelected = true;
                    mQuit.IsSelected = false;
                    mStart.IsSelected = false;
                }
                else if (mControls.IsSelected && SingleKeyPress(Keys.S)) //down controls to quit
                {
                    mControls.IsSelected = false;
                    mStart.IsSelected = false;
                    mQuit.IsSelected = true;
                }
                else if (mQuit.IsSelected && SingleKeyPress(Keys.S)) //down quit to start
                {
                    mQuit.IsSelected = false;
                    mControls.IsSelected = false;
                    mStart.IsSelected = true;
                }
                else if (mStart.IsSelected && SingleKeyPress(Keys.W)) //up start to quit
                {
                    mStart.IsSelected = false;
                    mControls.IsSelected = false;
                    mQuit.IsSelected = true;
                }
                else if (mControls.IsSelected && SingleKeyPress(Keys.W)) //up controls to start
                {
                    mControls.IsSelected = false;
                    mQuit.IsSelected = false;
                    mStart.IsSelected = true;
                }
                else if (mQuit.IsSelected && SingleKeyPress(Keys.W)) //up quit to controls
                {
                    mQuit.IsSelected = false;
                    mStart.IsSelected = false;
                    mControls.IsSelected = true;
                }
            }

            //PAUSE MENU
            if (currentState == GameState.GamePause)
            {
                if (pContinue.IsSelected && (SingleKeyPress(Keys.S) || SingleKeyPress(Keys.W)))
                {
                    pContinue.IsSelected = false;
                    pMenu.IsSelected = true;
                }
                else if (pMenu.IsSelected && (SingleKeyPress(Keys.S) || SingleKeyPress(Keys.W)))
                {
                    pMenu.IsSelected = false;
                    pContinue.IsSelected = true;
                }
            }

            //GAME OVER MENU
            if (currentState == GameState.GameOver)
            {
                if (gRetry.IsSelected && (SingleKeyPress(Keys.S) || SingleKeyPress(Keys.W)))
                {
                    gRetry.IsSelected = false;
                    gMenu.IsSelected = true;
                }
                else if (gMenu.IsSelected && (SingleKeyPress(Keys.S) || SingleKeyPress(Keys.W)))
                {
                    gMenu.IsSelected = false;
                    gRetry.IsSelected = true;
                }
            }



            //GAMEPLAY
            if (currentState == GameState.Game)
            {
                //MOVEMENT
                p1.Run(fog.bounds);
                playerLoc.X = p1.location.X;
                playerLoc.Y = p1.location.Y;
                //checks to see if spacebar is pressed, as well as if the player is not falling, 
                //as to only allow the spacebar to be pressed when the player is on the ground or "not falling"
                if ((ks.IsKeyDown(Keys.Space) && previousState.IsKeyUp(Keys.Space)) && !p1.falling)
                {
                    //below method sets player class bool jumping = true
                    p1.Jump();
                }
                //if the bool jumping from the player class is true then the below method will move the player
                p1.JumpUpdate();
                fog.Move(p1);
                //
                //enemy.Run(fog.location, p1);
                //testTank.Run(fog.location, p1);
                //test the tank charge
                //testTank.chargeUpdate(7, p1);
                //
                //fog.consumeEnemy(enemy);
                fog.damagePlayer(p1);

                /*if (testTank.location.X <= fog.location.X+100 && testTank.location.X > fog.location.X)
                {
                    fog.consumeEnemy(testTank);
                    testTank.isDead = true;
                }*/
                //fog.consumeEnemy(testTank);
                for (int i = 0; i < spawner.EnemyList.Count; i++)
                {
                    if (spawner.EnemyList[i].location.X <= fog.location.X + 600 && spawner.EnemyList[i].location.X > fog.location.X)
                    {
                        fog.consumeEnemy(spawner.EnemyList[i]);
                        spawner.EnemyList[i].isDead = true;
                    }

                }

                //player taking damage
                /*foreach(Enemy enemy in spawner.EnemyList)
                {
                    if (p1.location.Intersects(enemy.location))
                    {
                        if (p1.invincibilityFrame <= 0)
                        {
                            p1.isHit = true;
                            p1.takeDamage(enemy.Damage);
                            p1.invincibilityFrame = 180;
                        }
                    }
                    if (p1.invincibilityFrame > 0)
                    {
                        p1.invincibilityFrame--;
                    }
                    else
                    {
                        p1.color = Color.White;
                    }
                }*/

                //Player Attack

                
                var ms = Mouse.GetState();
                if (ms.LeftButton == ButtonState.Pressed && p1.location.Y < 330)
                {
                    if (startSpawn == true)
                    {
                        for (int i = 0; i < spawner.EnemyList.Count; i++)
                        {
                            if (p1.falling == true)
                            {
                                p1.airAttack(spawner.EnemyList[i], jumpAttack);
                            }
                            
                        }
                    }
                    else
                    {
                        if (p1.falling == true)
                        {
                            p1.airAttack(enemy, jumpAttack);
                        }
                        

                    }
                }
                else if (ms.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released && p1.location.Y == 330)
                {
                    if (startSpawn == true)
                    {
                        for (int i = 0; i < spawner.EnemyList.Count; i++)
                        {
                            p1.Attack(spawner.EnemyList[i], this);
                        }
                    }
                    else
                    {
                        p1.Attack(enemy, this);
                    }
                }
                oldState = ms;


                //animation timing
                timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

                if (timeCounter >= timePerFrame)
                {
                    frame += 1;                     // Adjust the frame

                    if (frame > WALK_FRAME_COUNT)
                    {
                        frame = 1;
                    }

                    if (p1.attacking)
                    {
                        swordFrame += 1;                     // Adjust the frame
                        swordSwing.Play();

                        if (swordFrame > SWORD_FRAME_COUNT)
                        {
                            p1.attacking = false;
                            swordFrame = 1;

                        }
                    }

                    gruntFrame += 1;                     // Adjust the frame

                    if (gruntFrame > GRUNT_FRAME_COUNT)
                    {
                        gruntFrame = 1;
                    }

                    if (count == 2)//makes this go slower
                    {
                        tankFrame += 1;                     // Adjust the frame

                        if (tankFrame >= TANK_FRAME_COUNT)
                        {
                            tankFrame = 1;
                        }
                        count = 0;
                    }

                    count++;
                    timeCounter -= timePerFrame;    // Remove the time we "used"

                }

                //DISTANCE AND SCORE UPDATE
                if (p1.location.X > farPoint)
                {
                    farPoint = p1.location.X;
                }

                currentScore = (farPoint / 4) - 50;
                if (currentScore != 0 && currentScore % 300 == 0)
                {
                    spawner.CreateSpawn("values.txt", gruntSheet, tankSheet, p1.location, gruntDie, tankDie);
                    startSpawn = true;
                }

                //update healthstate based on player health

                if (p1.Health == 3.0)
                {
                    p1.healthState = HealthState.ThreeFull;
                }
                else if (p1.Health == 2.5)
                {
                    p1.healthState = HealthState.FiveHalves;
                }
                else if (p1.Health == 2.0)
                {
                    p1.healthState = HealthState.TwoFull;
                }
                else if (p1.Health == 1.5)
                {
                    p1.healthState = HealthState.ThreeHalves;
                }
                else if (p1.Health == 1.0)
                {
                    p1.healthState = HealthState.OneFull;
                }
                else if (p1.Health == 0.5)
                {
                    p1.healthState = HealthState.OneHalf;
                }

                //CAMERA
                if (ks.IsKeyDown(Keys.D))
                {
                    
                        camera.LookAt(new Vector2(p1.location.X + 200, 240));

                    //camera.Position += new Vector2(250, 0) * deltaTime / 2;
                }
                if (ks.IsKeyDown(Keys.A))
                {
                    
                        camera.LookAt(new Vector2(p1.location.X + 200, 240));
                    //camera.Position -= new Vector2(250, 0) * deltaTime / 2;
                }
            }

            previousState = ks;
            //oldState = ms;

            base.Update(gameTime);
        }

        /*private void RandomSpawn()
        {
            if (spawner.EnemyList[0].isDead == false)
            {
                DrawGruntHopping(0, spawner.EnemyList[0]);
                spawner.EnemyList[0].Run(fog.bounds, p1);
            }
            else
            {
                spawnRandom = false;
            }
           
        }*/

        private void DrawWave()
        {

            //spawner.CreateSpawn("values.txt", gruntSheet, tankSheet, p1.location);
            if (spawner.EnemyList[0].isDead == false)
            {
                if (spawner.EnemyList[0] is Grunt)
                {
                    if (spawner.EnemyList[0].eState == EnemyState.FaceRight)
                    {
                        DrawGruntHopping(SpriteEffects.FlipHorizontally, spawner.EnemyList[0]);
                        spawner.EnemyList[0].Run(fog.bounds, p1);
                    }
                    else
                    {
                        DrawGruntHopping(0, spawner.EnemyList[0]);
                        spawner.EnemyList[0].Run(fog.bounds, p1);
                    }

                }
                else if (spawner.EnemyList[0] is Tank)
                {
                    if (spawner.EnemyList[0].eState == EnemyState.FaceRight)
                    {
                        DrawTankRunning(SpriteEffects.FlipHorizontally, spawner.EnemyList[0]);
                        spawner.EnemyList[0].Run(fog.location, p1);
                        spawner.EnemyList[0].chargeUpdate(7, p1);
                    }
                    else
                    {
                        DrawTankRunning(0, spawner.EnemyList[0]);
                        spawner.EnemyList[0].Run(fog.location, p1);
                        spawner.EnemyList[0].chargeUpdate(7, p1);
                    }
                }
            }

            if (spawner.EnemyList[1].isDead == false)
            {
                if (spawner.EnemyList[1] is Grunt)
                {
                    if (spawner.EnemyList[1].eState == EnemyState.FaceRight)
                    {
                        DrawGruntHopping(SpriteEffects.FlipHorizontally, spawner.EnemyList[1]);
                        spawner.EnemyList[1].Run(fog.bounds, p1);
                    }
                    else
                    {
                        DrawGruntHopping(0, spawner.EnemyList[1]);
                        spawner.EnemyList[1].Run(fog.bounds, p1);
                    }
                }
                else if (spawner.EnemyList[1] is Tank)
                {
                    if (spawner.EnemyList[1].eState == EnemyState.FaceRight)
                    {
                        DrawTankRunning(SpriteEffects.FlipHorizontally, spawner.EnemyList[1]);
                        spawner.EnemyList[1].Run(fog.location, p1);
                        spawner.EnemyList[1].chargeUpdate(7, p1);
                    }
                    else
                    {
                        DrawTankRunning(0, spawner.EnemyList[1]);
                        spawner.EnemyList[1].Run(fog.location, p1);
                        spawner.EnemyList[1].chargeUpdate(7, p1);
                    }
                }
            }

            if (spawner.EnemyList[2].isDead == false)
            {
                if (spawner.EnemyList[2] is Grunt)
                {
                    if (spawner.EnemyList[2].eState == EnemyState.FaceRight)
                    {
                        DrawGruntHopping(SpriteEffects.FlipHorizontally, spawner.EnemyList[2]);
                        spawner.EnemyList[2].Run(fog.bounds, p1);
                    }
                    else
                    {
                        DrawGruntHopping(0, spawner.EnemyList[2]);
                        spawner.EnemyList[2].Run(fog.bounds, p1);
                    }
                }
                else if (spawner.EnemyList[2] is Tank)
                {
                    if (spawner.EnemyList[2].eState == EnemyState.FaceRight)
                    {
                        DrawTankRunning(SpriteEffects.FlipHorizontally, spawner.EnemyList[2]);
                        spawner.EnemyList[2].Run(fog.location, p1);
                        spawner.EnemyList[2].chargeUpdate(7, p1);
                    }
                    else
                    {
                        DrawTankRunning(0, spawner.EnemyList[2]);
                        spawner.EnemyList[2].Run(fog.location, p1);
                        spawner.EnemyList[2].chargeUpdate(7, p1);
                    }
                }
            }

            if (spawner.EnemyList[3].isDead == false)
            {
                if (spawner.EnemyList[3] is Grunt)
                {
                    if (spawner.EnemyList[3].eState == EnemyState.FaceRight)
                    {
                        DrawGruntHopping(SpriteEffects.FlipHorizontally, spawner.EnemyList[3]);
                        spawner.EnemyList[3].Run(fog.bounds, p1);
                    }
                    else
                    {
                        DrawGruntHopping(0, spawner.EnemyList[3]);
                        spawner.EnemyList[3].Run(fog.bounds, p1);
                    }
                }
                else if (spawner.EnemyList[3] is Tank)
                {
                    if (spawner.EnemyList[3].eState == EnemyState.FaceRight)
                    {
                        DrawTankRunning(SpriteEffects.FlipHorizontally, spawner.EnemyList[3]);
                        spawner.EnemyList[3].Run(fog.location, p1);
                        spawner.EnemyList[3].chargeUpdate(7, p1);
                    }
                    else
                    {
                        DrawTankRunning(0, spawner.EnemyList[3]);
                        spawner.EnemyList[3].Run(fog.location, p1);
                        spawner.EnemyList[3].chargeUpdate(7, p1);
                    }
                }
            }

            if (spawner.EnemyList[4].isDead == false)
            {
                if (spawner.EnemyList[4] is Grunt)
                {
                    if (spawner.EnemyList[4].eState == EnemyState.FaceRight)
                    {
                        DrawGruntHopping(SpriteEffects.FlipHorizontally, spawner.EnemyList[4]);
                        spawner.EnemyList[4].Run(fog.bounds, p1);
                    }
                    else
                    {
                        DrawGruntHopping(0, spawner.EnemyList[4]);
                        spawner.EnemyList[4].Run(fog.bounds, p1);
                    }
                }
                else if (spawner.EnemyList[4] is Tank)
                {
                    if (spawner.EnemyList[4].eState == EnemyState.FaceRight)
                    {
                        DrawTankRunning(SpriteEffects.FlipHorizontally, spawner.EnemyList[4]);
                        spawner.EnemyList[4].Run(fog.location, p1);
                        spawner.EnemyList[4].chargeUpdate(7, p1);
                    }
                    else
                    {
                        DrawTankRunning(0, spawner.EnemyList[4]);
                        spawner.EnemyList[4].Run(fog.location, p1);
                        spawner.EnemyList[4].chargeUpdate(7, p1);
                    }
                }
            }

            else
            {
                spawner.EnemyList.Clear();
                startSpawn = false;
            }
        }
        //ANIMATION
        private void EnemyDie(Enemy enemy, int counter)
        {
            if (counter < 5)
            {
                //spriteBatch.Draw(deathFog, new Rectangle(enemy.deathLocation, 372, 100, 100), Color.White);
                //EnemyDie(enemy, counter + 1);
            }

        }

        private void DrawPlayerStanding(SpriteEffects flipSprite)
        {
            spriteBatch.Draw(
                spriteSheet,                    // - The texture to draw
                playerLoc,                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    0,                          //   - This rectangle specifies
                    PLAYER_RECT_Y_OFFSET,        //	   where "inside" the texture
                    PLAYER_RECT_WIDTH,           //     to get pixels (We don't want to
                    PLAYER_RECT_HEIGHT),         //     draw the whole thing)
                p1.color,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        private void DrawPlayerWalking(SpriteEffects flipSprite)
        {
            spriteBatch.Draw(
                spriteSheet,                    // - The texture to draw
                playerLoc,                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    frame * PLAYER_RECT_WIDTH,   //   - This rectangle specifies
                    PLAYER_RECT_Y_OFFSET,        //	   where "inside" the texture
                    PLAYER_RECT_WIDTH,           //     to get pixels (We don't want to
                    PLAYER_RECT_HEIGHT),         //     draw the whole thing)
                p1.color,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }
        private void DrawSword(SpriteEffects flipSprite)
        {
            if (p1.AirAttack == false)
            {


                if (flipSprite != 0)
                {
                    swordPos.X = (playerLoc.X - 40);
                    swordPos.Y = (playerLoc.Y - 30);
                }
                else
                {
                    swordPos.X = (playerLoc.X + 30);
                    swordPos.Y = (playerLoc.Y - 30);
                }
                spriteBatch.Draw(
                    sword,
                    new Vector2(playerLoc.X + 10, playerLoc.Y - 40),
                    new Rectangle(0, 0, 140, 140),
                    Color.White,
                    0,
                    Vector2.Zero,
                    1.0f,
                    flipSprite,
                    0);
            }
        }
        private void SwordSwing(SpriteEffects flipSprite)
        {
            if (p1.AirAttack == false)
            {
                if (flipSprite != 0)
                {
                    swordPos.X = (playerLoc.X - 40);
                    swordPos.Y = (playerLoc.Y - 30);
                }
                else
                {
                    swordPos.X = (playerLoc.X + 30);
                    swordPos.Y = (playerLoc.Y - 30);
                }
                spriteBatch.Draw(
                    swordSprite,                    // - The texture to draw
                    swordPos, // - The location to draw on the screen
                    new Rectangle(                  // - The "source" rectangle
                        swordFrame * SWORD_RECT_WIDTH,   //   - This rectangle specifies
                        SWORD_RECT_Y_OFFSET,        //	   where "inside" the texture
                        SWORD_RECT_WIDTH,           //     to get pixels (We don't want to
                        SWORD_RECT_HEIGHT),         //     draw the whole thing)
                    Color.White,                    // - The color
                    0,                              // - Rotation (none currently)
                    Vector2.Zero,                   // - Origin inside the image (top left)
                    1.0f,                           // - Scale (100% - no change)
                    flipSprite,                     // - Can be used to flip the image
                    0);                             // - Layer depth (unused)
            }
        }

        private void DrawGruntHopping(SpriteEffects flipSprite, Enemy grunt)
        {
            spriteBatch.Draw(
                gruntSheet,                    // - The texture to draw
                new Vector2(grunt.location.X, grunt.location.Y),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    gruntFrame * GRUNT_RECT_WIDTH,   //   - This rectangle specifies
                    GRUNT_RECT_Y_OFFSET,        //	   where "inside" the texture
                    GRUNT_RECT_WIDTH,           //     to get pixels (We don't want to
                    GRUNT_RECT_HEIGHT),         //     draw the whole thing)
                grunt.color,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        private void DrawTankRunning(SpriteEffects flipSprite, Enemy tank)
        {
            spriteBatch.Draw(
                tankSheet,                    // - The texture to draw
                new Vector2(tank.location.X, tank.location.Y),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    tankFrame * TANK_RECT_WIDTH,   //   - This rectangle specifies
                    TANK_RECT_Y_OFFSET,        //	   where "inside" the texture
                    TANK_RECT_WIDTH,           //     to get pixels (We don't want to
                    TANK_RECT_HEIGHT),         //     draw the whole thing)
                tank.color,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }


        private void DrawFullHeart(Vector2 location)
        {

            spriteBatch.Draw(
                heart,
                location,
                new Rectangle(
                    0,
                    0,
                    36,
                    34),
                Color.White);
        }

        private void DrawHalfHeart(Vector2 location)
        {

            spriteBatch.Draw(
                heart,
                location,
                new Rectangle(
                    36,
                    0,
                    36,
                    34),
                Color.White);
        }

        //DRAW ///////////////////////////////////////////
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var viewMatrix = camera.GetViewMatrix();
            spriteBatch.Begin(transformMatrix: viewMatrix);

            switch (currentState)
            {
                //MAIN MENU
                case GameState.Menu:
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Draw(mainMenuImage, new Vector2(camera.Position.X, 0));
                    mStart.DrawSelectText(spriteBatch, textFont, "START", new Vector2(camera.Position.X + 350, 250));
                    mControls.DrawSelectText(spriteBatch, textFont, "CONTROLS", new Vector2(camera.Position.X + 350, 300));
                    mQuit.DrawSelectText(spriteBatch, textFont, "QUIT", new Vector2(camera.Position.X + 350, 350));

                    break;

                //CONTROLS
                case GameState.Controls:
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Draw(controlsImage, new Vector2(0, 0));
                    cReturn.DrawSelectText(spriteBatch, textFont, "RETURN", new Vector2(camera.Position.X + 600, 20));
                    break;

                //GAMEPLAY
                case GameState.Game:
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Draw(bg, new Rectangle(-p1.currentX, 0, 2000, GraphicsDevice.Viewport.Height), Color.White);

                    for (int i = 2; i < 50; i += 2) //temporary fix to bg cut off
                    {
                        spriteBatch.Draw(bg, new Rectangle((i * 999 - p1.currentX), 0, 2000, GraphicsDevice.Viewport.Height), Color.White);

                    }

                    spriteBatch.Draw(floor, new Rectangle((int)camera.Position.X - 5, 450, 861, 30), Color.White);

                    if (p1.AirAttack == true)
                    {
                        spriteBatch.Draw(jumpAttack, new Rectangle(p1.location.X, p1.location.Y, 140, 200), Color.White);
                    }
                    else
                    {
                        switch (p1.playerState)
                        {
                            //FACELEFT
                            case PlayerState.FaceLeft:
                                if ((p1.prevPlayerState == PlayerState.FaceRight) || (p1.prevPlayerState == PlayerState.WalkRight))
                                {
                                    DrawPlayerStanding(SpriteEffects.FlipHorizontally);
                                    if (p1.attacking)
                                    {
                                        SwordSwing(SpriteEffects.FlipHorizontally);
                                    }
                                    else
                                    {
                                        DrawSword(SpriteEffects.FlipHorizontally);

                                    }
                                }
                                else
                                {
                                    DrawPlayerStanding(SpriteEffects.FlipHorizontally); //if he wasnt facing or walking right, draw him standing facing left
                                    if (p1.attacking)
                                    {
                                        SwordSwing(SpriteEffects.FlipHorizontally);
                                    }
                                    else
                                    {
                                        DrawSword(SpriteEffects.FlipHorizontally);
                                    }
                                }
                                break;

                            //WALKLEFT
                            case PlayerState.WalkLeft:
                                DrawPlayerWalking(SpriteEffects.FlipHorizontally);
                                if (p1.attacking)
                                {
                                    SwordSwing(SpriteEffects.FlipHorizontally);
                                }
                                else
                                {
                                    DrawSword(SpriteEffects.FlipHorizontally);
                                }
                                break;

                            //FACERIGHT
                            case PlayerState.FaceRight:
                                if ((p1.prevPlayerState == PlayerState.FaceLeft) || (p1.prevPlayerState == PlayerState.WalkLeft))
                                {
                                    DrawPlayerStanding(SpriteEffects.FlipHorizontally);
                                    if (p1.attacking)
                                    {
                                        SwordSwing(SpriteEffects.FlipHorizontally);
                                    }
                                    else
                                    {
                                        DrawSword(SpriteEffects.FlipHorizontally);
                                    }
                                }
                                else
                                {
                                    DrawPlayerStanding(0);
                                    if (p1.attacking)
                                    {
                                        SwordSwing(0);
                                    }
                                    else
                                    {
                                        DrawSword(0);
                                    }
                                }
                                break;

                            //WALKRIGHT
                            case PlayerState.WalkRight:
                                DrawPlayerWalking(0);
                                if (p1.attacking)
                                {
                                    SwordSwing(0);
                                }
                                else
                                {
                                    DrawSword(0);
                                }
                                break;

                            //JUMP_LEFT
                            case PlayerState.JumpLeft:
                                DrawPlayerStanding(SpriteEffects.FlipHorizontally);
                                if (p1.attacking)
                                {
                                    SwordSwing(SpriteEffects.FlipHorizontally);
                                }
                                else
                                {
                                    DrawSword(SpriteEffects.FlipHorizontally);
                                }
                                break;
                            //JUMP_RIGHT
                            case PlayerState.JumpRight:
                                DrawPlayerStanding(0);
                                if (p1.attacking)
                                {
                                    SwordSwing(0);
                                }
                                else
                                {
                                    DrawSword(0);
                                }
                                break;

                            default:
                                break;

                        }

                    }




                    if (startSpawn == true)
                    {
                        DrawWave();
                    }


                    spriteBatch.Draw(fogSprite, new Rectangle(fog.location.X, fog.location.Y, fog.location.Width, fog.location.Height), Color.White);

                    spriteBatch.Draw(uIBar, new Rectangle((int)camera.Position.X - 20, 0, 888, 50), Color.White);

                    //check health state and draw hearts
                    switch (p1.healthState)
                    {
                        case HealthState.ThreeFull:
                            DrawFullHeart(new Vector2(camera.Position.X + 20, 5));
                            DrawFullHeart(new Vector2(camera.Position.X + 60, 5));
                            DrawFullHeart(new Vector2(camera.Position.X + 100, 5));
                            break;

                        case HealthState.FiveHalves:
                            DrawFullHeart(new Vector2(camera.Position.X + 20, 5));
                            DrawFullHeart(new Vector2(camera.Position.X + 60, 5));
                            DrawHalfHeart(new Vector2(camera.Position.X + 100, 5));
                            break;

                        case HealthState.TwoFull:
                            DrawFullHeart(new Vector2(camera.Position.X + 20, 5));
                            DrawFullHeart(new Vector2(camera.Position.X + 60, 5));
                            break;

                        case HealthState.ThreeHalves:
                            DrawFullHeart(new Vector2(camera.Position.X + 20, 5));
                            DrawHalfHeart(new Vector2(camera.Position.X + 60, 5));
                            break;

                        case HealthState.OneFull:
                            DrawFullHeart(new Vector2(camera.Position.X + 20, 5));
                            break;

                        case HealthState.OneHalf:
                            DrawHalfHeart(new Vector2(camera.Position.X + 20, 5));
                            break;

                    }
                    spriteBatch.DrawString(textFont, "HIGH SCORE", new Vector2(camera.Position.X + 500, 10), Color.White);
                    spriteBatch.DrawString(textFont, data.loadHighScore().ToString(), new Vector2(camera.Position.X + 720, 10), Color.White); //high score num
                    spriteBatch.DrawString(titleFont, currentScore.ToString(), new Vector2(camera.Position.X + 380, 10), Color.White); //current score num

                    break;

                //GAME PAUSE
                case GameState.GamePause:
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Draw(bg, new Rectangle(-p1.currentX, 0, 2000, GraphicsDevice.Viewport.Height), Color.White);

                    for (int i = 2; i < 50; i += 2) //temporary fix to bg cut off
                    {
                        spriteBatch.Draw(bg, new Rectangle((i * 999 - p1.currentX), 0, 2000, GraphicsDevice.Viewport.Height), Color.White);

                    }

                    spriteBatch.Draw(floor, new Rectangle((int)camera.Position.X - 5, 450, 861, 30), Color.White);

                    spriteBatch.Draw(fog.sprite, new Rectangle(fog.location.X, fog.location.Y, fog.location.Width, fog.location.Height), Color.White);

                    spriteBatch.Draw(uIBar, new Rectangle((int)camera.Position.X - 20, 0, 888, 50), Color.White);

                    if (p1.AirAttack == true)
                    {
                        spriteBatch.Draw(jumpAttack, new Rectangle(p1.location.X, p1.location.Y, 140, 200), Color.White);
                        break;
                    }
                    else
                    {
                        switch (p1.playerState)
                        {
                            //FACELEFT
                            case PlayerState.FaceLeft:
                                if ((p1.prevPlayerState == PlayerState.FaceRight) || (p1.prevPlayerState == PlayerState.WalkRight))
                                {
                                    DrawPlayerStanding(SpriteEffects.FlipHorizontally);
                                    if (p1.attacking)
                                    {
                                        SwordSwing(SpriteEffects.FlipHorizontally);
                                    }
                                    else
                                    {
                                        DrawSword(SpriteEffects.FlipHorizontally);
                                    }
                                }
                                else
                                {
                                    DrawPlayerStanding(SpriteEffects.FlipHorizontally); //if he wasnt facing or walking right, draw him standing facing left
                                    if (p1.attacking)
                                    {
                                        SwordSwing(SpriteEffects.FlipHorizontally);
                                    }
                                    else
                                    {
                                        DrawSword(SpriteEffects.FlipHorizontally);
                                    }
                                }
                                break;

                            //WALKLEFT
                            case PlayerState.WalkLeft:
                                DrawPlayerWalking(SpriteEffects.FlipHorizontally);
                                if (p1.attacking)
                                {
                                    SwordSwing(SpriteEffects.FlipHorizontally);
                                }
                                else
                                {
                                    DrawSword(SpriteEffects.FlipHorizontally);
                                }
                                break;

                            //FACERIGHT
                            case PlayerState.FaceRight:
                                if ((p1.prevPlayerState == PlayerState.FaceLeft) || (p1.prevPlayerState == PlayerState.WalkLeft))
                                {
                                    DrawPlayerStanding(SpriteEffects.FlipHorizontally);
                                    if (p1.attacking)
                                    {
                                        SwordSwing(SpriteEffects.FlipHorizontally);
                                    }
                                    else
                                    {
                                        DrawSword(SpriteEffects.FlipHorizontally);
                                    }
                                }
                                else
                                {
                                    DrawPlayerStanding(0);
                                    if (p1.attacking)
                                    {
                                        SwordSwing(0);
                                    }
                                    else
                                    {
                                        DrawSword(0);
                                    }
                                }
                                break;

                            //WALKRIGHT
                            case PlayerState.WalkRight:
                                DrawPlayerWalking(0);
                                if (p1.attacking)
                                {
                                    SwordSwing(0);
                                }
                                else
                                {
                                    DrawSword(0);
                                }
                                break;

                            //JUMP_LEFT
                            case PlayerState.JumpLeft:
                                DrawPlayerStanding(SpriteEffects.FlipHorizontally);
                                if (p1.attacking)
                                {
                                    SwordSwing(SpriteEffects.FlipHorizontally);
                                }
                                else
                                {
                                    DrawSword(SpriteEffects.FlipHorizontally);
                                }
                                break;
                            //JUMP_RIGHT
                            case PlayerState.JumpRight:
                                DrawPlayerStanding(0);
                                if (p1.attacking)
                                {
                                    SwordSwing(0);
                                }
                                else
                                {
                                    DrawSword(0);
                                }
                                break;

                        }


                        switch (p1.healthState)
                        {
                            case HealthState.ThreeFull:
                                DrawFullHeart(new Vector2(camera.Position.X + 20, 5));
                                DrawFullHeart(new Vector2(camera.Position.X + 60, 5));
                                DrawFullHeart(new Vector2(camera.Position.X + 100, 5));
                                break;

                            case HealthState.FiveHalves:
                                DrawFullHeart(new Vector2(camera.Position.X + 20, 5));
                                DrawFullHeart(new Vector2(camera.Position.X + 60, 5));
                                DrawHalfHeart(new Vector2(camera.Position.X + 100, 5));
                                break;

                            case HealthState.TwoFull:
                                DrawFullHeart(new Vector2(camera.Position.X + 20, 5));
                                DrawFullHeart(new Vector2(camera.Position.X + 60, 5));
                                break;

                            case HealthState.ThreeHalves:
                                DrawFullHeart(new Vector2(camera.Position.X + 20, 5));
                                DrawHalfHeart(new Vector2(camera.Position.X + 60, 5));
                                break;

                            case HealthState.OneFull:
                                DrawFullHeart(new Vector2(camera.Position.X + 20, 5));
                                break;

                            case HealthState.OneHalf:
                                DrawHalfHeart(new Vector2(camera.Position.X + 20, 5));
                                break;

                        }

                        spriteBatch.DrawString(textFont, "HIGH SCORE", new Vector2(camera.Position.X + 500, 10), Color.White);
                        spriteBatch.DrawString(textFont, data.loadHighScore().ToString(), new Vector2(camera.Position.X + 720, 10), Color.White); //high score num
                        spriteBatch.DrawString(titleFont, currentScore.ToString(), new Vector2(camera.Position.X + 380, 10), Color.White); //current score num

                        spriteBatch.Draw(pauseImage, new Vector2(camera.Position.X, 0));
                        pContinue.DrawSelectText(spriteBatch, textFont, "CONTINUE", new Vector2(camera.Position.X + 335, 205));
                        pMenu.DrawSelectText(spriteBatch, textFont, "MAIN MENU", new Vector2(camera.Position.X + 330, 245));
                        break;
                    }

                //GAME OVER
                case GameState.GameOver:

                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Draw(gameOverImage, new Vector2(camera.Position.X, 0));
                    gRetry.DrawSelectText(spriteBatch, textFont, "RETRY", new Vector2(camera.Position.X + 330, 250));
                    gMenu.DrawSelectText(spriteBatch, textFont, "MAIN MENU", new Vector2(camera.Position.X + 330, 300));
                    spriteBatch.DrawString(textFont, data.loadHighScore().ToString(), new Vector2(camera.Position.X + 700, 60), Color.White); //high score num
                    spriteBatch.DrawString(titleFont, currentScore.ToString(), new Vector2(camera.Position.X + 60, 20), Color.White); //current score num

                    break;

                default:
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);


        }

        //METHODS
        //SingleKeyPress. check if key is pressed, useful for menu navigation
        public bool SingleKeyPress(Keys key)
        {
            if (ks.IsKeyDown(key) && previousState.IsKeyUp(key))
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        //ResetGame. resets game to initial state
        public void ResetGame()
        {
            if (currentScore > data.loadHighScore())
            {
                data.newHighScore(currentScore);
            }
            camera.Position = new Vector2(0, 0); //reset camera so it doesnt stay off-centered in other states
            p1.location = new Rectangle(200, 350, 120, 140);
            p1.isDead = false;
            p1.color = Color.White;
            p1.Health = 3;
            currentScore = 0;
            farPoint = 0;
            //enemy.location = new Rectangle(600, 360, 100, 100);
            fog.location = new Rectangle(-800, 0, 1000, 500);
            fog.bounds = new Rectangle(-700, 0, 300, 700);
            fog.Speed = 1;
            startSpawn = false;
            MediaPlayer.Play(backgroundMusic);
        }


    }
}