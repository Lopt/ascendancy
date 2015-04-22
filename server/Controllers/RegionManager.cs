using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using @base.model;
using @base.model.definitions;

namespace server.control
{
    public class RegionManager : @base.control.RegionManager
	{
        public RegionManager()
        {
            for (int regionX = Constants.START_X; regionX < Constants.END_X; ++ regionX)
            {
                for (int regionY = Constants.START_Y; regionY < Constants.END_Y; ++ regionY)
                {
                    var regionPosition = new RegionPosition(regionX, regionY);
                    var path = ReplacePath(Constants.REGIONFILE, regionPosition);
                    string json = System.IO.File.ReadAllText(path);
                    var region = JsonToRegion(json, regionPosition);
                    World.Instance.RegionManager.AddRegion(region);

                }
            }
        }
    }
}

