// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using GameWithTests;
using System;

namespace SeeBatleGameWithTest.Tests
{
    [TestFixture]
    public class PlayerFixture

    {

        [Test]
        [TestCase("A1", ShotResult.Past, CellState.AffectedEmpty)]
        [TestCase("B2", ShotResult.Injured, CellState.AffectedDeck)]
        [TestCase("C3", ShotResult.Affected, CellState.AffectedDeck)]
        [TestCase("D4", ShotResult.Killed, CellState.AffectedDeck)]


        public void ShouldSetCellState(string stringCoordinate, ShotResult result, CellState expected)
        {
            //Given;
            var testPlayer = CreateInstance();
            var coordinateOfShot = Coordinate.DefineCoordinateFromString(stringCoordinate);

            //When;
            testPlayer.SetShotsResult(coordinateOfShot, result);

            //Then;
            var actual = testPlayer.Board_2.Cells.Find(cell => cell.Address == coordinateOfShot).State;
            Assert.AreEqual(expected, actual);
        }
        [TestCaseSource(typeof(PlayerFixture), "TestCases")]
        public CellState ShouldSetCellState(Coordinate coordinateOfShot, ShotResult result)
        {
            //Given;
            var testPlayer = CreateInstance();

            //When;
            testPlayer.SetShotsResult(coordinateOfShot, result);

            //Then;
            var actual = testPlayer.Board_2.Cells.Find(cell => cell.Address == coordinateOfShot).State;
            return actual;
        }
        public static IEnumerable TestCases
        {
            get
            {
                TestCaseData[] data = new TestCaseData[]
                {
                new TestCaseData(new Coordinate(AddressLetter.A, AddressNumber.One), ShotResult.Past).Returns(CellState.AffectedEmpty),
                new TestCaseData(new Coordinate(AddressLetter.B, AddressNumber.Two), ShotResult.Injured).Returns(CellState.AffectedDeck),
                new TestCaseData(new Coordinate(AddressLetter.C, AddressNumber.Three), ShotResult.Affected).Returns(CellState.AffectedDeck),
                new TestCaseData(new Coordinate(AddressLetter.D, AddressNumber.Four), ShotResult.Killed).Returns(CellState.AffectedDeck)
                };

                return data;
            }
        }

        private Player CreateInstance()
        {
            Player testPlayer = new Player("TestPlayer");
            return testPlayer;
        }
    }
}