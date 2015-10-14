using CodeChallenge1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleshipChecker
{
    public class TeamResults
    {
        public double TimeTaken { get; set; }
        public string TeamName { get; set; }
        public int FireCount { get; set; }

    }
    public class BattleshipChecker
    {
        private string path;

        public BattleshipChecker(string p)
        {
            this.path = p;
        }
        public BattleshipChecker()
        {
            this.path = Directory.GetCurrentDirectory();
        }
        public List<IBattleshipAi> GetBattleshipAis()
        {
            string[] dlls = Directory.GetFiles(this.path + "\\" + "BattleshipDlls", "*.dll");

            var battleshipAiDlls = (
                    from file in dlls
                    let asm = Assembly.LoadFile(file)
                    from type in asm.GetExportedTypes()
                    where typeof(IBattleshipAi).IsAssignableFrom(type)
                    select (IBattleshipAi)Activator.CreateInstance(type)
                ).ToList();

            return battleshipAiDlls;
        }

        public Dictionary<string, TeamResults> CheckTopic(string[,] problem, int repeatCount)
        {
            Dictionary<string, TeamResults> results = new Dictionary<string, TeamResults>();
            for (int i = 0; i < repeatCount; i++)
            {
                var dlls = GetBattleshipAis();
                foreach (var ai in dlls)
                {
                    var teamName = "no name";
                    try
                    {
                        teamName = ai.GetTeamName();
                    }
                    catch {}
                    if (!results.ContainsKey(teamName))
                        results[teamName] = new TeamResults() { TeamName = teamName, FireCount = 0 };

                    DummyDisplay display = new DummyDisplay();
                    BattleshipBoard board = new BattleshipBoard(display, problem);
                    var start = DateTime.Now;

                    ////
                    var tokenSource = new CancellationTokenSource();
                    CancellationToken token = tokenSource.Token;
                    int timeOut = 20000;
                    var task = Task.Factory.StartNew(() => ai.Play(board), token);

                    if (!task.Wait(timeOut, token))
                    {
                        Console.WriteLine("The Task " + teamName + " timed out!");
                    }
                    //

                    //ai.Play(board);
                    if (display.IsMissionCompleted)
                    {
                        results[teamName].FireCount = results[teamName].FireCount + board.FireCount;
                    }
                    else
                    {
                        results[teamName].FireCount = -1;
                    }
                    var timeTaken = DateTime.Now.Subtract(start).TotalMilliseconds;
                    results[teamName].TimeTaken = timeTaken;

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
