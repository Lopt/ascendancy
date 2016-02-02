namespace Core.Controllers.AStar_Indicator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Controllers.Actions;
    using Core.Models;

    /// <summary>
    /// Indicator.
    /// </summary>
    public class Indicator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Controllers.AStar_Indicator.Indicator"/> class.
        /// </summary>
        /// <param name="startPosition">Start position.</param>
        /// <param name="moves">Moves.</param>
        /// <param name="accountID">Account I.</param>
        public Indicator(PositionI startPosition, int moves, int accountID)
        {
            m_startPoint = startPosition;
            m_moves = moves;
            m_accountID = accountID;
            m_indicatorPoints = new HashSet<PositionI>();
        }

        /// <summary>
        /// Finds the possible positions.
        /// </summary>
        /// <returns>The possible positions.</returns>
        public HashSet<PositionI> FindPossiblePositions()
        {
            Search();
            return m_indicatorPoints;
        }

        /// <summary>
        /// Search all tiles for possible Locations.
        /// </summary>
        private void Search()
        {
            m_indicatorPoints = GetSurroundedPositions(m_startPoint, m_accountID, m_moves);           
        }

        private static HashSet<PositionI> GetSurroundedPositions(PositionI startPoint, int accountID, int moves)
        {
            var fieldSet = new HashSet<PositionI>();
            var fieldSetHelper = new HashSet<PositionI>();

            fieldSet.Add(startPoint);
            fieldSetHelper.Add(startPoint);

            for (int index = 0; index != moves; ++index)
            {
                var tempfieldSet = new HashSet<PositionI>();
                foreach (var item in fieldSetHelper)
                {
                    var surroundedTiles = LogicRules.GetSurroundedFields(item);
                    foreach (var tile in surroundedTiles)
                    {
                        if (!fieldSet.Contains(tile))
                        {
                            var region = World.Instance.RegionManager.GetRegion(tile.RegionPosition);
                            var terrainDefinition = region.GetTerrain(tile.CellPosition);
                            if (terrainDefinition.Walkable)
                            {                                
                                var unit = region.GetEntity(tile.CellPosition);
                                if (unit == null)
                                {
                                    fieldSet.Add(tile);
                                    tempfieldSet.Add(tile);
                                }
                            }
                        }
                    }
                }
                fieldSetHelper = tempfieldSet;
            }
            return fieldSet;
        }

        /// <summary>
        /// The indicator points.
        /// </summary>
        private HashSet<PositionI> m_indicatorPoints;

        /// <summary>
        /// The movepoints from a unit.
        /// </summary>
        private int m_moves;

        /// <summary>
        /// The start point for the indicator.
        /// </summary>
        private PositionI m_startPoint;

        /// <summary>
        /// The account ID.
        /// </summary>
        private int m_accountID;
    }
}
