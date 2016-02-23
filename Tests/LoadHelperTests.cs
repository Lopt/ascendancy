namespace Tests
{
    using System;
    using Core.Models;
    using NUnit.Framework;
    /// <summary>
    /// Load helper tests.
    /// </summary>
    [TestFixture]
    public class LoadHelperTests
    {
        /// <summary>
        /// Tests the ReplacePath function.
        /// </summary>
        [Test]
        public void ReplacePath()
        {
            var regionPosition = new RegionPosition(new Position(new LatLon(50.97695325, 11.02396488)));
            var newPath = Core.Helper.LoadHelper.ReplacePath(Server.Models.ServerConstants.REGION_FILE, regionPosition);
            Assert.IsNotNull(newPath);
        }

        /// <summary>
        /// Tests the JSONToTerrain function.
        /// </summary>
        [Test]
        public void JsonToTerrain()
        {
            var regionPosition = new RegionPosition(new Position(new LatLon(50.97695325, 11.02396488)));
            var newPath = Core.Helper.LoadHelper.ReplacePath(Server.Models.ServerConstants.REGION_FILE, regionPosition);
            // var json = await Core.RequestAsync(newpath);
        }

        /// <summary>
        /// Tests the JSONToRegion function.
        /// </summary>
        [Test]
        public void JsonToRegion()
        {    
        }
    }
}