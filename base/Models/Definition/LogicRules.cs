using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

using @base.model;
using @base.Models;

namespace @base.Models.Definition
{
    class LogicRules
    {       
        public static ConcurrentBag<PositionI> SurroundTiles
        {
            get
            {
                ConcurrentBag<PositionI> list = new ConcurrentBag<PositionI>();

                // From North to NorthEast in clockwise
                list.Add(new PositionI(0, 1));
                list.Add(new PositionI(1, 0));
                list.Add(new PositionI(1, 1));
                list.Add(new PositionI(0, 1));
                list.Add(new PositionI(-1, -1));
                list.Add(new PositionI(-1, 0));

                return list;
            }
        }

        public static ConcurrentBag<RegionPosition> SurroundRegions
        {

            get
            {
                ConcurrentBag<RegionPosition> list = new ConcurrentBag<RegionPosition>();
                      
                // Surrounded Regions from topleft clockwise
                list.Add(new RegionPosition(-1, -1));
                list.Add(new RegionPosition(-1,  0));
                list.Add(new RegionPosition(-1, +1));
                list.Add(new RegionPosition( 0, +1));
                list.Add(new RegionPosition(+1, +1));
                list.Add(new RegionPosition(+1,  0));
                list.Add(new RegionPosition(+1, -1));
                list.Add(new RegionPosition( 0, -1));

                return list;
            }
        }

    }
}
