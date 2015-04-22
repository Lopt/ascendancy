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
            for (int regionX = Constants.startX; regionX < Constants.endX; ++ regionX)
            {
                for (int regionY = Constants.startY; regionY < Constants.endY; ++ regionY)
                {
                    var regionPosition = new RegionPosition(regionX, regionY);
                    var path = ReplacePath(Constants.regionFile, regionPosition);
                    string json = System.IO.File.ReadAllText(path);
                    var region = JsonToRegion(json, regionPosition);
                    World.Instance.RegionManager.AddRegion(region);

                }
            }
        }
    }
}

