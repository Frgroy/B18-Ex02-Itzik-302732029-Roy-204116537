using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02
{
     public class CheckersGame
     {
          private Team m_player1 = new Team();
          private Team m_player2 = new Team();
          private Board m_gameBoard = new Board();
          private Team m_activeTeam = new Team();
          private Team m_inactiveTeam = new Team();
          private eGameMode m_gameMode;
          private eGameStatus m_gameStatus;

          public eGameMode gameMode
          {
               get { return m_gameMode; }
               set { m_gameMode = value; }
          }

          public void Run()
          {
               CreateNewGame();
               while (m_gameStatus == eGameStatus.active)
               {
                    CreateNewRound();
                    RunRound();
                    if (m_gameStatus == eGameStatus.endOfRound)
                    {
                         m_gameStatus = eGameStatus.active;
                    }
               }
          }

          public void CreateNewGame()
          {
               string player1Name;
               string player2Name;
               int gameBoardSize;
               UserInterface.RunPreGameDialog(out player1Name, out player2Name, out gameBoardSize, gameMode);
               InitializeTeams(player1Name, player2Name, gameMode);
               m_gameBoard.InitializeBoard(gameBoardSize);
               m_gameStatus = eGameStatus.active;
          }

          public void InitializeTeams(string i_player1Name, string i_player2Name, eGameMode i_gameMode)
          {
               m_player1.InitializeTeam(i_player1Name, Team.eTeamType.user, Team.eDirectionOfMovement.up, Team.eTeamSign.X);
               if (i_gameMode == eGameMode.VersusAnotherPlayer)
               {
                    m_player2.InitializeTeam(i_player2Name, Team.eTeamType.user, Team.eDirectionOfMovement.down, Team.eTeamSign.O);
               }

               else
               {
                    m_player2.InitializeTeam(i_player2Name, Team.eTeamType.computer, Team.eDirectionOfMovement.down, Team.eTeamSign.O);
               }
          }

          public void AssignMenToTeams()
          {
               for (int i = 0; i < m_gameBoard.boardSize / 2 - 1; i++)
               {
                    for (int j = 0; j < m_gameBoard.boardSize; j++)
                    {
                         if (m_gameBoard.GetSquare(i, j).squareColor == Square.eSquareColor.Black)
                         {
                              m_player2.AssignManToSquare(m_gameBoard.GetSquare(i, j));
                         }
                    }
               }

               for (int i = (m_gameBoard.boardSize / 2) + 1; i < m_gameBoard.boardSize; i++)
               {
                    for (int j = 0; j < m_gameBoard.boardSize; j++)
                    {
                         if (m_gameBoard.GetSquare(i, j).squareColor == Square.eSquareColor.Black)
                         {
                              m_player1.AssignManToSquare(m_gameBoard.GetSquare(i, j));
                         }
                    }
               }
          }

          public void CreateNewRound()
          {
               m_gameBoard.ClearBoard();
               AssignMenToTeams();
               UserInterface.PrintGameBoard(m_gameBoard);
               m_gameStatus = eGameStatus.inRound;
               m_activeTeam = m_player1;
               UserInterface.PrintFirstMoveInfo(m_activeTeam);
          }

          public void RestoreTeams()
          {
               DisposeMenFromTeams();
               AssignMenToTeams();
          }

          public void DisposeMenFromTeams()
          {
               m_player1.DisposeMen();
               m_player2.DisposeMen();
          }

          public void RunRound()
          {
               while (m_gameStatus == eGameStatus.inRound)
               {
                    Ex02.ConsoleUtils.Screen.Clear();
                    UserInterface.PrintGameBoard(m_gameBoard);
                    UserInterface.PrintMoveInfo(m_activeTeam, m_inactiveTeam);
                    ExecutePlayerTurn();
               }
          }

          public void ExecutePlayerTurn()
          {
               m_activeTeam.UpdateAttackMovesForAllTeam();
               m_activeTeam.UpdateRegularMovesForAllTeam();
               if (m_activeTeam.teamType == Team.eTeamType.user)
               {
                    ExecuteUserTurn();
               }

               else //m_activeTeam.teamType == Team.eTeamType.computer
               {
                    ExecuteComputerTurn();
               }
          }

          public void ExecuteUserTurn()
          {
               Move requestedMove = new Move();
               UserInterface.HandleUserInput(ref requestedMove, m_activeTeam);
               requestedMove.ExecuteMove();               
          }

          public void ExecuteComputerTurn()
          {
               Move playerCurrentMove = GenerateMoveRequest();
          }

          public Move GenerateMoveRequest()
          {

               return null;
          }

          public enum eGameStatus
          {
               active,
               inRound,
               endOfRound,
               inactive
          }

          public enum eGameMode
          {
               VersusAnotherPlayer = 1,
               VersusComputer
          }

          public enum ePossibleDirections
          {
               upLeft,
               upRight,
               downLeft,
               downRight
          }
     }
}
