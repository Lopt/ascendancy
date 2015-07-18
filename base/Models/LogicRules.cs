using System;
using System.Collections;
using System.Collections.Generic;

using Core.Models;

namespace Core.Models
{
    class LogicRules
    {
        // From North to NorthEast in clockwise
        public static readonly PositionI[] SurroundTilesEven =
            {
                new PositionI(0, -1),
                new PositionI(1, 0),
                new PositionI(1, 1),
                new PositionI(0, 1),
                new PositionI(-1, 1),
                new PositionI(-1, 0)
            };

        // From North to NorthEast in clockwise
        public static readonly PositionI[] SurroundTilesOdd =
            {
                new PositionI(0, -1),
                new PositionI(1, -1),
                new PositionI(1, 0),
                new PositionI(0, 1),
                new PositionI(-1, 0),
                new PositionI(-1, -1)
            };

        public static PositionI[] GetSurroundedFields(PositionI pos)
        {
            var surroundedFields = SurroundTilesEven;
            if (pos.X % 2 == 0)
            {
                surroundedFields = SurroundTilesOdd;
            }

            var surrounded = new PositionI[6];
            for (var Index = 0; Index < surroundedFields.Length; ++Index)
            {
                surrounded[Index] = pos + surroundedFields[Index];
            }
            return surrounded;
        }




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
    }        
}
