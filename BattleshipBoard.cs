using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeChallenge1
{
    public enum Result { HIT, MISS, MISSION_COMPLETED };


    interface IFireable
    {
        Result Fire(int column, int row);
    }
    public class BattheshipBoard : IFireable
    {
        private int MAX_TOTAL_HIT = 17;
        private int START_INDEX = 1;
        private int END_INDEX = 10;
        private int hitCount = 0;
        private int hitShipCount = 0;
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

        public BattheshipBoard()
        {
            hitCount = 0;
            hitShipCount = 0;
            boardOriginal = (string[,]) board.Clone();
        }

        public Result Fire(int column, int row) 
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
