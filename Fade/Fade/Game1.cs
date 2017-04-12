using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        int currentScore = 0;

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
        //Song backgroundMusic;
        //SoundEffect hitSound;
        //we will uncomment these when we're ready to use them

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
        Texture2D enemySheet;
        Texture2D sword;
        Texture2D swordSprite;
        Texture2D heart;

        //OBJECTS
        Player p1;
        Fog fog;
        Camera2D camera;
        ExternalTool tool = new ExternalTool();
        Enemy enemy;
        EnemySpawner spawner = new EnemySpawner();

        //ENUMS
        GameState currentState = GameState.Menu;

        //KB & MOUSE
        KeyboardState ks;
        KeyboardState previousState = Keyboard.GetState();
        MouseState ms;
        MouseState oldState = Mouse.GetState();

        //ANIMATION
        int frame;
        int swordFrame;
        int gruntFrame;
        double timeCounter;     
        double fps;             
        double timePerFrame;    
        Vector2 playerLoc;

        // player rectangle
        const int WALK_FRAME_COUNT = 6;         // The number of frames in the animation
        const int PLAYER_RECT_Y_OFFSET = 0;    // How far down in the image are the frames?
        const int PLAYER_RECT_HEIGHT = 142;       // The height of a single frame
        const int PLAYER_RECT_WIDTH = 137;        // The width of a single frame

        // grunt rectangle
        const int GRUNT_FRAME_COUNT = 7;         // The number of frames in the animation
        const int GRUNT_RECT_Y_OFFSET = 0;    // How far down in the image are the frames?
        const int GRUNT_RECT_HEIGHT = 102;       // The height of a single frame
        const int GRUNT_RECT_WIDTH = 150;        // The width of a single frame

        //sword rectangle
        const int SWORD_FRAME_COUNT = 3;         // The number of frames in the animation
        const int SWORD_RECT_Y_OFFSET = 0;    // How far down in the image are the frames?
        const int SWORD_RECT_HEIGHT = 200;       // The height of a single frame
        const int SWORD_RECT_WIDTH = 132;        // The width of a single frame

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
            fps = 8.0;
            timePerFrame = 1.0 / fps;
            startPoint = 200;
            farPoint = 200;
            tool.writeFile();
            //tool = new ExternalTool();
            //tool.writeFile();
            base.Initialize();
        }

        //LOAD ///////////////////////////////////////////
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //textures
            //playerSprite = Content.Load<Texture2D>("char1sword");
            fogSprite = Content.Load<Texture2D>("fogfull");
            bg = Content.Load<Texture2D>("bg");
            mainMenuImage = Content.Load<Texture2D>("menus/menu2");
            pauseImage = Content.Load<Texture2D>("menus/pausebg");
            controlsImage = Content.Load<Texture2D>("menus/controls");
            gameOverImage = Content.Load<Texture2D>("menus/gameoverbg");
            uIBar = Content.Load<Texture2D>("menus/uiBar");
            spriteSheet = Content.Load<Texture2D>("characters/charsprite");
            enemySheet = Content.Load<Texture2D>("characters/grunt2");
            swordSprite = Content.Load<Texture2D>("characters/swordSprite2");
            sword = Content.Load<Texture2D>("characters/sword");
            floor = Content.Load<Texture2D>("floor");
            heart = Content.Load<Texture2D>("menus/hearts");

            //type
            textFont = Content.Load<SpriteFont>("textFont");
            titleFont = Content.Load<SpriteFont>("titleFont");

            //objects
            p1 = new Player(spriteSheet, new Rectangle(200, 330, 120, 140));
            mStart = new SelectText(true, Color.White, Color.Black);
            mQuit = new SelectText();
            mControls = new SelectText();
            cReturn = new SelectText(true, Color.White, Color.Magenta);
            pContinue = new SelectText(true, Color.Black, Color.Magenta);
            pMenu = new SelectText(false, Color.Black, Color.Magenta);
            gRetry = new SelectText(true, Color.White, Color.Magenta);
            gMenu = new SelectText(false, Color.White, Color.Magenta);
            fog = new Fog(fogSprite, new Rectangle(-600, 0, 800, 480), new Rectangle(-500, 0, 300, 700), 1, 0);
            enemy = new Enemy(enemySheet, new Rectangle(600,380,0,0), 1, 3, 1);
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
            else if(currentState == GameState.Controls && SingleKeyPress(Keys.Enter))
            {
                currentState = GameState.Menu;
            }
            else if (currentState == GameState.Game && SingleKeyPress(Keys.P))
            {
                currentState = GameState.GamePause;
            }
            else if (currentState == GameState.Game && (p1.isDead || SingleKeyPress(Keys.B)))  //B key is temp for testing til we have damage going
            {
                ResetGame();
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
            else if (currentState == GameState.GameOver && SingleKeyPress(Keys.Enter))
            {
                currentState = GameState.Menu;
            }

            
            //MAIN MENU
            if(currentState == GameState.Menu)
            {
                if(mStart.IsSelected && SingleKeyPress(Keys.S)) //down start to controls
                {
                    mControls.IsSelected = true;
                    mQuit.IsSelected = false;
                    mStart.IsSelected = false;
                }
                else if(mControls.IsSelected && SingleKeyPress(Keys.S)) //down controls to quit
                {
                    mControls.IsSelected = false;
                    mStart.IsSelected = false;
                    mQuit.IsSelected = true;
                }
                else if(mQuit.IsSelected && SingleKeyPress(Keys.S)) //down quit to start
                {
                    mQuit.IsSelected = false;
                    mControls.IsSelected = false;
                    mStart.IsSelected = true;
                }
                else if(mStart.IsSelected && SingleKeyPress(Keys.W)) //up start to quit
                {
                    mStart.IsSelected = false;
                    mControls.IsSelected = false;
                    mQuit.IsSelected = true;
                }
                else if(mControls.IsSelected && SingleKeyPress(Keys.W)) //up controls to start
                {
                    mControls.IsSelected = false;
                    mQuit.IsSelected = false;
                    mStart.IsSelected = true;
                }
                else if(mQuit.IsSelected && SingleKeyPress(Keys.W)) //up quit to controls
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
            if(currentState == GameState.GameOver)
            {
                if(gRetry.IsSelected && (SingleKeyPress(Keys.S) || SingleKeyPress(Keys.W)))
                {
                    gRetry.IsSelected = false;
                    gMenu.IsSelected = true;
                }
                else if(gMenu.IsSelected && (SingleKeyPress(Keys.S) || SingleKeyPress(Keys.W)))
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
                //as to only allow the spacebar to be pressed when the player is on the grounf or "not falling"
                if (ks.IsKeyDown(Keys.Space) && !p1.falling)
                {
                    //below method sets player class bool jumping = true
                     p1.Jump();

                }
                //if the bool jumping frmo the player class is true then the below method will move the player
                p1.JumpUpdate();
                fog.Move(p1);
                //
                enemy.Run(fog.location,p1);
                //
                fog.consumeEnemy(enemy);
                fog.damagePlayer(p1);

                
                //player taking damage
                if (p1.location.Intersects(enemy.location))
                {
                    if (p1.invincibilityFrame <= 0)
                    {
                        p1.takeDamage(enemy.Damage, spriteBatch);
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

                //Player Attack
                ms = Mouse.GetState();
                if (ms.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
                {
                    p1.Attack();
                }

               //walk animation timing
                timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
                if (timeCounter >= timePerFrame)
                {
                    frame += 1;                     // Adjust the frame

                    if (frame > WALK_FRAME_COUNT)   // Check the bounds
                        frame = 1;                  // Back to 1 (since 0 is the "standing" frame)

                    timeCounter -= timePerFrame;    // Remove the time we "used"
                }

                if (timeCounter >= timePerFrame)
                {
                    swordFrame += 1;                     // Adjust the frame

                    if (swordFrame > SWORD_FRAME_COUNT)   // Check the bounds
                        swordFrame = 1; 

                    timeCounter -= timePerFrame;    // Remove the time we "used"
                }

                if (timeCounter >= timePerFrame)
                {
                    gruntFrame += 1;                     // Adjust the frame

                    if (gruntFrame > GRUNT_FRAME_COUNT)   // Check the bounds
                        gruntFrame = 1;

                    timeCounter -= timePerFrame;    // Remove the time we "used"
                }

                //DISTANCE AND SCORE UPDATE
                if (p1.location.X > farPoint)
                {
                    farPoint = p1.location.X;
                }
                
                currentScore = (farPoint/4) - 50;
                if(currentScore == 200)
                {
                    startSpawn = true;
                }

                //CAMERA
                if (ks.IsKeyDown(Keys.D))
                {
                    if(startSpawn == true)
                    {
                        
                    }
                    else
                    {
                        camera.LookAt(new Vector2(p1.location.X + 200, 240));
                    }
                        
                    //camera.Position += new Vector2(250, 0) * deltaTime / 2;
                }
                if (ks.IsKeyDown(Keys.A))
                {
                    if (startSpawn == true)
                    {
                        
                    }
                    else
                        camera.LookAt(new Vector2(p1.location.X + 200, 240));
                    //camera.Position -= new Vector2(250, 0) * deltaTime / 2;
                }
            }

            previousState = ks;
            oldState = ms;

            base.Update(gameTime);
        }

        private void DrawWave()
        {
            spawner.CreateSpawn("values.txt", sword, sword, p1.location);
            DrawGruntHopping(0, spawner.EnemyList[0]);
            DrawGruntHopping(0, spawner.EnemyList[1]);
            DrawGruntHopping(0, spawner.EnemyList[2]);
            DrawGruntHopping(0, spawner.EnemyList[3]);
            DrawGruntHopping(0, spawner.EnemyList[4]);
            //startSpawn = false;
        }
        //ANIMATION

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
            spriteBatch.Draw(
                sword,
                new Vector2(playerLoc.X + 10, playerLoc.Y - 40),
                new Rectangle(0, 0, 140, 140),
                p1.color,
                0,
                Vector2.Zero,
                1.0f,
                flipSprite,
                0);
        }
        private void SwordSwing(SpriteEffects flipSprite)
        {
            spriteBatch.Draw(
                swordSprite,                    // - The texture to draw
                new Vector2(playerLoc.X + 45, playerLoc.Y - 40), // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    frame * SWORD_RECT_WIDTH,   //   - This rectangle specifies
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

        private void DrawGruntHopping(SpriteEffects flipSprite, Enemy grunt)
        {
            spriteBatch.Draw(
                enemySheet,                    // - The texture to draw
                new Vector2(grunt.location.X, grunt.location.Y),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    frame * GRUNT_RECT_WIDTH,   //   - This rectangle specifies
                    GRUNT_RECT_Y_OFFSET,        //	   where "inside" the texture
                    GRUNT_RECT_WIDTH,           //     to get pixels (We don't want to
                    GRUNT_RECT_HEIGHT),         //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        private void DrawPlayerDMG(SpriteEffects flipSprite)
        {
            spriteBatch.Draw(
                spriteSheet,                    // - The texture to draw
                playerLoc,                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    0,                          //   - This rectangle specifies
                    PLAYER_RECT_Y_OFFSET,        //	   where "inside" the texture
                    PLAYER_RECT_WIDTH,           //     to get pixels (We don't want to
                    PLAYER_RECT_HEIGHT),         //     draw the whole thing)
                Color.Red,                    // - The color
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
                    36),     
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
                    36),
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
                    spriteBatch.Draw(bg, destinationRectangle: new Rectangle(GraphicsDevice.Viewport.Width - p1.currentX + 20, 0, 2000, GraphicsDevice.Viewport.Height));
         
                    for (int i = 2; i < 50; i++) //temporary fix to bg cut off
                    {
                        spriteBatch.Draw(bg, new Rectangle(i * GraphicsDevice.Viewport.Width - p1.currentX, 0, 2000, GraphicsDevice.Viewport.Height), Color.White);

                    }

                    spriteBatch.Draw(floor, new Rectangle((int)camera.Position.X, 450, 861, 30), Color.White);
                    DrawGruntHopping(0, enemy);

                    switch (p1.playerState)
                    {
                        //FACELEFT
                        case PlayerState.FaceLeft:
                            if ((p1.prevPlayerState == PlayerState.FaceRight) || (p1.prevPlayerState == PlayerState.WalkRight))
                            {
                                DrawPlayerStanding(SpriteEffects.FlipHorizontally); //if he was facing or walking right, flip so he faces left
                            }
                            else
                            {
                                DrawPlayerStanding(SpriteEffects.FlipHorizontally); //if he wasnt facing or walking right, draw him standing facing left
                            }
                            break;

                        //WALKLEFT
                        case PlayerState.WalkLeft:
                            DrawPlayerWalking(SpriteEffects.FlipHorizontally);
                            break;

                        //FACERIGHT
                        case PlayerState.FaceRight:
                            if ((p1.prevPlayerState == PlayerState.FaceLeft) || (p1.prevPlayerState == PlayerState.WalkLeft))
                            {
                                DrawPlayerStanding(SpriteEffects.FlipHorizontally);
                            }
                            else
                            {
                                DrawPlayerStanding(0);
                            }
                            break;

                        //WALKRIGHT
                        case PlayerState.WalkRight:
                            DrawPlayerWalking(0);
                            break;
                        
                        //JUMP_LEFT
                        case PlayerState.JumpLeft:
                            DrawPlayerStanding(SpriteEffects.FlipHorizontally);
                            break;
                        //JUMP_RIGHT
                        case PlayerState.JumpRight:
                            DrawPlayerStanding(0);
                            break;
                            
                        default:
                            break;
                            //TAKE_DAMAGE
                            if (p1.isHit == true)
                            {
                                DrawPlayerDMG(0);
                                p1.isHit = false;
                            }
                    }
                    
                    //sword swing animation
                    if (p1.attacking && ( p1.playerState == PlayerState.FaceRight 
                        && (p1.prevPlayerState == PlayerState.FaceLeft) || (p1.prevPlayerState == PlayerState.WalkLeft)))
                    {
                        SwordSwing(SpriteEffects.FlipHorizontally);
                    }
                    else if (p1.attacking && (p1.playerState == PlayerState.FaceLeft
                        && (p1.prevPlayerState == PlayerState.FaceRight) || (p1.prevPlayerState == PlayerState.WalkRight)))
                    {
                        SwordSwing(SpriteEffects.FlipHorizontally);
                    }
                    else if (p1.attacking && (p1.playerState == PlayerState.FaceLeft || p1.playerState == PlayerState.WalkLeft))
                    {
                        SwordSwing(SpriteEffects.FlipHorizontally);
                    }
                    else if (p1.attacking && (p1.playerState == PlayerState.FaceRight || p1.playerState == PlayerState.WalkRight))
                    {
                        SwordSwing(0);
                    }

                    //sword is not swinging, just draw the sword
                    else
                    {
                        if (p1.playerState == PlayerState.FaceLeft || p1.playerState == PlayerState.WalkLeft)
                            DrawSword(SpriteEffects.FlipHorizontally);
                        else
                        {
                            DrawSword(0);
                        }
                    }

                    spriteBatch.Draw(fogSprite, new Rectangle(fog.location.X, fog.location.Y, fog.location.Width, fog.location.Height), Color.White);

                    spriteBatch.Draw(uIBar, new Rectangle((int)camera.Position.X -20, 0, 888, 50), Color.White);

                    //check health state and draw hearts
                    switch (p1.healthState)
                    {
                        case HealthState.ThreeFull:
                            DrawFullHeart(new Vector2(camera.Position.X+20, 5));
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
                    spriteBatch.DrawString(textFont, hiScore.ToString(), new Vector2(camera.Position.X + 720, 40), Color.White); //high score num
                    spriteBatch.DrawString(titleFont, currentScore.ToString(), new Vector2(camera.Position.X + 380, 10), Color.White); //current score num

                    if (startSpawn == true)
                    {
                        DrawWave();
                    }
                    
                    break;

                //GAME PAUSE
                case GameState.GamePause:
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Draw(bg, new Rectangle(-p1.currentX, 0, 2000, GraphicsDevice.Viewport.Height), Color.White);
                    spriteBatch.Draw(bg, destinationRectangle: new Rectangle(GraphicsDevice.Viewport.Width - p1.currentX, 0, 2000, GraphicsDevice.Viewport.Height));
                    for (int i = 2; i < 50; i++) //temporary fix to bg cut off
                    {
                        spriteBatch.Draw(bg, new Rectangle(i * GraphicsDevice.Viewport.Width - p1.currentX, 0, 2000, GraphicsDevice.Viewport.Height), Color.White);

                    }

                    spriteBatch.Draw(p1.sprite, new Rectangle(p1.location.X, p1.location.Y, p1.location.Width, p1.location.Height), Color.White);
                    spriteBatch.Draw(fog.sprite, new Rectangle(fog.location.X, fog.location.Y, fog.location.Width, fog.location.Height), Color.White);

                    spriteBatch.Draw(uIBar, new Rectangle((int)camera.Position.X - 20, 0, 888, 50), Color.White);
                    //spriteBatch.Draw(hearts go here);
                    spriteBatch.DrawString(textFont, "HIGH SCORE", new Vector2(camera.Position.X + 600, 0), Color.White);
                    spriteBatch.DrawString(textFont, hiScore.ToString(), new Vector2(camera.Position.X + 600, 0), Color.White); //high score num
                    spriteBatch.DrawString(titleFont, currentScore.ToString(), new Vector2(camera.Position.X + 380, 0), Color.White); //current score num

                    spriteBatch.Draw(pauseImage, new Vector2(camera.Position.X, 0));
                    pContinue.DrawSelectText(spriteBatch, textFont, "CONTINUE", new Vector2(camera.Position.X + 335, 205));
                    pMenu.DrawSelectText(spriteBatch, textFont, "MAIN MENU", new Vector2(camera.Position.X + 330, 245));
                    break;

                //GAME OVER
                case GameState.GameOver:

                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Draw(gameOverImage, new Vector2(0,0));
                    gRetry.DrawSelectText(spriteBatch, textFont, "RETRY", new Vector2(camera.Position.X + 330, 250));
                    gMenu.DrawSelectText(spriteBatch, textFont, "MAIN MENU", new Vector2(camera.Position.X + 330, 300));
                    spriteBatch.DrawString(textFont, hiScore.ToString(), new Vector2(625, 60), Color.White); //high score num
                    spriteBatch.DrawString(titleFont, currentScore.ToString(), new Vector2(20, 20), Color.White); //current score num

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
            camera.Position = new Vector2(0, 0); //reset camera so it doesnt stay off-centered in other states
            p1.location = new Rectangle(200, 350, 120, 140);
            p1.isDead = false;
            p1.Health = 3;
            currentScore = 0;
            farPoint = 0;
            enemy.location = new Rectangle(600, 360, 100, 100);
            fog.location = new Rectangle(-600, 0, 800, 480);
            fog.bounds = new Rectangle(-500, 0, 300, 700);
            fog.Speed = 1;
        }


    }
}