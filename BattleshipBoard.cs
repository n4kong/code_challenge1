using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeChallenge1
{
    public enum Result { HIT, MISS, MISSION_COMPLETED };

    public interface IDisplay
    {
        void UpdateDisplay(Guid id, int column, int row, Result result);
    }
    public interface IFireable
    {
        Result Fire(int column, int row);
    }
    public interface IBattleshipAi
    {
        void Play(IFireable fireable);
        string GetTeamName();
    }
    public class BattleshipBoard : IFireable
    {
        private const int MAX_TOTAL_HIT = 17;
        private const int START_INDEX = 1;
        private const int END_INDEX = 10;
        private readonly int[] SHIPS = { 5, 4, 3, 3, 2 };
        private readonly Random RANDOM = new Random();
        private Guid Id = Guid.NewGuid();

        private int hitCount = 0;
        private string[,] boardOriginal;
        private string[,] board = new string[10, 10] 
        {
            {".","X",".",".",".",".",".",".",".","."},
            {".","X",".",".",".",".","X","X","X","."},
            {".","X",".",".",".",".",".",".","X","."},
            {".","X",".",".",".",".",".",".","X","."},
            {".","X",".","X","X",".",".",".","X","."},
            {".",".",".",".",".",".",".","X",".","."},
            {".",".",".",".",".",".",".","X",".","."},
            {".",".",".",".",".",".",".","X",".","."},
            {".",".",".",".",".",".",".","X",".","."},
            {".",".",".",".",".",".",".",".",".","."}
        };

        public BattleshipBoard()
        {
            hitCount = 0;
            boardOriginal = (string[,]) board.Clone();
        }
        private IDisplay display;
        public BattleshipBoard(IDisplay display) : this()
        {
            this.display = display;
        }

        public Result Fire(int column, int row)
        {
            var result = fireShip(column, row);

            if (display != null)
                display.UpdateDisplay(Id, column, row, result);

            return result;
        }

        private Result fireShip(int column, int row) 
        {
            if (column < START_INDEX || column > END_INDEX || row < START_INDEX || row > END_INDEX)
                throw new ArgumentException("A column and row number must be a value between 1 to 10.");

            column--;
            row--;
            hitCount++;

            if (IsMissionCompleted())
                return Result.MISSION_COMPLETED;

            if (boardOriginal[row, column] == "X")
            {                
                board[row, column] = ".";
                return IsMissionCompleted() ? Result.MISSION_COMPLETED : Result.HIT;
            }
            
            return Result.MISS;
        }

        private bool IsMissionCompleted()
        {
            foreach (var item in board)
            {
                if (item == "X")
                {
                    return false;
                }
            }
            return true;
        }

        public object GetHit()
        {
            return hitCount;
        }
        public Guid GetId()
        {
            return Id;
        }
        #region Random Board

        public void CreateRandomBoard()
        {
            board = new string[10, 10];
            foreach (int shipSize in SHIPS)
            {
                while (true)
                {
                    var isVertical = RANDOM.Next(0, 2) == 0;
                    var isDone = pasteShip(isVertical, shipSize);
                    if (isDone)
                        break;
                }
            }
            hitCount = 0;
            boardOriginal = (string[,])board;
        }

        private bool pasteShip(bool isVertical, int shipSize)
        {
            var startRowIndex = RANDOM.Next(0, isVertical ? END_INDEX - shipSize : END_INDEX);
            var startColumnIndex = RANDOM.Next(0, !isVertical ? END_INDEX - shipSize : END_INDEX);
            var canPaste = true;
            for (int i = 0; i < shipSize; i++)
            {
                var column = getNextColumnIndex(isVertical, startColumnIndex, i);
                var row = getNextRowIndex(isVertical, startRowIndex, i);
                if (board[column, row] == "X")
                {
                    canPaste = false;
                    break;
                }
            }


            if (canPaste)
            {
                for (int i = 0; i < shipSize; i++)
                {
                    var column = getNextColumnIndex(isVertical, startColumnIndex, i);
                    var row = getNextRowIndex(isVertical, startRowIndex, i);
                    board[column, row] = "X";
                }
            }

            return canPaste;
        }

        private static int getNextRowIndex(bool isVertical, int startRowIndex, int i)
        {
            return isVertical ? startRowIndex + i : startRowIndex;
        }

        private static int getNextColumnIndex(bool isVertical, int startColumnIndex, int i)
        {
            return !isVertical ? startColumnIndex + i : startColumnIndex;
        }

        #endregion RandomBoard
    }


    //public class BattleshipBoardX 
    //{
    //    public override void Play(IFireable fireable)
    //    {
    //        // Implement AI logic here

    //        #region example code 
            
    //        var isCompleted = false;
    //        for (int i = 1; i <= 10; i++)
    //        {
    //            for (int j = 1; j <= 10; j++)
    //            {
    //                var result = fireable.Fire(i, j);
    //                PrintBoard();
    //                if (result == Result.MISSION_COMPLETED)
    //                {
    //                    isCompleted = true;
    //                    break;
    //                }
    //            }
    //            if (isCompleted)
    //                break;
    //        }
            
    //        #endregion
    //    }

    //    public void PrintBoard()
    //    {
    //        Console.Clear();
    //        for (int i = 0; i < 10; i++)
    //        {
    //            for (int j = 0; j < 10; j++)
    //            {
    //                Console.Write(board[i, j] ?? ".");
    //            }
    //            Console.WriteLine();
    //        }
    //    }
    //}
}
