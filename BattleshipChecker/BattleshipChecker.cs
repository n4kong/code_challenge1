using CodeChallenge1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipChecker
{
    public class TeamResults
    {
        public string TeamName { get; set; }
        public int FireCount { get; set; }

    }
    public class BattleshipChecker
    {
        public List<IBattleshipAi> GetBattleshipAis()
        {
            var dir = Directory.GetCurrentDirectory();
            string[] dlls = Directory.GetFiles(dir + "\\" + "BattleshipDlls", "*.dll");

            var battleshipAiDlls = (
                    from file in dlls
                    let asm = Assembly.LoadFile(file)
                    from type in asm.GetExportedTypes()
                    where typeof(IBattleshipAi).IsAssignableFrom(type)
                    select (IBattleshipAi)Activator.CreateInstance(type)
                ).ToList();

            return battleshipAiDlls;
        }

        public Dictionary<string, TeamResults> CheckTopic1(string[,] problem, int repeatCount)
        {
            Dictionary<string, TeamResults> results = new Dictionary<string, TeamResults>();
            for (int i = 0; i < repeatCount; i++)
            {
                var dlls = GetBattleshipAis();
                foreach (var ai in dlls)
                {
                    var teamName = ai.GetTeamName();
                    if (!results.ContainsKey(teamName))
                        results[teamName] = new TeamResults() { TeamName = teamName, FireCount = 0 };

                    DummyDisplay display = new DummyDisplay();
                    BattleshipBoard board = new BattleshipBoard(display, problem);
                    ai.Play(board);
                    if (display.IsMissionCompleted)
                    {
                        results[teamName].FireCount = results[teamName].FireCount + board.FireCount;
                    }
                    else
                    {
                        results[teamName].FireCount = -1;
                    }
                }
            }

            foreach (var result in results)
            {
                result.Value.FireCount = (result.Value.FireCount < 0 ? result.Value.FireCount : result.Value.FireCount / repeatCount);
            }

            return results;
        }
    }

    class DummyDisplay : IDisplay
    {
        public bool IsMissionCompleted { get; set; }
        public void UpdateDisplay(Guid id, int column, int row, Result result)
        {
            if (result == Result.MISSION_COMPLETED)
            {
                IsMissionCompleted = true;
            }
        }
    }
}
