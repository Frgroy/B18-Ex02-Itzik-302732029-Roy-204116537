using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02
{
     public class Man
     {
          public Team m_manTeam;
          private Square m_currentPosition;
          public bool m_isKing;
          public Team.eDirectionOfMovement m_directionOfMovement;
          
          public Man(Square position, Team manTeam, Team.eDirectionOfMovement dir)
          {
               m_currentPosition = position;
               m_manTeam = manTeam;
               m_directionOfMovement = dir;
          }

          public void Move(Square destination)
          {
               destination.AttachManToSquare(this);
               m_currentPosition.MakeEmpty();
               m_currentPosition = destination;
          }

          public void BeEaten()
          {
               m_currentPosition.MakeEmpty();
               m_manTeam.m_numberOfActiveMen--;
          }
     }
}
