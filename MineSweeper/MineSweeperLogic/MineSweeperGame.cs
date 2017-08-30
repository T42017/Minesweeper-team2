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
            _board = new PositionInfo[sizeX,sizeY];
            this.NumberOfMines = nrOfMines;
            NumberOfOpenTiles = 0;
            NumberOfTiles = sizeY * sizeX;
            PosX = 0;
            PosY = 0;
            _bus = bus;
            ResetBoard();
            State = GameState.Playing;           
        }

        private PositionInfo[,] _board;
        private IServiceBus _bus;
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int SizeX => _board.GetLength(0);
        public int SizeY => _board.GetLength(1);
        public int NumberOfMines { get; }
        public int NumberOfTiles { get; }
        public int NumberOfOpenTiles { get; private set; }
        public GameState State { get; private set; }

        public PositionInfo GetCoordinate(int x, int y)
        {
            return _board[x, y];
        }

        public void FlagCoordinate()
        {
        }

        public void ClickCoordinate()
        {

            var positionOfPlayer = GetCoordinate(PosX, PosY);

            if (positionOfPlayer.IsOpen)
            {
                return;
            }

            if (positionOfPlayer.IsFlagged)
            {
                return;
            }

            if (positionOfPlayer.IsOpen == false)
            {
                positionOfPlayer.IsOpen = true;
                NumberOfOpenTiles++;
                State = NumberOfTiles - NumberOfOpenTiles == NumberOfMines ? GameState.Won : GameState.Playing;
            }

            if (positionOfPlayer.HasMine)
            {
                foreach (var cell in _board)
                {
                    if (cell.HasMine)
                    {
                        cell.IsOpen = true;
                    }
                }
                State = GameState.Lost;
            }   
        }

        public void ResetBoard()
        {
            State = GameState.Playing;

            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    _board[x, y] = new PositionInfo()
                    {
                        HasMine = false,
                        IsFlagged = false,
                        IsOpen = false,
                        NrOfNeighbours = 1,
                        Y = y,
                        X = x
                    };
                }
            }
            PlacementOfMines();
        }

        public void PlacementOfMines ()
        {
            int mines = 0;
            do
            {
                var x = _bus.Next(SizeX);
                var y = _bus.Next(SizeY);

                if (GetCoordinate(x, y).HasMine) continue;
                GetCoordinate(x, y).HasMine = true;
                mines++;
            }
            while (mines < NumberOfMines);
        }

        public void DrawBoard()
        {
            for (int y = 0; y < SizeX; y++)
            {
                for (int x = 0; x < SizeY; x++)
                {
                    if (_board[x, y].IsOpen)
                    {
                        if (x == PosX && y == PosY && !_board[x,y].HasMine)
                            _bus.Write("O ", ConsoleColor.DarkCyan);
                        else if (_board[x, y].HasMine)
                        {
                            if (x == PosX && y == PosY)
                                _bus.Write("¤ ", ConsoleColor.DarkCyan);
                            else
                                _bus.Write("¤ ");
                        }
                        else if (_board[x,y].IsFlagged)
                        {
                            if (x == PosX && y == PosY)
                                _bus.Write("F ", ConsoleColor.DarkCyan);
                            else
                            {
                                _bus.Write("F ");
                            }
                        }
                        else
                        {
                            _bus.Write("O ");
                        }
                    }
                    else
                    {
                        if (x == PosX && y == PosY)
                            _bus.Write("? ", ConsoleColor.Cyan);
                        else
                            _bus.Write("? ");
                    }
                    
                }

                _bus.WriteLine();

            }
          
            
        }
       
        #region MoveCursor Methods

        public void MoveCursorUp()
        {
            PosY -= 1;
            if (PosY <= SizeY * 0 -1)
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
            if (PosX <= SizeX * 0-1)
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
}
