using System;
using System.Reflection;
using CocosSharp;

namespace client.Common.Models
{
	public class MapCellPosition
	{
		public MapCellPosition (int cellX, int cellY)
		{
			m_CellX = cellX;
			m_CellY = cellY;
		}

		public MapCellPosition (CCTileMapCoordinates tileMapCoordinates)
		{
			var x = tileMapCoordinates.Column;
			var y = tileMapCoordinates.Row;
			m_CellX = (x * 2) + (y % 2);
			m_CellY = (y / 2);
		}

		public int CellX {
			get { return m_CellX; }
		}

		public int CellY {
			get { return m_CellY; }
		}

		public CCTileMapCoordinates GetTileMapCoordinates ()
		{
			return new CCTileMapCoordinates (m_CellX / 2, (m_CellY * 2) + (m_CellX % 2));
		}

		public CCPoint GetMapPoint ()
		{
			var tileMapCoordinates = GetTileMapCoordinates ();
			float x = tileMapCoordinates.Column / 80.0f;
			float y = tileMapCoordinates.Row / 320.0f;
			return new CCPoint (x, y);
		}

		private readonly int m_CellX;
		private readonly int m_CellY;
	}
}

