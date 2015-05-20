using System;
using CocosSharp;
using @base.model;
using client.Common.helper;

namespace client.Common.Helper
{
	public class Modify
	{
		public Modify ()
		{
		}

		public static float GetScaleFactor (CCSize Content, CCSize Space)
		{
			var ContentSquare = Content.Height * Content.Width;
			var SpaceSquare = Space.Height * Space.Width;
			var ScaleFactor = SpaceSquare / ContentSquare;
			return ScaleFactor;
		}

		//		public static CCTileMapCoordinates MapCellPosToTilePos (CellPosition cellPosition)
		//		{
		//			return MapCellPosToTilePos (cellPosition.CellX, cellPosition.CellY);
		//		}
		//
		//		public static CCTileMapCoordinates MapCellPosToTilePos (int x, int y)
		//		{
		//			return new CCTileMapCoordinates (x / 2, (y * 2) + (x % 2));
		//		}
		//
		//		public static CellPosition TilePosToMapCellPos (CCTileMapCoordinates tileMapCoordinates)
		//		{
		//			var x = tileMapCoordinates.Column;
		//			var y = tileMapCoordinates.Row;
		//			return new CellPosition ((x * 2) + (y % 2), y / 2);
		//		}
		//
		//		public static CCPoint TilePosToMapPoint (CCTileMapCoordinates tileMapCoordinates)
		//		{
		//			float x = tileMapCoordinates.Column / 80.0f;
		//			float y = tileMapCoordinates.Row / 320.0f;
		//			return new CCPoint (x, y / 2.0f);
		//		}
	}
}

