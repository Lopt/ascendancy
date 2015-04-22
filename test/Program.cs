using System;
using server.control;
using Newtonsoft.Json;

namespace test
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var world = @base.model.World.Instance;

            var terrainManager = new TerrainManager();
            var regionManager = new RegionManager();

            var latlon = new @base.model.LatLon(50.9849, 11.0442);
            var position = new @base.model.Position(latlon);
            var regionPosition = new @base.model.RegionPosition(position);
            var cellPosition = new @base.model.CellPosition(position);

            var terrain = world.RegionManager.GetRegion(regionPosition).GetTerrain(cellPosition);
            //Console.WriteLine("/" + regionPosition.MajorX.ToString() + "/" + regionPosition.MajorY.ToString() + "/germany-" + regionPosition.RegionX.ToString() + "-" + regionPosition.RegionY.ToString() + ".png");
            //Console.WriteLine (world.RegionManager.GetRegion(new @base.model.RegionPosition(41504, 26188)).GetTerrain(new @base.model.CellPosition(31, 31)).TerrainType);
        
        }
    }
}
