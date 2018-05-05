using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02
{
     public class Move
     {
          private Square m_sourceSquare;
          private Square m_capturedSquare;
          private Square m_destinationSquare;
          private eMoveOption m_moveOption;

          public eMoveOption moveOption
          {
               get { return m_moveOption; }
               set { m_moveOption = value; }
          }

          public Move()
          {
               m_sourceSquare = new Square();
               m_destinationSquare = new Square();
          }

          public Move(Square i_sourceSquare, Square i_destinationSquare)
          {
               m_sourceSquare = i_sourceSquare;
               m_destinationSquare = i_destinationSquare;
               m_moveOption = eMoveOption.move;
          }

          public Move(Square i_sourceSquare, Square i_capturedSquare, Square i_destinationSquare, eMoveOption i_moveOption)
          {
               m_sourceSquare = i_sourceSquare;
               m_capturedSquare = i_capturedSquare;
               m_destinationSquare = i_destinationSquare;
               m_moveOption = i_moveOption;
          }

          public Move(eMoveOption i_moveOption)
          {
               m_moveOption = i_moveOption;
          }

          public Square sourceSquare
          {
               get { return m_sourceSquare; }
               set { m_sourceSquare = value; }
          }

          public Square capturedSquare
          {
               get { return m_capturedSquare; }
               set { m_capturedSquare = value; }
          }

          public Square destinationSquare
          {
               get { return m_destinationSquare; }
               set { m_destinationSquare = value; }
          }

          public new string ToString()
          {
               const string seperatorInMoveString = ">";
               char sourceLengthLetter = (char)(m_sourceSquare.squarePosition.x + (int)'A');
               char sourceWidthLetter = (char)(m_sourceSquare.squarePosition.y + (int)'a');
               char destinationLengthLetter = (char)(m_destinationSquare.squarePosition.x + (int)'A');
               char destinationWidthLetter = (char)(m_destinationSquare.squarePosition.y + (int)'a');
               string stringOfMove = string.Format(
                    "{1}{2}{0}{3}{4}",
                    seperatorInMoveString,
                    sourceLengthLetter.ToString(),
                    sourceWidthLetter.ToString(),
                    destinationLengthLetter.ToString(),
                    destinationWidthLetter.ToString());

               return stringOfMove;
          }

          public Move Parse(Move.eMoveOption moveOption)
          {
               Move requestedPlayerMove = new Move(moveOption);

               return requestedPlayerMove;
          }

          public Move Parse(string i_userInput, Team i_activeTeam)
          {
               string[] splittedInput = i_userInput.Split('>');
               Square sourceSquare = new Square((int)splittedInput[0][1] - 'a', (int)splittedInput[0][0] - 'A');
               Square destinationSquare = new Square((int)splittedInput[1][1] - 'a', (int)splittedInput[1][0] - 'A');
               Move requestedPlayerMove = new Move(sourceSquare, destinationSquare);

               foreach (Move attackMove in i_activeTeam.attackMoves)
               {
                    if (attackMove.sourceSquare.squarePosition.x == sourceSquare.squarePosition.x &&
                         attackMove.sourceSquare.squarePosition.y == sourceSquare.squarePosition.y &&
                         attackMove.destinationSquare.squarePosition.x == destinationSquare.squarePosition.x &&
                         attackMove.destinationSquare.squarePosition.y == destinationSquare.squarePosition.y)
                    {
                         requestedPlayerMove = attackMove;
                         requestedPlayerMove.moveOption = eMoveOption.attackMove;
                    }
               }

               foreach (Move regularMove in i_activeTeam.regularMoves)
               {
                    if (regularMove.sourceSquare.squarePosition.y == sourceSquare.squarePosition.x &&
                         regularMove.sourceSquare.squarePosition.y == sourceSquare.squarePosition.y &&
                         regularMove.destinationSquare.squarePosition.x == destinationSquare.squarePosition.x &&
                         regularMove.destinationSquare.squarePosition.y == destinationSquare.squarePosition.y)
                    {
                         requestedPlayerMove = regularMove;
                         requestedPlayerMove.moveOption = eMoveOption.move;
                    }
               }

               return requestedPlayerMove;
          }

          public bool TryParse(string i_userInput, ref Move i_requestedMove, Team i_activeTeam)
          {
               bool canConvertInputToMove = false;
               i_requestedMove = i_requestedMove.Parse(i_userInput, i_activeTeam);
               if (i_requestedMove.IsLegalMove(ref i_requestedMove, i_activeTeam))
               {
                    canConvertInputToMove = true;
               }

               return canConvertInputToMove;
          }

          public bool TryParse(Move.eMoveOption moveOption, ref Move i_requestedMove, Team i_activeTeam)
          {
               bool canConvertInputToMove = false;
               if (i_activeTeam.IsTeamCanQuit())
               {
                    i_requestedMove = i_requestedMove.Parse(Move.eMoveOption.quit);
                    canConvertInputToMove = true;
               }

               return canConvertInputToMove;
          }

          public bool IsLegalMove(ref Move i_userRequestForMove, Team i_activeTeam)
          {
               bool isLegalMove = false;

               isLegalMove = IsAttackMove(i_userRequestForMove, i_activeTeam);
               if (isLegalMove == false && i_activeTeam.attackMoves.Count == 0)
               {
                    isLegalMove = CheckRegularMoves(ref i_userRequestForMove, i_activeTeam);
               }

               return isLegalMove;
          }

          public bool IsAttackMove(Move io_userRequestForMove, Team i_activeTeam)
          {
               bool isLegalAttackMove = false;
               foreach (Move availableMove in i_activeTeam.attackMoves)
               {
                    if (IsMoveMatchToMoveInMovesList(ref io_userRequestForMove, availableMove))
                    {
                         isLegalAttackMove = true;
                    }
               }

               return isLegalAttackMove;
          }

          public bool CheckRegularMoves(ref Move io_userRequestForMove, Team i_activeTeam)
          {
               bool isLegalRegularMove = false;
               foreach (Move availableMove in i_activeTeam.regularMoves)
               {
                    if (IsMoveMatchToMoveInMovesList(ref io_userRequestForMove, availableMove))
                    {
                         isLegalRegularMove = true;
                    }
               }

               return isLegalRegularMove;
          }

          public bool IsMoveMatchToMoveInMovesList(ref Move i_userRequestForMove, Move i_availableMove)
          {
               bool isMoveMatchToMoveInMovesList = false;
               if (i_availableMove.m_destinationSquare.squarePosition.x == i_userRequestForMove.m_destinationSquare.squarePosition.x &&
                    i_availableMove.m_destinationSquare.squarePosition.y == i_userRequestForMove.m_destinationSquare.squarePosition.y &&
                    i_availableMove.m_sourceSquare.squarePosition.x == i_userRequestForMove.m_sourceSquare.squarePosition.x &&
                   i_availableMove.m_sourceSquare.squarePosition.y == i_userRequestForMove.m_sourceSquare.squarePosition.y)
               {
                    i_userRequestForMove = i_availableMove;
                    isMoveMatchToMoveInMovesList = true;
               }

               return isMoveMatchToMoveInMovesList;
          }

          public void ExecuteMove()
          {
               m_destinationSquare.currentMan = m_sourceSquare.currentMan;
               m_sourceSquare.currentMan.currentPosition = m_destinationSquare;
               m_sourceSquare.currentMan = null;
               if (m_moveOption == eMoveOption.attackMove)
               {
                    m_capturedSquare.currentMan.manTeam.armyOfMen.Remove(m_capturedSquare.currentMan);
                    m_capturedSquare.currentMan = null;
               }
          }

          public enum eMoveOption
          {
               quit,
               move,
               attackMove,
          }
     }
}
