namespace Tests
{
    using Core.Models;
    using NUnit.Framework;

    /// <summary>
    /// Map.Region tests.
    /// </summary>
    [TestFixture]
    public class MapRegionTests
    {
        /// <summary>
        /// Tests the Region class.
        /// </summary>
        [Test]
        public void MapRegion()
        {
            // Test with a RegionPosition as Input
            var regionPosition = new RegionPosition(new Position(new LatLon(50.97695325, 11.02396488)));
            var region = new Core.Models.Region(regionPosition);

            Assert.IsNotNull(region);
            Assert.IsInstanceOf<Core.Models.Region>(region);

            // Test with a Region as Input
            var region2 = new Region(region);

            Assert.IsNotNull(region2);
            Assert.IsInstanceOf<Core.Models.Region>(region2);
            Assert.AreEqual(region.RegionPosition, region2.RegionPosition);

            // Test with a RegionPosition and an TerrainDefinition as Input
            int[] res = { 0, 0, 0, 0, 0 };
            var terDef = new Core.Models.Definitions.TerrainDefinition(Core.Models.Definitions.EntityType.Grassland, res, true, true, 4, 5, 6);
            // var region3 = new Region(regionPosition, TerDef);
      }
    }
}