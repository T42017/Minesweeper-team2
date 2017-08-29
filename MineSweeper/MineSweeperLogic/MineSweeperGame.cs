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
            this.SizeY = sizeY;
            this.SizeX = sizeX;
            this.NumberOfMines = nrOfMines;
            ResetBoard();
            State = GameState.Playing;
            NumberOfOpenTiles = 0;
            NumberOfTiles = sizeY * sizeX;
        }

        private PositionInfo[,] _board;

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

        //public void ConstructorShouldSetSize()
        //{
            
        //    var game = new MineSweeperGame(10, 10, 10, new ServiceBus());
            
           
        //   .AreEqual(game.SizeX, 10);
        //    .AreEqual(game.SizeY, 10);
        //}

       
        //public void ConstructorShouldSetCorrectSizeX()
        //{
      
        //    var game = new MineSweeperGame(5, 6, 10, new ServiceBus());

            
        //   .AreEqual(game.SizeX, 5);
        //}


    //public void ConstructorShouldSetCorrectSizeY()
    //{

    //    var game = new MineSweeperGame(5, 6, 10, new ServiceBus());


    //    .AreEqual(game.SizeY, 6);
    //}


    //public void ConstructorShouldSetCorrectMineCount()
    //{

    //    var game = new MineSweeperGame(5, 6, 10, new ServiceBus());


    //    .AreEqual(game.NumberOfMines, 10);
    //}


    //public void ConstructorShouldStartInStatePlaying()
    //{

    //    var game = new MineSweeperGame(5, 6, 10, new ServiceBus());


    //    Assert.AreEqual(game.State, GameState.Playing);
    //}


    //public void ConstructorShouldCallResetBoard()
    {

        var game = new MineSweeperGame(5, 6, 10, new ServiceBus());


        var coord = game.GetCoordinate(3, 3);

        ResetBoard



        //}
    }
}

;
}
