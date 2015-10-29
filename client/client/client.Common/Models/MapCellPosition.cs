namespace Client.Common.Models
{
    using System;
    using System.Reflection;
    using Client.Common.Helper;
    using CocosSharp;

    /// <summary>
    /// The Map cell position to convert between Tile Map and map cell.
    /// </summary>
    public class MapCellPosition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Models.MapCellPosition"/> class.
        /// </summary>
        /// <param name="cellX">Cell x.</param>
        /// <param name="cellY">Cell y.</param>
        public MapCellPosition(int cellX, int cellY)
        {
            m_cellX = cellX;
            m_celly = cellY;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Models.MapCellPosition"/> class.
        /// </summary>
        /// <param name="tileMapCoordinates">Tile map coordinates.</param>
        public MapCellPosition(CCTileMapCoordinates tileMapCoordinates)
        {
            var x = tileMapCoordinates.Column;
            var y = tileMapCoordinates.Row;
        
            m_cellX = (x * 2) + (y % 2);
            m_celly = y / 2;
        }

        /// <summary>
        /// Gets the cell x.
        /// </summary>
        /// <value>The cell x.</value>
        public int CellX
        {
            get
            {
                return m_cellX;
            }
        }

        /// <summary>
        /// Gets the cell y.
        /// </summary>
        /// <value>The cell y.</value>
        public int CellY
        {
            get
            {
                return m_celly;
            }
        }

        /// <summary>
        /// Gets the tile map coordinates.
        /// </summary>
        /// <returns>The tile map coordinates.</returns>
        public CCTileMapCoordinates GetTileMapCoordinates()
        {
            return new CCTileMapCoordinates(m_cellX / 2, (m_celly * 2) + (m_cellX % 2));
        }

        /// <summary>
        /// Gets the anchor.
        /// </summary>
        /// <returns>The anchor.</returns>
        public CCPoint GetAnchor()
        {
            float x = m_cellX / (Common.Constants.ClientConstants.CELLMAP_160x160_SIZE - 1.0f);
            float y = m_celly / (Common.Constants.ClientConstants.CELLMAP_160x160_SIZE - 1.0f);

            return new CCPoint(x, (1 - y) / 2);
        }

        /// <summary>
        /// The m_cell x.
        /// </summary>
        private readonly int m_cellX;

        /// <summary>
        /// The m_cell y.
        /// </summary>
        private readonly int m_celly;
    }
}