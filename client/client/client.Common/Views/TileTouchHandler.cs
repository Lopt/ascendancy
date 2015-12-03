using Client.Common.Manager;
using System.Runtime.CompilerServices;

namespace Client.Common.Views
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Client.Common.Helper;
    using CocosSharp;
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
        private WorldLayerHex m_worldLayerHex;

        private RegionManagerController m_regionManagerController;

        /// <summary>
        /// The m_new scale.
        /// </summary>
        private float m_newZoom = Common.Constants.ClientConstants.TILEMAP_NORM_ZOOM;

        /// <summary>
        /// The m_scale.
        /// </summary>
        private float m_zoom = Common.Constants.ClientConstants.TILEMAP_NORM_ZOOM;

        /// <summary>
        /// The m_start location.
        /// </summary>
        private CCPoint m_startLocation;

        /// <summary>
        /// The m_menu view.
        /// </summary>
        private MenuView m_menuView;

        private GameScene m_scene;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.TileTouchHandler"/> class.
        /// </summary>
        /// <param name="scene">The entire scene.</param>
        public TileTouchHandler(GameScene scene)
        {
            m_timer = new Stopwatch();
            m_touchGesture = TouchGesture.None;
            m_worldLayerHex = scene.WorldLayerHex;
            m_scene = scene;
            m_startLocation = new CCPoint(1, 1);
            m_regionManagerController = (RegionManagerController)Core.Controllers.Controller.Instance.RegionManagerController;
            TouchHandler.Instance.ListenBegan(m_worldLayerHex, OnTouchesBegan);
            TouchHandler.Instance.ListenEnded(m_worldLayerHex, OnTouchesEnded);
            TouchHandler.Instance.ListenMoved(m_worldLayerHex, OnTouchesMoved);
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

                m_scene.ViewMode = GameScene.ViewModes.CameraPosition;

                var diff = touches[0].LocationOnScreen - touches[0].StartLocationOnScreen;
                var move = new CCPoint(-diff.X, diff.Y) * m_worldLayerHex.GetZoom();
                var cameraDiff = touches[0].StartLocationOnScreen - m_scene.VisibleBoundsScreenspace.Center;
                var cameraMove = new CCPoint(-cameraDiff.X, cameraDiff.Y) * m_worldLayerHex.GetZoom();
                m_worldLayerHex.SetWorldPosition(m_startLocation + cameraMove + move);
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
                                 m_scene.VisibleBoundsScreenspace.MaxX,
                                 m_scene.VisibleBoundsScreenspace.MaxY); 

                float startDistance = screenStart0.DistanceSquared(ref screenStart1);
                float currentDistance = currentPoint0.DistanceSquared(ref currentPoint1);
                float screenDistance = screen.LengthSquared;

                float relation = (currentDistance - startDistance) / screenDistance;

                m_newZoom = m_zoom + (relation * m_newZoom);
                m_worldLayerHex.ZoomWorld(m_newZoom);
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
            var oldWorldPosition = m_startLocation;
            var oldGamePositionI = Helper.PositionHelper.WorldPointToGamePositionI(oldWorldPosition);

            m_startLocation = m_worldLayerHex.ConvertToWorldspace(touches[0].Location);
            var gamePositionI = Helper.PositionHelper.WorldPointToGamePositionI(m_startLocation);

            switch (m_touchGesture)
            {
                case TouchGesture.MoveUnit:
                    
                    var action = ActionHelper.MoveUnit(oldGamePositionI, gamePositionI);

                    var actionC = (Core.Controllers.Actions.Action)action.Control;
                    var possible = actionC.Possible();
                    if (possible)
                    {
                        m_worldLayerHex.DoAction(action);
                    }
                    m_touchGesture = TouchGesture.None;
                    break;

                case TouchGesture.Menu:
//                    var def = m_menuView.GetSelectedDefinition(coord);
//                    if (def != null)
//                    {
//                        var oldPositionI2 = new Core.Models.PositionI((int)oldPosition.X, (int)oldPosition.Y);
//                        var action2 = ActionHelper.CreateEntity(oldPositionI2, def);
//                        var actionC2 = (Core.Controllers.Actions.Action)action2.Control;
//                        if (actionC2.Possible())
//                        {
//                            m_worldLayerHex.DoAction(action2);
//                        }
//                    }
//                    m_menuView.CloseMenu();
//                    m_menuView = null;
//                    m_touchGesture = TouchGesture.None;

                    return true;

                case TouchGesture.None:
                    m_touchGesture = TouchGesture.Start;
                    break;
            }

            m_timer.Reset();
            m_timer.Start();

            m_zoom = m_worldLayerHex.GetZoom();

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

            var startPositionI = Helper.PositionHelper.WorldPointToGamePositionI(m_startLocation);
            switch (m_touchGesture)
            {
                case TouchGesture.Zoom:
                    // Set Current Zoom
                    m_worldLayerHex.ZoomWorld(m_newZoom);
                    m_touchGesture = TouchGesture.None;
                    break;

                case TouchGesture.Move:
                    m_touchGesture = TouchGesture.None;
                    break;

                case TouchGesture.Start:
                    var entity = m_regionManagerController.GetRegion(startPositionI.RegionPosition).GetEntity(startPositionI.CellPosition);
                    if (entity == null)
                    {
                        var types = new Core.Models.Definitions.Definition[6];
                        var defM = Core.Models.World.Instance.DefinitionManager;

                        types[0] = defM.GetDefinition(EntityType.Headquarter);
                        types[1] = defM.GetDefinition(EntityType.Headquarter);
                        types[2] = defM.GetDefinition(EntityType.Headquarter);
                        types[3] = defM.GetDefinition(EntityType.Headquarter);
                        types[4] = defM.GetDefinition(EntityType.Headquarter);
                        types[5] = defM.GetDefinition(EntityType.Headquarter);

                        //                        m_menuView = new MenuView(m_worldLayerHex.MenuLayer, coord, types);
                        //                        m_menuView.DrawMenu();
                        //                        m_touchGesture = TouchGesture.Menu;
                    }
                    else if (entity.Definition.Category == Category.Unit)
                    {
                        m_touchGesture = TouchGesture.MoveUnit;
                    }
                    else if (entity.Definition.Category == Category.Building)
                    {
                        var types = new Core.Models.Definitions.Definition[6];
                        var defM = Core.Models.World.Instance.DefinitionManager;

                        types[0] = defM.GetDefinition(EntityType.Archer);
                        types[1] = defM.GetDefinition(EntityType.Hero);
                        types[2] = defM.GetDefinition(EntityType.Warrior);
                        types[3] = defM.GetDefinition(EntityType.Mage);
                        types[4] = defM.GetDefinition(EntityType.Archer);
                        types[5] = defM.GetDefinition(EntityType.Archer);

//                        m_menuView = new MenuView(m_worldLayerHex.MenuLayer, coord, types);
//                        m_menuView.DrawMenu();
//                        m_touchGesture = TouchGesture.Menu;
                    }

                    break;
            }

            return true;
        }
    }
}