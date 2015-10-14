using CodeChallenge1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeChallenge1
{
    public class DummyTeamAi : IBattleshipAi
    {
        public void Play(IFireable fireable)
        {
            var isMissionCompleted = false;
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    var result = fireable.Fire(j, i);
                    if (result == Result.MISSION_COMPLETED)
                    {
                        isMissionCompleted = true;
                        break;
                    }
                }
                if (isMissionCompleted)
                    break;
            }
        }


        public string GetTeamName()
        {
            return "Super AI";
        }
    }
}
