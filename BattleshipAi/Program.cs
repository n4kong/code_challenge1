using CodeChallenge1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeChallenge1
{
    class Program
    {
        static void Main(string[] args)
        {
            var board = new BattleshipBoard();
            DummyTeamAi ai = new DummyTeamAi();
            ai.Play(board);

            Console.ReadLine();
        }
    }
}
