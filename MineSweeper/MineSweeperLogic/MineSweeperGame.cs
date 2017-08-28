using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeperLogic
{
    public class MineSweeperGame
    {

        public MineSweeperGame(int sizeX, int sizeY, int nrOfMines, IServiceBus bus)
        {

        }

        public int PosX { get; private set; }
        public int PosY { get; private set; }
        public int SizeX { get; }
        public int SizeY { get; }
        public int NumberOfMines { get; }
        public int NumberOfTiles { get; }
        public int NumberOfOpenTiles;
        public GameState State { get; private set; }
        public MineSweeperGame Game;


        public PositionInfo GetCoordinate(int x, int y)
        {
            return null;
        }

        public void FlagCoordinate()
        {
        }

        public void ClickCoordinate()
        {
            
            var positionOfPlayer = Game.GetCoordinate(PosX, PosY);
            if (positionOfPlayer.IsOpen == true)
            {
                State = GameState.Playing;
                return;
            }
            else
            {
                positionOfPlayer.IsOpen = true;
                NumberOfOpenTiles++;
                //if (NumberOfTiles - NumberOfOpenTiles = NumberOfMines)
                //{
                //    State = GameState.Won;
                //}
                //else
                //{
                //    State = GameState.Playing;
                //}
            }

            if (positionOfPlayer.HasMine == true)
            {
                State = GameState.Lost;
            }
            else
            {
                positionOfPlayer.IsOpen = true;
                NumberOfOpenTiles++;
            }

        }

        public void ResetBoard()
        {
        }

        public void DrawBoard()
        {
        }

        #region MoveCursor Methods

        public void MoveCursorUp()
        {
        }

        public void MoveCursorDown()
        {
        }

        public void MoveCursorLeft()
        {
        }

        public void MoveCursorRight()
        {
        }

        #endregion

    }
}
