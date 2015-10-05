using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeChallenge1;

namespace CodeChallengeTest
{
    [TestClass]
    public class FireTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "A column and row number must be a value between 1 to 10.")]
        public void Fire_ColumnOverThan10ShouldThrowException()
        {
            BattleshipBoard board = new BattleshipBoard();
            board.Fire(11, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "A column and row number must be a value between 1 to 10.")]
        public void Fire_ColumnLowerThan0ShouldThrowException()
        {
            BattleshipBoard board = new BattleshipBoard();
            board.Fire(0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "A column and row number must be a value between 1 to 10.")]
        public void Fire_RowOverThan10ShouldThrowException()
        {
            BattleshipBoard board = new BattleshipBoard();
            board.Fire(1, 11);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "A column and row number must be a value between 1 to 10.")]
        public void Fire_RowLowerThan0ShouldThrowException()
        {
            BattleshipBoard board = new BattleshipBoard();
            board.Fire(1, 0);
        }

        [TestMethod]
        public void Fire_WhenCorrectColumnAndRow_ThenReturnHIT()
        {
            BattleshipBoard board = new BattleshipBoard();
            var result = board.Fire(2, 1);

            Assert.AreEqual(Result.HIT, result);
        }

        [TestMethod]
        public void Fire_WhenIncorrectColumnAndRow_ThenReturnMISS()
        {
            BattleshipBoard board = new BattleshipBoard();
            var result = board.Fire(1, 1);

            Assert.AreEqual(Result.MISS, result);
        }

        [TestMethod]
        public void Fire_WhenCorrectAll_ThenReturnCompleted()
        {
            BattleshipBoard board = new BattleshipBoard();
            // Ship 5 length
            var result = board.Fire(2, 1);
            Assert.AreEqual(Result.HIT, result);
            result = board.Fire(2, 2);
            Assert.AreEqual(Result.HIT, result);
            result = board.Fire(2, 3);
            Assert.AreEqual(Result.HIT, result);
            result = board.Fire(2, 4);
            Assert.AreEqual(Result.HIT, result);
            result = board.Fire(2, 5);
            Assert.AreEqual(Result.HIT, result);

            // Ship 2 length
            result = board.Fire(4, 5);
            Assert.AreEqual(Result.HIT, result);
            result = board.Fire(5, 5);
            Assert.AreEqual(Result.HIT, result);

            // Ship 3 length x 2
            result = board.Fire(7, 2);
            Assert.AreEqual(Result.HIT, result);
            result = board.Fire(8, 2);
            Assert.AreEqual(Result.HIT, result);
            result = board.Fire(9, 2);
            Assert.AreEqual(Result.HIT, result);
            result = board.Fire(9, 3);
            Assert.AreEqual(Result.HIT, result);
            result = board.Fire(9, 4);
            Assert.AreEqual(Result.HIT, result);
            result = board.Fire(9, 5);
            Assert.AreEqual(Result.HIT, result);

            // Ship 4 length 
            result = board.Fire(8, 6);
            Assert.AreEqual(Result.HIT, result);
            result = board.Fire(8, 7);
            Assert.AreEqual(Result.HIT, result);
            result = board.Fire(8, 7);
            Assert.AreEqual(Result.HIT, result);
            result = board.Fire(1, 1);
            Assert.AreEqual(Result.MISS, result);
            result = board.Fire(8, 8);
            Assert.AreEqual(Result.HIT, result);
            result = board.Fire(8, 9);
            Assert.AreEqual(Result.MISSION_COMPLETED, result);
            result = board.Fire(8, 9);
            Assert.AreEqual(Result.MISSION_COMPLETED, result);
            result = board.Fire(1, 1);
            Assert.AreEqual(Result.MISSION_COMPLETED, result);

            Assert.AreEqual(21, board.GetHit());
        }
    }
}
