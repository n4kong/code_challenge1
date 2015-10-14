using CodeChallenge1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterGame
{
    class Program
    {
        static void Main(string[] args)
        {
            BattleshipBoard b = new BattleshipBoard();
            AI ai = new AI();
            b.Fire()

                ai.Play()

                
        }

        class AI : IBattleshipAi
        {

            public string GetTeamName()
            {
                throw new NotImplementedException();
            }

            public void Play(IFireable fireable)
            {
                throw new NotImplementedException();
            }
        }
    }
}
