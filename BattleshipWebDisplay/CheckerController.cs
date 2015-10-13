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


        [HttpGet, Route("checker/problem")]
        public List<CheckerResult> GetTeam1Object()
        {
            var battleshipChecker1 = new BattleshipChecker.BattleshipChecker("c:");
            var result1s = battleshipChecker1.CheckTopic(problem1, 1000);

            var battleshipChecker2 = new BattleshipChecker.BattleshipChecker("c:");
            var result2s = battleshipChecker2.CheckTopic(problem2, 1000);

            var battleshipChecker3 = new BattleshipChecker.BattleshipChecker("c:");
            var result3s = battleshipChecker3.CheckTopic(problem3, 1000);

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
                    Problem2 = result2.FireCount,
                    Problem3 = result3.FireCount,
                    Sum = isFoul ? 300 : result1.FireCount + result2.FireCount + result2.FireCount,
                    IsFoul = isFoul
                };
                checkerResults.Add(cr);
            }

            return checkerResults.OrderBy(c => c.Sum).ToList(); ;
        }

        
    }

    public class CheckerResult {
        public bool IsFoul; 
        public string TeamName {get;set;}
        public int Problem1 { get; set; }
        public int Problem2 { get; set; }
        public int Problem3 { get; set; }
        public int Sum { get; set; }
    }

}
