using System.Reflection;

namespace Client.Common.Views
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Linq;

    using Client.Common.Helper;
    using Client.Common.Manager;
    using Client.Common.Models;
    using Client.Common.Constants;
    using CocosSharp;
    using Core.Controllers.Actions;
    using Core.Models;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The World layer with hexagonal Tilemaps.
    /// </summary>
    public class WorldLayerHex : CCLayer
    {

        public enum ViewModes
        {
            CurrentGPSPosition,
            CameraPosition,
            HeadquarterPosition
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.WorldLayer"/> class.
        /// </summary>
        /// <param name="gameScene">Ga me scene.</param>
        public WorldLayerHex(GameScene gameScene)
            : base()
        {
            m_gameScene = gameScene;

            ViewMode = ViewModes.CurrentGPSPosition;

            m_currentWorldPoint = PositionHelper.PositionToWorldspace(Geolocation.Instance.CurrentGamePosition);
            m_worker = new Views.Worker(this);
            EntityManagerController.Instance.Worker = m_worker;
  
            m_regionViewHexDic = new Dictionary<RegionPosition, RegionViewHex>();

            m_geolocationPositionNode = new DrawNode();

            m_touchHandler = new TileTouchHandler(this);

            this.AddChild(m_geolocationPositionNode);

            Schedule(m_worker.Schedule);
            Schedule(CheckView);
        }

        /// <summary>
        /// Gets the scale factor.
        /// </summary>
        /// <returns>The scale factor.</returns>
        public float GetZoom()
        {
            return m_zoom;
        }

        public void ZoomWorld(float newZoom)
        {
            if (Common.Constants.ClientConstants.TILEMAP_MIN_ZOOM < newZoom &&
                newZoom < Common.Constants.ClientConstants.TILEMAP_MAX_ZOOM)
            {
                m_zoom = newZoom;
                var size = m_gameScene.VisibleBoundsScreenspace.Size;
                this.Camera.OrthographicViewSizeWorldspace = new CCSize(size.Width * m_zoom, size.Height * m_zoom);
            }
        }

        public void SetWorldPosition(CCPoint worldPoint)
        {
            m_currentWorldPoint = worldPoint;
        }

        public void UglyDraw()
        {
            foreach (var region in m_regionViewHexDic)
            {
                region.Value.UglyDraw();
            }
        }

        public RegionViewHex GetRegionViewHex(RegionPosition regionPosition)
        {
            RegionViewHex viewRegionHex = null;
            m_regionViewHexDic.TryGetValue(regionPosition, out viewRegionHex);

            return viewRegionHex;
        }

        public void DoAction(Core.Models.Action action)
        {
            m_worker.Queue.Enqueue(action);
        }

        public void DrawBorders(Entity entity)
        {
            // alle Gebäude des entity owners
            var buildings = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(entity.Position.RegionPosition)
                .GetEntity(entity.Position.CellPosition).Owner.TerritoryBuildings.Keys;
            
            var color = new CCColor4B();
            color = CCColor4B.Green;
            if (GameAppDelegate.Account != entity.Owner)
            {
                color = CCColor4B.Red;
            }

            var surroundedPositionsAll = new HashSet<PositionI>();
            int range;
            // alle Felder finden die zu der entity gehören
            foreach (var building in buildings)
            {

                var buildingEntity = Core.Controllers.Controller.Instance.RegionManagerController
                    .GetRegion(building.RegionPosition).GetEntity(building.CellPosition);
                if (buildingEntity.Definition.SubType == Core.Models.Definitions.EntityType.Headquarter)
                {
                    range = Core.Models.Constants.HEADQUARTER_TERRITORY_RANGE;
                }
                else
                {
                    range = Core.Models.Constants.GUARDTOWER_TERRITORY_RANGE;
                }
                    
                var surroundedPositionsBuilding = LogicRules.GetSurroundedPositions(building, range);
                surroundedPositionsAll.UnionWith(surroundedPositionsBuilding);
            }

            // alle Grenzfelder finden und nach Region sortieren
            var regionBorders = new Dictionary<RegionPosition,HashSet<PositionI>>();
            foreach (var pos in surroundedPositionsAll)
            {
                var posOwner = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(pos.RegionPosition).
                    GetClaimedTerritory(pos);
                
                var surroundedFields = LogicRules.GetSurroundedFields(pos);
                foreach (var field in surroundedFields)
                {
                    var fieldOwner = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(field.RegionPosition).
                        GetClaimedTerritory(field);
                    if (posOwner != fieldOwner)
                    {
                        if (!regionBorders.ContainsKey(pos.RegionPosition))
                        {
                            regionBorders.Add(pos.RegionPosition, new HashSet<PositionI>());
                        }
                        HashSet<PositionI> position;
                        regionBorders.TryGetValue(pos.RegionPosition, out position);
                        position.Add(pos);
                        break;
                    }
                }
            }

            // zeichne Grenzen in den regionen
            HashSet<PositionI> borderPositions;
            foreach (var regionPosition in regionBorders.Keys)
            {
                regionBorders.TryGetValue(regionPosition, out borderPositions);
                this.GetRegionViewHex(regionPosition).DrawBorder(borderPositions, color);
            }

        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            InitCamera(m_currentWorldPoint);
            ZoomWorld(ClientConstants.TILEMAP_NORM_ZOOM);
        }

        private void InitCamera(CCPoint worldPoint)
        {
            var cameraTargetPoint = worldPoint;
            var cameraWidth = m_gameScene.VisibleBoundsScreenspace.MaxX;
            var cameraHeight = m_gameScene.VisibleBoundsScreenspace.MaxY; 

            this.Camera = new CCCamera(CCCameraProjection.Projection2D, 
                new CCRect(cameraTargetPoint.X, cameraTargetPoint.Y,
                    cameraWidth,
                    cameraHeight));
        }

        private void SetCamera(CCPoint3 newTargetPoint, CCPoint3 newCameraPoint)
        {
            this.Camera.TargetInWorldspace = newTargetPoint;
            this.Camera.CenterInWorldspace = newCameraPoint;
        }

        private void CheckView(float frameTimesInSecond)
        {
            var oldCameraPoint = this.Camera.CenterInWorldspace;
            var oldTargetPoint = this.Camera.TargetInWorldspace;
            if (m_touchHandler.Gesture != TileTouchHandler.TouchGesture.Move)
            {           
                LoadRegionViews(m_currentWorldPoint);
            }              
            var newTargetPoint = new CCPoint3(m_currentWorldPoint.X, m_currentWorldPoint.Y, oldTargetPoint.Z);
            var newCameraPoint = new CCPoint3(m_currentWorldPoint.X, m_currentWorldPoint.Y, oldCameraPoint.Z);
            SetCamera(newTargetPoint, newCameraPoint); 
        }

        private void LoadRegionViews(CCPoint point)
        {
            var position = PositionHelper.WorldspaceToPosition(point);
            var regionManagerController = Core.Controllers.Controller.Instance.RegionManagerController as Client.Common.Manager.RegionManagerController;
            var newKeys = regionManagerController.GetWorldNearRegionPositions(position.RegionPosition);
            var oldKeys = new HashSet<RegionPosition>(m_regionViewHexDic.Keys);
            var deleteKeys = new HashSet<RegionPosition>(m_regionViewHexDic.Keys);
            deleteKeys.ExceptWith(newKeys);

            foreach (var regionPos in deleteKeys)
            {
                this.RemoveChild(m_regionViewHexDic[regionPos].GetTileMap().TileLayersContainer);
                m_regionViewHexDic.Remove(regionPos);
            }

            newKeys.ExceptWith(oldKeys);
            foreach (var regionPos in newKeys)
            {
                var region = regionManagerController.GetRegion(regionPos);
                RegionViewHex regionViewHex = (RegionViewHex)region.View;
                if (regionViewHex == null)
                {
                    regionViewHex = new RegionViewHex(region);
                }
                this.AddChild(regionViewHex.GetTileMap().TileLayersContainer);
                m_regionViewHexDic.Add(regionPos, regionViewHex);
            }
                
        }

        #region Properties

        /// <summary>
        /// The m worker.
        /// </summary>
        private Worker m_worker;

        /// <summary>
        /// The m region view hex dictionary.
        /// </summary>
        private Dictionary<RegionPosition, RegionViewHex> m_regionViewHexDic;
    
        /// <summary>
        /// The m current position.
        /// </summary>
        private CCPoint m_currentWorldPoint;

        /// <summary>
        /// The m current position node.
        /// </summary>
        private DrawNode m_geolocationPositionNode;

        /// <summary>
        /// The m scale.
        /// </summary>
        private float m_zoom;

        /// <summary>
        /// The m_game scene.
        /// </summary>
        private GameScene m_gameScene;


        public ViewModes ViewMode;

        private TileTouchHandler m_touchHandler;

        #endregion
    }
}