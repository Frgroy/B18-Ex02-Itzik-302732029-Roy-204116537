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
                         RestoreTeams();
                    }
               }
          }

          public void CreateNewGame()
          {
               string player1Name;
               string player2Name;
               int gameBoardSize;
               eGameMode insertedGameMode;
               UserInterface.RunPreGameDialog(out player1Name, out player2Name, out gameBoardSize, out insertedGameMode);
               InitializeTeams(player1Name, player2Name, insertedGameMode);
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
               m_activeTeam.DisposeMen();
               m_inactiveTeam.DisposeMen();
               AssignMenToTeams();
               m_activeTeam.PrepareTeamMovesForNewTurn();
               m_inactiveTeam.PrepareTeamMovesForNewTurn();
          }

          public void RunRound()
          {
               while (m_gameStatus == eGameStatus.inRound)
               {
                    ExecutePlayerTurn();
                    SwapActiveTeam();
                    UserInterface.PrintGameBoard(m_gameBoard);
                    UserInterface.PrintMoveInfo(m_activeTeam, m_inactiveTeam);
               }
          }

          public void UpdateMovesInTeams()
          {
               m_activeTeam.PrepareTeamMovesForNewTurn();
               m_inactiveTeam.PrepareTeamMovesForNewTurn();
          }

          public void ExecutePlayerTurn()
          {
               UpdateMovesInTeams();
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
               if (requestedMove.capturedSquare != null)
               {
                    m_activeTeam.UpdateAttackMovesForAllTeam();
                    while (IsAttackMovesStillExist(requestedMove))
                    {
                         Move newMove = new Move();
                         UserInterface.HandleUserInput(ref newMove, m_activeTeam);
                         if (newMove.destinationSquare.squarePosition == requestedMove.sourceSquare.squarePosition)
                         {
                              newMove.ExecuteMove();
                              m_activeTeam.UpdateAttackMovesForAllTeam();
                         }
                    }
               }
          }

          public void SwapActiveTeam()
          {
               if (m_activeTeam == m_player1)
               {
                    m_activeTeam = m_player2;
                    m_inactiveTeam = m_player1;
               }

               else //m_activeTeam == m_player2
               {
                    m_activeTeam = m_player1;
                    m_inactiveTeam = m_player2;
               }
          }

          public bool IsAttackMovesStillExist(Move i_requestedMove)
          {
               bool isAttackMovesStillExist = false;
               foreach (Move activeMove in i_requestedMove.destinationSquare.currentMan.manTeam.attackMoves)
               {
                    if (i_requestedMove.destinationSquare.squarePosition.x == activeMove.sourceSquare.squarePosition.x &&
                         i_requestedMove.destinationSquare.squarePosition.y == activeMove.sourceSquare.squarePosition.y)
                    {
                         isAttackMovesStillExist = true;
                    }
               }

               return isAttackMovesStillExist;
          }

          public void ExecuteComputerTurn()
          {
               Move requestedMove = GenerateMoveRequest();
               requestedMove.ExecuteMove();
               m_activeTeam.UpdateAttackMovesForAllTeam();
               if (requestedMove.capturedSquare != null)
               {
                    while (IsAttackMovesStillExist(requestedMove))
                    {
                         GenerateProgressiveAttack(ref requestedMove);
                         requestedMove.ExecuteMove();
                         m_activeTeam.UpdateAttackMovesForAllTeam();                     
                    }
               }
          }

          public Move GenerateMoveRequest()
          {
               Random randomMove = new Random();
               Move generatedMove = new Move();
               if (m_activeTeam.attackMoves.Capacity > 0)
               {
                    generatedMove = m_activeTeam.attackMoves[randomMove.Next(0, m_activeTeam.attackMoves.Capacity)];
               }
               else
               {
                    generatedMove = m_activeTeam.regularMoves[randomMove.Next(0, m_activeTeam.regularMoves.Capacity)];
               }

               return generatedMove;
          }

          public void GenerateProgressiveAttack(ref Move io_executedMove)
          {
               List<Move> relevantMoves = new List<Move>();
               foreach (Move move in io_executedMove.sourceSquare.currentMan.manTeam.attackMoves)
               {
                    if (move.destinationSquare.squarePosition == io_executedMove.sourceSquare.squarePosition)
                    {
                         relevantMoves.Add(move);
                    }
               }
               Random randomMove = new Random();
               io_executedMove = relevantMoves[randomMove.Next(0, relevantMoves.Capacity)];
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
               VersusComputer = 2
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
