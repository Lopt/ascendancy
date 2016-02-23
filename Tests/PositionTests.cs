namespace Tests
{
    using System;
    using Core.Models;
    using NUnit.Framework;

    /// <summary>
    /// Position tests.
    /// </summary>
    [TestFixture]
    public class PositionTests
    {
        /// <summary>
        /// Tests the Latitude and Longitude class.
        /// </summary>
        [Test]
        public void LatLonTest()
        {
            var latLon = new LatLon(50.97695325, 11.02396488);
            Assert.IsNotNull(latLon);

            // Tests to get a Latitude and Longitude out of an GamePosition
            var pos = new Position(latLon);
            var latLon2 = new LatLon(pos);
            Assert.IsNotNull(latLon2);

            // Tests if the Latitude and Longitude are the same till the 4. digit after conversion
            var lat = latLon.Lat * 10000;
            var lon = latLon.Lon * 10000;

            var lat2 = latLon2.Lat * 10000;
            var lon2 = latLon2.Lon * 10000;

            Assert.AreEqual((int)lat, (int)lat2);
            Assert.AreEqual((int)lon, (int)lon2);
        }

        /// <summary>
        /// Tests the CellPosition class.
        /// </summary>
        [Test]
        public void CellPositionTest()
        {
            // standart constructor
            var cellPos = new CellPosition(0, 0);
            Assert.IsNotNull(cellPos);

            // Constructor CellPosition out of Position
            var position = new Position(0, 0);
            cellPos = new CellPosition(position);
            Assert.IsNotNull(cellPos);

            // Constructor CellPosition out of PositionI
            var positionI = new PositionI(0, 0);
            cellPos = new CellPosition(positionI);
            Assert.IsNotNull(cellPos);

            // Tests the == operator
            var cellPos2 = new CellPosition(0, 0);
            Assert.True(cellPos == cellPos2);

            // Tests the Equas function
            Assert.True(cellPos.Equals(cellPos2));

            // Tests the != operator
            cellPos2 = new CellPosition(1, 1);
            Assert.True(cellPos != cellPos2);

            // Tests if its Posible to create a cell position bigger than 31 and smaller then 0
            cellPos = new CellPosition(32, 32);
            Assert.AreNotEqual(32, cellPos.CellX);
            Assert.AreNotEqual(32, cellPos.CellY);

            cellPos = new CellPosition(-1, -1);
            Assert.AreNotEqual(-1, cellPos.CellX);
            Assert.AreNotEqual(-1, cellPos.CellY);
        }

        /// <summary>
        /// Tests the RegionPosition class.
        /// </summary>
        [Test]
        public void RegionPositionTest()
        {
            // standart constructor
            var regionPos = new RegionPosition(0, 0);
            Assert.IsNotNull(regionPos);

            // Constructor RegionPosition out of a Position
            var position = new Position(0, 0);
            regionPos = new RegionPosition(position);
            Assert.IsNotNull(regionPos);

            // Constructor RegionPosition out of a PositionI
            var positionI = new PositionI(0, 0);
            regionPos = new RegionPosition(positionI);
            Assert.IsNotNull(regionPos);

            // Tests the + Operator
            var regionPos2 = new RegionPosition(1, 1);
            regionPos += regionPos2;
            Assert.AreEqual(regionPos2, regionPos);

            // Tests the == Operator
            Assert.True(regionPos == regionPos2);

            // Tests the Equals funktion
            Assert.True(regionPos.Equals(regionPos2));
        }

        /// <summary>
        /// Tests the Position class.
        /// </summary>
        [Test]
        public void PositionTest()
        {
            // Standart Constructor
            var position = new Position(0, 0);
            Assert.IsNotNull(position);

            // Constructor Position out of PositionI
            position = new Position(new PositionI(0, 0));
            Assert.IsNotNull(position);

            // Constructor Position out of Latitude and Longitude
            position = new Position(new LatLon(50.97695325, 11.02396488));
            Assert.IsNotNull(position);

            // Constructor Position out of a ReagionPosition
            var regionPos = new RegionPosition(0, 0);
            position = new Position(regionPos);
            Assert.IsNotNull(position);

            // Constructor Position out of a RegionPosition and a CellPosition
            var cellPos = new CellPosition(0, 0);
            position = new Position(regionPos, cellPos);
            Assert.IsNotNull(position);

            // Constructor Position out of a RegionPositionX, a RegionPositionY, a CellPositionX and a CellPositionY
            position = new Position(regionPos.RegionX, regionPos.RegionY, cellPos.CellX, cellPos.CellY);
            Assert.IsNotNull(position);

            // tests the + Operator
            position = new Position(new LatLon(50.97695325, 11.02396488));
            var position2 = new Position(0, 0);

            position2 += position;

            Assert.AreEqual(position, position2);

            // tests the - Operator
            position -= position2;

            Assert.AreNotEqual(position2, position);
            Assert.AreEqual(new Position(0, 0), position);

            // tests the == Operator
            Assert.True(new Position(0, 0) == position);

            // tests the Equals function
            Assert.True(position.Equals(new Position(0, 0)));

            // test Distance with Position
            var dist = position.Distance(position2);
            Assert.IsNotNull(dist);

            // test Distance with PositionI
            var dist2 = position2.Distance(new PositionI(0, 0));
            Assert.IsNotNull(dist2);
            Assert.AreEqual(dist, dist2);
        }

        /// <summary>
        /// Tests the PositionI class.
        /// </summary>
        [Test]
        public void PositionITest()
        {
            // standart Constructor
            var position = new PositionI(0, 0);
            Assert.IsNotNull(position);

            // Constructor Position out of a ReagionPosition
            var regionPos = new RegionPosition(0, 0);
            var cellPos = new CellPosition(0, 0);
            position = new PositionI(regionPos, cellPos);
            Assert.IsNotNull(position);

            position = new PositionI(new Position(0, 0));
            Assert.IsNotNull(position);

            // tests the + Operator
            position = new PositionI(1, 1);
            var position2 = new PositionI(0, 0);
            position2 += position;

            Assert.AreEqual(position, position2);

            // tests the - Operator
            position -= position2;

            Assert.AreNotEqual(position2, position);
            Assert.AreEqual(new PositionI(0, 0), position);

            // tests the == Operator
            Assert.True(new PositionI(0, 0) == position);

            // tests the Equals function
            Assert.True(position.Equals(new PositionI(0, 0)));

            // test Distance with PositionI
            var dist = position.Distance(position2);
            Assert.IsNotNull(dist);

            // test Distance with PositionI
            var dist2 = position2.Distance(new Position(0, 0));
            Assert.IsNotNull(dist2);
            Assert.AreEqual(dist, dist2);
        }
    }
}