using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02
{
     public class Square
     {
          public class SquarePosition
          {
               private int m_squareLine;
               private int m_squareColumm;

               public SquarePosition(int i_squareLine, int i_squareColumn)
               {
                    m_squareLine = i_squareLine;
                    m_squareColumm = i_squareColumn;
               }

               public int x
               {
                    get { return m_squareColumm; }
                    set { m_squareColumm = value; }
               }

               public int y
               {
                    get { return m_squareLine; }
                    set { m_squareLine = value; }
               }

          }

          public class SquareNeighbours
          {
               Square m_upRightSquare;
               Square m_upLeftSquare;
               Square m_downRightSquare;
               Square m_downLeftSquare;

               public Square upRight
               {
                    get { return m_upRightSquare; }
                    set { m_upRightSquare = value; }
               }

               public Square upLeft
               {
                    get { return m_upLeftSquare; }
                    set { m_upLeftSquare = value; }
               }

               public Square downRight
               {
                    get { return m_downRightSquare; }
                    set { m_downRightSquare = value; }
               }

               public Square downLeft
               {
                    get { return m_downLeftSquare; }
                    set { m_downLeftSquare = value; }
               }
          }

          private SquarePosition m_squarePosition;
          private Man m_currentMan;
          private SquareNeighbours m_squareNeighbours = new SquareNeighbours();
          private eSquareColor m_squareColor;

          public SquarePosition squarePosition
          {
               get { return m_squarePosition; }
               set { m_squarePosition = value; }
          }

          public eSquareColor squareColor
          {
               get { return m_squareColor; }
               set { m_squareColor = value; }
          }

          public Man currentMan
          {
               get { return m_currentMan; }
               set { m_currentMan = value; }
          }

          public SquareNeighbours squareNeighbours
          {
               get { return m_squareNeighbours; }
               set { m_squareNeighbours = value; }
          }

          public void InitializeSquare(int i_squareLine, int i_squareColumm)
          {
               m_squarePosition = new SquarePosition(i_squareLine, i_squareColumm);
               if ((m_squarePosition.x + m_squarePosition.y) % 2 == 0)
               {
                    m_squareColor = eSquareColor.White;
               }
               else
               {
                    m_squareColor = eSquareColor.Black;
               }
          }

          public void AssignNeighbour(Square i_neighbourSquare, CheckersGame.ePossibleDirections i_squareDirection)
          {
               if (i_squareDirection == CheckersGame.ePossibleDirections.upLeft)
               {
                    m_squareNeighbours.upLeft = new Square();
                    m_squareNeighbours.upLeft = i_neighbourSquare;
               }

               else if (i_squareDirection == CheckersGame.ePossibleDirections.upRight)
               {
                    m_squareNeighbours.upRight = new Square();
                    m_squareNeighbours.upRight = i_neighbourSquare;
               }

               else if (i_squareDirection == CheckersGame.ePossibleDirections.downLeft)
               {
                    m_squareNeighbours.downLeft = new Square();
                    m_squareNeighbours.downLeft = i_neighbourSquare;
               }

               else // i_squareDirection == CheckersGame.ePossibleDirections.downRight
               {
                    m_squareNeighbours.downRight = new Square();
                    m_squareNeighbours.downRight = i_neighbourSquare;
               }
          }

          
          public enum eSquareColor
          {
               Black, White
          }

     }
}
