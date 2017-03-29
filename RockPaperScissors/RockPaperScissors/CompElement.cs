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
    class CompElement
    {
        #region Fields

        //pictures support
        Texture2D spriteBack;
        Texture2D spriteFace;
        Texture2D currentSprite;
        Rectangle drawRectangle;
        Vector2 centre;

        //state of the element
        public const int BACK = 0;
        public const int FACE = 1;
        public int elementState;

        //values of every element
        public const int ROCK = 0;
        public const int SPOCK = 1;
        public const int PAPER = 2;
        public const int LIZARD = 3;
        public const int SCISSORS = 4;
        int value;
        //////////////////////////

        //movement support
        // current position
        int x_position;
        int y_position;

        // initial position and time
        int x_0;
        int y_0;
        float firstVelocity_x;
        float firstVelocity_y;
        float secondVelocity_x;
        float secondVelocity_y;

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
        /// Constructor of the CompElement
        /// </summary>
        /// <param name="content">content manager</param>
        /// <param name="mode">three or five elements</param>
        /// <param name="name">used to load appropriate picture</param>
        /// <param name="newValue">value of the element (paper or rock, etc)</param>
        /// <param name="center">center of the element</param>
        public CompElement(ContentManager content, int mode, string name, int newValue, Vector2 newCentre)
        {
            this.LoadContent(content, name);
            this.Initialize(newCentre);

            this.centre = newCentre;
            this.value = newValue;
            this.gameMode = mode;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the element to check for a button click and move it
        /// </summary>
        public void Update(GameTime gameTime, MouseState mouse)
        {
            ////////////////////////////////////////////////////
            //IF THREE ELEMENTS
            ///////////////////////////////////////////////////
            if (this.gameMode == CompElement.THREE_MODE)
            {
                FirstMode.elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

                // reverse the choise of the computer so player can see it
                if (FirstMode.isTimeForResult && (this.value == FirstMode.compChoise))
                {
                    this.elementState = CompElement.FACE;
                }

                // draw appropriate side of the element (face or back)
                this.correctSide();

                // the main logic of the game (three elements)
                if (FirstMode.levelState == LevelState.WAITING_FOR_COMPUTER)
                {
                    //we are moving all elements to the centre
                    this.firstModeComputerChoise();
                }
                else if (FirstMode.levelState == LevelState.COMPUTER_MOVES)
                {
                    if (this.isChosen)    // we are moving the choise to the centre of the screen 
                    {
                        this.firstModePut2Centre();
                    }
                    else                 //other elements we put on the initial positions
                    {
                        this.firstModeGoHome();
                    }
                }
                this.drawRectangle = new Rectangle(this.x_position - this.currentSprite.Width / 2,
                                                    this.y_position - this.currentSprite.Height / 2,
                                                    this.currentSprite.Width, this.currentSprite.Height);
            }

            ////////////////////////////////////////////////////
            //IF FIVE ELEMENTS  
            ////////////////////////////////////////////////////
            else if (this.gameMode == CompElement.FIVE_MODE)
            {
                // movement of the element to the second destination
                SecondMode.elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

                // reverse the choise of the computer so player can see it
                if (SecondMode.isTimeForResult && (this.value == SecondMode.compChoise))
                {
                    this.elementState = CompElement.FACE;
                }

                // draw appropriate side of the element (face or back)
                this.correctSide();

                // the main logic of the game (five elements)
                if (SecondMode.levelState == LevelState.WAITING_FOR_COMPUTER)
                {
                    this.secondModeComputerChoise();
                }
                else if (SecondMode.levelState == LevelState.COMPUTER_MOVES)
                {
                    if (this.isChosen)    // we are mowing the choise to the centre of the screen 
                    {
                        this.secondModePut2Centre();
                    }
                    else
                    {
                        this.secondModeGoHome();
                    }
                }

                this.drawRectangle = new Rectangle(this.x_position - this.currentSprite.Width / 2,
                                                    this.y_position - this.currentSprite.Height / 2,
                                                    this.currentSprite.Width, this.currentSprite.Height);
            }
        }

        /// <summary>
        /// Draws the element
        /// </summary>
        /// <param name="spriteBatch">the spritebatch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.gameMode == CompElement.THREE_MODE)
            {
                if (FirstMode.levelState != LevelState.RESULTS || (this.isChosen))
                {
                    spriteBatch.Draw(this.currentSprite, this.drawRectangle, Color.White);
                }
            }
            else if (this.gameMode == CompElement.FIVE_MODE)
            {
                if (SecondMode.levelState != LevelState.RESULTS || (this.isChosen))
                {
                    spriteBatch.Draw(this.currentSprite, this.drawRectangle, Color.White);
                }
            }
        }

        //Function that sets the value of chosen element
        public void Choose()
        {
            this.isChosen = true;
            if (this.gameMode == CompElement.THREE_MODE)
            {
                FirstMode.compChoise = this.value;
            }
            else
            {
                SecondMode.compChoise = this.value;
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
            this.elementState = 0;
            this.currentSprite = this.spriteBack;
            FirstMode.elapsedTime = 0;
            SecondMode.elapsedTime = 0;

            this.x_0 = (int)center.X;
            this.y_0 = (int)center.Y;
            this.x_position = this.x_0;
            this.y_position = this.y_0;
            this.firstVelocity_x = (float)(GameConstants.FIRST_DESTINATION_COMP_X - x_0) / 3000;
            this.firstVelocity_y = (float)(GameConstants.FIRST_DESTINATION_COMP_Y - y_0) / 3000;
            this.secondVelocity_x = (float)(GameConstants.SECOND_DESTINATION_COMP_X - GameConstants.FIRST_DESTINATION_COMP_X) / 3000;
            this.secondVelocity_y = (float)(GameConstants.SECOND_DESTINATION_COMP_Y - GameConstants.FIRST_DESTINATION_COMP_Y) / 3000;

            // set initial draw and source rectangles
            this.drawRectangle = new Rectangle(
                (int)(center.X - this.spriteBack.Width / 2),
                (int)(center.Y - this.spriteBack.Height / 2),
                this.spriteBack.Width, this.spriteBack.Height);

        }

        /// <summary>
        /// Load all pictures, sounds and other things for the element
        /// </summary>
        /// <param name="content"> content manager in game </param> 
        /// <param name="name"> name of file with the picture </param> 
        private void LoadContent(ContentManager content, string name)
        {
            this.spriteFace = content.Load<Texture2D>("Elements/" + name);
            this.spriteBack = content.Load<Texture2D>("Elements/CardBack");
            this.sound = content.Load<SoundEffect>("Sounds/ButtonSound");
        }

        //Function that calculates the correct side for the element (back or face)
        private void correctSide()
        {
            if (this.elementState == CompElement.BACK)  // drawing back side
            {
                this.currentSprite = this.spriteBack;
            }
            else                                        // drawing face side
            {
                this.currentSprite = this.spriteFace;
            }

            this.drawRectangle = new Rectangle(this.x_position - this.currentSprite.Width / 2, this.y_position - this.currentSprite.Height / 2,
                                                           this.currentSprite.Width, this.currentSprite.Height);
        }

        //Function that play animation of computer actions in the first mode
        private void firstModeComputerChoise()
        {
            if ((Math.Abs(this.x_position - GameConstants.FIRST_DESTINATION_COMP_X) > 5) ||
                        (Math.Abs(this.y_position - GameConstants.FIRST_DESTINATION_COMP_Y) > 5))
            {
                this.x_position = (int)(this.x_0 + this.firstVelocity_x * FirstMode.elapsedTime * 2);
                this.y_position = (int)(this.y_0 + this.firstVelocity_y * FirstMode.elapsedTime * 2);
            }
            // if we are in the center, but not the central element
            else if (Math.Abs(this.x_0 - GameConstants.FIRST_DESTINATION_COMP_X) > 100)
            {
                this.x_position = GameConstants.FIRST_DESTINATION_COMP_X;
                this.y_position = GameConstants.FIRST_DESTINATION_COMP_Y;
                FirstMode.isTimeToChoose = true; // it is time for computer to make a choise 
                FirstMode.levelState = LevelState.COMPUTER_MOVES;
                FirstMode.elapsedTime = 0;
            }
        }

        //Function that moves chosen element to the centre of the screen in the first mode
        private void firstModePut2Centre()
        {
            if ((Math.Abs(this.x_position - GameConstants.SECOND_DESTINATION_COMP_X) > 3) ||
                            (Math.Abs(this.y_position - GameConstants.SECOND_DESTINATION_COMP_Y) > 3))
            {
                this.x_position = (int)(GameConstants.FIRST_DESTINATION_COMP_X + this.secondVelocity_x * FirstMode.elapsedTime * 1.5);
                this.y_position = (int)(GameConstants.FIRST_DESTINATION_COMP_Y + this.secondVelocity_y * FirstMode.elapsedTime * 1.5);
            }
            else // if the element are there, change the level state so player can make a move
            {
                this.x_position = GameConstants.SECOND_DESTINATION_COMP_X;
                this.y_position = GameConstants.SECOND_DESTINATION_COMP_Y;
                FirstMode.elapsedTime = 0;
                FirstMode.levelState = LevelState.WAITING_FOR_PLAYER;
            }
        }

        //Function that moves element to the given position in the first mode
        private void firstModeGoTo(int destinationX, int destinationY)
        {
            float Velocity_x = (float)(destinationX - GameConstants.FIRST_DESTINATION_COMP_X) / 1000;
            float Velocity_y = (float)(destinationY - GameConstants.FIRST_DESTINATION_COMP_Y) / 1000;

            if ((Math.Abs(this.x_position - destinationX) > 5) || (Math.Abs(this.y_position - destinationY) > 5))
            {
                this.x_position = (int)(GameConstants.FIRST_DESTINATION_COMP_X + Velocity_x * FirstMode.elapsedTime);
                this.y_position = (int)(GameConstants.FIRST_DESTINATION_COMP_Y + Velocity_y * FirstMode.elapsedTime);
            }
            else
            {
                this.x_position = destinationX;
                this.y_position = destinationY;
            }
        }

        //Function that moves element in the first mode if it is not chosen 
        private void firstModeGoHome()
        {
            if (this.value == CompElement.ROCK) //if it is a central element we should know where it have to stay
            {
                if (FirstMode.compChoise == CompElement.PAPER) // if it was chosen the left, take it position 
                {
                    this.firstModeGoTo(GameConstants.START_COMPUTER_PAPER_X, GameConstants.START_COMPUTER_PAPER_Y);
                }
                else //if it was chosen the right hand, take its position 
                {
                    this.firstModeGoTo(GameConstants.START_COMPUTER_SCISSORS_X, GameConstants.START_COMPUTER_SCISSORS_Y);
                }
            }
            else if (this.value == CompElement.SCISSORS) //if it is the right hand element we should know where it have to stay
            {
                this.firstModeGoTo(GameConstants.START_COMPUTER_SCISSORS_X, GameConstants.START_COMPUTER_SCISSORS_Y);
            }
            else if (this.value == CompElement.PAPER) //if it is the left hand element we should know where it have to stay
            {
                this.firstModeGoTo(GameConstants.START_COMPUTER_PAPER_X, GameConstants.START_COMPUTER_PAPER_Y);
            }
        }

        //Function that play animation of computer actions in the second mode
        private void secondModeComputerChoise()
        {
            if ((Math.Abs(this.x_position - GameConstants.FIRST_DESTINATION_COMP_X) > 5) ||
                        (Math.Abs(this.y_position - GameConstants.FIRST_DESTINATION_COMP_Y) > 5))
            {
                this.x_position = (int)(this.x_0 + this.firstVelocity_x * SecondMode.elapsedTime);
                this.y_position = (int)(this.y_0 + this.firstVelocity_y * SecondMode.elapsedTime);
            }
            // if we are in the center, but not the central element and last elements
            else if (Math.Abs(this.x_0 - GameConstants.FIRST_DESTINATION_COMP_X) > 100 &&
                     Math.Abs(this.x_0 - GameConstants.FIRST_DESTINATION_COMP_X) < 300)
            {
                this.x_position = GameConstants.FIRST_DESTINATION_COMP_X;
                this.y_position = GameConstants.FIRST_DESTINATION_COMP_Y;
            }
            else if (Math.Abs(this.x_0 - GameConstants.FIRST_DESTINATION_COMP_X) > 300) // if we are the last element
            {
                SecondMode.isTimeToChoose = true; // it is time for computer to make a choise 
                SecondMode.levelState = LevelState.COMPUTER_MOVES;
                SecondMode.elapsedTime = 0;
            }
        }

        //Function that moves chosen element to the centre of the screen in the second mode
        private void secondModePut2Centre()
        {
            if ((Math.Abs(this.x_position - GameConstants.SECOND_DESTINATION_COMP_X) > 5) ||
                            (Math.Abs(this.y_position - GameConstants.SECOND_DESTINATION_COMP_Y) > 5))
            {
                this.x_position = (int)(GameConstants.FIRST_DESTINATION_COMP_X + this.secondVelocity_x * SecondMode.elapsedTime * 1.5);
                this.y_position = (int)(GameConstants.FIRST_DESTINATION_COMP_Y + this.secondVelocity_y * SecondMode.elapsedTime * 1.5);
            }
            else // if the element are there, change the level state so player can make a move
            {
                this.x_position = GameConstants.SECOND_DESTINATION_COMP_X;
                this.y_position = GameConstants.SECOND_DESTINATION_COMP_Y;
                SecondMode.elapsedTime = 0;
                SecondMode.levelState = LevelState.WAITING_FOR_PLAYER;
            }
        }

        //Function that moves element to the given position in the second mode
        private void secondModeGoTo(int destinationX, int destinationY)
        {
            float Velocity_x = (float)(destinationX - GameConstants.FIRST_DESTINATION_COMP_X) / 2000;
            float Velocity_y = (float)(destinationY - GameConstants.FIRST_DESTINATION_COMP_Y) / 2000;

            if ((Math.Abs(this.x_position - destinationX) > 5) || (Math.Abs(this.y_position - destinationY) > 5))
            {
                this.x_position = (int)(GameConstants.FIRST_DESTINATION_COMP_X + Velocity_x * SecondMode.elapsedTime);
                this.y_position = (int)(GameConstants.FIRST_DESTINATION_COMP_Y + Velocity_y * SecondMode.elapsedTime);
            }
            else
            {
                this.x_position = destinationX;
                this.y_position = destinationY;
            }
        }

        //Function that moves element in the second mode if it is not chosen 
        private void secondModeGoHome()
        {
            if (this.value == CompElement.ROCK) //if it is a central element we should know where it have to stay
            {
                if (SecondMode.compChoise == CompElement.SPOCK) // take it position 
                {
                    this.secondModeGoTo(GameConstants.START_COMPUTER_SPOCK_X, GameConstants.START_COMPUTER_SPOCK_Y);
                }
                else if (SecondMode.compChoise == CompElement.PAPER) // take its position 
                {
                    this.secondModeGoTo(GameConstants.START_COMPUTER_PAPER_X, GameConstants.START_COMPUTER_PAPER_Y);
                }
                else if (SecondMode.compChoise == CompElement.LIZARD) // take its position 
                {
                    this.secondModeGoTo(GameConstants.START_COMPUTER_LIZARD_X, GameConstants.START_COMPUTER_LIZARD_Y);
                }
                else if (SecondMode.compChoise == CompElement.SCISSORS) // take its position 
                {
                    this.secondModeGoTo(GameConstants.START_COMPUTER_SCISSORS_X, GameConstants.START_COMPUTER_SCISSORS_Y);
                }
            }
            else if (this.value == CompElement.SPOCK) // we should know where it have to stay
            {
                this.secondModeGoTo(GameConstants.START_COMPUTER_SPOCK_X, GameConstants.START_COMPUTER_SPOCK_Y);
            }
            else if (this.value == CompElement.PAPER) // we should know where it have to stay
            {
                this.secondModeGoTo(GameConstants.START_COMPUTER_PAPER_X, GameConstants.START_COMPUTER_PAPER_Y);
            }
            else if (this.value == CompElement.LIZARD) // we should know where it have to stay
            {
                this.secondModeGoTo(GameConstants.START_COMPUTER_LIZARD_X, GameConstants.START_COMPUTER_LIZARD_Y);
            }
            else if (this.value == CompElement.SCISSORS) // we should know where it have to stay
            {
                this.secondModeGoTo(GameConstants.START_COMPUTER_SCISSORS_X, GameConstants.START_COMPUTER_SCISSORS_Y);
            }
        }


        #endregion
    }
}
