using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using @base.model;
using server.model;

namespace server.control
{
	public class RegionManagerController : @base.control.RegionManagerController
	{
		public RegionManagerController()
			: base()
		{
		}

		override public Region GetRegion(RegionPosition regionPosition)
		{
			var region = RegionManager.GetRegion (regionPosition);
			if (!region.Exist)
			{
				var path = ReplacePath(ServerConstants.REGION_FILE, regionPosition);
				try 
				{
					string json = System.IO.File.ReadAllText(path);
					region.AddTerrain(JsonToTerrain(json));
					RegionManager.AddRegion(region);
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

