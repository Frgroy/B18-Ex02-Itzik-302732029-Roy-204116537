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
          const int smallBoardSize = 6;
          const int mediumBoardSize = 8;
          const int bigBoardSize = 10;
          const int maximumUserNameSize = 20;
          Team player1 = new Team();
          Team player2 = new Team();
          Board activeBoard = new Board();
          string firstPlayerUserName;
          string secondPlayerUserName;
          eGameMode gameMode;
          eGameStatus gameStatus;

          public void Run()
          {
               RunPreGameDialog();
               CreateNewCheckersGame();
               ManageCheckersGame();
          }

          public void ManageCheckersGame()
          {
               while (gameStatus == eGameStatus.active)
               {
                    gameStatus = eGameStatus.inRound; //to do change the name
                    while (gameStatus == eGameStatus.inRound)
                    {
                         ManageDuelTurn();
                    }
               }
          }

          public void ManageDuelTurn()
          {
               ManagePlayerTurn(player1);
               activeBoard.PrintBoard();
               ManagePlayerTurn(player2);
               activeBoard.PrintBoard();

          }

          public void ManagePlayerTurn(Team player)
          {
               HandlePlayerInput(player);
               printInfo();
          }

          public void HandlePlayerInput(Team player) // to dodoododo
          {
               string legalMovePattern = @"^[A-Z][a-z]>[A-Z][a-z]$";
               string userInput = Console.ReadLine();
               while (!Regex.IsMatch(userInput, legalMovePattern))
               {
                    IllegalInputMassage();
                    userInput = Console.ReadLine();
               }

               while (!HandleUserRequestToMove(userInput, player))
               {
                    IllegalInputMassage();
                    userInput = Console.ReadLine();
               }
          }

          public void IllegalInputMassage()
          {
               Console.WriteLine("LOO TOOVVVV!!!"); //todo UI class
          }

          public bool HandleUserRequestToMove(string userInput, Team player)
          {
               bool IsHandledMove = false;
               Square sourceSquare = new Square();
               Square destinationSquare = new Square();
               ConvertUserInputToMove(userInput, ref sourceSquare,  ref destinationSquare);

               if (IsSourceLegal(player, sourceSquare))
               {
                    if (IsSquarePositionInBoardRange(destinationSquare.squarePosition) && destinationSquare.m_man == null)
                    {
                         if (IsManMoving(ref sourceSquare,ref destinationSquare))
                         {
                              IsHandledMove = true;
                         }

                         else if (IsManEating(ref sourceSquare,ref destinationSquare))
                         {
                              IsHandledMove = true;
                         }
                    }
               }

               return IsHandledMove;
          }

          public bool IsManMoving(ref Square sourceSquare, ref Square destinationSquare)
          {
               bool isManMoved = false;
               if (sourceSquare.m_man.m_directionOfMovement == Team.eDirectionOfMovement.up) // to do or to king
               {
                    if (sourceSquare.squarePosition.x - 1 == destinationSquare.squarePosition.x &&
                         sourceSquare.squarePosition.y - 1 == destinationSquare.squarePosition.y)
                    {
                         HandleManMove(ref sourceSquare, ref destinationSquare);
                         isManMoved = true;
                    }
                    else if (sourceSquare.squarePosition.x + 1 == destinationSquare.squarePosition.x &&
                         sourceSquare.squarePosition.y - 1 == destinationSquare.squarePosition.y)
                    {
                         HandleManMove(ref sourceSquare, ref destinationSquare);
                         isManMoved = true;
                    }
               }

               else
               {
                    if (sourceSquare.squarePosition.x - 1 == destinationSquare.squarePosition.x &&
                         sourceSquare.squarePosition.y + 1 == destinationSquare.squarePosition.y)
                    {
                         HandleManMove(ref sourceSquare, eDirectionType.downLeft);
                         isManMoved = true;


                    }
                    else if (sourceSquare.squarePosition.x + 1 == destinationSquare.squarePosition.x &&
                         sourceSquare.squarePosition.y + 1 == destinationSquare.squarePosition.y)
                    {
                         HandleManMove(ref sourceSquare, ref destinationSquare);
                         isManMoved = true;
                    }
               }
               return isManMoved;
          }

          public bool IsManEating(ref Square sourceSquare, ref Square destinationSquare)
          {
               bool isManAte = false;
               if (sourceSquare.m_man.m_directionOfMovement == Team.eDirectionOfMovement.up) // to do or to king
               {
                    if (sourceSquare.squarePosition.x - 2 == destinationSquare.squarePosition.x &&
                         sourceSquare.squarePosition.y - 2 == destinationSquare.squarePosition.y)
                    {
                         if (activeBoard.gameBoard[destinationSquare.squarePosition.y + 1, destinationSquare.squarePosition.x + 1].m_man != null &&
                              activeBoard.gameBoard[destinationSquare.squarePosition.y + 1, destinationSquare.squarePosition.x + 1].m_man.m_manTeam != sourceSquare.m_man.m_manTeam)
                         {
                              HandleManEat(ref sourceSquare, ref destinationSquare, eDirectionType.upLeft);
                              isManAte = true;
                         }
                    }
                    else if (sourceSquare.squarePosition.x + 2 == destinationSquare.squarePosition.x &&
                         sourceSquare.squarePosition.y - 2 == destinationSquare.squarePosition.y)
                    {
                         if (activeBoard.gameBoard[destinationSquare.squarePosition.y + 1, destinationSquare.squarePosition.x - 1].m_man != null &&
                              activeBoard.gameBoard[destinationSquare.squarePosition.y + 1, destinationSquare.squarePosition.x - 1].m_man.m_manTeam != sourceSquare.m_man.m_manTeam)
                         {
                              HandleManEat(ref sourceSquare, ref destinationSquare, eDirectionType.upRight);
                              isManAte = true;
                         }
                    }
               }

               else
               {
                    if (sourceSquare.squarePosition.x - 2 == destinationSquare.squarePosition.x &&
                         sourceSquare.squarePosition.y + 2 == destinationSquare.squarePosition.y)
                    {
                         if (activeBoard.gameBoard[destinationSquare.squarePosition.y - 1, destinationSquare.squarePosition.x + 1].m_man != null &&
                              activeBoard.gameBoard[destinationSquare.squarePosition.y - 1, destinationSquare.squarePosition.x + 1].m_man.m_manTeam != sourceSquare.m_man.m_manTeam)
                         {
                              HandleManEat(ref sourceSquare, ref destinationSquare, eDirectionType.downLeft);
                              isManAte = true;
                         }
                    }
                    else if (sourceSquare.squarePosition.x + 2 == destinationSquare.squarePosition.x &&
                         sourceSquare.squarePosition.y + 2 == destinationSquare.squarePosition.y)
                    {
                         if (activeBoard.gameBoard[destinationSquare.squarePosition.y - 1, destinationSquare.squarePosition.x - 1].m_man != null &&
                              activeBoard.gameBoard[destinationSquare.squarePosition.y - 1, destinationSquare.squarePosition.x - 1].m_man.m_manTeam != sourceSquare.m_man.m_manTeam)
                         {
                              HandleManEat(ref sourceSquare, ref destinationSquare, eDirectionType.downRight);
                              isManAte = true;
                         }
                    }
               }
               return isManAte;
          }

          public void HandleManMove(ref Square sourceSquare, ref Square destinationSquare)
          {
               sourceSquare.m_man.Move(destinationSquare);
          }

          public void HandleManMove(ref Square sourceSquare, eDirectionType directionType)
          {
    
          }

          public void HandleManEat(ref Square sourceSquare, ref Square destinationSquare, eDirectionType directionType)
          {
               Man eatenMan = new Man();
               findEatenMan(destinationSquare, directionType, ref eatenMan);
               eatenMan.BeEaten();
               sourceSquare.m_man.Move(destinationSquare);
          }

          public void findEatenMan (Square destinationSquare, eDirectionType directionType, ref Man eatenMan)
          {
               if (directionType == eDirectionType.upRight)
               {
                    eatenMan = activeBoard.gameBoard[destinationSquare.squarePosition.y + 1, destinationSquare.squarePosition.x - 1].m_man;
               }
               else if (directionType == eDirectionType.upLeft)
               {
                    eatenMan = activeBoard.gameBoard[destinationSquare.squarePosition.y + 1, destinationSquare.squarePosition.x + 1].m_man;
               }

               else if (directionType == eDirectionType.downLeft)
               {
                    eatenMan = activeBoard.gameBoard[destinationSquare.squarePosition.y - 1, destinationSquare.squarePosition.x + 1].m_man;
               }

               else
               {
                    eatenMan = activeBoard.gameBoard[destinationSquare.squarePosition.y - 1, destinationSquare.squarePosition.x - 1].m_man;
               }

                         
          }

          public bool IsSourceLegal(Team player, Square squareSource)
          {
               return IsSquarePositionInBoardRange(squareSource.squarePosition) && IsManIsInCompatibleTeam(player, squareSource) ? true : false;
          }

          public bool IsSquarePositionInBoardRange(Square.SquarePosition squarePosition)
          {
               return (squarePosition.x >= 0 && squarePosition.x < activeBoard.size && squarePosition.y >= 0 && squarePosition.y < activeBoard.size) ? true : false;
          }

          public bool IsManIsInCompatibleTeam(Team manTeam, Square squarePosition)
          {
               return squarePosition.m_man != null && squarePosition.m_man.m_manTeam == manTeam ? true : false;
          }

          public bool IsManIsInOpponentTeam(Man man, Square squarePosition)
          {
               return squarePosition.m_man != null && squarePosition.m_man.m_manTeam != man.m_manTeam ? true : false;

          }

          public void ConvertUserInputToMove(string userInput, ref Square squareSource, ref Square squareDestination)
          {
               string[] splittedInput = userInput.Split('>'); ////bug
               squareSource = activeBoard.gameBoard[(int)splittedInput[0][1] - 'a', (int)splittedInput[0][0] - 'A'];
               squareDestination = activeBoard.gameBoard[(int)splittedInput[1][1] - 'a', (int)splittedInput[1][0] - 'A'];
          }

          public void RunPreGameDialog()
          {
               GetUserSettings();
          }

          public void GetUserSettings()
          {
               PrintIntroduction();
               firstPlayerUserName = GetUserName();
               activeBoard.size = GetRequestedBoardSize();
               gameMode = GetGameModeFromUser();
               if (gameMode == eGameMode.VersusAnotherPlayer)
               {
                    secondPlayerUserName = GetUserName();
               }
          }

          public void PrintIntroduction()
          {
               Console.WriteLine("Hello! Let's play CHECKERS!!! Have fun :-)");
          }

          public string GetUserName()
          {
               Console.WriteLine("Please enter user name:");
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
               return Regex.IsMatch(insertedUserName, "^[a-z,A-Z,0-9]+$") && insertedUserName.Length <= maximumUserNameSize ? true : false;
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

          public bool isLegalBoardSizeInserted(int insertedUserChoiseForBoardSize)
          {
               return (insertedUserChoiseForBoardSize == smallBoardSize || insertedUserChoiseForBoardSize == mediumBoardSize || insertedUserChoiseForBoardSize == bigBoardSize) ? true : false;
          }

          public eGameMode GetGameModeFromUser() // to do what about casting !?
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

          public int CalculateNumberOfMenInTeam(int boardSize)
          {
               return ((activeBoard.size / 2 - 1) * activeBoard.size / 2);
          }

          public void CreateNewCheckersGame()
          {
               gameStatus = eGameStatus.active;
               activeBoard.CreateNewBoard();
               player1.CreateNewTeam(Team.eTeamType.user, Team.eTeamSign.X, CalculateNumberOfMenInTeam(activeBoard.size), Team.eDirectionOfMovement.down, firstPlayerUserName);
               if (gameMode == eGameMode.VersusAnotherPlayer)
               {
                    player2.CreateNewTeam(Team.eTeamType.user, Team.eTeamSign.O, CalculateNumberOfMenInTeam(activeBoard.size), Team.eDirectionOfMovement.up, secondPlayerUserName);
               }
               else
               {
                    player2.CreateNewTeam(Team.eTeamType.computer, Team.eTeamSign.O, CalculateNumberOfMenInTeam(activeBoard.size), Team.eDirectionOfMovement.up, secondPlayerUserName);
               }
               AssignMenToTeams();
               activeBoard.PrintBoard();
               printInfo();
          }

          public void AssignMenToTeams()
          {
               int numberOfMen = 0;
               for (int i = 0; i < activeBoard.size / 2 - 1; i++)
               {
                    for (int j = 0; j < activeBoard.size; j++)
                    {
                         if (activeBoard.gameBoard[i, j].squareColor == Square.eSquareColor.Black)
                         {
                              activeBoard.gameBoard[i, j].m_man = player1.AssignManToPosition(activeBoard.gameBoard[i, j], numberOfMen);
                              numberOfMen++;
                         }
                    }
               }

               numberOfMen = 0;

               for (int i = (activeBoard.size / 2) + 1; i < activeBoard.size; i++)
               {
                    for (int j = 0; j < activeBoard.size; j++)
                    {
                         if (activeBoard.gameBoard[i, j].squareColor == Square.eSquareColor.Black)
                         {
                              activeBoard.gameBoard[i, j].m_man = player2.AssignManToPosition(activeBoard.gameBoard[i, j], numberOfMen);
                              numberOfMen++;
                         }
                    }
               }
          }

          public void printInfo() /// to do
          {
               System.Console.WriteLine("{0} Turn: ", player1.m_teamName);

          }

          public enum eGameStatus
          {
               active,
               inRound,
               endOfRound,
               inactive
          };

          public enum eDirectionType
          {
               upRight,
               upLeft,
               downRight,
               downLeft
          }

     }
}