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
    class Element
    {
        #region Fields

        //animated pictures support
        Texture2D sprite;
        Rectangle destRectangle;
        Rectangle sourceRectangle;
        int buttonWidth;
        int buttonHeight;
        int buttonState;

        //values of every element
        public const int ROCK = 0;
        public const int SPOCK = 1;
        public const int PAPER = 2;
        public const int LIZARD = 3;
        public const int SCISSORS = 4;
        int value;
        /////////////////////////////////

        //click support
        bool clickStarted = false;

        //movement support
        // current position
        int x_position;
        int y_position;

        // initial position and time
        int x_0;
        int y_0;
        float velocity_x;
        float velocity_y;
        int elapsedTime = 0;

        bool isChosen = false;

        //sound support
        SoundEffect sound;

        //differentiator of the modes
        public const int THREE_MODE = 3;
        public const int FIVE_MODE = 5;
        int gameMode;
        //////////////////////////////////

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor of the element
        /// </summary>
        /// <param name="content">content manager</param>
        /// <param name="mode">three or five elements</param>
        /// <param name="name">used to load appropriate picture</param>
        /// <param name="newValue">value of the element (paper or rock, etc)</param>
        /// <param name="center">center of the element</param>
        public Element(ContentManager content, int mode, string name, int newValue, Vector2 center)
        {
            this.LoadContent(content, name);
            this.Initialize(center);

            this.gameMode = mode;
            this.value = newValue;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the button to check for a button click
        /// </summary>
        public void Update(GameTime gameTime, MouseState mouse)
        {

            if (FirstMode.levelState == LevelState.WAITING_FOR_PLAYER
                || SecondMode.levelState == LevelState.WAITING_FOR_PLAYER) // it is a stage, where player schooses an element
            {
                this.playerChoise(mouse, this.gameMode);   //make a choise of player
            }
            else if (this.isChosen && (FirstMode.levelState == LevelState.PLAYER_MOVES
                                        || SecondMode.levelState == LevelState.PLAYER_MOVES))   // moving the chosen element 
            {                                                                                   // to the center of the screen 
                this.buttonState = 1;

                // movement of the element to the destination
                this.moveToken(gameTime, this.gameMode);
            }

            this.sourceRectangle = new Rectangle(this.buttonWidth * this.buttonState, 0, this.buttonWidth, this.buttonHeight);

        }

        /// <summary>
        /// Draws the button
        /// </summary>
        /// <param name="spriteBatch">the spritebatch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.gameMode == Element.THREE_MODE) // if it is a game of only three elements
            {
                if (FirstMode.levelState != LevelState.RESULTS || (this.isChosen)) //in the end only chosen elements left
                {
                    spriteBatch.Draw(this.sprite, this.destRectangle, this.sourceRectangle, Color.White);
                }
            }
            else if (this.gameMode == Element.FIVE_MODE) // if it is a game of five elements
            {
                if (SecondMode.levelState != LevelState.RESULTS || (this.isChosen)) //in the end only chosen elements left
                {
                    spriteBatch.Draw(this.sprite, this.destRectangle, this.sourceRectangle, Color.White);
                }
            }
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
            this.buttonWidth = this.sprite.Width / 3;
            this.buttonHeight = this.sprite.Height;

            //calculates button states
            this.buttonState = 0;
            this.isChosen = false;

            //movement support            
            this.x_0 = (int)center.X;
            this.y_0 = (int)center.Y;
            this.x_position = this.x_0;
            this.y_position = this.y_0;
            this.velocity_x = (float)(GameConstants.DESTINATION_PLAYER_X - x_0) / 500;
            this.velocity_y = (float)(GameConstants.DESTINATION_PLAYER_Y - y_0) / 500;
            this.elapsedTime = 0;

            // set initial draw and source rectangles
            this.destRectangle = new Rectangle(
                (int)(center.X - this.buttonWidth / 2),
                (int)(center.Y - this.buttonHeight / 2),
                this.buttonWidth, this.buttonHeight);
            sourceRectangle = new Rectangle(0, 0, this.buttonWidth, this.buttonHeight);
        }

        /// <summary>
        /// load all pictures, sounds and other things for button
        /// </summary>
        /// <param name="content"> content manager in game </param> 
        /// <param name="name"> name of file with the picture </param> 
        private void LoadContent(ContentManager content, string name)
        {
            this.sprite = content.Load<Texture2D>("Elements/" + name);
            this.sound = content.Load<SoundEffect>("Sounds/ButtonSound");
        }


        /// <summary>
        /// This function updates element until player will make a choise
        /// </summary>
        private void playerChoise(MouseState mouse, int mode)
        {
            // check for mouse over the element
            if (this.destRectangle.Contains(mouse.X, mouse.Y))
            {
                // highlight element
                this.buttonState = 1;

                // check for click started on element
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    this.clickStarted = true;
                    this.buttonState = 2;
                }
                else if (mouse.LeftButton == ButtonState.Released)
                {
                    this.buttonState = 1;
                    // if click finished on element, change level state
                    if (this.clickStarted)
                    {
                        this.clickStarted = false;
                        if (mode == Element.THREE_MODE)
                        {
                            FirstMode.levelState = LevelState.PLAYER_MOVES;
                        }
                        else if (mode == Element.FIVE_MODE)
                        {
                            SecondMode.levelState = LevelState.PLAYER_MOVES;
                        }

                        // save the player choise
                        this.isChosen = true;
                        if (mode == Element.THREE_MODE)
                        {
                            FirstMode.playerCoise = this.value;
                        }
                        else if (mode == Element.FIVE_MODE)
                        {
                            SecondMode.playerCoise = this.value;
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
        }

        /// <summary>
        /// This function moves chosen token to the center of the screen and changes level state
        /// </summary>
        private void moveToken(GameTime gameTime, int mode)
        {
            this.elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if ((Math.Abs(this.x_position - GameConstants.DESTINATION_PLAYER_X) > 5) ||
                    (Math.Abs(this.y_position - GameConstants.DESTINATION_PLAYER_Y) > 5))
            {
                this.x_position = (int)(this.x_0 + this.velocity_x * this.elapsedTime);
                this.y_position = (int)(this.y_0 + this.velocity_y * this.elapsedTime);
                this.destRectangle = new Rectangle(this.x_position - this.sprite.Width / 6, this.y_position - this.sprite.Height / 2,
                                                        this.sprite.Width / 3, this.sprite.Height);
            }
            else //if the element is already in the center, change state
            {
                this.x_position = GameConstants.DESTINATION_PLAYER_X;
                this.y_position = GameConstants.DESTINATION_PLAYER_Y;

                if (mode == Element.THREE_MODE)
                {
                    FirstMode.levelState = 5;
                    FirstMode.isTimeForResult = true; // it is time to culculate the results
                }
                else if (mode == Element.FIVE_MODE)
                {
                    SecondMode.levelState = 5;
                    SecondMode.isTimeForResult = true; // it is time to culculate the results
                }
            }
        }

        #endregion
    }
}
