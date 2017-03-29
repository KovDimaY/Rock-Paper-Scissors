using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RockPaperScissors
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //declare all windows that is used in the game
        MainMenu mainMenuWindow;
        FirstMode threeElementsMode;
        SecondMode fiveElementsMode;
        AboutWindow aboutWindow;

        //flag for optimization 
        private bool isReloaded = false;

        ///////////////////////////////////////
        //Game States:
        // 0 - main menu
        // 1 - tree objects
        // 2 - five objects
        // 3 - about
        // 4 - exit game
        public static int gameStage = GameState.MAIN_MENU;
        ///////////////////////////////////////


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = GameConstants.WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = GameConstants.WINDOW_HEIGHT;

            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            mainMenuWindow = new MainMenu(Content);
            threeElementsMode = new FirstMode(Content);
            fiveElementsMode = new SecondMode(Content);
            aboutWindow = new AboutWindow(Content);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                (gameStage == GameState.EXIT_GAME))
                this.Exit();

            MouseState mouse = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();

            //Exit to Main Menu
            if (keyboard.IsKeyDown(Keys.Escape))
            {
                gameStage = GameState.MAIN_MENU;
            }

            //Management of all the game states
            if (gameStage == GameState.MAIN_MENU)
            {
                //resolve the issue with music playing and new games
                if (!this.isReloaded) //to make it only once, not every game tick
                {
                    threeElementsMode.Update(gameTime, Content, mouse);
                    fiveElementsMode.Update(gameTime, Content, mouse);
                    threeElementsMode = new FirstMode(Content);
                    fiveElementsMode = new SecondMode(Content);
                    this.isReloaded = true;
                }

                //begin updating the main menu window
                mainMenuWindow.Update(mouse);
            }
            else if (gameStage == GameState.THREE_OBJECTS)
            {
                threeElementsMode.Update(gameTime, Content, mouse);
                this.isReloaded = false;
            }
            else if (gameStage == GameState.FIVE_OBJECTS)
            {
                fiveElementsMode.Update(gameTime, Content, mouse);
                this.isReloaded = false;
            }
            else if (gameStage == GameState.ABOUT_WINDOW)
            {
                aboutWindow.Update(mouse);
                this.isReloaded = false;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            if (gameStage == GameState.MAIN_MENU)
            {
                mainMenuWindow.Draw(spriteBatch);
            }
            else if (gameStage == GameState.THREE_OBJECTS)
            {
                threeElementsMode.Draw(spriteBatch);
            }
            else if (gameStage == GameState.FIVE_OBJECTS)
            {
                fiveElementsMode.Draw(spriteBatch);
            }
            else if (gameStage == GameState.ABOUT_WINDOW)
            {
                aboutWindow.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
