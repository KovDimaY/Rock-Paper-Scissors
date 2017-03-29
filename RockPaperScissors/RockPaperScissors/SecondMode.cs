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
    class SecondMode
    {
        #region Fields

        //graphics support
        Texture2D backgroundPicture;
        Rectangle backDrawRectandle;

        //sound support
        SoundEffectInstance backgroundMusic;
        SoundEffect winSound;
        SoundEffect loseSound;
        SoundEffect drawSound;


        //texting support and score
        int playerScore = 0;
        int computerScore = 0;
        Vector2 playerScorePosition;
        Vector2 computerScorePosition;
        SpriteFont scoreFont;
        SpriteFont resultFont;
        String message = "";
        Vector2 messagePosition;
        Color color = Color.Lime;


        //buttons to Exit and Return
        Button returnButton;
        Button exitGameButton;
        Button nextButton;

        //elements of the players
        Element playerRock;
        Element playerPaper;
        Element playerScissors;
        Element playerSpock;
        Element playerLizard;

        //elements of the computer
        CompElement compRock;
        CompElement compPaper;
        CompElement compScissors;
        CompElement compSpock;
        CompElement compLizard;
        Random myRand = new Random();


        /////////////////////////////
        //Level states
        // 1 - waiting for computer
        // 2 - computer moves
        // 3 - waiting for player
        // 4 - player moves
        // 5 - results
        // 6 - reload level
        public static int levelState = LevelState.WAITING_FOR_COMPUTER;
        public static int playerCoise;
        public static int compChoise;
        public static int elapsedTime;
        public static bool isTimeToChoose = false;
        public static bool isTimeForResult = false;

        bool isPlaying = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for the single player mode of the game
        /// </summary>
        /// <param name="content">Content manager of the game</param>
        public SecondMode(ContentManager content)
        {
            this.LoadContent(content);
            levelState = LevelState.WAITING_FOR_COMPUTER;
            isTimeToChoose = false;

            //messagePosition of the score of players on the screen
            playerScorePosition = new Vector2(GameConstants.SCORE_POSITION_PLAYER_X, GameConstants.SCORE_POSITION_PLAYER_Y);
            computerScorePosition = new Vector2(GameConstants.SCORE_POSITION_COMPUTER_X, GameConstants.SCORE_POSITION_COMPUTER_Y);

            //backGround
            this.backDrawRectandle = new Rectangle(-5, -20, backgroundPicture.Width, backgroundPicture.Height);

            //buttons
            this.returnButton = new Button(content, "Button_8", new Vector2(GameConstants.BUTTON_8_POSITION_X, GameConstants.BUTTON_8_POSITION_Y));
            this.exitGameButton = new Button(content, "Button_9", new Vector2(GameConstants.BUTTON_9_POSITION_X, GameConstants.BUTTON_9_POSITION_Y));
            this.nextButton = new Button(content, "Button_10", new Vector2(GameConstants.BUTTON_10_POSITION_X, GameConstants.BUTTON_10_POSITION_Y));

            //elements of the player
            this.playerRock = new Element(content, Element.FIVE_MODE, "Rock3", Element.ROCK,
                                            new Vector2(GameConstants.START_PLAYER5_ROCK_X, GameConstants.START_PLAYER5_ROCK_Y));
            this.playerSpock = new Element(content, Element.FIVE_MODE, "Spock3", Element.SPOCK,
                                            new Vector2(GameConstants.START_PLAYER5_SPOCK_X, GameConstants.START_PLAYER5_SPOCK_Y));
            this.playerPaper = new Element(content, Element.FIVE_MODE, "Paper3", Element.PAPER,
                                            new Vector2(GameConstants.START_PLAYER5_PAPER_X, GameConstants.START_PLAYER5_PAPER_Y));
            this.playerLizard = new Element(content, Element.FIVE_MODE, "Lizard3", Element.LIZARD,
                                            new Vector2(GameConstants.START_PLAYER5_LIZARD_X, GameConstants.START_PLAYER5_LIZARD_Y));
            this.playerScissors = new Element(content, Element.FIVE_MODE, "Scissors3", Element.SCISSORS,
                                            new Vector2(GameConstants.START_PLAYER5_SCISSORS_X, GameConstants.START_PLAYER5_SCISSORS_Y));

            //elements of the computer
            this.compRock = new CompElement(content, CompElement.FIVE_MODE, "Rock2", CompElement.ROCK,
                                            new Vector2(GameConstants.START_COMPUTER_ROCK_X, GameConstants.START_COMPUTER_ROCK_Y));
            this.compSpock = new CompElement(content, CompElement.FIVE_MODE, "Spock2", CompElement.SPOCK,
                                            new Vector2(GameConstants.START_COMPUTER_SPOCK_X, GameConstants.START_COMPUTER_SPOCK_Y));
            this.compPaper = new CompElement(content, CompElement.FIVE_MODE, "Paper2", CompElement.PAPER,
                                            new Vector2(GameConstants.START_COMPUTER_PAPER_X, GameConstants.START_COMPUTER_PAPER_Y));
            this.compLizard = new CompElement(content, CompElement.FIVE_MODE, "Lizard2", CompElement.LIZARD,
                                            new Vector2(GameConstants.START_COMPUTER_LIZARD_X, GameConstants.START_COMPUTER_LIZARD_Y));
            this.compScissors = new CompElement(content, CompElement.FIVE_MODE, "Scissors2", CompElement.SCISSORS,
                                            new Vector2(GameConstants.START_COMPUTER_SCISSORS_X, GameConstants.START_COMPUTER_SCISSORS_Y));

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updating single player mode
        /// </summary>
        /// <param name="gameTime">Game time manager</param>
        /// <param name="content">Content manager</param>
        /// <param name="keyboard">KeyBoard manager</param>
        public void Update(GameTime gameTime, ContentManager content, MouseState mouse)
        {

            // updating button, giving game stage needed to the main menu
            this.returnButton.Update(mouse, GameState.MAIN_MENU);
            this.exitGameButton.Update(mouse, GameState.EXIT_GAME);
            if (SecondMode.levelState == LevelState.RESULTS)   // this button reloads the level 
            {
                this.nextButton.Update(mouse, GameConstants.NEW_FIVE_OBJECTS_GAME);
            }

            //updating player elements
            this.playerRock.Update(gameTime, mouse);
            this.playerPaper.Update(gameTime, mouse);
            this.playerScissors.Update(gameTime, mouse);
            this.playerSpock.Update(gameTime, mouse);
            this.playerLizard.Update(gameTime, mouse);

            //updating computer elements
            this.compRock.Update(gameTime, mouse);
            this.compPaper.Update(gameTime, mouse);
            this.compScissors.Update(gameTime, mouse);
            this.compSpock.Update(gameTime, mouse);
            this.compLizard.Update(gameTime, mouse);

            // computer choise is calculated here when CompElement will say
            if (SecondMode.isTimeToChoose)
            {
                this.Choise();
                SecondMode.isTimeToChoose = false;
            }

            // computer choise is calculated here when Element will say
            if (SecondMode.isTimeForResult)
            {
                SecondMode.isTimeForResult = false;

                //the main logic is here
                this.computeResults();
            }

            //this code reloads level
            if (SecondMode.levelState == LevelState.RELOAD_LEVEL)
            {
                SecondMode.isTimeToChoose = false;
                SecondMode.isTimeForResult = false;

                //elements of the player
                this.playerRock = new Element(content, Element.FIVE_MODE, "Rock3", Element.ROCK,
                                                new Vector2(GameConstants.START_PLAYER5_ROCK_X, GameConstants.START_PLAYER5_ROCK_Y));
                this.playerSpock = new Element(content, Element.FIVE_MODE, "Spock3", Element.SPOCK,
                                                new Vector2(GameConstants.START_PLAYER5_SPOCK_X, GameConstants.START_PLAYER5_SPOCK_Y));
                this.playerPaper = new Element(content, Element.FIVE_MODE, "Paper3", Element.PAPER,
                                                new Vector2(GameConstants.START_PLAYER5_PAPER_X, GameConstants.START_PLAYER5_PAPER_Y));
                this.playerLizard = new Element(content, Element.FIVE_MODE, "Lizard3", Element.LIZARD,
                                                new Vector2(GameConstants.START_PLAYER5_LIZARD_X, GameConstants.START_PLAYER5_LIZARD_Y));
                this.playerScissors = new Element(content, Element.FIVE_MODE, "Scissors3", Element.SCISSORS,
                                                new Vector2(GameConstants.START_PLAYER5_SCISSORS_X, GameConstants.START_PLAYER5_SCISSORS_Y));

                //elements of the computer
                this.compRock = new CompElement(content, CompElement.FIVE_MODE, "Rock2", CompElement.ROCK,
                                                new Vector2(GameConstants.START_COMPUTER_ROCK_X, GameConstants.START_COMPUTER_ROCK_Y));
                this.compSpock = new CompElement(content, CompElement.FIVE_MODE, "Spock2", CompElement.SPOCK,
                                                new Vector2(GameConstants.START_COMPUTER_SPOCK_X, GameConstants.START_COMPUTER_SPOCK_Y));
                this.compPaper = new CompElement(content, CompElement.FIVE_MODE, "Paper2", CompElement.PAPER,
                                                new Vector2(GameConstants.START_COMPUTER_PAPER_X, GameConstants.START_COMPUTER_PAPER_Y));
                this.compLizard = new CompElement(content, CompElement.FIVE_MODE, "Lizard2", CompElement.LIZARD,
                                                new Vector2(GameConstants.START_COMPUTER_LIZARD_X, GameConstants.START_COMPUTER_LIZARD_Y));
                this.compScissors = new CompElement(content, CompElement.FIVE_MODE, "Scissors2", CompElement.SCISSORS,
                                                new Vector2(GameConstants.START_COMPUTER_SCISSORS_X, GameConstants.START_COMPUTER_SCISSORS_Y));

                //let computer make a move again
                SecondMode.levelState = LevelState.WAITING_FOR_COMPUTER;
            }

            //Background music support
            if (Game1.gameStage != GameState.FIVE_OBJECTS)
            {
                this.backgroundMusic.Stop();
                this.isPlaying = false;

            }
            else if (!this.isPlaying)
            {
                this.backgroundMusic.Play();
                this.isPlaying = true;
            }
        }



        /// <summary>
        /// Drawing all objects of the five objects mode
        /// </summary>
        /// <param name="spriteBatch">Drawinf manager of the game</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //background picture
            spriteBatch.Draw(backgroundPicture, backDrawRectandle, Color.AliceBlue);

            //buttons
            this.returnButton.Draw(spriteBatch);
            this.exitGameButton.Draw(spriteBatch);
            if (SecondMode.levelState == LevelState.RESULTS)    // this button reloads the level 
            {                                                   // we do not want to see it at other states
                this.nextButton.Draw(spriteBatch);
            }

            //elements
            this.compRock.Draw(spriteBatch);
            this.compPaper.Draw(spriteBatch);
            this.compScissors.Draw(spriteBatch);
            this.compSpock.Draw(spriteBatch);
            this.compLizard.Draw(spriteBatch);

            this.playerRock.Draw(spriteBatch);
            this.playerPaper.Draw(spriteBatch);
            this.playerScissors.Draw(spriteBatch);
            this.playerSpock.Draw(spriteBatch);
            this.playerLizard.Draw(spriteBatch);


            //messages output
            if (this.playerScore < 10)
            {
                spriteBatch.DrawString(this.scoreFont, this.playerScore.ToString(), this.playerScorePosition, Color.RoyalBlue);
            }
            else if (this.playerScore < 100)
            {
                spriteBatch.DrawString(this.scoreFont, this.playerScore.ToString(),
                    new Vector2(this.playerScorePosition.X - 20, this.playerScorePosition.Y), Color.RoyalBlue);
            }
            else
            {
                spriteBatch.DrawString(this.scoreFont, this.playerScore.ToString(),
                    new Vector2(this.playerScorePosition.X - 40, this.playerScorePosition.Y), Color.RoyalBlue);
            }

            if (this.computerScore < 10)
            {
                spriteBatch.DrawString(this.scoreFont, this.computerScore.ToString(), this.computerScorePosition, Color.Tomato);
            }
            else if (this.computerScore < 100)
            {
                spriteBatch.DrawString(this.scoreFont, this.computerScore.ToString(),
                    new Vector2(this.computerScorePosition.X - 20, this.computerScorePosition.Y), Color.Tomato);
            }
            else
            {
                spriteBatch.DrawString(this.scoreFont, this.computerScore.ToString(),
                    new Vector2(this.computerScorePosition.X - 40, this.computerScorePosition.Y), Color.Tomato);
            }

            // at the result state show a message about result (win, lose, draw)
            if (SecondMode.levelState == LevelState.RESULTS)
            {
                spriteBatch.DrawString(this.resultFont, this.message, this.messagePosition, this.color);
            }
        }


        /// <summary>
        /// This function make a computer choise between five elements
        /// </summary>
        public void Choise()
        {
            int randomNumber;
            randomNumber = myRand.Next(5);

            if (randomNumber == 0)      //Rock
            {
                compRock.Choose();
            }
            else if (randomNumber == 1) //Paper
            {
                compPaper.Choose();
            }
            else if (randomNumber == 2) //Scissors
            {
                compScissors.Choose();
            }
            else if (randomNumber == 3) //Spock
            {
                compSpock.Choose();
            }
            else if (randomNumber == 4) //Lizard
            {
                compLizard.Choose();
            }
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// load all pictures, sounds and other things for Intro class
        /// </summary>
        /// <param name="content"> content manager in game </param> 
        private void LoadContent(ContentManager content)
        {
            //load background picture
            this.backgroundPicture = content.Load<Texture2D>("Pictures/TB_BackGround");

            //load and initiate sounds
            SoundEffect music = content.Load<SoundEffect>("Sounds/Fight");
            this.backgroundMusic = music.CreateInstance();
            this.backgroundMusic.IsLooped = true;
            this.backgroundMusic.Volume = 0.3f;

            this.winSound = content.Load<SoundEffect>("Sounds/WinSound");
            this.loseSound = content.Load<SoundEffect>("Sounds/LoseSound");
            this.drawSound = content.Load<SoundEffect>("Sounds/DrawSound");

            //load fonts
            scoreFont = content.Load<SpriteFont>("ScoreFont");
            resultFont = content.Load<SpriteFont>("ScoreFont2");

        }


        /// <summary>
        /// This procedure computes the result of the turn
        /// </summary>
        private void computeResults()
        {
            if ((SecondMode.compChoise + 1) % 5 == SecondMode.playerCoise) //player wins
            {
                this.playerScore += 1;
                this.message = "You Win!!!";
                this.color = Color.Lime;
                this.messagePosition = new Vector2(GameConstants.MESSAGE_POSITION_WIN_X, GameConstants.MESSAGE_POSITION_WIN_Y);
                winSound.Play(0.25f, 0.0f, 0.0f);
            }
            else if ((SecondMode.compChoise + 2) % 5 == SecondMode.playerCoise) //player wins
            {
                this.playerScore += 1;
                this.message = "You Win!!!";
                this.color = Color.Lime;
                this.messagePosition = new Vector2(GameConstants.MESSAGE_POSITION_WIN_X, GameConstants.MESSAGE_POSITION_WIN_Y);
                winSound.Play(0.25f, 0.0f, 0.0f);
            }
            else if (SecondMode.compChoise == SecondMode.playerCoise)     //draw
            {
                this.message = "Draw";
                this.color = Color.Yellow;
                this.messagePosition = new Vector2(GameConstants.MESSAGE_POSITION_DRAW_X, GameConstants.MESSAGE_POSITION_DRAW_Y);
                drawSound.Play(0.5f, 0.0f, 0.0f);
            }
            else                                    //computer wins
            {
                this.computerScore += 1;
                this.message = "You Lose!";
                this.color = Color.Red;
                this.messagePosition = new Vector2(GameConstants.MESSAGE_POSITION_LOSE_X, GameConstants.MESSAGE_POSITION_LOSE_Y);
                loseSound.Play(0.5f, 0.0f, 0.0f);
            }
        }
        
        #endregion
    }
}
