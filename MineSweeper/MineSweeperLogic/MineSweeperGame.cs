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
            _bus = bus;
            this.SizeY = sizeY;
            this.SizeX = sizeX;
            this.NumberOfMines = nrOfMines;
            ResetBoard();
            State = GameState.Playing;
            NumberOfOpenTiles = 0;
            NumberOfTiles = sizeY * sizeX;
        }

        private PositionInfo[,] _board;
        private IServiceBus _bus;
        public int PosX { get; private set; }
        public int PosY { get; private set; }
        public int SizeX { get; }
        public int SizeY { get; }
        public int NumberOfMines { get; }
        public int NumberOfTiles { get; }
        public int NumberOfOpenTiles { get; private set; }
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

            var positionOfPlayer = GetCoordinate(PosX, PosY);
            if (positionOfPlayer.IsOpen)
            {
                State = GameState.Playing;
            }
            else
            {
                positionOfPlayer.IsOpen = true;
                NumberOfOpenTiles++;
                State = NumberOfTiles - NumberOfOpenTiles == NumberOfMines ? GameState.Won : GameState.Playing;
            }

            if (positionOfPlayer.HasMine)
            {
                State = GameState.Lost;
            }
            else
            {
                positionOfPlayer.IsOpen = true;
                NumberOfOpenTiles++;
                State = NumberOfTiles - NumberOfOpenTiles == NumberOfMines ? GameState.Won : GameState.Playing;
            }

            if (positionOfPlayer.IsFlagged)
            {
                State = GameState.Playing;
            }
            else
            {
                positionOfPlayer.IsOpen = true;
                NumberOfOpenTiles++;
                State = NumberOfTiles - NumberOfOpenTiles == NumberOfMines ? GameState.Won : GameState.Playing;
            }


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
            for (int y = 0; y < SizeX; y++)
            {
                for (int x = 0; x < SizeY; x++)
                {
                    _bus.Write(" ? ");
                }
                _bus.WriteLine();
            }
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
