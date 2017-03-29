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
    class AboutWindow
    {
        #region Fields

        //graphics support
        Texture2D backgroundPicture3;
        Rectangle backDrawRectandle3;
        Texture2D backgroundPicture5;
        Rectangle backDrawRectandle5;

        //all buttons of the game
        Button threeObjectsButton;
        Button fiveObjectsButton;
        Button returnButton;

        // variable to understand what kind of information we shoud show
        public static bool aboutThreeElements = true;


        #endregion

        #region Constructors
        /// <summary>
        /// Constuctor for the Main Menu of game
        /// </summary>
        /// <param name="content">Content manager of game</param>
        public AboutWindow(ContentManager content)
        {
            this.LoadContent(content);
            this.backDrawRectandle3 = new Rectangle(-5, -5, this.backgroundPicture3.Width, this.backgroundPicture3.Height);
            this.backDrawRectandle5 = new Rectangle(-5, -5, this.backgroundPicture5.Width, this.backgroundPicture5.Height);
            this.threeObjectsButton = new Button(content, "Button_5", new Vector2(GameConstants.BUTTON_5_POSITION_X, 
                                                                                  GameConstants.BUTTON_5_POSITION_Y));
            this.fiveObjectsButton = new Button(content, "Button_6", new Vector2(GameConstants.BUTTON_6_POSITION_X, 
                                                                                 GameConstants.BUTTON_6_POSITION_Y));
            this.returnButton = new Button(content, "Button_7", new Vector2(GameConstants.BUTTON_7_POSITION_X, 
                                                                            GameConstants.BUTTON_7_POSITION_Y));

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updating main menu
        /// </summary>
        /// <param name="mouse">Mouse manager of the game</param>
        public void Update(MouseState mouse)
        {
            // updating button, giving game stage needed to the main menu
            this.threeObjectsButton.Update(mouse, GameConstants.ABOUT_THREE_OBJECTS);
            this.fiveObjectsButton.Update(mouse, GameConstants.ABOUT_FIVE_OBJECTS);
            this.returnButton.Update(mouse, GameState.MAIN_MENU);
        }


        /// <summary>
        /// Drawing main menu
        /// </summary>
        /// <param name="spriteBatch">Drawing manager</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //what kind of information show to player
            if (AboutWindow.aboutThreeElements)
            {
                spriteBatch.Draw(this.backgroundPicture3, this.backDrawRectandle3, Color.AliceBlue);
            }
            else
            {
                spriteBatch.Draw(this.backgroundPicture5, this.backDrawRectandle5, Color.AliceBlue);
            }

            //drawing buttons
            this.threeObjectsButton.Draw(spriteBatch);
            this.fiveObjectsButton.Draw(spriteBatch);
            this.returnButton.Draw(spriteBatch);

        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Load all pictures, sounds and other things for Intro class
        /// </summary>
        /// <param name="content"> content manager in game </param> 
        private void LoadContent(ContentManager content)
        {
            this.backgroundPicture3 = content.Load<Texture2D>("Pictures/AB_BackGround3");
            this.backgroundPicture5 = content.Load<Texture2D>("Pictures/AB_BackGround5");
        }

        #endregion
    }
}
