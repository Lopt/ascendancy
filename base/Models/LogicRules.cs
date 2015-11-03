namespace Core.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Core.Models;

    /// <summary>
    /// Logic rules.
    /// </summary>
    public class LogicRules
    {
        /// <summary>
        /// The surround tiles on even x positions.
        /// From North to NorthEast in clockwise
        /// </summary>
        public static readonly PositionI[] SurroundTilesEven =
        {
            new PositionI(0, -1),
            new PositionI(1, 0),
            new PositionI(1, 1),
            new PositionI(0, 1),
            new PositionI(-1, 1),
            new PositionI(-1, 0)
        };

        /// <summary>
        /// The surround tiles on odd x positions.
        /// From North to NorthEast in clockwise
        /// </summary>
        public static readonly PositionI[] SurroundTilesOdd =
        {
            new PositionI(0, -1),
            new PositionI(1, -1),
            new PositionI(1, 0),
            new PositionI(0, 1),
            new PositionI(-1, 0),
            new PositionI(-1, -1)
        };

        /// <summary>
        /// Gets the surrounded fields.
        /// </summary>
        /// <returns>The surrounded fields.</returns>
        /// <param name="pos">Center Position.</param>
        public static PositionI[] GetSurroundedFields(PositionI pos)
        {
            var surroundedFields = SurroundTilesEven;
            if (pos.X % 2 == 0)
            {
                surroundedFields = SurroundTilesOdd;
            }

            var surrounded = new PositionI[6];
            for (var index = 0; index < surroundedFields.Length; ++index)
            {
                surrounded[index] = pos + surroundedFields[index];
            }
            return surrounded;
        }

        /// <summary>
        /// Surrounded Regions from top left clockwise
        /// </summary>
        public static readonly RegionPosition[] SurroundRegions =
        {
            new RegionPosition(-1, -1),
            new RegionPosition(-1,  0),
            new RegionPosition(-1, +1),
            new RegionPosition(0, +1),
            new RegionPosition(+1, +1),
            new RegionPosition(+1,  0),
            new RegionPosition(+1, -1),
            new RegionPosition(0, -1)
        };
    }        
}
