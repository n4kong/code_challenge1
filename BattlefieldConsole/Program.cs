using BattleshipAi2;
using CodeChallenge1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattlefieldConsole
{
    class Program
    {

        

        static void Main(string[] args)
        {

            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            ConsoleDisplay2Board console1 = new ConsoleDisplay2Board();
            BattleshipBoard board1 = new BattleshipBoard(console1);
            BattleshipBoard board2 = new BattleshipBoard(console1);  
         
            IBattleshipAi ai1 = new BattleshipAi();
            IBattleshipAi ai2 = new MyAi2();


            console1.Display(ai1, ai2, board1, board2);


            Console.ReadLine();

        }
    }

    class BoardObject
    {
        Guid id;
        bool isCompleted = false;
        private string teamName;
        private int count;
        private int shipLeft = 17;

        public int GetShipLeft()
        {
            return shipLeft;
        }

        public void DecreaseShipLeft()
        {
            shipLeft--;
        }


        private string[,] board = new string[10, 10] 
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
        public BoardObject(Guid id, string name)
        {
            this.teamName = name;
            this.id = id;
        }
        public string[,] GetBoard() {
            return board;
        }

        internal void MissFire(int column, int row)
        {
            board[column, row] = "*";
        }

        internal void HitFire(int column, int row)
        {
            board[column, row] = "X";
        }

        internal void IncreaseFire()
        {
            count++;
        }
        public int GetCount()
        {
            return count;
        }

        internal string GetName()
        {
            return teamName;
        }

        internal void SetCompleted()
        {
            this.isCompleted = true;
        }
        internal bool IsCompleted()
        {
            return isCompleted;
        }
    }

    class ConsoleDisplay2Board : IDisplay
    {
        internal void PrintBoard()
        {
            lock (boardData)
            {
                //Console.Clear();

                
                var board1Obj = boardData[board1Id];
                var board2Obj = boardData[board2Id];
                var board1 = board1Obj.GetBoard();
                var board2 = board2Obj.GetBoard();
                for (int i = 0; i < 10; i++)
                {
                    Console.SetCursorPosition(10, 5 + i);

                    for (int j = 0; j < 10; j++)
                    {
                        if (board1[j, i] == ".") { Console.BackgroundColor = ConsoleColor.DarkGray; Console.ForegroundColor = ConsoleColor.DarkGray; }
                        if (board1[j, i] == "X") {Console.BackgroundColor = ConsoleColor.Red; Console.ForegroundColor = ConsoleColor.Red;}
                        if (board1[j, i] == "*") { Console.BackgroundColor = ConsoleColor.Yellow; Console.ForegroundColor = ConsoleColor.Yellow; }
                        Console.Write(board1[j, i]);
                    }

                    //Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.Black;
                    //Console.Write("\t\t\t\t\t");
                    
                    Console.SetCursorPosition(50, 5 + i);
                    for (int j = 0; j < 10; j++)
                    {
                        if (board2[j, i] == ".") { Console.BackgroundColor = ConsoleColor.DarkGray; Console.ForegroundColor = ConsoleColor.DarkGray; }
                        if (board2[j, i] == "X") {Console.BackgroundColor = ConsoleColor.Red; Console.ForegroundColor = ConsoleColor.Red;}
                        if (board2[j, i] == "*") { Console.BackgroundColor = ConsoleColor.Yellow; Console.ForegroundColor = ConsoleColor.Yellow; }
                        Console.Write(board2[j, i]);
                    }
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                
                Console.SetCursorPosition(10, 20);
                Console.Write("Team Name: " + board1Obj.GetName());
                Console.SetCursorPosition(10, 21);
                Console.Write("Total Ship Left: " + board1Obj.GetShipLeft() + "   ");
                Console.SetCursorPosition(10, 22);
                Console.Write("Turn: " + board1Obj.GetCount());

                Console.SetCursorPosition(50, 20);
                Console.Write("Team Name: " + board2Obj.GetName());
                Console.SetCursorPosition(50, 21);
                Console.Write("Total Ship Left: " + board2Obj.GetShipLeft() +  "   ");
                Console.SetCursorPosition(50, 22);
                Console.Write("Turn: " + board2Obj.GetCount());
            }
        }

        Dictionary<Guid, BoardObject> boardData = new Dictionary<Guid, BoardObject>();
        Guid board1Id;
        Guid board2Id;
        bool IsCompleted = false;
        public void UpdateDisplay(Guid id, int column, int row, Result result)
        {
            var board = boardData[id];

            if (board.IsCompleted())
                return;

            column--;
            row--;
            board.IncreaseFire();
            if (result == Result.MISS)
            {
                board.MissFire(column, row);
            }
            else if (result == Result.HIT || result == Result.MISSION_COMPLETED)
            {
                board.DecreaseShipLeft();
                board.HitFire(column, row);
                if (result == Result.MISSION_COMPLETED && !IsCompleted)
                {
                    IsCompleted = true;
                    board.SetCompleted();
                    PrintBoard();
                }
            }

            if (!IsCompleted)
            {
                PrintBoard();
                Thread.Sleep(500);
            }
        }

        internal void Display(IBattleshipAi ai1, IBattleshipAi ai2, BattleshipBoard board1, BattleshipBoard board2)
        {
            board1Id = board1.GetId();
            board2Id = board2.GetId();
            boardData[board1Id] = new BoardObject(board1Id, ai1.GetTeamName());
            boardData[board2Id] = new BoardObject(board2Id, ai2.GetTeamName());

            PrintBoard();
                Parallel.Invoke(() =>
                {
                    ai1.Play(board1);

                }, () =>
                {
                    ai2.Play(board2);
                });
            
    
            //Console.WriteLine();
            //Console.WriteLine(ai1.GetTeamName() + " WIN!!");
        }
    }
}
