using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeperLogic
{
    public class MineSweeperGame
    {

        public MineSweeperGame(int sizeX, int sizeY, int nrOfMines, IServiceBus bus)
        {
            this.SizeY = sizeY;
            this.SizeX = sizeX;
            this.NumberOfMines = nrOfMines;
            ResetBoard();
            State=GameState.Playing;
        }

        private PositionInfo[,] _board;

        public int PosX { get; private set; }
        public int PosY { get; private set; }
        public int SizeX { get; }
        public int SizeY { get; }
        public int NumberOfMines { get; }
        public GameState State { get; private set; }

        public PositionInfo GetCoordinate(int x, int y)
        {
            return _board[x,y];
        }

        public void FlagCoordinate()
        {
        }

        public void ClickCoordinate()
        {
        }

        public void ResetBoard()
        {
            State = GameState.Playing;
            _board= new PositionInfo[SizeX,SizeY];

            for(int x= 0; x<SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    _board[x, y] = new PositionInfo();
                }
            }
        }

        public void DrawBoard()
        {

        }

        #region MoveCursor Methods

        public void MoveCursorUp()
        {
            PosY -= 1;
            if (PosY <= SizeY * 0)
            {
                PosY += 1;
            }
        }

        public void MoveCursorDown()
        {
            PosY += 1;
            if (PosY >= SizeY)
            {
                PosY -= 1;
            }
        }

        public void MoveCursorLeft()
        {
            PosX -= 1;
            if (PosX <= SizeX * 0)
            {
                PosX += 1;
            }
        }

        public void MoveCursorRight()
        {
            PosX += 1;
            if (PosX >= SizeX)
            {
                PosX -= 1;
            }
        }

        #endregion
      



    }

;
}
