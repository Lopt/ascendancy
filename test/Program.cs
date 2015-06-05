using System;
using @base.model;
using @server.DB;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace test
{
    class MainClass
    {
		enum test
		{
			a,
			b,
			c = 5,
			d,
			e,
				
		}
		
        public static void Main(string[] args)
		{

            Entity en = new Entity(0,new Definition(1),new PositionI(12,15));
            Account acc = new Account(0, "Test");
            DBHandle.Instance.CreateNewDBAccount(new Account(0, "Test"), "bla");
            DBHandle.Instance.InsertIntoResource(1, 2, 3, 4, 5, 6, 0);

            DBHandle.Instance.InsertIntoUnit(0, en);
            DBHandle.Instance.InsertIntoBuilding(0, en);
            DBHandle.Instance.InsertIntoBuilding(1, en);

            var data = DBHandle.Instance.GetAccountData(acc, "bla");

            DBHandle.Instance.DeleteAccountFromAllTables(0);

          

//            Console.WriteLine (a);
//            Console.WriteLine (d);


//            var latlon = new LatLon(50.9849, 11.0442);
//            var position = new Position(latlon);
//            var combinedPos = new CombinedPosition(position);
//            //var affectedRegion = controller.RegionManagerController.GetRegion (combinedPos.RegionPosition);


//            var request = new @base.connection.LoginRequest (new Position (0, 0), "Test", "Test");
//            Console.WriteLine(JsonConvert.SerializeObject (request));

//            RegionPosition[] regionPositions = {combinedPos.RegionPosition };

//            var request2 = new @base.connection.LoadRegionsRequest (Guid.NewGuid(), position, regionPositions);
//            Console.WriteLine(JsonConvert.SerializeObject (request2));

//            var testAccount = new Account (Guid.NewGuid(), "Test");
//            var testAccountC = new server.control.AccountController (testAccount, "Test");


//            var parameters = new ConcurrentDictionary<string, object> ();
//            parameters [@base.control.action.CreateHeadquarter.CREATE_POSITION] = combinedPos;
//            var action = new @base.model.Action (testAccount, @base.model.Action.ActionType.CreateHeadquarter, parameters);

//            @base.model.Action[] actions = { action, };

//            var request3 = new @base.connection.DoActionsRequest (Guid.NewGuid(), position, actions);


//            //			var response = new @base.connection.Response();
////			var requestdoubled = JsonConvert.DeserializeObject<@base.connection.Response>(JsonConvert.SerializeObject(response));

//            Console.WriteLine(JsonConvert.SerializeObject (request3));




			/*
            var world = World.Instance;
			var controller = @base.control.Controller.Instance;

			Region[] regions = { affectedRegion };

			var api = new server.control.APIController ();

			controller.RegionManagerController = new server.control.RegionManagerController ();
			controller.TerrainManagerController = new server.control.TerrainManagerController ();
			controller.AccountManagerController = new server.control.AccountManagerController ();



			var parameters = new ConcurrentDictionary<string, object> ();
            parameters [@base.control.action.CreateHeadquarter.CREATE_POSITION] = combinedPos;
			var action = new @base.control.action.CreateHeadquarter (
				             testAccount,
				             parameters);


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