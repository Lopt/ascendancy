using System.Linq.Expressions;
using Xamarin.Forms;

namespace Client.Common.Views
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Threading.Tasks;

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
        /// <summary>
        /// Load phases.
        /// </summary>
        public enum Phases
        {
            Start,
            LoadTerrain,
            LoadEntities,
            Idle,
            Exit
        }

        /// <summary>
        /// Gets the load phase.
        /// </summary>
        /// <value>The phase.</value>
        public Phases Phase
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.WorldLayer"/> class.
        /// </summary>
        /// <param name="gameScene">Game scene.</param>
        public WorldLayerHex(GameScene gameScene)
            : base()
        {
            m_gameScene = gameScene;
            m_currentPosition = Geolocation.Instance.CurrentGamePosition;
            m_lastPosition = m_currentPosition;
            m_worker = new Views.Worker(this);
            EntityManagerController.Instance.Worker = m_worker;
            // m_menuLayer = TODO
            // ClearLayers();
            m_regionViewHexDic = new Dictionary<RegionPosition, RegionViewHex>();

            m_currentPositionNode = new DrawNode();

            LoadRegionViewHexDic();

            foreach (RegionViewHex regionViewHex in m_regionViewHexDic.Values)
            {
                this.AddChild(regionViewHex.GetTileMap());
            }

            this.AddChild(m_currentPositionNode);

            Schedule(m_worker.Schedule);
            Schedule(CheckGeolocation);
        }

        /// <summary>
        /// Gets the scale factor.
        /// </summary>
        /// <returns>The scale factor.</returns>
        public float GetScale()
        {
            return m_scale;
        }

        public void ScaleWorld(float newScale)
        {
            if (Common.Constants.ClientConstants.TILEMAP_MIN_SCALE < newScale &&
                newScale < Common.Constants.ClientConstants.TILEMAP_MAX_SCALE)
            {
                m_scale = newScale;
                this.Scale = m_scale;
            }
        }

        public void MoveWorld(CCPoint diff)
        {
            var anchor = this.AnchorPoint;
            diff.X = diff.X / this.ScaledContentSize.Width;
            diff.Y = diff.Y / this.ScaledContentSize.Height;
            anchor.X -= diff.X;
            anchor.Y -= diff.Y;
            this.AnchorPoint = anchor;
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

        protected override void AddedToScene()
        {
            base.AddedToScene();
            SetRegionViewsHexIntoTheWorld();
        }

        private void SetRegionViewsHexIntoTheWorld()
        {
            var offsetX = 0.0f;
            var offsetY = 0.0f;

            foreach (var pair in m_regionViewHexDic)
            {
                var tileMap = pair.Value.GetTileMap();

                offsetX = tileMap.TileLayersContainer.ScaledContentSize.Width;
                offsetY = tileMap.TileLayersContainer.ScaledContentSize.Height;

                //tileMap.AnchorPoint = new CCPoint(0.0f, 1.0f);
                tileMap.TileLayersContainer.PositionX = pair.Key.RegionX * offsetX;
                tileMap.TileLayersContainer.PositionY = pair.Key.RegionY * offsetY;


            }

            //this.PositionX = (float)m_currentPosition.RegionPosition.RegionX * offsetX;
            // this.PositionY = (float)m_currentPosition.RegionPosition.RegionY * offsetY;

            var cameraTarget = new CCPoint3((float)m_currentPosition.RegionPosition.RegionX * offsetX,
                                   (float)m_currentPosition.RegionPosition.RegionY * offsetY, 3.0f);
            var cameraPosition = new CCPoint3((float)m_currentPosition.RegionPosition.RegionX * offsetX,
                                     (float)m_currentPosition.RegionPosition.RegionY * offsetY, 100.0f);
//            this.Camera = new CCCamera(80.0f, 16.0f / 9.0f, cameraPosition, cameraTarget);
            int help = 1;
        }

        private void CheckGeolocation(float frameTimesInSecond)
        {
            if (m_currentPosition != Geolocation.Instance.CurrentGamePosition)
            {
                m_lastPosition = m_currentPosition;
                m_currentPosition = Geolocation.Instance.CurrentGamePosition;

                LoadRegionViewHexDicAsync();
            }
                
        }

        /// <summary>
        /// Clears the Layers for initialization.
        /// </summary>
        private void ClearLayers()
        {
            var coordHelper = new CCTileMapCoordinates(0, 0);
            m_menuLayer.RemoveTile(coordHelper);
        }

        /// <summary>
        /// Loads the region view hex dictionary with all regions (5x5) arround the currentPosition.
        /// </summary>
        /// <returns>The region view hex dic.</returns>
        private async Task LoadRegionViewHexDicAsync()
        {
            var regionManagerController = Core.Controllers.Controller.Instance.RegionManagerController as Client.Common.Manager.RegionManagerController;
            Phase = Phases.Start;
            Phase = Phases.LoadTerrain;
            await regionManagerController.LoadTerrainsAsync(m_currentPosition.RegionPosition);
            Phase = Phases.LoadEntities;
            await EntityManagerController.Instance.LoadEntitiesAsync(m_currentPosition, m_currentPosition.RegionPosition);
            Phase = Phases.Idle;

            LoadRegionViewHexDic();

        }

        private void LoadRegionViewHexDic()
        {
            var regionManagerController = Core.Controllers.Controller.Instance.RegionManagerController as Client.Common.Manager.RegionManagerController;
            m_regionViewHexDic.Clear();
            foreach (var regionPosition in regionManagerController.GetWorldNearRegionPositions(m_currentPosition.RegionPosition))
            {
                var region = regionManagerController.GetRegion(regionPosition);
                m_regionViewHexDic.Add(regionPosition, new RegionViewHex(region));

            }
        }

        private void testInfos()
        {
            RegionViewHex regionViewHex;
            m_regionViewHexDic.TryGetValue(m_currentPosition.RegionPosition, out regionViewHex);
            var tilemap = regionViewHex.GetTileMap();
            var mapDimensions = tilemap.MapDimensions;
            int numberOfColumns = (int)mapDimensions.Size.Width;
            int numberOfRows = (int)mapDimensions.Size.Height;

            var tileMapUnitLayer = tilemap.LayerNamed(ClientConstants.LAYER_UNIT);
            var LayerSize = tileMapUnitLayer.LayerSize;
             
            int tileWidth = (int)tilemap.TileTexelSize.Width;
            int tileHeight = (int)tilemap.TileTexelSize.Height;

            int worldx = tileWidth * numberOfColumns + tileWidth / 2;
            int worldy = tileHeight * numberOfRows + tileHeight / 2;
            var cord = tileMapUnitLayer.ClosestTileCoordAtNodePosition(new CCPoint(worldx, worldy));


            var size = this.ContentSize;
            var layersize = this.Layer.LayerSizeInPixels;

            int test = 1;

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
        private Position m_currentPosition;

        /// <summary>
        /// The m last position.
        /// </summary>
        private Position m_lastPosition;

        /// <summary>
        /// The m menu layer.
        /// </summary>
        private CCTileMapLayer m_menuLayer;

        /// <summary>
        /// The m current position node.
        /// </summary>
        private DrawNode m_currentPositionNode;

        /// <summary>
        /// The m scale.
        /// </summary>
        private float m_scale;

        /// <summary>
        /// The m_game scene.
        /// </summary>
        private GameScene m_gameScene;

        #endregion
    }
}