namespace Client.Common.Views.HUD
{
    using System;
    using CocosSharp;

    /// <summary>
    /// HUD layer. Contains everything which lays in front of the screen and never move
    /// (btw it moves with the camera). Ressources, Dialogs, etc.
    /// </summary>
    public class HUDLayer : CCLayerColor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.HUD.HUDLayer"/> class.
        /// </summary>
        /// <param name="gameScene">Game scene.</param>
        public HUDLayer(GameScene gameScene)
            : base()
        {
            m_gameScene = gameScene;

            m_gps = new Button(
                "radars2-standard",
                "radars2-touched",
                new Action(BackToGPS));
            
            m_gps.AnchorPoint = CCPoint.AnchorLowerLeft;
            AddChild(m_gps);

            m_debug = new Button(
                "debug-standard",
                "debug-touched",
                new Action(StartDebug));
            m_debug.AnchorPoint = CCPoint.AnchorLowerLeft;
            AddChild(m_debug);

            m_energyRessource = new EnergyResource();
            m_energyRessource.AnchorPoint = CCPoint.AnchorUpperLeft;
            AddChild(m_energyRessource);
        }  

        /// <summary>
        /// Add the logo and loaded sprite to scene.
        /// </summary>
        protected override void AddedToScene()
        {
            base.AddedToScene();

            m_debug.PositionX = VisibleBoundsWorldspace.MinX;
            m_debug.PositionY = VisibleBoundsWorldspace.MinY;

            m_gps.PositionX = VisibleBoundsWorldspace.MinX + m_debug.Size.Width;
            m_gps.PositionY = VisibleBoundsWorldspace.MinY;

            m_energyRessource.PositionX = VisibleBoundsWorldspace.MinX;
            m_energyRessource.PositionY = VisibleBoundsWorldspace.MaxY;

            //m_touched.AnchorPoint = CCPoint.AnchorUpperLeft;
            //m_standard.AnchorPoint = CCPoint.AnchorUpperLeft;
        }

        /// <summary>
        /// Relocates center of the Map to GPS coordinates.
        /// </summary>
        public void BackToGPS()
        {
            m_gameScene.WorldLayer.DrawRegionsAsync();
        }

        /// <summary>
        /// Starts the debugging.
        /// </summary>
        public void StartDebug()
        {
            m_gameScene.DebugLayer.Toggle();
        }

        /// <summary>
        /// The back to gps coordinates position button.
        /// </summary>
        private Button m_gps;

        /// <summary>
        /// The open debug layer button.
        /// </summary>
        private Button m_debug;

        /// <summary>
        /// The energy ressource hud element.
        /// </summary>
        private EnergyResource m_energyRessource;

        /// <summary>
        /// The game scene.
        /// </summary>
        private GameScene m_gameScene;
    }
}

