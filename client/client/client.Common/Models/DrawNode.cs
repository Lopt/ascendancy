using System;
using CocosSharp;

namespace client.Common.Models
{
	public class DrawNode : CCDrawNode
	{
		public DrawNode ()
		{
		}

		public void DrawHexagonForHexMap (CCTileMapLayer layer, CCTileMapCoordinates tileCoordinates, CCColor4F borderColor, byte opacity, float borderWith)
		{
			// Calculate our width and height of the tile
			CCSize texelToContentScaling = CCTileMapLayer.DefaultTexelToContentSizeRatios;
			float Tilewidth = layer.TileTexelSize.Width * texelToContentScaling.Width;
			float Tileheight = layer.TileTexelSize.Height * texelToContentScaling.Height;

			// Convert the tile coordinates position to world coordinates for
			// our outline drawing
			var worldPos = layer.TilePosition (tileCoordinates);
			var X = worldPos.X + (Tilewidth / 2);
			var Y = worldPos.Y + (Tileheight / 2);

			//zeichnet ein Sechseck um das getouchte Feld
			Clear ();

			Opacity = opacity;

			var center = new CCPoint (X, Y);

			var right = center;
			right.X += Tilewidth / 2;

			var righttop = center;
			righttop.X += Tilewidth / 4;
			righttop.Y += Tileheight / 2;

			var rightbottom = center;
			rightbottom.X += Tilewidth / 4;
			rightbottom.Y -= Tileheight / 2;

			var left = center;
			left.X -= Tilewidth / 2;

			var lefttop = center;
			lefttop.X -= Tilewidth / 4;
			lefttop.Y += Tileheight / 2;

			var leftbottom = center;
			leftbottom.X -= Tilewidth / 4;
			leftbottom.Y -= Tileheight / 2;

			// Hightlight our iso tile
			DrawPolygon (new CCPoint[] { right, righttop, lefttop, left, leftbottom, rightbottom }, 6, CCColor4B.Transparent, 3, borderColor);

		}

		public void DrawHexagonForIsoStagMap (float pngWidth, CCTileMapLayer layer, CCTileMapCoordinates tileCoordinates, CCColor4F borderColor, byte opacity, float borderWith)
		{
			// Calculate our width and height of the tile
			CCSize texelToContentScaling = CCTileMapLayer.DefaultTexelToContentSizeRatios;
			float Tilewidth = layer.TileTexelSize.Width * texelToContentScaling.Width;
			float Tileheight = layer.TileTexelSize.Height * texelToContentScaling.Height;

			// Convert the tile coordinates position to world coordinates for
			// our outline drawing
			var worldPos = layer.TilePosition (tileCoordinates);
			var X = worldPos.X + (Tilewidth / 2);
			var Y = worldPos.Y + (Tileheight / 2);

			//zeichnet ein Sechseck um das getouchte Feld
			Clear ();

			Opacity = opacity;

			var center = new CCPoint (X, Y);

			var right = center;
			right.X += pngWidth / 2;

			var righttop = center;
			righttop.X += pngWidth / 4;
			righttop.Y += Tileheight / 2;

			var rightbottom = center;
			rightbottom.X += pngWidth / 4;
			rightbottom.Y -= Tileheight / 2;

			var left = center;
			left.X -= pngWidth / 2;

			var lefttop = center;
			lefttop.X -= pngWidth / 4;
			lefttop.Y += Tileheight / 2;

			var leftbottom = center;
			leftbottom.X -= pngWidth / 4;
			leftbottom.Y -= Tileheight / 2;

			// Hightlight our iso tile
			DrawPolygon (new CCPoint[] { right, righttop, lefttop, left, leftbottom, rightbottom }, 6, CCColor4B.Transparent, 3, borderColor);

		}
	}
}

