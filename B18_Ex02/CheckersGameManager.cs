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
                         ManageRound();
                    }
               }
          }

          public void ManageRound()
          {
               ManagePlayerTurn(player1);
               ManagePlayerTurn(player2);
          }

          public void ManagePlayerTurn(Team player)
          {
               HandlePlayerInput(System.Console.ReadLine(), player);
               printInfo();
          }

          public void HandlePlayerInput(string userInput, Team player) // to dodoododo
          {
               //to do find function that checks template
               int source_i = 0;
               int source_j = 0;
               int destination_i = 0;
               int destination_j = 0;
               ConvertInput(userInput, source_i, source_j, destination_i, destination_j);
               while (!IsLegalMove(player, source_i, source_j, destination_i, destination_j))
               {
                    ManagePlayerTurn(player);
               }
               Move(source_i, source_j, destination_i, destination_j);
          }

          public bool IsLegalMove(Team player, int source_i, int source_j, int destination_i, int destination_j)
          {
               if (IsSourceLegal(player, source_i, source_j))
               {
                    if (IsDestinationLegal(activeBoard.gameBoard[source_i, source_j].m_man, destination_i, destination_j))
                    {
                         return true;
                    }
               }
               return false;          
          }

          public bool IsSourceLegal(Team player, int source_i, int source_j) //format
          {
               if (source_i >= 0 && source_i < activeBoard.size && source_j >= 0 && source_j < activeBoard.size)
               {
                    if (activeBoard.gameBoard[source_i, source_j].m_man != null)
                    {
                         if (activeBoard.gameBoard[source_i, source_j].m_man.m_manTeam == player.m_teamSign)
                         {
                              return true; //change return to end
                         }
                    }
               }
               return false;
          }

          public bool IsDestinationLegal (Man man, int destination_i, int destination_j)
          {
               if (destination_i >= 0 && destination_i < activeBoard.size && destination_j >= 0 && destination_j < activeBoard.size)
               {
                    
               }
               return false;
          }

          public void Move(int source_i, int source_j, int destination_i, int destination_j)
          {

          }

          public void ConvertInput(string userInput, int source_i, int source_j, int destination_i, int  destination_j)
          {
               char[] hara = new char[20];
               hara = userInput.ToCharArray();
               source_i = (int)hara[0] - 'A';
               source_j = (int)hara[1] - 'a';
               destination_i = (int)hara[3] - 'A';
               destination_j = (int)hara[4] - 'a';
               
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

     }
}