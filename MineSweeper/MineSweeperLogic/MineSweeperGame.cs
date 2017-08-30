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
            NumberOfMines = nrOfMines;
            NumberOfOpenTiles = 0;
            NumberOfFlagedTiles = 0;
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
        public int NumberOfFlagedTiles { get; private set; }
        public GameState State { get; private set; }

        public PositionInfo GetCoordinate(int x, int y)
        {
            return _board[x, y];
        }

        public void FlagCoordinate()
        {
            var positionOfPlayer = GetCoordinate(PosX, PosY);

            if (!positionOfPlayer.IsOpen)
            {
                positionOfPlayer.IsFlagged ^= true;
            }
            
        }

        private void FloodFill(bool[,] escapes, int x, int y)
         {
            if (x < 0 || x >= SizeX) return;
            if (y < 0 || y >= SizeY) return;
            if (GetCoordinate(x, y).HasMine || escapes[x, y]) return;

            escapes[x, y] = true;
            GetCoordinate(x, y).IsOpen = true;
            NumberOfOpenTiles++;
            State = NumberOfTiles - NumberOfOpenTiles == NumberOfMines ? GameState.Won : GameState.Playing;

            if (GetCoordinate(x, y).NrOfNeighbours == 0)
            {
                FloodFill(escapes, x, y + 1);
                FloodFill(escapes, x, y - 1);
                FloodFill(escapes, x + 1, y);
                FloodFill(escapes, x - 1, y);
            }
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

            FloodFill(new bool[SizeX, SizeY], positionOfPlayer.X, positionOfPlayer.Y);

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
                        Y = y,
                        X = x
                    };
                }
            }
            PlacementOfMines();
            foreach (var cell in _board)
            {
                cell.NrOfNeighbours = GetNumberOfNeighbours(cell.X, cell.Y);
            }
        }

        public void PlacementOfMines ()
        {
            int mines = 0;
            if (NumberOfMines <= 0)
            {
                return;
            }

            while (mines != NumberOfMines)
            {
                var x = _bus.Next(SizeX);
                var y = _bus.Next(SizeY);

                if (GetCoordinate(x, y).HasMine) continue;
                GetCoordinate(x, y).HasMine = true;
                mines++;
            }
        }

        public PositionInfo[] GetCellsInfo(int x, int y)
        {
            var cells = new List<PositionInfo>();

            for (int X = x - 1; X <= x + 1; X++)
            {
                for (int Y = y - 1; Y <= y + 1; Y++)
                {
                    if (X == x && Y == y ||
                        X < 0 || X >= SizeX ||
                        Y < 0 || Y >= SizeY)
                        continue;
                    cells.Add(GetCoordinate(X, Y));
                }
            }
            return cells.ToArray();
        }

        private int GetNumberOfNeighbours(int x, int y)
        {
            if (x < 0 || x >= SizeX ||
                y < 0 || y >= SizeY)
            {throw new Exception("");}

            var neighbours = GetCellsInfo(x, y);
            return neighbours.Count(neighbour => neighbour.HasMine);
        }

        public void DrawBoard()
        {
            for (int y = 0; y < SizeX; y++)
            {
                for (int x = 0; x < SizeY; x++)
                {
                    if (_board[x, y].IsOpen)
                    {
                        if (x == PosX && y == PosY && !_board[x, y].HasMine && !_board[x, y].IsFlagged && _board[x, y].NrOfNeighbours == 0)
                            _bus.Write("O ", ConsoleColor.DarkCyan);
                        else if (_board[x, y].HasMine)
                        {
                            if (x == PosX && y == PosY)
                                _bus.Write("¤ ", ConsoleColor.DarkCyan);
                            else
                                _bus.Write("¤ ");
                        }
                        else if (_board[x, y].NrOfNeighbours == 1)
                        {
                            if (x == PosX && y == PosY)
                                _bus.Write("1 ", ConsoleColor.DarkCyan);
                            else
                            {
                                _bus.Write("1 ");
                            }
                        }
                        else if (_board[x, y].NrOfNeighbours == 2)
                        {
                            if (x == PosX && y == PosY)
                                _bus.Write("2 ", ConsoleColor.DarkCyan);
                            else
                            {
                                _bus.Write("2 ");
                            }
                        }
                        else if (_board[x, y].NrOfNeighbours == 3)
                        {
                            if (x == PosX && y == PosY)
                                _bus.Write("3 ", ConsoleColor.DarkCyan);
                            else
                            {
                                _bus.Write("3 ");
                            }
                        }
                        else if (_board[x, y].NrOfNeighbours == 4)
                        {
                            if (x == PosX && y == PosY)
                                _bus.Write("4 ", ConsoleColor.DarkCyan);
                            else
                            {
                                _bus.Write("4 ");
                            }
                        }
                        else if (_board[x, y].NrOfNeighbours == 5)
                        {
                            if (x == PosX && y == PosY)
                                _bus.Write("5 ", ConsoleColor.DarkCyan);
                            else
                            {
                                _bus.Write("5 ");
                            }
                        }
                        else if (_board[x, y].NrOfNeighbours == 6)
                        {
                            if (x == PosX && y == PosY)
                                _bus.Write("6 ", ConsoleColor.DarkCyan);
                            else
                            {
                                _bus.Write("6 ");
                            }
                        }
                        else if (_board[x, y].NrOfNeighbours == 7)
                        {
                            if (x == PosX && y == PosY)
                                _bus.Write("7 ", ConsoleColor.DarkCyan);
                            else
                            {
                                _bus.Write("7 ");
                            }
                        }
                        else if (_board[x, y].NrOfNeighbours == 8)
                        {
                            if (x == PosX && y == PosY)
                                _bus.Write("8 ", ConsoleColor.DarkCyan);
                            else
                            {
                                _bus.Write("8 ");
                            }
                        }
                        else
                        {
                            _bus.Write("O ");
                        }
                    }
                    else if (_board[x, y].IsFlagged)
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
            if (PosY <= SizeY * 0 - 1)
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
            if (PosX <= SizeX * 0 - 1)
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
