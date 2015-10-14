using BattleshipAi2;
using CodeChallenge1;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using ThomsonReuters.Eikon.BattleshipWebDisplay.Display;

namespace ThomsonReuters.Eikon.BattleshipWebDisplay
{
    public class BattleshipWebDisplayController : ApiController
    {
        public static WebDisplay display = new WebDisplay();
        #region problem
        static string[,] problem1 = new string[10, 10] 
        {
            {".",".",".",".",".",".",".","X","X","X"},
            {".",".","X",".",".",".",".",".",".","."},
            {".",".","X",".",".",".",".",".",".","."},
            {".",".","X",".","X",".",".",".",".","."},
            {".",".","X",".","X",".",".",".",".","."},
            {".",".","X",".","X",".",".",".",".","."},
            {".",".",".",".","X",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","X"},
            {"X","X","X",".",".",".",".",".",".","X"}
        };

        static string[,] problem2 = new string[10, 10] 
        {
            {".",".",".",".",".","X",".",".",".","."},
            {".",".","X","X","X","X",".",".",".","."},
            {".",".",".",".",".","X",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".","X","X","X","X"},
            {".",".","X",".",".",".",".",".",".","."},
            {".",".","X",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","."},
            {".",".","X","X","X","X","X",".",".","."}
        };

        static string[,] problem3 = new string[10, 10] 
        {
            {".",".",".",".",".",".",".",".",".","."},
            {".",".","X",".",".",".",".",".",".","."},
            {".",".","X",".",".",".",".",".",".","."},
            {".",".","X",".","X",".",".",".","X","."},
            {".",".","X",".","X",".",".",".","X","."},
            {".",".","X",".","X",".","X",".","X","."},
            {".",".",".",".","X",".","X",".",".","."},
            {".",".",".",".",".",".","X",".",".","."},
            {".",".",".",".",".",".",".",".",".","X"},
            {".",".",".",".",".",".",".",".",".","X"}
        };

        static string[,] problem4 = new string[10, 10] 
        {
            {".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".","X",".","."},
            {".",".",".",".","X","X","X","X",".","."},
            {".","X","X","X",".",".","X","X","X","X"},
            {".",".",".",".","X","X","X","X","X","."}
        };

        static string[,] problem5 = new string[10, 10] 
        {
            {".",".",".",".",".",".",".",".","X","."},
            {".",".",".",".",".",".",".",".","X","."},
            {".",".",".",".",".",".",".",".","X","."},
            {".",".",".","X","X","X","X",".",".","."},
            {".",".",".",".",".",".",".",".",".","X"},
            {".",".",".",".",".",".",".",".",".","X"},
            {".","X",".",".",".",".",".",".",".","X"},
            {".","X",".",".",".",".",".",".",".","X"},
            {".","X","X","X",".",".",".",".",".","X"},
            {".",".",".",".",".",".",".",".",".","."}
        };
        #endregion 

        public class BattleshipWebDisplayType
        {
            public string Host { get; set; }
            public DateTime Time { get; set; }
        }

        public BattleshipWebDisplayType Get()
        {
            return new BattleshipWebDisplayType
            {
                Host = Environment.MachineName,
                Time = DateTime.Now
            };
        }

        [HttpGet, Route("battleship/team1")]
        public TeamObject GetTeam1Object()
        {
            return display.GetTeam1Object();
        }


        [HttpGet, Route("battleship/team2")]
        public TeamObject GetTeam2Object()
        {
            return display.GetTeam2Object();
        }

        public List<IBattleshipAi> GetBattleshipAis()
        {
            string[] dlls = Directory.GetFiles("c:\\FinalBattleshipDlls", "*.dll");

            var battleshipAiDlls = (
                    from file in dlls
                    let asm = Assembly.LoadFile(file)
                    from type in asm.GetExportedTypes()
                    where typeof(IBattleshipAi).IsAssignableFrom(type)
                    select (IBattleshipAi)Activator.CreateInstance(type)
                ).ToList();

            //hardcode for final round only
            return battleshipAiDlls.Where(c => c.GetTeamName() == "codespeed" || c.GetTeamName() == "StoneHopper").ToList();
        }


        [HttpGet, Route("battleship/play/{problemNo}")]
        public async Task<string> DoPlay(int problemNo)
        {
            if (display.IsPlaying())
            {
                return "Playing game.";
            }
            var dlls = GetBattleshipAis();            
            IBattleshipAi ai1 = dlls[0];
            IBattleshipAi ai2 = dlls[1];
            display = new WebDisplay();
            //display.Delay = 200;
            string[,] problem = getProblem(problemNo);

            BattleshipBoard board1 = new BattleshipBoard(display, problem);
            BattleshipBoard board2 = new BattleshipBoard(display, problem);

            display.setBoardId1(board1.GetId(), ai1.GetTeamName());
            display.setBoardId2(board2.GetId(), ai2.GetTeamName());

            Task.Factory.StartNew(() => {
                Parallel.Invoke(() => ai1.Play(board1), () => ai2.Play(board2));            
            });

            return "Starting game.";
        }

        private string[,] getProblem(int problemNo)
        {
            if (problemNo == 1)
                return problem1;
            else if (problemNo == 2)
                return problem2;
            else if (problemNo == 3)
                return problem3;
            else if (problemNo == 4)
                return problem4;
            else
                return problem5;
        }

         [HttpGet, Route("battleship/reset")]
        public async Task<string> DoReset()
        {
            //if (display.IsPlaying())
            //{
            //    return "The game is playing.";
            //}

            display.Reset();
            display.Delay = 0;

            return "The game is reset.";
        }
    }

}
