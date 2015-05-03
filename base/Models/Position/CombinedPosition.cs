using System;
using Newtonsoft.Json.Linq;

using @base.model;

namespace @base.model
{
    public class CombinedPosition
	{
        public CombinedPosition (Position position)
        {
            m_regionPosition = new RegionPosition(position);
            m_cellPosition = new CellPosition(position);
        }

        public CombinedPosition (RegionPosition regionPosition, CellPosition cellPosition)
		{
            m_regionPosition = regionPosition;
            m_cellPosition = cellPosition;
		}

        public CombinedPosition (JContainer obj)
        {
            m_regionPosition = new RegionPosition((JContainer) obj.SelectToken("RegionPosition"));
            m_cellPosition = new CellPosition((JContainer) obj.SelectToken("CellPosition"));
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

