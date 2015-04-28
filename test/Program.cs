using System;
using @base.model;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace test
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var world = World.Instance;
			var controller = @base.control.Controller.Instance;


			var api = new server.control.APIController ();

			controller.RegionManagerController = new server.control.RegionManagerController ();
			controller.TerrainManagerController = new server.control.TerrainManagerController ();
			controller.AccountManagerController = new @base.control.AccountManagerController ();

			var testAccount = new Account (Guid.NewGuid(), "Test");
            var testAccountC = new server.control.Account (testAccount);

			var latlon = new LatLon(50.9849, 11.0442);
			var position = new Position(latlon);
			var combinedPos = new CombinedPosition(position);
			var affectedRegion = controller.RegionManagerController.GetRegion (combinedPos.RegionPosition);

			var parameters = new ConcurrentDictionary<string, object> ();

            parameters [@base.control.action.CreateHeadquarter.CREATE_POSITION] = combinedPos;
			var action = new @base.control.action.CreateHeadquarter (
				             testAccount,
				             parameters);


            Region[] regions = { affectedRegion };
            RegionPosition[] regionPositions = { affectedRegion.RegionPosition };
			@base.control.action.Action[] actions = { action };
			
			
            var test = api.LoadRegions (testAccount, regionPositions);

            api.DoAction (testAccount, actions);
            api.Worker(regions);

            var test2 = api.LoadRegions (testAccount, regionPositions);

            var bla = test2.ActionDict;
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