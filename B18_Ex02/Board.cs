using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02
{
     public class Board
     {
          private int m_boardSize;
          private readonly Square[,] m_actualBoard;

          public int boardSize
          {
               get { return m_boardSize; }


               set { m_boardSize = value; }
          }

          public char GetSquareContent(int i_boardLine, int i_boardColumn)
          {
               char squareContent;
               if (m_actualBoard[i_boardLine, i_boardColumn].currentMan == null)
               {
                    squareContent = ' ';
               }
               else
               {
                    squareContent = m_actualBoard[i_boardLine, i_boardColumn].currentMan.manSign;
               }

               return squareContent;
          }

          public Square GetSquare(int i_boardLine, int i_boardColumn)
          {
               return m_actualBoard[i_boardLine, i_boardColumn];
          }

          public Board(int i_BoardSize)
          {
               m_boardSize = i_BoardSize;
               m_actualBoard = new Square[m_boardSize, m_boardSize];
               for (int i = 0; i < m_boardSize; i++)
               {
                    for (int j = 0; j < m_boardSize; j++)
                    {
                         m_actualBoard[i, j] = new Square(i, j);
                    }
               }

               NeighboursAssignation();
          }

          public void ClearBoard()
          {
               for (int i = 0; i < m_boardSize; i++)
               {
                    for (int j = 0; j < m_boardSize; j++)
                    {
                         m_actualBoard[i, j].currentMan = null;
                    }
               }
          }

          private void NeighboursAssignation()
          {
               for (int i = 0; i < m_boardSize; i++)
               {
                    for (int j = 0; j < m_boardSize; j++)
                    {                     
                        if (IsSquarePositionInBoardRange(i - 1, j - 1))
                         {
                              m_actualBoard[i, j].AssignNeighbour(m_actualBoard[i - 1, j - 1], CheckersGame.ePossibleDirections.upLeft);
                         }

                         if (IsSquarePositionInBoardRange(i - 1, j + 1))
                         {
                              m_actualBoard[i, j].AssignNeighbour(m_actualBoard[i - 1, j + 1], CheckersGame.ePossibleDirections.upRight);
                         }

                         if (IsSquarePositionInBoardRange(i + 1, j - 1))
                         {
                              m_actualBoard[i, j].AssignNeighbour(m_actualBoard[i + 1 , j - 1], CheckersGame.ePossibleDirections.downLeft);
                         }

                         if (IsSquarePositionInBoardRange(i + 1, j + 1))
                         {
                              m_actualBoard[i, j].AssignNeighbour(m_actualBoard[i + 1, j + 1], CheckersGame.ePossibleDirections.downRight);
                         }
                    }
               }
          }

          public bool IsSquarePositionInBoardRange(int squareLine, int squareColumn)
          {
               return (squareLine >= 0 && squareLine < m_boardSize && squareColumn >= 0 && squareColumn< m_boardSize) ? true : false;
          }  
     }


}
