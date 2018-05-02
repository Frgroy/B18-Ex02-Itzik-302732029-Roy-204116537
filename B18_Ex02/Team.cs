using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02
{
     public class Team
     {
          private string m_teamName;
          private eTeamType m_teamType;
          private eTeamSign m_teamSign;
          private eDirectionOfMovement m_direction;
          private int m_teamScore;
          private bool m_isActive;
          private Move m_lastMoveExecuted;
          private LinkedList<Man> m_armyOfMen = new LinkedList<Man>();
          private List<Move> m_attackMoves = new List<Move>();
          private List<Move> m_regularMoves = new List<Move>();

          public string teamName
          {
               get { return m_teamName; }
               set { m_teamName = value; }
          }

          public Move lastMoveExecuted
          {
               get { return m_lastMoveExecuted; }
               set { m_lastMoveExecuted = value; }
          }

          public eTeamSign teamSign
          {
               get { return m_teamSign; }
               set { m_teamSign = value; }
          }

          public eTeamType teamType
          {
               get { return m_teamType; }
               set { m_teamType = value; }
          }

          public LinkedList<Man> armyOfMen
          {
               get { return m_armyOfMen; }
               set { m_armyOfMen = value; }
          }

          public List<Move> attackMoves
          {
               get { return m_attackMoves; }
               set { m_attackMoves = value; }
          }

          public List<Move> regularMoves
          {
               get { return m_regularMoves; }
               set { m_regularMoves = value; }
          }

          public void InitializeTeam(string i_playerName, eTeamType i_teamType, eDirectionOfMovement i_teamDirection, eTeamSign i_teamSign)
          {
               m_teamName = i_playerName;
               m_teamType = i_teamType;
               m_direction = i_teamDirection;
               m_teamSign = i_teamSign;
               m_teamScore = 0;
          }

          public void DisposeMen()
          {
               m_armyOfMen.Clear();
          }

          public void AssignManToSquare(Square i_manSquare)
          {
               Man recruitedMan = new Man();
               recruitedMan.CreateNewMan(this, i_manSquare, m_direction);
               m_armyOfMen.AddLast(recruitedMan);
               i_manSquare.currentMan = recruitedMan;
          }

          public void PrepareTeamMovesForNewTurn()
          {
               UpdateAttackMovesForAllTeam();
               UpdateRegularMovesForAllTeam();
          }

          public void UpdateAttackMovesForAllTeam()
          {
               m_attackMoves.Clear();
               foreach (Man man in m_armyOfMen)
               {
                    UpdateAttackMoveForSquare(man.currentPosition);
               }
          }

          public void UpdateAttackMoveForSquare(Square i_squareToBeUpdated)
          {
               if (i_squareToBeUpdated.currentMan.isKing)
               {
                    UpdateUpAttacks(i_squareToBeUpdated);
                    UpdateDownAttacks(i_squareToBeUpdated);
               }

               else if (i_squareToBeUpdated.currentMan.manDirection == Team.eDirectionOfMovement.up)
               {
                    UpdateUpAttacks(i_squareToBeUpdated);
               }

               else //squareToBeUpdated.currentMan.manDirection == Team.eDirectionOfMovement.down
               {
                    UpdateDownAttacks(i_squareToBeUpdated);
               }
          }

          public void UpdateUpAttacks(Square i_squareToBeUpdated)
          {
               if (i_squareToBeUpdated.squareNeighbours.upLeft != null)
               {
                    Square upLeftSquare = new Square();
                    upLeftSquare = i_squareToBeUpdated.squareNeighbours.upLeft;
                    if (upLeftSquare.currentMan != null)
                    {
                         if (upLeftSquare.currentMan.manTeam != i_squareToBeUpdated.currentMan.manTeam)
                         {
                              if (upLeftSquare.squareNeighbours.upLeft != null)
                              {
                                   if (upLeftSquare.squareNeighbours.upLeft.currentMan == null)
                                   {
                                        Move addedMoveToAttackList = new Move(i_squareToBeUpdated, upLeftSquare, upLeftSquare.squareNeighbours.upLeft);
                                        m_attackMoves.Add(addedMoveToAttackList);
                                   }
                              }
                         }
                    }
               }

               if (i_squareToBeUpdated.squareNeighbours.upRight != null)
               {
                    Square upRightSquare = new Square();
                    upRightSquare = i_squareToBeUpdated.squareNeighbours.upRight;
                    if (upRightSquare.currentMan != null)
                    {
                         if (upRightSquare.currentMan.manTeam != i_squareToBeUpdated.currentMan.manTeam)
                         {
                              if (upRightSquare.squareNeighbours.upRight != null)
                              {
                                   if (upRightSquare.squareNeighbours.upRight.currentMan == null)
                                   {
                                        Move addedMoveToAttackList = new Move(i_squareToBeUpdated, upRightSquare, upRightSquare.squareNeighbours.upRight);
                                        m_attackMoves.Add(addedMoveToAttackList);
                                   }
                              }
                         }
                    }
               }


          }

          public void UpdateDownAttacks(Square i_squareToBeUpdated)
          {
               if (i_squareToBeUpdated.squareNeighbours.downLeft != null)
               {
                    Square downLeftSquare = new Square();
                    downLeftSquare = i_squareToBeUpdated.squareNeighbours.downLeft;
                    if (downLeftSquare.currentMan != null)
                    {
                         if (downLeftSquare.currentMan.manTeam != i_squareToBeUpdated.currentMan.manTeam)
                         {
                              if (downLeftSquare.squareNeighbours.downLeft != null)
                              {
                                   if (downLeftSquare.squareNeighbours.downLeft.currentMan == null)
                                   {
                                        Move addedMoveToAttackList = new Move(i_squareToBeUpdated, downLeftSquare, downLeftSquare.squareNeighbours.downLeft);
                                        m_attackMoves.Add(addedMoveToAttackList);
                                   }
                              }
                         }
                    }
               }

               if (i_squareToBeUpdated.squareNeighbours.downRight != null)
               {
                    Square downRightSquare = new Square();
                    downRightSquare = i_squareToBeUpdated.squareNeighbours.downRight;
                    if (downRightSquare.currentMan != null)
                    {
                         if (downRightSquare.currentMan.manTeam != i_squareToBeUpdated.currentMan.manTeam)
                         {
                              if (downRightSquare.squareNeighbours.downRight != null)
                              {
                                   if (downRightSquare.squareNeighbours.downRight.currentMan == null)
                                   {
                                        Move addedMoveToAttackList = new Move(i_squareToBeUpdated, downRightSquare, downRightSquare.squareNeighbours.downRight);
                                        m_attackMoves.Add(addedMoveToAttackList);
                                   }
                              }
                         }
                    }
               }
          }

          public void UpdateRegularMovesForAllTeam()
          {
               m_regularMoves.Clear();
               foreach (Man man in m_armyOfMen)
               {
                    UpdateRegularMoveForSquare(man.currentPosition);
               }
          }

          public void UpdateRegularMoveForSquare(Square i_squareToBeUpdated)
          {
               if (i_squareToBeUpdated.currentMan.isKing)
               {
                    UpdateUpMoves(i_squareToBeUpdated);
                    UpdateDownMoves(i_squareToBeUpdated);
               }

               else if (i_squareToBeUpdated.currentMan.manDirection == Team.eDirectionOfMovement.up)
               {
                    UpdateUpMoves(i_squareToBeUpdated);
               }

               else //squareToBeUpdated.currentMan.manDirection == Team.eDirectionOfMovement.down
               {
                    UpdateDownMoves(i_squareToBeUpdated);
               }
          }

          public void UpdateUpMoves(Square i_squareToBeUpdated)
          {
               if (i_squareToBeUpdated.squareNeighbours.upLeft != null)
               {
                    if (i_squareToBeUpdated.squareNeighbours.upLeft.currentMan == null)
                    {
                         Square upLeftSquare = new Square();
                         upLeftSquare = i_squareToBeUpdated.squareNeighbours.upLeft;
                         Move addedMoveToMoveList = new Move(ref i_squareToBeUpdated, ref upLeftSquare);
                         m_regularMoves.Add(addedMoveToMoveList);
                    }
               }

               if (i_squareToBeUpdated.squareNeighbours.upRight != null)
               {
                    if (i_squareToBeUpdated.squareNeighbours.upRight.currentMan == null)
                    {
                         Square upRightSquare = new Square();
                         upRightSquare = i_squareToBeUpdated.squareNeighbours.upRight;
                         Move addedMoveToMoveList = new Move(ref i_squareToBeUpdated, ref upRightSquare);
                         m_regularMoves.Add(addedMoveToMoveList);
                    }
               }
          }

          public void UpdateDownMoves(Square i_squareToBeUpdated)
          {
               if (i_squareToBeUpdated.squareNeighbours.downLeft != null)
               {
                    if (i_squareToBeUpdated.squareNeighbours.downLeft.currentMan == null)
                    {
                         Square downLeftSquare = new Square();
                         downLeftSquare = i_squareToBeUpdated.squareNeighbours.downLeft;
                         Move addedMoveToMoveList = new Move(ref i_squareToBeUpdated, ref downLeftSquare);
                         m_regularMoves.Add(addedMoveToMoveList);
                    }
               }

               if (i_squareToBeUpdated.squareNeighbours.downRight != null)
               {
                    if (i_squareToBeUpdated.squareNeighbours.downRight.currentMan == null)
                    {
                         Square downRightSquare = new Square();
                         downRightSquare = i_squareToBeUpdated.squareNeighbours.downRight;
                         Move addedMoveToMoveList = new Move(ref i_squareToBeUpdated, ref downRightSquare);
                         m_regularMoves.Add(addedMoveToMoveList);
                    }
               }
          }

          public enum eTeamType
          {
               user,
               computer
          }

          public enum eTeamSign
          {
               X,
               O
          }

          public enum eDirectionOfMovement
          {
               up,
               down,
          }
     }
}
