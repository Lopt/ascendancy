namespace Client.Common.Views
{
    using System;
    using System.Collections.Generic;
    using Client.Common.Models;
    using CocosSharp;

    /// <summary>
    /// The Game scene.
    /// </summary>
    public class GameScene : CCScene
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.GameScene"/> class.
        /// </summary>
        /// <param name="mainWindow">Main window.</param>
        public GameScene(CCWindow mainWindow)
            : base(mainWindow)
        {

            WorldLayerHex = new WorldLayerHex(this);
            AddChild(WorldLayerHex);

//            m_touchHandler = new TileTouchHandler(this);

//            HUD = new Client.Common.Views.HUD.HUDLayer(this);
//            AddChild(HUD);

//            DebugLayer = new DebugLayer();
//            AddChild(DebugLayer);

            TouchHandler.Instance.Init(WorldLayerHex);
        }

        #region Properties


        /// <summary>
        /// The world in hex (whole game field).
        /// </summary>
        public WorldLayerHex WorldLayerHex;

        /// <summary>
        /// The debug layer (shows logging information).
        /// </summary>
        public DebugLayer DebugLayer;

        /// <summary>
        /// The HUD with all player output information.
        /// </summary>
        //public HUD.HUDLayer HUD;

        /// <summary>
        /// The m_touch handler.
        /// </summary>
        //        private TileTouchHandler m_touchHandler;

        #endregion
    }
}