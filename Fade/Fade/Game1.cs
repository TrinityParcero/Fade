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
        bool isSelected;  //used for menus where...
        Color typeColor; //text color changes when selected

        //SOUND
        //Song backgroundMusic;
        //SoundEffect hitSound;
        //we will uncomment these when we're ready to use them

        //IMAGES
        Texture2D mainMenuImage;
        Texture2D pauseImage;
        Texture2D controlsImage;
        Texture2D player;
        Texture2D playerSprite; //will be replaced by a spritesheet
        Texture2D fogSprite;
        Texture2D bg;
        Texture2D spriteSheet;

        //OBJECTS
        Player p1;
        Fog fog;
        Camera2D camera;

        //ENUMS
        GameState currentState = GameState.Menu;

        //KBSTATES
        KeyboardState ks;
        KeyboardState previousState = Keyboard.GetState();

        //ANIMATION
        int frame;              // The current animation frame
        double timeCounter;     // The amount of time that has passed
        double fps;             // The speed of the animation
        double timePerFrame;    // The amount of time (in fractional seconds) per frame
        Vector2 playerLoc;

        // Constants for "source" rectangle (inside the image)
        const int WALK_FRAME_COUNT = 6;         // The number of frames in the animation
        const int PLAYER_RECT_Y_OFFSET = 0;    // How far down in the image are the frames?
        const int PLAYER_RECT_HEIGHT = 140;       // The height of a single frame
        const int PLAYER_RECT_WIDTH = 135;        // The width of a single frame


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
            base.Initialize();
        }

        //LOAD ///////////////////////////////////////////
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //textures
            playerSprite = Content.Load<Texture2D>("char1sword");
            fogSprite = Content.Load<Texture2D>("fogfull");
            bg = Content.Load<Texture2D>("background");
            mainMenuImage = Content.Load<Texture2D>("menuprocess");
            pauseImage = Content.Load<Texture2D>("pausebg");
            spriteSheet = Content.Load<Texture2D>("charsprite");

            //type
            textFont = Content.Load<SpriteFont>("textFont");
            titleFont = Content.Load<SpriteFont>("titleFont");

            //objects
            p1 = new Player(playerSprite, new Rectangle(GraphicsDevice.Viewport.Width / 2, 300, 120, 140));
            
            fog = new Fog(fogSprite, new Rectangle(-400, 80, 400, 400), 1, 0);
        }

        //UNLOAD /////////////////////////////////////////
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        // UPDATE ////////////////////////////////////////
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            ks = Keyboard.GetState();
            //position.X += 1;

            //GAMESTATE CHANGES
            if (currentState == GameState.Menu && SingleKeyPress(Keys.Enter))
            {
                currentState = GameState.Game;
            }
            if (currentState == GameState.Game && SingleKeyPress(Keys.P))
            {
                currentState = GameState.GamePause;
            }
            if (currentState == GameState.Game && (p1.isDead || SingleKeyPress(Keys.B)))  //B key is temp for testing til we have damage going
            {
                ResetGame();
                currentState = GameState.GameOver;
            }
            if (currentState == GameState.GamePause && SingleKeyPress(Keys.Enter))
            {
                currentState = GameState.Game;
            }
            if (currentState == GameState.GameOver && SingleKeyPress(Keys.Enter))
            {
                currentState = GameState.Menu;
            }

            previousState = ks;

            //GAMEPLAY
            if (currentState == GameState.Game)
            {
                //MOVEMENT
                p1.Run(fog.location);
                playerLoc.X = p1.location.X;
                playerLoc.Y = p1.location.Y;
                //checks to see if spacebar is pressed
                if (ks.IsKeyDown(Keys.Space))
                {
                    //below method sets player class bool jumping = true
                     p1.Jump();
                   

                }
                //if the bool jumping frmo the player class is true then the below method will move the player
                p1.JumpUpdate();
                fog.Move();

                //Handle animation timing
                // - Add to the time counter
                // - Check if we have enough "time" to advance the frame
                timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
                if (timeCounter >= timePerFrame)
                {
                    frame += 1;                     // Adjust the frame

                    if (frame > WALK_FRAME_COUNT)   // Check the bounds
                        frame = 1;                  // Back to 1 (since 0 is the "standing" frame)

                    timeCounter -= timePerFrame;    // Remove the time we "used"
                }

                
                //CAMERA
                if (ks.IsKeyDown(Keys.D))
                {
                    camera.Position += new Vector2(250, 0) * deltaTime / 2;
                }
                if (ks.IsKeyDown(Keys.A))
                {
                    if(camera.Position == new Vector2(0,0))
                    {

                    }
                    else
                    {
                        camera.Position -= new Vector2(150, 0) * deltaTime / 2;
                    }
                }

            }

            
            base.Update(gameTime);
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
                Color.White,                    // - The color
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
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
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
                    spriteBatch.Draw(mainMenuImage, new Vector2(0, 0));
                    spriteBatch.DrawString(textFont, "START", new Vector2(350, 250), Color.White);
                    spriteBatch.DrawString(textFont, "CONTROLS", new Vector2(350, 300), Color.White);
                    spriteBatch.DrawString(textFont, "QUIT", new Vector2(350, 350), Color.White);

                    break;

                //CONTROLS
                case GameState.Controls:
                    GraphicsDevice.Clear(Color.Black);
                    //spriteBatch.Draw(controlsImage, new Vector2(0, 0));
                    break;

                //GAMEPLAY
                case GameState.Game:
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Draw(bg, new Rectangle(-p1.currentX, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    spriteBatch.Draw(bg, destinationRectangle: new Rectangle(GraphicsDevice.Viewport.Width - p1.currentX, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), effects: SpriteEffects.FlipHorizontally);
                    for (int i = 2; i < 50; i++) //temporary fix to bg cut off
                    {
                        spriteBatch.Draw(bg, new Rectangle(i * GraphicsDevice.Viewport.Width - p1.currentX, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

                    }

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
                        
                        //JUMP
                        case PlayerState.Jump:
                            DrawPlayerStanding(0);
                            break;
                            
                        default:
                            break;
                    }
                    
                    spriteBatch.Draw(fog.sprite, new Rectangle(fog.location.X, fog.location.Y, fog.location.Width, fog.location.Height), Color.White);

                    //spriteBatch.Draw(UI bar goes here);
                    //spriteBatch.Draw(hearts go here);
                    spriteBatch.DrawString(textFont, "HIGH SCORE", new Vector2(camera.Position.X + 600, 20), Color.White);
                    spriteBatch.DrawString(textFont, "0", new Vector2(camera.Position.X + 600, 40), Color.White); //high score num
                    spriteBatch.DrawString(titleFont, "0", new Vector2(camera.Position.X + 380, 20), Color.White); //current score num
                    break;

                //GAME PAUSE
                case GameState.GamePause:
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Draw(bg, new Rectangle(-p1.currentX, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    spriteBatch.Draw(bg, destinationRectangle: new Rectangle(GraphicsDevice.Viewport.Width - p1.currentX, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), effects: SpriteEffects.FlipHorizontally);
                    for (int i = 2; i < 50; i++) //temporary fix to bg cut off
                    {
                        spriteBatch.Draw(bg, new Rectangle(i * GraphicsDevice.Viewport.Width - p1.currentX, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

                    }

                    spriteBatch.Draw(p1.sprite, new Rectangle(p1.location.X, p1.location.Y, p1.location.Width, p1.location.Height), Color.White);
                    spriteBatch.Draw(fog.sprite, new Rectangle(fog.location.X, fog.location.Y, fog.location.Width, fog.location.Height), Color.White);

                    //spriteBatch.Draw(UI bar goes here);
                    //spriteBatch.Draw(hearts go here);
                    spriteBatch.DrawString(textFont, "HIGH SCORE", new Vector2(camera.Position.X + 600, 20), Color.White);
                    spriteBatch.DrawString(textFont, "0", new Vector2(camera.Position.X + 600, 40), Color.White); //high score num
                    spriteBatch.DrawString(titleFont, "0", new Vector2(camera.Position.X + 380, 20), Color.White); //current score num

                    spriteBatch.Draw(pauseImage, new Vector2(camera.Position.X, 0));
                    spriteBatch.DrawString(textFont, "CONTINUE", new Vector2(camera.Position.X + 335, 205), Color.Black);
                    spriteBatch.DrawString(textFont, "MAIN MENU", new Vector2(camera.Position.X + 330, 245), Color.Black);
                    break;

                //GAME OVER
                case GameState.GameOver:

                    GraphicsDevice.Clear(Color.Black);
                    //spriteBatch.Draw(gameOverImage);
                    spriteBatch.DrawString(textFont, "RETRY", new Vector2(350, 250), Color.White);
                    spriteBatch.DrawString(textFont, "MAIN MENU", new Vector2(350, 300), Color.White);
                    spriteBatch.DrawString(textFont, "HIGH SCORE", new Vector2(625, 20), Color.White);
                    spriteBatch.DrawString(textFont, "0", new Vector2(625, 60), Color.White); //high score num
                    spriteBatch.DrawString(titleFont, "0", new Vector2(20, 20), Color.White); //current score num

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
            p1.location = new Rectangle(GraphicsDevice.Viewport.Width / 2, 300, 120, 140);
            fog.location = new Rectangle(-300, 80, 400, 400);
        }


    }
}