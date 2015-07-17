using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Core.Models;
using server.model;

namespace server.control
{
	/// <summary>
	/// Contains all loaded regions and loads regions.
	/// </summary>
    public class RegionManagerController : Core.Controllers.RegionManagerController
    {
        public RegionManagerController()
        {
        }

		/// <summary>
		/// Tries to get the Region.
		/// </summary>
		/// <returns>The region. If it couln't be loaded, region.exist is false.</returns>
		/// <param name="regionPosition">Region position.</param>
        override public Region GetRegion(RegionPosition regionPosition)
        {
            var regionManager = World.Instance.RegionManager;
            var region = regionManager.GetRegion(regionPosition);
            if (!region.Exist)
            {
                var path = Core.Helper.LoadHelper.ReplacePath(ServerConstants.REGION_FILE, regionPosition);
                try
                {
                    string json = System.IO.File.ReadAllText(path);
					region.AddTerrain(Core.Helper.LoadHelper.JsonToTerrain(json));
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

