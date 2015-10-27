using CodeChallenge1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThomsonReuters.Eikon.BattleshipWebDisplay.Display
{
    public class WebDisplay : IDisplay
    {
        Dictionary<Guid, TeamObject> TeamData = new Dictionary<Guid, TeamObject>();
        Guid board1Id;
        Guid board2Id;
        public int Delay {get;set;}

        public WebDisplay()
        {
            Delay = 1000;
        }
        public void UpdateDisplay(Guid id, int column, int row, Result result)
        {
            

            TeamData[id].SetResult(column, row, result);

            while (TeamData[board1Id].TotalFires != TeamData[board2Id].TotalFires)
            {
                Thread.Sleep(500);
            }
            Thread.Sleep(Delay);
        }

        internal void setBoardId1(Guid guid, string teamName)
        {
            board1Id = guid;
            TeamData[board1Id] = new TeamObject(teamName);
            
        }

        internal void setBoardId2(Guid guid, string teamName)
        {
            board2Id = guid;
            TeamData[board2Id] = new TeamObject(teamName);
        }


        public TeamObject GetTeam1Object()
        {
            if (!TeamData.ContainsKey(board1Id))
                return new TeamObject("Not initial.");

            return TeamData[board1Id];
        }
        public TeamObject GetTeam2Object()
        {
            if (!TeamData.ContainsKey(board2Id))
                return new TeamObject("Not initial.");

            return TeamData[board2Id];
        }

        internal bool IsPlaying()
        {
            if (TeamData.ContainsKey(board1Id) && TeamData[board1Id].IsPlaying && !TeamData.ContainsKey(board2Id))
            {
                return true;
            }

            if (TeamData.ContainsKey(board1Id) && TeamData[board1Id].IsPlaying && TeamData.ContainsKey(board2Id) && TeamData[board2Id].IsPlaying)
            {
                return true;
            }
            return false;
        }

        internal void Reset()
        {
            TeamData[board1Id] = new TeamObject("Not initial.");
            TeamData[board2Id] = new TeamObject("Not initial.");
        }
    }
    public class TeamObject
    {
        public int TotalFires { get; set; }
        public int TotalShips { get; set; }
        public string TeamName { get; set; }
        public string[,] Board { get; set; }
        public bool IsPlaying { get; set; }

        public bool IsWin { get; set; }
        public bool IsHit { get; set; }

        public TeamObject(string teamName)
        {
            Board = new string[10, 10] 
            {
                {".",".",".",".",".",".",".",".",".","."},
                {".",".",".",".",".",".",".",".",".","."},
                {".",".",".",".",".",".",".",".",".","."},
                {".",".",".",".",".",".",".",".",".","."},
                {".",".",".",".",".",".",".",".",".","."},
                {".",".",".",".",".",".",".",".",".","."},
                {".",".",".",".",".",".",".",".",".","."},
                {".",".",".",".",".",".",".",".",".","."},
                {".",".",".",".",".",".",".",".",".","."},
                {".",".",".",".",".",".",".",".",".","."}
            };
            TeamName = teamName;
            TotalShips = 17;
            TotalFires = 0;
            IsPlaying = false;
            IsWin = false;
            IsHit = false;
        }

        internal void SetResult(int column, int row, Result result)
        {
            IsPlaying = true;
            if (Result.MISSION_COMPLETED == result && TotalShips == 0)
            {
                IsWin = true;
                IsPlaying = false;
                IsHit = true;
                return;
            }

            column--;
            row--;
            TotalFires++;
            if (result == Result.HIT || result == Result.MISSION_COMPLETED)
            {

                TotalShips--;
                IsHit = true;
                if (result == Result.MISSION_COMPLETED)
                    IsPlaying = false;
            }
            else
            {
                IsHit = false;

            }
            Board[column, row] = result.ToString();
        }

    }
}
