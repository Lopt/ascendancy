using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Core.Models;
using server.model;

namespace server.control
{
    public class RegionManagerController : Core.Controllers.RegionManagerController
    {
        public RegionManagerController()
        {
        }

        override public Region GetRegion(RegionPosition regionPosition)
        {
            var regionManager = World.Instance.RegionManager;
            var region = regionManager.GetRegion(regionPosition);
            if (!region.Exist)
            {
                var path = ReplacePath(ServerConstants.REGION_FILE, regionPosition);
                try
                {
                    string json = System.IO.File.ReadAllText(path);
                    region.AddTerrain(JsonToTerrain(json));
                    regionManager.AddRegion(region);
                }
                catch (System.IO.DirectoryNotFoundException exception)
                {
                    Console.WriteLine(exception.ToString());
                }
                catch (System.IO.FileNotFoundException exception)
                {
                    Console.WriteLine(exception.ToString());
                }
            }
            return region;
        }
    }
}

