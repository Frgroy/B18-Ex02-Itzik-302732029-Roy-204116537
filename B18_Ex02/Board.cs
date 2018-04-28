using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02
{
     using Ex02.ConsoleUtils;
     class Board
     {
          const int smallBoardSize = 6;
          const int regularBoardSize = 8;
          const int bigBoardSize = 10;
          private int m_size;
          public Square[,] gameBoard; //to do change to priate
          public int size
          {
               get { return m_size; }


               set { m_size = value; }
          }

          public void CreateNewBoard()
          {
               gameBoard = new Square[m_size, m_size];
               for (int i = 0; i < m_size; i++)
               {
                    for (int j = 0; j < m_size; j++)
                    {
                         gameBoard[i, j] = new Square();
                         gameBoard[i, j].InitializeSquare(i, j);
                    }
               }
          }

          public void PrintBoard()
          {
               Ex02.ConsoleUtils.Screen.Clear();
               for (int i = 0; i < m_size; i++)
               {
                    for (int j = 0; j < m_size; j++)
                    {
                         if (gameBoard[i, j].m_man == null)
                         {
                              System.Console.Write(" ");
                         }

                         else if (gameBoard[i, j].m_man.m_manTeam == Team.eTeamSign.O)
                         {
                              System.Console.Write("O");
                         }

                         else
                         {
                              System.Console.Write("X");
                         }
                    }
                    System.Console.WriteLine();
               }
          }

          public Man GetValueFromWidthAndLength(int i, int j)
          {
               Square.eLengthLetter l = (Square.eLengthLetter)i - 'A';
               Square.eLengthLetter w = (Square.eLengthLetter)j - 'a';
               return gameBoard[(int)l, (int)w].m_man;
          }
     }
}