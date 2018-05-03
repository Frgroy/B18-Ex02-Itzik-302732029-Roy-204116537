using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace B18_Ex02
{
     public static class UserInterface
     {
          private const int maximumUserNameSize = 20;
          private const int smallBoardSize = 6;
          private const int mediumBoardSize = 8;
          private const int bigBoardSize = 10;

          public static void RunPreGameDialog(out string o_player1Name, out string o_player2Name, out int o_gameBoardSize, out CheckersGame.eGameMode o_gameMode)
          {
               PrintIntroduction();
               o_player1Name = GetUserName();
               o_gameBoardSize = GetRequestedBoardSize();
               o_gameMode = GetGameModeFromUser();
               if (o_gameMode == CheckersGame.eGameMode.VersusAnotherPlayer)
               {
                    o_player2Name = GetUserName();
               }

               else //gameMode == VersusComputer
               {
                    o_player2Name = "Computer";
               }
          }

          public static void PrintIntroduction()
          {
               Console.WriteLine("Hello! Let's play CHECKERS!!! Have fun :-)");
          }

          public static void PrintIllegalInputMassage()
          {
               Console.WriteLine("Illegal input inserted. Please try again.");
          }

          public static string GetUserName()
          {
               Console.WriteLine("Please enter your name: ");
               string userInputForName = Console.ReadLine();
               while (!IsLegalUserNameInserted(userInputForName))
               {
                    PrintIllegalInputMassage();
                    userInputForName = Console.ReadLine();
               }

               return userInputForName;
          }

          public static bool IsLegalUserNameInserted(string i_insertedUserName)
          {
               return Regex.IsMatch(i_insertedUserName, "^[a-z,A-Z,0-9,!-/]+$") && i_insertedUserName.Length <= maximumUserNameSize ? true : false;
          }

          public static int GetRequestedBoardSize()
          {
               int userChoiseForBoardSize;
               string boardSizeMassage = string.Format(@"Please enter preferred board size:
(6) 6x6 
(8) 8x8
(10) 10x10");
               Console.WriteLine(boardSizeMassage);
               string userInputForBoardSize = Console.ReadLine();

               while (!int.TryParse(userInputForBoardSize, out userChoiseForBoardSize) || !IsLegalBoardSizeInserted(userChoiseForBoardSize))
               {
                    PrintIllegalInputMassage();
                    userInputForBoardSize = Console.ReadLine();
               }

               return userChoiseForBoardSize;
          }

          public static bool IsLegalBoardSizeInserted(int i_insertedUserChoiseForBoardSize)
          {
               return (i_insertedUserChoiseForBoardSize == smallBoardSize || i_insertedUserChoiseForBoardSize == mediumBoardSize || i_insertedUserChoiseForBoardSize == bigBoardSize) ? true : false;
          }

          public static CheckersGame.eGameMode GetGameModeFromUser()
          {
               string gameModeMassage = string.Format(@"Please enter prefered game mode:
(1) Versus another player
(2) Versus Computer");
               Console.WriteLine(gameModeMassage);
               string userInputForGameMode = Console.ReadLine();
               int userChoiseForGameMode;
               while (!int.TryParse(userInputForGameMode, out userChoiseForGameMode) || !IsLegalGameModeInserted(userChoiseForGameMode))
               {
                    PrintIllegalInputMassage();
                    userInputForGameMode = Console.ReadLine();
               }

               return (CheckersGame.eGameMode)userChoiseForGameMode;
          }

          public static bool IsLegalGameModeInserted(int i_insertedUserChoiseForGameMode)
          {
               return (i_insertedUserChoiseForGameMode == 1 || i_insertedUserChoiseForGameMode == 2 ? true : false);
          }

          public static void PrintGameBoard(Board gameBoard)
          {
               Ex02.ConsoleUtils.Screen.Clear();
               for (int i = 0; i < gameBoard.boardSize; i++)
               {
                    Console.Write("   ");
                    Console.Write((char)('A' + i));
               }
               Console.WriteLine("   ");
               printGameBoardSeperator(gameBoard.boardSize);

               for (int i = 0; i < gameBoard.boardSize; i++)
               {
                    Console.Write((char)('a' + i));
                    Console.Write('|');
                    for (int j = 0; j < gameBoard.boardSize; j++)
                    {
                         Console.Write(" {0} |", gameBoard.GetSquareContent(i, j));
                    }
                    Console.WriteLine(' ');
                    printGameBoardSeperator(gameBoard.boardSize);
               }
          }

          private static void printGameBoardSeperator(int i_gameBoardSize)
          {
               Console.Write(' ');
               for (int i = 0; i < (i_gameBoardSize * 4) + 2; i++)
               {
                    Console.Write('=');
               }
               Console.WriteLine(' ');
          }

          public static void PrintMoveInfo(Team i_activeTeam, Team i_inactiveTeam)
          {
               Console.WriteLine(string.Format("{0}'s move was ({1}): {2}", i_inactiveTeam.teamName, i_inactiveTeam.teamSign, i_inactiveTeam.lastMoveExecuted));
               Console.WriteLine(string.Format("{0}'s Turn ({1}):", i_activeTeam.teamName, i_activeTeam.teamSign));
          }

          public static void PrintFirstMoveInfo(Team i_activeTeam)
          {
               Console.WriteLine(string.Format("{0}'s Turn ({1}):", i_activeTeam.teamName, i_activeTeam.teamSign));
          }

          public static void HandleUserInput(ref Move requestedMove, Team i_activeTeam)
          {
               //to do handle q
               string userInput = Console.ReadLine();
               while (!requestedMove.TryParse(userInput, ref requestedMove, i_activeTeam))
               {
                    PrintIllegalInputMassage();
                    userInput = Console.ReadLine();
               }

          }

          public static bool IsLegalInputFormat(string i_userInput)
          {
               string legalMovePattern = @"^[A-Z][a-z]>[A-Z][a-z]$";

               return Regex.IsMatch(i_userInput, legalMovePattern);
          }
     }
}
