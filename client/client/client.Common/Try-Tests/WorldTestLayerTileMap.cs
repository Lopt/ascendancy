using System;
using CocosSharp;
using System.Collections.Generic;
using SQLitePCL;
using Microsoft.Xna.Framework.Input.Touch;
using System.Diagnostics;
using client.Common.Models;
using client.Common.Controllers;
using @base.control;
using @base.model;


namespace client.Common.TryTests
{
	public class WorldTestLayerTileMap : CCLayerColor
	{
		private CCTileMap TileMap;
		private DrawNode drawNode;
		private CCSprite water;

		float X;
		float Y;
		Stopwatch Watch;
		RegionController regionController;
		bool MapIsChanged = true;
		RegionPosition m_regionPosition = null;

		public WorldTestLayerTileMap (RegionPosition regionPosition)
		{
			regionController = Controller.Instance.RegionManagerController as RegionController;
			m_regionPosition = regionPosition;
			Watch = new Stopwatch ();
			//CCTile tile = new CCTile ();
			TileMap = new CCTileMap ("Worldmap-64x16-smalltiles-iso");
			//var info = new CCTileMapInfo ();
			AddChild (TileMap);

		
			CCLayerColor color = new CCLayerColor (new CCColor4B (0, 0, 255, 255));
			AddChild (color, -1);

			drawNode = new DrawNode ();
			TileMap.TileLayersContainer.AddChild (drawNode);

			water = new CCSprite ("water");
			water.AnchorPoint = CCPoint.AnchorMiddle;
			TileMap.TileLayersContainer.AddChild (water);

			//tile.Position(new CCPoint())

			// Register Touch Event
			var touchListenerMove = new CCEventListenerTouchAllAtOnce ();
			touchListenerMove.OnTouchesMoved = onTouchesMoved;
			touchListenerMove.OnTouchesEnded = onToucheEnds;

			var touchListenerBegan = new CCEventListenerTouchOneByOne ();
			touchListenerBegan.OnTouchBegan = onTouchBegan;
			touchListenerBegan.OnTouchEnded = onLongTouch;
		

			AddEventListener (touchListenerMove);
			AddEventListener (touchListenerBegan);
			//AddEventListener (touchListenerEnd);

			this.Schedule (SetMap);
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			TileMap.TileLayersContainer.PositionX = VisibleBoundsWorldspace.MaxX / 2;
			TileMap.TileLayersContainer.PositionY = VisibleBoundsWorldspace.MaxY / 2;
			TileMap.TileLayersContainer.AnchorPoint = new CCPoint (0.5f, 0.25f);

		}

		void SetMap (float FrameTimesInSecond)
		{
			if (regionController.GetRegion (m_regionPosition).Exist && MapIsChanged) {
				regionController.SetTilesInMap (TileMap.LayerNamed ("Layer 0"), new CCTileMapCoordinates (0, 0), regionController.GetRegion (m_regionPosition));
				MapIsChanged = false;
			}
				

		}

		void onTouchesMoved (List<CCTouch> touches, CCEvent touchEvent)
		{
			var touch = touches [0];
			CCPoint diff = touch.Delta;
			TileMap.TileLayersContainer.Position += diff;
		}

		bool onTouchBegan (CCTouch touch, CCEvent touchEvent)
		{
			Watch.Start ();
			//var layer = TileMap.LayerNamed ("Kachelebene 1");
			var layer = TileMap.LayerNamed ("Layer 0");
			var layersize = layer.LayerSize;
			var layertyp = layer.MapType;
			var layersetinfo = layer.TileSetInfo;

			var location = layer.WorldToParentspace (touch.Location);
			var tileCoordinates = layer.ClosestTileCoordAtNodePosition (location);

			var gid = layer.TileGIDAndFlags (tileCoordinates);
			var gid0 = layer.TileGIDAndFlags (0, 0);
			layer.SetTileGID (gid, new CCTileMapCoordinates (0, 0));

			//layer.ReplaceTileGIDQuad (gid.Gid, gid0.Gid);
			//var sprite = layer.ExtractTile (tileCoordinates);
			//layer.AddChild (new CCSprite("water"));

			//layer.SetTileGID (gid, new CCTileMapCoordinates (0, 6));
			//layer.RemoveTile (tileCoordinates);

			return true;
		}

		void onToucheEnds (List<CCTouch> touches, CCEvent touchEvent)
		{


			//water.Position = worldPos;
		}

		void onLongTouch (CCTouch touch, CCEvent touchEvent)
		{
			Watch.Stop ();
			if (Watch.ElapsedMilliseconds > 2000) {
				Watch.Reset ();
				//var layer = TileMap.LayerNamed ("Kachelebene 1");
				var layer = TileMap.LayerNamed ("Layer 0");

				var location = layer.WorldToParentspace (touch.Location);
				var tileCoordinates = layer.ClosestTileCoordAtNodePosition (location);

				drawNode.DrawHexagonForIsoStagMap (83.0f, layer, tileCoordinates, new CCColor4F (CCColor3B.Red), 255, 3.0f);

			}
		
		}

	}

}

