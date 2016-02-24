namespace Client.Common.Views
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Client.Common.Constants;
    using Client.Common.Helper;
    using Client.Common.Manager;
    using Client.Common.Models;
    using CocosSharp;
    using Core.Controllers.Actions;
    using Core.Models;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The World layer with hexagonal Tile map.
    /// </summary>
    public class WorldLayerHex : CCLayer
    {
        /// <summary>
        /// View modes.
        /// </summary>
        public enum ViewModes
        {
            CurrentGPSPosition,
            CameraPosition,
            HeadquarterPosition
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.WorldLayerHex"/> class.
        /// </summary>
        /// <param name="gameScene">Game scene.</param>
        public WorldLayerHex(GameScene gameScene)
            : base()
        {
            m_gameScene = gameScene;

            ViewMode = ViewModes.CurrentGPSPosition;

            m_currentWorldPoint = PositionHelper.PositionToWorldspace(Geolocation.Instance.CurrentGamePosition);
            m_regionViewHexDic = new Dictionary<RegionPosition, RegionViewHex>();
            m_geolocationPositionNode = new DrawNode();
            m_touchHandler = new TileTouchHandler(this);
            this.AddChild(m_geolocationPositionNode);
        }

        /// <summary>
        /// Gets the zoom.
        /// </summary>
        /// <returns>The zoom.</returns>
        public float GetZoom()
        {
            return m_zoom;
        }

        /// <summary>
        /// Zooms the world.
        /// </summary>
        /// <param name="newZoom">New zoom.</param>
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

        /// <summary>
        /// Sets the world position.
        /// </summary>
        /// <param name="worldPoint">World point.</param>
        public void SetWorldPosition(CCPoint worldPoint)
        {
            m_currentWorldPoint = worldPoint;
        }

        /// <summary>
        /// Make a ugle draw (moves the tile layers container a little bit).
        /// </summary>
        public void UglyDraw()
        {
            foreach (var region in m_regionViewHexDic)
            {
                region.Value.UglyDraw();
            }
        }

        /// <summary>
        /// Gets the region view hex.
        /// </summary>
        /// <returns>The region view hex.</returns>
        /// <param name="regionPosition">Region position.</param>
        public RegionViewHex GetRegionViewHex(RegionPosition regionPosition)
        {
            RegionViewHex viewRegionHex = null;
            m_regionViewHexDic.TryGetValue(regionPosition, out viewRegionHex);

            return viewRegionHex;
        }

        /// <summary>
        /// Dos the action.
        /// </summary>
        /// <param name="action">The action.</param>
        public void DoAction(Core.Models.Action action)
        {
            if (Cheats.OFFLINE_MODE)
            {
                Worker.Instance.Queue.Enqueue(action);
            }
            else
            {
                Controllers.NetworkController.Instance.DoActionsAsync(Models.Geolocation.Instance.CurrentGamePosition, new Core.Models.Action[] { action });
                ScheduleOnce(RefreshRegions, 0.3f);
            }
        }

        /// <summary>
        /// Refreshs the regions.
        /// </summary>
        /// <param name="time">The time.</param>
        public async Task RefreshRegionsAsync(float time)
        {
            var regions = m_regionViewHexDic.Keys.ToArray();
            await Manager.EntityManagerController.Instance.LoadEntitiesAsync(regions);
            UglyDraw();
        }

        public void RefreshRegions(float time)
        {
            var regions = m_regionViewHexDic.Keys.ToArray();
            Manager.EntityManagerController.Instance.LoadEntitiesAsync(regions);
            UglyDraw();
        }

        /// <summary>
        /// Draws the borders.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public void DrawBorders(Account owner)
        {
            // alle Gebäude des entity owners
            var buildings = owner.TerritoryBuildings.Keys;
            
            var color = new CCColor4B();
            color = CCColor4B.Green;
            if (GameAppDelegate.Account != owner)
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

            if (surroundedPositionsAll.Count > 0)
            {
                // alle Grenzfelder finden und nach Region sortieren
                var regionBorders = new Dictionary<RegionPosition, HashSet<PositionI>>();
                foreach (var pos in surroundedPositionsAll)
                {
                    var posOwner = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(pos.RegionPosition).GetClaimedTerritory(pos);

                    var surroundedFields = LogicRules.GetSurroundedFields(pos);
                    foreach (var field in surroundedFields)
                    {
                        var fieldOwner = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(field.RegionPosition).GetClaimedTerritory(field);
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
                    this.GetRegionViewHex(regionPosition).DrawBorder(borderPositions, color, owner);
                }
            }
            else
            {
                foreach (var regionPosition in m_regionViewHexDic.Keys)
                {
                    this.GetRegionViewHex(regionPosition).DrawBorder(null, color, owner);
                }
            }
        }

        /// <summary>
        /// Addeds to scene.
        /// </summary>
        protected override void AddedToScene()
        {
            base.AddedToScene();
            InitCamera(m_currentWorldPoint);
            ZoomWorld(ClientConstants.TILEMAP_NORM_ZOOM);

            CheckGPS(0);
            CheckView(0);
            Schedule(CheckGPS);
            Schedule(CheckView);
            Schedule(RefreshRegionsPeriodic);
            Schedule(Worker.Instance.Schedule);
        }

        float m_gameTime;
        private void RefreshRegionsPeriodic(float time)
        {
            m_gameTime += time;
            if (m_gameTime > ClientConstants.DATA_REFRESH_TIME)
            {
                RefreshRegionsAsync(time);
                m_gameTime -= ClientConstants.DATA_REFRESH_TIME;
            }
        }

        /// <summary>
        /// Inits the camera.
        /// </summary>
        /// <param name="worldPoint">World point.</param>
        private void InitCamera(CCPoint worldPoint)
        {
            var cameraTargetPoint = worldPoint;
            var cameraWidth = m_gameScene.VisibleBoundsScreenspace.MaxX;
            var cameraHeight = m_gameScene.VisibleBoundsScreenspace.MaxY; 

            this.Camera = new CCCamera(CCCameraProjection.Projection2D, new CCRect(cameraTargetPoint.X, cameraTargetPoint.Y, cameraWidth, cameraHeight));
        }

        /// <summary>
        /// Sets the camera.
        /// </summary>
        /// <param name="newTargetPoint">New target point.</param>
        /// <param name="newCameraPoint">New camera point.</param>
        private void SetCamera(CCPoint3 newTargetPoint, CCPoint3 newCameraPoint)
        {
            this.Camera.TargetInWorldspace = newTargetPoint;
            this.Camera.CenterInWorldspace = newCameraPoint;
        }

        /// <summary>
        /// Checks the GPS.
        /// </summary>
        /// <param name="elapsedTime">Elapsed time.</param>
        private void CheckGPS(float elapsedTime)
        {
            if (ViewMode == ViewModes.CurrentGPSPosition)
            {
                var cameraPoint = PositionHelper.PositionToWorldspace(Geolocation.Instance.CurrentGamePosition);
                SetWorldPosition(cameraPoint);
            }
        }

        /// <summary>
        /// Checks the view for position updats.
        /// </summary>
        /// <param name="frameTimesInSecond">Frame times in second.</param>
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

        /// <summary>
        /// Loads the region views.
        /// </summary>
        /// <param name="point">The point.</param>
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
                foreach (RegionViewHex.LayerTypes layer in Enum.GetValues(typeof(RegionViewHex.LayerTypes)))
                {
                    this.RemoveChild(m_regionViewHexDic[regionPos].GetChildrens(layer));
                }
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

                foreach (RegionViewHex.LayerTypes layer in Enum.GetValues(typeof(RegionViewHex.LayerTypes)))
                {
                    if (regionViewHex.GetChildrens(layer) != null)
                    {
                        this.AddChild(regionViewHex.GetChildrens(layer), (int)layer);
                    }
                }
                m_regionViewHexDic.Add(regionPos, regionViewHex);
            }         
        }

        /// <summary>
        /// The view mode.
        /// </summary>
        public ViewModes ViewMode;

        #region Properties

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

        /// <summary>
        /// The touch handler.
        /// </summary>
        private TileTouchHandler m_touchHandler;

        #endregion
    }
}