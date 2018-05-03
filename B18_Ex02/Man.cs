using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02
{
     public class Man
     {
          private const char k_upDirectionKingSign = 'K';
          private const char k_upDirectionManSign = 'X';
          private const char k_downDirectionKingSign = 'U';
          private const char k_downDirectionManSign = 'O';
          private Team m_manTeam;
          private Square m_currentPosition;
          private bool m_isKing;
          private Team.eDirectionOfMovement m_manDirection;
          private Team.eTeamSign m_manSign;
          
          public bool isKing
          {
               get { return m_isKing; }
               set { m_isKing = value; }
          }

          public Team manTeam
          {
               get { return m_manTeam; }
               set { m_manTeam = value; }
          }

          public Team.eDirectionOfMovement manDirection
          {
               get { return m_manDirection; }
               set { m_manDirection = value; }
          }

          public Square currentPosition
          {
               get { return m_currentPosition; }
               set { m_currentPosition = value; }
          }

          public char manSign
          {
               get
               {
                    char signOutput;
                    if (m_manSign == Team.eTeamSign.X)
                    {
                         if (m_isKing == true)
                         {
                              signOutput = k_upDirectionKingSign;
                         }
                         else
                         {
                              signOutput = k_upDirectionManSign;
                         }
                    }
                    else
                    {
                         if (m_isKing == true)
                         {
                              signOutput = k_downDirectionKingSign;
                         }
                         else
                         {
                              signOutput = k_downDirectionManSign;
                         }
                    }
                    return signOutput;
               }
          }

          public Man(Team i_manTeam, Square i_manPosition, Team.eDirectionOfMovement i_manDirection)
          {
               m_manTeam = i_manTeam;
               m_currentPosition = i_manPosition;
               m_manDirection = i_manDirection;
               m_manSign = i_manTeam.teamSign;
               m_isKing = false;
          }

          public void Crown()
          {
               m_isKing = true;
          }
     }
}
