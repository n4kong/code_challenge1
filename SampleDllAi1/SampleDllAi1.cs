using CodeChallenge1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDllAi1
{
    public class SampleDllAi1 : IBattleshipAi
    {
        private readonly Random RANDOM = new Random();
        string[,] board = new string[10, 10] 
        {
            {".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","."}
        };
        public void Play(IFireable fireable)
        {
            var result = Result.MISS;
            while (result != Result.MISSION_COMPLETED)
            {
                var x = RANDOM.Next(1, 11);
                var y = RANDOM.Next(1, 11);
                
                x--;
                y--;
                if (board[x, y] != "X")
                {
                    board[x, y] = "X";
                    result = fireable.Fire(x+1, y+1);
                }
            }
        }

        public string GetTeamName()
        {
            return "Sample DLL Ai 1";
        }
    }
}
