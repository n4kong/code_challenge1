using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BattleshipChecker;

namespace ThomsonReuters.Eikon.BattleshipWebDisplay
{
    public class CheckerController : ApiController
    {

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
            {"X",".",".",".",".",".",".",".",".","."},
            {"X",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".","."},
            {"X","X","X",".",".","X","X","X","X","."},
            {".",".","X",".","X",".",".",".",".","."},
            {".",".","X",".","X",".",".",".",".","."},
            {".",".","X",".","X",".",".",".",".","."},
            {".",".",".",".","X",".",".",".",".","."},
            {".",".",".",".","X",".",".",".",".","."}
        };
        #endregion 

        [HttpGet, Route("checker/problem/{no}")]
        public string[,] GetProblem(int no)
        {
            if (no == 1)
                return problem1;
            else if (no == 2)
                return problem2;
            else
                return problem3;
        }


        [HttpGet, Route("checker/{sampling}")]
        public List<CheckerResult> GetTeam1Object(int sampling )
        {
            var battleshipChecker1 = new BattleshipChecker.BattleshipChecker("c:");
            var result1s = battleshipChecker1.CheckTopic(problem1, sampling);

            var battleshipChecker2 = new BattleshipChecker.BattleshipChecker("c:");
            var result2s = battleshipChecker2.CheckTopic(problem2, sampling);

            var battleshipChecker3 = new BattleshipChecker.BattleshipChecker("c:");
            var result3s = battleshipChecker3.CheckTopic(problem3, sampling);

            List<CheckerResult> checkerResults = new List<CheckerResult>();
            foreach (var item in result1s)
            {
                var result1 = result1s[item.Key];
                var result2 = result2s[item.Key];
                var result3 = result3s[item.Key];
                var isFoul = (result1.FireCount < 0 || result2.FireCount < 0 || result3.FireCount < 0) ? true : false;
                var cr = new CheckerResult()
                {
                    TeamName = result1.TeamName,
                    Problem1 = result1.FireCount,
                    Problem1Time = result1.TimeTaken,
                    Problem2 = result2.FireCount,
                    Problem2Time = result2.TimeTaken,
                    Problem3 = result3.FireCount,
                    Problem3Time = result3.TimeTaken,
                    Sum = isFoul ? 300 : result1.FireCount + result2.FireCount + result2.FireCount,
                    TotalTime = result1.TimeTaken+ result2.TimeTaken+result3.TimeTaken,
                    IsFoul = isFoul
                };
                checkerResults.Add(cr);
            }

            return checkerResults.OrderBy(c => c.Sum).ToList(); ;
        }

        
    }

    public class CheckerResult {
        public bool IsFoul { get; set; }
        public double Problem1Time { get; set; }
        public double Problem2Time { get; set; }
        public double Problem3Time { get; set; }
        public double TotalTime { get; set; }
        public string TeamName {get;set;}
        public int Problem1 { get; set; }
        public int Problem2 { get; set; }
        public int Problem3 { get; set; }
        public int Sum { get; set; }
    }

}
