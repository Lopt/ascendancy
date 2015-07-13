using System;
using server.DB;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System.Diagnostics;

namespace test
{
    class MainClass
    {


        /*
        static int REGION_POS_X = 166148;
        static int REGION_POS_Y = 104835;
        static int RUNS = 10;

        static int REGIONS_X = 100;
        static int REGIONS_Y = 100;
        static int AFFECTED_REGIONS = 4;
        static int REQUESTS_PER_REGION = 32;

        static List<RegionPosition> NORTH_WEST = new List<RegionPosition> () {
            new RegionPosition (0, 0),
            new RegionPosition (-1, 1),
            new RegionPosition (0, 1),
            new RegionPosition (-1, 0),
        };

        static List<RegionPosition> NORTH_EAST = new List<RegionPosition> () {
            new RegionPosition (0, 0),
            new RegionPosition (1, 1),
            new RegionPosition (0, 1),
            new RegionPosition (1, 0),
        };

        static List<RegionPosition> SOUTH_WEST = new List<RegionPosition> () {
            new RegionPosition (0, 0),
            new RegionPosition (-1, -1),
            new RegionPosition (0, -1),
            new RegionPosition (-1, 0),
        };

        static List<RegionPosition> SOUTH_EAST = new List<RegionPosition> () {
            new RegionPosition (0, 0),
            new RegionPosition (1, -1),
            new RegionPosition (0, -1),
            new RegionPosition (1, 0),
        };

        static List<List<RegionPosition>> POSSIBLE_AFFECTED_REGIONS = new List<List<RegionPosition>> () {
            NORTH_WEST,
            NORTH_EAST,
            SOUTH_WEST,
            SOUTH_EAST
        };


        public static void PreloadRegions()
        {
            var regionManagerC = @base.control.Controller.Instance.RegionManagerController;

            for (var x = 0; x < REGIONS_X; ++x)
            {   for (var y = 0; y < REGIONS_Y; ++y)
                {
                    var regionPosition = new RegionPosition (REGION_POS_X + x, REGION_POS_Y + y);
                    regionManagerC.GetRegion (regionPosition);
                }
            }
        }

        private static void GenerateRequests(Account account)
        {
            var api = server.control.APIController.Instance;
            var rng = new Random();  
            var regionManagerC = @base.control.Controller.Instance.RegionManagerController;

            var maxRequests = REQUESTS_PER_REGION * REGIONS_X * REGIONS_Y;
            for (var requestNr = 0; requestNr <  maxRequests; ++requestNr)
            {
                var affectedRegions = new ConcurrentBag<Region> ();

                var centralPosition = new RegionPosition(REGION_POS_X + rng.Next() % REGIONS_X, REGION_POS_Y + rng.Next() % REGIONS_Y);
                var affectedPositions = POSSIBLE_AFFECTED_REGIONS [rng.Next() % POSSIBLE_AFFECTED_REGIONS.Count];

                for (var regionNr = 0; regionNr < AFFECTED_REGIONS; ++regionNr)
                {                       
                    var affectedRegionPosition = new RegionPosition (centralPosition.RegionX + affectedPositions [regionNr].RegionX,
                        centralPosition.RegionY + affectedPositions [regionNr].RegionY);

                    affectedRegions.Add (regionManagerC.GetRegion(affectedRegionPosition));
                }


                var paras = new ConcurrentDictionary<string, object>();
                paras["Regions"] = affectedRegions;
                var action = new @base.model.Action(account, @base.model.Action.ActionType.TestAction, paras);
                var actions = new @base.model.Action[] {action};
                api.DoAction(account, actions);
            }
        }
            

        private static void WaitUntilFinished()
        {
            var api = server.control.APIController.Instance;
            var done = false;

            while (!done)
            {
                done = true;
                for (var i = 0; i < server.model.ServerConstants.ACTION_THREADS; ++i)
                {
                    var x = api.m_threadingInfos [0].Count;
                    var y = api.m_threadingInfos [1].Count;

                    if (api.m_threadingInfos[i].Count > 0)
                    {
                        done = false;
                        Thread.Sleep (10);
                    }
                }
            }
        }

        public static void TestWriter()
        {
            var regionManagerC = @base.control.Controller.Instance.RegionManagerController;
            var account = new Account (0);
            var accountC = new server.control.AccountController(account, "passwd");
            var rng = new Random();  

            Console.WriteLine("Start");
            for (var runNr = 0; runNr < RUNS; ++runNr)
            {
                GenerateRequests (account);

                Console.WriteLine("Threading-Start (" + server.model.ServerConstants.ACTION_THREADS.ToString() + ")");

                Stopwatch runWatch = new Stopwatch();
                runWatch.Start();

                //server.MvcApplication.Phase = server.MvcApplication.Phases.Running;
                WaitUntilFinished ();
                //server.MvcApplication.Phase = server.MvcApplication.Phases.Pause;

                runWatch.Stop();
                TimeSpan ts = runWatch.Elapsed;

                // Format and display the TimeSpan value.
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                Console.WriteLine("Run-Zeit " + elapsedTime);
            }
        }



        public static void TestReader()
        {
            
        }
        */

        public static void Main(string[] args)
        {
            var json = "{\"Status\":0,\"Actions\":[[],[],[],[],[],[],[],[],[],[],[],[],[{\"Parameters\":{\"CreatePosition\":{\"X\":5316345,\"Y\":3354734},\"CreateBuilding\":276},\"Type\":2}],[],[],[],[],[],[],[],[],[],[],[],[]],\"Entities\":[]}";
            //var entitiesResponse = JsonConvert.DeserializeObject<core.connection.Response>(json);
            throw new Exception();
            //var app = new server.MvcApplication ();
            //app.Application_Start ();

            //server.MvcApplication.Phase = server.MvcApplication.Phases.Pause;


            //PreloadRegions ();

            //TestWriter ();


            //TestReader ();

            //server.MvcApplication.Phase = server.MvcApplication.Phases.Exit;
        }
    }
}