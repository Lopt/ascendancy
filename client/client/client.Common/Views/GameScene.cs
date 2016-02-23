namespace Client.Common.Views
{
    using System;
    using System.Collections.Generic;
    using Client.Common.Helper;
    using Client.Common.Models;
    using CocosSharp;
    using Core.Models;

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

            HUD = new Client.Common.Views.HUD.HUDLayer(this);

            DebugLayer = new DebugLayer();

            Worker.Instance.Init(WorldLayerHex);
            TouchHandler.Instance.Init(WorldLayerHex);

            AddChild(WorldLayerHex);
            AddChild(HUD);
            AddChild(DebugLayer);
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
        public HUD.HUDLayer HUD;

        #endregion
    }
}