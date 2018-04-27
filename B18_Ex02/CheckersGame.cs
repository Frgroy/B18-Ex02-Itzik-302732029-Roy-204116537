using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace B18_Ex02
{
     public class CheckersGame
     {
          string userName;
          eGameMode gameMode;
          Board activeBoard = null;

          public void Run()
          {
               RunPreGameDialog();

          }

          public void RunPreGameDialog()
          {
               GetUserSettings();
               CreateNewCheckersGame();
          }

          public void GetUserSettings()
          {
               PrintIntroduction();
               userName = GetUserName();
               activeBoard.size = GetRequestedBoardSize();
               gameMode = GetGameModeFromUser();
          }

          public void PrintIntroduction()
          {
               Console.WriteLine("Hello! Let's play CHECKERS!!! Have fun :-)");
          }

          public string GetUserName()
          {
               Console.WriteLine("Please enter your name :-)");
               string userName = Console.ReadLine();
               while (!IsLegalUserNameInserted(userName))         
               {
                    Console.WriteLine("Error notifecated! Please try again :-)");
                    userName = Console.ReadLine();
               }

               return userName;
          }

          public bool IsLegalUserNameInserted(string insertedUserName)
          {
               return Regex.IsMatch(insertedUserName, "^[a-z,A-Z,0-9]+$") && insertedUserName.Length <= 20 ? true : false; //TODO ADD SIGNS
          }

          public int GetRequestedBoardSize()
          {
               string BoardSizeMassage = string.Format(@"Please enter prefered board size:
(6) 6x6 
(8) 8x8
(10) 10x10");
               Console.WriteLine(BoardSizeMassage);
               int userChoiseForBoardSize = int.Parse(Console.ReadLine());
               while (!isLegalBoardSizeInserted(userChoiseForBoardSize))                
               {
                    Console.WriteLine("Error notifecated! Please try again :-)");
                    userChoiseForBoardSize = int.Parse(Console.ReadLine());
               }

               return userChoiseForBoardSize;
          }

          public bool isLegalBoardSizeInserted(int insertedUserChoiseForBoardSize) // not to do
          {
               return (insertedUserChoiseForBoardSize == 6 || insertedUserChoiseForBoardSize == 8 || insertedUserChoiseForBoardSize == 10) ? true : false;  //TODO enum
          }

          public eGameMode GetGameModeFromUser()
          {
               string gameModeMassage = string.Format(@"Please enter prefered game mode:
(1) Versus Computer
(2) Versus another player");
               Console.WriteLine(gameModeMassage);
               eGameMode userChoiseForGameMode = (eGameMode)int.Parse(Console.ReadLine()); 
               while (!IsLegalGameModeInserted((int)userChoiseForGameMode))
               {
                    Console.WriteLine("Error notifecated! Please try again :-)");
                    userChoiseForGameMode = (eGameMode)int.Parse(Console.ReadLine());
               }

               return userChoiseForGameMode;
          }

          public bool IsLegalGameModeInserted(int insertedUserChoiseForGameMode)
          {
               return (insertedUserChoiseForGameMode == 1 || insertedUserChoiseForGameMode == 2 ? true : false);
          }

          public enum eGameMode
          {
               VersusComputer = 1,
               VersusAnotherPlayer
          }

          public void CreateNewCheckersGame()
          {
               activeBoard.CreateNewBoard();
          }
     }
}
