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
            myBattleshipAi.Play(board);

        }
    }

    public class MyBattleshipAi : IBattleshipAi 
    {
        public void Play(IFireable fireable)
        {
            throw new NotImplementedException();
            // var result = fireable.Fire(1, 2);
        }

        public string GetTeamName()
        {
            throw new NotImplementedException();
        }
    }
}
