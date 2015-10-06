using CodeChallenge1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleshipAi2
{
    public class MyAi2 : IBattleshipAi
    {
        public void Play(IFireable fireable)
        {
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    var result = fireable.Fire(i, j);
                    if (result == Result.MISSION_COMPLETED)
                    {
                        break;
                    }
                }
            }
        }

        public string GetTeamName()
        {
            return "SWAT Team";
        }
    }

    public class KongAi2 : IBattleshipAi
    {
        public void Play(IFireable fireable)
        {
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    var result = fireable.Fire(j, i);
                    if (result == Result.MISSION_COMPLETED)
                    {
                        break;
                    }
                }
            }
        }

        public string GetTeamName()
        {
            return "The KONG AI";
        }
    }
}
