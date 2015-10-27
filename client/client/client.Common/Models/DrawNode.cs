namespace Client.Common.Models
{
    using System;
    using CocosSharp;

    /// <summary>
    /// The Draw node to draw polygons.
    /// </summary>
    public class DrawNode : CCDrawNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Models.DrawNode"/> class.
        /// </summary>
        public DrawNode()
        {
        }

        /// <summary>
        /// Draws the hexagon for a hex map.
        /// </summary>
        /// <param name="layer">Layer.</param>
        /// <param name="tileCoordinates">Tilecoordinates.</param>
        /// <param name="borderColor">Bordercolor.</param>
        /// <param name="opacity">Opacity.</param>
        /// <param name="borderWith">Borderwith.</param>
        public void DrawHexagonForHexMap(
            CCTileMapLayer layer, 
            CCTileMapCoordinates tileCoordinates, 
            CCColor4F borderColor, 
            byte opacity, 
            float borderWith)
        {
            // Calculate our width and height of the tile
            CCSize texelToContentScaling = CCTileMapLayer.DefaultTexelToContentSizeRatios;
            float tilewidth = layer.TileTexelSize.Width * texelToContentScaling.Width;
            float tileheight = layer.TileTexelSize.Height * texelToContentScaling.Height;

            // Convert the tile coordinates position to world coordinates for
            // our outline drawing
            var worldPos = layer.TilePosition(tileCoordinates);
            var x = worldPos.X + (tilewidth / 2);
            var y = worldPos.Y + (tileheight / 2);

            this.Clear();

            this.Opacity = opacity;

            var center = new CCPoint(x, y);

            var right = center;
            right.X += tilewidth / 2;

            var righttop = center;
            righttop.X += tilewidth / 4;
            righttop.Y += tileheight / 2;

            var rightbottom = center;
            rightbottom.X += tilewidth / 4;
            rightbottom.Y -= tileheight / 2;

            var left = center;
            left.X -= tilewidth / 2;

            var lefttop = center;
            lefttop.X -= tilewidth / 4;
            lefttop.Y += tileheight / 2;

            var leftbottom = center;
            leftbottom.X -= tilewidth / 4;
            leftbottom.Y -= tileheight / 2;

            DrawPolygon(
                new CCPoint[]
                {
                    right,
                    righttop,
                    lefttop,
                    left,
                    leftbottom,
                    rightbottom
                },
                6,
                CCColor4B.Transparent,
                3,
                borderColor);
        }

        /// <summary>
        /// Draws the hexagon for a iso stag map.
        /// </summary>
        /// <param name="pngWidth">Png width.</param>
        /// <param name="layer">Layer.</param>
        /// <param name="tileCoordinates">Tile coordinates.</param>
        /// <param name="borderColor">Border color.</param>
        /// <param name="opacity">Opacity.</param>
        /// <param name="borderWith">Border with.</param>
        public void DrawHexagonForIsoStagMap(
            float pngWidth,
            CCTileMapLayer layer,
            CCTileMapCoordinates tileCoordinates,
            CCColor4F borderColor,
            byte opacity,
            float borderWith)
        {
            // Calculate our width and height of the tile
            CCSize texelToContentScaling = CCTileMapLayer.DefaultTexelToContentSizeRatios;
            float tilewidth = layer.TileTexelSize.Width * texelToContentScaling.Width;
            float tileheight = layer.TileTexelSize.Height * texelToContentScaling.Height;

            // Convert the tile coordinates position to world coordinates for
            // our outline drawing
            var worldPos = layer.TilePosition(tileCoordinates);
            var x = worldPos.X + (tilewidth / 2);
            var y = worldPos.Y + (tileheight / 2);

            this.Clear();

            this.Opacity = opacity;

            var center = new CCPoint(x, y);

            var right = center;
            right.X += pngWidth / 2;

            var righttop = center;
            righttop.X += pngWidth / 4;
            righttop.Y += tileheight / 2;

            var rightbottom = center;
            rightbottom.X += pngWidth / 4;
            rightbottom.Y -= tileheight / 2;

            var left = center;
            left.X -= pngWidth / 2;

            var lefttop = center;
            lefttop.X -= pngWidth / 4;
            lefttop.Y += tileheight / 2;

            var leftbottom = center;
            leftbottom.X -= pngWidth / 4;
            leftbottom.Y -= tileheight / 2;

            DrawPolygon(
                new CCPoint[]
                {
                    right,
                    righttop,
                    lefttop,
                    left,
                    leftbottom,
                    rightbottom
                },
                6,
                CCColor4B.Transparent,
                3, 
                borderColor);
        }

        /// <summary>
        /// Draws the ISO trapez for iso stag map.
        /// </summary>
        /// <param name="pngWidth">Png width.</param>
        /// <param name="layer">Layer.</param>
        /// <param name="tileCoordinates">Tile coordinates.</param>
        /// <param name="borderColor">Border color.</param>
        /// <param name="opacity">Opacity.</param>
        /// <param name="borderWith">Border with.</param>
        public void DrawISOForIsoStagMap(
            float pngWidth,
            CCTileMapLayer layer,
            CCTileMapCoordinates tileCoordinates,
            CCColor4F borderColor, 
            byte opacity,
            float borderWith)
        {
            // Calculate our width and height of the tile
            CCSize texelToContentScaling = CCTileMapLayer.DefaultTexelToContentSizeRatios;
            float tilewidth = layer.TileTexelSize.Width * texelToContentScaling.Width;
            float tileheight = layer.TileTexelSize.Height * texelToContentScaling.Height;

            // Convert the tile coordinates position to world coordinates for
            // our outline drawing
            var worldPos = layer.TilePosition(tileCoordinates);
            var x = worldPos.X + (tilewidth / 2);
            var y = worldPos.Y + (tileheight / 2);

            this.Clear();

            this.Opacity = opacity;

            var center = new CCPoint(x, y);

            var right = center;
            right.X += pngWidth / 2;

            var top = center;
            top.Y += tileheight / 2;

            var bottom = center;
            bottom.Y -= tileheight / 2;

            var left = center;
            left.X -= pngWidth / 2;

            DrawPolygon(
                new CCPoint[]
                {
                    right,
                    top,
                    left,
                    bottom
                },
                4,
                CCColor4B.Transparent, 
                3,
                borderColor);
        }
    }
}
