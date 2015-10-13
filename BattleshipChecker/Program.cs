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
    class Program
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

        static void Main(string[] args)
        {
            var repeatCount = 1000;
            BattleshipChecker bc = new BattleshipChecker();

            Console.WriteLine("#### Problem 1 ####" );
            var result1 = bc.CheckTopic(problem1, repeatCount);
            PrintResult(result1);

            Console.WriteLine("#### Problem 2 ####");
            var result2 = bc.CheckTopic(problem2, repeatCount);
            PrintResult(result2);

            Console.WriteLine("#### Problem 3 ####");
            var result3 = bc.CheckTopic(problem3, repeatCount);
            PrintResult(result3);

            Console.WriteLine();
            Console.WriteLine("#### Summary ####");
            foreach (var team in result1)
            {
                var teamName = team.Value.TeamName;
                Console.WriteLine(teamName + "\t\t" + sumFireCount(teamName, result1,result2,result3) );   
            }

            Console.ReadLine();
        }

        private static string sumFireCount(string teamName, Dictionary<string, TeamResults> result1, Dictionary<string, TeamResults> result2, Dictionary<string, TeamResults> result3)
        {
            if (result1[teamName].FireCount < 0 || result2[teamName].FireCount < 0 || result3[teamName].FireCount < 0)
            {
                return "Foul";
            }
            return (result1[teamName].FireCount + result2[teamName].FireCount + result3[teamName].FireCount).ToString();
        }

        private static void PrintResult(Dictionary<string, TeamResults> topic1Results)
        {
            foreach (var topic1Result in topic1Results)
            {
                Console.WriteLine(topic1Result.Value.TeamName + "\t\t" + (topic1Result.Value.FireCount < 0 ? "Foul" : topic1Result.Value.FireCount.ToString()));
            }
        }
    }

   
}
