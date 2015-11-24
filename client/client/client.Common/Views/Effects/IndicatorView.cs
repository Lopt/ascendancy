namespace Client.Common.Views.Effects
{
    using CocosSharp;
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// Indicator view.
    /// </summary>
    public class IndicatorView
    {
        public IndicatorView(CCTileMapCoordinates center, CCTileMapLayer layer)
        {
            m_center = center;
            m_layer = layer;

        }

        public CCTileMapCoordinates[] GetSurroundedTiles(CCTileMapCoordinates center)
        {
            var coordHelper = new CCTileMapCoordinates[6];
            coordHelper[0] = new CCTileMapCoordinates(
                center.Column + (center.Row % 2),
                center.Row - 1);

            coordHelper[1] = new CCTileMapCoordinates(
                center.Column + (center.Row % 2),
                center.Row + 1);

            coordHelper[2] = new CCTileMapCoordinates(
                center.Column,
                center.Row + 2);

            coordHelper[3] = new CCTileMapCoordinates(
                center.Column - ((center.Row + 1) % 2),
                center.Row + 1);

            coordHelper[4] = new CCTileMapCoordinates(
                center.Column - ((center.Row + 1) % 2),
                center.Row - 1);

            coordHelper[5] = new CCTileMapCoordinates(
                center.Column,
                center.Row - 2);
            return coordHelper;   
        }

        public HashSet<CCTileMapCoordinates> GetSurroundedTilesinRange(CCTileMapCoordinates center, int range)
        {
            var TileSet = new HashSet<CCTileMapCoordinates>();
            var TileSetHelper = new HashSet<CCTileMapCoordinates>();

            TileSet.Add(center);
            TileSetHelper.Add(center);

            for (int i = 0; i != range; ++i)
            {
                var temptileset = new HashSet<CCTileMapCoordinates>();
                foreach (var item in TileSetHelper)
                {
                    var surroundedTiles = GetSurroundedTiles(item);
                    foreach (var tile in surroundedTiles)
                    {
                        if (!TileSet.Contains(tile))
                        {
                            TileSet.Add(tile);
                            temptileset.Add(tile);
                        }
                    }
                }
                TileSetHelper = temptileset;
            }
            return TileSet;
        }
//
//            Tilelist = new List<CCTileMapCoordinates>();
//
//            Tilelist.Add(center);
//            //var SurroudedTiles = GetSurroundedTiles(center);
//
//            //foreach (var node in SurroudedTiles)
//            //{
//            //    Testlist.Add(node);
//            //}
//
//            for (int i = 0; i != range; i++)
//            {
//                //Tiles = GetSurtiles(Tiles);
//                List<CCTileMapCoordinates> result = new List<CCTileMapCoordinates>(Tilelist);
//
//                foreach (var item in Tilelist)
//                {
//                    var test = GetSurroundedTiles(item);
//                    foreach (var tile in test)
//                    {
//                        if (!result.Contains(tile))
//                        {
//                            result.Add(tile);
//                        }
//                    }
//                }
//
//                Tilelist = result;
//            }
//            return Tilelist;

        /*List<CCTileMapCoordinates> GetSurtiles (List<CCTileMapCoordinates> list)
        {
            List<CCTileMapCoordinates> result = new List<CCTileMapCoordinates>(list);

            foreach (var item in list)
            {
                var test = GetSurroundedTiles(item);
                foreach (var tile in test)
                {
                    if (!result.Contains(tile))
                    {
                        result.Add(tile);
                    }
                }
            }
            
            return result;
        }*/

        public void ShowIndicator(CCTileMapCoordinates center, int range, CCTileMapLayer layer, int type)
        {
            var gid = new CCTileGidAndFlags(74);
            switch (type)
            {
                //influence range
                case 1:
                    gid = new CCTileGidAndFlags(74);
                    break;
                //attack range
                case 2:
                    gid = new CCTileGidAndFlags(75);
                    break;
                //movement range
                case 3:
                    gid = new CCTileGidAndFlags(76);
                    break;
            }

            surroundedTileSet = GetSurroundedTilesinRange(center, range);


            foreach (var tile in surroundedTileSet)
            {
                m_layer.SetTileGID(gid,tile);
            }
                
        }

        public void removeIndicator()
        {
            foreach (var item in surroundedTileSet)
            {
                m_layer.RemoveTile(item); 
            }
        }

        /// <summary>
        /// The center point for the indicator
        /// </summary>
        private CCTileMapCoordinates m_center;

        /// <summary>
        /// The layer which should be colored as indicator.
        /// </summary>
        private CCTileMapLayer m_layer;

        private HashSet<CCTileMapCoordinates> surroundedTileSet;
    }
}
