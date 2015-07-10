using @base.control;
using @base.model;
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
            m_regionManagerController = Controller.Instance.RegionManagerController as client.Common.Manager.RegionManagerController;

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


        public void CreateUnit(CCTileMapCoordinates location, int type)
        {
            var gameApp = GameAppDelegate.Instance;
            var dictParam = new System.Collections.Generic.Dictionary<string,object>();
            var tapMapCellPosition = new MapCellPosition(location);
            var tapPosition = RegionView.GetCurrentGamePosition(tapMapCellPosition, CenterPosition.RegionPosition);
            var tapPositionI = new PositionI((int)tapPosition.X, (int)tapPosition.Y);
            dictParam[@base.control.action.CreateUnit.CREATE_POSITION] = tapPositionI; 
            dictParam[@base.control.action.CreateUnit.CREATION_TYPE] = (long)type;
            var newAction = new @base.model.Action(gameApp.Account, @base.model.Action.ActionType.CreateUnit, dictParam);
            var actionC = (@base.control.action.Action)newAction.Control;
            var possible = actionC.Possible(m_regionManagerController);

            if (possible)
            {
                //actionC.Do (m_regionManagerController);
                //m_worker.Queue.Enqueue (newAction);
                DoAction(newAction);
            }
        }

        public void CreateBuilding(CCTileMapCoordinates location, long type)
        {
            var gameApp = GameAppDelegate.Instance;
            var dictParam = new System.Collections.Generic.Dictionary<string,object>();
            var tapMapCellPosition = new MapCellPosition(location);
            var tapPosition = RegionView.GetCurrentGamePosition(tapMapCellPosition, CenterPosition.RegionPosition);
            var tapPositionI = new PositionI((int)tapPosition.X, (int)tapPosition.Y);
            dictParam[@base.control.action.CreatBuilding.CREATE_POSITION] = tapPositionI; 
            dictParam[@base.control.action.CreatBuilding.CREATION_TYPE] = (long)type;
            var newAction = new @base.model.Action(gameApp.Account, @base.model.Action.ActionType.CreateBuilding, dictParam);
            var actionC = (@base.control.action.Action)newAction.Control;
            var possible = actionC.Possible(m_regionManagerController);

            if (possible)
            {
                //actionC.Do (m_regionManagerController);
                //m_worker.Queue.Enqueue (newAction);      
                DoAction(newAction);
            }
        }

        public void ShowMenu(CCTileMapCoordinates location, int menutype)
        {
            CCTileMapCoordinates coordHelper1, coordHelper2, coordHelper3, coordHelper4, coordHelper5, coordHelper6; 
            CCTileGidAndFlags gidHelper1, gidHelper2, gidHelper3, gidHelper4, gidHelper5, gidHelper6, gidHelpercenter;


            coordHelper1.Column = location.Column + (location.Row) % 2;
            coordHelper1.Row = location.Row - 1;

            coordHelper2.Column = location.Column + (location.Row) % 2;
            coordHelper2.Row = location.Row + 1;        

            coordHelper3.Column = location.Column;
            coordHelper3.Row = location.Row + 2;

            coordHelper4.Column = location.Column - (location.Row + 1) % 2;
            coordHelper4.Row = location.Row + 1;

            coordHelper5.Column = location.Column - (location.Row + 1) % 2;
            coordHelper5.Row = location.Row - 1;

            coordHelper6.Column = location.Column;
            coordHelper6.Row = location.Row - 2;

            switch (menutype)
            {
            //clears the Menu at around a given Position
                case 0:
                    MenuLayer.RemoveTile(location);
                    MenuLayer.RemoveTile(coordHelper1);
                    MenuLayer.RemoveTile(coordHelper2);
                    MenuLayer.RemoveTile(coordHelper3);
                    MenuLayer.RemoveTile(coordHelper4);
                    MenuLayer.RemoveTile(coordHelper5);
                    MenuLayer.RemoveTile(coordHelper6);
                    //m_touchGesture = TouchGesture.None;
                    break;
                case 1: //UnitMenu
                    gidHelpercenter.Gid = ClientConstants.CROSS_GID;
                    gidHelper1.Gid = ClientConstants.MENUEBOWMAN_GID;
                    gidHelper2.Gid = ClientConstants.MENUEHERO_GID;
                    gidHelper3.Gid = ClientConstants.MENUEWARRIOR_GID;
                    gidHelper4.Gid = ClientConstants.MENUEMAGE_GID;
                    gidHelper5.Gid = ClientConstants.MENUESCOUT_GID;
                    gidHelper6.Gid = ClientConstants.MENUEUNKNOWN_GID;
                    MenuLayer.SetTileGID(gidHelpercenter, location);
                    MenuLayer.SetTileGID(gidHelper1, coordHelper1);
                    MenuLayer.SetTileGID(gidHelper2, coordHelper2);
                    MenuLayer.SetTileGID(gidHelper3, coordHelper3);
                    MenuLayer.SetTileGID(gidHelper4, coordHelper4);
                    MenuLayer.SetTileGID(gidHelper5, coordHelper5);
                    MenuLayer.SetTileGID(gidHelper6, coordHelper6);
                    break;
                case 2: //BuildingMenu
                    gidHelpercenter.Gid = ClientConstants.CROSS_GID;
                    gidHelper1.Gid = ClientConstants.MENUEEARTH_GID;
                    gidHelper2.Gid = ClientConstants.MENUEFIRE_GID;
                    gidHelper3.Gid = ClientConstants.MENUEGOLD_GID;
                    gidHelper4.Gid = ClientConstants.MENUEAIR_GID;
                    gidHelper5.Gid = ClientConstants.MENUEMANA_GID;
                    gidHelper6.Gid = ClientConstants.MENUEWATER_GID;
                    MenuLayer.SetTileGID(gidHelpercenter, location);
                    MenuLayer.SetTileGID(gidHelper1, coordHelper1);
                    MenuLayer.SetTileGID(gidHelper2, coordHelper2);
                    MenuLayer.SetTileGID(gidHelper3, coordHelper3);
                    MenuLayer.SetTileGID(gidHelper4, coordHelper4);
                    MenuLayer.SetTileGID(gidHelper5, coordHelper5);
                    MenuLayer.SetTileGID(gidHelper6, coordHelper6);
                    break;
                default:
                    gidHelpercenter.Gid = ClientConstants.CROSS_GID;
                    gidHelper1.Gid = ClientConstants.MENUEEARTH_GID;
                    gidHelper2.Gid = ClientConstants.MENUEFIRE_GID;
                    gidHelper3.Gid = ClientConstants.MENUEGOLD_GID;
                    gidHelper4.Gid = ClientConstants.MENUEAIR_GID;
                    gidHelper5.Gid = ClientConstants.MENUEMANA_GID;
                    gidHelper6.Gid = ClientConstants.MENUEWATER_GID;
                    MenuLayer.SetTileGID(gidHelpercenter, location);
                    MenuLayer.SetTileGID(gidHelper1, coordHelper1);
                    MenuLayer.SetTileGID(gidHelper2, coordHelper2);
                    MenuLayer.SetTileGID(gidHelper3, coordHelper3);
                    MenuLayer.SetTileGID(gidHelper4, coordHelper4);
                    MenuLayer.SetTileGID(gidHelper5, coordHelper5);
                    MenuLayer.SetTileGID(gidHelper6, coordHelper6);
                    break;

            }
            UglyDraw();

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

        public CCPoint WorldToParentspace(CCPoint point)
        {
            return TerrainLayer.WorldToParentspace(point);
        }

        public CCTileMapCoordinates ClosestTileCoordAtNodePosition(CCPoint point)
        {
            return TerrainLayer.ClosestTileCoordAtNodePosition(point);
        }

        public void DoAction(@base.model.Action action)
        {
            var actions = new List<@base.model.Action>();
            actions.Add(action);
            m_regionManagerController.DoActionAsync(m_geolocation.CurrentGamePosition, actions.ToArray());
            //var mapCell = GetMapCell(m_terrainLayer, new CCPoint(VisibleBoundsWorldspace.MidX, VisibleBoundsWorldspace.MidY));
            //var position = RegionView.GetCurrentGamePosition(mapCell, CenterPosition.RegionPosition);
            //DrawRegionsAsync (position);
        }

        //clears a Layer
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

