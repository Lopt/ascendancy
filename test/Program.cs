using System;
using server.control;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace test
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var world = @base.model.World.Instance;
			var controller = @base.control.Controller.Instance;

			controller.RegionManagerController = new RegionManagerController (@base.model.World.Instance.RegionManager);
			controller.TerrainManagerController = new TerrainManagerController ();



			var testAccount = new @base.model.Account (Guid.NewGuid(), "Test");


			var latlon = new @base.model.LatLon(50.9849, 11.0442);
			var position = new @base.model.Position(latlon);
			var combinedPos = new @base.model.CombinedPosition(position);
			var affectedRegion = controller.RegionManagerController.GetRegion (combinedPos.RegionPosition);

			var parameters = new ConcurrentDictionary<string, object> ();
			var regions = new @base.model.Region[1] { affectedRegion };

			parameters [@base.control.action.CreateHeadquarter.CREATE_POSITION] = combinedPos;
			var action = new @base.control.action.CreateHeadquarter (
				             testAccount,
				             regions,
				             parameters);

			foreach (var region in action.Regions)
			{
				region.AddAction (action);
			}

			var action2 = affectedRegion.GetAction();
			if (action2.Possible())
			{
				if (!action2.Do())
				{
					action2.Catch();
				}
			}

			affectedRegion.ActionCompleted ();

			Console.WriteLine(affectedRegion.GetEntity(combinedPos.CellPosition).GUID);


			/*
            var regionPosition = new @base.model.RegionPosition(position);
            var cellPosition = new @base.model.CellPosition(position);

            var terrain = .GetTerrain(cellPosition);
            //Console.WriteLine("/" + regionPosition.MajorX.ToString() + "/" + regionPosition.MajorY.ToString() + "/germany-" + regionPosition.RegionX.ToString() + "-" + regionPosition.RegionY.ToString() + ".png");
            //Console.WriteLine (world.RegionManager.GetRegion(new @base.model.RegionPosition(41504, 26188)).GetTerrain(new @base.model.CellPosition(31, 31)).TerrainType);
	        */
        }
    }
}