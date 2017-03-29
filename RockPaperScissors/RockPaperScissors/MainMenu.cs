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
    /// First window of the game, Main Menu
    /// </summary>
    class MainMenu
    {
        #region Fields

        //graphics support
        Texture2D backgroundPicture;
        Rectangle backDrawRectandle;

        //all buttons of the game
        Button threeObjectsButton;
        Button fiveObjectsButton;
        Button aboutButton;
        Button exitGameButton;

        //sound support
        SoundEffectInstance backgroundMusic;
        bool isPlaying = false;

        #endregion

        #region Constructors
        /// <summary>
        /// Constuctor for the Main Menu of game
        /// </summary>
        /// <param name="content">Content manager of game</param>
        public MainMenu(ContentManager content)
        {
            this.LoadContent(content);

            this.backDrawRectandle = new Rectangle(-5, -5, backgroundPicture.Width, backgroundPicture.Height);

            this.threeObjectsButton = new Button(content, "Button_1", new Vector2(GameConstants.BUTTON_1_POSITION_X, GameConstants.BUTTON_1_POSITION_Y));
            this.fiveObjectsButton = new Button(content, "Button_2", new Vector2(GameConstants.BUTTON_2_POSITION_X, GameConstants.BUTTON_2_POSITION_Y));
            this.aboutButton = new Button(content, "Button_3", new Vector2(GameConstants.BUTTON_3_POSITION_X, GameConstants.BUTTON_3_POSITION_Y));
            this.exitGameButton = new Button(content, "Button_4", new Vector2(GameConstants.BUTTON_4_POSITION_X, GameConstants.BUTTON_4_POSITION_Y));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updating main menu
        /// </summary>
        /// <param name="mouse">mouse manager of the game</param>
        public void Update(MouseState mouse)
        {
            // updating button, giving game stage needed to the main menu
            this.threeObjectsButton.Update(mouse, GameState.THREE_OBJECTS);
            this.fiveObjectsButton.Update(mouse, GameState.FIVE_OBJECTS);
            this.aboutButton.Update(mouse, GameState.ABOUT_WINDOW);
            this.exitGameButton.Update(mouse, GameState.EXIT_GAME);

            //sound controling
            if ((Game1.gameStage == GameState.THREE_OBJECTS)
                || (Game1.gameStage == GameState.FIVE_OBJECTS))
            {
                this.backgroundMusic.Stop();
                this.isPlaying = false;
            }
            else if (!isPlaying)
            {
                this.backgroundMusic.Play();
                this.isPlaying = true;
            }
        }


        /// <summary>
        /// Drawing main menu
        /// </summary>
        /// <param name="spriteBatch">Drawing manager</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundPicture, backDrawRectandle, Color.AliceBlue);
            this.threeObjectsButton.Draw(spriteBatch);
            this.fiveObjectsButton.Draw(spriteBatch);
            this.aboutButton.Draw(spriteBatch);
            this.exitGameButton.Draw(spriteBatch);

        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Load all pictures, sounds and other things for Intro class
        /// </summary>
        /// <param name="content"> Content manager in the game </param> 
        private void LoadContent(ContentManager content)
        {
            this.backgroundPicture = content.Load<Texture2D>("Pictures/MM_BackGround");
            SoundEffect music = content.Load<SoundEffect>("Sounds/MainMenu");
            this.backgroundMusic = music.CreateInstance();
            this.backgroundMusic.IsLooped = true;
            this.backgroundMusic.Volume = 0.3f;
        }

        #endregion
    }
}
