﻿using Core.Controllers.Actions;
using Core.Models;
using client.Common.Helper;
using client.Common.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using CocosSharp;
using Microsoft.Xna.Framework;
using client.Common.Manager;




namespace client.Common.Views
{
    public class WorldLayer : CCLayerColor
    {

        public enum Phases
        {
            Start,
            LoadTerrain,
            LoadEntities,
            Idle,
            Exit
        }

        public Phases Phase
        {
            get;
            private set;
        }

        public RegionView RegionView
        {
            get;
            private set;
        }

        public Position CenterPosition
        {
            get;
            private set;
        }

        public WorldLayer(GameScene gameScene)
            : base()
        {
            m_gameScene = gameScene;

            RegionView = new RegionView();
			m_regionManagerController = Core.Controllers.Controller.Instance.RegionManagerController as client.Common.Manager.RegionManagerController;

            WorldTileMap = new CCTileMap(ClientConstants.TILEMAP_FILE);
            m_geolocation = Geolocation.Instance;
            CenterPosition = m_geolocation.CurrentGamePosition;

            m_currentPositionNode = new DrawNode();
            WorldTileMap.TileLayersContainer.AddChild(m_currentPositionNode);

            TerrainLayer = WorldTileMap.LayerNamed(ClientConstants.LAYER_TERRAIN);
            BuildingLayer = WorldTileMap.LayerNamed(ClientConstants.LAYER_BUILDING);
            UnitLayer = WorldTileMap.LayerNamed(ClientConstants.LAYER_UNIT);
            MenuLayer = WorldTileMap.LayerNamed(ClientConstants.LAYER_MENU);


            ClearLayers();

            RegionView.TerrainLayer = TerrainLayer;
            RegionView.BuildingLayer = BuildingLayer;
            RegionView.UnitLayer = UnitLayer;
            RegionView.MenuLayer = MenuLayer;


           
            this.AddChild(WorldTileMap);


            this.Schedule(CheckGeolocation);


            m_worker = new Views.Worker(this);
            EntityManagerController.Worker = m_worker;

            Schedule(m_worker.Schedule);


        }

        #region overide

        protected override void AddedToScene()
        {
            base.AddedToScene();

            SetMapAnchor(m_geolocation.CurrentGamePosition);
            WorldTileMap.TileLayersContainer.PositionX = VisibleBoundsWorldspace.MidX;
            WorldTileMap.TileLayersContainer.PositionY = VisibleBoundsWorldspace.MidY;
            ScaleWorld(ClientConstants.TILEMAP_NORM_SCALE);

        }

        #endregion

		/// <summary>
		/// Creates a unit with a given Location and unit type.
		/// </summary>
		/// <param name="location">Given Location.</param>
		/// <param name="type">Unittype.</param>
        public void CreateUnit(CCTileMapCoordinates location, int type)
        {
            var dictParam = new System.Collections.Generic.Dictionary<string,object>();
            var tapMapCellPosition = new MapCellPosition(location);
            var tapPosition = RegionView.GetCurrentGamePosition(tapMapCellPosition, CenterPosition.RegionPosition);
            var tapPositionI = new PositionI((int)tapPosition.X, (int)tapPosition.Y);
            dictParam[Core.Controllers.Actions.CreateUnit.CREATE_POSITION] = tapPositionI; 
            dictParam[Core.Controllers.Actions.CreateUnit.CREATION_TYPE] = (long)type;
            var newAction = new Core.Models.Action(GameAppDelegate.Account, Core.Models.Action.ActionType.CreateUnit, dictParam);
            var actionC = (Core.Controllers.Actions.Action)newAction.Control;
            var possible = actionC.Possible(m_regionManagerController);

            if (possible)
            {
                //actionC.Do (m_regionManagerController);
                //m_worker.Queue.Enqueue (newAction);
                DoAction(newAction);
            }
        }

		/// <summary>
		/// Creates the building at a given Location and Building Type.
		/// </summary>
		/// <param name="location">Given Location.</param>
		/// <param name="type">Buildingtype.</param>
        public void CreateBuilding(CCTileMapCoordinates location, long type)
        {
            var dictParam = new System.Collections.Generic.Dictionary<string,object>();
            var tapMapCellPosition = new MapCellPosition(location);
            var tapPosition = RegionView.GetCurrentGamePosition(tapMapCellPosition, CenterPosition.RegionPosition);
            var tapPositionI = new PositionI((int)tapPosition.X, (int)tapPosition.Y);
            dictParam[Core.Controllers.Actions.CreatBuilding.CREATE_POSITION] = tapPositionI; 
            dictParam[Core.Controllers.Actions.CreatBuilding.CREATION_TYPE] = (long)type;
			var newAction = new Core.Models.Action(GameAppDelegate.Account, Core.Models.Action.ActionType.CreateBuilding, dictParam);
            var actionC = (Core.Controllers.Actions.Action)newAction.Control;
            var possible = actionC.Possible(m_regionManagerController);

            if (possible)
            {
                //actionC.Do (m_regionManagerController);
                //m_worker.Queue.Enqueue (newAction);      
                DoAction(newAction);
            }
        }

        #region Scheduling

        public void UglyDraw()
        {
            //TODO: find better solution
            WorldTileMap.TileLayersContainer.Position += new CCPoint(0.0001f, 0.0001f);
        }

        void CheckGeolocation(float frameTimesInSecond)
        {
            if (m_geolocation.IsPositionChanged)
            {
                DrawRegionsAsync(m_geolocation.CurrentGamePosition);
                m_geolocation.IsPositionChanged = false;
            }

        }

        #endregion

        #region


        void SetCurrentPositionOnce(Position position)
        {
            var tileCoordinate = Helper.PositionHelper.PositionToTileMapCoordinates(CenterPosition, new PositionI(position));
            m_currentPositionNode.DrawHexagonForIsoStagMap(ClientConstants.TILE_IMAGE_WIDTH, TerrainLayer,
                tileCoordinate, new CCColor4F(CCColor3B.Red), 255, 3.0f);
//            var tileCoordinate = m_regionView.GetCurrentTileInMap (m_geolocation.CurrentGamePosition);

            bool isInWorld = false;
            m_currentPositionNode.Visible = false;

            if (CenterPosition.RegionPosition.Equals(m_geolocation.CurrentRegionPosition))
                isInWorld = true;

            if (tileCoordinate.Column > -1 && isInWorld)
            {
                m_currentPositionNode.Visible = true;
                m_currentPositionNode.DrawHexagonForIsoStagMap(ClientConstants.TILE_IMAGE_WIDTH, TerrainLayer,
                    tileCoordinate, new CCColor4F(CCColor3B.Red), 255, 3.0f);
            } 
        }

        async Task DrawRegionsAsync(Position gamePosition)
        {
            CenterPosition = gamePosition;
            Phase = Phases.LoadTerrain;
            await m_regionManagerController.LoadRegionsAsync(new RegionPosition(gamePosition));
            Phase = Phases.LoadEntities;
            await EntityManagerController.Instance.LoadEntitiesAsync(gamePosition, CenterPosition.RegionPosition);
            Phase = Phases.Idle;


            RegionView.SetTilesInMap160(m_regionManagerController.GetRegionByGamePosition(gamePosition));
            SetCurrentPositionOnce(gamePosition);
            SetMapAnchor(gamePosition);

            CenterPosition = gamePosition;
            //SetMapAnchor (gamePosition);
            UglyDraw();
        }

        public CCPoint LayerWorldToParentspace(CCPoint point)
        {
            return TerrainLayer.WorldToParentspace(point);
        }

        public CCTileMapCoordinates ClosestTileCoordAtNodePosition(CCPoint point)
        {
            return TerrainLayer.ClosestTileCoordAtNodePosition(point);
        }

        public void DoAction(Core.Models.Action action)
        {
            var actions = new List<Core.Models.Action>();
            actions.Add(action);
            m_regionManagerController.DoActionAsync(m_geolocation.CurrentGamePosition, actions.ToArray());
            //var mapCell = GetMapCell(m_terrainLayer, new CCPoint(VisibleBoundsWorldspace.MidX, VisibleBoundsWorldspace.MidY));
            //var position = RegionView.GetCurrentGamePosition(mapCell, CenterPosition.RegionPosition);
            //DrawRegionsAsync (position);
        }

        /// <summary>
		/// Clears the Layers for initialization.
        /// </summary>
        void ClearLayers()
        {
            var coordHelper = new CCTileMapCoordinates(0, 0);
            BuildingLayer.RemoveTile(coordHelper);
            UnitLayer.RemoveTile(coordHelper);
            MenuLayer.RemoveTile(coordHelper);
        }



        public void CheckCenterRegion()
        {  
            var mapCell = GetMapCell(TerrainLayer, new CCPoint(VisibleBoundsWorldspace.MidX, VisibleBoundsWorldspace.MidY));

            if (RegionView.IsCellInOutsideRegion(mapCell))
            {
                CenterPosition = RegionView.GetCurrentGamePosition(mapCell, CenterPosition.RegionPosition);
                DrawRegionsAsync(CenterPosition);
            }

        }

        public void MoveWorld(CCPoint diff)
        {
            var anchor = WorldTileMap.TileLayersContainer.AnchorPoint;
            diff.X = diff.X / WorldTileMap.TileLayersContainer.ContentSize.Width;
            diff.Y = diff.Y / WorldTileMap.TileLayersContainer.ContentSize.Height;
            anchor.X -= diff.X;
            anchor.Y -= diff.Y;
            WorldTileMap.TileLayersContainer.AnchorPoint = anchor;
        }

        public float GetScale()
        {
            return m_scale;
        }

        public void ScaleWorld(float newScale)
        {
            if (ClientConstants.TILEMAP_MIN_SCALE < newScale &&
                newScale < ClientConstants.TILEMAP_MAX_SCALE)
            {
				m_scale = newScale;
				WorldTileMap.TileLayersContainer.Scale = m_scale;
            }
        }


        void SetMapAnchor(Position anchorPosition)
        {
            var mapCellPosition = PositionHelper.PositionToMapCellPosition(CenterPosition, new PositionI(anchorPosition));//new MapCellPosition(RegionView.GetCurrentTileInMap(anchorPosition));
            var anchor = mapCellPosition.GetAnchor();
            WorldTileMap.TileLayersContainer.AnchorPoint = anchor;
        }

        MapCellPosition GetMapCell(CCTileMapLayer layer, CCPoint location)
        {
            var point = layer.WorldToParentspace(location);
            var tileMapCooardinate = layer.ClosestTileCoordAtNodePosition(point);
            return new MapCellPosition(tileMapCooardinate);
        }





        #endregion

        #region Properties

        public CCTileMap WorldTileMap
        {
            get;
            private set;
        }

        public CCTileMapLayer TerrainLayer
        {
            get;
            private set;
        }

        public CCTileMapLayer BuildingLayer
        {
            get;
            private set;
        }

        public CCTileMapLayer UnitLayer
        {
            get;
            private set;
        }

        public CCTileMapLayer MenuLayer
        {
            get;
            private set;
        }

        client.Common.Manager.RegionManagerController m_regionManagerController;

        DrawNode m_currentPositionNode;
        Geolocation m_geolocation;

        Worker m_worker;
        float m_scale;



        GameScene m_gameScene;

        #endregion
    }
}

