using CodeChallenge1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipAi2
{
    class Program
    {
        static void Main(string[] args)
        {
            BattleshipBoard board = new BattleshipBoard();
            MyBattleshipAi myBattleshipAi = new MyBattleshipAi();
            //board.CreateRandomBoard();
            myBattleshipAi.Play(board);

            Console.ReadLine();
        }
    }

    public class MyBattleshipAi : IBattleshipAi 
    {
        public void Play(IFireable fireable)
        {
            var result = fireable.Fire(2, 1);
            Console.WriteLine(result);
            result = fireable.Fire(2, 1);
            Console.WriteLine(result);
            fireable.Fire(2, 1);
            fireable.Fire(2, 1);
            fireable.Fire(2, 1);
            fireable.Fire(2, 1);
            fireable.Fire(2, 1);
            fireable.Fire(4, 3);
            fireable.Fire(6, 7);
        }

        public string GetTeamName()
        {
            return "Preview Borad";
        }
    }
}
