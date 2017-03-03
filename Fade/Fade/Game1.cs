using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        Texture2D controlsImage;
        Texture2D player;
        Texture2D playerSprite; //will be replaced by a spritesheet
        Texture2D fogSprite;
        Texture2D bg;

        //OBJECTS
        Player p1;
        Fog fog;
        Camera2D camera;

        //ENUMS
        GameState currentState = GameState.Menu;

        //KBSTATES
        KeyboardState ks;
        KeyboardState previousState = Keyboard.GetState();

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

            base.Initialize();
        }

        //LOAD ///////////////////////////////////////////
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //textures
            playerSprite = Content.Load<Texture2D>("char1sword");
            fogSprite= Content.Load<Texture2D>("fogfull");
            bg = Content.Load<Texture2D>("background");
            mainMenuImage = Content.Load<Texture2D>("menuprocess");

            //type
            textFont = Content.Load<SpriteFont>("textFont");
            titleFont = Content.Load<SpriteFont>("titleFont");

            //objects
            p1 = new Player(playerSprite,new Rectangle(GraphicsDevice.Viewport.Width/2,300,120,140));
            fog = new Fog(fogSprite, new Rectangle(-300, 40, 400, 400), 1, 0);
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

            previousState = ks;

            //MOVEMENT
            p1.Run(gameTime);
            fog.Move();

            //CAMERA
            if(ks.IsKeyDown(Keys.D))
            {
                camera.Position += new Vector2(250, 0) * deltaTime/2;
            }
            if (ks.IsKeyDown(Keys.A))
            {
                camera.Position -= new Vector2(250, 0) * deltaTime / 2;
            }
            

            base.Update(gameTime);
        }

        //DRAW ///////////////////////////////////////////
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var viewMatrix = camera.GetViewMatrix();
            spriteBatch.Begin(transformMatrix: viewMatrix);

            switch (currentState)
            {
                case GameState.Menu:
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Draw(mainMenuImage, new Vector2(0, 0));
                    spriteBatch.DrawString(textFont, "START", new Vector2(350, 250), Color.White);
                    spriteBatch.DrawString(textFont, "CONTROLS", new Vector2(350, 300), Color.White);
                    spriteBatch.DrawString(textFont, "QUIT", new Vector2(350, 350), Color.White);

                    break;

                case GameState.Controls:
                    GraphicsDevice.Clear(Color.Black);
                    //spriteBatch.Draw(controlsImage, new Vector2(0, 0));
                    break;

                case GameState.Game:
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
                    spriteBatch.DrawString(textFont, "HIGH SCORE", new Vector2(camera.Position.X + 600, 20), Color.White);
                    spriteBatch.DrawString(titleFont, "0", new Vector2(camera.Position.X + 380, 20), Color.White);
                    break;

                case GameState.GamePause:

                    break;

                case GameState.GameOver:

                    break;

                default:
                    break;
            }

                    spriteBatch.End();

                    base.Draw(gameTime);
            

        }

        //METHODS
        //SingleKeyPress. check if key is pressed
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

    }
}

