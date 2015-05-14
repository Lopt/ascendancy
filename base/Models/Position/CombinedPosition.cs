using System;
using Newtonsoft.Json.Linq;

using @base.model;

namespace @base.model
{
    public class CombinedPositionA
	{
        public CombinedPositionA (Position position)
        {
            m_regionPosition = new RegionPosition(position);
            m_cellPosition = new CellPosition(position);
        }

        public CombinedPositionA (RegionPosition regionPosition, CellPosition cellPosition)
		{
            m_regionPosition = regionPosition;
            m_cellPosition = cellPosition;
		}

        public CombinedPositionA (JContainer obj)
        {
            m_regionPosition = new RegionPosition((JContainer) obj.SelectToken("RegionPosition"));
            m_cellPosition = new CellPosition((JContainer) obj.SelectToken("CellPosition"));
        }

        public CombinedPositionA Add(int X, int Y)
        {
            var newCellX = m_cellPosition.CellX + X % Constants.REGION_SIZE_X;
            var newCellY = m_cellPosition.CellY + Y % Constants.REGION_SIZE_Y;

            var newRegionX = m_regionPosition.RegionX + X / Constants.REGION_SIZE_X;
            var newRegionY = m_regionPosition.RegionY + Y / Constants.REGION_SIZE_Y;

            return new CombinedPositionA(new RegionPosition(newRegionX, newRegionY),
                new CellPosition(newCellX, newCellY));
        }
            
        public RegionPosition RegionPosition
		{
			get { return this.m_regionPosition; }
		}

        public CellPosition CellPosition
		{
			get { return this.m_cellPosition; }
		}

        RegionPosition m_regionPosition;
        CellPosition m_cellPosition;
	}
}

