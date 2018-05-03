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

          public eGameMode gameMode
          {
               get { return m_gameMode; }
               set { m_gameMode = value; }
          }

          public eGameStatus status
          {
               get { return m_gameStatus; }
               set { m_gameStatus = value; }
          }

          public void CreateNewGame()
          {
               string player1Name;
               string player2Name;
               int gameBoardSize;
               eGameMode insertedGameMode;
               UserInterface.RunPreGameDialog(out player1Name, out player2Name, out gameBoardSize, out insertedGameMode);
               InitializeTeams(player1Name, player2Name, insertedGameMode);
               m_gameBoard = new Board(gameBoardSize);
               m_gameStatus = eGameStatus.activeGame;
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
               m_gameStatus = eGameStatus.inRound;
               m_activeTeam = m_player1;
               m_inactiveTeam = m_player2;
               RestoreTeams();
          }

          public void RestoreTeams()
          {
               m_activeTeam.teamScore = 0;
               m_inactiveTeam.teamScore = 0;
               m_activeTeam.isLeadingTeam = true;
               m_inactiveTeam.isLeadingTeam = true;
               m_activeTeam.lastMoveExecuted = null;
               m_inactiveTeam.lastMoveExecuted = null;
               m_activeTeam.DisposeMen();
               m_inactiveTeam.DisposeMen();
               AssignMenToTeams();
               UpdateMovesInTeams();
          }

          public void ManageRound()
          {
               ExecutePlayerTurn();
               SwapActiveTeam();
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

               else // m_activeTeam.teamDirectionOfMovement == Team.eDirectionOfMovement.down
               {
                    relevantLineForCrown = m_gameBoard.boardSize - 1;
               }

               m_activeTeam.CrownTeamKings(relevantLineForCrown);
          }

          public void MakeAMoveProcess(Move i_executingMove)
          {
               i_executingMove.ExecuteMove();
               m_activeTeam.lastMoveExecuted = i_executingMove;
               if (i_executingMove.IsCaptureMove())
               {
                    m_activeTeam.UpdateAttackMoves();
               }
               UserInterface.PrintGameBoard(m_gameBoard);
               UserInterface.PrintMoveInfo(m_activeTeam, m_inactiveTeam);
          }

          public void UpdateScore()
          {
               m_activeTeam.CalculateTeamScore(m_activeTeam.CalculateTeamRank(), m_inactiveTeam.CalculateTeamRank());
               m_inactiveTeam.CalculateTeamScore(m_inactiveTeam.CalculateTeamRank(), m_activeTeam.CalculateTeamRank());
               if (m_activeTeam.teamScore > m_inactiveTeam.teamScore)
               {
                    m_activeTeam.isLeadingTeam = true;
                    m_inactiveTeam.isLeadingTeam = false;
               }

               else if (m_activeTeam.teamScore == m_inactiveTeam.teamScore)
               {
                    m_activeTeam.isLeadingTeam = true;
                    m_inactiveTeam.isLeadingTeam = true;
               }

               else //m_activeTeam.teamScore < m_inactiveTeam.teamScore
               {
                    m_activeTeam.isLeadingTeam = false;
                    m_inactiveTeam.isLeadingTeam = true;
               }

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
               CrownNewKings();
               UpdateScore();
               if (IsEndOfRound())
               {
                    if (m_activeTeam.isLeadingTeam == true)
                    {
                         HandleEndOfRound(m_activeTeam);
                    }
                    else
                    {
                         HandleEndOfRound(m_inactiveTeam);
                    }
               }
          }

          public void ExecuteUserTurn()
          {
               UserInterface.PrintGameBoard(m_gameBoard);
               UserInterface.PrintMoveInfo(m_activeTeam, m_inactiveTeam);
               Move requestedMove = new Move();
               UserInterface.HandleUserInput(ref requestedMove, ref m_gameStatus, m_activeTeam);
               if (m_gameStatus == eGameStatus.inRound)
               {
                    MakeAMoveProcess(requestedMove);
                    if (requestedMove.IsCaptureMove() == true)
                    {
                         while (IsAttackMovesStillExist(requestedMove))
                         {
                              Move progressiveMove = new Move();
                              UserInterface.HandleUserInput(ref progressiveMove, ref m_gameStatus, m_activeTeam);
                              if (progressiveMove.sourceSquare.squarePosition.x == requestedMove.destinationSquare.squarePosition.x &&
                            progressiveMove.sourceSquare.squarePosition.y == requestedMove.destinationSquare.squarePosition.y)
                              {
                                   MakeAMoveProcess(progressiveMove);
                                   requestedMove = progressiveMove;
                              }
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
               Move executingMove = GenerateMoveRequest();
               MakeAMoveProcess(executingMove);
               if (executingMove.IsCaptureMove() == true)
               {
                    while (IsAttackMovesStillExist(executingMove))
                    {
                         GenerateProgressiveAttack(ref executingMove);
                         MakeAMoveProcess(executingMove);
                    }
               }
               m_activeTeam.lastMoveExecuted = executingMove;
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
                    generatedMove = m_activeTeam.regularMoves[randomMove.Next(0, m_activeTeam.regularMoves.Count)];
               }

               return generatedMove;
          }

          public void GenerateProgressiveAttack(ref Move io_executedMove)
          {
               List<Move> relevantMoves = new List<Move>();
               foreach (Move move in io_executedMove.sourceSquare.currentMan.manTeam.attackMoves)
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
               return m_inactiveTeam.armyOfMen.Count == 0 ||
                    (m_inactiveTeam.attackMoves.Count + m_inactiveTeam.regularMoves.Count == 0) ||
                    m_gameStatus == eGameStatus.roundEnd ?
                     true : false;
          }

          public void HandleEndOfRound(Team i_winningTeam)
          {
               UserInterface.RunAnotherRoundDialog(i_winningTeam, ref m_gameStatus);
          }

          public enum eGameStatus
          {
               inRound = 0,
               startingNewRound = 1,
               gameEnd = 2,
               activeGame = 3,
               roundEnd = 4
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
