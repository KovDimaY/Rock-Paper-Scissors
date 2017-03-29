using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RockPaperScissors
{
    class GameConstants
    {
        //window dimentions
        public const int WINDOW_WIDTH = 1100;
        public const int WINDOW_HEIGHT = 600;

        //Button clicks
        public const int ABOUT_THREE_OBJECTS = 5;
        public const int ABOUT_FIVE_OBJECTS = 6;
        public const int NEW_THREE_OBJECTS_GAME = 10;
        public const int NEW_FIVE_OBJECTS_GAME = 11;

        #region ButtonPositions

        //buttons of MainMenu
        public const int BUTTON_1_POSITION_X = 220;
        public const int BUTTON_1_POSITION_Y = 200;
        public const int BUTTON_2_POSITION_X = GameConstants.BUTTON_1_POSITION_X;
        public const int BUTTON_2_POSITION_Y = 300;
        public const int BUTTON_3_POSITION_X = GameConstants.BUTTON_1_POSITION_X;
        public const int BUTTON_3_POSITION_Y = 400;
        public const int BUTTON_4_POSITION_X = GameConstants.BUTTON_1_POSITION_X;
        public const int BUTTON_4_POSITION_Y = 500;

        //buttons of AboutWindow
        public const int BUTTON_5_POSITION_X = 250;
        public const int BUTTON_5_POSITION_Y = 520;
        public const int BUTTON_6_POSITION_X = 850;
        public const int BUTTON_6_POSITION_Y = GameConstants.BUTTON_5_POSITION_Y;
        public const int BUTTON_7_POSITION_X = 550;
        public const int BUTTON_7_POSITION_Y = GameConstants.BUTTON_5_POSITION_Y;

        //buttons of ThreeElements (FirstMode)
        public const int BUTTON_8_POSITION_X = 90;
        public const int BUTTON_8_POSITION_Y = 550;
        public const int BUTTON_9_POSITION_X = 1010;
        public const int BUTTON_9_POSITION_Y = GameConstants.BUTTON_8_POSITION_Y;
        public const int BUTTON_10_POSITION_X = 550;
        public const int BUTTON_10_POSITION_Y = 500;


        #endregion

        #region ElementPositions

        //player elements (THREE MODE)
        public const int START_PLAYER3_ROCK_X = 550;
        public const int START_PLAYER3_ROCK_Y = 500;
        public const int START_PLAYER3_PAPER_X = 350;
        public const int START_PLAYER3_PAPER_Y = 490;
        public const int START_PLAYER3_SCISSORS_X = 750;
        public const int START_PLAYER3_SCISSORS_Y = GameConstants.START_PLAYER3_PAPER_Y;

        //player elements (FIVE MODE)
        public const int START_PLAYER5_ROCK_X = 550;
        public const int START_PLAYER5_ROCK_Y = 505;
        public const int START_PLAYER5_SPOCK_X = 230;
        public const int START_PLAYER5_SPOCK_Y = 475;
        public const int START_PLAYER5_PAPER_X = 390;
        public const int START_PLAYER5_PAPER_Y = 495;
        public const int START_PLAYER5_LIZARD_X = 870;
        public const int START_PLAYER5_LIZARD_Y = 475;
        public const int START_PLAYER5_SCISSORS_X = 710;
        public const int START_PLAYER5_SCISSORS_Y = 495;

        //computer elements
        public const int START_COMPUTER_ROCK_X = 550;
        public const int START_COMPUTER_ROCK_Y = 100;
        public const int START_COMPUTER_SPOCK_X = 150;
        public const int START_COMPUTER_SPOCK_Y = GameConstants.START_COMPUTER_ROCK_Y;
        public const int START_COMPUTER_PAPER_X = 350;
        public const int START_COMPUTER_PAPER_Y = GameConstants.START_COMPUTER_ROCK_Y;
        public const int START_COMPUTER_LIZARD_X = 950;
        public const int START_COMPUTER_LIZARD_Y = GameConstants.START_COMPUTER_ROCK_Y;
        public const int START_COMPUTER_SCISSORS_X = 750;
        public const int START_COMPUTER_SCISSORS_Y = GameConstants.START_COMPUTER_ROCK_Y;

        //movements of elements
        public const int DESTINATION_PLAYER_X = 650;
        public const int DESTINATION_PLAYER_Y = 300;
        public const int FIRST_DESTINATION_COMP_X = 550;
        public const int FIRST_DESTINATION_COMP_Y = 100;
        public const int SECOND_DESTINATION_COMP_X = 450;
        public const int SECOND_DESTINATION_COMP_Y = 300;


        #endregion


        //scores and messages positions
        public const int SCORE_POSITION_PLAYER_X = 930;
        public const int SCORE_POSITION_PLAYER_Y = 230;
        public const int SCORE_POSITION_COMPUTER_X = 100;
        public const int SCORE_POSITION_COMPUTER_Y = GameConstants.SCORE_POSITION_PLAYER_Y;

        public const int MESSAGE_POSITION_WIN_X = 270;
        public const int MESSAGE_POSITION_WIN_Y = 20;
        public const int MESSAGE_POSITION_DRAW_X = 400;
        public const int MESSAGE_POSITION_DRAW_Y = GameConstants.MESSAGE_POSITION_WIN_Y;
        public const int MESSAGE_POSITION_LOSE_X = 285;
        public const int MESSAGE_POSITION_LOSE_Y = GameConstants.MESSAGE_POSITION_WIN_Y;
    }
}
