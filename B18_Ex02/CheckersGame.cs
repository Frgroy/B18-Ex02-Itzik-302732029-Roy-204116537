using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02
{
     public class CheckersGame
     {
          private Team m_player1;
          private Team m_player2;
          private Board m_gameBoard;
          private Team m_activeTeam = new Team();
          private Team m_inactiveTeam = new Team();
          private eGameMode m_gameMode;
          private eGameStatus m_gameStatus;

          public Team activeTeam
          {
               get { return m_activeTeam; }
               set { m_activeTeam = value; }
          }

          public Team inactiveTeam
          {
               get { return m_inactiveTeam; }
               set { m_inactiveTeam = value; }
          }

          public Board board
          {
               get { return m_gameBoard; }

               set { m_gameBoard = value; }
          }

          public eGameMode mode
          {
               get { return m_gameMode; }
               set { m_gameMode = value; }
          }

          public eGameStatus status
          {
               get { return m_gameStatus; }
               set { m_gameStatus = value; }
          }

          public CheckersGame(string i_player1Name, string i_player2Name, int i_gameBoardSize, eGameMode i_gameMode)
          {
               m_gameStatus = eGameStatus.activeGame;
               m_gameMode = i_gameMode;
               m_gameBoard = new Board(i_gameBoardSize);
               InitializeTeams(i_player1Name, i_player2Name, i_gameMode);
          }

          public void InitializeTeams(string i_player1Name, string i_player2Name, eGameMode i_gameMode)
          {
               m_player1 = new Team(i_player1Name, Team.eTeamType.user, Team.eDirectionOfMovement.up, Team.eTeamSign.X);
               if (i_gameMode == eGameMode.VersusAnotherPlayer)
               {
                    m_player2 = new Team(i_player2Name, Team.eTeamType.user, Team.eDirectionOfMovement.down, Team.eTeamSign.O);
               }
               else
               {
                    m_player2 = new Team(i_player2Name, Team.eTeamType.computer, Team.eDirectionOfMovement.down, Team.eTeamSign.O);
               }
          }

          public void AssignMenToTeams()
          {
               for (int i = 0; i < (m_gameBoard.boardSize / 2) - 1; i++)
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
               m_gameStatus = eGameStatus.inRound;
               m_activeTeam = m_player1;
               m_inactiveTeam = m_player2;
               RestoreTeams();
          }

          public void RestoreTeams()
          {
               m_activeTeam.isLeadingTeam = true;
               m_inactiveTeam.isLeadingTeam = true;
               m_activeTeam.lastMoveExecuted = null;
               m_inactiveTeam.lastMoveExecuted = null;
               m_activeTeam.DisposeMen();
               m_inactiveTeam.DisposeMen();
               AssignMenToTeams();
               UpdateMovesInTeams();
          }

          public void UpdateMovesInTeams()
          {
               m_activeTeam.PrepareTeamMovesForNewTurn();
               m_inactiveTeam.PrepareTeamMovesForNewTurn();
          }

          public void CrownNewKings()
          {
               int relevantLineForCrown;
               if (m_activeTeam.teamDirectionOfMovement == Team.eDirectionOfMovement.up)
               {
                    relevantLineForCrown = 0;
               }
               else
               {
                    relevantLineForCrown = m_gameBoard.boardSize - 1;
               }

               m_activeTeam.CrownTeamKings(relevantLineForCrown);
          }

          public void MakeAMoveProcess(Move i_executingMove)
          {
               if (i_executingMove.moveOption == Move.eMoveOption.quit)
               {
                    m_gameStatus = eGameStatus.roundEnd;
               }
               else
               {
                    i_executingMove.ExecuteMove();
                    m_activeTeam.lastMoveExecuted = i_executingMove;
                    CrownNewKings();
                    UpdateLeaderTeam();
                    UpdateMovesInTeams();
               }

               if (IsEndOfRound())
               {
                    CalculateScore();
               }
          }

          public void UpdateLeaderTeam()
          {
               if (m_activeTeam.CalculateTeamRank() > m_inactiveTeam.CalculateTeamRank())
               {
                    m_activeTeam.isLeadingTeam = true;
                    m_inactiveTeam.isLeadingTeam = false;
               }
               else if (m_activeTeam.CalculateTeamRank() == m_inactiveTeam.CalculateTeamRank())
               {
                    m_activeTeam.isLeadingTeam = true;
                    m_inactiveTeam.isLeadingTeam = true;
               }
               else
               {
                    m_activeTeam.isLeadingTeam = false;
                    m_inactiveTeam.isLeadingTeam = true;
               }
          }

          public void CalculateScore()
          {
               if (m_activeTeam.isLeadingTeam == true && m_inactiveTeam.isLeadingTeam == true)
               {
                    m_activeTeam.CalculateTeamScore(m_activeTeam.CalculateTeamRank(), m_inactiveTeam.CalculateTeamRank());
                    m_inactiveTeam.CalculateTeamScore(m_inactiveTeam.CalculateTeamRank(), m_activeTeam.CalculateTeamRank());
               }
               else if (m_activeTeam.isLeadingTeam == true)
               {
                    m_activeTeam.CalculateTeamScore(m_activeTeam.CalculateTeamRank(), m_inactiveTeam.CalculateTeamRank());
               }
               else
               {
                    m_inactiveTeam.CalculateTeamScore(m_inactiveTeam.CalculateTeamRank(), m_activeTeam.CalculateTeamRank());
               }
          }

          public void SwapActiveTeam()
          {
               if (m_activeTeam == m_player1)
               {
                    m_activeTeam = m_player2;
                    m_inactiveTeam = m_player1;
               }
               else
               {
                    m_activeTeam = m_player1;
                    m_inactiveTeam = m_player2;
               }
          }

          public bool IsProgressiveMoveAvailable(Move i_requestedMove)
          {
               bool isProgressiveMoveAvailable = false;
               if (i_requestedMove.moveOption == Move.eMoveOption.attackMove)
               {
                    foreach (Move activeMove in i_requestedMove.destinationSquare.currentMan.manTeam.attackMoves)
                    {
                         if (i_requestedMove.destinationSquare.squarePosition.x == activeMove.sourceSquare.squarePosition.x &&
                              i_requestedMove.destinationSquare.squarePosition.y == activeMove.sourceSquare.squarePosition.y)
                         {
                              isProgressiveMoveAvailable = true;
                         }
                    }
               }

               return isProgressiveMoveAvailable;
          }

          public Move GenerateMoveRequest()
          {
               Random randomMove = new Random();
               Move generatedMove = new Move();
               if (m_activeTeam.attackMoves.Count > 0)
               {
                    generatedMove = m_activeTeam.attackMoves[randomMove.Next(0, m_activeTeam.attackMoves.Count)];
               }
               else
               {
                    if (m_activeTeam.regularMoves.Count > 0)
                    {
                         generatedMove = m_activeTeam.regularMoves[randomMove.Next(0, m_activeTeam.regularMoves.Count)];
                    }
               }

               return generatedMove;
          }

          public void GenerateProgressiveAttack(ref Move io_executedMove)
          {
               List<Move> relevantMoves = new List<Move>();
               foreach (Move move in io_executedMove.destinationSquare.currentMan.manTeam.attackMoves)
               {
                    if (move.sourceSquare.squarePosition.x == io_executedMove.destinationSquare.squarePosition.x &&
                         move.sourceSquare.squarePosition.y == io_executedMove.destinationSquare.squarePosition.y)
                    {
                         relevantMoves.Add(move);
                    }
               }

               Random randomMove = new Random();
               io_executedMove = relevantMoves[randomMove.Next(0, relevantMoves.Count)];
          }

          public bool IsEndOfRound()
          {
               bool isEndOfRound = false;
               if (m_activeTeam.attackMoves.Count + m_activeTeam.regularMoves.Count == 0
                    && m_inactiveTeam.attackMoves.Count + m_inactiveTeam.regularMoves.Count == 0)
               {
                    isEndOfRound = true;
                    m_gameStatus = eGameStatus.roundEndWithDraw;
               }
               else if (m_activeTeam.attackMoves.Count + m_activeTeam.regularMoves.Count == 0
                    || m_inactiveTeam.attackMoves.Count + m_inactiveTeam.regularMoves.Count == 0)
               {
                    isEndOfRound = true;
                    m_gameStatus = eGameStatus.roundEnd;
               }

               return isEndOfRound;
          }

          public enum eGameStatus
          {
               inRound,
               startingNewRound = 1,
               gameEnd = 2,
               activeGame = 3,
               roundEnd,
               roundEndWithDraw
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
