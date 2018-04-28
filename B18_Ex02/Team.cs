using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02
{
     public class Team
     {
          public string m_teamName;
          private eTeamType m_teamType;
          public eTeamSign m_teamSign;
          private bool canEat; //to do for future implementation ~!!@!#
          private int m_numberOfActiveMen;
          public Man[] m_armyOfMen;
          public eDirectionOfMovement m_direction;

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
               both
          }

          public void CreateNewTeam(eTeamType teamType, eTeamSign teamSign, int numberOfActiveMen, eDirectionOfMovement dir, string teamName)
          {
               m_teamName = teamName;
               m_teamType = teamType;
               m_teamSign = teamSign;
               m_numberOfActiveMen = numberOfActiveMen;
               m_direction = dir;
               m_armyOfMen = new Man[numberOfActiveMen];
          }

          public Man AssignManToPosition(Square square, int num)
          {
              m_armyOfMen[num] = new Man(square, m_teamSign, m_direction);
              return m_armyOfMen[num];
          }
     }
}
