using BattleshipAi2;
using CodeChallenge1;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using ThomsonReuters.Eikon.BattleshipWebDisplay.Display;

namespace ThomsonReuters.Eikon.BattleshipWebDisplay
{
    public class BattleshipWebDisplayController : ApiController
    {
        public static WebDisplay display = new WebDisplay();

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

        [HttpGet, Route("battleship/play")]
        public async Task<string> DoPlay()
        {
            if (display.IsPlaying())
            {
                return "Playing game.";
            }

            IBattleshipAi ai1 = new MyAi2();
            IBattleshipAi ai2 = new KongAi2();
            display = new WebDisplay();
            BattleshipBoard board1 = new BattleshipBoard(display);
            BattleshipBoard board2 = new BattleshipBoard(display);

            board1.CreateRandomBoard();
            board2.CreateRandomBoard();
            display.setBoardId1(board1.GetId(), ai1.GetTeamName());
            display.setBoardId2(board2.GetId(), ai2.GetTeamName());

            Task.Factory.StartNew(() => {
                Parallel.Invoke(() => ai1.Play(board1), () => ai2.Play(board2));            
            });

            return "Starting game.";
        }

         [HttpGet, Route("battleship/reset")]
        public async Task<string> DoReset()
        {
            if (display.IsPlaying())
            {
                return "The game is playing.";
            }

            display.Reset();

            return "The game is reset.";
        }
    }

}
