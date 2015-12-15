namespace Client.Common.Views
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Client.Common.Helper;
    using Client.Common.Views.Effects;
    using CocosSharp;
    using Core.Models;
    using Core.Models.Definitions;

    /// <summary>
    /// Touch handler.
    /// </summary>
    public class TileTouchHandler
    {
        /// <summary>
        /// Touch gesture states.
        /// </summary>
        public enum TouchGesture
        {
            None,
            Start,
            Menu,
            MenuDrawn,
            Move,
            MoveUnit,
            Zoom
        }

        /// <summary>
        /// The m_touch gesture.
        /// </summary>
        private TouchGesture m_touchGesture;

        /// <summary>
        /// The m_timer.
        /// </summary>
        private Stopwatch m_timer;

        /// <summary>
        /// The m_world layer.
        /// </summary>
        private WorldLayer m_worldLayer;

        /// <summary>
        /// The m_new scale.
        /// </summary>
        private float m_newScale = Common.Constants.ClientConstants.TILEMAP_NORM_SCALE;

        /// <summary>
        /// The m_scale.
        /// </summary>
        private float m_scale = Common.Constants.ClientConstants.TILEMAP_NORM_SCALE;

        /// <summary>
        /// The m_start location.
        /// </summary>
        private CCPoint m_startLocation;

        /// <summary>
        /// The m_menu view.
        /// </summary>
        private MenuView m_menuView;

        /// <summary>
        /// The indicator view.
        /// </summary>
        private IndicatorView m_indicator;

        private Position m_initialPosition;

        private bool m_extMenuFlag = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.TileTouchHandler"/> class.
        /// </summary>
        /// <param name="scene">The entire scene.</param>
        public TileTouchHandler(GameScene scene)
        {
            m_timer = new Stopwatch();
            m_touchGesture = TouchGesture.None;
            m_worldLayer = scene.WorldLayer;
            m_startLocation = new CCPoint(1, 1);

            TouchHandler.Instance.ListenBegan(m_worldLayer, OnTouchesBegan);
            TouchHandler.Instance.ListenEnded(m_worldLayer, OnTouchesEnded);
            TouchHandler.Instance.ListenMoved(m_worldLayer, OnTouchesMoved);
        }

        /// <summary>
        /// On the touches moved event. Set the gesture to move or scale and scale or move the map.
        /// </summary>
        /// <param name="touches">Touches, where the user touched.</param>
        /// <param name="touchEvent">Touch event.</param>
        /// <returns>Returns a true after processed the event. </returns>
        public bool OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count == 1 && m_touchGesture == TouchGesture.Zoom)
            {
                m_touchGesture = TouchGesture.Move;
            }
            else if (touches.Count == 2 && m_touchGesture == TouchGesture.Move)
            {
                m_touchGesture = TouchGesture.Zoom;
            }

            if (touches.Count == 1 &&
                (m_touchGesture == TouchGesture.Start ||
                m_touchGesture == TouchGesture.Move))
            {
                m_touchGesture = TouchGesture.Move;
                var touch = touches[0];

                CCPoint diff = touch.Delta;
                m_worldLayer.MoveWorld(diff);
            }
            else if (touches.Count >= 2 &&
                     (m_touchGesture == TouchGesture.Start ||
                     m_touchGesture == TouchGesture.Zoom))
            {
                m_touchGesture = TouchGesture.Zoom;

                CCPoint screenStart0 = touches[0].StartLocationOnScreen;
                CCPoint screenStart1 = touches[1].StartLocationOnScreen;

                // calculate Current Position
                CCPoint currentPoint0 = touches[0].LocationOnScreen;
                CCPoint currentPoint1 = touches[1].LocationOnScreen;

                var screen = new CCPoint(
                                 m_worldLayer.VisibleBoundsWorldspace.MaxX,
                                 m_worldLayer.VisibleBoundsWorldspace.MaxY); 

                float startDistance = screenStart0.DistanceSquared(ref screenStart1);
                float currentDistance = currentPoint0.DistanceSquared(ref currentPoint1);
                float screenDistance = screen.LengthSquared;

                float relation = (currentDistance - startDistance) / screenDistance;

                m_newScale = m_scale + (relation * m_newScale);
                m_worldLayer.ScaleWorld(m_newScale);
            }
            return true;
        }

        /// <summary>
        /// On the touches began event.
        /// </summary>
        /// <param name="touches">Touches, where the user touched.</param>
        /// <param name="touchEvent">Touch event.</param>
        /// <returns> Returns true after processing the touches.</returns>
        public bool OnTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            var oldStart = m_startLocation;
            var oldCoord = m_worldLayer.ClosestTileCoordAtNodePosition(oldStart);

            var oldMapCell = new Models.MapCellPosition(oldCoord);
            var oldPosition = m_worldLayer.RegionView.GetCurrentGamePosition(oldMapCell, m_worldLayer.CenterPosition.RegionPosition);

            m_startLocation = m_worldLayer.LayerWorldToParentspace(touches[0].Location);
            var coord = m_worldLayer.ClosestTileCoordAtNodePosition(m_startLocation);
            var newMapCellPosition = new Client.Common.Models.MapCellPosition(coord);
            var newPosition = m_worldLayer.RegionView.GetCurrentGamePosition(newMapCellPosition, m_worldLayer.CenterPosition.RegionPosition);
            var endPositionI = new Core.Models.PositionI((int)newPosition.X, (int)newPosition.Y);

            switch (m_touchGesture)
            {
                case TouchGesture.MoveUnit:
                    var startMapCellPosition = new Client.Common.Models.MapCellPosition(oldCoord);
                    var startPosition = m_worldLayer.RegionView.GetCurrentGamePosition(startMapCellPosition, m_worldLayer.CenterPosition.RegionPosition);
                    var startPositionI = new Core.Models.PositionI((int)startPosition.X, (int)startPosition.Y);

                    var oldPositionI = new Core.Models.PositionI((int)oldPosition.X, (int)oldPosition.Y);
                    var action = ActionHelper.MoveUnit(oldPositionI, endPositionI);

                    var actionC = (Core.Controllers.Actions.Action)action.Control;
                    var possible = actionC.Possible();
                    if (possible)
                    {
                        m_worldLayer.DoAction(action);
                    }
                    m_indicator.RemoveIndicator();
                    m_touchGesture = TouchGesture.None;
                    break;

                case TouchGesture.Menu:
                    var def = m_menuView.GetSelectedDefinition(coord);                  
                    if (def != null)
                    {
                        var oldPositionI2 = new Core.Models.PositionI((int)oldPosition.X, (int)oldPosition.Y);
                        if (m_extMenuFlag)
                        {
                            oldPositionI2 = new Core.Models.PositionI((int)m_initialPosition.X, (int)m_initialPosition.Y);                            
                        }
                        var action2 = ActionHelper.CreateEntity(oldPositionI2, def, GameAppDelegate.Account);
                        var actionC2 = (Core.Controllers.Actions.Action)action2.Control;
                        if (actionC2.Possible())
                        {
                            m_worldLayer.DoAction(action2);
                        }
                    }
                    if (m_worldLayer.MenuLayer.TileGIDAndFlags(coord).Gid < Client.Common.Constants.BuildingMenuGid.CANCEL && Client.Common.Constants.BuildingMenuGid.MILITARY <= m_worldLayer.MenuLayer.TileGIDAndFlags(coord).Gid)
                    {
                        m_menuView.ExtendMenu(m_worldLayer.MenuLayer.TileGIDAndFlags(coord).Gid, endPositionI);
                        m_initialPosition = oldPosition;
                        m_extMenuFlag = true;
                    }
                    else
                    {
                        m_menuView.CloseMenu();
                        m_menuView = null;
                        m_touchGesture = TouchGesture.None;
                        m_initialPosition = null;
                        m_extMenuFlag = false;
                    }
                    return true;

                case TouchGesture.None:
                    m_touchGesture = TouchGesture.Start;
                    break;
            }

            m_timer.Reset();
            m_timer.Start();

            m_scale = m_worldLayer.GetScale();

            return true;
        }

        /// <summary>
        /// On the touches ended event.
        /// </summary>
        /// <param name="touches">Touches, where the user touched.</param>
        /// <param name="touchEvent">Touch event.</param>
        /// <returns>Return true after processing the touch events and drawing.</returns>
        public bool OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            m_timer.Stop();
            var coord = m_worldLayer.ClosestTileCoordAtNodePosition(m_startLocation);
            var mapCellPosition = new Client.Common.Models.MapCellPosition(coord);
            var position = m_worldLayer.RegionView.GetCurrentGamePosition(mapCellPosition, m_worldLayer.CenterPosition.RegionPosition);
            var positionI = new Core.Models.PositionI((int)position.X, (int)position.Y);
            switch (m_touchGesture)
            {
                case TouchGesture.Zoom:
                    // Set Current Scale
                    m_worldLayer.ScaleWorld(m_newScale);
                    m_touchGesture = TouchGesture.None;
                    break;

                case TouchGesture.Move:
                    m_worldLayer.CheckCenterRegion();
                    m_touchGesture = TouchGesture.None;
                    break;

                case TouchGesture.Start:
                    if (m_worldLayer.UnitLayer.TileGIDAndFlags(coord).Gid != 0)
                    {
                        m_touchGesture = TouchGesture.MoveUnit;
                        var range = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(positionI.RegionPosition).GetEntity(positionI.CellPosition).Move;                       
                        m_indicator = new IndicatorView(m_worldLayer);
                        m_indicator.ShowIndicator(positionI, range, 1);
                    }
                    else if (m_worldLayer.BuildingLayer.TileGIDAndFlags(coord).Gid == Client.Common.Constants.BuildingGid.BARRACKS )//&& m_worldLayer.BuildingLayer.TileGIDAndFlags(coord).Gid <= 77)
                    {
                        var types = new Core.Models.Definitions.Definition[6];
                        var defM = Core.Models.World.Instance.DefinitionManager;

                        types[0] = defM.GetDefinition(EntityType.Archer);
                        types[1] = defM.GetDefinition(EntityType.Hero);
                        types[2] = defM.GetDefinition(EntityType.Warrior);
                        types[3] = defM.GetDefinition(EntityType.Mage);
                        types[4] = defM.GetDefinition(EntityType.Archer);
                        types[5] = defM.GetDefinition(EntityType.Archer);

                        m_menuView = new MenuView(m_worldLayer, positionI, types);
                        m_menuView.DrawMenu();
                        m_touchGesture = TouchGesture.Menu;
                    }
                    else if (m_worldLayer.BuildingLayer.TileGIDAndFlags(coord).Gid == 0)
                    {
                        bool cont = false;
                        var types = new Core.Models.Definitions.Definition[6];
                        var defM = Core.Models.World.Instance.DefinitionManager;
                        if (!GameAppDelegate.Account.TerritoryBuildings.ContainsKey((long)EntityType.Headquarter))
                        {
                            types[0] = defM.GetDefinition(EntityType.Headquarter);
                            types[1] = defM.GetDefinition(EntityType.Headquarter);
                            types[2] = defM.GetDefinition(EntityType.Headquarter);
                            types[3] = defM.GetDefinition(EntityType.Headquarter);
                            types[4] = defM.GetDefinition(EntityType.Headquarter);
                            types[5] = defM.GetDefinition(EntityType.Headquarter);
                        }
                        else
                        {   
                            cont = true;
                        }
                        m_menuView = new MenuView(m_worldLayer, positionI, types);
                        if (!cont)
                        {
                            m_menuView.DrawMenu();
                        }
                        else
                        {
                            var Gids = new short[6];
                            Gids[5] = Client.Common.Constants.BuildingMenuGid.MILITARY;
                            Gids[0] = Client.Common.Constants.BuildingMenuGid.RESOURCES;
                            Gids[1] = Client.Common.Constants.BuildingMenuGid.STORAGE;
                            Gids[2] = Client.Common.Constants.BuildingMenuGid.ZIVIL;
                            Gids[3] = Client.Common.Constants.BuildingMenuGid.BUILDINGPLACEHOLDER;
                            Gids[4] = Client.Common.Constants.BuildingMenuGid.CANCEL;
                            m_menuView.DrawMajorMenu(Gids);
                        }
                        m_touchGesture = TouchGesture.Menu;
                    }

                    break;
            }
                    
            m_worldLayer.UglyDraw();
            return true;
        }
    }
}