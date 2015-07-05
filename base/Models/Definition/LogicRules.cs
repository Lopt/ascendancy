using System;
using System.Collections;
using System.Collections.Generic;

using @base.model;
using @base.Models;

namespace @base.Models.Definition
{
    class LogicRules
    {
        // From North to NorthEast in clockwise
        public static readonly PositionI[] SurroundTiles = 
        {
                    new PositionI( 0, 1),
                    new PositionI( 1, 0),
                    new PositionI( 1, 1),
                    new PositionI( 0,-1),
                    new PositionI(-1,-1),
                    new PositionI(-1, 0)
        };

        // Surrounded Regions from topleft clockwise
        public static readonly RegionPosition[] SurroundRegions = 
        {
                new RegionPosition(-1, -1),
                new RegionPosition(-1,  0),
                new RegionPosition(-1, +1),
                new RegionPosition( 0, +1),
                new RegionPosition(+1, +1),
                new RegionPosition(+1,  0),
                new RegionPosition(+1, -1),
                new RegionPosition( 0, -1)
        };

        public static bool FightSystem()
        {
            var result = new Random().Next(1,10);
            
            if (result > 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }        
}
