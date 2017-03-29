using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Buttons support for all game
    /// </summary>
    class Button
    {
        #region Fields

        //pictures support
        Texture2D sprite;

        //animation support
        Rectangle destRectangle;
        Rectangle sourceRectangle;
        int buttonWidth;
        int buttonHeight;
        int buttonState;

        //click support
        bool clickStarted = false;

        //sound support
        SoundEffect sound;


        #endregion

        #region Constructors

        /// <summary>
        /// Constreuctor of the button
        /// </summary>
        /// <param name="content">Current ContentManager object of game</param>
        /// <param name="name">Name of the picture of the button in Content/Buttons folder</param>
        /// <param name="center">Center of the button</param>
        public Button(ContentManager content, string name, Vector2 center)
        {
            this.LoadContent(content, name);
            this.Initialize(center);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the button to check for a button click
        /// </summary>
        /// <param name="gamepad">the current mouse state</param>
        /// /// <param name="state">ation of the button</param>
        public void Update(MouseState mouse, int state)
        {
            // check for mouse over button
            if (this.destRectangle.Contains(mouse.X, mouse.Y))
            {
                // highlight button
                this.buttonState = 1;

                // check for click started on button
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    this.clickStarted = true;
                    this.buttonState = 2;
                }
                else if (mouse.LeftButton == ButtonState.Released)
                {
                    this.buttonState = 1;
                    // if click finished on button, change game state
                    if (this.clickStarted)
                    {
                        this.clickStarted = false;

                        // differentiation of the game states and buttons of the About Window
                        if (state == GameState.MAIN_MENU
                            || state == GameState.THREE_OBJECTS
                            || state == GameState.FIVE_OBJECTS
                            || state == GameState.ABOUT_WINDOW
                            || state == GameState.EXIT_GAME)
                        {
                            Game1.gameStage = state;
                        }
                        //if it is a buttons from the AboutWindow
                        else if (state == GameConstants.ABOUT_THREE_OBJECTS)
                        {
                            AboutWindow.aboutThreeElements = true;
                        }
                        else if (state == GameConstants.ABOUT_FIVE_OBJECTS)
                        {
                            AboutWindow.aboutThreeElements = false;
                        }
                        //if it is time to change the level states
                        else if (state == GameConstants.NEW_THREE_OBJECTS_GAME)
                        {
                            FirstMode.levelState = LevelState.RELOAD_LEVEL;
                        }
                        else if (state == GameConstants.NEW_FIVE_OBJECTS_GAME)
                        {
                            SecondMode.levelState = LevelState.RELOAD_LEVEL;
                        }

                        this.sound.Play(0.1f, 0.0f, 0.0f);
                    }
                }
            }
            else
            {
                this.buttonState = 0;

                // no clicking on this button
                this.clickStarted = false;
            }
            this.sourceRectangle = new Rectangle(0, this.buttonHeight * this.buttonState, 
                                                    this.buttonWidth, this.buttonHeight);

        }

        /// <summary>
        /// Draws the button
        /// </summary>
        /// <param name="spriteBatch">the spritebatch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.sprite, this.destRectangle, this.sourceRectangle, Color.White);
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the button characteristics
        /// </summary>
        /// <param name="center">the center of the button</param>
        private void Initialize(Vector2 center)
        {
            // calculate button width
            this.buttonWidth = sprite.Width;
            this.buttonHeight = sprite.Height / 3;
            this.buttonState = 0;


            // set initial draw and source rectangles
            this.destRectangle = new Rectangle(
                (int)(center.X - this.buttonWidth / 2),
                (int)(center.Y - this.buttonHeight / 2),
                buttonWidth, buttonHeight);
            this.sourceRectangle = new Rectangle(0, 0, this.buttonWidth, this.buttonHeight);
        }

        /// <summary>
        /// load all pictures, sounds and other things for button
        /// </summary>
        /// <param name="content"> content manager in game </param> 
        /// <param name="name"> name of file with the picture </param> 
        private void LoadContent(ContentManager content, string name)
        {
            this.sprite = content.Load<Texture2D>("Buttons/" + name);
            this.sound = content.Load<SoundEffect>("Sounds/ButtonSound");
        }

        #endregion

    }
}
