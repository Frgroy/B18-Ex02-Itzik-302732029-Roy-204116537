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
               public int x;
               public int y;
          }
          public Man m_man;
          public SquarePosition squarePosition = new SquarePosition();
          private eSquareColor m_squareColor;
          public eSquareColor squareColor
          {
               get { return m_squareColor; }
          }

          public void InitializeSquare(int i, int j) // to do casting 1??
          {
               squarePosition.x = j;
               squarePosition.y = i;
               if ((i + j) % 2 == 0)
               {
                    m_squareColor = eSquareColor.White;
               }
               else
               {
                    m_squareColor = eSquareColor.Black;
               }
          }

          public void MakeEmpty() //todo
          {
               m_man = null;
          }

          public void AttachManToSquare(Man mannnn) //todooo
          {
               m_man = mannnn;
          }
          public enum eSquareColor
          {
               Black, White
          }

          public enum eWidthLetter
          {
               A, B, C, D, E, F, G, H, I, J
          }
          public enum eLengthLetter
          {
               a, b, c, d, e, f, g, h, i, j
          }
     }
}
