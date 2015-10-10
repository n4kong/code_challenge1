using CodeChallenge1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDllAi1
{
    class Program
    {
        static void Main(string[] args)
        {
            BattleshipBoard board = new BattleshipBoard();
            SampleDllAi1 ai = new SampleDllAi1();
            ai.Play(board);

            Console.ReadLine();
        }
    }
}
