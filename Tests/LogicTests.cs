namespace Tests
{
    using NUnit.Framework;
    using Core.Models;

    /// <summary>
    /// Logic tests.
    /// </summary>
    [TestFixture]
    public class LogicTests
    {
        /// <summary>
        /// The surround tiles on even x positions.
        /// From North to NorthEast in clockwise
        /// </summary>
        public static readonly Core.Models.PositionI[] SurroundTilesEven =
            {
                new Core.Models.PositionI(0, -1),
                new Core.Models.PositionI(1, 0),
                new Core.Models.PositionI(1, 1),
                new Core.Models.PositionI(0, 1),
                new Core.Models.PositionI(-1, 1),
                new Core.Models.PositionI(-1, 0)
            };

        /// <summary>
        /// The surround tiles on odd x positions.
        /// From North to NorthEast in clockwise
        /// </summary>
        public static readonly Core.Models.PositionI[] SurroundTilesOdd =
            {
                new Core.Models.PositionI(0, -1),
                new Core.Models.PositionI(1, -1),
                new Core.Models.PositionI(1, 0),
                new Core.Models.PositionI(0, 1),
                new Core.Models.PositionI(-1, 0),
                new Core.Models.PositionI(-1, -1)
            };
    
        /// <summary>
        /// Gets the surrounded fields.
        /// </summary>
        [Test]
        public void GetSurroundedFields()
        {
            // Test for Even x Coordinate
            var startPosition = new Core.Models.PositionI(0, 0);
            var positions = Core.Models.LogicRules.GetSurroundedFields(startPosition);

            var expectedPos = new Core.Models.PositionI[]
            {
                startPosition + SurroundTilesOdd[0],
                startPosition + SurroundTilesOdd[1],
                startPosition + SurroundTilesOdd[2],
                startPosition + SurroundTilesOdd[3],
                startPosition + SurroundTilesOdd[4],
                startPosition + SurroundTilesOdd[5]
            };

            Assert.IsNotEmpty(positions);
            Assert.AreEqual(6, positions.Length);
            Assert.AreEqual(positions, expectedPos);

            // Test For uneven x Coordinate
            startPosition = new Core.Models.PositionI(1, 0);
            positions = Core.Models.LogicRules.GetSurroundedFields(startPosition);

            expectedPos = new Core.Models.PositionI[]
                {
                    startPosition + SurroundTilesEven[0],
                    startPosition + SurroundTilesEven[1],
                    startPosition + SurroundTilesEven[2],
                    startPosition + SurroundTilesEven[3],
                    startPosition + SurroundTilesEven[4],
                    startPosition + SurroundTilesEven[5]
                };
            
            Assert.AreEqual(positions, expectedPos);
        }

        /// <summary>
        /// Tests the Surrounded Positions function.
        /// </summary>
        [Test]
        public void GetSurroundedPositions()
        {
            var startPosition = new Core.Models.PositionI(0, 0);
            int range = 1;
            var positions = Core.Models.LogicRules.GetSurroundedPositions(startPosition, range);

            Assert.AreEqual(positions.Count, 7);

            range = 2;
            positions = Core.Models.LogicRules.GetSurroundedPositions(startPosition, range);

            Assert.AreEqual(positions.Count, 19);

            range = 3;
            positions = Core.Models.LogicRules.GetSurroundedPositions(startPosition, range);

            Assert.AreEqual(positions.Count, 37);

            range = 4;
            positions = Core.Models.LogicRules.GetSurroundedPositions(startPosition, range);

            Assert.AreEqual(positions.Count, 61);

            range = 5;
            positions = Core.Models.LogicRules.GetSurroundedPositions(startPosition, range);

            Assert.AreEqual(positions.Count, 91);
        }

        [Test]
        public void Storage()
        {
        }
    }
}