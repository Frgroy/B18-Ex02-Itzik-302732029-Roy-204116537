using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
               string player1Name;
               string player2Name;
               int gameBoardSize;
               CheckersGame.eGameMode gameMode;
               RunPreGameDialog(out player1Name, out player2Name, out gameBoardSize, out gameMode);
               CheckersGame game = new CheckersGame(player1Name, player2Name, gameBoardSize, gameMode);
               ManageGame(game);
          }

          public static void ManageGame(CheckersGame game)
          {
               while (game.status == CheckersGame.eGameStatus.activeGame)
               {
                    game.CreateNewRound();
                    while (game.status == CheckersGame.eGameStatus.inRound)
                    {
                         ManageRound(game);
                    }

                    if (game.status == CheckersGame.eGameStatus.startingNewRound)
                    {
                         game.status = CheckersGame.eGameStatus.activeGame;
                    }
               }
          }

          public static void ManageRound(CheckersGame game)
          {
               PrintGameBoard(game.board);
               PrintMoveInfo(game.activeTeam, game.inactiveTeam);
               System.Threading.Thread.Sleep(300);
               game.UpdateMovesInTeams();
               Move requestedMove = new Move();
               if (game.mode == CheckersGame.eGameMode.VersusAnotherPlayer)
               {
                    requestedMove = HandleUserMoveInput(game);
               }
               else
               {
                    requestedMove = game.GenerateMoveRequest();
               }

               if (game.status == CheckersGame.eGameStatus.inRound)
               {
                    game.MakeAMoveProcess(requestedMove);
                    while (game.IsProgressiveMoveAvailable(requestedMove) && game.status == CheckersGame.eGameStatus.inRound)
                    {
                         PrintGameBoard(game.board);
                         PrintMoveInfo(game.activeTeam, game.inactiveTeam);
                         System.Threading.Thread.Sleep(100);
                         if (game.mode == CheckersGame.eGameMode.VersusAnotherPlayer)
                         {
                              requestedMove = HandleUserProgressiveMoveInput(requestedMove, game.status, game.activeTeam);
                         }
                         else
                         {
                              game.GenerateProgressiveAttack(ref requestedMove);
                         }

                         game.MakeAMoveProcess(requestedMove);
                    }
               }

               if (game.IsEndOfRound())
               {
                    HandleEndOfRound(game);
               }
               else
               {
                    game.SwapActiveTeam();
               }
          }

          public static void HandleEndOfRound(CheckersGame game)
          {
               Ex02.ConsoleUtils.Screen.Clear();
               CheckersGame.eGameStatus newStatusFromUser;
               if (game.status == CheckersGame.eGameStatus.roundEndWithDraw)
               {
                    RunAnotherRoundDialog(game.activeTeam, game.inactiveTeam, out newStatusFromUser);
               }
               else
               {
                    RunAnotherRoundDialog(game.activeTeam, out newStatusFromUser);
               }

               game.status = newStatusFromUser;
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
               else
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
               return i_insertedUserChoiseForGameMode == 1 || i_insertedUserChoiseForGameMode == 2 ? true : false;
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

          public static Move HandleUserMoveInput(CheckersGame i_game)
          {
               Move returnedMoveRequest = new Move();
               string userInput = Console.ReadLine();

               while (IsLegalUserMoveInput(userInput, ref returnedMoveRequest, i_game.activeTeam) == false)
               {
                    PrintGameBoard(i_game.board);
                    PrintMoveInfo(i_game.activeTeam, i_game.inactiveTeam);
                    PrintIllegalInputMassage();
                    userInput = Console.ReadLine();
               }

               return returnedMoveRequest;
          }

          public static Move HandleUserProgressiveMoveInput(Move i_previousMove, CheckersGame.eGameStatus i_gameStatus, Team i_activeTeam)
          {
               Move returnedMoveRequest = new Move();
               string userInput = Console.ReadLine();

               while (IsLegalUserMoveInput(userInput, ref returnedMoveRequest, i_activeTeam) == false ||
                    i_previousMove.destinationSquare.squarePosition.x != returnedMoveRequest.sourceSquare.squarePosition.x ||
                    i_previousMove.destinationSquare.squarePosition.y != returnedMoveRequest.sourceSquare.squarePosition.y)
               {
                    PrintIllegalInputMassage();
                    userInput = Console.ReadLine();
               }

               return returnedMoveRequest;
          }

          public static bool IsLegalUserMoveInput(string i_userInput, ref Move io_moveInput, Team i_activeTeam)
          {
               bool isLegalUserMoveInput = false;

               if (IsLegalQuitRequest(i_userInput))
               {
                    if (io_moveInput.TryParse(i_userInput, ref io_moveInput, i_activeTeam) == true)
                    {
                         isLegalUserMoveInput = true;
                    }
               }
               else if (IsLegalMoveInputFormat(i_userInput))
               {
                    if (io_moveInput.TryParse(i_userInput, ref io_moveInput, i_activeTeam) == true)
                    {
                         isLegalUserMoveInput = true;
                    }
               }

               return isLegalUserMoveInput;
          }

          public static bool IsLegalQuitRequest(string i_userInput)
          {
               return i_userInput == quitRequest ? true : false;
          }

          public static bool IsLegalMoveInputFormat(string i_userInput)
          {
               string legalMovePattern = @"^[A-Z][a-z]>[A-Z][a-z]$";

               return Regex.IsMatch(i_userInput, legalMovePattern);
          }

          public static void RunAnotherRoundDialog(Team i_winningTeam, out CheckersGame.eGameStatus io_gameStatus)
          {
               PrintWinningTeamMassage(i_winningTeam);
               io_gameStatus = GetGameStatusFromUser();
          }

          public static void RunAnotherRoundDialog(Team i_firstTeam, Team i_secondTeam, out CheckersGame.eGameStatus io_gameStatus)
          {
               PrintDrawMassage(i_firstTeam, i_secondTeam);
               io_gameStatus = GetGameStatusFromUser();
          }

          public static void PrintWinningTeamMassage(Team i_winningTeam)
          {
               string winningTeamMassage = string.Format(@"The winner is {0}, with {1} points!", i_winningTeam.teamName, i_winningTeam.teamScore);
               Console.WriteLine(winningTeamMassage);
          }

          public static void PrintDrawMassage(Team i_firstTeam, Team i_secondTeam)
          {
               string drawMassage = string.Format(
                    @"The game ended with draw. {0} with {1} points, {2} with {3} points.",
                    i_firstTeam.teamName,
                    i_firstTeam.teamScore,
                    i_secondTeam.teamName,
                    i_secondTeam.teamScore);
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
               return i_insertedUserChoiseForGameStatus == 1 || i_insertedUserChoiseForGameStatus == 2 ? true : false;
          }
     }
}
