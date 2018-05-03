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
          private const string quitRequest = "Q";

          public static void RunGame()
          {
               CheckersGame game = new CheckersGame();
               game.CreateNewGame();
               while (game.status == CheckersGame.eGameStatus.activeGame)
               {
                    game.CreateNewRound();
                    while (game.status == CheckersGame.eGameStatus.inRound)
                    {
                         game.ManageRound();
                    } 
                    if (game.status == CheckersGame.eGameStatus.startingNewRound)
                    {
                         game.status = CheckersGame.eGameStatus.activeGame;
                    }
               }
          }

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
               return Regex.IsMatch(i_insertedUserName, "^[a-z,A-Z,0-9,!-/,:-@,[-`, {-~]+$") && i_insertedUserName.Length <= maximumUserNameSize ? true : false;
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
(1) Versus Another Player
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
               if (i_inactiveTeam.lastMoveExecuted != null)
               {
                    Console.WriteLine(string.Format("{0}'s move was ({1}): {2}", i_inactiveTeam.teamName, i_inactiveTeam.teamSign, i_inactiveTeam.lastMoveExecuted.ToString()));
               }
               Console.WriteLine(string.Format("{0}'s Turn ({1}):", i_activeTeam.teamName, i_activeTeam.teamSign));
          }

          public static void HandleUserInput(ref Move io_requestedMove, ref CheckersGame.eGameStatus i_gameStatus, Team i_activeTeam)
          {
               string userInput = Console.ReadLine();
               while (!io_requestedMove.TryParse(userInput, ref io_requestedMove, i_activeTeam) && !IsLegalQuitRequest(ref i_gameStatus, userInput, i_activeTeam))
               {
                    PrintIllegalInputMassage();
                    userInput = Console.ReadLine();
               }
          }

          public static bool IsLegalQuitRequest(ref CheckersGame.eGameStatus i_gameStatus, string i_userInput, Team i_activeTeam)
          {
               bool isLegalQuitRequest = false;

               if (i_userInput == "Q")
               {
                    if (i_activeTeam.isLeadingTeam == false)
                    {
                         isLegalQuitRequest = true;
                         i_gameStatus = CheckersGame.eGameStatus.roundEnd;
                    }
               }

               return isLegalQuitRequest;
          }

          public static bool IsLegalInputFormat(string i_userInput)
          {
               string legalMovePattern = @"^[A-Z][a-z]>[A-Z][a-z]$";

               return Regex.IsMatch(i_userInput, legalMovePattern);
          }

          public static void RunAnotherRoundDialog(Team i_winningTeam, ref CheckersGame.eGameStatus io_gameStatus)
          {
               PrintWinningTeamMassage(i_winningTeam);
               io_gameStatus = GetGameStatusFromUser();
          }

          public static void PrintWinningTeamMassage(Team i_winningTeam)
          {
               string winningTeamMassage = string.Format(@"The winner is {0}, with {1} points!", i_winningTeam.teamName, i_winningTeam.teamScore);
               Console.WriteLine(winningTeamMassage);
          }

          public static CheckersGame.eGameStatus GetGameStatusFromUser()
          {
               string gameStatusMassage = string.Format(@"What do you want to do now?
(1) Play Another Round!
(2) Exit");
               Console.WriteLine(gameStatusMassage);
               string userInputForGameStatus = Console.ReadLine();
               int userChoiseForGameStatus;
               while (!int.TryParse(userInputForGameStatus, out userChoiseForGameStatus) || !IsLegalStatusInserted(userChoiseForGameStatus))
               {
                    PrintIllegalInputMassage();
                    userInputForGameStatus = Console.ReadLine();
               }

               return (CheckersGame.eGameStatus)userChoiseForGameStatus;
          }

          public static bool IsLegalStatusInserted(int i_insertedUserChoiseForGameStatus)
          {
               return (i_insertedUserChoiseForGameStatus == 1 || i_insertedUserChoiseForGameStatus == 2 ? true : false);
          }
     }
}
