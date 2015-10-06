using CodeChallenge1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattlefieldConsole
{
    public class ConsoleDisplay : IDisplay
    {
        private string teamName;
        private int count;
        private string[,] board = new string[10, 10] 
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

        public void Print(Result result = Result.MISS)
        {
            Console.Clear();
            PrintBoard();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Total Hit: " + count);
            Console.WriteLine();
            Console.WriteLine("Team Name: " + teamName);
        }



        internal void SetTeamName(string name)
        {
            teamName = name;
        }

        internal void PrintBoard()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(board[j, i]);
                }
                Console.WriteLine();
            }
        }

        public void UpdateDisplay(Guid id, int column, int row, Result result)
        {
            column--;
            row--;
            count++;
            if (result == Result.MISS)
            {
                board[column, row] = "*";
            }
            else if (result == Result.HIT || result == Result.MISSION_COMPLETED)
            {
                board[column, row] = "X";
            }
            Print(result);
            Thread.Sleep(500);
        }

        internal void Display(BattleshipAi ai, BattleshipBoard board)
        {
            SetTeamName(ai.GetTeamName());
            Print();
            ai.Play(board);

            Console.WriteLine();
            Console.WriteLine(ai.GetTeamName() + " WIN!!");
        }
    }
}
