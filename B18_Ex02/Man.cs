using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02
{
     public class Man
     {
          public Team.eTeamSign m_manTeam;
          private Square m_currentPosition;
          Team.eDirectionOfMovement m_direction;
          
          public Man(Square position, Team.eTeamSign manTeam, Team.eDirectionOfMovement dir)
          {
               m_currentPosition = position;
               m_manTeam = manTeam;
               m_direction = dir;
          }

          public void Move(Square source, Square destination)
          {
               destination.AttachManToSquare(source.m_man);
               source.MakeEmpty();
               m_currentPosition = destination;
          }
           
     }
}
