using Client.Common.Helper;

namespace Client.Common.Views
{
    using System;
    using System.Collections.Generic;
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
            AddChild(WorldLayerHex);


            HUD = new Client.Common.Views.HUD.HUDLayer(this);
            AddChild(HUD);

            DebugLayer = new DebugLayer();
            AddChild(DebugLayer);

            TouchHandler.Instance.Init(WorldLayerHex);

            Schedule(CheckGPS);
        }

        void CheckGPS(float elapsedTime)
        {
            if (WorldLayerHex.ViewMode == WorldLayerHex.ViewModes.CurrentGPSPosition)
            {
                var cameraPoint = PositionHelper.PositionToWorldspace(Geolocation.Instance.CurrentGamePosition);
                WorldLayerHex.SetWorldPosition(cameraPoint);
            }
        }

        #region Properties



        public Position CurrentBasePosition;

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

        //healthbar
        public Effects.EffectLayer Test;


        #endregion
    }
}